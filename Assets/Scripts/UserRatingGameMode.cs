// ILSpyBased#2
using System;
using System.Collections.Generic;

public class UserRatingGameMode
{
    private MapMode.MODE mode;

    private long win;

    private long lose;

    private long playedTime;

    private string playedTimeStr = string.Empty;

    public MapMode.MODE Mode
    {
        get
        {
            return this.mode;
        }
    }

    public long Win
    {
        get
        {
            return this.win;
        }
    }

    public long Lose
    {
        get
        {
            return this.lose;
        }
    }

    public long PlayedTime
    {
        get
        {
            return this.playedTime;
        }
    }

    public string PlayedTimeString
    {
        get
        {
            return this.playedTimeStr;
        }
    }

    public UserRatingGameMode(JSONObject data)
    {
        if (data.type != JSONObject.Type.OBJECT)
        {
            throw new Exception("UserRatingGameMode::UserRatingGameMode unkown data format");
        }
        this.mode = (MapMode.MODE)Convert.ToInt32(data.GetField("m").n);
        this.win = Convert.ToInt64(data.GetField("w").n);
        this.lose = Convert.ToInt64(data.GetField("l").n);
        this.playedTime = Convert.ToInt64(data.GetField("pt").n);
        if (this.playedTime > 60)
        {
            short num = Convert.ToInt16(this.playedTime % 60);
            this.playedTimeStr = LanguageManager.GetTextFormat("{0} h {1} min", (this.playedTime - num) / 60, num);
        }
        else
        {
            this.playedTimeStr = LanguageManager.GetTextFormat("{0} min.", this.playedTime);
        }
    }

    public UserRatingGameMode(Dictionary<string, object> data)
    {
        this.mode = (MapMode.MODE)Convert.ToInt32((!data.ContainsKey("m")) ? ((object)0) : data["m"]);
        this.win = Convert.ToInt64((!data.ContainsKey("w")) ? ((object)0) : data["w"]);
        this.lose = Convert.ToInt64((!data.ContainsKey("l")) ? ((object)0) : data["l"]);
        this.playedTime = Convert.ToInt64((!data.ContainsKey("pt")) ? ((object)0) : data["pt"]);
        if (this.playedTime > 60)
        {
            short num = Convert.ToInt16(this.playedTime % 60);
            this.playedTimeStr = LanguageManager.GetTextFormat("{0} h {1} min", (this.playedTime - num) / 60, num);
        }
        else
        {
            this.playedTimeStr = LanguageManager.GetTextFormat("{0} min.", this.playedTime);
        }
    }

    public void AddFromDictionary(Dictionary<string, object> data)
    {
        this.win += Convert.ToInt64((!data.ContainsKey("w")) ? ((object)0) : data["w"]);
        this.lose += Convert.ToInt64((!data.ContainsKey("l")) ? ((object)0) : data["l"]);
        this.playedTime += Convert.ToInt64((!data.ContainsKey("pt")) ? ((object)0) : data["pt"]);
        if (this.playedTime > 60)
        {
            short num = Convert.ToInt16(this.playedTime % 60);
            this.playedTimeStr = LanguageManager.GetTextFormat("{0} h {1} min", (this.playedTime - num) / 60, num);
        }
        else
        {
            this.playedTimeStr = LanguageManager.GetTextFormat("{0} min.", this.playedTime);
        }
    }
}


