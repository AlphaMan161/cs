// dnSpy decompiler from Assembly-CSharp.dll class: ShopManager
using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class ShopManager
{
	public static event ShopManager.ShopEventHandler OnLoad;

	public static event ShopManager.ShopEventHandler OnError;

	public static event ShopManager.ShopEventHandler OnBuyed;

	public static event ShopManager.ShopEventHandler OnBuyedError;

	public static ShopManager Instance
	{
		get
		{
			if (ShopManager.hInstance == null)
			{
				ShopManager.hInstance = new ShopManager();
			}
			return ShopManager.hInstance;
		}
	}

	public List<CCItem> Items
	{
		get
		{
			object obj = this.lockItems;
			List<CCItem> result;
			lock (obj)
			{
				result = this.items;
			}
			return result;
		}
	}

	public List<Wear> Wears
	{
		get
		{
			object obj = this.lockWears;
			List<Wear> result;
			lock (obj)
			{
				result = this.wears;
			}
			return result;
		}
	}

	public List<Taunt> Taunts
	{
		get
		{
			object obj = this.lockTaunts;
			List<Taunt> result;
			lock (obj)
			{
				result = this.taunts;
			}
			return result;
		}
	}

	public List<Enhancer> Enhancers
	{
		get
		{
			object obj = this.lockEnhancer;
			List<Enhancer> result;
			lock (obj)
			{
				result = this.enhancers;
			}
			return result;
		}
	}

	public List<Weapon> Weapons
	{
		get
		{
			object obj = this.lockWeapons;
			List<Weapon> result;
			lock (obj)
			{
				result = this.weapons;
			}
			return result;
		}
	}

	public List<Weapon> WeaponUpgrades
	{
		get
		{
			object obj = this.lockWeaponUpgrade;
			List<Weapon> result;
			lock (obj)
			{
				result = this.weaponUpgrade;
			}
			return result;
		}
	}

	public void Init()
	{
		this.loading_cnt = 0;
		this.items.Clear();
		Ajax.Request(string.Format("{0}&weap=1&wear=1&taunt=1&enh=1", WebUrls.SHOP_ITEMS_URL), new AjaxRequest.AjaxHandler(ShopManager.Instance.OnLoadItems));
	}

	private void OnLoadItems(object result, AjaxRequest request)
	{
		JSONNode jsonnode = JSON.Parse(result.ToString());
		JSONObject jsonobject = new JSONObject(result.ToString());
		if (jsonnode["weap"] != null)
		{
			this.OnLoadingWeapons(jsonobject["weap"]);
		}
		if (jsonnode["wear"] != null)
		{
			this.OnLoadingWears(jsonobject["wear"]);
		}
		if (jsonnode["taunt"] != null)
		{
			this.OnLoadingTaunt(jsonnode["taunt"]);
		}
		if (jsonnode["enh"] != null)
		{
			this.OnLoadingEnhancer(jsonnode["enh"]);
		}
	}

	private void AddCompleteLoadingCounter()
	{
		this.loading_cnt += 1;
		if (this.loading_cnt >= 2 && ShopManager.OnLoad != null)
		{
			ShopManager.OnLoad(this);
		}
	}

	private void OnLoadingTaunt(JSONNode json)
	{
		if (json["items"] != null)
		{
			object obj = this.lockTaunts;
			lock (obj)
			{
				foreach (JSONNode json2 in json["items"].Childs)
				{
					Taunt taunt = new Taunt(json2);
					if (Inventory.Instance.IsHave(taunt))
					{
						taunt.IsBuyed = true;
						Taunt taunt2 = Inventory.Instance.Taunts.Find((Taunt x) => x.TauntID == taunt.TauntID);
						if (taunt2 != null)
						{
							taunt.Duration = taunt2.Duration;
						}
					}
					this.taunts.Add(taunt);
				}
			}
		}
	}

	private void OnLoadingEnhancer(JSONNode json)
	{
		if (json["items"] != null)
		{
			object obj = this.lockEnhancer;
			lock (obj)
			{
				foreach (JSONNode json2 in json["items"].Childs)
				{
					Enhancer enhancer = new Enhancer(json2);
					if (Inventory.Instance.IsHave(enhancer))
					{
						enhancer.IsBuyed = true;
						Enhancer enhancer2 = Inventory.Instance.Enhancers.Find((Enhancer x) => x.EnhancerID == enhancer.EnhancerID);
						if (enhancer2 != null)
						{
							enhancer.Duration = enhancer2.Duration;
						}
					}
					if (enhancer.IsClan)
					{
						ClanShopManager.AddClanEnhancer(enhancer);
					}
					else
					{
						this.enhancers.Add(enhancer);
					}
				}
			}
		}
	}

	private void OnLoadingWeapons(JSONObject json)
	{
		JSONObject field = json.GetField("upg");
		if (field != null && field.type == JSONObject.Type.ARRAY)
		{
			object obj = this.lockItems;
			lock (obj)
			{
				object obj2 = this.lockWeapons;
				lock (obj2)
				{
					for (int i = 0; i < field.Count; i++)
					{
						if (field[i].type != JSONObject.Type.NULL)
						{
							Weapon weapon = new Weapon(field[i]);
							if (Inventory.Instance.IsHave(weapon))
							{
								weapon.IsBuyed = true;
								Weapon weapon3 = Inventory.Instance.Weapons.Find((Weapon x) => x.WeaponID == weapon.WeaponID);
								weapon3.Upgrade = weapon;
							}
							this.weaponUpgrade.Add(weapon);
						}
					}
				}
			}
		}
		field = json.GetField("items");
		if (field != null && field.type == JSONObject.Type.ARRAY)
		{
			object obj3 = this.lockItems;
			lock (obj3)
			{
				object obj4 = this.lockWeapons;
				lock (obj4)
				{
					for (int j = 0; j < field.Count; j++)
					{
						if (field[j].type != JSONObject.Type.NULL)
						{
							Weapon weapon = new Weapon(field[j]);
							if (Inventory.Instance.IsHave(weapon))
							{
								weapon.IsBuyed = true;
							}
							Weapon weapon2 = this.weaponUpgrade.Find((Weapon x) => x.WeaponID == weapon.WeaponID);
							if (weapon2 != null)
							{
								weapon.Upgrade = weapon2;
							}
							this.Weapons.Add(weapon);
							this.Items.Add(weapon);
						}
					}
				}
			}
		}
		this.AddCompleteLoadingCounter();
	}

	private void OnLoadingWeapons(object result, AjaxRequest request)
	{
		JSONObject jsonobject = new JSONObject(result.ToString());
		if (jsonobject.GetField("result").type == JSONObject.Type.BOOL && jsonobject.GetField("result").b)
		{
			this.OnLoadingWeapons(jsonobject);
		}
		else
		{
			if (ShopManager.OnError == null)
			{
				throw new Exception("[Inventory] OnLoadingWeapons not init: " + result);
			}
			ShopManager.OnError(result);
		}
	}

	private void OnLoadingWears(JSONObject json)
	{
		JSONObject field = json.GetField("items");
		if (field != null && field.type == JSONObject.Type.ARRAY)
		{
			object obj = this.lockItems;
			lock (obj)
			{
				object obj2 = this.lockWears;
				lock (obj2)
				{
					for (int i = 0; i < field.Count; i++)
					{
						if (field[i].type != JSONObject.Type.NULL)
						{
							Wear wear = new Wear(field[i]);
							if (Inventory.Instance.IsHave(wear))
							{
								wear.IsBuyed = true;
							}
							this.Wears.Add(wear);
							this.Items.Add(wear);
						}
					}
					this.FilteringWear();
				}
			}
		}
		this.AddCompleteLoadingCounter();
	}

	private void OnLoadingWears(object result, AjaxRequest request)
	{
		JSONObject jsonobject = new JSONObject(result.ToString());
		if (jsonobject.GetField("result").type == JSONObject.Type.BOOL && jsonobject.GetField("result").b)
		{
			this.OnLoadingWears(jsonobject);
		}
		else
		{
			if (ShopManager.OnError == null)
			{
				throw new Exception("[Inventory] OnLoadingWears not init: " + result);
			}
			ShopManager.OnError(result);
		}
	}

	public List<Wear> WearHats
	{
		get
		{
			return this.wearHats;
		}
	}

	public List<Wear> WearMasks
	{
		get
		{
			return this.wearMasks;
		}
	}

	public List<Wear> WearGloves
	{
		get
		{
			return this.wearGloves;
		}
	}

	public List<Wear> WearShirts
	{
		get
		{
			return this.wearShirts;
		}
	}

	public List<Wear> WearPants
	{
		get
		{
			return this.wearPants;
		}
	}

	public List<Wear> WearBoots
	{
		get
		{
			return this.wearBoots;
		}
	}

	public List<Wear> WearBackPacks
	{
		get
		{
			return this.wearBackPacks;
		}
	}

	public List<Wear> WearOthers
	{
		get
		{
			return this.wearOthers;
		}
	}

	public List<Wear> WearHeads
	{
		get
		{
			return this.wearHeads;
		}
	}

	private void FilteringWear()
	{
		this.wearHats = this.Wears.FindAll((Wear x) => x.WearType == CCWearType.Hats);
		this.wearMasks = this.Wears.FindAll((Wear x) => x.WearType == CCWearType.Masks);
		this.wearGloves = this.Wears.FindAll((Wear x) => x.WearType == CCWearType.Gloves);
		this.wearShirts = this.Wears.FindAll((Wear x) => x.WearType == CCWearType.Shirts);
		this.wearPants = this.Wears.FindAll((Wear x) => x.WearType == CCWearType.Pants);
		this.wearBoots = this.Wears.FindAll((Wear x) => x.WearType == CCWearType.Boots);
		this.wearBackPacks = this.Wears.FindAll((Wear x) => x.WearType == CCWearType.Backpacks);
		this.wearOthers = this.Wears.FindAll((Wear x) => x.WearType == CCWearType.Others);
		this.wearHeads = this.Wears.FindAll((Wear x) => x.WearType == CCWearType.Heads);
	}

	public void BuyWear(Wear wear)
	{
		if (Inventory.Instance.IsHave(wear))
		{
			UnityEngine.Debug.LogError("[ShopManager] isHaveWear");
		}
		else
		{
			AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.BUY_WEAR_URL + "&id=" + wear.WearID, wear);
			ajaxRequest.OnComplete += this.OnBuyWear;
			Ajax.Request(ajaxRequest);
		}
	}

	private void OnBuyWear(object result, AjaxRequest request)
	{
		JSONObject jsonobject = new JSONObject(result.ToString());
		if (jsonobject.GetField("result") != null && jsonobject.GetField("result").type == JSONObject.Type.BOOL && jsonobject.GetField("result").b)
		{
			string data = null;
			if (jsonobject.GetField("req") != null)
			{
				data = jsonobject.GetField("req").str;
			}
			GameLogicServerNetworkController.SendChange(1, data);
			LocalUser.SubShopCost((request.Tag as CCItem).Shop_Cost);
			(request.Tag as CCItem).IsBuyed = true;
			if (ShopManager.OnBuyed != null)
			{
				ShopManager.OnBuyed(request.Tag);
			}
			LocalUser.View.DressUp(request.Tag as Wear);
			Wear wear = request.Tag as Wear;
			WebCall.Analitic("BuyWear", string.Format("{0}_{1}_{2}", wear.WearType.ToString(), wear.SystemName, wear.WearID), new object[0]);
		}
		else
		{
			if (ShopManager.OnBuyedError == null)
			{
				throw new Exception("[ShopManager]OnBuyWear ERROR. Response: " + result);
			}
			ShopManager.OnBuyedError(request.Tag);
		}
	}

	public void BuyWeapon(Weapon weapon)
	{
		if (Inventory.Instance.IsHave(weapon))
		{
			UnityEngine.Debug.LogError("[ShopManager] isHaveWeapon");
		}
		else
		{
			AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.BUY_WEAPON_URL + "&id=" + weapon.WeaponID, weapon);
			ajaxRequest.OnComplete += this.OnBuyWeapon;
			Ajax.Request(ajaxRequest);
		}
	}

	private void OnBuyWeapon(object result, AjaxRequest request)
	{
		JSONObject jsonobject = new JSONObject(result.ToString());
		if (jsonobject.GetField("result") != null && jsonobject.GetField("result").type == JSONObject.Type.BOOL && jsonobject.GetField("result").b)
		{
			string data = null;
			if (jsonobject.GetField("req") != null)
			{
				data = jsonobject.GetField("req").str;
			}
			GameLogicServerNetworkController.SendChange(1, data);
			LocalUser.SubShopCost((request.Tag as CCItem).Shop_Cost);
			(request.Tag as CCItem).IsBuyed = true;
			if (ShopManager.OnBuyed != null)
			{
				ShopManager.OnBuyed(request.Tag);
			}
			LocalUser.WeaponSlot.Set(request.Tag);
			Weapon weapon = request.Tag as Weapon;
			WebCall.Analitic("BuyWeapon", string.Format("{0}_{1}_{2}", weapon.WeaponSlot.ToString(), weapon.SystemName, weapon.WeaponID), new object[0]);
		}
		else
		{
			if (ShopManager.OnBuyedError == null)
			{
				throw new Exception("[ShopManager]OnBuyWear ERROR. Response: " + result);
			}
			ShopManager.OnBuyedError(request.Tag);
		}
	}

	public void BuyEnhancer(Enhancer enhancer)
	{
		AjaxRequest ajaxRequest = new AjaxRequest(string.Concat(new object[]
		{
			WebUrls.BUY_ENHANCER_URL,
			"&id=",
			enhancer.EnhancerID,
			"&dur=",
			(int)enhancer.Shop_Cost.SelectedDuration
		}), enhancer);
		ajaxRequest.OnComplete += this.OnBuyEnhancer;
		Ajax.Request(ajaxRequest);
	}

	private void OnBuyEnhancer(object result, AjaxRequest request)
	{
		JSONNode jsonnode = JSON.Parse(result.ToString());
		if (jsonnode["result"] != null && jsonnode["result"].AsBool)
		{
			string data = null;
			if (jsonnode["req"] != null)
			{
				data = jsonnode["req"].Value;
			}
			GameLogicServerNetworkController.SendChange(1, data);
			Enhancer enhancer = request.Tag as Enhancer;
			LocalUser.SubShopCost(enhancer.Shop_Cost.SelectedVCost);
			enhancer.IsBuyed = true;
			int num = 86460;
			if (enhancer.Shop_Cost.SelectedDuration == DurationType.WEEK)
			{
				num *= 7;
			}
			else if (enhancer.Shop_Cost.SelectedDuration == DurationType.MONTH)
			{
				num *= 30;
			}
			else if (enhancer.Shop_Cost.SelectedDuration == DurationType.PERMANENT)
			{
				num = 0;
			}
			if (num != 0)
			{
				if (enhancer.Duration == null)
				{
					enhancer.Duration = new Duration((long)num);
				}
				else
				{
					enhancer.Duration = new Duration(enhancer.Duration.TotalSec + (long)num);
				}
			}
			if (ShopManager.OnBuyed != null)
			{
				ShopManager.OnBuyed(enhancer);
			}
			WebCall.Analitic("BuyEnhancer", string.Format("{0}_{1}", enhancer.EnhancerID, enhancer.Shop_Cost.SelectedDuration), new object[0]);
		}
		else
		{
			if (ShopManager.OnBuyedError == null)
			{
				throw new Exception("[ShopManager]OnBuyTaunt ERROR. Response: " + result);
			}
			ShopManager.OnBuyedError(request.Tag);
		}
	}

	public void BuyTaunt(Taunt taunt)
	{
		AjaxRequest ajaxRequest = new AjaxRequest(string.Concat(new object[]
		{
			WebUrls.BUY_TAUNT_URL,
			"&id=",
			taunt.TauntID,
			"&dur=",
			(int)taunt.Shop_Cost.SelectedDuration
		}), taunt);
		ajaxRequest.OnComplete += this.OnBuyTaunt;
		Ajax.Request(ajaxRequest);
	}

	private void OnBuyTaunt(object result, AjaxRequest request)
	{
		JSONNode jsonnode = JSON.Parse(result.ToString());
		if (jsonnode["result"] != null && jsonnode["result"].AsBool)
		{
			string data = null;
			if (jsonnode["req"] != null)
			{
				data = jsonnode["req"].Value;
			}
			GameLogicServerNetworkController.SendChange(1, data);
			Taunt taunt = request.Tag as Taunt;
			LocalUser.SubShopCost(taunt.Shop_Cost.SelectedVCost);
			taunt.IsBuyed = true;
			int num = 86460;
			if (taunt.Shop_Cost.SelectedDuration == DurationType.WEEK)
			{
				num *= 7;
			}
			else if (taunt.Shop_Cost.SelectedDuration == DurationType.MONTH)
			{
				num *= 30;
			}
			else if (taunt.Shop_Cost.SelectedDuration == DurationType.PERMANENT)
			{
				num = 0;
			}
			if (num != 0)
			{
				if (taunt.Duration == null)
				{
					taunt.Duration = new Duration((long)num);
				}
				else
				{
					taunt.Duration = new Duration(taunt.Duration.TotalSec + (long)num);
				}
			}
			if (ShopManager.OnBuyed != null)
			{
				ShopManager.OnBuyed(taunt);
			}
			WebCall.Analitic("BuyTaunt", string.Format("{0}_{1}", taunt.TauntID, taunt.Shop_Cost.SelectedDuration), new object[0]);
		}
		else
		{
			if (ShopManager.OnBuyedError == null)
			{
				throw new Exception("[ShopManager]OnBuyTaunt ERROR. Response: " + result);
			}
			ShopManager.OnBuyedError(request.Tag);
		}
	}

	public void BuyWeaponUpgrade(Weapon weapon)
	{
		if (weapon.Upgrade != null)
		{
			if (weapon.Duration != null)
			{
				weapon.Upgrade.Duration = weapon.Duration;
			}
			weapon = weapon.Upgrade;
		}
		AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.BUY_WEAPON_UPGRADE_URL + "&id=" + weapon.UpgradeID, weapon);
		ajaxRequest.OnComplete += this.OnBuyWeaponUpgrade;
		Ajax.Request(ajaxRequest);
	}

	private void OnBuyWeaponUpgrade(object result, AjaxRequest request)
	{
		JSONObject jsonobject = new JSONObject(result.ToString());
		if (jsonobject.GetField("result") != null && jsonobject.GetField("result").type == JSONObject.Type.BOOL && jsonobject.GetField("result").b)
		{
			string data = null;
			if (jsonobject.GetField("req") != null)
			{
				data = jsonobject.GetField("req").str;
			}
			GameLogicServerNetworkController.SendChange(1, data);
			Weapon weapon = request.Tag as Weapon;
			if (weapon.Duration == null)
			{
				weapon.Duration = new Duration(86460L);
			}
			else
			{
				weapon.Duration = new Duration(weapon.Duration.TotalSec + 86460L);
			}
			if (weapon.Shop_Cost != null)
			{
				LocalUser.SubShopCost(weapon.Shop_Cost.Time1VCost);
			}
			Weapon weapon2 = Inventory.Instance.Weapons.Find((Weapon x) => x.WeaponID == weapon.WeaponID);
			weapon2.Replace(weapon);
			if (LocalUser.WeaponSlot.IsEquip(weapon2))
			{
				LocalUser.WeaponSlot.Set(weapon2);
			}
			WebCall.Analitic("UpgradeWeapon", string.Format("{0}_{1}_{2}", weapon.WeaponSlot.ToString(), weapon.SystemName, weapon.WeaponID), new object[0]);
			return;
		}
		if (ShopManager.OnBuyedError != null)
		{
			ShopManager.OnBuyedError(request.Tag);
			return;
		}
		throw new Exception("[ShopManager]OnBuyWear ERROR. Response: " + result);
	}

	public void UnloadResource()
	{
		foreach (Enhancer enhancer in this.Enhancers)
		{
			enhancer.UnloadIco();
		}
		foreach (Wear wear in this.Wears)
		{
			wear.UnloadIco();
		}
		foreach (Weapon weapon in this.Weapons)
		{
			weapon.UnloadIco();
		}
		foreach (Taunt taunt in this.Taunts)
		{
			taunt.UnloadIco();
		}
	}

	private static ShopManager hInstance;

	private object lockItems = new object();

	private List<CCItem> items = new List<CCItem>();

	private object lockWears = new object();

	private List<Wear> wears = new List<Wear>();

	private object lockTaunts = new object();

	private List<Taunt> taunts = new List<Taunt>();

	private object lockEnhancer = new object();

	private List<Enhancer> enhancers = new List<Enhancer>();

	private object lockWeapons = new object();

	private List<Weapon> weapons = new List<Weapon>();

	private object lockWeaponUpgrade = new object();

	private List<Weapon> weaponUpgrade = new List<Weapon>();

	private short loading_cnt;

	private List<Wear> wearHats = new List<Wear>();

	private List<Wear> wearMasks = new List<Wear>();

	private List<Wear> wearGloves = new List<Wear>();

	private List<Wear> wearShirts = new List<Wear>();

	private List<Wear> wearPants = new List<Wear>();

	private List<Wear> wearBoots = new List<Wear>();

	private List<Wear> wearBackPacks = new List<Wear>();

	private List<Wear> wearOthers = new List<Wear>();

	private List<Wear> wearHeads = new List<Wear>();

	public delegate void ShopEventHandler(object sender);
}
