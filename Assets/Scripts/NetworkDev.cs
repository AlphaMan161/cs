// ILSpyBased#2
using System;

public static class NetworkDev
{
    public static bool Remote_Animation = true;

    public static bool Remote_Animation_Send = true;

    public static int TPS = 100;

    public static int TCP_TPS = 0;

    public static bool Destroy_Geometry = true;

    public static int LagValue = 0;

    public static int DelayValue = 0;

    public static bool CreateDummyPlayers = false;

    public static bool CheckAim = false;

    public static InterpolationMode InterpolationMode = InterpolationMode.SMOOTH_LINEAR_IN_EX;

    public static bool TestPosition = false;

    public static Random randomLag = new Random();

    public static bool CheckLag()
    {
        if (NetworkDev.LagValue == 0)
        {
            return false;
        }
        if (NetworkDev.randomLag.Next(0, 100) >= NetworkDev.LagValue)
        {
            return false;
        }
        return true;
    }
}


