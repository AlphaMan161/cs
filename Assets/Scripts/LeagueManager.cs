// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Threading;

public class LeagueManager
{
    public delegate void LeagueEventHandler(object sender);

    private List<League> leagueList = new List<League>();

    private short leagueIndex = 1;

    private League selectedLeague;

    private static LeagueManager hInstance = null;

    private static object syncLook = new object();

    private UserRating currentUser;

    private bool isInit;

    private YesterDayBest best;

    public short LeagueIndex
    {
        get
        {
            return this.leagueIndex;
        }
        set
        {
            if (value > 0 && value <= this.leagueList.Count)
            {
                this.selectedLeague = this.leagueList[value - 1];
            }
            this.leagueIndex = value;
        }
    }

    public League SelectedLeague
    {
        get
        {
            return this.selectedLeague;
        }
    }

    public static LeagueManager Instance
    {
        get
        {
            if (LeagueManager.hInstance == null)
            {
                object obj = LeagueManager.syncLook;
                Monitor.Enter(obj);
                try
                {
                    if (LeagueManager.hInstance == null)
                    {
                        LeagueManager.hInstance = new LeagueManager();
                    }
                }
                finally
                {
                    Monitor.Exit(obj);
                }
            }
            return LeagueManager.hInstance;
        }
    }

    public UserRating CurrentUser
    {
        get
        {
            return this.currentUser;
        }
    }

    public YesterDayBest Best
    {
        get
        {
            return this.best;
        }
    }

    public event LeagueEventHandler OnLoad;

    public static void Init()
    {
        if (!LeagueManager.Instance.isInit)
        {
            LeagueManager.Instance.isInit = true;
            Ajax.Request(WebUrls.TOP_RATING_LEAGUE_URL, new AjaxRequest.AjaxHandler(LeagueManager.Instance.OnInit));
        }
    }

    private void OnInit(object result, AjaxRequest request)
    {
        JSONObject jSONObject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
        JSONNode jSONNode = JSONNode.Parse(result.ToString());
        if (jSONNode["result"] != (object)null && jSONNode["result"].AsBool)
        {
            JSONObject field = jSONObject.GetField("ls");
            JSONNode jSONNode2 = jSONNode["ls"];
            this.currentUser = new UserRating(jSONNode["u"]);
            this.leagueList.Clear();
            this.leagueList.Add(new League(jSONNode["l1"], jSONNode2["1"]));
            this.leagueList.Add(new League(jSONNode["l2"], jSONNode2["2"]));
            this.leagueList.Add(new League(jSONNode["l3"], jSONNode2["3"]));
            this.leagueList.Add(new League(jSONNode["l4"], jSONNode2["4"]));
            this.leagueList.Add(new League(jSONNode["l5"], jSONNode2["5"]));
            this.leagueList.Add(new League(jSONNode["l6"], jSONNode2["6"]));
            this.leagueList.Add(new League(jSONNode["l7"], jSONNode2["7"]));
            this.leagueList.Add(new League(jSONNode["l8"], jSONNode2["8"]));
            this.leagueList.Add(new League(jSONNode["l9"], jSONNode2["9"]));
            this.leagueList.Add(new League(jSONNode["l10"], jSONNode2["10"]));
            this.leagueList.Add(new League(jSONNode["l11"], jSONNode2["11"]));
            this.leagueList.Add(new League(jSONNode["l12"], jSONNode2["12"]));
            this.leagueList.Add(new League(jSONNode["l13"], jSONNode2["13"]));
            this.leagueList.Add(new League(jSONNode["l14"], jSONNode2["14"]));
            this.leagueList.Add(new League(jSONNode["l15"], jSONNode2["15"]));
            this.LeagueIndex = 1;
            this.AddLocalUser2League();
        }
        if (this.OnLoad != null)
        {
            this.OnLoad(this);
        }
    }

    public static void InitYesterdayBest()
    {
        Ajax.Request(WebUrls.TOP_YESTERDAY_BEST_URL, new AjaxRequest.AjaxHandler(LeagueManager.Instance.OnInitYesterdayBest));
    }

    private void OnInitYesterdayBest(object result, AjaxRequest request)
    {
        JSONNode jSONNode = JSONNode.Parse(Ajax.DecodeUtf(result.ToString()));
        if (jSONNode["result"] != (object)null)
        {
            this.best = new YesterDayBest(jSONNode["yb"]);
        }
    }

    private int CalculateLocalUserLeague()
    {
        for (int num = this.leagueList.Count - 1; num >= 0; num--)
        {
            if (this.leagueList[num].MinExp <= this.currentUser.Exp && (this.leagueList[num].MaxExp > this.currentUser.Exp || this.leagueList[num].MaxExp == 0))
            {
                return num + 1;
            }
        }
        return 0;
    }

    private void AddLocalUser2League()
    {
        int num = this.CalculateLocalUserLeague();
        if (num != 0)
        {
            League league = this.leagueList[num - 1];
            int num2 = -1;
            List<UserRating>.Enumerator enumerator = league.Users.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    UserRating current = enumerator.Current;
                    if (current.UserID == this.currentUser.UserID)
                    {
                        num2 = league.Users.IndexOf(current);
                        break;
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            if (num2 > 0)
            {
                league.Users[num2] = this.currentUser;
                league.ReSort();
            }
            else
            {
                league.Users.Add(this.currentUser);
            }
            league.ReSort();
            this.LeagueIndex = (short)num;
            this.currentUser.League = num;
        }
    }

    public void AddLocalData(Dictionary<byte, object> uStatRating)
    {
        if (this.currentUser != null)
        {
            UserRating userRating = null;
            List<League>.Enumerator enumerator = this.leagueList.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    League current = enumerator.Current;
                    userRating = current.Users.Find((UserRating x) => x.UserID == LocalUser.UserID);
                    if (userRating != null)
                    {
                        userRating.AddFromDictionary(uStatRating);
                        break;
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
        }
    }
}


