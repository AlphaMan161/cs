// dnSpy decompiler from Assembly-CSharp.dll class: GUIHover
using System;
using GameLogic.Ability;
using UnityEngine;

public class GUIHover : MonoBehaviour
{
	public static short ZIndex
	{
		get
		{
			return GUIHover.zIndex;
		}
		set
		{
			GUIHover.zIndex = value;
		}
	}

	public static bool Enable
	{
		get
		{
			return GUIHover.enable;
		}
		set
		{
			GUIHover.enable = value;
			if (!GUIHover.enable && GUIHover.item != null)
			{
				GUIHover.item = null;
				GUIHover.isShowHover = false;
				GUIHover.isShowedHover = false;
			}
		}
	}

	public void Awake()
	{
		GUIHover.lastCheckTime = Time.time;
	}

	public void Start()
	{
		GUIHover.lastCheckTime = Time.time;
	}

	public static void Hover(Event currentEvent, string hoverText, Rect objectPos)
	{
		GUIHover.Hover(currentEvent, hoverText, objectPos, 1, objectPos);
	}

	public static void Hover(Event currentEvent, string hoverText, Rect objectPos, Rect workingAreaRect)
	{
		GUIHover.Hover(currentEvent, hoverText, objectPos, 1, workingAreaRect);
	}

	public static void Hover(Event currentEvent, string hoverText, Rect objectPos, short z_index, Rect workingAreaRect)
	{
		if (currentEvent.type == EventType.Repaint && z_index == GUIHover.zIndex && objectPos.Contains(currentEvent.mousePosition) && workingAreaRect.Contains(currentEvent.mousePosition))
		{
			GUIHover.hoverType = GUIHover.HoverType.HString;
			GUIHover.lastCheckTime = Time.time;
			if (GUIHover.needResize)
			{
				GUIHover.hoverRect.x = UnityEngine.Input.mousePosition.x;
				GUIHover.hoverRect.y = (float)Screen.height - UnityEngine.Input.mousePosition.y;
				GUIHover.hoverRect.x = GUIHover.hoverRect.x + (objectPos.x - currentEvent.mousePosition.x - (GUIHover.hoverRect.width - objectPos.width) * 0.5f);
				GUIHover.hoverRect.y = GUIHover.hoverRect.y + (objectPos.y - currentEvent.mousePosition.y + objectPos.height);
				if ((float)Screen.height < GUIHover.hoverRect.y + GUIHover.hoverRect.height)
				{
					if (GUIHover.hoverRect.y - (GUIHover.hoverRect.height + objectPos.height) < 0f)
					{
						if (GUIHover.hoverRect.x - (GUIHover.hoverRect.width * 0.5f + objectPos.width * 0.5f) < 0f)
						{
							GUIHover.hoverRect.x = GUIHover.hoverRect.x + (GUIHover.hoverRect.width * 0.5f + objectPos.width * 0.5f);
						}
						else
						{
							GUIHover.hoverRect.x = GUIHover.hoverRect.x - (GUIHover.hoverRect.width * 0.5f + objectPos.width * 0.5f);
						}
						GUIHover.hoverRect.y = GUIHover.hoverRect.y - (GUIHover.hoverRect.height + objectPos.height) * 0.5f;
						if (GUIHover.hoverRect.y + GUIHover.hoverRect.height > (float)Screen.height)
						{
							GUIHover.hoverRect.y = GUIHover.hoverRect.y - (GUIHover.hoverRect.y + GUIHover.hoverRect.height - (float)Screen.height);
						}
					}
					else
					{
						GUIHover.hoverRect.y = GUIHover.hoverRect.y - (GUIHover.hoverRect.height + objectPos.height);
					}
				}
				if (GUIHover.hoverRect.x < 0f)
				{
					GUIHover.hoverRect.x = 0f;
				}
				if (GUIHover.hoverRect.y < 0f)
				{
					GUIHover.hoverRect.y = 0f;
				}
				if (GUIHover.hoverRect.x + GUIHover.hoverRect.width > (float)Screen.width)
				{
					GUIHover.hoverRect.x = GUIHover.hoverRect.x - (GUIHover.hoverRect.x + GUIHover.hoverRect.width - (float)Screen.width);
				}
				GUIHover.needResize = false;
			}
			if (!GUIHover.isShowHover || GUIHover.itemText != hoverText)
			{
				if (GUIHover.lastShow != 0f && Time.time > GUIHover.lastShow + GUIHover.showOffset)
				{
					GUIHover.isShowHover = true;
					GUIHover.needResize = true;
					GUIHover.itemText = hoverText;
					GUIHover.isShowedHover = false;
					GUIHover.isShowedHover = false;
				}
				else if (GUIHover.lastShow == 0f)
				{
					GUIHover.lastShow = Time.time;
				}
			}
		}
		else if (currentEvent.type == EventType.Repaint && GUIHover.isShowHover && Time.time - GUIHover.lastCheckTime > GUIHover.hideTime)
		{
			GUIHover.isShowHover = false;
			GUIHover.needResize = true;
			GUIHover.lastShow = 0f;
		}
	}

	public static void Hover(Event currentEvent, CCItem hoverItem, Rect objectPos)
	{
		GUIHover.Hover(currentEvent, hoverItem, objectPos, 1, objectPos);
	}

	public static void Hover(Event currentEvent, CCItem hoverItem, Rect objectPos, short zIndex)
	{
		GUIHover.Hover(currentEvent, hoverItem, objectPos, zIndex, objectPos);
	}

	public static void Hover(Event currentEvent, CCItem hoverItem, Rect objectPos, Rect workingAreaRect)
	{
		GUIHover.Hover(currentEvent, hoverItem, objectPos, 1, workingAreaRect);
	}

	public static void Hover(Event currentEvent, CCItem hoverItem, Rect objectPos, short z_index, Rect workingAreaRect)
	{
		if (currentEvent.type == EventType.Repaint && z_index == GUIHover.zIndex && objectPos.Contains(currentEvent.mousePosition) && (workingAreaRect.Equals(objectPos) || workingAreaRect.Contains(GUIUtility.GUIToScreenPoint(currentEvent.mousePosition))))
		{
			if (GUIHover.hoverType != GUIHover.HoverType.HItem)
			{
				GUIHover.hoverRect = new Rect(0f, 0f, 350f, 200f);
			}
			GUIHover.hoverType = GUIHover.HoverType.HItem;
			GUIHover.lastCheckTime = Time.time;
			if (GUIHover.needResize)
			{
				GUIHover.hoverRect.x = UnityEngine.Input.mousePosition.x;
				GUIHover.hoverRect.y = (float)Screen.height - UnityEngine.Input.mousePosition.y;
				GUIHover.hoverRect.x = GUIHover.hoverRect.x + (objectPos.x - currentEvent.mousePosition.x - (GUIHover.hoverRect.width - objectPos.width) * 0.5f);
				GUIHover.hoverRect.y = GUIHover.hoverRect.y + (objectPos.y - currentEvent.mousePosition.y + objectPos.height);
				if ((float)Screen.height < GUIHover.hoverRect.y + GUIHover.hoverRect.height)
				{
					if (GUIHover.hoverRect.y - (GUIHover.hoverRect.height + objectPos.height) < 0f)
					{
						if (GUIHover.hoverRect.x - (GUIHover.hoverRect.width * 0.5f + objectPos.width * 0.5f) < 0f)
						{
							GUIHover.hoverRect.x = GUIHover.hoverRect.x + (GUIHover.hoverRect.width * 0.5f + objectPos.width * 0.5f);
						}
						else
						{
							GUIHover.hoverRect.x = GUIHover.hoverRect.x - (GUIHover.hoverRect.width * 0.5f + objectPos.width * 0.5f);
						}
						GUIHover.hoverRect.y = GUIHover.hoverRect.y - (GUIHover.hoverRect.height + objectPos.height) * 0.5f;
						if (GUIHover.hoverRect.y + GUIHover.hoverRect.height > (float)Screen.height)
						{
							GUIHover.hoverRect.y = GUIHover.hoverRect.y - (GUIHover.hoverRect.y + GUIHover.hoverRect.height - (float)Screen.height);
						}
					}
					else
					{
						GUIHover.hoverRect.y = GUIHover.hoverRect.y - (GUIHover.hoverRect.height + objectPos.height);
					}
				}
				if (GUIHover.hoverRect.x < 0f)
				{
					GUIHover.hoverRect.x = 0f;
				}
				if (GUIHover.hoverRect.y < 0f)
				{
					GUIHover.hoverRect.y = 0f;
				}
				if (GUIHover.hoverRect.x + GUIHover.hoverRect.width > (float)Screen.width)
				{
					GUIHover.hoverRect.x = GUIHover.hoverRect.x - (GUIHover.hoverRect.x + GUIHover.hoverRect.width - (float)Screen.width);
				}
				GUIHover.needResize = false;
			}
			if (!GUIHover.isShowHover)
			{
				if (GUIHover.lastShow != 0f && Time.time > GUIHover.lastShow + GUIHover.showOffset)
				{
					GUIHover.isShowHover = true;
					GUIHover.needResize = true;
					GUIHover.item = hoverItem;
					GUIHover.isShowedHover = false;
				}
				else if (GUIHover.lastShow == 0f)
				{
					GUIHover.lastShow = Time.time;
				}
			}
			else if (GUIHover.item != hoverItem)
			{
				GUIHover.isShowHover = false;
				GUIHover.lastShow = Time.time;
			}
		}
		else if (currentEvent.type == EventType.Repaint && GUIHover.isShowHover && Time.time - GUIHover.lastCheckTime > GUIHover.hideTime)
		{
			GUIHover.isShowHover = false;
			GUIHover.needResize = true;
			GUIHover.lastShow = 0f;
		}
	}

	private void DrawWear(Wear wear)
	{
		GUILayout.Label(wear.Name, "name", new GUILayoutOption[0]);
		GUILayout.Label(wear.WearType.GetName(), "itemType", new GUILayoutOption[0]);
		GUILayout.Space(3f);
		GUILayout.Label(wear.Desc, "itemDesc", new GUILayoutOption[0]);
		if (wear.DescAdditional != string.Empty)
		{
			GUILayout.Space(3f);
			GUILayout.Label(wear.DescAdditional, "itemDescAdditional", new GUILayoutOption[0]);
		}
		if (wear.NDescAdditional != string.Empty)
		{
			GUILayout.Space(3f);
			GUILayout.Label(wear.NDescAdditional, "itemNDescAdditional", new GUILayoutOption[0]);
		}
		if ((ulong)wear.NeedLvl > (ulong)((long)LocalUser.Level))
		{
			GUILayout.Space(3f);
			GUILayout.Label(LanguageManager.GetTextFormat("Unavailable: you need to be level {0}", new object[]
			{
				wear.NeedLvl
			}), "itemLowLevel", new GUILayoutOption[0]);
		}
		if (wear.Shop_Cost != null)
		{
			GUILayout.Space(9f);
			GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
			GUILayout.Label(LanguageManager.GetText("Cost:"), "costName", new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.Label(GUIContent.none, "costIcon", new GUILayoutOption[0]);
			GUILayout.Label(wear.Shop_Cost.TimePVCost.ToString(), "costValue", new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		if (wear.AssemblageRel != null)
		{
			GUILayout.Space(12f);
			GUILayout.Label(LanguageManager.GetTextFormat("Set \"{0}\":", new object[]
			{
				wear.AssemblageRel.Name
			}), "itemAssembleName", new GUILayoutOption[0]);
			GUILayout.Space(4f);
			foreach (Wear wear2 in wear.AssemblageRel.Wears)
			{
				if (LocalUser.View.IsDressed(wear2))
				{
					GUILayout.Label(string.Format("+ {0}", wear2.Name), "itemAssembleOn", new GUILayoutOption[0]);
				}
				else
				{
					GUILayout.Label(string.Format("- {0}", wear2.Name), "itemAssembleOff", new GUILayoutOption[0]);
				}
				GUILayout.Space(3f);
			}
			foreach (Weapon weapon in wear.AssemblageRel.Weapons)
			{
				if (LocalUser.WeaponSlot.IsEquip(weapon))
				{
					GUILayout.Label(string.Format("+ {0}", weapon.Name), "itemAssembleOn", new GUILayoutOption[0]);
				}
				else
				{
					GUILayout.Label(string.Format("- {0}", weapon.Name), "itemAssembleOff", new GUILayoutOption[0]);
				}
				GUILayout.Space(3f);
			}
			GUILayout.Space(2f);
			GUILayout.Label(LanguageManager.GetTextFormat("Bonus for complete set:", new object[]
			{
				wear.AssemblageRel.Name
			}), "itemAssembleName", new GUILayoutOption[0]);
			GUILayout.Space(3f);
			GUILayout.Label(wear.AssemblageRel.DescAdditional, "itemDescAdditionalSmall", new GUILayoutOption[0]);
			if (wear.AssemblageRel.NDescAdditional != string.Empty)
			{
				GUILayout.Space(3f);
				GUILayout.Label(wear.AssemblageRel.NDescAdditional, "itemNDescAdditionalSmall", new GUILayoutOption[0]);
			}
			if (wear.AssemblageRel.WeekItemRel != null)
			{
				GUILayout.Space(2f);
				GUILayout.Label(LanguageManager.GetText("Bonus Set week:"), "itemAssembleName", new GUILayoutOption[0]);
				GUILayout.Space(3f);
				GUILayout.Label(wear.AssemblageRel.WeekItemRel.Description, "itemDescAdditionalSmall", new GUILayoutOption[0]);
			}
		}
	}

	private void DrawWeapon(Weapon weapon)
	{
		GUILayout.Label(weapon.Name, "name", new GUILayoutOption[0]);
		GUILayout.Label(weapon.WeaponType.GetName(), "itemType", new GUILayoutOption[0]);
		GUILayout.Space(3f);
		if (weapon.AmmoTotal > 0u)
		{
			GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
			GUILayout.Label(LanguageManager.GetText("Ammunition"), "valueName", new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			uint num = AbilityManager.Instance.GetNewValue(GameLogic.Ability.ValueType.WeaponAmmo, weapon.AmmoTotal);
			string str = "value";
			if (num != weapon.AmmoTotal)
			{
				str = "value02";
			}
			num -= weapon.Ammo;
			if (num <= 0u)
			{
				GUILayout.Label(string.Format("{0}", weapon.Ammo), "value", new GUILayoutOption[0]);
			}
			else
			{
				GUILayout.Label(string.Format("{0} / ", weapon.Ammo), "value", new GUILayoutOption[0]);
				GUILayout.Space(2f);
				GUILayout.Label(string.Format("{0}", num), str, new GUILayoutOption[0]);
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(4f);
		}
		if (weapon.StarRapidity > 0)
		{
			GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
			GUILayout.Label(LanguageManager.GetText("Shooting rate"), "valueName", new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			for (int i = 1; i <= 5; i++)
			{
				if (i <= (int)weapon.StarRapidity)
				{
					GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("starActive"), new GUILayoutOption[0]);
				}
				else
				{
					GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("starNone"), new GUILayoutOption[0]);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(4f);
		}
		if (weapon.StarDistance > 0)
		{
			GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
			GUILayout.Label(LanguageManager.GetText("Shoot range"), "valueName", new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			for (int j = 1; j <= 5; j++)
			{
				if (j <= (int)weapon.StarDistance)
				{
					GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("starActive"), new GUILayoutOption[0]);
				}
				else
				{
					GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("starNone"), new GUILayoutOption[0]);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(4f);
		}
		if (weapon.StarDamage > 0)
		{
			GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
			GUILayout.Label(LanguageManager.GetText("Damage"), "valueName", new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			for (int k = 1; k <= 5; k++)
			{
				if (k <= (int)weapon.StarDamage)
				{
					GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("starActive"), new GUILayoutOption[0]);
				}
				else
				{
					GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("starNone"), new GUILayoutOption[0]);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(4f);
		}
		GUILayout.Space(3f);
		GUILayout.Label(weapon.Desc, "itemDesc", new GUILayoutOption[0]);
		if (weapon.DescAdditional != string.Empty)
		{
			GUILayout.Space(3f);
			GUILayout.Label(weapon.DescAdditional, "itemDescAdditional", new GUILayoutOption[0]);
		}
		if (weapon.NDescAdditional != string.Empty)
		{
			GUILayout.Space(3f);
			GUILayout.Label(weapon.NDescAdditional, "itemNDescAdditional", new GUILayoutOption[0]);
		}
		if ((ulong)weapon.NeedLvl > (ulong)((long)LocalUser.Level))
		{
			GUILayout.Space(3f);
			GUILayout.Label(LanguageManager.GetTextFormat("Unavailable: you need to be level {0}", new object[]
			{
				weapon.NeedLvl
			}), "itemLowLevel", new GUILayoutOption[0]);
		}
		if (weapon.Shop_Cost != null && !weapon.IsUpgrade)
		{
			GUILayout.Space(7f);
			GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
			GUILayout.Label(LanguageManager.GetText("Cost:"), "costName", new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.Label(GUIContent.none, "costIcon", new GUILayoutOption[0]);
			GUILayout.Label(weapon.Shop_Cost.TimePVCost.ToString(), "costValue", new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		if (weapon.AssemblageRel != null)
		{
			GUILayout.Space(12f);
			GUILayout.Label(LanguageManager.GetTextFormat("Set \"{0}\":", new object[]
			{
				weapon.AssemblageRel.Name
			}), "itemAssembleName", new GUILayoutOption[0]);
			GUILayout.Space(4f);
			foreach (Wear wear in weapon.AssemblageRel.Wears)
			{
				if (LocalUser.View.IsDressed(wear))
				{
					GUILayout.Label(string.Format("+ {0}", wear.Name), "itemAssembleOn", new GUILayoutOption[0]);
				}
				else
				{
					GUILayout.Label(string.Format("- {0}", wear.Name), "itemAssembleOff", new GUILayoutOption[0]);
				}
				GUILayout.Space(3f);
			}
			foreach (Weapon weapon2 in weapon.AssemblageRel.Weapons)
			{
				if (LocalUser.WeaponSlot.IsEquip(weapon2))
				{
					GUILayout.Label(string.Format("+ {0}", weapon2.Name), "itemAssembleOn", new GUILayoutOption[0]);
				}
				else
				{
					GUILayout.Label(string.Format("- {0}", weapon2.Name), "itemAssembleOff", new GUILayoutOption[0]);
				}
				GUILayout.Space(3f);
			}
			GUILayout.Space(2f);
			GUILayout.Label(LanguageManager.GetTextFormat("Bonus for complete set:", new object[]
			{
				weapon.AssemblageRel.Name
			}), "itemAssembleName", new GUILayoutOption[0]);
			GUILayout.Space(3f);
			GUILayout.Label(weapon.AssemblageRel.DescAdditional, "itemDescAdditionalSmall", new GUILayoutOption[0]);
			if (weapon.AssemblageRel.NDescAdditional != string.Empty)
			{
				GUILayout.Space(3f);
				GUILayout.Label(weapon.AssemblageRel.NDescAdditional, "itemNDescAdditionalSmall", new GUILayoutOption[0]);
			}
		}
		if (weapon.WeekItemRel != null)
		{
			GUILayout.Space(2f);
			GUILayout.Label(LanguageManager.GetText("Bonus Weapon week:"), "itemAssembleName", new GUILayoutOption[0]);
			GUILayout.Space(3f);
			GUILayout.Label(weapon.WeekItemRel.Description, "itemDescAdditionalSmall", new GUILayoutOption[0]);
		}
	}

	private void DrawEnhancer(Enhancer enhancer)
	{
		GUILayout.Label(enhancer.Name, "name", new GUILayoutOption[0]);
		GUILayout.Label(LanguageManager.GetText("Enhancer"), "itemType", new GUILayoutOption[0]);
		GUILayout.Space(3f);
		GUILayout.Label(enhancer.Desc, "itemDesc", new GUILayoutOption[0]);
		if (enhancer.Duration != null)
		{
			GUILayout.Label(LanguageManager.GetTextFormat("remains: {0}", new object[]
			{
				enhancer.Duration.ToString()
			}), "itemAssembleName", new GUILayoutOption[0]);
		}
		if (enhancer.IsClan && LocalUser.Clan != null && ClanManager.SelectedClan != null && ClanManager.SelectedClan.ClanID == LocalUser.Clan.ClanID && (ulong)enhancer.NeedLvl > (ulong)((long)ClanManager.SelectedClan.Level))
		{
			GUILayout.Space(3f);
			GUILayout.Label(LanguageManager.GetTextFormat("Unavailable: you need to be level {0}", new object[]
			{
				enhancer.NeedLvl
			}), "itemLowLevel", new GUILayoutOption[0]);
		}
		else if (!enhancer.IsClan && (ulong)enhancer.NeedLvl > (ulong)((long)LocalUser.Level))
		{
			GUILayout.Space(3f);
			GUILayout.Label(LanguageManager.GetTextFormat("Unavailable: you need to be level {0}", new object[]
			{
				enhancer.NeedLvl
			}), "itemLowLevel", new GUILayoutOption[0]);
		}
		if (enhancer.Shop_Cost != null)
		{
			GUILayout.Space(7f);
			if (enhancer.Shop_Cost.Time1VCost > 0u)
			{
				GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
				GUILayout.Label(LanguageManager.GetText("Cost 1 day"), "costName", new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUILayout.Label(GUIContent.none, "costIcon", new GUILayoutOption[0]);
				GUILayout.Label(enhancer.Shop_Cost.Time1VCost.ToString(), "costValue", new GUILayoutOption[]
				{
					GUILayout.MinWidth(40f)
				});
				GUILayout.EndHorizontal();
				GUILayout.Space(3f);
			}
			if (enhancer.Shop_Cost.Time7VCost > 0u)
			{
				GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
				GUILayout.Label(LanguageManager.GetText("Cost 7 day"), "costName", new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUILayout.Label(GUIContent.none, "costIcon", new GUILayoutOption[0]);
				GUILayout.Label(enhancer.Shop_Cost.Time7VCost.ToString(), "costValue", new GUILayoutOption[]
				{
					GUILayout.MinWidth(40f)
				});
				GUILayout.EndHorizontal();
				GUILayout.Space(3f);
			}
			if (enhancer.Shop_Cost.Time30VCost > 0u)
			{
				GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
				GUILayout.Label(LanguageManager.GetText("Cost 30 day"), "costName", new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUILayout.Label(GUIContent.none, "costIcon", new GUILayoutOption[0]);
				GUILayout.Label(enhancer.Shop_Cost.Time30VCost.ToString(), "costValue", new GUILayoutOption[]
				{
					GUILayout.MinWidth(40f)
				});
				GUILayout.EndHorizontal();
				GUILayout.Space(3f);
			}
			if (enhancer.Shop_Cost.TimePVCost > 0u)
			{
				GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
				GUILayout.Label(LanguageManager.GetText("Cost permanent"), "costName", new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUILayout.Label(GUIContent.none, "costIcon", new GUILayoutOption[0]);
				GUILayout.Label(enhancer.Shop_Cost.TimePVCost.ToString(), "costValue", new GUILayoutOption[]
				{
					GUILayout.MinWidth(40f)
				});
				GUILayout.EndHorizontal();
			}
		}
	}

	private void DrawTaunt(Taunt taunt)
	{
		GUILayout.Label(taunt.Name, "name", new GUILayoutOption[0]);
		GUILayout.Label(LanguageManager.GetText("Taunt"), "itemType", new GUILayoutOption[0]);
		GUILayout.Space(3f);
		GUILayout.Label(taunt.Desc, "itemDesc", new GUILayoutOption[0]);
		if (taunt.Duration != null)
		{
			GUILayout.Label(LanguageManager.GetTextFormat("remains: {0}", new object[]
			{
				taunt.Duration.ToString()
			}), "itemAssembleName", new GUILayoutOption[0]);
		}
		if ((ulong)taunt.NeedLvl > (ulong)((long)LocalUser.Level))
		{
			GUILayout.Space(3f);
			GUILayout.Label(LanguageManager.GetTextFormat("Unavailable: you need to be level {0}", new object[]
			{
				taunt.NeedLvl
			}), "itemLowLevel", new GUILayoutOption[0]);
		}
		if (taunt.Shop_Cost != null)
		{
			GUILayout.Space(7f);
			if (taunt.Shop_Cost.Time1VCost > 0u)
			{
				GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
				GUILayout.Label(LanguageManager.GetText("Cost 1 day"), "costName", new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUILayout.Label(GUIContent.none, "costIcon", new GUILayoutOption[0]);
				GUILayout.Label(taunt.Shop_Cost.Time1VCost.ToString(), "costValue", new GUILayoutOption[]
				{
					GUILayout.MinWidth(40f)
				});
				GUILayout.EndHorizontal();
				GUILayout.Space(3f);
			}
			if (taunt.Shop_Cost.Time7VCost > 0u)
			{
				GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
				GUILayout.Label(LanguageManager.GetText("Cost 7 day"), "costName", new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUILayout.Label(GUIContent.none, "costIcon", new GUILayoutOption[0]);
				GUILayout.Label(taunt.Shop_Cost.Time7VCost.ToString(), "costValue", new GUILayoutOption[]
				{
					GUILayout.MinWidth(40f)
				});
				GUILayout.EndHorizontal();
				GUILayout.Space(3f);
			}
			if (taunt.Shop_Cost.Time30VCost > 0u)
			{
				GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
				GUILayout.Label(LanguageManager.GetText("Cost 30 day"), "costName", new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUILayout.Label(GUIContent.none, "costIcon", new GUILayoutOption[0]);
				GUILayout.Label(taunt.Shop_Cost.Time30VCost.ToString(), "costValue", new GUILayoutOption[]
				{
					GUILayout.MinWidth(40f)
				});
				GUILayout.EndHorizontal();
				GUILayout.Space(3f);
			}
			if (taunt.Shop_Cost.TimePVCost > 0u)
			{
				GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
				GUILayout.Label(LanguageManager.GetText("Cost permanent"), "costName", new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUILayout.Label(GUIContent.none, "costIcon", new GUILayoutOption[0]);
				GUILayout.Label(taunt.Shop_Cost.TimePVCost.ToString(), "costValue", new GUILayoutOption[]
				{
					GUILayout.MinWidth(40f)
				});
				GUILayout.EndHorizontal();
			}
		}
	}

	public static void Hover(Event currentEvent, Achievement hoverItem, Rect objectPos)
	{
		GUIHover.Hover(currentEvent, hoverItem, objectPos, 1, objectPos);
	}

	public static void Hover(Event currentEvent, Achievement hoverItem, Rect objectPos, short zIndex)
	{
		GUIHover.Hover(currentEvent, hoverItem, objectPos, zIndex, objectPos);
	}

	public static void Hover(Event currentEvent, Achievement hoverItem, Rect objectPos, Rect workingAreaRect)
	{
		GUIHover.Hover(currentEvent, hoverItem, objectPos, 1, workingAreaRect);
	}

	public static void Hover(Event currentEvent, Achievement hoverItem, Rect objectPos, short z_index, Rect workingAreaRect)
	{
		if (currentEvent.type == EventType.Repaint && z_index == GUIHover.zIndex && objectPos.Contains(currentEvent.mousePosition) && (workingAreaRect.Equals(objectPos) || workingAreaRect.Contains(GUIUtility.GUIToScreenPoint(currentEvent.mousePosition))))
		{
			if (GUIHover.hoverType != GUIHover.HoverType.HItem)
			{
				GUIHover.hoverRect = new Rect(0f, 0f, 350f, 200f);
			}
			GUIHover.hoverType = GUIHover.HoverType.HItem;
			GUIHover.lastCheckTime = Time.time;
			if (GUIHover.needResize)
			{
				GUIHover.hoverRect.x = UnityEngine.Input.mousePosition.x;
				GUIHover.hoverRect.y = (float)Screen.height - UnityEngine.Input.mousePosition.y;
				GUIHover.hoverRect.x = GUIHover.hoverRect.x + (objectPos.x - currentEvent.mousePosition.x - (GUIHover.hoverRect.width - objectPos.width) * 0.5f);
				GUIHover.hoverRect.y = GUIHover.hoverRect.y + (objectPos.y - currentEvent.mousePosition.y + objectPos.height);
				if ((float)Screen.height < GUIHover.hoverRect.y + GUIHover.hoverRect.height)
				{
					if (GUIHover.hoverRect.y - (GUIHover.hoverRect.height + objectPos.height) < 0f)
					{
						if (GUIHover.hoverRect.x - (GUIHover.hoverRect.width * 0.5f + objectPos.width * 0.5f) < 0f)
						{
							GUIHover.hoverRect.x = GUIHover.hoverRect.x + (GUIHover.hoverRect.width * 0.5f + objectPos.width * 0.5f);
						}
						else
						{
							GUIHover.hoverRect.x = GUIHover.hoverRect.x - (GUIHover.hoverRect.width * 0.5f + objectPos.width * 0.5f);
						}
						GUIHover.hoverRect.y = GUIHover.hoverRect.y - (GUIHover.hoverRect.height + objectPos.height) * 0.5f;
						if (GUIHover.hoverRect.y + GUIHover.hoverRect.height > (float)Screen.height)
						{
							GUIHover.hoverRect.y = GUIHover.hoverRect.y - (GUIHover.hoverRect.y + GUIHover.hoverRect.height - (float)Screen.height);
						}
					}
					else
					{
						GUIHover.hoverRect.y = GUIHover.hoverRect.y - (GUIHover.hoverRect.height + objectPos.height);
					}
				}
				if (GUIHover.hoverRect.x < 0f)
				{
					GUIHover.hoverRect.x = 0f;
				}
				if (GUIHover.hoverRect.y < 0f)
				{
					GUIHover.hoverRect.y = 0f;
				}
				if (GUIHover.hoverRect.x + GUIHover.hoverRect.width > (float)Screen.width)
				{
					GUIHover.hoverRect.x = GUIHover.hoverRect.x - (GUIHover.hoverRect.x + GUIHover.hoverRect.width - (float)Screen.width);
				}
				GUIHover.needResize = false;
			}
			if (!GUIHover.isShowHover)
			{
				if (GUIHover.lastShow != 0f && Time.time > GUIHover.lastShow + GUIHover.showOffset)
				{
					GUIHover.isShowHover = true;
					GUIHover.needResize = true;
					GUIHover.item = hoverItem;
					GUIHover.isShowedHover = false;
				}
				else if (GUIHover.lastShow == 0f)
				{
					GUIHover.lastShow = Time.time;
				}
			}
			else if (GUIHover.item != hoverItem)
			{
				GUIHover.isShowHover = false;
				GUIHover.lastShow = Time.time;
			}
		}
		else if (currentEvent.type == EventType.Repaint && GUIHover.isShowHover && Time.time - GUIHover.lastCheckTime > GUIHover.hideTime)
		{
			GUIHover.isShowHover = false;
			GUIHover.needResize = true;
			GUIHover.lastShow = 0f;
		}
	}

	private void DrawAchievement(Achievement achievement)
	{
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.Label(achievement.Name, GUISkinManager.Text.GetStyle("normal01"), new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		for (int i = 1; i <= achievement.MaxLevel; i++)
		{
			if (i <= achievement.Level)
			{
				GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("starActive"), new GUILayoutOption[0]);
			}
			else
			{
				GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("starNone"), new GUILayoutOption[0]);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.Label(achievement.Description, GUISkinManager.Text.GetStyle("normal02"), new GUILayoutOption[0]);
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.Label(LanguageManager.GetTextFormat("{0} step reward:", new object[]
		{
			achievement.Level
		}), GUISkinManager.Text.GetStyle("normal03"), new GUILayoutOption[0]);
		GUILayout.Space(4f);
		GUILayout.Label(achievement.Reward.ToString(), GUISkinManager.Label.GetStyle("costSmall"), new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
	}

	private void OnGUI()
	{
		GUISkin skin = GUI.skin;
		GUI.skin = GUISkinManager.Hover;
		GUI.depth = 0;
		if (GUIHover.hoverType != GUIHover.HoverType.HNone && GUIHover.enable && GUIHover.isShowHover && GUIHover.isShowedHover)
		{
			try
			{
				GUILayout.BeginArea(GUIHover.hoverRect, GUIContent.none, GUIStyle.none);
				GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical(GUIContent.none, "backgound", new GUILayoutOption[0]);
				if (GUIHover.hoverType == GUIHover.HoverType.HItem)
				{
					if (GUIHover.item.GetType() == typeof(Wear))
					{
						this.DrawWear(GUIHover.item as Wear);
					}
					else if (GUIHover.item.GetType() == typeof(Weapon))
					{
						this.DrawWeapon(GUIHover.item as Weapon);
					}
					else if (GUIHover.item.GetType() == typeof(Achievement))
					{
						this.DrawAchievement(GUIHover.item as Achievement);
					}
					else if (GUIHover.item.GetType() == typeof(Taunt))
					{
						this.DrawTaunt(GUIHover.item as Taunt);
					}
					else if (GUIHover.item.GetType() == typeof(Enhancer))
					{
						this.DrawEnhancer(GUIHover.item as Enhancer);
					}
				}
				else if (GUIHover.hoverType == GUIHover.HoverType.HString)
				{
					GUILayout.Label(GUIHover.itemText, "string", new GUILayoutOption[0]);
				}
				else if (GUIHover.hoverType == GUIHover.HoverType.HContent)
				{
					GUILayout.Label(GUIHover.itemContent, new GUILayoutOption[0]);
				}
				GUILayout.EndVertical();
				GUIHover.ResizeHover(GUILayoutUtility.GetLastRect());
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
			catch (ArgumentException message)
			{
				UnityEngine.Debug.LogWarning(message);
			}
		}
		GUI.skin = skin;
		if (GUIHover.isShowHover && !GUIHover.isShowedHover)
		{
			GUIHover.isShowedHover = true;
		}
		else if (!GUIHover.isShowHover && GUIHover.isShowedHover)
		{
			GUIHover.isShowedHover = false;
		}
	}

	private static void ResizeHover(Rect newSize)
	{
		if (newSize.height > 20f && newSize.height != GUIHover.hoverRect.height)
		{
			GUIHover.hoverRect.height = newSize.height;
			GUIHover.needResize = true;
		}
	}

	public static void DebugInfo()
	{
		UnityEngine.Debug.Log("[DebugGUIHover] Enable        = " + GUIHover.Enable);
		UnityEngine.Debug.Log("[DebugGUIHover] zIndex        = " + GUIHover.zIndex);
		UnityEngine.Debug.Log("[DebugGUIHover] hideTime      = " + GUIHover.hideTime);
		UnityEngine.Debug.Log("[DebugGUIHover] lastCheckTime = " + GUIHover.lastCheckTime);
		UnityEngine.Debug.Log("[DebugGUIHover] showOffset    = " + GUIHover.showOffset);
		UnityEngine.Debug.Log("[DebugGUIHover] lastShow      = " + GUIHover.lastShow);
	}

	private static short zIndex = 1;

	private static object item = null;

	private static string itemText = string.Empty;

	private static GUIContent itemContent = GUIContent.none;

	private static GUIHover.HoverType hoverType = GUIHover.HoverType.HNone;

	private static Rect hoverRect = new Rect(0f, 0f, 350f, 200f);

	private static bool needResize = true;

	private static bool isShowHover = false;

	private static bool isShowedHover = false;

	private static float hideTime = 0.3f;

	private static float lastCheckTime = 0f;

	private static float showOffset = 0.8f;

	private static float lastShow = 0f;

	private static bool enable = true;

	private enum HoverType
	{
		HNone,
		HString,
		HItem,
		HContent
	}
}
