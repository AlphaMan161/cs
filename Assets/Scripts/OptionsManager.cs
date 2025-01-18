// ILSpyBased#2
using System;
using UnityEngine;

public class OptionsManager
{
    private static OptionsManager hInstance;

    private bool enableBadWorldFilter = true;

    private bool enableBattleChat = true;

    private bool soundIsMute;

    private float soundVolMaster = 0.5f;

    private float soundVolMusic;

    private float soundVolEffect = 0.5f;

    private QualityLevel qualityLevel = QualityLevel.Good;

    private bool vSync = true;

    private float sensivity = 3f;

    private bool invertMouse;

    private bool showTargetPointers = true;

    private static bool invertMouseY;

    private static float mouseSensitivityX = 230f;

    private static int shiptype;

    private static int team;

    private static int gameMode;

    private static string username = string.Empty;

    private static string roomname = string.Empty;

    private static MyRoomSetting roomsetting;

    private static Map connectingMap;

    public static OptionsManager Instance
    {
        get
        {
            if (OptionsManager.hInstance == null)
            {
                OptionsManager.hInstance = new OptionsManager();
                if (PlayerPrefs.HasKey("use"))
                {
                    OptionsManager.hInstance.Load();
                }
                else
                {
                    OptionsManager.hInstance.Save();
                }
            }
            return OptionsManager.hInstance;
        }
    }

    public static bool EnableBadWorldFilter
    {
        get
        {
            return OptionsManager.Instance.enableBadWorldFilter;
        }
        set
        {
            OptionsManager.Instance.enableBadWorldFilter = value;
        }
    }

    public static bool EnableBattleChat
    {
        get
        {
            return OptionsManager.Instance.enableBattleChat;
        }
        set
        {
            OptionsManager.Instance.enableBattleChat = value;
        }
    }

    public static bool SoundIsMute
    {
        get
        {
            return OptionsManager.Instance.soundIsMute;
        }
        set
        {
            if (OptionsManager.Instance.soundIsMute == value || !value)
            {
                ;
            }
            OptionsManager.Instance.soundIsMute = value;
            AudioListener.pause = value;
        }
    }

    public static float SoundVolumeMaster
    {
        get
        {
            return OptionsManager.Instance.soundVolMaster;
        }
        set
        {
            AudioListener.volume = value;
            OptionsManager.Instance.soundVolMaster = AudioListener.volume;
        }
    }

    public static float SoundVolumeMusic
    {
        get
        {
            return OptionsManager.Instance.soundVolMusic;
        }
        set
        {
            OptionsManager.Instance.soundVolMusic = value;
        }
    }

    public static float SoundVolumeEffect
    {
        get
        {
            return OptionsManager.Instance.soundVolEffect;
        }
        set
        {
            OptionsManager.Instance.soundVolEffect = value;
        }
    }

    public static QualityLevel QualityLevel
    {
        get
        {
            return OptionsManager.Instance.qualityLevel;
        }
        set
        {
            if (QualitySettings.currentLevel == value)
            {
                goto IL_000b;
            }
            goto IL_000b;
            IL_000b:
            QualitySettings.currentLevel = value;
            OptionsManager.Instance.qualityLevel = QualitySettings.currentLevel;
        }
    }

    public static bool VSync
    {
        get
        {
            return OptionsManager.Instance.vSync;
        }
        set
        {
            OptionsManager.Instance.vSync = value;
            if (OptionsManager.Instance.vSync)
            {
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
            }
        }
    }

    public static bool FullScreen
    {
        get
        {
            return Screen.fullScreen;
        }
        set
        {
            if (Screen.fullScreen != value)
            {
                Screen.fullScreen = value;
                if (value)
                {
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                }
            }
        }
    }

    public static float Sensivity
    {
        get
        {
            return OptionsManager.Instance.sensivity;
        }
        set
        {
            OptionsManager.Instance.sensivity = value;
            OptionsManager.mouseSensitivityX = value * 50f;
        }
    }

    public static bool InvertMouse
    {
        get
        {
            return OptionsManager.Instance.invertMouse;
        }
        set
        {
            OptionsManager.Instance.invertMouse = value;
            OptionsManager.InvertMouseY = value;
        }
    }

    public static CameraOrbitMode MouseMode
    {
        get
        {
            return Configuration.MouseMode;
        }
        set
        {
            Configuration.MouseMode = value;
        }
    }

    public static bool ShowTargetPointers
    {
        get
        {
            return OptionsManager.Instance.showTargetPointers;
        }
        set
        {
            OptionsManager.Instance.showTargetPointers = value;
        }
    }

    public static bool InvertMouseY
    {
        get
        {
            return OptionsManager.invertMouseY;
        }
        set
        {
            OptionsManager.invertMouseY = value;
        }
    }

    public static float MouseSensityX
    {
        get
        {
            return OptionsManager.mouseSensitivityX;
        }
        set
        {
            OptionsManager.mouseSensitivityX = value;
        }
    }

    public static int ShipType
    {
        get
        {
            return OptionsManager.shiptype;
        }
        set
        {
            OptionsManager.shiptype = value;
        }
    }

    public static int Team
    {
        get
        {
            return OptionsManager.team;
        }
        set
        {
            OptionsManager.team = value;
        }
    }

    public static int GameMode
    {
        get
        {
            return OptionsManager.gameMode;
        }
        set
        {
            OptionsManager.gameMode = value;
        }
    }

    public static string UserName
    {
        get
        {
            return OptionsManager.username;
        }
        set
        {
            OptionsManager.username = value;
        }
    }

    public static string RoomName
    {
        get
        {
            return OptionsManager.roomname;
        }
        set
        {
            OptionsManager.roomname = value;
        }
    }

    public static MyRoomSetting RoomSetting
    {
        get
        {
            return OptionsManager.roomsetting;
        }
        set
        {
            OptionsManager.roomsetting = value;
        }
    }

    public static Map ConnectingMap
    {
        get
        {
            return OptionsManager.connectingMap;
        }
        set
        {
            OptionsManager.connectingMap = value;
        }
    }

    public void Debuging(string command)
    {
        if (command == "list")
        {
            if (PlayerPrefs.HasKey("smute"))
            {
                UnityEngine.Debug.Log(string.Format("smute: {0}", Convert.ToBoolean(PlayerPrefs.GetString("smute"))));
            }
            else
            {
                UnityEngine.Debug.Log(string.Format("smute: not set"));
            }
            if (PlayerPrefs.HasKey("sma"))
            {
                UnityEngine.Debug.Log(string.Format("sma: {0}", PlayerPrefs.GetFloat("sma")));
            }
            else
            {
                UnityEngine.Debug.Log(string.Format("sma: not set"));
            }
            if (PlayerPrefs.HasKey("smu"))
            {
                UnityEngine.Debug.Log(string.Format("smu: {0}", PlayerPrefs.GetFloat("smu")));
            }
            else
            {
                UnityEngine.Debug.Log(string.Format("smu: not set"));
            }
            if (PlayerPrefs.HasKey("sme"))
            {
                UnityEngine.Debug.Log(string.Format("sme: {0}", PlayerPrefs.GetFloat("sme")));
            }
            else
            {
                UnityEngine.Debug.Log(string.Format("sme: not set"));
            }
        }
        else if (command == "clear")
        {
            PlayerPrefs.DeleteAll();
            UnityEngine.Debug.Log("[OptionsManager] PlayerPrefs cleared");
        }
        else
        {
            UnityEngine.Debug.Log("[OptionsManager] unkown command. available: list, clear");
        }
    }

    public void Save()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("use", "true");
        PlayerPrefs.SetString("smute", OptionsManager.SoundIsMute.ToString());
        PlayerPrefs.SetFloat("sma", OptionsManager.SoundVolumeMaster);
        PlayerPrefs.SetFloat("smu", OptionsManager.SoundVolumeMusic);
        PlayerPrefs.SetFloat("sme", OptionsManager.SoundVolumeEffect);
        PlayerPrefs.SetInt("vq", (int)OptionsManager.QualityLevel);
        PlayerPrefs.SetString("vsync", OptionsManager.VSync.ToString());
        PlayerPrefs.SetFloat("cmsensity", OptionsManager.Sensivity);
        PlayerPrefs.SetString("cminvert", OptionsManager.InvertMouse.ToString());
        PlayerPrefs.SetString("cmshowTP", OptionsManager.ShowTargetPointers.ToString());
        PlayerPrefs.SetInt("cmmode", (int)OptionsManager.MouseMode);
        PlayerPrefs.SetInt("cbForward", (int)TRInput.Forward);
        PlayerPrefs.SetInt("cbBackward", (int)TRInput.Backward);
        PlayerPrefs.SetInt("cbLeftStrafe", (int)TRInput.LeftStrafe);
        PlayerPrefs.SetInt("cbRightStrafe", (int)TRInput.RightStrafe);
        PlayerPrefs.SetInt("cbJump", (int)TRInput.Jump);
        PlayerPrefs.SetInt("cbWeapon1", (int)TRInput.Weapon1);
        PlayerPrefs.SetInt("cbWeapon2", (int)TRInput.Weapon2);
        PlayerPrefs.SetInt("cbWeapon3", (int)TRInput.Weapon3);
        PlayerPrefs.SetInt("cbWeapon4", (int)TRInput.Weapon4);
        PlayerPrefs.SetInt("cbWeapon5", (int)TRInput.Weapon5);
        PlayerPrefs.SetInt("cbWeapon6", (int)TRInput.Weapon6);
        PlayerPrefs.SetInt("cbWeapon7", (int)TRInput.Weapon7);
        PlayerPrefs.SetInt("cbEnterChat", (int)TRInput.EnterChat);
        PlayerPrefs.SetInt("cbCallTechnic", (int)TRInput.CallTechnic);
        PlayerPrefs.SetInt("cbFire1", (int)TRInput.Fire1);
        PlayerPrefs.SetInt("cbQuickChange", (int)TRInput.QuickChange);
        PlayerPrefs.SetInt("cbEnhancer1", (int)TRInput.Enhancer1);
        PlayerPrefs.SetInt("cbEnhancer2", (int)TRInput.Enhancer2);
        PlayerPrefs.SetInt("cbZoom", (int)TRInput.Zoom);
        PlayerPrefs.SetInt("cbReload", (int)TRInput.Reload);
        PlayerPrefs.SetInt("cbTaunt1", (int)TRInput.Taunt1);
        PlayerPrefs.SetInt("cbTaunt2", (int)TRInput.Taunt2);
        PlayerPrefs.SetInt("cbTaunt3", (int)TRInput.Taunt3);
        PlayerPrefs.SetInt("cbScreenShot", (int)TRInput.ScreenShot);
        PlayerPrefs.SetString("ebwf", OptionsManager.EnableBadWorldFilter.ToString());
        PlayerPrefs.SetString("ebc", OptionsManager.EnableBattleChat.ToString());
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("smute"))
        {
            OptionsManager.SoundIsMute = Convert.ToBoolean(PlayerPrefs.GetString("smute"));
        }
        if (PlayerPrefs.HasKey("sma"))
        {
            OptionsManager.SoundVolumeMaster = PlayerPrefs.GetFloat("sma");
        }
        if (PlayerPrefs.HasKey("smu"))
        {
            OptionsManager.SoundVolumeMusic = PlayerPrefs.GetFloat("smu");
        }
        if (PlayerPrefs.HasKey("sme"))
        {
            OptionsManager.SoundVolumeEffect = PlayerPrefs.GetFloat("sme");
        }
        if (PlayerPrefs.HasKey("vq"))
        {
            OptionsManager.QualityLevel = (QualityLevel)PlayerPrefs.GetInt("vq");
        }
        if (PlayerPrefs.HasKey("vsync"))
        {
            OptionsManager.VSync = Convert.ToBoolean(PlayerPrefs.GetInt("vsync"));
        }
        if (PlayerPrefs.HasKey("cmsensity"))
        {
            OptionsManager.Sensivity = PlayerPrefs.GetFloat("cmsensity");
        }
        if (PlayerPrefs.HasKey("cminvert"))
        {
            OptionsManager.InvertMouse = Convert.ToBoolean(PlayerPrefs.GetString("cminvert"));
        }
        if (PlayerPrefs.HasKey("cmshowTP"))
        {
            OptionsManager.ShowTargetPointers = Convert.ToBoolean(PlayerPrefs.GetString("cmshowTP"));
        }
        if (PlayerPrefs.HasKey("cmmode"))
        {
            OptionsManager.MouseMode = (CameraOrbitMode)(byte)PlayerPrefs.GetInt("cmmode");
        }
        if (PlayerPrefs.HasKey("cbForward"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Forward, (KeyCode)PlayerPrefs.GetInt("cbForward"));
        }
        if (PlayerPrefs.HasKey("cbBackward"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Backward, (KeyCode)PlayerPrefs.GetInt("cbBackward"));
        }
        if (PlayerPrefs.HasKey("cbLeftStrafe"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.LeftStrafe, (KeyCode)PlayerPrefs.GetInt("cbLeftStrafe"));
        }
        if (PlayerPrefs.HasKey("cbRightStrafe"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.RightStrafe, (KeyCode)PlayerPrefs.GetInt("cbRightStrafe"));
        }
        if (PlayerPrefs.HasKey("cbJump"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Jump, (KeyCode)PlayerPrefs.GetInt("cbJump"));
        }
        if (PlayerPrefs.HasKey("cbWeapon1"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Weapon1, (KeyCode)PlayerPrefs.GetInt("cbWeapon1"));
        }
        if (PlayerPrefs.HasKey("cbWeapon2"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Weapon2, (KeyCode)PlayerPrefs.GetInt("cbWeapon2"));
        }
        if (PlayerPrefs.HasKey("cbWeapon3"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Weapon3, (KeyCode)PlayerPrefs.GetInt("cbWeapon3"));
        }
        if (PlayerPrefs.HasKey("cbWeapon4"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Weapon4, (KeyCode)PlayerPrefs.GetInt("cbWeapon4"));
        }
        if (PlayerPrefs.HasKey("cbWeapon5"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Weapon5, (KeyCode)PlayerPrefs.GetInt("cbWeapon5"));
        }
        if (PlayerPrefs.HasKey("cbWeapon6"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Weapon6, (KeyCode)PlayerPrefs.GetInt("cbWeapon6"));
        }
        if (PlayerPrefs.HasKey("cbWeapon7"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Weapon7, (KeyCode)PlayerPrefs.GetInt("cbWeapon7"));
        }
        if (PlayerPrefs.HasKey("cbEnterChat"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.EnterChat, (KeyCode)PlayerPrefs.GetInt("cbEnterChat"));
        }
        if (PlayerPrefs.HasKey("cbCallTechnic"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.CallTechnic, (KeyCode)PlayerPrefs.GetInt("cbCallTechnic"));
        }
        if (PlayerPrefs.HasKey("cbFire1"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Fire1, (KeyCode)PlayerPrefs.GetInt("cbFire1"));
        }
        if (PlayerPrefs.HasKey("cbQuickChange"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.QuickChange, (KeyCode)PlayerPrefs.GetInt("cbQuickChange"));
        }
        if (PlayerPrefs.HasKey("cbEnhancer1"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Enhancer1, (KeyCode)PlayerPrefs.GetInt("cbEnhancer1"));
        }
        if (PlayerPrefs.HasKey("cbEnhancer2"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Enhancer2, (KeyCode)PlayerPrefs.GetInt("cbEnhancer2"));
        }
        if (PlayerPrefs.HasKey("cbZoom"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Zoom, (KeyCode)PlayerPrefs.GetInt("cbZoom"));
        }
        if (PlayerPrefs.HasKey("cbReload"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Reload, (KeyCode)PlayerPrefs.GetInt("cbReload"));
        }
        if (PlayerPrefs.HasKey("cbTaunt1"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Taunt1, (KeyCode)PlayerPrefs.GetInt("cbTaunt1"));
        }
        if (PlayerPrefs.HasKey("cbTaunt2"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Taunt2, (KeyCode)PlayerPrefs.GetInt("cbTaunt2"));
        }
        if (PlayerPrefs.HasKey("cbTaunt3"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.Taunt3, (KeyCode)PlayerPrefs.GetInt("cbTaunt3"));
        }
        if (PlayerPrefs.HasKey("cbScreenShot"))
        {
            TRInputHelper.SetButton(TRInput.TRKeyCodeDefault.ScreenShot, (KeyCode)PlayerPrefs.GetInt("cbScreenShot"));
        }
        if (PlayerPrefs.HasKey("ebwf"))
        {
            OptionsManager.EnableBadWorldFilter = Convert.ToBoolean(PlayerPrefs.GetString("ebwf"));
        }
        if (PlayerPrefs.HasKey("ebc"))
        {
            OptionsManager.EnableBattleChat = Convert.ToBoolean(PlayerPrefs.GetString("ebc"));
        }
    }
}


