// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIShop : MonoBehaviour
{
    private static Rect draged_items_scroll_rect = new Rect(0f, 0f, 0f, 0f);

    private static Rect draged_item_rect = new Rect(0f, 0f, 0f, 0f);

    private static Rect debug_drag_droped_rect_global = new Rect(0f, 0f, 0f, 0f);

    private static CCItem draged_item = null;

    private static int debug_droped_item = -1;

    private static Vector2 draged_items_scroll_itemSize = new Vector2(97f, 85f);

    private static Vector2 debug_menu_GearVector = new Vector2(0f, 0f);

    private static PlayerView preview = new PlayerView();

    private static Rect draged_view_hat = new Rect(0f, 0f, 0f, 0f);

    private static Rect draged_view_mask = new Rect(0f, 0f, 0f, 0f);

    private static Rect draged_view_gloves = new Rect(0f, 0f, 0f, 0f);

    private static Rect draged_view_shirt = new Rect(0f, 0f, 0f, 0f);

    private static Rect draged_view_pants = new Rect(0f, 0f, 0f, 0f);

    private static Rect draged_view_boots = new Rect(0f, 0f, 0f, 0f);

    private static Rect draged_view_backpack = new Rect(0f, 0f, 0f, 0f);

    private static Rect draged_view_other = new Rect(0f, 0f, 0f, 0f);

    private static Rect draged_view_head = new Rect(0f, 0f, 0f, 0f);

    private static List<Wear> selectedWearsList = ShopManager.Instance.Wears;

    private static List<CCWearType> menu_wears = new List<CCWearType>();

    private static CCWearType menu_wear_type_seleted = CCWearType.Hats;

    private static List<Weapon> selectedWeaponList = Inventory.Instance.Weapons;

    private static List<uint> menu_weapons = new List<uint>();

    private static uint menu_weapon_slot_selected = 1u;

    public static bool IsInit = false;

    private static CCItem Draged_item
    {
        get
        {
            return GUIShop.draged_item;
        }
        set
        {
            GUIShop.draged_item = value;
            if (GUIShop.draged_item == null)
            {
                GUIHover.Enable = true;
            }
            else
            {
                GUIHover.Enable = false;
            }
        }
    }

    public static void ClearSet()
    {
        GUIShop.preview = new PlayerView();
    }

    private static void Init()
    {
        if (!GUIShop.IsInit)
        {
            GUIShop.IsInit = true;
            GUIShop.ClearSet();
            CharacterCameraManager.Instance.SetPlayerViewAdditional(GUIShop.preview);
            GUIShop.ApplyWearFilter(GUIShop.menu_wear_type_seleted);
            GUIShop.menu_wears.Clear();
            GUIShop.menu_wears.Add(CCWearType.Hats);
            GUIShop.menu_wears.Add(CCWearType.Masks);
            GUIShop.menu_wears.Add(CCWearType.Gloves);
            GUIShop.menu_wears.Add(CCWearType.Shirts);
            GUIShop.menu_wears.Add(CCWearType.Pants);
            GUIShop.menu_wears.Add(CCWearType.Boots);
            GUIShop.menu_wears.Add(CCWearType.Backpacks);
            GUIShop.menu_wears.Add(CCWearType.Others);
            if (ShopManager.Instance.WearHeads.Count > 0)
            {
                GUIShop.menu_wears.Add(CCWearType.Heads);
            }
            GUIShop.ApplyWeaponFilter(GUIShop.menu_weapon_slot_selected);
            GUIShop.menu_weapons.Clear();
            GUIShop.menu_weapons.Add(1u);
            GUIShop.menu_weapons.Add(2u);
            GUIShop.menu_weapons.Add(3u);
            GUIShop.menu_weapons.Add(4u);
            GUIShop.menu_weapons.Add(5u);
            GUIShop.menu_weapons.Add(6u);
            GUIShop.menu_weapons.Add(7u);
        }
    }

    public static void OnGUI()
    {
        GUIShop.Init();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width((float)Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuRow03"), GUILayout.Height(41f));
        if (GUILayout.Button(LanguageManager.GetText("Appearance"), GUISkinManager.Button.GetStyle((MenuSelecter.ShopMenuSelect != MenuSelecter.ShopMenuEnum.Appearance) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.ShopMenuSelect = MenuSelecter.ShopMenuEnum.Appearance;
        }
        if (GUILayout.Button(LanguageManager.GetText("Weapons"), GUISkinManager.Button.GetStyle((MenuSelecter.ShopMenuSelect != MenuSelecter.ShopMenuEnum.Weapon) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.ShopMenuSelect = MenuSelecter.ShopMenuEnum.Weapon;
        }
        if (GUILayout.Button(LanguageManager.GetText("Taunts"), GUISkinManager.Button.GetStyle((MenuSelecter.ShopMenuSelect != MenuSelecter.ShopMenuEnum.Taunt) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.ShopMenuSelect = MenuSelecter.ShopMenuEnum.Taunt;
        }
        if (GUILayout.Button(LanguageManager.GetText("Enhancers"), GUISkinManager.Button.GetStyle((MenuSelecter.ShopMenuSelect != MenuSelecter.ShopMenuEnum.Enhancer) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.ShopMenuSelect = MenuSelecter.ShopMenuEnum.Enhancer;
        }
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(760f));
        if (MenuSelecter.ShopMenuSelect == MenuSelecter.ShopMenuEnum.Appearance)
        {
            GUILayout.Space(5f);
            if (GUIShop.menu_wear_type_seleted == CCWearType.Heads)
            {
                GUIShop.DrawBarbershopSlots();
            }
            else
            {
                GUIShop.DrawWearSlots();
            }
            GUILayout.Space(8f);
        }
        else
        {
            GUILayout.FlexibleSpace();
        }
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle((MenuSelecter.ShopMenuSelect != MenuSelecter.ShopMenuEnum.Appearance) ? "winMain2" : "winMain"), GUILayout.Width(334f), GUILayout.Height(454f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.MinHeight(55f));
        if (MenuSelecter.ShopMenuSelect == MenuSelecter.ShopMenuEnum.Weapon)
        {
            GUILayout.Space(8f);
            List<uint>.Enumerator enumerator = GUIShop.menu_weapons.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    uint current = enumerator.Current;
                    if (current == GUIShop.menu_weapon_slot_selected)
                    {
                        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive3"));
                        GUILayout.Label(GUIContent.none, GUISkinManager.PartsWeapon.GetStyle("ws" + current + "Active"));
                        GUIHover.Hover(Event.current, LanguageManager.GetText("ws_name_" + current), GUILayoutUtility.GetLastRect());
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        if (GUILayout.Button(GUIContent.none, GUISkinManager.PartsWeapon.GetStyle("ws" + current)))
                        {
                            GUIShop.ApplyWeaponFilter(current);
                        }
                        GUIHover.Hover(Event.current, LanguageManager.GetText("ws_name_" + current), GUILayoutUtility.GetLastRect());
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            GUILayout.Space(5f);
        }
        else if (MenuSelecter.ShopMenuSelect == MenuSelecter.ShopMenuEnum.Appearance)
        {
            GUILayout.Space(12f);
            List<CCWearType>.Enumerator enumerator2 = GUIShop.menu_wears.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    CCWearType current2 = enumerator2.Current;
                    if (current2 == GUIShop.menu_wear_type_seleted)
                    {
                        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
                        GUILayout.Label(GUIContent.none, GUISkinManager.PartsGear.GetStyle("part" + current2 + "Active"));
                        GUIHover.Hover(Event.current, current2.GetName(), GUILayoutUtility.GetLastRect());
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        if (GUILayout.Button(GUIContent.none, GUISkinManager.PartsGear.GetStyle("part" + current2)))
                        {
                            GUIShop.ApplyWearFilter(current2);
                        }
                        GUIHover.Hover(Event.current, current2.GetName(), GUILayoutUtility.GetLastRect());
                    }
                    GUILayout.Space(5f);
                }
            }
            finally
            {
                ((IDisposable)enumerator2).Dispose();
            }
        }
        else if (MenuSelecter.ShopMenuSelect == MenuSelecter.ShopMenuEnum.Taunt)
        {
            GUILayout.Space(8f);
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive3"));
            GUILayout.Label(GUIContent.none, GUISkinManager.PartsGear.GetStyle("partTauntActive"));
            GUIHover.Hover(Event.current, LanguageManager.GetText("Taunts"), GUILayoutUtility.GetLastRect());
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.Space(8f);
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive3"));
            GUILayout.Label(GUIContent.none, GUISkinManager.PartsGear.GetStyle("partEnhancerActive"));
            GUIHover.Hover(Event.current, LanguageManager.GetText("Enhancers"), GUILayoutUtility.GetLastRect());
            GUILayout.EndHorizontal();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(3f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(4f);
        GUIShop.debug_menu_GearVector = GUILayout.BeginScrollView(GUIShop.debug_menu_GearVector, false, true, GUILayout.Height(379f));
        if (MenuSelecter.ShopMenuSelect == MenuSelecter.ShopMenuEnum.Appearance)
        {
            int num = 0;
            int count = GUIShop.selectedWearsList.Count;
            List<Wear>.Enumerator enumerator3 = GUIShop.selectedWearsList.GetEnumerator();
            try
            {
                while (enumerator3.MoveNext())
                {
                    CCItem current3 = enumerator3.Current;
                    if (num % 3 == 0)
                    {
                        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                        GUILayout.Space(3f);
                    }
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(97f));
                    if (GUIShop.Draged_item == null && !GUIShop.preview.IsDressed(current3 as Wear))
                    {
                        GUILayout.Box(current3.Ico, GUISkinManager.Backgound.GetStyle("itemShop01"), GUILayout.Width(97f), GUILayout.Height(85f));
                    }
                    else
                    {
                        GUILayout.Label(current3.Ico, GUISkinManager.Backgound.GetStyle("itemShop01"), GUILayout.Width(97f), GUILayout.Height(85f));
                    }
                    GUILayout.EndVertical();
                    Rect lastRect = GUILayoutUtility.GetLastRect();
                    if (current3.IsSale)
                    {
                        GUI.Label(new Rect(lastRect.x, lastRect.y, 33f, 29f), GUIContent.none, GUISkinManager.Ico.GetStyle("sale"));
                    }
                    lastRect.height -= 27f;
                    GUIHover.Hover(Event.current, current3, lastRect);
                    lastRect.x += 68f;
                    lastRect.y += 62f;
                    lastRect.width = 24f;
                    lastRect.height = 17f;
                    if (current3.IsBuyed)
                    {
                        lastRect.width = 24f;
                        lastRect.height = 23f;
                        lastRect.y -= 6f;
                        GUI.Label(lastRect, GUIContent.none, GUISkinManager.Label.GetStyle("isBuyed"));
                        lastRect.y += 6f;
                    }
                    else if (current3.NeedLvl > LocalUser.Level)
                    {
                        lastRect.width = 24f;
                        lastRect.height = 23f;
                        lastRect.y -= 6f;
                        GUI.Label(lastRect, GUIContent.none, GUISkinManager.Label.GetStyle("isDisabled"));
                        lastRect.y += 6f;
                    }
                    else if (GUI.Button(lastRect, GUIContent.none, GUISkinManager.Button.GetStyle("buy")))
                    {
                        GUIShop.OnBuy(current3);
                    }
                    lastRect.x -= 64f;
                    lastRect.width = 60f;
                    lastRect.y -= 1f;
                    GUI.Label(lastRect, current3.Shop_Cost.TimePVCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                    GUILayout.Space(3f);
                    if (num % 3 == 2 || num == count - 1)
                    {
                        GUILayout.EndHorizontal();
                        GUILayout.Space(3f);
                    }
                    num++;
                }
            }
            finally
            {
                ((IDisposable)enumerator3).Dispose();
            }
        }
        else if (MenuSelecter.ShopMenuSelect == MenuSelecter.ShopMenuEnum.Weapon)
        {
            int num2 = 0;
            int count2 = GUIShop.selectedWeaponList.Count;
            List<Weapon>.Enumerator enumerator4 = GUIShop.selectedWeaponList.GetEnumerator();
            try
            {
                while (enumerator4.MoveNext())
                {
                    CCItem current4 = enumerator4.Current;
                    if (num2 % 3 == 0)
                    {
                        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                        GUILayout.Space(3f);
                    }
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(97f));
                    if (GUIShop.Draged_item == null && !LocalUser.WeaponSlot.IsEquip(current4 as Weapon))
                    {
                        GUILayout.Box(current4.Ico, GUISkinManager.Backgound.GetStyle("itemShop02"), GUILayout.Width(97f), GUILayout.Height(85f));
                    }
                    else
                    {
                        GUILayout.Label(current4.Ico, GUISkinManager.Backgound.GetStyle("itemShop02"), GUILayout.Width(97f), GUILayout.Height(85f));
                    }
                    GUILayout.EndVertical();
                    Rect lastRect2 = GUILayoutUtility.GetLastRect();
                    if (current4.IsSale)
                    {
                        GUI.Label(new Rect(lastRect2.x, lastRect2.y, 33f, 29f), GUIContent.none, GUISkinManager.Ico.GetStyle("sale"));
                    }
                    lastRect2.height -= 27f;
                    GUIHover.Hover(Event.current, current4, lastRect2);
                    lastRect2.x += 68f;
                    lastRect2.y += 62f;
                    lastRect2.width = 24f;
                    lastRect2.height = 17f;
                    if (current4.IsBuyed)
                    {
                        lastRect2.width = 24f;
                        lastRect2.height = 23f;
                        lastRect2.y -= 6f;
                        GUI.Label(lastRect2, GUIContent.none, GUISkinManager.Label.GetStyle("isBuyed"));
                        lastRect2.y += 6f;
                    }
                    else if (current4.NeedLvl > LocalUser.Level)
                    {
                        lastRect2.width = 24f;
                        lastRect2.height = 23f;
                        lastRect2.y -= 6f;
                        GUI.Label(lastRect2, GUIContent.none, GUISkinManager.Label.GetStyle("isDisabled"));
                        lastRect2.y += 6f;
                    }
                    else if (GUI.Button(lastRect2, GUIContent.none, GUISkinManager.Button.GetStyle("buy")))
                    {
                        GUIShop.OnBuy(current4);
                    }
                    lastRect2.x -= 64f;
                    lastRect2.width = 60f;
                    lastRect2.y -= 1f;
                    GUI.Label(lastRect2, current4.Shop_Cost.TimePVCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                    GUILayout.Space(3f);
                    if (num2 % 3 == 2 || num2 == count2 - 1)
                    {
                        GUILayout.EndHorizontal();
                        GUILayout.Space(3f);
                    }
                    num2++;
                }
            }
            finally
            {
                ((IDisposable)enumerator4).Dispose();
            }
        }
        else if (MenuSelecter.ShopMenuSelect == MenuSelecter.ShopMenuEnum.Taunt)
        {
            int num3 = 0;
            int count3 = ShopManager.Instance.Taunts.Count;
            List<Taunt>.Enumerator enumerator5 = ShopManager.Instance.Taunts.GetEnumerator();
            try
            {
                while (enumerator5.MoveNext())
                {
                    CCItem current5 = enumerator5.Current;
                    if (num3 % 3 == 0)
                    {
                        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                        GUILayout.Space(3f);
                    }
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(97f));
                    GUILayout.Label(current5.Ico, GUISkinManager.Backgound.GetStyle("itemShop02"), GUILayout.Width(97f), GUILayout.Height(85f));
                    GUILayout.EndVertical();
                    Rect lastRect3 = GUILayoutUtility.GetLastRect();
                    if (current5.IsSale)
                    {
                        GUI.Label(new Rect(lastRect3.x, lastRect3.y, 33f, 29f), GUIContent.none, GUISkinManager.Ico.GetStyle("sale"));
                    }
                    if (GUI.Button(new Rect(lastRect3.x + 5f, lastRect3.y + 5f, 24f, 17f), GUIContent.none, GUISkinManager.Button.GetStyle("play")))
                    {
                        LocalUser.TauntSlot.Play(current5 as Taunt);
                    }
                    lastRect3.height -= 27f;
                    GUIHover.Hover(Event.current, current5, lastRect3);
                    lastRect3.x += 68f;
                    lastRect3.y += 62f;
                    lastRect3.width = 24f;
                    lastRect3.height = 17f;
                    if (current5.IsBuyed)
                    {
                        lastRect3.width = 24f;
                        lastRect3.height = 23f;
                        lastRect3.y -= 6f;
                        GUI.Label(lastRect3, GUIContent.none, GUISkinManager.Label.GetStyle("isBuyed"));
                        lastRect3.y += 6f;
                    }
                    else if (current5.NeedLvl > LocalUser.Level)
                    {
                        lastRect3.width = 24f;
                        lastRect3.height = 23f;
                        lastRect3.y -= 6f;
                        GUI.Label(lastRect3, GUIContent.none, GUISkinManager.Label.GetStyle("isDisabled"));
                        lastRect3.y += 6f;
                    }
                    else if (GUI.Button(lastRect3, GUIContent.none, GUISkinManager.Button.GetStyle("buy")))
                    {
                        GUIShop.OnBuy(current5);
                    }
                    lastRect3.x -= 64f;
                    lastRect3.width = 60f;
                    lastRect3.y -= 1f;
                    if (current5.Shop_Cost.Time1VCost != 0)
                    {
                        GUI.Label(lastRect3, current5.Shop_Cost.Time1VCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                    }
                    else if (current5.Shop_Cost.Time7VCost != 0)
                    {
                        GUI.Label(lastRect3, current5.Shop_Cost.Time7VCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                    }
                    else if (current5.Shop_Cost.Time30VCost != 0)
                    {
                        GUI.Label(lastRect3, current5.Shop_Cost.Time30VCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                    }
                    else if (current5.Shop_Cost.TimePVCost != 0)
                    {
                        GUI.Label(lastRect3, current5.Shop_Cost.TimePVCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                    }
                    GUILayout.Space(3f);
                    if (num3 % 3 == 2 || num3 == count3 - 1)
                    {
                        GUILayout.EndHorizontal();
                        GUILayout.Space(3f);
                    }
                    num3++;
                }
            }
            finally
            {
                ((IDisposable)enumerator5).Dispose();
            }
        }
        else if (MenuSelecter.ShopMenuSelect == MenuSelecter.ShopMenuEnum.Enhancer)
        {
            int num4 = 0;
            int count4 = ShopManager.Instance.Enhancers.Count;
            List<Enhancer>.Enumerator enumerator6 = ShopManager.Instance.Enhancers.GetEnumerator();
            try
            {
                while (enumerator6.MoveNext())
                {
                    CCItem current6 = enumerator6.Current;
                    if (num4 % 3 == 0)
                    {
                        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                        GUILayout.Space(3f);
                    }
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(97f));
                    GUILayout.Label(current6.Ico, GUISkinManager.Backgound.GetStyle("itemShop02"), GUILayout.Width(97f), GUILayout.Height(85f));
                    GUILayout.EndVertical();
                    Rect lastRect4 = GUILayoutUtility.GetLastRect();
                    if (current6.IsSale)
                    {
                        GUI.Label(new Rect(lastRect4.x, lastRect4.y, 33f, 29f), GUIContent.none, GUISkinManager.Ico.GetStyle("sale"));
                    }
                    lastRect4.height -= 27f;
                    GUIHover.Hover(Event.current, current6, lastRect4);
                    lastRect4.x += 68f;
                    lastRect4.y += 62f;
                    lastRect4.width = 24f;
                    lastRect4.height = 17f;
                    if (current6.IsBuyed)
                    {
                        lastRect4.width = 24f;
                        lastRect4.height = 23f;
                        lastRect4.y -= 6f;
                        GUI.Label(lastRect4, GUIContent.none, GUISkinManager.Label.GetStyle("isBuyed"));
                        lastRect4.y += 6f;
                    }
                    else if (current6.NeedLvl > LocalUser.Level)
                    {
                        lastRect4.width = 24f;
                        lastRect4.height = 23f;
                        lastRect4.y -= 6f;
                        GUI.Label(lastRect4, GUIContent.none, GUISkinManager.Label.GetStyle("isDisabled"));
                        lastRect4.y += 6f;
                    }
                    else if (GUI.Button(lastRect4, GUIContent.none, GUISkinManager.Button.GetStyle("buy")))
                    {
                        GUIShop.OnBuy(current6);
                    }
                    lastRect4.x -= 64f;
                    lastRect4.width = 60f;
                    lastRect4.y -= 1f;
                    if (current6.Shop_Cost.Time1VCost != 0)
                    {
                        GUI.Label(lastRect4, current6.Shop_Cost.Time1VCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                    }
                    else if (current6.Shop_Cost.Time7VCost != 0)
                    {
                        GUI.Label(lastRect4, current6.Shop_Cost.Time7VCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                    }
                    else if (current6.Shop_Cost.Time30VCost != 0)
                    {
                        GUI.Label(lastRect4, current6.Shop_Cost.Time30VCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                    }
                    else if (current6.Shop_Cost.TimePVCost != 0)
                    {
                        GUI.Label(lastRect4, current6.Shop_Cost.TimePVCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                    }
                    GUILayout.Space(3f);
                    if (num4 % 3 == 2 || num4 == count4 - 1)
                    {
                        GUILayout.EndHorizontal();
                        GUILayout.Space(3f);
                    }
                    num4++;
                }
            }
            finally
            {
                ((IDisposable)enumerator6).Dispose();
            }
        }
        GUILayout.EndScrollView();
        if (Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_items_scroll_rect = GUILayoutUtility.GetLastRect();
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
        if (Event.current.type == EventType.MouseDown && GUIShop.draged_items_scroll_rect.Contains(Event.current.mousePosition))
        {
            Vector2 zero = Vector2.zero;
            Vector2 mousePosition = Event.current.mousePosition;
            zero.x = mousePosition.x - GUIShop.draged_items_scroll_rect.x + GUIShop.debug_menu_GearVector.x;
            Vector2 mousePosition2 = Event.current.mousePosition;
            zero.y = mousePosition2.y - GUIShop.draged_items_scroll_rect.y + GUIShop.debug_menu_GearVector.y;
            int num5 = (int)(zero.x / GUIShop.draged_items_scroll_itemSize.x);
            int num6 = (int)(zero.y / GUIShop.draged_items_scroll_itemSize.y);
            if (MenuSelecter.ShopMenuSelect == MenuSelecter.ShopMenuEnum.Appearance && num6 * 3 + num5 < GUIShop.selectedWearsList.Count)
            {
                GUIShop.Draged_item = GUIShop.selectedWearsList[num6 * 3 + num5];
                if (GUIShop.preview.IsDressed(GUIShop.Draged_item as Wear))
                {
                    GUIShop.Draged_item = null;
                }
            }
        }
        if (GUIShop.menu_wear_type_seleted == CCWearType.Heads && GUIShop.preview.Head != null && Event.current.type == EventType.MouseDown && GUIShop.draged_view_head.Contains(Event.current.mousePosition))
        {
            GUIShop.Draged_item = GUIShop.preview.Head;
        }
        else if (GUIShop.preview.Hat != null && Event.current.type == EventType.MouseDown && GUIShop.draged_view_hat.Contains(Event.current.mousePosition))
        {
            GUIShop.Draged_item = GUIShop.preview.Hat;
        }
        else if (GUIShop.menu_wear_type_seleted != CCWearType.Heads && GUIShop.preview.Mask != null && Event.current.type == EventType.MouseDown && GUIShop.draged_view_mask.Contains(Event.current.mousePosition))
        {
            GUIShop.Draged_item = GUIShop.preview.Mask;
        }
        else if (GUIShop.preview.Gloves != null && Event.current.type == EventType.MouseDown && GUIShop.draged_view_gloves.Contains(Event.current.mousePosition))
        {
            GUIShop.Draged_item = GUIShop.preview.Gloves;
        }
        else if (GUIShop.preview.Shirt != null && Event.current.type == EventType.MouseDown && GUIShop.draged_view_shirt.Contains(Event.current.mousePosition))
        {
            GUIShop.Draged_item = GUIShop.preview.Shirt;
        }
        else if (GUIShop.preview.Pants != null && Event.current.type == EventType.MouseDown && GUIShop.draged_view_pants.Contains(Event.current.mousePosition))
        {
            GUIShop.Draged_item = GUIShop.preview.Pants;
        }
        else if (GUIShop.preview.Boots != null && Event.current.type == EventType.MouseDown && GUIShop.draged_view_boots.Contains(Event.current.mousePosition))
        {
            GUIShop.Draged_item = GUIShop.preview.Boots;
        }
        else if (GUIShop.preview.Backpack != null && Event.current.type == EventType.MouseDown && GUIShop.draged_view_backpack.Contains(Event.current.mousePosition))
        {
            GUIShop.Draged_item = GUIShop.preview.Backpack;
        }
        else if (GUIShop.preview.Other != null && Event.current.type == EventType.MouseDown && GUIShop.draged_view_other.Contains(Event.current.mousePosition))
        {
            GUIShop.Draged_item = GUIShop.preview.Other;
        }
        else if (GUIShop.Draged_item != null)
        {
            Vector2 mousePosition3 = Event.current.mousePosition;
            float x = mousePosition3.x - GUIShop.draged_items_scroll_itemSize.x * 0.5f;
            Vector2 mousePosition4 = Event.current.mousePosition;
            GUI.Label(new Rect(x, mousePosition4.y - GUIShop.draged_items_scroll_itemSize.y * 0.5f, GUIShop.draged_items_scroll_itemSize.x, GUIShop.draged_items_scroll_itemSize.y), GUIShop.Draged_item.Ico, GUIStyle.none);
        }
        if (Event.current.type == EventType.MouseUp && GUIShop.Draged_item != null)
        {
            if (GUIShop.draged_item_rect.Contains(Event.current.mousePosition) || GUIShop.debug_drag_droped_rect_global.Contains(Event.current.mousePosition))
            {
                if (GUIShop.Draged_item.GetType() == typeof(Wear))
                {
                    GUIShop.preview.DressUp(GUIShop.Draged_item as Wear);
                }
            }
            else if (GUIShop.draged_items_scroll_rect.Contains(Event.current.mousePosition) && GUIShop.Draged_item.GetType() == typeof(Wear) && GUIShop.preview.IsDressed(GUIShop.Draged_item as Wear))
            {
                GUIShop.preview.UnDress(GUIShop.Draged_item as Wear);
            }
            GUIShop.Draged_item = null;
        }
    }

    private static void DrawBarbershopSlots()
    {
        CCWearType cCWearType = CCWearType.None;
        if (GUIShop.Draged_item != null && typeof(Wear) == GUIShop.Draged_item.GetType())
        {
            cCWearType = (GUIShop.Draged_item as Wear).WearType;
        }
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.MinHeight(423f));
        GUILayout.Space(11f);
        GUILayout.Label((GUIShop.preview.Head == null) ? Wear.GetEmptyTexture(CCWearType.Heads) : GUIShop.preview.Head.Ico, GUISkinManager.Backgound.GetStyle((cCWearType != CCWearType.Heads) ? "itemLeft" : "itemLeftActive"), GUILayout.Width(105f), GUILayout.Height(93f));
        if (GUIShop.preview.Head != null)
        {
            GUIHover.Hover(Event.current, GUIShop.preview.Head, GUILayoutUtility.GetLastRect());
        }
        if (cCWearType == CCWearType.Heads && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_item_rect = GUILayoutUtility.GetLastRect();
        }
        if (GUIShop.Draged_item == null && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_view_head = GUILayoutUtility.GetLastRect();
        }
        GUILayout.EndVertical();
        if (Event.current.type == EventType.Repaint)
        {
            GUIShop.debug_drag_droped_rect_global = GUILayoutUtility.GetLastRect();
            GUIShop.debug_drag_droped_rect_global.x -= 180f;
            GUIShop.debug_drag_droped_rect_global.width = 165f;
        }
    }

    private static void DrawWearSlots()
    {
        CCWearType cCWearType = CCWearType.None;
        if (GUIShop.Draged_item != null && typeof(Wear) == GUIShop.Draged_item.GetType())
        {
            cCWearType = (GUIShop.Draged_item as Wear).WearType;
        }
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Space(11f);
        GUILayout.Label((GUIShop.preview.Hat == null) ? Wear.GetEmptyTexture(CCWearType.Hats) : GUIShop.preview.Hat.Ico, GUISkinManager.Backgound.GetStyle((cCWearType != CCWearType.Hats) ? "itemLeft" : "itemLeftActive"), GUILayout.Width(105f), GUILayout.Height(93f));
        if (GUIShop.preview.Hat != null)
        {
            GUIHover.Hover(Event.current, GUIShop.preview.Hat, GUILayoutUtility.GetLastRect());
        }
        if (cCWearType == CCWearType.Hats && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_item_rect = GUILayoutUtility.GetLastRect();
        }
        if (GUIShop.Draged_item == null && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_view_hat = GUILayoutUtility.GetLastRect();
        }
        GUILayout.Space(8f);
        GUILayout.Label((GUIShop.preview.Shirt == null) ? Wear.GetEmptyTexture(CCWearType.Shirts) : GUIShop.preview.Shirt.Ico, GUISkinManager.Backgound.GetStyle((cCWearType != CCWearType.Shirts) ? "itemLeft" : "itemLeftActive"), GUILayout.Width(105f), GUILayout.Height(93f));
        if (GUIShop.preview.Shirt != null)
        {
            GUIHover.Hover(Event.current, GUIShop.preview.Shirt, GUILayoutUtility.GetLastRect());
        }
        if (cCWearType == CCWearType.Shirts && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_item_rect = GUILayoutUtility.GetLastRect();
        }
        if (GUIShop.Draged_item == null && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_view_shirt = GUILayoutUtility.GetLastRect();
        }
        GUILayout.Space(8f);
        GUILayout.Label((GUIShop.preview.Gloves == null) ? Wear.GetEmptyTexture(CCWearType.Gloves) : GUIShop.preview.Gloves.Ico, GUISkinManager.Backgound.GetStyle((cCWearType != CCWearType.Gloves) ? "itemLeft" : "itemLeftActive"), GUILayout.Width(105f), GUILayout.Height(93f));
        if (GUIShop.preview.Gloves != null)
        {
            GUIHover.Hover(Event.current, GUIShop.preview.Gloves, GUILayoutUtility.GetLastRect());
        }
        if (cCWearType == CCWearType.Gloves && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_item_rect = GUILayoutUtility.GetLastRect();
        }
        if (GUIShop.Draged_item == null && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_view_gloves = GUILayoutUtility.GetLastRect();
        }
        GUILayout.Space(8f);
        GUILayout.Label((GUIShop.preview.Pants == null) ? Wear.GetEmptyTexture(CCWearType.Pants) : GUIShop.preview.Pants.Ico, GUISkinManager.Backgound.GetStyle((cCWearType != CCWearType.Pants) ? "itemLeft" : "itemLeftActive"), GUILayout.Width(105f), GUILayout.Height(93f));
        if (GUIShop.preview.Pants != null)
        {
            GUIHover.Hover(Event.current, GUIShop.preview.Pants, GUILayoutUtility.GetLastRect());
        }
        if (cCWearType == CCWearType.Pants && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_item_rect = GUILayoutUtility.GetLastRect();
        }
        if (GUIShop.Draged_item == null && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_view_pants = GUILayoutUtility.GetLastRect();
        }
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Space(11f);
        GUILayout.Label((GUIShop.preview.Mask == null) ? Wear.GetEmptyTexture(CCWearType.Masks) : GUIShop.preview.Mask.Ico, GUISkinManager.Backgound.GetStyle((cCWearType != CCWearType.Masks) ? "itemLeft" : "itemLeftActive"), GUILayout.Width(105f), GUILayout.Height(93f));
        if (GUIShop.preview.Mask != null)
        {
            GUIHover.Hover(Event.current, GUIShop.preview.Mask, GUILayoutUtility.GetLastRect());
        }
        if (cCWearType == CCWearType.Masks && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_item_rect = GUILayoutUtility.GetLastRect();
        }
        if (GUIShop.Draged_item == null && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_view_mask = GUILayoutUtility.GetLastRect();
        }
        GUILayout.Space(8f);
        GUILayout.Label((GUIShop.preview.Backpack == null) ? Wear.GetEmptyTexture(CCWearType.Backpacks) : GUIShop.preview.Backpack.Ico, GUISkinManager.Backgound.GetStyle((cCWearType != CCWearType.Backpacks) ? "itemLeft" : "itemLeftActive"), GUILayout.Width(105f), GUILayout.Height(93f));
        if (GUIShop.preview.Backpack != null)
        {
            GUIHover.Hover(Event.current, GUIShop.preview.Backpack, GUILayoutUtility.GetLastRect());
        }
        if (cCWearType == CCWearType.Backpacks && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_item_rect = GUILayoutUtility.GetLastRect();
        }
        if (GUIShop.Draged_item == null && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_view_backpack = GUILayoutUtility.GetLastRect();
        }
        GUILayout.Space(8f);
        GUILayout.Label((GUIShop.preview.Other == null) ? Wear.GetEmptyTexture(CCWearType.Others) : GUIShop.preview.Other.Ico, GUISkinManager.Backgound.GetStyle((cCWearType != CCWearType.Others) ? "itemLeft" : "itemLeftActive"), GUILayout.Width(105f), GUILayout.Height(93f));
        if (GUIShop.preview.Other != null)
        {
            GUIHover.Hover(Event.current, GUIShop.preview.Other, GUILayoutUtility.GetLastRect());
        }
        if (cCWearType == CCWearType.Others && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_item_rect = GUILayoutUtility.GetLastRect();
        }
        if (GUIShop.Draged_item == null && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_view_other = GUILayoutUtility.GetLastRect();
        }
        GUILayout.Space(8f);
        GUILayout.Label((GUIShop.preview.Boots == null) ? Wear.GetEmptyTexture(CCWearType.Boots) : GUIShop.preview.Boots.Ico, GUISkinManager.Backgound.GetStyle((cCWearType != CCWearType.Boots) ? "itemLeft" : "itemLeftActive"), GUILayout.Width(105f), GUILayout.Height(93f));
        if (GUIShop.preview.Boots != null)
        {
            GUIHover.Hover(Event.current, GUIShop.preview.Boots, GUILayoutUtility.GetLastRect());
        }
        if (cCWearType == CCWearType.Boots && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_item_rect = GUILayoutUtility.GetLastRect();
        }
        if (GUIShop.Draged_item == null && Event.current.type == EventType.Repaint)
        {
            GUIShop.draged_view_boots = GUILayoutUtility.GetLastRect();
        }
        GUILayout.EndVertical();
        if (Event.current.type == EventType.Repaint)
        {
            GUIShop.debug_drag_droped_rect_global = GUILayoutUtility.GetLastRect();
            GUIShop.debug_drag_droped_rect_global.x -= 180f;
            GUIShop.debug_drag_droped_rect_global.width = 165f;
        }
    }

    private static void OnBuy(CCItem item)
    {
        if (item.Shop_Cost.SelectedVCost > LocalUser.Money)
        {
            ErrorInfo.CODE code = ErrorInfo.CODE.MISSING_MONEY;
            if (item.GetType() == typeof(Weapon))
            {
                code.AddNotification(ErrorInfo.TYPE.BUY_WEAPON);
            }
            else
            {
                code.AddNotification(ErrorInfo.TYPE.BUY_WEAR);
            }
        }
        else
        {
            NotificationWindow.Add(new Notification(Notification.Type.BUY_ITEM, item, new Notification.ButtonClick(GUIShop.OnBuyConfirmed), item));
        }
    }

    private static void OnBuyConfirmed(object item)
    {
        if (item.GetType() == typeof(Wear))
        {
            ShopManager.Instance.BuyWear(item as Wear);
        }
        else if (item.GetType() == typeof(Weapon))
        {
            ShopManager.Instance.BuyWeapon(item as Weapon);
        }
        else if (item.GetType() == typeof(Taunt))
        {
            ShopManager.Instance.BuyTaunt(item as Taunt);
        }
        else if (item.GetType() == typeof(Enhancer))
        {
            ShopManager.Instance.BuyEnhancer(item as Enhancer);
        }
    }

    private static void ApplyWearFilter(CCWearType cwtype)
    {
        GUIShop.menu_wear_type_seleted = cwtype;
        switch (cwtype)
        {
            case CCWearType.Backpacks:
                GUIShop.selectedWearsList = ShopManager.Instance.WearBackPacks;
                break;
            case CCWearType.Boots:
                GUIShop.selectedWearsList = ShopManager.Instance.WearBoots;
                break;
            case CCWearType.Gloves:
                GUIShop.selectedWearsList = ShopManager.Instance.WearGloves;
                break;
            case CCWearType.Hats:
                GUIShop.selectedWearsList = ShopManager.Instance.WearHats;
                break;
            case CCWearType.Masks:
                GUIShop.selectedWearsList = ShopManager.Instance.WearMasks;
                break;
            case CCWearType.Others:
                GUIShop.selectedWearsList = ShopManager.Instance.WearOthers;
                break;
            case CCWearType.Pants:
                GUIShop.selectedWearsList = ShopManager.Instance.WearPants;
                break;
            case CCWearType.Shirts:
                GUIShop.selectedWearsList = ShopManager.Instance.WearShirts;
                break;
            case CCWearType.Heads:
                GUIShop.selectedWearsList = ShopManager.Instance.WearHeads;
                break;
        }
    }

    private static void ApplyWeaponFilter(uint weapon_slot)
    {
        GUIShop.menu_weapon_slot_selected = weapon_slot;
        GUIShop.selectedWeaponList = ShopManager.Instance.Weapons.FindAll((Weapon x) => x.WeaponSlot == GUIShop.menu_weapon_slot_selected);
    }
}


