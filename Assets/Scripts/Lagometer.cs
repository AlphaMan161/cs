// ILSpyBased#2
using System;
using System.Collections.Generic;

public static class Lagometer
{
    private const int LAG_THRESHOLD = 400;

    private const int LAG_PERIOD = 50000000;

    private const int LAG_MIN_REPORTS = 12;

    private const int LAG_MIN_PING = 150;

    private static int lagCounter = 0;

    private static int lagReports = 0;

    private static int lagValue = -2;

    public static string LagLine = string.Empty;

    private static Queue<int> lagList = new Queue<int>();

    private static long lastLagReset = DateTime.Now.Ticks;

    public static bool CountLags = false;

    public static int LagValue
    {
        get
        {
            return Lagometer.lagValue;
        }
    }

    public static void Restart()
    {
        Lagometer.Reset();
        Lagometer.lagList.Clear();
        Lagometer.LagLine = string.Empty;
        Lagometer.lagValue = -2;
    }

    public static void Reset()
    {
        Lagometer.lastLagReset = DateTime.Now.Ticks;
        Lagometer.lagCounter = 0;
        Lagometer.lagReports = 0;
    }

    public static void Report(long delta, int ping)
    {
        if (DateTime.Now.Ticks - Lagometer.lastLagReset > 50000000)
        {
            if (Lagometer.lagReports > 12 && Lagometer.lagValue >= -1)
            {
                Lagometer.lagValue = Lagometer.lagCounter * 1000 * 100 / (Lagometer.lagReports * NetworkDev.TPS);
                Lagometer.lagList.Enqueue(Lagometer.lagValue);
                if (Lagometer.lagList.Count > 20)
                {
                    Lagometer.lagList.Dequeue();
                }
                Lagometer.LagLine = string.Empty;
                Queue<int>.Enumerator enumerator = Lagometer.lagList.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        int current = enumerator.Current;
                        Lagometer.LagLine = Lagometer.LagLine + " " + current;
                    }
                }
                finally
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
            else
            {
                Lagometer.lagValue = -1;
            }
            Lagometer.Reset();
        }
        if (ping < 150 && delta < 50000000)
        {
            if (delta > 400)
            {
                Lagometer.lagCounter++;
            }
            Lagometer.lagReports++;
        }
    }
}


