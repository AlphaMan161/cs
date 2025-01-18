// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIAchievement : MonoBehaviour
{
    private static Vector2 achievmentScroll = Vector2.zero;

    public static void OnGUI()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
        GUILayout.Label(LanguageManager.GetText("Achievements"), GUISkinManager.Text.GetStyle("partActive"));
        GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), LanguageManager.GetText("Achievements"), GUISkinManager.Text.GetStyle("partActive"), GUISkinManager.Text.GetStyle("partActiveShadow"));
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.Space(3f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(2f);
        GUIAchievement.achievmentScroll = GUILayout.BeginScrollView(GUIAchievement.achievmentScroll, false, true, GUILayout.MinHeight(378f));
        int num = 0;
        List<Achievement>.Enumerator enumerator = AchievementManager.Instance.ShowedList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Achievement current = enumerator.Current;
                if (num % 2 == 0)
                {
                    GUILayout.Space(7f);
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                }
                GUILayout.Space(13f);
                GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Width(344f), GUILayout.Height(101f));
                GUILayout.Label(current.Ico, GUIStyle.none, GUILayout.Width(97f), GUILayout.Height(85f));
                GUILayout.Space(6f);
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.MinHeight(70f));
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Label(current.Name, GUISkinManager.Text.GetStyle("normal01"));
                GUILayout.FlexibleSpace();
                if (current.Complete && current.Level == current.MaxLevel)
                {
                    for (int i = 1; i <= current.MaxLevel; i++)
                    {
                        GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("starActive"));
                    }
                }
                else
                {
                    for (int j = 1; j <= current.MaxLevel; j++)
                    {
                        if (j < current.Level)
                        {
                            GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("starActive"));
                        }
                        else
                        {
                            GUILayout.Label(GUIContent.none, GUISkinManager.Label.GetStyle("starNone"));
                        }
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(5f);
                GUILayout.Label(current.Description, GUISkinManager.Text.GetStyle("normal02"), GUILayout.MaxWidth(225f));
                GUILayout.Space(5f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                if (current.Complete && current.Level == current.MaxLevel)
                {
                    GUILayout.Label(LanguageManager.GetText("Complete"), GUISkinManager.Text.GetStyle("normal03"));
                }
                else
                {
                    GUILayout.Label(LanguageManager.GetTextFormat("{0} step reward:", current.Level), GUISkinManager.Text.GetStyle("normal03"));
                    GUILayout.Space(4f);
                    GUILayout.Label(current.Reward.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                if (current.UserLvl > LocalUser.Level)
                {
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.Label(LanguageManager.GetTextFormat("Available from level {0}", current.UserLvl), GUISkinManager.Text.GetStyle("error02"));
                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUIProgressBar.ProgressBar(143f, (float)current.MaxValue, (float)current.Value, "pb4");
                    GUILayout.Space(4f);
                    GUILayout.Label(string.Format("{0}/{1}", current.Value, current.MaxValue), GUISkinManager.Text.GetStyle("normal04"));
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                num++;
                if (num % 2 == 0 || num >= AchievementManager.Instance.ShowedList.Count)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.Space(7f);
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        GUILayout.EndScrollView();
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}


