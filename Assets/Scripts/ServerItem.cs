// dnSpy decompiler from Assembly-CSharp.dll class: ServerItem
using System;
using UnityEngine;

public class ServerItem
{
	public ServerItem(string country_code, string in_host, int[] in_ports, string in_name, int in_capacity_min, int in_capacity_max)
	{
		this.flag = (Texture2D)Resources.Load(string.Format("CountryFlags/icon_flag_{0}", country_code));
		this.host = in_host;
		this.ports = in_ports;
		this.name = in_name;
		this.capacity_min = in_capacity_min;
		this.capacity_max = in_capacity_max;
		this.ping = new Ping(this.host);
		this.isActive = false;
	}

	public ServerItem(string country_code, string in_host, int[] in_ports, string in_name, int in_capacity_min, int in_capacity_max, bool recommended)
	{
		this.flag = (Texture2D)Resources.Load(string.Format("CountryFlags/icon_flag_{0}", country_code));
		this.host = in_host;
		this.ports = in_ports;
		this.name = in_name;
		this.capacity_min = in_capacity_min;
		this.capacity_max = in_capacity_max;
		this.ping = new Ping(this.host);
		this.isActive = false;
		this.isRecommended = recommended;
	}

	public ServerItem(string country_code, string in_host, int[] in_ports, string in_name, int in_capacity_min, int in_capacity_max, int level_min, int level_max)
	{
		this.flag = (Texture2D)Resources.Load(string.Format("CountryFlags/icon_flag_{0}", country_code));
		this.host = in_host;
		this.ports = in_ports;
		this.name = in_name;
		this.capacity_min = in_capacity_min;
		this.capacity_max = in_capacity_max;
		this.ping = new Ping(this.host);
		this.level_min = level_min;
		this.level_max = level_max;
		this.isActive = false;
	}

	public event ServerItem.ServerItemListenerDelegate OnRefresh;

	public Texture2D Flag
	{
		get
		{
			return this.flag;
		}
	}

	public string Name
	{
		get
		{
			return this.name;
		}
	}

	public string Host
	{
		get
		{
			return this.host;
		}
	}

	public int[] Ports
	{
		get
		{
			return this.ports;
		}
	}

	public int LevelMin
	{
		get
		{
			return this.level_min;
		}
	}

	public int LevelMax
	{
		get
		{
			return this.level_max;
		}
	}

	public int CapacityMin
	{
		get
		{
			return this.capacity_min;
		}
		set
		{
			this.capacity_min = value;
		}
	}

	public int CapacityMax
	{
		get
		{
			return this.capacity_max;
		}
	}

	public bool IsActive
	{
		get
		{
			return this.isActive && this.capacity_min < this.capacity_max;
		}
		set
		{
			this.isActive = value;
		}
	}

	public bool IsRefreshed
	{
		get
		{
			return this.isRefreshed;
		}
		set
		{
			this.isRefreshed = value;
		}
	}

	public ServerItem.ServerItemListenerDelegate ServerItemListener
	{
		get
		{
			return this.serverItemListener;
		}
		set
		{
			this.serverItemListener = value;
		}
	}

	public int Latency
	{
		get
		{
			if (this.ping.isDone)
			{
			}
			if (this.ping != null && this.ping.isDone)
			{
				this.latency = this.ping.time;
				this.ping.DestroyPing();
				ServersList.NeedResort = true;
			}
			return this.latency;
		}
		set
		{
			this.latency = value;
		}
	}

	public int Load
	{
		get
		{
			return this.load;
		}
		set
		{
			this.load = value;
		}
	}

	protected virtual void photonEventListener(PhotonEvent photonEvent)
	{
		byte code = photonEvent.Code;
		if (code != 82)
		{
			if (code == 83)
			{
				if (photonEvent.Data != null)
				{
					this.capacity_min = (int)photonEvent.Data[75] - 1;
				}
				this.isActive = true;
				this.isRefreshed = true;
				if (this.ServerItemListener != null)
				{
					this.serverItemListener(this);
				}
				if (this.OnRefresh != null)
				{
					this.OnRefresh(this);
				}
			}
		}
		else
		{
			this.capacity_min = 0;
			this.isActive = false;
			this.isRefreshed = true;
			if (this.ServerItemListener != null)
			{
				this.serverItemListener(this);
			}
		}
	}

	public bool Refresh()
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"[ServerConf] Refresh() host: ",
			this.host,
			":",
			this.ports[0]
		}));
		this.ping = new Ping(this.host);
		if (MainNetworkController.Instance != null)
		{
			MainNetworkController.Instance.SendMessage("ServerListConnect");
		}
		this.isRefreshed = false;
		return PhotonConnection.Connection.ServerListConnection(this.host + ":" + this.ports[0], new FUFPSServerListItem.PhotonEventListener(this.photonEventListener));
	}

	public bool Disconnect()
	{
		return PhotonConnection.Connection.ServerListDisconnect(this.host + ":" + this.ports[0], new FUFPSServerListItem.PhotonEventListener(this.photonEventListener));
	}

	public bool IsRecommended
	{
		get
		{
			return this.isRecommended;
		}
	}

	public override string ToString()
	{
		return string.Format("[ServerItem: Flag={0}, Name={1}, Host={2}, Ports={3}, LevelMin={4}, LevelMax={5}, CapacityMin={6}, CapacityMax={7}, IsActive={8}, IsRefreshed={9}, ServerItemListener={10}, Latency={11}, Load={12}, IsConnected={13}, NeedReconnect={14}]", new object[]
		{
			this.Flag,
			this.Name,
			this.Host,
			this.Ports,
			this.LevelMin,
			this.LevelMax,
			this.CapacityMin,
			this.CapacityMax,
			this.IsActive,
			this.IsRefreshed,
			this.ServerItemListener,
			this.Latency,
			this.Load,
			this.IsConnected,
			this.NeedReconnect
		});
	}

	public bool IsConnected
	{
		get
		{
			return this.isConnected;
		}
		set
		{
			this.isConnected = value;
		}
	}

	public bool NeedReconnect
	{
		get
		{
			return this.needReconnect;
		}
		set
		{
			this.needReconnect = value;
		}
	}

	private Texture2D flag;

	private string name;

	protected string host;

	protected int[] ports;

	private int level_min;

	private int level_max = 100;

	private int capacity_min;

	private int capacity_max;

	protected bool isActive;

	private bool isRefreshed;

	protected ServerItem.ServerItemListenerDelegate serverItemListener;

	private int latency = 999;

	private int load;

	private Ping ping;

	private bool isRecommended;

	private bool isConnected;

	private bool needReconnect;

	public delegate void ServerItemListenerDelegate(ServerItem serverItem);
}
