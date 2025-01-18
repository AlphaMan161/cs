// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class SetNamePopup : MonoBehaviour
{
    private enum NameState
    {
        Valid = 1,
        NotValid,
        Checking
    }

    private static string userName = string.Empty;

    private static string checkedName = string.Empty;

    private static bool isSetted = false;

    private static NameState nameState = NameState.NotValid;

    private static List<string> availableNames = new List<string>();

    private static bool isShowAvailableNames = false;

    private static int showAvailableSelectIndex = 0;

    private static ErrorInfo.CODE lastError = ErrorInfo.CODE.NONE;

    private static Vector2 WindowSize = new Vector2(496f, 362f);

    private static Rect windowRect = new Rect(0f, 0f, 0f, 0f);

    private static bool isShow = false;

    public static bool Show
    {
        get
        {
            return SetNamePopup.isShow;
        }
        set
        {
            SetNamePopup.isShow = value;
        }
    }

    public static void OnGUI()
    {
        SetNamePopup.isShow = true;
        if (SetNamePopup.userName == string.Empty && !SetNamePopup.isSetted && LocalUser.FullName != string.Empty)
        {
            SetNamePopup.userName = LocalUser.FullName;
            SetNamePopup.userName = SetNamePopup.userName.Replace(" ", string.Empty);
            if (SetNamePopup.userName.Length > 16)
            {
                SetNamePopup.userName = SetNamePopup.userName.Substring(0, 16);
            }
            SetNamePopup.isSetted = true;
        }
        GUISkin skin = GUI.skin;
        GUI.skin = GUISkinManager.Main;
        if (Time.frameCount % 30 == 0)
        {
            SetNamePopup.windowRect = new Rect(((float)Screen.width - SetNamePopup.WindowSize.x) * 0.5f, ((float)Screen.height - SetNamePopup.WindowSize.y) * 0.5f, SetNamePopup.WindowSize.x, SetNamePopup.WindowSize.y);
        }
        GUIHover.Enable = false;
        GUI.Window(4, SetNamePopup.windowRect, new GUI.WindowFunction(SetNamePopup.DrawPopup), string.Empty, GUISkinManager.Backgound.window);
        GUI.Button(SetNamePopup.windowRect, GUIContent.none, GUIStyle.none);
        GUI.skin = skin;
    }

    private static void DrawPopup(int windowId)
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Name yourself"), GUISkinManager.Text.GetStyle("popupTitle"), GUILayout.Height(34f));
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Please, enter the name which will be shown to other players."), GUISkinManager.Label.GetStyle("notificationDesc"));
        GUILayout.Space(15f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUI.SetNextControlName("userNameInput");
        GUILayout.Label(LanguageManager.GetText("Name:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Height(29f));
        GUI.SetNextControlName("userNameInput");
        if (SetNamePopup.nameState != NameState.Checking)
        {
            SetNamePopup.userName = GUILayout.TextField(SetNamePopup.userName, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.MinWidth(226f));
        }
        else
        {
            GUILayout.TextField(SetNamePopup.userName, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.MinWidth(226f));
        }
        GUI.FocusControl("userNameInput");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        if (SetNamePopup.lastError != 0)
        {
            GUILayout.Space(5f);
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(20f));
            GUILayout.FlexibleSpace();
            GUILayout.Label(LanguageManager.GetTextFormat("'{0}' " + SetNamePopup.lastError.GetDescription(), SetNamePopup.checkedName), GUISkinManager.Text.GetStyle("error01"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (SetNamePopup.lastError != ErrorInfo.CODE.USER_NAME_EXISTS)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                GUILayout.Label(LanguageManager.GetText("The name of the character should be created according to the rules:"), GUISkinManager.Text.GetStyle("normal07"));
                GUILayout.Label(LanguageManager.GetText("- character’s name can contain from 3 to 16 characters"), GUISkinManager.Text.GetStyle("normal07"));
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
        else if (SetNamePopup.nameState != NameState.Valid)
        {
            goto IL_02b4;
        }
        goto IL_02b4;
        IL_02b4:
        if (SetNamePopup.isShowAvailableNames)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.Label(LanguageManager.GetText("You may choose from the following:"), GUISkinManager.Label.GetStyle("notificationDesc"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
            SetNamePopup.showAvailableSelectIndex = GUILayout.SelectionGrid(SetNamePopup.showAvailableSelectIndex, SetNamePopup.availableNames.ToArray(), 1, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Width(122f));
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(LanguageManager.GetText("Ok"), GUISkinManager.Button.GetStyle("green"), GUILayout.MinWidth(106f), GUILayout.Height(37f)))
        {
            SetNamePopup.ConfirmName();
        }
        GUILayout.Space(5f);
        if (!GUILayout.Button(LanguageManager.GetText("Close"), GUISkinManager.Button.GetStyle("cancel"), GUILayout.Width(106f), GUILayout.Height(37f)) || !(LocalUser.Name != string.Empty))
        {
            ;
        }
        GUILayout.EndHorizontal();
    }

    private static void ConfirmName()
    {
        if (SetNamePopup.isShowAvailableNames && SetNamePopup.checkedName == SetNamePopup.userName)
        {
            SetNamePopup.userName = SetNamePopup.availableNames[SetNamePopup.showAvailableSelectIndex];
        }
        SetNamePopup.checkedName = SetNamePopup.userName;
        string text = SetNamePopup.userName;
        Ajax.Request(WebUrls.CHANGE_NAME_URL + "&v=" + text + "&ve=" + WWW.EscapeURL(text), new AjaxRequest.AjaxHandler(SetNamePopup.OnChangeNameResult));
    }

    private static void OnChangeNameResult(object result, AjaxRequest request)
    {
        JSONObject jSONObject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
        if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
        {
            SetNamePopup.nameState = NameState.Valid;
            SetNamePopup.availableNames.Clear();
            SetNamePopup.isShowAvailableNames = false;
            LocalUser.Name = SetNamePopup.userName;
            SetNamePopup.isShow = false;
            MasterServerNetworkController.ConnectToMaster(ServerListBehaviour.Instance);
            GameLogicServerNetworkController.ConnectToGameLogic(ServerListBehaviour.Instance);
        }
        else
        {
            SetNamePopup.nameState = NameState.NotValid;
            SetNamePopup.availableNames.Clear();
            SetNamePopup.isShowAvailableNames = false;
        }
        if (jSONObject.GetField("names") != null && jSONObject.GetField("names").type == JSONObject.Type.ARRAY)
        {
            SetNamePopup.availableNames.Clear();
            for (int i = 0; i < jSONObject.GetField("names").Count; i++)
            {
                if (jSONObject.GetField("names")[i].type != 0)
                {
                    SetNamePopup.availableNames.Add(jSONObject.GetField("names")[i].str);
                }
            }
            SetNamePopup.isShowAvailableNames = true;
        }
        if (jSONObject.GetField("err") != null && jSONObject.GetField("err").type == JSONObject.Type.ARRAY)
        {
            SetNamePopup.lastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("err")[0].n);
        }
        else
        {
            SetNamePopup.lastError = ErrorInfo.CODE.NONE;
        }
    }
}


