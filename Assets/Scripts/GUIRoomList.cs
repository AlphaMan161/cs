// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIRoomList : MonoBehaviour
{
    private static Vector2 serverScroll = new Vector2(0f, 0f);

    private static Vector2 gameListScroll = new Vector2(0f, 0f);

    private static double clickTime;

    private static double doubleClickTime = 0.3;

    private static string gameListSelectedKey = string.Empty;

    private static GUIDropDownList.GUIDropDownSetting dropDownSetting = null;

    private static bool isInitCallback = false;

    private static MyRoomSetting roomSetting = null;

    private static MapMode.MODE MAP_CREATE_SELECT_MODE = MapMode.MODE.NONE;

    private static Vector2 mapsScroll = Vector2.zero;

    private static bool enableTimer = false;

    private static int timerCount = 20;

    private static float timer_nextActionTime = 0f;

    public static float timer_period = 1f;

    public static int TotalPlayers
    {
        get
        {
            int num = 0;
            Dictionary<string, ResponseRoomList>.ValueCollection.Enumerator enumerator = MainNetworkController.Instance.RoomList.Values.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ResponseRoomList current = enumerator.Current;
                    num += current.UserOnline;
                }
                return num;
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
        }
    }

    public static MyRoomSetting RoomSetting
    {
        get
        {
            return GUIRoomList.roomSetting;
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public static void OnGUI()
    {
        if (!GUIRoomList.isInitCallback && ServersList.Instance != null)
        {
            GUIRoomList.isInitCallback = true;
            ServersList.OnConnect += new ServersList.ServerListHandler(GUIRoomList.HandleOnComplete);
        }
        if (GUIRoomList.roomSetting == null)
        {
            GUIRoomList.roomSetting = new MyRoomSetting(LocalUser.Name + " room", LocalUser.Level, 0);
        }
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width((float)Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuRow03"), GUILayout.Height(41f));
        if (GUILayout.Button(LanguageManager.GetText("Available battles"), GUISkinManager.Button.GetStyle((MenuSelecter.RoomListMenuSelect != MenuSelecter.RoomListMenuEnum.RoomList) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.RoomListMenuSelect = MenuSelecter.RoomListMenuEnum.RoomList;
            MainNetworkController.Instance.UpdateRoomList();
        }
        if (GUILayout.Button(LanguageManager.GetText("Create battle"), GUISkinManager.Button.GetStyle((MenuSelecter.RoomListMenuSelect != MenuSelecter.RoomListMenuEnum.CreateGame) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.RoomListMenuSelect = MenuSelecter.RoomListMenuEnum.CreateGame;
            if (GUIRoomList.roomSetting != null)
            {
                GUIRoomList.roomSetting.GenerateName();
                GUIRoomList.roomSetting.Password = string.Empty;
            }
        }
        if (GUILayout.Button(LanguageManager.GetText("Maps"), GUISkinManager.Button.GetStyle((MenuSelecter.RoomListMenuSelect != MenuSelecter.RoomListMenuEnum.Maps) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.RoomListMenuSelect = MenuSelecter.RoomListMenuEnum.Maps;
        }
        if (ServerConf.ServerList.Count > 1 && GUILayout.Button(LanguageManager.GetText("Change server"), GUISkinManager.Button.GetStyle((MenuSelecter.RoomListMenuSelect != MenuSelecter.RoomListMenuEnum.ServerList) ? "menuRow03" : "menuRow03active"), GUILayout.Height(31f)))
        {
            MenuSelecter.RoomListMenuSelect = MenuSelecter.RoomListMenuEnum.ServerList;
        }
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        if (ServersList.SelectServer == null || !ServersList.SelectServer.IsConnected)
        {
            MenuSelecter.RoomListMenuSelect = MenuSelecter.RoomListMenuEnum.ServerList;
        }
        if (GameLogicServerNetworkController.Connected)
        {
            GUIRoomList.enableTimer = false;
            GUIRoomList.timerCount = 20;
            if (MenuSelecter.RoomListMenuSelect == MenuSelecter.RoomListMenuEnum.CreateGame)
            {
                GUIRoomList.DrawCreateGame();
            }
            else if (MenuSelecter.RoomListMenuSelect == MenuSelecter.RoomListMenuEnum.RoomList)
            {
                GUIRoomList.DrawRoomList();
            }
            else if (MenuSelecter.RoomListMenuSelect == MenuSelecter.RoomListMenuEnum.ServerList)
            {
                GUIRoomList.DrawSelectServer();
            }
            else if (MenuSelecter.RoomListMenuSelect == MenuSelecter.RoomListMenuEnum.Maps)
            {
                GUIRoomList.DrawMaps();
            }
        }
        else
        {
            GUIRoomList.DrawWaitConnect2GM();
        }
        if (GUIRoomList.dropDownSetting != null && GUIRoomList.dropDownSetting.ShowList)
        {
            GUIDropDownList.List(GUIRoomList.dropDownSetting);
        }
    }

    private static void HandlerListModeClickCallBack(object sender, object entry)
    {
        if (entry != null && entry.GetType() == typeof(GUIDropDownList.GUIDropDownEntry))
        {
            GUIDropDownList.GUIDropDownEntry gUIDropDownEntry = entry as GUIDropDownList.GUIDropDownEntry;
            GUIRoomList.roomSetting.GameMode = (MapMode.MODE)(byte)Convert.ToInt32(gUIDropDownEntry.Tag);
        }
    }

    private static void HandlerListMapClickCallBack(object sender, object entry)
    {
        if (entry != null && entry.GetType() == typeof(GUIDropDownList.GUIDropDownEntry))
        {
            GUIDropDownList.GUIDropDownEntry gUIDropDownEntry = entry as GUIDropDownList.GUIDropDownEntry;
            GUIRoomList.roomSetting.Map = (gUIDropDownEntry.Tag as Map);
        }
    }

    private static void DrawMaps()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        if (GUIRoomList.MAP_CREATE_SELECT_MODE == MapMode.MODE.NONE)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("All"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("All"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            GUIRoomList.MAP_CREATE_SELECT_MODE = MapMode.MODE.NONE;
        }
        if (GUIRoomList.MAP_CREATE_SELECT_MODE == MapMode.MODE.DEATHMATCH)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("Deathmatch"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("Deathmatch"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            GUIRoomList.MAP_CREATE_SELECT_MODE = MapMode.MODE.DEATHMATCH;
        }
        if (GUIRoomList.MAP_CREATE_SELECT_MODE == MapMode.MODE.TEAM_DEATHMATCH)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("Team Deathmatch"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("Team Deathmatch"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            GUIRoomList.MAP_CREATE_SELECT_MODE = MapMode.MODE.TEAM_DEATHMATCH;
        }
        if (GUIRoomList.MAP_CREATE_SELECT_MODE == MapMode.MODE.CAPTURE_THE_FLAG)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("Capture the flag"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("Capture the flag"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            GUIRoomList.MAP_CREATE_SELECT_MODE = MapMode.MODE.CAPTURE_THE_FLAG;
        }
        if (GUIRoomList.MAP_CREATE_SELECT_MODE == MapMode.MODE.CONTROL_POINTS)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("Control point"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("Control point"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            GUIRoomList.MAP_CREATE_SELECT_MODE = MapMode.MODE.CONTROL_POINTS;
        }
        if (GUIRoomList.MAP_CREATE_SELECT_MODE == MapMode.MODE.ZOMBIE)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("Infection"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("Infection"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            GUIRoomList.MAP_CREATE_SELECT_MODE = MapMode.MODE.ZOMBIE;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.Space(3f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(2f);
        GUIRoomList.mapsScroll = GUILayout.BeginScrollView(GUIRoomList.mapsScroll, false, true, GUILayout.MinHeight(378f));
        int num = 0;
        int count = MapList.GetMaps(GUIRoomList.MAP_CREATE_SELECT_MODE).Count;
        foreach (Map map in MapList.GetMaps(GUIRoomList.MAP_CREATE_SELECT_MODE))
        {
            if (num % 3 == 0)
            {
                GUILayout.Space(7f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
            }
            GUILayout.Space(13f);
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Width(224f), GUILayout.Height(160f));
            GUILayout.Label(map.Ico, GUIStyle.none, GUILayout.Width(224f), GUILayout.Height(135f));
            Rect lastRect = GUILayoutUtility.GetLastRect();
            GUITextShadow.TextShadow(lastRect, map.Name.ToUpper(), GUISkinManager.Text.GetStyle("mapNameBig02"), GUISkinManager.Text.GetStyle("mapNameBig02Shadow"));
            lastRect.y += lastRect.height - 10f;
            lastRect.height = 35f;
            GUI.BeginGroup(lastRect, GUIContent.none, GUIStyle.none);
            if (map.ShopCost != null && !map.Buyed && GUI.Button(new Rect(11f, 0f, 82f, 32f), map.ShopCost.TimePVCost.ToString(), GUISkinManager.Button.GetStyle("buyAbility")))
            {
                MapList.Instance.BuyMap(map);
            }
            if (map.ShopCost != null && !map.Buyed && !map.Tryed && GUI.Button(new Rect(101f, 0f, 111f, 32f), LanguageManager.GetText("Try"), GUISkinManager.Button.GetStyle("btn01")))
            {
                ServersList.ConnectQuick(Ajax.Instance, map.SystemName, GUIRoomList.MAP_CREATE_SELECT_MODE);
            }
            if (map.Buyed && GUI.Button(new Rect(101f, 0f, 111f, 32f), LanguageManager.GetText("Play"), GUISkinManager.Button.GetStyle("btn01")))
            {
                ServersList.ConnectQuick(Ajax.Instance, map.SystemName, GUIRoomList.MAP_CREATE_SELECT_MODE);
            }
            GUI.EndGroup();
            GUILayout.EndHorizontal();
            if (num % 3 == 2 || num == count - 1)
            {
                GUILayout.EndHorizontal();
                GUILayout.Space(7f);
            }
            num++;
        }
        if (count <= 0)
        {
            GUILayout.Label(LanguageManager.GetText("Currently there are no available maps for this game mode."), GUISkinManager.Text.GetStyle("noneBattle"), GUILayout.Height(325f));
        }
        GUILayout.EndScrollView();
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private static void DrawCreateGame()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
        GUILayout.Label(LanguageManager.GetText("Creating the battle"), GUISkinManager.Text.GetStyle("partActive"));
        GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), LanguageManager.GetText("Creating the battle"), GUISkinManager.Text.GetStyle("partActive"), GUISkinManager.Text.GetStyle("partActiveShadow"));
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(13f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(15f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(320f));
        GUILayout.Label(GUIRoomList.roomSetting.Map.Ico, GUIStyle.none, GUILayout.Width(316f), GUILayout.Height(188f));
        GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), GUIRoomList.roomSetting.Map.Name.ToUpper(), GUISkinManager.Text.GetStyle("mapNameBig"), GUISkinManager.Text.GetStyle("mapNameBigShadow"));
        GUILayout.Space(12f);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Width(316f), GUILayout.Height(126f));
        GUILayout.Space(5f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Label(GUIRoomList.roomSetting.Map.Name, GUISkinManager.Text.GetStyle("mapName"));
        GUILayout.Space(1f);
        GUILayout.Label(GUIRoomList.roomSetting.Map.Desc, GUISkinManager.Text.GetStyle("mapDesc"));
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(45f));
        GUILayout.Label(LanguageManager.GetText("Mode:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(121f), GUILayout.Height(29f));
        if (GUIRoomList.dropDownSetting != null && GUIRoomList.dropDownSetting.ShowList)
        {
            GUILayout.Label(GUIRoomList.roomSetting.GameMode.GetFullName(), GUISkinManager.Backgound.GetStyle("dropdownlist"), GUILayout.Width(256f), GUILayout.Height(33f));
        }
        else if (GUILayout.Button(GUIRoomList.roomSetting.GameMode.GetFullName(), GUISkinManager.Backgound.GetStyle("dropdownlist"), GUILayout.Width(256f), GUILayout.Height(33f)))
        {
            List<GUIDropDownList.GUIDropDownEntry> list = new List<GUIDropDownList.GUIDropDownEntry>();
            foreach (byte value in Enum.GetValues(typeof(MapMode.MODE)))
            {
                if ((GUIRoomList.roomSetting.Map.Modes & value) == value && value != 0)
                {
                    list.Add(new GUIDropDownList.GUIDropDownEntry(((MapMode.MODE)value).GetFullName(), (MapMode.MODE)value));
                }
            }
            GUIRoomList.dropDownSetting = new GUIDropDownList.GUIDropDownSetting("modeSelect", true, list.ToArray(), new GUIDropDownList.ListCallBack(GUIRoomList.HandlerListModeClickCallBack));
        }
        if (Event.current.type == EventType.Repaint && GUIRoomList.dropDownSetting != null && !GUIRoomList.dropDownSetting.ValidRect && GUIRoomList.dropDownSetting.Owner == "modeSelect")
        {
            Rect lastRect = GUILayoutUtility.GetLastRect();
            lastRect.y += lastRect.height - 11f;
            lastRect.height = 300f;
            Vector2 vector = UnityEngine.Input.mousePosition;
            vector.y = (float)Screen.height - vector.y;
            float x = lastRect.x;
            float x2 = vector.x;
            Vector2 mousePosition = Event.current.mousePosition;
            lastRect.x = x + (x2 - mousePosition.x);
            float y = lastRect.y;
            float y2 = vector.y;
            Vector2 mousePosition2 = Event.current.mousePosition;
            lastRect.y = y + (y2 - mousePosition2.y);
            GUIRoomList.dropDownSetting.SetValidRect(lastRect);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(45f));
        GUILayout.Label(LanguageManager.GetText("Map:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(121f), GUILayout.Height(29f));
        if (GUIRoomList.dropDownSetting != null && GUIRoomList.dropDownSetting.ShowList)
        {
            GUILayout.Label(GUIRoomList.roomSetting.Map.Name, GUISkinManager.Backgound.GetStyle("dropdownlist"), GUILayout.Width(256f), GUILayout.Height(33f));
        }
        else if (GUILayout.Button(GUIRoomList.roomSetting.Map.Name, GUISkinManager.Backgound.GetStyle("dropdownlist"), GUILayout.Width(256f), GUILayout.Height(33f)))
        {
            List<GUIDropDownList.GUIDropDownEntry> list2 = new List<GUIDropDownList.GUIDropDownEntry>();
            foreach (Map item in MapList.GetAllowedMap())
            {
                list2.Add(new GUIDropDownList.GUIDropDownEntry(item.Name.ToString(), item));
            }
            GUIRoomList.dropDownSetting = new GUIDropDownList.GUIDropDownSetting("mapSelect", true, list2.ToArray(), new GUIDropDownList.ListCallBack(GUIRoomList.HandlerListMapClickCallBack));
        }
        if (Event.current.type == EventType.Repaint && GUIRoomList.dropDownSetting != null && !GUIRoomList.dropDownSetting.ValidRect && GUIRoomList.dropDownSetting.Owner == "mapSelect")
        {
            Rect lastRect2 = GUILayoutUtility.GetLastRect();
            lastRect2.y += lastRect2.height - 11f;
            lastRect2.height = 320f;
            Vector2 vector2 = UnityEngine.Input.mousePosition;
            vector2.y = (float)Screen.height - vector2.y;
            float x3 = lastRect2.x;
            float x4 = vector2.x;
            Vector2 mousePosition3 = Event.current.mousePosition;
            lastRect2.x = x3 + (x4 - mousePosition3.x);
            float y3 = lastRect2.y;
            float y4 = vector2.y;
            Vector2 mousePosition4 = Event.current.mousePosition;
            lastRect2.y = y3 + (y4 - mousePosition4.y);
            GUIRoomList.dropDownSetting.SetValidRect(lastRect2);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Room name:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(121f), GUILayout.Height(29f));
        if (GUIRoomList.dropDownSetting != null && GUIRoomList.dropDownSetting.ShowList)
        {
            GUILayout.Label(GUIRoomList.roomSetting.Name, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.Width(257f));
        }
        else
        {
            GUIRoomList.roomSetting.Name = GUILayout.TextField(GUIRoomList.roomSetting.Name, GUIRoomList.roomSetting.MaxNameLenght, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.MaxWidth(257f), GUILayout.Width(257f));
            GUILayout.Space(4f);
            if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("refresh"), GUILayout.Width(32f), GUILayout.Height(32f)))
            {
                GUIRoomList.roomSetting.GenerateName();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Max. players:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(121f), GUILayout.Height(29f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("selectGrid"), GUILayout.Height(32f));
        int num = GUILayout.SelectionGrid(GUIRoomList.roomSetting.SelectPlayerIndex, GUIRoomList.roomSetting.Map.AvailPlayers, GUIRoomList.roomSetting.Map.AvailPlayers.Length, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Height(28f), GUILayout.MinWidth(222f));
        if (GUIRoomList.roomSetting.SelectPlayerIndex != num && (GUIRoomList.dropDownSetting == null || (!GUIRoomList.dropDownSetting.ShowList && GUIRoomList.dropDownSetting.HidedTime + 0.3f < Time.time)))
        {
            GUIRoomList.roomSetting.SelectPlayerIndex = num;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();
        if (GUIRoomList.roomSetting.GameMode == MapMode.MODE.CAPTURE_THE_FLAG || GUIRoomList.roomSetting.GameMode == MapMode.MODE.CONTROL_POINTS)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
            GUILayout.Label(LanguageManager.GetText("Victory points:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(121f), GUILayout.Height(29f));
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("selectGrid"), GUILayout.Height(32f));
            num = GUILayout.SelectionGrid(GUIRoomList.roomSetting.SelectedFragIndex, GUIRoomList.roomSetting.AvailFragLimit, GUIRoomList.roomSetting.AvailFragLimit.Length, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Height(28f), GUILayout.MinWidth(181f));
            if (GUIRoomList.roomSetting.SelectedFragIndex != num && (GUIRoomList.dropDownSetting == null || (!GUIRoomList.dropDownSetting.ShowList && GUIRoomList.dropDownSetting.HidedTime + 0.3f < Time.time)))
            {
                GUIRoomList.roomSetting.SelectedFragIndex = num;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
            GUILayout.Label(LanguageManager.GetText("Time played:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(121f), GUILayout.Height(29f));
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("selectGrid"), GUILayout.Height(32f));
            num = GUILayout.SelectionGrid(GUIRoomList.roomSetting.SelectTimeIndex, GUIRoomList.roomSetting.AvailTimes, GUIRoomList.roomSetting.AvailTimes.Length, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Height(28f), GUILayout.MinWidth(181f));
            if (GUIRoomList.roomSetting.SelectTimeIndex != num && (GUIRoomList.dropDownSetting == null || (!GUIRoomList.dropDownSetting.ShowList && GUIRoomList.dropDownSetting.HidedTime + 0.3f < Time.time)))
            {
                GUIRoomList.roomSetting.SelectTimeIndex = num;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
        }
        if (LocalUser.Permission.Password)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
            GUILayout.Label(LanguageManager.GetText("Password:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(121f), GUILayout.Height(29f));
            if (GUIRoomList.dropDownSetting != null && GUIRoomList.dropDownSetting.ShowList)
            {
                GUILayout.Label(GUIRoomList.roomSetting.Password, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.Width(176f));
            }
            else
            {
                GUIRoomList.roomSetting.Password = GUILayout.TextField(GUIRoomList.roomSetting.Password, GUIRoomList.roomSetting.MaxPasswordLenght, GUISkinManager.Main.textField, GUILayout.Height(32f), GUILayout.MaxWidth(176f), GUILayout.Width(176f));
            }
            GUILayout.EndHorizontal();
        }
        if (LocalUser.Permission.Guest)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
            GUILayout.Label(LanguageManager.GetText("Guest:"), GUISkinManager.Text.GetStyle("mapPropertie"), GUILayout.Width(121f), GUILayout.Height(29f));
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("selectGrid"), GUILayout.Height(32f));
            num = GUILayout.SelectionGrid(GUIRoomList.roomSetting.GuestMode, GUIRoomList.roomSetting.GuestModes, GUIRoomList.roomSetting.GuestModes.Length, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Height(28f), GUILayout.MinWidth(120f));
            if (GUIRoomList.roomSetting.GuestMode != num && (GUIRoomList.dropDownSetting == null || (!GUIRoomList.dropDownSetting.ShowList && GUIRoomList.dropDownSetting.HidedTime + 0.3f < Time.time)))
            {
                GUIRoomList.roomSetting.GuestMode = num;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        Rect lastRect3 = GUILayoutUtility.GetLastRect();
        lastRect3.width = 115f;
        lastRect3.height = 115f;
        lastRect3.x += 295f;
        lastRect3.y += 225f;
        if (GUI.Button(lastRect3, GUIContent.none, GUISkinManager.Button.GetStyle("start")))
        {
            GUIRoomList.roomSetting.Name = GUIRoomList.roomSetting.Name.Trim();
            if (GUIRoomList.roomSetting.Name.Equals(string.Empty) || GUIRoomList.roomSetting.Name == string.Empty)
            {
                GUIRoomList.roomSetting.Name = LocalUser.Name + UnityEngine.Random.Range(1, 1000);
            }
            OptionsManager.RoomSetting = GUIRoomList.roomSetting;
            OptionsManager.ConnectingMap = GUIRoomList.roomSetting.Map;
            MainNetworkController.Instance.CreateRoom(GUIRoomList.roomSetting);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private static void DrawRoomList()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        if (MainNetworkController.Instance.ListSelectMode == MapMode.MODE.NONE)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("All"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("All"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            MainNetworkController.Instance.ListSelectMode = MapMode.MODE.NONE;
        }
        if (MainNetworkController.Instance.ListSelectMode == MapMode.MODE.DEATHMATCH)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("Deathmatch"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("Deathmatch"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            MainNetworkController.Instance.ListSelectMode = MapMode.MODE.DEATHMATCH;
        }
        if (MainNetworkController.Instance.ListSelectMode == MapMode.MODE.TEAM_DEATHMATCH)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("Team Deathmatch"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("Team Deathmatch"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            MainNetworkController.Instance.ListSelectMode = MapMode.MODE.TEAM_DEATHMATCH;
        }
        if (MainNetworkController.Instance.ListSelectMode == MapMode.MODE.CAPTURE_THE_FLAG)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("Capture the flag"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("Capture the flag"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            MainNetworkController.Instance.ListSelectMode = MapMode.MODE.CAPTURE_THE_FLAG;
        }
        if (MainNetworkController.Instance.ListSelectMode == MapMode.MODE.CONTROL_POINTS)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("Control point"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("Control point"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            MainNetworkController.Instance.ListSelectMode = MapMode.MODE.CONTROL_POINTS;
        }
        if (MainNetworkController.Instance.ListSelectMode == MapMode.MODE.ZOMBIE)
        {
            GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
            GUILayout.Label(LanguageManager.GetText("Infection"), GUISkinManager.Text.GetStyle("partActiveSmall"));
            GUILayout.EndHorizontal();
        }
        else if (GUILayout.Button(LanguageManager.GetText("Infection"), GUISkinManager.Text.GetStyle("partSmall")))
        {
            MainNetworkController.Instance.ListSelectMode = MapMode.MODE.ZOMBIE;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(36f));
        GUILayout.Space(9f);
        GUILayout.Label(LanguageManager.GetText("Map"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(156f));
        GUILayout.Label(LanguageManager.GetText("Room name"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(283f));
        GUILayout.Label(LanguageManager.GetText("Mode"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(81f));
        GUILayout.Label(LanguageManager.GetText("Players"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(81f));
        GUILayout.Space(105f);
        if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("refresh"), GUILayout.Width(32f), GUILayout.Height(32f)))
        {
            MainNetworkController.Instance.UpdateRoomList();
        }
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUIRoomList.gameListScroll = GUILayout.BeginScrollView(GUIRoomList.gameListScroll, false, true, GUILayout.MinHeight(340f));
        if (MainNetworkController.Instance.FilteredRoomList.Count > 0)
        {
            Dictionary<string, ResponseRoomList>.Enumerator enumerator = MainNetworkController.Instance.FilteredRoomList.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, ResponseRoomList> current = enumerator.Current;
                    ResponseRoomList value = current.Value;
                    bool flag = true;
                    GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                    GUILayout.Space(4f);
                    if (flag)
                    {
                        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("room"), GUILayout.Height(32f));
                        GUILayout.Label(value.MapName, GUISkinManager.Text.GetStyle("room"), GUILayout.Width(156f));
                        GUILayout.Label(value.FilteredName, GUISkinManager.Text.GetStyle("room"), GUILayout.Width(285f));
                        GUILayout.Label(value.Mode, GUISkinManager.Text.GetStyle("room"), GUILayout.Width(81f));
                        GUILayout.Label(value.UserOnline + " / " + value.UserMax, GUISkinManager.Text.GetStyle("room"), GUILayout.Width(81f));
                        if (GUILayout.Button(LanguageManager.GetText("JOIN"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(111f), GUILayout.Height(32f)))
                        {
                            MainNetworkController.Instance.JoinRoom(current.Key.ToString(), value.MapSystemName, false);
                        }
                        if (current.Value.IsPassword)
                        {
                            Rect lastRect = GUILayoutUtility.GetLastRect();
                            lastRect.x += lastRect.width - 24f;
                            lastRect.y += 5f;
                            lastRect.width = 19f;
                            lastRect.height = 23f;
                            GUI.Label(lastRect, GUIContent.none, GUISkinManager.Ico.GetStyle("lock"));
                        }
                        if (LocalUser.Permission.Guest && GUILayout.Button(LanguageManager.GetText("GUEST"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(111f), GUILayout.Height(32f)))
                        {
                            MainNetworkController.Instance.JoinRoom(current.Key.ToString(), value.MapSystemName, true);
                        }
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("roomDisable"), GUILayout.Height(32f));
                        GUILayout.Label(value.MapName, GUISkinManager.Text.GetStyle("roomDisable"), GUILayout.Width(156f));
                        GUILayout.Label(current.Key, GUISkinManager.Text.GetStyle("roomDisable"), GUILayout.Width(214f));
                        GUILayout.Label(value.Mode, GUISkinManager.Text.GetStyle("roomDisable"), GUILayout.Width(113f));
                        GUILayout.Label(value.UserOnline + " / " + value.UserMax, GUISkinManager.Text.GetStyle("roomDisable"), GUILayout.Width(113f));
                        GUILayout.Button(LanguageManager.GetText("JOIN"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(111f), GUILayout.Height(32f));
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
                    GUILayout.EndVertical();
                    if (Event.current.type == EventType.MouseUp && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                    {
                        GUIRoomList.gameListSelectedKey = current.Key.ToString();
                        if ((double)Time.time - GUIRoomList.clickTime < GUIRoomList.doubleClickTime)
                        {
                            MainNetworkController.Instance.JoinRoom(current.Key.ToString(), value.MapSystemName, false);
                        }
                        GUIRoomList.clickTime = (double)Time.time;
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
        }
        else
        {
            GUILayout.Label(LanguageManager.GetText("There are no active rooms. You can start new battle by pressing \"Create battle\" button"), GUISkinManager.Text.GetStyle("noneBattle"), GUILayout.Height(320f));
        }
        GUILayout.EndScrollView();
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private static void HandleOnComplete(object server)
    {
        MenuSelecter.RoomListMenuSelect = MenuSelecter.RoomListMenuEnum.RoomList;
    }

    private static void DrawSelectServer()
    {
        bool list = MasterServerMonitor.Instance.List;
        if (list)
        {
            MasterServerMonitor.Instance.ListUpdate();
        }
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive"));
        GUILayout.Label(LanguageManager.GetText("SELECT SERVER"), GUISkinManager.Text.GetStyle("partActive"));
        GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), LanguageManager.GetText("SELECT SERVER"), GUISkinManager.Text.GetStyle("partActive"), GUISkinManager.Text.GetStyle("partActiveShadow"));
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuTitle"), GUILayout.Height(36f));
        GUILayout.Space(11f);
        GUILayout.Label(LanguageManager.GetText("Server"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(109f));
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetText("Capacity"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(159f));
        if (list)
        {
            GUILayout.Label(LanguageManager.GetText("Load"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(63.5f));
            GUILayout.Label(LanguageManager.GetText("Ping"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(53.5f));
        }
        else
        {
            GUILayout.Label(LanguageManager.GetText("Ping"), GUISkinManager.Text.GetStyle("menuTitle"), GUILayout.Width(117f));
        }
        GUILayout.Space(120f);
        if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("refresh"), GUILayout.Width(32f), GUILayout.Height(32f)))
        {
            ServersList.Refresh(MainNetworkController.Instance, false);
        }
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUIRoomList.serverScroll = GUILayout.BeginScrollView(GUIRoomList.serverScroll, false, true, GUILayout.MinHeight(340f));
        List<ServerItem>.Enumerator enumerator = ServersList.ServerList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ServerItem current = enumerator.Current;
                bool flag = true;
                if (!current.IsRefreshed || !current.IsActive)
                {
                    flag = false;
                }
                if (LocalUser.Level < current.LevelMin || LocalUser.Level > current.LevelMax)
                {
                    flag = false;
                }
                GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
                GUILayout.Space(4f);
                GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(36f));
                if (flag)
                {
                    GUILayout.Space(11f);
                    GUILayout.Label(current.Name, GUISkinManager.Text.GetStyle("room"));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(string.Format("{0} / {1}", current.CapacityMin, current.CapacityMax), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(149f));
                    if (list)
                    {
                        GUILayout.Label(string.Format("{0} %", current.Load), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(63.5f));
                        GUILayout.Label(string.Format("{0} ms", current.Latency), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(53.5f));
                    }
                    else
                    {
                        GUILayout.Label(string.Format("{0} ms", current.Latency), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(117f));
                    }
                    GUILayout.Space(18f);
                    if (GUILayout.Button(LanguageManager.GetText("JOIN"), GUISkinManager.Button.GetStyle("btn01"), GUILayout.Width(111f), GUILayout.Height(32f)))
                    {
                        ServersList.Connect(current);
                    }
                }
                else
                {
                    GUILayout.Space(11f);
                    GUILayout.Label(current.Name, GUISkinManager.Text.GetStyle("roomDisable"));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(string.Format("{0} / {1}", current.CapacityMin, current.CapacityMax), GUISkinManager.Text.GetStyle("roomDisable"), GUILayout.Width(149f));
                    if (list)
                    {
                        GUILayout.Label(string.Format("{0} %", current.Load), GUISkinManager.Text.GetStyle("roomDisable"), GUILayout.Width(63.5f));
                        GUILayout.Label(string.Format("{0} ms", current.Latency), GUISkinManager.Text.GetStyle("roomDisable"), GUILayout.Width(53.5f));
                    }
                    else
                    {
                        GUILayout.Label(string.Format("{0} ms", current.Latency), GUISkinManager.Text.GetStyle("roomDisable"), GUILayout.Width(117f));
                    }
                    GUILayout.Space(18f);
                    GUILayout.Label(LanguageManager.GetText("JOIN"), GUISkinManager.Button.GetStyle("btn01Disable"), GUILayout.Width(111f), GUILayout.Height(32f));
                }
                GUILayout.Space(4f);
                GUILayout.EndHorizontal();
                GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("black1Ver"), GUILayout.Height(1f));
                GUILayout.EndVertical();
                if (flag && Event.current.type == EventType.MouseUp && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    if ((double)Time.time - GUIRoomList.clickTime < GUIRoomList.doubleClickTime)
                    {
                        if (current.IsActive)
                        {
                            ServersList.Connect(current);
                        }
                        else
                        {
                            ErrorInfo.CODE code = ErrorInfo.CODE.SERVER_NOT_RESPOND;
                            code.AddNotification(ErrorInfo.TYPE.SERVER_CONNECT);
                            UnityEngine.Debug.LogError("This server is inactive!");
                        }
                    }
                    GUIRoomList.clickTime = (double)Time.time;
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

    public static void UpdateTimer()
    {
        if (GUIRoomList.enableTimer && Time.time > GUIRoomList.timer_nextActionTime)
        {
            GUIRoomList.timer_nextActionTime += GUIRoomList.timer_period;
            GUIRoomList.timerCount -= (int)GUIRoomList.timer_period;
            if (GUIRoomList.timerCount < 0)
            {
                GUIRoomList.timerCount = 10;
            }
        }
    }

    private static void DrawWaitConnect2GM()
    {
        GUIRoomList.enableTimer = true;
        GUIRoomList.UpdateTimer();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("winMain"), GUILayout.Width(755f), GUILayout.Height(454f));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.Label(LanguageManager.GetTextFormat("Please wait. We're accessing the battle field! {0} sec", GUIRoomList.timerCount), GUISkinManager.Text.GetStyle("friendConnecting"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}


