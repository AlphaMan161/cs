// ILSpyBased#2
using System;
using UnityEngine;

public static class Datameter
{
    public const long LAG_PERIOD = 50000000L;

    public static bool enabled = false;

    public static ShotStatistics ShotStatistics = new ShotStatistics();

    public static int MovementCounter = 0;

    public static int AnimationCounter = 0;

    public static float AnimationSizeCounter = 0f;

    public static float NetworkSizeCounter = 0f;

    public static int JumpCrouchCounter = 0;

    public static int TextureCounter = 0;

    public static float TextureSizeCounter = 0f;

    public static float MovementSizeCounter = 0f;

    private static long lastReset = DateTime.Now.Ticks;

    public static void Restart()
    {
        Datameter.Reset();
    }

    public static void Reset()
    {
        Datameter.lastReset = DateTime.Now.Ticks;
        Datameter.MovementCounter = 0;
        Datameter.AnimationCounter = 0;
        Datameter.JumpCrouchCounter = 0;
        Datameter.AnimationSizeCounter = 0f;
        Datameter.ShotStatistics.ExplosionCounter = 0;
        Datameter.ShotStatistics.ShotSizeCounter = 0f;
        Datameter.ShotStatistics.ShotCounter = 0;
        Datameter.NetworkSizeCounter = 0f;
        Datameter.MovementSizeCounter = 0f;
    }

    public static void Report()
    {
        if (Datameter.enabled && DateTime.Now.Ticks - Datameter.lastReset > 50000000 && (UnityEngine.Object)PlayerManager.Instance != (UnityEngine.Object)null)
        {
            float num = (float)PlayerManager.Instance.Players.Count;
            float num2 = (float)Datameter.MovementCounter / num;
            float num3 = (float)Datameter.AnimationCounter / num;
            float num4 = (float)Datameter.JumpCrouchCounter / num;
            float num5 = (float)Datameter.ShotStatistics.ExplosionCounter / num;
            float value = Datameter.ShotStatistics.ShotSizeCounter / num;
            float value2 = Datameter.AnimationSizeCounter / num;
            float value3 = Datameter.NetworkSizeCounter / num;
            float num6 = (float)Datameter.ShotStatistics.ShotCounter / num;
            float value4 = Datameter.MovementSizeCounter / num;
            GameHUDFPS.Instance.SetDebugLine(string.Format("Shots:{0} Explosions:{1} AnimationKeys:{2} JumpCrouchStates:{3} PositionsUDP:{4}", num6, num5, num3, num4, num2), 0);
            GameHUDFPS.Instance.SetDebugLine(string.Format("SIZES Shot:{0} Animation:{1} Move:{2} Network:{3}", Datameter.RV(value), Datameter.RV(value2), Datameter.RV(value4), Datameter.RV(value3)), 1);
            GameHUDFPS.Instance.SetDebugLine(string.Format("SIZES% Shot:{0} Animation:{1} Move:{2} Network:{3}", Datameter.PSC(value), Datameter.PSC(value2), Datameter.PSC(value4), Datameter.PSC(value3)), 2);
        }
    }

    private static int RV(float value)
    {
        return (int)value;
    }

    private static int PSC(float value)
    {
        float num = (float)PlayerManager.Instance.Players.Count;
        return (int)(100f * value * num / Datameter.NetworkSizeCounter);
    }
}


