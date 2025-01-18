// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterServerMonitor : MonoBehaviour
{
    private struct ServerReport
    {
        public string serverIP;

        public ServerState state;

        public LogType logType;

        public string name;

        public ServerReport(string serverIP, ServerState state, LogType logType, string name)
        {
            this.serverIP = serverIP;
            this.state = state;
            this.logType = logType;
            this.name = name;
        }
    }

    private const int MONITOR_PERIOD = 50000000;

    private const int LIST_PERIOD = 50000000;

    private static MasterServerMonitor mInstance;

    private bool report;

    private bool list = true;

    private static int LIST_FAIL_PERIOD = 60000000;

    private Dictionary<string, Queue<ServerState>> reportList = new Dictionary<string, Queue<ServerState>>();

    private long lastMonitorTime = DateTime.Now.Ticks;

    private long lastServerListTime;

    private string detailServer = string.Empty;

    private int failConnectMasterServer;

    private bool logList;

    private long lastServerListTimeRequest;

    public static MasterServerMonitor Instance
    {
        get
        {
            if ((UnityEngine.Object)MasterServerMonitor.mInstance == (UnityEngine.Object)null)
            {
                MasterServerMonitor.mInstance = (new GameObject("MasterServerMonitor").AddComponent(typeof(MasterServerMonitor)) as MasterServerMonitor);
            }
            return MasterServerMonitor.mInstance;
        }
    }

    public bool Report
    {
        get
        {
            return this.report;
        }
        set
        {
            if (this.report != value)
            {
                this.Reset();
            }
            this.report = value;
        }
    }

    public bool List
    {
        get
        {
            if (!Configuration.EnableMasterRL)
            {
                this.list = false;
            }
            return this.list;
        }
        set
        {
            if (this.list != value)
            {
                this.Reset();
            }
            this.list = value;
        }
    }

    public long LastServerListTime
    {
        get
        {
            return this.lastServerListTime;
        }
    }

    public bool LogList
    {
        get
        {
            return this.logList;
        }
        set
        {
            this.logList = value;
        }
    }

    public bool ServerListRequestSuccess()
    {
        if (this.lastServerListTime == 0L)
        {
            if (this.lastServerListTimeRequest == 0L)
            {
                this.lastServerListTimeRequest = DateTime.Now.Ticks;
            }
            if (DateTime.Now.Ticks - this.lastServerListTimeRequest > MasterServerMonitor.LIST_FAIL_PERIOD)
            {
                return false;
            }
        }
        return true;
    }

    public void Reset()
    {
        this.failConnectMasterServer = 0;
        this.lastMonitorTime = DateTime.Now.Ticks - 50000000 + 10000000;
        this.reportList.Clear();
    }

    private void ReadServerStates(Hashtable serverData)
    {
        foreach (object key2 in serverData.Keys)
        {
            string key = (string)key2;
            Hashtable hashtable = (Hashtable)serverData[key2];
            if (!this.reportList.ContainsKey(key))
            {
                this.reportList[key] = new Queue<ServerState>();
            }
            ServerState item = default(ServerState);
            item.peerCount = (int)hashtable[(byte)214];
            item.lag = (short)hashtable[(byte)199];
            item.ping = 0;
            if (hashtable.ContainsKey((byte)202))
            {
                item.ping = (short)hashtable[(byte)202];
            }
            item.disconnectConnect = 0;
            item.reportTime = Time.time;
            if (hashtable.ContainsKey((byte)198))
            {
                item.disconnectConnect = (short)hashtable[(byte)198];
            }
            item.memory = 0;
            if (hashtable.ContainsKey((byte)198))
            {
                item.memory = (short)hashtable[(byte)200];
            }
            item.cpuUsage = 0;
            if (hashtable.ContainsKey((byte)198))
            {
                item.cpuUsage = (byte)hashtable[(byte)201];
            }
            item.glServerLoads = null;
            if (hashtable.ContainsKey((byte)196))
            {
                item.glServerLoads = (Dictionary<string, Hashtable>)hashtable[(byte)196];
            }
            this.reportList[key].Enqueue(item);
        }
    }

    private void ProcessReport()
    {
        int num = 0;
        int num2 = 0;
        DebugConsole.Instance.Clear();
        string message = string.Format("No.{0, -10} {1, -20} {2, -8} {3, -8} {4, -8} {5, -8} {6, -8} {7, -15} {8, -8}", "S", "IP Adderss", "Count", "CPU", "Memory", "Ping", "Lag", "Name", "GLCount");
        DebugConsole.Instance.Log(message, string.Empty, LogType.Assert);
        SortedDictionary<int, ServerReport> sortedDictionary = new SortedDictionary<int, ServerReport>();
        Dictionary<string, Hashtable> dictionary = new Dictionary<string, Hashtable>();
        int num3 = -1;
        Dictionary<string, Queue<ServerState>>.Enumerator enumerator = this.reportList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, Queue<ServerState>> current = enumerator.Current;
                ServerState state = current.Value.ToArray()[current.Value.Count - 1];
                string key = current.Key;
                if (this.reportList[key].Count > 20)
                {
                    this.reportList[key].Dequeue();
                }
                string serverName = ServersList.GetServerName(key);
                string serverNumber = ServersList.GetServerNumber(key);
                decimal value = 0m;
                int num4 = 0;
                for (num4 = ((!decimal.TryParse(serverNumber, out value)) ? num3-- : Convert.ToInt32(value)); sortedDictionary.ContainsKey(num4); num4++)
                {
                }
                if (state.cpuUsage >= 98 || state.memory > 2000 || state.lag > 80 || Time.time - state.reportTime > 10f)
                {
                    string path = "Sounds/Noo";
                    if ((UnityEngine.Object)base.gameObject.GetComponent<AudioSource>() == (UnityEngine.Object)null)
                    {
                        base.gameObject.AddComponent<AudioSource>();
                    }
                    AudioClip clip = (AudioClip)Resources.Load(path);
                    base.GetComponent<AudioSource>().PlayOneShot(clip);
                    sortedDictionary.Add(num4, new ServerReport(key, state, LogType.Error, serverName));
                }
                else
                {
                    sortedDictionary.Add(num4, new ServerReport(key, state, LogType.Warning, serverName));
                }
                num += state.peerCount;
                num2++;
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        SortedDictionary<int, ServerReport>.Enumerator enumerator2 = sortedDictionary.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                KeyValuePair<int, ServerReport> current2 = enumerator2.Current;
                ServerReport value2 = current2.Value;
                int num6 = 0;
                if (value2.state.glServerLoads != null)
                {
                    num6 = value2.state.glServerLoads.Count;
                    Dictionary<string, Hashtable>.Enumerator enumerator3 = value2.state.glServerLoads.GetEnumerator();
                    try
                    {
                        while (enumerator3.MoveNext())
                        {
                            KeyValuePair<string, Hashtable> current3 = enumerator3.Current;
                            dictionary[current3.Key] = current3.Value;
                        }
                    }
                    finally
                    {
                        ((IDisposable)enumerator3).Dispose();
                    }
                }
                string message2 = string.Format("No.{0, -10} {1, -20} {2, -10} {3, -10} {4, -10} {5, -10} {6, -10} {7, -15} {8, -40}", current2.Key, value2.serverIP, value2.state.peerCount, value2.state.cpuUsage, value2.state.memory, value2.state.ping, value2.state.lag, value2.name, num6);
                DebugConsole.Instance.Log(message2, string.Empty, value2.logType);
            }
        }
        finally
        {
            ((IDisposable)enumerator2).Dispose();
        }
        DebugConsole.Instance.Log(string.Format("Total Servers: {0} Player: {1}", num2, num), string.Empty, LogType.Assert);
        string message3 = string.Format("No.{0, -10} {1, -20} {2, -8} {3, -8} {4, -8}", "S", "IP Adderss", "Count", "CPU", "Memory");
        DebugConsole.Instance.Log(message3, string.Empty, LogType.Assert);
        Dictionary<string, Hashtable>.Enumerator enumerator4 = dictionary.GetEnumerator();
        try
        {
            while (enumerator4.MoveNext())
            {
                KeyValuePair<string, Hashtable> current4 = enumerator4.Current;
                Hashtable value3 = current4.Value;
                string key2 = current4.Key;
                float num7 = (float)value3[(byte)195];
                short num8 = (short)value3[(byte)194];
                string message4 = string.Format("No.{0, -10} {1, -20} {2, -10} {3, -10} {4, -10}", "?", key2, "??", (int)num7, num8);
                DebugConsole.Instance.Log(message4, string.Empty, LogType.Warning);
            }
        }
        finally
        {
            ((IDisposable)enumerator4).Dispose();
        }
    }

    private void ProcessList()
    {
        this.lastServerListTime = DateTime.Now.Ticks;
        int num = 0;
        int num2 = 0;
        if (this.LogList)
        {
            DebugConsole.Instance.Clear();
        }
        ServersList.ClearServers();
        Dictionary<string, Queue<ServerState>>.Enumerator enumerator = this.reportList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, Queue<ServerState>> current = enumerator.Current;
                ServerState serverState = current.Value.ToArray()[current.Value.Count - 1];
                string key = current.Key;
                if (this.reportList[key].Count > 1)
                {
                    this.reportList[key].Dequeue();
                }
                int serverMaxCCU = ServersList.GetServerMaxCCU(key);
                int peerCount = serverState.peerCount;
                float num3 = Mathf.Max((float)serverState.lag - 10f, 0f);
                float num4 = num3 * 2f;
                if (serverMaxCCU != 0)
                {
                    num4 = Mathf.Min(num4 + ((float)peerCount + 10f) * 100f / (float)serverMaxCCU, 100f);
                }
                ServerItem serverItem = ServersList.GetServerItem(key);
                if (serverItem != null)
                {
                    serverItem.Load = (int)num4;
                    serverItem.CapacityMin = peerCount;
                    serverItem.IsActive = (num4 < 100f);
                    serverItem.IsRefreshed = true;
                }
                if (this.LogList)
                {
                    string message = string.Format("No.{0} {1} ccu:{2}/{3} lag:{4} load:{5}", ServersList.GetServerNumber(key), key, serverState.peerCount, serverMaxCCU, serverState.lag, (int)num4);
                    if (serverState.lag > 30)
                    {
                        DebugConsole.Instance.Log(message, string.Empty, LogType.Error);
                    }
                    else
                    {
                        DebugConsole.Instance.Log(message, string.Empty, LogType.Warning);
                    }
                }
                num += serverState.peerCount;
                num2++;
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        if (this.LogList)
        {
            DebugConsole.Instance.Log(string.Format("Total Servers: {0} Player: {1}", num2, num), string.Empty, LogType.Assert);
        }
        if (ServersList.QuickConnect)
        {
            ServersList.onServerItemListener(null);
        }
    }

    public void OnReport(Hashtable serverData)
    {
        this.ReadServerStates(serverData);
        if (this.report)
        {
            this.ProcessReport();
        }
        if (this.list)
        {
            this.ProcessList();
        }
    }

    public void ProcessCommand(params object[] args)
    {
        if (args.Length == 1)
        {
            MasterServerMonitor.Instance.Report = !MasterServerMonitor.Instance.Report;
        }
        else if (args[1].ToString() == "-d")
        {
            if (args.Length == 2)
            {
                this.detailServer = string.Empty;
            }
            string text = args[2].ToString();
            if (text.Contains("."))
            {
                this.detailServer = args[2].ToString();
            }
            else
            {
                this.detailServer = ServersList.GetServerHost(text);
            }
        }
        else if (args[1].ToString() == "-l")
        {
            if (args.Length > 2)
            {
                if (args[2].ToString() == "-log")
                {
                    MasterServerMonitor.Instance.LogList = !MasterServerMonitor.Instance.LogList;
                }
            }
            else
            {
                MasterServerMonitor.Instance.List = !MasterServerMonitor.Instance.List;
            }
        }
    }

    public void ListUpdate()
    {
        if (this.list)
        {
            if (DateTime.Now.Ticks - this.lastMonitorTime > 50000000)
            {
                if (!MasterServerNetworkController.MasterServer.SendRequest(MasterEventCode.ServerListRefresh, null))
                {
                    this.failConnectMasterServer++;
                    if (this.failConnectMasterServer > 2)
                    {
                        this.list = false;
                        ServersList.Refresh(MainNetworkController.Instance, false);
                    }
                }
                this.lastMonitorTime = DateTime.Now.Ticks;
            }
            if (!this.ServerListRequestSuccess())
            {
                this.list = false;
                MasterServerMonitor.LIST_FAIL_PERIOD = 50000000;
                ServersList.Refresh(MainNetworkController.Instance, false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (this.report && DateTime.Now.Ticks - this.lastMonitorTime > 50000000)
        {
            MasterServerNetworkController.MasterServer.SendRequest(MasterEventCode.ServerListRefresh, null);
            this.lastMonitorTime = DateTime.Now.Ticks;
        }
    }
}


