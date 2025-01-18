// ILSpyBased#2
using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonConnection
{
    public delegate void PhotonEventListener(PhotonEvent photonEvent);

    private FUFPSLobbyGame photonListener;

    public string ipPort = "192.168.0.13:5055";

    public static readonly string LobbyName = "ThoriumRush_lobby";

    public static readonly string RoomNamePrefix = "ThoriumRushRoom";

    public static readonly string AppName = "FUFPSApplication";

    private static PhotonConnection mInstance;

    private PhotonEventListener photonEventListener;

    private Dictionary<string, FUFPSServerListItem> photonServerList = new Dictionary<string, FUFPSServerListItem>();

    public static PhotonConnection Connection
    {
        get
        {
            if (PhotonConnection.mInstance == null)
            {
                PhotonConnection.mInstance = new PhotonConnection();
            }
            return PhotonConnection.mInstance;
        }
    }

    public bool Connected
    {
        get
        {
            if (this.photonListener == null)
            {
                return false;
            }
            if (this.photonListener.peer == null)
            {
                return false;
            }
            return this.photonListener.peer.PeerState == PeerStateValue.Connected;
        }
    }

    public void DebugReturn(string debug)
    {
        UnityEngine.Debug.Log("[DebugReturn] " + debug);
    }

    public bool Connect(string ipPort)
    {
        this.ipPort = ipPort;
        this.photonListener = new FUFPSLobbyGame(ipPort, PhotonConnection.AppName, PhotonConnection.LobbyName, new FUFPSLobbyGame.DebugOutputDelegate(this.DebugReturn), PhotonConnection.mInstance);
        return this.photonListener.Connect();
    }

    public void Update(bool updateServerList)
    {
        if (this.photonListener != null)
        {
            this.photonListener.Update();
        }
        if (updateServerList)
        {
            Dictionary<string, FUFPSServerListItem>.ValueCollection.Enumerator enumerator = this.photonServerList.Values.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    FUFPSServerListItem current = enumerator.Current;
                    current.Update();
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
        }
    }

    public void UpdateRoomList()
    {
        this.photonListener.UpdateRoomList();
    }

    public void AddEventListener(PhotonEventListener photonEventListener)
    {
        this.photonEventListener = photonEventListener;
    }

    public void FireEvent(PhotonEvent photonEvent)
    {
        if (this.photonEventListener != null)
        {
            this.photonEventListener(photonEvent);
        }
    }

    public void JoinLobby(Hashtable actorProperties)
    {
        this.photonListener.JoinLobby(actorProperties);
    }

    public void LeaveToLobby()
    {
        this.photonListener.LeaveToLobby();
    }

    public void JoinRoom(Hashtable roomProperties, Hashtable actorProperties)
    {
        this.photonListener.JoinRoom(roomProperties, actorProperties);
    }

    public void CreateRoom(Hashtable roomSettings, Hashtable actorProperties)
    {
        this.photonListener.CreateRoom(roomSettings, actorProperties);
    }

    public void SendRequest(FUFPSOpCode OpCode)
    {
        this.photonListener.SendRequest(OpCode);
    }

    public void SendRequest(FUFPSOpCode OpCode, Hashtable data)
    {
        this.photonListener.SendRequest(OpCode, data);
    }

    public void SendRequest(FUFPSOpCode OpCode, Hashtable data, bool reliable)
    {
        this.photonListener.SendRequest(OpCode, data, reliable);
    }

    public void Request(FUFPSParameterKeys ParameterKey)
    {
        this.photonListener.Request(ParameterKey);
    }

    public long getServerTimestamp()
    {
        return this.photonListener.getServerTimestamp();
    }

    public long getLocalTimestamp()
    {
        return this.photonListener.getLocalTimestamp();
    }

    public int getAveragePing()
    {
        return this.photonListener.getAveragePing();
    }

    public void Disconnect()
    {
        if (this.photonListener != null)
        {
            this.photonListener.Disconnect();
        }
    }

    public bool ServerListConnection(string ipPort, FUFPSServerListItem.PhotonEventListener photonEventListener)
    {
        FUFPSServerListItem fUFPSServerListItem;
        if (this.photonServerList.ContainsKey(ipPort))
        {
            fUFPSServerListItem = this.photonServerList[ipPort];
        }
        else
        {
            fUFPSServerListItem = new FUFPSServerListItem(ipPort, PhotonConnection.AppName, "ThoriumRush_list_lobby", new FUFPSServerListItem.DebugOutputDelegate(this.DebugReturn), photonEventListener);
            this.photonServerList.Add(ipPort, fUFPSServerListItem);
        }
        return fUFPSServerListItem.Connect();
    }

    public bool ServerListDisconnect(string ipPort, FUFPSServerListItem.PhotonEventListener photonEventListener)
    {
        if (this.photonServerList.ContainsKey(ipPort))
        {
            FUFPSServerListItem fUFPSServerListItem = this.photonServerList[ipPort];
            fUFPSServerListItem.Disconnect();
            return true;
        }
        return false;
    }

    public bool ServerListSendRequest(string ipPort, FUFPSServerListItem.PhotonEventListener photonEventListener, byte opCode, Hashtable requestData)
    {
        if (this.photonServerList.ContainsKey(ipPort))
        {
            FUFPSServerListItem fUFPSServerListItem = this.photonServerList[ipPort];
            return fUFPSServerListItem.SendRequest(opCode, requestData);
        }
        return false;
    }
}


