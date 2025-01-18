// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGameHUD : MonoBehaviour
{
    public enum PlayerStates
    {
        Load,
        Dead,
        DeadReady,
        TimeOver,
        TimeOverReady,
        Play
    }

    private readonly float respawnDelay = 5.5f;

    private readonly float timeOverDelay = 20.5f;

    public ProgressBar playerLifeBar;

    public GUIStyle healthTextStyle;

    public GUIStyle ammoTextStyle;

    public GUIStyle weaponTextStyle;

    private float bloodOverlayAmount;

    public Renderer bloodOverlay;

    public Renderer zoomOverlay;

    public GameObject enemyHitPointer;

    public Transform CrossHairs;

    public Transform TaskBox;

    private bool showScore = true;

    private int reward;

    private static LocalGameHUD instance;

    public PlayerStates PlayerState;

    public static bool isShowMenu;

    private GUIStyle[] ammoStyle;

    private GUIStyle[] ammoStyleActive;

    private GUIStyle ammoStyleEmpty;

    private GUIStyle[] enhancerStyle = new GUIStyle[2];

    private GUIStyle enhancerStyleEmpty;

    private bool weaponStylesInit;

    public static LocalGameHUD Instance
    {
        get
        {
            return LocalGameHUD.instance;
        }
    }

    public void setReward(int reward)
    {
        this.reward = reward;
    }

    private void InitWeaponStyles()
    {
        if (!this.weaponStylesInit && LocalPlayerManager.Instance.IsInit)
        {
            CombatWeapon[] weapons = LocalShotController.Instance.Weapons;
            this.ammoStyle = new GUIStyle[weapons.Length];
            this.ammoStyleActive = new GUIStyle[weapons.Length];
            this.ammoStyleEmpty = new GUIStyle("Ammo");
            this.ammoStyleEmpty.normal.background = (Texture2D)Resources.Load("Ammo/icon_ammo_e0");
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i] == null)
                {
                    this.ammoStyle[i] = GUIStyle.none;
                    this.ammoStyleActive[i] = GUIStyle.none;
                }
                else
                {
                    this.ammoStyleActive[i] = new GUIStyle("Ammo");
                    this.ammoStyle[i] = new GUIStyle("Ammo");
                    this.ammoStyleActive[i].normal.background = (Texture2D)Resources.Load("Ammo/icon_ammo" + weapons[i].GetType().ToString());
                    this.ammoStyle[i].normal.background = (Texture2D)Resources.Load("Ammo/icon_ammo_e" + weapons[i].GetType().ToString());
                    this.ammoStyleActive[i].normal.textColor = new Color(0.333333343f, 0.31764707f, 0.282352954f, 0.75f);
                    this.ammoStyle[i].normal.textColor = new Color(0.333333343f, 0.31764707f, 0.282352954f, 1f);
                }
            }
            CombatEnhancer[] enhancers = LocalShotController.Instance.Enhancers;
            this.enhancerStyle = new GUIStyle[2];
            this.enhancerStyleEmpty = new GUIStyle("Ammo");
            this.enhancerStyleEmpty.normal.background = (Texture2D)Resources.Load("Enhancers/icon_enhancer_e0");
            this.enhancerStyleEmpty.normal.textColor = new Color(0.333333343f, 0.31764707f, 0.282352954f, 0.75f);
            string empty = string.Empty;
            for (int j = 0; j < 2; j++)
            {
                empty = string.Empty;
                this.enhancerStyle[j] = new GUIStyle("Ammo");
                if (enhancers[j] == null)
                {
                    this.enhancerStyle[j] = GUIStyle.none;
                }
                else
                {
                    if (enhancers[j].EnhancerID == 1 || enhancers[j].EnhancerID == 3)
                    {
                        empty = "Enhancers/icon_enhancer_e1";
                    }
                    else if (enhancers[j].EnhancerID == 2 || enhancers[j].EnhancerID == 4)
                    {
                        empty = "Enhancers/icon_enhancer_e2";
                    }
                    else if (enhancers[j].EnhancerID == 5 || enhancers[j].EnhancerID == 7)
                    {
                        empty = "Enhancers/icon_enhancer_e5";
                    }
                    else if (enhancers[j].EnhancerID == 6 || enhancers[j].EnhancerID == 8)
                    {
                        empty = "Enhancers/icon_enhancer_e6";
                    }
                    this.enhancerStyle[j].normal.background = (Texture2D)Resources.Load(empty);
                    this.enhancerStyle[j].normal.textColor = new Color(0.333333343f, 0.31764707f, 0.282352954f, 1f);
                }
            }
            this.weaponStylesInit = true;
        }
    }

    private void Awake()
    {
        LocalGameHUD.instance = this;
        Application.runInBackground = true;
    }

    public bool isActive()
    {
        if (this.PlayerState != PlayerStates.Play)
        {
            return false;
        }
        if (LocalGameHUD.isShowMenu)
        {
            return false;
        }
        return true;
    }

    private void Start()
    {
        this.weaponStylesInit = false;
    }

    public void LockAndHideCursor()
    {
        Cursor.visible = false;
        Screen.lockCursor = true;
    }

    private void OnGUI()
    {
        GUI.skin = GUISkinManager.Battle;
        if (!((UnityEngine.Object)TimeManager.Instance == (UnityEngine.Object)null))
        {
            this.drawFrags();
            if (LocalGameHUD.isShowMenu)
            {
                this.draw_menu();
                return;
            }
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                goto IL_0075;
            }
            if (Event.current.type == EventType.Used && Event.current.keyCode == KeyCode.Return)
            {
                goto IL_0075;
            }
            goto IL_0082;
        }
        return;
        IL_0075:
        BattleChat.IsWrite = !BattleChat.IsWrite;
        goto IL_0082;
        IL_0082:
        if (!this.showScore || (this.PlayerState != PlayerStates.Play && (this.PlayerState == PlayerStates.Dead || this.PlayerState != PlayerStates.DeadReady)))
        {
            ;
        }
        if ((int)LocalPlayerManager.Instance.RoomSettings.GameMode > 1)
        {
            this.drawTeamScore(Screen.width / 2 - 100 - 26, 10, 1);
            this.drawTeamScore(Screen.width / 2 + 100 - 26, 10, 2);
        }
        long num = TimeManager.Instance.NetworkTime - LocalPlayerManager.Instance.RoomSettings.StartTime;
        if (num < 0)
        {
            num = 0L;
        }
        if (this.PlayerState == PlayerStates.Load || this.PlayerState == PlayerStates.TimeOver)
        {
            num = 0L;
        }
        long num2 = (long)Math.Floor((double)((float)num / 60000f));
        long num3 = (long)Math.Floor((double)((float)num / 1000f)) - num2 * 60;
        GUI.Label(new Rect((float)(Screen.width / 2 - 25), 7f, 300f, 20f), string.Format("{0}:{1:00}", num2, num3), "Time_Text_Shadow");
        GUI.Label(new Rect((float)(Screen.width / 2 - 25), 7f, 300f, 20f), string.Format("{0}:{1:00}", num2, num3), "Time_Text");
        switch (this.PlayerState)
        {
            case PlayerStates.Dead:
                break;
            case PlayerStates.DeadReady:
                break;
            case PlayerStates.TimeOver:
                break;
            case PlayerStates.TimeOverReady:
                break;
            case PlayerStates.Play:
                this.InitWeaponStyles();
                if ((int)LocalPlayerManager.Instance.RoomSettings.GameMode > 1 && LocalPlayerManager.Instance.LocalPlayer.Team == 1)
                {
                    GUI.Label(new Rect(12f, (float)(Screen.height - 60), 60f, 60f), GUIContent.none, "Health_Red");
                    GUI.Label(new Rect(132f, (float)(Screen.height - 60), 60f, 60f), GUIContent.none, "Shield_Red");
                    GUI.Label(new Rect((float)(Screen.width - 52), (float)(Screen.height - 85), 60f, 60f), GUIContent.none, "Ammo_Red");
                }
                else if ((int)LocalPlayerManager.Instance.RoomSettings.GameMode > 1 && LocalPlayerManager.Instance.LocalPlayer.Team == 2)
                {
                    GUI.Label(new Rect(12f, (float)(Screen.height - 60), 60f, 60f), GUIContent.none, "Health_Blue");
                    GUI.Label(new Rect(132f, (float)(Screen.height - 60), 60f, 60f), GUIContent.none, "Shield_Blue");
                    GUI.Label(new Rect((float)(Screen.width - 52), (float)(Screen.height - 85), 60f, 60f), GUIContent.none, "Ammo_Blue");
                }
                else
                {
                    GUI.Label(new Rect(12f, (float)(Screen.height - 60), 60f, 60f), GUIContent.none, "Health_Neutral");
                    GUI.Label(new Rect(132f, (float)(Screen.height - 60), 60f, 60f), GUIContent.none, "Shield_Neutral");
                    GUI.Label(new Rect((float)(Screen.width - 52), (float)(Screen.height - 85), 60f, 60f), GUIContent.none, "Ammo_Neutral");
                }
                if ((UnityEngine.Object)LocalPlayerManager.Instance.LocalPlayer != (UnityEngine.Object)null)
                {
                    int health = LocalPlayerManager.Instance.LocalPlayer.Health;
                    GUI.Label(new Rect(62f, (float)(Screen.height - 50), 300f, 20f), string.Empty + health, "Health_Text_Shadow");
                    GUI.Label(new Rect(62f, (float)(Screen.height - 50), 300f, 20f), string.Empty + health, "Health_Text");
                    int energy = LocalPlayerManager.Instance.LocalPlayer.Energy;
                    GUI.Label(new Rect(182f, (float)(Screen.height - 50), 300f, 20f), string.Empty + energy, "Health_Text_Shadow");
                    GUI.Label(new Rect(182f, (float)(Screen.height - 50), 300f, 20f), string.Empty + energy, "Health_Text");
                }
                GUI.Label(new Rect((float)(Screen.width - 58), (float)(Screen.height - 50), 300f, 20f), LocalShotController.Instance.GetAmmoCountString(), "Ammo_Text_Shadow");
                GUI.Label(new Rect((float)(Screen.width - 58), (float)(Screen.height - 50), 300f, 20f), LocalShotController.Instance.GetAmmoCountString(), "Ammo_Text");
                if (this.weaponStylesInit)
                {
                    GUILayout.BeginArea(new Rect((float)(Screen.width / 2 - 140), (float)(Screen.height - 60), 417f, 56f));
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.FlexibleSpace();
                    for (int i = 0; i < 5; i++)
                    {
                        if (LocalShotController.Instance.Weapons[i] == LocalShotController.Instance.CurrentWeapon)
                        {
                            GUILayout.BeginHorizontal(LocalShotController.Instance.GetAmmoCount(i).ToString(), this.ammoStyleActive[i]);
                        }
                        else if (LocalShotController.Instance.Weapons[i] == null)
                        {
                            GUILayout.BeginHorizontal(GUIContent.none, this.ammoStyleEmpty);
                        }
                        else
                        {
                            GUILayout.BeginHorizontal(LocalShotController.Instance.GetAmmoCount(i).ToString(), this.ammoStyle[i]);
                        }
                        if (LocalShotController.Instance.Weapons[i] != null)
                        {
                            if ((LocalShotController.Instance.Weapons[i].EnhacerModes & 1) == 1)
                            {
                                GUILayout.Label(GUIContent.none, "icoUseEnhDamage");
                                GUILayout.FlexibleSpace();
                            }
                            if ((LocalShotController.Instance.Weapons[i].EnhacerModes & 2) == 2)
                            {
                                GUILayout.Label(GUIContent.none, "icoUseEnhRapidity");
                            }
                        }
                        GUILayout.Label(GUIContent.none, GUIStyle.none);
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.Space(25f);
                    for (int i = 0; i < 2; i++)
                    {
                        if (LocalShotController.Instance.Enhancers[i] != null || (LocalShotController.Instance.Enhancers[i] != null && LocalShotController.Instance.Enhancers[i].Count > 0))
                        {
                            GUILayout.Label(LocalShotController.Instance.GetEnhancerCount(i).ToString(), this.enhancerStyle[i]);
                        }
                        else
                        {
                            GUILayout.Label(GUIContent.none, this.enhancerStyleEmpty);
                        }
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndArea();
                    GUILayout.BeginArea(new Rect((float)(Screen.width / 2 - 140), (float)(Screen.height - 60), 417f, 56f));
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    for (int i = 0; i < 5; i++)
                    {
                        GUILayout.BeginScrollView(new Vector2((float)LocalShotController.Instance.getWeaponReload(i) * 56f, 0f), GUILayout.Width(55f));
                        GUILayout.Label(GUIContent.none, "AmmoReload");
                        GUILayout.EndScrollView();
                    }
                    GUILayout.Space(25f);
                    for (int i = 0; i < 2; i++)
                    {
                        GUILayout.BeginScrollView(new Vector2((float)LocalShotController.Instance.getEnhancerReload(i) * 56f, 0f), GUILayout.Width(55f));
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
                    GUILayout.Label(TRInput.Weapon1.ToDisplayString(), (LocalShotController.Instance.CurrentWeapon.Index != 0) ? "hotkeyInfo" : "hotkeyInfoActive", GUILayout.MinWidth(20f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(56f));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(TRInput.Weapon2.ToDisplayString(), (LocalShotController.Instance.CurrentWeapon.Index != 1) ? "hotkeyInfo" : "hotkeyInfoActive", GUILayout.MinWidth(20f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(56f));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(TRInput.Weapon3.ToDisplayString(), (LocalShotController.Instance.CurrentWeapon.Index != 2) ? "hotkeyInfo" : "hotkeyInfoActive", GUILayout.MinWidth(20f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(56f));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(TRInput.Weapon4.ToDisplayString(), (LocalShotController.Instance.CurrentWeapon.Index != 3) ? "hotkeyInfo" : "hotkeyInfoActive", GUILayout.MinWidth(20f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(56f));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(TRInput.Weapon5.ToDisplayString(), (LocalShotController.Instance.CurrentWeapon.Index != 4) ? "hotkeyInfo" : "hotkeyInfoActive", GUILayout.MinWidth(20f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(25f);
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(56f));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(TRInput.Enhancer1.ToDisplayString(), (LocalShotController.Instance.Enhancers[0] == null) ? "hotkeyInfo" : "hotkeyInfoActive", GUILayout.MinWidth(20f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(56f));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(TRInput.Enhancer2.ToDisplayString(), (LocalShotController.Instance.Enhancers[1] == null) ? "hotkeyInfo" : "hotkeyInfoActive", GUILayout.MinWidth(20f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();
                    GUILayout.EndArea();
                }
                break;
        }
    }

    private void drawFrags()
    {
        if (LocalPlayerManager.GameScore != null)
        {
            GUILayout.BeginArea(new Rect((float)(Screen.width - 350), 5f, 350f, 300f), GUIStyle.none);
            Queue<FragKill>.Enumerator enumerator = LocalPlayerManager.GameScore.Frags.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    FragKill current = enumerator.Current;
                    short num = (short)current.KillerTeam;
                    short num2 = (short)current.KilledTeam;
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal(string.Empty, "FragLine");
                    switch (num)
                    {
                        case 1:
                            GUILayout.Label(current.KillerShowedName, "FragLineTxtRed");
                            break;
                        case 2:
                            GUILayout.Label(current.KillerShowedName, "FragLineTxtBlue");
                            break;
                        default:
                            GUILayout.Label(current.KillerShowedName, "FragLineTxtNeutral");
                            break;
                    }
                    if (current.KillerShowedName.Equals(current.KilledShowedName))
                    {
                        GUILayout.Label(GUIContent.none, "FragSkull");
                    }
                    else
                    {
                        if (current.WeaponType == WeaponType.NONE)
                        {
                            GUILayout.Label(GUIContent.none, "FragSkull");
                        }
                        else
                        {
                            GUILayout.Label(GUIContent.none, "frag_wt_" + ((int)current.WeaponType).ToString());
                        }
                        switch (num2)
                        {
                            case 1:
                                GUILayout.Label(current.KilledShowedName, "FragLineTxtRed");
                                break;
                            case 2:
                                GUILayout.Label(current.KilledShowedName, "FragLineTxtBlue");
                                break;
                            default:
                                GUILayout.Label(current.KilledShowedName, "FragLineTxtNeutral");
                                break;
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();
                    if (current.FType != 0)
                    {
                        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                        GUILayout.FlexibleSpace();
                        GUILayout.BeginHorizontal(string.Empty, "FragLine");
                        switch (num)
                        {
                            case 1:
                                GUILayout.Label(current.KillerShowedName, "FragLineTxtRed");
                                break;
                            case 2:
                                GUILayout.Label(current.KillerShowedName, "FragLineTxtBlue");
                                break;
                            default:
                                GUILayout.Label(current.KillerShowedName, "FragLineTxtNeutral");
                                break;
                        }
                        GUILayout.Label(GUIContent.none, "icoDomination");
                        if (current.FType == FragKill.FragType.Domination)
                        {
                            GUILayout.Label(LanguageManager.GetText("is DOMINATING"), "FragLineTxtNeutral");
                        }
                        else
                        {
                            GUILayout.Label(LanguageManager.GetText("got REVENGE on"), "FragLineTxtNeutral");
                        }
                        GUILayout.Space(2f);
                        switch (num2)
                        {
                            case 1:
                                GUILayout.Label(current.KilledShowedName, "FragLineTxtRed");
                                break;
                            case 2:
                                GUILayout.Label(current.KilledShowedName, "FragLineTxtBlue");
                                break;
                            default:
                                GUILayout.Label(current.KilledShowedName, "FragLineTxtNeutral");
                                break;
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
        GUILayout.FlexibleSpace();
        GUILayout.Label(stringReward, "scoreTitle", GUILayout.Width(80f));
        GUILayout.Label(this.reward.ToString(), "scoreRewardBlue");
        GUILayout.Label(LanguageManager.GetText("Credits"), "scoreTitle", GUILayout.Width(60f));
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void drawTeamScore(int xs, int ys, int team)
    {
        GameScore gameScore = LocalPlayerManager.GameScore;
        GUILayout.BeginArea(new Rect((float)xs, (float)ys, 52f, 52f), GUIContent.none, "Team_Score");
        if (team <= 1)
        {
            GUILayout.Label(string.Empty + gameScore.GetTeamPoints(1), "scoreTeamRed");
        }
        else
        {
            GUILayout.Label(string.Empty + gameScore.GetTeamPoints(2), "scoreTeamBlue");
        }
        GUILayout.EndArea();
    }

    private void draw_menu()
    {
        GUILayout.BeginArea(new Rect(((float)Screen.width - 226f) / 2f, ((float)Screen.height - 194f) / 2f, 226f, 194f), string.Empty, "menuLayer");
        if (GUILayout.Button(LanguageManager.GetText("RESUME GAME"), GUILayout.Width(206f), GUILayout.Height(37f)))
        {
            LocalGameHUD.isShowMenu = false;
            this.LockAndHideCursor();
        }
        if (GUILayout.Button(LanguageManager.GetText("FULLSCREEN"), GUILayout.Width(206f), GUILayout.Height(37f)))
        {
            LocalGameHUD.isShowMenu = false;
            this.LockAndHideCursor();
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
        if (!GUILayout.Button(LanguageManager.GetText("OPTIONS"), GUILayout.Width(206f), GUILayout.Height(37f)))
        {
            goto IL_011b;
        }
        goto IL_011b;
        IL_011b:
        if (GUILayout.Button(LanguageManager.GetText("LEAVE BATTLE"), GUILayout.Width(206f), GUILayout.Height(37f)))
        {
            this.ExitToMenu();
        }
        GUILayout.EndArea();
    }

    public void Zoom(bool on)
    {
        Transform transform = this.CrossHairs.FindChild("CrossHair");
        if (on)
        {
            this.zoomOverlay.material.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                ((Component)transform).GetComponent<Renderer>().enabled = false;
            }
        }
        else
        {
            this.zoomOverlay.material.SetColor("_Color", new Color(1f, 1f, 1f, 0f));
            if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
            {
                ((Component)transform).GetComponent<Renderer>().enabled = true;
            }
        }
    }

    private void Update()
    {
        if (this.bloodOverlayAmount > 0f)
        {
            this.bloodOverlay.material.SetColor("_TintColor", new Color(0.31f, 0.31f, 0.31f, this.bloodOverlayAmount));
            this.bloodOverlayAmount -= Time.deltaTime;
        }
        else
        {
            this.bloodOverlay.material.SetColor("_TintColor", new Color(0.31f, 0.31f, 0.31f, 0f));
        }
        this.showScore = UnityEngine.Input.GetKey("tab");
        if (Input.GetButtonUp("Escape") || UnityEngine.Input.GetKeyUp(KeyCode.Escape))
        {
            LocalGameHUD.isShowMenu = true;
            Cursor.visible = true;
            Screen.lockCursor = false;
            if (Screen.fullScreen)
            {
                Screen.fullScreen = false;
            }
        }
        else if (UnityEngine.Input.GetKeyUp(KeyCode.F10))
        {
            LocalGameHUD.isShowMenu = !LocalGameHUD.isShowMenu;
            Cursor.visible = LocalGameHUD.isShowMenu;
            Screen.lockCursor = !LocalGameHUD.isShowMenu;
        }
        if (this.PlayerState == PlayerStates.DeadReady && Input.GetMouseButtonDown(0))
        {
            this.PlayerState = PlayerStates.Load;
            LocalPlayerManager.Instance.SpawnMe();
        }
        if (UnityEngine.Input.GetKey(KeyCode.F))
        {
            LocalGameHUD.isShowMenu = false;
            this.LockAndHideCursor();
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
    }

    public void Play()
    {
        if (this.PlayerState == PlayerStates.Load)
        {
            this.PlayerState = PlayerStates.Play;
        }
    }

    public void ShotMe()
    {
        this.bloodOverlayAmount = 1f;
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
        targetPointer = gameObject.GetComponent<TargetPointer>();
        targetPointer.Me = me;
        targetPointer.Target = enemy;
        targetPointer.TargetID = enemyID;
        gameObject.transform.parent = this.bloodOverlay.transform.parent;
        gameObject.transform.localPosition = new Vector3(0f, 0f, 2f);
        gameObject.transform.localRotation = new Quaternion(0f, 0f, 2f, 1f);
    }

    public void UpdateHealth()
    {
        int health = LocalPlayerManager.Instance.LocalPlayer.Health;
        if (LocalPlayerManager.Instance.LocalPlayer.IsDead && this.PlayerState == PlayerStates.Play)
        {
            this.PlayerState = PlayerStates.Dead;
            base.StartCoroutine(this.SetDeadState());
        }
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
        }
    }

    private IEnumerator SetTimeOverState()
    {
        yield return (object)new WaitForSeconds(this.timeOverDelay);
        this.PlayerState = PlayerStates.TimeOverReady;
    }

    public void ExitToMenu()
    {
        this.PlayerState = PlayerStates.Load;
        LocalGameHUD.isShowMenu = false;
        Cursor.visible = true;
        Screen.lockCursor = false;
        OptionsManager.RoomSetting = null;
        UnityEngine.SceneManagement.SceneManager.LoadScene("title");
        GC.Collect();
    }
}


