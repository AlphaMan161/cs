// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ClanManager
{
    public static int COST_CLAN_CREATE = 1500;

    public static int[] COST_EXPAN_CLAN_MEMBER = new int[9] {
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9
    };

    public static int COST_EXPAND_CLAN_MEMBER_CURRENT = 1000;

    public static int COST_REQUESTS = 500;

    public static int COST_CHANGE_NAME = 1500;

    public static int COST_CHANGE_TAG = 1500;

    private static ClanManager hInstance = null;

    private static object syncLook = new object();

    private int serverTime;

    private Clan selectedClan;

    private uint preSelectClan;

    private short clan_used_request;

    private short clan_max_request = 10;

    private List<int> localInvites = new List<int>();

    private Dictionary<int, Clan> clanList = new Dictionary<int, Clan>();

    private Dictionary<int, ClanArm> arms = new Dictionary<int, ClanArm>();

    private bool newRequests;

    private Dictionary<int, ClanList> clanByPage = new Dictionary<int, ClanList>();

    private int maxPage = 1;

    private int selectedPage = 1;

    private ErrorInfo.CODE currentLastError;

    private static bool joinInProgress = false;

    private bool buyRequestInProgress;

    private string[] availKoef = new string[4] {
        "0",
        "25",
        "50",
        "75"
    };

    private int selectedIndexKoef;

    private bool isSearchResult;

    private bool isSearchInProgress;

    private string searchValue = string.Empty;

    private Dictionary<string, ClanList> searchResults = new Dictionary<string, ClanList>();

    private static ClanManager Instance
    {
        get
        {
            if (ClanManager.hInstance == null)
            {
                object obj = ClanManager.syncLook;
                Monitor.Enter(obj);
                try
                {
                    if (ClanManager.hInstance == null)
                    {
                        ClanManager.hInstance = new ClanManager();
                    }
                }
                finally
                {
                    Monitor.Exit(obj);
                }
            }
            return ClanManager.hInstance;
        }
    }

    public static int ServerTime
    {
        get
        {
            return ClanManager.Instance.serverTime;
        }
    }

    public static Clan SelectedClan
    {
        get
        {
            return ClanManager.Instance.selectedClan;
        }
    }

    public static uint PreSelectClan
    {
        get
        {
            return ClanManager.Instance.preSelectClan;
        }
        set
        {
            ClanManager.Instance.preSelectClan = value;
        }
    }

    public static short ClanUsedRequest
    {
        get
        {
            return ClanManager.Instance.clan_used_request;
        }
        set
        {
            ClanManager.Instance.clan_used_request = value;
        }
    }

    public static short ClanMaxRequest
    {
        get
        {
            return ClanManager.Instance.clan_max_request;
        }
        set
        {
            ClanManager.Instance.clan_max_request = value;
        }
    }

    public static List<int> LocalInvites
    {
        get
        {
            return ClanManager.Instance.localInvites;
        }
    }

    public static Dictionary<int, Clan> ClanList
    {
        get
        {
            return ClanManager.Instance.clanList;
        }
    }

    public static bool NewRequests
    {
        get
        {
            return ClanManager.Instance.newRequests;
        }
        set
        {
            ClanManager.Instance.newRequests = value;
        }
    }

    public static int MaxPage
    {
        get
        {
            return ClanManager.Instance.maxPage;
        }
    }

    public static int SelectedPage
    {
        get
        {
            return ClanManager.Instance.selectedPage;
        }
        set
        {
            ClanManager.Instance.selectedPage = value;
            if (!ClanManager.Instance.clanByPage.ContainsKey(ClanManager.Instance.selectedPage))
            {
                ClanManager.Instance.clanByPage.Add(ClanManager.Instance.selectedPage, new ClanList());
                Ajax.Request(string.Format("{0}&pg={1}&lite={2}", WebUrls.CLAN_GET_URL, ClanManager.Instance.selectedPage, 1), new AjaxRequest.AjaxHandler(ClanManager.Instance.OnInit));
            }
        }
    }

    public static ClanList SelectedClanList
    {
        get
        {
            if (ClanManager.Instance.clanByPage.ContainsKey(ClanManager.Instance.selectedPage))
            {
                return ClanManager.Instance.clanByPage[ClanManager.Instance.selectedPage];
            }
            return null;
        }
    }

    public static ErrorInfo.CODE CurrentLastError
    {
        get
        {
            return ClanManager.Instance.currentLastError;
        }
    }

    public static bool BuyRequestInProgress
    {
        get
        {
            return ClanManager.Instance.buyRequestInProgress;
        }
    }

    public static string[] AvailableKoef
    {
        get
        {
            return ClanManager.Instance.availKoef;
        }
    }

    public static int SelectedIndexKoef
    {
        get
        {
            return ClanManager.Instance.selectedIndexKoef;
        }
        set
        {
            ClanManager.Instance.selectedIndexKoef = value;
            ClanManager.ChangeKoef(LocalUser.Clan.ClanID, Convert.ToInt32(ClanManager.AvailableKoef[value]));
        }
    }

    public static bool IsSearchResult
    {
        get
        {
            return ClanManager.Instance.isSearchResult;
        }
        set
        {
            ClanManager.Instance.isSearchResult = value;
        }
    }

    public static bool IsSearchInProgess
    {
        get
        {
            return ClanManager.Instance.isSearchInProgress;
        }
    }

    public static ClanList SearchResult
    {
        get
        {
            if (ClanManager.Instance.searchValue != string.Empty && ClanManager.Instance.searchResults.ContainsKey(ClanManager.Instance.searchValue))
            {
                return ClanManager.Instance.searchResults[ClanManager.Instance.searchValue];
            }
            return null;
        }
    }

    public static void Init()
    {
        Ajax.Request(WebUrls.CLAN_GET_URL, new AjaxRequest.AjaxHandler(ClanManager.Instance.OnInit));
        DurationManager.OnEnd += new DurationManager.DurationEventHandler(ClanManager.HandleOnEnd);
    }

    private static void HandleOnEnd(object sender)
    {
        if (sender.GetType() == typeof(ClanEvent))
        {
            UnityEngine.Debug.LogError(string.Format("OnEnd {0} {1}", sender, sender.GetType()));
            ClanEvent clanEvent = sender as ClanEvent;
            UnityEngine.Debug.LogError("_event.Type=" + clanEvent.Type);
            if (clanEvent.Type == ClanEventType.ChangeOwner)
            {
                ClanManager.Instance.OnChangeOwner(clanEvent.ClanId, Convert.ToInt32(clanEvent.Data["nuid"]));
            }
            if (clanEvent.Type == ClanEventType.Delete)
            {
                ClanManager.Instance.DeleteClan(clanEvent.ClanId);
            }
            if (clanEvent.Type == ClanEventType.LeaveMember || clanEvent.Type == ClanEventType.DeleteMember)
            {
                ClanManager.Instance.OnRemoveMember(clanEvent.ClanId, Convert.ToInt32(clanEvent.Data["uid"]));
            }
            ClanManager.Instance.OnDeleteEvent(clanEvent.ClanId, clanEvent.EventId);
        }
    }

    private void OnInit(object result, AjaxRequest request)
    {
        JSONNode jSONNode = JSON.Parse(Ajax.DecodeUtf(result.ToString()));
        JSONObject jSONObject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
        if (jSONNode["result"] != (object)null && jSONNode["result"].AsBool)
        {
            if (jSONNode["time"] != (object)null)
            {
                this.serverTime = jSONNode["time"].AsInt;
            }
            if (jSONNode["costs"] != (object)null)
            {
                if (jSONNode["costs"]["cc"] != (object)null)
                {
                    ClanManager.COST_CLAN_CREATE = jSONNode["costs"]["cc"].AsInt;
                }
                if (jSONNode["costs"]["cr"] != (object)null)
                {
                    ClanManager.COST_REQUESTS = jSONNode["costs"]["cr"].AsInt;
                }
                if (jSONNode["costs"]["ccn"] != (object)null)
                {
                    ClanManager.COST_CHANGE_NAME = jSONNode["costs"]["ccn"].AsInt;
                }
                if (jSONNode["costs"]["cct"] != (object)null)
                {
                    ClanManager.COST_CHANGE_TAG = jSONNode["costs"]["cct"].AsInt;
                }
                if (jSONNode["costs"]["cecm"] != (object)null)
                {
                    List<int> list = new List<int>();
                    foreach (JSONNode child in jSONNode["costs"]["cecm"].Childs)
                    {
                        list.Add(child.AsInt);
                    }
                    ClanManager.COST_EXPAN_CLAN_MEMBER = list.ToArray();
                }
            }
            if (jSONObject.GetField("ui") != null)
            {
                if (jSONObject.GetField("ui").GetField("i") != null && jSONObject.GetField("ui").GetField("i").type == JSONObject.Type.ARRAY)
                {
                    JSONObject field = jSONObject.GetField("ui").GetField("i");
                    this.localInvites.Clear();
                    for (int i = 0; i < field.Count; i++)
                    {
                        if (field[i] != null)
                        {
                            this.localInvites.Add(Convert.ToInt32(field[i].n));
                        }
                    }
                }
                if (jSONObject.GetField("ui").GetField("uc") != null)
                {
                    if (jSONObject.GetField("ui").GetField("uc").GetField("u") != null)
                    {
                        this.clan_used_request = Convert.ToInt16(jSONObject.GetField("ui").GetField("uc").GetField("u")
                            .n);
                        }
                        if (jSONObject.GetField("ui").GetField("uc").GetField("m") != null)
                        {
                            this.clan_max_request = Convert.ToInt16(jSONObject.GetField("ui").GetField("uc").GetField("m")
                                .n);
                            }
                            if (jSONObject.GetField("ui").GetField("uc").GetField("ek") != null)
                            {
                                int num = Convert.ToInt32(jSONObject.GetField("ui").GetField("uc").GetField("ek")
                                    .n);
                                    if (num == 100)
                                    {
                                        num = 75;
                                    }
                                    this.selectedIndexKoef = Array.IndexOf(this.availKoef, num.ToString());
                                }
                            }
                        }
                        if (jSONNode["d"] != (object)null)
                        {
                            int key = 1;
                            if (jSONNode["pg"] != (object)null)
                            {
                                key = jSONNode["pg"].AsInt;
                            }
                            if (jSONNode["dtot"] != (object)null)
                            {
                                this.maxPage = jSONNode["dtot"].AsInt;
                            }
                            foreach (JSONNode child2 in jSONNode["d"].Childs)
                            {
                                Clan clan = new Clan(child2);
                                if (!this.clanList.ContainsKey(clan.ClanID))
                                {
                                    this.clanList.Add(clan.ClanID, clan);
                                }
                                else
                                {
                                    this.clanList[clan.ClanID] = clan;
                                    if (ClanManager.SelectedClan != null && ClanManager.SelectedClan.ClanID == clan.ClanID)
                                    {
                                        this.selectedClan = clan;
                                    }
                                    if (LocalUser.Clan != null && LocalUser.Clan.ClanID == clan.ClanID)
                                    {
                                        LocalUser.Clan = clan;
                                    }
                                }
                                if (!this.clanByPage.ContainsKey(key))
                                {
                                    this.clanByPage.Add(key, new ClanList());
                                }
                                this.clanByPage[key].Add(clan);
                            }
                        }
                        if (jSONNode["id"] != (object)null)
                        {
                            int asInt = jSONNode["id"].AsInt;
                            if (asInt != 0)
                            {
                                if (this.clanList.ContainsKey(asInt))
                                {
                                    this.clanList[asInt].Update(jSONNode["cinfo"]);
                                }
                                else
                                {
                                    this.clanList.Add(asInt, new Clan(jSONNode["cinfo"]));
                                }
                            }
                        }
                        if (request.Tag != null && request.Tag.GetType() == typeof(int))
                        {
                            int key2 = (int)request.Tag;
                            this.selectedClan = this.clanList[key2];
                        }
                        else
                        {
                            if (LocalUser.Clan != null && this.selectedClan == null)
                            {
                                if (this.clanList.ContainsKey(LocalUser.Clan.ClanID))
                                {
                                    this.selectedClan = this.clanList[LocalUser.Clan.ClanID];
                                    UnityEngine.Debug.LogError("SELECT CLAN: " + this.selectedClan.ToString());
                                }
                                else
                                {
                                    AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.CLAN_GET_EXTRA_URL + "&cid=" + LocalUser.Clan.ClanID, false, LocalUser.Clan.ClanID, AjaxRequest.DataType.Text);
                                    ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(this.OnInit);
                                    Ajax.Request(ajaxRequest);
                                }
                            }
                            if (jSONObject.GetField("id") != null)
                            {
                                int num2 = Convert.ToInt32(jSONObject["id"].n);
                                LocalUser.Clan = this.clanList[num2];
                                ClanManager.View(num2);
                            }
                        }
                        Dictionary<int, Clan>.Enumerator enumerator3 = this.clanList.GetEnumerator();
                        try
                        {
                            while (enumerator3.MoveNext())
                            {
                                KeyValuePair<int, Clan> current3 = enumerator3.Current;
                                if (this.localInvites.Contains(current3.Key))
                                {
                                    current3.Value.SendInvite = true;
                                }
                            }
                        }
                        finally
                        {
                            ((IDisposable)enumerator3).Dispose();
                        }
                    }
                }

                public static void OnServerRequest(Hashtable input)
                {
                    UnityEngine.Debug.LogError("[ClanManager] OnServerRequest");
                    ClanEventCode clanEventCode = (ClanEventCode)(int)input[0];
                    int clan_id = (int)input[1];
                    Hashtable hashtable = (Hashtable)input[2];
                    switch (clanEventCode)
                    {
                        case ClanEventCode.ChangeUrl:
                        case ClanEventCode.Create:
                        case (ClanEventCode)11:
                        case ClanEventCode.AddMember:
                        case (ClanEventCode)13:
                        case (ClanEventCode)19:
                            break;
                        case ClanEventCode.ChangeArm:
                            ClanManager.Instance.OnChangeArm(clan_id, (int)hashtable[0], (int)hashtable[1]);
                            break;
                        case ClanEventCode.ChangeTag:
                            ClanManager.Instance.OnChangeTag(clan_id, hashtable[0].ToString());
                            break;
                        case ClanEventCode.ChangeName:
                            ClanManager.Instance.OnChangeName(clan_id, hashtable[0].ToString());
                            break;
                        case ClanEventCode.ChangeDesc:
                            ClanManager.Instance.OnChangeDesc(clan_id, hashtable[0].ToString());
                            break;
                        case ClanEventCode.AddInvite:
                            ClanManager.Instance.OnJoin((int)hashtable[0], clan_id);
                            break;
                        case ClanEventCode.RejectInvite:
                            ClanManager.Instance.OnReject((int)hashtable[0], clan_id);
                            break;
                        case ClanEventCode.AcceptInvite:
                            ClanManager.Instance.OnAccept((int)hashtable[0], clan_id, new ClanMember((Hashtable)hashtable[1]));
                            break;
                        case ClanEventCode.ChangeKoef:
                            ClanManager.Instance.OnChangeKoef(clan_id, (int)hashtable[0], (int)hashtable[1]);
                            break;
                        case ClanEventCode.ChangeMaxMember:
                            ClanManager.Instance.OnExpand(clan_id, (int)hashtable[0]);
                            break;
                        case ClanEventCode.ChangeOwner:
                            ClanManager.Instance.OnChangeOwner(clan_id, (int)hashtable[0]);
                            break;
                        case ClanEventCode.AddEvent:
                            ClanManager.RefreshEvent(clan_id);
                            break;
                        case ClanEventCode.RemoveEvent:
                            ClanManager.Instance.OnDeleteEvent(clan_id, (int)hashtable[0]);
                            break;
                        case ClanEventCode.AddMoney:
                            ClanManager.Instance.OnAddMoney((int)hashtable[3], clan_id, (int)hashtable[0], hashtable[2].ToString(), (int)hashtable[1], ClanTreasuryEventType.Add, DateTime.Now.ToString("d.MM.yyyy H:mm"));
                            break;
                        case ClanEventCode.BuyEnhancer:
                        {
                            object[] array = new object[1] {
                                (int)hashtable[0]
                            };
                            ClanManager.OnChangeEnhancer(clan_id, (int)hashtable[0], (int)hashtable[1]);
                            break;
                        }
                    }
                }

                public static void View(int clan_id)
                {
                    if (MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Clan)
                    {
                        MenuSelecter.MainMenuSelect = MenuSelecter.MainMenuEnum.Clan;
                    }
                    if (MenuSelecter.ClanMenuSelect != MenuSelecter.ClanMenuEnum.Hall)
                    {
                        MenuSelecter.ClanMenuSelect = MenuSelecter.ClanMenuEnum.Hall;
                    }
                    if (MenuSelecter.ClanHallMenuSelect != MenuSelecter.ClanHallMenuEnum.Main)
                    {
                        MenuSelecter.ClanHallMenuSelect = MenuSelecter.ClanHallMenuEnum.Main;
                    }
                    if (ClanManager.Instance.clanList.ContainsKey(clan_id) && ClanManager.Instance.clanList[clan_id].FullLoad)
                    {
                        ClanManager.Instance.selectedClan = ClanManager.Instance.clanList[clan_id];
                    }
                    else
                    {
                        AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.CLAN_GET_EXTRA_URL + "&cid=" + clan_id, false, clan_id, AjaxRequest.DataType.Text);
                        ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ClanManager.Instance.OnInit);
                        Ajax.Request(ajaxRequest);
                    }
                }

                public static void Create(string clanName, string clanTag, ClanArm arm)
                {
                    ClanManager.Instance.currentLastError = ErrorInfo.CODE.NONE;
                    if (clanName.Trim() == string.Empty)
                    {
                        ClanManager.Instance.currentLastError = ErrorInfo.CODE.CLAN_NAME;
                    }
                    else if (clanTag.Trim() == string.Empty)
                    {
                        ClanManager.Instance.currentLastError = ErrorInfo.CODE.CLAN_TAG;
                    }
                    else if (ClanManager.COST_CLAN_CREATE > LocalUser.Money)
                    {
                        ErrorInfo.CODE code = ErrorInfo.CODE.MISSING_MONEY;
                        code.AddNotification(ErrorInfo.TYPE.BUY_OTHER);
                    }
                    else
                    {
                        object[] tag = new object[2] {
                            clanName,
                            clanTag
                        };
                        WWWForm wWWForm = new WWWForm();
                        wWWForm.AddField("data[name]", clanName);
                        wWWForm.AddField("data[tag]", clanTag);
                        wWWForm.AddField("data[arm_id]", arm.ArmID.ToString());
                        AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.CLAN_CREATE_URL, tag);
                        ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ClanManager.Instance.OnCreate);
                        ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ClanManager.Instance.OnInit);
                        ajaxRequest.AddForm(wWWForm);
                        Ajax.Request(ajaxRequest);
                    }
                }

                private void OnCreate(object result, AjaxRequest request)
                {
                    object[] array = (object[])request.Tag;
                    string text = (string)array[0];
                    string text2 = (string)array[1];
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        LocalUser.Money -= ClanManager.COST_CLAN_CREATE;
                    }
                    if (jSONObject.GetField("code") != null)
                    {
                        if (jSONObject.GetField("code").type == JSONObject.Type.STRING)
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").str);
                        }
                        else
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").n);
                        }
                    }
                }

                public static void DeleteClanLazy(int clan_id)
                {
                    Ajax.Request(string.Format("{0}&cid={1}", WebUrls.CLAN_DELETE_URL, clan_id), new AjaxRequest.AjaxHandler(ClanManager.Instance.OnDeleteClanLazy));
                }

                private void OnDeleteClanLazy(object result, AjaxRequest request)
                {
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = LocalUser.UserID;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.AddEvent, LocalUser.Clan.ClanID, hashtable);
                        }
                        else
                        {
                            ClanManager.RefreshEvent(LocalUser.Clan.ClanID);
                        }
                    }
                }

                private void DeleteClan(int clan_id)
                {
                    if (this.selectedClan != null && this.selectedClan.ClanID == clan_id)
                    {
                        this.selectedClan = null;
                    }
                    if (LocalUser.Clan != null && LocalUser.Clan.ClanID == clan_id)
                    {
                        LocalUser.Clan = null;
                    }
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList.Remove(clan_id);
                    }
                    if (MenuSelecter.ClanMenuSelect == MenuSelecter.ClanMenuEnum.Hall)
                    {
                        MenuSelecter.ClanMenuSelect = MenuSelecter.ClanMenuEnum.List;
                    }
                }

                public static void Join(int clan_id)
                {
                    if (LocalUser.Level < 15)
                    {
                        ErrorInfo.CODE code = ErrorInfo.CODE.CLAN_USER_LVL_LESS;
                        code.AddNotification(ErrorInfo.TYPE.CLAN_JOIN);
                    }
                    else if (!ClanManager.joinInProgress)
                    {
                        ClanManager.joinInProgress = true;
                        Ajax.Request(string.Format("{0}&cid={1}", WebUrls.CLAN_JOIN_URL, clan_id), new AjaxRequest.AjaxHandler(ClanManager.Instance.OnJoin));
                    }
                }

                private void OnJoin(object result, AjaxRequest request)
                {
                    ClanManager.joinInProgress = false;
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        this.clan_used_request++;
                        if (jSONObject.GetField("id") != null)
                        {
                            int num = Convert.ToInt32(jSONObject.GetField("id").n);
                            this.localInvites.Add(num);
                            if (this.clanList.ContainsKey(num))
                            {
                                this.clanList[num].SendInvite = true;
                            }
                            if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                            {
                                Hashtable hashtable = new Hashtable();
                                hashtable[0] = LocalUser.UserID;
                                MasterServerNetworkController.SendClanEvent(ClanEventCode.AddInvite, num, hashtable);
                            }
                        }
                    }
                    if (jSONObject.GetField("code") != null)
                    {
                        if (jSONObject.GetField("code").type == JSONObject.Type.STRING)
                        {
                            ErrorInfo.CODE code = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").str);
                            code.AddNotification(ErrorInfo.TYPE.CLAN_JOIN);
                        }
                        else
                        {
                            ErrorInfo.CODE code2 = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").n);
                            code2.AddNotification(ErrorInfo.TYPE.CLAN_JOIN);
                        }
                    }
                }

                private void OnJoin(int user_id, int clan_id)
                {
                    if (ClanManager.ClanList.ContainsKey(clan_id))
                    {
                        Clan clan = ClanManager.ClanList[clan_id];
                        if (clan.FounderID == LocalUser.UserID)
                        {
                            clan.RefreshInvites();
                        }
                    }
                    if (LocalUser.Clan != null && LocalUser.Clan.ClanID == clan_id && LocalUser.Clan.FounderID == LocalUser.UserID)
                    {
                        ClanManager.NewRequests = true;
                    }
                }

                public static void BuyRequests()
                {
                    if (LocalUser.Money < ClanManager.COST_REQUESTS)
                    {
                        ErrorInfo.CODE code = ErrorInfo.CODE.MISSING_MONEY;
                        code.AddNotification(ErrorInfo.TYPE.BUY_OTHER);
                    }
                    else
                    {
                        ClanManager.Instance.buyRequestInProgress = true;
                        Ajax.Request(WebUrls.CLAN_BUY_REQUEST_URL, new AjaxRequest.AjaxHandler(ClanManager.Instance.OnBuyRequest));
                    }
                }

                private void OnBuyRequest(object result, AjaxRequest request)
                {
                    ClanManager.Instance.buyRequestInProgress = false;
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        this.clan_max_request += 5;
                        LocalUser.Money -= ClanManager.COST_REQUESTS;
                    }
                }

                public static void Accept(int user_id, int clan_id)
                {
                    Ajax.Request(string.Format("{0}&cid={1}&uid={2}", WebUrls.CLAN_ACCEPT_URL, clan_id, user_id), new AjaxRequest.AjaxHandler(ClanManager.Instance.OnAccept));
                }

                private void OnAccept(object result, AjaxRequest request)
                {
                    JSONNode jSONNode = JSONNode.Parse(Ajax.DecodeUtf(result.ToString()));
                    if (jSONNode["result"] != (object)null && jSONNode["id"] != (object)null && jSONNode["i"] != (object)null)
                    {
                        int clanID = LocalUser.Clan.ClanID;
                        int asInt = jSONNode["id"].AsInt;
                        this.clanList[clanID].RemoveInvite(asInt);
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = asInt;
                            hashtable[1] = new ClanMember(jSONNode["i"]).ToHashtable();
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.AcceptInvite, clanID, hashtable);
                        }
                        else
                        {
                            this.clanList[clanID].AddMember(new ClanMember(jSONNode["i"]));
                        }
                    }
                }

                private void OnAccept(int user_id, int clan_id, ClanMember user)
                {
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].AddMember(user);
                        if (user_id == LocalUser.UserID)
                        {
                            LocalUser.Clan = this.clanList[clan_id];
                            LocalUser.Clan.SendInvite = false;
                            this.selectedClan = LocalUser.Clan;
                            GameLogicServerNetworkController.SendChange(8);
                        }
                    }
                }

                public static void Reject(int user_id, int clan_id)
                {
                    Ajax.Request(string.Format("{0}&cid={1}&uid={2}", WebUrls.CLAN_REJECT_URL, clan_id, user_id), new AjaxRequest.AjaxHandler(ClanManager.Instance.OnReject));
                }

                private void OnReject(object result, AjaxRequest request)
                {
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        int clanID = LocalUser.Clan.ClanID;
                        int num = Convert.ToInt32(jSONObject.GetField("id").n);
                        this.clanList[clanID].RemoveInvite(num);
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = num;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.RejectInvite, clanID, hashtable);
                        }
                    }
                }

                private void OnReject(int user_id, int clan_id)
                {
                    if (user_id == LocalUser.UserID && this.localInvites.Contains(clan_id))
                    {
                        this.localInvites.Remove(clan_id);
                    }
                }

                public static void RemoveMemberLazy(int user_id, int clan_id)
                {
                    Ajax.Request(string.Format("{0}&cid={1}&uid={2}", WebUrls.CLAN_REMOVE_MEMBER_URL, clan_id, user_id), new AjaxRequest.AjaxHandler(ClanManager.Instance.OnRemoveMemberLazy));
                }

                private void OnRemoveMemberLazy(object result, AjaxRequest request)
                {
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = LocalUser.UserID;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.AddEvent, LocalUser.Clan.ClanID, hashtable);
                        }
                        else
                        {
                            ClanManager.RefreshEvent(LocalUser.Clan.ClanID);
                        }
                    }
                }

                private void OnRemoveMember(int clan_id, int user_id)
                {
                    UnityEngine.Debug.LogError(string.Format("[ClanManager] OnRemoveMember({0}, {1})", clan_id, user_id));
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].RemoveMember(user_id);
                    }
                    if (user_id == LocalUser.UserID)
                    {
                        GameLogicServerNetworkController.SendChange(8);
                        LocalUser.Clan = null;
                        MenuSelecter.ClanHallMenuSelect = MenuSelecter.ClanHallMenuEnum.Main;
                        MenuSelecter.ClanMenuSelect = MenuSelecter.ClanMenuEnum.List;
                    }
                }

                public static void LeaveLazy(int clan_id)
                {
                    Ajax.Request(string.Format("{0}&cid={1}", WebUrls.CLAN_LEAVE_URL, clan_id), new AjaxRequest.AjaxHandler(ClanManager.Instance.OnLeaveLazy));
                }

                private void OnLeaveLazy(object result, AjaxRequest request)
                {
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = LocalUser.UserID;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.AddEvent, LocalUser.Clan.ClanID, hashtable);
                        }
                        else
                        {
                            ClanManager.RefreshEvent(LocalUser.Clan.ClanID);
                        }
                    }
                }

                public static void BuyMaxMember(int clan_id)
                {
                    if (ClanManager.COST_EXPAND_CLAN_MEMBER_CURRENT > ClanManager.SelectedClan.Money)
                    {
                        ErrorInfo.CODE code = ErrorInfo.CODE.MISSING_MONEY_TREASURY;
                        code.AddNotification(ErrorInfo.TYPE.BUY_OTHER);
                    }
                    else
                    {
                        Ajax.Request(string.Format("{0}&cid={1}", WebUrls.CLAN_EXPAND_MEMBER_URL, clan_id), new AjaxRequest.AjaxHandler(ClanManager.Instance.OnExpand));
                    }
                }

                private void OnExpand(object result, AjaxRequest request)
                {
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        int clan_id = Convert.ToInt32(jSONObject.GetField("cid").n);
                        int num = Convert.ToInt32(jSONObject.GetField("macnt").n);
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = num;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.ChangeMaxMember, clan_id, hashtable);
                        }
                        else
                        {
                            this.OnExpand(clan_id, num);
                        }
                    }
                }

                private void OnExpand(int clan_id, int max_member)
                {
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].MaxMemberCount = Convert.ToInt16(max_member);
                        this.clanList[clan_id].Money -= ClanManager.COST_EXPAND_CLAN_MEMBER_CURRENT;
                    }
                }

                public static void AddMoney(int clan_id, int money)
                {
                    if (money > 0)
                    {
                        object[] tag = new object[1] {
                            money
                        };
                        AjaxRequest ajaxRequest = new AjaxRequest(string.Format("{0}&cid={1}&money={2}", WebUrls.CLAN_ADD_MONEY_URL, clan_id, money), tag);
                        ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ClanManager.Instance.OnAddMoney);
                        Ajax.Request(ajaxRequest);
                    }
                }

                private void OnAddMoney(object result, AjaxRequest request)
                {
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        object[] array = (object[])request.Tag;
                        int num = Convert.ToInt32(jSONObject.GetField("id").n);
                        int clan_id = Convert.ToInt32(jSONObject.GetField("cid").n);
                        int num2 = Convert.ToInt32(array[0]);
                        LocalUser.Money -= num2;
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = LocalUser.UserID;
                            hashtable[1] = num2;
                            hashtable[2] = LocalUser.Name;
                            hashtable[3] = num;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.AddMoney, clan_id, hashtable);
                        }
                        else
                        {
                            this.OnAddMoney(num, clan_id, LocalUser.UserID, LocalUser.Name, num2, ClanTreasuryEventType.Add, DateTime.Now.ToString("d.MM.yyyy H:mm"));
                        }
                    }
                }

                private void OnAddMoney(int eventId, int clan_id, int user_id, string user_name, int money, ClanTreasuryEventType type, string date)
                {
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].Money += money;
                        this.clanList[clan_id].AddTreasuryEvent(eventId, clan_id, user_id, user_name, money, type, date);
                    }
                }

                public static void ChangeName(int clan_id, string name)
                {
                    ClanManager.Instance.currentLastError = ErrorInfo.CODE.NONE;
                    if (name.Trim() == string.Empty)
                    {
                        ClanManager.Instance.currentLastError = ErrorInfo.CODE.CLAN_NAME;
                    }
                    else if (ClanManager.COST_CHANGE_NAME > ClanManager.SelectedClan.Money)
                    {
                        ErrorInfo.CODE code = ErrorInfo.CODE.MISSING_MONEY_TREASURY;
                        code.AddNotification(ErrorInfo.TYPE.BUY_OTHER);
                    }
                    else
                    {
                        object[] tag = new object[1] {
                            name
                        };
                        WWWForm wWWForm = new WWWForm();
                        wWWForm.AddField("data[name]", name);
                        AjaxRequest ajaxRequest = new AjaxRequest(string.Format("{0}&cid={1}", WebUrls.CLAN_CHANGE_NAME_URL, clan_id), tag);
                        ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ClanManager.Instance.OnChangeName);
                        ajaxRequest.AddForm(wWWForm);
                        Ajax.Request(ajaxRequest);
                    }
                }

                private void OnChangeName(object result, AjaxRequest request)
                {
                    object[] array = (object[])request.Tag;
                    string text = (string)array[0];
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        int clan_id = Convert.ToInt32(jSONObject.GetField("id").n);
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = text;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.ChangeName, LocalUser.Clan.ClanID, hashtable);
                        }
                        else
                        {
                            this.OnChangeName(clan_id, text);
                        }
                    }
                    if (jSONObject.GetField("code") != null)
                    {
                        if (jSONObject.GetField("code").type == JSONObject.Type.STRING)
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").str);
                        }
                        else
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").n);
                        }
                    }
                }

                private void OnChangeName(int clan_id, string name)
                {
                    object[] data = new object[3] {
                        2,
                        clan_id,
                        name
                    };
                    GameLogicServerNetworkController.SendChange(6, data);
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].Name = name;
                        this.clanList[clan_id].Money -= ClanManager.COST_CHANGE_NAME;
                    }
                }

                public static void ChangeTag(int clan_id, string clan_tag)
                {
                    ClanManager.Instance.currentLastError = ErrorInfo.CODE.NONE;
                    if (clan_tag.Trim() == string.Empty)
                    {
                        ClanManager.Instance.currentLastError = ErrorInfo.CODE.CLAN_TAG;
                    }
                    else if (ClanManager.COST_CHANGE_TAG > ClanManager.SelectedClan.Money)
                    {
                        ErrorInfo.CODE code = ErrorInfo.CODE.MISSING_MONEY_TREASURY;
                        code.AddNotification(ErrorInfo.TYPE.BUY_OTHER);
                    }
                    else
                    {
                        object[] tag = new object[1] {
                            clan_tag
                        };
                        WWWForm wWWForm = new WWWForm();
                        wWWForm.AddField("data[tag]", clan_tag);
                        AjaxRequest ajaxRequest = new AjaxRequest(string.Format("{0}&cid={1}", WebUrls.CLAN_CHANGE_TAG_URL, clan_id), tag);
                        ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ClanManager.Instance.OnChangeTag);
                        ajaxRequest.AddForm(wWWForm);
                        Ajax.Request(ajaxRequest);
                    }
                }

                private void OnChangeTag(object result, AjaxRequest request)
                {
                    object[] array = (object[])request.Tag;
                    string text = (string)array[0];
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        int clan_id = Convert.ToInt32(jSONObject.GetField("id").n);
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = text;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.ChangeTag, LocalUser.Clan.ClanID, hashtable);
                        }
                        else
                        {
                            this.OnChangeTag(clan_id, text);
                        }
                    }
                    if (jSONObject.GetField("code") != null)
                    {
                        if (jSONObject.GetField("code").type == JSONObject.Type.STRING)
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").str);
                        }
                        else
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").n);
                        }
                    }
                }

                private void OnChangeTag(int clan_id, string clanTag)
                {
                    object[] data = new object[3] {
                        1,
                        clan_id,
                        clanTag
                    };
                    GameLogicServerNetworkController.SendChange(6, data);
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].Tag = clanTag;
                        this.clanList[clan_id].Money -= ClanManager.COST_CHANGE_TAG;
                    }
                }

                public static void ChangeAccessLvl(int clan_id, int access_lvl)
                {
                    ClanManager.Instance.currentLastError = ErrorInfo.CODE.NONE;
                    object[] tag = new object[1] {
                        access_lvl
                    };
                    AjaxRequest ajaxRequest = new AjaxRequest(string.Format("{0}&cid={1}&accesslvl={2}", WebUrls.CLAN_CHANGE_ACCESS_LVL_URL, clan_id, access_lvl), tag);
                    ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ClanManager.Instance.OnChangeAccessLvl);
                    Ajax.Request(ajaxRequest);
                }

                private void OnChangeAccessLvl(object result, AjaxRequest request)
                {
                    object[] array = (object[])request.Tag;
                    int access_lvl = (int)array[0];
                    JSONNode jSONNode = JSON.Parse(result.ToString());
                    if (jSONNode["result"] != (object)null && jSONNode["result"].AsBool)
                    {
                        int asInt = jSONNode["id"].AsInt;
                        this.OnChangeAccessLvl(asInt, access_lvl);
                    }
                    if (jSONNode["code"] != (object)null)
                    {
                        this.currentLastError = (ErrorInfo.CODE)jSONNode["code"].AsInt;
                    }
                }

                private void OnChangeAccessLvl(int clan_id, int access_lvl)
                {
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].AccessLvl = access_lvl;
                    }
                }

                public static void ChangeAccess(int clan_id, int access)
                {
                    ClanManager.Instance.currentLastError = ErrorInfo.CODE.NONE;
                    object[] tag = new object[1] {
                        access
                    };
                    AjaxRequest ajaxRequest = new AjaxRequest(string.Format("{0}&cid={1}&access={2}", WebUrls.CLAN_CHANGE_ACCESS_URL, clan_id, access), tag);
                    ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ClanManager.Instance.OnChangeAccess);
                    Ajax.Request(ajaxRequest);
                }

                private void OnChangeAccess(object result, AjaxRequest request)
                {
                    object[] array = (object[])request.Tag;
                    int access = (int)array[0];
                    JSONNode jSONNode = JSON.Parse(result.ToString());
                    if (jSONNode["result"] != (object)null && jSONNode["result"].AsBool)
                    {
                        int asInt = jSONNode["id"].AsInt;
                        this.OnChangeAccess(asInt, access);
                    }
                    if (jSONNode["code"] != (object)null)
                    {
                        this.currentLastError = (ErrorInfo.CODE)jSONNode["code"].AsInt;
                    }
                }

                private void OnChangeAccess(int clan_id, int access)
                {
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].Access = access;
                    }
                }

                public static void ChangeUrl(int clan_id, string clan_url)
                {
                    ClanManager.Instance.currentLastError = ErrorInfo.CODE.NONE;
                    object[] tag = new object[1] {
                        clan_url
                    };
                    WWWForm wWWForm = new WWWForm();
                    wWWForm.AddField("data[url]", clan_url);
                    AjaxRequest ajaxRequest = new AjaxRequest(string.Format("{0}&cid={1}", WebUrls.CLAN_CHANGE_PAGE_URL, clan_id), tag);
                    ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ClanManager.Instance.OnChangeUrl);
                    ajaxRequest.AddForm(wWWForm);
                    Ajax.Request(ajaxRequest);
                }

                private void OnChangeUrl(object result, AjaxRequest request)
                {
                    object[] array = (object[])request.Tag;
                    string text = (string)array[0];
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        int clan_id = Convert.ToInt32(jSONObject.GetField("id").n);
                        this.OnChangeUrl(clan_id, text);
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = text;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.ChangeUrl, LocalUser.Clan.ClanID, hashtable);
                        }
                    }
                    if (jSONObject.GetField("code") != null)
                    {
                        if (jSONObject.GetField("code").type == JSONObject.Type.STRING)
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").str);
                        }
                        else
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").n);
                        }
                    }
                }

                private void OnChangeUrl(int clan_id, string clanUrl)
                {
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].Homepage = clanUrl;
                    }
                }

                public static void ChangeDesc(int clan_id, string clan_desc)
                {
                    ClanManager.Instance.currentLastError = ErrorInfo.CODE.NONE;
                    object[] tag = new object[1] {
                        clan_desc
                    };
                    WWWForm wWWForm = new WWWForm();
                    wWWForm.AddField("data[desc]", clan_desc);
                    AjaxRequest ajaxRequest = new AjaxRequest(string.Format("{0}&cid={1}", WebUrls.CLAN_CHANGE_DESC_URL, clan_id), tag);
                    ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ClanManager.Instance.OnChangeDesc);
                    ajaxRequest.AddForm(wWWForm);
                    Ajax.Request(ajaxRequest);
                }

                private void OnChangeDesc(object result, AjaxRequest request)
                {
                    object[] array = (object[])request.Tag;
                    string text = ((string)array[0]).Substring(0, (((string)array[0]).Length >= 1024) ? 1024 : ((string)array[0]).Length);
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        int clan_id = Convert.ToInt32(jSONObject.GetField("id").n);
                        this.OnChangeDesc(clan_id, text);
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = text;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.ChangeDesc, LocalUser.Clan.ClanID, hashtable);
                        }
                    }
                    if (jSONObject.GetField("code") != null)
                    {
                        if (jSONObject.GetField("code").type == JSONObject.Type.STRING)
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").str);
                        }
                        else
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").n);
                        }
                    }
                }

                private void OnChangeDesc(int clan_id, string clanDesc)
                {
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].Desc = clanDesc;
                    }
                }

                public static void ChangeArm(int clan_id, int armId)
                {
                    ClanManager.Instance.currentLastError = ErrorInfo.CODE.NONE;
                    ClanArm arm = ClanArmManager.GetArm(armId);
                    if (arm != null && arm.Cost != null)
                    {
                        if (arm.Cost.TimePVCost > ClanManager.SelectedClan.Money)
                        {
                            ErrorInfo.CODE code = ErrorInfo.CODE.MISSING_MONEY_TREASURY;
                            code.AddNotification(ErrorInfo.TYPE.BUY_OTHER);
                        }
                        else
                        {
                            object[] tag = new object[1] {
                                armId
                            };
                            WWWForm wWWForm = new WWWForm();
                            wWWForm.AddField("data[arm_id]", armId.ToString());
                            AjaxRequest ajaxRequest = new AjaxRequest(string.Format("{0}&cid={1}", WebUrls.CLAN_CHANGE_ARM_URL, clan_id), tag);
                            ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ClanManager.Instance.OnChangeArm);
                            ajaxRequest.AddForm(wWWForm);
                            Ajax.Request(ajaxRequest);
                        }
                    }
                }

                private void OnChangeArm(object result, AjaxRequest request)
                {
                    object[] array = (object[])request.Tag;
                    int num = (int)array[0];
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        int clan_id = Convert.ToInt32(jSONObject.GetField("id").n);
                        ClanArm arm = ClanArmManager.GetArm(num);
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = num;
                            hashtable[1] = (int)arm.Cost.TimePVCost;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.ChangeArm, LocalUser.Clan.ClanID, hashtable);
                        }
                        else
                        {
                            this.OnChangeArm(clan_id, num, (int)arm.Cost.TimePVCost);
                        }
                    }
                    if (jSONObject.GetField("code") != null)
                    {
                        if (jSONObject.GetField("code").type == JSONObject.Type.STRING)
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").str);
                        }
                        else
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").n);
                        }
                    }
                }

                private void OnChangeArm(int clan_id, int armId, int cost)
                {
                    object[] data = new object[3] {
                        3,
                        clan_id,
                        armId
                    };
                    GameLogicServerNetworkController.SendChange(6, data);
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].ArmsID = (short)armId;
                        this.clanList[clan_id].Money -= cost;
                    }
                }

                public static void OnChangeEnhancer(int clan_id, int enhancer_id, int duration)
                {
                    GameLogicServerNetworkController.SendChange(9, enhancer_id);
                    Enhancer enhancer = ClanShopManager.Enhancers.Find((Enhancer x) => x.EnhancerID == enhancer_id);
                    if (enhancer != null)
                    {
                        enhancer.Shop_Cost.SelectedDuration = (DurationType)duration;
                        if (ClanManager.Instance.clanList.ContainsKey(clan_id))
                        {
                            ClanManager.Instance.clanList[clan_id].Money -= (int)enhancer.Shop_Cost.SelectedVCost;
                        }
                        enhancer.IsBuyed = true;
                        int num = 86460;
                        if (enhancer.Shop_Cost.SelectedDuration == DurationType.WEEK)
                        {
                            num *= 7;
                        }
                        else if (enhancer.Shop_Cost.SelectedDuration == DurationType.MONTH)
                        {
                            num *= 30;
                        }
                        else if (enhancer.Shop_Cost.SelectedDuration == DurationType.PERMANENT)
                        {
                            num = 0;
                        }
                        if (num != 0)
                        {
                            if (enhancer.Duration == null)
                            {
                                enhancer.Duration = new Duration(num);
                            }
                            else
                            {
                                enhancer.Duration = new Duration(enhancer.Duration.TotalSec + num);
                            }
                        }
                        UnityEngine.Debug.LogError("OnChangeEnhancer " + Time.time);
                    }
                }

                public static void SetIndexKoefInPercent(int percent, bool sendRequest)
                {
                    ClanManager.Instance.selectedIndexKoef = Array.IndexOf(ClanManager.Instance.availKoef, percent.ToString());
                    if (sendRequest)
                    {
                        ClanManager.ChangeKoef(LocalUser.Clan.ClanID, Convert.ToInt32(ClanManager.AvailableKoef[ClanManager.Instance.selectedIndexKoef]));
                    }
                }

                public static void ChangeOwnerLazy(int clan_id, int new_owner_id)
                {
                    Ajax.Request(string.Format("{0}&cid={1}&nid={2}", WebUrls.CLAN_CHANGE_OWNER_URL, clan_id, new_owner_id), new AjaxRequest.AjaxHandler(ClanManager.Instance.OnChangeOwner));
                }

                private void OnChangeOwner(object result, AjaxRequest request)
                {
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        int num = Convert.ToInt32(jSONObject.GetField("cid").n);
                        int num2 = Convert.ToInt32(jSONObject.GetField("nid").n);
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = LocalUser.UserID;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.AddEvent, LocalUser.Clan.ClanID, hashtable);
                        }
                        else
                        {
                            ClanManager.RefreshEvent(LocalUser.Clan.ClanID);
                        }
                    }
                    if (jSONObject.GetField("code") != null)
                    {
                        if (jSONObject.GetField("code").type == JSONObject.Type.STRING)
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").str);
                        }
                        else
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").n);
                        }
                    }
                }

                private void OnChangeOwner(int clan_id, int owner_id)
                {
                    UnityEngine.Debug.LogError(string.Format("OnChangeOwner(int clan_id={0}, int owner_id={1})", clan_id, owner_id));
                    if (LocalUser.UserID != owner_id && (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Edit || MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Invites))
                    {
                        MenuSelecter.ClanHallMenuSelect = MenuSelecter.ClanHallMenuEnum.Main;
                    }
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].SetOwner(owner_id);
                    }
                }

                public static void ChangeKoef(int clan_id, int koef)
                {
                    Ajax.Request(string.Format("{0}&cid={1}&val={2}", WebUrls.CLAN_CHANGE_KOEF_URL, clan_id, koef), new AjaxRequest.AjaxHandler(ClanManager.Instance.OnChangeKoef));
                }

                private void OnChangeKoef(object result, AjaxRequest request)
                {
                    object[] array = (object[])request.Tag;
                    JSONObject jSONObject = new JSONObject(result.ToString());
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        int num = Convert.ToInt32(jSONObject.GetField("cid").n);
                        int num2 = Convert.ToInt32(jSONObject.GetField("id").n);
                        int num3 = Convert.ToInt32(jSONObject.GetField("val").n);
                        object[] data = new object[3] {
                            num,
                            num2,
                            num3
                        };
                        GameLogicServerNetworkController.SendChange(7, data);
                        this.OnChangeKoef(num, num2, num3);
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = num2;
                            hashtable[1] = num3;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.ChangeKoef, LocalUser.Clan.ClanID, hashtable);
                        }
                    }
                    if (jSONObject.GetField("code") != null)
                    {
                        if (jSONObject.GetField("code").type == JSONObject.Type.STRING)
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").str);
                        }
                        else
                        {
                            this.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("code").n);
                        }
                    }
                }

                private void OnChangeKoef(int clan_id, int user_id, int koef)
                {
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].SetKoef(user_id, koef);
                    }
                }

                public static void RefreshEvent(int clan_id)
                {
                    Ajax.Request(string.Format("{0}&cid={1}", WebUrls.CLAN_GET_EVENT_URL, clan_id), new AjaxRequest.AjaxHandler(ClanManager.Instance.OnRefreshEvent));
                }

                private void OnRefreshEvent(object result, AjaxRequest request)
                {
                    object[] array = (object[])request.Tag;
                    JSONNode jSONNode = JSONNode.Parse(Ajax.DecodeUtf(result.ToString()));
                    if (jSONNode["result"] != (object)null && jSONNode["cid"] != (object)null && jSONNode["ev"] != (object)null)
                    {
                        int asInt = jSONNode["cid"].AsInt;
                        if (this.clanList.ContainsKey(asInt))
                        {
                            this.clanList[asInt].InitEvents(jSONNode["ev"]);
                        }
                    }
                }

                public static void DeleteEvent(int clan_id, int event_id)
                {
                    Ajax.Request(string.Format("{0}&cid={1}&eid={2}", WebUrls.CLAN_DEL_EVENT_URL, clan_id, event_id), new AjaxRequest.AjaxHandler(ClanManager.Instance.OnDeleteEvent));
                }

                private void OnDeleteEvent(object result, AjaxRequest request)
                {
                    JSONObject jSONObject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
                    if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
                    {
                        int clan_id = Convert.ToInt32(jSONObject.GetField("cid").n);
                        int num = Convert.ToInt32(jSONObject.GetField("eid").n);
                        this.OnDeleteEvent(clan_id, num);
                        if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable[0] = num;
                            MasterServerNetworkController.SendClanEvent(ClanEventCode.RemoveEvent, LocalUser.Clan.ClanID, hashtable);
                        }
                    }
                }

                private void OnDeleteEvent(int clan_id, int event_id)
                {
                    if (this.clanList.ContainsKey(clan_id))
                    {
                        this.clanList[clan_id].RemoveEvent(event_id);
                        if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Events && this.clanList[clan_id].Events.Count == 0)
                        {
                            MenuSelecter.ClanHallMenuSelect = MenuSelecter.ClanHallMenuEnum.Main;
                        }
                    }
                }

                public static void Search(string value)
                {
                    value = value.Trim();
                    if (!(value == string.Empty))
                    {
                        ClanManager.Instance.searchValue = value;
                        if (!ClanManager.Instance.searchResults.ContainsKey(ClanManager.Instance.searchValue))
                        {
                            ClanManager.Instance.isSearchInProgress = true;
                            object[] tag = new object[1] {
                                value
                            };
                            WWWForm wWWForm = new WWWForm();
                            wWWForm.AddField("v", value);
                            AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.CLAN_SEARCH_URL, tag);
                            ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ClanManager.Instance.OnSearch);
                            ajaxRequest.AddForm(wWWForm);
                            Ajax.Request(ajaxRequest);
                        }
                    }
                }

                private void OnSearch(object result, AjaxRequest request)
                {
                    ClanManager.Instance.isSearchInProgress = false;
                    JSONNode jSONNode = JSONNode.Parse(Ajax.DecodeUtf(result.ToString()));
                    if (jSONNode["result"] != (object)null && jSONNode["d"] != (object)null)
                    {
                        if (!this.searchResults.ContainsKey(this.searchValue))
                        {
                            this.searchResults.Add(this.searchValue, new ClanList());
                        }
                        ClanList clanList = this.searchResults[this.searchValue];
                        foreach (JSONNode child in jSONNode["d"].Childs)
                        {
                            Clan clan = new Clan(child);
                            clanList.Add(clan);
                        }
                    }
                }
            }


