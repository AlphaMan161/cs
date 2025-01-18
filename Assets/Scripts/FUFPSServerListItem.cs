// ILSpyBased#2
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class FUFPSServerListItem : IPhotonPeerListener
{
    public delegate void DebugOutputDelegate(string debug);

    public delegate void PhotonEventListener(PhotonEvent photonEvent);

    public string RoomName = string.Empty;

    private string ipPort;

    private string appName;

    private string lobbyName;

    public FUFPSPeer peer;

    private int actorNrReturnedForOpJoin = -1;

    public DebugOutputDelegate DebugListeners;

    private PhotonEventListener photonEventListener;

    public FUFPSServerListItem(string ipPort, string appName, string lobbyName, DebugOutputDelegate debugDelegate, PhotonEventListener photonEventListener)
    {
        this.ipPort = ipPort;
        this.appName = appName;
        this.lobbyName = lobbyName;
        this.DebugListeners = debugDelegate;
        this.photonEventListener = photonEventListener;
    }

    public void Update()
    {
        this.Service();
    }

    public void DebugReturn(DebugLevel level, string debug)
    {
        this.DebugListeners(debug);
    }

    public void OnStatusChanged(StatusCode returnCode)
    {
        switch (returnCode)
        {
            case StatusCode.DisconnectByServerLogic:
                break;
            case StatusCode.Connect:
            {
                Hashtable hashtable = new Hashtable();
                hashtable[FUFPSParameterKeys.AuthID] = (int)Auth.UserID;
                hashtable[FUFPSParameterKeys.AuthKey] = Auth.Key;
                this.JoinLobby(hashtable);
                break;
            }
            case StatusCode.Disconnect:
                this.photonEventListener(new PhotonEvent(104, null));
                break;
            case StatusCode.ExceptionOnConnect:
                UnityEngine.Debug.LogError(string.Format("Exception_Connect(ed) serverAddress:{0} peer.state: {1}", this.peer.ServerAddress, this.peer.PeerState));
                this.photonEventListener(new PhotonEvent(83, null));
                break;
            case StatusCode.SecurityExceptionOnConnect:
                UnityEngine.Debug.LogError(string.Format("Exception serverAddress:{0} peer.state: {1}", this.peer.ServerAddress, this.peer.PeerState));
                this.photonEventListener(new PhotonEvent(82));
                break;
            case StatusCode.Exception:
                UnityEngine.Debug.LogError(string.Format("Exception serverAddress:{0} peer.state: {1}", this.peer.ServerAddress, this.peer.PeerState));
                this.photonEventListener(new PhotonEvent(82));
                break;
            case StatusCode.SendError:
                UnityEngine.Debug.LogError(string.Format("Send Error serverAddress:{0} peer.state: {1}", this.peer.ServerAddress, this.peer.PeerState));
                this.photonEventListener(new PhotonEvent(82));
                break;
            default:
                UnityEngine.Debug.LogError(string.Format("Unknown serverAddress:{0} PeerStatusCallback: {1}", this.peer.ServerAddress, returnCode));
                this.photonEventListener(new PhotonEvent(82));
                break;
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        byte operationCode = operationResponse.OperationCode;
        int returnCode = operationResponse.ReturnCode;
        switch (returnCode)
        {
            case -1:
                break;
            case -3:
            {
                Dictionary<byte, object> parameters = operationResponse.Parameters;
                if (parameters.ContainsKey(75))
                {
                    this.photonEventListener(new PhotonEvent(83, new Hashtable(parameters)));
                }
                else
                {
                    this.photonEventListener(new PhotonEvent(82, new Hashtable(parameters)));
                }
                break;
            }
            case -4:
                this.photonEventListener(new PhotonEvent(75, null));
                break;
            default:
            {
                byte b = operationCode;
                if (b == 255)
                {
                    this.photonEventListener(new PhotonEvent(operationCode));
                }
                else
                {
                    UnityEngine.Debug.LogError("Unknown PeerStatusCallback: " + returnCode);
                }
                break;
            }
        }
    }

    public void UpdateRoomList()
    {
        this.peer.OpRaiseEvent(86, null, true);
    }

    public void OnEvent(EventData photonEventData)
    {
        byte code = photonEventData.Code;
        int actorID = -1;
        if (photonEventData.Parameters.ContainsKey(225))
        {
            actorID = (int)photonEventData.Parameters[225];
        }
        this.photonEventListener(new PhotonEvent(code, (Hashtable)photonEventData[213], actorID));
    }

    public bool Connect()
    {
        if (this.peer == null)
        {
            this.peer = new FUFPSPeer(this);
        }
        else if (this.peer.PeerState != 0 && this.peer.PeerState != PeerStateValue.Disconnecting)
        {
            this.DebugReturn("already connected! disconnect first. Peer State: " + this.peer.PeerState.ToString());
            this.photonEventListener(new PhotonEvent(82, null));
            return false;
        }
        try
        {
            if (!this.peer.Connect(this.ipPort, this.appName))
            {
                UnityEngine.Debug.LogError("Not connected");
                this.photonEventListener(new PhotonEvent(82, null));
                return false;
            }
        }
        catch (SecurityException arg)
        {
            this.DebugReturn("Security Exception (check policy file): " + arg);
            this.photonEventListener(new PhotonEvent(82, null));
            return false;
            IL_00e8:;
        }
        this.peer.DebugOut = DebugLevel.ERROR;
        return true;
    }

    public void JoinLobby(Hashtable actorProperties)
    {
        this.peer.OpJoin(this.lobbyName, null, actorProperties, false);
    }

    internal void Disconnect()
    {
        if (this.peer != null)
        {
            this.peer.Disconnect();
        }
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

    public bool SendRequest(byte OpCode, Hashtable data)
    {
        if (this.peer != null && this.peer.PeerState == PeerStateValue.Connected)
        {
            this.peer.OpRaiseEvent(OpCode, data, true);
            return true;
        }
        return false;
    }

    public void SendRequest(FUFPSOpCode OpCode, Hashtable data, bool reliable)
    {
        if (OpCode != FUFPSOpCode.Move)
        {
            UnityEngine.Debug.Log("SendEvent: " + OpCode.ToString());
        }
        this.peer.OpRaiseEvent((byte)OpCode, data, reliable);
    }

    public int getAveragePing()
    {
        return this.peer.RoundTripTime;
    }
}


