// dnSpy decompiler from Assembly-CSharp.dll class: LocalWeaponSlot
using System;
using UnityEngine;

public class LocalWeaponSlot : WeaponSlot
{
	public LocalWeaponSlot(JSONObject json) : base(json)
	{
	}

	public event WeaponSlot.PlayerWeaponSlotEventHandler OnSet;

	public event WeaponSlot.PlayerWeaponSlotEventHandler OnUnSet;

	public void Set(object sender)
	{
		if (sender == null || sender.GetType() != typeof(Weapon))
		{
			UnityEngine.Debug.LogError("[LocalWeaponSlot] Set sender is not valid: " + sender);
			return;
		}
		bool flag = false;
		bool changeIds = false;
		Weapon weapon = sender as Weapon;
		if (weapon.WeaponSlot == 1)
		{
			if (this.w_id1 != weapon.WeaponID)
			{
				changeIds = true;
			}
			this.w_id1 = weapon.WeaponID;
			this.weapon1 = weapon;
			flag = true;
		}
		else if (weapon.WeaponSlot == 2)
		{
			if (this.w_id2 != weapon.WeaponID)
			{
				changeIds = true;
			}
			this.w_id2 = weapon.WeaponID;
			this.weapon2 = weapon;
			flag = true;
		}
		else if (weapon.WeaponSlot == 3)
		{
			if (this.w_id3 != weapon.WeaponID)
			{
				changeIds = true;
			}
			this.w_id3 = weapon.WeaponID;
			this.weapon3 = weapon;
			flag = true;
		}
		else if (weapon.WeaponSlot == 4)
		{
			if (this.w_id4 != weapon.WeaponID)
			{
				changeIds = true;
			}
			this.w_id4 = weapon.WeaponID;
			this.weapon4 = weapon;
			flag = true;
		}
		else if (weapon.WeaponSlot == 5)
		{
			if (this.w_id5 != weapon.WeaponID)
			{
				changeIds = true;
			}
			this.w_id5 = weapon.WeaponID;
			this.weapon5 = weapon;
			flag = true;
		}
		else if (weapon.WeaponSlot == 6)
		{
			if (this.w_id6 != weapon.WeaponID)
			{
				changeIds = true;
			}
			this.w_id6 = weapon.WeaponID;
			this.weapon6 = weapon;
			flag = true;
		}
		else if (weapon.WeaponSlot == 7)
		{
			if (this.w_id7 != weapon.WeaponID)
			{
				changeIds = true;
			}
			this.w_id7 = weapon.WeaponID;
			this.weapon7 = weapon;
			flag = true;
		}
		object[] data = new object[]
		{
			weapon.WeaponID
		};
		GameLogicServerNetworkController.SendChange(2, data);
		if (this.OnSet != null && flag)
		{
			this.OnSet(sender, changeIds);
		}
	}

	public void UnSet(object sender)
	{
		if (sender == null || sender.GetType() != typeof(Weapon))
		{
			UnityEngine.Debug.LogError("[LocalWeaponSlot] UnSet sender is not valid: " + sender);
			return;
		}
		bool flag = false;
		bool changeIds = false;
		Weapon weapon = sender as Weapon;
		if (weapon.WeaponSlot == 1)
		{
			if (this.w_id1 != 0)
			{
				changeIds = true;
			}
			this.w_id1 = 0;
			this.weapon1 = null;
			flag = true;
		}
		else if (weapon.WeaponSlot == 2)
		{
			if (this.w_id2 != 0)
			{
				changeIds = true;
			}
			this.w_id2 = 0;
			this.weapon2 = null;
			flag = true;
		}
		else if (weapon.WeaponSlot == 3)
		{
			if (this.w_id3 != 0)
			{
				changeIds = true;
			}
			this.w_id3 = 0;
			this.weapon3 = null;
			flag = true;
		}
		else if (weapon.WeaponSlot == 4)
		{
			if (this.w_id4 != 0)
			{
				changeIds = true;
			}
			this.w_id4 = 0;
			this.weapon4 = null;
			flag = true;
		}
		else if (weapon.WeaponSlot == 5)
		{
			if (this.w_id5 != 0)
			{
				changeIds = true;
			}
			this.w_id5 = 0;
			this.weapon5 = null;
			flag = true;
		}
		else if (weapon.WeaponSlot == 6)
		{
			if (this.w_id6 != 0)
			{
				changeIds = true;
			}
			this.w_id6 = 0;
			this.weapon6 = null;
			flag = true;
		}
		else if (weapon.WeaponSlot == 7)
		{
			if (this.w_id7 != 0)
			{
				changeIds = true;
			}
			this.w_id7 = 0;
			this.weapon7 = null;
			flag = true;
		}
		if (flag)
		{
			base.SetDefaultWeapons();
		}
		object[] data = new object[]
		{
			null,
			weapon.WeaponSlot
		};
		GameLogicServerNetworkController.SendChange(2, data);
		if (this.OnUnSet != null && flag)
		{
			this.OnUnSet(sender, changeIds);
		}
	}
}
