// ILSpyBased#2
using UnityEngine;

public class SavePopup : MonoBehaviour
{
    private static bool isShow = false;

    private static string msg = string.Empty;

    private static bool isError = false;

    private static Rect windowRect = new Rect(0f, 0f, 346f, 215f);

    private static float startTime = 0f;

    public static bool Show
    {
        get
        {
            return SavePopup.isShow;
        }
    }

    public static void Init(string message)
    {
    }

    public static void Complete()
    {
        SavePopup.isShow = false;
        SavePopup.isError = false;
        GUIHover.Enable = true;
    }

    public static void Error(string message)
    {
        SavePopup.msg = message;
        SavePopup.isError = true;
    }

    public static void Progress(string message)
    {
        SavePopup.msg = message;
    }

    private void OnEnable()
    {
        SavePopup.startTime = Time.time;
    }

    private void OnGUI()
    {
        GUISkin skin = GUI.skin;
        if (SavePopup.isShow && !NotificationWindow.IsShow && !LoadingMapPopup.Show)
        {
            SavePopup.windowRect.x = (float)Screen.width * 0.5f - SavePopup.windowRect.width * 0.5f;
            SavePopup.windowRect.y = (float)Screen.height * 0.5f - SavePopup.windowRect.height * 0.5f;
            GUIHover.Enable = false;
            if (!GUI.Button(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), GUIContent.none, GUIStyle.none))
            {
                goto IL_00a7;
            }
            goto IL_00a7;
        }
        goto IL_00d7;
        IL_00d7:
        GUI.skin = skin;
        return;
        IL_00a7:
        SavePopup.windowRect = GUI.Window(3, SavePopup.windowRect, new GUI.WindowFunction(this.draw_window), GUIContent.none, GUISkinManager.Backgound.GetStyle("windowLoading"));
        goto IL_00d7;
    }

    private void draw_window(int windowId)
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(52f));
        GUILayout.Space(7f);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("partActive2"));
        GUILayout.Label(LanguageManager.GetText("Сохранение"), GUISkinManager.Text.GetStyle("partActive"));
        GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), LanguageManager.GetText("Сохранение"), GUISkinManager.Text.GetStyle("partActive"), GUISkinManager.Text.GetStyle("partActiveShadow"));
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(15f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(15f);
        GUILayout.BeginVertical(GUIContent.none, GUISkinManager.Backgound.GetStyle("itemBlock"), GUILayout.Width(316f), GUILayout.Height(126f));
        if (SavePopup.isError)
        {
            GUILayout.Label(LanguageManager.GetText("Произошла ошибка при сохранение"), GUISkinManager.Text.GetStyle("mapName"));
        }
        else
        {
            GUILayout.Label(LanguageManager.GetText("Пожалуйста подождите"), GUISkinManager.Text.GetStyle("mapName"));
        }
        GUILayout.Space(1f);
        GUILayout.Label(SavePopup.msg, GUISkinManager.Text.GetStyle("mapDesc"));
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        if (Time.time - SavePopup.startTime > 30f && !SavePopup.isError)
        {
            UnityEngine.Debug.LogError(string.Format("SavePopup Time.time:{0} startTime:{1}", Time.time, SavePopup.startTime));
            SavePopup.Error(LanguageManager.GetText("Не пришел ответ от сервера"));
        }
        if (SavePopup.isError && GUI.Button(new Rect(SavePopup.windowRect.width - 30f, 2f, 30f, 30f), GUIContent.none, GUISkinManager.Button.GetStyle("popupClose")))
        {
            SavePopup.Complete();
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}


