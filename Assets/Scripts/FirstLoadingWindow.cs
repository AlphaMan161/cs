// ILSpyBased#2
using System;
using UnityEngine;

public class FirstLoadingWindow : MonoBehaviour
{
    private string message = string.Empty;

    private float progress;

    private float oldCharacterProgress;

    private void Update()
    {
        if (!Input.GetKeyUp(KeyCode.Keypad6) && !Input.GetKeyUp(KeyCode.RightArrow))
        {
            return;
        }
        NetworkDev.CheckAim = true;
    }

    private void OnGUI()
    {
        GUI.skin = GUISkinManager.Main;
        DebugConsole.ShowDebug(Event.current);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width((float)Screen.width), GUILayout.Height((float)Screen.height));
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(712f));
        GUILayout.Space(22f);
        GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("firstLoading"));
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(this.message, GUISkinManager.Text.GetStyle("normal06"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUIProgressBar.ProgressBar(589f, 100f, this.progress, "pb2");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginArea(new Rect(202f, 236f, 518f, (float)Screen.height - 236f), GUIStyle.none);
        if (LeagueManager.Instance != null && LeagueManager.Instance.Best != null && LeagueManager.Instance.Best.Count > 0)
        {
            GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("tabYesterdayBest"));
            GUILayout.Label(LanguageManager.GetText("Players of the day"), GUISkinManager.Text.GetStyle("normal24"));
            GUILayout.Space(24f);
            if (LeagueManager.Instance.Best.Kill != null)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(23f));
                GUILayout.Label(LanguageManager.GetText("Most kills:"), GUISkinManager.Text.GetStyle("yesterdayBestType"), GUILayout.Width(182f));
                GUILayout.Label(LeagueManager.Instance.Best.Kill.Kill.ToString(), GUISkinManager.Text.GetStyle("yesterdayBestValue"), GUILayout.Width(62f));
                if (LeagueManager.Instance.Best.Kill.ClanID != 0)
                {
                    Texture2D texture = ClanArmManager.GetTexture(LeagueManager.Instance.Best.Kill.ClanArmId);
                    if ((UnityEngine.Object)texture != (UnityEngine.Object)null)
                    {
                        GUILayout.Label(texture, GUIStyle.none, GUILayout.Width(22f), GUILayout.Height(22f));
                    }
                    GUILayout.Label(string.Format("[{0}] {1}", LeagueManager.Instance.Best.Kill.ClanName, LeagueManager.Instance.Best.Kill.FilteredName), GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                else
                {
                    GUILayout.Label(LeagueManager.Instance.Best.Kill.FilteredName, GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                GUILayout.EndHorizontal();
            }
            if (LeagueManager.Instance.Best.Exp != null)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(23f));
                GUILayout.Label(LanguageManager.GetText("Got experience:"), GUISkinManager.Text.GetStyle("yesterdayBestType"), GUILayout.Width(182f));
                GUILayout.Label(LeagueManager.Instance.Best.Exp.Exp.ToString(), GUISkinManager.Text.GetStyle("yesterdayBestValue"), GUILayout.Width(62f));
                if (LeagueManager.Instance.Best.Exp.ClanID != 0)
                {
                    Texture2D texture2 = ClanArmManager.GetTexture(LeagueManager.Instance.Best.Exp.ClanArmId);
                    if ((UnityEngine.Object)texture2 != (UnityEngine.Object)null)
                    {
                        GUILayout.Label(texture2, GUIStyle.none, GUILayout.Width(22f), GUILayout.Height(22f));
                    }
                    GUILayout.Label(string.Format("[{0}] {1}", LeagueManager.Instance.Best.Exp.ClanName, LeagueManager.Instance.Best.Exp.FilteredName), GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                else
                {
                    GUILayout.Label(LeagueManager.Instance.Best.Exp.FilteredName, GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                GUILayout.EndHorizontal();
            }
            if (LeagueManager.Instance.Best.Domination != null)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(23f));
                GUILayout.Label(LanguageManager.GetText("Dominations:"), GUISkinManager.Text.GetStyle("yesterdayBestType"), GUILayout.Width(182f));
                GUILayout.Label(LeagueManager.Instance.Best.Domination.Domination.ToString(), GUISkinManager.Text.GetStyle("yesterdayBestValue"), GUILayout.Width(62f));
                if (LeagueManager.Instance.Best.Domination.ClanID != 0)
                {
                    Texture2D texture3 = ClanArmManager.GetTexture(LeagueManager.Instance.Best.Domination.ClanArmId);
                    if ((UnityEngine.Object)texture3 != (UnityEngine.Object)null)
                    {
                        GUILayout.Label(texture3, GUIStyle.none, GUILayout.Width(22f), GUILayout.Height(22f));
                    }
                    GUILayout.Label(string.Format("[{0}] {1}", LeagueManager.Instance.Best.Domination.ClanName, LeagueManager.Instance.Best.Domination.FilteredName), GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                else
                {
                    GUILayout.Label(LeagueManager.Instance.Best.Domination.FilteredName, GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                GUILayout.EndHorizontal();
            }
            if (LeagueManager.Instance.Best.Head != null)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(23f));
                GUILayout.Label(LanguageManager.GetText("Kills to head:"), GUISkinManager.Text.GetStyle("yesterdayBestType"), GUILayout.Width(182f));
                GUILayout.Label(LeagueManager.Instance.Best.Head.Head.ToString(), GUISkinManager.Text.GetStyle("yesterdayBestValue"), GUILayout.Width(62f));
                if (LeagueManager.Instance.Best.Head.ClanID != 0)
                {
                    Texture2D texture4 = ClanArmManager.GetTexture(LeagueManager.Instance.Best.Head.ClanArmId);
                    if ((UnityEngine.Object)texture4 != (UnityEngine.Object)null)
                    {
                        GUILayout.Label(texture4, GUIStyle.none, GUILayout.Width(22f), GUILayout.Height(22f));
                    }
                    GUILayout.Label(string.Format("[{0}] {1}", LeagueManager.Instance.Best.Head.ClanName, LeagueManager.Instance.Best.Head.FilteredName), GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                else
                {
                    GUILayout.Label(LeagueManager.Instance.Best.Head.FilteredName, GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                GUILayout.EndHorizontal();
            }
            if (LeagueManager.Instance.Best.Nut != null)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(23f));
                GUILayout.Label(LanguageManager.GetText("Kills to nuts:"), GUISkinManager.Text.GetStyle("yesterdayBestType"), GUILayout.Width(182f));
                GUILayout.Label(LeagueManager.Instance.Best.Nut.Nuts.ToString(), GUISkinManager.Text.GetStyle("yesterdayBestValue"), GUILayout.Width(62f));
                if (LeagueManager.Instance.Best.Nut.ClanID != 0)
                {
                    Texture2D texture5 = ClanArmManager.GetTexture(LeagueManager.Instance.Best.Nut.ClanArmId);
                    if ((UnityEngine.Object)texture5 != (UnityEngine.Object)null)
                    {
                        GUILayout.Label(texture5, GUIStyle.none, GUILayout.Width(22f), GUILayout.Height(22f));
                    }
                    GUILayout.Label(string.Format("[{0}] {1}", LeagueManager.Instance.Best.Nut.ClanName, LeagueManager.Instance.Best.Nut.FilteredName), GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                else
                {
                    GUILayout.Label(LeagueManager.Instance.Best.Nut.FilteredName, GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                GUILayout.EndHorizontal();
            }
            if (LeagueManager.Instance.Best.Flag != null)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(23f));
                GUILayout.Label(LanguageManager.GetText("Got flags:"), GUISkinManager.Text.GetStyle("yesterdayBestType"), GUILayout.Width(182f));
                GUILayout.Label(LeagueManager.Instance.Best.Flag.Flag.ToString(), GUISkinManager.Text.GetStyle("yesterdayBestValue"), GUILayout.Width(62f));
                if (LeagueManager.Instance.Best.Flag.ClanID != 0)
                {
                    Texture2D texture6 = ClanArmManager.GetTexture(LeagueManager.Instance.Best.Flag.ClanArmId);
                    if ((UnityEngine.Object)texture6 != (UnityEngine.Object)null)
                    {
                        GUILayout.Label(texture6, GUIStyle.none, GUILayout.Width(22f), GUILayout.Height(22f));
                    }
                    GUILayout.Label(string.Format("[{0}] {1}", LeagueManager.Instance.Best.Flag.ClanName, LeagueManager.Instance.Best.Flag.FilteredName), GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                else
                {
                    GUILayout.Label(LeagueManager.Instance.Best.Flag.FilteredName, GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                GUILayout.EndHorizontal();
            }
            if (LeagueManager.Instance.Best.Point != null)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(23f));
                GUILayout.Label(LanguageManager.GetText("Points captured:"), GUISkinManager.Text.GetStyle("yesterdayBestType"), GUILayout.Width(182f));
                GUILayout.Label(LeagueManager.Instance.Best.Point.ControlPoint.ToString(), GUISkinManager.Text.GetStyle("yesterdayBestValue"), GUILayout.Width(62f));
                if (LeagueManager.Instance.Best.Point.ClanID != 0)
                {
                    Texture2D texture7 = ClanArmManager.GetTexture(LeagueManager.Instance.Best.Point.ClanArmId);
                    if ((UnityEngine.Object)texture7 != (UnityEngine.Object)null)
                    {
                        GUILayout.Label(texture7, GUIStyle.none, GUILayout.Width(22f), GUILayout.Height(22f));
                    }
                    GUILayout.Label(string.Format("[{0}] {1}", LeagueManager.Instance.Best.Point.ClanName, LeagueManager.Instance.Best.Point.FilteredName), GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                else
                {
                    GUILayout.Label(LeagueManager.Instance.Best.Point.FilteredName, GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                GUILayout.EndHorizontal();
            }
            if (LeagueManager.Instance.Best.Assist != null)
            {
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(23f));
                GUILayout.Label(LanguageManager.GetText("Help with kills:"), GUISkinManager.Text.GetStyle("yesterdayBestType"), GUILayout.Width(182f));
                GUILayout.Label(LeagueManager.Instance.Best.Assist.Assist.ToString(), GUISkinManager.Text.GetStyle("yesterdayBestValue"), GUILayout.Width(62f));
                if (LeagueManager.Instance.Best.Assist.ClanID != 0)
                {
                    Texture2D texture8 = ClanArmManager.GetTexture(LeagueManager.Instance.Best.Assist.ClanArmId);
                    if ((UnityEngine.Object)texture8 != (UnityEngine.Object)null)
                    {
                        GUILayout.Label(texture8, GUIStyle.none, GUILayout.Width(22f), GUILayout.Height(22f));
                    }
                    GUILayout.Label(string.Format("[{0}] {1}", LeagueManager.Instance.Best.Assist.ClanName, LeagueManager.Instance.Best.Assist.FilteredName), GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                else
                {
                    GUILayout.Label(LeagueManager.Instance.Best.Assist.FilteredName, GUISkinManager.Text.GetStyle("yesterdayBestName"));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(15f);
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
    }

    private void OnEnable()
    {
        UnityEngine.Debug.Log("Configuration.Lang: " + Configuration.Lang);
        this.message = LanguageManager.GetText("Checking Authorization");
        Auth.Instance.OnValid += new Auth.AuthEventHandler(this.OnValidAuth);
        Auth.Check();
        LeagueManager.InitYesterdayBest();
    }

    private void OnLogin(object sender)
    {
        UnityEngine.Debug.Log("Configuration.Lang: " + Configuration.Lang);
        this.message = LanguageManager.GetText("Checking Authorization");
        Auth.Instance.OnValid += new Auth.AuthEventHandler(this.OnValidAuth);
        Auth.Check();
    }

    private void OnValidAuth(object sender)
    {
        ActionRotater.Init();
        this.progress += 14f;
        this.message = LanguageManager.GetText("Receiving Player Stats");
        AssemblageManager.OnLoad += new AssemblageManager.AssemblageEventHandler(this.OnAssemblage);
        AssemblageManager.OnError += new AssemblageManager.AssemblageEventHandler(this.OnAssemblage);
        AssemblageManager.Init();
    }

    private void OnValidLocalUserInfo(object sender)
    {
        this.progress += 14f;
        this.message = LanguageManager.GetText("Loading Inventory");
        Inventory.OnLoad += new Inventory.InventoryEventHandler(this.OnLoadUserInventory);
        Inventory.Instance.Init();
    }

    private void OnAssemblage(object sender)
    {
        LocalUser.OnValid += new LocalUser.LocalUserEventHandler(this.OnValidLocalUserInfo);
        LocalUser.Refresh();
    }

    private void OnLoadUserInventory(object sender, EventArgs args)
    {
        this.progress += 14f;
        this.message = LanguageManager.GetText("Loading Shop");
        ShopManager.OnLoad += new ShopManager.ShopEventHandler(this.OnLoadShop);
        ShopManager.Instance.Init();
    }

    private void OnLoadShop(object sender)
    {
        this.progress += 14f;
        this.message = LanguageManager.GetText("Loading Achievements");
        AchievementManager.OnLoad += new AchievementManager.AchievementManagerHandler(this.OnLoadAchievement);
        AchievementManager.Instance.Init();
    }

    private void OnLoadAchievement(object sender)
    {
        this.progress += 14f;
        this.message = LanguageManager.GetText("Loading Character");
        CharacterManager.OnLoad += new CharacterManager.CharacterManagerHandler(this.OnLoadCharacter);
        CharacterManager.OnLoadProgress += new CharacterManager.CharacterManagerProgressHandler(this.OnLoadCharacterProgress);
        CharacterManager.Instance.Init();
    }

    private void OnLoadCharacterProgress(object sender, float characterProgress)
    {
        float num = (characterProgress - this.oldCharacterProgress) * 14f;
        this.oldCharacterProgress = characterProgress;
        this.progress += num;
    }

    private void OnLoadCharacter(object sender)
    {
        this.message = LanguageManager.GetText("Loading Weapons");
        AbilityManager.OnLoad += new AbilityManager.AbilityManagerHandler(this.OnLoadAbility);
        AbilityManager.Instance.Init();
    }

    private void OnLoadAbility(object sender)
    {
        this.progress += 14f;
        this.message = LanguageManager.GetText("Loading Map");
        MapList.OnLoad += new MapList.MapListHandler(this.OnLoadMap);
        MapList.Instance.Init();
    }

    private void OnLoadMap(object sender)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void Start()
    {
    }
}


