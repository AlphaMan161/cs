// dnSpy decompiler from Assembly-CSharp.dll class: ShotCalculator
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShotCalculator
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map0;
	public static Vector3 DownScan(Vector3 pos)
	{
		int num = 1792;
		Ray ray = new Ray(pos, Vector3.down);
		num = ~num;
		RaycastHit raycastHit;
		Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, num);
		return raycastHit.point;
	}

	public static int StraightScan(Ray ray, bool isBot)
	{
		int num = 772;
		if (isBot)
		{
			num = 516;
		}
		num = ~num;
		RaycastHit raycastHit;
		Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, num);
		int result = -1;
		string name = raycastHit.transform.name;
		if (name != null)
		{
			if (ShotCalculator._003C_003Ef__switch_0024map0 == null)
			{
				ShotCalculator._003C_003Ef__switch_0024map0 = new Dictionary<string, int>(14)
				{
					{
						"ShipModel",
						0
					},
					{
						"ShipBase",
						0
					},
					{
						"ShipEngine",
						0
					},
					{
						"ShipCabin",
						0
					},
					{
						"Bip01 Spine2",
						0
					},
					{
						"Bip01 Head",
						0
					},
					{
						"Bip01 R Calf",
						0
					},
					{
						"Bip01 L Calf",
						0
					},
					{
						"Bip01 R Thigh",
						0
					},
					{
						"Bip01 L Thigh",
						0
					},
					{
						"Bip01 R UpperArm",
						0
					},
					{
						"Bip01 L UpperArm",
						0
					},
					{
						"Bip01 R Forearm",
						0
					},
					{
						"Bip01 L Forearm",
						0
					}
				};
			}
			int num2;
			if (ShotCalculator._003C_003Ef__switch_0024map0.TryGetValue(name, out num2))
			{
				if (num2 == 0)
				{
					CombatPlayer component = raycastHit.transform.parent.parent.GetComponent<CombatPlayer>();
					if (component != null)
					{
						result = component.playerID;
					}
					else
					{
						component = raycastHit.transform.parent.GetComponent<CombatPlayer>();
						if (component != null)
						{
							result = component.playerID;
						}
						else
						{
							Item component2 = raycastHit.transform.GetComponent<Item>();
							if (component2 != null && component2.Root != null)
							{
								component = component2.Root.GetComponent<CombatPlayer>();
								if (component != null)
								{
									result = component.playerID;
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	private static bool CheckLineBox(Vector3 B1, Vector3 B2, Vector3 L1, Vector3 L2, ref Vector3 Hit)
	{
		if (L2.x < B1.x && L1.x < B1.x)
		{
			return false;
		}
		if (L2.x > B2.x && L1.x > B2.x)
		{
			return false;
		}
		if (L2.y < B1.y && L1.y < B1.y)
		{
			return false;
		}
		if (L2.y > B2.y && L1.y > B2.y)
		{
			return false;
		}
		if (L2.z < B1.z && L1.z < B1.z)
		{
			return false;
		}
		if (L2.z > B2.z && L1.z > B2.z)
		{
			return false;
		}
		if (L1.x > B1.x && L1.x < B2.x && L1.y > B1.y && L1.y < B2.y && L1.z > B1.z && L1.z < B2.z)
		{
			Hit = L1;
			return true;
		}
		return (ShotCalculator.GetIntersection(L1.x - B1.x, L2.x - B1.x, L1, L2, ref Hit) && ShotCalculator.InBox(Hit, B1, B2, 1)) || (ShotCalculator.GetIntersection(L1.y - B1.y, L2.y - B1.y, L1, L2, ref Hit) && ShotCalculator.InBox(Hit, B1, B2, 2)) || (ShotCalculator.GetIntersection(L1.z - B1.z, L2.z - B1.z, L1, L2, ref Hit) && ShotCalculator.InBox(Hit, B1, B2, 3)) || (ShotCalculator.GetIntersection(L1.x - B2.x, L2.x - B2.x, L1, L2, ref Hit) && ShotCalculator.InBox(Hit, B1, B2, 1)) || (ShotCalculator.GetIntersection(L1.y - B2.y, L2.y - B2.y, L1, L2, ref Hit) && ShotCalculator.InBox(Hit, B1, B2, 2)) || (ShotCalculator.GetIntersection(L1.z - B2.z, L2.z - B2.z, L1, L2, ref Hit) && ShotCalculator.InBox(Hit, B1, B2, 3));
	}

	private static bool GetIntersection(float fDst1, float fDst2, Vector3 P1, Vector3 P2, ref Vector3 Hit)
	{
		if (fDst1 * fDst2 >= 0f)
		{
			return false;
		}
		if (fDst1 == fDst2)
		{
			return false;
		}
		Hit = P1 + (P2 - P1) * (-fDst1 / (fDst2 - fDst1));
		return true;
	}

	private static bool InBox(Vector3 Hit, Vector3 B1, Vector3 B2, int Axis)
	{
		return (Axis == 1 && Hit.z > B1.z && Hit.z < B2.z && Hit.y > B1.y && Hit.y < B2.y) || (Axis == 2 && Hit.z > B1.z && Hit.z < B2.z && Hit.x > B1.x && Hit.x < B2.x) || (Axis == 3 && Hit.x > B1.x && Hit.x < B2.x && Hit.y > B1.y && Hit.y < B2.y);
	}

	public static bool CheckStraightShot(Vector3 originPoint, Vector3 hitPoint, ref Vector3 hit)
	{
		BaseEnterTrigger[] array = UnityEngine.Object.FindObjectsOfType<BaseEnterTrigger>();
		foreach (BaseEnterTrigger baseEnterTrigger in array)
		{
			if (!(baseEnterTrigger.name != "Smertipodobno"))
			{
				if (!(baseEnterTrigger.transform.parent != null) || !(baseEnterTrigger.transform.parent.gameObject.name == "SmertiPodUgol"))
				{
					Bounds bounds = baseEnterTrigger.transform.GetComponent<Collider>().bounds;
					if (ShotCalculator.CheckLineBox(bounds.min, bounds.max, originPoint, hitPoint, ref hit))
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public static Shot StraightShot(Ray cray, Transform TWeapon, int weaponType, int maxDistance, bool isBot)
	{
		Shot shot = null;
		ShotTargetType targetType = ShotTargetType.NONE;
		Transform targetTransform = null;
		PlayerHitZone hitZone = PlayerHitZone.BASE;
		int num = 772;
		if (isBot)
		{
			num = 516;
		}
		num = ~num;
		Ray ray = cray;
		RaycastHit raycastHit;
		Physics.SphereCast(ray, 0.2f, out raycastHit, float.PositiveInfinity, num);
		int num2 = -1;
		long itemTimeStamp = -1L;
		string name = raycastHit.transform.name;
		switch (name)
		{
		case "ShipModel":
		case "ShipBase":
		case "ShipEngine":
		case "ShipCabin":
		case "Bip01":
		case "Bip01 Spine2":
		case "Bip01 Head":
		case "Bip01 R Calf":
		case "Bip01 L Calf":
		case "Bip01 R Thigh":
		case "Bip01 L Thigh":
		case "Bip01 R UpperArm":
		case "Bip01 L UpperArm":
		case "Bip01 R Forearm":
		case "Bip01 L Forearm":
		{
			CombatPlayer component = raycastHit.transform.parent.parent.GetComponent<CombatPlayer>();
			if (component != null)
			{
				num2 = component.playerID;
				targetTransform = component.transform;
			}
			else
			{
				component = raycastHit.transform.parent.GetComponent<CombatPlayer>();
				if (component != null)
				{
					num2 = component.playerID;
					targetTransform = component.transform;
				}
				else
				{
					Item component2 = raycastHit.transform.GetComponent<Item>();
					if (component2 != null && component2.Root != null)
					{
						component = component2.Root.GetComponent<CombatPlayer>();
						if (component != null)
						{
							num2 = component.playerID;
							targetTransform = component.transform;
						}
					}
				}
			}
			if (component != null && component != PlayerManager.Instance.LocalPlayer && component.CanCheckNickName(PlayerManager.Instance.LocalPlayer))
			{
				bool nicknameCorrect = component.CheckNickname(PlayerManager.Instance.LocalPlayer);
				if (!PlayerManager.Instance.LocalPlayer.CheckNicknameCheating(nicknameCorrect))
				{
					PlayerManager.Instance.SendEnterBaseRequest(28);
				}
			}
			targetType = ShotTargetType.PLAYER;
			if (raycastHit.transform.name == "ShipEngine")
			{
				hitZone = PlayerHitZone.ENGINE;
			}
			else if (raycastHit.transform.name == "Bip01 Head")
			{
				hitZone = PlayerHitZone.CABIN;
			}
			else if (raycastHit.transform.name == "Bip01")
			{
				hitZone = PlayerHitZone.ENGINE;
			}
			break;
		}
		case "Grenade":
		{
			BombTracer component3 = raycastHit.transform.parent.GetComponent<BombTracer>();
			if (component3 != null)
			{
				itemTimeStamp = component3.TimeStamp;
				num2 = component3.Owner.playerID;
				targetType = ShotTargetType.PLAYER_ITEM;
				targetTransform = component3.transform;
			}
			break;
		}
		case "Mine":
		{
			MineTracer component4 = raycastHit.transform.parent.GetComponent<MineTracer>();
			if (component4 != null)
			{
				itemTimeStamp = component4.TimeStamp;
				num2 = component4.Owner.playerID;
				targetType = ShotTargetType.PLAYER_ITEM;
				targetTransform = component4.transform;
			}
			break;
		}
		case "Turret":
		{
			TurretTracer component5 = raycastHit.transform.parent.GetComponent<TurretTracer>();
			if (component5 != null)
			{
				itemTimeStamp = component5.TimeStamp;
				num2 = component5.Owner.playerID;
				targetType = ShotTargetType.PLAYER_ITEM;
				targetTransform = component5.transform;
			}
			break;
		}
		case "Monster":
		{
			CombatPlayer component6 = raycastHit.transform.parent.GetComponent<CombatPlayer>();
			if (component6 != null)
			{
				num2 = component6.playerID;
				targetType = ShotTargetType.BOT;
				targetTransform = component6.transform;
			}
			break;
		}
		}
		if (raycastHit.distance < (float)maxDistance || maxDistance == 0)
		{
			shot = new Shot(raycastHit.point, raycastHit.normal, (byte)weaponType);
			if (num2 != -1)
			{
				ShotTarget shotTarget = new ShotTarget();
				shotTarget.TargetID = num2;
				shotTarget.ItemTimeStamp = itemTimeStamp;
				shotTarget.TargetType = targetType;
				shotTarget.TargetTransform = targetTransform;
				shotTarget.HitZone = hitZone;
				shot.Targets.Add(shotTarget);
			}
		}
		Vector3 zero = Vector3.zero;
		if (shot.HasTargets && !ShotCalculator.CheckStraightShot(ray.origin, raycastHit.point, ref zero))
		{
			if (Configuration.DebugEnableRTTX)
			{
				GameHUDFPS.Instance.SetDebugLine(string.Format("SHOT WALLS cheating detected: DEATH TRIGGER {0}", zero), 4);
			}
			Hashtable hashtable = new Hashtable();
			hashtable[1] = zero.x;
			hashtable[2] = zero.y;
			hashtable[3] = zero.z;
			PlayerManager.Instance.SendEnterBaseRequest(27, hashtable);
			return null;
		}
		if (num != -773)
		{
			if (Configuration.DebugEnableRTTX)
			{
				GameHUDFPS.Instance.SetDebugLine(string.Format("SHOT WALLS cheating detected: LAYER MASK {0}", num), 1);
			}
			PlayerManager.Instance.SendEnterBaseRequest(26);
			return null;
		}
		return shot;
	}

	public static Shot CrossbowShot(Ray cray, Transform TWeapon, int weaponType, int maxDistance, bool isBot)
	{
		Shot shot = null;
		ShotTargetType targetType = ShotTargetType.NONE;
		Transform targetTransform = null;
		PlayerHitZone hitZone = PlayerHitZone.BASE;
		int num = 772;
		if (isBot)
		{
			num = 516;
		}
		num = ~num;
		Ray ray = cray;
		RaycastHit raycastHit;
		Physics.SphereCast(ray, 0.2f, out raycastHit, float.PositiveInfinity, num);
		int num2 = -1;
		long itemTimeStamp = -1L;
		string name = raycastHit.transform.name;
		switch (name)
		{
		case "ShipModel":
		case "ShipBase":
		case "ShipEngine":
		case "ShipCabin":
		case "Bip01":
		case "Bip01 Spine2":
		case "Bip01 Head":
		case "Bip01 R Calf":
		case "Bip01 L Calf":
		case "Bip01 R Thigh":
		case "Bip01 L Thigh":
		case "Bip01 R UpperArm":
		case "Bip01 L UpperArm":
		case "Bip01 R Forearm":
		case "Bip01 L Forearm":
		{
			CombatPlayer component = raycastHit.transform.parent.parent.GetComponent<CombatPlayer>();
			if (component != null)
			{
				num2 = component.playerID;
				targetTransform = component.transform;
			}
			else
			{
				component = raycastHit.transform.parent.GetComponent<CombatPlayer>();
				if (component != null)
				{
					num2 = component.playerID;
					targetTransform = component.transform;
				}
				else
				{
					Item component2 = raycastHit.transform.GetComponent<Item>();
					if (component2 != null && component2.Root != null)
					{
						component = component2.Root.GetComponent<CombatPlayer>();
						if (component != null)
						{
							num2 = component.playerID;
							targetTransform = component.transform;
						}
					}
				}
			}
			if (component != null && component != PlayerManager.Instance.LocalPlayer && component.CanCheckNickName(PlayerManager.Instance.LocalPlayer))
			{
				bool nicknameCorrect = component.CheckNickname(PlayerManager.Instance.LocalPlayer);
				if (!PlayerManager.Instance.LocalPlayer.CheckNicknameCheating(nicknameCorrect))
				{
					PlayerManager.Instance.SendEnterBaseRequest(28);
				}
			}
			targetType = ShotTargetType.PLAYER;
			if (raycastHit.transform.name == "ShipEngine")
			{
				hitZone = PlayerHitZone.ENGINE;
			}
			else if (raycastHit.transform.name == "Bip01 Head")
			{
				hitZone = PlayerHitZone.CABIN;
			}
			else if (raycastHit.transform.name == "Bip01")
			{
				hitZone = PlayerHitZone.ENGINE;
			}
			break;
		}
		case "Grenade":
		{
			BombTracer component3 = raycastHit.transform.parent.GetComponent<BombTracer>();
			if (component3 != null)
			{
				itemTimeStamp = component3.TimeStamp;
				num2 = component3.Owner.playerID;
				targetType = ShotTargetType.PLAYER_ITEM;
				targetTransform = component3.transform;
			}
			break;
		}
		case "Mine":
		{
			MineTracer component4 = raycastHit.transform.parent.GetComponent<MineTracer>();
			if (component4 != null)
			{
				itemTimeStamp = component4.TimeStamp;
				num2 = component4.Owner.playerID;
				targetType = ShotTargetType.PLAYER_ITEM;
				targetTransform = component4.transform;
			}
			break;
		}
		case "Turret":
		{
			TurretTracer component5 = raycastHit.transform.parent.GetComponent<TurretTracer>();
			if (component5 != null)
			{
				itemTimeStamp = component5.TimeStamp;
				num2 = component5.Owner.playerID;
				targetType = ShotTargetType.PLAYER_ITEM;
				targetTransform = component5.transform;
			}
			break;
		}
		case "Monster":
		{
			CombatPlayer component6 = raycastHit.transform.parent.GetComponent<CombatPlayer>();
			if (component6 != null)
			{
				num2 = component6.playerID;
				targetType = ShotTargetType.BOT;
				targetTransform = component6.transform;
			}
			break;
		}
		}
		if (raycastHit.transform.parent != null && raycastHit.transform.parent.name.StartsWith("Door_"))
		{
			num2 = 0;
			targetType = ShotTargetType.GATES;
			targetTransform = raycastHit.transform.parent;
		}
		if (raycastHit.distance < (float)maxDistance || maxDistance == 0)
		{
			shot = new Shot(raycastHit.point + raycastHit.normal * 0.2f, raycastHit.normal, (byte)weaponType);
			if (num2 != -1)
			{
				ShotTarget shotTarget = new ShotTarget();
				shotTarget.TargetID = num2;
				shotTarget.ItemTimeStamp = itemTimeStamp;
				shotTarget.TargetType = targetType;
				shotTarget.TargetTransform = targetTransform;
				shotTarget.HitZone = hitZone;
				shot.Targets.Add(shotTarget);
			}
		}
		Vector3 zero = Vector3.zero;
		if (shot.HasTargets && !ShotCalculator.CheckStraightShot(ray.origin, raycastHit.point, ref zero))
		{
			if (Configuration.DebugEnableRTTX)
			{
				GameHUDFPS.Instance.SetDebugLine(string.Format("SHOT WALLS cheating detected: DEATH TRIGGER {0}", zero), 4);
			}
			Hashtable hashtable = new Hashtable();
			hashtable[1] = zero.x;
			hashtable[2] = zero.y;
			hashtable[3] = zero.z;
			PlayerManager.Instance.SendEnterBaseRequest(27, hashtable);
		}
		if (num != -773)
		{
			if (Configuration.DebugEnableRTTX)
			{
				GameHUDFPS.Instance.SetDebugLine(string.Format("SHOT WALLS cheating detected: LAYER MASK {0}", num), 1);
			}
			PlayerManager.Instance.SendEnterBaseRequest(26);
		}
		return shot;
	}

	public static bool CheckShotRay(Ray cray, float distance, bool isBot)
	{
		RaycastHit raycastHit;
		return ShotCalculator.CheckShotRay(cray, out raycastHit, distance, isBot);
	}

	public static bool CheckShotRay(Ray cray, out RaycastHit hit, float distance, bool isBot)
	{
		float num = 5f;
		int num2 = 2820;
		if (isBot)
		{
			num2 = 2564;
		}
		num2 = ~num2;
		Physics.Raycast(cray, out hit, distance + num, num2);
		return hit.distance + num >= distance || !(hit.transform != null);
	}

	public static bool CheckShotNormalRay(Ray cray, out RaycastHit outHit, Vector3 position, float distance, float verticalAngle, bool isBot)
	{
		int num = 772;
		if (isBot)
		{
			num = 516;
		}
		num = ~num;
		RaycastHit raycastHit;
		Physics.Raycast(cray, out raycastHit, float.PositiveInfinity, num);
		Ray ray = new Ray(position, raycastHit.point - position);
		Physics.SphereCast(ray, 0.5f, out raycastHit, float.PositiveInfinity, num);
		outHit = raycastHit;
		return (raycastHit.distance <= distance || !(raycastHit.transform != null)) && !(raycastHit.transform == null) && Mathf.Abs(Vector3.Angle(raycastHit.normal, new Vector3(0f, 1f, 0f))) <= verticalAngle;
	}

	public static Shot RailShot(Ray ray, Transform TWeapon, int weaponType, int maxDistance, bool isBot)
	{
		Shot shot = null;
		ShotTargetType targetType = ShotTargetType.NONE;
		int num = 772;
		if (isBot)
		{
			num = 516;
		}
		num = ~num;
		RaycastHit raycastHit;
		Physics.SphereCast(ray, 0.5f, out raycastHit, float.PositiveInfinity, num);
		int num2 = -1;
		long itemTimeStamp = -1L;
		string name = raycastHit.transform.name;
		switch (name)
		{
		case "ShipModel":
		case "ShipBase":
		case "ShipEngine":
		case "ShipCabin":
		case "Bip01 Spine2":
		case "Bip01 Head":
		case "Bip01 R Calf":
		case "Bip01 L Calf":
		case "Bip01 R Thigh":
		case "Bip01 L Thigh":
		case "Bip01 R UpperArm":
		case "Bip01 L UpperArm":
		case "Bip01 R Forearm":
		case "Bip01 L Forearm":
		{
			CombatPlayer component = raycastHit.transform.parent.parent.GetComponent<CombatPlayer>();
			if (component != null)
			{
				num2 = component.playerID;
			}
			else
			{
				component = raycastHit.transform.parent.GetComponent<CombatPlayer>();
				if (component != null)
				{
					num2 = component.playerID;
				}
			}
			targetType = ShotTargetType.PLAYER;
			break;
		}
		case "Mine":
		{
			MineTracer component2 = raycastHit.transform.parent.GetComponent<MineTracer>();
			if (component2 != null)
			{
				itemTimeStamp = component2.TimeStamp;
				num2 = component2.Owner.playerID;
				targetType = ShotTargetType.PLAYER_ITEM;
			}
			break;
		}
		case "Turret":
		{
			TurretTracer component3 = raycastHit.transform.parent.GetComponent<TurretTracer>();
			if (component3 != null)
			{
				itemTimeStamp = component3.TimeStamp;
				num2 = component3.Owner.playerID;
				targetType = ShotTargetType.PLAYER_ITEM;
			}
			break;
		}
		case "Monster":
		{
			CombatPlayer component4 = raycastHit.transform.parent.GetComponent<CombatPlayer>();
			if (component4 != null)
			{
				num2 = component4.playerID;
				targetType = ShotTargetType.BOT;
			}
			break;
		}
		}
		if (raycastHit.distance < (float)maxDistance || maxDistance == 0)
		{
			shot = new Shot(raycastHit.point, raycastHit.normal, (byte)weaponType);
			if (num2 != -1)
			{
				ShotTarget shotTarget = new ShotTarget();
				shotTarget.TargetID = num2;
				shotTarget.ItemTimeStamp = itemTimeStamp;
				shotTarget.TargetType = targetType;
				shot.Targets.Add(shotTarget);
			}
		}
		return shot;
	}

	public static Shot RocketShot(Ray cray, Transform TWeapon, int weaponType, float velocity)
	{
		int num = 772;
		num = ~num;
		RaycastHit raycastHit;
		Physics.Raycast(cray, out raycastHit, float.PositiveInfinity, num);
		Ray ray = new Ray(TWeapon.position, raycastHit.point - TWeapon.position);
		float magnitude = (raycastHit.point - TWeapon.position).magnitude;
		if (magnitude > 5f)
		{
			Physics.SphereCast(ray, 0.5f, out raycastHit, float.PositiveInfinity, num);
		}
		long num2 = TimeManager.Instance.NetworkTime + 200L;
		long landingTimeStamp = num2 + (long)(100f * (raycastHit.point - ray.origin).magnitude / velocity);
		Shot shot = new Shot(raycastHit.point, raycastHit.normal, (byte)weaponType);
		shot.Launch(TWeapon.position, ray.direction, num2, landingTimeStamp);
		shot.addPoint(ray.origin);
		return shot;
	}

	public static Shot GrenadeShot(Ray cray, Transform TWeapon, int weaponType, float velocity, float life, bool stick)
	{
		int num = 772;
		num = ~num;
		RaycastHit raycastHit;
		Physics.Raycast(cray, out raycastHit, float.PositiveInfinity, num);
		Ray ray = new Ray(TWeapon.position, raycastHit.point - TWeapon.position);
		float magnitude = (raycastHit.point - TWeapon.position).magnitude;
		int num2 = Mathf.RoundToInt(life / 100f);
		if (num2 <= 0)
		{
			num2 = 2;
		}
		float num3 = 0.2f;
		float num4 = velocity;
		if (num4 <= 0f)
		{
			num4 = 2f;
		}
		float num5 = 0.3f;
		int i = 1;
		ray.direction.Normalize();
		Vector3 vector = ray.direction;
		vector.Scale(new Vector3(num4, num4, num4));
		ray.direction = vector;
		Shot shot = new Shot(ray.origin, ray.direction, (byte)weaponType);
		shot.addPoint(ray.origin);
		while (i < num2)
		{
			if (vector.sqrMagnitude > 0f)
			{
				if (Physics.Raycast(ray, out raycastHit, vector.magnitude, num))
				{
					float num6 = vector.magnitude;
					vector = Vector3.Reflect(vector, raycastHit.normal);
					vector.Normalize();
					vector.Scale(new Vector3(num5 * (num6 - raycastHit.distance), num5 * (num6 - raycastHit.distance), num5 * (num6 - raycastHit.distance)));
					ray.origin = raycastHit.point;
					vector.Normalize();
					num6 *= num5;
					vector.Scale(new Vector3(num6, num6, num6));
					if (num6 < 0.5f)
					{
						ray.origin = raycastHit.point;
						ray.direction = new Vector3(0f, 0f, 0f);
						vector = new Vector3(0f, 0f, 0f);
					}
				}
				else
				{
					ray.origin += vector;
					vector += new Vector3(0f, -num3, 0f);
				}
				if ((i == 1 && magnitude < 5f) || (stick && raycastHit.transform != null && !raycastHit.transform.name.StartsWith("Bip") && !raycastHit.transform.name.StartsWith("Smerti")))
				{
					ray.direction = vector;
					shot.addPoint(ray.origin);
					i++;
					break;
				}
			}
			ray.direction = vector;
			shot.addPoint(ray.origin);
			i++;
		}
		long num7 = TimeManager.Instance.NetworkTime + 200L;
		long landingTimeStamp = num7 + (long)(1000f * (float)i / 20f);
		Shot shot2 = new Shot(ray.origin, ray.direction, (byte)weaponType);
		shot2.Launch(cray.origin, vector, num7, landingTimeStamp);
		shot2.Trajectory = shot.Trajectory;
		return shot2;
	}

	public static Shot MineShot(Ray cray, Transform TWeapon, int weaponType)
	{
		int num = 516;
		num = ~num;
		Ray ray = new Ray(TWeapon.position, cray.direction);
		RaycastHit raycastHit;
		Physics.SphereCast(ray, 0.5f, out raycastHit, float.PositiveInfinity, num);
		long launchTimeStamp = TimeManager.Instance.NetworkTime + 200L;
		long landingTimeStamp = TimeManager.Instance.NetworkTime + 200L + 60000L;
		if (weaponType == 104)
		{
			landingTimeStamp = TimeManager.Instance.NetworkTime + 200L + 3000L;
		}
		Shot shot = new Shot(raycastHit.point + raycastHit.normal * 0.1f, raycastHit.normal, (byte)weaponType);
		shot.Launch(ray.origin, ray.direction, launchTimeStamp, landingTimeStamp);
		return shot;
	}

	public static Shot StraightMineShot(Ray cray, Transform TWeapon, int weaponType, int maxDistance, float maxAngle, bool isBot)
	{
		Shot shot = null;
		ShotTargetType targetType = ShotTargetType.NONE;
		Transform targetTransform = null;
		PlayerHitZone hitZone = PlayerHitZone.BASE;
		int num = 772;
		if (isBot)
		{
			num = 516;
		}
		num = ~num;
		RaycastHit raycastHit;
		Physics.Raycast(cray, out raycastHit, float.PositiveInfinity, num);
		Ray ray = new Ray(TWeapon.position, raycastHit.point - TWeapon.position);
		Physics.SphereCast(ray, 0.5f, out raycastHit, float.PositiveInfinity, num);
		int num2 = -1;
		long itemTimeStamp = -1L;
		if (raycastHit.distance < (float)maxDistance || maxDistance == 0)
		{
			if (maxAngle < 180f && Mathf.Abs(Vector3.Angle(raycastHit.normal, new Vector3(0f, 1f, 0f))) > maxAngle)
			{
				return shot;
			}
			shot = new Shot(raycastHit.point + raycastHit.normal * 0.2f, raycastHit.normal, (byte)weaponType);
			if (num2 != -1)
			{
				ShotTarget shotTarget = new ShotTarget();
				shotTarget.TargetID = num2;
				shotTarget.ItemTimeStamp = itemTimeStamp;
				shotTarget.TargetType = targetType;
				shotTarget.TargetTransform = targetTransform;
				shotTarget.HitZone = hitZone;
				shot.Targets.Add(shotTarget);
			}
		}
		long launchTimeStamp = TimeManager.Instance.NetworkTime + 200L;
		long landingTimeStamp = TimeManager.Instance.NetworkTime + 200L + 60000L;
		if (shot == null)
		{
			return null;
		}
		shot.Launch(TWeapon.position, cray.direction, launchTimeStamp, landingTimeStamp);
		return shot;
	}

	public static Shot StraightTurretShot(Ray cray, Transform TWeapon, int weaponType, int maxDistance, float maxAngle, bool isBot)
	{
		Shot shot = ShotCalculator.StraightShot(cray, TWeapon, weaponType, maxDistance, isBot);
		long launchTimeStamp = TimeManager.Instance.NetworkTime + 200L;
		long landingTimeStamp = TimeManager.Instance.NetworkTime + 200L + 60000L;
		if (shot == null)
		{
			return null;
		}
		shot.Launch(TWeapon.position, cray.direction, launchTimeStamp, landingTimeStamp);
		return shot;
	}

	public static void RadialShot(CombatPlayer player, Shot shot, bool allowDirect)
	{
		CombatWeapon weaponByType = player.ShotController.GetWeaponByType((int)shot.WeaponType);
		Vector3 b = new Vector3(0f, 0f, 0f);
		Vector3 b2 = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z) + new Vector3(0f, 3.5f, 0f);
		Vector3 a = new Vector3(shot.Origin.x, shot.Origin.y, shot.Origin.z);
		Vector3 vector = a - b2;
		Vector3 vector2 = new Vector3(0f, 0f, 0f);
		Vector3 vector3 = new Vector3(0f, 0f, 0f);
		ShotTarget shotTarget = null;
		if (allowDirect && shot.HasTargets && (shot.Targets[0].TargetType == ShotTargetType.PLAYER || shot.Targets[0].TargetType == ShotTargetType.PLAYER_ITEM))
		{
			CombatPlayer combatPlayer = PlayerManager.Instance.Players[shot.Targets[0].TargetID];
			if (player.Team != combatPlayer.Team || (player.Team == 0 && PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.TOWER_DEFENSE) || player == combatPlayer || PlayerManager.Instance.RoomSettings.FriendlyFire)
			{
				shot.Targets[0].Direct = true;
				shotTarget = shot.Targets[0];
			}
			else
			{
				shot.Targets.Clear();
			}
		}
		List<ItemTracer> list = new List<ItemTracer>();
		if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.TOWER_DEFENSE)
		{
			foreach (CombatPlayer combatPlayer2 in PlayerManager.Instance.Campaign.Actors.Values)
			{
				if (shotTarget == null || shotTarget.TargetType != ShotTargetType.BOT || combatPlayer2.playerID != shotTarget.TargetID)
				{
					b = new Vector3(combatPlayer2.transform.position.x, combatPlayer2.transform.position.y, combatPlayer2.transform.position.z);
					vector3 = a - b;
					float num = vector3.magnitude;
					float num2 = weaponByType.Distance * 1.5f;
					if (weaponByType.Type == WeaponType.ROCKET_LAUNCHER)
					{
						num2 = 30f;
					}
					if (num <= num2)
					{
						ShotTarget shotTarget2 = new ShotTarget();
						shotTarget2.TargetID = combatPlayer2.playerID;
						shotTarget2.TargetType = ShotTargetType.BOT;
						shot.Targets.Add(shotTarget2);
					}
				}
			}
		}
		foreach (CombatPlayer combatPlayer3 in PlayerManager.Instance.Players.Values)
		{
			if (player.Team != combatPlayer3.Team || PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.DEATHMATCH || PlayerManager.Instance.RoomSettings.FriendlyFire || combatPlayer3 == player || shot.WeaponType == WeaponType.TURRET_TESLA || shot.WeaponType == WeaponType.CHARGER || shot.WeaponType == WeaponType.MEGA_CHARGER)
			{
				if (shot.WeaponType != WeaponType.TURRET_TESLA)
				{
					foreach (ItemTracer itemTracer in combatPlayer3.RegisteredItems.Values)
					{
						if (!(itemTracer == null))
						{
							if (!(itemTracer.transform == null))
							{
								if (itemTracer.WeaponType == WeaponType.BOMB_LAUNCHER || itemTracer.WeaponType == WeaponType.TURRET_TESLA || itemTracer.WeaponType == WeaponType.TURRET_MACHINE_GUN)
								{
									list.Add(itemTracer);
								}
							}
						}
					}
					foreach (ItemTracer itemTracer2 in list)
					{
						if (shotTarget == null || shotTarget.TargetType != ShotTargetType.PLAYER_ITEM || combatPlayer3.playerID != shotTarget.TargetID || shotTarget.ItemTimeStamp != itemTracer2.TimeStamp)
						{
							b = new Vector3(itemTracer2.transform.position.x, itemTracer2.transform.position.y, itemTracer2.transform.position.z);
							vector3 = a - b;
							float num = vector3.magnitude;
							if (num <= weaponByType.Distance)
							{
								ShotTarget shotTarget2 = new ShotTarget();
								shotTarget2.TargetID = combatPlayer3.playerID;
								shotTarget2.TargetType = ShotTargetType.PLAYER_ITEM;
								shotTarget2.ItemTimeStamp = itemTracer2.TimeStamp;
								shot.Targets.Add(shotTarget2);
							}
						}
					}
					list.Clear();
				}
				if (!combatPlayer3.IsDead)
				{
					if (shotTarget == null || shotTarget.TargetType != ShotTargetType.PLAYER || combatPlayer3.playerID != shotTarget.TargetID)
					{
						b = new Vector3(combatPlayer3.transform.position.x, combatPlayer3.transform.position.y, combatPlayer3.transform.position.z) + new Vector3(0f, 3.5f, 0f);
						vector3 = a - b;
						float num = vector3.magnitude;
						float num3 = weaponByType.Distance * 1.5f;
						if (weaponByType.Type == WeaponType.ROCKET_LAUNCHER)
						{
							num3 = 30f;
						}
						if (num <= num3)
						{
							ShotTarget shotTarget2 = new ShotTarget();
							shotTarget2.TargetID = combatPlayer3.playerID;
							shotTarget2.TargetType = ShotTargetType.PLAYER;
							shot.Targets.Add(shotTarget2);
						}
					}
				}
			}
		}
	}

	public static void NearestRadialShot(CombatPlayer player, Shot shot, bool allowDirect)
	{
		CombatWeapon weaponByType = player.ShotController.GetWeaponByType((int)shot.WeaponType);
		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 b = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
		Vector3 vector2 = new Vector3(shot.Origin.x, shot.Origin.y, shot.Origin.z);
		Vector3 vector3 = vector2 - b;
		Vector3 vector4 = new Vector3(0f, 0f, 0f);
		Vector3 vector5 = new Vector3(0f, 0f, 0f);
		ShotTarget shotTarget = null;
		if (allowDirect && shot.HasTargets)
		{
			shot.Targets[0].Direct = true;
			shotTarget = shot.Targets[0];
		}
		List<ItemTracer> list = new List<ItemTracer>();
		ShotTarget shotTarget2 = null;
		float num = weaponByType.Distance;
		if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.TOWER_DEFENSE)
		{
			foreach (CombatPlayer combatPlayer in PlayerManager.Instance.Campaign.Actors.Values)
			{
				if (shotTarget == null || shotTarget.TargetType != ShotTargetType.BOT || combatPlayer.playerID != shotTarget.TargetID)
				{
					vector = new Vector3(combatPlayer.transform.position.x, combatPlayer.transform.position.y, combatPlayer.transform.position.z);
					vector5 = vector2 - vector;
					float num2 = vector5.magnitude;
					if (num2 <= weaponByType.Distance)
					{
						Vector3 vector6 = -vector5.normalized;
						if (ShotCalculator.CheckShotRay(new Ray(vector2 + vector6 * 2f, vector6), num2, false))
						{
							if (num2 < num)
							{
								num = num2;
								shotTarget2 = new ShotTarget();
								shotTarget2.TargetID = combatPlayer.playerID;
								shotTarget2.TargetType = ShotTargetType.BOT;
							}
						}
					}
				}
			}
		}
		foreach (CombatPlayer combatPlayer2 in PlayerManager.Instance.Players.Values)
		{
			if (player.Team != combatPlayer2.Team || PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.DEATHMATCH || PlayerManager.Instance.RoomSettings.FriendlyFire || combatPlayer2 == player || shot.WeaponType == WeaponType.TURRET_TESLA)
			{
				if (shot.WeaponType != WeaponType.TURRET_TESLA)
				{
					foreach (ItemTracer itemTracer in combatPlayer2.RegisteredItems.Values)
					{
						if (!(itemTracer == null))
						{
							if (!(itemTracer.transform == null))
							{
								if (itemTracer.WeaponType == WeaponType.TURRET_TESLA || itemTracer.WeaponType == WeaponType.TURRET_MACHINE_GUN)
								{
									list.Add(itemTracer);
								}
							}
						}
					}
					foreach (ItemTracer itemTracer2 in list)
					{
						if (shotTarget == null || shotTarget.TargetType != ShotTargetType.PLAYER_ITEM || combatPlayer2.playerID != shotTarget.TargetID || shotTarget.ItemTimeStamp != itemTracer2.TimeStamp)
						{
							vector = new Vector3(itemTracer2.transform.position.x, itemTracer2.transform.position.y, itemTracer2.transform.position.z);
							vector5 = vector2 - vector;
							float num2 = vector5.magnitude;
							if (num2 <= weaponByType.Distance)
							{
								if (num2 < num)
								{
									num = num2;
									shotTarget2 = new ShotTarget();
									shotTarget2.TargetID = combatPlayer2.playerID;
									shotTarget2.TargetType = ShotTargetType.PLAYER_ITEM;
									shotTarget2.ItemTimeStamp = itemTracer2.TimeStamp;
								}
							}
						}
					}
					list.Clear();
				}
				if (!combatPlayer2.IsDead)
				{
					if (shotTarget == null || shotTarget.TargetType != ShotTargetType.PLAYER || combatPlayer2.playerID != shotTarget.TargetID)
					{
						vector = new Vector3(combatPlayer2.transform.position.x, combatPlayer2.transform.position.y, combatPlayer2.transform.position.z);
						vector5 = vector2 - vector;
						float num2 = vector5.magnitude;
						if (num2 <= weaponByType.Distance)
						{
							if (ShotCalculator.CheckShotRay(new Ray(vector2, vector - vector2), num2, false))
							{
								if (num2 < num)
								{
									num = num2;
									shotTarget2 = new ShotTarget();
									shotTarget2.TargetID = combatPlayer2.playerID;
									shotTarget2.TargetType = ShotTargetType.PLAYER;
								}
							}
						}
					}
				}
			}
		}
		if (shotTarget2 != null)
		{
			shot.Targets.Add(shotTarget2);
		}
	}

	public static void ChainRadialShot(CombatPlayer player, Shot shot, bool allowDirect)
	{
		CombatWeapon weaponByType = player.ShotController.GetWeaponByType((int)shot.WeaponType);
		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 b = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
		Vector3 position = new Vector3(shot.Origin.x, shot.Origin.y, shot.Origin.z);
		Vector3 vector2 = position - b;
		Vector3 vector3 = new Vector3(0f, 0f, 0f);
		Vector3 vector4 = new Vector3(0f, 0f, 0f);
		Dictionary<int, ShotTarget> dictionary = new Dictionary<int, ShotTarget>();
		Dictionary<int, ShotTarget> dictionary2 = new Dictionary<int, ShotTarget>();
		if (allowDirect && shot.HasTargets)
		{
			shot.Targets[0].Direct = true;
			ShotTarget shotTarget = shot.Targets[0];
			if (shotTarget.TargetType == ShotTargetType.PLAYER)
			{
				dictionary.Add(shotTarget.TargetID, shotTarget);
			}
			else if (shotTarget.TargetType == ShotTargetType.BOT)
			{
				dictionary2.Add(shotTarget.TargetID, shotTarget);
			}
			if (shotTarget.TargetTransform != null)
			{
				position = shotTarget.TargetTransform.position;
			}
		}
		ShotTarget shotTarget2;
		do
		{
			shotTarget2 = null;
			float num = weaponByType.Distance;
			if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.TOWER_DEFENSE)
			{
				foreach (CombatPlayer combatPlayer in PlayerManager.Instance.Campaign.Actors.Values)
				{
					if (!dictionary2.ContainsKey(combatPlayer.playerID))
					{
						vector = new Vector3(combatPlayer.transform.position.x, combatPlayer.transform.position.y, combatPlayer.transform.position.z);
						vector4 = position - vector;
						float num2 = vector4.magnitude;
						if (num2 <= weaponByType.Distance)
						{
							Vector3 vector5 = -vector4.normalized;
							if (ShotCalculator.CheckShotRay(new Ray(position + vector5 * 2f, vector5), num2, false))
							{
								if (num2 < num)
								{
									num = num2;
									shotTarget2 = new ShotTarget();
									shotTarget2.TargetID = combatPlayer.playerID;
									shotTarget2.TargetType = ShotTargetType.BOT;
									shotTarget2.TargetTransform = combatPlayer.transform;
								}
							}
						}
					}
				}
			}
			foreach (CombatPlayer combatPlayer2 in PlayerManager.Instance.Players.Values)
			{
				if (player.Team != combatPlayer2.Team || PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.DEATHMATCH || PlayerManager.Instance.RoomSettings.FriendlyFire || combatPlayer2 == player || shot.WeaponType == WeaponType.TURRET_TESLA)
				{
					if (!combatPlayer2.IsDead)
					{
						if (!dictionary.ContainsKey(combatPlayer2.playerID))
						{
							vector = new Vector3(combatPlayer2.transform.position.x, combatPlayer2.transform.position.y, combatPlayer2.transform.position.z);
							vector4 = position - vector;
							float num2 = vector4.magnitude;
							if (num2 <= weaponByType.Distance)
							{
								if (ShotCalculator.CheckShotRay(new Ray(position, vector - position), num2, false))
								{
									if (num2 < num)
									{
										num = num2;
										shotTarget2 = new ShotTarget();
										shotTarget2.TargetID = combatPlayer2.playerID;
										shotTarget2.TargetType = ShotTargetType.PLAYER;
										shotTarget2.TargetTransform = combatPlayer2.transform;
									}
								}
							}
						}
					}
				}
			}
			if (shotTarget2 != null)
			{
				shot.Targets.Add(shotTarget2);
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Target: ",
					shotTarget2.TargetID,
					" ",
					shotTarget2.TargetType.ToString()
				}));
				if (shotTarget2.TargetType == ShotTargetType.PLAYER)
				{
					dictionary.Add(shotTarget2.TargetID, shotTarget2);
				}
				else if (shotTarget2.TargetType == ShotTargetType.BOT)
				{
					dictionary2.Add(shotTarget2.TargetID, shotTarget2);
				}
				position = shotTarget2.TargetTransform.position;
			}
		}
		while (shotTarget2 != null);
	}

	public static void GunShot(CombatPlayer player, Shot shot)
	{
		if (shot.HasTargets && (shot.Targets[0].TargetType == ShotTargetType.PLAYER || shot.Targets[0].TargetType == ShotTargetType.PLAYER_ITEM))
		{
			CombatPlayer combatPlayer = PlayerManager.Instance.Players[shot.Targets[0].TargetID];
			if (player.Team != combatPlayer.Team || (player.Team == 0 && PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.TOWER_DEFENSE) || player == combatPlayer || PlayerManager.Instance.RoomSettings.FriendlyFire)
			{
				return;
			}
			shot.Targets.Clear();
		}
	}

	public static void SegmentShot(CombatPlayer player, Shot shot)
	{
		CombatWeapon weaponByType = player.ShotController.GetWeaponByType((int)shot.WeaponType);
		if (weaponByType == null)
		{
			return;
		}
		Vector3 a = new Vector3(0f, 0f, 0f);
		Vector3 vector = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z) + new Vector3(0f, 3.5f, 0f);
		Vector3 a2 = new Vector3(shot.Origin.x, shot.Origin.y, shot.Origin.z);
		Vector3 to = a2 - vector;
		Vector3 from = new Vector3(0f, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, 0f, 0f);
		List<ItemTracer> list = new List<ItemTracer>();
		int num = -1;
		if (shot.Targets.Count > 0)
		{
			num = shot.Targets[0].TargetID;
		}
		if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.TOWER_DEFENSE)
		{
			foreach (CombatPlayer combatPlayer in PlayerManager.Instance.Campaign.Actors.Values)
			{
				a = new Vector3(combatPlayer.transform.position.x, combatPlayer.transform.position.y, combatPlayer.transform.position.z);
				from = a - vector;
				float num2 = from.magnitude;
				if (num2 <= weaponByType.Distance / 5f)
				{
					float num3 = Vector3.Angle(from, to) * 3.14159274f / 180f;
					if (num3 <= weaponByType.Angle)
					{
						if (ShotCalculator.CheckShotRay(new Ray(vector, a - vector), num2, false))
						{
							ShotTarget shotTarget = new ShotTarget();
							shotTarget.TargetID = combatPlayer.playerID;
							shotTarget.TargetType = ShotTargetType.BOT;
							shot.Targets.Add(shotTarget);
						}
					}
				}
			}
		}
		foreach (CombatPlayer combatPlayer2 in PlayerManager.Instance.Players.Values)
		{
			if (combatPlayer2 != player && combatPlayer2.playerID != num && (player.Team != combatPlayer2.Team || PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.DEATHMATCH || PlayerManager.Instance.RoomSettings.FriendlyFire))
			{
				foreach (ItemTracer itemTracer in combatPlayer2.RegisteredItems.Values)
				{
					if (!(itemTracer == null))
					{
						if (!(itemTracer.transform == null))
						{
							if (itemTracer.WeaponType == WeaponType.BOMB_LAUNCHER || itemTracer.WeaponType == WeaponType.TURRET_TESLA || itemTracer.WeaponType == WeaponType.TURRET_MACHINE_GUN)
							{
								list.Add(itemTracer);
							}
						}
					}
				}
				foreach (ItemTracer itemTracer2 in list)
				{
					a = new Vector3(itemTracer2.transform.position.x, itemTracer2.transform.position.y, itemTracer2.transform.position.z);
					from = a - vector;
					float num2 = from.magnitude;
					if (num2 <= weaponByType.Distance)
					{
						float num3 = Vector3.Angle(from, to) * 3.14159274f / 180f;
						if (num3 <= weaponByType.Angle)
						{
							if (ShotCalculator.CheckShotRay(new Ray(vector, a - vector), num2, false))
							{
								ShotTarget shotTarget = new ShotTarget();
								shotTarget.TargetID = combatPlayer2.playerID;
								shotTarget.TargetType = ShotTargetType.PLAYER_ITEM;
								shotTarget.ItemTimeStamp = itemTracer2.TimeStamp;
								shot.Targets.Add(shotTarget);
							}
						}
					}
				}
				list.Clear();
				if (!(combatPlayer2 == player))
				{
					if (!combatPlayer2.IsDead)
					{
						a = new Vector3(combatPlayer2.transform.position.x, combatPlayer2.transform.position.y, combatPlayer2.transform.position.z) + new Vector3(0f, 3.5f, 0f);
						from = a - vector;
						float num2 = from.magnitude;
						if (num2 <= weaponByType.Distance)
						{
							float num3 = Vector3.Angle(from, to) * 3.14159274f / 180f;
							if (num3 <= weaponByType.Angle)
							{
								if (ShotCalculator.CheckShotRay(new Ray(vector, a - vector), num2, false))
								{
									ShotTarget shotTarget = new ShotTarget();
									shotTarget.TargetID = combatPlayer2.playerID;
									shotTarget.TargetType = ShotTargetType.PLAYER;
									shot.Targets.Add(shotTarget);
								}
							}
						}
					}
				}
			}
		}
	}

	public static void NearestSegmentShot(CombatPlayer player, Shot shot)
	{
		CombatWeapon weaponByType = player.ShotController.GetWeaponByType((int)shot.WeaponType);
		Vector3 a = new Vector3(0f, 0f, 0f);
		Vector3 vector = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
		Vector3 a2 = new Vector3(shot.Origin.x, shot.Origin.y, shot.Origin.z);
		Vector3 to = a2 - vector;
		Vector3 from = new Vector3(0f, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, 0f, 0f);
		List<ItemTracer> list = new List<ItemTracer>();
		Dictionary<ShotTarget, float> dictionary = new Dictionary<ShotTarget, float>();
		if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.TOWER_DEFENSE)
		{
			foreach (CombatPlayer combatPlayer in PlayerManager.Instance.Campaign.Actors.Values)
			{
				a = new Vector3(combatPlayer.transform.position.x, combatPlayer.transform.position.y, combatPlayer.transform.position.z);
				from = a - vector;
				float num = from.magnitude;
				if (num <= weaponByType.Distance)
				{
					float num2 = Vector3.Angle(from, to);
					if (num2 <= weaponByType.Angle)
					{
						if (ShotCalculator.CheckShotRay(new Ray(vector, a - vector), num, false))
						{
							dictionary.Add(new ShotTarget
							{
								TargetID = combatPlayer.playerID,
								TargetType = ShotTargetType.BOT
							}, num);
						}
					}
				}
			}
		}
		foreach (CombatPlayer combatPlayer2 in PlayerManager.Instance.Players.Values)
		{
			if (player.Team != combatPlayer2.Team || ((PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.DEATHMATCH || PlayerManager.Instance.RoomSettings.FriendlyFire || shot.WeaponType == WeaponType.CHARGER || shot.WeaponType == WeaponType.MEGA_CHARGER) && combatPlayer2 != player))
			{
				foreach (ItemTracer itemTracer in combatPlayer2.RegisteredItems.Values)
				{
					if (!(itemTracer == null))
					{
						if (!(itemTracer.transform == null))
						{
							if (itemTracer.WeaponType == WeaponType.TURRET_TESLA || itemTracer.WeaponType == WeaponType.TURRET_MACHINE_GUN)
							{
								list.Add(itemTracer);
							}
						}
					}
				}
				foreach (ItemTracer itemTracer2 in list)
				{
					a = new Vector3(itemTracer2.transform.position.x, itemTracer2.transform.position.y, itemTracer2.transform.position.z);
					from = a - vector;
					float num = from.magnitude;
					if (num <= weaponByType.Distance)
					{
						float num2 = Vector3.Angle(from, to);
						if (num2 <= weaponByType.Angle)
						{
							if (ShotCalculator.CheckShotRay(new Ray(vector, a - vector), num, false))
							{
								ShotTarget shotTarget = new ShotTarget();
								shotTarget.TargetID = combatPlayer2.playerID;
								shotTarget.TargetType = ShotTargetType.PLAYER_ITEM;
								shotTarget.ItemTimeStamp = itemTracer2.TimeStamp;
								shot.Targets.Add(shotTarget);
							}
						}
					}
				}
				list.Clear();
				if (!combatPlayer2.IsDead)
				{
					a = new Vector3(combatPlayer2.transform.position.x, combatPlayer2.transform.position.y, combatPlayer2.transform.position.z);
					from = a - vector;
					float num = from.magnitude;
					if (num <= weaponByType.Distance)
					{
						float num2 = Vector3.Angle(from, to);
						if (num2 <= weaponByType.Angle)
						{
							if (ShotCalculator.CheckShotRay(new Ray(vector, a - vector), num, false))
							{
								dictionary.Add(new ShotTarget
								{
									TargetID = combatPlayer2.playerID,
									TargetType = ShotTargetType.PLAYER
								}, num);
							}
						}
					}
				}
			}
		}
		if (dictionary.Count == 0)
		{
			return;
		}
		float num3 = weaponByType.Distance;
		ShotTarget item = null;
		foreach (ShotTarget shotTarget2 in dictionary.Keys)
		{
			float num = dictionary[shotTarget2];
			if (num < num3)
			{
				num3 = num;
				item = shotTarget2;
			}
		}
		shot.Targets.Add(item);
	}
}
