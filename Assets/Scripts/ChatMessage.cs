// ILSpyBased#2
using System;

public class ChatMessage
{
    private string userName;

    private string message;

    private BattleChat.MessageType type;

    private TimeSpan time;

    public string UserName
    {
        get
        {
            return this.userName;
        }
    }

    public string Message
    {
        get
        {
            return this.message;
        }
    }

    public BattleChat.MessageType Type
    {
        get
        {
            return this.type;
        }
    }

    public TimeSpan Time
    {
        get
        {
            return this.time;
        }
    }

    public ChatMessage(string user, string message, BattleChat.MessageType type)
    {
        this.message = message;
        this.userName = user;
        this.type = type;
        this.time = DateTime.Now.TimeOfDay;
    }
}


