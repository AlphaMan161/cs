// ILSpyBased#2
using UnityEngine;

public class GUIComrads : MonoBehaviour
{
    public static void OnGUI()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width((float)Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuRow03"), GUILayout.Height(41f));
        GUIContent gUIContent = new GUIContent(LanguageManager.GetText("Comrades"));
        if (MasterServerNetworkController.Instance.FriendList != null && MasterServerNetworkController.Instance.FriendList.Request.Count > 0)
        {
            goto IL_0097;
        }
        if (MasterServerNetworkController.NewPrivateMessages)
        {
            goto IL_0097;
        }
        goto IL_00b7;
        IL_00b7:
        if (GUILayout.Button(gUIContent, GUISkinManager.Button.GetStyle((MenuSelecter.ComradsMenuSelect != MenuSelecter.ComradsMenuEnum.Friends) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.ComradsMenuSelect = MenuSelecter.ComradsMenuEnum.Friends;
        }
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        if (MenuSelecter.ComradsMenuSelect == MenuSelecter.ComradsMenuEnum.Friends)
        {
            GUIFriends.OnGUI();
        }
        else if (MenuSelecter.ComradsMenuSelect != MenuSelecter.ComradsMenuEnum.Clan)
        {
            return;
        }
        return;
        IL_0097:
        if (MenuSelecter.ComradsMenuSelect != MenuSelecter.ComradsMenuEnum.Friends)
        {
            gUIContent.image = (Texture)Resources.Load("GUI/Icons/Alert/message_alert01");
        }
        goto IL_00b7;
    }
}


