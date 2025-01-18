// dnSpy decompiler from Assembly-CSharp.dll class: CombatPlayerSecurity
using System;
using UnityEngine;

public class CombatPlayerSecurity
{
	public CombatPlayerSecurity(CombatPlayer player)
	{
		this.Init(player);
	}

	public bool CheckNicknameCheating(bool nicknameCorrect)
	{
		if (nicknameCorrect)
		{
			this.nicknameCounter = 0;
			return true;
		}
		this.nicknameCounter++;
		return this.nicknameCounter <= 10;
	}

	private void Init(CombatPlayer player)
	{
		if (player.IsDead)
		{
			this.runSpeed = new SecurityValue((int)player.SoldierController.runSpeed, 0);
			this.walkSpeed = new SecurityValue((int)player.SoldierController.walkSpeed, 0);
			this.crouchSpeed = new SecurityValue((int)player.SoldierController.crouchWalkSpeed, 0);
			this.motorGravity = new SecurityValue((int)player.CharacterMotor.movement.gravity, 0);
			this.jumpMax = new SecurityValue((int)player.CharacterMotor.jumping.baseHeight, 0);
		}
	}

	public void InitWeapon(CombatPlayer player)
	{
		this.weaponSecurity = new CombatWeaponSecurity[7];
		foreach (CombatWeapon combatWeapon in player.ShotController.Weapons)
		{
			this.weaponSecurity[combatWeapon.Index] = new CombatWeaponSecurity(combatWeapon);
		}
	}

	public int Check(CombatPlayer player)
	{
		if (this.runSpeed == null || this.walkSpeed == null || this.crouchSpeed == null || !this.runSpeed.Check((int)player.SoldierController.runSpeed) || !this.walkSpeed.Check((int)player.SoldierController.walkSpeed) || !this.crouchSpeed.Check((int)player.SoldierController.crouchWalkSpeed))
		{
			return 5;
		}
		if (this.motorGravity == null || !this.motorGravity.Check((int)player.CharacterMotor.movement.gravity))
		{
			return 6;
		}
		if (this.jumpMax == null || (!this.jumpMax.Check((int)player.CharacterMotor.jumping.baseHeight) && !player.IsZombie))
		{
			return 35;
		}
		if (this.weaponSecurity != null)
		{
			foreach (CombatWeapon combatWeapon in player.ShotController.Weapons)
			{
				if (!player.IsZombie)
				{
					NotificationType notificationType = this.weaponSecurity[combatWeapon.Index].Check(combatWeapon);
					if (notificationType != NotificationType.None)
					{
						return (int)notificationType;
					}
				}
			}
		}
		if (!this.CheckFPSCamera(player))
		{
			return 29;
		}
		return 0;
	}

	private bool CheckFPSCamera(CombatPlayer player)
	{
		if (player.IsDead)
		{
			return true;
		}
		FPSCamera component = player.SoldierController.FPSCamera.GetComponent<FPSCamera>();
		if (Mathf.Abs(component.transform.localPosition.y) < 2f)
		{
			this.cameraUnderfloorCounter++;
			if (this.cameraUnderfloorCounter > 300)
			{
				return false;
			}
		}
		else
		{
			this.cameraUnderfloorCounter = 0;
		}
		return true;
	}

	private void InitColliders()
	{
		this.triggers = (UnityEngine.Object.FindSceneObjectsOfType(typeof(BaseEnterTrigger)) as BaseEnterTrigger[]);
	}

	public void CheckColliders()
	{
		if (this.triggers == null)
		{
			return;
		}
		foreach (BaseEnterTrigger baseEnterTrigger in this.triggers)
		{
			if (baseEnterTrigger.Team == 3)
			{
				if (baseEnterTrigger == null)
				{
					UnityEngine.Debug.LogError("Trigger Destroyed");
					return;
				}
				if (!baseEnterTrigger.enabled)
				{
					UnityEngine.Debug.LogError("Trigger Disabled");
					return;
				}
				if (!baseEnterTrigger.gameObject.activeSelf)
				{
					UnityEngine.Debug.LogError("Trigger GameObject Disabled");
					return;
				}
				if (!baseEnterTrigger.GetComponent<Collider>().enabled)
				{
					UnityEngine.Debug.LogError("Trigger Collider Disabled");
					return;
				}
				if (!baseEnterTrigger.GetComponent<Collider>().enabled)
				{
					UnityEngine.Debug.LogError("Collider Disabled");
					return;
				}
			}
		}
	}

	private SecurityValue runSpeed;

	private SecurityValue walkSpeed;

	private SecurityValue crouchSpeed;

	private SecurityValue motorGravity;

	private SecurityValue jumpMax;

	private SecurityValue doorOpened;

	private CombatWeaponSecurity[] weaponSecurity;

	private BaseEnterTrigger[] triggers;

	private int nicknameCounter;

	private int cameraUnderfloorCounter;
}
