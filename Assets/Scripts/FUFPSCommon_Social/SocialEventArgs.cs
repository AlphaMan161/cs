// dnSpy decompiler from Assembly-CSharp.dll class: FUFPSCommon.Social.SocialEventArgs
using System;

namespace FUFPSCommon.Social
{
	public class SocialEventArgs : EventArgs
	{
		public SocialEventArgs(int sender_id, int receiver_id)
		{
			this.sender_id = sender_id;
			this.receiver_id = receiver_id;
		}

		public int sender_id;

		public int receiver_id;
	}
}
