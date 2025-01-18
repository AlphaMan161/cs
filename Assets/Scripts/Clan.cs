// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Clan
{
    private ClanArm arm;

    private int clanId;

    private short lvl;

    private int founderId;

    private string founderName = string.Empty;

    private int access;

    private int accessLvl;

    private short memberCount;

    private short maxMemberCount = 5;

    private bool isMaxMemberCount;

    private int exp;

    private int money;

    private string name = string.Empty;

    private string tag = string.Empty;

    private string tagColor = string.Empty;

    private short armsId;

    private string homepage = string.Empty;

    private string desc = string.Empty;

    private bool sendInvite;

    private bool fullLoad;

    private ClanTreasuryEvent lastDonate;

    private ClanTreasuryEvent bestDonate;

    private List<ClanEvent> events = new List<ClanEvent>();

    private List<ClanTreasuryEvent> treasuryEvents = new List<ClanTreasuryEvent>();

    public ClanInventory Inventory;

    private List<ClanMember> members = new List<ClanMember>();

    private bool memberIsLoading;

    private List<ClanMember> invites = new List<ClanMember>();

    private bool invitesIsLoading;

    public ClanArm Arm
    {
        get
        {
            if (this.armsId != 0 && this.arm == null)
            {
                this.arm = ClanArmManager.GetArm(this.armsId);
            }
            return this.arm;
        }
    }

    public int ClanID
    {
        get
        {
            return this.clanId;
        }
    }

    public short Level
    {
        get
        {
            return this.lvl;
        }
    }

    public int FounderID
    {
        get
        {
            return this.founderId;
        }
    }

    public string FounderName
    {
        get
        {
            return this.founderName;
        }
    }

    public int Access
    {
        get
        {
            return this.access;
        }
        set
        {
            this.access = value;
        }
    }

    public int AccessLvl
    {
        get
        {
            return this.accessLvl;
        }
        set
        {
            this.accessLvl = value;
        }
    }

    public short MemberCount
    {
        get
        {
            return this.memberCount;
        }
    }

    public short MaxMemberCount
    {
        get
        {
            return this.maxMemberCount;
        }
        set
        {
            this.maxMemberCount = value;
            if (this.maxMemberCount >= 50)
            {
                this.isMaxMemberCount = true;
            }
        }
    }

    public bool IsMaxMemberCount
    {
        get
        {
            return this.isMaxMemberCount;
        }
    }

    public int Exp
    {
        get
        {
            return this.exp;
        }
    }

    public int Money
    {
        get
        {
            return this.money;
        }
        set
        {
            this.money = value;
        }
    }

    public string Name
    {
        get
        {
            return this.name;
        }
        set
        {
            this.name = value;
        }
    }

    public string Tag
    {
        get
        {
            return this.tag;
        }
        set
        {
            this.tag = value;
        }
    }

    public string TagColor
    {
        get
        {
            return this.tagColor;
        }
    }

    public short ArmsID
    {
        get
        {
            return this.armsId;
        }
        set
        {
            this.armsId = value;
            this.arm = null;
        }
    }

    public string Homepage
    {
        get
        {
            return this.homepage;
        }
        set
        {
            this.homepage = value;
        }
    }

    public string Desc
    {
        get
        {
            return this.desc;
        }
        set
        {
            this.desc = value;
        }
    }

    public bool SendInvite
    {
        get
        {
            return this.sendInvite;
        }
        set
        {
            this.sendInvite = value;
        }
    }

    public bool FullLoad
    {
        get
        {
            return this.fullLoad;
        }
    }

    public ClanTreasuryEvent LastDonate
    {
        get
        {
            return this.lastDonate;
        }
    }

    public ClanTreasuryEvent BestDonate
    {
        get
        {
            return this.bestDonate;
        }
    }

    public List<ClanEvent> Events
    {
        get
        {
            return this.events;
        }
    }

    public List<ClanTreasuryEvent> TreasuryEvents
    {
        get
        {
            return this.treasuryEvents;
        }
    }

    public List<ClanMember> Members
    {
        get
        {
            if (!this.memberIsLoading)
            {
                this.LoadClanMembers();
            }
            return this.members;
        }
    }

    public List<ClanMember> Invites
    {
        get
        {
            if (!this.invitesIsLoading)
            {
                this.LoadClanInvites();
            }
            return this.invites;
        }
    }

    public Clan(JSONNode json)
    {
        this.Update(json);
    }

    public void Update(JSONNode json)
    {
        if (json["cid"] != (object)null)
        {
            this.clanId = json["cid"].AsInt;
        }
        if (json["l"] != (object)null)
        {
            this.lvl = Convert.ToInt16(json["l"].AsInt);
        }
        if (json["fid"] != (object)null)
        {
            this.founderId = json["fid"].AsInt;
        }
        if (json["fn"] != (object)null)
        {
            this.founderName = json["fn"].Value;
        }
        if (json["a"] != (object)null)
        {
            this.access = json["a"].AsInt;
        }
        if (json["alvl"] != (object)null)
        {
            this.accessLvl = json["alvl"].AsInt;
        }
        if (json["mcnt"] != (object)null)
        {
            this.memberCount = Convert.ToInt16(json["mcnt"].AsInt);
        }
        if (json["macnt"] != (object)null)
        {
            this.MaxMemberCount = Convert.ToInt16(json["macnt"].AsInt);
        }
        if (json["e"] != (object)null)
        {
            this.exp = json["e"].AsInt;
        }
        if (json["n"] != (object)null)
        {
            this.name = json["n"].Value;
        }
        if (json["t"] != (object)null)
        {
            this.tag = json["t"].Value;
        }
        if (json["tc"] != (object)null)
        {
            this.tagColor = json["tc"].Value;
        }
        if (json["aid"] != (object)null)
        {
            this.armsId = Convert.ToInt16(json["aid"].AsInt);
        }
        if (json["h"] != (object)null)
        {
            this.fullLoad = true;
            this.homepage = json["h"].Value;
        }
        if (json["d"] != (object)null)
        {
            this.fullLoad = true;
            this.desc = json["d"].Value;
        }
        if (json["mlist"] != (object)null)
        {
            this.memberIsLoading = true;
            this.InitMembers(json["mlist"]);
        }
        if (json["vc"] != (object)null)
        {
            this.money = json["vc"].AsInt;
        }
        if (!(json["inv"] != (object)null))
        {
            goto IL_0326;
        }
        goto IL_0326;
        IL_0326:
        if (json["ev"] != (object)null)
        {
            this.InitEvents(json["ev"]);
        }
        if (json["etreas"] != (object)null)
        {
            this.InitTreasuryEvents(json["etreas"]);
        }
        if (json["inventory"] != (object)null && json["inventory"]["items"] != (object)null)
        {
            this.Inventory = new ClanInventory(json["inventory"]["items"]);
        }
    }

    public void InitEvents(JSONNode json)
    {
        this.events = ClanEvent.FromArray(json);
    }

    public void InitTreasuryEvents(JSONNode json)
    {
        this.treasuryEvents = ClanTreasuryEvent.FromArray(json);
        this.treasuryEvents.Sort((ClanTreasuryEvent x, ClanTreasuryEvent y) => y.EventId.CompareTo(x.EventId));
        this.lastDonate = null;
        this.bestDonate = null;
        List<ClanTreasuryEvent>.Enumerator enumerator = this.treasuryEvents.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ClanTreasuryEvent current = enumerator.Current;
                if (this.lastDonate == null && current.Type == ClanTreasuryEventType.Add)
                {
                    this.lastDonate = current;
                }
                if (current.Type == ClanTreasuryEventType.Add)
                {
                    if (this.bestDonate == null)
                    {
                        this.bestDonate = current;
                    }
                    else if (this.bestDonate.Money < current.Money)
                    {
                        this.bestDonate = current;
                    }
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public override string ToString()
    {
        return string.Format("[Clan: ClanID={0}, Level={1}, FounderID={2}, Access={3}, MemberCount={4}, MaxMemberCount={5}, Exp={6}, Money={7}, Name={8}, Tag={9}, TagColor={10}, ArmsID={11}, Homepage={12}]", this.ClanID, this.Level, this.FounderID, this.Access, this.MemberCount, this.MaxMemberCount, this.Exp, this.Money, this.Name, this.Tag, this.TagColor, this.ArmsID, this.Homepage);
    }

    private void LoadClanMembers()
    {
        if (!this.memberIsLoading)
        {
            this.memberIsLoading = true;
            Ajax.Request(string.Format("{0}&cid={1}", WebUrls.CLAN_MEMBERS_URL, this.ClanID), new AjaxRequest.AjaxHandler(this.OnLoadMembers));
        }
    }

    private void InitMembers(JSONNode json)
    {
        this.members.Clear();
        foreach (JSONNode child in json.Childs)
        {
            ClanMember item = new ClanMember(child);
            this.members.Add(item);
        }
        this.members = (from x in this.members
        orderby x.ClanExp descending
        select x).ToList();
        this.memberCount = Convert.ToInt16(this.members.Count);
    }

    private void OnLoadMembers(object result, AjaxRequest request)
    {
        JSONNode jSONNode = JSONNode.Parse(Ajax.DecodeUtf(result.ToString()));
        if (jSONNode["result"] != (object)null && jSONNode["mlist"] != (object)null)
        {
            this.InitMembers(jSONNode["mlist"]);
        }
    }

    public bool AddMember(ClanMember member)
    {
        ClanMember clanMember = this.members.Find((ClanMember x) => x.UserID == member.UserID);
        if (clanMember != null)
        {
            return false;
        }
        this.members.Add(member);
        this.memberCount++;
        this.members = (from x in this.members
        orderby x.ClanExp descending
        select x).ToList();
        return true;
    }

    public bool RemoveInvite(int user_id)
    {
        ClanMember clanMember = this.invites.Find((ClanMember x) => x.UserID == user_id);
        if (clanMember != null)
        {
            if (this.invites.Count <= 1)
            {
                ClanManager.NewRequests = false;
            }
            return this.invites.Remove(clanMember);
        }
        return false;
    }

    public bool RemoveMember(int user_id)
    {
        ClanMember clanMember = this.members.Find((ClanMember x) => x.UserID == user_id);
        if (clanMember != null)
        {
            this.memberCount--;
            this.exp -= Convert.ToInt32(clanMember.ClanExp);
            UnityEngine.Debug.LogError("Try Remove member");
            return this.members.Remove(clanMember);
        }
        return false;
    }

    public bool RemoveEvent(int event_id)
    {
        ClanEvent clanEvent = this.events.Find((ClanEvent x) => x.EventId == event_id);
        if (clanEvent != null)
        {
            this.events.Remove(clanEvent);
        }
        return false;
    }

    private void LoadClanInvites()
    {
        if (!this.invitesIsLoading)
        {
            this.invitesIsLoading = true;
            Ajax.Request(string.Format("{0}&cid={1}", WebUrls.CLAN_INIVTES_URL, this.ClanID), new AjaxRequest.AjaxHandler(this.OnLoadInvites));
        }
    }

    public void RefreshInvites()
    {
        if (this.invites.Count >= 100)
        {
            UnityEngine.Debug.Log("[Clan] Disable refresh because is limit");
        }
        else
        {
            this.invitesIsLoading = false;
            this.LoadClanInvites();
        }
    }

    private void OnLoadInvites(object result, AjaxRequest request)
    {
        JSONNode jSONNode = JSONNode.Parse(Ajax.DecodeUtf(result.ToString()));
        if (jSONNode["result"] != (object)null && jSONNode["inv"] != (object)null)
        {
            this.InitInvites(jSONNode["inv"]);
        }
    }

    private void InitInvites(JSONNode json)
    {
        this.invites.Clear();
        foreach (JSONNode child in json.Childs)
        {
            ClanMember item = new ClanMember(child);
            this.invites.Add(item);
        }
        if (this.invites.Count > 0 && LocalUser.Clan != null && LocalUser.Clan.ClanID == this.ClanID)
        {
            ClanManager.NewRequests = true;
        }
    }

    public bool SetKoef(int user_id, int koef)
    {
        ClanMember clanMember = this.members.Find((ClanMember x) => x.UserID == user_id);
        if (clanMember == null)
        {
            return false;
        }
        clanMember.ClanExpKoef = Convert.ToUInt16(koef);
        return true;
    }

    public bool SetOwner(int new_owner_id)
    {
        ClanMember clanMember = this.members.Find((ClanMember x) => x.UserID == new_owner_id);
        if (clanMember == null)
        {
            return false;
        }
        this.founderId = new_owner_id;
        this.founderName = clanMember.Name;
        return true;
    }

    public bool AddTreasuryEvent(int eventId, int clan_id, int user_id, string user_name, int money, ClanTreasuryEventType type, string date)
    {
        this.treasuryEvents.Add(new ClanTreasuryEvent(eventId, clan_id, user_id, user_name, money, type, date));
        this.treasuryEvents.Sort((ClanTreasuryEvent x, ClanTreasuryEvent y) => y.EventId.CompareTo(x.EventId));
        this.lastDonate = this.treasuryEvents[0];
        return true;
    }
}


