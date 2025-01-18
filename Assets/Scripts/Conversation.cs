// ILSpyBased#2
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Conversation
{
    private object lockDisplayMessages = new object();

    private bool newMessages;

    private SocialPlayer user;

    private List<ChatMessage> messages = new List<ChatMessage>();

    private List<ChatMessage> displayMessages = new List<ChatMessage>();

    private bool refreshDisplayMessages = true;

    private Vector2 scroll = new Vector2(0f, 0f);

    private bool tail = true;

    public object LockDisplayMessages
    {
        get
        {
            return this.lockDisplayMessages;
        }
    }

    public bool NewMessages
    {
        get
        {
            return this.newMessages;
        }
    }

    public SocialPlayer User
    {
        get
        {
            return this.user;
        }
    }

    public List<ChatMessage> DisplayMessages
    {
        get
        {
            this.newMessages = false;
            return this.displayMessages;
        }
    }

    public Vector2 Scroll
    {
        get
        {
            return this.scroll;
        }
        set
        {
            if (this.scroll.y > value.y && this.scroll.y != 3.40282347E+38f)
            {
                this.tail = false;
            }
            this.scroll = value;
        }
    }

    public bool Tail
    {
        get
        {
            return this.tail;
        }
        set
        {
            this.tail = value;
        }
    }

    public Conversation(SocialPlayer user)
    {
        this.user = user;
    }

    public bool isPrivate()
    {
        return this.user == null;
    }

    public void Add(ChatMessage chatMessage)
    {
        if (chatMessage.UserName != LocalUser.Name)
        {
            this.newMessages = true;
        }
        this.messages.Add(chatMessage);
        this.refreshDisplayMessages = true;
    }

    public void TryRefreshDisplayMessages()
    {
        if (this.refreshDisplayMessages)
        {
            this.RefreshDisplayMessages();
        }
    }

    private void RefreshDisplayMessages()
    {
        this.refreshDisplayMessages = false;
        object obj = this.lockDisplayMessages;
        Monitor.Enter(obj);
        try
        {
            if (this.displayMessages.Count > 200)
            {
                this.displayMessages.RemoveRange(0, 10);
            }
            this.displayMessages.AddRange(this.messages);
            this.messages = new List<ChatMessage>();
            if (this.Tail)
            {
                this.scroll.y = 3.40282347E+38f;
            }
        }
        finally
        {
            Monitor.Exit(obj);
        }
    }
}


