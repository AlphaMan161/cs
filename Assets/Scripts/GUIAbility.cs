// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIAbility : MonoBehaviour
{
    private static Vector2 abilityScroll = Vector2.zero;

    public static void OnGUI()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
        GUILayout.Label(LanguageManager.GetText("Practice hall"), GUISkinManager.Text.GetStyle("partActive"));
        GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), LanguageManager.GetText("Practice hall"), GUISkinManager.Text.GetStyle("partActive"), GUISkinManager.Text.GetStyle("partActiveShadow"));
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.Space(3f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(2f);
        GUIAbility.abilityScroll = GUILayout.BeginScrollView(GUIAbility.abilityScroll, false, true, GUILayout.MinHeight(378f));
        int num = 0;
        List<Ability>.Enumerator enumerator = AbilityManager.Instance.ShowedList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Ability current = enumerator.Current;
                if (num % 2 == 0)
                {
                    GUILayout.Space(7f);
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                }
                GUILayout.Space(13f);
                GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Width(344f), GUILayout.Height(102f));
                GUILayout.Label(current.Ico, GUIStyle.none, GUILayout.Width(97f), GUILayout.Height(85f));
                GUILayout.Space(6f);
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.MinHeight(51f));
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Label(current.Name, GUISkinManager.Text.GetStyle("normal01"));
                GUILayout.EndHorizontal();
                GUILayout.Space(5f);
                GUILayout.Label(LanguageManager.GetText("cur. lev: ") + (current.IsBuyed ? current.Description : LanguageManager.GetText("no")), GUISkinManager.Text.GetStyle("normal02"));
                GUILayout.Space(5f);
                if (current.NextLevel != null)
                {
                    if (!current.IsBuyed)
                    {
                        GUILayout.Label(LanguageManager.GetText("Nxt. lev: ") + current.Description, GUISkinManager.Text.GetStyle("normal05"));
                    }
                    else
                    {
                        GUILayout.Label(LanguageManager.GetText("Nxt. lev: ") + current.NextLevel.Description, GUISkinManager.Text.GetStyle("normal05"));
                    }
                }
                GUILayout.EndVertical();
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(33f));
                if (!current.IsBuyed)
                {
                    GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("bg_ability_lvl0"), GUILayout.Width(143f), GUILayout.Height(19f));
                }
                else
                {
                    GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("bg_ability_lvl" + current.Level.ToString()), GUILayout.Width(143f), GUILayout.Height(19f));
                }
                GUILayout.Space(4f);
                if (!current.IsBuyed)
                {
                    if (GUILayout.Button(current.Cost.TimePVCost.ToString(), GUISkinManager.Button.GetStyle("buyAbility"), GUILayout.Width(82f), GUILayout.Height(33f)))
                    {
                        AbilityManager.Instance.Buy(current);
                    }
                }
                else if (current.NextLevel != null && GUILayout.Button(current.NextLevel.Cost.TimePVCost.ToString(), GUISkinManager.Button.GetStyle("buyAbility"), GUILayout.Width(82f), GUILayout.Height(33f)))
                {
                    AbilityManager.Instance.Buy(current.NextLevel);
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                num++;
                if (num % 2 == 0 || num >= AbilityManager.Instance.ShowedList.Count)
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


