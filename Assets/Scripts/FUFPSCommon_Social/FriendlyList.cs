// dnSpy decompiler from Assembly-CSharp.dll class: FUFPSCommon.Social.FriendlyList
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FUFPSCommon.Social
{
	public class FriendlyList
	{
		public FriendlyList(int user_id, Hashtable friendListData)
		{
			this.user_id = user_id;
			this.list = new Dictionary<int, Friend>();
			this.notConfirm = new Dictionary<int, Friend>();
			this.request = new Dictionary<int, Friend>();
			this.maybe = new Dictionary<int, Friend>();
			string name = string.Empty;
			short lvl = 0;
			byte status = 0;
			string connectionString = string.Empty;
			string name2 = string.Empty;
			byte state = 0;
			byte userOnline = 0;
			byte userMax = 0;
			foreach (object obj in friendListData.Keys)
			{
				Hashtable hashtable = (Hashtable)friendListData[(int)obj];
				name = BadWorldFilter.CheckLite((string)hashtable[(byte)212]);
				if (hashtable.ContainsKey((byte)205))
				{
					lvl = (short)hashtable[(byte)205];
				}
				if (hashtable.ContainsKey((byte)208))
				{
					state = (byte)hashtable[(byte)208];
				}
				Friend friend = new Friend((int)obj, name, lvl, 0u, (UserStatus)status, (UserState)state);
				if (hashtable.ContainsKey((byte)210))
				{
					connectionString = (string)hashtable[(byte)210];
				}
				if (hashtable.ContainsKey((byte)209))
				{
					name2 = (string)hashtable[(byte)209];
				}
				if (hashtable.ContainsKey((byte)204))
				{
					userOnline = (byte)hashtable[(byte)204];
				}
				if (hashtable.ContainsKey((byte)203))
				{
					userMax = (byte)hashtable[(byte)203];
				}
				friend.RoomInfo = new RoomInfo(name2, connectionString, (short)userOnline, (short)userMax);
				if (hashtable.ContainsKey((byte)211))
				{
					friend.Status = (UserStatus)((byte)hashtable[(byte)211]);
				}
				object obj2 = this.lockFriends;
				lock (obj2)
				{
					switch (state)
					{
					case 1:
						this.list.Add((int)obj, friend);
						break;
					case 2:
						this.notConfirm.Add((int)obj, friend);
						break;
					case 3:
						this.request.Add((int)obj, friend);
						break;
					}
				}
			}
		}

		public int UserID
		{
			get
			{
				return this.user_id;
			}
		}

		public Dictionary<int, Friend> List
		{
			get
			{
				object obj = this.lockFriends;
				Dictionary<int, Friend> result;
				lock (obj)
				{
					result = this.list;
				}
				return result;
			}
		}

		public Dictionary<int, Friend> NotConfirm
		{
			get
			{
				object obj = this.lockFriends;
				Dictionary<int, Friend> result;
				lock (obj)
				{
					result = this.notConfirm;
				}
				return result;
			}
		}

		public Dictionary<int, Friend> Request
		{
			get
			{
				object obj = this.lockFriends;
				Dictionary<int, Friend> result;
				lock (obj)
				{
					result = this.request;
				}
				return result;
			}
		}

		public bool IsFriend(int friend_id)
		{
			object obj = this.lockFriends;
			lock (obj)
			{
				if (this.list.ContainsKey(friend_id))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsFriend(Friend user)
		{
			return this.IsFriend(user.UserID);
		}

		public void OnFriendRequest(int sender_id, int target_id)
		{
			object obj = this.lockFriends;
			lock (obj)
			{
				if (sender_id == this.user_id)
				{
					this.notConfirm.Add(target_id, new Friend(target_id, "?", 0, 0u, UserStatus.Offline, UserState.NotConfirm));
				}
				else
				{
					this.request.Add(sender_id, new Friend(sender_id, "?", 0, 0u, UserStatus.Online, UserState.Request));
				}
			}
		}

		public void OnConfirmRequest(int sender_id, int target_id)
		{
			object obj = this.lockFriends;
			lock (obj)
			{
				if (sender_id == this.user_id)
				{
					Friend friend = this.request[target_id];
					this.request.Remove(target_id);
					if (this.notConfirm.ContainsKey(target_id))
					{
						this.notConfirm.Remove(target_id);
					}
					this.list.Add(target_id, friend);
					friend.State = UserState.Friend;
				}
				else
				{
					Friend friend = this.notConfirm[sender_id];
					this.notConfirm.Remove(sender_id);
					if (this.request.ContainsKey(sender_id))
					{
						this.request.Remove(sender_id);
					}
					this.list.Add(sender_id, friend);
					friend.State = UserState.Friend;
				}
			}
		}

		public void OnDeclineRequest(int sender_id, int target_id)
		{
			object obj = this.lockFriends;
			lock (obj)
			{
				if (sender_id == this.user_id)
				{
					this.request.Remove(target_id);
					if (this.notConfirm.ContainsKey(target_id))
					{
						this.notConfirm.Remove(target_id);
					}
				}
				else
				{
					this.notConfirm.Remove(sender_id);
					if (this.request.ContainsKey(sender_id))
					{
						this.request.Remove(sender_id);
					}
				}
			}
		}

		public void OnRemoveFriend(int sender_id, int target_id)
		{
			object obj = this.lockFriends;
			lock (obj)
			{
				if (sender_id == this.user_id)
				{
					if (this.list.ContainsKey(target_id))
					{
						this.list.Remove(target_id);
					}
					else if (this.notConfirm.ContainsKey(target_id))
					{
						this.notConfirm.Remove(target_id);
					}
				}
				else if (this.list.ContainsKey(sender_id))
				{
					this.list.Remove(sender_id);
				}
				else if (this.request.ContainsKey(sender_id))
				{
					this.request.Remove(sender_id);
				}
			}
		}

		public void OnFriendUpdateState(int userID, Hashtable friendData)
		{
			Friend friend = null;
			object obj = this.lockFriends;
			lock (obj)
			{
				if (this.list.ContainsKey(userID))
				{
					friend = this.list[userID];
				}
				if (this.request.ContainsKey(userID))
				{
					friend = this.request[userID];
				}
				if (this.notConfirm.ContainsKey(userID))
				{
					friend = this.notConfirm[userID];
				}
			}
			if (friend == null)
			{
				return;
			}
			string text = string.Empty;
			string text2 = string.Empty;
			byte b = 0;
			byte b2 = 0;
			if (friendData.ContainsKey((byte)211))
			{
				friend.Status = (UserStatus)((byte)friendData[(byte)211]);
			}
			if (friendData.ContainsKey((byte)209))
			{
				text = (string)friendData[(byte)209];
			}
			if (friendData.ContainsKey((byte)210))
			{
				text2 = (string)friendData[(byte)210];
			}
			if (friendData.ContainsKey((byte)204))
			{
				b = (byte)friendData[(byte)204];
			}
			if (friendData.ContainsKey((byte)203))
			{
				b2 = (byte)friendData[(byte)203];
			}
			friend.RoomInfo = new RoomInfo(text, text2, (short)b, (short)b2);
			UnityEngine.Debug.Log(string.Format("player {0} status:{1} room:{2} serverID:{3} {4}/{5}", new object[]
			{
				userID,
				friend.Status.ToString(),
				text,
				text2,
				b,
				b2
			}));
		}

		public Friend GetFriend(int friendID)
		{
			Friend result = null;
			object obj = this.lockFriends;
			lock (obj)
			{
				if (this.list.ContainsKey(friendID))
				{
					result = this.list[friendID];
				}
				if (this.request.ContainsKey(friendID))
				{
					result = this.request[friendID];
				}
				if (this.notConfirm.ContainsKey(friendID))
				{
					result = this.notConfirm[friendID];
				}
			}
			return result;
		}

		public UserState OnFriendUpdateStatus(int friendID, UserStatus friendStatus)
		{
			Friend friend = null;
			object obj = this.lockFriends;
			lock (obj)
			{
				if (this.list.ContainsKey(friendID))
				{
					friend = this.list[friendID];
				}
				if (this.request.ContainsKey(friendID))
				{
					friend = this.request[friendID];
				}
				if (this.notConfirm.ContainsKey(friendID))
				{
					friend = this.notConfirm[friendID];
				}
			}
			if (friend == null)
			{
				return UserState.NotFriend;
			}
			friend.Status = friendStatus;
			return friend.State;
		}

		private void CheckFriendlyRequest()
		{
		}

		private object lockFriends = new object();

		private int user_id;

		private Dictionary<int, Friend> list;

		private Dictionary<int, Friend> notConfirm;

		private Dictionary<int, Friend> request;

		private Dictionary<int, Friend> maybe;
	}
}
