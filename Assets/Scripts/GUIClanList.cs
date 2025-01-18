// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIClanList : MonoBehaviour
{
    private static Vector2 userScroll = new Vector2(0f, 0f);

    private static string searchTag = string.Empty;

    public static void OnGUI()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
        GUILayout.Label(LanguageManager.GetText("All clans"), GUISkinManager.Text.GetStyle("partActive"));
        GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), LanguageManager.GetText("All clans"), GUISkinManager.Text.GetStyle("partActive"), GUISkinManager.Text.GetStyle("partActiveShadow"));
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(37f));
        GUILayout.Space(15f);
        GUILayout.Label(LanguageManager.GetText("Pages:"), GUISkinManager.Text.GetStyle("normal16"), GUILayout.Height(37f));
        for (int i = 1; i <= 10 && i <= ClanManager.MaxPage; i++)
        {
            if (GUILayout.Button(i.ToString(), GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Height(27f), GUILayout.Width(27f)))
            {
                ClanManager.SelectedPage = i;
            }
        }
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(34f));
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("Find by tag:"), GUISkinManager.Text.GetStyle("menuTitle02"));
        GUILayout.Space(2f);
        string text = GUILayout.TextField(GUIClanList.searchTag, 20, GUISkinManager.Main.GetStyle("textfield02"), GUILayout.Width(125f), GUILayout.Height(29f));
        if (!ClanManager.IsSearchInProgess)
        {
            GUIClanList.searchTag = text;
        }
        GUILayout.Space(2f);
        if (ClanManager.IsSearchResult)
        {
            if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnDelete"), GUILayout.Width(32f), GUILayout.Height(32f)))
            {
                ClanManager.IsSearchResult = false;
            }
            GUILayout.Space(3f);
        }
        if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnSearch"), GUILayout.Width(32f), GUILayout.Height(32f)))
        {
            ClanManager.Search(GUIClanList.searchTag);
            ClanManager.IsSearchResult = true;
        }
        GUILayout.Space(3f);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(32f));
        GUILayout.Space(5f);
        GUILayout.Label(LanguageManager.GetText("Rank"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(66f));
        GUILayout.Label(LanguageManager.GetText("Name"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(276f));
        GUILayout.Label(LanguageManager.GetText("Level"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(85f));
        GUILayout.Label(LanguageManager.GetText("Exp"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(85f));
        GUILayout.Label(LanguageManager.GetText("Fighters"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(90f));
        GUILayout.Label(GUIContent.none, GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(110f));
        GUILayout.Space(34f);
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(2f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUIClanList.userScroll = GUILayout.BeginScrollView(GUIClanList.userScroll, false, true, GUILayout.Height(263f));
        List<Clan> list = null;
        int num = (ClanManager.SelectedPage - 1) * 100;
        if (ClanManager.IsSearchResult)
        {
            if (ClanManager.SearchResult != null)
            {
                list = ClanManager.SearchResult.Clans;
            }
        }
        else if (ClanManager.SelectedClanList != null)
        {
            list = ClanManager.SelectedClanList.Clans;
        }
        if (list != null)
        {
            List<Clan>.Enumerator enumerator = list.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Clan current = enumerator.Current;
                    num++;
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                    GUILayout.Space(4f);
                    GUILayout.BeginHorizontal(GUIContent.none, (current.FounderID != LocalUser.UserID) ? GUIStyle.none : GUISkinManager.Backgound.GetStyle("roomActive"));
                    GUILayout.Space(5f);
                    GUILayout.Label(num.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(66f));
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(276f));
                    if (current.Arm != null)
                    {
                        GUILayout.Label(current.Arm.Ico, GUIStyle.none, GUILayout.Width(32f), GUILayout.Height(32f));
                    }
                    GUILayout.Space(5f);
                    GUILayout.Label(string.Format("[{0}]", current.Tag), GUISkinManager.Text.GetStyle("room02"), GUILayout.Width(70f));
                    GUILayout.Space(2f);
                    GUILayout.Label(current.Name, GUISkinManager.Text.GetStyle("room02"));
                    GUILayout.Space(5f);
                    if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnInfo"), GUILayout.Width(32f), GUILayout.Height(32f)))
                    {
                        ClanManager.View(current.ClanID);
                        MenuSelecter.ClanHallMenuSelect = MenuSelecter.ClanHallMenuEnum.Main;
                        MenuSelecter.ClanMenuSelect = MenuSelecter.ClanMenuEnum.Hall;
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Label(current.Level.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(85f));
                    GUILayout.Label(current.Exp.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(85f));
                    GUILayout.Label(string.Format("{0} / {1}", current.MemberCount, current.MaxMemberCount), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(90f));
                    if (current.Access == 0)
                    {
                        GUILayout.Label(LanguageManager.GetText("(selection is closed)"), GUISkinManager.Text.GetStyle("roomDisable"), GUILayout.Width(110f));
                    }
                    else if (current.AccessLvl > LocalUser.Level)
                    {
                        GUILayout.Label(LanguageManager.GetTextFormat("(from lvl. {0})", current.AccessLvl), GUISkinManager.Text.GetStyle("roomDisable"), GUILayout.Width(110f));
                    }
                    else if (LocalUser.Clan == null)
                    {
                        if (current.SendInvite)
                        {
                            if (ClanManager.LocalInvites.Contains(current.ClanID))
                            {
                                GUILayout.Label(LanguageManager.GetText("(Pending)"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(110f));
                            }
                            else
                            {
                                GUILayout.Label(LanguageManager.GetText("(Rejected)"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(110f));
                            }
                        }
                        else if (ClanManager.ClanUsedRequest >= ClanManager.ClanMaxRequest)
                        {
                            GUILayout.Label(LanguageManager.GetText("No requests"), GUISkinManager.Text.GetStyle("roomDisable"), GUILayout.Width(110f));
                        }
                        else if (GUILayout.Button(LanguageManager.GetText("Send request"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(110f), GUILayout.Height(32f)))
                        {
                            ClanManager.Join(current.ClanID);
                        }
                    }
                    else if (current.MemberCount < current.MaxMemberCount)
                    {
                        GUILayout.Label(LanguageManager.GetText("(selection is open)"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(110f));
                    }
                    else
                    {
                        GUILayout.Label(LanguageManager.GetText("(selection is closed)"), GUISkinManager.Text.GetStyle("roomDisable"), GUILayout.Width(110f));
                    }
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
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.Space(3f);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(50f));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(37f));
        GUILayout.Space(15f);
        GUILayout.Label(LanguageManager.GetTextFormat("Total requests: {0}/{1}", ClanManager.ClanUsedRequest, ClanManager.ClanMaxRequest), GUISkinManager.Text.GetStyle("normal16"), GUILayout.Height(37f));
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("Buy 5 more requests:"), GUISkinManager.Text.GetStyle("normal16"), GUILayout.Height(37f));
        GUILayout.Space(3f);
        GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("money"), GUILayout.Width(36f), GUILayout.Height(37f));
        GUILayout.Space(3f);
        GUILayout.Label(ClanManager.COST_REQUESTS.ToString(), GUISkinManager.Label.GetStyle("money"));
        GUILayout.Space(10f);
        if (ClanManager.BuyRequestInProgress)
        {
            GUILayout.Label(LanguageManager.GetText("Buy"), GUISkinManager.Button.GetStyle("greenDisable"), GUILayout.Width(106f), GUILayout.Height(37f));
        }
        else if (GUILayout.Button(LanguageManager.GetText("Buy"), GUISkinManager.Button.GetStyle("green"), GUILayout.Width(106f), GUILayout.Height(37f)))
        {
            ClanManager.BuyRequests();
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


