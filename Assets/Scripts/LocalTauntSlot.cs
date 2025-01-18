// dnSpy decompiler from Assembly-CSharp.dll class: LocalTauntSlot
using System;
using SimpleJSON;
using UnityEngine;

public class LocalTauntSlot : TauntSlot
{
	public LocalTauntSlot(JSONNode json)
	{
		if (Inventory.Instance.Initialized)
		{
			this.OnLoadUserInventory(Inventory.Instance, EventArgs.Empty);
		}
		Inventory.OnLoad += this.OnLoadUserInventory;
        this.taunt_id0 = Convert.ToInt16(json["i0"].AsInt);
        this.taunt_id1 = Convert.ToInt16(json["i1"].AsInt);
		this.taunt_id2 = Convert.ToInt16(json["i2"].AsInt);
		base.OnSet += this.HandleOnSet;
		base.OnUnSet += this.HandleOnUnSet;
	}

	public short TauntID0
	{
		get
		{
			return this.taunt_id0;
		}
	}

	public short TauntID1
	{
		get
		{
			return this.taunt_id1;
		}
	}

	public short TauntID2
	{
		get
		{
			return this.taunt_id2;
		}
	}

	private void HandleOnSet(object sender, int slot)
	{
		if (sender == null || sender.GetType() != typeof(Taunt))
		{
			UnityEngine.Debug.LogError("[LocalTauntSlot] HandleOnSet sender is not valid: " + sender);
			return;
		}
		Taunt taunt = sender as Taunt;
		if (slot == 0)
		{
			this.taunt_id0 = taunt.TauntID;
		}
		else if (slot == 1)
		{
			this.taunt_id1 = taunt.TauntID;
		}
		else
		{
			this.taunt_id2 = taunt.TauntID;
		}
	}

	private void HandleOnUnSet(object sender, int slot)
	{
		if (sender == null || sender.GetType() != typeof(Taunt))
		{
			UnityEngine.Debug.LogError("[LocalTauntSlot] HandleOnUnSet sender is not valid: " + sender);
			return;
		}
		Taunt taunt = sender as Taunt;
		if (taunt.TauntID == this.taunt_id0)
		{
			this.taunt_id0 = 0;
		}
		else if (taunt.TauntID == this.taunt_id1)
		{
			this.taunt_id1 = 0;
		}
		else if (taunt.TauntID == this.taunt_id2)
		{
			this.taunt_id2 = 0;
		}
	}

	private void OnLoadUserInventory(object sender, EventArgs arg)
	{
		foreach (Taunt taunt in Inventory.Instance.Taunts)
		{
			if (this.taunt_id0 != 0 && taunt.TauntID == this.taunt_id0)
			{
				this.taunt0 = taunt;
			}
			else if (this.taunt_id1 != 0 && taunt.TauntID == this.taunt_id1)
			{
				this.taunt1 = taunt;
			}
			else if (this.taunt_id2 != 0 && taunt.TauntID == this.taunt_id2)
			{
				this.taunt2 = taunt;
			}
		}
		if (this.taunt_id0 != 0 && this.taunt0 == null)
		{
			this.taunt_id0 = 0;
		}
		if (this.taunt_id1 != 0 && this.taunt1 == null)
		{
			this.taunt_id1 = 0;
		}
		if (this.taunt_id2 != 0 && this.taunt2 == null)
		{
			this.taunt_id2 = 0;
		}
	}

	private short taunt_id0;

	private short taunt_id1;

	private short taunt_id2;
}
