// ILSpyBased#2
using System;
using UnityEngine;

public class OptionPopup : MonoBehaviour
{
    private static bool isShowWindow = false;

    private static Rect windowRect;

    private static int selectMenu = 1;

    private static bool targetPointers = true;

    private static bool isSetNewButton = false;

    private static TRInput.TRKeyCodeDefault setButtonKey;

    private static Vector2 keybordScroll = new Vector2(0f, 0f);

    public int tmp_style_index;

    public static bool Show
    {
        get
        {
            return OptionPopup.isShowWindow;
        }
        set
        {
            OptionPopup.isShowWindow = value;
        }
    }

    private void Start()
    {
        OptionPopup.windowRect = new Rect((float)(Screen.width - 390) * 0.5f, (float)(Screen.height - 420) * 0.5f, 390f, 437f);
    }

    private void OnGUI()
    {
        GUISkin skin = GUI.skin;
        GUI.skin = GUISkinManager.Main;
        if (Time.frameCount % 30 == 0)
        {
            OptionPopup.windowRect = new Rect((float)(Screen.width - 390) * 0.5f, (float)(Screen.height - 420) * 0.5f, 390f, 437f);
        }
        if (OptionPopup.isShowWindow)
        {
            GUIHover.Enable = false;
            GUI.Window(1, OptionPopup.windowRect, new GUI.WindowFunction(this.draw_popup), string.Empty, GUISkinManager.Backgound.window);
            GUI.Button(OptionPopup.windowRect, GUIContent.none, GUIStyle.none);
        }
        GUI.skin = skin;
    }

    private void draw_popup(int windowId)
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("OPTIONS"), GUISkinManager.Text.GetStyle("popupTitle"), GUILayout.Height(34f));
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("selectGrid"), GUILayout.Height(32f));
        OptionPopup.selectMenu = GUILayout.Toolbar(OptionPopup.selectMenu, new string[3] {
            LanguageManager.GetText("CONTROLS"),
            LanguageManager.GetText("VIDEO"),
            LanguageManager.GetText("AUDIO")
        }, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Height(28f), GUILayout.MinWidth(350f));
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        if (OptionPopup.selectMenu == 0)
        {
            this.draw_Controls();
        }
        else if (OptionPopup.selectMenu == 1)
        {
            this.draw_Video();
        }
        else if (OptionPopup.selectMenu == 2)
        {
            this.draw_Audio();
        }
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(LanguageManager.GetText("OK"), GUISkinManager.Button.GetStyle("green"), GUILayout.Width(106f), GUILayout.Height(37f)))
        {
            this.clickOK();
            GUIHover.Enable = true;
        }
        GUILayout.Space(5f);
        if (GUILayout.Button(LanguageManager.GetText("Cancel"), GUISkinManager.Button.GetStyle("cancel"), GUILayout.Width(106f), GUILayout.Height(37f)))
        {
            this.clickReset();
            GUIHover.Enable = true;
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(6f);
        if (GUI.Button(new Rect(OptionPopup.windowRect.width - 32f, 2f, 30f, 30f), GUIContent.none, GUISkinManager.Button.GetStyle("popupClose")))
        {
            OptionPopup.isShowWindow = false;
        }
    }

    private void draw_Controls()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Mouse Sensitivity"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(121f), GUILayout.Height(29f));
        GUILayout.Label(OptionsManager.Sensivity.ToString("0.00"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(36f), GUILayout.Height(29f));
        OptionsManager.Sensivity = GUILayout.HorizontalSlider(OptionsManager.Sensivity, 0.01f, 10f, GUILayout.Width(170f));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Invert mouse"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(121f), GUILayout.Height(29f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("selectGrid"), GUILayout.Height(32f));
        OptionsManager.InvertMouse = !Convert.ToBoolean(GUILayout.SelectionGrid((!OptionsManager.InvertMouse) ? 1 : 0, new string[2] {
            LanguageManager.GetText("YES"),
            LanguageManager.GetText("NO")
        }, 2, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Height(28f), GUILayout.MinWidth(91f)));
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Mouse mode"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(121f), GUILayout.Height(29f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("selectGrid"), GUILayout.Height(32f));
        OptionsManager.MouseMode = (CameraOrbitMode)(byte)(GUILayout.SelectionGrid((int)(OptionsManager.MouseMode - 1), new string[2] {
            LanguageManager.GetText("Default"),
            LanguageManager.GetText("Alternative")
        }, 2, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.MinWidth(192f), GUILayout.Height(28f)) + 1);
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Keyboard Settings"), GUISkinManager.Text.GetStyle("room"), GUILayout.Height(28f));
        if (GUILayout.Button(LanguageManager.GetText("by default"), GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Width(150f), GUILayout.Height(22f)))
        {
            TRInputHelper.ResetDefault();
        }
        GUILayout.EndHorizontal();
        OptionPopup.keybordScroll = GUILayout.BeginScrollView(OptionPopup.keybordScroll, false, true, GUILayout.MinHeight(112f));
        this.controlButton(TRInput.TRKeyCodeDefault.Forward, TRInput.Forward);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Backward, TRInput.Backward);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.LeftStrafe, TRInput.LeftStrafe);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.RightStrafe, TRInput.RightStrafe);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Crouch, TRInput.Crouch);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Jump, TRInput.Jump);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Weapon1, TRInput.Weapon1);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Weapon2, TRInput.Weapon2);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Weapon3, TRInput.Weapon3);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Weapon4, TRInput.Weapon4);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Weapon5, TRInput.Weapon5);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Weapon6, TRInput.Weapon6);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Weapon7, TRInput.Weapon7);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Fire1, TRInput.Fire1);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Zoom, TRInput.Zoom);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Reload, TRInput.Reload);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Enhancer1, TRInput.Enhancer1);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Enhancer2, TRInput.Enhancer2);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.QuickChange, TRInput.QuickChange);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Taunt1, TRInput.Taunt1);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Taunt2, TRInput.Taunt2);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.Taunt3, TRInput.Taunt3);
        GUILayout.Space(2f);
        this.controlButton(TRInput.TRKeyCodeDefault.ScreenShot, TRInput.ScreenShot);
        GUILayout.EndScrollView();
        if (OptionPopup.isSetNewButton)
        {
            Event current = Event.current;
            if (!current.isKey && !current.isMouse)
            {
                return;
            }
            if (current.isKey)
            {
                TRInputHelper.SetButton(OptionPopup.setButtonKey, current.keyCode);
            }
            if (current.isMouse)
            {
                if (current.button == 0)
                {
                    TRInputHelper.SetButton(OptionPopup.setButtonKey, KeyCode.Mouse0);
                }
                if (current.button == 1)
                {
                    TRInputHelper.SetButton(OptionPopup.setButtonKey, KeyCode.Mouse1);
                }
                if (current.button == 2)
                {
                    TRInputHelper.SetButton(OptionPopup.setButtonKey, KeyCode.Mouse2);
                }
            }
            OptionPopup.isSetNewButton = false;
        }
    }

    private void draw_Video()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none);
        GUILayout.Space(113f);
        GUILayout.BeginVertical(GUIContent.none, GUIStyle.none);
        GUILayout.Label(LanguageManager.GetText("Quality Settings"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(128f), GUILayout.Height(28f));
        QualityLevel qualityLevel = OptionsManager.QualityLevel;
        OptionsManager.QualityLevel = (QualityLevel)GUILayout.SelectionGrid((int)OptionsManager.QualityLevel, new string[5] {
            LanguageManager.GetText("Fast"),
            LanguageManager.GetText("Simple"),
            LanguageManager.GetText("Good"),
            LanguageManager.GetText("Beautiful"),
            LanguageManager.GetText("Fantastic")
        }, 1, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Width(122f), GUILayout.Height(150f));
        if (qualityLevel != OptionsManager.QualityLevel)
        {
            if ((UnityEngine.Object)PlayerManager.Instance != (UnityEngine.Object)null)
            {
                PlayerManager.Instance.SetCharacterQuality(OptionsManager.QualityLevel);
            }
            else if ((UnityEngine.Object)CharacterCameraManager.Instance != (UnityEngine.Object)null)
            {
                CharacterCameraManager.Instance.SetCharacterQuality(OptionsManager.QualityLevel);
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Space(15f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(32f));
        GUILayout.Space(15f);
        GUILayout.Label(LanguageManager.GetText("VSync"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(50f), GUILayout.Height(29f));
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("selectGrid"), GUILayout.Height(32f));
        OptionsManager.VSync = !Convert.ToBoolean(GUILayout.SelectionGrid((!OptionsManager.VSync) ? 1 : 0, new string[2] {
            LanguageManager.GetText("YES"),
            LanguageManager.GetText("NO")
        }, 2, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Height(28f), GUILayout.MinWidth(91f)));
        GUILayout.EndHorizontal();
        if (!OptionsManager.VSync)
        {
            GUILayout.Space(5f);
            GUILayout.Label(LanguageManager.GetText("Unstable playing is possible"), GUISkinManager.Text.GetStyle("room"), GUILayout.Height(29f));
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(7f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(32f));
        GUILayout.Space(15f);
        GUILayout.Label(LanguageManager.GetText("Game chat"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(100f), GUILayout.Height(29f));
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("selectGrid"), GUILayout.Height(32f));
        OptionsManager.EnableBattleChat = !Convert.ToBoolean(GUILayout.SelectionGrid((!OptionsManager.EnableBattleChat) ? 1 : 0, new string[2] {
            LanguageManager.GetText("On"),
            LanguageManager.GetText("Off")
        }, 2, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Height(28f), GUILayout.MinWidth(100f)));
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
    }

    private void draw_Audio()
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Master Volume"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(121f), GUILayout.Height(29f));
        GUILayout.Label(((int)(OptionsManager.SoundVolumeMaster * 100f)).ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(36f), GUILayout.Height(29f));
        OptionsManager.SoundVolumeMaster = GUILayout.HorizontalSlider(OptionsManager.SoundVolumeMaster, 0f, 1f, GUILayout.Width(150f));
        GUILayout.EndHorizontal();
        GUILayout.Space(14f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Music Volume"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(121f), GUILayout.Height(29f));
        GUILayout.Label(((int)(OptionsManager.SoundVolumeMusic * 100f)).ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(36f), GUILayout.Height(29f));
        OptionsManager.SoundVolumeMusic = GUILayout.HorizontalSlider(OptionsManager.SoundVolumeMusic, 0f, 1f, GUILayout.Width(150f));
        GUILayout.EndHorizontal();
        GUILayout.Space(14f);
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Effects Volume"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(121f), GUILayout.Height(29f));
        GUILayout.Label(((int)(OptionsManager.SoundVolumeEffect * 100f)).ToString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(36f), GUILayout.Height(29f));
        OptionsManager.SoundVolumeEffect = GUILayout.HorizontalSlider(OptionsManager.SoundVolumeEffect, 0f, 1f, GUILayout.Width(150f));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(43f));
        GUILayout.Label(LanguageManager.GetText("Mute audio"), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(121f), GUILayout.Height(29f));
        GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("selectGrid"), GUILayout.Height(32f));
        OptionsManager.SoundIsMute = !Convert.ToBoolean(GUILayout.SelectionGrid((!OptionsManager.SoundIsMute) ? 1 : 0, new string[2] {
            LanguageManager.GetText("YES"),
            LanguageManager.GetText("NO")
        }, 2, GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Height(28f), GUILayout.MinWidth(91f)));
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();
    }

    private void controlButton(TRInput.TRKeyCodeDefault input, KeyCode keyCode)
    {
        GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, GUILayout.Height(28f));
        GUILayout.Space(50f);
        GUILayout.Label(input.ToDisplayString(), GUISkinManager.Text.GetStyle("room"), GUILayout.Width(120f));
        if (OptionPopup.isSetNewButton && OptionPopup.setButtonKey == input)
        {
            GUILayout.Label(LanguageManager.GetText("Press key.."), GUISkinManager.Button.GetStyle("active01"), GUILayout.Width(150f), GUILayout.Height(22f));
        }
        else if (OptionPopup.isSetNewButton)
        {
            GUILayout.Label(keyCode.ToDisplayString(), GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Width(150f), GUILayout.Height(22f));
        }
        else if (GUILayout.Button(keyCode.ToDisplayString(), GUISkinManager.Button.GetStyle("selectGrid"), GUILayout.Width(150f), GUILayout.Height(22f)))
        {
            OptionPopup.isSetNewButton = true;
            OptionPopup.setButtonKey = input;
        }
        GUILayout.EndHorizontal();
    }

    private void clickReset()
    {
        OptionsManager.Instance.Load();
        OptionPopup.isShowWindow = false;
    }

    private void clickOK()
    {
        OptionsManager.Instance.Save();
        OptionPopup.isShowWindow = false;
    }
}


