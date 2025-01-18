// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    public enum PlayerStates
    {
        Load,
        Dead,
        DeadReady,
        TimeOver,
        TimeOverReady,
        Play,
        Zombie_Dead_Zombie,
        Zombie_Dead_Player,
        Zombie_Waiting_Players,
        Zombie_Boss_Infection
    }

    private const int Zombie_Infection_Time = 20;

    private BattleGUI battleGUI;

    private float screenShotDelay = 5f;

    private float lastScreenShot;

    private float respawnDelay = 11f;

    private readonly float timeOverDelay = 10.5f;

    private bool showSelectTeam;

    private float bloodOverlayAmount;

    public Renderer bloodOverlay;

    public Renderer zoomOverlay;

    public Animation damageIndicator;

    public GameObject enemyHitPointer;

    public GameObject flagPointer;

    public GameObject controlPointPointer;

    public Transform CrossHairs;

    private bool isCrossHairsVisible;

    private bool showScore = true;

    private int reward;

    private long timeOfDeath;

    private float offsetShowWeapon = 2f;

    private float hideWeaponTime;

    private bool developHideGUI;

    private GameObject developGUICamera;

    private GameObject developWeapon;

    private NetworkManager networkManager;

    private static GameHUD instance;

    private PlayerStates playerState;

    private static bool _isShowMenu;

    public static bool isVoteKick;

    private GUIStyle[] ammoStyle;

    private GUIStyle[] ammoStyleActive;

    private GUIStyle ammoStyleEmpty;

    private GUIStyle[] enhancerStyle = new GUIStyle[2];

    private GUIStyle enhancerStyleEmpty;

    private bool weaponStylesInit;

    private string screenMessage = string.Empty;

    private string cheatMessage = string.Empty;

    private bool zoom;

    private long zombieInfectionStart;

    private Rect scoreRect = default(Rect);

    private int local_exp_before_save;

    private static GUIDropDownList.GUIDropDownSetting dropDownSetting;

    private static KickReason selectedKickReason = KickReason.Threats;

    private static int selectedUser2Kick;

    public BattleGUI BattleGUI
    {
        get
        {
            return this.battleGUI;
        }
    }

    public static bool ShowSelectTeam
    {
        get
        {
            return GameHUD.Instance.showSelectTeam;
        }
        set
        {
            GameHUD.Instance.showSelectTeam = value;
            if (GameHUD.Instance.showSelectTeam)
            {
                GameHUD.Instance.ShowCursor();
            }
        }
    }

    private NetworkManager NetworkManager
    {
        get
        {
            if ((UnityEngine.Object)this.networkManager == (UnityEngine.Object)null)
            {
                this.networkManager = ((Component)base.transform).GetComponent<NetworkManager>();
            }
            return this.networkManager;
        }
    }

    public static GameHUD Instance
    {
        get
        {
            return GameHUD.instance;
        }
    }

    public PlayerStates PlayerState
    {
        get
        {
            return this.playerState;
        }
        set
        {
            this.playerState = value;
        }
    }

    public static bool IsShowMenu
    {
        get
        {
            return GameHUD._isShowMenu;
        }
        set
        {
            GameHUD._isShowMenu = value;
            if (!GameHUD._isShowMenu)
            {
                GameHUD.isVoteKick = false;
            }
        }
    }

    private void DevelopHideShowGUI()
    {
        this.developHideGUI = !this.developHideGUI;
        if (this.developHideGUI)
        {
            this.battleGUI.gameObject.SetActive(false);
            this.developGUICamera = GameObject.Find("GUICamera");
            this.developGUICamera.SetActive(false);
            this.developWeapon = GameObject.Find("Game/Fighter/MainCamera/Weapon");
            this.developWeapon.SetActive(false);
            base.gameObject.GetComponent<GameHUDFPS>().enabled = false;
        }
        else
        {
            this.battleGUI.gameObject.SetActive(true);
            this.developGUICamera.SetActive(true);
            this.developWeapon.SetActive(true);
            base.gameObject.GetComponent<GameHUDFPS>().enabled = true;
        }
    }

    public void setReward(int reward)
    {
        this.reward = reward;
    }

    private void InitWeaponStyles()
    {
        if (!this.weaponStylesInit && PlayerManager.Instance.IsInit)
        {
            CombatWeapon[] weapons = ShotController.Instance.Weapons;
        }
    }

    public void CheatMessage(NotificationType notificationType)
    {
        if (this.cheatMessage == string.Empty)
        {
            base.StartCoroutine(this.SetCheatMessage());
        }
        this.cheatMessage = string.Format("CHEAT: {0}", notificationType.ToString());
    }

    public void CheatMessage(string message)
    {
        if (this.cheatMessage == string.Empty)
        {
            base.StartCoroutine(this.SetCheatMessage());
        }
        this.cheatMessage = message;
    }

    public void CheatMessage()
    {
        if (this.cheatMessage == string.Empty)
        {
            base.StartCoroutine(this.SetCheatMessage());
        }
        this.cheatMessage = LanguageManager.GetText("cheat_msg");
    }

    private IEnumerator SetCheatMessage()
    {
        yield return (object)new WaitForSeconds(30f);
        this.cheatMessage = string.Empty;
    }

    public void Message(GameHUDMessageType messageType, params object[] args)
    {
        if (this.screenMessage == string.Empty)
        {
            base.StartCoroutine(this.SetMessage());
        }
        switch (messageType)
        {
            case (GameHUDMessageType)13:
            case (GameHUDMessageType)14:
            case (GameHUDMessageType)15:
            case (GameHUDMessageType)16:
            case (GameHUDMessageType)17:
            case (GameHUDMessageType)18:
            case (GameHUDMessageType)19:
            case (GameHUDMessageType)20:
                break;
            case GameHUDMessageType.TOWER_CORE_ATTACK:
                break;
            case GameHUDMessageType.EMPTY_FLAG_POINT:
                this.screenMessage = LanguageManager.GetText("You need to return your flag first!");
                break;
            case GameHUDMessageType.FLAG_YOU_CAPTURED:
                this.screenMessage = LanguageManager.GetText("Captured enemy flag - dring it fast to your base!");
                break;
            case GameHUDMessageType.FLAG_CAPTURED:
                this.screenMessage = LanguageManager.GetText("Enemy flag is captured! Help your team to bring it!");
                break;
            case GameHUDMessageType.FLAG_ENEMY_CAPTURED:
                this.screenMessage = LanguageManager.GetText("Our flag is captured! Return it from your enemy!");
                break;
            case GameHUDMessageType.FLAG_YOU_DELIVERED:
                this.screenMessage = LanguageManager.GetText("You returned your flag! Congratulations!");
                break;
            case GameHUDMessageType.FLAG_DELIVERED:
                this.screenMessage = LanguageManager.GetText("Teammates took the enemy flag! Congratulations!");
                break;
            case GameHUDMessageType.FLAG_ENEMY_DELIVERED:
                this.screenMessage = LanguageManager.GetText("Enemy took our flag to their base. Be aware!");
                break;
            case GameHUDMessageType.FLAG_LOST:
                this.screenMessage = LanguageManager.GetText("Enemy flag is lost! Catch it up first!");
                break;
            case GameHUDMessageType.FLAG_ENEMY_LOST:
                this.screenMessage = LanguageManager.GetText("We struggled our flag from the enemy! Bring it back to your base!");
                break;
            case GameHUDMessageType.FLAG_YOU_RETURNED:
                this.screenMessage = LanguageManager.GetText("You defended your flag! Forth to victory!");
                break;
            case GameHUDMessageType.FLAG_RETURNED:
                this.screenMessage = LanguageManager.GetText("Glory all your teammates defended the flag!");
                break;
            case GameHUDMessageType.FLAG_ENEMY_RETURNED:
                this.screenMessage = LanguageManager.GetText("Enemy took their flag back but we;ll still win!");
                break;
            case GameHUDMessageType.TOWER_NEW_WAVE:
                this.screenMessage = LanguageManager.GetText("Warning! New wave is coming!");
                break;
            case GameHUDMessageType.TOWER_FINISH_WAVE:
                this.screenMessage = LanguageManager.GetText("Wave is over!");
                break;
            case GameHUDMessageType.ZOMBIE_WAITING_PLAYERS:
                this.screenMessage = LanguageManager.GetTextFormat("Waiting for players: {0} / {1}", PlayerManager.Instance.Players.Count, 4);
                break;
            case GameHUDMessageType.ZOMBIE_BOSS_INFECTION:
                this.screenMessage = LanguageManager.GetText("Zombie boss infection in 5 seconds");
                break;
            case GameHUDMessageType.ZOMBIE_DEAD_PLAYER:
                this.screenMessage = LanguageManager.GetText("You are self killed as Player! Wait for the next match.");
                break;
            case GameHUDMessageType.ZOMBIE_DEAD_ZOMBIE:
                this.screenMessage = LanguageManager.GetText("You are dead as Zombie! Wait for the next match.");
                break;
        }
    }

    private IEnumerator SetMessage()
    {
        yield return (object)new WaitForSeconds(5f);
        this.screenMessage = string.Empty;
    }

    public void Zoom(bool on)
    {
        this.zoom = on;
        this.battleGUI.SetZoom(on);
    }

    public void SetCrossHairVisible(bool on)
    {
        if (this.isCrossHairsVisible != on)
        {
            this.isCrossHairsVisible = on;
            this.CrossHairs.gameObject.SetActive(on);
        }
    }

    private void Awake()
    {
        GameHUD.instance = this;
        Application.runInBackground = true;
    }

    public bool isActive()
    {
        if (this.PlayerState != PlayerStates.Play && this.PlayerState != PlayerStates.Zombie_Boss_Infection)
        {
            return false;
        }
        if (GameHUD.IsShowMenu)
        {
            return false;
        }
        return true;
    }

    private void Start()
    {
        WebCall.Init();
        this.weaponStylesInit = false;
        base.gameObject.AddComponent<GUIHover>();
        GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("GUI/Battle/Prefab/BattleGUI")) as GameObject;
        this.battleGUI = (BattleGUI)gameObject.GetComponent(typeof(BattleGUI));
        if ((UnityEngine.Object)this.battleGUI == (UnityEngine.Object)null)
        {
            UnityEngine.Debug.LogError("[GameHUD] BattleGUI not init");
        }
		bloodOverlay.material.shader = Shader.Find ("Particles/Additive");
		zoomOverlay.material.shader = Shader.Find ("Particles/Additive");
        this.battleGUI.SetTeam(0, PlayerManager.Instance.RoomSettings.GameMode);
        this.battleGUI.SetBloodOverlay(0f);
    }

    public static void HideCursor()
    {
        Cursor.visible = false;
        Screen.lockCursor = true;
        GUIHover.Enable = false;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Screen.lockCursor = false;
        GUIHover.Enable = true;
    }

    public void SetRespawnDelay(bool isPremium)
    {
        if (isPremium)
        {
            this.respawnDelay = 6f;
        }
    }

    public void SetZombieMode(byte zombieModeCode)
    {
        switch (zombieModeCode)
        {
            case 4:
                if (!PlayerManager.Instance.LocalPlayer.IsDead)
                {
                    this.PlayerState = PlayerStates.Play;
                }
                else if (PlayerManager.Instance.LocalPlayer.Team == 1)
                {
                    this.PlayerState = PlayerStates.Zombie_Dead_Zombie;
                }
                else
                {
                    this.PlayerState = PlayerStates.Zombie_Dead_Player;
                }
                break;
            case 1:
                this.PlayerState = PlayerStates.TimeOver;
                break;
            case 3:
                if (PlayerManager.Instance.LocalPlayer.IsDead)
                {
                    this.PlayerState = PlayerStates.Zombie_Dead_Player;
                }
                this.zombieInfectionStart = TimeManager.Instance.NetworkTime;
                this.PlayerState = PlayerStates.Zombie_Boss_Infection;
                break;
            case 2:
                this.PlayerState = PlayerStates.Zombie_Waiting_Players;
                break;
        }
    }

    private void DrawUpTimer()
    {
        long num = PlayerManager.Instance.RoomSettings.StartTime + PlayerManager.Instance.RoomSettings.TimeLimit * 60 * 1000 - TimeManager.Instance.NetworkTime;
        if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.CONTROL_POINTS || PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.CAPTURE_THE_FLAG)
        {
            num = TimeManager.Instance.NetworkTime - PlayerManager.Instance.RoomSettings.StartTime;
        }
        if (this.PlayerState == PlayerStates.Load)
        {
            num = 0L;
        }
        else if (this.PlayerState == PlayerStates.TimeOver)
        {
            num = (long)(this.timeOverDelay + 1f) * 1000 + PlayerManager.Instance.RoomSettings.StartTime - TimeManager.Instance.NetworkTime;
        }
        if (num < 0)
        {
            num = 0L;
        }
        long num2 = (long)Math.Floor((double)((float)num / 60000f));
        long num3 = (long)Math.Floor((double)((float)num / 1000f)) - num2 * 60;
        GUITextShadow.TextShadow(new Rect((float)(Screen.width / 2 - 20), 7f, 40f, 20f), string.Format("{0}:{1:00}", num2, num3), GUISkinManager.BattleText.GetStyle("upTime"), GUISkinManager.BattleText.GetStyle("upTimeShadow"));
    }

    public void ShowWeapon()
    {
        this.hideWeaponTime = Time.time + this.offsetShowWeapon;
    }

    public void ShowImpact(BattleGUIImpact.GUIImpact battleImpact, float time)
    {
        this.battleGUI.ShowImpact(battleImpact, 2f);
    }

    private void DrawUserStats()
    {
        if ((UnityEngine.Object)PlayerManager.Instance.LocalPlayer != (UnityEngine.Object)null)
        {
            this.battleGUI.SetAmmo(ShotController.Instance.Ammo, ShotController.Instance.MaxAmmo, ShotController.Instance.AmmoReserve);
            if (this.hideWeaponTime > Time.time)
            {
                Rect screenRect = new Rect((float)(Screen.width / 2) - 325f, (float)Screen.height - 155f, 650f, 56f);
                if ((float)Screen.width >= 1015f)
                {
                    screenRect.x = (float)(Screen.width / 2) - 250f;
                    screenRect.y = (float)Screen.height - 60f;
                    screenRect.width = 650f;
                    screenRect.height = 56f;
                }
                int index = ShotController.Instance.CurrentWeapon.GetIndex();
                if ((UnityEngine.Object)ShotController.Instance != (UnityEngine.Object)null && ShotController.Instance.Weapons != null)
                {
                    GUILayout.BeginArea(screenRect, GUIContent.none, GUIStyle.none);
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                    GUILayout.Label(GUIContent.none, GUISkinManager.BattleIcoWeapon.GetStyle((index != 1) ? "ws1" : "ws1active"));
                    GUILayout.Label(TRInput.Weapon1.ToDisplayString(), GUISkinManager.BattleIcoWeapon.GetStyle((index != 1) ? "wsTxt" : "wsTxtActive"));
                    GUILayout.EndVertical();
                    GUILayout.Space(5f);
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                    GUILayout.Label(GUIContent.none, GUISkinManager.BattleIcoWeapon.GetStyle((index != 2) ? "ws2" : "ws2active"));
                    GUILayout.Label(TRInput.Weapon2.ToDisplayString(), GUISkinManager.BattleIcoWeapon.GetStyle((index != 2) ? "wsTxt" : "wsTxtActive"));
                    GUILayout.EndVertical();
                    GUILayout.Space(5f);
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                    GUILayout.Label(GUIContent.none, GUISkinManager.BattleIcoWeapon.GetStyle((index != 3) ? "ws3" : "ws3active"));
                    GUILayout.Label(TRInput.Weapon3.ToDisplayString(), GUISkinManager.BattleIcoWeapon.GetStyle((index != 3) ? "wsTxt" : "wsTxtActive"));
                    GUILayout.EndVertical();
                    GUILayout.Space(5f);
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                    GUILayout.Label(GUIContent.none, GUISkinManager.BattleIcoWeapon.GetStyle((index != 4) ? "ws4" : "ws4active"));
                    GUILayout.Label(TRInput.Weapon4.ToDisplayString(), GUISkinManager.BattleIcoWeapon.GetStyle((index != 4) ? "wsTxt" : "wsTxtActive"));
                    GUILayout.EndVertical();
                    GUILayout.Space(5f);
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                    GUILayout.Label(GUIContent.none, GUISkinManager.BattleIcoWeapon.GetStyle((index != 5) ? "ws5" : "ws5active"));
                    GUILayout.Label(TRInput.Weapon5.ToDisplayString(), GUISkinManager.BattleIcoWeapon.GetStyle((index != 5) ? "wsTxt" : "wsTxtActive"));
                    GUILayout.EndVertical();
                    GUILayout.Space(5f);
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                    GUILayout.Label(GUIContent.none, GUISkinManager.BattleIcoWeapon.GetStyle((index != 6) ? "ws6" : "ws6active"));
                    GUILayout.Label(TRInput.Weapon6.ToDisplayString(), GUISkinManager.BattleIcoWeapon.GetStyle((index != 6) ? "wsTxt" : "wsTxtActive"));
                    GUILayout.EndVertical();
                    GUILayout.Space(5f);
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                    GUILayout.Label(GUIContent.none, GUISkinManager.BattleIcoWeapon.GetStyle((index != 7) ? "ws7" : "ws7active"));
                    GUILayout.Label(TRInput.Weapon7.ToDisplayString(), GUISkinManager.BattleIcoWeapon.GetStyle((index != 7) ? "wsTxt" : "wsTxtActive"));
                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndArea();
                }
            }
        }
    }

    public void DrawScore()
    {
        this.DrawScore((short)PlayerManager.Instance.RoomSettings.GameMode, false);
    }

    public void DrawFightEnd()
    {
        this.HideBattleGUI();
        this.DrawScore((short)PlayerManager.Instance.RoomSettings.GameMode, true);
    }

    private void DrawScore(short game_mode, bool isEnd)
    {
        int num = -1;
        if (game_mode > 1)
        {
            if (PlayerManager.GameScore.Teams[0].Point > PlayerManager.GameScore.Teams[1].Point)
            {
                num = 0;
            }
            else if (PlayerManager.GameScore.Teams[1].Point > PlayerManager.GameScore.Teams[0].Point)
            {
                num = 1;
            }
        }
        Vector2 vector = new Vector2(710f, 610f);
        if (isEnd)
        {
            vector.y = 640f;
            if (this.local_exp_before_save == 0)
            {
                this.local_exp_before_save = LocalUser.Exp;
            }
        }
        else
        {
            this.local_exp_before_save = 0;
        }
        this.scoreRect = new Rect((float)Screen.width * 0.5f - vector.x * 0.5f, (float)Screen.height * 0.5f - vector.y * 0.5f, vector.x, vector.y);
        GUILayout.BeginArea(this.scoreRect, GUIContent.none, GUISkinManager.BattleBackgound.GetStyle((!isEnd) ? "tab" : "tabEnd"));
        if (isEnd)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(50f));
            GUILayout.Label(LanguageManager.GetText("Combat Results"), GUISkinManager.BattleBackgound.GetStyle("tabPart"));
            GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), LanguageManager.GetText("Combat Results"), GUISkinManager.BattleText.GetStyle("tabPart"), GUISkinManager.BattleText.GetStyle("tabPartShadow"));
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Label(LanguageManager.GetText("Next round starts in:"), GUISkinManager.BattleText.GetStyle("txt1Value"));
            GUILayout.Space(4f);
            GUILayout.Label(((int)((this.timeOverDelay + 1f) * 1000f + (float)PlayerManager.Instance.RoomSettings.StartTime - (float)TimeManager.Instance.NetworkTime) / 1000).ToString(), GUISkinManager.BattleText.GetStyle("txt1ValueActive"));
            GUILayout.Space(4f);
            GUILayout.Label(LanguageManager.GetText("sec"), GUISkinManager.BattleText.GetStyle("txt1Value"));
            GUILayout.Space(15f);
            GUILayout.EndHorizontal();
            GUILayout.Space(12f);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("tabTitleOffset"), GUILayout.Height(54f));
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Label(LanguageManager.GetText("Room name:"), GUISkinManager.BattleText.GetStyle("txt1"));
            GUILayout.Space(5f);
            GUILayout.Label(PlayerManager.Instance.RoomSettings.FilteredName, GUISkinManager.BattleText.GetStyle("txt1Value"));
            GUILayout.EndHorizontal();
            GUILayout.Space(5f);
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Label(LanguageManager.GetText("Server:"), GUISkinManager.BattleText.GetStyle("txt1"));
            GUILayout.Space(5f);
            GUILayout.Label(ServersList.SelectServer.Name, GUISkinManager.BattleText.GetStyle("txt1Value"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Label(LanguageManager.GetText("Map:"), GUISkinManager.BattleText.GetStyle("txt1"));
            GUILayout.Space(5f);
            GUILayout.Label(PlayerManager.Instance.RoomSettings.Map.Name, GUISkinManager.BattleText.GetStyle("txt1Value"));
            GUILayout.EndHorizontal();
            GUILayout.Space(5f);
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Label(LanguageManager.GetText("Mode:"), GUISkinManager.BattleText.GetStyle("txt1"));
            GUILayout.Space(5f);
            GUILayout.Label(PlayerManager.Instance.RoomSettings.GameMode.GetFullName(), GUISkinManager.BattleText.GetStyle("txt1Value"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(10f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(286f));
        GUILayout.Space(10f);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("tabItem"), GUILayout.Width(273f), GUILayout.Height(73f));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("Rank"), GUISkinManager.BattleText.GetStyle("txt1"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        if (PlayerManager.GameScore.ContainsUser(LocalUser.UserID))
        {
            GUILayout.Label(string.Format("{0}/{1}", PlayerManager.GameScore[LocalUser.UserID].Position, (game_mode <= 1) ? PlayerManager.Instance.RoomSettings.MaxPlayers : PlayerManager.Instance.RoomSettings.MaxPlayersTeam), GUISkinManager.BattleText.GetStyle("txt24"));
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.Space(6f);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("tabItem"), GUILayout.Width(273f), GUILayout.MinHeight(83f));
        GUILayout.Space(45f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Level:"), GUISkinManager.BattleText.GetStyle("txt1"));
        GUILayout.Space(5f);
        GUILayout.Label(LocalUser.Level.ToString(), GUISkinManager.BattleText.GetStyle("txt1Value"));
        GUILayout.EndHorizontal();
        GUILayout.Space(1f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Exp:"), GUISkinManager.BattleText.GetStyle("txt1"));
        GUILayout.Space(5f);
        if (PlayerManager.GameScore.ContainsUser(LocalUser.UserID))
        {
            GUILayout.Label(string.Format("{0}/{1}", ((this.local_exp_before_save != 0) ? this.local_exp_before_save : LocalUser.Exp) + PlayerManager.GameScore[LocalUser.UserID].Exp, LocalUser.MaxExp), GUISkinManager.BattleText.GetStyle("txt1Value"));
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(1f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Experience earned:"), GUISkinManager.BattleText.GetStyle("txt1"));
        GUILayout.Space(5f);
        if (PlayerManager.GameScore.ContainsUser(LocalUser.UserID))
        {
            GUILayout.Label(PlayerManager.GameScore[LocalUser.UserID].Exp.ToString(), GUISkinManager.BattleText.GetStyle("txt1ValueActive"));
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(1f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("EXP to the next level:"), GUISkinManager.BattleText.GetStyle("txt1"));
        GUILayout.Space(5f);
        if (PlayerManager.GameScore.ContainsUser(LocalUser.UserID))
        {
            int num2 = LocalUser.MaxExp - (((this.local_exp_before_save != 0) ? this.local_exp_before_save : LocalUser.Exp) + PlayerManager.GameScore[LocalUser.UserID].Exp);
            if (num2 < 0)
            {
                num2 = 0;
            }
            GUILayout.Label(num2.ToString(), GUISkinManager.BattleText.GetStyle("txt1Value"));
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Space(6f);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("tabItem"), GUILayout.Width(273f), GUILayout.MinHeight(165f));
        GUILayout.Space(45f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Space(11f);
        GUILayout.Label(LanguageManager.GetText("Kills using:"), GUISkinManager.BattleText.GetStyle("txt1"));
        GUILayout.Space(1f);
        if (PlayerManager.GameScore.ContainsUser(LocalUser.UserID))
        {
            Dictionary<WeaponType, int>.Enumerator enumerator = PlayerManager.GameScore[PlayerManager.Instance.LocalPlayer.AuthID].StatsOnWeaponType.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<WeaponType, int> current = enumerator.Current;
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.Label(LanguageManager.GetText("kwt" + current.Key), GUISkinManager.BattleText.GetStyle("txt1Value"), GUILayout.Width(180f));
                    GUILayout.Label(current.Value.ToString(), GUISkinManager.BattleText.GetStyle("txt1ValueActive"));
                    GUILayout.EndHorizontal();
                    GUILayout.Space(1f);
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Space(6f);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("tabItem"), GUILayout.Width(273f), GUILayout.MinHeight(157f));
        GUILayout.Space(15f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("Earned achievements:"), GUISkinManager.BattleText.GetStyle("txt1"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(9f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        if (PlayerManager.GameScore.ContainsUser(LocalUser.UserID) && PlayerManager.GameScore[PlayerManager.Instance.LocalPlayer.AuthID].Achievements.Count > 0)
        {
            GUILayout.FlexibleSpace();
            for (int i = 0; i < PlayerManager.GameScore[PlayerManager.Instance.LocalPlayer.AuthID].Achievements.Count && i < 2; i++)
            {
                GUILayout.Space(5f);
                GUILayout.Label(PlayerManager.GameScore[PlayerManager.Instance.LocalPlayer.AuthID].Achievements[i].Ico, GUILayout.Width(97f), GUILayout.Height(86f));
                GUIHover.Hover(Event.current, PlayerManager.GameScore[PlayerManager.Instance.LocalPlayer.AuthID].Achievements[i], GUILayoutUtility.GetLastRect());
                GUILayout.Space(5f);
            }
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndVertical();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Hor"), GUILayout.Width(1f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("tabTitle"), GUILayout.Height(36f));
        if (game_mode > 1)
        {
            if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.ZOMBIE)
            {
                GUILayout.Label(LanguageManager.GetText("Infected"), GUISkinManager.BattleText.GetStyle("txtTitle19boldRed"), GUILayout.Width(134f));
            }
            else
            {
                GUILayout.Label(LanguageManager.GetText("Red"), GUISkinManager.BattleText.GetStyle("txtTitle19boldRed"), GUILayout.Width(124f));
            }
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
            GUILayout.Space(10f);
            GUILayout.Label(string.Format("{0}/{1}", PlayerManager.GameScore.Teams[0].Count, PlayerManager.Instance.RoomSettings.MaxPlayersTeam), GUISkinManager.BattleText.GetStyle("txt1Value"));
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.Label(LanguageManager.GetText("Players"), GUISkinManager.BattleText.GetStyle("txtTitle19bold"), GUILayout.Width(124f));
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
            GUILayout.Space(10f);
            GUILayout.Label(string.Format("{0}/{1}", PlayerManager.GameScore.Teams[0].Count, PlayerManager.Instance.RoomSettings.MaxPlayers), GUISkinManager.BattleText.GetStyle("txt1Value"));
            GUILayout.EndVertical();
        }
        if (game_mode > 1)
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(PlayerManager.GameScore.Teams[0].Point.ToString(), GUISkinManager.BattleText.GetStyle("txtTitle19boldRed"));
            GUILayout.Space(10f);
        }
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Hor"), GUILayout.Width(1f));
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.ZOMBIE && PlayerManager.GameScore.Teams[1].Count > 8)
        {
            this.DrawUsers(0, isEnd, 0f);
        }
        else
        {
            this.DrawUsers(0, isEnd, 212f);
        }
        if (game_mode > 1)
        {
            GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("tabTitle"), GUILayout.Height(36f));
            if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.ZOMBIE)
            {
                GUILayout.Label(LanguageManager.GetText("Survivors"), GUISkinManager.BattleText.GetStyle("txtTitle19boldBlue"), GUILayout.Width(134f));
            }
            else
            {
                GUILayout.Label(LanguageManager.GetText("Blue"), GUISkinManager.BattleText.GetStyle("txtTitle19boldBlue"), GUILayout.Width(124f));
            }
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
            GUILayout.Space(10f);
            GUILayout.Label(string.Format("{0}/{1}", PlayerManager.GameScore.Teams[1].Count, PlayerManager.Instance.RoomSettings.MaxPlayersTeam), GUISkinManager.BattleText.GetStyle("txt1Value"));
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label(PlayerManager.GameScore.Teams[1].Point.ToString(), GUISkinManager.BattleText.GetStyle("txtTitle19boldBlue"));
            GUILayout.Space(10f);
            GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Hor"), GUILayout.Width(1f));
            GUILayout.EndHorizontal();
            GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
            this.DrawUsers(1, isEnd, 0f);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        if (isEnd)
        {
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Height(73f));
            if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.ZOMBIE)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.FlexibleSpace();
                if (num == -1)
                {
                    GUILayout.Label(LanguageManager.GetText("Draw"), GUISkinManager.BattleText.GetStyle("tabPartSmall"));
                }
                else
                {
                    GUILayout.Label(LanguageManager.GetText("Won"), GUISkinManager.BattleText.GetStyle("tabPartSmall"));
                    GUILayout.Space(3f);
                    if (num == 0)
                    {
                        GUILayout.Label(LanguageManager.GetText("Infected!"), GUISkinManager.BattleText.GetStyle("tabPartSmallRed"));
                    }
                    else
                    {
                        GUILayout.Label(LanguageManager.GetText("Survived!"), GUISkinManager.BattleText.GetStyle("tabPartSmallBlue"));
                    }
                    GUILayout.Space(8f);
                    GUILayout.Label(LanguageManager.GetText("Overall round score"), GUISkinManager.BattleText.GetStyle("tabPartSmall"));
                    GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("upScoreZombie"), GUILayout.Width(37f), GUILayout.Height(33f));
                    GUILayout.Label(PlayerManager.GameScore.Teams[0].Wins.ToString(), GUISkinManager.BattleText.GetStyle("tabPartSmallRed"));
                    GUILayout.Label(":", GUISkinManager.BattleText.GetStyle("tabPartSmall"));
                    GUILayout.Label(PlayerManager.GameScore.Teams[1].Wins.ToString(), GUISkinManager.BattleText.GetStyle("tabPartSmallBlue"));
                    GUILayout.Space(3f);
                    GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("upScoreHuman"), GUILayout.Width(37f), GUILayout.Height(33f));
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.FlexibleSpace();
                int num3 = (int)((this.timeOverDelay + 1f) * 1000f + (float)PlayerManager.Instance.RoomSettings.StartTime - (float)TimeManager.Instance.NetworkTime) / 1000;
                GUILayout.Label(LanguageManager.GetTextFormat("Next round starts in: {0} sec", num3), GUISkinManager.BattleText.GetStyle("tabPartSmall"));
                if (Configuration.SType != ServerType.MM && Configuration.SType != ServerType.FACEBOOK && Configuration.SType != ServerType.KONGREGATE)
                {
                    GUILayout.Space(15f);
                    if (GUILayout.Button(LanguageManager.GetText("Share"), GUISkinManager.Button.GetStyle("btnShareResult"), GUILayout.Width(217f), GUILayout.Height(27f)))
                    {
                        if (Configuration.SType == ServerType.VK)
                        {
                            WallManager.CreateAndUpload(new Rect(this.scoreRect.x, (float)Screen.height - (this.scoreRect.height + this.scoreRect.y), this.scoreRect.width, this.scoreRect.height));
                        }
                        else
                        {
                            ScreenshotManager.CreateAndUpload();
                        }
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.Space(10f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.FlexibleSpace();
                int num4 = (int)((this.timeOverDelay + 1f) * 1000f + (float)PlayerManager.Instance.RoomSettings.StartTime - (float)TimeManager.Instance.NetworkTime) / 1000;
                GUILayout.Label(LanguageManager.GetTextFormat("Next round starts in: {0} sec", num4), GUISkinManager.BattleText.GetStyle("tabPartSmall"));
                if (Configuration.SType != ServerType.MM)
                {
                    GUILayout.Space(15f);
                    if (GUILayout.Button(LanguageManager.GetText("Share"), GUISkinManager.Button.GetStyle("btnShareResult"), GUILayout.Width(217f), GUILayout.Height(27f)))
                    {
                        if (Configuration.SType == ServerType.VK)
                        {
                            WallManager.CreateAndUpload(new Rect(this.scoreRect.x, (float)Screen.height - (this.scoreRect.height + this.scoreRect.y), this.scoreRect.width, this.scoreRect.height));
                        }
                        else
                        {
                            ScreenshotManager.CreateAndUpload();
                        }
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(42f));
            GUILayout.FlexibleSpace();
            if (LocalUser.Clan != null)
            {
                if (PlayerManager.GameScore.ContainsUser(LocalUser.UserID))
                {
                    GUILayout.Label(LanguageManager.GetText("Your experience:"), GUISkinManager.BattleText.GetStyle("txt1"), GUILayout.Height(42f));
                    GUILayout.Space(6f);
                    GUILayout.Label(PlayerManager.GameScore[LocalUser.UserID].Exp.ToString(), GUISkinManager.BattleText.GetStyle("txt1ValueActive"), GUILayout.Height(42f));
                    GUILayout.Space(10f);
                    GUILayout.Label(LanguageManager.GetText("Clan's experience:"), GUISkinManager.BattleText.GetStyle("txt1"), GUILayout.Height(42f));
                    GUILayout.Space(6f);
                    GUILayout.Label(PlayerManager.GameScore[LocalUser.UserID].Exp2clan.ToString(), GUISkinManager.BattleText.GetStyle("txt1ValueActive"), GUILayout.Height(42f));
                }
            }
            else
            {
                GUILayout.Label(LanguageManager.GetText("Experience earned:"), GUISkinManager.BattleText.GetStyle("txt1"), GUILayout.Height(42f));
                GUILayout.Space(6f);
                if (PlayerManager.GameScore.ContainsUser(LocalUser.UserID))
                {
                    GUILayout.Label(PlayerManager.GameScore[LocalUser.UserID].Exp.ToString(), GUISkinManager.BattleText.GetStyle("txt1ValueActive"), GUILayout.Height(42f));
                }
            }
            GUILayout.Space(24f);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }

    private void DrawUsers(short commandNum, bool isEnd, float minHeight)
    {
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("tabUListOffset"), GUILayout.MinHeight(minHeight));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUIStyle style = (!isEnd) ? GUISkinManager.BattleText.GetStyle("txtTabTitle") : GUISkinManager.BattleText.GetStyle("txtTabTitleEnd");
        GUILayout.Label(GUIContent.none, GUIStyle.none, GUILayout.Width(27f));
        GUILayout.Label(LanguageManager.GetText("Rank"), style, GUILayout.Width(38f));
        GUILayout.Label(LanguageManager.GetText("Name"), style, GUILayout.Width(115f));
        GUILayout.Label(GUIContent.none, GUIStyle.none, GUILayout.Width(50f));
        GUILayout.Label(LanguageManager.GetText("Score"), style, GUILayout.Width(40f));
        GUILayout.Label(LanguageManager.GetText("Kills"), style, GUILayout.Width(50f));
        GUILayout.Label(LanguageManager.GetText("Deaths"), style, GUILayout.Width(46f));
        GUILayout.Label(LanguageManager.GetText("Ping"), style, GUILayout.Width(38f));
        GUILayout.EndHorizontal();
        int index = (commandNum >= 1) ? 1 : 0;
        short num = 1;
        Dictionary<int, ScorePlayer>.Enumerator enumerator = PlayerManager.GameScore.Teams[index].SortedUserList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, ScorePlayer> current = enumerator.Current;
                this.DrawUserLine(PlayerManager.Instance.LocalPlayer.AuthID == current.Key, current.Value.IsDead, current.Value.Victim, current.Value.Nemesis, current.Value.Domination, isEnd, num, current.Value.UserName, current.Value.Point, current.Value.Kill, current.Value.Death, current.Value.Ping, (byte)((MasterServerNetworkController.Instance != null && MasterServerNetworkController.IsFriend(current.Value.UserID)) ? 1 : 0) != 0, (current.Value.ClanArmId == 0) ? null : ClanArmManager.GetTexture(current.Value.ClanArmId), current.Key);
                num = (short)(num + 1);
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        GUILayout.EndVertical();
    }

    private void DrawUserLine(bool isActive, bool isDead, bool isVictim, bool isNemesis, int domination, bool isEnd, short position, string name, int points, int kills, int deaths, int ping, bool isFriend, Texture2D clanIco, int user_id)
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle((!isActive) ? "tabLine" : "tabLineActive"));
        GUILayout.Space(2f);
        GUIStyle gUIStyle = GUIStyle.none;
        string text = "tabItemName";
        if (isFriend)
        {
            text = "tabItemNameFriend";
            gUIStyle = GUISkinManager.BattleIco.GetStyle("tabFriend");
        }
        GUILayout.Label(GUIContent.none, (!isDead || isEnd) ? gUIStyle : GUISkinManager.BattleIco.GetStyle("tabDead"), GUILayout.Width(25f));
        GUILayout.Label(position.ToString(), GUISkinManager.BattleText.GetStyle((!isActive && !isEnd) ? "tabItemPos" : "tabItemPosActive"), GUILayout.Width(38f));
        if ((UnityEngine.Object)clanIco != (UnityEngine.Object)null)
        {
            GUILayout.Label(clanIco, GUISkinManager.BattleIco.GetStyle("tabClanArm"));
            GUILayout.Label(name, GUISkinManager.BattleText.GetStyle((!isActive && !isEnd) ? text : "tabItemNameActive"), GUILayout.Width(92f));
        }
        else
        {
            GUILayout.Label(name, GUISkinManager.BattleText.GetStyle((!isActive && !isEnd) ? text : "tabItemNameActive"), GUILayout.Width(115f));
        }
        if (isVictim)
        {
            GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("tabVictim"));
        }
        else if (isNemesis)
        {
            GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("tabNemesis"));
        }
        else
        {
            GUILayout.Label(GUIContent.none, GUIStyle.none, GUILayout.Width(25f));
        }
        if (domination > 0)
        {
            domination = ((domination <= 15) ? domination : 15);
            GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("tabMedal" + domination));
        }
        else
        {
            GUILayout.Label(GUIContent.none, GUIStyle.none, GUILayout.Width(25f));
        }
        GUILayout.Label(points.ToString(), GUISkinManager.BattleText.GetStyle((!isActive && !isEnd) ? "tabItem" : "tabItemActive"), GUILayout.Width(40f));
        GUILayout.Label(kills.ToString(), GUISkinManager.BattleText.GetStyle((!isActive && !isEnd) ? "tabItem" : "tabItemActive"), GUILayout.Width(50f));
        GUILayout.Label(deaths.ToString(), GUISkinManager.BattleText.GetStyle((!isActive && !isEnd) ? "tabItem" : "tabItemActive"), GUILayout.Width(46f));
        GUILayout.Label(ping.ToString(), GUISkinManager.BattleText.GetStyle("tabItemPing"), GUILayout.Width(38f));
        GUILayout.EndHorizontal();
    }

    private void DrawSelectTeam()
    {
        GUILayout.BeginArea(new Rect(((float)Screen.width - 449f) * 0.5f, ((float)Screen.height - 448f) * 0.5f, 449f, 448f), GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("selectTeam"));
        GUILayout.Label(LanguageManager.GetText("Choose a team"), GUISkinManager.BattleBackgound.GetStyle("tabPart"));
        GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), LanguageManager.GetText("Choose a team"), GUISkinManager.BattleText.GetStyle("tabPart"), GUISkinManager.BattleText.GetStyle("tabPartShadow"));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("selectRed"), GUILayout.Width(222f), GUILayout.Height(323f));
        GUILayout.Space(13f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(9f);
        GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("selectRed"), GUILayout.Width(204f), GUILayout.Height(204f));
        GUILayout.EndHorizontal();
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(15f);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("tabItem"), GUILayout.Width(193f), GUILayout.Height(73f));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("Players"), GUISkinManager.BattleText.GetStyle("txt1"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(string.Format("{0}/{1}", PlayerManager.GameScore.GetTeamCount(1), PlayerManager.Instance.RoomSettings.MaxPlayersTeam), GUISkinManager.BattleText.GetStyle("txt24"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Hor"), GUILayout.Width(1f));
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("selectBlue"), GUILayout.Width(222f), GUILayout.Height(323f));
        GUILayout.Space(13f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(9f);
        GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("selectBlue"), GUILayout.Width(204f), GUILayout.Height(204f));
        GUILayout.EndHorizontal();
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(15f);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("tabItem"), GUILayout.Width(193f), GUILayout.Height(73f));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("Players"), GUISkinManager.BattleText.GetStyle("txt1"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(string.Format("{0}/{1}", PlayerManager.GameScore.GetTeamCount(2), PlayerManager.Instance.RoomSettings.MaxPlayersTeam), GUISkinManager.BattleText.GetStyle("txt24"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(75f));
        GUILayout.Space(15f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(193f));
        GUILayout.Space(13f);
        if (PlayerManager.GameScore.GetTeamCount(1) < PlayerManager.Instance.RoomSettings.MaxPlayersTeam && PlayerManager.GameScore.GetTeamCount(1) - PlayerManager.GameScore.GetTeamCount(2) < 2 && GUILayout.Button(LanguageManager.GetText("Join Red"), GUISkinManager.Button.GetStyle("joinRed"), GUILayout.Width(193f), GUILayout.Height(47f)))
        {
            this.showSelectTeam = false;
            GameHUD.HideCursor();
            OptionsManager.Team = 1;
            this.battleGUI.SetTeam(1, PlayerManager.Instance.RoomSettings.GameMode);
            PlayerManager.Instance.LocalPlayer.Team = 1;
            GameHUD.Instance.UpdateHealth();
            GameHUD.Instance.PlayerState = PlayerStates.Dead;
        }
        GUILayout.EndVertical();
        GUILayout.Space(30f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(193f));
        GUILayout.Space(13f);
        if (PlayerManager.GameScore.GetTeamCount(2) < PlayerManager.Instance.RoomSettings.MaxPlayersTeam && PlayerManager.GameScore.GetTeamCount(2) - PlayerManager.GameScore.GetTeamCount(1) < 2 && GUILayout.Button(LanguageManager.GetText("Join Blue"), GUISkinManager.Button.GetStyle("joinBlue"), GUILayout.Width(193f), GUILayout.Height(47f)))
        {
            this.showSelectTeam = false;
            GameHUD.HideCursor();
            OptionsManager.Team = 2;
            this.battleGUI.SetTeam(2, PlayerManager.Instance.RoomSettings.GameMode);
            PlayerManager.Instance.LocalPlayer.Team = 2;
            GameHUD.Instance.UpdateHealth();
            GameHUD.Instance.PlayerState = PlayerStates.Dead;
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    public void DrawUpTeamScores()
    {
        GameScore gameScore = PlayerManager.GameScore;
        int teamPoints = gameScore.GetTeamPoints(1);
        int teamPoints2 = gameScore.GetTeamPoints(2);
        if (teamPoints < 100)
        {
            GUISkinManager.BattleText.GetStyle("upScoreRed").fontSize = 35;
        }
        else if (teamPoints > 100 && teamPoints < 1000)
        {
            GUISkinManager.BattleText.GetStyle("upScoreRed").fontSize = 28;
        }
        else if (teamPoints > 1000)
        {
            GUISkinManager.BattleText.GetStyle("upScoreRed").fontSize = 18;
        }
        if (teamPoints2 < 100)
        {
            GUISkinManager.BattleText.GetStyle("upScoreBlue").fontSize = 35;
        }
        else if (teamPoints2 > 100 && teamPoints2 < 1000)
        {
            GUISkinManager.BattleText.GetStyle("upScoreBlue").fontSize = 28;
        }
        else if (teamPoints2 > 1000)
        {
            GUISkinManager.BattleText.GetStyle("upScoreBlue").fontSize = 18;
        }
        float num = 145f;
        if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.CAPTURE_THE_FLAG || PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.CONTROL_POINTS)
        {
            num = 175f;
        }
        GUILayout.BeginArea(new Rect((float)Screen.width * 0.5f - num - 40f, 0f, num, 41f), GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("leftScore"));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.CAPTURE_THE_FLAG || PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.CONTROL_POINTS)
        {
            GUILayout.Label(string.Format("{0}\\", PlayerManager.Instance.RoomSettings.Frags), GUISkinManager.BattleText.GetStyle("upScoreRedSmall"));
            GUILayout.Label(teamPoints.ToString(), GUISkinManager.BattleText.GetStyle("upScoreRed"));
        }
        else
        {
            GUILayout.Label(teamPoints.ToString(), GUISkinManager.BattleText.GetStyle("upScoreRed"));
        }
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.CAPTURE_THE_FLAG)
        {
            GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("upScoreCTFRed_" + PlayerManager.GameScore.Teams[0].FlagState), GUILayout.Width(37f), GUILayout.Height(33f));
        }
        else if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.ZOMBIE)
        {
            GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("upScoreZombie"), GUILayout.Width(37f), GUILayout.Height(33f));
        }
        else
        {
            GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("upScoreRed"), GUILayout.Width(37f), GUILayout.Height(33f));
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        Rect screenRect = new Rect((float)Screen.width * 0.5f + 40f, 0f, num, 41f);
        GUILayout.BeginArea(screenRect, GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("rightScore"));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.CAPTURE_THE_FLAG)
        {
            GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("upScoreCTFBlue_" + PlayerManager.GameScore.Teams[1].FlagState), GUILayout.Width(37f), GUILayout.Height(33f));
        }
        else if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.ZOMBIE)
        {
            GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("upScoreHuman"), GUILayout.Width(37f), GUILayout.Height(33f));
        }
        else
        {
            GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("upScoreBlue"), GUILayout.Width(37f), GUILayout.Height(33f));
        }
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.CAPTURE_THE_FLAG || PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.CONTROL_POINTS)
        {
            GUILayout.Label(teamPoints2.ToString(), GUISkinManager.BattleText.GetStyle("upScoreBlue"));
            GUILayout.Label(string.Format("/{0}", PlayerManager.Instance.RoomSettings.Frags), GUISkinManager.BattleText.GetStyle("upScoreBlueSmall"));
        }
        else
        {
            GUILayout.Label(teamPoints2.ToString(), GUISkinManager.BattleText.GetStyle("upScoreBlue"));
        }
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawFrags()
    {
        GUILayout.BeginArea(new Rect((float)(Screen.width - 350), 5f, 350f, 300f), GUIStyle.none);
        Queue<FragKill>.Enumerator enumerator = PlayerManager.GameScore.Frags.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                FragKill current = enumerator.Current;
                short num = (short)current.KillerTeam;
                short num2 = (short)current.KilledTeam;
                bool flag = current.KillerID == LocalUser.UserID;
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle((!flag) ? "fragLine" : "fragLineLocal"));
                if (flag)
                {
                    GUILayout.Label(current.KillerShowedName, GUISkinManager.BattleText.GetStyle("fragLocal"));
                }
                else if (current.KillerIsFriend)
                {
                    GUILayout.Label(current.KillerShowedName, GUISkinManager.BattleText.GetStyle("fragFriend"));
                }
                else
                {
                    switch (num)
                    {
                        case 1:
                            GUILayout.Label(current.KillerShowedName, GUISkinManager.BattleText.GetStyle("fragRed"));
                            break;
                        case 2:
                            GUILayout.Label(current.KillerShowedName, GUISkinManager.BattleText.GetStyle("fragBlue"));
                            break;
                        default:
                            GUILayout.Label(current.KillerShowedName, GUISkinManager.BattleText.GetStyle("fragDefault"));
                            break;
                    }
                }
                if (current.KillerID == current.KilledID)
                {
                    GUILayout.Label(GUIContent.none, GUISkinManager.BattleFrags.GetStyle((!flag) ? "fragSuicide" : "fragSuicideLocal"));
                }
                else
                {
                    GUIStyle gUIStyle = null;
                    gUIStyle = ((current.PlayerHitZone != PlayerHitZone.CABIN) ? ((current.PlayerHitZone != PlayerHitZone.ENGINE) ? GUISkinManager.BattleFrags.FindStyle("frag" + current.WeaponSystemName + ((!flag) ? string.Empty : "Local")) : GUISkinManager.BattleFrags.FindStyle("fragNuts" + ((!flag) ? string.Empty : "Local"))) : GUISkinManager.BattleFrags.FindStyle("fragHead" + ((!flag) ? string.Empty : "Local")));
                    if (gUIStyle == null)
                    {
                        gUIStyle = GUISkinManager.BattleFrags.GetStyle("fragNone");
                    }
                    GUILayout.Label(GUIContent.none, gUIStyle);
                    if (flag)
                    {
                        GUILayout.Label(current.KilledShowedName, GUISkinManager.BattleText.GetStyle("fragLocal"));
                    }
                    else if (current.KilledIsFriend)
                    {
                        GUILayout.Label(current.KilledShowedName, GUISkinManager.BattleText.GetStyle("fragFriend"));
                    }
                    else
                    {
                        switch (num2)
                        {
                            case 1:
                                GUILayout.Label(current.KilledShowedName, GUISkinManager.BattleText.GetStyle("fragRed"));
                                break;
                            case 2:
                                GUILayout.Label(current.KilledShowedName, GUISkinManager.BattleText.GetStyle("fragBlue"));
                                break;
                            default:
                                GUILayout.Label(current.KilledShowedName, GUISkinManager.BattleText.GetStyle("fragDefault"));
                                break;
                        }
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.EndHorizontal();
                if (current.FType != 0)
                {
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("fragLine"));
                    bool flag2 = current.KillerIsFriend;
                    string text = current.KillerShowedName;
                    bool flag3 = current.KilledIsFriend;
                    string text2 = current.KilledShowedName;
                    if (current.FType == FragKill.FragType.Assist)
                    {
                        text2 = text;
                        flag3 = flag2;
                        num2 = num;
                        text = current.AssistantShowedName;
                        flag2 = current.AssistantIsFriend;
                        num = (short)current.AssistantTeam;
                    }
                    if (flag2)
                    {
                        GUILayout.Label(text, GUISkinManager.BattleText.GetStyle("fragFriend"));
                    }
                    else
                    {
                        switch (num)
                        {
                            case 1:
                                GUILayout.Label(text, GUISkinManager.BattleText.GetStyle("fragRed"));
                                break;
                            case 2:
                                GUILayout.Label(text, GUISkinManager.BattleText.GetStyle("fragBlue"));
                                break;
                            default:
                                GUILayout.Label(text, GUISkinManager.BattleText.GetStyle("fragDefault"));
                                break;
                        }
                    }
                    if (current.FType == FragKill.FragType.Domination)
                    {
                        GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("domination"));
                        GUILayout.Label(LanguageManager.GetText("is DOMINATING"), GUISkinManager.BattleText.GetStyle("fragDefault"));
                    }
                    else if (current.FType == FragKill.FragType.Revenge)
                    {
                        GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("domination"));
                        GUILayout.Label(LanguageManager.GetText("got REVENGE on"), GUISkinManager.BattleText.GetStyle("fragDefault"));
                    }
                    else if (current.FType == FragKill.FragType.Assist)
                    {
                        GUILayout.Space(2f);
                        GUILayout.Label(LanguageManager.GetText("assisted"), GUISkinManager.BattleText.GetStyle("fragDefault"));
                    }
                    GUILayout.Space(2f);
                    if (flag3)
                    {
                        GUILayout.Label(text2, GUISkinManager.BattleText.GetStyle("fragFriend"));
                    }
                    else
                    {
                        switch (num2)
                        {
                            case 1:
                                GUILayout.Label(text2, GUISkinManager.BattleText.GetStyle("fragRed"));
                                break;
                            case 2:
                                GUILayout.Label(text2, GUISkinManager.BattleText.GetStyle("fragBlue"));
                                break;
                            default:
                                GUILayout.Label(text2, GUISkinManager.BattleText.GetStyle("fragDefault"));
                                break;
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        GUILayout.EndArea();
    }

    private void DrawMenu()
    {
        bool flag = false;
        if (LocalUser.Permission.Kick)
        {
            flag = true;
        }
        if (GameHUD.isVoteKick)
        {
            this.DrawKickWindow();
        }
        else
        {
            float num = 248f;
            if (flag)
            {
                num += 41f;
            }
            GUILayout.BeginArea(new Rect(((float)Screen.width - 226f) / 2f, ((float)Screen.height - 194f) / 2f, 226f, num), GUIContent.none, GUISkinManager.Backgound.window);
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Label(LanguageManager.GetText("Menu"), GUISkinManager.Text.GetStyle("popupTitle"), GUILayout.Height(34f));
            GUILayout.EndHorizontal();
            GUILayout.Space(8f);
            if (GUILayout.Button(LanguageManager.GetText("Resume"), GUISkinManager.Button.GetStyle("green"), GUILayout.Width(191f), GUILayout.Height(37f)))
            {
                GameHUD.IsShowMenu = false;
                if (!this.showSelectTeam)
                {
                    GameHUD.HideCursor();
                }
            }
            GUILayout.Space(4f);
            if (GUILayout.Button(LanguageManager.GetText("Fullscreen"), GUISkinManager.Button.GetStyle("cancel"), GUILayout.Width(191f), GUILayout.Height(37f)))
            {
                GameHUD.IsShowMenu = false;
                GameHUD.HideCursor();
                OptionsManager.FullScreen = true;
            }
            GUILayout.Space(4f);
            if (flag)
            {
                if (GUILayout.Button(LanguageManager.GetText("Vote kick"), GUISkinManager.Button.GetStyle("cancel"), GUILayout.Width(191f), GUILayout.Height(37f)))
                {
                    GameHUD.isVoteKick = true;
                }
                GUILayout.Space(4f);
            }
            if (GUILayout.Button(LanguageManager.GetText("Options"), GUISkinManager.Button.GetStyle("cancel"), GUILayout.Width(191f), GUILayout.Height(37f)))
            {
                OptionPopup.Show = true;
            }
            GUILayout.Space(12f);
            if (GUILayout.Button(LanguageManager.GetText("Exit to My HQ"), GUISkinManager.Button.GetStyle("cancel"), GUILayout.Width(191f), GUILayout.Height(37f)))
            {
                this.ExitToMenu();
            }
            GUILayout.EndArea();
        }
    }

    private static void HandlerListVoteReasonChangeClickCallBack(object sender, object entry)
    {
        if (entry != null && entry.GetType() == typeof(GUIDropDownList.GUIDropDownEntry))
        {
            GUIDropDownList.GUIDropDownEntry gUIDropDownEntry = entry as GUIDropDownList.GUIDropDownEntry;
            GameHUD.selectedKickReason = (KickReason)(byte)gUIDropDownEntry.Tag;
        }
    }

    private void DrawVoteKick()
    {
        GUILayout.BeginArea(new Rect(0f, (float)Screen.height * 0.5f, 292f, 182f), GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("tips"));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(5f);
        GUILayout.Label(LanguageManager.GetText("Exception:"), GUISkinManager.BattleText.GetStyle("tips02"), GUILayout.Height(25f));
        GUILayout.FlexibleSpace();
        GUILayout.Label(string.Format("{0} ({1})", KickManager.Instance.KickedName, KickManager.Instance.KickedAuthID), GUISkinManager.BattleText.GetStyle("tabItem"));
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("transHor"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(KickManagerHelper.ToString(KickManager.Instance.VoteReason), GUISkinManager.BattleText.GetStyle("txtKickReason"), GUILayout.Height(36f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("transHor"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(5f);
        GUILayout.Label(LanguageManager.GetText("Applicant:"), GUISkinManager.BattleText.GetStyle("tips02"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(KickManager.Instance.KickStarterName, GUISkinManager.BattleText.GetStyle("tips02"));
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.Space(6f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("Yes (Y)"), GUISkinManager.BattleText.GetStyle("txtVoteHelpBtn"), GUILayout.Width(90f));
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("No (N)"), GUISkinManager.BattleText.GetStyle("txtVoteHelpBtn"), GUILayout.Width(90f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(KickManager.Instance.VoteYes.ToString(), GUISkinManager.BattleText.GetStyle("txt24"), GUILayout.Width(90f));
        GUILayout.FlexibleSpace();
        GUILayout.Label(KickManager.Instance.VoteNo.ToString(), GUISkinManager.BattleText.GetStyle("txt24"), GUILayout.Width(90f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(11f);
        if (KickManager.Instance.State == KickVoteState.Progress)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Space(5f);
            GUILayout.Label(LanguageManager.GetText("Voting time left:"), GUISkinManager.BattleText.GetStyle("tips02"), GUILayout.Height(25f));
            GUILayout.Label(string.Format("{0}", KickManager.Instance.TimeLeft()), GUISkinManager.BattleText.GetStyle("tabItem"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else if (KickManager.Instance.State == KickVoteState.Result)
        {
            if (KickManager.Instance.VoteResult)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Space(5f);
                GUILayout.Label(LanguageManager.GetText("Request is accepted"), GUISkinManager.BattleText.GetStyle("tabItem"));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Space(5f);
                GUILayout.Label(LanguageManager.GetText("Request is declined"), GUISkinManager.BattleText.GetStyle("tabItem"));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndArea();
    }

    private void DrawKickWindow()
    {
        GUILayout.BeginArea(new Rect(((float)Screen.width - 355f) / 2f, 0f, 355f, (float)Screen.height), GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.window, GUILayout.MinWidth(302f));
        GUILayout.Label(LanguageManager.GetText("Vote kick"), GUISkinManager.Text.GetStyle("popupTitle"), GUILayout.Height(34f));
        GUILayout.Space(8f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(45f));
        GUILayout.FlexibleSpace();
        if (GameHUD.dropDownSetting != null && GameHUD.dropDownSetting.ShowList)
        {
            GUILayout.Label(KickManagerHelper.ToString(GameHUD.selectedKickReason), GUISkinManager.Backgound.GetStyle("dropdownlist"), GUILayout.Width(316f), GUILayout.Height(33f));
        }
        else if (GUILayout.Button(KickManagerHelper.ToString(GameHUD.selectedKickReason), GUISkinManager.Backgound.GetStyle("dropdownlist"), GUILayout.Width(316f), GUILayout.Height(33f)))
        {
            List<GUIDropDownList.GUIDropDownEntry> list = new List<GUIDropDownList.GUIDropDownEntry>();
            list.Add(new GUIDropDownList.GUIDropDownEntry(KickManagerHelper.ToString(KickReason.Threats), KickReason.Threats));
            list.Add(new GUIDropDownList.GUIDropDownEntry(KickManagerHelper.ToString(KickReason.Cheating), KickReason.Cheating));
            list.Add(new GUIDropDownList.GUIDropDownEntry(KickManagerHelper.ToString(KickReason.Other), KickReason.Other));
            if (LocalUser.Permission.Kick)
            {
                list.Add(new GUIDropDownList.GUIDropDownEntry(KickManagerHelper.ToString(KickReason.Instant), KickReason.Instant));
            }
            GameHUD.dropDownSetting = new GUIDropDownList.GUIDropDownSetting("modeSelect", true, list.ToArray(), new GUIDropDownList.ListCallBack(GameHUD.HandlerListVoteReasonChangeClickCallBack));
        }
        if (Event.current.type == EventType.Repaint && GameHUD.dropDownSetting != null && !GameHUD.dropDownSetting.ValidRect && GameHUD.dropDownSetting.Owner == "modeSelect")
        {
            Rect lastRect = GUILayoutUtility.GetLastRect();
            lastRect.y += lastRect.height - 11f;
            lastRect.height = 300f;
            Vector2 vector = UnityEngine.Input.mousePosition;
            vector.y = (float)Screen.height - vector.y;
            float x = lastRect.x;
            float x2 = vector.x;
            Vector2 mousePosition = Event.current.mousePosition;
            lastRect.x = x + (x2 - mousePosition.x);
            float y = lastRect.y;
            float y2 = vector.y;
            Vector2 mousePosition2 = Event.current.mousePosition;
            lastRect.y = y + (y2 - mousePosition2.y);
            GameHUD.dropDownSetting.SetValidRect(lastRect);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        List<string> list2 = new List<string>();
        List<int> list3 = new List<int>();
        List<ScoreTeam>.Enumerator enumerator = PlayerManager.GameScore.Teams.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ScoreTeam current = enumerator.Current;
                Dictionary<int, ScorePlayer>.Enumerator enumerator2 = current.SortedUserList.GetEnumerator();
                try
                {
                    while (enumerator2.MoveNext())
                    {
                        KeyValuePair<int, ScorePlayer> current2 = enumerator2.Current;
                        if (GameHUD.dropDownSetting != null && GameHUD.dropDownSetting.ShowList)
                        {
                            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                            GUILayout.Space(10f);
                            GUILayout.Label(current2.Value.UserName, GUISkinManager.Button.GetStyle("selectGridRadio02"), GUILayout.MinWidth(140f));
                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            list2.Add(current2.Value.UserName);
                            list3.Add(current2.Value.UserID);
                        }
                    }
                }
                finally
                {
                    ((IDisposable)enumerator2).Dispose();
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        if (list2.Count > 0)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Space(10f);
            if (GameHUD.selectedUser2Kick == 0 || !list3.Contains(GameHUD.selectedUser2Kick))
            {
                GameHUD.selectedUser2Kick = list3[0];
            }
            int index = GUILayout.SelectionGrid(list3.IndexOf(GameHUD.selectedUser2Kick), list2.ToArray(), 1, GUISkinManager.Button.GetStyle("selectGridRadio02"), GUILayout.MinWidth(240f));
            GameHUD.selectedUser2Kick = list3[index];
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        if (GUILayout.Button(LanguageManager.GetText("YES"), GUISkinManager.Button.GetStyle("green"), GUILayout.Width(157f), GUILayout.Height(37f)))
        {
            if (KickManager.Instance.CanStartVote() || (GameHUD.selectedKickReason == KickReason.Instant && LocalUser.Permission.Kick))
            {
                bool flag = KickManager.Instance.InitVote(GameHUD.selectedUser2Kick, (byte)GameHUD.selectedKickReason);
                UnityEngine.Debug.LogError("InitVote: " + flag);
            }
            else
            {
                UnityEngine.Debug.LogError("Can`t start vote");
            }
            GameHUD.IsShowMenu = false;
            GameHUD.HideCursor();
        }
        GUILayout.Space(5f);
        if (GUILayout.Button(LanguageManager.GetText("Cancel"), GUISkinManager.Button.GetStyle("cancel"), GUILayout.Width(157f), GUILayout.Height(37f)))
        {
            GameHUD.IsShowMenu = false;
            GameHUD.HideCursor();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(6f);
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
        if (GameHUD.dropDownSetting != null && GameHUD.dropDownSetting.ShowList)
        {
            GUIDropDownList.List(GameHUD.dropDownSetting);
        }
    }

    private void DrawStateDead(long respawnTime, FragKill frag)
    {
        this.HideBattleGUI();
        GUILayout.BeginArea(new Rect(0f, (float)Screen.height * 0.5f - 157f, 244f, 92f + ((Configuration.SType == ServerType.MM || Configuration.SType == ServerType.FACEBOOK || Configuration.SType == ServerType.KONGREGATE) ? 0f : 23f)), GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("tips"));
        if (Configuration.SType != ServerType.MM && Configuration.SType != ServerType.FACEBOOK && Configuration.SType != ServerType.KONGREGATE)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Label(TRInput.ScreenShot.ToDisplayString(), GUISkinManager.BattleText.GetStyle("tips01"), GUILayout.Width(50f));
            GUILayout.Label(LanguageManager.GetText("Screenshot"), GUISkinManager.BattleText.GetStyle("tips02"));
            GUILayout.EndHorizontal();
            GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("transHor"), GUILayout.Height(1f));
        }
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label("ALT + F", GUISkinManager.BattleText.GetStyle("tips01"), GUILayout.Width(50f));
        GUILayout.Label(LanguageManager.GetText("Fullscreen"), GUISkinManager.BattleText.GetStyle("tips02"));
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("transHor"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label("Tab", GUISkinManager.BattleText.GetStyle("tips01"), GUILayout.Width(50f));
        GUILayout.Label(LanguageManager.GetText("Combat Stats"), GUISkinManager.BattleText.GetStyle("tips02"));
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("transHor"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("RMB"), GUISkinManager.BattleText.GetStyle("tips01"), GUILayout.Width(50f));
        GUILayout.Label(LanguageManager.GetText("Change camera"), GUISkinManager.BattleText.GetStyle("tips02"));
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("transHor"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label("Esc", GUISkinManager.BattleText.GetStyle("tips01"), GUILayout.Width(50f));
        GUILayout.Label(LanguageManager.GetText("Menu"), GUISkinManager.BattleText.GetStyle("tips02"));
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("transHor"), GUILayout.Height(1f));
        GUILayout.EndArea();
        if (frag != null)
        {
            GUILayout.BeginArea(new Rect((float)Screen.width - 227f, (float)Screen.height - 293f, 227f, 183f), GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("killerInfo"));
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(20f));
            GUILayout.Space(2f);
            GUILayout.Label(LanguageManager.GetText("Killer:"), GUISkinManager.BattleText.GetStyle("txt1"));
            GUILayout.Space(2f);
            GUILayout.Label(frag.KillerShowedName, GUISkinManager.BattleText.GetStyle("txt1Value"));
            GUILayout.EndHorizontal();
            GUILayout.Space(8f);
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(33f));
            GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("healthSmall"), GUILayout.Width(22f), GUILayout.Height(22f));
            GUILayout.Space(2f);
            GUILayout.Label(frag.KillerHealth.ToString(), GUISkinManager.BattleText.GetStyle("healthSmall"), GUILayout.MinWidth(47f));
            GUILayout.Label(GUIContent.none, GUISkinManager.BattleIco.GetStyle("armorSmall"), GUILayout.Width(22f), GUILayout.Height(22f));
            GUILayout.Label(frag.KillerEnergy.ToString(), GUISkinManager.BattleText.GetStyle("healthSmall"), GUILayout.MinWidth(47f));
            GUILayout.EndHorizontal();
            GUILayout.Space(2f);
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Space(2f);
            GUILayout.Label(LanguageManager.GetText("Weapon:"), GUISkinManager.BattleText.GetStyle("txt1"));
            GUILayout.Space(2f);
            GUILayout.Label(frag.Weapon.Name, GUISkinManager.BattleText.GetStyle("txt1Value"));
            GUILayout.EndHorizontal();
            GUILayout.Space(2f);
            GUILayout.Label(frag.Weapon.Ico, GUISkinManager.Backgound.GetStyle("itemLeft2"), GUILayout.Width(101f), GUILayout.Height(90f));
            GUILayout.EndArea();
        }
        if (respawnTime <= 0)
        {
            this.DrawBottomMessage(LanguageManager.GetText("PRESS"), LanguageManager.GetText("FIRE"), LanguageManager.GetText("TO RESPAWN"));
        }
        else
        {
            this.DrawBottomMessage(LanguageManager.GetText("RESPAWN IN"), respawnTime.ToString(), LanguageManager.GetText("SECONDS"));
        }
    }

    public void DrawBottomMessage(string msg1, string msg2, string msg3)
    {
        GUILayout.BeginArea(new Rect(0f, (float)Screen.height - 80f, (float)Screen.width, 43f), GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("respawnLine"));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(msg1, GUISkinManager.BattleText.GetStyle("respawn01"));
        GUILayout.Label(msg2, GUISkinManager.BattleText.GetStyle("respawn02"));
        GUILayout.Label(msg3, GUISkinManager.BattleText.GetStyle("respawn01"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void ShowBattleGUI()
    {
        if (!this.battleGUI.enabled)
        {
            this.battleGUI.enabled = true;
        }
    }

    private void HideBattleGUI()
    {
        if (this.battleGUI.enabled)
        {
            this.battleGUI.enabled = false;
        }
    }

    private void OnGUI()
    {
        if ((UnityEngine.Object)PlayerManager.Instance.LocalPlayer == (UnityEngine.Object)null)
        {
            DebugConsole.ShowDebug(Event.current);
            if (GameHUD.IsShowMenu)
            {
                this.DrawMenu();
            }
        }
        else
        {
            if (this.cheatMessage != string.Empty)
            {
                GUITextShadow.TextShadow(new Rect(0f, 45f, (float)Screen.width, (float)Screen.height - 146f), this.cheatMessage, GUISkinManager.BattleText.GetStyle("cheatMsg"), GUISkinManager.BattleText.GetStyle("cheatMsgBorder"));
            }
            if (!this.developHideGUI)
            {
                this.DrawUpTimer();
                if ((int)PlayerManager.Instance.RoomSettings.GameMode > 1 && PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.TOWER_DEFENSE)
                {
                    this.DrawUpTeamScores();
                }
                switch (PlayerManager.Instance.RoomSettings.GameMode)
                {
                    case MapMode.MODE.CONTROL_POINTS:
                        this.DrawPointStates();
                        break;
                    case MapMode.MODE.TOWER_DEFENSE:
                        this.drawCampaignState();
                        break;
                    case MapMode.MODE.ESCORT:
                        this.drawEscortState();
                        break;
                }
                DebugConsole.ShowDebug(Event.current);
                GUI.skin = GUISkinManager.Battle;
                if (!((UnityEngine.Object)TimeManager.Instance == (UnityEngine.Object)null))
                {
                    if ((UnityEngine.Object)PlayerManager.Instance != (UnityEngine.Object)null && (UnityEngine.Object)PlayerManager.Instance.LocalPlayer != (UnityEngine.Object)null && (int)(TimeManager.Instance.NetworkTime / 1000) % 2 == 0 && PlayerManager.GameScore.ContainsUser(PlayerManager.Instance.LocalPlayer.AuthID))
                    {
                        PlayerManager.GameScore[PlayerManager.Instance.LocalPlayer.AuthID].Ping = (short)TimeManager.Instance.AveragePing;
                    }
                    this.DrawFrags();
                    if (GameHUD.IsShowMenu)
                    {
                        this.DrawMenu();
                        return;
                    }
                    BattleChat.Draw();
                    BattleInfo.Draw();
                    if ((UnityEngine.Object)KickManager.Instance != (UnityEngine.Object)null && KickManager.Instance.State != 0 && LocalUser.Permission.Kick)
                    {
                        this.DrawVoteKick();
                    }
                    if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
                    {
                        goto IL_0266;
                    }
                    if (Event.current.type == EventType.Used && Event.current.keyCode == KeyCode.Return)
                    {
                        goto IL_0266;
                    }
                    goto IL_02bd;
                }
            }
        }
        return;
        IL_0266:
        BattleChat.IsWrite = !BattleChat.IsWrite;
        if (Event.current.control && BattleChat.IsWrite && (int)PlayerManager.Instance.RoomSettings.GameMode > 1 && PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.TOWER_DEFENSE)
        {
            BattleChat.IsTeam = true;
        }
        goto IL_02bd;
        IL_02bd:
        switch (this.PlayerState)
        {
            case PlayerStates.Play:
            case PlayerStates.Zombie_Boss_Infection:
                if (this.PlayerState == PlayerStates.Zombie_Boss_Infection)
                {
                    if ((UnityEngine.Object)PlayerManager.Instance.LocalPlayer != (UnityEngine.Object)null && PlayerManager.Instance.LocalPlayer.IsDead)
                    {
                        this.PlayerState = PlayerStates.Zombie_Dead_Player;
                        break;
                    }
                    long num = 20000 + this.zombieInfectionStart - TimeManager.Instance.NetworkTime;
                    int num2 = (int)(num / 1000);
                    this.DrawBottomMessage(LanguageManager.GetText("BOSS INFECTION IN"), string.Format("{0}/{1}", num2, 20), LanguageManager.GetText("SECONDS"));
                    this.HideBattleGUI();
                }
                else
                {
                    this.ShowBattleGUI();
                }
                this.InitWeaponStyles();
                this.DrawUserStats();
                if (this.weaponStylesInit)
                {
                    GUILayout.BeginArea(new Rect((float)(Screen.width / 2 - 140), (float)(Screen.height - 60), 417f, 56f));
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.FlexibleSpace();
                    for (int i = 0; i < 5; i++)
                    {
                        if (ShotController.Instance.Weapons[i] == ShotController.Instance.CurrentWeapon)
                        {
                            GUILayout.BeginHorizontal(ShotController.Instance.GetAmmoCount(i).ToString(), this.ammoStyleActive[i]);
                        }
                        else if (ShotController.Instance.Weapons[i] == null)
                        {
                            GUILayout.BeginHorizontal(GUIContent.none, this.ammoStyleEmpty);
                        }
                        else
                        {
                            GUILayout.BeginHorizontal(ShotController.Instance.GetAmmoCount(i).ToString(), this.ammoStyle[i]);
                        }
                        if (ShotController.Instance.Weapons[i] != null)
                        {
                            if ((ShotController.Instance.Weapons[i].EnhacerModes & 1) == 1)
                            {
                                GUILayout.Label(GUIContent.none, "icoUseEnhDamage");
                                GUILayout.FlexibleSpace();
                            }
                            if ((ShotController.Instance.Weapons[i].EnhacerModes & 2) == 2)
                            {
                                GUILayout.Label(GUIContent.none, "icoUseEnhRapidity");
                            }
                        }
                        GUILayout.Label(GUIContent.none, GUIStyle.none);
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.Space(25f);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndArea();
                    GUILayout.BeginArea(new Rect((float)(Screen.width / 2 - 140), (float)(Screen.height - 60), 417f, 56f));
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    for (int i = 0; i < 5; i++)
                    {
                        GUILayout.BeginScrollView(new Vector2((float)ShotController.Instance.getWeaponReload(i) * 56f, 0f), GUILayout.Width(55f));
                        GUILayout.Label(GUIContent.none, "AmmoReload");
                        GUILayout.EndScrollView();
                    }
                    GUILayout.Space(25f);
                    for (int i = 0; i < 2; i++)
                    {
                        GUILayout.BeginScrollView(new Vector2((float)ShotController.Instance.getEnhancerReload(i) * 56f, 0f), GUILayout.Width(55f));
                        GUILayout.Label(GUIContent.none, "AmmoReload");
                        GUILayout.EndScrollView();
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndArea();
                    GUILayout.BeginArea(new Rect((float)(Screen.width / 2 - 140), (float)(Screen.height - 74), 417f, 19f));
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(56f));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(TRInput.Weapon1.ToDisplayString(), (ShotController.Instance.CurrentWeapon.Index != 0) ? "hotkeyInfo" : "hotkeyInfoActive", GUILayout.MinWidth(20f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(56f));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(TRInput.Weapon2.ToDisplayString(), (ShotController.Instance.CurrentWeapon.Index != 1) ? "hotkeyInfo" : "hotkeyInfoActive", GUILayout.MinWidth(20f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(56f));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(TRInput.Weapon3.ToDisplayString(), (ShotController.Instance.CurrentWeapon.Index != 2) ? "hotkeyInfo" : "hotkeyInfoActive", GUILayout.MinWidth(20f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(56f));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(TRInput.Weapon4.ToDisplayString(), (ShotController.Instance.CurrentWeapon.Index != 3) ? "hotkeyInfo" : "hotkeyInfoActive", GUILayout.MinWidth(20f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(56f));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(TRInput.Weapon5.ToDisplayString(), (ShotController.Instance.CurrentWeapon.Index != 4) ? "hotkeyInfo" : "hotkeyInfoActive", GUILayout.MinWidth(20f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(25f);
                    GUILayout.EndHorizontal();
                    GUILayout.EndArea();
                }
                break;
            case PlayerStates.Zombie_Waiting_Players:
                this.DrawBottomMessage(LanguageManager.GetText("WAITING FOR SUFFICIENT NUMBER OF PLAYERS"), string.Format("{0}/{1}", PlayerManager.Instance.Players.Count, 3), string.Empty);
                this.HideBattleGUI();
                break;
            case PlayerStates.Zombie_Dead_Zombie:
            case PlayerStates.Zombie_Dead_Player:
                this.HideBattleGUI();
                this.DrawBottomMessage(LanguageManager.GetText("WAIT FOR THE NEXT MATCH PLEASE"), LanguageManager.GetText("RIGHT CLICK TO SWITCH CAMERA"), string.Empty);
                break;
            case PlayerStates.Dead:
                if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.ZOMBIE && (UnityEngine.Object)PlayerManager.Instance.LocalPlayer != (UnityEngine.Object)null && PlayerManager.Instance.LocalPlayer.IsDead)
                {
                    this.PlayerState = PlayerStates.Zombie_Dead_Player;
                }
                else
                {
                    if (this.timeOfDeath == 0L)
                    {
                        this.timeOfDeath = TimeManager.Instance.NetworkTime;
                        base.StartCoroutine(this.SetDeadState());
                    }
                    long num3 = (this.timeOfDeath + (long)this.respawnDelay * 1000 - TimeManager.Instance.NetworkTime) / 1000;
                    if (num3 < 0)
                    {
                        num3 = 0L;
                    }
                    this.DrawStateDead(num3, (!PlayerManager.GameScore.ContainsUser(PlayerManager.Instance.LocalPlayer.AuthID)) ? null : PlayerManager.GameScore[PlayerManager.Instance.LocalPlayer.AuthID].LastFrag);
                }
                break;
            case PlayerStates.DeadReady:
                if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.ZOMBIE && (UnityEngine.Object)PlayerManager.Instance.LocalPlayer != (UnityEngine.Object)null && PlayerManager.Instance.LocalPlayer.IsDead)
                {
                    this.PlayerState = PlayerStates.Zombie_Dead_Player;
                }
                else
                {
                    this.DrawStateDead(0L, (!PlayerManager.GameScore.ContainsUser(PlayerManager.Instance.LocalPlayer.AuthID)) ? null : PlayerManager.GameScore[PlayerManager.Instance.LocalPlayer.AuthID].LastFrag);
                }
                break;
            case PlayerStates.TimeOver:
                this.ShowCursor();
                this.DrawFightEnd();
                break;
            case PlayerStates.TimeOverReady:
                if (!Screen.lockCursor && !GameHUD.IsShowMenu && !this.showSelectTeam && !NotificationWindow.IsShow && !LoadingMapPopup.Show && Screen.width > 10)
                {
                    GameHUD.HideCursor();
                }
                this.DrawStateDead(0L, null);
                break;
        }
        if (this.screenMessage != string.Empty)
        {
            GUITextShadow.TextShadow(new Rect(0f, 45f, (float)Screen.width, 50f), this.screenMessage, GUISkinManager.BattleText.GetStyle("displayed"), GUISkinManager.BattleText.GetStyle("displayedBorder"));
        }
        if (this.showScore && (this.PlayerState == PlayerStates.Play || this.PlayerState == PlayerStates.Zombie_Boss_Infection || this.PlayerState == PlayerStates.Zombie_Dead_Player || this.PlayerState == PlayerStates.Zombie_Dead_Zombie || this.PlayerState == PlayerStates.Dead || this.PlayerState == PlayerStates.DeadReady))
        {
            this.DrawScore();
        }
        else if (this.showSelectTeam)
        {
            this.DrawSelectTeam();
        }
    }

    private void drawEscortState()
    {
        GUILayout.BeginArea(new Rect((float)(Screen.width / 2 - 70), 40f, 140f, 45f), GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        int num = (int)Mathf.Ceil((100f - PlayerManager.Instance.GetEscortProgress(1)) / 8.33f);
        GUILayout.BeginScrollView(new Vector2((float)num * 140f, 0f), GUILayout.Width(140f));
        GUILayout.Label(GUIContent.none, "escortRed");
        GUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        int num2 = (int)Mathf.Ceil((100f - PlayerManager.Instance.GetEscortProgress(2)) / 8.33f);
        GUILayout.BeginScrollView(new Vector2((float)num2 * 140f, 0f), GUILayout.Width(140f));
        GUILayout.Label(GUIContent.none, "escortBlue");
        GUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void drawCampaignState()
    {
        GUILayout.BeginArea(new Rect(0f, 18f, 153f, 46f), GUIContent.none, "towerWaveBlock");
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Wave:"), "towerWaveTxt", GUILayout.MinWidth(79f));
        GUILayout.Label(string.Format("{0}/{1}", PlayerManager.Instance.Campaign.WaveIndex, PlayerManager.Instance.Campaign.MaxWaves), "towerWaveNum");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetTextFormat("Enemies: {0}", PlayerManager.Instance.Campaign.EnemiesCount), "toweWaveEnemiesTxt");
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect((float)(Screen.width / 2 - 70), 40f, 140f, 30f), GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        int num = (int)((100f - (float)PlayerManager.Instance.GetCampaignProgress()) / 8.33f);
        GUILayout.BeginScrollView(new Vector2((float)num * 140f, 0f), GUILayout.Width(140f));
        GUILayout.Label(GUIContent.none, "towerCore");
        GUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawPointStates()
    {
        GUISkin skin = GUI.skin;
        GUI.skin = GUISkinManager.BattleBackgound;
        float x = (float)Screen.width * 0.5f - 125f;
        GUILayout.BeginArea(new Rect(x, 43f, 250f, 43f), GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        for (int i = 0; i < PlayerManager.GameScore.ControlPointCount; i++)
        {
            short team = PlayerManager.GameScore.ControlPoints[i].Team;
            int score = PlayerManager.GameScore.ControlPoints[i].Score;
            GUIStyle style = GUISkinManager.BattleBackgound.GetStyle("cpNeutral");
            if (score == 10)
            {
                style = GUISkinManager.BattleBackgound.GetStyle((team != 1) ? "cpBlue" : "cpRed");
            }
            GUILayout.BeginScrollView(new Vector2(0f, (float)score * 43f - 86f), style, GUILayout.Width(43f), GUILayout.Height(43f));
            if (score < 2)
            {
                GUILayout.Label(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("cpCaptureNeutral"), GUILayout.Width(43f), GUILayout.Height(43f));
            }
            else
            {
                switch (team)
                {
                    case 1:
                        GUILayout.Label(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("cpCaptureRed"), GUILayout.Width(43f), GUILayout.Height(344f));
                        break;
                    case 2:
                        GUILayout.Label(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("cpCaptureBlue"), GUILayout.Width(43f), GUILayout.Height(344f));
                        break;
                }
            }
            GUILayout.EndScrollView();
            if (score != 10)
            {
                Rect lastRect = GUILayoutUtility.GetLastRect();
                GUI.Label(lastRect, ((char)(ushort)(65 + i)).ToString(), GUISkinManager.BattleText.GetStyle("cpPointNum"));
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        GUI.skin = skin;
    }

    private void drawReward(string stringReward, bool isTeam)
    {
        if (isTeam)
        {
            GUILayout.BeginArea(new Rect(2f, 385f, 736f, 32f), GUIContent.none, GUIStyle.none);
        }
        else
        {
            GUILayout.BeginArea(new Rect(2f, 385f, 366f, 34f), GUIContent.none, GUIStyle.none);
        }
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(3f);
        GUILayout.Label(PlayerManager.Instance.RoomSettings.Map.Name, "mapName");
        GUILayout.Label("(" + PlayerManager.Instance.RoomSettings.FilteredName + ")", "roomName");
        GUILayout.FlexibleSpace();
        GUILayout.Label(stringReward, "scoreTitle");
        GUILayout.Space(2f);
        GUILayout.Label(this.reward.ToString(), "scoreRewardBlue");
        GUILayout.Label(LanguageManager.GetText("Credits"), "scoreTitle", GUILayout.Width(60f));
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void drawFightEndStatistic(Rect rect, short teamNum, Dictionary<string, int> items)
    {
        GUILayout.BeginArea(rect, GUIContent.none);
        GUILayout.FlexibleSpace();
        Dictionary<string, int>.Enumerator enumerator = items.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, int> current = enumerator.Current;
                GUILayout.BeginHorizontal();
                if (teamNum < 2)
                {
                    GUILayout.FlexibleSpace();
                }
                GUILayout.Label(string.Format("{0}: {1}", current.Key, current.Value), "fightEndStatisticsLabel");
                GUILayout.Space(2f);
                if (teamNum > 1)
                {
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
    }

    private void drawTextBorder(Rect rect, string text, string style, string borderStyle)
    {
        GUI.Label(new Rect(rect.x - 1f, rect.y - 1f, rect.width, rect.height), text, borderStyle);
        GUI.Label(new Rect(rect.x - 1f, rect.y + 1f, rect.width, rect.height), text, borderStyle);
        GUI.Label(new Rect(rect.x + 1f, rect.y - 1f, rect.width, rect.height), text, borderStyle);
        GUI.Label(new Rect(rect.x + 1f, rect.y + 1f, rect.width, rect.height), text, borderStyle);
        GUI.Label(rect, text, style);
    }

    private void drawTeamStripe(int team)
    {
        GameScore gameScore = PlayerManager.GameScore;
        if (team > 1)
        {
            GUI.Label(new Rect(371f, 325f, 367f, 57f), GUIContent.none, "scoreTeamStripeBlue");
            GUILayout.BeginArea(new Rect(371f, 345f, 367f, 57f));
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.Label(string.Format(LanguageManager.GetText("Blue team: {0}"), gameScore.GetTeamCount(2)), "scoreLabelNormal");
            GUILayout.Space(15f);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        else
        {
            GUI.Label(new Rect(2f, 325f, 367f, 57f), GUIContent.none, "scoreTeamStripeRed");
            GUILayout.BeginArea(new Rect(2f, 345f, 367f, 57f));
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Space(15f);
            GUILayout.Label(string.Format(LanguageManager.GetText("Red team: {0}"), gameScore.GetTeamCount(1)), "scoreLabelNormal");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }

    private void drawTeamStripeResult(int team)
    {
        if (team > 1)
        {
            GUI.Label(new Rect(371f, 325f, 367f, 57f), GUIContent.none, "scoreTeamStripeBlue");
            GUILayout.BeginArea(new Rect(371f, 345f, 367f, 57f));
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.Label(string.Format(LanguageManager.GetText("Blue team: {0}"), PlayerManager.GameScore.GetTeamCount(2)), "scoreLabelNormal");
            GUILayout.Space(15f);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        else
        {
            GUI.Label(new Rect(2f, 325f, 367f, 57f), GUIContent.none, "scoreTeamStripeRed");
            GUILayout.BeginArea(new Rect(2f, 345f, 367f, 57f));
            GUILayout.BeginHorizontal();
            GUILayout.Space(15f);
            GUILayout.Label(string.Format(LanguageManager.GetText("Red team: {0}"), PlayerManager.GameScore.GetTeamCount(1)), "scoreLabelNormal");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }

    private void Update()
    {
        if ((UnityEngine.Object)PlayerManager.Instance.LocalPlayer == (UnityEngine.Object)null)
        {
            if (Input.GetButtonUp("Escape") || UnityEngine.Input.GetKeyUp(KeyCode.Escape))
            {
                GameHUD.IsShowMenu = true;
                this.ShowCursor();
                if (OptionsManager.FullScreen)
                {
                    OptionsManager.FullScreen = false;
                }
            }
            else if (UnityEngine.Input.GetKeyUp(KeyCode.F10))
            {
                GameHUD.IsShowMenu = !GameHUD.IsShowMenu;
                if (GameHUD.IsShowMenu)
                {
                    this.ShowCursor();
                }
                else
                {
                    GameHUD.HideCursor();
                }
            }
        }
        else
        {
            this.SetCrossHairVisible(PlayerManager.Instance.LocalPlayer.SoldierController.FPSCamera.gameObject.activeSelf && !this.zoom);
            if (this.bloodOverlayAmount > 0f)
            {
                this.bloodOverlayAmount -= Time.deltaTime;
            }
            else
            {
                this.bloodOverlayAmount = 0f;
            }
            this.battleGUI.SetBloodOverlay(this.bloodOverlayAmount);
            this.showScore = UnityEngine.Input.GetKey("tab");
            if (Input.GetButtonUp("Escape") || UnityEngine.Input.GetKeyUp(KeyCode.Escape))
            {
                GameHUD.IsShowMenu = true;
                this.ShowCursor();
                if (OptionsManager.FullScreen)
                {
                    OptionsManager.FullScreen = false;
                }
            }
            else if (UnityEngine.Input.GetKeyUp(KeyCode.F10))
            {
                GameHUD.IsShowMenu = !GameHUD.IsShowMenu;
                if (GameHUD.IsShowMenu)
                {
                    this.ShowCursor();
                }
                else if (!this.showSelectTeam)
                {
                    GameHUD.HideCursor();
                }
            }
            else if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !GameHUD.IsShowMenu && !this.showSelectTeam && !NotificationWindow.IsShow && !LoadingMapPopup.Show && this.PlayerState != PlayerStates.TimeOver)
            {
                GameHUD.HideCursor();
            }
            if ((this.PlayerState == PlayerStates.Dead || this.PlayerState == PlayerStates.DeadReady) && Input.GetMouseButtonDown(1))
            {
                PlayerManager.Instance.nextCamera();
            }
            if (PlayerManager.Instance.LocalPlayer.IsGuest)
            {
                if (Input.GetMouseButtonDown(1) && !GameHUD.IsShowMenu)
                {
                    PlayerManager.Instance.nextCamera(1);
                }
                if (Input.GetMouseButtonDown(0) && !GameHUD.IsShowMenu)
                {
                    PlayerManager.Instance.nextCamera(2);
                }
            }
            else
            {
                if (this.PlayerState == PlayerStates.DeadReady && Input.GetMouseButtonDown(0) && !GameHUD.IsShowMenu)
                {
                    this.PlayerState = PlayerStates.Load;
                    if (PlayerManager.Instance.LocalPlayer.Team >= 0)
                    {
                        if (PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.ZOMBIE)
                        {
                            this.NetworkManager.SendSpawnRequest(PlayerManager.Instance.LocalPlayer.Team);
                        }
                    }
                    else if (PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.ZOMBIE)
                    {
                        this.NetworkManager.InitSpawn();
                    }
                }
                if ((this.PlayerState == PlayerStates.Zombie_Dead_Player || this.PlayerState == PlayerStates.Zombie_Dead_Zombie) && Input.GetMouseButtonDown(1) && !GameHUD.IsShowMenu)
                {
                    PlayerManager.Instance.nextCamera();
                }
                if (this.PlayerState == PlayerStates.TimeOverReady && Input.GetMouseButtonDown(0) && !GameHUD.IsShowMenu)
                {
                    this.PlayerState = PlayerStates.Load;
                    if (PlayerManager.Instance.LocalPlayer.Team >= 0)
                    {
                        if (PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.ZOMBIE)
                        {
                            this.NetworkManager.SendSpawnRequest(PlayerManager.Instance.LocalPlayer.Team);
                        }
                    }
                    else if (PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.ZOMBIE)
                    {
                        this.NetworkManager.InitSpawn();
                    }
                }
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.F) && (UnityEngine.Input.GetKey(KeyCode.LeftAlt) || UnityEngine.Input.GetKey(KeyCode.RightAlt)))
            {
                GameHUD.IsShowMenu = false;
                GameHUD.HideCursor();
                OptionsManager.FullScreen = true;
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.H) && UnityEngine.Input.GetKey(KeyCode.LeftControl))
            {
                this.DevelopHideShowGUI();
            }
            if (UnityEngine.Input.GetKeyDown(TRInput.ScreenShot))
            {
                if (Time.time > this.lastScreenShot + this.screenShotDelay)
                {
                    ScreenshotManager.CreateAndUpload();
                    this.lastScreenShot = Time.time;
                }
                else
                {
                    UnityEngine.Debug.LogError(string.Format("Screen shots allowed per 5 seconds only. {0} sec left", this.lastScreenShot + this.screenShotDelay - Time.time));
                }
            }
        }
    }

    public void Play()
    {
        if (this.PlayerState == PlayerStates.Load)
        {
            this.PlayerState = PlayerStates.Play;
        }
        GameHUD.HideCursor();
    }

    public void ShotMe()
    {
        this.bloodOverlayAmount = 1f;
    }

    public void SetPointersVisibility(bool visible)
    {
        FlagPointer[] componentsInChildren = ((Component)this.bloodOverlay.transform.parent).GetComponentsInChildren<FlagPointer>();
        FlagPointer[] array = componentsInChildren;
        foreach (FlagPointer flagPointer in array)
        {
            flagPointer.SetColor((!visible) ? 0f : 0.8f);
        }
    }

    public void SetFlagPointer(Transform me, Transform flag, int team, float alpha)
    {
    }

    public void IndicateDamage()
    {
        if ((UnityEngine.Object)this.damageIndicator != (UnityEngine.Object)null)
        {
            this.damageIndicator.Rewind();
            this.damageIndicator.Play();
        }
    }

    public void ShotMe(Transform me, Transform enemy, int enemyID)
    {
        TargetPointer targetPointer = null;
        TargetPointer[] componentsInChildren = ((Component)this.bloodOverlay.transform.parent).GetComponentsInChildren<TargetPointer>();
        TargetPointer[] array = componentsInChildren;
        foreach (TargetPointer targetPointer2 in array)
        {
            if (targetPointer2.TargetID == enemyID)
            {
                targetPointer = targetPointer2;
            }
        }
        if ((UnityEngine.Object)targetPointer != (UnityEngine.Object)null)
        {
            UnityEngine.Object.Destroy(targetPointer.gameObject);
        }
        GameObject gameObject = UnityEngine.Object.Instantiate(this.enemyHitPointer);
		gameObject.GetComponentInChildren<Renderer> ().material.shader = Shader.Find ("Particles/Additive (Soft)");
        targetPointer = gameObject.GetComponent<TargetPointer>();

        if (!((UnityEngine.Object)targetPointer == (UnityEngine.Object)null))
        {
            targetPointer.Me = me;
            targetPointer.Target = enemy;
            targetPointer.TargetID = enemyID;
            gameObject.transform.parent = this.bloodOverlay.transform.parent;
            gameObject.transform.localPosition = new Vector3(0f, 0f, 2f);
            gameObject.transform.localRotation = new Quaternion(0f, 0f, 2f, 1f);
        }
    }

    public void UpdateHealth()
    {
        if (PlayerManager.Instance.LocalPlayer.IsDead && this.PlayerState == PlayerStates.Play)
        {
            this.PlayerState = PlayerStates.Dead;
            this.timeOfDeath = TimeManager.Instance.NetworkTime;
            base.StartCoroutine(this.SetDeadState());
        }
        int health = (PlayerManager.Instance.LocalPlayer.Health >= 0) ? PlayerManager.Instance.LocalPlayer.Health : 0;
        int armor = (PlayerManager.Instance.LocalPlayer.Energy >= 0) ? PlayerManager.Instance.LocalPlayer.Energy : 0;
        this.battleGUI.SetHealth(health, PlayerManager.Instance.LocalPlayer.MaxHealth);
        this.battleGUI.SetArmor(armor, PlayerManager.Instance.LocalPlayer.MaxEnergy);
    }

    private IEnumerator SetDeadState()
    {
        yield return (object)new WaitForSeconds(this.respawnDelay);
        if (this.PlayerState == PlayerStates.Dead)
        {
            this.PlayerState = PlayerStates.DeadReady;
        }
    }

    public void TimeOver()
    {
        this.PlayerState = PlayerStates.TimeOver;
    }

    public void SetTimeOverReadyState()
    {
        if (this.PlayerState == PlayerStates.TimeOver)
        {
            this.PlayerState = PlayerStates.TimeOverReady;
            if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.ZOMBIE)
            {
                this.PlayerState = PlayerStates.Zombie_Waiting_Players;
            }
        }
    }

    public void ExitToMenu()
    {
        this.PlayerState = PlayerStates.Load;
        GameHUD.IsShowMenu = false;
        Cursor.visible = true;
        Screen.lockCursor = false;
        this.NetworkManager.LeaveToLobby();
    }
}


