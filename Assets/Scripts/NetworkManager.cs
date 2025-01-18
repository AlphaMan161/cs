// ILSpyBased#2
using System;
using System.Collections;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private bool shotRegisteredBefore;

    private bool shotRegisteredAfter;

    private Notification disconnectNotification;

    private ErrorInfo.CODE disconnectNotificationCode;

    private int shotPPlayerCounter;

    private PhotonConnection photonConnection;

    private static NetworkManager instance;

    public static NetworkManager Instance
    {
        get
        {
            return NetworkManager.instance;
        }
    }

    public void RegisterShotBefore()
    {
        this.shotRegisteredBefore = true;
    }

    public void RegisterShotAfter()
    {
        this.shotRegisteredAfter = true;
    }

    public void SendAnimationState(byte state)
    {
        Hashtable hashtable = new Hashtable();
        hashtable[FUFPSParameterKeys.AnimationState] = state;
        long networkTime = TimeManager.Instance.NetworkTime;
        hashtable[FUFPSParameterKeys.TimeStamp] = networkTime;
        if (this.photonConnection != null)
        {
            this.photonConnection.SendRequest(FUFPSOpCode.AnimationEvent, hashtable);
        }
    }

    public void SendAnimationKey(byte key)
    {
        Hashtable hashtable = new Hashtable();
        hashtable[FUFPSParameterKeys.AnimationKey] = key;
        long networkTime = TimeManager.Instance.NetworkTime;
        hashtable[FUFPSParameterKeys.TimeStamp] = networkTime;
        if (this.photonConnection != null)
        {
            this.photonConnection.SendRequest(FUFPSOpCode.AnimationEvent, hashtable);
        }
    }

    public string getUserName()
    {
        return string.Empty;
    }

    public void SendGameStateRequest()
    {
        if (this.photonConnection != null)
        {
            this.photonConnection.SendRequest(FUFPSOpCode.GameStateRequest, null);
        }
    }

    public void SendKickVote(Hashtable data)
    {
        if (this.photonConnection != null)
        {
            this.photonConnection.SendRequest(FUFPSOpCode.Kick, data);
        }
    }

    public void SendSpawnRequest(short team)
    {
        UnityEngine.Debug.Log("[NetworkManager] Send Spawn Request");
        Hashtable hashtable = new Hashtable();
        hashtable[FUFPSGameKeys.Team] = team;
        if (this.photonConnection != null)
        {
            this.photonConnection.SendRequest(FUFPSOpCode.SpawnRequest, hashtable);
        }
    }

    public void SendTShot(NetworkTrajectory shotTrajectory)
    {
    }

    public void SendEnhancer(int enhancerSlotID)
    {
        Hashtable hashtable = new Hashtable();
        hashtable[(byte)48] = enhancerSlotID;
        if (this.photonConnection != null)
        {
            this.photonConnection.SendRequest(FUFPSOpCode.Enhancer, hashtable);
        }
    }

    public void SendShot(Shot shot)
    {
        Hashtable hashtable = shot.ToHashtable();
        if (this.shotPPlayerCounter == 10)
        {
            hashtable[(byte)22] = NetworkTransform.Vector3ToHashtable(PlayerManager.Instance.LocalPlayer.transform.position);
            this.shotPPlayerCounter = 0;
        }
        else
        {
            this.shotPPlayerCounter++;
        }
        if (shot.LaunchMode == LaunchModes.SHOT && (!this.shotRegisteredAfter || !this.shotRegisteredBefore))
        {
            if (Configuration.DebugVersion)
            {
                Configuration.DebugEnableFps = true;
                GameHUDFPS.Instance.SetDebugLine(string.Format("SHOT SEND CHEATING before: {0} after: {1}", this.shotRegisteredBefore, this.shotRegisteredAfter), 3);
                UnityEngine.Debug.LogError(string.Format("SHOT SEND CHEATING before: {0} after: {1}", this.shotRegisteredBefore, this.shotRegisteredAfter));
            }
            PlayerManager.Instance.SendEnterBaseRequest(34);
        }
        else
        {
            this.shotRegisteredBefore = false; this.shotRegisteredAfter = (this.shotRegisteredBefore );
            if (this.photonConnection != null)
            {
                this.photonConnection.SendRequest(FUFPSOpCode.Shot, hashtable);
            }
        }
    }

    public void SendShot(Shot shot, Vector3 source)
    {
        if (!Configuration.EnableWallShotCheckData)
        {
            this.SendShot(shot);
        }
        else
        {
            Hashtable hashtable = shot.ToHashtable();
            if (this.shotPPlayerCounter == 5)
            {
                hashtable[(byte)22] = NetworkTransform.Vector3ToHashtable(source);
                this.shotPPlayerCounter = 0;
            }
            else
            {
                this.shotPPlayerCounter++;
            }
            if (shot.LaunchMode == LaunchModes.SHOT && (!this.shotRegisteredAfter || !this.shotRegisteredBefore))
            {
                if (Configuration.DebugVersion)
                {
                    Configuration.DebugEnableFps = true;
                    GameHUDFPS.Instance.SetDebugLine(string.Format("SHOT SEND CHEATING before: {0} after: {1}", this.shotRegisteredBefore, this.shotRegisteredAfter), 3);
                    UnityEngine.Debug.LogError(string.Format("SHOT SEND CHEATING before: {0} after: {1}", this.shotRegisteredBefore, this.shotRegisteredAfter));
                }
                PlayerManager.Instance.SendEnterBaseRequest(34);
            }
            else
            {
                this.shotRegisteredBefore = false; this.shotRegisteredAfter = (this.shotRegisteredBefore );
                if (this.photonConnection != null)
                {
                    this.photonConnection.SendRequest(FUFPSOpCode.Shot, hashtable);
                }
            }
        }
    }

    public void SendTaunt(int tauntIndex, int tauntID)
    {
        Hashtable hashtable = new Hashtable();
        hashtable[(byte)45] = tauntIndex;
        hashtable[(byte)55] = tauntID;
        if (this.photonConnection != null)
        {
            this.photonConnection.SendRequest(FUFPSOpCode.AnimationEvent, hashtable);
        }
    }

    public void SendReload(WeaponType weaponType)
    {
        Hashtable hashtable = new Hashtable();
        hashtable[(byte)89] = weaponType;
        if (this.photonConnection != null)
        {
            this.photonConnection.SendRequest(FUFPSOpCode.ReloadWeapon, hashtable);
        }
    }

    public void SendChange(int weapon_num, byte weaponType)
    {
        Hashtable hashtable = new Hashtable();
        hashtable[(byte)78] = weapon_num;
        hashtable[(byte)89] = weaponType;
        if (this.photonConnection != null)
        {
            this.photonConnection.SendRequest(FUFPSOpCode.ChangeWeapon, hashtable);
        }
    }

    public void OnDisconnect(string msg)
    {
        UnityEngine.Debug.Log("[NetworkManager] OnDisconnect" + msg);
        Screen.lockCursor = false;
        Cursor.visible = true;
        GameHUD.Instance.PlayerState = GameHUD.PlayerStates.Load;
        if (this.disconnectNotification == null)
        {
            this.disconnectNotificationCode = ErrorInfo.CODE.SERVER_CONNECTION_LOST_ERROR_105;
            this.disconnectNotification = this.disconnectNotificationCode.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
            this.disconnectNotification.CallbackClick = new Notification.ButtonClick(this.ExitToMenu);
        }
        ServersList.Disconnect();
    }

    public void ProcessNotificationEvent(Hashtable notificationEventData)
    {
        NotificationType notificationType = (NotificationType)(byte)notificationEventData[(byte)65];
        if (this.disconnectNotification != null)
        {
            NotificationWindow.Complete(this.disconnectNotification);
        }
        switch (notificationType)
        {
            case NotificationType.PingOverhead:
                this.disconnectNotificationCode = ErrorInfo.CODE.SERVER_HIGH_PING;
                this.disconnectNotification = this.disconnectNotificationCode.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
                this.disconnectNotification.CallbackClick = new Notification.ButtonClick(this.ExitToMenu);
                break;
            case NotificationType.ActivityIdle:
                this.disconnectNotificationCode = ErrorInfo.CODE.SERVER_AFK;
                this.disconnectNotification = this.disconnectNotificationCode.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
                this.disconnectNotification.CallbackClick = new Notification.ButtonClick(this.ExitToMenu);
                break;
            case NotificationType.MoveCheating:
                this.disconnectNotificationCode = ErrorInfo.CODE.SERVER_MOVE_CHEATING;
                this.disconnectNotification = this.disconnectNotificationCode.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
                this.disconnectNotification.CallbackClick = new Notification.ButtonClick(this.ExitToMenu);
                break;
            case NotificationType.TimeCheating:
                this.disconnectNotificationCode = ErrorInfo.CODE.SERVER_TIME_CHEATING;
                this.disconnectNotification = this.disconnectNotificationCode.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
                this.disconnectNotification.CallbackClick = new Notification.ButtonClick(this.ExitToMenu);
                break;
            default:
                if (notificationEventData.ContainsKey((byte)45))
                {
                    switch ((byte)notificationEventData[(byte)45])
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        case 5:
                            break;
                        default:
                            GameHUD.Instance.CheatMessage();
                            break;
                    }
                }
                else
                {
                    GameHUD.Instance.CheatMessage();
                }
                break;
        }
    }

    private void ExitToMenu(object evt)
    {
        GameHUD.Instance.ExitToMenu();
    }

    public long getServerTimestamp()
    {
        if (this.photonConnection != null)
        {
            return this.photonConnection.getServerTimestamp();
        }
        return 0L;
    }

    public long getLocalTimestamp()
    {
        if (this.photonConnection != null)
        {
            return this.photonConnection.getLocalTimestamp();
        }
        return 0L;
    }

    public int getAveragePing()
    {
        if (this.photonConnection != null)
        {
            return this.photonConnection.getAveragePing();
        }
        return 0;
    }

    private void Awake()
    {
        NetworkManager.instance = this;
    }

    private void Start()
    {
        this.photonConnection = PhotonConnection.Connection;
        this.photonConnection.AddEventListener(new PhotonConnection.PhotonEventListener(this.onConnectionEvent));
        this.photonConnection.Request(FUFPSParameterKeys.JoinRoomSettings);
    }

    public void LeaveToLobby()
    {
        this.photonConnection.LeaveToLobby();
        this.photonConnection = null;
        OptionsManager.RoomSetting = null;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        GC.Collect();
        ServersList.SelectServer.NeedReconnect = true;
    }

    public void OnJoinRoom(Hashtable joinData)
    {
        if (joinData != null)
        {
            PlayerManager.Instance.Init(joinData);
            this.SendGameStateRequest();
        }
    }

    public void OnGetPlayerData(int actorID, Hashtable actorData)
    {
        if (actorData != null)
        {
            PlayerManager.Instance.InitEnemy(actorID, actorData, true);
        }
    }

    public void OnGetGameState(Hashtable gameStateData)
    {
        PlayerManager.Instance.SetGameState(gameStateData);
        if (!PlayerManager.Instance.RoomSettings.GamePause && PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.ZOMBIE)
        {
            this.InitSpawn();
        }
    }

    public void InitSpawn()
    {
        GameHUD.Instance.ShowCursor();
        if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.ZOMBIE || ((UnityEngine.Object)PlayerManager.Instance.LocalPlayer != (UnityEngine.Object)null && PlayerManager.Instance.LocalPlayer.IsGuest && LocalUser.Permission.Guest))
        {
            GameHUD.ShowSelectTeam = false;
            OptionsManager.Team = 0;
        }
        else if ((int)PlayerManager.Instance.RoomSettings.GameMode >= 2 && PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.TOWER_DEFENSE)
        {
            GameHUD.ShowSelectTeam = true;
        }
        else
        {
            GameHUD.ShowSelectTeam = false;
            OptionsManager.Team = 0;
            GameHUD.Instance.UpdateHealth();
            GameHUD.Instance.PlayerState = GameHUD.PlayerStates.Dead;
            PlayerManager.Instance.LocalPlayer.Team = 0;
        }
    }

    public void OnActorJoinRoom(int actorID, Hashtable actorData)
    {
        if (actorData != null)
        {
            PlayerManager.Instance.InitEnemy(actorID, actorData, true);
        }
    }

    public void OnActorLeavesRoom(int actorID)
    {
        PlayerManager.Instance.DestroyEnemy(actorID);
    }

    private void FixedUpdate()
    {
        if (this.photonConnection != null)
        {
            this.photonConnection.Update(true);
        }
        if (DebugConsole.IsShow)
        {
            RTTXCamera.Init();
        }
        Datameter.Report();
    }

    private void onConnectionEvent(PhotonEvent photonEvent)
    {
        if (Datameter.enabled && photonEvent.Code != 99)
        {
            Datameter.NetworkSizeCounter += (float)PlayerManager.GetObjectSize(photonEvent.Data);
        }
        switch (photonEvent.Code)
        {
            case 100:
                PlayerManager.Instance.SpawnPlayer(photonEvent.ActorID, photonEvent.Data);
                break;
            case 84:
                this.OnGetGameState(photonEvent.Data);
                break;
            case 107:
                this.OnJoinRoom(photonEvent.Data);
                break;
            case 67:
                this.OnGetPlayerData(photonEvent.ActorID, photonEvent.Data);
                break;
            case 105:
                this.OnActorJoinRoom(photonEvent.ActorID, photonEvent.Data);
                break;
            case 155:
                BattleChat.OnMessage(photonEvent.Data);
                break;
            case 99:
                if (!NetworkDev.CheckLag())
                {
                    PlayerManager.Instance.MovePlayer(photonEvent.ActorID, photonEvent.Data);
                }
                break;
            case 97:
                PlayerManager.Instance.PlayerShot(photonEvent.ActorID, photonEvent.Data);
                break;
            case 74:
                PlayerManager.Instance.PlayerImpact(photonEvent.ActorID, photonEvent.Data);
                break;
            case 85:
                PlayerManager.Instance.UpdatePlayerEnergy(photonEvent.ActorID, photonEvent.Data);
                break;
            case 95:
                PlayerManager.Instance.KillPlayer(photonEvent.ActorID, photonEvent.Data);
                break;
            case 90:
                PlayerManager.Instance.UpdateScore(photonEvent.Data, false);
                break;
            case 76:
                UnityEngine.Debug.LogError("[NetworkManager] FUFPSOpCode.Achievement");
                PlayerManager.Instance.CompleteAchievement(photonEvent.ActorID, photonEvent.Data);
                break;
            case 89:
                PlayerManager.Instance.UpdateFlag(photonEvent.Data);
                break;
            case 88:
                PlayerManager.Instance.UpdateControlPoint(photonEvent.Data);
                break;
            case 73:
                PlayerManager.Instance.UpdateZombie(photonEvent.ActorID, photonEvent.Data);
                break;
            case 94:
                PlayerManager.Instance.SpawnItem(photonEvent.Data);
                break;
            case 93:
                PlayerManager.Instance.PickItem(photonEvent.ActorID, photonEvent.Data);
                break;
            case 96:
                PlayerManager.Instance.ReloadWeapon(photonEvent.ActorID, photonEvent.Data);
                break;
            case 98:
                PlayerManager.Instance.ChangeWeapon(photonEvent.ActorID, photonEvent.Data);
                break;
            case 87:
                PlayerManager.Instance.UpdateEnhancer(photonEvent.ActorID, photonEvent.Data);
                break;
            case 92:
                PlayerManager.Instance.TimeOver(photonEvent.Data);
                break;
            case 91:
                PlayerManager.Instance.NewGame(photonEvent.Data);
                break;
            case 106:
                this.OnActorLeavesRoom(photonEvent.ActorID);
                break;
            case 104:
                this.OnDisconnect("Disconnected from Photon");
                break;
            case 156:
                BattleInfo.AddMessage(photonEvent.Data);
                break;
            case 81:
                PlayerManager.Instance.ProcessCampaignEvent(photonEvent.Data);
                break;
            case 80:
                this.ProcessNotificationEvent(photonEvent.Data);
                break;
            case 78:
                PlayerManager.Instance.ProcessEscortEvent(photonEvent.Data);
                break;
            case 77:
                PlayerManager.Instance.ProcessAnimationEvent(photonEvent.ActorID, photonEvent.Data);
                break;
            case 70:
                KickManager.Instance.ReadVote(photonEvent.Data);
                break;
            case 72:
                PlayerManager.Instance.SendEnterBaseRequest(23);
                break;
            case 82:
                if (photonEvent.ReturnCode == -17)
                {
                    string message = GameLogicServerNetworkController.Instance.GameLogicErrorNotificationMessage(photonEvent.Message);
                    Notification notification = new Notification(Notification.Type.GAMELOGIC_ERROR, LanguageManager.GetText("SERVER GAME ERROR"), message);
                    notification.WindowSize = new Vector2(500f, 243f);
                    NotificationWindow.Add(notification);
                    LoadingMapPopup.Complete();
                }
                break;
            default:
                UnityEngine.Debug.Log(string.Format("[NetworkManager] Unhandled FUFPS Event: {0} Code: {1}", photonEvent.Message, photonEvent.Code));
                break;
        }
    }
}


