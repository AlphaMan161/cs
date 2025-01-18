// dnSpy decompiler from Assembly-CSharp.dll class: CombatWeaponSecurity
using System;

public class CombatWeaponSecurity
{
	public CombatWeaponSecurity(CombatWeapon weapon)
	{
		this.Init(weapon);
	}

	public void Init(CombatWeapon weapon)
	{
		if (this.weaponDeviation == null)
		{
			this.weaponDeviation = new SecurityValue((int)(weapon.Deviation * 1000f), 0);
		}
		if (this.weaponRapidityOfFire == null)
		{
			this.weaponRapidityOfFire = new SecurityValue(weapon.ShotTime, 0);
		}
		if (this.weaponSpeedMultiplier == null)
		{
			this.weaponSpeedMultiplier = new SecurityValue((int)weapon.SpeedValue, 0);
		}
	}

	public NotificationType Check(CombatWeapon weapon)
	{
		if (this.weaponDeviation != null && !this.weaponDeviation.Check((int)(weapon.Deviation * 1000f)))
		{
			return NotificationType.DeviationCheating;
		}
		if (this.weaponRapidityOfFire != null && !this.weaponRapidityOfFire.Check(weapon.ShotTime))
		{
			return NotificationType.RapidityCheating;
		}
		if (this.weaponSpeedMultiplier != null && !this.weaponSpeedMultiplier.Check((int)weapon.SpeedValue))
		{
			return NotificationType.SpeedCheating;
		}
		return NotificationType.None;
	}

	private SecurityValue weaponDeviation;

	private SecurityValue weaponRapidityOfFire;

	private SecurityValue weaponSpeedMultiplier;
}
