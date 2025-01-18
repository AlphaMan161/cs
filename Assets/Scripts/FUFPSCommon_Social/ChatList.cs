// dnSpy decompiler from Assembly-CSharp.dll class: FUFPSCommon.Social.ChatList
using System;
using System.Collections;
using System.Collections.Generic;

namespace FUFPSCommon.Social
{
	public class ChatList
	{
		public ChatList(int meID)
		{
			this.list = new Dictionary<int, SocialPlayer>();
		}

		public static void UpdateLastMessageTime()
		{
			ChatList.lastMessageTime = DateTime.Now.Ticks;
		}

		public static bool CanSendMessage
		{
			get
			{
				return ChatList.lastMessageTime + ChatList.MessageMinPeriod < DateTime.Now.Ticks;
			}
		}

		public object LockDisplayList
		{
			get
			{
				return this.lockDisplayList;
			}
		}

		public Dictionary<int, Conversation> PrivateConversations
		{
			get
			{
				return ChatList.privateConversations;
			}
		}

		public Dictionary<int, SocialPlayer> List
		{
			get
			{
				return this.list;
			}
		}

		public List<SocialPlayer> DisplayList
		{
			get
			{
				return this.displayList;
			}
		}

		public string DisplayFilter
		{
			get
			{
				return this.displayFilter;
			}
			set
			{
				if (this.displayFilter != value)
				{
					this.refreshDisplayList = true;
				}
				this.displayFilter = value;
			}
		}

		public void TryRefreshDisplayList()
		{
			if (this.refreshDisplayListComplete)
			{
				this.refreshDisplayListComplete = false;
				this.displayList = this.tmpdisplayList;
			}
			if (this.refreshDisplayList || DateTime.Now.Ticks - this.lastRefreshTicks > ChatList.RefreshTime)
			{
				this.refreshDisplayListComplete = false;
				this.lastRefreshTicks = DateTime.Now.Ticks;
				this.RefreshDisplayList();
			}
		}

		private void RefreshDisplayList()
		{
			this.refreshDisplayListComplete = false;
			if (!this.refreshDisplayListInProcess)
			{
				this.refreshDisplayListInProcess = true;
				this.refreshDisplayList = false;
				this.tmpdisplayList = new List<SocialPlayer>();
				foreach (SocialPlayer socialPlayer in this.list.Values)
				{
					if (socialPlayer.UserID != LocalUser.UserID)
					{
						if (this.displayFilter == string.Empty || socialPlayer.Name.StartsWith(this.displayFilter))
						{
							this.tmpdisplayList.Add(socialPlayer);
						}
					}
				}
				this.refreshDisplayListComplete = true;
				this.refreshDisplayListInProcess = false;
			}
		}

		public bool AddUser(int userID, Hashtable userData)
		{
			return this.AddUser(userID, userData, UserState.NotFriend);
		}

		public bool AddUser(int userID, Hashtable userData, UserState state)
		{
			SocialPlayer socialPlayer = null;
			if (ChatList.privateConversations.ContainsKey(userID) && ChatList.privateConversations[userID] != null)
			{
				socialPlayer = ChatList.privateConversations[userID].User;
				socialPlayer.Status = (UserStatus)((byte)userData[(byte)211]);
				socialPlayer.State = state;
			}
			if (this.list.ContainsKey(userID))
			{
				return false;
			}
			if (socialPlayer == null)
			{
				string name = (string)userData[(byte)212];
				UserStatus status = UserStatus.Online;
				if (userData.ContainsKey((byte)211))
				{
					status = (UserStatus)((byte)userData[(byte)211]);
				}
				socialPlayer = new SocialPlayer(userID, name, 0, status, state);
			}
			this.list.Add(userID, socialPlayer);
			return true;
		}

		public bool RemoveUser(int userID)
		{
			if (!this.list.ContainsKey(userID))
			{
				return false;
			}
			this.list.Remove(userID);
			return true;
		}

		public void AddPrivateConversation(SocialPlayer user)
		{
			if (!ChatList.privateConversations.ContainsKey(user.UserID))
			{
				ChatList.privateConversations.Add(user.UserID, new Conversation(user));
			}
		}

		private bool Remove(int userID)
		{
			return this.list.Remove(userID);
		}

		public SocialPlayer OnUserUpdateState(int userID, Hashtable userData)
		{
			SocialPlayer socialPlayer = null;
			if (this.list.ContainsKey(userID))
			{
				socialPlayer = this.list[userID];
			}
			if (ChatList.privateConversations.ContainsKey(userID) && ChatList.privateConversations[userID] != null)
			{
				socialPlayer = ChatList.privateConversations[userID].User;
			}
			if (socialPlayer == null)
			{
				return null;
			}
			string name = string.Empty;
			string connectionString = string.Empty;
			short userOnline = 0;
			short userMax = 0;
			if (userData.ContainsKey((byte)211))
			{
				socialPlayer.Status = (UserStatus)((byte)userData[(byte)211]);
			}
			if (userData.ContainsKey((byte)209))
			{
				name = (string)userData[(byte)209];
			}
			if (userData.ContainsKey((byte)210))
			{
				connectionString = (string)userData[(byte)210];
			}
			if (userData.ContainsKey((byte)204))
			{
				userOnline = Convert.ToInt16((byte)userData[(byte)204]);
			}
			if (userData.ContainsKey((byte)203))
			{
				userMax = Convert.ToInt16((byte)userData[(byte)203]);
			}
			socialPlayer.RoomInfo = new RoomInfo(name, connectionString, userOnline, userMax);
			return socialPlayer;
		}

		public static readonly long MessageMinPeriod = 50000000L;

		private static long lastMessageTime = DateTime.Now.Ticks;

		private static readonly long RefreshTime = 100000000L;

		private object lockDisplayList = new object();

		private object lockPrivateConversations = new object();

		private static Dictionary<int, Conversation> privateConversations = new Dictionary<int, Conversation>();

		private Dictionary<int, SocialPlayer> list = new Dictionary<int, SocialPlayer>();

		private long lastRefreshTicks = DateTime.Now.Ticks;

		private List<SocialPlayer> tmpdisplayList = new List<SocialPlayer>();

		private List<SocialPlayer> displayList = new List<SocialPlayer>();

		private string displayFilter = string.Empty;

		private bool refreshDisplayList = true;

		private bool refreshDisplayListComplete;

		private bool refreshDisplayListInProcess;
	}
}
