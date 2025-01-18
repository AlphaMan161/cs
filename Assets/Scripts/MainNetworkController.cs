// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainNetworkController : MonoBehaviour
{
    public enum StateRoom
    {
        Connection = 1,
        Wait,
        Error
    }

    private AssetLoader levelLoader;

    private static MainNetworkController hInstance;

    private PhotonConnection photonConnection;

    private object roomListLocker = new object();

    private MapMode.MODE listSelectMode;

    private string mac = string.Empty;

    public Dictionary<string, ResponseRoomList> RoomList = new Dictionary<string, ResponseRoomList>();

    public Dictionary<string, ResponseRoomList> FilteredRoomList = new Dictionary<string, ResponseRoomList>();

    private StateRoom currentRoomState = StateRoom.Wait;

    public static MainNetworkController Instance
    {
        get
        {
            return MainNetworkController.hInstance;
        }
    }

    public MapMode.MODE ListSelectMode
    {
        get
        {
            return this.listSelectMode;
        }
        set
        {
            this.listSelectMode = value;
            if (this.listSelectMode == MapMode.MODE.NONE)
            {
                this.FilteredRoomList = this.RoomList;
            }
            else
            {
                this.FilteredRoomList = (from x in this.RoomList
                where x.Value.MapMode == this.listSelectMode
                select x).ToDictionary((KeyValuePair<string, ResponseRoomList> x) => x.Key, (KeyValuePair<string, ResponseRoomList> x) => x.Value);
            }
        }
    }

    private void Start()
    {
        MainNetworkController.hInstance = this;
        this.mac = SystemInfo.deviceUniqueIdentifier;
        WebCall.Analitic("UID", this.mac, LocalUser.UserID.ToString());
    }

    private void OnEnable()
    {
        MainNetworkController.hInstance = this;
        this.photonConnection = PhotonConnection.Connection;
        if (this.photonConnection != null)
        {
            this.photonConnection.AddEventListener(new PhotonConnection.PhotonEventListener(this.onConnectionEvent));
        }
        this.ServerListConnect();
    }

    private void Update()
    {
        if (!Input.GetKeyUp(KeyCode.Keypad6) && !Input.GetKeyUp(KeyCode.RightArrow))
        {
            return;
        }
        NetworkDev.CheckAim = true;
    }

    private void FixedUpdate()
    {
        if (this.photonConnection != null)
        {
            this.photonConnection.Update(true);
        }
    }

    private void ServerListConnect()
    {
        if (ServersList.SelectServer != null && ServersList.SelectServer.NeedReconnect)
        {
            ServersList.Connect(ServersList.SelectServer);
            ServersList.SelectServer.NeedReconnect = false;
        }
    }

    public void StartRoom()
    {
        if (!MapList.Instance[OptionsManager.ConnectingMap.SystemName].Buyed)
        {
            MapList.Instance[OptionsManager.ConnectingMap.SystemName].Tryed = true;
        }
        this.LoadRoomLevel();
    }

    private void LoadRoomLevelEditor()
    {
        if (Application.CanStreamedLevelBeLoaded(OptionsManager.ConnectingMap.SystemName))
        {
            UnityEngine.Debug.Log("[TitleGUI] loadLevelIdle->LoadLevel:" + OptionsManager.ConnectingMap.SystemName);
            Application.LoadLevelAsync(OptionsManager.ConnectingMap.SystemName);
        }
        else
        {
            base.StartCoroutine(this.loadLevelIdle());
        }
    }

    private void LoadRoomLevel()
    {
        string loadingURL = LoadManager.getLoadingURL(WebUrls.GetMapLoadingUrl(OptionsManager.ConnectingMap.SystemName));
        if (LevelLoader.Loader != null)
        {
            LevelLoader.Loader.Dispose();
        }
        this.levelLoader = new AssetLoader(loadingURL);
        this.levelLoader.AddCallback(new AssetLoaderCallback(null, new AssetLoaderCallback.AssetLoaderFinishListener(this.onLoadLevel), new AssetLoaderCallback.AssetLoaderProgressListener(this.onLoadLevelProgress)));
        this.levelLoader.LoadAssetBundle(null, false, 0);
    }

    private void onConnectionEvent(PhotonEvent photonEvent)
    {
        Hashtable hashtable = null;
        switch (photonEvent.Code)
        {
            case 101:
                hashtable = new Hashtable();
                hashtable[(byte)241] = (int)Auth.UserID;
                hashtable[(byte)240] = Auth.Key;
                hashtable[FUFPSGameKeys.Team] = (short)(-1);
                hashtable[(byte)31] = GameLogicServerNetworkController.GameLogicServer.Host;
                UnityEngine.Debug.Log(string.Format("GL onConnectionEvent(): {0}", (string)hashtable[(byte)31]));
                this.photonConnection.JoinLobby(hashtable);
                break;
            case 102:
                if (ServersList.QuickConnect)
                {
                    break;
                }
                break;
            case 103:
                this.StartRoom();
                break;
            case 86:
                this.OnUpdateRoomList(photonEvent.Data);
                if (ServersList.QuickConnect)
                {
                    ServersList.QuickConnect = false;
                    if (ServersList.FriendRoomName != string.Empty)
                    {
                        string text = string.Empty;
                        if (this.RoomList.ContainsKey(ServersList.FriendRoomName) && this.RoomList[ServersList.FriendRoomName] != null)
                        {
                            text = this.RoomList[ServersList.FriendRoomName].MapSystemName;
                        }
                        if (text == string.Empty)
                        {
                            ErrorInfo.CODE code2 = ErrorInfo.CODE.FRIEND_ROOM_CONNECT;
                            code2.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
                        }
                        else
                        {
                            this.JoinRoom(ServersList.FriendRoomName, text, false);
                        }
                        ServersList.FriendConnectReset();
                    }
                    else
                    {
                        this.JoinRandomRoom();
                    }
                }
                break;
            case 104:
            {
                UnityEngine.Debug.Log("[titleGUI] Photon Disconnected!");
                LoadingMapPopup.Complete();
                ServersList.Disconnect();
                ErrorInfo.CODE code3 = ErrorInfo.CODE.SERVER_DISCONNECTED;
                code3.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
                this.currentRoomState = StateRoom.Wait;
                break;
            }
            case 82:
            {
                LoadingMapPopup.Complete();
                ErrorInfo.CODE code = ErrorInfo.CODE.SERVER_CONNECTION_ERROR;
                if (photonEvent.ReturnCode == -12)
                {
                    code = ErrorInfo.CODE.SERVER_INVALID_PASSWORD;
                }
                else if (photonEvent.ReturnCode == -13)
                {
                    code = ErrorInfo.CODE.SERVER_MAP_NOT_ALLOWED;
                }
                else if (photonEvent.ReturnCode == -11)
                {
                    code = ErrorInfo.CODE.ROOM_IS_FULL;
                }
                else if (photonEvent.ReturnCode == -16)
                {
                    code = ErrorInfo.CODE.SERVER_PLAYER_KICKED;
                }
                else
                {
                    ServersList.Disconnect();
                }
                this.currentRoomState = StateRoom.Wait;
                if (photonEvent.ReturnCode == -17)
                {
                    string message = GameLogicServerNetworkController.Instance.GameLogicErrorNotificationMessage(photonEvent.Message);
                    Notification notification = new Notification(Notification.Type.GAMELOGIC_ERROR, LanguageManager.GetText("SERVER ERROR"), message);
                    notification.WindowSize = new Vector2(500f, 243f);
                    NotificationWindow.Add(notification);
                }
                else
                {
                    code.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
                }
                break;
            }
        }
    }

    private IEnumerator loadLevelIdle()
    {
        yield return (object)new WaitForSeconds(1f);
        if (Application.CanStreamedLevelBeLoaded(OptionsManager.ConnectingMap.SystemName))
        {
            UnityEngine.Debug.Log("[titleGUI] loadLevelIdle->LoadLevel:" + OptionsManager.ConnectingMap.SystemName);
            this.UnloadResource();
            SceneManager.LoadSceneAsync(OptionsManager.ConnectingMap.SystemName);
        }
        else
        {
            base.StartCoroutine(this.loadLevelIdle());
        }
    }

    private void UnloadResource()
    {
        UnityEngine.Debug.LogError("UnloadResource");
        ShopManager.Instance.UnloadResource();
        AchievementManager.Instance.UnloadResource();
        AbilityManager.Instance.UnloadResource();
        MapList.Instance.UnloadResource();
        Resources.UnloadUnusedAssets();
    }

    private void onLoadLevelProgress(float progress)
    {
        LoadingMapPopup.Progress(progress * 100f);
    }

    private void onLoadLevel(bool success, AssetBundle assetBundle, Hashtable parameters)
    {
        if (!success)
        {
            UnityEngine.Debug.LogError("[titleGUI] Level Data Downloading Error!");
        }
        else
        {
            LevelLoader.Loader = this.levelLoader;
            if (Application.CanStreamedLevelBeLoaded(OptionsManager.ConnectingMap.SystemName))
            {
                Application.LoadLevelAsync(OptionsManager.ConnectingMap.SystemName);
            }
            else
            {
                base.StartCoroutine(this.loadLevelIdle());
            }
        }
    }

    public bool JoinRandomRoom()
    {
        UnityEngine.Debug.Log(string.Format("ServersList.QuickMapSystemName = " + ServersList.QuickMapSystemName + "  ServersList.QuickMapMode = " + ServersList.QuickMapMode));
        if (this.RoomList.Count > 0)
        {
            List<ResponseRoomList> list = new List<ResponseRoomList>();
            Dictionary<string, ResponseRoomList>.Enumerator enumerator = this.RoomList.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, ResponseRoomList> current = enumerator.Current;
                    ResponseRoomList value = current.Value;
                    if (ServersList.QuickMapSystemName == string.Empty && current.Value.MapMode != MapMode.MODE.DEATHMATCH)
                    {
                        goto IL_00b9;
                    }
                    if (current.Value.MapSystemName == ServersList.QuickMapSystemName)
                    {
                        goto IL_00b9;
                    }
                    continue;
                    IL_00b9:
                    if ((ServersList.QuickMapMode == MapMode.MODE.NONE || current.Value.MapMode == ServersList.QuickMapMode) && current.Value.UserMax > current.Value.UserOnline + 2 && !current.Value.IsPassword && (MapList.Instance[current.Value.MapSystemName].Buyed || !MapList.Instance[current.Value.MapSystemName].Tryed) && (LocalUser.Level >= 15 || current.Value.MapMode != MapMode.MODE.ZOMBIE))
                    {
                        list.Add(current.Value);
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            ServersList.QuickMapSystemName = string.Empty;
            if (list.Count > 0)
            {
                List<ResponseRoomList> list2 = (from entry in list
                orderby entry.SqrAverage
                select entry).ToList();
                this.JoinRoom(list2[0].Name, list2[0].MapSystemName, false);
                return true;
            }
        }
        MenuSelecter.MainMenuSelect = MenuSelecter.MainMenuEnum.Fight;
        MenuSelecter.RoomListMenuSelect = MenuSelecter.RoomListMenuEnum.RoomList;
        ErrorInfo.CODE code = ErrorInfo.CODE.LIST_ROOM_IS_EMPTY;
        code.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
        return false;
    }

    public void JoinRoom(string roomName, string mapName, bool guest)
    {
        if (this.currentRoomState == StateRoom.Connection)
        {
            UnityEngine.Debug.LogError("Join room: currentRoomState == StateRoom.Connection wait connect");
        }
        else
        {
            this.currentRoomState = StateRoom.Connection;
            UnityEngine.Debug.Log("[TitleGUI] JoinRoom " + roomName + " " + mapName);
            ResponseRoomList responseRoomList = this.RoomList[roomName];
            if (responseRoomList.UserMax == responseRoomList.UserOnline && !guest)
            {
                ErrorInfo.CODE code = ErrorInfo.CODE.ROOM_IS_FULL;
                code.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
                this.currentRoomState = StateRoom.Error;
            }
            else
            {
                responseRoomList.IsGuest = guest;
                if (responseRoomList.IsPassword)
                {
                    Notification notification = new Notification(Notification.Type.ENTER_PASSWORD, LanguageManager.GetText("SECURED ROOM"), string.Empty, LanguageManager.GetText("JOIN"), new Notification.ButtonClick(this.OnJoinRoom), responseRoomList);
                    notification.Item = responseRoomList;
                    notification.WindowSize = new Vector2(500f, 243f);
                    NotificationWindow.Add(notification);
                    this.currentRoomState = StateRoom.Error;
                }
                else
                {
                    this.OnJoinRoom(responseRoomList);
                }
            }
        }
    }

    private void OnJoinRoom(object obj)
    {
        UnityEngine.Debug.Log("OnJoinRoom" + Time.time);
        ResponseRoomList responseRoomList = obj as ResponseRoomList;
        if (responseRoomList.MapMode == MapMode.MODE.ZOMBIE && LocalUser.Level < 10)
        {
            ErrorInfo.CODE code = ErrorInfo.CODE.GAME_MODE_LOW_LEVEL;
            code.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
            this.currentRoomState = StateRoom.Wait;
        }
        else
        {
            Map map = MapList.Instance[responseRoomList.MapSystemName];
            LoadingMapPopup.Init(map);
            OptionsManager.ConnectingMap = map;
            Hashtable hashtable = new Hashtable();
            hashtable[FUFPSParameterKeys.AuthID] = (int)Auth.UserID;
            hashtable[FUFPSParameterKeys.AuthKey] = Auth.Key;
            hashtable[FUFPSParameterKeys.Team] = (short)(-1);
            hashtable[FUFPSParameterKeys.UniqueID] = this.mac;
            hashtable[(byte)31] = GameLogicServerNetworkController.GameLogicServer.Host;
            UnityEngine.Debug.Log(string.Format("GL OnJoinRoom(): {0}", (string)hashtable[(byte)31]));
            Hashtable roomProperties = MyRoomSetting.JoinRoomNameToHashtable(responseRoomList.Name, responseRoomList.ConnectingPassword, responseRoomList.IsGuest);
            this.photonConnection.JoinRoom(roomProperties, hashtable);
        }
    }

    public void Connect(object serverInfo)
    {
        object obj = this.roomListLocker;
        Monitor.Enter(obj);
        try
        {
            this.RoomList.Clear();
            this.FilteredRoomList.Clear();
        }
        finally
        {
            Monitor.Exit(obj);
        }
        if (serverInfo.GetType() != typeof(ServerItem))
        {
            throw new Exception("Connect:: Incorect input object");
        }
        ServerItem serverItem = serverInfo as ServerItem;
        this.photonConnection = PhotonConnection.Connection;
        this.photonConnection.AddEventListener(new PhotonConnection.PhotonEventListener(this.onConnectionEvent));
        if (!this.photonConnection.Connected)
        {
            UnityEngine.Debug.Log("[MainNetworkController] Connecting Server: " + serverItem.Host);
            this.photonConnection.Connect(serverItem.Host + ":" + serverItem.Ports[0]);
        }
    }

    private void OnUpdateRoomList(Hashtable data)
    {
        object obj = this.roomListLocker;
        Monitor.Enter(obj);
        try
        {
            this.RoomList.Clear();
            try
            {
                int num = 0;
                IDictionaryEnumerator enumerator = data.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
                        string[] data2 = dictionaryEntry.Value as string[];
                        ResponseRoomList responseRoomList = new ResponseRoomList(dictionaryEntry.Key.ToString(), data2, LocalUser.Level);
                        if (responseRoomList.MapName != string.Empty)
                        {
                            this.RoomList.Add(dictionaryEntry.Key.ToString(), responseRoomList);
                        }
                        num += responseRoomList.UserOnline;
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                this.RoomList = (from entry in this.RoomList
                orderby entry.Value.IsAvail descending
                select entry).ToDictionary((KeyValuePair<string, ResponseRoomList> pair) => pair.Key, (KeyValuePair<string, ResponseRoomList> pair) => pair.Value);
                this.ListSelectMode = this.ListSelectMode;
            }
            catch (Exception arg)
            {
                UnityEngine.Debug.LogError("OnUpdateRoomList Exception:" + arg);
            }
        }
        finally
        {
            Monitor.Exit(obj);
        }
    }

    public void UpdateRoomList()
    {
        this.photonConnection.UpdateRoomList();
    }

    public void CreateRoom(MyRoomSetting roomSettings)
    {
        if (roomSettings.GameMode == MapMode.MODE.ZOMBIE && LocalUser.Level < 10)
        {
            ErrorInfo.CODE code = ErrorInfo.CODE.GAME_MODE_LOW_LEVEL;
            code.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
        }
        else
        {
            if (this.RoomList.ContainsKey(roomSettings.Name))
            {
                roomSettings.Name += "(2)";
            }
            LoadingMapPopup.Init(roomSettings.Map);
            Hashtable hashtable = new Hashtable();
            hashtable[FUFPSParameterKeys.AuthID] = (int)Auth.UserID;
            hashtable[FUFPSParameterKeys.AuthKey] = Auth.Key;
            hashtable[FUFPSParameterKeys.Team] = (short)(-1);
            hashtable[FUFPSParameterKeys.UniqueID] = this.mac;
            hashtable[(byte)31] = GameLogicServerNetworkController.GameLogicServer.Host;
            UnityEngine.Debug.Log(string.Format("GL CreateRoom(): {0}", (string)hashtable[(byte)31]));
             this.photonConnection.CreateRoom(roomSettings.ToHashtable(), hashtable);
        }
    }
}


