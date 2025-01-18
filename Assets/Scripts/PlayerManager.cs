// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    private const long KillSeriesPeriod = 10000L;

    private const long PING_SEND_PERIOD = 20000000L;

    private const long LAG_SEND_PERIOD = 50000000L;

    private const float POS_ADD = 0.0001f;

    public GameObject enemyPrefab;

    public GameObject playerPrefab;

    public GameObject starshipExplodePrefab;

    public GameObject ammoKitPrefab;

    public GameObject ammoKitSmallPrefab;

    public GameObject healthKitPrefab;

    public GameObject healthKitSmallPrefab;

    public GameObject armorKitPrefab;

    public GameObject armorKitSmallPrefab;

    public GameObject bloodPrefab;

    public GameObject flagPrefab;

    public Material flagRed;

    public Material flagBlue;

    public Material flagCaptureRed;

    public Material flagCaptureBlue;

    public Material controlPointNeutral;

    public Material controlPointRed;

    public Material controlPointBlue;

    public GameObject damageInfoPrefab;

    public GameObject bot1Prefab;

    public GameObject bot2Prefab;

    public GameObject bot3Prefab;

    public GameObject escortMachineRedPrefab;

    public GameObject escortMachineBluePrefab;

    public long lastKill;

    public int killSeries;

    public PlayerHitZone lastKillZone;

    public bool hasKills;

    public Material SmokeMaterial;

    public bool water;

    public bool WaterBlock;

    private Camera freeCamera;

    private Camera localCamera;

    private CombatPlayer previousCameraPlayer;

    public int fogDensity = 1500;

    private Dictionary<int, EscortMachine> escortMachines;

    private static PlayerManager instance;

    private EffectManager effectManager;

    private Dictionary<int, GameObject> items = new Dictionary<int, GameObject>();

    public Dictionary<int, CombatPlayer> Players;

    public CombatPlayer LocalPlayer;

    private Campaign campaign;

    private FlagPoint[] flags;

    private Transform[] cpoints;

    private GameObject[] gates;

    private int cpointc;

    private MyRoomSetting roomSettings;

    private GameScore gameScore;

    private bool isInit;

    private byte zombieMode;

    private long streakPeriod = 40000000L;

    private long lastStreak = DateTime.Now.Ticks;

    private int streakCounter = 1;

    private CombatPlayer dummyPlayer;

    private GameObject displayObject;

    private long lastTCPTransformSend = DateTime.Now.Ticks;

    private long lastPingSend = DateTime.Now.Ticks;

    private long lastLagSend = DateTime.Now.Ticks;

    private float lastPosAdd = 0.0001f;

    private PhotonConnection photonConnection;

    public static PlayerManager Instance
    {
        get
        {
            return PlayerManager.instance;
        }
    }

    public Camera ActiveCamera
    {
        get
        {
            if (this.localCamera.gameObject.activeSelf)
            {
                return this.localCamera;
            }
            return this.freeCamera;
        }
    }

    private EffectManager EffectManager
    {
        get
        {
            if ((UnityEngine.Object)this.effectManager == (UnityEngine.Object)null)
            {
                this.effectManager = ((Component)base.transform).GetComponent<EffectManager>();
            }
            return this.effectManager;
        }
    }

    public Campaign Campaign
    {
        get
        {
            return this.campaign;
        }
    }

    public static GameScore GameScore
    {
        get
        {
            return PlayerManager.Instance.gameScore;
        }
    }

    public bool IsInit
    {
        get
        {
            return this.isInit;
        }
    }

    public MyRoomSetting RoomSettings
    {
        get
        {
            return this.roomSettings;
        }
    }

    public byte ZombieMode
    {
        get
        {
            return this.zombieMode;
        }
    }

    public GameObject DisplayObject
    {
        get
        {
            return this.displayObject;
        }
    }

    private void Awake()
    {
		//flagCaptureRed.shader = Shader.Find("Legacy Shaders/Diffuse");
		//flagCaptureBlue.shader = Shader.Find("Legacy Shaders/Diffuse");
		//controlPointNeutral.shader = Shader.Find("Legacy Shaders/Diffuse");
		//controlPointBlue.shader = Shader.Find("Legacy Shaders/Diffuse");
		//controlPointRed.shader = Shader.Find("Legacy Shaders/Diffuse");
        PlayerManager.instance = this;
        base.gameObject.AddComponent<KickManager>();
    }

    public void Init(Hashtable joinData)
    {
        Lagometer.Restart();
        this.lastKill = TimeManager.Instance.NetworkTime - 10000;
        this.freeCamera = ((Component)base.transform).GetComponentInChildren<Camera>();
        this.freeCamera.cullingMask = (this.freeCamera.cullingMask & -2049);
        Hashtable data = (Hashtable)joinData[(byte)100];
        this.roomSettings = new MyRoomSetting(data);
        this.gameScore = new GameScore(this.roomSettings.GameMode);
        this.Players = new Dictionary<int, CombatPlayer>();
        Hashtable hashtable = (Hashtable)joinData[(byte)99];
        Hashtable hashtable2 = (Hashtable)joinData[(byte)98];
        int actorID = (int)joinData[(byte)97];
        if (hashtable2 != null && hashtable2.ContainsKey((byte)96))
        {
            this.InitMe(actorID, hashtable2);
        }
        if (hashtable != null)
        {
            foreach (object key in hashtable.Keys)
            {
                this.InitEnemy((int)key, (Hashtable)hashtable[key], false);
            }
        }
        this.InitControlPoints(this.roomSettings.GameMode == MapMode.MODE.CONTROL_POINTS);
        this.InitFlagPoints(this.roomSettings.GameMode == MapMode.MODE.CAPTURE_THE_FLAG);
        this.InitGates(this.roomSettings.GameMode != MapMode.MODE.DEATHMATCH && this.roomSettings.GameMode != MapMode.MODE.TOWER_DEFENSE);
        this.InitWater();
        this.InitBots(this.roomSettings.GameMode == MapMode.MODE.TOWER_DEFENSE);
        this.InitEscort(this.roomSettings.GameMode == MapMode.MODE.ESCORT);
        if (NetworkDev.CheckAim)
        {
            RTTXCamera.Init();
        }
        UnityEngine.Debug.Log("[Player Manager] Init: GameMode = " + this.roomSettings.GameMode + " startTime=" + this.RoomSettings.StartTime);
    }

    public void InitBots(bool init)
    {
        if (init)
        {
            this.campaign = base.gameObject.AddComponent<Campaign>();
        }
    }

    public void ProcessCampaignEvent(Hashtable campaignEventData)
    {
        switch ((byte)campaignEventData[(byte)72])
        {
            case 2:
                break;
            case 1:
            {
                Hashtable actionData = (Hashtable)campaignEventData[(byte)71];
                this.SpawnBot(actionData);
                break;
            }
            case 3:
            {
                Hashtable actionData = (Hashtable)campaignEventData[(byte)71];
                int key = (int)campaignEventData[(byte)97];
                WeaponType weaponType = (WeaponType)(byte)campaignEventData[(byte)91];
                ActionType actionType = (ActionType)(byte)actionData[(byte)27];
                PlayerManager.GameScore.AddFrag(new FragKill(this.Players[key].AuthID, this.Players[key].Name, this.Players[key].ClanTag, 0, 0, actionType.ToString(), string.Empty, 0, weaponType, FragKill.FragType.None));
                if (this.LocalPlayer.AuthID == this.Players[key].AuthID)
                {
                    SoundManager.Instance.Play(this.LocalPlayer.Audio, "TowerDefense/bot death", AudioPlayMode.Play);
                }
                this.FinishBot(actionData);
                this.campaign.EnemiesCount--;
                break;
            }
            case 4:
            {
                this.campaign.Progress = (int)campaignEventData[(byte)69];
                Hashtable actionData = (Hashtable)campaignEventData[(byte)71];
                this.campaign.EnemiesCount--;
                this.FinishBot(actionData);
                this.CoreUnderAttack();
                break;
            }
            case 5:
                this.campaign.WaveIndex = (short)campaignEventData[(byte)67];
                this.campaign.EnemiesCount = (int)campaignEventData[(byte)66];
                this.NewWave(this.campaign.WaveIndex, this.campaign.EnemiesCount);
                break;
            case 6:
                this.campaign.WaveIndex = (short)campaignEventData[(byte)67];
                this.FinishWave(this.campaign.WaveIndex);
                break;
        }
    }

    public void TestEnemyAnimation()
    {
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator = this.Players.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CombatPlayer current = enumerator.Current;
                if (!((UnityEngine.Object)current == (UnityEngine.Object)this.LocalPlayer))
                {
                    current.ActorAnimator.TestAnimation();
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    private void NewWave(short waveIndex, int actorCount)
    {
        GameHUD.Instance.Message(GameHUDMessageType.TOWER_NEW_WAVE, waveIndex);
        SoundManager.Instance.Play(this.LocalPlayer.Audio, "TowerDefense/wave is coming", AudioPlayMode.Play);
    }

    private void FinishWave(short waveIndex)
    {
        GameHUD.Instance.Message(GameHUDMessageType.TOWER_FINISH_WAVE, waveIndex);
        SoundManager.Instance.Play(this.LocalPlayer.Audio, "TowerDefense/wave is over", AudioPlayMode.Play);
        UnityEngine.Debug.Log("/FINISH WAVE: " + waveIndex);
    }

    private void CoreUnderAttack()
    {
        GameHUD.Instance.Message(GameHUDMessageType.TOWER_CORE_ATTACK);
        SoundManager.Instance.Play(this.LocalPlayer.Audio, "TowerDefense/bot hits generator", AudioPlayMode.Play);
        UnityEngine.Debug.Log("/CORE UNDER ATTACK!");
    }

    public int GetCampaignProgress()
    {
        if ((UnityEngine.Object)this.campaign == (UnityEngine.Object)null)
        {
            return 100;
        }
        return this.campaign.Progress;
    }

    public void ResetCampaign()
    {
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator = this.campaign.Actors.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CombatPlayer current = enumerator.Current;
                if ((UnityEngine.Object)current != (UnityEngine.Object)null)
                {
                    UnityEngine.Object.Destroy(current.gameObject);
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        this.campaign.Actors.Clear();
    }

    public void FinishBot(Hashtable actionData)
    {
        ActionType actionType = (ActionType)(byte)actionData[(byte)27];
        short num = (short)actionData[(byte)239];
        int key = (int)actionData[(byte)26];
        if (this.campaign.Actors.ContainsKey(key))
        {
            CombatPlayer combatPlayer = this.campaign.Actors[key];
            MonsterBotNavigationController component = ((Component)combatPlayer.transform).GetComponent<MonsterBotNavigationController>();
            component.Blow();
            this.campaign.Actors.Remove(key);
        }
    }

    public void SpawnBot(Hashtable actionData)
    {
        ActionType actionType = (ActionType)(byte)actionData[(byte)27];
        short num = (short)actionData[(byte)239];
        int num2 = (int)actionData[(byte)26];
        long launchTime = (long)actionData[(byte)25];
        long landingTime = (long)actionData[(byte)24];
        int num3 = (int)actionData[(byte)23];
        GameObject gameObject = null;
        switch (actionType)
        {
            default:
                return;
            case ActionType.BOT_L1:
                gameObject = this.bot1Prefab;
                break;
            case ActionType.BOT_L2:
                gameObject = this.bot2Prefab;
                break;
            case ActionType.BOT_L3:
                gameObject = this.bot3Prefab;
                break;
        }
        GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
        MonsterBotNavigationController component = ((Component)gameObject2.transform).GetComponent<MonsterBotNavigationController>();
        CombatPlayer component2 = ((Component)gameObject2.transform).GetComponent<CombatPlayer>();
        this.campaign.Actors.Add(num2, component2);
        component2.playerID = num2;
        System.Random random = new System.Random();
        int num4 = random.Next(1, 3);
        Transform transform = base.transform.Find("BotTrajectory" + num3);
        int num5 = 1;
        Transform transform2 = transform.Find("Point" + num5);
        while ((UnityEngine.Object)transform2 != (UnityEngine.Object)null)
        {
            component.Trajectory.AddNode(transform2.position);
            num5++;
            transform2 = transform.Find("Point" + num5);
        }
        component.Launch(launchTime, landingTime);
    }

    public float GetEscortProgress(short team)
    {
        if (this.escortMachines != null && this.escortMachines.ContainsKey(team - 1))
        {
            EscortMachine escortMachine = this.escortMachines[team - 1];
            if ((UnityEngine.Object)escortMachine == (UnityEngine.Object)null)
            {
                return 0f;
            }
            return escortMachine.GetProgress() * 100f;
        }
        return 0f;
    }

    public void InitEscort(bool on)
    {
        this.escortMachines = new Dictionary<int, EscortMachine>();
    }

    public void SetupEscort(Hashtable escortData)
    {
        foreach (object key in escortData.Keys)
        {
            Hashtable escortMachineData = (Hashtable)escortData[key];
            this.SpawnEscortMachine(escortMachineData);
        }
    }

    public CombatPlayer ProcessAnimationEvent(int actorID, Hashtable data)
    {
        CombatPlayer combatPlayer = null;
        if (this.Players.ContainsKey(actorID))
        {
            combatPlayer = this.Players[actorID];
            if ((UnityEngine.Object)this.LocalPlayer != (UnityEngine.Object)null && actorID == this.LocalPlayer.playerID)
            {
                UnityEngine.Debug.LogError("[PlayerManager] Animation Event LEAK!!!");
                return null;
            }
            if (data.ContainsKey((byte)55))
            {
                int num = (int)data[(byte)55];
                if (!combatPlayer.IsDead)
                {
                    combatPlayer.WeaponController.LaunchTaunt(combatPlayer, num);
                    if (combatPlayer.IsZombie)
                    {
                        this.EffectManager.PlayerSoundEffect(combatPlayer.Audio, "ZombieSound_3");
                    }
                    else
                    {
                        this.EffectManager.TauntEffect(combatPlayer.Audio, string.Format("Taunt{0}", num));
                    }
                }
                return null;
            }
            if (!data.ContainsKey((byte)58))
            {
                UnityEngine.Debug.LogError("[PlayerManager] Animation with NO timestamp!!!");
                return null;
            }
            long timeStamp = (long)data[(byte)58];
            if (data.ContainsKey((byte)59))
            {
                combatPlayer.NetworkTransformReceiver.ReceiveAnimationKey((byte)data[(byte)59], timeStamp);
                if (Datameter.enabled)
                {
                    Datameter.AnimationCounter++;
                    Datameter.AnimationSizeCounter += (float)PlayerManager.GetObjectSize(data);
                }
            }
            if (data.ContainsKey((byte)60))
            {
                combatPlayer.NetworkTransformReceiver.ReceiveAnimationState((byte)data[(byte)60], timeStamp);
            }
            return combatPlayer;
        }
        return null;
    }

    public void ProcessEscortEvent(Hashtable escortEventData)
    {
        if (this.escortMachines != null && this.escortMachines.Count >= 2)
        {
            EscortMachineEventType escortMachineEventType = (EscortMachineEventType)(byte)escortEventData[(byte)63];
            Hashtable hashtable = (Hashtable)escortEventData[(byte)62];
            switch (escortMachineEventType)
            {
                case EscortMachineEventType.Move:
                {
                    short key = (short)hashtable[(byte)13];
                    if (this.escortMachines.ContainsKey(key))
                    {
                        EscortMachine escortMachine = this.escortMachines[key];
                        float progress = (float)hashtable[(byte)11];
                        escortMachine.Move(progress);
                    }
                    break;
                }
                case EscortMachineEventType.Finish:
                {
                    UnityEngine.Debug.Log("MAchine Finish");
                    short key = (short)hashtable[(byte)13];
                    if (this.escortMachines.ContainsKey(key))
                    {
                        EscortMachine escortMachine = this.escortMachines[key];
                        float progress = (float)hashtable[(byte)11];
                        escortMachine.Move(progress);
                    }
                    break;
                }
                case EscortMachineEventType.Reset:
                {
                    UnityEngine.Debug.Log("MAchine Reset");
                    short key = (short)hashtable[(byte)13];
                    if (this.escortMachines.ContainsKey(key))
                    {
                        EscortMachine escortMachine = this.escortMachines[key];
                        float progress = (float)hashtable[(byte)11];
                        escortMachine.Reset(progress);
                    }
                    break;
                }
            }
        }
    }

    public void SpawnEscortMachine(Hashtable escortMachineData)
    {
        short num = (short)escortMachineData[(byte)13];
        GameObject gameObject = null;
        int num2 = num + 1;
        switch (num2)
        {
            default:
                return;
            case 1:
                gameObject = this.escortMachineRedPrefab;
                break;
            case 2:
                gameObject = this.escortMachineBluePrefab;
                break;
        }
        GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
        EscortMachine escortMachine = gameObject2.AddComponent<EscortMachine>();
        this.escortMachines.Add(num, escortMachine);
        Transform transform = base.transform.Find("BotTrajectory" + num2);
        Trajectory trajectory = new Trajectory();
        int num3 = 1;
        Transform transform2 = transform.Find("Point" + num3);
        while ((UnityEngine.Object)transform2 != (UnityEngine.Object)null)
        {
            trajectory.AddNode(transform2.position);
            num3++;
            transform2 = transform.Find("Point" + num3);
        }
        escortMachine.SetupEscortMachine(escortMachineData, trajectory);
    }

    public void InitControlPoints(bool on)
    {
        int i = 1;
        this.cpoints = new Transform[5];
        for (; (bool)base.transform.Find("finish4").Find("ControlPoint" + i); i++)
        {
            this.cpoints[i - 1] = base.transform.Find("finish4").Find("ControlPoint" + i).Find("ControlPoint");
            if (this.roomSettings.GameMode == MapMode.MODE.CONTROL_POINTS)
            {
                PlayerManager.GameScore.ControlPoints.Add(new ScoreControlPoint(0, 0));
            }
            if (!on)
            {
                ((Component)this.cpoints[i - 1]).GetComponent<Renderer>().enabled = false;
                this.cpoints[i - 1].gameObject.SetActive(false);
                this.SetPointOn(this.cpoints[i - 1], false);
            }
        }
    }

    public void InitFlagPoints(bool on)
    {
        int i = 1;
        this.flags = new FlagPoint[2];
        for (; (bool)base.transform.Find("finish4").Find("FlagPoint" + i); i++)
        {
            Transform transform = base.transform.Find("finish4").Find("FlagPoint" + i);
            if (!on)
            {
                ((Component)transform).GetComponent<Renderer>().enabled = false;
                transform.gameObject.SetActive(false);
            }
        }
    }

    public void InitWater()
    {
        Transform transform = base.transform.FindChild("finish4").FindChild("Water");
        if ((UnityEngine.Object)transform != (UnityEngine.Object)null)
        {
            transform.gameObject.SetActive(true);
            for (int i = 0; i < transform.GetChildCount(); i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            this.water = true;
        }
        else
        {
            this.water = false;
        }
    }

    public void InitGates(bool on)
    {
        for (int i = 1; (bool)base.transform.Find("finish4").Find("GatesRed" + i); i++)
        {
            Transform transform = base.transform.Find("finish4").Find("GatesRed" + i);
            if (!on)
            {
                ((Component)transform).GetComponent<Renderer>().enabled = false;
                transform.gameObject.SetActive(false);
            }
        }
        for (int i = 1; (bool)base.transform.Find("finish4").Find("GatesBlue" + i); i++)
        {
            Transform transform = base.transform.Find("finish4").Find("GatesBlue" + i);
            if (!on)
            {
                ((Component)transform).GetComponent<Renderer>().enabled = false;
                transform.gameObject.SetActive(false);
            }
        }
        for (int i = 1; (bool)base.transform.Find("finish4").Find("GatesRedTrigger" + i); i++)
        {
            Transform transform = base.transform.Find("finish4").Find("GatesRedTrigger" + i);
            if (!on)
            {
                transform.gameObject.SetActive(false);
            }
        }
        for (int i = 1; (bool)base.transform.Find("finish4").Find("GatesBlueTrigger" + i); i++)
        {
            Transform transform = base.transform.Find("finish4").Find("GatesBlueTrigger" + i);
            if (!on)
            {
                transform.gameObject.SetActive(false);
            }
        }
    }

    public void InitEnvironment()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.5137255f, 0.6313726f, 0.7490196f);
        RenderSettings.fogDensity = 0.001f;
    }

    public void SetGameState(Hashtable gameStateData)
    {
        Hashtable hashtable = (Hashtable)gameStateData[(byte)98];
        if (hashtable != null)
        {
            int actorID = (int)gameStateData[(byte)97];
            this.InitMe(actorID, hashtable);
            Hashtable hashtable2 = (Hashtable)gameStateData[(byte)99];
            if (hashtable2 != null)
            {
                foreach (object key in hashtable2.Keys)
                {
                    this.InitEnemy((int)key, (Hashtable)hashtable2[key], false);
                }
            }
        }
        Hashtable hashtable3 = (Hashtable)gameStateData[(byte)80];
        Hashtable hashtable4 = (Hashtable)gameStateData[(byte)88];
        Hashtable hashtable5 = (Hashtable)gameStateData[(byte)74];
        Hashtable hashtable6 = (Hashtable)gameStateData[(byte)57];
        Hashtable hashtable7 = (Hashtable)gameStateData[(byte)79];
        Hashtable hashtable8 = (Hashtable)gameStateData[(byte)78];
        Hashtable hashtable9 = (Hashtable)gameStateData[(byte)70];
        Hashtable hashtable10 = null;
        if (gameStateData.ContainsKey((byte)61))
        {
            hashtable10 = (Hashtable)gameStateData[(byte)61];
        }
        this.roomSettings.GamePause = (bool)gameStateData[(byte)77];
        this.roomSettings.StartTime = (long)gameStateData[(byte)95];
        if (hashtable3 != null)
        {
            foreach (object key2 in hashtable3.Keys)
            {
                this.SpawnItem((Hashtable)hashtable3[key2]);
            }
        }
        if (hashtable4 != null)
        {
            this.UpdateScore(hashtable4, true);
        }
        if (hashtable7 != null)
        {
            this.SetupFlags(hashtable7);
        }
        if (hashtable8 != null)
        {
            this.SetupControlPoints(hashtable8);
        }
        this.InitBots(this.roomSettings.GameMode == MapMode.MODE.TOWER_DEFENSE);
        if (hashtable9 != null)
        {
            this.SetupCampaign(hashtable9);
        }
        if (hashtable10 != null)
        {
            this.SetupEscort(hashtable10);
        }
        if (this.roomSettings.GamePause)
        {
            GameHUD.Instance.TimeOver();
            Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator3 = this.Players.Values.GetEnumerator();
            try
            {
                while (enumerator3.MoveNext())
                {
                    CombatPlayer current3 = enumerator3.Current;
                    current3.Kill();
                    if (PlayerManager.GameScore.ContainsUser(current3.AuthID))
                    {
                        PlayerManager.GameScore[current3.AuthID].IsDead = true;
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator3).Dispose();
            }
            GameHUD.Instance.PlayerState = GameHUD.PlayerStates.TimeOver;
        }
        else
        {
            if (hashtable5 != null)
            {
                this.SetupPlayerItems(hashtable5);
            }
            if (hashtable6 != null)
            {
                this.SetupPlayerAnimations(hashtable6);
            }
            if (gameStateData.ContainsKey((byte)51))
            {
                this.UpdateZombie(-1, (Hashtable)gameStateData[(byte)51]);
            }
            else
            {
                GameHUD.Instance.Play();
            }
        }
        LoadingMapPopup.Complete();
        GameHUD.HideCursor();
    }

    private void SetupCampaign(Hashtable campaignData)
    {
        foreach (object key in campaignData.Keys)
        {
            Hashtable actionData = (Hashtable)campaignData[key];
            this.SpawnBot(actionData);
        }
    }

    private void SetupPlayerItems(Hashtable playerItemData)
    {
        foreach (object key in playerItemData.Keys)
        {
            Dictionary<long, Hashtable> dictionary = (Dictionary<long, Hashtable>)playerItemData[key];
            int actorID = (int)key;
            Dictionary<long, Hashtable>.ValueCollection.Enumerator enumerator2 = dictionary.Values.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    Hashtable current2 = enumerator2.Current;
                    this.SpawnPlayerItem(actorID, current2);
                }
            }
            finally
            {
                ((IDisposable)enumerator2).Dispose();
            }
        }
    }

    private void SetupPlayerAnimations(Hashtable playerAnimationData)
    {
        foreach (object key in playerAnimationData.Keys)
        {
            Hashtable hashtable = (Hashtable)playerAnimationData[key];
            int num = (int)key;
            CombatPlayer combatPlayer = null;
            if (this.Players.ContainsKey(num))
            {
                combatPlayer = this.Players[num];
                if ((UnityEngine.Object)this.LocalPlayer != (UnityEngine.Object)null && num == this.LocalPlayer.playerID)
                {
                    UnityEngine.Debug.LogError("[PlayerManager] Animation LEAK!!!");
                }
                else if (!combatPlayer.IsDead)
                {
                    if (hashtable.ContainsKey((byte)58))
                    {
                        long timeStamp = (long)hashtable[(byte)58];
                        if (hashtable.ContainsKey((byte)59))
                        {
                            combatPlayer.NetworkTransformReceiver.ReceiveAnimationKey((byte)hashtable[(byte)59], timeStamp);
                            combatPlayer.SetAnimationKey((byte)hashtable[(byte)59]);
                        }
                        if (hashtable.ContainsKey((byte)60))
                        {
                            combatPlayer.NetworkTransformReceiver.ReceiveAnimationState((byte)hashtable[(byte)60], timeStamp);
                            combatPlayer.SetAnimationState((byte)hashtable[(byte)60]);
                            if (Datameter.enabled)
                            {
                                Datameter.JumpCrouchCounter++;
                            }
                        }
                        int weaponNum = (int)hashtable[(byte)56];
                        combatPlayer.WeaponController.OnChangeWeapon(weaponNum);
                        ActorAnimator actorAnimator = combatPlayer.ActorAnimator;
                        if (!((UnityEngine.Object)actorAnimator == (UnityEngine.Object)null))
                        {
                            if (!combatPlayer.IsZombie)
                            {
                                WeaponType type = combatPlayer.WeaponController.CurrentWeapon.Type;
                                this.ChangeWeaponAnimation(actorAnimator, type, combatPlayer.WeaponController.GetWeaponByType((int)type).SystemName);
                            }
                            continue;
                        }
                        break;
                    }
                    UnityEngine.Debug.LogError("[PlayerManager] Animation with NO timestamp!!!");
                }
            }
        }
    }

    public void InitMe(int actorID, Hashtable actorData)
    {
        if ((UnityEngine.Object)this.LocalPlayer != (UnityEngine.Object)null)
        {
            UnityEngine.Debug.LogError("[PlayerManager] Init Me Second Time!!!");
        }
        Hashtable hashtable = (Hashtable)actorData[(byte)96];
        bool flag = false;
        if (hashtable.ContainsKey((byte)243))
        {
            flag = (bool)hashtable[(byte)243];
        }
        if (flag)
        {
            try
            {
                LocalUser.NeedLiteRefresh = false;
                AchievementManager.NeedLiteRefresh = false;
                StatisticManager.NeedLiteRefresh = false;
                SavePopup.Init(LanguageManager.GetText("РџСЂРѕРёСЃС…РѕРґРёС‚ СЃРѕС…СЂР°РЅРµРЅРёРµ..."));
            }
            catch (Exception message)
            {
                UnityEngine.Debug.LogError(message);
            }
        }
        else
        {
            try
            {
                LocalUser.NeedLiteRefresh = true;
                AchievementManager.NeedLiteRefresh = true;
                StatisticManager.NeedLiteRefresh = true;
            }
            catch (Exception message2)
            {
                UnityEngine.Debug.LogError(message2);
            }
        }
        GameObject gameObject = UnityEngine.Object.Instantiate(CharacterManager.Instance.GetPlayer());
        GameObject gameObject2 = gameObject.transform.FindChild("Fighter").gameObject;
        this.LocalPlayer = gameObject2.AddComponent<CombatPlayer>();
        this.LocalPlayer.Team = -1;
        this.LocalPlayer.Init(actorID, actorData, this.LocalPlayer.SoldierController.FPSCamera.gameObject.GetComponent<AudioSource>());
        this.LocalPlayer.WalkController.setSpeed(this.LocalPlayer.Speed, this.LocalPlayer.Jump);
        this.Players.Add(actorID, this.LocalPlayer);
        this.localCamera = this.LocalPlayer.Camera;
        this.localCamera.gameObject.SetActive(false);
        this.LocalPlayer.WalkController.enabled = false;
        this.LocalPlayer.ShotController.enabled = false;
        CharacterMotor component = ((Component)this.LocalPlayer.transform).GetComponent<CharacterMotor>();
        component.movement.maxGroundAcceleration = (float)(this.LocalPlayer.Speed * 20);
        SoldierController soldierController = this.LocalPlayer.SoldierController;
        soldierController.runSpeed = (soldierController.runStrafeSpeed = (float)this.LocalPlayer.Speed);
        this.LocalPlayer.InitSecurity(true);
        GameHUD.Instance.SetRespawnDelay(this.LocalPlayer.IsPremium);
    }

    public void InitEnemy(int actorID, Hashtable actorData, bool async)
    {
        if (!((UnityEngine.Object)this.LocalPlayer == (UnityEngine.Object)null) && actorData.ContainsKey((byte)96))
        {
            if (actorID == this.LocalPlayer.playerID)
            {
                UnityEngine.Debug.LogError("[PlayerManager] Init Me As Enemy Leak!!!");
            }
            else if (async)
            {
                CharacterManager.Instance.GetPlayerEnemyAsync(new CharacterManager.OnCharacterAssetLoaded(this.OnEnemyAssetLoaded), actorData, actorID);
            }
            else
            {
                this.OnEnemyAssetLoaded(CharacterManager.Instance.GetPlayerEnemy(), actorData, actorID);
            }
        }
    }

    private void OnEnemyAssetLoaded(GameObject characterObject, Hashtable actorData, int actorID)
    {
        Hashtable hashtable = (Hashtable)actorData[(byte)96];
        GameObject gameObject = UnityEngine.Object.Instantiate(characterObject);
        CombatPlayer combatPlayer = gameObject.AddComponent<CombatPlayer>();
        combatPlayer.Init(actorID, actorData, combatPlayer.gameObject.GetComponent<AudioSource>());
        combatPlayer.AnimationSynchronizer.StartReceivingAnimation();
        combatPlayer.InitEnemyInfo();
        this.Players.Add(actorID, combatPlayer);
    }

    public void UpdateScore(Hashtable scoreData, bool init)
    {
        Hashtable hashtable = null;
        Hashtable hashtable2 = null;
        CombatPlayer combatPlayer = null;
        int reward = (int)scoreData[(byte)89];
        if (scoreData.ContainsKey((byte)88))
        {
            hashtable = (Hashtable)scoreData[(byte)88];
            foreach (int key in hashtable.Keys)
            {
                Hashtable hashtable3 = (Hashtable)hashtable[key];
                if (this.Players.ContainsKey(key))
                {
                    combatPlayer = this.Players[key];
                }
                else
                {
                    UnityEngine.Debug.LogError("[PlayerManager] UpdateScore Player Not Found on Players ActorID:" + key);
                }
                if (init)
                {
                    short num2 = (short)hashtable3[(byte)239];
                    if (num2 >= 0 && (UnityEngine.Object)combatPlayer != (UnityEngine.Object)null)
                    {
                        this.gameScore.AddUser(combatPlayer.AuthID, combatPlayer.Name, combatPlayer.Level, combatPlayer.IsPremium, num2, combatPlayer.WeaponInfo, combatPlayer.ClanArmId, combatPlayer.ClanTag);
                    }
                    if (hashtable3.ContainsKey((byte)101) && GameHUD.Instance.PlayerState != GameHUD.PlayerStates.TimeOver)
                    {
                        NetworkTransform ntransform = NetworkTransform.FromHashtable((Hashtable)hashtable3[(byte)237]);
                        ZombieType zombieType = ZombieType.Human;
                        if (hashtable3.ContainsKey((byte)10))
                        {
                            zombieType = (ZombieType)(byte)hashtable3[(byte)10];
                        }
                        if (this.LocalPlayer.playerID != key)
                        {
                            this.SpawnEnemy(key, ntransform, num2, false, zombieType);
                        }
                        else if (this.RoomSettings.GameMode == MapMode.MODE.ZOMBIE && this.LocalPlayer.IsDead)
                        {
                            int health = (int)hashtable3[(byte)100];
                            int energy = (int)hashtable3[(byte)99];
                            this.SpawnMe(key, ntransform, num2, health, energy, zombieType);
                        }
                    }
                }
                try
                {
                    this.gameScore[combatPlayer.AuthID].Kill = (int)hashtable3[(byte)69];
                    this.gameScore[combatPlayer.AuthID].Death = (int)hashtable3[(byte)68];
                    this.gameScore[combatPlayer.AuthID].Point = (int)hashtable3[(byte)67];
                    this.gameScore[combatPlayer.AuthID].Domination = (int)hashtable3[(byte)32];
                    if (hashtable3.ContainsKey((byte)107))
                    {
                        this.gameScore[combatPlayer.AuthID].Exp2clan = (int)hashtable3[(byte)107];
                        this.gameScore[combatPlayer.AuthID].Exp = (int)hashtable3[(byte)102];
                    }
                    else
                    {
                        this.gameScore[combatPlayer.AuthID].Exp = (int)hashtable3[(byte)102];
                    }
                }
                catch (Exception)
                {
                    UnityEngine.Debug.LogError("[PlayerManager : UpdateScore] User Not Found: " + combatPlayer);
                }
            }
        }
        if (scoreData.ContainsKey((byte)87))
        {
            hashtable2 = (Hashtable)scoreData[(byte)87];
            foreach (byte key2 in hashtable2.Keys)
            {
                Hashtable hashtable4 = (Hashtable)hashtable2[key2];
                this.gameScore.SetTeamPoints(key2, (int)hashtable4[(byte)67]);
            }
        }
        GameHUD.Instance.setReward(reward);
    }

    public void SetupGates(int team)
    {
        if (this.roomSettings.GameMode != MapMode.MODE.DEATHMATCH && this.roomSettings.GameMode != MapMode.MODE.TOWER_DEFENSE)
        {
            for (int i = 1; (bool)base.transform.Find("finish4").Find("GatesRed" + i); i++)
            {
                Transform transform = base.transform.Find("finish4").Find("GatesRed" + i);
                if (team == 2 || team == 0)
                {
                    transform.gameObject.layer = 0;
                }
                else
                {
                    transform.gameObject.layer = 12;
                }
            }
            for (int i = 1; (bool)base.transform.Find("finish4").Find("GatesBlue" + i); i++)
            {
                Transform transform = base.transform.Find("finish4").Find("GatesBlue" + i);
                if (team == 1 || team == 0)
                {
                    transform.gameObject.layer = 0;
                }
                else
                {
                    transform.gameObject.layer = 12;
                }
            }
            for (int i = 1; (bool)base.transform.Find("finish4").Find("GatesRedTrigger" + i); i++)
            {
                Transform transform = base.transform.Find("finish4").Find("GatesRedTrigger" + i);
                if (team == 2 || team == 0)
                {
                    transform.gameObject.layer = 0;
                }
                else
                {
                    transform.gameObject.layer = 2;
                }
            }
            for (int i = 1; (bool)base.transform.Find("finish4").Find("GatesBlueTrigger" + i); i++)
            {
                Transform transform = base.transform.Find("finish4").Find("GatesBlueTrigger" + i);
                if (team == 1 || team == 0)
                {
                    transform.gameObject.layer = 0;
                }
                else
                {
                    transform.gameObject.layer = 2;
                }
            }
        }
    }

    public void SetupFlags(Hashtable flagsData)
    {
        short num = 0;
        int num2 = -1;
        FlagState flagState = FlagState.None;
        foreach (Hashtable value in flagsData.Values)
        {
            num = (short)value[(byte)64];
            NetworkTransform ntransform = NetworkTransform.FromHashtable((Hashtable)value[(byte)65]);
            num2 = -1;
            flagState = FlagState.None;
            if (value.ContainsKey((byte)63))
            {
                num2 = (int)value[(byte)63];
            }
            if (value.ContainsKey((byte)62))
            {
                flagState = (FlagState)(int)value[(byte)62];
            }
            this.SpawnFlag(ntransform, num, num2);
            FlagEventType flagEventType = FlagEventType.Resetted;
            if (flagState == FlagState.Captured)
            {
                flagEventType = FlagEventType.Captured;
            }
            this.SetFlagState(num2, num, flagEventType, ntransform);
            PlayerManager.GameScore.Teams[num - 1].FlagState = flagState;
        }
    }

    public void SpawnFlag(NetworkTransform ntransform, int team, int bearerID)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(this.flagPrefab);
        gameObject.transform.position = ntransform.Position + new Vector3(0f, 0f, 0f);
        gameObject.transform.localEulerAngles = ntransform.Rotation;
        gameObject.transform.parent = base.transform;
        Transform transform = base.transform.Find("finish4").Find("FlagPoint" + team);
        this.flags[team - 1] = transform.gameObject.AddComponent<FlagPoint>();
        this.flags[team - 1].flagObject = gameObject;
        this.flags[team - 1].bearerID = bearerID;
        this.flags[team - 1].team = team;
        Transform transform2 = gameObject.transform.FindChild("Flag");
		flagBlue.shader = Shader.Find("Legacy Shaders/Diffuse");
		flagRed.shader = Shader.Find("Legacy Shaders/Diffuse");
        if (team < 2)
        {
            ((Component)transform2).GetComponent<Renderer>().material = this.flagRed;
        }
        else
        {
            ((Component)transform2).GetComponent<Renderer>().material = this.flagBlue;
        }
        GameHUD.Instance.SetFlagPointer(this.LocalPlayer.transform, gameObject.transform, team, 0.8f);
    }

    public void MovePlayer(int actorID, Hashtable data)
    {
        NetworkTransform networkTransform = NetworkTransform.FromHashtable(data);
        if (networkTransform != null)
        {
            CombatPlayer combatPlayer = null;
            if (this.Players.ContainsKey(actorID))
            {
                combatPlayer = this.Players[actorID];
                if ((UnityEngine.Object)this.LocalPlayer != (UnityEngine.Object)null && actorID == this.LocalPlayer.playerID)
                {
                    return;
                }
                long delta = combatPlayer.NetworkTransformReceiver.ReceiveTransform(networkTransform);
                if (Datameter.enabled)
                {
                    Datameter.MovementCounter++;
                    Datameter.MovementSizeCounter += (float)PlayerManager.GetObjectSize(data);
                }
                if (this.gameScore.ContainsUser(combatPlayer.AuthID) && data.ContainsKey((byte)81))
                {
                    try
                    {
                        this.gameScore[combatPlayer.AuthID].Ping = (short)data[(byte)81];
                        combatPlayer.Ping = this.gameScore[combatPlayer.AuthID].Ping;
                    }
                    catch (Exception)
                    {
                        UnityEngine.Debug.LogError("[PlayerManager : MovePlayer] User Not Found: " + combatPlayer.Name);
                    }
                }
                Lagometer.Report(delta, combatPlayer.Ping);
                if (Lagometer.CountLags)
                {
                    GameHUDFPS.Instance.SetDebugLine(string.Format("{0}", Lagometer.LagLine));
                }
            }
        }
    }

    public void UpdateFlag(Hashtable flagEventData)
    {
        FlagEventType flagEventType = (FlagEventType)(short)flagEventData[(byte)85];
        Hashtable hashtable = (Hashtable)flagEventData[(byte)84];
        short num = (short)hashtable[(byte)64];
        NetworkTransform ntransform = NetworkTransform.FromHashtable((Hashtable)hashtable[(byte)65]);
        FlagState flagState = FlagState.None;
        if (hashtable.ContainsKey((byte)62))
        {
            flagState = (FlagState)(int)hashtable[(byte)62];
        }
        int bearerID = -1;
        if (hashtable.ContainsKey((byte)63))
        {
            bearerID = (int)hashtable[(byte)63];
        }
        this.SetFlagState(bearerID, num, flagEventType, ntransform);
        PlayerManager.GameScore.Teams[num - 1].FlagState = flagState;
    }

    public void SetFlagState(int bearerID, short team, FlagEventType flagEventType, NetworkTransform ntransform)
    {
        GameObject flagObject = this.flags[team - 1].flagObject;
        if (!((UnityEngine.Object)flagObject == (UnityEngine.Object)null))
        {
            switch (flagEventType)
            {
                case FlagEventType.Captured:
                    if (bearerID == this.LocalPlayer.playerID)
                    {
                        GameHUD.Instance.Message(GameHUDMessageType.FLAG_YOU_CAPTURED);
                        SoundManager.Instance.Play(((Component)this.LocalPlayer.Camera.transform).GetComponent<AudioSource>(), "pick up flag", AudioPlayMode.Play);
                    }
                    else if (team != this.LocalPlayer.Team)
                    {
                        GameHUD.Instance.Message(GameHUDMessageType.FLAG_CAPTURED);
                    }
                    else
                    {
                        GameHUD.Instance.Message(GameHUDMessageType.FLAG_ENEMY_CAPTURED);
                    }
                    break;
                case FlagEventType.Delivered:
                    if ((UnityEngine.Object)this.LocalPlayer != (UnityEngine.Object)null && (UnityEngine.Object)flagObject.transform.parent == (UnityEngine.Object)this.LocalPlayer.Biped.Find("Bip01 Pelvis/Bip01 Spine/Bip01 Spine1"))
                    {
                        GameHUD.Instance.Message(GameHUDMessageType.FLAG_YOU_DELIVERED);
                        SoundManager.Instance.Play(((Component)this.LocalPlayer.Camera.transform).GetComponent<AudioSource>(), "pick up flag", AudioPlayMode.Play);
                    }
                    else if (team != this.LocalPlayer.Team)
                    {
                        GameHUD.Instance.Message(GameHUDMessageType.FLAG_DELIVERED);
                    }
                    else
                    {
                        GameHUD.Instance.Message(GameHUDMessageType.FLAG_ENEMY_DELIVERED);
                    }
                    break;
                case FlagEventType.Droped:
                    if (team != this.LocalPlayer.Team)
                    {
                        GameHUD.Instance.Message(GameHUDMessageType.FLAG_LOST);
                    }
                    else
                    {
                        GameHUD.Instance.Message(GameHUDMessageType.FLAG_ENEMY_LOST);
                    }
                    break;
                case FlagEventType.Returned:
                    if (team == this.LocalPlayer.Team)
                    {
                        GameHUD.Instance.Message(GameHUDMessageType.FLAG_RETURNED);
                    }
                    else
                    {
                        GameHUD.Instance.Message(GameHUDMessageType.FLAG_ENEMY_RETURNED);
                    }
                    break;
            }
            switch (flagEventType)
            {
                case FlagEventType.Droped:
                {
                    if ((UnityEngine.Object)flagObject.transform.parent == (UnityEngine.Object)this.LocalPlayer.transform)
                    {
                        this.SetupGates(this.LocalPlayer.Team);
                    }
                    GameHUD.Instance.SetFlagPointer(this.LocalPlayer.transform, flagObject.transform, team, 0.8f);
                    this.DropFlag(ntransform.Position, flagObject, team);
                    Transform transform = flagObject.transform;
                    transform.position -= new Vector3(0f, 5.2f, 0f);
                    break;
                }
                case FlagEventType.Delivered:
                case FlagEventType.Returned:
                case FlagEventType.Resetted:
                    if ((UnityEngine.Object)flagObject.transform.parent == (UnityEngine.Object)this.LocalPlayer.transform)
                    {
                        this.SetupGates(this.LocalPlayer.Team);
                    }
                    GameHUD.Instance.SetFlagPointer(this.LocalPlayer.transform, flagObject.transform, team, 0.8f);
                    this.DropFlag(ntransform.Position, flagObject, team);
                    break;
                case FlagEventType.Captured:
                    if (bearerID == this.LocalPlayer.playerID)
                    {
                        this.SetupGates(0);
                        GameHUD.Instance.SetFlagPointer(this.LocalPlayer.transform, flagObject.transform, team, 0f);
                    }
                    this.CaptureFlag(bearerID, flagObject, team);
                    break;
            }
        }
    }

    public void DropFlag(Vector3 position, GameObject flagObj, short team)
    {
        if ((UnityEngine.Object)flagObj.transform.parent == (UnityEngine.Object)this.LocalPlayer.transform)
        {
            NetworkTransformSender networkTransformSender = this.LocalPlayer.NetworkTransformSender;
            networkTransformSender.FlagBearer = false;
        }
        flagObj.transform.parent = null;
        this.flags[team - 1].bearerID = -1;
        flagObj.transform.position = position;
        flagObj.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        flagObj.transform.localScale = new Vector3(10f, 10f, 10f);
        ((Component)flagObj.transform).GetComponentInChildren<Renderer>().enabled = true;
    }

    public void CaptureFlag(int bearerID, GameObject flagObj, short team)
    {
        if (bearerID != -1)
        {
            this.flags[team - 1].bearerID = bearerID;
            if ((UnityEngine.Object)this.LocalPlayer == (UnityEngine.Object)null || bearerID != this.LocalPlayer.playerID)
            {
                CombatPlayer combatPlayer = this.Players[bearerID];
                if (!((UnityEngine.Object)combatPlayer == (UnityEngine.Object)null))
                {
                    flagObj.transform.parent = combatPlayer.Biped.Find("Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
                    flagObj.transform.localPosition = new Vector3(5.6f, -0.6f, 0f);
                    flagObj.transform.localEulerAngles = new Vector3(270f, 90f, 0f);
                    flagObj.transform.localScale = new Vector3(10f, 10f, 10f);
                }
            }
            else
            {
                flagObj.transform.parent = this.LocalPlayer.Biped.Find("Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
                flagObj.transform.localPosition = new Vector3(3.4f, -0.7f, 0f);
                flagObj.transform.localEulerAngles = new Vector3(290f, 180f, 270f);
                flagObj.transform.localScale = new Vector3(6f, 6f, 6f);
                NetworkTransformSender networkTransformSender = this.LocalPlayer.NetworkTransformSender;
                networkTransformSender.FlagBearer = true;
            }
        }
    }

    public void SpawnItem(Hashtable itemData)
    {
        int key = (int)itemData[(byte)75];
        ItemType itemType = (ItemType)(byte)itemData[(byte)73];
        NetworkTransform networkTransform = NetworkTransform.FromHashtable((Hashtable)itemData[(byte)71]);
        int num = 0;
        if (itemData.ContainsKey((byte)72))
        {
            num = (short)itemData[(byte)72];
        }
        GameObject original = null;
        switch (itemType)
        {
            case ItemType.Ammo:
                original = ((num != 1) ? this.ammoKitPrefab : this.ammoKitSmallPrefab);
                break;
            case ItemType.Armor:
                original = ((num != 1) ? this.armorKitPrefab : this.armorKitSmallPrefab);
                break;
            case ItemType.Health:
                original = ((num != 1) ? this.healthKitPrefab : this.healthKitSmallPrefab);
                break;
        }
        GameObject gameObject = UnityEngine.Object.Instantiate(original);
        gameObject.transform.position = networkTransform.Position + new Vector3(0f, 0f, 0f);
        gameObject.transform.localEulerAngles = networkTransform.Rotation;
        gameObject.transform.parent = base.transform;
        this.items[key] = gameObject;
    }

    public GameObject SpawnAmmo(NetworkTransform ntransform, int type)
    {
        GameObject original = (GameObject)Resources.Load("AmmoCrates/AmmoCrate" + type);
        GameObject gameObject = UnityEngine.Object.Instantiate(original);
        gameObject.transform.position = ntransform.Position + new Vector3(0f, 0f, 0f);
        gameObject.transform.localEulerAngles = ntransform.Rotation;
        gameObject.transform.parent = base.transform;
        gameObject.AddComponent<AudioSource>();
        SoundManager.Instance.Play(gameObject.GetComponent<AudioSource>(), "ammo container birth", AudioPlayMode.Play);
        return gameObject;
    }

    public void MoveItem(int id, NetworkTransform ntransform)
    {
        GameObject gameObject = this.items[id];
        gameObject.SetActive(true);
        NetworkTransformReceiver networkTransformReceiver = (NetworkTransformReceiver)gameObject.GetComponent("NetworkTransformReceiver");
        networkTransformReceiver.ReceiveTransform(ntransform);
    }

    public void PickItem(int actorID, Hashtable pickItemData)
    {
        int id = (int)pickItemData[(byte)75];
        ItemType itemType = (ItemType)(byte)pickItemData[(byte)73];
        short weaponType = 0;
        if (pickItemData.ContainsKey((byte)72))
        {
            weaponType = (short)pickItemData[(byte)72];
        }
        int num = (int)pickItemData[(byte)70];
        if ((UnityEngine.Object)this.LocalPlayer != (UnityEngine.Object)null && actorID == this.LocalPlayer.playerID)
        {
            switch (itemType)
            {
                case ItemType.Armor:
                    SoundManager.Instance.Play(((Component)this.LocalPlayer.Camera.transform).GetComponent<AudioSource>(), "pick up energy container", AudioPlayMode.Play);
                    this.LocalPlayer.Energy += num;
                    GameHUD.Instance.UpdateHealth();
                    break;
                case ItemType.Health:
                    SoundManager.Instance.Play(((Component)this.LocalPlayer.Camera.transform).GetComponent<AudioSource>(), "pick up energy container", AudioPlayMode.Play);
                    this.LocalPlayer.Health += num;
                    GameHUD.Instance.UpdateHealth();
                    break;
                case ItemType.Ammo:
                    SoundManager.Instance.Play(((Component)this.LocalPlayer.Camera.transform).GetComponent<AudioSource>(), "pick up ammo container", AudioPlayMode.Play);
                    ShotController.Instance.AddAmmoToReserve(weaponType, num, true);
                    break;
                case ItemType.MortarWhizbang:
                case ItemType.Whizbang:
                    PlayerManager.Instance.RemoveWhizbang(id);
                    return;
            }
        }
        else
        {
            switch (itemType)
            {
            }
        }
        PlayerManager.Instance.RemoveItem(id);
    }

    public void RemoveItem(int id)
    {
        if (this.items.ContainsKey(id))
        {
            UnityEngine.Object.Destroy(this.items[id]);
            this.items.Remove(id);
        }
    }

    public void RemoveWhizbang(int id)
    {
        if (this.items.ContainsKey(id))
        {
            TriggerDeleteAfterSeconds component = ((Component)this.items[id].transform).GetComponent<TriggerDeleteAfterSeconds>();
            this.items[id].transform.FindChild("Rocket").gameObject.GetComponent<Renderer>().enabled = false;
            ParticleEmitter[] componentsInChildren = ((Component)this.items[id].transform).GetComponentsInChildren<ParticleEmitter>();
            ParticleEmitter[] array = componentsInChildren;
            foreach (ParticleEmitter particleEmitter in array)
            {
                particleEmitter.emit = false;
            }
            component.Trigger();
        }
    }

    public void ReloadWeapon(int actorID, Hashtable ammoData)
    {
        CombatPlayer combatPlayer = null;
        CombatWeapon combatWeapon = null;
        if (!((UnityEngine.Object)this.LocalPlayer == (UnityEngine.Object)null) && actorID == this.LocalPlayer.playerID)
        {
            int loadedAmmo = (int)ammoData[(byte)81];
            int ammo = (int)ammoData[(byte)80];
            int index = (int)ammoData[(byte)78];
            this.LocalPlayer.ShotController.OnReload(index, loadedAmmo, ammo);
            combatWeapon = this.LocalPlayer.ShotController.CurrentWeapon;
            combatPlayer = this.LocalPlayer;
            goto IL_00d4;
        }
        if (this.Players.ContainsKey(actorID))
        {
            combatPlayer = this.Players[actorID];
            combatWeapon = combatPlayer.WeaponController.CurrentWeapon;
            EffectManager.Instance.reloadEffect(combatWeapon.Type, combatPlayer, combatWeapon.SystemName, 0);
            goto IL_00d4;
        }
        return;
        IL_00d4:
        ActorAnimator component = ((Component)combatPlayer.transform).GetComponent<ActorAnimator>();
        if (!((UnityEngine.Object)component == (UnityEngine.Object)null) && !((UnityEngine.Object)combatPlayer == (UnityEngine.Object)this.LocalPlayer))
        {
            component.ReloadWeaponAnimation(CombatWeapon.getName(combatWeapon.Type), combatWeapon);
        }
    }

    public void ChangeWeapon(int actorID, Hashtable weaponChangeData)
    {
        ActorAnimator actorAnimator = null;
        int weaponNum = (int)weaponChangeData[(byte)78];
        WeaponType weaponType = (WeaponType)(byte)weaponChangeData[(byte)89];
        if ((UnityEngine.Object)this.LocalPlayer != (UnityEngine.Object)null && actorID == this.LocalPlayer.playerID)
        {
            this.LocalPlayer.ShotController.OnChangeWeapon(weaponNum);
            actorAnimator = ((Component)this.LocalPlayer.transform).GetComponent<ActorAnimator>();
            this.ChangeWeaponAnimation(actorAnimator, weaponType, this.LocalPlayer.ShotController.GetWeaponByType((int)weaponType).SystemName);
        }
        else
        {
            CombatPlayer combatPlayer = null;
            if (this.Players.ContainsKey(actorID))
            {
                combatPlayer = this.Players[actorID];
                if (combatPlayer.WeaponController.GetWeapon(1) != null)
                {
                    combatPlayer.WeaponController.OnChangeWeapon(weaponNum);
                    actorAnimator = ((Component)combatPlayer.transform).GetComponent<ActorAnimator>();
                    if (!((UnityEngine.Object)actorAnimator == (UnityEngine.Object)null))
                    {
                        this.ChangeWeaponAnimation(actorAnimator, weaponType, combatPlayer.WeaponController.GetWeaponByType((int)weaponType).SystemName);
                    }
                }
            }
        }
    }

    public void ChangeWeaponAnimation(ActorAnimator animator, WeaponType weaponType, string systemName)
    {
        if ((UnityEngine.Object)animator == (UnityEngine.Object)null)
        {
            UnityEngine.Debug.LogError("No animator to show weapon change!");
        }
        else
        {
            switch (weaponType)
            {
                case WeaponType.ELECTRO_SHOCKER:
                case WeaponType.BIO_SHOCKER:
                    break;
                case WeaponType.ONE_HANDED_COLD_ARMS:
                    animator.ChangeWeaponAnimation("Bat");
                    break;
                case WeaponType.TWO_HANDED_COLD_ARMS:
                    animator.ChangeWeaponAnimation("Katana");
                    break;
                case WeaponType.HAND_GUN:
                    animator.ChangeWeaponAnimation("Makarov");
                    break;
                case WeaponType.MACHINE_GUN:
                    if (systemName.StartsWith("MG_AssaultRifle02"))
                    {
                        animator.ChangeWeaponAnimation("AssaultRifle02");
                    }
                    else if (systemName.StartsWith("MG_AssaultRifle03"))
                    {
                        animator.ChangeWeaponAnimation("AssaultRifle03");
                    }
                    else if (systemName.StartsWith("MG_AUG"))
                    {
                        animator.ChangeWeaponAnimation("SteyrAUG");
                    }
                    else if (systemName.StartsWith("MG_UMP45D") || systemName.StartsWith("MG_UMP45V"))
                    {
                        animator.ChangeWeaponAnimation("UMP45");
                    }
                    else if (systemName.StartsWith("MG_UMP45"))
                    {
                        animator.ChangeWeaponAnimation("TMP");
                    }
                    else
                    {
                        animator.ChangeWeaponAnimation("AK47");
                    }
                    break;
                case WeaponType.FLAMER:
                case WeaponType.GATLING_GUN:
                case WeaponType.SNOW_GUN:
                case WeaponType.ACID_THROWER:
                    if (systemName == "GG_M249")
                    {
                        animator.ChangeWeaponAnimation("M249");
                    }
                    else if (systemName == "GG_FNMAG")
                    {
                        animator.ChangeWeaponAnimation("FNMAG");
                    }
                    else
                    {
                        animator.ChangeWeaponAnimation("M134");
                    }
                    break;
                case WeaponType.SHOT_GUN:
                    if (systemName == "SG_Novapump")
                    {
                        animator.ChangeWeaponAnimation("Novapump");
                    }
                    else if (systemName == "SG_Spas")
                    {
                        animator.ChangeWeaponAnimation("Spas");
                    }
                    else if (systemName == "SG_Remington")
                    {
                        animator.ChangeWeaponAnimation("Spas");
                    }
                    else
                    {
                        animator.ChangeWeaponAnimation("Winchester1887");
                    }
                    break;
                case WeaponType.ROCKET_LAUNCHER:
                    if (systemName == "RL_RPG7")
                    {
                        animator.ChangeWeaponAnimation("RPG7");
                    }
                    else if (systemName == "RL_M202A1")
                    {
                        animator.ChangeWeaponAnimation("M202A1");
                    }
                    else
                    {
                        animator.ChangeWeaponAnimation("RPG26");
                    }
                    break;
                case WeaponType.GRENADE_LAUNCHER:
                case WeaponType.BOMB_LAUNCHER:
                    if (systemName.StartsWith("GL_EX41"))
                    {
                        animator.ChangeWeaponAnimation("Shotgun02");
                    }
                    else if (systemName.StartsWith("GL_GrenadeLauncher03"))
                    {
                        animator.ChangeWeaponAnimation("GrenadeLauncher03");
                    }
                    else if (systemName.Contains("GL_SnowLauncher"))
                    {
                        animator.ChangeWeaponAnimation("SnowLauncher");
                    }
                    else
                    {
                        animator.ChangeWeaponAnimation("Milkor");
                    }
                    break;
                case WeaponType.SNIPER_RIFLE:
                    if (systemName.StartsWith("SR_SniperRifle03"))
                    {
                        animator.ChangeWeaponAnimation("SniperRifle03");
                    }
                    else if (systemName.StartsWith("SR_Wildcat"))
                    {
                        animator.ChangeWeaponAnimation("Wildcat");
                    }
                    else if (systemName.StartsWith("SR_Vintorez"))
                    {
                        animator.ChangeWeaponAnimation("Vintorez");
                    }
                    else
                    {
                        animator.ChangeWeaponAnimation("SVD");
                    }
                    break;
            }
        }
    }

    public void UpdateEnhancer(int actorID, Hashtable enhancerData)
    {
        if ((UnityEngine.Object)this.LocalPlayer == (UnityEngine.Object)null || actorID != this.LocalPlayer.playerID)
        {
            UnityEngine.Debug.LogError("[PlayerManager] Update Enhancer Leak!!!");
        }
        else
        {
            this.LocalPlayer.UpdateEnhancer(enhancerData);
        }
    }

    private void Update()
    {
        if (!Input.GetKeyUp(KeyCode.Keypad6) && !Input.GetKeyUp(KeyCode.RightArrow))
        {
            return;
        }
        RTTXCamera.Init();
        NetworkDev.CheckAim = true;
    }

    private void FixedUpdate()
    {
        if (NetworkDev.CreateDummyPlayers)
        {
            this.CreateDummyPlayer();
        }
    }

    private void CreateDummyPlayer()
    {
        int playerID = 66;
        if ((UnityEngine.Object)this.dummyPlayer != (UnityEngine.Object)null)
        {
            playerID = this.dummyPlayer.playerID + 1;
        }
        GameObject gameObject = UnityEngine.Object.Instantiate(CharacterManager.Instance.GetPlayerEnemy());
        this.dummyPlayer = gameObject.AddComponent<CombatPlayer>();
        this.dummyPlayer.playerID = playerID;
        this.dummyPlayer.Spawn(PlayerManager.Instance.transform, Vector3.zero, Vector3.zero, 2, 100, 100, true, ZombieType.Human);
        Collider[] componentsInChildren = ((Component)this.dummyPlayer.transform).GetComponentsInChildren<Collider>();
        foreach (Collider collider in componentsInChildren)
        {
            collider.enabled = true;
        }
        Renderer[] componentsInChildren2 = this.dummyPlayer.transform.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer renderer in componentsInChildren2)
        {
            renderer.enabled = true;
        }
    }

    public void SpawnPlayer(int actorID, Hashtable spawnData)
    {
        Hashtable data = (Hashtable)spawnData[(byte)237];
        short team = (short)spawnData[(byte)239];
        int health = (int)spawnData[(byte)100];
        int energy = (int)spawnData[(byte)99];
        ZombieType zombieType = ZombieType.Human;
        if (spawnData.ContainsKey((byte)10))
        {
            zombieType = (ZombieType)(byte)spawnData[(byte)10];
        }
        NetworkTransform ntransform = NetworkTransform.FromHashtable(data);
        if ((UnityEngine.Object)this.LocalPlayer == (UnityEngine.Object)null || actorID != this.LocalPlayer.playerID)
        {
            this.SpawnEnemy(actorID, ntransform, team, true, zombieType);
        }
        else
        {
            this.SpawnMe(actorID, ntransform, team, health, energy, zombieType);
        }
    }

    public void SpawnMe(int actorID, NetworkTransform ntransform, short team, int health, int energy, ZombieType zombieType)
    {
        this.LocalPlayer.Spawn(base.transform, ntransform.Position, ntransform.Speed, team, health, energy, false, zombieType);
        this.LocalPlayer.SoldierController.Reset();
        this.LocalPlayer.NetworkTransformSender.enabled = true;
        this.LocalPlayer.SoldierController.enabled = true;
        this.LocalPlayer.SoldierCamera.enabled = true;
        this.LocalPlayer.NetworkTransformSender.StartSendTransform();
        if (this.LocalPlayer.Team >= 0)
        {
            GameHUD.Instance.BattleGUI.SetTeam(this.LocalPlayer.Team, this.RoomSettings.GameMode);
            if (this.RoomSettings.GameMode == MapMode.MODE.ZOMBIE && this.gameScore.ContainsUser(this.LocalPlayer.AuthID))
            {
                this.gameScore.UpdateUser(this.LocalPlayer.AuthID, this.LocalPlayer.Team);
            }
            else
            {
                this.gameScore.AddUser(this.LocalPlayer.AuthID, this.LocalPlayer.Name, this.LocalPlayer.Level, this.LocalPlayer.IsPremium, this.LocalPlayer.Team, this.LocalPlayer.WeaponInfo, this.LocalPlayer.ClanArmId, this.LocalPlayer.ClanTag);
            }
            this.SetupGates(team);
        }
        else
        {
            UnityEngine.Debug.LogError("[PlayerManager] Spawning Player With Team -1 !!!");
        }
        this.LocalPlayer.ActorAnimator.Ressurrect();
        this.LocalPlayer.CharacterMotor.SetExplosionForce(Vector3.zero, 0f);
        this.LocalPlayer.CharacterMotor.Reset();
        this.LocalPlayer.InitWeapon();
        this.LocalPlayer.InitWear(false, this.RoomSettings.GameMode != MapMode.MODE.ZOMBIE);
        GameHUD.Instance.Play();
        this.LocalPlayer.ShotController.SwitchWeaponTexture(WeaponType.TWO_HANDED_COLD_ARMS, 0);
        this.LocalPlayer.ShotController.enabled = true;
        this.LocalPlayer.WalkController.enabled = true;
        try
        {
            this.gameScore[this.Players[actorID].AuthID].IsDead = false;
        }
        catch (Exception)
        {
            UnityEngine.Debug.LogError("[PlayerManager : KillMe] User Not Found: " + this.Players[actorID].Name);
        }
        this.freeCamera.gameObject.SetActive(false);
        this.freeCamera = this.LocalPlayer.SoldierController.TPSCamera;
        this.freeCamera.gameObject.SetActive(false);
        this.freeCamera.transform.localPosition = new Vector3(0f, 0f, -10f);
        this.freeCamera.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        this.localCamera.gameObject.SetActive(true);
        this.isInit = true;
        GameHUD.Instance.UpdateHealth();
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator = this.Players.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CombatPlayer current = enumerator.Current;
                if ((UnityEngine.Object)current != (UnityEngine.Object)this.LocalPlayer || current.Team >= 0)
                {
                    current.PlayerRemote.IsEnemy = (this.roomSettings.GameMode == MapMode.MODE.DEATHMATCH || current.Team != this.LocalPlayer.Team);
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public void SpawnEnemy(int actorID, NetworkTransform ntransform, short team, bool resurrect, ZombieType zombieType)
    {
        if (this.Players.ContainsKey(actorID))
        {
            CombatPlayer combatPlayer = this.Players[actorID];
            combatPlayer.Spawn(base.transform, ntransform.Position, ntransform.Rotation, team, 100, 100, resurrect, zombieType);
            ActorAnimator actorAnimator = null;
            actorAnimator = combatPlayer.ActorAnimator;
            combatPlayer.NetworkTransformReceiver.StartReceiving();
            combatPlayer.InitEnemyWeapon();
            combatPlayer.InitWear(true, this.RoomSettings.GameMode != MapMode.MODE.ZOMBIE);
            if (combatPlayer.Team >= 0)
            {
                if (this.RoomSettings.GameMode == MapMode.MODE.ZOMBIE && this.gameScore.ContainsUser(combatPlayer.AuthID))
                {
                    this.gameScore.UpdateUser(combatPlayer.AuthID, combatPlayer.Team);
                }
                else
                {
                    this.gameScore.AddUser(combatPlayer.AuthID, combatPlayer.Name, combatPlayer.Level, combatPlayer.IsPremium, combatPlayer.Team, combatPlayer.WeaponInfo, combatPlayer.ClanArmId, combatPlayer.ClanTag);
                }
            }
            else
            {
                UnityEngine.Debug.LogError("[PlayerManager] Spawning Player With Team -1 !!!");
            }
            if (this.roomSettings.GameMode == MapMode.MODE.CAPTURE_THE_FLAG)
            {
                FlagPoint[] array = this.flags;
                foreach (FlagPoint flagPoint in array)
                {
                    if ((UnityEngine.Object)flagPoint != (UnityEngine.Object)null && flagPoint.bearerID == combatPlayer.playerID)
                    {
                        flagPoint.flagObject.transform.parent = combatPlayer.transform;
                        flagPoint.flagObject.transform.localPosition = new Vector3(0f, 1.8f, 0f);
                    }
                }
            }
            combatPlayer.NetworkTransformReceiver.Reset();
            try
            {
                this.gameScore[this.Players[actorID].AuthID].IsDead = false;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("[PlayerManager : SpwanEnemy] User Not Found: " + combatPlayer.Name);
            }
            if (!((UnityEngine.Object)actorAnimator == (UnityEngine.Object)null))
            {
                actorAnimator.Ressurrect();
                actorAnimator.Refresh();
                if (this.LocalPlayer.Team >= 0)
                {
                    combatPlayer.PlayerRemote.IsEnemy = (this.roomSettings.GameMode == MapMode.MODE.DEATHMATCH || combatPlayer.Team != this.LocalPlayer.Team);
                }
                if (!combatPlayer.IsZombie)
                {
                    WeaponType type = combatPlayer.WeaponController.CurrentWeapon.Type;
                    this.ChangeWeaponAnimation(actorAnimator, type, combatPlayer.WeaponController.GetWeaponByType((int)type).SystemName);
                }
            }
        }
    }

    public void SetupControlPoints(Hashtable controlPointsData)
    {
        foreach (Hashtable value in controlPointsData.Values)
        {
            this.SetControlPoint(value);
        }
    }

    public void UpdateControlPoint(Hashtable controlPointEventData)
    {
        Hashtable controlPoint = (Hashtable)controlPointEventData[(byte)82];
        this.SetControlPoint(controlPoint);
    }

    public void SetControlPoint(Hashtable controlPointData)
    {
        short point = (short)controlPointData[(byte)61];
        ControlPointState state = ControlPointState.None;
        byte progress = 0;
        if (controlPointData.ContainsKey((byte)59))
        {
            state = (ControlPointState)(int)controlPointData[(byte)59];
            progress = (byte)controlPointData[(byte)58];
        }
        short team = -1;
        if (controlPointData.ContainsKey((byte)60))
        {
            team = (short)controlPointData[(byte)60];
        }
        this.SetPointState(point, state, team, progress);
    }

    public void SetPointColor(Transform point, Color color)
    {
        Renderer[] componentsInChildren = ((Component)point.parent).GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in componentsInChildren)
        {
            if (renderer.gameObject.name == "ControlPointAura" || renderer.gameObject.name == "ControlPointTablet")
            {
                renderer.material.color = color;
            }
        }
    }

    public void SetPointOn(Transform point, bool on)
    {
        Renderer[] componentsInChildren = ((Component)point.parent).GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in componentsInChildren)
        {
            if (renderer.gameObject.name == "ControlPointAura" || renderer.gameObject.name == "ControlPointTablet")
            {
                renderer.enabled = on;
                renderer.gameObject.SetActive(false);
            }
        }
    }

    public void SetPointState(int point, ControlPointState state, int team, byte progress)
    {
        Transform transform = this.cpoints[point - 1];
        switch (state)
        {
            case ControlPointState.Captured:
                if (team == 1)
                {
                    ((Component)transform).GetComponent<Renderer>().material = this.controlPointRed;
                    this.SetPointColor(transform, new Color32(210, 50, 0, 255));
                }
                else
                {
                    ((Component)transform).GetComponent<Renderer>().material = this.controlPointBlue;
                    this.SetPointColor(transform, new Color32(0, 80, 210, 255));
                }
                break;
            case ControlPointState.None:
                ((Component)transform).GetComponent<Renderer>().material = this.controlPointNeutral;
                this.SetPointColor(transform, new Color32(160, 160, 160, 255));
                break;
        }
        PlayerManager.GameScore.ControlPoints[point - 1].Set((int)((float)(int)progress / 10f), (short)team);
    }

    public void UpdateZombie(int actorID, Hashtable zombieData)
    {
        if (zombieData.ContainsKey((byte)51))
        {
            this.zombieMode = (byte)zombieData[(byte)51];
            GameHUD.Instance.SetZombieMode(this.zombieMode);
        }
        else if (zombieData.ContainsKey((byte)239))
        {
            short team = (short)zombieData[(byte)239];
            int health = (int)zombieData[(byte)100];
            int energy = (int)zombieData[(byte)99];
            ZombieType zombieType = ZombieType.Human;
            if (zombieData.ContainsKey((byte)10))
            {
                zombieType = (ZombieType)(byte)zombieData[(byte)10];
            }
            if ((UnityEngine.Object)this.LocalPlayer == (UnityEngine.Object)null || actorID != this.LocalPlayer.playerID)
            {
                this.UpdateZombieEnemy(actorID, team, health, energy, zombieType);
            }
            else
            {
                this.UpdateZombieMe(actorID, team, health, energy, zombieType);
            }
            if (zombieData.ContainsKey((byte)97))
            {
                int key = (int)zombieData[(byte)97];
                CombatPlayer combatPlayer = this.Players[key];
                CombatPlayer combatPlayer2 = this.Players[actorID];
                if ((UnityEngine.Object)combatPlayer != (UnityEngine.Object)null && (UnityEngine.Object)combatPlayer2 != (UnityEngine.Object)null)
                {
                    PlayerManager.GameScore.AddFrag(new FragKill(this.Players[key].AuthID, combatPlayer.Name, combatPlayer.ClanTag, PlayerManager.GameScore.GetUserTeam(this.Players[key].AuthID), this.Players[actorID].AuthID, combatPlayer2.Name, combatPlayer2.ClanTag, 2, 0, WeaponType.NONE, "Zombie", PlayerHitZone.BASE, 0, 0, FragKill.FragType.None));
                }
            }
        }
    }

    private void UpdateZombieEnemy(int actorID, short team, int health, int energy, ZombieType zombieType)
    {
        if (this.Players.ContainsKey(actorID))
        {
            CombatPlayer combatPlayer = this.Players[actorID];
            combatPlayer.SetupZombiePlayer(team, health, energy, zombieType);
            if (this.gameScore.ContainsUser(combatPlayer.AuthID))
            {
                this.gameScore.UpdateUser(combatPlayer.AuthID, combatPlayer.Team);
            }
            else
            {
                this.gameScore.AddUser(combatPlayer.AuthID, combatPlayer.Name, combatPlayer.Level, combatPlayer.IsPremium, combatPlayer.Team, combatPlayer.WeaponInfo, combatPlayer.ClanArmId, combatPlayer.ClanTag);
            }
            ActorAnimator actorAnimator = null;
            actorAnimator = combatPlayer.ActorAnimator;
            this.gameScore.UpdateUser(combatPlayer.AuthID, combatPlayer.Team);
            if (!((UnityEngine.Object)actorAnimator == (UnityEngine.Object)null))
            {
                actorAnimator.Ressurrect();
                actorAnimator.Refresh();
                if (this.LocalPlayer.Team >= 0)
                {
                    combatPlayer.PlayerRemote.IsEnemy = (this.roomSettings.GameMode == MapMode.MODE.DEATHMATCH || combatPlayer.Team != this.LocalPlayer.Team);
                }
            }
        }
    }

    private void UpdateZombieMe(int actorID, short team, int health, int energy, ZombieType zombieType)
    {
        this.LocalPlayer.SetupZombiePlayer(team, health, energy, zombieType);
        if (this.gameScore.ContainsUser(this.LocalPlayer.AuthID))
        {
            this.gameScore.UpdateUser(this.LocalPlayer.AuthID, this.LocalPlayer.Team);
        }
        else
        {
            this.gameScore.AddUser(this.LocalPlayer.AuthID, this.LocalPlayer.Name, this.LocalPlayer.Level, this.LocalPlayer.IsPremium, this.LocalPlayer.Team, this.LocalPlayer.WeaponInfo, this.LocalPlayer.ClanArmId, this.LocalPlayer.ClanTag);
        }
        this.SetupGates(team);
        GameHUD.Instance.BattleGUI.SetTeam(this.LocalPlayer.Team, this.RoomSettings.GameMode);
        GameHUD.Instance.Play();
        this.LocalPlayer.ShotController.SwitchWeaponTexture(WeaponType.TWO_HANDED_COLD_ARMS, 0);
        GameHUD.Instance.UpdateHealth();
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator = this.Players.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CombatPlayer current = enumerator.Current;
                if ((UnityEngine.Object)current != (UnityEngine.Object)this.LocalPlayer || current.Team >= 0)
                {
                    current.PlayerRemote.IsEnemy = (this.roomSettings.GameMode == MapMode.MODE.DEATHMATCH || current.Team != this.LocalPlayer.Team);
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public void UpdatePlayerEnergy(int actorID, Hashtable energyData)
    {
        if ((UnityEngine.Object)this.LocalPlayer == (UnityEngine.Object)null || actorID != this.LocalPlayer.playerID)
        {
            UnityEngine.Debug.LogError("[PlayerManager] Player Energy Message Leak!!!");
        }
        else
        {
            int num = this.LocalPlayer.Health;
            int num2 = this.LocalPlayer.Energy;
            if (energyData.ContainsKey((byte)99))
            {
                num2 = (int)energyData[(byte)99];
            }
            if (energyData.ContainsKey((byte)100))
            {
                num = (int)energyData[(byte)100];
                if (num + num2 < this.LocalPlayer.Health + this.LocalPlayer.Energy)
                {
                    FPSCamera component = ((Component)this.LocalPlayer.SoldierController.FPSCamera).GetComponent<FPSCamera>();
                    component.Shake(new Vector3(0f, 0f, -5f));
                    GameHUD.Instance.ShotMe();
                    SoundManager.Instance.Play(this.LocalPlayer.Audio, "Bat_Shot_Enemy", AudioPlayMode.Play);
                }
            }
            this.LocalPlayer.Health = num;
            this.LocalPlayer.Energy = num2;
            GameHUD.Instance.UpdateHealth();
        }
    }

    public void DestroyEnemy(int id)
    {
        if (this.Players.ContainsKey(id))
        {
            CombatPlayer combatPlayer = this.Players[id];
            try
            {
                BattleChat.UserLeave(this.gameScore[combatPlayer.AuthID]);
                this.gameScore.RemoveUser(combatPlayer.AuthID);
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("[PlayerManager : DestroyEnemy] User Not Found: " + combatPlayer.Name);
            }
            this.DetachFlag(combatPlayer);
            if ((UnityEngine.Object)this.freeCamera == (UnityEngine.Object)combatPlayer.PlayerRemote.TPSCamera)
            {
                this.freeCamera.gameObject.SetActive(false);
                this.freeCamera = this.LocalPlayer.SoldierController.TPSCamera;
                if (this.LocalPlayer.IsDead)
                {
                    this.freeCamera.gameObject.SetActive(true);
                }
            }
            combatPlayer.DestroyItems();
            this.Players.Remove(id);
            UnityEngine.Object.Destroy(combatPlayer.gameObject);
        }
    }

    private void DetachFlag(CombatPlayer player)
    {
        if (this.flags != null && this.roomSettings.GameMode == MapMode.MODE.CAPTURE_THE_FLAG)
        {
            if ((UnityEngine.Object)player == (UnityEngine.Object)this.LocalPlayer)
            {
                NetworkTransformSender networkTransformSender = this.LocalPlayer.NetworkTransformSender;
                networkTransformSender.FlagBearer = false;
            }
            for (int i = 0; i < this.flags.Length; i++)
            {
                if (!((UnityEngine.Object)this.flags[i] == (UnityEngine.Object)null) && (UnityEngine.Object)this.flags[i].flagObject.transform.parent == (UnityEngine.Object)player.transform)
                {
                    this.flags[i].flagObject.transform.parent = null;
                    this.flags[i].bearerID = -1;
                    if ((UnityEngine.Object)this.LocalPlayer.transform == (UnityEngine.Object)player.transform)
                    {
                        this.SetupGates(this.LocalPlayer.Team);
                    }
                }
            }
        }
    }

    public void SyncAnimation(int id, string msg, int layer)
    {
        CombatPlayer combatPlayer = this.Players[id];
        if (!((UnityEngine.Object)combatPlayer == (UnityEngine.Object)null))
        {
            switch (layer)
            {
                case 0:
                    combatPlayer.AnimationSynchronizer.RemoteStateUpdate(msg);
                    break;
                case 1:
                    combatPlayer.AnimationSynchronizer.RemoteSecondStateUpdate(msg);
                    break;
            }
        }
    }

    public void KillPlayer(int actorID, Hashtable killData)
    {
        int num = -1;
        if (killData.ContainsKey((byte)94))
        {
            num = (int)killData[(byte)94];
        }
        WeaponType weaponType = WeaponType.NONE;
        if (killData.ContainsKey((byte)91))
        {
            weaponType = (WeaponType)(byte)killData[(byte)91];
        }
        PlayerHitZone playerHitZone = PlayerHitZone.BASE;
        if (killData.ContainsKey((byte)68))
        {
            playerHitZone = (PlayerHitZone)(byte)killData[(byte)68];
        }
        int killerHealth = 0;
        if (killData.ContainsKey((byte)92))
        {
            killerHealth = (int)killData[(byte)92];
        }
        int killerEnergy = 0;
        if (killData.ContainsKey((byte)93))
        {
            killerEnergy = (int)killData[(byte)93];
        }
        if (this.Players.ContainsKey(num) && this.Players.ContainsKey(actorID))
        {
            CombatPlayer combatPlayer = this.Players[actorID];
            CombatPlayer combatPlayer2 = this.Players[num];
            Vector3 zero = Vector3.zero;
            if (killData.ContainsKey((byte)54))
            {
                Hashtable hashtable = (Hashtable)killData[(byte)54];
                zero.x = (float)hashtable[(byte)1];
                zero.y = (float)hashtable[(byte)2];
                zero.z = (float)hashtable[(byte)3];
                zero.Normalize();
                switch (weaponType)
                {
                    case WeaponType.ONE_HANDED_COLD_ARMS:
                    case WeaponType.HAND_GUN:
                    case WeaponType.MACHINE_GUN:
                    case WeaponType.FLAMER:
                    case WeaponType.GATLING_GUN:
                    case WeaponType.SNOW_GUN:
                    case WeaponType.ACID_THROWER:
                        zero.Scale(new Vector3(1000f, 1000f, 1000f));
                        break;
                    case WeaponType.TWO_HANDED_COLD_ARMS:
                        zero.Scale(new Vector3(1500f, 1500f, 1500f));
                        if (actorID == this.LocalPlayer.playerID)
                        {
                            this.LocalPlayer.ShotController.SwitchWeaponTexture(WeaponType.TWO_HANDED_COLD_ARMS, 1);
                        }
                        break;
                    case WeaponType.SHOT_GUN:
                    case WeaponType.SNIPER_RIFLE:
                        zero.Scale(new Vector3(2500f, 2500f, 2500f));
                        break;
                    case WeaponType.ROCKET_LAUNCHER:
                    case WeaponType.GRENADE_LAUNCHER:
                        zero.Scale(new Vector3(5000f, 5000f, 5000f));
                        break;
                }
            }
            if ((UnityEngine.Object)this.LocalPlayer == (UnityEngine.Object)null || num != this.LocalPlayer.playerID)
            {
                this.KillEnemy(num, false, weaponType, zero);
            }
            else
            {
                this.KillMe(false, combatPlayer, weaponType, zero);
            }
            string name = this.Players[num].Name;
            string name2 = this.Players[actorID].Name;
            FragKill.FragType fragType = FragKill.FragType.None;
            if (killData.ContainsKey((byte)33))
            {
                fragType = (FragKill.FragType)(((byte)killData[(byte)33] == 35) ? 1 : 2);
            }
            if ((UnityEngine.Object)this.LocalPlayer != (UnityEngine.Object)null && num == this.LocalPlayer.playerID)
            {
                if (actorID != this.LocalPlayer.playerID)
                {
                    if (fragType == FragKill.FragType.Domination)
                    {
                        if ((UnityEngine.Object)combatPlayer == (UnityEngine.Object)null)
                        {
                            return;
                        }
                        combatPlayer.IsDominator = true;
                        PlayerManager.GameScore[this.Players[actorID].AuthID].Nemesis = true;
                    }
                    PlayerManager.GameScore[this.Players[actorID].AuthID].Victim = false;
                }
            }
            else if ((UnityEngine.Object)this.LocalPlayer != (UnityEngine.Object)null && actorID == this.LocalPlayer.playerID)
            {
                switch (fragType)
                {
                    case FragKill.FragType.Revenge:
                        if (!((UnityEngine.Object)combatPlayer2 == (UnityEngine.Object)null))
                        {
                            combatPlayer2.IsDominator = false;
                            PlayerManager.GameScore[this.Players[num].AuthID].Nemesis = false;
                            break;
                        }
                        return;
                    case FragKill.FragType.Domination:
                        PlayerManager.GameScore[this.Players[num].AuthID].Victim = true;
                        break;
                }
            }
            string weaponSystemName = string.Empty;
            int weaponID = -1;
            if (weaponType != 0)
            {
                CombatWeapon weaponByType;
                if ((UnityEngine.Object)combatPlayer == (UnityEngine.Object)this.LocalPlayer)
                {
                    this.CheckKillStreak(fragType, playerHitZone);
                    weaponByType = this.LocalPlayer.ShotController.GetWeaponByType((int)weaponType);
                    switch (playerHitZone)
                    {
                        case PlayerHitZone.CABIN:
                            this.EffectManager.HeadShotEffect(this.LocalPlayer);
                            break;
                        case PlayerHitZone.ENGINE:
                            this.EffectManager.NutShotEffect(this.LocalPlayer);
                            break;
                    }
                    this.lastKill = TimeManager.Instance.NetworkTime;
                    this.lastKillZone = playerHitZone;
                    if (!this.hasKills && !PlayerManager.GameScore.HasKills())
                    {
                        this.hasKills = true;
                        this.EffectManager.FirstBloodEffect(this.LocalPlayer);
                    }
                }
                else
                {
                    weaponByType = combatPlayer.WeaponController.GetWeaponByType((int)weaponType);
                }
                weaponSystemName = weaponByType.SystemName;
                weaponID = weaponByType.WeaponID;
            }
            FragKill fragKill = new FragKill(this.Players[actorID].AuthID, name2, this.Players[actorID].ClanTag, PlayerManager.GameScore.GetUserTeam(this.Players[actorID].AuthID), this.Players[num].AuthID, name, this.Players[num].ClanTag, PlayerManager.GameScore.GetUserTeam(this.Players[num].AuthID), weaponID, weaponType, weaponSystemName, playerHitZone, killerHealth, killerEnergy, fragType);
            if (killData.ContainsKey((byte)47))
            {
                CombatPlayer combatPlayer3 = null;
                int num2 = (int)killData[(byte)47];
                if (num2 != -1 && this.Players.ContainsKey(num2))
                {
                    combatPlayer3 = this.Players[num2];
                }
                if ((UnityEngine.Object)combatPlayer3 != (UnityEngine.Object)null && (combatPlayer3.AuthID == LocalUser.UserID || fragType == FragKill.FragType.None))
                {
                    fragKill.SetAssistant(combatPlayer3.AuthID, combatPlayer3.Name, combatPlayer3.ClanTag, PlayerManager.GameScore.GetUserTeam(combatPlayer3.AuthID));
                }
            }
            PlayerManager.GameScore.AddFrag(fragKill);
        }
    }

    public void CheckKillStreak(FragKill.FragType fragType, PlayerHitZone hitZone)
    {
        KillStreakType killStreakType = KillStreakType.None;
        switch (hitZone)
        {
            case PlayerHitZone.CABIN:
                killStreakType = KillStreakType.HeadShot;
                break;
            case PlayerHitZone.ENGINE:
                killStreakType = KillStreakType.NutShot;
                break;
        }
        if (fragType == FragKill.FragType.Revenge)
        {
            killStreakType = KillStreakType.Revenge;
        }
        if (TimeManager.Instance.NetworkTime - this.lastKill < 10000)
        {
            this.killSeries++;
            if (this.killSeries > 25)
            {
                killStreakType = KillStreakType.Irrepressible25Plus;
            }
            else if (this.killSeries == 25)
            {
                killStreakType = KillStreakType.Fierce25X;
            }
            else if (this.killSeries == 20)
            {
                killStreakType = KillStreakType.Heartless20X;
            }
            else if (this.killSeries == 15)
            {
                killStreakType = KillStreakType.Brutal15X;
            }
            else if (this.killSeries == 10)
            {
                killStreakType = KillStreakType.Ruthless10X;
            }
            else if (this.killSeries == 5)
            {
                killStreakType = KillStreakType.Bloodthirsty5X;
            }
            else if (this.killSeries == 4)
            {
                killStreakType = KillStreakType.KillingFour;
            }
            else if (this.killSeries == 3)
            {
                killStreakType = KillStreakType.TripleKill;
            }
            else if (this.killSeries == 2)
            {
                killStreakType = KillStreakType.DoubleKill;
            }
            if (this.killSeries >= 2 && this.lastKillZone == hitZone)
            {
                switch (hitZone)
                {
                    case PlayerHitZone.CABIN:
                        killStreakType = KillStreakType.HeadSeries;
                        break;
                    case PlayerHitZone.ENGINE:
                        killStreakType = KillStreakType.OmeletteMaster;
                        break;
                }
            }
        }
        else
        {
            this.killSeries = 1;
        }
        if (!this.hasKills && !PlayerManager.GameScore.HasKills())
        {
            this.hasKills = true;
            killStreakType = KillStreakType.FirstBlood;
        }
        if (killStreakType != 0)
        {
            GameHUD.Instance.BattleGUI.SetKillStreak(killStreakType.GetName(), killStreakType > KillStreakType.DoubleKill);
        }
    }

    public void KillEnemy(int id, bool endGame, WeaponType weaponType, Vector3 shotImpulse)
    {
        if (this.Players.ContainsKey(id))
        {
            CombatPlayer combatPlayer = this.Players[id];
            combatPlayer.NetworkTransformReceiver.StopReceiving();
            try
            {
                this.gameScore[combatPlayer.AuthID].IsDead = true;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("[PlayerManager: KillEnemy] User Not Found: " + combatPlayer.Name);
            }
            this.DetachFlag(combatPlayer);
            combatPlayer.Kill();
            if (endGame)
            {
                combatPlayer.IsDominator = false;
                combatPlayer.DestroyItems();
                combatPlayer.Hide();
            }
            else
            {
                ActorAnimator actorAnimator = null;
                actorAnimator = ((Component)combatPlayer.transform).GetComponent<ActorAnimator>();
                int num = (int)Math.Ceiling((double)(UnityEngine.Random.value * 2f));
                actorAnimator.Defeat(shotImpulse);
                this.BloodEffect(combatPlayer.transform.position);
            }
        }
    }

    public void KillMe(bool endGame, CombatPlayer player, WeaponType weaponType, Vector3 shotImpulse)
    {
        if (!((UnityEngine.Object)this.LocalPlayer == (UnityEngine.Object)null))
        {
            this.LocalPlayer.ShotController.ResetWeapon();
            this.localCamera.gameObject.SetActive(false);
            if ((UnityEngine.Object)this.freeCamera != (UnityEngine.Object)null)
            {
                this.freeCamera.gameObject.SetActive(false);
            }
            this.freeCamera = this.LocalPlayer.SoldierController.TPSCamera;
            this.freeCamera.transform.localPosition = new Vector3(0f, 0f, -10f);
            this.freeCamera.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            this.freeCamera.gameObject.SetActive(true);
            this.freeCamera.transform.rotation = this.localCamera.transform.rotation;
            if ((UnityEngine.Object)player != (UnityEngine.Object)this.LocalPlayer && (UnityEngine.Object)player != (UnityEngine.Object)null)
            {
                player.PlayerRemote.TPSCamera.transform.transform.localPosition = new Vector3(0f, 0f, -10f);
                player.PlayerRemote.TPSCamera.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                this.previousCameraPlayer = player;
                base.StartCoroutine(this.DelayedCameraSwitch(player.PlayerRemote.TPSCamera));
            }
            if ((UnityEngine.Object)player != (UnityEngine.Object)this.LocalPlayer && (UnityEngine.Object)player != (UnityEngine.Object)null)
            {
                this.freeCamera.transform.LookAt(player.transform.position);
                MouseLook componentInChildren = this.freeCamera.gameObject.GetComponentInChildren<MouseLook>();
                if ((UnityEngine.Object)componentInChildren != (UnityEngine.Object)null)
                {
                    componentInChildren.Target = player.transform;
                }
            }
            this.DetachFlag(this.LocalPlayer);
            try
            {
                this.gameScore[this.LocalPlayer.AuthID].IsDead = true;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("[PlayerManager : KillMe] User Not Found: " + player.Name);
            }
            this.LocalPlayer.ShotController.enabled = false;
            this.LocalPlayer.WalkController.enabled = false;
            this.LocalPlayer.SoldierController.enabled = false;
            this.LocalPlayer.SoldierCamera.enabled = false;
            this.LocalPlayer.Kill();
            this.LocalPlayer.CharacterMotor.SetExplosionForce(Vector3.zero, 0f);
            this.LocalPlayer.CharacterMotor.Reset();
            this.LocalPlayer.NetworkTransformSender.enabled = false;
            GameHUD.Instance.UpdateHealth();
            ShotController.Instance.Zoom = false;
            if (endGame)
            {
                this.LocalPlayer.DestroyItems();
            }
            else
            {
                this.LocalPlayer.InitWear(true, this.RoomSettings.GameMode != MapMode.MODE.ZOMBIE);
                ActorAnimator component = ((Component)this.LocalPlayer.transform).GetComponent<ActorAnimator>();
                component.Defeat(shotImpulse);
            }
        }
    }

    private IEnumerator DelayedCameraSwitch(Camera newCamera)
    {
        yield return (object)new WaitForSeconds(2f);
        this.freeCamera.gameObject.SetActive(false);
        this.freeCamera = newCamera;
        this.freeCamera.gameObject.SetActive(true);
    }

    public void nextCamera()
    {
        Camera newCamera = null;
        CombatPlayer combatPlayer = null;
        bool flag = false;
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator = this.Players.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CombatPlayer current = enumerator.Current;
                if ((UnityEngine.Object)current == (UnityEngine.Object)this.previousCameraPlayer)
                {
                    flag = true;
                }
                else if (!current.IsDead && (current.Team == this.LocalPlayer.Team || (this.LocalPlayer.Team < 1 && this.RoomSettings.GameMode == MapMode.MODE.ZOMBIE)) && flag)
                {
                    combatPlayer = current;
                    break;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        if ((UnityEngine.Object)combatPlayer == (UnityEngine.Object)null)
        {
            Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator2 = this.Players.Values.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    CombatPlayer current2 = enumerator2.Current;
                    if ((UnityEngine.Object)current2 == (UnityEngine.Object)this.previousCameraPlayer)
                    {
                        break;
                    }
                    if (current2.IsDead || (current2.Team != this.LocalPlayer.Team && (this.LocalPlayer.Team >= 1 || this.RoomSettings.GameMode != MapMode.MODE.ZOMBIE)))
                    {
                        continue;
                    }
                    combatPlayer = current2;
                    break;
                }
            }
            finally
            {
                ((IDisposable)enumerator2).Dispose();
            }
        }
        if ((UnityEngine.Object)combatPlayer != (UnityEngine.Object)null)
        {
            newCamera = combatPlayer.PlayerRemote.TPSCamera;
            this.previousCameraPlayer = combatPlayer;
        }
        this.CameraSwitch(newCamera);
    }

    public void nextCamera(int team)
    {
        if (this.RoomSettings.GameMode == MapMode.MODE.DEATHMATCH)
        {
            team = 0;
        }
        Camera newCamera = null;
        CombatPlayer combatPlayer = null;
        bool flag = false;
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator = this.Players.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CombatPlayer current = enumerator.Current;
                if ((UnityEngine.Object)current == (UnityEngine.Object)this.previousCameraPlayer)
                {
                    flag = true;
                }
                else if (!current.IsDead && current.Team == team && flag)
                {
                    combatPlayer = current;
                    break;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        if ((UnityEngine.Object)combatPlayer == (UnityEngine.Object)null)
        {
            Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator2 = this.Players.Values.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    CombatPlayer current2 = enumerator2.Current;
                    if ((UnityEngine.Object)current2 == (UnityEngine.Object)this.previousCameraPlayer)
                    {
                        break;
                    }
                    if (current2.IsDead || current2.Team != team)
                    {
                        continue;
                    }
                    combatPlayer = current2;
                    break;
                }
            }
            finally
            {
                ((IDisposable)enumerator2).Dispose();
            }
        }
        if ((UnityEngine.Object)combatPlayer != (UnityEngine.Object)null)
        {
            newCamera = combatPlayer.PlayerRemote.TPSCamera;
            this.previousCameraPlayer = combatPlayer;
        }
        this.CameraSwitch(newCamera);
    }

    public void CameraSwitch(Camera newCamera)
    {
        if (!((UnityEngine.Object)newCamera == (UnityEngine.Object)null))
        {
            this.freeCamera.gameObject.SetActive(false);
            this.freeCamera = newCamera;
            this.freeCamera.gameObject.SetActive(true);
        }
    }

    public void CameraSwitch(Camera oldCamera, Camera newCamera)
    {
        if ((UnityEngine.Object)oldCamera != (UnityEngine.Object)null)
        {
            oldCamera.gameObject.SetActive(false);
        }
        if ((UnityEngine.Object)newCamera != (UnityEngine.Object)null)
        {
            newCamera.gameObject.SetActive(true);
        }
        this.freeCamera = newCamera;
    }

    public void TimeOver(Hashtable rewardData)
    {
        if (this.roomSettings.GameMode == MapMode.MODE.TOWER_DEFENSE)
        {
            this.ResetCampaign();
        }
        GameHUD.Instance.setReward(0);
        long startTime = (long)rewardData[(byte)95];
        if (rewardData.ContainsKey((byte)50))
        {
            short[] array = (short[])rewardData[(byte)50];
            UnityEngine.Debug.Log(string.Format("TOTAL TEAM WIN Red:{0} Blue:{1}", array[0], array[1]));
            PlayerManager.GameScore.Teams[0].Wins = array[0];
            PlayerManager.GameScore.Teams[1].Wins = array[1];
        }
        this.roomSettings.StartTime = startTime;
        MonoBehaviour.print("[PlayerManager] Time Over! Kill Players!");
        GameHUD.Instance.TimeOver();
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator = this.Players.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CombatPlayer current = enumerator.Current;
                if (!((UnityEngine.Object)current == (UnityEngine.Object)this.LocalPlayer))
                {
                    this.KillEnemy(current.playerID, true, WeaponType.NONE, new Vector3(0f, 0f, 0f));
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        this.KillMe(true, null, WeaponType.NONE, new Vector3(0f, 0f, 0f));
        this.EffectManager.EndGameEffect(this.LocalPlayer);
    }

    public void NewGame(Hashtable newGameData)
    {
        KickManager.Instance.ResetVote();
        long num = (long)newGameData[(byte)95];
        this.RoomSettings.StartTime = num;
        GameHUD.Instance.SetTimeOverReadyState();
        this.gameScore.Reset();
        if (this.roomSettings.GameMode == MapMode.MODE.ZOMBIE)
        {
            GameHUD.Instance.Play();
            this.EffectManager.StartGameEffect(this.LocalPlayer);
        }
        if (this.roomSettings.GameMode == MapMode.MODE.TOWER_DEFENSE)
        {
            this.campaign.ResetCampaign(num);
        }
        this.hasKills = false;
    }

    public void BloodEffect(Vector3 position)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(this.bloodPrefab);
        gameObject.transform.position = position;
    }

    public void SpawnPlayerItem(int actorID, Hashtable shotData)
    {
        Shot shot = Shot.FromHashtable(shotData);
        if (this.Players.ContainsKey(actorID))
        {
            CombatPlayer player = this.Players[actorID];
            if (!((UnityEngine.Object)this.LocalPlayer == (UnityEngine.Object)null) && actorID == this.LocalPlayer.playerID)
            {
                return;
            }
            this.ShotEffect(shot, player, ShotEffectType.ENEMY_SET_GAME_STATE);
        }
    }

    public static int GetObjectSize(object TestObject)
    {
        if (TestObject == null)
        {
            return 0;
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();
        binaryFormatter.Serialize(memoryStream, TestObject);
        byte[] array = memoryStream.ToArray();
        return array.Length;
    }

    public void PlayerShot(int actorID, Hashtable shotData)
    {
        if (Datameter.enabled)
        {
            Datameter.ShotStatistics.ShotCounter++;
            Datameter.ShotStatistics.ShotSizeCounter += (float)PlayerManager.GetObjectSize(shotData);
        }
        Shot shot = Shot.FromHashtable(shotData);
        int num = -1;
        if (this.Players.ContainsKey(actorID))
        {
            CombatPlayer combatPlayer = this.Players[actorID];
            CombatPlayer combatPlayer2 = null;
            CombatPlayer combatPlayer3 = null;
            if (shot.HasTargets)
            {
                List<ShotTarget>.Enumerator enumerator = shot.Targets.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        ShotTarget current = enumerator.Current;
                        switch (current.TargetType)
                        {
                            case ShotTargetType.BOT:
                                if (this.campaign.Actors.ContainsKey(current.TargetID))
                                {
                                    combatPlayer3 = this.campaign.Actors[current.TargetID];
                                    if ((UnityEngine.Object)combatPlayer3 != (UnityEngine.Object)null)
                                    {
                                        if (actorID == this.LocalPlayer.playerID && current.HealthDamage + current.EnergyDamage != 0)
                                        {
                                            this.ShotDamage(current.HealthDamage + current.EnergyDamage, combatPlayer3.transform, shot.Crit, current.HitZone);
                                        }
                                        current.TargetTransform = combatPlayer3.transform;
                                    }
                                }
                                break;
                            case ShotTargetType.PLAYER_ITEM:
                                if (this.Players.ContainsKey(current.TargetID))
                                {
                                    combatPlayer2 = this.Players[current.TargetID];
                                    if (combatPlayer2.RegisteredItems.ContainsKey(current.ItemTimeStamp))
                                    {
                                        ItemTracer itemTracer = combatPlayer2.RegisteredItems[current.ItemTimeStamp];
                                        if (actorID == this.LocalPlayer.playerID && current.HealthDamage + current.EnergyDamage != 0)
                                        {
                                            this.ShotDamage(current.HealthDamage + current.EnergyDamage, itemTracer.transform, shot.Crit, current.HitZone);
                                        }
                                        current.TargetTransform = itemTracer.transform;
                                    }
                                }
                                break;
                            case ShotTargetType.PLAYER:
                                {
                                    if (this.Players.ContainsKey(current.TargetID))
                                    {
                                        if (current.TargetID != this.LocalPlayer.playerID)
                                        {
                                            combatPlayer2 = this.Players[current.TargetID];
                                        }
                                        else
                                        {
                                            combatPlayer2 = this.LocalPlayer;
                                            if (!this.LocalPlayer.IsDead)
                                            {
                                                this.LocalPlayer.Health -= current.HealthDamage;
                                                this.LocalPlayer.Energy -= current.EnergyDamage;
                                            }
                                            GameHUD.Instance.UpdateHealth();
                                            if (actorID != this.LocalPlayer.playerID)
                                            {
                                                if ((UnityEngine.Object)combatPlayer != (UnityEngine.Object)null && !combatPlayer.IsDead && ((shot.WeaponType != WeaponType.CHARGER && shot.WeaponType != WeaponType.MEGA_CHARGER && shot.WeaponType != WeaponType.TURRET_TESLA && shot.WeaponType != WeaponType.TURRET_MACHINE_GUN) || combatPlayer.Team != this.LocalPlayer.Team || combatPlayer.Team == 0))
                                                {
                                                    switch (shot.LaunchMode)
                                                    {
                                                        case LaunchModes.SHOT:
                                                            if ((shot.WeaponType == WeaponType.SHOT_GUN || shot.WeaponType == WeaponType.FLAMER || shot.WeaponType == WeaponType.ACID_THROWER || shot.WeaponType == WeaponType.ELECTRO_SHOCKER) && current.HealthDamage + current.EnergyDamage == 0)
                                                            {
                                                                break;
                                                            }
                                                            GameHUD.Instance.ShotMe(this.LocalPlayer.transform, combatPlayer.transform, combatPlayer.playerID);
                                                            break;
                                                        case LaunchModes.TURRET_SHOT:
                                                        {
                                                            Dictionary<long, ItemTracer>.ValueCollection.Enumerator enumerator2 = combatPlayer.RegisteredItems.Values.GetEnumerator();
                                                            try
                                                            {
                                                                while (enumerator2.MoveNext())
                                                                {
                                                                    ItemTracer current2 = enumerator2.Current;
                                                                    if (current2.GetType() == typeof(TurretTeslaTracer))
                                                                    {
                                                                        GameHUD.Instance.ShotMe(this.LocalPlayer.transform, current2.transform, combatPlayer.playerID);
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            finally
                                                            {
                                                                ((IDisposable)enumerator2).Dispose();
                                                            }
                                                            break;
                                                        }
                                                    }
                                                    if (shot.WeaponType != WeaponType.SHOT_GUN && shot.WeaponType != WeaponType.FLAMER && shot.WeaponType != WeaponType.ACID_THROWER && shot.WeaponType != WeaponType.ELECTRO_SHOCKER)
                                                    {
                                                        goto IL_0489;
                                                    }
                                                    if (current.HealthDamage + current.EnergyDamage != 0)
                                                    {
                                                        goto IL_0489;
                                                    }
                                                }
                                            }
                                            else if (shot.WeaponType != WeaponType.TURRET_TESLA && shot.WeaponType != WeaponType.TURRET_MACHINE_GUN)
                                            {
                                                GameHUD.Instance.ShotMe();
                                                SoundManager.Instance.Play(this.LocalPlayer.Audio, "Bat_Shot_Enemy", AudioPlayMode.Play);
                                            }
                                        }
                                        goto IL_04f5;
                                    }
                                    break;
                                }
                                IL_0489:
                                GameHUD.Instance.ShotMe();
                                this.ShotForce(shot, this.LocalPlayer, combatPlayer, current.HealthDamage + current.EnergyDamage);
                                goto IL_04f5;
                                IL_04f5:
                                if (current.HealthDamage + current.EnergyDamage > 0)
                                {
                                    this.EffectManager.HitEffect(shot, combatPlayer, combatPlayer2, current.EnergyDamage, current.HealthDamage);
                                }
                                if (actorID == this.LocalPlayer.playerID && current.HealthDamage + current.EnergyDamage != 0)
                                {
                                    this.ShotDamage(current.HealthDamage + current.EnergyDamage, combatPlayer2.transform, shot.Crit, current.HitZone);
                                    GameHUD.Instance.IndicateDamage();
                                }
                                current.TargetTransform = combatPlayer2.transform;
                                break;
                        }
                    }
                }
                finally
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
            if (actorID != this.LocalPlayer.playerID)
            {
                this.ShotEffect(shot, combatPlayer, ShotEffectType.ENEMY_AFTER_SERVER);
            }
            else if (this.ShotEffect(shot, combatPlayer, ShotEffectType.ME_AFTER_SERVER))
            {
                ShotController.Instance.OnShot(shot.WeaponType);
            }
        }
    }

    public void PlayerImpact(int actorID, Hashtable impactData)
    {
        if (this.Players.ContainsKey(actorID))
        {
            int num = (int)impactData[(byte)94];
            if (num != -1 && this.Players.ContainsKey(num))
            {
                CombatPlayer player = this.Players[actorID];
                CombatPlayer combatPlayer = null;
                int num2 = 0;
                int num3 = 0;
                if (impactData.ContainsKey((byte)92))
                {
                    num2 = (short)impactData[(byte)92];
                }
                if (impactData.ContainsKey((byte)93))
                {
                    num3 = (short)impactData[(byte)93];
                }
                if (num != this.LocalPlayer.playerID)
                {
                    combatPlayer = this.Players[num];
                }
                else
                {
                    combatPlayer = this.LocalPlayer;
                    if (!this.LocalPlayer.IsDead)
                    {
                        this.LocalPlayer.Health -= num2;
                        this.LocalPlayer.Energy -= num3;
                    }
                    GameHUD.Instance.UpdateHealth();
                    GameHUD.Instance.ShotMe();
                }
                ImpactType impactType = (ImpactType)(byte)impactData[(byte)52];
                if (num2 + num3 > 0 || impactType == ImpactType.Stunning)
                {
                    this.EffectManager.ImpactEffect(impactType, player, combatPlayer);
                }
                if (actorID == this.LocalPlayer.playerID && num2 + num3 != 0)
                {
                    this.ShotDamage(num2 + num3, combatPlayer.transform, false, PlayerHitZone.BASE);
                    GameHUD.Instance.IndicateDamage();
                }
            }
        }
    }

    private bool ShotForce(Shot shot, CombatPlayer player, CombatPlayer fromPlayer, int damage)
    {
        if (this.RoomSettings.GameMode == MapMode.MODE.ZOMBIE && fromPlayer.IsZombie)
        {
            return false;
        }
        if (shot.WeaponType != WeaponType.ROCKET_LAUNCHER && shot.WeaponType != WeaponType.GRENADE_LAUNCHER)
        {
            Vector3 normalized = shot.Direction.normalized;
            float num = 15f;
            switch (shot.WeaponType)
            {
                case WeaponType.ONE_HANDED_COLD_ARMS:
                case WeaponType.TWO_HANDED_COLD_ARMS:
                    if (shot.LaunchMode == LaunchModes.BLOW)
                    {
                        return true;
                    }
                    num = 20f;
                    if (((Component)player).GetComponent<PlayerRemote>().InAir)
                    {
                        num = 5f;
                    }
                    break;
                case WeaponType.GATLING_GUN:
                    if (((Component)player).GetComponent<PlayerRemote>().InAir)
                    {
                        num = 5f;
                    }
                    num = 15f;
                    break;
                case WeaponType.SHOT_GUN:
                    num = 40f;
                    if (((Component)player).GetComponent<PlayerRemote>().InAir)
                    {
                        num = 10f;
                    }
                    break;
                case WeaponType.HAND_GUN:
                case WeaponType.FLAMER:
                case WeaponType.SNOW_GUN:
                case WeaponType.ACID_THROWER:
                    num = 0f;
                    break;
                case WeaponType.MACHINE_GUN:
                    num = 0f;
                    break;
            }
            if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.ZOMBIE && PlayerManager.Instance.LocalPlayer.Team == 1)
            {
                if (damage <= 0)
                {
                    return true;
                }
                switch (shot.WeaponType)
                {
                    case WeaponType.ONE_HANDED_COLD_ARMS:
                    case WeaponType.TWO_HANDED_COLD_ARMS:
                        if (shot.LaunchMode == LaunchModes.BLOW)
                        {
                            return true;
                        }
                        num = 180f;
                        break;
                    case WeaponType.GATLING_GUN:
                        num = 65f;
                        break;
                    case WeaponType.SHOT_GUN:
                        num = 90f;
                        break;
                    case WeaponType.HAND_GUN:
                        num = 45f;
                        break;
                    case WeaponType.FLAMER:
                    case WeaponType.SNOW_GUN:
                    case WeaponType.ACID_THROWER:
                        num = 75f;
                        break;
                    case WeaponType.MACHINE_GUN:
                        num = 65f;
                        break;
                    case WeaponType.SNIPER_RIFLE:
                        num = 45f;
                        break;
                }
                if (((Component)player).GetComponent<PlayerRemote>().InAir)
                {
                    num *= 0.2f;
                }
                if (player.ZombieType == ZombieType.Boss)
                {
                    num *= 0.35f;
                }
            }
            CharacterMotor componentInChildren = ((Component)player).GetComponentInChildren<CharacterMotor>();
            componentInChildren.SetExplosionForce(fromPlayer.transform.position, num);
            FPSCamera component = ((Component)player.SoldierController.FPSCamera).GetComponent<FPSCamera>();
            if ((UnityEngine.Object)component != (UnityEngine.Object)null)
            {
                component.Shake(new Vector3(-1f, 0f, 0f));
            }
            return true;
        }
        return false;
    }

    private bool ShotEffect(Shot shot, CombatPlayer player, ShotEffectType shotEffectType)
    {
        bool isMe = shotEffectType == ShotEffectType.ME_BEFORE_SERVER || shotEffectType == ShotEffectType.ME_AFTER_SERVER;
        ActorAnimator actorAnimator = null;
        switch (shot.WeaponType)
        {
            case WeaponType.ONE_HANDED_COLD_ARMS:
                actorAnimator = ((Component)player.transform).GetComponent<ActorAnimator>();
                if ((UnityEngine.Object)actorAnimator != (UnityEngine.Object)null && shot.LaunchMode == LaunchModes.LAUNCH && (UnityEngine.Object)player != (UnityEngine.Object)this.LocalPlayer)
                {
                    if (player.IsZombie)
                    {
                        actorAnimator.ShotWeaponAnimation("Zombie");
                    }
                    else
                    {
                        actorAnimator.ShotWeaponAnimation("Bat");
                    }
                }
                this.EffectManager.batEffect(shot, player, isMe);
                return false;
            case WeaponType.TWO_HANDED_COLD_ARMS:
                actorAnimator = ((Component)player.transform).GetComponent<ActorAnimator>();
                if ((UnityEngine.Object)actorAnimator != (UnityEngine.Object)null && shot.LaunchMode == LaunchModes.LAUNCH && (UnityEngine.Object)player != (UnityEngine.Object)this.LocalPlayer)
                {
                    actorAnimator.ShotWeaponAnimation("Katana");
                }
                this.EffectManager.batEffect(shot, player, isMe);
                return false;
            case WeaponType.GATLING_GUN:
                if (shotEffectType == ShotEffectType.ME_AFTER_SERVER)
                {
                    if (shot.LaunchMode != LaunchModes.LAUNCH && shot.LaunchMode != LaunchModes.BLOW)
                    {
                        return true;
                    }
                    return false;
                }
                if (shot.LaunchMode != LaunchModes.LAUNCH && shot.LaunchMode != LaunchModes.BLOW)
                {
                    if (Datameter.enabled)
                    {
                        Datameter.ShotStatistics.RapidFire++;
                    }
                    this.EffectManager.machineGunEffect(shot, player, isMe);
                    break;
                }
                this.EffectManager.gatlingGunEffect(shot.LaunchMode, player, isMe);
                return true;
            case WeaponType.SHOT_GUN:
                if (shotEffectType == ShotEffectType.ME_AFTER_SERVER)
                {
                    return true;
                }
                if (Datameter.enabled)
                {
                    Datameter.ShotStatistics.RapidFire++;
                }
                this.EffectManager.shotGunEffect(shot, player, isMe);
                break;
            case WeaponType.HAND_GUN:
                if (shot.LaunchMode == LaunchModes.BLOW)
                {
                    return false;
                }
                if (shotEffectType == ShotEffectType.ME_AFTER_SERVER)
                {
                    return true;
                }
                if (Datameter.enabled)
                {
                    Datameter.ShotStatistics.RapidFire++;
                }
                this.EffectManager.machineGunEffect(shot, player, isMe);
                break;
            case WeaponType.MACHINE_GUN:
                if (shot.LaunchMode == LaunchModes.BLOW)
                {
                    return false;
                }
                if (shotEffectType == ShotEffectType.ME_AFTER_SERVER)
                {
                    return true;
                }
                if (Datameter.enabled)
                {
                    Datameter.ShotStatistics.RapidFire++;
                }
                this.EffectManager.machineGunEffect(shot, player, isMe);
                break;
            case WeaponType.SNIPER_RIFLE:
                if (shot.LaunchMode == LaunchModes.BLOW)
                {
                    return false;
                }
                if (shotEffectType == ShotEffectType.ME_AFTER_SERVER)
                {
                    return true;
                }
                if (Datameter.enabled)
                {
                    Datameter.ShotStatistics.PowerFire++;
                }
                this.EffectManager.machineGunEffect(shot, player, isMe);
                break;
            case WeaponType.FLAMER:
            case WeaponType.SNOW_GUN:
            case WeaponType.ACID_THROWER:
                if (shot.LaunchMode == LaunchModes.BLOW)
                {
                    return false;
                }
                if (shotEffectType == ShotEffectType.ME_AFTER_SERVER)
                {
                    return true;
                }
                if (Datameter.enabled)
                {
                    Datameter.ShotStatistics.CloseFire++;
                }
                this.EffectManager.flamerEffect(shot, player, isMe);
                break;
            case WeaponType.ROCKET_LAUNCHER:
                if (shotEffectType == ShotEffectType.ME_AFTER_SERVER && shot.LaunchMode == LaunchModes.LAUNCH)
                {
                    actorAnimator = player.ActorAnimator;
                    if ((UnityEngine.Object)actorAnimator != (UnityEngine.Object)null && (UnityEngine.Object)player == (UnityEngine.Object)this.LocalPlayer)
                    {
                        WeaponLook[] componentsInChildren2 = ((Component)player.transform).GetComponentsInChildren<WeaponLook>();
                        foreach (WeaponLook weaponLook2 in componentsInChildren2)
                        {
                            if (weaponLook2.gameObject.name == "RocketLauncherWeapon")
                            {
                                actorAnimator.ShotWeaponAnimation("Shot", weaponLook2.transform);
                            }
                        }
                    }
                    return true;
                }
                if (shot.LaunchMode == LaunchModes.LAUNCH)
                {
                    if (Datameter.enabled)
                    {
                        Datameter.ShotStatistics.LaunchCounter++;
                    }
                    this.EffectManager.rocketLauncherEffect(shot, player, isMe);
                    break;
                }
                if (Datameter.enabled)
                {
                    Datameter.ShotStatistics.ExplosionCounter++;
                }
                this.EffectManager.launcherEffect(shot, player, this.LocalPlayer, isMe);
                return false;
            case WeaponType.GRENADE_LAUNCHER:
                if (shotEffectType == ShotEffectType.ME_AFTER_SERVER && shot.LaunchMode == LaunchModes.LAUNCH)
                {
                    actorAnimator = player.ActorAnimator;
                    if ((UnityEngine.Object)actorAnimator != (UnityEngine.Object)null && (UnityEngine.Object)player == (UnityEngine.Object)this.LocalPlayer)
                    {
                        WeaponLook[] componentsInChildren3 = ((Component)player.transform).GetComponentsInChildren<WeaponLook>();
                        foreach (WeaponLook weaponLook3 in componentsInChildren3)
                        {
                            if (weaponLook3.gameObject.name == "GrenadeLauncherWeapon" || weaponLook3.gameObject.name == "SnowLauncherWeapon")
                            {
                                actorAnimator.ShotWeaponAnimation("Shot", weaponLook3.transform);
                            }
                        }
                    }
                    return true;
                }
                if (shot.LaunchMode == LaunchModes.LAUNCH)
                {
                    if (Datameter.enabled)
                    {
                        Datameter.ShotStatistics.LaunchCounter++;
                    }
                    this.EffectManager.grenadeLauncherEffect(shot, player, isMe);
                    break;
                }
                if (Datameter.enabled)
                {
                    Datameter.ShotStatistics.ExplosionCounter++;
                }
                this.EffectManager.launcherEffect(shot, player, this.LocalPlayer, isMe);
                return false;
            case WeaponType.BOMB_LAUNCHER:
                if (shotEffectType == ShotEffectType.ME_AFTER_SERVER && shot.LaunchMode == LaunchModes.LAUNCH)
                {
                    actorAnimator = player.ActorAnimator;
                    if ((UnityEngine.Object)actorAnimator != (UnityEngine.Object)null && (UnityEngine.Object)player == (UnityEngine.Object)this.LocalPlayer)
                    {
                        WeaponLook[] componentsInChildren = ((Component)player.transform).GetComponentsInChildren<WeaponLook>();
                        foreach (WeaponLook weaponLook in componentsInChildren)
                        {
                            if (weaponLook.gameObject.name == "BombLauncherWeapon")
                            {
                                actorAnimator.ShotWeaponAnimation("Shot", weaponLook.transform);
                            }
                        }
                    }
                    return true;
                }
                if (shot.LaunchMode == LaunchModes.LAUNCH)
                {
                    if (Datameter.enabled)
                    {
                        Datameter.ShotStatistics.LaunchCounter++;
                    }
                    CombatWeapon weaponByType = player.GetWeaponByType(shot.WeaponType);
                    this.EffectManager.bombLauncherEffect(shot, weaponByType, player, isMe, shotEffectType == ShotEffectType.ENEMY_SET_GAME_STATE);
                    break;
                }
                if (Datameter.enabled)
                {
                    Datameter.ShotStatistics.ExplosionCounter++;
                }
                this.EffectManager.launcherEffect(shot, player, this.LocalPlayer, isMe);
                return false;
        }
        return true;
    }

    public void ShotDamage(int damage, Transform targetTransform, bool crit, PlayerHitZone playerHitZone)
    {
        if (!((UnityEngine.Object)targetTransform == (UnityEngine.Object)null))
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(CharacterManager.Instance.GetDamageInfo());
            TextMesh componentInChildren = gameObject.GetComponentInChildren<TextMesh>();
            string empty = string.Empty;
            if (crit)
            {
                ((Component)componentInChildren).GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 0.1f, 0.2f, 1f));
            }
            else
            {
                switch (playerHitZone)
                {
                    case PlayerHitZone.CABIN:
                        ((Component)componentInChildren).GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 0.2f, 1f));
                        break;
                    case PlayerHitZone.ENGINE:
                        ((Component)componentInChildren).GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 0.2f, 1f));
                        break;
                }
            }
            if (damage < 0)
            {
                componentInChildren.text = empty + "+" + (-damage).ToString();
            }
            else
            {
                componentInChildren.text = empty + (-damage).ToString();
            }
            gameObject.transform.position = targetTransform.position + new Vector3(0f, 5f, 0f);
            gameObject.transform.localEulerAngles = new Vector3(0f, (float)this.EffectManager.RandomGenerator.NextDouble() * 360f - 180f, 0f);
            SpriteInfo componentInChildren2 = gameObject.GetComponentInChildren<SpriteInfo>();
            if ((UnityEngine.Object)componentInChildren2 != (UnityEngine.Object)null)
            {
                componentInChildren2.Invert = true;
            }
        }
    }

    public int TeamScan(Vector3 pos, float dist, int team)
    {
        return this.TeamScan(pos, dist, team, false);
    }

    public int TeamScan(Vector3 pos, float dist, int team, bool scanBots)
    {
        return this.TeamScan(pos, dist, team, false, false);
    }

    public int TeamScan(Vector3 pos, float dist, int team, bool scanBots, bool scanMe)
    {
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator = this.Players.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CombatPlayer current = enumerator.Current;
                if (!((UnityEngine.Object)current.transform == (UnityEngine.Object)null) && !current.IsDead && (!((UnityEngine.Object)current == (UnityEngine.Object)this.LocalPlayer) || scanMe) && (current.Team != team || current.Team == 0) && Vector3.Distance(current.transform.position, pos) < dist)
                {
                    return current.playerID;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        if (this.roomSettings.GameMode == MapMode.MODE.TOWER_DEFENSE && scanBots)
        {
            Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator2 = PlayerManager.Instance.Campaign.Actors.Values.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    CombatPlayer current2 = enumerator2.Current;
                    if (!((UnityEngine.Object)current2.transform == (UnityEngine.Object)null) && !current2.IsDead && Vector3.Distance(current2.transform.position, pos) < dist)
                    {
                        return current2.playerID;
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator2).Dispose();
            }
            return -1;
        }
        return -1;
    }

    public bool LocalScan(Vector3 pos, float dist)
    {
        if ((UnityEngine.Object)this.LocalPlayer.transform == (UnityEngine.Object)null)
        {
            return false;
        }
        if (this.LocalPlayer.IsDead)
        {
            return false;
        }
        if (Vector3.Distance(this.LocalPlayer.transform.position, pos) > dist)
        {
            return false;
        }
        return true;
    }

    public int Scan(Vector3 pos, float dist)
    {
        return this.Scan(pos, dist, false);
    }

    public int Scan(Vector3 pos, float dist, bool includingMe)
    {
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator = this.Players.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CombatPlayer current = enumerator.Current;
                if (!((UnityEngine.Object)current.transform == (UnityEngine.Object)null) && !current.IsDead && (!((UnityEngine.Object)current == (UnityEngine.Object)this.LocalPlayer) || includingMe) && Vector3.Distance(current.transform.position, pos) < dist)
                {
                    return current.playerID;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        if (this.roomSettings.GameMode != MapMode.MODE.TOWER_DEFENSE)
        {
            return -1;
        }
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator2 = PlayerManager.Instance.Campaign.Actors.Values.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                CombatPlayer current2 = enumerator2.Current;
                if (!((UnityEngine.Object)current2.transform == (UnityEngine.Object)null) && !current2.IsDead && Vector3.Distance(current2.transform.position, pos) < dist)
                {
                    return current2.playerID;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator2).Dispose();
        }
        return -1;
    }

    public Transform ClosestTransformScan(Vector3 pos, float dist)
    {
        Transform result = null;
        float num = 100f;
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator = this.Players.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CombatPlayer current = enumerator.Current;
                if (!((UnityEngine.Object)current.transform == (UnityEngine.Object)null) && !((UnityEngine.Object)current == (UnityEngine.Object)this.LocalPlayer) && !current.IsDead)
                {
                    float num2 = Vector3.SqrMagnitude(current.transform.position - pos);
                    if (num > num2)
                    {
                        num = num2;
                        result = current.transform;
                    }
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        if (num < dist * dist)
        {
            return result;
        }
        return null;
    }

    public void CompleteAchievement(int actorID, Hashtable achievementData)
    {
        if (achievementData.Count > 3)
        {
            int num = Convert.ToInt32(achievementData[4]);
            if (num == this.LocalPlayer.AuthID)
            {
                PlayerManager.GameScore[this.LocalPlayer.AuthID].AddCompleteAchievement(Convert.ToInt64(achievementData[0]), Convert.ToInt32(achievementData[1]), Convert.ToInt32(achievementData[2]), Convert.ToInt32(achievementData[3]));
            }
            else
            {
                try
                {
                    long achievement_id = Convert.ToInt64(achievementData[0]);
                    int user_id = Convert.ToInt32(achievementData[4]);
                    string name = Achievement.GetName(achievement_id);
                    ScorePlayer scorePlayer = PlayerManager.GameScore[user_id];
                    if (scorePlayer != null)
                    {
                        BattleChat.AchievementMessage(scorePlayer.UserName, name);
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError("[PlayerManager] CompleteAchievement Exception: " + ex.Message);
                }
            }
        }
        else if (actorID == this.LocalPlayer.playerID)
        {
            PlayerManager.GameScore[this.LocalPlayer.AuthID].AddCompleteAchievement(Convert.ToInt64(achievementData[0]), Convert.ToInt32(achievementData[1]), Convert.ToInt32(achievementData[2]), Convert.ToInt32(achievementData[3]));
        }
    }

    public void SetCameraTexture(Texture2D tex)
    {
        if ((UnityEngine.Object)this.displayObject != (UnityEngine.Object)null && this.displayObject.activeSelf)
        {
            this.displayObject.GetComponent<Renderer>().material.mainTexture = tex;
        }
    }

    public bool SetCameraTextureActive()
    {
        Configuration.DebugEnableRTTX = !Configuration.DebugEnableRTTX;
        if ((UnityEngine.Object)this.displayObject == (UnityEngine.Object)null)
        {
            this.displayObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            this.displayObject.transform.parent = GameHUD.Instance.BattleGUI.transform;
            this.displayObject.transform.localPosition = new Vector3(-600f, 0f, 0f);
            this.displayObject.transform.localEulerAngles = new Vector3(90f, 180f, 0f);
            this.displayObject.transform.localScale = new Vector3(25f, 25f, 25f);
            this.displayObject.layer = 31;
            this.displayObject.SetActive(false);
        }
        if (Configuration.DebugEnableRTTX)
        {
            Configuration.DebugEnableFps = true;
        }
        this.displayObject.SetActive(Configuration.DebugEnableRTTX);
        return Configuration.DebugEnableRTTX;
    }

    public void SetCameraRTTX(Vector3 pos)
    {
        RTTXCamera.SetPosition(new Vector3(19.61503f, -61.20974f, 284.8907f) + pos);
    }

    public void SetCameraRTTX(bool rttx)
    {
        RTTXCamera.TestCameraRTTX(rttx);
    }

    public void SetCameraTest(bool rttx)
    {
        RTTXCamera.TestCamera(rttx);
    }

    public void CheckFPSCamera()
    {
        if (!((UnityEngine.Object)this.LocalPlayer == (UnityEngine.Object)null))
        {
            FPSCamera component = ((Component)this.LocalPlayer.SoldierController.FPSCamera).GetComponent<FPSCamera>();
            object[] obj = new object[4] {
                component.CroachHeight,
                component.CroachWalkHeight,
                component.NormalHeight,
                null
            };
            Vector3 localPosition = component.transform.localPosition;
            obj[3] = localPosition.y;
            UnityEngine.Debug.LogError(string.Format("Crouch: {0}, CrouchWalk: {1}, NormalWalk {2} HEIGHT: {3}", obj));
        }
    }

    public void SetRTTXScene()
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(CharacterManager.Instance.GetPrefab("RTTX"));
        gameObject.transform.position = new Vector3(19.61503f, -61.20974f, 284.8907f);
    }

    public void SetEnemyTest(bool rttx)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(CharacterManager.Instance.GetPlayerEnemy());
        CombatPlayer combatPlayer = gameObject.AddComponent<CombatPlayer>();
        if (!rttx)
        {
            combatPlayer.transform.position = new Vector3(19.61503f, -61.20974f, 284.8907f);
        }
        else
        {
            combatPlayer.transform.position = new Vector3(2006.003f, -2003.261f, 2000.089f);
        }
        combatPlayer.PlayerCustomisator.enabled = false;
        Renderer[] componentsInChildren = combatPlayer.transform.GetComponentsInChildren<Renderer>(true);
        Renderer[] array = componentsInChildren;
        foreach (Renderer renderer in array)
        {
            if (renderer.GetType() == typeof(SkinnedMeshRenderer))
            {
                renderer.enabled = true;
                renderer.gameObject.SetActive(true);
            }
            else
            {
                renderer.enabled = false;
            }
        }
    }

    public void SetCharacterQuality(QualityLevel level)
    {
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator = this.Players.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CombatPlayer current = enumerator.Current;
                current.PlayerCustomisator.SetDiffuseShader(level != QualityLevel.Fast && level != QualityLevel.Fastest);
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        if ((UnityEngine.Object)this.LocalPlayer != (UnityEngine.Object)null)
        {
            this.LocalPlayer.ShotController.SetDiffuseShader(level != QualityLevel.Fast && level != QualityLevel.Fastest);
        }
    }

    public CombatPlayer GetPlayer(int actorID)
    {
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator = this.Players.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CombatPlayer current = enumerator.Current;
                if (current.AuthID == actorID)
                {
                    return current;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        return null;
    }

    public CombatPlayer GetPlayerByAuthID(int authID)
    {
        Dictionary<int, CombatPlayer>.ValueCollection.Enumerator enumerator = this.Players.Values.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CombatPlayer current = enumerator.Current;
                if (current.AuthID == authID)
                {
                    return current;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        return null;
    }

    private void Start()
    {
        this.photonConnection = PhotonConnection.Connection;
    }

    public NetworkTransform SendTransform(Transform transform, bool sendHeight, bool InAir, NetworkTransform lastStateSent)
    {
        ActorAnimator componentInChildren = ((Component)transform).GetComponentInChildren<ActorAnimator>();
        float num = 0.5f;
        if ((UnityEngine.Object)componentInChildren != (UnityEngine.Object)null)
        {
            num = componentInChildren.BendAnimationAngle;
        }
        Quaternion identity = Quaternion.identity;
        float x = num;
        Vector3 localEulerAngles = transform.localEulerAngles;
        identity.eulerAngles = new Vector3(x, localEulerAngles.y, 0f);
        NetworkTransform networkTransform = NetworkTransform.FromTransform(transform, identity);
        if (this.lastPosAdd > 0f)
        {
            this.lastPosAdd = 0f;
        }
        else
        {
            this.lastPosAdd = 0.0001f;
            NetworkTransform networkTransform2 = networkTransform;
            Vector3 position = networkTransform.Position;
            float x2 = position.x;
            Vector3 position2 = networkTransform.Position;
            float y = position2.y;
            Vector3 position3 = networkTransform.Position;
            networkTransform2.Position = new Vector3(x2, y, position3.z + this.lastPosAdd);
        }
        if (NetworkDev.TestPosition)
        {
            networkTransform = lastStateSent;
        }
        long num2 = networkTransform.TimeStamp = TimeManager.Instance.NetworkTime;
        if (sendHeight && InAir)
        {
            Vector3 vector = ShotCalculator.DownScan(base.transform.position);
            NetworkTransform networkTransform3 = networkTransform;
            Vector3 speed = networkTransform.Speed;
            float x3 = speed.x;
            Vector3 speed2 = networkTransform.Speed;
            networkTransform3.Speed = new Vector3(x3, speed2.y, vector.y);
            PlayerManager.Instance.SendTransform(networkTransform, true);
        }
        else
        {
            PlayerManager.Instance.SendTransform(networkTransform);
        }
        return networkTransform;
    }

    public void SendTransform(NetworkTransform ntransform)
    {
        this.SendTransform(ntransform, false);
    }

    public void SendTransform(NetworkTransform ntransform, bool sendHeight)
    {
        if (GameHUD.Instance.PlayerState != GameHUD.PlayerStates.Play && GameHUD.Instance.PlayerState != GameHUD.PlayerStates.Zombie_Boss_Infection)
        {
            return;
        }
        Hashtable hashtable = ntransform.ToHashtable(sendHeight);
        if (DateTime.Now.Ticks - this.lastLagSend > 50000000 && Lagometer.LagValue >= 0)
        {
            int num = Lagometer.LagValue;
            if (num > 255)
            {
                num = 255;
            }
            hashtable[(byte)53] = (byte)num;
            this.lastLagSend = DateTime.Now.Ticks;
        }
        else if (DateTime.Now.Ticks - this.lastPingSend > 20000000)
        {
            this.lastPingSend = DateTime.Now.Ticks;
            hashtable[(byte)81] = (short)TimeManager.Instance.AveragePing;
        }
        if (PlayerManager.Instance.LocalPlayer.SoldierController.LSTCCounter > 5)
        {
            hashtable[(byte)49] = (short)PlayerManager.Instance.LocalPlayer.SoldierController.LSTCCounter;
        }
        if (PlayerManager.Instance.LocalPlayer.CharacterMotor.LSTCCounter > 5)
        {
            hashtable[(byte)48] = (short)PlayerManager.Instance.LocalPlayer.CharacterMotor.LSTCCounter;
        }
        if (this.photonConnection != null)
        {
            bool reliable = false;
            if (NetworkDev.TCP_TPS != 0 && (NetworkDev.TCP_TPS == NetworkDev.TPS || DateTime.Now.Ticks - this.lastTCPTransformSend > NetworkDev.TCP_TPS * 10000))
            {
                this.lastTCPTransformSend = DateTime.Now.Ticks;
                reliable = true;
            }
            this.photonConnection.SendRequest(FUFPSOpCode.Move, hashtable, reliable);
        }
    }

    private void CheckData(Hashtable data)
    {
        Vector3 position = this.LocalPlayer.transform.position;
        Vector3 vector = default(Vector3);
        vector.x = (float)data[(byte)1] - position.x;
        vector.y = (float)data[(byte)2] - position.y;
        vector.z = (float)data[(byte)3] - position.z;
        if (vector.sqrMagnitude > 10f)
        {
            PlayerManager.Instance.SendEnterBaseRequest(32);
        }
    }

    public void SendEnterBaseRequest(int team)
    {
        Hashtable hashtable = new Hashtable();
        hashtable[(byte)239] = team;
        if (this.photonConnection != null)
        {
            this.photonConnection.SendRequest(FUFPSOpCode.BaseEnter, hashtable);
        }
    }

    public void SendEnterBaseRequest(int team, Hashtable eventData)
    {
        Hashtable hashtable = new Hashtable();
        hashtable[(byte)239] = team;
        hashtable[(byte)98] = eventData;
        if (this.photonConnection != null)
        {
            this.photonConnection.SendRequest(FUFPSOpCode.BaseEnter, hashtable);
        }
    }

    public void SendEnterBaseRequest(int team, int actorID)
    {
        Hashtable hashtable = new Hashtable();
        hashtable[(byte)239] = team;
        hashtable[(byte)97] = actorID;
        if (this.photonConnection != null)
        {
            this.photonConnection.SendRequest(FUFPSOpCode.BaseEnter, hashtable);
        }
        CombatPlayer player = PlayerManager.Instance.GetPlayer(actorID);
        int num = -1;
        if ((UnityEngine.Object)player != (UnityEngine.Object)null)
        {
            num = player.AuthID;
        }
    }

    public void SendEnterBaseRequest(int team, int actorID, byte eventCode)
    {
        Hashtable hashtable = new Hashtable();
        hashtable[(byte)239] = team;
        hashtable[(byte)97] = actorID;
        hashtable[(byte)98] = eventCode;
        if (this.photonConnection != null)
        {
            this.photonConnection.SendRequest(FUFPSOpCode.BaseEnter, hashtable);
        }
        CombatPlayer player = PlayerManager.Instance.GetPlayer(actorID);
        int num = -1;
        if ((UnityEngine.Object)player != (UnityEngine.Object)null)
        {
            num = player.AuthID;
        }
    }
}


