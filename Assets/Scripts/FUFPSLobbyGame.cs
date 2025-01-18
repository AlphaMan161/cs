// ILSpyBased#2
using ExitGames.Client.Photon;
using System;
using System.Collections;
using UnityEngine;

public class FUFPSLobbyGame : IPhotonPeerListener
{
    public delegate void DebugOutputDelegate(string debug);

    public string RoomName = string.Empty;

    public bool useTcp;

    private string ipPort;

    private string appName;

    private string lobbyName;

    private bool gameRuns;

    private Hashtable joinData;

    public FUFPSPeer peer;

    private int actorNrReturnedForOpJoin = -1;

    public DebugOutputDelegate DebugListeners;

    private readonly PhotonConnection photonConnection;

    private Hashtable gameList = new Hashtable();

    public FUFPSLobbyGame(string ipPort, string appName, string lobbyName, DebugOutputDelegate debugDelegate, PhotonConnection photonConnection)
    {
        this.ipPort = ipPort;
        this.appName = appName;
        this.lobbyName = lobbyName;
        this.photonConnection = photonConnection;
        this.DebugListeners = debugDelegate;
    }

    public void Update()
    {
        if (this.gameRuns)
        {
            this.Service();
        }
    }

    public void DebugReturn(DebugLevel level, string debug)
    {
        this.DebugListeners(debug);
    }

    public void OnStatusChanged(StatusCode returnCode)
    {
        switch (returnCode)
        {
            case StatusCode.Connect:
                this.photonConnection.FireEvent(new PhotonEvent(101));
                break;
            case StatusCode.Disconnect:
            case StatusCode.DisconnectByServer:
            case StatusCode.DisconnectByServerUserLimit:
            case StatusCode.DisconnectByServerLogic:
                UnityEngine.Debug.Log("Disconnect(ed) peer.state: " + this.peer.PeerState);
                this.photonConnection.FireEvent(new PhotonEvent(104));
                break;
            case StatusCode.ExceptionOnConnect:
                UnityEngine.Debug.LogError("Exception_Connect(ed) peer.state: " + this.peer.PeerState);
                this.photonConnection.FireEvent(new PhotonEvent(82));
                break;
            case StatusCode.Exception:
                UnityEngine.Debug.LogError("Exception peer.state: " + this.peer.PeerState);
                this.photonConnection.FireEvent(new PhotonEvent(82));
                break;
            case StatusCode.SendError:
                UnityEngine.Debug.LogError("SendError! peer.state: " + this.peer.PeerState);
                this.photonConnection.FireEvent(new PhotonEvent(104));
                break;
            case StatusCode.TimeoutDisconnect:
                UnityEngine.Debug.LogError("SendError! peer.state: " + this.peer.PeerState);
                this.photonConnection.FireEvent(new PhotonEvent(104));
                break;
            default:
                UnityEngine.Debug.LogError("Unknown PeerStatusCallback: " + returnCode);
                break;
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        byte operationCode = operationResponse.OperationCode;
        short returnCode = operationResponse.ReturnCode;
        string debugMessage = operationResponse.DebugMessage;
        short num = returnCode;
        if (num == -1)
        {
            UnityEngine.Debug.LogError("Error Response");
            this.photonConnection.FireEvent(new PhotonEvent(104, returnCode));
        }
        else if (returnCode < 0)
        {
            this.photonConnection.FireEvent(new PhotonEvent(82, returnCode, debugMessage));
        }
        else
        {
            byte b = operationCode;
            if (b == 255)
            {
                this.actorNrReturnedForOpJoin = (int)operationResponse[254];
                Hashtable value = (Hashtable)operationResponse[249];
                Hashtable value2 = (Hashtable)operationResponse[248];
                this.joinData = new Hashtable();
                this.joinData[(byte)100] = value2;
                this.joinData[(byte)99] = value;
                this.joinData[(byte)97] = this.actorNrReturnedForOpJoin;
                this.photonConnection.FireEvent(new PhotonEvent(102));
            }
        }
    }

    public void UpdateRoomList()
    {
        this.peer.OpRaiseEvent(86, null, true);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte code = photonEvent.Code;
        int num = (int)photonEvent.Parameters[254];
        Hashtable hashtable = null;
        Hashtable hashtable2 = null;
        switch (code)
        {
            case 255:
            {
                int num2 = (int)photonEvent[254];
                if (this.actorNrReturnedForOpJoin == num2)
                {
                    this.joinData[(byte)98] = (Hashtable)photonEvent[249];
                    this.photonConnection.FireEvent(new PhotonEvent(103));
                }
                else
                {
                    hashtable = (Hashtable)photonEvent[249];
                    hashtable2 = (Hashtable)this.joinData[(byte)99];
                    if (hashtable2 == null)
                    {
                        hashtable2 = new Hashtable();
                    }
                    if (!hashtable2.ContainsKey(num2))
                    {
                        hashtable2[num2] = hashtable;
                    }
                    this.joinData[(byte)99] = hashtable2;
                    UnityEngine.Debug.Log("Actor Joins: " + num);
                    this.photonConnection.FireEvent(new PhotonEvent(105, hashtable, num2));
                }
                break;
            }
            case 252:
            {
                Hashtable hashtable3 = (Hashtable)photonEvent.Parameters[245];
                this.gameList = new Hashtable();
                foreach (string key3 in hashtable3.Keys)
                {
                    this.gameList[key3] = hashtable3[key3];
                }
                this.photonConnection.FireEvent(new PhotonEvent(86, this.gameList));
                break;
            }
            case 251:
            {
                Hashtable hashtable3 = (Hashtable)photonEvent.Parameters[245];
                foreach (string key4 in hashtable3.Keys)
                {
                    if (hashtable3[key4].GetType() == Type.GetType("System.String[]"))
                    {
                        this.gameList[key4] = hashtable3[key4];
                    }
                    else
                    {
                        this.gameList.Remove(key4);
                    }
                }
                this.photonConnection.FireEvent(new PhotonEvent(86, this.gameList));
                break;
            }
            case 254:
                hashtable2 = (Hashtable)this.joinData[(byte)99];
                if (hashtable2 != null && !hashtable2.ContainsKey(num))
                {
                    hashtable2.Remove(num);
                }
                this.photonConnection.FireEvent(new PhotonEvent(106, null, num));
                break;
            case 100:
                this.photonConnection.FireEvent(new PhotonEvent(100, (Hashtable)photonEvent.Parameters[245], num));
                break;
            case 67:
                this.photonConnection.FireEvent(new PhotonEvent(67, (Hashtable)photonEvent.Parameters[245], num));
                break;
            default:
                this.photonConnection.FireEvent(new PhotonEvent(code, (Hashtable)photonEvent.Parameters[245], num));
                break;
        }
    }

    public bool ConnectAndJoinRoom(Hashtable joinRoomProperties, Hashtable joinActorProperties)
    {
        this.actorNrReturnedForOpJoin = -1;
        if (this.joinData != null)
        {
            this.joinData.Clear();
            this.joinData = null;
        }
        return this.Connect();
    }

    public bool Connect()
    {
        if (this.peer == null)
        {
            this.peer = new FUFPSPeer(this);
        }
        else if (this.peer.PeerState != 0)
        {
            this.DebugReturn("already connected! disconnect first.");
            this.photonConnection.FireEvent(new PhotonEvent(82));
            return false;
        }
        this.peer.DebugOut = DebugLevel.ERROR;
        try
        {
            if (!this.peer.Connect(this.ipPort, this.appName))
            {
                this.DebugReturn("not connected");
                this.photonConnection.FireEvent(new PhotonEvent(82));
                return false;
            }
        }
        catch (Exception arg)
        {
            this.DebugReturn("Security Exception (check policy file): " + arg);
            this.photonConnection.FireEvent(new PhotonEvent(82));
            return false;
            IL_00c7:;
        }
        this.gameRuns = true;
        return true;
    }

    public void JoinLobby(Hashtable actorProperties)
    {
        this.peer.OpJoin(this.lobbyName, null, actorProperties, false);
    }

    public void LeaveToLobby()
    {
        this.peer.OpLeave(this.RoomName);
    }

    public void CreateRoom(Hashtable roomProperties, Hashtable actorProperties)
    {
        this.JoinRoom(roomProperties, actorProperties);
    }

    public void JoinRoom(Hashtable roomProperties, Hashtable actorProperties)
    {
        this.RoomName = (string)roomProperties["name"];
        //this.actorNrReturnedForOpJoin = -1;
        if (this.joinData != null)
        {
            this.joinData.Clear();
            this.joinData = null;
        }
        this.peer.OpJoinFromLobby(this.RoomName, this.lobbyName, roomProperties, actorProperties, true);
    }

    internal void Disconnect()
    {
        if (this.peer != null)
        {
            this.peer.Disconnect();
        }
        this.gameRuns = false;
    }

    public void Service()
    {
        this.peer.Service();
    }

    public void DebugReturn(string debug)
    {
        this.DebugListeners(debug);
    }

    public void SendRequest(FUFPSOpCode OpCode)
    {
        this.peer.OpRaiseEvent((byte)OpCode, null, true);
    }

    public void SendRequest(FUFPSOpCode OpCode, Hashtable data)
    {
        this.peer.OpRaiseEvent((byte)OpCode, data, true);
    }

    public void SendRequest(FUFPSOpCode OpCode, Hashtable data, bool reliable)
    {
        if (OpCode != FUFPSOpCode.Move)
        {
            UnityEngine.Debug.Log("SendEvent: " + OpCode.ToString());
        }
        this.peer.OpRaiseEvent((byte)OpCode, data, reliable);
    }

    public void Request(FUFPSParameterKeys ParameterKey)
    {
        byte b = (byte)ParameterKey;
        if (b == 100)
        {
            this.photonConnection.FireEvent(new PhotonEvent(107, this.joinData));
        }
    }

    public long getServerTimestamp()
    {
        return this.peer.ServerTimeInMilliSeconds;
    }

    public long getLocalTimestamp()
    {
        return this.peer.LocalTimeInMilliSeconds;
    }

    public int getAveragePing()
    {
        return this.peer.RoundTripTime;
    }
}


