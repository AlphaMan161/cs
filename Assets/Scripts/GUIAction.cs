// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIAction : MonoBehaviour
{
    public static void OnGUI()
    {
        GUILayout.Space(46f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(760f));
        if (BankActionPackageManager.PackageList.Count > 0)
        {
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Height(413f));
            GUILayout.Label(LanguageManager.GetText("ALL AND NOW!"), GUISkinManager.Backgound.GetStyle("langle"), GUILayout.Height(33f));
            List<BankActionPackage>.Enumerator enumerator = BankActionPackageManager.PackageList.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    BankActionPackage current = enumerator.Current;
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(current.Ico, GUISkinManager.Backgound.GetStyle("action"), GUILayout.Width(111f), GUILayout.Height(111f));
                    Rect lastRect = GUILayoutUtility.GetLastRect();
                    lastRect.x += 80f;
                    lastRect.y += 7f;
                    lastRect.width = 25f;
                    lastRect.height = 25f;
                    if (GUI.Button(lastRect, GUIContent.none, GUISkinManager.Button.GetStyle("btnZoom")))
                    {
                        current.Click();
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            if (BankActionPackageManager.PackageList.Count < 3)
            {
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
        }
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(333f), GUILayout.Height(454f));
        GUILayout.Label(ActionRotater.Action.Ico, GUISkinManager.Backgound.GetStyle("action"), GUILayout.Width(333f), GUILayout.Height(413f));
        if (ActionRotater.Action.ButtonText != string.Empty)
        {
            Rect lastRect2 = GUILayoutUtility.GetLastRect();
            lastRect2.x += 163f;
            lastRect2.y += 351f;
            lastRect2.width = 157f;
            lastRect2.height = 46f;
            if (GUI.Button(lastRect2, LanguageManager.GetText(ActionRotater.Action.ButtonText), GUISkinManager.Button.GetStyle("cancel")))
            {
                ActionRotater.Action.Click();
            }
        }
        GUILayout.EndVertical();
        GUILayout.Space(10f);
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}


