// ILSpyBased#2
using System;
using UnityEngine;

public class FragKill
{
    public enum FragType
    {
        None,
        Domination,
        Revenge,
        Assist
    }

    private int killerId;

    private string killerShowedName;

    private string killerName;

    private string killerClanTag;

    private int killerTeam;

    private bool killerIsFriend;

    private int killedId;

    private string killedShowedName;

    private string killedName;

    private string killedClanTag;

    private int killedTeam;

    private bool killedIsFriend;

    private int assistantId = -1;

    private string assistantShowedName;

    private string assistantName;

    private string assistantClanTag;

    private int assistantTeam;

    private bool assistantIsFriend;

    private WeaponType weaponType;

    private string weaponSName;

    private FragType fragType;

    private float timeKill;

    private PlayerHitZone playerHitZone;

    private int killerHealth;

    private int killerEnergy;

    private int weaponID;

    private Weapon weapon;

    public int KillerID
    {
        get
        {
            return this.killerId;
        }
    }

    public string KillerShowedName
    {
        get
        {
            return this.killerShowedName;
        }
    }

    public string KillerClanTag
    {
        get
        {
            return this.killerClanTag;
        }
    }

    public int KillerTeam
    {
        get
        {
            return this.killerTeam;
        }
    }

    public bool KillerIsFriend
    {
        get
        {
            return this.killerIsFriend;
        }
    }

    public int KilledID
    {
        get
        {
            return this.killedId;
        }
    }

    public string KilledShowedName
    {
        get
        {
            return this.killedShowedName;
        }
    }

    public string KilledClanTag
    {
        get
        {
            return this.killedClanTag;
        }
    }

    public int KilledTeam
    {
        get
        {
            return this.killedTeam;
        }
    }

    public bool KilledIsFriend
    {
        get
        {
            return this.killedIsFriend;
        }
    }

    public int AssistantID
    {
        get
        {
            return this.assistantId;
        }
    }

    public string AssistantShowedName
    {
        get
        {
            return this.assistantShowedName;
        }
    }

    public string AssistantClanTag
    {
        get
        {
            return this.assistantClanTag;
        }
    }

    public int AssistantTeam
    {
        get
        {
            return this.assistantTeam;
        }
    }

    public bool AssistantIsFriend
    {
        get
        {
            return this.assistantIsFriend;
        }
    }

    public WeaponType WeaponType
    {
        get
        {
            return this.weaponType;
        }
    }

    public string WeaponSystemName
    {
        get
        {
            return this.weaponSName;
        }
    }

    public FragType FType
    {
        get
        {
            return this.fragType;
        }
    }

    public float TimeKill
    {
        get
        {
            return this.timeKill;
        }
    }

    public PlayerHitZone PlayerHitZone
    {
        get
        {
            return this.playerHitZone;
        }
    }

    public int KillerHealth
    {
        get
        {
            return this.killerHealth;
        }
    }

    public int KillerEnergy
    {
        get
        {
            return this.killerEnergy;
        }
    }

    public int WeaponID
    {
        get
        {
            return this.weaponID;
        }
    }

    public Weapon Weapon
    {
        get
        {
            return this.weapon;
        }
    }

    public FragKill(int killerId, string killerName, string killerClanTag, int killer_team, int killedId, string killedName, string killedClanTag, int killed_team, WeaponType weaponType, FragType type)
    {
        this.killerId = killerId;
        this.killerName = BadWorldFilter.CheckLite(killerName);
        this.killerClanTag = killerClanTag;
        this.killerTeam = killer_team;
        this.killedId = killedId;
        this.killedName = BadWorldFilter.CheckLite(killedName);
        this.killedClanTag = killedClanTag;
        this.killedTeam = killed_team;
        this.fragType = type;
        this.weaponType = weaponType;
        this.timeKill = Time.time;
        this.killedShowedName = ((!(this.killedClanTag == string.Empty)) ? string.Format("[{0}] {1}", this.killedClanTag, this.killedName) : this.killedName);
        this.killerShowedName = ((!(this.killerClanTag == string.Empty)) ? string.Format("[{0}] {1}", this.killerClanTag, this.killerName) : this.killerName);
        if (MasterServerNetworkController.Instance != null)
        {
            this.killerIsFriend = MasterServerNetworkController.IsFriend(killerId);
            this.killedIsFriend = MasterServerNetworkController.IsFriend(killedId);
        }
    }

    public FragKill(int killerId, string killerName, string killerClanTag, int killer_team, int killedId, string killedName, string killedClanTag, int killed_team, int weaponID, WeaponType weaponType, string weaponSystemName, PlayerHitZone playerHitZone, int killerHealth, int killerEnergy, FragType type)
    {
        this.killerId = killerId;
        this.killerName = BadWorldFilter.CheckLite(killerName);
        this.killerClanTag = killerClanTag;
        this.killerTeam = killer_team;
        this.killedId = killedId;
        this.killedName = BadWorldFilter.CheckLite(killedName);
        this.killedClanTag = killedClanTag;
        this.killedTeam = killed_team;
        this.fragType = type;
        if (weaponID != -1)
        {
            this.weapon = new Weapon(Convert.ToInt32(weaponID), weaponType, weaponSystemName);
        }
        this.weaponID = weaponID;
        this.weaponType = weaponType;
        this.weaponSName = weaponSystemName;
        this.playerHitZone = playerHitZone;
        this.killerHealth = killerHealth;
        this.killerEnergy = killerEnergy;
        this.timeKill = Time.time;
        this.killedShowedName = ((!(this.killedClanTag == string.Empty)) ? string.Format("[{0}] {1}", this.killedClanTag, this.killedName) : this.killedName);
        this.killerShowedName = ((!(this.killerClanTag == string.Empty)) ? string.Format("[{0}] {1}", this.killerClanTag, this.killerName) : this.killerName);
        if (MasterServerNetworkController.Instance != null)
        {
            this.killerIsFriend = MasterServerNetworkController.IsFriend(killerId);
            this.killedIsFriend = MasterServerNetworkController.IsFriend(killedId);
        }
    }

    public void SetAssistant(int assistantId, string assistantName, string assistantClanTag, int assistant_team)
    {
        if (assistantId != -1)
        {
            this.assistantId = assistantId;
            this.assistantName = BadWorldFilter.CheckLite(assistantName);
            this.assistantClanTag = assistantClanTag;
            this.assistantTeam = assistant_team;
            this.fragType = FragType.Assist;
            this.assistantShowedName = ((!(this.assistantClanTag == string.Empty)) ? string.Format("[{0}] {1}", this.assistantClanTag, this.assistantName) : this.assistantName);
            if (MasterServerNetworkController.Instance != null)
            {
                this.assistantIsFriend = MasterServerNetworkController.IsFriend(assistantId);
            }
        }
    }
}


