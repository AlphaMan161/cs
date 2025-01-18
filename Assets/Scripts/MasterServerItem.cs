// dnSpy decompiler from Assembly-CSharp.dll class: MasterServerItem
using System;
using System.Collections;
using UnityEngine;

public class MasterServerItem : ServerItem
{
	public MasterServerItem(string in_host, int[] in_ports) : base(string.Empty, in_host, in_ports, "Master", 0, 10000)
	{
	}

	protected override void photonEventListener(PhotonEvent photonEvent)
	{
		byte code = photonEvent.Code;
		switch (code)
		{
		case 209:
			ClanManager.OnServerRequest(photonEvent.Data);
			break;
		default:
			if (code != 75)
			{
				if (code != 82)
				{
					if (code != 104)
					{
						if (code == 255)
						{
							base.IsConnected = true;
							UnityEngine.Debug.Log("[CommunicationManger] photonEventListener(): Master Server Lobby Joined");
						}
					}
					else
					{
						this.isActive = false;
						base.IsConnected = false;
						if (base.ServerItemListener != null)
						{
							this.serverItemListener(this);
						}
					}
				}
				else
				{
					this.isActive = false;
					base.IsConnected = false;
					base.Disconnect();
				}
			}
			else
			{
				this.isActive = false;
				base.IsConnected = false;
				base.Disconnect();
				if (PhotonConnection.Connection != null && PhotonConnection.Connection.Connected)
				{
					PhotonConnection.Connection.Disconnect();
				}
			}
			break;
		case 211:
		{
			int actorID = photonEvent.ActorID;
			MasterServerNetworkController.Instance.OnUserUpdateGameState(actorID, photonEvent.Data);
			break;
		}
		case 212:
		{
			int actorID = photonEvent.ActorID;
			MasterServerNetworkController.Instance.OnUserLeavesLobby(actorID, photonEvent.Data);
			break;
		}
		case 213:
		{
			int actorID = photonEvent.ActorID;
			MasterServerNetworkController.Instance.OnUserJoinsLobby(actorID, photonEvent.Data);
			break;
		}
		case 214:
			MasterServerNetworkController.Instance.OnChatList(photonEvent.Data);
			break;
		case 217:
		{
			int actorID = photonEvent.ActorID;
			MasterServerNetworkController.Instance.OnChatMessage(actorID, photonEvent.Data);
			break;
		}
		case 219:
		{
			int actorID = photonEvent.ActorID;
			MasterServerNetworkController.Instance.OnFriendRemove(actorID, photonEvent.Data);
			break;
		}
		case 220:
		{
			int actorID = photonEvent.ActorID;
			MasterServerNetworkController.Instance.OnFriendDecline(actorID, photonEvent.Data);
			break;
		}
		case 221:
		{
			int actorID = photonEvent.ActorID;
			MasterServerNetworkController.Instance.OnFriendConfirm(actorID, photonEvent.Data);
			break;
		}
		case 222:
		{
			int actorID = photonEvent.ActorID;
			MasterServerNetworkController.Instance.OnFriendRequest(actorID, photonEvent.Data);
			break;
		}
		case 223:
			MasterServerNetworkController.Instance.OnServerList(photonEvent.Data);
			break;
		case 224:
			MasterServerNetworkController.Instance.OnFriendList(photonEvent.Data);
			break;
		}
	}

	public bool SendRequest(MasterEventCode evCode, Hashtable requestData)
	{
		return PhotonConnection.Connection.ServerListSendRequest(this.host + ":" + this.ports[0], new FUFPSServerListItem.PhotonEventListener(this.photonEventListener), (byte)evCode, requestData);
	}
}
