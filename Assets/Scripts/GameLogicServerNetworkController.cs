// dnSpy decompiler from Assembly-CSharp.dll class: GameLogicServerNetworkController
using System;
using System.Collections;
using UnityEngine;

public class GameLogicServerNetworkController
{
	public static GameLogicServerItem GameLogicServer
	{
		get
		{
			return GameLogicServerNetworkController.gameLogicServerItem;
		}
	}

	public static bool Connected
	{
		get
		{
			return GameLogicServerNetworkController.Instance != null && GameLogicServerNetworkController.gameLogicServerItem != null && GameLogicServerNetworkController.gameLogicServerItem.IsConnected;
		}
	}

	public bool AddLastGameUser(Player user)
	{
		return false;
	}

	public static GameLogicServerNetworkController Instance
	{
		get
		{
			if (GameLogicServerNetworkController.instance == null)
			{
				GameLogicServerNetworkController.instance = new GameLogicServerNetworkController();
			}
			return GameLogicServerNetworkController.instance;
		}
	}

	public static bool ConnectToGameLogic(MonoBehaviour serverListBehaviour)
	{
		if (ServerConf.GMList.Count == 0)
		{
			return false;
		}
		GameLogicServerNetworkController.needReconnect = true;
		GameLogicServerNetworkController.Instance.serverListBehaviour = serverListBehaviour;
		if (GameLogicServerNetworkController.gameLogicServerItem == null)
		{
			ServerItem serverItem = ServerConf.GMList[GameLogicServerNetworkController.currentServerIndex];
			GameLogicServerNetworkController.gameLogicServerItem = new GameLogicServerItem(serverItem.Host, serverItem.Ports);
			GameLogicServerNetworkController.gameLogicServerItem.ServerItemListener = new ServerItem.ServerItemListenerDelegate(GameLogicServerNetworkController.onServerItemListener);
			GameLogicServerNetworkController.StartReconnectGameLogicServer();
		}
		else if (!GameLogicServerNetworkController.gameLogicServerItem.IsConnected)
		{
			GameLogicServerNetworkController.StartReconnectGameLogicServer();
		}
		return false;
	}

	public static void ManualReconnectGameLogicServer(int index)
	{
		if (index >= ServerConf.GMList.Count || index < 0)
		{
			UnityEngine.Debug.LogError("Invalid GL index!");
			return;
		}
		if (GameLogicServerNetworkController.gameLogicServerItem.IsConnected)
		{
			GameLogicServerNetworkController.DisconnectFromGameLogic();
		}
		GameLogicServerNetworkController.currentServerIndex = index;
		ServerItem serverItem = ServerConf.GMList[GameLogicServerNetworkController.currentServerIndex];
		if (GameLogicServerNetworkController.gameLogicServerItem == null)
		{
			GameLogicServerNetworkController.gameLogicServerItem = new GameLogicServerItem(serverItem.Host, serverItem.Ports);
			GameLogicServerNetworkController.gameLogicServerItem.ServerItemListener = new ServerItem.ServerItemListenerDelegate(GameLogicServerNetworkController.onServerItemListener);
		}
		else
		{
			GameLogicServerNetworkController.gameLogicServerItem.Reset(serverItem.Host, serverItem.Ports);
		}
		GameLogicServerNetworkController.Instance.serverListBehaviour.StartCoroutine(GameLogicServerNetworkController.ReconnectGameLogicServer());
	}

	private static void StartReconnectGameLogicServer()
	{
		if (GameLogicServerNetworkController.gameLogicServerItem.IsConnected)
		{
			return;
		}
		ServerItem serverItem = ServerConf.GMList.Find((ServerItem x) => x.IsRecommended);
		UnityEngine.Debug.LogError("GM Recommended server: " + serverItem);
		if (serverItem == null)
		{
			GameLogicServerNetworkController.currentServerIndex = LocalUser.UserID % ServerConf.GMList.Count;
			serverItem = ServerConf.GMList[GameLogicServerNetworkController.currentServerIndex];
		}
		if (GameLogicServerNetworkController.gameLogicServerItem == null)
		{
			GameLogicServerNetworkController.gameLogicServerItem = new GameLogicServerItem(serverItem.Host, serverItem.Ports);
			GameLogicServerNetworkController.gameLogicServerItem.ServerItemListener = new ServerItem.ServerItemListenerDelegate(GameLogicServerNetworkController.onServerItemListener);
		}
		else
		{
			GameLogicServerNetworkController.gameLogicServerItem.Reset(serverItem.Host, serverItem.Ports);
		}
		GameLogicServerNetworkController.Instance.serverListBehaviour.StartCoroutine(GameLogicServerNetworkController.ReconnectGameLogicServer());
	}

	private static void RepeatReconnectGameLogicServer()
	{
		if (GameLogicServerNetworkController.gameLogicServerItem.IsConnected)
		{
			return;
		}
		if (PlayerManager.Instance == null)
		{
			GameLogicServerNetworkController.currentServerIndex++;
			if (GameLogicServerNetworkController.currentServerIndex >= ServerConf.GMList.Count)
			{
				GameLogicServerNetworkController.currentServerIndex = 0;
			}
		}
		ServerItem serverItem = ServerConf.GMList[GameLogicServerNetworkController.currentServerIndex];
		if (GameLogicServerNetworkController.gameLogicServerItem == null)
		{
			GameLogicServerNetworkController.gameLogicServerItem = new GameLogicServerItem(serverItem.Host, serverItem.Ports);
			GameLogicServerNetworkController.gameLogicServerItem.ServerItemListener = new ServerItem.ServerItemListenerDelegate(GameLogicServerNetworkController.onServerItemListener);
		}
		else
		{
			GameLogicServerNetworkController.gameLogicServerItem.Reset(serverItem.Host, serverItem.Ports);
		}
		if (GameLogicServerNetworkController.Instance.serverListBehaviour != null)
		{
			GameLogicServerNetworkController.Instance.serverListBehaviour.StartCoroutine(GameLogicServerNetworkController.ReconnectGameLogicServer());
		}
		else if (NetworkManager.Instance != null)
		{
			NetworkManager.Instance.StartCoroutine(GameLogicServerNetworkController.ReconnectGameLogicServer());
		}
	}

	private static IEnumerator ReconnectGameLogicServer()
	{
		UnityEngine.Debug.Log("[GameLogicServerNetworkController] ReconnectGameLogicServer() host:" + GameLogicServerNetworkController.gameLogicServerItem.Host);
		yield return new WaitForSeconds(GameLogicServerNetworkController.ReconnectTimeout);
		GameLogicServerNetworkController.ReconnectTimeout = 5f;
		UnityEngine.Debug.Log("[GameLogicServerNetworkController] ReconnectGameLogicServer() 2 host:" + GameLogicServerNetworkController.gameLogicServerItem.Host);
		if (!GameLogicServerNetworkController.gameLogicServerItem.Refresh())
		{
		}
		yield return null;
		yield break;
	}

	public static void onServerItemListener(ServerItem serverItem)
	{
		if (GameLogicServerNetworkController.needReconnect)
		{
			GameLogicServerNetworkController.RepeatReconnectGameLogicServer();
		}
	}

	public static bool DisconnectFromGameLogic()
	{
		GameLogicServerNetworkController.needReconnect = false;
		GameLogicServerNetworkController.gameLogicServerItem.Disconnect();
		return false;
	}

	public static void SendConsole(string[] args)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)46] = args;
		GameLogicServerNetworkController.gameLogicServerItem.SendRequest(GameLogicEventCode.Console, hashtable);
	}

	public static void SendChange(byte changeType)
	{
		GameLogicServerNetworkController.SendChange(changeType, null);
	}

	public static void SendChange(byte changeType, object data)
	{
		UnityEngine.Debug.LogError(string.Format("SendChange changeType:{0} data:{1}", changeType, data));
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)51] = changeType;
		hashtable[(byte)213] = data;
		GameLogicServerNetworkController.gameLogicServerItem.SendRequest(GameLogicEventCode.Change, hashtable);
	}

	public static void OnChange(Hashtable changeData)
	{
	}

	public bool OnUserJoinsLobby(int userID, Hashtable userJoinData)
	{
		return false;
	}

	public bool OnUserLeavesLobby(int userID, Hashtable userJoinData)
	{
		return false;
	}

	public bool OnServerList(Hashtable data)
	{
		this.ReportList(data);
		return false;
	}

	private void ReportList(Hashtable data)
	{
		UnityEngine.Debug.Log(string.Format("GameLogicNetworkController: ReportList({0})", data));
		GameLogicServerMonitor.Instance.OnReport(data);
	}

	public string GameLogicErrorNotificationMessage(string code)
	{
		UnityEngine.Debug.Log(string.Format("Game Logic Error Message: {0}!", code));
		return LanguageManager.GetText(code);
	}

	private static int currentServerIndex;

	private static GameLogicServerNetworkController instance;

	private static GameLogicServerItem gameLogicServerItem;

	private static bool needReconnect;

	private static readonly int USER_LAST_GAME_MAX = 50;

	private MonoBehaviour serverListBehaviour;

	private static float ReconnectTimeout;

	public delegate void EventHandler(object sender, object e);
}
