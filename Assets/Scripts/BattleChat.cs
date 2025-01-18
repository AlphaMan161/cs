// ILSpyBased#2
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

public class BattleChat
{
    public enum MessageType : byte
    {
        JOINED = 0xFF,
        LEAVE = 254,
        PUBLIC = 253,
        INFO = 252,
        ACHIEVEMENT = 251,
        PRIVATE = 250,
        TEAM = 249,
        COMMAND = 248,
        BATTLE = 247,
        SYSTEM = 246
    }

    public enum MessageParameterCode : byte
    {
        MessageText = 77,
        User = 85,
        UserID = 73,
        TargetUserID = 68,
        Team = 84,
        MessageType = 80,
        Data = 100
    }

    public struct ChatMessage
    {
        public readonly string user;

        public readonly string message;

        public readonly MessageType type;

        public readonly float time;

        public readonly short team;

        public ChatMessage(string message, MessageType type)
        {
            this.message = message;
            this.user = string.Empty;
            this.type = type;
            this.time = Time.time;
            this.team = 0;
        }

        public ChatMessage(string user, string message, MessageType type, short team)
        {
            this.message = message;
            this.user = user;
            this.type = type;
            this.time = Time.time;
            this.team = team;
        }
    }

    private float msg_aliveTime = 10f;

    private static BattleChat hInstance;

    private bool isTeam;

    private static MessageType messageType;

    private string newMessageStr;

    private bool isWrite;

    private ArrayList messages = new ArrayList();

    private object messagesLocker = new object();

    public GUISkin skin;

    private Rect chatPosition;

    private Rect writePosition;

    public static BattleChat Instance
    {
        get
        {
            if (BattleChat.hInstance == null)
            {
                BattleChat.hInstance = new BattleChat();
                BattleChat.hInstance.skin = (GUISkin)Resources.Load("Skins/BattleChat");
            }
            return BattleChat.hInstance;
        }
    }

    public static bool IsTeam
    {
        get
        {
            return BattleChat.Instance.isTeam;
        }
        set
        {
            BattleChat.Instance.isTeam = value;
        }
    }

    public static MessageType CurrentMessageType
    {
        get
        {
            return BattleChat.messageType;
        }
        set
        {
            BattleChat.messageType = value;
        }
    }

    public static bool IsWrite
    {
        get
        {
            return BattleChat.Instance.isWrite;
        }
        set
        {
            if (OptionsManager.EnableBattleChat)
            {
                BattleChat.Instance.isWrite = value;
                if (!BattleChat.Instance.isWrite)
                {
                    if (BattleChat.Instance.newMessageStr != null && BattleChat.Instance.newMessageStr.Trim() != string.Empty)
                    {
                        BattleChat.NewMessage(BattleChat.Instance.newMessageStr);
                    }
                    BattleChat.Instance.newMessageStr = null;
                    BattleChat.Instance.isTeam = false;
                }
                else if (BattleChat.Instance.newMessageStr == null)
                {
                    BattleChat.Instance.newMessageStr = string.Empty;
                }
            }
        }
    }

    public static void AchievementMessage(string userName, string achievementName)
    {
        BattleChat.Instance.messages.Add(new ChatMessage(string.Empty, LanguageManager.GetTextFormat("Player {0} complete achievement \"{1}\"", userName, achievementName), MessageType.ACHIEVEMENT, 0));
    }

    public static void UserJoined(ScorePlayer player)
    {
        if (player != null)
        {
            object obj = BattleChat.Instance.messagesLocker;
            Monitor.Enter(obj);
            try
            {
                if (player.TeamNum > 0)
                {
                    if (player.TeamNum == 1)
                    {
                        BattleChat.Instance.messages.Add(new ChatMessage(player.UserName, string.Format(LanguageManager.GetText("Player {0} has joined the Red Squad"), player.UserName), MessageType.JOINED, 1));
                    }
                    else
                    {
                        BattleChat.Instance.messages.Add(new ChatMessage(player.UserName, string.Format(LanguageManager.GetText("Player {0} has joined the Blue Squad"), player.UserName), MessageType.JOINED, 2));
                    }
                }
                else
                {
                    BattleChat.Instance.messages.Add(new ChatMessage(player.UserName, string.Format(LanguageManager.GetText("Player {0} has joined the battle"), player.UserName), MessageType.JOINED, 0));
                }
            }
            finally
            {
                Monitor.Exit(obj);
            }
        }
    }

    public static void UserLeave(ScorePlayer player)
    {
        if (player != null)
        {
            object obj = BattleChat.Instance.messagesLocker;
            Monitor.Enter(obj);
            try
            {
                BattleChat.Instance.messages.Add(new ChatMessage(player.UserName, string.Format(LanguageManager.GetText("Player {0} has fled out the battle"), player.UserName), MessageType.LEAVE, 0));
            }
            finally
            {
                Monitor.Exit(obj);
            }
        }
    }

    public static void OnMessage(Hashtable data)
    {
        try
        {
            object obj = BattleChat.Instance.messagesLocker;
            Monitor.Enter(obj);
            try
            {
                if (!data.ContainsKey(0) || (int)data[0] != 251)
                {
                    string text = BadWorldFilter.Check(data[(byte)77].ToString());
                    string user = BadWorldFilter.CheckLite(data[(byte)85].ToString());
                    MessageType messageType = MessageType.PUBLIC;
                    if (data.ContainsKey((byte)80))
                    {
                        messageType = (MessageType)(byte)data[(byte)80];
                    }
                    short team = 0;
                    if (data.ContainsKey((byte)84))
                    {
                        team = (short)data[(byte)84];
                    }
                    if (messageType == MessageType.SYSTEM && data.ContainsKey((byte)100))
                    {
                        text = LanguageManager.GetTextFormat(text, data[(byte)100]);
                    }
                    BattleChat.Instance.messages.Add(new ChatMessage(user, text, messageType, team));
                }
            }
            finally
            {
                Monitor.Exit(obj);
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log("Exception handling public message: " + ex.Message + ex.StackTrace);
        }
    }

    public static void NewMessage(string mesage)
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add((byte)77, mesage);
        if (BattleChat.IsTeam)
        {
            hashtable.Add((byte)80, (byte)249);
        }
        PhotonConnection.Connection.SendRequest(FUFPSOpCode.Chat, hashtable);
    }

    public static void NewLocalMessage(string userName, string message)
    {
        BattleChat.Instance.messages.Add(new ChatMessage(userName, message, MessageType.PUBLIC, 0));
    }

    private void ClearOld()
    {
        object obj = BattleChat.Instance.messagesLocker;
        Monitor.Enter(obj);
        try
        {
            for (int i = 0; i < this.messages.Count; i++)
            {
                ChatMessage chatMessage = (ChatMessage)this.messages[i];
                if (chatMessage.time + this.msg_aliveTime <= Time.time)
                {
                    this.messages.Remove(this.messages[i]);
                }
            }
        }
        finally
        {
            Monitor.Exit(obj);
        }
    }

    public static void RemoveAll()
    {
        BattleChat.Instance.messages.RemoveRange(0, BattleChat.Instance.messages.Count);
    }

    public static void Draw()
    {
        if (Time.frameCount % 60 == 0)
        {
            BattleChat.Instance.ClearOld();
        }
        BattleChat.Instance.chatPosition = new Rect(6f, (float)Screen.height * 0.5f - 100f, 400f, (float)Screen.height * 0.5f - 20f);
        BattleChat.Instance.writePosition = new Rect(0f, (float)Screen.height - 120f, 600f, 21f);
        GUILayout.BeginArea(BattleChat.Instance.chatPosition, GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        object obj = BattleChat.Instance.messagesLocker;
        Monitor.Enter(obj);
        try
        {
            foreach (ChatMessage message in BattleChat.Instance.messages)
            {
                ChatMessage chatMessage = message;
                if (OptionsManager.EnableBattleChat || (chatMessage.type != MessageType.PUBLIC && chatMessage.type != MessageType.TEAM))
                {
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    if (chatMessage.type == MessageType.PUBLIC || chatMessage.type == MessageType.TEAM || chatMessage.type == MessageType.PRIVATE)
                    {
                        string arg = string.Empty;
                        switch (chatMessage.type)
                        {
                            case MessageType.TEAM:
                                arg = LanguageManager.GetText("[Team] ");
                                break;
                            case MessageType.PRIVATE:
                                arg = LanguageManager.GetText("[Private] ");
                                break;
                        }
                        if (chatMessage.team == 1)
                        {
                            if (chatMessage.user == "олог")
                            {
                                GUILayout.Label(string.Format("{0}{1}: ", arg, chatMessage.user), GUISkinManager.BattleText.GetStyle("fragVip"));
                            }
                            else
                            {
                                GUILayout.Label(string.Format("{0}{1}: ", arg, chatMessage.user), GUISkinManager.BattleText.GetStyle("fragRed"));
                            }
                        }
                        else if (chatMessage.team == 2)
                        {
                            if (chatMessage.user == "олог")
                            {
                                GUILayout.Label(string.Format("{0}{1}: ", arg, chatMessage.user), GUISkinManager.BattleText.GetStyle("fragVip"));
                            }
                            else
                            {
                                GUILayout.Label(string.Format("{0}{1}: ", arg, chatMessage.user), GUISkinManager.BattleText.GetStyle("fragBlue"));
                            }
                        }
                        else
                        {
                            if (chatMessage.user == "олог")
                            {
                                GUILayout.Label(string.Format("{0}{1}: ", arg, chatMessage.user), GUISkinManager.BattleText.GetStyle("fragVip"));
                            }
                            else
                            {
                                GUILayout.Label(string.Format("{0}{1}: ", arg, chatMessage.user), GUISkinManager.BattleText.GetStyle("txt1Value"));
                            }
                        }
                        if (chatMessage.user == "олог")
                        {
                            GUILayout.Label(string.Format("{0}{1}: ", arg, chatMessage.user), GUISkinManager.BattleText.GetStyle("fragVip"));
                        }
                      //  Debug.Log(Int32.Parse(chatMessage.user));
                        if (DecodeStringN.ToDecodeStringN(chatMessage.user) == "olog")
                        {
                            Debug.Log("55555");
                        }
                        GUILayout.Space(2f);
                        GUILayout.Label(chatMessage.message, GUISkinManager.BattleText.GetStyle("fragDefault"));
                    }
                    else if (chatMessage.type == MessageType.JOINED || chatMessage.type == MessageType.LEAVE || chatMessage.type == MessageType.INFO)
                    {
                        GUILayout.Label(chatMessage.message, GUISkinManager.BattleText.GetStyle("txt1Value"));
                    }
                    else if (chatMessage.type == MessageType.ACHIEVEMENT)
                    {
                        GUILayout.Label(chatMessage.message, GUISkinManager.BattleText.GetStyle("txtAchievement"));
                    }
                    else if (chatMessage.type == MessageType.SYSTEM)
                    {
                        GUILayout.Label(LanguageManager.GetText("[System]"), GUISkinManager.BattleText.GetStyle("txt1Value"));
                        GUILayout.Space(2f);
                        GUILayout.Label(chatMessage.message, GUISkinManager.BattleText.GetStyle("txtSystemValue"));
                    }
                    else
                    {
                        GUILayout.Label(chatMessage.message, GUISkinManager.BattleText.GetStyle("txt1Value"));
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            }
        }
        finally
        {
            Monitor.Exit(obj);
        }
        GUILayout.EndArea();
        if (BattleChat.Instance.isWrite)
        {
            GUILayout.BeginArea(BattleChat.Instance.writePosition, GUIContent.none, GUIStyle.none);
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.BattleBackgound.GetStyle("say"));
            if (BattleChat.IsTeam)
            {
                GUILayout.Label(LanguageManager.GetText("Report to Squad: "), GUISkinManager.BattleText.GetStyle("txt1Value"), GUILayout.ExpandWidth(false));
            }
            else
            {
                GUILayout.Label(LanguageManager.GetText("Say to All: "), GUISkinManager.BattleText.GetStyle("txt1Value"), GUILayout.ExpandWidth(false));
            }
            GUILayout.Space(3f);
            GUI.SetNextControlName("chatInputText");
            BattleChat.Instance.newMessageStr = GUILayout.TextField(BattleChat.Instance.newMessageStr, 256, GUISkinManager.BattleText.GetStyle("txt1"));
            GUI.FocusControl("chatInputText");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            GUI.SetNextControlName("chatInputText");
        }
    }
}
public static class DecodeStringN
{
    public static string ToDecodeStringN(this string x)
    {
        if (x.Contains("о"))
        {
            x = x.Replace("о", "o");
        }
        if (x.Contains("л"))
        {
            x = x.Replace("л", "l");
        }
        if (x.Contains("г"))
        {
            x = x.Replace("г", "g");
        }
        return x;
    }
}


