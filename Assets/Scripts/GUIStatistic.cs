// ILSpyBased#2
using UnityEngine;

public class GUIStatistic : MonoBehaviour
{
    public static void OnGUI()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width((float)Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuRow03"), GUILayout.Height(41f));
        if (GUILayout.Button(LanguageManager.GetText("Achievements"), GUISkinManager.Button.GetStyle((MenuSelecter.StatisticsMenuSelect != MenuSelecter.StatisticsMenuEnum.Achievement) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.StatisticsMenuSelect = MenuSelecter.StatisticsMenuEnum.Achievement;
        }
        if (GUILayout.Button(LanguageManager.GetText("Statistics"), GUISkinManager.Button.GetStyle((MenuSelecter.StatisticsMenuSelect != MenuSelecter.StatisticsMenuEnum.Main) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.StatisticsMenuSelect = MenuSelecter.StatisticsMenuEnum.Main;
        }
        if (GUILayout.Button(LanguageManager.GetText("Weapons stat."), GUISkinManager.Button.GetStyle((MenuSelecter.StatisticsMenuSelect != MenuSelecter.StatisticsMenuEnum.Weapon) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.StatisticsMenuSelect = MenuSelecter.StatisticsMenuEnum.Weapon;
        }
        if (GUILayout.Button(LanguageManager.GetText("Players rating"), GUISkinManager.Button.GetStyle((MenuSelecter.StatisticsMenuSelect != MenuSelecter.StatisticsMenuEnum.Rating) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.StatisticsMenuSelect = MenuSelecter.StatisticsMenuEnum.Rating;
        }
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        if (MenuSelecter.StatisticsMenuSelect == MenuSelecter.StatisticsMenuEnum.Achievement)
        {
            GUIAchievement.OnGUI();
        }
        else if (MenuSelecter.StatisticsMenuSelect == MenuSelecter.StatisticsMenuEnum.Rating)
        {
            GUILeague.OnGUI();
        }
        else if (MenuSelecter.StatisticsMenuSelect == MenuSelecter.StatisticsMenuEnum.Weapon)
        {
            if (StatisticManager.CurrentUser.Ready)
            {
                GUIStatWeapon.OnGUI();
            }
        }
        else if (MenuSelecter.StatisticsMenuSelect == MenuSelecter.StatisticsMenuEnum.Main && StatisticManager.CurrentUser.Ready)
        {
            GUIStatMain.OnGUI();
        }
    }

    public static void DrawWait()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("Please wait loading statistic!"), GUISkinManager.Text.GetStyle("friendConnecting"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    public static void DrawDisable()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("Statistics is currently unavailable!"), GUISkinManager.Text.GetStyle("friendConnecting"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}


