// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIRating : MonoBehaviour
{
    private static Vector2 userScroll = new Vector2(0f, 0f);

    public static void OnGUI()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
        GUILayout.Label(LanguageManager.GetText("Players rating"), GUISkinManager.Text.GetStyle("partActive"));
        GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), LanguageManager.GetText("Players rating"), GUISkinManager.Text.GetStyle("partActive"), GUISkinManager.Text.GetStyle("partActiveShadow"));
        GUILayout.EndHorizontal();
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
        GUIRating.userScroll = GUILayout.BeginScrollView(GUIRating.userScroll, false, true, GUILayout.Height(340f));
        List<UserRating>.Enumerator enumerator = Rating.UserList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UserRating current = enumerator.Current;
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                GUILayout.Space(4f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Space(11f);
                GUILayout.Label(current.Place.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(66f));
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(310f));
                GUILayout.FlexibleSpace();
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
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}


