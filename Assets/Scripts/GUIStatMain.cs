// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIStatMain : MonoBehaviour
{
    private static List<Weapon> selectedWeaponList = Inventory.Instance.Weapons;

    private static Dictionary<short, string> menu = new Dictionary<short, string>();

    private static short menu_slot_selected = 0;

    private static Vector2 debug_menu_GearVector = new Vector2(0f, 0f);

    private static bool isInit = false;

    public static bool IsInit
    {
        get
        {
            return GUIStatMain.isInit;
        }
        set
        {
            GUIStatMain.isInit = value;
        }
    }

    private static void Init()
    {
        if (!GUIStatMain.isInit && StatisticManager.CurrentUser.Ready)
        {
            object currentUser = StatisticManager.CurrentUser;
            GUIStatMain.menu.Clear();
            GUIStatMain.menu.Add(1, LanguageManager.GetText("Common"));
            GUIStatMain.menu.Add(2, LanguageManager.GetText("Maps"));
            GUIStatMain.menu.Add(3, LanguageManager.GetText("Modes"));
            if (GUIStatMain.menu_slot_selected == 0)
            {
                GUIStatMain.menu_slot_selected = 1;
            }
            GUIStatMain.isInit = true;
            StatisticManager.OnChange -= new StatisticManager.StatisticEventHandler(GUIStatMain.OnChange);
            StatisticManager.OnChange += new StatisticManager.StatisticEventHandler(GUIStatMain.OnChange);
        }
    }

    private static void OnChange(object obj)
    {
        UnityEngine.Debug.LogError("[GUIStatMain] OnChange");
        GUIStatMain.isInit = false;
    }

    private static void ApplyMenuFilter(short num)
    {
        GUIStatMain.menu_slot_selected = num;
    }

    public static void OnGUI()
    {
        GUIStatMain.Init();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.MinWidth(760f));
        GUILayout.Space(5f);
        GUIStatMain.DrawWearSlots();
        GUILayout.Space(8f);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(334f), GUILayout.Height(454f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.MinHeight(55f));
        GUILayout.Space(8f);
        Dictionary<short, string>.Enumerator enumerator = GUIStatMain.menu.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<short, string> current = enumerator.Current;
                if (current.Key == GUIStatMain.menu_slot_selected)
                {
                    GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
                    GUILayout.Label(current.Value, GUISkinManager.Text.GetStyle("partActive"));
                    GUILayout.EndHorizontal();
                }
                else if (GUILayout.Button(current.Value, GUISkinManager.Text.GetStyle("part")))
                {
                    GUIStatMain.ApplyMenuFilter(current.Key);
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
        GUIStatMain.debug_menu_GearVector = GUILayout.BeginScrollView(GUIStatMain.debug_menu_GearVector, false, true, GUILayout.Height(380f));
        if (GUIStatMain.menu_slot_selected == 1)
        {
            GUIStatMain.DrawStatCommon();
        }
        else if (GUIStatMain.menu_slot_selected == 2)
        {
            GUIStatMain.DrawStatMap();
        }
        else if (GUIStatMain.menu_slot_selected == 3)
        {
            GUIStatMain.DrawStatGameMode();
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
    }

    private static void DrawStatMap()
    {
        List<UserRatingMap>.Enumerator enumerator = StatisticManager.CurrentUser.MapStat.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UserRatingMap current = enumerator.Current;
                if (current.Map != null)
                {
                    GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.MinHeight(75f));
                    GUILayout.Label(current.Map.Ico, GUIStyle.none, GUILayout.Width(100f), GUILayout.Height(60f));
                    GUILayout.Space(6f);
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.Label(current.Map.Name, GUISkinManager.Text.GetStyle("normal01"));
                    GUILayout.EndHorizontal();
                    GUILayout.Space(4f);
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.Label(LanguageManager.GetText("Victories"), GUISkinManager.Text.GetStyle("normal03"));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(current.Win.ToString(), GUISkinManager.Text.GetStyle("normal04"));
                    GUILayout.EndHorizontal();
                    GUILayout.Space(2f);
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.Label(LanguageManager.GetText("Time in the game"), GUISkinManager.Text.GetStyle("normal03"));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(current.PlayedTimeString, GUISkinManager.Text.GetStyle("normal04"));
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
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
                StatisticManager.ClearStatistic(4);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
        }
    }

    private static void DrawStatGameMode()
    {
        List<UserRatingGameMode>.Enumerator enumerator = StatisticManager.CurrentUser.GameModeStat.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UserRatingGameMode current = enumerator.Current;
                GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.MinHeight(70f));
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Label(current.Mode.GetFullName(), GUISkinManager.Text.GetStyle("normal01"));
                GUILayout.EndHorizontal();
                GUILayout.Space(4f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Label(LanguageManager.GetText("Victories"), GUISkinManager.Text.GetStyle("normal03"));
                GUILayout.FlexibleSpace();
                GUILayout.Label(current.Win.ToString(), GUISkinManager.Text.GetStyle("normal04"));
                GUILayout.EndHorizontal();
                GUILayout.Space(4f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Label(LanguageManager.GetText("Time in the game"), GUISkinManager.Text.GetStyle("normal03"));
                GUILayout.FlexibleSpace();
                GUILayout.Label(current.PlayedTimeString, GUISkinManager.Text.GetStyle("normal04"));
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
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
                StatisticManager.ClearStatistic(3);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
        }
    }

    private static void DrawStatCommon()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.MinHeight(70f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Battle statistics"), GUISkinManager.Text.GetStyle("normal01"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Level"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.Level.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Time in the game"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.PlayedTimeString, GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Kills"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.Kill.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Deaths"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.Death.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("K/D"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(string.Format("{0:F2} %", StatisticManager.CurrentUser.KD), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Victories"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.Win.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Defeats"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.Lose.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Accuracy"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(string.Format("{0:F2} %", StatisticManager.CurrentUser.Accuracy), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(8f);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.MinHeight(70f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Kills"), GUISkinManager.Text.GetStyle("normal01"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Overall"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.Kill.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Kills to head"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.HeadShot.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Kills to nuts"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.NutsShot.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(8f);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.MinHeight(70f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Deaths"), GUISkinManager.Text.GetStyle("normal01"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Overall"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.Death.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Suicide"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.Suicide.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Dead of head shot"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.DeathHeadShot.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Dead of nuts shot"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.DeathNutsShot.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(8f);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.MinHeight(70f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Various"), GUISkinManager.Text.GetStyle("normal01"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Overall dominations"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.Domination.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Max. dominations"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.MaxDomination.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Overall revanges"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.Revenge.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Max. revanges"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.MaxRevenge.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Overall shots"), GUISkinManager.Text.GetStyle("normal03"));
        GUILayout.FlexibleSpace();
        GUILayout.Label(StatisticManager.CurrentUser.Shot.ToString(), GUISkinManager.Text.GetStyle("normal04"));
        GUILayout.EndHorizontal();
        if (StatisticManager.CurrentUser.Clan != null || (LocalUser.UserID == StatisticManager.CurrentUser.UserID && LocalUser.Clan != null))
        {
            Clan clan = (StatisticManager.CurrentUser.Clan == null) ? LocalUser.Clan : StatisticManager.CurrentUser.Clan;
            GUILayout.Space(4f);
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Label(LanguageManager.GetText("Clan"), GUISkinManager.Text.GetStyle("normal03"));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(clan.Name, GUISkinManager.Text.GetStyle("normal04")))
            {
                ClanManager.View(clan.ClanID);
            }
            GUILayout.EndHorizontal();
        }
        if (StatisticManager.CurrentUser.SocialUrl != string.Empty)
        {
            GUILayout.Space(4f);
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Label(LanguageManager.GetText("Profile in social network"), GUISkinManager.Text.GetStyle("normal03"));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(StatisticManager.CurrentUser.SocialUrl, GUISkinManager.Text.GetStyle("normal04")))
            {
                WebCall.OpenUrl(StatisticManager.CurrentUser.SocialUrl);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(8f);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
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
                StatisticManager.ClearStatistic(2);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
        }
    }

    public static void DrawName()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(210f), GUILayout.Height(32f));
        GUILayout.Label(StatisticManager.CurrentUser.Name, GUISkinManager.Text.GetStyle("statisticTopName"));
        if (StatisticManager.CurrentUser.UserID != LocalUser.UserID && GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnHome"), GUILayout.Width(32f), GUILayout.Height(32f)))
        {
            StatisticManager.SetLocal();
        }
        GUILayout.EndHorizontal();
    }

    private static void DrawWearSlots()
    {
        if (StatisticManager.CurrentUser != null)
        {
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
            GUILayout.Space(11f);
            GUILayout.Label((StatisticManager.CurrentUser.View.Head == null) ? Wear.GetEmptyTexture(CCWearType.Heads) : StatisticManager.CurrentUser.View.Head.Ico, GUISkinManager.Backgound.GetStyle("itemLeft"), GUILayout.Width(94.5f), GUILayout.Height(83.7f));
            if (StatisticManager.CurrentUser.View.Head != null)
            {
                GUIHover.Hover(Event.current, StatisticManager.CurrentUser.View.Head, GUILayoutUtility.GetLastRect());
            }
            GUILayout.Space(6f);
            GUILayout.Label((StatisticManager.CurrentUser.View.Hat == null) ? Wear.GetEmptyTexture(CCWearType.Hats) : StatisticManager.CurrentUser.View.Hat.Ico, GUISkinManager.Backgound.GetStyle("itemLeft"), GUILayout.Width(94.5f), GUILayout.Height(83.7f));
            if (StatisticManager.CurrentUser.View.Hat != null)
            {
                GUIHover.Hover(Event.current, StatisticManager.CurrentUser.View.Hat, GUILayoutUtility.GetLastRect());
            }
            GUILayout.Space(6f);
            GUILayout.Label((StatisticManager.CurrentUser.View.Shirt == null) ? Wear.GetEmptyTexture(CCWearType.Shirts) : StatisticManager.CurrentUser.View.Shirt.Ico, GUISkinManager.Backgound.GetStyle("itemLeft"), GUILayout.Width(94.5f), GUILayout.Height(83.7f));
            if (StatisticManager.CurrentUser.View.Shirt != null)
            {
                GUIHover.Hover(Event.current, StatisticManager.CurrentUser.View.Shirt, GUILayoutUtility.GetLastRect());
            }
            GUILayout.Space(6f);
            GUILayout.Label((StatisticManager.CurrentUser.View.Gloves == null) ? Wear.GetEmptyTexture(CCWearType.Gloves) : StatisticManager.CurrentUser.View.Gloves.Ico, GUISkinManager.Backgound.GetStyle("itemLeft"), GUILayout.Width(94.5f), GUILayout.Height(83.7f));
            if (StatisticManager.CurrentUser.View.Gloves != null)
            {
                GUIHover.Hover(Event.current, StatisticManager.CurrentUser.View.Gloves, GUILayoutUtility.GetLastRect());
            }
            GUILayout.Space(6f);
            GUILayout.Label((StatisticManager.CurrentUser.View.Pants == null) ? Wear.GetEmptyTexture(CCWearType.Pants) : StatisticManager.CurrentUser.View.Pants.Ico, GUISkinManager.Backgound.GetStyle("itemLeft"), GUILayout.Width(94.5f), GUILayout.Height(83.7f));
            if (StatisticManager.CurrentUser.View.Pants != null)
            {
                GUIHover.Hover(Event.current, StatisticManager.CurrentUser.View.Pants, GUILayoutUtility.GetLastRect());
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUIStatMain.DrawName();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
            GUILayout.Space(11f);
            GUILayout.Label((StatisticManager.CurrentUser.View.Mask == null) ? Wear.GetEmptyTexture(CCWearType.Masks) : StatisticManager.CurrentUser.View.Mask.Ico, GUISkinManager.Backgound.GetStyle("itemLeft"), GUILayout.Width(94.5f), GUILayout.Height(83.7f));
            if (StatisticManager.CurrentUser.View.Mask != null)
            {
                GUIHover.Hover(Event.current, StatisticManager.CurrentUser.View.Mask, GUILayoutUtility.GetLastRect());
            }
            GUILayout.Space(6f);
            GUILayout.Label((StatisticManager.CurrentUser.View.Backpack == null) ? Wear.GetEmptyTexture(CCWearType.Backpacks) : StatisticManager.CurrentUser.View.Backpack.Ico, GUISkinManager.Backgound.GetStyle("itemLeft"), GUILayout.Width(94.5f), GUILayout.Height(83.7f));
            if (StatisticManager.CurrentUser.View.Backpack != null)
            {
                GUIHover.Hover(Event.current, StatisticManager.CurrentUser.View.Backpack, GUILayoutUtility.GetLastRect());
            }
            GUILayout.Space(6f);
            GUILayout.Label((StatisticManager.CurrentUser.View.Other == null) ? Wear.GetEmptyTexture(CCWearType.Others) : StatisticManager.CurrentUser.View.Other.Ico, GUISkinManager.Backgound.GetStyle("itemLeft"), GUILayout.Width(94.5f), GUILayout.Height(83.7f));
            if (StatisticManager.CurrentUser.View.Other != null)
            {
                GUIHover.Hover(Event.current, StatisticManager.CurrentUser.View.Other, GUILayoutUtility.GetLastRect());
            }
            GUILayout.Space(6f);
            GUILayout.Label((StatisticManager.CurrentUser.View.Boots == null) ? Wear.GetEmptyTexture(CCWearType.Boots) : StatisticManager.CurrentUser.View.Boots.Ico, GUISkinManager.Backgound.GetStyle("itemLeft"), GUILayout.Width(94.5f), GUILayout.Height(83.7f));
            if (StatisticManager.CurrentUser.View.Boots != null)
            {
                GUIHover.Hover(Event.current, StatisticManager.CurrentUser.View.Boots, GUILayoutUtility.GetLastRect());
            }
            GUILayout.EndVertical();
        }
    }
}


