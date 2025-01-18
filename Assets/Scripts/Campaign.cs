// ILSpyBased#2
using System.Collections.Generic;
using UnityEngine;

public class Campaign : MonoBehaviour
{
    public delegate CombatPlayer ActivateActor(GameObject actionPrefab, long launchTime);

    private Dictionary<int, ActionWave> actionWaves;

    private Dictionary<int, CombatPlayer> actors;

    private int progress = 100;

    private int actorCounter;

    private int enemiesCount;

    private int currentWaveKey;

    public short WaveIndex;

    public ActivateActor ActivateActorListener;

    public Dictionary<int, ActionWave> ActionWaves
    {
        get
        {
            return this.actionWaves;
        }
    }

    public Dictionary<int, CombatPlayer> Actors
    {
        get
        {
            if (this.actors == null)
            {
                this.actors = new Dictionary<int, CombatPlayer>();
            }
            return this.actors;
        }
    }

    public int Progress
    {
        get
        {
            return this.progress;
        }
        set
        {
            this.progress = value;
        }
    }

    public int EnemiesCount
    {
        get
        {
            return this.enemiesCount;
        }
        set
        {
            this.enemiesCount = value;
        }
    }

    public short MaxWaves
    {
        get
        {
            return 8;
        }
    }

    private void Start()
    {
        this.Init();
    }

    protected void Init()
    {
        this.currentWaveKey = 0;
    }

    public bool AddWave(GameObject actionPrefab, long launchTime, int trajectoryIndex, int count, ActionType actionType, short team)
    {
        if (this.actionWaves == null)
        {
            this.actionWaves = new Dictionary<int, ActionWave>();
        }
        this.actionWaves.Add(this.actionWaves.Count + 1, new ActionWave(actionPrefab, launchTime, trajectoryIndex, count, actionType, team));
        return true;
    }

    public bool AddActor(short team, ActionType actionType, GameObject actionPrefab, long launchTime)
    {
        CombatPlayer combatPlayer = this.ActivateActorListener(actionPrefab, launchTime);
        if ((Object)combatPlayer == (Object)null)
        {
            return false;
        }
        combatPlayer.playerID = this.actorCounter++;
        combatPlayer.Team = team;
        if (this.actors == null)
        {
            this.actors = new Dictionary<int, CombatPlayer>();
        }
        this.actors.Add(combatPlayer.playerID, combatPlayer);
        return true;
    }

    private void ActivateWave(ActionWave actionWave)
    {
        if (this.ActivateActorListener != null)
        {
            CombatPlayer combatPlayer = null;
            for (int i = 0; i < actionWave.count; i++)
            {
                this.AddActor(actionWave.team, actionWave.actionType, actionWave.actionPrefab, actionWave.launchTime);
            }
        }
    }

    private void FixedUpdate()
    {
        return;
        IL_0001:;
    }

    public void ResetCampaign(long time)
    {
        UnityEngine.Debug.LogError(string.Format("[Campaign] ResetCampaign(long time={0})", time));
        if (this.actionWaves != null)
        {
            this.actionWaves.Clear();
            UnityEngine.Debug.Log("[Campaign] ResetCampaign actionWaves != null");
        }
        if (this.actors != null)
        {
            this.actors.Clear();
            UnityEngine.Debug.Log("[Campaign] ResetCampaign actors != null");
        }
        this.progress = 100;
        this.WaveIndex = 0;
        this.enemiesCount = 0;
    }
}


