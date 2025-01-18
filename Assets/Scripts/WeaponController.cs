// dnSpy decompiler from Assembly-CSharp.dll class: WeaponController
using System;
using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	public CombatWeapon CurrentWeapon
	{
		get
		{
			return this.weapon;
		}
	}

	public CombatWeapon[] Weapons
	{
		get
		{
			return this.weapons;
		}
	}

	public virtual void ResetWeapon()
	{
		if (this.weapons == null)
		{
			return;
		}
		this.weapon = this.weapons[0];
	}

	public virtual CombatWeapon GetWeapon(int weaponNum)
	{
		return this.weapons[weaponNum - 1];
	}

	public virtual WeaponType OnChangeWeapon(int weaponNum)
	{
		int previousWeaponNum = 0;
		if (this.weapon != null)
		{
			previousWeaponNum = this.weapon.Index;
		}
		this.weapon = this.weapons[weaponNum - 1];
		if (this.weapon == null)
		{
			return WeaponType.ONE_HANDED_COLD_ARMS;
		}
		this.weapon.Change();
		base.StartCoroutine(this.Change(previousWeaponNum, weaponNum - 1));
		return this.weapon.Type;
	}

	public virtual void InitZombieWeapon(bool show)
	{
		base.StartCoroutine(this.ChangeZombieWeapon(new CombatWeapon(1, WeaponType.ONE_HANDED_COLD_ARMS, 700, 0, 0, 0, 0)
		{
			Distance = 8f,
			Angle = 2.05f,
			SystemName = "OHCA_Zombie"
		}));
	}

	protected virtual IEnumerator ChangeZombieWeapon(CombatWeapon zombieWeapon)
	{
		yield return new WaitForSeconds(0.2f);
		this.weapon = zombieWeapon;
		this.HideWeapons();
		yield break;
	}

	public virtual int InitWeapon(int index, Hashtable weaponData)
	{
		return this.InitWeapon(index, new CombatWeapon(index, weaponData));
	}

	public virtual int InitWeapon(int index, CombatWeapon new_weapon)
	{
		this.weapons[index] = new_weapon;
		foreach (WeaponLook weaponLook in base.transform.GetComponentsInChildren<WeaponLook>(true))
		{
			if (weaponLook.transform.name == this.weapons[index].GetName() || weaponLook.transform.name == this.weapons[index].GetName() + "L" || weaponLook.transform.name == this.weapons[index].GetName() + "R")
			{
				this.weapons[index].Transform = weaponLook.transform;
			}
		}
		if (index == 0)
		{
			this.weapon = this.weapons[index];
			if (this.weapon.Transform != null)
			{
				for (int j = 0; j < this.weapon.Transform.GetChildCount(); j++)
				{
					Transform child = this.weapon.Transform.GetChild(j);
					if (child.gameObject.name == this.weapon.SystemName)
					{
						child.gameObject.SetActive(true);
						this.setWeaponVisible(child, true);
					}
					else
					{
						this.setWeaponVisible(child, false);
						if (NetworkDev.Destroy_Geometry)
						{
							UnityEngine.Object.Destroy(child.gameObject);
						}
					}
				}
			}
		}
		else if (this.weapons[index].Transform != null)
		{
			for (int k = 0; k < this.weapons[index].Transform.GetChildCount(); k++)
			{
				Transform child2 = this.weapons[index].Transform.GetChild(k);
				if (child2.gameObject.name == this.weapons[index].SystemName)
				{
					child2.gameObject.SetActive(true);
					this.setWeaponVisible(child2, false);
				}
				else
				{
					this.setWeaponVisible(child2, false);
					if (NetworkDev.Destroy_Geometry)
					{
						UnityEngine.Object.Destroy(child2.gameObject);
					}
				}
			}
		}
		return this.weapons[index].GetType();
	}

	public virtual void HideWeapons()
	{
		foreach (CombatWeapon combatWeapon in this.weapons)
		{
			if (combatWeapon != null && combatWeapon.Transform != null)
			{
				foreach (Renderer renderer in combatWeapon.Transform.GetComponentsInChildren<Renderer>(true))
				{
					renderer.enabled = false;
				}
			}
		}
	}

	public void HideWeapon()
	{
		Transform transform = this.weapon.Transform;
		foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>(true))
		{
			renderer.enabled = false;
		}
	}

	public void ShowWeapon()
	{
		Transform transform = this.weapon.Transform;
		Renderer[] componentsInChildren = transform.GetComponentsInChildren<Renderer>(true);
		int i = 0;
		while (i < componentsInChildren.Length)
		{
			Renderer renderer = componentsInChildren[i];
			if (this.weapon.Type != WeaponType.ONE_HANDED_COLD_ARMS && this.weapon.Type != WeaponType.TWO_HANDED_COLD_ARMS)
			{
				goto IL_8C;
			}
			if (renderer.gameObject.name == this.weapon.SystemName)
			{
				renderer.enabled = true;
			}
			else if (!renderer.gameObject.name.StartsWith("OHCA"))
			{
				goto IL_8C;
			}
			IL_93:
			i++;
			continue;
			IL_8C:
			renderer.enabled = true;
			goto IL_93;
		}
	}

	private void Awake()
	{
		this.weapons = new CombatWeapon[WeaponController.WeaponNum];
	}

	protected void Reload(CombatWeapon reloadWeapon)
	{
		reloadWeapon.Reload();
	}

	protected IEnumerator Change(int previousWeaponNum, int weaponNum)
	{
		yield return new WaitForSeconds(0.2f);
		Transform weaponTransform = this.weapons[previousWeaponNum].Transform;
		foreach (Renderer g in weaponTransform.GetComponentsInChildren<Renderer>(true))
		{
			g.enabled = false;
		}
		weaponTransform = this.weapons[weaponNum].Transform;
		for (int i = 0; i < weaponTransform.GetChildCount(); i++)
		{
			Transform weaponChild = weaponTransform.GetChild(i);
			if (weaponChild.gameObject.name == this.weapons[weaponNum].SystemName)
			{
				this.setWeaponVisible(weaponChild, true);
			}
			else
			{
				this.setWeaponVisible(weaponChild, false);
				if (NetworkDev.Destroy_Geometry)
				{
					UnityEngine.Object.Destroy(weaponChild.gameObject);
				}
			}
		}
		yield break;
	}

	public void setWeaponVisible(Transform weaponTransform, bool visible)
	{
		foreach (Renderer renderer in weaponTransform.GetComponentsInChildren<Renderer>(true))
		{
			renderer.enabled = visible;
		}
	}

	public CombatWeapon GetWeaponByType(int weaponType)
	{
		foreach (CombatWeapon combatWeapon in this.weapons)
		{
			if (combatWeapon != null && combatWeapon.GetType() == weaponType)
			{
				return combatWeapon;
			}
		}
		if (weaponType == 203)
		{
			return new CombatWeapon(1, 996, WeaponType.KAMIKADZE, "OHCA_Kamikadze");
		}
        return null;
	}

	public void LaunchShot(CombatPlayer player, Shot shot)
	{
		player.ActorAnimator.ShotAnimationReset(shot.WeaponType.GetName());
	}

	public void LaunchTaunt(CombatPlayer player, int tauntID)
	{
		player.ActorAnimator.TauntAnimation(string.Format("Taunt{0}", tauntID));
		player.InitTaunt(true, tauntID);
		this.taunt = true;
		if (!player.IsZombie)
		{
			this.HideWeapon();
		}
        base.StartCoroutine(this.FinishTaunt(player, tauntID));
	}

	private IEnumerator FinishTaunt(CombatPlayer player, int tauntID)
	{
		float seconds = 1.5f;
		if (tauntID == 9)
		{
			seconds = 3.5f;
		}
		else if (tauntID == 8)
		{
			seconds = 8.5f;
		}
		else if (tauntID == 7)
		{
			seconds = 3.5f;
		}
		else if (tauntID > 5)
		{
			seconds = 5.6f;
		}
		else if (tauntID > 1)
		{
			seconds = 2.8f;
		}
		yield return new WaitForSeconds(seconds);
		player.InitTaunt(false, tauntID);
		if (!player.IsDead)
		{
			player.ActorAnimator.FinishTauntAnimation();
			if (!player.IsZombie)
			{
				this.ShowWeapon();
			}
		}
		this.taunt = false;
		yield break;
	}

	public virtual void ResumeWeapon()
	{
		foreach (CombatWeapon combatWeapon in this.weapons)
		{
			Transform transform = combatWeapon.Transform;
			if (combatWeapon != this.weapon)
			{
			}
			for (int j = 0; j < transform.GetChildCount(); j++)
			{
				Transform child = transform.GetChild(j);
				if (child.gameObject.name == this.weapon.SystemName)
				{
					this.setWeaponVisible(child, true);
				}
				else
				{
					this.setWeaponVisible(child, false);
					if (NetworkDev.Destroy_Geometry)
					{
						UnityEngine.Object.Destroy(child.gameObject);
					}
				}
			}
		}
	}

	public static int WeaponNum = 7;

	protected CombatWeapon weapon;

	private bool taunt;

	protected CombatWeapon[] weapons;
}
