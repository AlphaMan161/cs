// dnSpy decompiler from Assembly-CSharp.dll class: FUFPSCommon.Social.SocialManager
using System;

namespace FUFPSCommon.Social
{
	public class SocialManager
	{
		public event SocialManager.EventHandler OnFriendRequest;

		public event SocialManager.EventHandler OnConfirmRequest;

		public event SocialManager.EventHandler OnRemoveFriend;

		public static SocialManager Instance
		{
			get
			{
				if (SocialManager.hInstance == null)
				{
					SocialManager.hInstance = new SocialManager();
				}
				return SocialManager.hInstance;
			}
		}

		public static void ConfirmRequest(int inviter_id, int invited_id)
		{
			if (SocialManager.Instance.OnConfirmRequest != null)
			{
				SocialManager.Instance.OnConfirmRequest(SocialManager.Instance, new SocialEventArgs(invited_id, inviter_id));
			}
		}

		public static void FriendRequest(int inviter_id, int invited_id)
		{
			if (SocialManager.Instance.OnFriendRequest != null)
			{
				SocialManager.Instance.OnFriendRequest(SocialManager.Instance, new SocialEventArgs(inviter_id, invited_id));
			}
		}

		public static void RemoveFriend(int user_id, int friend_id)
		{
			if (SocialManager.Instance.OnRemoveFriend != null)
			{
				SocialManager.Instance.OnRemoveFriend(SocialManager.Instance, new SocialEventArgs(user_id, friend_id));
			}
		}

		private static SocialManager hInstance;

		public delegate void EventHandler(object sender, SocialEventArgs e);
	}
}
