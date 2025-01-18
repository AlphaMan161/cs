// ILSpyBased#2
using UnityEngine;

public class GUISkinManager
{
    private static GUISkin ico;

    private static GUISkin battleIco;

    private static GUISkin battleIcoWeapon;

    private static GUISkin backgound;

    private static GUISkin battleBackgound;

    private static GUISkin button;

    private static GUISkin separators;

    private static GUISkin text;

    private static GUISkin battleText;

    private static GUISkin partsGear;

    private static GUISkin partsWeapon;

    private static GUISkin main;

    private static GUISkin label;

    private static GUISkin progressBar;

    private static GUISkin hover;

    private static GUISkin battle;

    private static GUISkin dropDownList;

    private static GUISkin mainMenuCameraFix;

    private static GUISkin battleFrags;

    private static GUISkin statsLeague;

    private static GUISkin debugSkin;

    public static GUISkin Ico
    {
        get
        {
            if ((Object)GUISkinManager.ico == (Object)null)
            {
                GUISkinManager.ico = (GUISkin)Resources.Load("GUI/Skins/Ico");
            }
            return GUISkinManager.ico;
        }
    }

    public static GUISkin BattleIco
    {
        get
        {
            if ((Object)GUISkinManager.battleIco == (Object)null)
            {
                GUISkinManager.battleIco = (GUISkin)Resources.Load("GUI/Skins/BattleIco");
            }
            return GUISkinManager.battleIco;
        }
    }

    public static GUISkin BattleIcoWeapon
    {
        get
        {
            if ((Object)GUISkinManager.battleIcoWeapon == (Object)null)
            {
                GUISkinManager.battleIcoWeapon = (GUISkin)Resources.Load("GUI/Skins/BattleIcoWeapon");
            }
            return GUISkinManager.battleIcoWeapon;
        }
    }

    public static GUISkin Backgound
    {
        get
        {
            if ((Object)GUISkinManager.backgound == (Object)null)
            {
                GUISkinManager.backgound = (GUISkin)Resources.Load("GUI/Skins/Backgound");
            }
            return GUISkinManager.backgound;
        }
    }

    public static GUISkin BattleBackgound
    {
        get
        {
            if ((Object)GUISkinManager.battleBackgound == (Object)null)
            {
                GUISkinManager.battleBackgound = (GUISkin)Resources.Load("GUI/Skins/BattleBackgound");
            }
            return GUISkinManager.battleBackgound;
        }
    }

    public static GUISkin Button
    {
        get
        {
            if ((Object)GUISkinManager.button == (Object)null)
            {
                GUISkinManager.button = (GUISkin)Resources.Load("GUI/Skins/Buttons");
            }
            return GUISkinManager.button;
        }
    }

    public static GUISkin Separators
    {
        get
        {
            if ((Object)GUISkinManager.separators == (Object)null)
            {
                GUISkinManager.separators = (GUISkin)Resources.Load("GUI/Skins/Separators");
            }
            return GUISkinManager.separators;
        }
    }

    public static GUISkin Text
    {
        get
        {
            if ((Object)GUISkinManager.text == (Object)null)
            {
                GUISkinManager.text = (GUISkin)Resources.Load("GUI/Skins/Text");
            }
            return GUISkinManager.text;
        }
    }

    public static GUISkin BattleText
    {
        get
        {
            if ((Object)GUISkinManager.battleText == (Object)null)
            {
                GUISkinManager.battleText = (GUISkin)Resources.Load("GUI/Skins/BattleText");
            }
            return GUISkinManager.battleText;
        }
    }

    public static GUISkin PartsGear
    {
        get
        {
            if ((Object)GUISkinManager.partsGear == (Object)null)
            {
                GUISkinManager.partsGear = (GUISkin)Resources.Load("GUI/Skins/PartsGear");
            }
            return GUISkinManager.partsGear;
        }
    }

    public static GUISkin PartsWeapon
    {
        get
        {
            if ((Object)GUISkinManager.partsWeapon == (Object)null)
            {
                GUISkinManager.partsWeapon = (GUISkin)Resources.Load("GUI/Skins/PartsWeapon");
            }
            return GUISkinManager.partsWeapon;
        }
    }

    public static GUISkin Main
    {
        get
        {
            if ((Object)GUISkinManager.main == (Object)null)
            {
                GUISkinManager.main = (GUISkin)Resources.Load("GUI/Skins/Main");
            }
            return GUISkinManager.main;
        }
    }

    public static GUISkin Label
    {
        get
        {
            if ((Object)GUISkinManager.label == (Object)null)
            {
                GUISkinManager.label = (GUISkin)Resources.Load("GUI/Skins/Label");
            }
            return GUISkinManager.label;
        }
    }

    public static GUISkin ProgressBar
    {
        get
        {
            if ((Object)GUISkinManager.progressBar == (Object)null)
            {
                GUISkinManager.progressBar = (GUISkin)Resources.Load("GUI/Skins/Progressbar");
            }
            return GUISkinManager.progressBar;
        }
    }

    public static GUISkin Hover
    {
        get
        {
            if ((Object)GUISkinManager.hover == (Object)null)
            {
                GUISkinManager.hover = (GUISkin)Resources.Load("GUI/Skins/Hover");
            }
            return GUISkinManager.hover;
        }
    }

    public static GUISkin Battle
    {
        get
        {
            if ((Object)GUISkinManager.battle == (Object)null)
            {
                GUISkinManager.battle = (GUISkin)Resources.Load("Skins/BattleWindowGUI");
            }
            return GUISkinManager.battle;
        }
    }

    public static GUISkin DropDownList
    {
        get
        {
            if ((Object)GUISkinManager.dropDownList == (Object)null)
            {
                GUISkinManager.dropDownList = (GUISkin)Resources.Load("GUI/Skins/DropDownList");
            }
            return GUISkinManager.dropDownList;
        }
    }

    public static GUISkin MainMenuCameraFix
    {
        get
        {
            if ((Object)GUISkinManager.mainMenuCameraFix == (Object)null)
            {
                GUISkinManager.mainMenuCameraFix = (GUISkin)Resources.Load("GUI/Skins/MainMenuCameraFix");
            }
            return GUISkinManager.mainMenuCameraFix;
        }
    }

    public static GUISkin BattleFrags
    {
        get
        {
            if ((Object)GUISkinManager.battleFrags == (Object)null)
            {
                GUISkinManager.battleFrags = (GUISkin)Resources.Load("GUI/Skins/BattleFrags");
            }
            return GUISkinManager.battleFrags;
        }
    }

    public static GUISkin StatsLeague
    {
        get
        {
            if ((Object)GUISkinManager.statsLeague == (Object)null)
            {
                GUISkinManager.statsLeague = (GUISkin)Resources.Load("GUI/Skins/StatsLeague");
            }
            return GUISkinManager.statsLeague;
        }
    }

    public static GUISkin DebugSkin
    {
        get
        {
            if ((Object)GUISkinManager.debugSkin == (Object)null)
            {
                GUISkinManager.debugSkin = (GUISkin)Resources.Load("GUI/Skins/Debug");
            }
            return GUISkinManager.debugSkin;
        }
    }
}


