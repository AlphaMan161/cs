// ILSpyBased#2
public class WeaponTypeHelper
{
    public static int GetWeaponSlot(WeaponType wt)
    {
        switch (wt)
        {
            case WeaponType.ONE_HANDED_COLD_ARMS:
            case WeaponType.TWO_HANDED_COLD_ARMS:
                return 1;
            case WeaponType.HAND_GUN:
                return 2;
            case WeaponType.MACHINE_GUN:
                return 3;
            case WeaponType.FLAMER:
            case WeaponType.GATLING_GUN:
            case WeaponType.SNOW_GUN:
            case WeaponType.ACID_THROWER:
                return 4;
            case WeaponType.SHOT_GUN:
                return 5;
            case WeaponType.ROCKET_LAUNCHER:
            case WeaponType.GRENADE_LAUNCHER:
            case WeaponType.BOMB_LAUNCHER:
                return 6;
            case WeaponType.SNIPER_RIFLE:
                return 7;
            default:
                return 0;
        }
    }
}


