// dnSpy decompiler from Assembly-CSharp.dll class: LocalUser
using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class LocalUser : Player
{
	private LocalUser()
	{
	}

	public static event LocalUser.LocalUserEventHandler OnValid;

	public static event LocalUser.LocalUserEventHandler OnError;

	private static LocalUser Instance
	{
		get
		{
			if (LocalUser.hInstance == null)
			{
				LocalUser.hInstance = new LocalUser();
			}
			return LocalUser.hInstance;
		}
	}

	public new static int UserID
	{
		get
		{
			return LocalUser.Instance.user_id;
		}
	}

	public new static string Name
	{
		get
		{
			return LocalUser.Instance.name;
		}
		set
		{
			LocalUser.Instance.name = value;
		}
	}

	public static string FullName
	{
		get
		{
			return LocalUser.Instance.fullName;
		}
	}

	public new static short Level
	{
		get
		{
			return LocalUser.Instance.level;
		}
		set
		{
			LocalUser.Instance.level = value;
		}
	}

	public static int Money
	{
		get
		{
			return LocalUser.Instance.money;
		}
		set
		{
			LocalUser.Instance.money = value;
		}
	}

	public static int Exp
	{
		get
		{
			return LocalUser.Instance.exp;
		}
		set
		{
			LocalUser.Instance.exp = value;
		}
	}

	public static Clan Clan
	{
		get
		{
			return LocalUser.Instance.clan;
		}
		set
		{
			LocalUser.Instance.clan = value;
		}
	}

	public static int MinExp
	{
		get
		{
			return LocalUser.Instance.minExp;
		}
	}

	public static int MaxExp
	{
		get
		{
			return LocalUser.Instance.maxExp;
		}
	}

	public static bool NeedLiteRefresh
	{
		get
		{
			return LocalUser.Instance.needLiteRefresh;
		}
		set
		{
			LocalUser.Instance.needLiteRefresh = value;
		}
	}

	public static Permission Permission
	{
		get
		{
			return LocalUser.Instance.permission;
		}
	}

	public static void Refresh()
	{
		Ajax.Request(WebUrls.USER_INFO_EXTENDED_URL, new AjaxRequest.AjaxHandler(LocalUser.Instance.OnRefresh));
	}

	public static void RefreshLite()
	{
		Ajax.Request(WebUrls.USER_INFO_URL, new AjaxRequest.AjaxHandler(LocalUser.Instance.OnRefreshLite));
	}

	private void OnUid(object result, AjaxRequest request)
	{
	}

	private void BaseInfo(JSONObject json)
	{
		UnityEngine.Debug.Log(json.input_str);
		JSONObject field = json.GetField("info");
		JSONObject field2 = json.GetField("info").GetField("exp");
		this.user_id = Convert.ToInt32(field.GetField("u_id").n);
		this.name = Ajax.DecodeUtf(field.GetField("un").str);
		if (field.GetField("fname") != null && field.GetField("fname").type == JSONObject.Type.STRING)
		{
			this.fullName = field.GetField("fname").str;
		}
		if (json["uid"] != null)
		{
			Ajax.Request(WebUrls.USER_INFO_UID + "&uid=" + SystemInfo.deviceUniqueIdentifier.ToString(), new AjaxRequest.AjaxHandler(LocalUser.Instance.OnUid));
		}
		this.level = Convert.ToInt16(field.GetField("lvl").n);
		this.exp = Convert.ToInt32(field2.GetField("cur").n);
		this.minExp = Convert.ToInt32(field2.GetField("min").n);
		this.maxExp = Convert.ToInt32(field2.GetField("max").n);
		this.money = Convert.ToInt32(field.GetField("vcur").n);
		if (json.GetField("cl") != null)
		{
			this.clan = new Clan(JSONNode.Parse(json["cl"].input_str));
			if (json["cl"]["ue"] != null)
			{
			}
			if (json["cl"]["ek"] != null)
			{
				int num = Convert.ToInt32(json["cl"]["ek"].n);
				ClanManager.SetIndexKoefInPercent(Convert.ToInt32(json["cl"]["ek"].n), false);
			}
			UnityEngine.Debug.Log("[LocalUser] " + this.clan.ToString());
		}
		if (json.GetField("dR"))
		{
			List<GLEvent> list = GLEvent.FromArray(json.GetField("dR"));
			foreach (GLEvent glevent in list)
			{
				NotificationWindow.Add(new Notification(Notification.Type.DAILY_BONUS, LanguageManager.GetText("DAILY BONUS"), LanguageManager.GetText("Enter the game each day and your reward will indrease.\\nDon't break the chain to get the maximum money"), LanguageManager.GetText("Done"), new Notification.ButtonClick(glevent.Confirm), null)
				{
					Item = glevent,
					WindowSize = new Vector2(550f, 346f)
				});
			}
		}
		if (this.permission == null)
		{
			this.permission = new Permission();
		}
		JSONObject field3 = json.GetField("conf");
		if (field3 != null)
		{
			if (field3["mdr"] != null)
			{
				this.permission.Update(JSON.Parse(field3["mdr"].input_str));
			}
			if (field3.GetField("cst") != null)
			{
				JSONObject field4 = field3.GetField("cst");
				if (field4.GetField("cn") != null)
				{
					ChangeNameManager.CHANGE_NAME_COST = Convert.ToInt32(field4.GetField("cn").n);
				}
			}
		}
		UnityEngine.Debug.LogError("Permission: " + this.permission);
	}

	private void OnRefreshLite(object result, AjaxRequest request)
	{
		JSONObject jsonobject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
		if (jsonobject.GetField("result").type == JSONObject.Type.BOOL && jsonobject.GetField("result").b && jsonobject.GetField("info") != null)
		{
			this.BaseInfo(jsonobject);
			if (jsonobject.GetField("addItem") != null)
			{
				JSONObject field = jsonobject.GetField("addItem");
				UnityEngine.Debug.LogError("AddItem: " + field.input_str);
				if (field != null && field.type == JSONObject.Type.ARRAY)
				{
					Inventory.Instance.JsonItems2Inventory(field, false);
					GUIInventory.IsInit = false;
				}
			}
			if (jsonobject.GetField("addItemReq") != null)
			{
				string str = jsonobject.GetField("addItemReq").str;
				GameLogicServerNetworkController.SendChange(1, str);
			}
		}
	}

	private void OnRefresh(object result, AjaxRequest request)
	{
		JSONObject jsonobject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
		if (jsonobject.GetField("result").type == JSONObject.Type.BOOL && jsonobject.GetField("result").b && jsonobject.GetField("info") != null)
		{
			this.BaseInfo(jsonobject);
			if (jsonobject.GetField("view") != null)
			{
				LocalUser.View = new LocalPlayerView(jsonobject.GetField("view"));
				LocalUser.View.LocalViewOnDreesUp += this.ViewOnChange;
				LocalUser.View.LocalViewOnUnDress += this.ViewOnChange;
			}
			if (jsonobject.GetField("weap") != null)
			{
				LocalUser.WeaponSlot = new LocalWeaponSlot(jsonobject.GetField("weap"));
				LocalUser.WeaponSlot.OnSet += this.WeaponOnChange;
				LocalUser.WeaponSlot.OnUnSet += this.WeaponOnChange;
				UnityEngine.Debug.Log("LocalUser WeaponSlot: " + LocalUser.WeaponSlot.ToString());
			}
			if (jsonobject.GetField("taun") != null)
			{
				LocalUser.TauntSlot = new LocalTauntSlot(JSON.Parse(jsonobject["taun"].input_str));
				LocalUser.TauntSlot.OnSet += this.TauntOnChange;
				LocalUser.TauntSlot.OnUnSet += this.TauntOnChange;
			}
			if (LocalUser.OnValid != null)
			{
				LocalUser.OnValid(this);
			}
		}
		else
		{
			if (LocalUser.OnError == null)
			{
				throw new AuthException("LocalUser not init: " + result);
			}
			LocalUser.OnError(result);
		}
	}

	private void ViewOnChange(object sender)
	{
		string str = string.Format("&hat={0}&mask={1}&gloves={2}&shirt={3}&pants={4}&boots={5}&backpack={6}&other={7}&head={8}", new object[]
		{
			LocalUser.View.HatID,
			LocalUser.View.MaskID,
			LocalUser.View.GlovesID,
			LocalUser.View.ShirtID,
			LocalUser.View.PantsID,
			LocalUser.View.BootsID,
			LocalUser.View.BackpackID,
			LocalUser.View.OtherID,
			LocalUser.View.HeadID
		});
		Ajax.Request(WebUrls.USER_SET_VIEW_URL + str, new AjaxRequest.AjaxHandler(this.ViewOnChanged));
	}

	private void WeaponOnChange(object sender, bool isChangeIds)
	{
		if (!isChangeIds)
		{
			return;
		}
		string str = string.Format("&i1={0}&i2={1}&i3={2}&i4={3}&i5={4}&i6={5}&i7={6}", new object[]
		{
			LocalUser.WeaponSlot.WeaponID1,
			LocalUser.WeaponSlot.WeaponID2,
			LocalUser.WeaponSlot.WeaponID3,
			LocalUser.WeaponSlot.WeaponID4,
			LocalUser.WeaponSlot.WeaponID5,
			LocalUser.WeaponSlot.WeaponID6,
			LocalUser.WeaponSlot.WeaponID7
		});
		Ajax.Request(WebUrls.USER_SET_WEAPON_URL + str, new AjaxRequest.AjaxHandler(this.ViewOnChanged));
	}

	private void TauntOnChange(object sender, int slot)
	{
		string str = string.Format("&i1={0}&i2={1}&i3={2}", LocalUser.TauntSlot.TauntID0, LocalUser.TauntSlot.TauntID1, LocalUser.TauntSlot.TauntID2);
		Ajax.Request(WebUrls.USER_SET_TAUNT_URL + str, new AjaxRequest.AjaxHandler(this.ViewOnChanged));
	}

	private void ViewOnChanged(object result, AjaxRequest request)
	{
	}

	public static void SubShopCost(ShopCost cost)
	{
		LocalUser.Instance.money -= Convert.ToInt32(cost.TimePVCost);
	}

	public static void SubShopCost(uint money)
	{
		LocalUser.Instance.money -= Convert.ToInt32(money);
	}

	public static void ccDebug()
	{
		UnityEngine.Debug.Log(string.Format("[LocalUser] user_id: {0}", LocalUser.Instance.user_id));
		UnityEngine.Debug.Log(string.Format("[LocalUser] name: {0}", LocalUser.Instance.name));
		UnityEngine.Debug.Log(string.Format("[LocalUser] level: {0}", LocalUser.Instance.level));
		UnityEngine.Debug.Log(string.Format("[LocalUser] exp: {0}", LocalUser.Instance.exp));
		UnityEngine.Debug.Log(string.Format("[LocalUser] min exp: {0}", LocalUser.Instance.minExp));
		UnityEngine.Debug.Log(string.Format("[LocalUser] max exp: {0}", LocalUser.Instance.maxExp));
		UnityEngine.Debug.Log(string.Format("[LocalUser] money: {0}", LocalUser.Instance.money));
	}

	private static LocalUser hInstance;

	private string fullName = string.Empty;

	private int money;

	private int exp;

	private Clan clan;

	private int minExp;

	private int maxExp;

	private bool needLiteRefresh;

	private Permission permission = new Permission();

	public static LocalPlayerView View;

	public static LocalWeaponSlot WeaponSlot;

	public static LocalTauntSlot TauntSlot;

	public delegate void LocalUserEventHandler(object sender);
}
