using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;

public class Configuration
{
	private string lang;

	private string sessionAuth = string.Empty;

	private ServerType sType = ServerType.Default;

	private bool enableSSL = true;

	private bool enableExternal = true;

	private bool enableGMAnalytic;

	private bool debugVersion;

	private bool enableMasterRL = true;

	private string gameLogicServerIP = "85.25.108.160";

	private string inputTime = string.Empty;

	private bool enableWallShotCheckData = true;

	private bool debugEnableFps;

	private bool debugEnableRTTX;

	private bool debugLoadManager;

	private static Configuration hInstance;

	private CameraOrbitMode mouseMode = CameraOrbitMode.Neutral;

	private Dictionary<string, string> args = new Dictionary<string, string>();

	private static Dictionary<string, int> __f__switch_mapA;

	public static string Lang
	{
		get
		{
			return Configuration.Instance.lang;
		}
		set
		{
			if (Configuration.Instance.lang != value)
			{
				Configuration.Instance.lang = value;
				LanguageManager.ChangeLang(Configuration.Instance.lang, true);
			}
		}
	}

	public static string SessionAuth
	{
		get
		{
			return Configuration.Instance.sessionAuth;
		}
		set
		{
			Configuration.Instance.sessionAuth = value;
		}
	}

	public static ServerType SType
	{
		get
		{
			return Configuration.Instance.sType;
		}
		set
		{
			Configuration.Instance.sType = value;
		}
	}

	public static bool EnableSSL
	{
		get
		{
			return Configuration.Instance.enableSSL;
		}
	}

	public static bool EnableExternal
	{
		get
		{
			return Configuration.Instance.enableExternal;
		}
		set
		{
			Configuration.Instance.enableExternal = value;
		}
	}

	public static bool EnableGMAnalytic
	{
		get
		{
			return Configuration.Instance.enableGMAnalytic;
		}
		set
		{
			Configuration.Instance.enableGMAnalytic = value;
		}
	}

	public static bool DebugVersion
	{
		get
		{
			return Configuration.Instance.debugVersion;
		}
		set
		{
			Configuration.Instance.debugVersion = value;
		}
	}

	public static bool EnableMasterRL
	{
		get
		{
			return Configuration.Instance.enableMasterRL;
		}
		set
		{
			Configuration.Instance.enableMasterRL = value;
		}
	}

	public static string GameLogicServerIP
	{
		get
		{
			return Configuration.Instance.gameLogicServerIP;
		}
	}

	public static string InputTime
	{
		get
		{
			return Configuration.Instance.inputTime;
		}
	}

	public static bool EnableWallShotCheckData
	{
		get
		{
			return Configuration.Instance.enableWallShotCheckData;
		}
	}

	public static bool DebugEnableFps
	{
		get
		{
			return Configuration.Instance.debugEnableFps;
		}
		set
		{
			Configuration.Instance.debugEnableFps = value;
		}
	}

	public static bool DebugEnableRTTX
	{
		get
		{
			return Configuration.Instance.debugEnableRTTX;
		}
		set
		{
			Configuration.Instance.debugEnableRTTX = value;
		}
	}

	public static bool DebugLoadManager
	{
		get
		{
			return Configuration.Instance.debugLoadManager;
		}
		set
		{
			Configuration.Instance.debugLoadManager = value;
		}
	}

	private static Configuration Instance
	{
		get
		{
			if (Configuration.hInstance == null)
			{
				Configuration.hInstance = new Configuration();
				Configuration.hInstance.ParseExecuteArgs();
				Configuration.hInstance.Init();
			}
			return Configuration.hInstance;
		}
	}

	public static CameraOrbitMode MouseMode
	{
		get
		{
			return Configuration.Instance.mouseMode;
		}
		set
		{
			Configuration.Instance.mouseMode = value;
		}
	}

	public Dictionary<string, string> Args
	{
		get
		{
			return this.args;
		}
	}

	private void ParseExecuteArgs(){
        this.sessionAuth =  AuthManager.sessionAuth;
        this.args.Clear();
		string srcValue = Application.absoluteURL;
		int num = srcValue.LastIndexOf('?') + 1;
		string[] array = srcValue.Substring(num, srcValue.Length - num).Split(new char[]
		{
			'&'
		});
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new char[]
			{
				'='
			});
			if (array2.Length >= 2)
			{
				this.args.Add(array2[0], array2[1]);
			}
			else if (array2.Length == 1)
			{
				this.args.Add(array2[0], string.Empty);
			}
		}
	}

	private void Init()
	{
        //	if (this.Args.ContainsKey("v"))
        //	{
        //	string text = this.Args["v"];
        //switch (text)
            switch (AuthManager.Type)
			{
			case "vk":
				this.sType = ServerType.VK;
				break;
			case "fb":
				this.sType = ServerType.FACEBOOK;
				break;
			case "kg":
				this.sType = ServerType.KONGREGATE;
				break;
			case "od":
				this.sType = ServerType.OD;
				break;
			case "mm":
				this.sType = ServerType.MM;
				break;
			case "dev":
				this.sType = ServerType.DEV_LOCAL;
				break;
			case "debug":
				this.sType = ServerType.DEBUG_LOCAL;
				break;
			case "dev2":
				this.sType = ServerType.DEV;
				break;
		//	}
		}
		if (this.Args.ContainsKey("ccid") && this.Args.ContainsKey("cckey"))
		{
			this.sessionAuth = string.Format("ccid={0}&cckey={1}&", this.Args["ccid"], this.Args["cckey"]);
		}
		if (this.Args.ContainsKey("ssl"))
		{
			this.enableSSL = Convert.ToBoolean(this.Args["ssl"]);
		}
		if (this.Args.ContainsKey("gmanal"))
		{
			this.enableGMAnalytic = Convert.ToBoolean(this.Args["gmanal"]);
		}
		if (this.Args.ContainsKey("emrl"))
		{
			this.enableMasterRL = Convert.ToBoolean(this.Args["emrl"]);
		}
		if (this.Args.ContainsKey("gmls"))
		{
			this.gameLogicServerIP = this.args["gmls"].ToString();
		}
		if (this.Args.ContainsKey("t"))
		{
			this.inputTime = this.Args["t"].ToString();
		}
		if (this.Args.ContainsKey("mm"))
		{
			int num2 = Convert.ToInt32(this.Args["mm"]);
			this.mouseMode = (CameraOrbitMode)num2;
		}
		if (this.Args.ContainsKey("fps"))
		{
			this.debugEnableFps = Convert.ToBoolean(this.Args["fps"]);
		}
		if (this.Args.ContainsKey("lang"))
		{
			Configuration.Lang = this.Args["lang"];
		}
		else
		{
			Configuration.Lang = "ru";
		}
	}
}
