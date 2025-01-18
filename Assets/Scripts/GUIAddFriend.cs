// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIAddFriend : MonoBehaviour
{
    private static Vector2 friendChatScroll = Vector2.zero;

    private static bool isSearching = false;

    private static bool isFoundUser = true;

    private static string searchingName = string.Empty;

    private static Dictionary<int, string> searchedNames = new Dictionary<int, string>();

    private static Vector2 requestListScroll = new Vector2(0f, 0f);

    public static void OnGUI()
    {
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(36f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(5f);
        GUILayout.Label(LanguageManager.GetText("Add a comrade by name"), GUISkinManager.Text.GetStyle("menuTitle02"));
        GUILayout.Space(4f);
        if (!GUIAddFriend.isSearching)
        {
            GUIAddFriend.searchingName = GUILayout.TextField(GUIAddFriend.searchingName, GUISkinManager.Main.GetStyle("textfield02"), GUILayout.Width(200f), GUILayout.Height(29f));
        }
        else
        {
            GUILayout.Label(GUIAddFriend.searchingName, GUISkinManager.Main.GetStyle("textfield02"), GUILayout.Width(200f), GUILayout.Height(29f));
        }
        GUILayout.Space(4f);
        if (GUILayout.Button(LanguageManager.GetText("Add"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(111f), GUILayout.Height(32f)) && !GUIAddFriend.isSearching)
        {
            GUIAddFriend.SearchName(GUIAddFriend.searchingName);
        }
        GUILayout.Space(5f);
        if (!GUIAddFriend.isFoundUser)
        {
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
            GUILayout.Space(7f);
            GUILayout.Label(LanguageManager.GetText("Comrade not found"), GUISkinManager.Text.GetStyle("error01"));
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Space(2f);
        GUIAddFriend.requestListScroll = GUILayout.BeginScrollView(GUIAddFriend.requestListScroll, false, true, GUILayout.Height(344f));
        if (MasterServerNetworkController.Instance.FriendList != null)
        {
            Dictionary<int, Friend>.ValueCollection.Enumerator enumerator = MasterServerNetworkController.Instance.FriendList.NotConfirm.Values.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    SocialPlayer current = enumerator.Current;
                    if (current != null)
                    {
                        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                        GUILayout.Space(4f);
                        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(36f));
                        GUILayout.Space(11f);
                        GUILayout.Label(LanguageManager.GetTextFormat("{0} (level {1})", current.Name, current.Level), GUISkinManager.Text.GetStyle("room"));
                        GUILayout.Space(5f);
                        if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnInfo"), GUILayout.Width(20f), GUILayout.Height(20f)))
                        {
                            StatisticManager.View(current.UserID);
                        }
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(LanguageManager.GetText("(comrade request sent successfully)"), GUISkinManager.Text.GetStyle("room"));
                        GUILayout.Space(120f);
                        if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnDelete"), GUILayout.Width(32f), GUILayout.Height(32f)))
                        {
                            MasterServerNetworkController.FriendRemove(current.UserID);
                        }
                        GUILayout.Space(4f);
                        GUILayout.EndHorizontal();
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
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.Space(4f);
        GUILayout.EndHorizontal();
    }

    private static void SearchName(string name)
    {
        GUIAddFriend.isFoundUser = true;
        name = name.Trim();
        if (!(name == string.Empty))
        {
            AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.SEARCH_PLAYER_URL + "&v=" + name, name);
            ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(GUIAddFriend.OnSearchName);
            Ajax.Request(ajaxRequest);
        }
    }

    private static void OnSearchName(object result, AjaxRequest request)
    {
        GUIAddFriend.searchedNames.Clear();
        JSONObject jSONObject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
        if (jSONObject.GetField("names") != null && jSONObject.GetField("names").type == JSONObject.Type.ARRAY)
        {
            for (int i = 0; i < jSONObject.GetField("names").Count; i++)
            {
                if (jSONObject.GetField("names")[i].type != 0)
                {
                    int key = Convert.ToInt32(jSONObject.GetField("names")[i].GetField("i").str);
                    GUIAddFriend.searchedNames.Add(key, jSONObject.GetField("names")[i].GetField("n").str);
                }
            }
        }
        Dictionary<int, string>.Enumerator enumerator = GUIAddFriend.searchedNames.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, string> current = enumerator.Current;
                if (current.Key != LocalUser.UserID)
                {
                    MasterServerNetworkController.SendFriendRequest(current.Key);
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        if (GUIAddFriend.searchedNames.Count == 0)
        {
            GUIAddFriend.isFoundUser = false;
        }
    }
}


