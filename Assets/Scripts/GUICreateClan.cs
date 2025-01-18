// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUICreateClan : MonoBehaviour
{
    private static Vector2 scroll = new Vector2(0f, 0f);

    private static string clanName = string.Empty;

    private static string clanTag = string.Empty;

    private static ClanArm clanArm = null;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public static void OnGUI()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
        GUILayout.Label(LanguageManager.GetText("Create clan"), GUISkinManager.Text.GetStyle("partActive"));
        GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), LanguageManager.GetText("Create clan"), GUISkinManager.Text.GetStyle("partActive"), GUISkinManager.Text.GetStyle("partActiveShadow"));
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(36f));
        GUILayout.Space(15f);
        GUILayout.Label(LanguageManager.GetText("You need to be at leat at level 30 to create the clan"), GUISkinManager.Text.GetStyle("menuTitle02"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Clan name:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(160f), GUILayout.Height(29f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUICreateClan.clanName = GUILayout.TextField(GUICreateClan.clanName, 64, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.MaxWidth(257f), GUILayout.Width(257f));
        GUILayout.Label(LanguageManager.GetText("(minimum 3 and maximum 16 symbols)"), GUISkinManager.Text.GetStyle("txtTip"));
        GUILayout.EndVertical();
        if (ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_NAME || ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_NAME_LEN || ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_NAME_EXIST || ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_USER_LVL_LESS || ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_CREATE_YOU_ARE_IN_CLAN)
        {
            GUILayout.Space(10f);
            if (ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_USER_LVL_LESS)
            {
                GUILayout.Label("* " + LanguageManager.GetText("Your level is too low to create the Clan"), GUISkinManager.Text.GetStyle("error01"), GUILayout.Height(29f));
            }
            else
            {
                GUILayout.Label("* " + ClanManager.CurrentLastError.GetDescription(), GUISkinManager.Text.GetStyle("error01"), GUILayout.Height(29f));
            }
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Tag of the clan:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(160f), GUILayout.Height(29f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUICreateClan.clanTag = GUILayout.TextField(GUICreateClan.clanTag, 6, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.MaxWidth(257f), GUILayout.Width(257f));
        GUILayout.Label(LanguageManager.GetText("(minimum 2 and maximum 6 symbols)"), GUISkinManager.Text.GetStyle("txtTip"));
        GUILayout.EndVertical();
        if (ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_TAG || ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_TAG_LEN || ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_TAG_EXIST)
        {
            GUILayout.Space(10f);
            GUILayout.Label("* " + ClanManager.CurrentLastError.GetDescription(), GUISkinManager.Text.GetStyle("error01"), GUILayout.Height(29f));
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Clan crest:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(160f), GUILayout.Height(29f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Height(80f));
        List<ClanArm>.Enumerator enumerator = ClanArmManager.DefaultArms.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ClanArm current = enumerator.Current;
                if (GUICreateClan.clanArm == null)
                {
                    GUICreateClan.clanArm = current;
                }
                if (GUILayout.Button(current.Ico, GUISkinManager.Backgound.GetStyle((GUICreateClan.clanArm != current) ? "clanArm" : "clanArmActive"), GUILayout.Width(64f), GUILayout.Height(64f)))
                {
                    GUICreateClan.clanArm = current;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        GUILayout.EndHorizontal();
        GUILayout.Label(LanguageManager.GetText("(more clan crests will be opened after clan creation)"), GUISkinManager.Text.GetStyle("txtTip"));
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(50f));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(37f));
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("Cost:"), GUISkinManager.Text.GetStyle("normal16"), GUILayout.Height(37f));
        GUILayout.Space(3f);
        GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("money"), GUILayout.Width(36f), GUILayout.Height(37f));
        GUILayout.Space(3f);
        GUILayout.Label(ClanManager.COST_CLAN_CREATE.ToString(), GUISkinManager.Label.GetStyle("money"));
        GUILayout.Space(10f);
        if (GUILayout.Button(LanguageManager.GetText("Create"), GUISkinManager.Button.GetStyle("green"), GUILayout.Width(106f), GUILayout.Height(37f)))
        {
            ClanManager.Create(GUICreateClan.clanName, GUICreateClan.clanTag, GUICreateClan.clanArm);
        }
        GUILayout.Space(10f);
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.Space(15f);
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}


