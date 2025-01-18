// dnSpy decompiler from Assembly-CSharp.dll class: GameLogicServerMonitor
using System;
using System.Collections;
using UnityEngine;

public class GameLogicServerMonitor : MonoBehaviour
{
	public static GameLogicServerMonitor Instance
	{
		get
		{
			if (GameLogicServerMonitor.mInstance == null)
			{
				GameLogicServerMonitor.mInstance = (new GameObject("GameLogicServerMonitor").AddComponent(typeof(GameLogicServerMonitor)) as GameLogicServerMonitor);
			}
			return GameLogicServerMonitor.mInstance;
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
			if (DateTime.Now.Ticks - this.lastServerListTimeRequest > (long)GameLogicServerMonitor.LIST_FAIL_PERIOD)
			{
				return false;
			}
		}
		return true;
	}

	public void Reset()
	{
		this.failConnectGameLogicServer = 0;
		this.lastMonitorTime = DateTime.Now.Ticks - 50000000L + 10000000L;
	}

	private void ProcessReport(Hashtable data)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		DebugConsole.Instance.Clear();
		string message = string.Format("{0, -10} {1, -20} {2, -10} {3, -10} {4, -10} {5, -10} {6, -10} {7, -10} {8, -10} {9, -10}", new object[]
		{
			"No.",
			"IP Address",
			"Count",
			"Online",
			"Offline",
			"Sent",
			"Queued",
			"MaxT",
			"MinT",
			"AveT"
		});
		DebugConsole.Instance.Log(message, string.Empty, LogType.Assert);
		int num5 = (int)data[6];
		Hashtable hashtable = (Hashtable)data[7];
		foreach (object obj in hashtable.Keys)
		{
			string text = (string)obj;
			Hashtable hashtable2 = (Hashtable)hashtable[obj];
			int num6 = (int)hashtable2[6];
			int num7 = (int)hashtable2[1];
			int num8 = (int)hashtable2[2];
			int num9 = (int)hashtable2[10];
			int num10 = (int)hashtable2[11];
			long num11 = (long)hashtable2[7];
			long num12 = (long)hashtable2[8];
			long num13 = (long)hashtable2[9];
			string serverNumber = ServersList.GetServerNumber(text);
			decimal value = 0m;
			int num14;
			if (decimal.TryParse(serverNumber, out value))
			{
				num14 = Convert.ToInt32(value);
			}
			else
			{
				num14 = num4--;
			}
			string message2 = string.Format("No.{0, -10} {1, -20} {2, -10} {3, -10} {4, -10} {5, -10} {6, -10}  {7, -10}  {8, -10}  {9, -10}", new object[]
			{
				num14,
				text,
				num6,
				num7,
				num8,
				num9,
				num10,
				num11,
				num12,
				num13
			});
			DebugConsole.Instance.Log(message2, string.Empty, LogType.Warning);
			num3++;
			num += num7 + num8;
			num2 += num6;
		}
		DebugConsole.Instance.Log(string.Format("Total Servers: {0} Peers:{1} Players: {2} GameLogicClients: {3}", new object[]
		{
			num3,
			num2,
			num,
			num5
		}), string.Empty, LogType.Assert);
		if (!this.report)
		{
			return;
		}
	}

	public void OnReport(Hashtable serverData)
	{
		if (this.report)
		{
			this.ProcessReport(serverData);
		}
	}

	public void ProcessCommand(params object[] args)
	{
		if (args.Length == 1)
		{
			GameLogicServerMonitor.Instance.Report = !GameLogicServerMonitor.Instance.Report;
			return;
		}
		if (args[1].ToString() == "-d")
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
			return;
		}
	}

	private void FixedUpdate()
	{
		if (this.report && DateTime.Now.Ticks - this.lastMonitorTime > 50000000L)
		{
			GameLogicServerNetworkController.GameLogicServer.SendRequest(GameLogicEventCode.ServerListRefresh, null);
			this.lastMonitorTime = DateTime.Now.Ticks;
		}
	}

	private const int MONITOR_PERIOD = 50000000;

	private const int LIST_PERIOD = 50000000;

	private static GameLogicServerMonitor mInstance;

	private bool report;

	private bool list = true;

	private static int LIST_FAIL_PERIOD = 60000000;

	private long lastMonitorTime = DateTime.Now.Ticks;

	private long lastServerListTime;

	private string detailServer = string.Empty;

	private int failConnectGameLogicServer;

	private bool logList;

	private long lastServerListTimeRequest;

	private struct ServerReport
	{
		public ServerReport(string serverIP, ServerState state, LogType logType)
		{
			this.serverIP = serverIP;
			this.state = state;
			this.logType = logType;
		}

		public string serverIP;

		public ServerState state;

		public LogType logType;
	}
}
