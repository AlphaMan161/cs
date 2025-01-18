// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServersList
{
    public delegate void ServerListHandler(object obj);

    public const int MAX_PING = 1000;

    private static string quick_mapSystemName = string.Empty;

    private static MapMode.MODE quick_mapMode;

    private static string friendRoomName = string.Empty;

    private static string friendServerID = string.Empty;

    private static ServersList instance;

    private static bool quickConnect;

    private int totalPlayers;

    private List<ServerItem> serverList = ServerConf.ServerList;

    private bool needResort = true;

    private bool isHandlingOnRefresh;

    public static ServerItem SelectServer;

    public static string QuickMapSystemName
    {
        get
        {
            return ServersList.quick_mapSystemName;
        }
        set
        {
            ServersList.quick_mapSystemName = value;
        }
    }

    public static MapMode.MODE QuickMapMode
    {
        get
        {
            return ServersList.quick_mapMode;
        }
        set
        {
            ServersList.quick_mapMode = value;
        }
    }

    public static string FriendRoomName
    {
        get
        {
            return ServersList.friendRoomName;
        }
    }

    public static string FriendServerID
    {
        get
        {
            return ServersList.friendServerID;
        }
    }

    public static ServersList Instance
    {
        get
        {
            if (ServersList.instance == null)
            {
                ServersList.instance = new ServersList();
            }
            return ServersList.instance;
        }
    }

    public static bool QuickConnect
    {
        get
        {
            return ServersList.quickConnect;
        }
        set
        {
            ServersList.quickConnect = value;
        }
    }

    public static int TotalPlayers
    {
        get
        {
            return ServersList.Instance.totalPlayers;
        }
    }

    public static List<ServerItem> ServerList
    {
        get
        {
            if (ServersList.Instance.needResort)
            {
                ServersList.Instance.serverList = (from entry in ServersList.Instance.serverList
                orderby entry.Latency
                select entry).ToList();
                ServersList.Instance.needResort = false;
            }
            return ServersList.Instance.serverList;
        }
    }

    public static bool NeedResort
    {
        get
        {
            return ServersList.Instance.needResort;
        }
        set
        {
            ServersList.Instance.needResort = value;
        }
    }

    public static event ServerListHandler OnConnect;

    private ServersList()
    {
    }

    public static void DoQuickConnect()
    {
        ServersList.quickConnect = true;
    }

    public static void Refresh(MonoBehaviour serverListBehaviour, bool doQuickConnect)
    {
        if (!MasterServerMonitor.Instance.List)
        {
            ServersList.Instance.totalPlayers = 0;
            if (doQuickConnect)
            {
                ServersList.quickConnect = doQuickConnect;
                ServersList.prepareQuickConnect();
            }
            else if (!ServersList.Instance.isHandlingOnRefresh)
            {
                ServersList.Instance.isHandlingOnRefresh = true;
                List<ServerItem>.Enumerator enumerator = ServersList.Instance.serverList.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        ServerItem current = enumerator.Current;
                        current.OnRefresh += delegate(ServerItem serverItem)
                        {
                            ServersList.Instance.totalPlayers += serverItem.CapacityMin;
                        };
                    }
                }
                finally
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
            serverListBehaviour.StartCoroutine(ServersList.RefreshServerItems());
        }
    }

    public static void ClearServers()
    {
        List<ServerItem>.Enumerator enumerator = ServersList.Instance.serverList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ServerItem current = enumerator.Current;
                current.CapacityMin = 0;
                current.Load = 0;
                current.IsActive = false;
                current.IsRefreshed = false;
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public static void ConnectQuick(MonoBehaviour serverListBehaviour, string map_system_name, MapMode.MODE map_mode)
    {
        UnityEngine.Debug.Log(string.Format("[ServerList] ConnectQuick(MonoBehaviour serverListBehaviour[{0}], string map_system_name[{1}], MapMode.MODE map_mode[{2}])", serverListBehaviour, map_system_name, map_mode));
        if (!ServersList.quickConnect)
        {
            ServersList.quickConnect = true;
            ServersList.quick_mapSystemName = map_system_name;
            ServersList.quick_mapMode = map_mode;
            if (PhotonConnection.Connection.Connected)
            {
                MainNetworkController.Instance.JoinRandomRoom();
                ServersList.quickConnect = false;
            }
            else if (MasterServerMonitor.Instance.List)
            {
                MasterServerMonitor.Instance.ListUpdate();
                MenuSelecter.MainMenuSelect = MenuSelecter.MainMenuEnum.Fight;
            }
            else
            {
                ServersList.prepareQuickConnect();
                serverListBehaviour.StartCoroutine(ServersList.RefreshServerItems());
            }
        }
    }

    public static void ConnectFriend(string serverID, string roomName)
    {
        if (!ServersList.quickConnect)
        {
            ServersList.quickConnect = true;
            ServersList.friendRoomName = roomName;
            ServersList.friendServerID = serverID;
            if (PhotonConnection.Connection.Connected)
            {
                ServersList.Disconnect();
            }
            if (LocalUser.UserID == 7)
            {
                UnityEngine.Debug.LogError("My fix to friend connect");
                ServerItem serverItem = ServersList.ServerList.Find((ServerItem x) => x.Host == serverID.Split(':')[0]);
                if (serverItem != null)
                {
                    if (!serverItem.IsConnected)
                    {
                        ServersList.Connect(serverItem);
                    }
                    return;
                }
            }
            if (MasterServerMonitor.Instance.List)
            {
                MasterServerMonitor.Instance.ListUpdate();
                MenuSelecter.MainMenuSelect = MenuSelecter.MainMenuEnum.Fight;
            }
            else
            {
                ServersList.prepareQuickConnect();
                ServerListBehaviour.Instance.StartCoroutine(ServersList.RefreshServerItems());
            }
        }
    }

    public static void FriendConnectReset()
    {
        ServersList.friendRoomName = string.Empty;
        ServersList.friendServerID = string.Empty;
        ServersList.quickConnect = false;
    }

    private static void prepareQuickConnect()
    {
        List<ServerItem>.Enumerator enumerator = ServersList.Instance.serverList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ServerItem current = enumerator.Current;
                current.IsRefreshed = false;
                current.ServerItemListener = new ServerItem.ServerItemListenerDelegate(ServersList.onServerItemListener);
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public static string GetServerName(string host)
    {
        string empty = string.Empty;
        List<ServerItem>.Enumerator enumerator = ServersList.Instance.serverList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ServerItem current = enumerator.Current;
                if (current.Host.Contains(host))
                {
                    return current.Name;
                }
            }
            return empty;
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public static string GetServerNumber(string host)
    {
        int num = 1;
        string empty = string.Empty;
        List<ServerItem>.Enumerator enumerator = ServersList.Instance.serverList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ServerItem current = enumerator.Current;
                if (current.Host.Contains(host))
                {
                    if (current.Name.Split(' ').Count() < 2)
                    {
                        return num.ToString();
                    }
                    return current.Name.Split(' ')[1];
                }
                num++;
            }
            return empty;
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public static int GetServerMaxCCU(string host)
    {
        int result = 0;
        List<ServerItem>.Enumerator enumerator = ServersList.Instance.serverList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ServerItem current = enumerator.Current;
                if (current.Host.Contains(host))
                {
                    return current.CapacityMax;
                }
            }
            return result;
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public static ServerItem GetServerItem(string host)
    {
        return ServersList.Instance.serverList.Find((ServerItem x) => x.Host == host);
    }

    public static string GetServerHost(string serverID)
    {
        string empty = string.Empty;
        List<ServerItem>.Enumerator enumerator = ServersList.Instance.serverList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ServerItem current = enumerator.Current;
                if (current.Name.Split(' ')[1] == serverID)
                {
                    return current.Host;
                }
            }
            return empty;
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    private static IEnumerator RefreshServerItems()
    {
        UnityEngine.Debug.Log("[ServerList] RefreshServerItems");
        List<ServerItem>.Enumerator enumerator = ServersList.Instance.serverList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ServerItem currServer = enumerator.Current;
                currServer.Refresh();
                yield return (object)null;
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public static void Connect(ServerItem server)
    {
        ServersList.Disconnect();
        ServersList.SelectServer = server;
        ServersList.SelectServer.IsConnected = true;
        MainNetworkController.Instance.Connect(ServersList.SelectServer);
        if (ServersList.OnConnect != null)
        {
            ServersList.OnConnect(server);
        }
    }

    public static void Disconnect()
    {
        if (ServersList.SelectServer != null)
        {
            ServersList.SelectServer.IsConnected = false;
            PhotonConnection.Connection.Disconnect();
        }
    }

    public static void onServerItemListener(ServerItem serverItem)
    {
        if (serverItem != null)
        {
            UnityEngine.Debug.Log("Server " + serverItem.Name + " Ready.");
            serverItem.ServerItemListener = null;
        }
        bool flag = true;
        int num = 1000;
        ServerItem serverItem2 = null;
        if (ServersList.friendRoomName != string.Empty)
        {
            List<ServerItem>.Enumerator enumerator = ServersList.Instance.serverList.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ServerItem current = enumerator.Current;
                    if (!current.IsRefreshed)
                    {
                        flag = false;
                        break;
                    }
                    if (ServersList.friendServerID == string.Format("{0}:{1}", current.Host, current.Ports[0]) && current.IsActive)
                    {
                        serverItem2 = current;
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            if (flag)
            {
                UnityEngine.Debug.Log("SERVERS ARE READY.");
                if (serverItem2 != null)
                {
                    UnityEngine.Debug.Log("Friend Server: " + serverItem2.Name);
                    if (LocalUser.Level < serverItem2.LevelMin || LocalUser.Level > serverItem2.LevelMax)
                    {
                        ServersList.FriendConnectReset();
                        ErrorInfo.CODE code = ErrorInfo.CODE.SERVER_NOT_ALLOWED;
                        code.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
                    }
                    else
                    {
                        ServersList.Connect(serverItem2);
                    }
                }
                else
                {
                    ServersList.FriendConnectReset();
                    ErrorInfo.CODE code2 = ErrorInfo.CODE.SERVERS_IS_DOWN;
                    code2.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
                }
            }
        }
        else
        {
            List<ServerItem>.Enumerator enumerator2 = ServersList.Instance.serverList.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    ServerItem current2 = enumerator2.Current;
                    if (!current2.IsRefreshed)
                    {
                        flag = false;
                        break;
                    }
                    if (current2.IsActive && LocalUser.Level >= current2.LevelMin && LocalUser.Level <= current2.LevelMax)
                    {
                        if (current2.Latency < num)
                        {
                            serverItem2 = current2;
                            num = current2.Latency;
                        }
                        if (current2.LevelMax == 10 && LocalUser.Level < 10)
                        {
                            serverItem2 = current2;
                            num = 0;
                        }
                        if (current2.LevelMin == 10 && current2.LevelMax == 20 && LocalUser.Level >= 10 && LocalUser.Level <= 20)
                        {
                            serverItem2 = current2;
                            num = 0;
                        }
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator2).Dispose();
            }
            if (flag)
            {
                UnityEngine.Debug.Log("SERVERS ARE READY.");
                if (serverItem2 != null)
                {
                    UnityEngine.Debug.Log("Optimal Server: " + serverItem2.Name);
                    ServersList.Connect(serverItem2);
                }
                else
                {
                    ErrorInfo.CODE code3 = ErrorInfo.CODE.NO_AVAILABLE_SERVERS;
                    code3.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
                    ServersList.quickConnect = false;
                }
            }
        }
    }
}


