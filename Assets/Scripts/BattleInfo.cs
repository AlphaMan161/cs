// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BattleInfo
{
    public enum BattleMessageCode
    {
        NewLevel = 1,
        Domination,
        Revenge
    }

    private float msg_aliveTime = 10f;

    private Rect rectPosition = new Rect(10f, 10f, 100f, 100f);

    private static BattleInfo hInstance;

    private object messageLock;

    private List<BattleMessage> messages;

    private static BattleInfo Instance
    {
        get
        {
            if (BattleInfo.hInstance == null)
            {
                BattleInfo.hInstance = new BattleInfo();
            }
            return BattleInfo.hInstance;
        }
    }

    private BattleInfo()
    {
        this.messageLock = new object();
        this.messages = new List<BattleMessage>();
    }

    public static void AddMessage(BattleMessageCode msgCode, Hashtable data)
    {
        switch (msgCode)
        {
            case BattleMessageCode.NewLevel:
                UnityEngine.Debug.LogError("[BattleInfo] NewLevel");
                try
                {
                    int num = Convert.ToInt32(data[2]);
                    int num2 = Convert.ToInt32(data[3]);
                    if (num == LocalUser.UserID)
                    {
                        UnityEngine.Debug.LogError("Congatulate you have new level " + num2);
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError("[BattleInfo] NewLevel Exception: " + ex.Message);
                }
                break;
            case BattleMessageCode.Domination:
                BattleInfo.Instance.messages.Add(new BattleMessage(string.Format(LanguageManager.GetText("{0} has dominate on {1}"), PlayerManager.GameScore[(int)data[2]].UserName, PlayerManager.GameScore[(int)data[3]].UserName)));
                break;
            case BattleMessageCode.Revenge:
                BattleInfo.Instance.messages.Add(new BattleMessage(string.Format(LanguageManager.GetText("{0} took revenge on {1}"), PlayerManager.GameScore[(int)data[2]].UserName, PlayerManager.GameScore[(int)data[3]].UserName)));
                break;
        }
    }

    public static void AddMessage(Hashtable data)
    {
        try
        {
            BattleMessageCode msgCode = (BattleMessageCode)(int)data[1];
            BattleInfo.AddMessage(msgCode, data);
        }
        catch (Exception arg)
        {
            UnityEngine.Debug.LogError("BattleInfo::AddMessage exception: " + arg);
        }
    }

    private void ClearOld()
    {
        object obj = this.messageLock;
        Monitor.Enter(obj);
        try
        {
            List<int> list = new List<int>();
            List<BattleMessage>.Enumerator enumerator = this.messages.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    BattleMessage current = enumerator.Current;
                    if (current.TimeMsg + this.msg_aliveTime <= Time.time)
                    {
                        list.Add(this.messages.IndexOf(current));
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            List<int>.Enumerator enumerator2 = list.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    int current2 = enumerator2.Current;
                    this.messages.RemoveAt(current2);
                }
            }
            finally
            {
                ((IDisposable)enumerator2).Dispose();
            }
        }
        finally
        {
            Monitor.Exit(obj);
        }
    }

    public static void Draw()
    {
        if (Time.frameCount % 60 == 0)
        {
            BattleInfo.Instance.ClearOld();
            BattleInfo.Instance.rectPosition.x = 10f;
            BattleInfo.Instance.rectPosition.y = 100f;
            BattleInfo.Instance.rectPosition.height = 300f;
            BattleInfo.Instance.rectPosition.width = 300f;
        }
        GUISkin skin = GUI.skin;
        GUILayout.BeginArea(BattleInfo.Instance.rectPosition, GUIContent.none);
        GUILayout.FlexibleSpace();
        object obj = BattleInfo.Instance.messageLock;
        Monitor.Enter(obj);
        try
        {
            List<BattleMessage>.Enumerator enumerator = BattleInfo.Instance.messages.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    BattleMessage current = enumerator.Current;
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.BeginHorizontal(GUIContent.none, "messageLine");
                    GUILayout.Label(current.Message, "info");
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
        }
        finally
        {
            Monitor.Exit(obj);
        }
        GUILayout.EndArea();
        GUI.skin = skin;
    }
}


