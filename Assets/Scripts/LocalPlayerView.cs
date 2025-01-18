// dnSpy decompiler from Assembly-CSharp.dll class: LocalPlayerView
using System;
using UnityEngine;

public class LocalPlayerView : PlayerView
{
	public LocalPlayerView(JSONObject json)
	{
		if (Inventory.Instance.Initialized)
		{
			this.OnLoadUserInventory(Inventory.Instance, EventArgs.Empty);
		}
		Inventory.OnLoad += this.OnLoadUserInventory;
		this.hat_id = Convert.ToUInt32(json.GetField("hat").n);
		this.head_id = Convert.ToUInt32(json.GetField("head").n);
		this.mask_id = Convert.ToUInt32(json.GetField("mask").n);
		this.gloves_id = Convert.ToUInt32(json.GetField("gloves").n);
		this.shirt_id = Convert.ToUInt32(json.GetField("shirt").n);
		this.pants_id = Convert.ToUInt32(json.GetField("pants").n);
		this.boots_id = Convert.ToUInt32(json.GetField("boots").n);
		this.backpack_id = Convert.ToUInt32(json.GetField("backpack").n);
		this.other_id = Convert.ToUInt32(json.GetField("other").n);
		base.OnDreesUp += this.HandleOnDreesUp;
		base.OnUnDress += this.HandleOnUnDress;
	}

	public event PlayerView.PlayerViewEventHandler LocalViewOnDreesUp;

	public event PlayerView.PlayerViewEventHandler LocalViewOnUnDress;

	public uint HatID
	{
		get
		{
			return this.hat_id;
		}
	}

	public uint HeadID
	{
		get
		{
			return this.head_id;
		}
	}

	public uint MaskID
	{
		get
		{
			return this.mask_id;
		}
	}

	public uint GlovesID
	{
		get
		{
			return this.gloves_id;
		}
	}

	public uint ShirtID
	{
		get
		{
			return this.shirt_id;
		}
	}

	public uint PantsID
	{
		get
		{
			return this.pants_id;
		}
	}

	public uint BootsID
	{
		get
		{
			return this.boots_id;
		}
	}

	public uint BackpackID
	{
		get
		{
			return this.backpack_id;
		}
	}

	public uint OtherID
	{
		get
		{
			return this.other_id;
		}
	}

	private void HandleOnDreesUp(object sender)
	{
		if (sender == null || sender.GetType() != typeof(Wear))
		{
			UnityEngine.Debug.LogError("[LocalPlayerView] HandleOnDreesUp sender is not valid: " + sender);
			return;
		}
		Wear wear = sender as Wear;
		if (wear.WearType == CCWearType.Hats)
		{
			this.hat_id = wear.WearID;
		}
		else if (wear.WearType == CCWearType.Masks)
		{
			this.mask_id = wear.WearID;
		}
		else if (wear.WearType == CCWearType.Gloves)
		{
			this.gloves_id = wear.WearID;
		}
		else if (wear.WearType == CCWearType.Shirts)
		{
			this.shirt_id = wear.WearID;
		}
		else if (wear.WearType == CCWearType.Pants)
		{
			this.pants_id = wear.WearID;
		}
		else if (wear.WearType == CCWearType.Boots)
		{
			this.boots_id = wear.WearID;
		}
		else if (wear.WearType == CCWearType.Backpacks)
		{
			this.backpack_id = wear.WearID;
		}
		else if (wear.WearType == CCWearType.Heads)
		{
			this.head_id = wear.WearID;
		}
		else if (wear.WearType == CCWearType.Others)
		{
			this.other_id = wear.WearID;
		}
		object[] data = new object[]
		{
			(int)wear.WearID
		};
		GameLogicServerNetworkController.SendChange(3, data);
		if (this.LocalViewOnDreesUp != null)
		{
			this.LocalViewOnDreesUp(sender);
		}
	}

	private void HandleOnUnDress(object sender)
	{
		if (sender == null || sender.GetType() != typeof(Wear))
		{
			UnityEngine.Debug.LogError("[LocalPlayerView] HandleOnUnDress sender is not valid: " + sender);
			return;
		}
		Wear wear = sender as Wear;
		if (wear.WearType == CCWearType.Hats)
		{
			this.hat_id = 0u;
		}
		else if (wear.WearType == CCWearType.Masks)
		{
			this.mask_id = 0u;
		}
		else if (wear.WearType == CCWearType.Gloves)
		{
			this.gloves_id = 0u;
		}
		else if (wear.WearType == CCWearType.Shirts)
		{
			this.shirt_id = 0u;
		}
		else if (wear.WearType == CCWearType.Pants)
		{
			this.pants_id = 0u;
		}
		else if (wear.WearType == CCWearType.Boots)
		{
			this.boots_id = 0u;
		}
		else if (wear.WearType == CCWearType.Backpacks)
		{
			this.backpack_id = 0u;
		}
		else if (wear.WearType == CCWearType.Heads)
		{
			this.head_id = 0u;
		}
		else if (wear.WearType == CCWearType.Others)
		{
			this.other_id = 0u;
		}
		object[] data = new object[]
		{
			null,
			(int)wear.WearType
		};
		GameLogicServerNetworkController.SendChange(3, data);
		if (this.LocalViewOnUnDress != null)
		{
			this.LocalViewOnUnDress(sender);
		}
	}

	private void OnLoadUserInventory(object sender, EventArgs arg)
	{
		foreach (Wear wear in Inventory.Instance.Wears)
		{
			if (this.hat_id != 0u && wear.WearType == CCWearType.Hats && wear.WearID == this.hat_id)
			{
				this.hat = wear;
			}
			else if (this.mask_id != 0u && wear.WearType == CCWearType.Masks && wear.WearID == this.mask_id)
			{
				this.mask = wear;
			}
			else if (this.gloves_id != 0u && wear.WearType == CCWearType.Gloves && wear.WearID == this.gloves_id)
			{
				this.gloves = wear;
			}
			else if (this.shirt_id != 0u && wear.WearType == CCWearType.Shirts && wear.WearID == this.shirt_id)
			{
				this.shirt = wear;
			}
			else if (this.pants_id != 0u && wear.WearType == CCWearType.Pants && wear.WearID == this.pants_id)
			{
				this.pants = wear;
			}
			else if (this.boots_id != 0u && wear.WearType == CCWearType.Boots && wear.WearID == this.boots_id)
			{
				this.boots = wear;
			}
			else if (this.backpack_id != 0u && wear.WearType == CCWearType.Backpacks && wear.WearID == this.backpack_id)
			{
				this.backpack = wear;
			}
			else if (this.head_id != 0u && wear.WearType == CCWearType.Heads && wear.WearID == this.head_id)
			{
				this.head = wear;
			}
			else if (this.other_id != 0u && wear.WearType == CCWearType.Others && wear.WearID == this.other_id)
			{
				this.other = wear;
			}
		}
	}

	private uint hat_id;

	private uint head_id;

	private uint mask_id;

	private uint gloves_id;

	private uint shirt_id;

	private uint pants_id;

	private uint boots_id;

	private uint backpack_id;

	private uint other_id;
}
