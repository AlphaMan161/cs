// dnSpy decompiler from Assembly-CSharp.dll class: GUIInventory
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIInventory : MonoBehaviour
{
	private static CCItem Draged_item
	{
		get
		{
			return GUIInventory.draged_item;
		}
		set
		{
			GUIInventory.draged_item = value;
			if (GUIInventory.draged_item == null)
			{
				GUIHover.Enable = true;
			}
			else
			{
				GUIHover.Enable = false;
			}
		}
	}

	private static void Init()
	{
		if (!GUIInventory.IsInit)
		{
			GUIInventory.IsInit = true;
			CharacterCameraManager.Instance.SetPlayerViewDefault(LocalUser.View);
			GUIInventory.menu_wears.Clear();
			if (Inventory.Instance.Wears.FindAll((Wear x) => x.WearType == CCWearType.Hats).Count > 0)
			{
				GUIInventory.menu_wears.Add(CCWearType.Hats);
				if (GUIInventory.menu_wear_type_seleted == CCWearType.None)
				{
					GUIInventory.menu_wear_type_seleted = CCWearType.Hats;
				}
			}
			if (Inventory.Instance.Wears.FindAll((Wear x) => x.WearType == CCWearType.Masks).Count > 0)
			{
				GUIInventory.menu_wears.Add(CCWearType.Masks);
				if (GUIInventory.menu_wear_type_seleted == CCWearType.None)
				{
					GUIInventory.menu_wear_type_seleted = CCWearType.Masks;
				}
			}
			if (Inventory.Instance.Wears.FindAll((Wear x) => x.WearType == CCWearType.Gloves).Count > 0)
			{
				GUIInventory.menu_wears.Add(CCWearType.Gloves);
				if (GUIInventory.menu_wear_type_seleted == CCWearType.None)
				{
					GUIInventory.menu_wear_type_seleted = CCWearType.Gloves;
				}
			}
			if (Inventory.Instance.Wears.FindAll((Wear x) => x.WearType == CCWearType.Shirts).Count > 0)
			{
				GUIInventory.menu_wears.Add(CCWearType.Shirts);
				if (GUIInventory.menu_wear_type_seleted == CCWearType.None)
				{
					GUIInventory.menu_wear_type_seleted = CCWearType.Shirts;
				}
			}
			if (Inventory.Instance.Wears.FindAll((Wear x) => x.WearType == CCWearType.Pants).Count > 0)
			{
				GUIInventory.menu_wears.Add(CCWearType.Pants);
				if (GUIInventory.menu_wear_type_seleted == CCWearType.None)
				{
					GUIInventory.menu_wear_type_seleted = CCWearType.Pants;
				}
			}
			if (Inventory.Instance.Wears.FindAll((Wear x) => x.WearType == CCWearType.Boots).Count > 0)
			{
				GUIInventory.menu_wears.Add(CCWearType.Boots);
				if (GUIInventory.menu_wear_type_seleted == CCWearType.None)
				{
					GUIInventory.menu_wear_type_seleted = CCWearType.Boots;
				}
			}
			if (Inventory.Instance.Wears.FindAll((Wear x) => x.WearType == CCWearType.Backpacks).Count > 0)
			{
				GUIInventory.menu_wears.Add(CCWearType.Backpacks);
				if (GUIInventory.menu_wear_type_seleted == CCWearType.None)
				{
					GUIInventory.menu_wear_type_seleted = CCWearType.Backpacks;
				}
			}
			if (Inventory.Instance.Wears.FindAll((Wear x) => x.WearType == CCWearType.Others).Count > 0)
			{
				GUIInventory.menu_wears.Add(CCWearType.Others);
				if (GUIInventory.menu_wear_type_seleted == CCWearType.None)
				{
					GUIInventory.menu_wear_type_seleted = CCWearType.Others;
				}
			}
			if (Inventory.Instance.Wears.FindAll((Wear x) => x.WearType == CCWearType.Heads).Count > 0)
			{
				GUIInventory.menu_wears.Add(CCWearType.Heads);
				if (GUIInventory.menu_wear_type_seleted == CCWearType.None)
				{
					GUIInventory.menu_wear_type_seleted = CCWearType.Heads;
				}
			}
			GUIInventory.ApplyWearFilter(GUIInventory.menu_wear_type_seleted);
			GUIInventory.menu_weapons.Clear();
			if (Inventory.Instance.Weapons.FindAll((Weapon x) => x.WeaponSlot == 1).Count > 0)
			{
				GUIInventory.menu_weapons.Add(1u);
				if (GUIInventory.menu_weapon_slot_selected == 0u)
				{
					GUIInventory.menu_weapon_slot_selected = 1u;
				}
			}
			if (Inventory.Instance.Weapons.FindAll((Weapon x) => x.WeaponSlot == 2).Count > 0)
			{
				GUIInventory.menu_weapons.Add(2u);
				if (GUIInventory.menu_weapon_slot_selected == 0u)
				{
					GUIInventory.menu_weapon_slot_selected = 2u;
				}
			}
			if (Inventory.Instance.Weapons.FindAll((Weapon x) => x.WeaponSlot == 3).Count > 0)
			{
				GUIInventory.menu_weapons.Add(3u);
				if (GUIInventory.menu_weapon_slot_selected == 0u)
				{
					GUIInventory.menu_weapon_slot_selected = 3u;
				}
			}
			if (Inventory.Instance.Weapons.FindAll((Weapon x) => x.WeaponSlot == 4).Count > 0)
			{
				GUIInventory.menu_weapons.Add(4u);
				if (GUIInventory.menu_weapon_slot_selected == 0u)
				{
					GUIInventory.menu_weapon_slot_selected = 4u;
				}
			}
			if (Inventory.Instance.Weapons.FindAll((Weapon x) => x.WeaponSlot == 5).Count > 0)
			{
				GUIInventory.menu_weapons.Add(5u);
				if (GUIInventory.menu_weapon_slot_selected == 0u)
				{
					GUIInventory.menu_weapon_slot_selected = 5u;
				}
			}
			if (Inventory.Instance.Weapons.FindAll((Weapon x) => x.WeaponSlot == 6).Count > 0)
			{
				GUIInventory.menu_weapons.Add(6u);
				if (GUIInventory.menu_weapon_slot_selected == 0u)
				{
					GUIInventory.menu_weapon_slot_selected = 6u;
				}
			}
			if (Inventory.Instance.Weapons.FindAll((Weapon x) => x.WeaponSlot == 7).Count > 0)
			{
				GUIInventory.menu_weapons.Add(7u);
				if (GUIInventory.menu_weapon_slot_selected == 0u)
				{
					GUIInventory.menu_weapon_slot_selected = 7u;
				}
			}
			GUIInventory.ApplyWeaponFilter(GUIInventory.menu_weapon_slot_selected);
		}
	}

	private static void ApplyWearFilter(CCWearType cwtype)
	{
		GUIInventory.menu_wear_type_seleted = cwtype;
		GUIInventory.selectedWearsList = Inventory.Instance.Wears.FindAll((Wear x) => x.WearType == GUIInventory.menu_wear_type_seleted);
	}

	private static void ApplyWeaponFilter(uint cwtype)
	{
		GUIInventory.menu_weapon_slot_selected = cwtype;
		GUIInventory.selectedWeaponList = Inventory.Instance.Weapons.FindAll((Weapon x) => (long)x.WeaponSlot == (long)((ulong)GUIInventory.menu_weapon_slot_selected));
	}

	public static void OnGUI()
	{
		GUIInventory.Init();
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width((float)Screen.width)
		});
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuRow03"), new GUILayoutOption[]
		{
			GUILayout.Height(41f)
		});
		if (GUILayout.Button(LanguageManager.GetText("Appearance"), GUISkinManager.Button.GetStyle((MenuSelecter.HeadquaterMenuSelect != MenuSelecter.HeadquaterMenuEnum.Appearance) ? "menuRow03" : "menuRow03active"), new GUILayoutOption[]
		{
			GUILayout.Height(31f)
		}))
		{
			MenuSelecter.HeadquaterMenuSelect = MenuSelecter.HeadquaterMenuEnum.Appearance;
		}
		if (GUILayout.Button(LanguageManager.GetText("Weapons"), GUISkinManager.Button.GetStyle((MenuSelecter.HeadquaterMenuSelect != MenuSelecter.HeadquaterMenuEnum.Weapon) ? "menuRow03" : "menuRow03active"), new GUILayoutOption[]
		{
			GUILayout.Height(31f)
		}))
		{
			MenuSelecter.HeadquaterMenuSelect = MenuSelecter.HeadquaterMenuEnum.Weapon;
		}
		if (GUILayout.Button(LanguageManager.GetText("Taunts"), GUISkinManager.Button.GetStyle((MenuSelecter.HeadquaterMenuSelect != MenuSelecter.HeadquaterMenuEnum.Taunt) ? "menuRow03" : "menuRow03active"), new GUILayoutOption[]
		{
			GUILayout.Height(31f)
		}))
		{
			MenuSelecter.HeadquaterMenuSelect = MenuSelecter.HeadquaterMenuEnum.Taunt;
		}
		if (GUILayout.Button(LanguageManager.GetText("Practice hall"), GUISkinManager.Button.GetStyle((MenuSelecter.HeadquaterMenuSelect != MenuSelecter.HeadquaterMenuEnum.Ability) ? "menuRow03" : "menuRow03active"), new GUILayoutOption[]
		{
			GUILayout.Height(31f)
		}))
		{
			MenuSelecter.HeadquaterMenuSelect = MenuSelecter.HeadquaterMenuEnum.Ability;
		}
		if (GUILayout.Button(LanguageManager.GetText("Workshop"), GUISkinManager.Button.GetStyle((MenuSelecter.HeadquaterMenuSelect != MenuSelecter.HeadquaterMenuEnum.Workshop) ? "menuRow03" : "menuRow03active"), new GUILayoutOption[]
		{
			GUILayout.Height(31f)
		}))
		{
			MenuSelecter.HeadquaterMenuSelect = MenuSelecter.HeadquaterMenuEnum.Workshop;
		}
		if (GUILayout.Button(LanguageManager.GetText("Change name"), GUISkinManager.Button.GetStyle("menuRow03"), new GUILayoutOption[]
		{
			GUILayout.Height(31f)
		}))
		{
			NotificationWindow.Add(new Notification(Notification.Type.SET_NAME, LanguageManager.GetText("CHANGE NAME"), string.Empty, string.Empty, null, null)
			{
				WindowSize = new Vector2(500f, 343f)
			});
		}
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Ability)
		{
			GUIAbility.OnGUI();
			return;
		}
		if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Workshop)
		{
			GUIWorkshop.OnGUI();
			return;
		}
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.MinWidth(760f)
		});
		if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Appearance)
		{
			GUILayout.Space(5f);
			if (GUIInventory.menu_wear_type_seleted == CCWearType.Heads)
			{
				GUIInventory.DrawHeadSlots();
			}
			else
			{
				GUIInventory.DrawWearSlots();
			}
			GUILayout.Space(8f);
		}
		else
		{
			GUILayout.FlexibleSpace();
		}
		if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Weapon || MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Taunt)
		{
			GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain2"), new GUILayoutOption[]
			{
				GUILayout.Width(334f),
				GUILayout.Height(335f)
			});
		}
		else
		{
			GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), new GUILayoutOption[]
			{
				GUILayout.Width(334f),
				GUILayout.Height(454f)
			});
		}
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.MinHeight(55f)
		});
		if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Weapon)
		{
			GUILayout.Space(8f);
			foreach (uint num in GUIInventory.menu_weapons)
			{
				if (num == GUIInventory.menu_weapon_slot_selected)
				{
					GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive3"), new GUILayoutOption[0]);
					GUILayout.Label(GUIContent.none, GUISkinManager.PartsWeapon.GetStyle("ws" + num + "Active"), new GUILayoutOption[0]);
					GUIHover.Hover(Event.current, LanguageManager.GetText("ws_name_" + num), GUILayoutUtility.GetLastRect());
					GUILayout.EndHorizontal();
				}
				else
				{
					if (GUILayout.Button(GUIContent.none, GUISkinManager.PartsWeapon.GetStyle("ws" + num), new GUILayoutOption[0]))
					{
						GUIInventory.ApplyWeaponFilter(num);
					}
					GUIHover.Hover(Event.current, LanguageManager.GetText("ws_name_" + num), GUILayoutUtility.GetLastRect());
				}
			}
			GUILayout.Space(5f);
		}
		else if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Appearance)
		{
			GUILayout.Space(12f);
			foreach (CCWearType ccwearType in GUIInventory.menu_wears)
			{
				if (ccwearType == GUIInventory.menu_wear_type_seleted)
				{
					GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"), new GUILayoutOption[0]);
					GUILayout.Label(GUIContent.none, GUISkinManager.PartsGear.GetStyle("part" + ccwearType + "Active"), new GUILayoutOption[0]);
					GUIHover.Hover(Event.current, ccwearType.GetName(), GUILayoutUtility.GetLastRect());
					GUILayout.EndHorizontal();
				}
				else
				{
					if (GUILayout.Button(GUIContent.none, GUISkinManager.PartsGear.GetStyle("part" + ccwearType), new GUILayoutOption[0]))
					{
						GUIInventory.ApplyWearFilter(ccwearType);
					}
					GUIHover.Hover(Event.current, ccwearType.GetName(), GUILayoutUtility.GetLastRect());
				}
				GUILayout.Space(5f);
			}
		}
		else
		{
			GUILayout.Space(8f);
			GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive3"), new GUILayoutOption[0]);
			GUILayout.Label(GUIContent.none, GUISkinManager.PartsGear.GetStyle("partTauntActive"), new GUILayoutOption[0]);
			GUIHover.Hover(Event.current, LanguageManager.GetText("Taunts"), GUILayoutUtility.GetLastRect());
			GUILayout.EndHorizontal();
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(3f);
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.Space(4f);
		if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Weapon)
		{
			GUIInventory.debug_menu_GearVector = GUILayout.BeginScrollView(GUIInventory.debug_menu_GearVector, false, true, new GUILayoutOption[]
			{
				GUILayout.Height(260f)
			});
			if (GUIInventory.selectedWeaponList.Count == 0)
			{
				GUILayout.Label(LanguageManager.GetText("Currently you have the basic weapons set. To buy new weapon you need to go to the \"SHOP\""), GUISkinManager.Text.GetStyle("noneBattle"), new GUILayoutOption[]
				{
					GUILayout.Height(260f)
				});
			}
			else
			{
				int num2 = 0;
				int count = GUIInventory.selectedWeaponList.Count;
				foreach (CCItem ccitem in GUIInventory.selectedWeaponList)
				{
					if (num2 % 3 == 0)
					{
						GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
						GUILayout.Space(3f);
					}
					if (GUIInventory.Draged_item == null)
					{
						GUILayout.Box(ccitem.Ico, GUISkinManager.Backgound.GetStyle("itemRight02"), new GUILayoutOption[]
						{
							GUILayout.Width(97f),
							GUILayout.Height(85f)
						});
					}
					else
					{
						GUILayout.Label(ccitem.Ico, GUISkinManager.Backgound.GetStyle("itemRight02"), new GUILayoutOption[]
						{
							GUILayout.Width(97f),
							GUILayout.Height(85f)
						});
					}
					Rect lastRect = GUILayoutUtility.GetLastRect();
					GUIHover.Hover(Event.current, ccitem, lastRect);
					GUILayout.Space(3f);
					if (num2 % 3 == 2 || num2 == count - 1)
					{
						GUILayout.EndHorizontal();
						GUILayout.Space(3f);
					}
					num2++;
				}
			}
			GUILayout.EndScrollView();
		}
		else if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Appearance)
		{
			GUIInventory.debug_menu_GearVector = GUILayout.BeginScrollView(GUIInventory.debug_menu_GearVector, false, true, new GUILayoutOption[]
			{
				GUILayout.Height(379f)
			});
			if (GUIInventory.selectedWearsList.Count == 0)
			{
				GUILayout.Label(LanguageManager.GetText("Currently you have the basic weapons set. To buy new weapon you need to go to the \"SHOP\""), GUISkinManager.Text.GetStyle("noneBattle"), new GUILayoutOption[]
				{
					GUILayout.Height(370f)
				});
			}
			else
			{
				int num3 = 0;
				int count2 = GUIInventory.selectedWearsList.Count;
				foreach (CCItem ccitem2 in GUIInventory.selectedWearsList)
				{
					if (num3 % 3 == 0)
					{
						GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
						GUILayout.Space(3f);
					}
					if (GUIInventory.Draged_item == null)
					{
						GUILayout.Box(ccitem2.Ico, GUISkinManager.Backgound.GetStyle("itemRight"), new GUILayoutOption[]
						{
							GUILayout.Width(97f),
							GUILayout.Height(85f)
						});
					}
					else
					{
						GUILayout.Label(ccitem2.Ico, GUISkinManager.Backgound.GetStyle("itemRight"), new GUILayoutOption[]
						{
							GUILayout.Width(97f),
							GUILayout.Height(85f)
						});
					}
					Rect lastRect2 = GUILayoutUtility.GetLastRect();
					GUIHover.Hover(Event.current, ccitem2, lastRect2);
					GUILayout.Space(3f);
					if (num3 % 3 == 2 || num3 == count2 - 1)
					{
						GUILayout.EndHorizontal();
						GUILayout.Space(3f);
					}
					num3++;
				}
			}
			GUILayout.EndScrollView();
		}
		else if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Taunt)
		{
			GUIInventory.debug_menu_GearVector = GUILayout.BeginScrollView(GUIInventory.debug_menu_GearVector, false, true, new GUILayoutOption[]
			{
				GUILayout.Height(260f)
			});
			if (Inventory.Instance.Taunts.Count == 0)
			{
				GUILayout.Label(LanguageManager.GetText("Currently you don't have any active Taunts. To buy some please go to the \"SHOP\"."), GUISkinManager.Text.GetStyle("noneBattle"), new GUILayoutOption[]
				{
					GUILayout.Height(260f)
				});
			}
			else
			{
				int num4 = 0;
				int count3 = Inventory.Instance.Taunts.Count;
				foreach (CCItem ccitem3 in Inventory.Instance.Taunts)
				{
					if (num4 % 3 == 0)
					{
						GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
						GUILayout.Space(3f);
					}
					if (GUIInventory.Draged_item == null)
					{
						GUILayout.Box(ccitem3.Ico, GUISkinManager.Backgound.GetStyle("itemRight02"), new GUILayoutOption[]
						{
							GUILayout.Width(97f),
							GUILayout.Height(85f)
						});
					}
					else
					{
						GUILayout.Label(ccitem3.Ico, GUISkinManager.Backgound.GetStyle("itemRight02"), new GUILayoutOption[]
						{
							GUILayout.Width(97f),
							GUILayout.Height(85f)
						});
					}
					Rect lastRect3 = GUILayoutUtility.GetLastRect();
					if (GUI.Button(new Rect(lastRect3.x + 5f, lastRect3.y + 5f, 24f, 17f), GUIContent.none, GUISkinManager.Button.GetStyle("play")))
					{
						LocalUser.TauntSlot.Play(ccitem3 as Taunt);
					}
					GUIHover.Hover(Event.current, ccitem3, lastRect3);
					GUILayout.Space(3f);
					if (num4 % 3 == 2 || num4 == count3 - 1)
					{
						GUILayout.EndHorizontal();
						GUILayout.Space(3f);
					}
					num4++;
				}
			}
			GUILayout.EndScrollView();
		}
		if (Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_items_scroll_rect = GUILayoutUtility.GetLastRect();
		}
		GUILayout.Space(6f);
		GUILayout.EndHorizontal();
		GUILayout.Space(4f);
		GUILayout.EndVertical();
		GUILayout.Space(10f);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Weapon)
		{
			GUILayout.Space(3f);
			GUIInventory.DrawWeaponSlots();
		}
		else if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Taunt)
		{
			GUILayout.Space(3f);
			GUIInventory.DrawTauntSlots();
		}
		if (Event.current.type == EventType.MouseDown && GUIInventory.draged_items_scroll_rect.Contains(Event.current.mousePosition))
		{
			Vector2 zero = Vector2.zero;
			zero.x = Event.current.mousePosition.x - GUIInventory.draged_items_scroll_rect.x + GUIInventory.debug_menu_GearVector.x;
			zero.y = Event.current.mousePosition.y - GUIInventory.draged_items_scroll_rect.y + GUIInventory.debug_menu_GearVector.y;
			int num5 = (int)(zero.x / GUIInventory.debug_draged_itemSize.x);
			int num6 = (int)(zero.y / GUIInventory.debug_draged_itemSize.y);
			if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Weapon && num6 * 3 + num5 < GUIInventory.selectedWeaponList.Count)
			{
				GUIInventory.Draged_item = GUIInventory.selectedWeaponList[num6 * 3 + num5];
			}
			else if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Appearance && num6 * 3 + num5 < GUIInventory.selectedWearsList.Count)
			{
				GUIInventory.Draged_item = GUIInventory.selectedWearsList[num6 * 3 + num5];
			}
			else if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Taunt && num6 * 3 + num5 < Inventory.Instance.Taunts.Count)
			{
				Taunt taunt = Inventory.Instance.Taunts[num6 * 3 + num5];
				if (!LocalUser.TauntSlot.IsSet(taunt))
				{
					GUIInventory.Draged_item = taunt;
				}
			}
		}
		if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Appearance)
		{
			if (GUIInventory.menu_wear_type_seleted == CCWearType.Heads && LocalUser.View.Head != null && Event.current.type == EventType.MouseDown && GUIInventory.draged_view_head.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.View.Head;
			}
			else if (GUIInventory.menu_wear_type_seleted != CCWearType.Heads && LocalUser.View.Hat != null && Event.current.type == EventType.MouseDown && GUIInventory.draged_view_hat.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.View.Hat;
			}
			else if (GUIInventory.menu_wear_type_seleted != CCWearType.Heads && LocalUser.View.Mask != null && Event.current.type == EventType.MouseDown && GUIInventory.draged_view_mask.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.View.Mask;
			}
			else if (GUIInventory.menu_wear_type_seleted != CCWearType.Heads && LocalUser.View.Gloves != null && Event.current.type == EventType.MouseDown && GUIInventory.draged_view_gloves.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.View.Gloves;
			}
			else if (GUIInventory.menu_wear_type_seleted != CCWearType.Heads && LocalUser.View.Shirt != null && Event.current.type == EventType.MouseDown && GUIInventory.draged_view_shirt.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.View.Shirt;
			}
			else if (GUIInventory.menu_wear_type_seleted != CCWearType.Heads && LocalUser.View.Pants != null && Event.current.type == EventType.MouseDown && GUIInventory.draged_view_pants.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.View.Pants;
			}
			else if (GUIInventory.menu_wear_type_seleted != CCWearType.Heads && LocalUser.View.Boots != null && Event.current.type == EventType.MouseDown && GUIInventory.draged_view_boots.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.View.Boots;
			}
			else if (GUIInventory.menu_wear_type_seleted != CCWearType.Heads && LocalUser.View.Backpack != null && Event.current.type == EventType.MouseDown && GUIInventory.draged_view_backpack.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.View.Backpack;
			}
			else if (GUIInventory.menu_wear_type_seleted != CCWearType.Heads && LocalUser.View.Other != null && Event.current.type == EventType.MouseDown && GUIInventory.draged_view_other.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.View.Other;
			}
		}
		else if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Weapon)
		{
			if (LocalUser.WeaponSlot.WeaponID1 != 0 && Event.current.type == EventType.MouseDown && GUIInventory.draged_slot_weapon1.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.WeaponSlot.Weapon1;
			}
			else if (LocalUser.WeaponSlot.WeaponID2 != 0 && Event.current.type == EventType.MouseDown && GUIInventory.draged_slot_weapon2.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.WeaponSlot.Weapon2;
			}
			else if (LocalUser.WeaponSlot.WeaponID3 != 0 && Event.current.type == EventType.MouseDown && GUIInventory.draged_slot_weapon3.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.WeaponSlot.Weapon3;
			}
			else if (LocalUser.WeaponSlot.WeaponID4 != 0 && Event.current.type == EventType.MouseDown && GUIInventory.draged_slot_weapon4.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.WeaponSlot.Weapon4;
			}
			else if (LocalUser.WeaponSlot.WeaponID5 != 0 && Event.current.type == EventType.MouseDown && GUIInventory.draged_slot_weapon5.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.WeaponSlot.Weapon5;
			}
			else if (LocalUser.WeaponSlot.WeaponID6 != 0 && Event.current.type == EventType.MouseDown && GUIInventory.draged_slot_weapon6.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.WeaponSlot.Weapon6;
			}
			else if (LocalUser.WeaponSlot.WeaponID7 != 0 && Event.current.type == EventType.MouseDown && GUIInventory.draged_slot_weapon7.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.WeaponSlot.Weapon7;
			}
		}
		else if (MenuSelecter.HeadquaterMenuSelect == MenuSelecter.HeadquaterMenuEnum.Taunt)
		{
			if (LocalUser.TauntSlot.TauntID0 != 0 && Event.current.type == EventType.MouseDown && GUIInventory.draged_slot_taunt0.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.TauntSlot.Taunt0;
			}
			else if (LocalUser.TauntSlot.TauntID1 != 0 && Event.current.type == EventType.MouseDown && GUIInventory.draged_slot_taunt1.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.TauntSlot.Taunt1;
			}
			else if (LocalUser.TauntSlot.TauntID2 != 0 && Event.current.type == EventType.MouseDown && GUIInventory.draged_slot_taunt2.Contains(Event.current.mousePosition))
			{
				GUIInventory.Draged_item = LocalUser.TauntSlot.Taunt2;
			}
		}
		if (GUIInventory.Draged_item != null)
		{
			GUI.Label(new Rect(Event.current.mousePosition.x - GUIInventory.debug_draged_itemSize.x * 0.5f, Event.current.mousePosition.y - GUIInventory.debug_draged_itemSize.y * 0.5f, GUIInventory.debug_draged_itemSize.x, GUIInventory.debug_draged_itemSize.y), GUIInventory.Draged_item.Ico, GUIStyle.none);
		}
		if (Event.current.type == EventType.MouseUp && GUIInventory.Draged_item != null)
		{
			if (GUIInventory.draged_item_rect.Contains(Event.current.mousePosition) || GUIInventory.debug_drag_droped_rect_global.Contains(Event.current.mousePosition))
			{
				if (GUIInventory.Draged_item.GetType() == typeof(Wear))
				{
					LocalUser.View.DressUp(GUIInventory.Draged_item as Wear);
				}
				else if (GUIInventory.Draged_item.GetType() == typeof(Weapon))
				{
					LocalUser.WeaponSlot.Set(GUIInventory.Draged_item as Weapon);
				}
			}
			else if (GUIInventory.draged_items_scroll_rect.Contains(Event.current.mousePosition))
			{
				if (GUIInventory.Draged_item.GetType() == typeof(Wear) && LocalUser.View.IsDressed(GUIInventory.Draged_item as Wear))
				{
					LocalUser.View.UnDress(GUIInventory.Draged_item as Wear);
				}
				else if (GUIInventory.Draged_item.GetType() == typeof(Weapon))
				{
					LocalUser.WeaponSlot.UnSet(GUIInventory.Draged_item as Weapon);
				}
			}
			if (GUIInventory.Draged_item.GetType() == typeof(Taunt))
			{
				if (GUIInventory.draged_slot_taunt0.Contains(Event.current.mousePosition))
				{
					LocalUser.TauntSlot.Set(GUIInventory.Draged_item as Taunt, 0);
				}
				else if (GUIInventory.draged_slot_taunt1.Contains(Event.current.mousePosition))
				{
					LocalUser.TauntSlot.Set(GUIInventory.Draged_item as Taunt, 1);
				}
				else if (GUIInventory.draged_slot_taunt2.Contains(Event.current.mousePosition))
				{
					LocalUser.TauntSlot.Set(GUIInventory.Draged_item as Taunt, 2);
				}
				else if (GUIInventory.draged_items_scroll_rect.Contains(Event.current.mousePosition))
				{
					LocalUser.TauntSlot.UnSet(GUIInventory.Draged_item as Taunt);
				}
			}
			GUIInventory.Draged_item = null;
		}
	}

	private static void DrawWeaponSlots()
	{
		int num = 0;
		if (GUIInventory.Draged_item != null && typeof(Weapon) == GUIInventory.Draged_item.GetType())
		{
			num = (GUIInventory.Draged_item as Weapon).WeaponSlot;
		}
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(707f)
		});
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(101f)
		});
		GUILayout.Label(TRInput.Weapon1.ToDisplayString(), GUISkinManager.Backgound.GetStyle("partWeaponNum"), new GUILayoutOption[]
		{
			GUILayout.Height(26f)
		});
		GUILayout.Label(LocalUser.WeaponSlot.Weapon1.Ico, GUISkinManager.Backgound.GetStyle((num != 1) ? "itemLeft2" : "itemLeft2Active"), new GUILayoutOption[]
		{
			GUILayout.Width(101f),
			GUILayout.Height(90f)
		});
		GUIHover.Hover(Event.current, LocalUser.WeaponSlot.Weapon1, GUILayoutUtility.GetLastRect());
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_slot_weapon1 = GUILayoutUtility.GetLastRect();
		}
		if (num == 1 && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUIInventory.draged_slot_weapon1;
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(101f)
		});
		GUILayout.Label(TRInput.Weapon2.ToDisplayString(), GUISkinManager.Backgound.GetStyle("partWeaponNum"), new GUILayoutOption[]
		{
			GUILayout.Height(26f)
		});
		GUILayout.Label(LocalUser.WeaponSlot.Weapon2.Ico, GUISkinManager.Backgound.GetStyle((num != 2) ? "itemLeft2" : "itemLeft2Active"), new GUILayoutOption[]
		{
			GUILayout.Width(101f),
			GUILayout.Height(90f)
		});
		GUIHover.Hover(Event.current, LocalUser.WeaponSlot.Weapon2, GUILayoutUtility.GetLastRect());
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_slot_weapon2 = GUILayoutUtility.GetLastRect();
		}
		if (num == 2 && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUIInventory.draged_slot_weapon2;
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(101f)
		});
		GUILayout.Label(TRInput.Weapon3.ToDisplayString(), GUISkinManager.Backgound.GetStyle("partWeaponNum"), new GUILayoutOption[]
		{
			GUILayout.Height(26f)
		});
		GUILayout.Label(LocalUser.WeaponSlot.Weapon3.Ico, GUISkinManager.Backgound.GetStyle((num != 3) ? "itemLeft2" : "itemLeft2Active"), new GUILayoutOption[]
		{
			GUILayout.Width(101f),
			GUILayout.Height(90f)
		});
		GUIHover.Hover(Event.current, LocalUser.WeaponSlot.Weapon3, GUILayoutUtility.GetLastRect());
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_slot_weapon3 = GUILayoutUtility.GetLastRect();
		}
		if (num == 3 && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUIInventory.draged_slot_weapon3;
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(101f)
		});
		GUILayout.Label(TRInput.Weapon4.ToDisplayString(), GUISkinManager.Backgound.GetStyle("partWeaponNum"), new GUILayoutOption[]
		{
			GUILayout.Height(26f)
		});
		GUILayout.Label(LocalUser.WeaponSlot.Weapon4.Ico, GUISkinManager.Backgound.GetStyle((num != 4) ? "itemLeft2" : "itemLeft2Active"), new GUILayoutOption[]
		{
			GUILayout.Width(101f),
			GUILayout.Height(90f)
		});
		GUIHover.Hover(Event.current, LocalUser.WeaponSlot.Weapon4, GUILayoutUtility.GetLastRect());
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_slot_weapon4 = GUILayoutUtility.GetLastRect();
		}
		if (num == 4 && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUIInventory.draged_slot_weapon4;
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(101f)
		});
		GUILayout.Label(TRInput.Weapon5.ToDisplayString(), GUISkinManager.Backgound.GetStyle("partWeaponNum"), new GUILayoutOption[]
		{
			GUILayout.Height(26f)
		});
		GUILayout.Label(LocalUser.WeaponSlot.Weapon5.Ico, GUISkinManager.Backgound.GetStyle((num != 5) ? "itemLeft2" : "itemLeft2Active"), new GUILayoutOption[]
		{
			GUILayout.Width(101f),
			GUILayout.Height(90f)
		});
		GUIHover.Hover(Event.current, LocalUser.WeaponSlot.Weapon5, GUILayoutUtility.GetLastRect());
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_slot_weapon5 = GUILayoutUtility.GetLastRect();
		}
		if (num == 5 && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUIInventory.draged_slot_weapon5;
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(101f)
		});
		GUILayout.Label(TRInput.Weapon6.ToDisplayString(), GUISkinManager.Backgound.GetStyle("partWeaponNum"), new GUILayoutOption[]
		{
			GUILayout.Height(26f)
		});
		GUILayout.Label(LocalUser.WeaponSlot.Weapon6.Ico, GUISkinManager.Backgound.GetStyle((num != 6) ? "itemLeft2" : "itemLeft2Active"), new GUILayoutOption[]
		{
			GUILayout.Width(101f),
			GUILayout.Height(90f)
		});
		GUIHover.Hover(Event.current, LocalUser.WeaponSlot.Weapon6, GUILayoutUtility.GetLastRect());
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_slot_weapon6 = GUILayoutUtility.GetLastRect();
		}
		if (num == 6 && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUIInventory.draged_slot_weapon6;
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(101f)
		});
		GUILayout.Label(TRInput.Weapon7.ToDisplayString(), GUISkinManager.Backgound.GetStyle("partWeaponNum"), new GUILayoutOption[]
		{
			GUILayout.Height(26f)
		});
		GUILayout.Label(LocalUser.WeaponSlot.Weapon7.Ico, GUISkinManager.Backgound.GetStyle((num != 7) ? "itemLeft2" : "itemLeft2Active"), new GUILayoutOption[]
		{
			GUILayout.Width(101f),
			GUILayout.Height(90f)
		});
		GUIHover.Hover(Event.current, LocalUser.WeaponSlot.Weapon7, GUILayoutUtility.GetLastRect());
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_slot_weapon7 = GUILayoutUtility.GetLastRect();
		}
		if (num == 7 && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUIInventory.draged_slot_weapon7;
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private static void DrawTauntSlots()
	{
		int num = -1;
		if (GUIInventory.Draged_item != null && typeof(Taunt) == GUIInventory.Draged_item.GetType())
		{
			if (LocalUser.TauntSlot.Taunt0 == null)
			{
				num = 0;
			}
			else if (LocalUser.TauntSlot.Taunt1 == null)
			{
				num = 1;
			}
			else if (LocalUser.TauntSlot.Taunt2 == null)
			{
				num = 2;
			}
		}
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(707f)
		});
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(101f)
		});
		GUILayout.Label(TRInput.Taunt1.ToDisplayString(), GUISkinManager.Backgound.GetStyle("partWeaponNum"), new GUILayoutOption[]
		{
			GUILayout.Height(26f)
		});
		GUILayout.Label((LocalUser.TauntSlot.Taunt0 == null) ? Taunt.GetEmptyTexture() : LocalUser.TauntSlot.Taunt0.Ico, GUISkinManager.Backgound.GetStyle((num != 0) ? "itemLeft2" : "itemLeft2Active"), new GUILayoutOption[]
		{
			GUILayout.Width(101f),
			GUILayout.Height(90f)
		});
		if (LocalUser.TauntSlot.Taunt0 != null)
		{
			GUIHover.Hover(Event.current, LocalUser.TauntSlot.Taunt0, GUILayoutUtility.GetLastRect());
		}
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_slot_taunt0 = GUILayoutUtility.GetLastRect();
		}
		if (num == 0 && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUIInventory.draged_slot_taunt0;
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(101f)
		});
		GUILayout.Label(TRInput.Taunt2.ToDisplayString(), GUISkinManager.Backgound.GetStyle("partWeaponNum"), new GUILayoutOption[]
		{
			GUILayout.Height(26f)
		});
		GUILayout.Label((LocalUser.TauntSlot.Taunt1 == null) ? Taunt.GetEmptyTexture() : LocalUser.TauntSlot.Taunt1.Ico, GUISkinManager.Backgound.GetStyle((num != 1) ? "itemLeft2" : "itemLeft2Active"), new GUILayoutOption[]
		{
			GUILayout.Width(101f),
			GUILayout.Height(90f)
		});
		if (LocalUser.TauntSlot.Taunt1 != null)
		{
			GUIHover.Hover(Event.current, LocalUser.TauntSlot.Taunt1, GUILayoutUtility.GetLastRect());
		}
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_slot_taunt1 = GUILayoutUtility.GetLastRect();
		}
		if (num == 1 && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUIInventory.draged_slot_taunt1;
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(101f)
		});
		GUILayout.Label(TRInput.Taunt3.ToDisplayString(), GUISkinManager.Backgound.GetStyle("partWeaponNum"), new GUILayoutOption[]
		{
			GUILayout.Height(26f)
		});
		GUILayout.Label((LocalUser.TauntSlot.Taunt2 == null) ? Taunt.GetEmptyTexture() : LocalUser.TauntSlot.Taunt2.Ico, GUISkinManager.Backgound.GetStyle((num != 2) ? "itemLeft2" : "itemLeft2Active"), new GUILayoutOption[]
		{
			GUILayout.Width(101f),
			GUILayout.Height(90f)
		});
		if (LocalUser.TauntSlot.Taunt2 != null)
		{
			GUIHover.Hover(Event.current, LocalUser.TauntSlot.Taunt2, GUILayoutUtility.GetLastRect());
		}
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_slot_taunt2 = GUILayoutUtility.GetLastRect();
		}
		if (num == 2 && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUIInventory.draged_slot_taunt2;
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private static void DrawHeadSlots()
	{
		CCWearType ccwearType = CCWearType.None;
		if (GUIInventory.Draged_item != null && typeof(Wear) == GUIInventory.Draged_item.GetType())
		{
			ccwearType = (GUIInventory.Draged_item as Wear).WearType;
		}
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.MinHeight(423f)
		});
		GUILayout.Space(11f);
		GUILayout.Label((LocalUser.View.Head == null) ? Wear.GetEmptyTexture(CCWearType.Heads) : LocalUser.View.Head.Ico, GUISkinManager.Backgound.GetStyle((ccwearType != CCWearType.Heads) ? "itemLeft" : "itemLeftActive"), new GUILayoutOption[]
		{
			GUILayout.Width(105f),
			GUILayout.Height(93f)
		});
		if (LocalUser.View.Head != null)
		{
			GUIHover.Hover(Event.current, LocalUser.View.Head, GUILayoutUtility.GetLastRect());
		}
		if (ccwearType == CCWearType.Heads && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUILayoutUtility.GetLastRect();
		}
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_view_head = GUILayoutUtility.GetLastRect();
		}
		GUILayout.EndVertical();
		if (Event.current.type == EventType.Repaint)
		{
			GUIInventory.debug_drag_droped_rect_global = GUILayoutUtility.GetLastRect();
			GUIInventory.debug_drag_droped_rect_global.x = GUIInventory.debug_drag_droped_rect_global.x - 180f;
			GUIInventory.debug_drag_droped_rect_global.width = 165f;
		}
	}

	private static void DrawWearSlots()
	{
		CCWearType ccwearType = CCWearType.None;
		if (GUIInventory.Draged_item != null && typeof(Wear) == GUIInventory.Draged_item.GetType())
		{
			ccwearType = (GUIInventory.Draged_item as Wear).WearType;
		}
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.Space(11f);
		GUILayout.Label((LocalUser.View.Hat == null) ? Wear.GetEmptyTexture(CCWearType.Hats) : LocalUser.View.Hat.Ico, GUISkinManager.Backgound.GetStyle((ccwearType != CCWearType.Hats) ? "itemLeft" : "itemLeftActive"), new GUILayoutOption[]
		{
			GUILayout.Width(105f),
			GUILayout.Height(93f)
		});
		if (LocalUser.View.Hat != null)
		{
			GUIHover.Hover(Event.current, LocalUser.View.Hat, GUILayoutUtility.GetLastRect());
		}
		if (ccwearType == CCWearType.Hats && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUILayoutUtility.GetLastRect();
		}
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_view_hat = GUILayoutUtility.GetLastRect();
		}
		GUILayout.Space(8f);
		GUILayout.Label((LocalUser.View.Shirt == null) ? Wear.GetEmptyTexture(CCWearType.Shirts) : LocalUser.View.Shirt.Ico, GUISkinManager.Backgound.GetStyle((ccwearType != CCWearType.Shirts) ? "itemLeft" : "itemLeftActive"), new GUILayoutOption[]
		{
			GUILayout.Width(105f),
			GUILayout.Height(93f)
		});
		if (LocalUser.View.Shirt != null)
		{
			GUIHover.Hover(Event.current, LocalUser.View.Shirt, GUILayoutUtility.GetLastRect());
		}
		if (ccwearType == CCWearType.Shirts && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUILayoutUtility.GetLastRect();
		}
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_view_shirt = GUILayoutUtility.GetLastRect();
		}
		GUILayout.Space(8f);
		GUILayout.Label((LocalUser.View.Gloves == null) ? Wear.GetEmptyTexture(CCWearType.Gloves) : LocalUser.View.Gloves.Ico, GUISkinManager.Backgound.GetStyle((ccwearType != CCWearType.Gloves) ? "itemLeft" : "itemLeftActive"), new GUILayoutOption[]
		{
			GUILayout.Width(105f),
			GUILayout.Height(93f)
		});
		if (LocalUser.View.Gloves != null)
		{
			GUIHover.Hover(Event.current, LocalUser.View.Gloves, GUILayoutUtility.GetLastRect());
		}
		if (ccwearType == CCWearType.Gloves && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUILayoutUtility.GetLastRect();
		}
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_view_gloves = GUILayoutUtility.GetLastRect();
		}
		GUILayout.Space(8f);
		GUILayout.Label((LocalUser.View.Pants == null) ? Wear.GetEmptyTexture(CCWearType.Pants) : LocalUser.View.Pants.Ico, GUISkinManager.Backgound.GetStyle((ccwearType != CCWearType.Pants) ? "itemLeft" : "itemLeftActive"), new GUILayoutOption[]
		{
			GUILayout.Width(105f),
			GUILayout.Height(93f)
		});
		if (LocalUser.View.Pants != null)
		{
			GUIHover.Hover(Event.current, LocalUser.View.Pants, GUILayoutUtility.GetLastRect());
		}
		if (ccwearType == CCWearType.Pants && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUILayoutUtility.GetLastRect();
		}
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_view_pants = GUILayoutUtility.GetLastRect();
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.Space(11f);
		GUILayout.Label((LocalUser.View.Mask == null) ? Wear.GetEmptyTexture(CCWearType.Masks) : LocalUser.View.Mask.Ico, GUISkinManager.Backgound.GetStyle((ccwearType != CCWearType.Masks) ? "itemLeft" : "itemLeftActive"), new GUILayoutOption[]
		{
			GUILayout.Width(105f),
			GUILayout.Height(93f)
		});
		if (LocalUser.View.Mask != null)
		{
			GUIHover.Hover(Event.current, LocalUser.View.Mask, GUILayoutUtility.GetLastRect());
		}
		if (ccwearType == CCWearType.Masks && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUILayoutUtility.GetLastRect();
		}
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_view_mask = GUILayoutUtility.GetLastRect();
		}
		GUILayout.Space(8f);
		GUILayout.Label((LocalUser.View.Backpack == null) ? Wear.GetEmptyTexture(CCWearType.Backpacks) : LocalUser.View.Backpack.Ico, GUISkinManager.Backgound.GetStyle((ccwearType != CCWearType.Backpacks) ? "itemLeft" : "itemLeftActive"), new GUILayoutOption[]
		{
			GUILayout.Width(105f),
			GUILayout.Height(93f)
		});
		if (LocalUser.View.Backpack != null)
		{
			GUIHover.Hover(Event.current, LocalUser.View.Backpack, GUILayoutUtility.GetLastRect());
		}
		if (ccwearType == CCWearType.Backpacks && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUILayoutUtility.GetLastRect();
		}
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_view_backpack = GUILayoutUtility.GetLastRect();
		}
		GUILayout.Space(8f);
		GUILayout.Label((LocalUser.View.Other == null) ? Wear.GetEmptyTexture(CCWearType.Others) : LocalUser.View.Other.Ico, GUISkinManager.Backgound.GetStyle((ccwearType != CCWearType.Others) ? "itemLeft" : "itemLeftActive"), new GUILayoutOption[]
		{
			GUILayout.Width(105f),
			GUILayout.Height(93f)
		});
		if (LocalUser.View.Other != null)
		{
			GUIHover.Hover(Event.current, LocalUser.View.Other, GUILayoutUtility.GetLastRect());
		}
		if (ccwearType == CCWearType.Others && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUILayoutUtility.GetLastRect();
		}
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_view_other = GUILayoutUtility.GetLastRect();
		}
		GUILayout.Space(8f);
		GUILayout.Label((LocalUser.View.Boots == null) ? Wear.GetEmptyTexture(CCWearType.Boots) : LocalUser.View.Boots.Ico, GUISkinManager.Backgound.GetStyle((ccwearType != CCWearType.Boots) ? "itemLeft" : "itemLeftActive"), new GUILayoutOption[]
		{
			GUILayout.Width(105f),
			GUILayout.Height(93f)
		});
		if (LocalUser.View.Boots != null)
		{
			GUIHover.Hover(Event.current, LocalUser.View.Boots, GUILayoutUtility.GetLastRect());
		}
		if (ccwearType == CCWearType.Boots && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_item_rect = GUILayoutUtility.GetLastRect();
		}
		if (GUIInventory.Draged_item == null && Event.current.type == EventType.Repaint)
		{
			GUIInventory.draged_view_boots = GUILayoutUtility.GetLastRect();
		}
		GUILayout.EndVertical();
		if (Event.current.type == EventType.Repaint)
		{
			GUIInventory.debug_drag_droped_rect_global = GUILayoutUtility.GetLastRect();
			GUIInventory.debug_drag_droped_rect_global.x = GUIInventory.debug_drag_droped_rect_global.x - 180f;
			GUIInventory.debug_drag_droped_rect_global.width = 165f;
		}
	}

	private static Rect draged_items_scroll_rect = new Rect(0f, 0f, 0f, 0f);

	private static Rect debug_draged_rect = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_item_rect = new Rect(0f, 0f, 0f, 0f);

	private static Rect debug_drag_droped_rect_global = new Rect(0f, 0f, 0f, 0f);

	private static CCItem draged_item = null;

	private static int debug_droped_item = -1;

	private static Vector2 debug_draged_itemSize = new Vector2(97f, 85f);

	private static Vector2 debug_menu_GearVector = new Vector2(0f, 0f);

	private static Rect draged_view_hat = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_view_mask = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_view_gloves = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_view_shirt = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_view_pants = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_view_boots = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_view_backpack = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_view_other = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_view_head = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_slot_weapon1 = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_slot_weapon2 = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_slot_weapon3 = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_slot_weapon4 = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_slot_weapon5 = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_slot_weapon6 = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_slot_weapon7 = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_slot_taunt0 = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_slot_taunt1 = new Rect(0f, 0f, 0f, 0f);

	private static Rect draged_slot_taunt2 = new Rect(0f, 0f, 0f, 0f);

	public static bool IsInit = false;

	private static List<Wear> selectedWearsList = Inventory.Instance.Wears;

	private static List<CCWearType> menu_wears = new List<CCWearType>();

	private static CCWearType menu_wear_type_seleted = CCWearType.None;

	private static List<Weapon> selectedWeaponList = Inventory.Instance.Weapons;

	private static List<uint> menu_weapons = new List<uint>();

	private static uint menu_weapon_slot_selected = 0u;
}
