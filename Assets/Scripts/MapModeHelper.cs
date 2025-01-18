// ILSpyBased#2
using System;

public static class MapModeHelper
{
    private static string[] nameList1 = null;

    private static string[] nameList2 = null;

    private static string[] nameList3 = null;

    private static Random nameRnd = new Random();

    public static string GenerateName()
    {
        if (MapModeHelper.nameList1 == null || MapModeHelper.nameList2 == null || MapModeHelper.nameList3 == null)
        {
            MapModeHelper.nameList1 = LanguageManager.GetText("map_words1").Split(';');
            MapModeHelper.nameList2 = LanguageManager.GetText("map_words2").Split(';');
            MapModeHelper.nameList3 = LanguageManager.GetText("map_words3").Split(';');
        }
        return string.Format("{0} {1} {2}", MapModeHelper.nameList1[MapModeHelper.nameRnd.Next(0, MapModeHelper.nameList1.Length)], MapModeHelper.nameList2[MapModeHelper.nameRnd.Next(0, MapModeHelper.nameList2.Length)], MapModeHelper.nameList3[MapModeHelper.nameRnd.Next(0, MapModeHelper.nameList3.Length)]);
    }

    public static MapMode.MODE FromByte(byte mode)
    {
        switch (mode)
        {
            case 1:
                return MapMode.MODE.DEATHMATCH;
            case 2:
                return MapMode.MODE.TEAM_DEATHMATCH;
            case 4:
                return MapMode.MODE.CAPTURE_THE_FLAG;
            case 8:
                return MapMode.MODE.CONTROL_POINTS;
            case 16:
                return MapMode.MODE.TOWER_DEFENSE;
            case 32:
                return MapMode.MODE.ESCORT;
            case 64:
                return MapMode.MODE.ZOMBIE;
            default:
                return MapMode.MODE.DEATHMATCH;
        }
    }

    public static string ToString(this MapMode.MODE mode)
    {
        switch (mode)
        {
            case MapMode.MODE.DEATHMATCH:
                return LanguageManager.GetText("DM");
            case MapMode.MODE.TEAM_DEATHMATCH:
                return LanguageManager.GetText("TDM");
            case MapMode.MODE.CAPTURE_THE_FLAG:
                return LanguageManager.GetText("CTF");
            case MapMode.MODE.CONTROL_POINTS:
                return LanguageManager.GetText("CP");
            case MapMode.MODE.TOWER_DEFENSE:
                return LanguageManager.GetText("TDEF");
            case MapMode.MODE.ESCORT:
                return LanguageManager.GetText("ESC");
            case MapMode.MODE.ZOMBIE:
                return LanguageManager.GetText("ZM");
            default:
                return string.Empty;
        }
    }

    public static string GetFullName(this MapMode.MODE mode)
    {
        switch (mode)
        {
            case MapMode.MODE.DEATHMATCH:
                return LanguageManager.GetText("DEATHMATCH");
            case MapMode.MODE.TEAM_DEATHMATCH:
                return LanguageManager.GetText("TEAM DEATHMATCH");
            case MapMode.MODE.CAPTURE_THE_FLAG:
                return LanguageManager.GetText("CAPTURE THE FLAG");
            case MapMode.MODE.CONTROL_POINTS:
                return LanguageManager.GetText("CONTROL POINT");
            case MapMode.MODE.TOWER_DEFENSE:
                return LanguageManager.GetText("TOWER DEFENSE");
            case MapMode.MODE.ESCORT:
                return LanguageManager.GetText("ESCORT");
            case MapMode.MODE.ZOMBIE:
                return LanguageManager.GetText("ZOMBIE");
            default:
                return string.Empty;
        }
    }

    public static string GetShortDescription(this MapMode.MODE mode)
    {
        switch (mode)
        {
            case MapMode.MODE.DEATHMATCH:
                return LanguageManager.GetText("Free for all battle");
            case MapMode.MODE.TEAM_DEATHMATCH:
                return LanguageManager.GetText("Blue Team vs Red Team");
            case MapMode.MODE.CAPTURE_THE_FLAG:
                return LanguageManager.GetText("Deliver opposite team's flag to you base");
            case MapMode.MODE.CONTROL_POINTS:
                return LanguageManager.GetText("Capture and defend Control Points");
            case MapMode.MODE.TOWER_DEFENSE:
                return LanguageManager.GetText("Defend Reactor from Space Invaders");
            case MapMode.MODE.ESCORT:
                return LanguageManager.GetText("Escort the Cargo");
            case MapMode.MODE.ZOMBIE:
                return LanguageManager.GetText("Zombie short desc");
            default:
                return string.Empty;
        }
    }

    public static string GetDescription(this MapMode.MODE mode)
    {
        switch (mode)
        {
            case MapMode.MODE.DEATHMATCH:
                return LanguageManager.GetText("No teams. No allies. Fight for yourself and became the One Winner!");
            case MapMode.MODE.TEAM_DEATHMATCH:
                return LanguageManager.GetText("There's strength in numbers! Band with your teammates and fight together!");
            case MapMode.MODE.CAPTURE_THE_FLAG:
                return LanguageManager.GetText("Tactical and strategic mode. Capture an enemy’s flag and protect yours one.");
            case MapMode.MODE.CONTROL_POINTS:
                return LanguageManager.GetText("Fight for strategic superiority – this is the real key to the victory!");
            case MapMode.MODE.TOWER_DEFENSE:
                return LanguageManager.GetText("Defend the Generator not letting the Mecnics Spiders to destroy it!");
            case MapMode.MODE.ESCORT:
                return LanguageManager.GetText("Escort the cargo description");
            case MapMode.MODE.ZOMBIE:
                return LanguageManager.GetText("Zombie desc");
            default:
                return string.Empty;
        }
    }
}


