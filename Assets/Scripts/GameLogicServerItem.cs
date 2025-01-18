// dnSpy decompiler from Assembly-CSharp.dll class: GameLogicServerItem
using System;
using System.Collections;
using UnityEngine;

public class GameLogicServerItem : ServerItem
{
	public GameLogicServerItem(string in_host, int[] in_ports) : base(string.Empty, in_host, in_ports, "FUFPSGameLogic", 0, 10000)
	{
	}

	public void Reset(string host, int[] ports)
	{
		this.host = host;
		this.ports = ports;
	}

	protected override void photonEventListener(PhotonEvent photonEvent)
	{
		byte code = photonEvent.Code;
		switch (code)
		{
		case 204:
			this.ProcessError(photonEvent.Data);
			break;
		case 205:
			this.ProcessSaveComplete(photonEvent.Data);
			break;
		case 206:
			GameLogicServerNetworkController.OnChange(photonEvent.Data);
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
							this.ProcessJoinLobby();
							UnityEngine.Debug.Log("[GameLogicNetworkController] photonEventListener(): Game Logic Server Lobby Joined " + this.host);
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
		case 210:
			GameLogicServerNetworkController.Instance.OnServerList(photonEvent.Data);
			break;
		case 212:
		{
			int actorID = photonEvent.ActorID;
			GameLogicServerNetworkController.Instance.OnUserLeavesLobby(actorID, photonEvent.Data);
			break;
		}
		case 213:
		{
			int actorID = photonEvent.ActorID;
			GameLogicServerNetworkController.Instance.OnUserJoinsLobby(actorID, photonEvent.Data);
			break;
		}
		}
	}

	public void ProcessSaveComplete(Hashtable data)
	{
		Hashtable hashtable = (Hashtable)data[(byte)52];
		if (hashtable != null)
		{
			GameLogicSaveController.ProcessSave(hashtable);
		}
		if (hashtable.ContainsKey((byte)53))
		{
			GameLogicErrorType gameLogicErrorType = (GameLogicErrorType)((byte)data[(byte)53]);
			string text = (string)data[(byte)54];
			SavePopup.Error(text);
			string message = GameLogicServerNetworkController.Instance.GameLogicErrorNotificationMessage(text);
			NotificationWindow.Add(new Notification(Notification.Type.GAMELOGIC_ERROR, LanguageManager.GetText("SAVE ERROR"), message)
			{
				WindowSize = new Vector2(500f, 243f)
			});
		}
		else
		{
			SavePopup.Complete();
		}
		UnityEngine.Debug.Log(string.Format("COMBAT PLAYER SAVE COMPLETED: {0}!", data));
	}

	public void ProcessError(Hashtable data)
	{
		GameLogicErrorType gameLogicErrorType = (GameLogicErrorType)((byte)data[(byte)53]);
		string code = (string)data[(byte)54];
		string message = GameLogicServerNetworkController.Instance.GameLogicErrorNotificationMessage(code);
		NotificationWindow.Add(new Notification(Notification.Type.GAMELOGIC_ERROR, LanguageManager.GetText("SERVER ERROR"), message)
		{
			WindowSize = new Vector2(500f, 243f),
			CanClose = true
		});
		ServersList.Disconnect();
		LoadingMapPopup.Complete();
	}

	public void ProcessJoinLobby()
	{
		if (PlayerManager.Instance != null)
		{
			this.SendRequest(GameLogicEventCode.OnReconnectInGame, null);
		}
	}

	public bool SendRequest(GameLogicEventCode evCode, Hashtable requestData)
	{
		return PhotonConnection.Connection.ServerListSendRequest(this.host + ":" + this.ports[0], new FUFPSServerListItem.PhotonEventListener(this.photonEventListener), (byte)evCode, requestData);
	}
}
