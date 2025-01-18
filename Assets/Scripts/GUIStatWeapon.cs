// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIStatWeapon : MonoBehaviour
{
    private static List<UserRatingWeapon> selectedWeaponList = new List<UserRatingWeapon>();

    private static List<uint> menu_weapons = new List<uint>();

    private static uint menu_weapon_slot_selected = 0u;

    private static Vector2 debug_menu_GearVector = new Vector2(0f, 0f);

    private static bool isInit = false;

    public static bool IsInit
    {
        get
        {
            return GUIStatWeapon.isInit;
        }
        set
        {
            GUIStatWeapon.isInit = value;
        }
    }

    private static void Init()
    {
        if (!GUIStatWeapon.isInit && StatisticManager.CurrentUser.Ready)
        {
            object currentUser = StatisticManager.CurrentUser;
            GUIStatWeapon.menu_weapons.Clear();
            GUIStatWeapon.menu_weapons.Add(1u);
            GUIStatWeapon.menu_weapons.Add(2u);
            GUIStatWeapon.menu_weapons.Add(3u);
            GUIStatWeapon.menu_weapons.Add(4u);
            GUIStatWeapon.menu_weapons.Add(5u);
            GUIStatWeapon.menu_weapons.Add(6u);
            GUIStatWeapon.menu_weapons.Add(7u);
            GUIStatWeapon.menu_weapon_slot_selected = 1u;
            GUIStatWeapon.isInit = true;
            GUIStatWeapon.ApplyWeaponFilter(GUIStatWeapon.menu_weapon_slot_selected);
            StatisticManager.OnChange -= new StatisticManager.StatisticEventHandler(GUIStatWeapon.OnChange);
            StatisticManager.OnChange += new StatisticManager.StatisticEventHandler(GUIStatWeapon.OnChange);
        }
    }

    private static void OnChange(object obj)
    {
        GUIStatWeapon.isInit = false;
    }

    private static void ApplyWeaponFilter(uint cwtype)
    {
        GUIStatWeapon.menu_weapon_slot_selected = cwtype;
        GUIStatWeapon.selectedWeaponList = StatisticManager.CurrentUser.WeaponStat.FindAll((UserRatingWeapon x) => WeaponTypeHelper.GetWeaponSlot(x.Weapon.WeaponType) == GUIStatWeapon.menu_weapon_slot_selected);
        GUIStatWeapon.selectedWeaponList.Sort((UserRatingWeapon x, UserRatingWeapon y) => y.Kill.CompareTo(x.Kill));
    }

    public static void OnGUI()
    {
        GUIStatWeapon.Init();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.MinWidth(760f));
        GUILayout.Space(101f);
        GUIStatMain.DrawName();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain2"), GUILayout.Width(334f), GUILayout.Height(361f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.MinHeight(55f));
        GUILayout.Space(8f);
        List<uint>.Enumerator enumerator = GUIStatWeapon.menu_weapons.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                uint current = enumerator.Current;
                if (current == GUIStatWeapon.menu_weapon_slot_selected)
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
                        GUIStatWeapon.ApplyWeaponFilter(current);
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
        GUILayout.EndHorizontal();
        GUILayout.Space(3f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(4f);
        GUIStatWeapon.debug_menu_GearVector = GUILayout.BeginScrollView(GUIStatWeapon.debug_menu_GearVector, false, true, GUILayout.Height(286f));
        List<UserRatingWeapon>.Enumerator enumerator2 = GUIStatWeapon.selectedWeaponList.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                UserRatingWeapon current2 = enumerator2.Current;
                GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.MinHeight(101f));
                GUILayout.Label(current2.Weapon.Ico, GUISkinManager.Backgound.GetStyle("itemRight02"), GUILayout.Width(97f), GUILayout.Height(85f));
                GUILayout.Space(6f);
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                GUILayout.Label(current2.Weapon.Name, GUISkinManager.Text.GetStyle("normal01"));
                GUILayout.Space(4f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Label(LanguageManager.GetText("Kills"), GUISkinManager.Text.GetStyle("normal03"));
                GUILayout.FlexibleSpace();
                GUILayout.Label(current2.Kill.ToString(), GUISkinManager.Text.GetStyle("normal04"));
                GUILayout.EndHorizontal();
                GUILayout.Space(4f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Label(LanguageManager.GetText("To head"), GUISkinManager.Text.GetStyle("normal03"));
                GUILayout.FlexibleSpace();
                GUILayout.Label(current2.HeadShot.ToString(), GUISkinManager.Text.GetStyle("normal04"));
                GUILayout.EndHorizontal();
                GUILayout.Space(4f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Label(LanguageManager.GetText("To nuts"), GUISkinManager.Text.GetStyle("normal03"));
                GUILayout.FlexibleSpace();
                GUILayout.Label(current2.NutsShot.ToString(), GUISkinManager.Text.GetStyle("normal04"));
                GUILayout.EndHorizontal();
                GUILayout.Space(4f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Label(LanguageManager.GetText("Accuracy"), GUISkinManager.Text.GetStyle("normal03"));
                GUILayout.FlexibleSpace();
                GUILayout.Label(string.Format("{0:F2} %", current2.Accuracy), GUISkinManager.Text.GetStyle("normal04"));
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }
        finally
        {
            ((IDisposable)enumerator2).Dispose();
        }
        if (StatisticManager.CurrentUser.UserID == LocalUser.UserID)
        {
            GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.MinHeight(60f));
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.Label(LanguageManager.GetText("Statistics reset"), GUISkinManager.Text.GetStyle("normal01"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(8f);
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.Label(LanguageManager.GetText("Cost:"), GUISkinManager.Text.GetStyle("normal03"));
            GUILayout.Space(4f);
            GUILayout.Label("30", GUISkinManager.Label.GetStyle("costSmall"));
            GUILayout.Space(5f);
            if (GUILayout.Button(LanguageManager.GetText("Reset"), GUISkinManager.Button.GetStyle("btnStatClear"), GUILayout.Width(77f), GUILayout.Height(22f)))
            {
                StatisticManager.ClearStatistic(1);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.Space(6f);
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.EndVertical();
        GUILayout.Space(10f);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        if (MenuSelecter.StatisticsMenuSelect == MenuSelecter.StatisticsMenuEnum.Weapon)
        {
            GUILayout.Space(3f);
            GUIStatWeapon.DrawWeeaponSlots();
        }
    }

    private static void DrawWeeaponSlots()
    {
        if (StatisticManager.CurrentUser != null)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(707f));
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(101f));
            GUILayout.Label(StatisticManager.CurrentUser.WeaponSlot.Weapon1.Ico, GUISkinManager.Backgound.GetStyle("itemLeft2"), GUILayout.Width(101f), GUILayout.Height(90f));
            GUIHover.Hover(Event.current, StatisticManager.CurrentUser.WeaponSlot.Weapon1, GUILayoutUtility.GetLastRect());
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(101f));
            GUILayout.Label(StatisticManager.CurrentUser.WeaponSlot.Weapon2.Ico, GUISkinManager.Backgound.GetStyle("itemLeft2"), GUILayout.Width(101f), GUILayout.Height(90f));
            GUIHover.Hover(Event.current, StatisticManager.CurrentUser.WeaponSlot.Weapon2, GUILayoutUtility.GetLastRect());
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(101f));
            GUILayout.Label(StatisticManager.CurrentUser.WeaponSlot.Weapon3.Ico, GUISkinManager.Backgound.GetStyle("itemLeft2"), GUILayout.Width(101f), GUILayout.Height(90f));
            GUIHover.Hover(Event.current, StatisticManager.CurrentUser.WeaponSlot.Weapon3, GUILayoutUtility.GetLastRect());
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(101f));
            GUILayout.Label(StatisticManager.CurrentUser.WeaponSlot.Weapon4.Ico, GUISkinManager.Backgound.GetStyle("itemLeft2"), GUILayout.Width(101f), GUILayout.Height(90f));
            GUIHover.Hover(Event.current, StatisticManager.CurrentUser.WeaponSlot.Weapon4, GUILayoutUtility.GetLastRect());
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(101f));
            GUILayout.Label(StatisticManager.CurrentUser.WeaponSlot.Weapon5.Ico, GUISkinManager.Backgound.GetStyle("itemLeft2"), GUILayout.Width(101f), GUILayout.Height(90f));
            GUIHover.Hover(Event.current, StatisticManager.CurrentUser.WeaponSlot.Weapon5, GUILayoutUtility.GetLastRect());
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(101f));
            GUILayout.Label(StatisticManager.CurrentUser.WeaponSlot.Weapon6.Ico, GUISkinManager.Backgound.GetStyle("itemLeft2"), GUILayout.Width(101f), GUILayout.Height(90f));
            GUIHover.Hover(Event.current, StatisticManager.CurrentUser.WeaponSlot.Weapon6, GUILayoutUtility.GetLastRect());
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(101f));
            GUILayout.Label(StatisticManager.CurrentUser.WeaponSlot.Weapon7.Ico, GUISkinManager.Backgound.GetStyle("itemLeft2"), GUILayout.Width(101f), GUILayout.Height(90f));
            GUIHover.Hover(Event.current, StatisticManager.CurrentUser.WeaponSlot.Weapon7, GUILayoutUtility.GetLastRect());
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}


