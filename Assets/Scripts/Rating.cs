// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections.Generic;

public class Rating
{
    public enum SortDirection
    {
        Asc = 1,
        Desc
    }

    public enum SortType
    {
        Level = 1,
        Exp,
        Kill,
        Death,
        KD,
        Achievement,
        PlayedTime
    }

    public delegate void RatingEventHandler(object sender);

    private static Rating hInstance;

    private SortType type;

    private int currentPage;

    private int maxPage = 100;

    private List<UserRating> userList;

    private UserRating currentUser;

    public static Rating Instance
    {
        get
        {
            if (Rating.hInstance == null)
            {
                Rating.hInstance = new Rating();
            }
            return Rating.hInstance;
        }
    }

    public static SortType Type
    {
        get
        {
            return Rating.Instance.type;
        }
        set
        {
            if (Rating.Instance.type != value)
            {
                Rating.Instance.type = value;
                Rating.Instance.UpdateInfo();
            }
        }
    }

    public static int Page
    {
        get
        {
            return Rating.Instance.currentPage;
        }
        set
        {
            if (value > 0 && value < 100 && value <= Rating.Instance.maxPage && Rating.Instance.currentPage != value)
            {
                Rating.Instance.currentPage = value;
                Rating.Instance.UpdateInfo();
            }
        }
    }

    public static int MaxPage
    {
        get
        {
            return Rating.Instance.maxPage;
        }
    }

    public static List<UserRating> UserList
    {
        get
        {
            return Rating.Instance.userList;
        }
    }

    public static UserRating CurrentUser
    {
        get
        {
            return Rating.Instance.currentUser;
        }
    }

    public event RatingEventHandler OnLoad;

    private Rating()
    {
        this.type = SortType.Exp;
        this.currentPage = 1;
        this.userList = new List<UserRating>();
        this.UpdateInfo();
    }

    private void UpdateInfo()
    {
        this.userList = new List<UserRating>();
        Ajax.Request(string.Format("{0}&t={1}&p={2}", WebUrls.TOP_RATING_URL, (int)this.type, this.currentPage - 1), new AjaxRequest.AjaxHandler(this.LoadRating));
    }

    private void LoadRating(object result, AjaxRequest request)
    {
        JSONObject jSONObject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
        if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b && jSONObject.GetField("users") != null && jSONObject.GetField("users").type != 0)
        {
            Rating.Instance.userList = UserRating.UserRaringFromList(JSONNode.Parse(jSONObject.GetField("users").input_str));
            if (jSONObject.GetField("musers") != null)
            {
                Rating.Instance.maxPage = Convert.ToInt32(jSONObject.GetField("musers").str) / 100 + 1;
            }
        }
        if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b && jSONObject.GetField("uinfo") != null && jSONObject.GetField("uinfo").type != 0)
        {
            Rating.Instance.currentUser = new UserRating(JSONNode.Parse(jSONObject["uinfo"].input_str));
        }
        if (this.OnLoad != null)
        {
            this.OnLoad(this);
        }
    }
}


