// ILSpyBased#2
using UnityEngine;

public class LoadingMapPopup : MonoBehaviour
{
    private const int MAX_TIPS = 23;

    private static bool isShow = false;

    private static int tipNum = 1;

    private static float progress = 0f;

    private static string msg = string.Empty;

    private static Rect windowRect = new Rect(0f, 0f, 678f, 515f);

    private static Map currMap = null;

    public float tmp_progress;

    public static bool Show
    {
        get
        {
            return LoadingMapPopup.isShow;
        }
    }

    public static void Init(Map map)
    {
        LoadingMapPopup.tipNum = UnityEngine.Random.Range(1, 23);
        LoadingMapPopup.isShow = true;
        LoadingMapPopup.progress = 0f;
        LoadingMapPopup.msg = LanguageManager.GetText("Map initialization...");
        LoadingMapPopup.currMap = map;
    }

    public static void Complete()
    {
        LoadingMapPopup.isShow = false;
        GUIHover.Enable = true;
    }

    public static void Progress(float percent)
    {
        LoadingMapPopup.progress = percent;
    }

    private void OnGUI()
    {
        GUISkin skin = GUI.skin;
        if (LoadingMapPopup.isShow && !NotificationWindow.IsShow)
        {
            LoadingMapPopup.windowRect.x = (float)Screen.width * 0.5f - LoadingMapPopup.windowRect.width * 0.5f;
            LoadingMapPopup.windowRect.y = (float)Screen.height * 0.5f - LoadingMapPopup.windowRect.height * 0.5f;
            GUIHover.Enable = false;
            GUI.Button(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), GUIContent.none, GUIStyle.none);
            LoadingMapPopup.windowRect = GUI.Window(3, LoadingMapPopup.windowRect, new GUI.WindowFunction(this.draw_window), GUIContent.none, GUISkinManager.Backgound.GetStyle("windowLoading"));
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.N)
        {
            goto IL_010c;
        }
        if (Event.current.type == EventType.Used && Event.current.keyCode == KeyCode.N)
        {
            goto IL_010c;
        }
        goto IL_012a;
        IL_010c:
        LoadingMapPopup.tipNum++;
        if (LoadingMapPopup.tipNum > 23)
        {
            LoadingMapPopup.tipNum = 1;
        }
        goto IL_012a;
        IL_012a:
        GUI.skin = skin;
    }

    private void draw_window(int windowId)
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(52f));
        GUILayout.Space(7f);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive2"));
        GUILayout.Label(LanguageManager.GetText("Map loading..."), GUISkinManager.Text.GetStyle("partActive"));
        GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), LanguageManager.GetText("Map loading..."), GUISkinManager.Text.GetStyle("partActive"), GUISkinManager.Text.GetStyle("partActiveShadow"));
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LoadingMapPopup.msg, GUISkinManager.BattleText.GetStyle("txt1Value"));
        GUILayout.Space(15f);
        GUILayout.EndHorizontal();
        GUILayout.Space(12f);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Space(15f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(15f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, GUILayout.Width(320f));
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Width(316f), GUILayout.Height(126f));
        GUILayout.Label(LanguageManager.GetText("Hint"), GUISkinManager.Text.GetStyle("mapName"));
        GUILayout.Space(1f);
        GUILayout.Label(TipsManager.GetTip(), GUISkinManager.Text.GetStyle("mapDesc"));
        GUILayout.EndVertical();
        GUILayout.Space(12f);
        GUILayout.Label(LoadingMapPopup.currMap.Ico, GUIStyle.none, GUILayout.Width(316f), GUILayout.Height(188f));
        GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), LoadingMapPopup.currMap.Name.ToUpper(), GUISkinManager.Text.GetStyle("mapNameBig"), GUISkinManager.Text.GetStyle("mapNameBigShadow"));
        GUILayout.EndVertical();
        GUILayout.Space(9f);
        GUILayout.Label(ActionRotater.LoadingAction.Ico, GUIStyle.none, GUILayout.Width(315f), GUILayout.Height(326f));
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(15f);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Width(644f), GUILayout.Height(80f));
        GUILayout.Space(19f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(15f);
        GUIProgressBar.ProgressBar(590f, 100f, LoadingMapPopup.progress, "pb2");
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
}


