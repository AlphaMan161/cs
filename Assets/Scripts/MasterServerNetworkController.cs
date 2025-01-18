// dnSpy decompiler from Assembly-CSharp.dll class: MasterServerNetworkController
using System;
using System.Collections;
using FUFPSCommon.Social;
using UnityEngine;

public class MasterServerNetworkController
{
	public MasterServerNetworkController()
	{
		this.chatList = new ChatList((int)Auth.UserID);
	}

	public static MasterServerItem MasterServer
	{
		get
		{
			return MasterServerNetworkController.masterServerItem;
		}
	}

	public static bool Connected
	{
		get
		{
			return MasterServerNetworkController.Instance != null && MasterServerNetworkController.masterServerItem != null && MasterServerNetworkController.masterServerItem.IsConnected && MasterServerNetworkController.Instance.FriendList != null && MasterServerNetworkController.Instance.ChatList != null;
		}
	}

	public FriendlyList FriendList
	{
		get
		{
			return this.friendList;
		}
	}

	public ChatList ChatList
	{
		get
		{
			return this.chatList;
		}
	}

	public static int UserOnline
	{
		get
		{
			if (MasterServerNetworkController.Instance.ChatList == null)
			{
				return 0;
			}
			if (MasterServerNetworkController.Instance.ChatList.DisplayList.Count == 0)
			{
				return 0;
			}
			return MasterServerNetworkController.Instance.ChatList.DisplayList.Count - 1;
		}
	}

	public static int PrivateChatUsers
	{
		get
		{
			if (MasterServerNetworkController.Instance.ChatList != null)
			{
				return MasterServerNetworkController.Instance.ChatList.PrivateConversations.Count;
			}
			return 0;
		}
	}

	public static int FriendCount
	{
		get
		{
			if (MasterServerNetworkController.Instance.FriendList != null)
			{
				return MasterServerNetworkController.Instance.FriendList.List.Count;
			}
			return 0;
		}
	}

	public static int RequestCount
	{
		get
		{
			if (MasterServerNetworkController.Instance.FriendList != null)
			{
				return MasterServerNetworkController.Instance.FriendList.Request.Count;
			}
			return 0;
		}
	}

	public static int NotConfirmCount
	{
		get
		{
			if (MasterServerNetworkController.Instance.FriendList != null)
			{
				return MasterServerNetworkController.Instance.FriendList.NotConfirm.Count;
			}
			return 0;
		}
	}

	public static bool NewEvents
	{
		get
		{
			return (MasterServerNetworkController.Instance.FriendList != null && MasterServerNetworkController.RequestCount > 0) || MasterServerNetworkController.NewPrivateMessages;
		}
	}

	public static bool NewPrivateMessages
	{
		get
		{
			if (MasterServerNetworkController.Instance.ChatList != null && MasterServerNetworkController.Instance.ChatList.PrivateConversations.Count > 0)
			{
				foreach (Conversation conversation in MasterServerNetworkController.Instance.ChatList.PrivateConversations.Values)
				{
					if (conversation.NewMessages)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}
	}

	public bool AddLastGameUser(Player user)
	{
		return false;
	}

	public static MasterServerNetworkController Instance
	{
		get
		{
			if (MasterServerNetworkController.instance == null)
			{
				MasterServerNetworkController.instance = new MasterServerNetworkController();
			}
			return MasterServerNetworkController.instance;
		}
	}

	public static bool ConnectToMaster(MonoBehaviour serverListBehaviour)
	{
		MasterServerNetworkController.needReconnect = true;
		MasterServerNetworkController.Instance.serverListBehaviour = serverListBehaviour;
		if (MasterServerNetworkController.masterServerItem == null)
		{
			string host = ServerConf.MasterServerItem.Host;
			MasterServerNetworkController.masterServerItem = new MasterServerItem(host, ServerConf.MasterServerItem.Ports);
			MasterServerNetworkController.masterServerItem.ServerItemListener = new ServerItem.ServerItemListenerDelegate(MasterServerNetworkController.onServerItemListener);
			MasterServerNetworkController.StartReconnectMasterServer();
		}
		else if (!MasterServerNetworkController.masterServerItem.IsConnected)
		{
			MasterServerNetworkController.StartReconnectMasterServer();
		}
		return false;
	}

	private static void StartReconnectMasterServer()
	{
		MasterServerNetworkController.Instance.serverListBehaviour.StartCoroutine(MasterServerNetworkController.ReconnectMasterServer());
	}

	private static IEnumerator ConnectMasterServer()
	{
		UnityEngine.Debug.Log("[MasterServerNetworkController] ConnectMasterServer() host:" + MasterServerNetworkController.masterServerItem.Host);
		MasterServerNetworkController.ReconnectMasterServer();
		yield return null;
		yield break;
	}

	private static IEnumerator ReconnectMasterServer()
	{
		UnityEngine.Debug.Log("[MasterServerNetworkController] ReconnectMasterServer() host:" + MasterServerNetworkController.masterServerItem.Host);
		if (MasterServerNetworkController.masterServerItem.IsConnected)
		{
			UnityEngine.Debug.Log("[MasterServerNetworkController] ReconnectMasterServer() 3 host:" + MasterServerNetworkController.masterServerItem.Host);
			yield return null;
		}
		MasterServerNetworkController.ReconnectTimeout += 2f;
		yield return new WaitForSeconds(MasterServerNetworkController.ReconnectTimeout);
		UnityEngine.Debug.Log("[MasterServerNetworkController] ReconnectMasterServer() 2 host:" + MasterServerNetworkController.masterServerItem.Host);
		if (!MasterServerNetworkController.masterServerItem.Refresh())
		{
		}
		yield return null;
		yield break;
	}

	public static void onServerItemListener(ServerItem serverItem)
	{
		if (MasterServerNetworkController.needReconnect)
		{
			MasterServerNetworkController.Instance.serverListBehaviour.StartCoroutine(MasterServerNetworkController.ReconnectMasterServer());
		}
	}

	public static bool DisconnectFromMaster()
	{
		MasterServerNetworkController.needReconnect = false;
		MasterServerNetworkController.masterServerItem.Disconnect();
		return false;
	}

	public static bool SendFriendRequest(int userID)
	{
		if (userID == MasterServerNetworkController.Instance.FriendList.UserID)
		{
			return false;
		}
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)207] = userID;
		MasterServerNetworkController.masterServerItem.SendRequest(MasterEventCode.FriendRequest, hashtable);
		return false;
	}

	public static bool FriendConfirm(int userID)
	{
		if (userID == MasterServerNetworkController.Instance.FriendList.UserID)
		{
			return false;
		}
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)207] = userID;
		MasterServerNetworkController.masterServerItem.SendRequest(MasterEventCode.FriendConfirm, hashtable);
		return false;
	}

	public static bool IsFriend(int user_id)
	{
		return MasterServerNetworkController.Instance.FriendList != null && MasterServerNetworkController.Instance.FriendList.List != null && MasterServerNetworkController.Instance.FriendList.List.ContainsKey(user_id);
	}

	public static bool FriendDecline(int userID)
	{
		if (userID == MasterServerNetworkController.Instance.FriendList.UserID)
		{
			return false;
		}
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)207] = userID;
		MasterServerNetworkController.masterServerItem.SendRequest(MasterEventCode.FriendDecline, hashtable);
		return false;
	}

	public static bool FriendRemove(int userID)
	{
		if (userID == MasterServerNetworkController.Instance.FriendList.UserID)
		{
			return false;
		}
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)207] = userID;
		MasterServerNetworkController.masterServerItem.SendRequest(MasterEventCode.FriendRemove, hashtable);
		return false;
	}

	public bool OnFriendRequest(int userID, Hashtable data)
	{
		if (this.FriendList == null)
		{
			return false;
		}
		int num = (int)data[(byte)207];
		UnityEngine.Debug.Log(string.Format("FRIEND REQUEST EVENT player:{0} target:{1}", userID, num));
		if (userID == this.FriendList.UserID)
		{
			MasterServerNetworkController.masterServerItem.SendRequest(MasterEventCode.FriendListRefresh, null);
			if (this.ChatList.List.ContainsKey(num))
			{
				this.ChatList.List[num].State = UserState.Request;
			}
		}
		else if (num == this.FriendList.UserID)
		{
			MasterServerNetworkController.masterServerItem.SendRequest(MasterEventCode.FriendListRefresh, null);
			if (this.ChatList.List.ContainsKey(userID))
			{
				this.ChatList.List[userID].State = UserState.NotConfirm;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Friend Request User Leak!");
		}
		return false;
	}

	public bool OnFriendConfirm(int userID, Hashtable data)
	{
		if (this.FriendList == null)
		{
			return false;
		}
		int num = (int)data[(byte)207];
		UnityEngine.Debug.Log(string.Format("FRIEND CONFIRM EVENT player:{0} target:{1}", userID, num));
		if (userID == this.FriendList.UserID)
		{
			this.FriendList.OnConfirmRequest(userID, num);
			if (this.ChatList.List.ContainsKey(num))
			{
				this.ChatList.List[num].State = UserState.Friend;
			}
		}
		else if (num == this.FriendList.UserID)
		{
			this.FriendList.OnConfirmRequest(userID, num);
			if (this.ChatList.List.ContainsKey(userID))
			{
				this.ChatList.List[userID].State = UserState.Friend;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Friend Confirm User Leak!");
		}
		MasterServerNetworkController.masterServerItem.SendRequest(MasterEventCode.FriendListRefresh, null);
		return false;
	}

	public bool OnFriendDecline(int userID, Hashtable data)
	{
		if (this.FriendList == null)
		{
			return false;
		}
		int num = (int)data[(byte)207];
		UnityEngine.Debug.Log(string.Format("FRIEND DECLINE EVENT player:{0} target:{1}", userID, num));
		if (userID == this.FriendList.UserID)
		{
			this.FriendList.OnDeclineRequest(userID, num);
			if (this.ChatList.List.ContainsKey(num))
			{
				this.ChatList.List[num].State = UserState.NotFriend;
			}
		}
		else if (num == this.FriendList.UserID)
		{
			this.FriendList.OnDeclineRequest(userID, num);
			if (this.ChatList.List.ContainsKey(userID))
			{
				this.ChatList.List[userID].State = UserState.NotFriend;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Friend Decline User Leak!");
		}
		return false;
	}

	public bool OnFriendRemove(int userID, Hashtable data)
	{
		if (this.FriendList == null)
		{
			return false;
		}
		int num = (int)data[(byte)207];
		UnityEngine.Debug.Log(string.Format("FRIEND REMOVE EVENT player:{0} target:{1}", userID, num));
		if (userID == this.FriendList.UserID)
		{
			this.FriendList.OnRemoveFriend(userID, num);
			MasterServerNetworkController.masterServerItem.SendRequest(MasterEventCode.FriendListRefresh, null);
			if (this.ChatList.List.ContainsKey(num))
			{
				this.ChatList.List[num].State = UserState.NotFriend;
			}
		}
		else if (num == this.FriendList.UserID)
		{
			this.FriendList.OnRemoveFriend(userID, num);
			MasterServerNetworkController.masterServerItem.SendRequest(MasterEventCode.FriendListRefresh, null);
			if (this.ChatList.List.ContainsKey(userID))
			{
				this.ChatList.List[userID].State = UserState.NotFriend;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Friend Confirm User Leak!");
		}
		return false;
	}

	private static bool MessageEmpty(string message)
	{
		return message.Trim() == string.Empty;
	}

	public static bool SendChatMessage(string message, BattleChat.MessageType type, Player targetUser)
	{
		if (MasterServerNetworkController.MessageEmpty(message))
		{
			return false;
		}
		if (message.Trim().StartsWith("/ban"))
		{
			type = BattleChat.MessageType.COMMAND;
		}
		if (message.Trim().StartsWith("/vip"))
		{
			type = BattleChat.MessageType.COMMAND;
		}
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)77] = message;
		if (type != BattleChat.MessageType.PUBLIC)
		{
			hashtable[(byte)80] = (byte)type;
		}
		if (targetUser != null)
		{
			hashtable[(byte)68] = targetUser.UserID;
		}
		MasterServerNetworkController.masterServerItem.SendRequest(MasterEventCode.Chat, hashtable);
		ChatList.UpdateLastMessageTime();
		return true;
	}

	public static bool SendClanEvent(ClanEventCode code, int clan_id, Hashtable data)
	{
		UnityEngine.Debug.LogError("SendClanEvent");
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)0] = code;
		hashtable[(byte)1] = clan_id;
		hashtable[(byte)2] = data;
		MasterServerNetworkController.masterServerItem.SendRequest(MasterEventCode.Clan, hashtable);
		return true;
	}

	public static void SendConsole(string[] args)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)46] = args;
		MasterServerNetworkController.masterServerItem.SendRequest(MasterEventCode.Console, hashtable);
	}

	public bool OnChatMessage(int userID, Hashtable data)
	{
		if (this.FriendList == null)
		{
			return false;
		}
		int num = -1;
		string text = (string)data[(byte)77];
		string empty = string.Empty;
		BattleChat.MessageType messageType = BattleChat.MessageType.PUBLIC;
		if (data.ContainsKey((byte)80))
		{
			messageType = (BattleChat.MessageType)((byte)data[(byte)80]);
		}
		if (data.ContainsKey((byte)68))
		{
			num = (int)data[(byte)68];
		}
		UnityEngine.Debug.Log("[MasterServerNetworkController] OnChatMessage() " + string.Format("CHAT MESSAGE EVENT player:{0} target:{1} message:\"{2}\"", userID, num, text));
		if (messageType == BattleChat.MessageType.PRIVATE)
		{
			if ((long)num == (long)((ulong)Auth.UserID))
			{
				if (this.chatList.PrivateConversations.ContainsKey(userID))
				{
					Conversation conversation = this.chatList.PrivateConversations[userID];
					conversation.Add(new ChatMessage(conversation.User.Name, text, messageType));
				}
				else if (this.chatList.List.ContainsKey(userID))
				{
					this.chatList.AddPrivateConversation(this.chatList.List[userID]);
					Conversation conversation = this.chatList.PrivateConversations[userID];
					conversation.Add(new ChatMessage(conversation.User.Name, text, messageType));
				}
			}
			else if ((long)userID == (long)((ulong)Auth.UserID))
			{
				if (this.chatList.PrivateConversations.ContainsKey(num))
				{
					Conversation conversation = this.chatList.PrivateConversations[num];
					conversation.Add(new ChatMessage(LocalUser.Name, text, messageType));
				}
				else if (this.chatList.List.ContainsKey(num))
				{
					this.chatList.AddPrivateConversation(this.chatList.List[num]);
					Conversation conversation = this.chatList.PrivateConversations[num];
					conversation.Add(new ChatMessage(LocalUser.Name, text, messageType));
				}
			}
			return false;
		}
		return false;
	}

	public bool OnFriendList(Hashtable friendListData)
	{
		if (Configuration.EnableMasterRL)
		{
			MasterServerMonitor.Instance.List = true;
		}
		this.friendList = new FriendlyList((int)Auth.UserID, friendListData);
		return false;
	}

	public bool OnChatList(Hashtable chatListData)
	{
		if (this.FriendList == null)
		{
			return false;
		}
		this.chatList = new ChatList((int)Auth.UserID);
		UnityEngine.Debug.Log("[MasterServerNetworkController] OnChatList() Lobby User List count: " + chatListData.Count);
		foreach (object obj in chatListData.Keys)
		{
			Hashtable userData = (Hashtable)chatListData[(int)obj];
			UserState state = UserState.NotFriend;
			Friend friend = this.FriendList.GetFriend((int)obj);
			if (friend != null)
			{
				state = friend.State;
				if (friend.Status == UserStatus.Offline)
				{
					friend.Status = UserStatus.Online;
				}
			}
			this.chatList.AddUser((int)obj, userData, state);
		}
		return false;
	}

	public bool OnUserJoinsLobby(int userID, Hashtable userJoinData)
	{
		if (this.FriendList == null)
		{
			return false;
		}
		UserState state = this.FriendList.OnFriendUpdateStatus(userID, UserStatus.Online);
		this.chatList.AddUser(userID, userJoinData, state);
		return false;
	}

	public bool OnUserLeavesLobby(int userID, Hashtable userJoinData)
	{
		if (this.chatList == null)
		{
			return false;
		}
		SocialPlayer socialPlayer = this.chatList.OnUserUpdateState(userID, userJoinData);
		if (socialPlayer != null)
		{
			socialPlayer.Status = UserStatus.Offline;
		}
		this.chatList.RemoveUser(userID);
		return false;
	}

	public bool OnUserUpdateGameState(int userID, Hashtable userGameStateData)
	{
		if (this.FriendList == null)
		{
			return false;
		}
		int num = (int)userGameStateData[(byte)207];
		UnityEngine.Debug.Log(string.Format("FRIEND UPDATE GAME STATE EVENT player:{0} target:{1}", userID, num));
		if (userID == this.FriendList.UserID)
		{
			this.FriendList.OnFriendUpdateState(num, userGameStateData);
		}
		else
		{
			UnityEngine.Debug.LogError("Friend Update State Leak!");
		}
		this.ChatList.OnUserUpdateState(num, userGameStateData);
		return false;
	}

	public bool OnServerList(Hashtable data)
	{
		if (this.FriendList == null)
		{
			return false;
		}
		MasterServerMonitor.Instance.OnReport(data);
		return false;
	}

	private static MasterServerNetworkController instance;

	private static MasterServerItem masterServerItem;

	private static bool needReconnect;

	private FriendlyList friendList;

	private ChatList chatList;

	private static readonly int USER_LAST_GAME_MAX = 50;

	private MonoBehaviour serverListBehaviour;

	private static float ReconnectTimeout = -2f;

	private bool searchInProgess;

	public delegate void EventHandler(object sender, object e);
}
