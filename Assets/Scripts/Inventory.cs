using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class Inventory
{
	public static event Inventory.InventoryEventHandler OnLoad;

	public static event Inventory.InventoryEventHandler OnError;

	public bool Initialize
	{
		get
		{
			return this.isIniting;
		}
	}

	public bool Initialized
	{
		get
		{
			return this.isInitialized;
		}
	}

	public uint ServerTime
	{
		get
		{
			return this.serverTime;
		}
	}

	public int Count
	{
		get
		{
			return this.itemCount;
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

	public Dictionary<int, Weapon> WeaponDefault
	{
		get
		{
			object obj = this.lockDefaultWeapon;
			Dictionary<int, Weapon> result;
			lock (obj)
			{
				result = this.weaponDefault;
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

	public static Inventory Instance
	{
		get
		{
			if (Inventory.hInstance == null)
			{
				Inventory.hInstance = new Inventory();
				Inventory.hInstance.Init();
			}
			return Inventory.hInstance;
		}
	}

	public void Init()
	{
		if (!this.isIniting)
		{
			this.isIniting = true;
			this.isInitialized = false;
			Ajax.Request(WebUrls.USER_INVENTORY, new AjaxRequest.AjaxHandler(Inventory.Instance.OnLoading));
			ShopManager.OnBuyed += this.ShopOnBuyed;
			DurationManager.OnEnd += this.OnDurationEnd;
		}
	}

	private void OnDurationEnd(object sender)
	{
		if (sender.GetType() == typeof(Weapon))
		{
			Weapon weapon = sender as Weapon;
			Weapon weapon2 = ShopManager.Instance.Weapons.Find((Weapon x) => x.WeaponID == weapon.WeaponID);
			if (weapon2 == null)
			{
				AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.GET_WEAPON_URL + "&id=" + weapon.WeaponID, weapon);
				ajaxRequest.OnComplete += this.OnGetWeaponInfo;
				Ajax.Request(ajaxRequest);
			}
			else
			{
				this.ReplaceWeapon(weapon2);
			}
		}
		else if (sender.GetType() == typeof(Taunt))
		{
			Taunt taunt = sender as Taunt;
			if (LocalUser.TauntSlot.IsSet(taunt))
			{
				LocalUser.TauntSlot.UnSet(taunt);
			}
			Taunt taunt2 = ShopManager.Instance.Taunts.Find((Taunt x) => x.TauntID == taunt.TauntID);
			if (taunt2 != null)
			{
				taunt2.Duration = null;
				taunt2.IsBuyed = false;
			}
			Taunt item = this.Taunts.Find((Taunt x) => x.TauntID == taunt.TauntID);
			object obj = this.lockItems;
			lock (obj)
			{
				object obj2 = this.lockTaunts;
				lock (obj2)
				{
					this.taunts.Remove(item);
					this.items.Remove(item);
				}
			}
		}
		else if (sender.GetType() == typeof(Enhancer))
		{
			Enhancer enhancer = sender as Enhancer;
			Enhancer enhancer2 = ShopManager.Instance.Enhancers.Find((Enhancer x) => x.EnhancerID == enhancer.EnhancerID);
			if (enhancer2 != null)
			{
				enhancer2.Duration = null;
				enhancer2.IsBuyed = false;
			}
			Enhancer item2 = this.Enhancers.Find((Enhancer x) => x.EnhancerID == enhancer.EnhancerID);
			object obj3 = this.lockItems;
			lock (obj3)
			{
				object obj4 = this.lockTaunts;
				lock (obj4)
				{
					this.enhancers.Remove(item2);
					this.items.Remove(item2);
				}
			}
		}
	}

	private void OnGetWeaponInfo(object result, AjaxRequest request)
	{
		JSONObject jsonobject = new JSONObject(result.ToString());
		if (jsonobject.GetField("result") != null && jsonobject.GetField("result").type == JSONObject.Type.BOOL && jsonobject.GetField("result").b)
		{
			Weapon weapon = new Weapon(jsonobject.GetField("item"));
			Weapon weapon2 = ShopManager.Instance.WeaponUpgrades.Find((Weapon x) => x.WeaponID == weapon.WeaponID);
			if (weapon2 != null)
			{
				weapon.Upgrade = weapon2;
			}
			this.ReplaceWeapon(weapon);
			return;
		}
		throw new Exception("[Inventory] OnGetWeaponInfo ERROR. Response: " + result);
	}

	private void ReplaceWeapon(Weapon weapon)
	{
		Weapon weapon2 = Inventory.Instance.Weapons.Find((Weapon x) => x.WeaponID == weapon.WeaponID);
		weapon2.Replace(weapon);
		if (LocalUser.WeaponSlot.IsEquip(weapon))
		{
			LocalUser.WeaponSlot.Set(weapon2);
		}
	}

	public bool IsHave(CCItem ccItem)
	{
		if (ccItem.GetType() == typeof(Wear))
		{
			object obj = this.lockWears;
			lock (obj)
			{
				foreach (Wear wear in this.wears)
				{
					if (wear.WearID == ccItem.ItemID)
					{
						return true;
					}
				}
			}
		}
		else if (ccItem.GetType() == typeof(Weapon))
		{
			object obj2 = this.lockWears;
			lock (obj2)
			{
				foreach (Weapon weapon in this.Weapons)
				{
					if ((long)weapon.WeaponID == (long)((ulong)ccItem.ItemID))
					{
						return true;
					}
				}
			}
		}
		else if (ccItem.GetType() == typeof(Taunt))
		{
			object obj3 = this.lockTaunts;
			lock (obj3)
			{
				foreach (Taunt taunt in this.taunts)
				{
					if ((long)taunt.TauntID == (long)((ulong)ccItem.ItemID))
					{
						return true;
					}
				}
			}
		}
		else if (ccItem.GetType() == typeof(Enhancer))
		{
			object obj4 = this.lockEnhancer;
			lock (obj4)
			{
				foreach (Enhancer enhancer in this.enhancers)
				{
					if (enhancer.EnhancerID == ccItem.ItemID)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private void ShopOnBuyed(object sender)
	{
		UnityEngine.Debug.Log("Shop on buyed: " + sender.ToString());
		this.AddItem(sender as CCItem);
	}

	public void JsonItems2Inventory(JSONObject json_items, bool clearPrevios)
	{
		object obj = this.lockItems;
		lock (obj)
		{
			object obj2 = this.lockWeapons;
			lock (obj2)
			{
				object obj3 = this.lockWears;
				lock (obj3)
				{
					object obj4 = this.lockTaunts;
					lock (obj4)
					{
						object obj5 = this.lockEnhancer;
						lock (obj5)
						{
							if (clearPrevios)
							{
								this.items.Clear();
								this.weapons.Clear();
							}
							for (int i = 0; i < json_items.Count; i++)
							{
								if (json_items[i].type != JSONObject.Type.NULL)
								{
									CCItemType ccitemType;
									if (json_items[i].GetField("itype").type == JSONObject.Type.NUMBER)
									{
										ccitemType = (CCItemType)Convert.ToInt32(json_items[i].GetField("itype").n);
									}
									else
									{
										ccitemType = (CCItemType)Convert.ToInt32(json_items[i].GetField("itype").str);
									}
									switch (ccitemType)
									{
									case CCItemType.WEAPON:
									{
										Weapon weapon = new Weapon(json_items[i]);
										if (!this.IsHave(weapon))
										{
											this.items.Add(weapon);
											this.weapons.Add(weapon);
										}
										break;
									}
									case CCItemType.ENHANCER:
									{
										Enhancer enhancer = new Enhancer(JSON.Parse(json_items[i].input_str));
										if (!this.IsHave(enhancer))
										{
											this.items.Add(enhancer);
											this.enhancers.Add(enhancer);
										}
										else
										{
											Enhancer enhancer2 = this.enhancers.Find((Enhancer x) => x.EnhancerID == enhancer.EnhancerID);
											if (enhancer2 != null && enhancer.Duration != null)
											{
												enhancer2.Duration.TotalSec = enhancer.Duration.TotalSec;
											}
										}
										break;
									}
									case CCItemType.WEAR:
									{
										Wear wear = new Wear(json_items[i]);
										if (!this.IsHave(wear))
										{
											this.items.Add(wear);
											this.wears.Add(wear);
										}
										break;
									}
									case CCItemType.TAUNT:
									{
										Taunt taunt = new Taunt(JSON.Parse(json_items[i].input_str));
										if (taunt.Duration != null)
										{
											bool flag = taunt.Duration.Min <= 1u;
										}
										if (!this.IsHave(taunt))
										{
											this.items.Add(taunt);
											this.taunts.Add(taunt);
										}
										else
										{
											Taunt taunt2 = this.taunts.Find((Taunt x) => x.TauntID == taunt.TauntID);
											if (taunt2 != null && taunt.Duration != null)
											{
												taunt2.Duration.TotalSec = taunt.Duration.TotalSec;
											}
										}
										break;
									}
									default:
										throw new Exception("Unkown inventory type");
									}
									this.itemCount = this.items.Count;
								}
							}
						}
					}
				}
			}
		}
	}

	private void OnLoading(object result, AjaxRequest request)
	{
		this.isIniting = false;
		JSONObject jsonobject = new JSONObject(result.ToString());
		if (jsonobject.GetField("result").type == JSONObject.Type.BOOL && jsonobject.GetField("result").b)
		{
			if (jsonobject.GetField("st") != null)
			{
				this.serverTime = Convert.ToUInt32(jsonobject.GetField("st").n);
			}
			JSONObject field = jsonobject.GetField("data");
			JSONObject jsonobject2 = field.GetField("items");
			if (jsonobject2 != null && jsonobject2.type == JSONObject.Type.ARRAY)
			{
				this.JsonItems2Inventory(jsonobject2, true);
			}
			JSONObject field2 = field.GetField("dw");
			if (field2 != null && field2.type == JSONObject.Type.ARRAY)
			{
				object obj = this.lockDefaultWeapon;
				lock (obj)
				{
					this.weaponDefault.Clear();
					for (int i = 0; i < field2.Count; i++)
					{
						if (field2[i].type != JSONObject.Type.NULL)
						{
							Weapon weapon = new Weapon(field2[i]);
							weapon.IsDefault = true;
							this.weaponDefault.Add(weapon.WeaponSlot, weapon);
						}
					}
				}
			}
			this.isInitialized = true;
			if (Inventory.OnLoad != null)
			{
				Inventory.OnLoad(this, EventArgs.Empty);
			}
		}
		else
		{
			if (Inventory.OnError == null)
			{
				throw new Exception("[Inventory] OnLoading not init: " + result);
			}
			Inventory.OnError(result, EventArgs.Empty);
		}
	}

	public CCItem AddItem(CCItem item)
	{
		CCItem result = null;
		object obj = this.lockItems;
		lock (obj)
		{
			if (item.GetType() == typeof(Wear))
			{
				object obj2 = this.lockWears;
				lock (obj2)
				{
					Wear wear = CCItem.Clone(item) as Wear;
					this.items.Add(wear);
					this.wears.Add(wear);
					result = wear;
				}
			}
			else if (item.GetType() == typeof(Weapon))
			{
				object obj3 = this.lockWeapons;
				lock (obj3)
				{
					Weapon weapon = CCItem.Clone(item) as Weapon;
					this.items.Add(weapon);
					this.weapons.Add(weapon);
					result = weapon;
				}
			}
			else if (item.GetType() == typeof(Taunt))
			{
				object obj4 = this.lockTaunts;
				lock (obj4)
				{
					Taunt taunt = CCItem.Clone(item) as Taunt;
					this.taunts.Add(taunt);
					this.items.Add(taunt);
					result = taunt;
				}
			}
			else if (item.GetType() == typeof(Enhancer))
			{
				object obj5 = this.lockEnhancer;
				lock (obj5)
				{
					Enhancer enhancer = CCItem.Clone(item) as Enhancer;
					this.enhancers.Add(enhancer);
					this.items.Add(enhancer);
					result = enhancer;
				}
			}
			this.itemCount = this.items.Count;
		}
		return result;
	}

	private bool isIniting;

	private bool isInitialized;

	private uint serverTime;

	private int itemCount;

	private object lockItems = new object();

	private List<CCItem> items = new List<CCItem>();

	private object lockWeapons = new object();

	private List<Weapon> weapons = new List<Weapon>();

	private object lockDefaultWeapon = new object();

	private Dictionary<int, Weapon> weaponDefault = new Dictionary<int, Weapon>();

	private object lockWears = new object();

	private List<Wear> wears = new List<Wear>();

	private object lockTaunts = new object();

	private List<Taunt> taunts = new List<Taunt>();

	private object lockEnhancer = new object();

	private List<Enhancer> enhancers = new List<Enhancer>();

	private static Inventory hInstance;

	public delegate void InventoryEventHandler(object sender, EventArgs args);
}
