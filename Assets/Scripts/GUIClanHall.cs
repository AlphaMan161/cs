// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIClanHall : MonoBehaviour
{
    private static Vector2 scroll = new Vector2(0f, 0f);

    private static int money2treasury = 0;

    private static ClanEvent clanEventClick2delete = null;

    private static ClanMember memberTmp = null;

    private static string clanName = string.Empty;

    private static string clanTag = string.Empty;

    private static string clanUrl = string.Empty;

    private static string clanDesc = string.Empty;

    private static int clanAccess = 0;

    private static int clanAccessLvl = 0;

    private static ClanArm clanArm = null;

    private static Vector2 armScroll = new Vector2(0f, 0f);

    private static Vector2 ownerScroll = new Vector2(0f, 0f);

    private static ClanMember newOwner = null;

    private static bool isInitedValues = false;

    public static void OnGUI()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Main)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("Clanhall"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("Clanhall"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            MenuSelecter.ClanHallMenuSelect = MenuSelecter.ClanHallMenuEnum.Main;
        }
        if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Members)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("Clan members"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("Clan members"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            MenuSelecter.ClanHallMenuSelect = MenuSelecter.ClanHallMenuEnum.Members;
        }
        if (ClanManager.SelectedClan != null && ClanManager.SelectedClan.FounderID == LocalUser.UserID)
        {
            GUIContent gUIContent = new GUIContent(LanguageManager.GetText("Requests"));
            if (ClanManager.NewRequests && MenuSelecter.ClanHallMenuSelect != MenuSelecter.ClanHallMenuEnum.Invites)
            {
                gUIContent.image = (Texture)Resources.Load("GUI/Icons/Alert/message_alert01");
            }
            if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Invites)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
                GUILayout.Label(gUIContent, GUISkinManager.Text.GetStyle("partActiveSmall"));
                GUILayout.EndHorizontal();
            }
            else if (GUILayout.Button(gUIContent, GUISkinManager.Text.GetStyle("partSmall")))
            {
                MenuSelecter.ClanHallMenuSelect = MenuSelecter.ClanHallMenuEnum.Invites;
            }
            if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Edit)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
                GUILayout.Label(LanguageManager.GetText("Clan settings"), GUISkinManager.Text.GetStyle("partActiveSmall"));
                GUILayout.EndHorizontal();
            }
            else if (GUILayout.Button(LanguageManager.GetText("Clan settings"), GUISkinManager.Text.GetStyle("partSmall")))
            {
                MenuSelecter.ClanHallMenuSelect = MenuSelecter.ClanHallMenuEnum.Edit;
            }
        }
        if (ClanManager.SelectedClan != null && LocalUser.Clan != null && LocalUser.Clan.ClanID == ClanManager.SelectedClan.ClanID)
        {
            if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Treasury)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
                GUILayout.Label(LanguageManager.GetText("Treasury"), GUISkinManager.Text.GetStyle("partActiveSmall"));
                GUILayout.EndHorizontal();
            }
            else if (GUILayout.Button(LanguageManager.GetText("Treasury"), GUISkinManager.Text.GetStyle("partSmall")))
            {
                MenuSelecter.ClanHallMenuSelect = MenuSelecter.ClanHallMenuEnum.Treasury;
            }
            if (ClanManager.SelectedClan.Events.Count > 0)
            {
                GUIContent gUIContent2 = new GUIContent(LanguageManager.GetText("Attention"));
                gUIContent2.image = (Texture)Resources.Load("GUI/Icons/Alert/message_alert01");
                if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Events)
                {
                    GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
                    GUILayout.Label(gUIContent2, GUISkinManager.Text.GetStyle("partActiveSmall"));
                    GUILayout.EndHorizontal();
                }
                else if (GUILayout.Button(gUIContent2, GUISkinManager.Text.GetStyle("partSmall")))
                {
                    MenuSelecter.ClanHallMenuSelect = MenuSelecter.ClanHallMenuEnum.Events;
                }
            }
            if (ClanShopManager.Enhancers.Count > 0)
            {
                if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Enhancers)
                {
                    GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
                    GUILayout.Label(LanguageManager.GetText("Enhancers"), GUISkinManager.Text.GetStyle("partActiveSmall"));
                    GUILayout.EndHorizontal();
                }
                else if (GUILayout.Button(LanguageManager.GetText("Enhancers"), GUISkinManager.Text.GetStyle("partSmall")))
                {
                    MenuSelecter.ClanHallMenuSelect = MenuSelecter.ClanHallMenuEnum.Enhancers;
                }
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(2f);
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Main && ClanManager.SelectedClan != null)
        {
            GUIClanHall.DrawClanInfo();
        }
        else if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Members && ClanManager.SelectedClan != null)
        {
            GUIClanHall.DrawMembers();
        }
        else if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Edit)
        {
            GUIClanHall.DrawEdit();
        }
        else if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Invites)
        {
            GUIClanHall.DrawInvites();
        }
        else if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Events)
        {
            GUIClanHall.DrawEvents();
        }
        else if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Treasury)
        {
            GUIClanHall.DrawTreasury();
        }
        else if (MenuSelecter.ClanHallMenuSelect == MenuSelecter.ClanHallMenuEnum.Enhancers)
        {
            GUIClanHall.DrawEnhancers();
        }
        else
        {
            GUIClanHall.scroll = GUILayout.BeginScrollView(GUIClanHall.scroll, false, true, GUILayout.MinHeight(378f));
            GUILayout.EndScrollView();
        }
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private static void DrawClanInfo()
    {
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(20f);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Height(116f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        if (ClanManager.SelectedClan.Arm != null)
        {
            GUILayout.Label(ClanManager.SelectedClan.Arm.Ico, GUIStyle.none, GUILayout.Width(100f), GUILayout.Height(100f));
        }
        GUILayout.Space(10f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Label(string.Format("[{0}] {1}", ClanManager.SelectedClan.Tag, ClanManager.SelectedClan.Name), GUISkinManager.Text.GetStyle("normal06"), GUILayout.Height(32f));
        GUILayout.Space(3f);
        GUILayout.Label(LanguageManager.GetTextFormat("{0} level", ClanManager.SelectedClan.Level), GUISkinManager.Text.GetStyle("normal06"), GUILayout.Height(32f));
        GUILayout.Space(3f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Site:"), GUISkinManager.Text.GetStyle("normal06"), GUILayout.Height(32f));
        GUILayout.Space(3f);
        if (GUILayout.Button(ClanManager.SelectedClan.Homepage, GUISkinManager.Text.GetStyle("normal06W"), GUILayout.Height(32f)) && ClanManager.SelectedClan.Homepage != string.Empty)
        {
            WebCall.OpenUrl(ClanManager.SelectedClan.Homepage);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Clan Leader:"), GUISkinManager.Text.GetStyle("normal06"), GUILayout.Height(32f));
        GUILayout.Space(3f);
        GUILayout.Label(ClanManager.SelectedClan.FounderName, GUISkinManager.Text.GetStyle("normal06W"), GUILayout.Height(32f));
        GUILayout.Space(3f);
        if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnInfo"), GUILayout.Width(32f), GUILayout.Height(32f)) && ClanManager.SelectedClan.FounderID != 0)
        {
            StatisticManager.View(ClanManager.SelectedClan.FounderID);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Clan experience:"), GUISkinManager.Text.GetStyle("normal06"), GUILayout.Height(32f));
        GUILayout.Space(3f);
        GUILayout.Label(ClanManager.SelectedClan.Exp.ToString(), GUISkinManager.Text.GetStyle("normal06W"), GUILayout.Height(32f));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Clan members:"), GUISkinManager.Text.GetStyle("normal06"), GUILayout.Height(32f));
        GUILayout.Space(3f);
        GUILayout.Label(string.Format("{0}/{1}", ClanManager.SelectedClan.MemberCount, ClanManager.SelectedClan.MaxMemberCount), GUISkinManager.Text.GetStyle("normal06W"), GUILayout.Height(32f));
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        if (LocalUser.Clan != null && ClanManager.SelectedClan.ClanID == LocalUser.Clan.ClanID)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Label(LanguageManager.GetText("Experience percentage which shared to clan:"), GUISkinManager.Text.GetStyle("normal06"), GUILayout.Height(28f));
            GUILayout.Space(3f);
            int num = GUILayout.SelectionGrid(ClanManager.SelectedIndexKoef, ClanManager.AvailableKoef, ClanManager.AvailableKoef.Length, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Height(28f), GUILayout.MinWidth(400f));
            if (ClanManager.SelectedIndexKoef != num)
            {
                ClanManager.SelectedIndexKoef = num;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);
        }
        GUILayout.EndVertical();
        GUILayout.Space(20f);
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Height(68f));
        GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("attention"), GUILayout.Width(44f), GUILayout.Height(44f));
        GUILayout.Space(5f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("- Share your experience with clan to make it grow faster"), GUISkinManager.Text.GetStyle("normal06"));
        GUILayout.Label(LanguageManager.GetText("- Rules violation upon p.3.1 will be punished up to clan deletion without prior notice"), GUISkinManager.Text.GetStyle("normal06"));
        GUILayout.Label(LanguageManager.GetText("- Statistics refresh ones within 24 hours"), GUISkinManager.Text.GetStyle("normal06"));
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Space(20f);
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(20f);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Height(152f - ((LocalUser.Clan == null || ClanManager.SelectedClan.ClanID != LocalUser.Clan.ClanID) ? 0f : 32f)), GUILayout.Width(715f));
        GUIClanHall.scroll = GUILayout.BeginScrollView(GUIClanHall.scroll, false, true, GUILayout.Height(136f - ((LocalUser.Clan == null || ClanManager.SelectedClan.ClanID != LocalUser.Clan.ClanID) ? 0f : 32f)));
        GUILayout.Label(ClanManager.SelectedClan.Desc, GUISkinManager.Text.GetStyle("normal06"));
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.Space(20f);
        GUILayout.EndHorizontal();
        GUILayout.Space(28f);
    }

    private static void DrawTreasury()
    {
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(20f);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Height(116f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        if (ClanManager.SelectedClan.Arm != null)
        {
            GUILayout.Label(ClanManager.SelectedClan.Arm.Ico, GUIStyle.none, GUILayout.Width(100f), GUILayout.Height(100f));
        }
        GUILayout.Space(10f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Label(string.Format("[{0}] {1}", ClanManager.SelectedClan.Tag, ClanManager.SelectedClan.Name), GUISkinManager.Text.GetStyle("normal06"), GUILayout.Height(32f));
        GUILayout.Space(3f);
        GUILayout.Label(LanguageManager.GetTextFormat("{0} level", ClanManager.SelectedClan.Level), GUISkinManager.Text.GetStyle("normal06"), GUILayout.Height(32f));
        GUILayout.Space(3f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Site:"), GUISkinManager.Text.GetStyle("normal06"), GUILayout.Height(32f));
        GUILayout.Space(3f);
        if (GUILayout.Button(ClanManager.SelectedClan.Homepage, GUISkinManager.Text.GetStyle("normal06W"), GUILayout.Height(32f)) && ClanManager.SelectedClan.Homepage != string.Empty)
        {
            WebCall.OpenUrl(ClanManager.SelectedClan.Homepage);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("In Treasury:"), GUISkinManager.Text.GetStyle("normal06"), GUILayout.Height(32f));
        GUILayout.Space(3f);
        GUILayout.Label(ClanManager.SelectedClan.Money.ToString(), GUISkinManager.Text.GetStyle("normal06W"), GUILayout.Height(32f));
        GUILayout.EndHorizontal();
        if (ClanManager.SelectedClan.BestDonate != null)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Label(LanguageManager.GetText("The greatest contribution:"), GUISkinManager.Text.GetStyle("normal06"), GUILayout.Height(32f));
            GUILayout.Space(3f);
            GUILayout.Label(string.Format("({0})", ClanManager.SelectedClan.BestDonate.Money), GUISkinManager.Text.GetStyle("normal06W"), GUILayout.Height(32f));
            GUILayout.Space(3f);
            GUILayout.Label(ClanManager.SelectedClan.BestDonate.UserName, GUISkinManager.Text.GetStyle("normal06W"), GUILayout.Height(32f));
            GUILayout.Space(3f);
            if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnInfo"), GUILayout.Width(32f), GUILayout.Height(32f)) && ClanManager.SelectedClan.FounderID != 0)
            {
                StatisticManager.View(ClanManager.SelectedClan.FounderID);
            }
            GUILayout.EndHorizontal();
        }
        if (ClanManager.SelectedClan.LastDonate != null)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            GUILayout.Label(LanguageManager.GetText("Last contribution:"), GUISkinManager.Text.GetStyle("normal06"), GUILayout.Height(32f));
            GUILayout.Space(3f);
            GUILayout.Label(string.Format("({0})", ClanManager.SelectedClan.LastDonate.Money), GUISkinManager.Text.GetStyle("normal06W"), GUILayout.Height(32f));
            GUILayout.Space(3f);
            GUILayout.Label(ClanManager.SelectedClan.LastDonate.UserName, GUISkinManager.Text.GetStyle("normal06W"), GUILayout.Height(32f));
            GUILayout.Space(3f);
            if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnInfo"), GUILayout.Width(32f), GUILayout.Height(32f)) && ClanManager.SelectedClan.FounderID != 0)
            {
                StatisticManager.View(ClanManager.SelectedClan.LastDonate.UserId);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.Space(20f);
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(36f));
        GUILayout.Space(15f);
        GUILayout.Label(LanguageManager.GetText("The history of money transactions:"), GUISkinManager.Text.GetStyle("menuTitle02"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(2f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUIClanHall.scroll = GUILayout.BeginScrollView(GUIClanHall.scroll, false, true, GUILayout.Height(152f));
        List<ClanTreasuryEvent>.Enumerator enumerator = ClanManager.SelectedClan.TreasuryEvents.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ClanTreasuryEvent current = enumerator.Current;
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                GUILayout.Space(4f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Space(11f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(226f));
                GUILayout.FlexibleSpace();
                GUILayout.Label(current.UserName, GUISkinManager.Text.GetStyle("room02"));
                GUILayout.Space(5f);
                if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnInfo"), GUILayout.Width(32f), GUILayout.Height(32f)) && current.UserId != 0)
                {
                    StatisticManager.View(current.UserId);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(120f));
                GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("money"), GUILayout.Width(32f), GUILayout.Height(32f));
                GUILayout.Space(2f);
                if (current.Type == ClanTreasuryEventType.Add)
                {
                    GUILayout.Label(string.Format("+{0}", current.Money), GUISkinManager.Text.GetStyle("room"));
                }
                else
                {
                    GUILayout.Label(string.Format("-{0}", current.Money), GUISkinManager.Text.GetStyle("roomDisable"));
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Label(current.Type.GetDesc(), GUISkinManager.Text.GetStyle((current.Type != ClanTreasuryEventType.Add) ? "roomDisable" : "room"), GUILayout.Width(226f));
                GUILayout.Label(current.Date, GUISkinManager.Text.GetStyle((current.Type != ClanTreasuryEventType.Add) ? "roomDisable" : "room"), GUILayout.Width(140f));
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
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(50f));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(37f));
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("Add money to Treasury:"), GUISkinManager.Text.GetStyle("normal16"), GUILayout.Height(37f));
        GUILayout.Space(3f);
        GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("money"), GUILayout.Width(36f), GUILayout.Height(37f));
        GUILayout.Space(5f);
        string value = GUILayout.TextField(GUIClanHall.money2treasury.ToString(), GUISkinManager.Main.GetStyle("money"), GUILayout.MinWidth(50f));
        try
        {
            GUIClanHall.money2treasury = Convert.ToInt32(value);
        }
        catch (Exception)
        {
            GUIClanHall.money2treasury = 0;
        }
        GUILayout.Space(10f);
        if (GUILayout.Button(LanguageManager.GetText("Add"), GUISkinManager.Button.GetStyle("green"), GUILayout.Width(146f), GUILayout.Height(37f)))
        {
            ClanManager.AddMoney(ClanManager.SelectedClan.ClanID, GUIClanHall.money2treasury);
        }
        GUILayout.Space(10f);
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.Space(15f);
    }

    private static void DrawEvents()
    {
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(2f);
        GUIClanHall.scroll = GUILayout.BeginScrollView(GUIClanHall.scroll, false, true, GUILayout.Height(365f));
        List<ClanEvent>.Enumerator enumerator = ClanManager.SelectedClan.Events.GetEnumerator();
        try
        {
            for (; enumerator.MoveNext(); GUILayout.EndHorizontal(), GUILayout.Space(20f), GUILayout.EndHorizontal(), GUILayout.Space(5f))
            {
                ClanEvent current = enumerator.Current;
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Space(20f);
                GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Height(68f));
                GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("attention"), GUILayout.Width(44f), GUILayout.Height(44f));
                GUILayout.Space(5f);
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                if (current.Type == ClanEventType.Delete)
                {
                    GUILayout.Space(10f);
                    GUILayout.Label(LanguageManager.GetText("- Clan was sent for deletion. Only Clan Leader can cancel the removal"), GUISkinManager.Text.GetStyle("normal06"));
                    GUILayout.Label(LanguageManager.GetTextFormat("- Clan will be deleted within {0}", current.Duration.ToString()), GUISkinManager.Text.GetStyle("normal06"));
                }
                if (current.Type == ClanEventType.ChangeOwner)
                {
                    GUILayout.Space(10f);
                    GUILayout.Label(LanguageManager.GetTextFormat("- New Clan Leader will be assigned to the clan"), GUISkinManager.Text.GetStyle("normal06"));
                    GUILayout.Label(LanguageManager.GetTextFormat("- {0} will be assigned as a Clan Leader to the clan within {1}", current.Data["n"], current.Duration.ToString()), GUISkinManager.Text.GetStyle("normal06"));
                }
                if (current.Type == ClanEventType.DeleteMember)
                {
                    GUILayout.Space(10f);
                    GUILayout.Label(LanguageManager.GetTextFormat("- Clan member removing"), GUISkinManager.Text.GetStyle("normal06"));
                    GUILayout.Label(LanguageManager.GetTextFormat("- {0} will be deleted from the clan within {1}", current.Data["n"], current.Duration.ToString()), GUISkinManager.Text.GetStyle("normal06"));
                }
                if (current.Type == ClanEventType.LeaveMember)
                {
                    GUILayout.Space(10f);
                    GUILayout.Label(LanguageManager.GetTextFormat("- Sign out from clan"), GUISkinManager.Text.GetStyle("normal06"));
                    GUILayout.Label(LanguageManager.GetTextFormat("- {0} will be sign out from clan within {1}", current.Data["n"], current.Duration.ToString()), GUISkinManager.Text.GetStyle("normal06"));
                }
                GUILayout.EndVertical();
                if (LocalUser.UserID == ClanManager.SelectedClan.FounderID && current.Type != ClanEventType.LeaveMember)
                {
                    goto IL_036c;
                }
                if (current.CreaterId != 0 && current.CreaterId == LocalUser.UserID && current.Type != ClanEventType.DeleteMember)
                {
                    goto IL_036c;
                }
                continue;
                IL_036c:
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                GUILayout.Space(10f);
                if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnDelete"), GUILayout.Width(32f), GUILayout.Height(32f)))
                {
                    GUIClanHall.clanEventClick2delete = current;
                    NotificationWindow.Add(new Notification(Notification.Type.NOTIFICATION, LanguageManager.GetText("Cancel event"), LanguageManager.GetText("Do you really want to cancel this event?"), LanguageManager.GetText("Approve"), delegate
                    {
                        ClanManager.DeleteEvent(LocalUser.Clan.ClanID, GUIClanHall.clanEventClick2delete.EventId);
                    }, null));
                }
                GUILayout.EndVertical();
                GUILayout.Space(10f);
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        GUILayout.EndScrollView();
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
    }

    private static void DrawEnhancers()
    {
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(2f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUIClanHall.scroll = GUILayout.BeginScrollView(GUIClanHall.scroll, false, true, GUILayout.Height(376f));
        int num = 0;
        int count = ClanShopManager.Enhancers.Count;
        List<Enhancer>.Enumerator enumerator = ClanShopManager.Enhancers.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Enhancer current = enumerator.Current;
                if (num % 7 == 0)
                {
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.Space(6f);
                }
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(97f));
                GUILayout.Label(current.Ico, GUISkinManager.Backgound.GetStyle("itemShop02"), GUILayout.Width(97f), GUILayout.Height(85f));
                GUILayout.EndVertical();
                Rect lastRect = GUILayoutUtility.GetLastRect();
                if (current.IsSale)
                {
                    GUI.Label(new Rect(lastRect.x, lastRect.y, 33f, 29f), GUIContent.none, GUISkinManager.Ico.GetStyle("sale"));
                }
                lastRect.height -= 27f;
                GUIHover.Hover(Event.current, current, lastRect);
                lastRect.x += 68f;
                lastRect.y += 62f;
                lastRect.width = 24f;
                lastRect.height = 17f;
                if (current.IsBuyed)
                {
                    lastRect.width = 24f;
                    lastRect.height = 23f;
                    lastRect.y -= 6f;
                    GUI.Label(lastRect, GUIContent.none, GUISkinManager.Label.GetStyle("isBuyed"));
                    lastRect.y += 6f;
                }
                else if (current.NeedLvl > ClanManager.SelectedClan.Level)
                {
                    lastRect.width = 24f;
                    lastRect.height = 23f;
                    lastRect.y -= 6f;
                    GUI.Label(lastRect, GUIContent.none, GUISkinManager.Label.GetStyle("isDisabled"));
                    lastRect.y += 6f;
                }
                else if (ClanManager.SelectedClan.FounderID == LocalUser.UserID && GUI.Button(lastRect, GUIContent.none, GUISkinManager.Button.GetStyle("buy")))
                {
                    GUIClanHall.OnBuy(current);
                }
                lastRect.x -= 64f;
                lastRect.width = 60f;
                lastRect.y -= 1f;
                if (current.Shop_Cost.Time1VCost != 0)
                {
                    GUI.Label(lastRect, current.Shop_Cost.Time1VCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                }
                else if (current.Shop_Cost.Time7VCost != 0)
                {
                    GUI.Label(lastRect, current.Shop_Cost.Time7VCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                }
                else if (current.Shop_Cost.Time30VCost != 0)
                {
                    GUI.Label(lastRect, current.Shop_Cost.Time30VCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                }
                else if (current.Shop_Cost.TimePVCost != 0)
                {
                    GUI.Label(lastRect, current.Shop_Cost.TimePVCost.ToString(), GUISkinManager.Label.GetStyle("costSmall"));
                }
                GUILayout.Space(6f);
                if (num % 7 == 6 || num == count - 1)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.Space(6f);
                }
                num++;
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
    }

    private static void OnBuy(CCItem item)
    {
        if (item.Shop_Cost.SelectedVCost > ClanManager.SelectedClan.Money)
        {
            ErrorInfo.CODE code = ErrorInfo.CODE.MISSING_MONEY;
            if (item.GetType() == typeof(Weapon))
            {
                code.AddNotification(ErrorInfo.TYPE.BUY_WEAPON);
            }
            else
            {
                code.AddNotification(ErrorInfo.TYPE.BUY_WEAR);
            }
        }
        else
        {
            NotificationWindow.Add(new Notification(Notification.Type.BUY_ITEM, item, new Notification.ButtonClick(GUIClanHall.OnBuyConfirmed), item));
        }
    }

    private static void OnBuyConfirmed(object item)
    {
        if (item.GetType() == typeof(Enhancer))
        {
            ClanShopManager.BuyEnhancer(item as Enhancer);
        }
    }

    private static void DrawInvites()
    {
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(36f));
        GUILayout.Space(11f);
        GUILayout.Label(LanguageManager.GetText("Name"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(300f));
        GUILayout.Label(LanguageManager.GetText("Level"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(90f));
        GUILayout.Label(LanguageManager.GetText("Exp"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(90f));
        GUILayout.Label(LanguageManager.GetText("Date"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(140f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(2f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUIClanHall.scroll = GUILayout.BeginScrollView(GUIClanHall.scroll, false, true, GUILayout.Height(340f));
        List<ClanMember>.Enumerator enumerator = ClanManager.SelectedClan.Invites.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ClanMember current = enumerator.Current;
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                GUILayout.Space(4f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Space(11f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(300f));
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
                GUILayout.Label(current.Exp.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(90f));
                GUILayout.Label(current.Date, GUISkinManager.Text.GetStyle("room"), GUILayout.Width(140f));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnDelete"), GUILayout.Width(32f), GUILayout.Height(32f)))
                {
                    GUIClanHall.memberTmp = current;
                    NotificationWindow.Add(new Notification(Notification.Type.NOTIFICATION, LanguageManager.GetText("Remove invite"), LanguageManager.GetTextFormat("Do you really want to remove the invite {0} from the list?", current.Name), LanguageManager.GetText("Remove invite"), delegate
                    {
                        ClanManager.Reject(GUIClanHall.memberTmp.UserID, LocalUser.Clan.ClanID);
                    }, null));
                }
                GUILayout.Space(10f);
                if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnAdd"), GUILayout.Width(32f), GUILayout.Height(32f)))
                {
                    GUIClanHall.memberTmp = current;
                    ClanManager.Accept(GUIClanHall.memberTmp.UserID, LocalUser.Clan.ClanID);
                }
                GUILayout.Space(8f);
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
    }

    private static void DrawMembers()
    {
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(36f));
        GUILayout.Space(11f);
        if (LocalUser.Clan != null && LocalUser.Clan.ClanID == ClanManager.SelectedClan.ClanID)
        {
            GUILayout.Label(LanguageManager.GetText("Rank"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(66f));
            GUILayout.Label(LanguageManager.GetText("Name"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(278f));
            GUILayout.Label(LanguageManager.GetText("Level"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(90f));
            GUILayout.Label(LanguageManager.GetText("Exp"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(90f));
            GUILayout.Label(LanguageManager.GetText("Exp Clan"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(90f));
            GUILayout.Label(LanguageManager.GetText("Exp Koef"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(62f));
            GUILayout.Space(32f);
        }
        else
        {
            GUILayout.Label(LanguageManager.GetText("Rank"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(66f));
            GUILayout.Label(LanguageManager.GetText("Name"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(340f));
            GUILayout.Label(LanguageManager.GetText("Level"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(90f));
            GUILayout.Label(LanguageManager.GetText("Exp"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(90f));
            GUILayout.Label(LanguageManager.GetText("Exp Clan"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(90f));
            GUILayout.Space(32f);
        }
        GUILayout.Space(32f);
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(2f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUIClanHall.scroll = GUILayout.BeginScrollView(GUIClanHall.scroll, false, true, GUILayout.Height(340f - ((ClanManager.SelectedClan.FounderID != LocalUser.UserID || ClanManager.SelectedClan.IsMaxMemberCount) ? 0f : 50f)));
        int num = 0;
        List<ClanMember>.Enumerator enumerator = ClanManager.SelectedClan.Members.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ClanMember current = enumerator.Current;
                num++;
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                GUILayout.Space(4f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                GUILayout.Space(11f);
                if (LocalUser.Clan != null && LocalUser.Clan.ClanID == ClanManager.SelectedClan.ClanID)
                {
                    GUILayout.Label(num.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(66f));
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(278f));
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
                    GUILayout.Label(current.Exp.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(90f));
                    GUILayout.Label(current.ClanExp.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(90f));
                    GUILayout.Label(current.ClanExpKoef.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(62f));
                    if (ClanManager.SelectedClan.FounderID == LocalUser.UserID && current.UserID != LocalUser.UserID)
                    {
                        if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnDelete"), GUILayout.Width(32f), GUILayout.Height(32f)))
                        {
                            GUIClanHall.memberTmp = current;
                            NotificationWindow.Add(new Notification(Notification.Type.NOTIFICATION, LanguageManager.GetText("Remove member"), LanguageManager.GetTextFormat("Do you really want to remove member {0} from the clan?", current.Name), LanguageManager.GetText("Remove"), delegate
                            {
                                ClanManager.RemoveMemberLazy(GUIClanHall.memberTmp.UserID, LocalUser.Clan.ClanID);
                            }, null));
                        }
                    }
                    else if (current.UserID == LocalUser.UserID && ClanManager.SelectedClan.FounderID != LocalUser.UserID)
                    {
                        if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("btnDelete"), GUILayout.Width(32f), GUILayout.Height(32f)))
                        {
                            NotificationWindow.Add(new Notification(Notification.Type.NOTIFICATION, LanguageManager.GetText("Leave clan"), LanguageManager.GetTextFormat("Do you really want to leave the clan?"), LanguageManager.GetText("Leave clan"), delegate
                            {
                                ClanManager.LeaveLazy(LocalUser.Clan.ClanID);
                            }, null));
                        }
                    }
                    else
                    {
                        GUILayout.Space(32f);
                    }
                }
                else
                {
                    GUILayout.Label(num.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(66f));
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(340f));
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
                    GUILayout.Label(current.Exp.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(90f));
                    GUILayout.Label(current.ClanExp.ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(90f));
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
        GUILayout.FlexibleSpace();
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        if (ClanManager.SelectedClan.FounderID == LocalUser.UserID && !ClanManager.SelectedClan.IsMaxMemberCount)
        {
            ClanManager.COST_EXPAND_CLAN_MEMBER_CURRENT = ClanManager.COST_EXPAN_CLAN_MEMBER[ClanManager.SelectedClan.MaxMemberCount / 5 - 1];
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(50f));
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(37f));
            GUILayout.FlexibleSpace();
            GUILayout.Label(LanguageManager.GetText("Enlarge the members quantity with 5:"), GUISkinManager.Text.GetStyle("normal16"), GUILayout.Height(37f));
            GUILayout.Space(3f);
            GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("money"), GUILayout.Width(36f), GUILayout.Height(37f));
            GUILayout.Space(3f);
            GUILayout.Label(ClanManager.COST_EXPAND_CLAN_MEMBER_CURRENT.ToString(), GUISkinManager.Label.GetStyle("money"));
            GUILayout.Space(10f);
            if (GUILayout.Button(LanguageManager.GetText("Enlarge"), GUISkinManager.Button.GetStyle("green"), GUILayout.Width(146f), GUILayout.Height(37f)))
            {
                ClanManager.BuyMaxMember(ClanManager.SelectedClan.ClanID);
            }
            GUILayout.Space(10f);
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.Space(15f);
        }
        GUILayout.EndVertical();
    }

    private static void DrawEdit()
    {
        if (GUIClanHall.clanName == string.Empty && !GUIClanHall.isInitedValues)
        {
            GUIClanHall.clanName = ClanManager.SelectedClan.Name;
        }
        if (GUIClanHall.clanTag == string.Empty && !GUIClanHall.isInitedValues)
        {
            GUIClanHall.clanTag = ClanManager.SelectedClan.Tag;
        }
        if (GUIClanHall.clanArm == null && !GUIClanHall.isInitedValues)
        {
            GUIClanHall.clanArm = ClanManager.SelectedClan.Arm;
        }
        if (GUIClanHall.clanUrl == string.Empty && !GUIClanHall.isInitedValues)
        {
            GUIClanHall.clanUrl = ClanManager.SelectedClan.Homepage;
        }
        if (GUIClanHall.clanDesc == string.Empty && !GUIClanHall.isInitedValues)
        {
            GUIClanHall.clanDesc = ClanManager.SelectedClan.Desc;
        }
        if (GUIClanHall.clanAccess == 0 && !GUIClanHall.isInitedValues)
        {
            GUIClanHall.clanAccess = ClanManager.SelectedClan.Access;
            UnityEngine.Debug.LogError("clanAccess=" + GUIClanHall.clanAccess);
        }
        if (GUIClanHall.clanAccessLvl == 0 && !GUIClanHall.isInitedValues)
        {
            GUIClanHall.clanAccessLvl = ClanManager.SelectedClan.AccessLvl;
        }
        GUIClanHall.isInitedValues = true;
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(36f));
        GUILayout.Space(15f);
        GUILayout.Label(LanguageManager.GetText("Here you can change the name of the clan, its clan tag and crest."), GUISkinManager.Text.GetStyle("menuTitle02"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.Space(4f);
        GUIClanHall.scroll = GUILayout.BeginScrollView(GUIClanHall.scroll, false, true, GUILayout.Height(290f), GUILayout.Width(750f));
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Clan name:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(160f), GUILayout.Height(29f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUIClanHall.clanName = GUILayout.TextField(GUIClanHall.clanName, 64, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.MaxWidth(257f), GUILayout.Width(241f));
        GUILayout.Label(LanguageManager.GetText("(minimum 3 and maximum 20 symbols)"), GUISkinManager.Text.GetStyle("txtTip"));
        GUILayout.EndVertical();
        GUILayout.Space(5f);
        if (GUILayout.Button(ClanManager.COST_CHANGE_NAME.ToString(), GUISkinManager.Button.GetStyle("buyAbility"), GUILayout.Width(82f), GUILayout.Height(33f)))
        {
            ClanManager.ChangeName(ClanManager.SelectedClan.ClanID, GUIClanHall.clanName);
        }
        if (ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_NAME || ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_NAME_LEN || ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_NAME_EXIST || ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_USER_LVL_LESS || ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_CREATE_YOU_ARE_IN_CLAN)
        {
            GUILayout.Space(10f);
            GUILayout.Label("* " + ClanManager.CurrentLastError.GetDescription(), GUISkinManager.Text.GetStyle("error01"), GUILayout.Height(29f));
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Tag of the clan:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(160f), GUILayout.Height(29f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUIClanHall.clanTag = GUILayout.TextField(GUIClanHall.clanTag, 6, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.MaxWidth(257f), GUILayout.Width(241f));
        GUILayout.Label(LanguageManager.GetText("(minimum 2 and maximum 6 symbols)"), GUISkinManager.Text.GetStyle("txtTip"));
        GUILayout.EndVertical();
        GUILayout.Space(5f);
        if (GUILayout.Button(ClanManager.COST_CHANGE_TAG.ToString(), GUISkinManager.Button.GetStyle("buyAbility"), GUILayout.Width(82f), GUILayout.Height(33f)))
        {
            ClanManager.ChangeTag(ClanManager.SelectedClan.ClanID, GUIClanHall.clanTag);
        }
        if (ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_TAG || ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_TAG_LEN || ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_TAG_EXIST)
        {
            GUILayout.Space(10f);
            GUILayout.Label("* " + ClanManager.CurrentLastError.GetDescription(), GUISkinManager.Text.GetStyle("error01"), GUILayout.Height(29f));
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Clan crest:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(160f), GUILayout.Height(29f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Height(150f), GUILayout.Width(241f));
        GUIClanHall.armScroll = GUILayout.BeginScrollView(GUIClanHall.armScroll, false, true, GUILayout.Height(134f));
        int num = 0;
        int count = ClanArmManager.Arms.Count;
        List<ClanArm>.Enumerator enumerator = ClanArmManager.Arms.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ClanArm current = enumerator.Current;
                if (num % 3 == 0)
                {
                    GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
                    GUILayout.Space(3f);
                }
                if (GUILayout.Button(current.Ico, GUISkinManager.Backgound.GetStyle((GUIClanHall.clanArm != current) ? "clanArm" : "clanArmActive"), GUILayout.Width(64f), GUILayout.Height(64f)))
                {
                    GUIClanHall.clanArm = current;
                }
                if (num % 3 == 2 || num == count - 1)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.Space(3f);
                }
                num++;
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        if (GUIClanHall.clanArm != null && GUIClanHall.clanArm.Cost != null && GUIClanHall.clanArm != ClanManager.SelectedClan.Arm)
        {
            GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
            GUILayout.Space(58f);
            if (GUILayout.Button(GUIClanHall.clanArm.Cost.TimePVCost.ToString(), GUISkinManager.Button.GetStyle("buyAbility"), GUILayout.Width(82f), GUILayout.Height(33f)))
            {
                ClanManager.ChangeArm(ClanManager.SelectedClan.ClanID, GUIClanHall.clanArm.ArmID);
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Site link:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(160f), GUILayout.Height(29f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUIClanHall.clanUrl = GUILayout.TextField(GUIClanHall.clanUrl, 80, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.MaxWidth(257f), GUILayout.Width(241f));
        GUILayout.EndVertical();
        GUILayout.Space(5f);
        if (GUILayout.Button(LanguageManager.GetText("Change"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(82f), GUILayout.Height(33f)))
        {
            ClanManager.ChangeUrl(ClanManager.SelectedClan.ClanID, GUIClanHall.clanUrl);
        }
        if (ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_URL)
        {
            GUILayout.Space(10f);
            GUILayout.Label("* " + ClanManager.CurrentLastError.GetDescription(), GUISkinManager.Text.GetStyle("error01"), GUILayout.Height(29f));
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Clan Description:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(160f), GUILayout.Height(29f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUIClanHall.clanDesc = GUILayout.TextArea(GUIClanHall.clanDesc, GUISkinManager.Main.textArea, GUILayout.Height(150f), GUILayout.Width(241f));
        GUILayout.EndVertical();
        GUILayout.Space(5f);
        if (GUILayout.Button(LanguageManager.GetText("Change"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(82f), GUILayout.Height(33f)))
        {
            ClanManager.ChangeDesc(ClanManager.SelectedClan.ClanID, GUIClanHall.clanDesc);
        }
        if (ClanManager.CurrentLastError == ErrorInfo.CODE.CLAN_DESC)
        {
            GUILayout.Space(10f);
            GUILayout.Label("* " + ClanManager.CurrentLastError.GetDescription(), GUISkinManager.Text.GetStyle("error01"), GUILayout.Height(29f));
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Recruitment to the clan:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(160f), GUILayout.Height(29f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUIClanHall.clanAccess = GUILayout.SelectionGrid(GUIClanHall.clanAccess, new string[2] {
            LanguageManager.GetText("Close"),
            LanguageManager.GetText("Open")
        }, 2, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Height(28f), GUILayout.MinWidth(241f));
        GUILayout.EndVertical();
        GUILayout.Space(5f);
        if (GUILayout.Button(LanguageManager.GetText("Change"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(82f), GUILayout.Height(33f)))
        {
            ClanManager.ChangeAccess(ClanManager.SelectedClan.ClanID, GUIClanHall.clanAccess);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Min. level to enter the Clan:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(160f), GUILayout.Height(29f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        string value = GUILayout.TextField(GUIClanHall.clanAccessLvl.ToString(), 5, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.MaxWidth(257f), GUILayout.Width(241f));
        try
        {
            GUIClanHall.clanAccessLvl = Convert.ToInt32(value);
        }
        catch (Exception)
        {
            GUIClanHall.clanAccessLvl = 0;
        }
        GUILayout.EndVertical();
        GUILayout.Space(5f);
        if (GUILayout.Button(LanguageManager.GetText("Change"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(82f), GUILayout.Height(33f)))
        {
            ClanManager.ChangeAccessLvl(ClanManager.SelectedClan.ClanID, GUIClanHall.clanAccessLvl);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Change Clan Leader:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(160f), GUILayout.Height(29f));
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Height(150f), GUILayout.Width(241f));
        GUIClanHall.ownerScroll = GUILayout.BeginScrollView(GUIClanHall.ownerScroll, false, true, GUILayout.Height(134f));
        num = 0;
        count = ClanManager.SelectedClan.Members.Count;
        List<ClanMember>.Enumerator enumerator2 = ClanManager.SelectedClan.Members.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                ClanMember current2 = enumerator2.Current;
                if (LocalUser.UserID != current2.UserID)
                {
                    GUILayout.BeginHorizontal(GUIContent.none, (GUIClanHall.newOwner != current2) ? GUIStyle.none : GUISkinManager.Backgound.GetStyle("roomActive"), GUILayout.Height(32f));
                    GUILayout.Space(5f);
                    if (GUILayout.Button(current2.Name, (GUIClanHall.newOwner != current2) ? GUISkinManager.Text.GetStyle("roomDisable") : GUISkinManager.Text.GetStyle("room02"), GUILayout.Width(190f)))
                    {
                        GUIClanHall.newOwner = current2;
                    }
                    GUILayout.Space(5f);
                    GUILayout.EndHorizontal();
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator2).Dispose();
        }
        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.Space(5f);
        if (GUILayout.Button(LanguageManager.GetText("Assign"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(82f), GUILayout.Height(33f)))
        {
            NotificationWindow.Add(new Notification(Notification.Type.NOTIFICATION, LanguageManager.GetText("Change Clan Leader"), LanguageManager.GetTextFormat("Do you really want to change the Clan Leader to {0}?", GUIClanHall.newOwner.Name), LanguageManager.GetText("Assign"), delegate
            {
                ClanManager.ChangeOwnerLazy(LocalUser.Clan.ClanID, GUIClanHall.newOwner.UserID);
            }, null));
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(50f));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(37f));
        GUILayout.Space(10f);
        if (GUILayout.Button(LanguageManager.GetText("Delete clan"), GUISkinManager.Button.GetStyle("cancel"), GUILayout.Width(176f), GUILayout.Height(37f)))
        {
            NotificationWindow.Add(new Notification(Notification.Type.NOTIFICATION, LanguageManager.GetText("Delete clan"), LanguageManager.GetText("Do you really want to delete the clan?"), LanguageManager.GetText("Delete"), delegate
            {
                ClanManager.DeleteClanLazy(LocalUser.Clan.ClanID);
            }, null));
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.Space(15f);
    }
}


