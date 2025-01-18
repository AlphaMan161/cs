// ILSpyBased#2
using System;

public static class KickManagerHelper
{
    public static KickReason FromByte(byte reason)
    {
        return (KickReason)(byte)Convert.ToInt32(reason);
    }

    public static string ToString(this KickReason reason)
    {
        switch (reason)
        {
            case KickReason.Cheating:
                return LanguageManager.GetText("Using of prohibited software");
            case KickReason.Threats:
                return LanguageManager.GetText("For rude behavior / threats");
            case KickReason.Other:
                return LanguageManager.GetText("Other");
            case KickReason.Instant:
                return LanguageManager.GetText("Instant");
            default:
                return string.Empty;
        }
    }
}


