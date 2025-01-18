// ILSpyBased#2
using System.Collections;

public class PhotonEvent
{
    private string msg = string.Empty;

    private Hashtable data;

    private int actorID = -1;

    private byte code;

    private short returnCode;

    public string Message
    {
        get
        {
            return this.msg;
        }
    }

    public byte Code
    {
        get
        {
            return this.code;
        }
    }

    public short ReturnCode
    {
        get
        {
            return this.returnCode;
        }
    }

    public Hashtable Data
    {
        get
        {
            return this.data;
        }
    }

    public int ActorID
    {
        get
        {
            return this.actorID;
        }
    }

    public PhotonEvent(byte code)
    {
        this.code = code;
    }

    public PhotonEvent(byte code, short returnCode)
    {
        this.code = code;
        this.returnCode = returnCode;
    }

    public PhotonEvent(byte code, short returnCode, string message)
    {
        this.code = code;
        this.returnCode = returnCode;
        this.msg = message;
    }

    public PhotonEvent(byte code, Hashtable data)
    {
        this.code = code;
        this.data = data;
    }

    public PhotonEvent(byte code, Hashtable data, int actorID)
    {
        this.code = code;
        this.data = data;
        this.actorID = actorID;
    }
}


