// ILSpyBased#2
using UnityEngine;

public class BattleMessage
{
    private string message;

    private float time;

    public string Message
    {
        get
        {
            return this.message;
        }
    }

    public float TimeMsg
    {
        get
        {
            return this.time;
        }
    }

    public BattleMessage(string msg)
    {
        this.message = msg;
        this.time = Time.time;
    }
}


