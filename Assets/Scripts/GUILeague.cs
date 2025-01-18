// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUILeague : MonoBehaviour
{
    private static Vector2 userScroll = new Vector2(0f, 0f);

    public static void OnGUI()
    {
        LeagueManager.Init();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        for (short num = 1; num < 16; num = (short)(num + 1))
        {
            if (LeagueManager.Instance.LeagueIndex == num)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
                GUILayout.Label(GUIContent.none, GUISkinManager.StatsLeague.GetStyle("league" + num + "Active"));
                GUILayout.EndHorizontal();
            }
            else if (GUILayout.Button(GUIContent.none, GUISkinManager.StatsLeague.GetStyle("league" + num)))
            {
                LeagueManager.Instance.LeagueIndex = num;
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(36f));
        GUILayout.Space(11f);
        GUILayout.Label(LanguageManager.GetText("Rank"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(66f));
        GUILayout.Label(LanguageManager.GetText("Name"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(310f));
        GUILayout.Label(LanguageManager.GetText("Level"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(90f));
        GUILayout.Label(LanguageManager.GetText("Kills"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(90f));
        GUILayout.Label(LanguageManager.GetText("Deaths"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(90f));
        GUILayout.Label(LanguageManager.GetText("K/D"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(60f));
        GUILayout.Space(34f);
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(2f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILeague.userScroll = GUILayout.BeginScrollView(GUILeague.userScroll, false, true, GUILayout.Height(310f));
        if (LeagueManager.Instance.SelectedLeague != null)
        {
            List<UserRating>.Enumerator enumerator = LeagueManager.Instance.SelectedLeague.Users.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    UserRating current = enumerator.Current;
                    if (current.Show)
                    {
                        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                        GUILayout.Space(4f);
                        GUILayout.BeginHorizontal(GUIContent.none, (current.UserID != LocalUser.UserID) ? GUIStyle.none : GUISkinManager.Backgound.GetStyle("roomActive"));
                        GUILayout.Space(11f);
                        GUILayout.Label(current.Place, GUISkinManager.Text.GetStyle("room"), GUILayout.Width(66f));
                        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(310f));
                        GUILayout.FlexibleSpace();
                        if (current.ClanID != 0 && GUILayout.Button(string.Format("[{0}]", current.ClanName), GUISkinManager.Text.GetStyle("room02")))
                        {
                            ClanManager.View(current.ClanID);
                        }
                        GUILayout.Label(current.Name, GUISkinManager.Text.GetStyle("room02"));
                        GUILayout.Space(5f);
                        if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnInfo"), GUILayout.Width(32f), GUILayout.Height(32f)) && current.UserID != 0)
                        {
                            StatisticManager.View(current.UserID);
                        }
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                        GUILayout.Label(current.Level.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(90f));
                        GUILayout.Label(current.Kill.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(90f));
                        GUILayout.Label(current.Death.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(90f));
                        GUILayout.Label(current.KD.ToString("F2"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(60f));
                        GUILayout.Space(4f);
                        GUILayout.EndHorizontal();
                        GUILayout.Space(4f);
                        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
                        GUILayout.EndVertical();
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
        }
        else
        {
            GUILayout.Label(LanguageManager.GetText("Current section is in maintenance mode. We are sorry for temporary nconveniences."), GUISkinManager.Text.GetStyle("noneBattle"), GUILayout.Height(310f));
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(50f));
        GUILayout.FlexibleSpace();
        if (LeagueManager.Instance != null && LeagueManager.Instance.CurrentUser != null)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Space(40f);
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("roomActive"));
            GUILayout.Label((!(LeagueManager.Instance.CurrentUser.Place != "-1")) ? string.Empty : LeagueManager.Instance.CurrentUser.Place, GUISkinManager.Text.GetStyle("room"), GUILayout.Width(66f));
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(281f));
            GUILayout.FlexibleSpace();
            if (LeagueManager.Instance.CurrentUser.ClanID != 0 && GUILayout.Button(string.Format("[{0}]", LeagueManager.Instance.CurrentUser.ClanName), GUISkinManager.Text.GetStyle("room02")))
            {
                ClanManager.View(LeagueManager.Instance.CurrentUser.ClanID);
            }
            GUILayout.Label(LeagueManager.Instance.CurrentUser.Name, GUISkinManager.Text.GetStyle("room02"));
            GUILayout.Space(5f);
            if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnInfo"), GUILayout.Width(32f), GUILayout.Height(32f)) && LeagueManager.Instance.CurrentUser.UserID != 0)
            {
                StatisticManager.View(LeagueManager.Instance.CurrentUser.UserID);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Label(LeagueManager.Instance.CurrentUser.Level.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(90f));
            GUILayout.Label(LeagueManager.Instance.CurrentUser.Kill.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(90f));
            GUILayout.Label(LeagueManager.Instance.CurrentUser.Death.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(90f));
            GUILayout.Label(LeagueManager.Instance.CurrentUser.KD.ToString("F2"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(60f));
            GUILayout.Space(29f);
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            Rect lastRect = GUILayoutUtility.GetLastRect();
            lastRect.width = 34f;
            lastRect.height = 41f;
            lastRect.x += 3f;
            lastRect.y -= 4.5f;
            GUI.Label(lastRect, GUIContent.none, GUISkinManager.StatsLeague.GetStyle("league" + LeagueManager.Instance.CurrentUser.League + "Active"));
            if (LeagueManager.Instance.CurrentUser.Place == "-1")
            {
                GUIHover.Hover(Event.current, LanguageManager.GetText("Unranked"), lastRect);
            }
        }
        else
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.Label(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}


