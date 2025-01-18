// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortMachine : MonoBehaviour
{
    public delegate void onProcessEscortMachinesDelegate(EscortMachineEvent escortMachineEvent);

    public Vector3 position;

    public int value;

    public ItemType type;

    public int respawn;

    public int power;

    private float escortProgress;

    private float escortPosition;

    private short escortTeam;

    private short oppositeTeam;

    private List<CombatPlayer> activePlayers = new List<CombatPlayer>();

    public Trajectory Trajectory;

    private float escortSpeed = 0.5f;

    private float escortProgressStep;

    private float escortLength;

    private bool isDead;

    private ServerNavigationController serverNavigationController;

    private EscortMachineState state;

    private byte[] escortCompetitionTeams;

    private short index;

    public short Team
    {
        get
        {
            return this.escortTeam;
        }
    }

    public EscortMachineState State
    {
        get
        {
            return this.state;
        }
        set
        {
            this.state = value;
        }
    }

    public short Index
    {
        get
        {
            return this.index;
        }
    }

    public bool Moving
    {
        get
        {
            return this.state == EscortMachineState.Moving;
        }
    }

    public void Reset(float progress)
    {
        this.state = EscortMachineState.None;
        this.escortProgress = progress;
        this.position = this.Trajectory.GetPosition(this.escortProgress);
        base.transform.position = this.position;
        this.serverNavigationController.Reset();
    }

    public int GetTeam()
    {
        return this.escortTeam;
    }

    public float GetProgress()
    {
        return this.escortProgress;
    }

    public void SetProgress(float escortProgress)
    {
        this.escortProgress = escortProgress;
    }

    private float Distance(CombatPlayer player)
    {
        Vector3 vector = player.transform.position;
        float x = vector.x;
        Vector3 vector2 = player.transform.position;
        float y = vector2.y;
        Vector3 vector3 = player.transform.position;
        Vector3 b = new Vector3(x, y, vector3.z);
        Vector3 a = new Vector3(this.position.x, this.position.y, this.position.z);
        return (a - b).sqrMagnitude;
    }

    public object Clone()
    {
        return base.MemberwiseClone();
    }

    public void SetupEscortMachine(Hashtable escortMachineData, Trajectory trajectory)
    {
        this.index = (short)escortMachineData[(byte)13];
        if (escortMachineData.ContainsKey((byte)239))
        {
            this.escortTeam = (short)escortMachineData[(byte)239];
            this.oppositeTeam = Convert.ToInt16((this.escortTeam != 1) ? 1 : 2);
        }
        else
        {
            UnityEngine.Debug.LogError("[Escort Machine.cs] No Escort Team!");
        }
        if (escortMachineData.ContainsKey((byte)12))
        {
            this.state = (EscortMachineState)(byte)escortMachineData[(byte)12];
            this.escortProgress = (float)escortMachineData[(byte)11];
        }
        this.Trajectory = trajectory;
        this.position = this.Trajectory.GetPosition(this.escortProgress);
        this.escortLength = this.Trajectory.TrajectoryLength;
        this.escortProgressStep = this.escortSpeed / this.Trajectory.TrajectoryLength;
        base.transform.position = this.position;
        this.serverNavigationController = ((Component)base.transform).GetComponent<ServerNavigationController>();
        NetworkTransform networkTransform = NetworkTransform.FromPoint(this.position, new Vector3(0f, 0f, 0f));
        networkTransform.TimeStamp = TimeManager.Instance.NetworkTime;
        this.serverNavigationController.StartReceiving();
        this.serverNavigationController.ReceiveTransform(networkTransform, true);
    }

    public Hashtable ToHashtable()
    {
        Hashtable hashtable = new Hashtable();
        hashtable[(byte)13] = this.Index;
        if (this.escortTeam > 0)
        {
            hashtable[(byte)239] = this.escortTeam;
        }
        if (this.state != 0)
        {
            hashtable[(byte)12] = this.state;
            hashtable[(byte)11] = this.escortProgress;
        }
        return hashtable;
    }

    public void Move(float progress)
    {
        this.escortProgress = progress;
        this.position = this.Trajectory.GetPosition(this.escortProgress);
        NetworkTransform networkTransform = NetworkTransform.FromPoint(this.position, new Vector3(0f, 0f, 0f));
        networkTransform.TimeStamp = TimeManager.Instance.NetworkTime;
        this.serverNavigationController.ReceiveTransform(networkTransform, true);
    }

    public Vector3 GetPosition(long time)
    {
        return this.Trajectory.GetPosition(this.escortProgress);
    }

    public Hashtable GetProgressHashtable()
    {
        Hashtable hashtable = new Hashtable();
        hashtable[(byte)13] = this.Index;
        hashtable[(byte)11] = this.escortProgress;
        return hashtable;
    }
}


