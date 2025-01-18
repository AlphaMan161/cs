// ILSpyBased#2
public static class ItemLocalization
{
    public static string GetName(this CCWearType type)
    {
        switch (type)
        {
            case CCWearType.Hats:
                return LanguageManager.GetText("Hats");
            case CCWearType.Masks:
                return LanguageManager.GetText("Masks");
            case CCWearType.Gloves:
                return LanguageManager.GetText("Gloves");
            case CCWearType.Shirts:
                return LanguageManager.GetText("Shirts");
            case CCWearType.Pants:
                return LanguageManager.GetText("Pants");
            case CCWearType.Boots:
                return LanguageManager.GetText("Boots");
            case CCWearType.Backpacks:
                return LanguageManager.GetText("Backpacks");
            case CCWearType.Heads:
                return LanguageManager.GetText("Heads");
            case CCWearType.Others:
                return LanguageManager.GetText("Others");
            default:
                return "unkown";
        }
    }

    public static string GetName(this WeaponType type)
    {
        switch (type)
        {
            case WeaponType.ONE_HANDED_COLD_ARMS:
                return LanguageManager.GetText("Melee");
            case WeaponType.TWO_HANDED_COLD_ARMS:
                return LanguageManager.GetText("Melee");
            case WeaponType.HAND_GUN:
                return LanguageManager.GetText("Pistol");
            case WeaponType.MACHINE_GUN:
                return LanguageManager.GetText("Machine Gun");
            case WeaponType.FLAMER:
                return LanguageManager.GetText("Flamer");
            case WeaponType.GATLING_GUN:
                return LanguageManager.GetText("Gatling Gun");
            case WeaponType.SHOT_GUN:
                return LanguageManager.GetText("Shot Gun");
            case WeaponType.ROCKET_LAUNCHER:
                return LanguageManager.GetText("Rocket Launcher");
            case WeaponType.GRENADE_LAUNCHER:
                return LanguageManager.GetText("Grenade Launcher");
            case WeaponType.SNIPER_RIFLE:
                return LanguageManager.GetText("Sniper Rifle");
            case WeaponType.SNOW_GUN:
                return LanguageManager.GetText("Frost Thrower");
            case WeaponType.BOMB_LAUNCHER:
                return LanguageManager.GetText("Bomb Launcher");
            default:
                return "unkown";
        }
    }
}


