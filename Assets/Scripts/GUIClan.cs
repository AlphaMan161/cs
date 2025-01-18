// ILSpyBased#2
using UnityEngine;

public class GUIClan : MonoBehaviour
{
    private static bool isInit;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public static void OnGUI()
    {
        if (!GUIClan.isInit)
        {
            GUIClan.isInit = true;
            ClanManager.Init();
        }
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width((float)Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuRow03"), GUILayout.Height(41f));
        if (GUILayout.Button(LanguageManager.GetText("Create"), GUISkinManager.Button.GetStyle((MenuSelecter.ClanMenuSelect != MenuSelecter.ClanMenuEnum.Create) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.ClanMenuSelect = MenuSelecter.ClanMenuEnum.Create;
        }
        if (GUILayout.Button(LanguageManager.GetText("All Clans"), GUISkinManager.Button.GetStyle((MenuSelecter.ClanMenuSelect != MenuSelecter.ClanMenuEnum.List) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.ClanMenuSelect = MenuSelecter.ClanMenuEnum.List;
        }
        if (LocalUser.Clan != null)
        {
            GUIContent gUIContent = new GUIContent(LanguageManager.GetText("My Clan"));
            if (ClanManager.NewRequests && MenuSelecter.ClanMenuSelect != MenuSelecter.ClanMenuEnum.Hall)
            {
                gUIContent.image = (Texture)Resources.Load("GUI/Icons/Alert/message_alert01");
            }
            if (GUILayout.Button(gUIContent, GUISkinManager.Button.GetStyle((MenuSelecter.ClanMenuSelect != MenuSelecter.ClanMenuEnum.Hall || ClanManager.SelectedClan == null || ClanManager.SelectedClan.ClanID != LocalUser.Clan.ClanID) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
            {
                ClanManager.View(LocalUser.Clan.ClanID);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        if (MenuSelecter.ClanMenuSelect == MenuSelecter.ClanMenuEnum.Create)
        {
            GUICreateClan.OnGUI();
        }
        else if (MenuSelecter.ClanMenuSelect == MenuSelecter.ClanMenuEnum.List)
        {
            GUIClanList.OnGUI();
        }
        else if (MenuSelecter.ClanMenuSelect == MenuSelecter.ClanMenuEnum.Hall)
        {
            GUIClanHall.OnGUI();
        }
    }
}


