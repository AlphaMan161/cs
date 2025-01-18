// dnSpy decompiler from Assembly-CSharp.dll class: ServerConf
using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class ServerConf
{
	public static ServerItem MasterServerItem
	{
		get
		{
			return ServerConf.Instance.masterServerItem;
		}
	}

	public static ServerConf Instance
	{
		get
		{
			if (ServerConf.instance == null)
			{
				ServerConf.instance = new ServerConf();
				ServerConf.instance.InitServerList();
			}
			return ServerConf.instance;
		}
	}

	public static List<ServerItem> ServerList
	{
		get
		{
			return ServerConf.Instance.serverList;
		}
	}

	public static List<ServerItem> GMList
	{
		get
		{
			return ServerConf.Instance.gmList;
		}
	}

	private void InitServerList()
	{
        if (Configuration.SType == ServerType.DEBUG_LOCAL || Configuration.SType == ServerType.DEV_LOCAL)
		{
			this.serverList.Add(new ServerItem("UA", "192.168.0.10", new int[]
			{
				5055,
				5056,
				5057,
				5255
			}, "(LOCAL) 192.168.0.10", 0, 100));
			this.serverList.Add(new ServerItem("UA", "192.168.0.13", new int[]
			{
				5055,
				5056,
				5057,
				5255
			}, "(LOCAL) 192.168.0.13", 0, 100));
			this.masterServerItem = new ServerItem("UA", "192.168.0.10", new int[]
			{
				5056,
				5057,
				5255
			}, "Мастер Сервер", 0, 20000);
		}
		else if (Configuration.SType == ServerType.VK)
		{
			this.masterServerItem = new ServerItem("UA", "188.138.95.48", new int[]
			{
				5056,
				5057,
				5255
			}, "Мастер Сервер", 0, 20000);
			this.serverList.Add(new ServerItem("RU", "85.25.248.88", new int[]
			{
				5055,
				5056,
				5057,
				5255
			}, "Сервер 1 ", 0, 1100));
			this.serverList.Add(new ServerItem("RU", "188.138.95.50", new int[]
			{
				5055,
				5056,
				5057,
				5255
			}, "Сервер 2 Октябрята (1 - 10 уровень)", 0, 900, 0, 10));
			this.serverList.Add(new ServerItem("RU", "188.138.95.48", new int[]
			{
				5055,
				5056,
				5057,
				5255
			}, "Сервер 3", 0, 900));
			this.serverList.Add(new ServerItem("RU", "85.25.108.144", new int[]
			{
				5055,
				5056,
				5057,
				5255
			}, "Сервер 4 Октябрята (1 - 10 уровень)", 0, 700, 0, 10));
			this.serverList.Add(new ServerItem("RU", "188.138.88.37", new int[]
			{
				5055,
				5056,
				5057,
				5255
			}, "Сервер 5 Октябрята (1 - 10 уровень)", 0, 800, 0, 10));
			this.serverList.Add(new ServerItem("RU", "85.25.99.68", new int[]
			{
				5055,
				5056,
				5057,
				5255
			}, "Сервер 6", 0, 1300));
			this.serverList.Add(new ServerItem("RU", "188.138.100.90", new int[]
			{
				5055,
				5056,
				5057,
				5255
			}, "Сервер 7", 0, 1300));
		}
	}

	public void InitGmServerList(JSONNode json)
	{
		this.gmList.Clear();
		if (json != null)
		{
			foreach (JSONNode jsonnode in json.Childs)
			{
				string[] array = jsonnode["p"].Value.Split(new char[]
				{
					','
				});
				List<int> list = new List<int>();
				foreach (string text in array)
				{
					if (text.Trim() != string.Empty)
					{
						list.Add(Convert.ToInt32(text.Trim()));
					}
				}
				bool flag = !(jsonnode["m"].Value == "0");
				bool flag2 = false;
				if (jsonnode["iD"] != null)
				{
					flag2 = !(jsonnode["iD"].Value == "0");
				}
				UnityEngine.Debug.LogError(string.Format("[ServerConf] GM h:{0} portsIList:{1} n:{2} pL:{3} lM:{4}, lMa:{5} isMaster:{6} isRecommended:{7}", new object[]
				{
					jsonnode["h"].Value,
					list,
					jsonnode["n"].Value,
					jsonnode["pL"].Value,
					jsonnode["lM"].Value,
					jsonnode["lMa"].Value,
					flag,
					flag2
				}));
				ServerItem item = new ServerItem("DE", jsonnode["h"].Value, list.ToArray(), jsonnode["n"].Value, 0, 50000, flag2);
				this.gmList.Add(item);
			}
		}
		else
		{
			this.gmList.Add(new ServerItem("DE", Configuration.GameLogicServerIP, new int[]
			{
				5058
			}, "gm0", 0, 50000));
		}
	}

	public void InitServerList(JSONNode json)
	{
		this.serverList.Clear();
		foreach (JSONNode jsonnode in json.Childs)
		{
			string[] array = jsonnode["p"].Value.Split(new char[]
			{
				','
			});
			List<int> list = new List<int>();
			foreach (string text in array)
			{
				if (text.Trim() != string.Empty)
				{
					list.Add(Convert.ToInt32(text.Trim()));
				}
			}
			bool flag = !(jsonnode["m"].Value == "0");
			ServerItem item = new ServerItem("DE", jsonnode["h"].Value, list.ToArray(), jsonnode["n"].Value, 0, Convert.ToInt32(jsonnode["pL"].Value), Convert.ToInt32(jsonnode["lM"].Value), Convert.ToInt32(jsonnode["lMa"].Value));
			if (flag)
			{
				this.masterServerItem = item;
			}
			else
			{
				this.serverList.Add(item);
			}
		}
	}

	private ServerItem masterServerItem;

	private static ServerConf instance;

	private List<ServerItem> serverList = new List<ServerItem>();

	private List<ServerItem> gmList = new List<ServerItem>();
}
