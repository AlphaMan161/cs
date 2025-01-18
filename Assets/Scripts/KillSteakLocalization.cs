// ILSpyBased#2
public static class KillSteakLocalization
{
    public static string GetName(this KillStreakType type)
    {
        switch (type)
        {
            case KillStreakType.None:
                return LanguageManager.GetText("MISS");
            case KillStreakType.FirstBlood:
                return LanguageManager.GetText("FIRST BLOOD");
            case KillStreakType.HeadShot:
                return LanguageManager.GetText("TO HEAD");
            case KillStreakType.NutShot:
                return LanguageManager.GetText("TO NUTS");
            case KillStreakType.Revenge:
                return LanguageManager.GetText("REVANGE");
            case KillStreakType.DoubleKill:
                return LanguageManager.GetText("DOUBLE KILL");
            case KillStreakType.TripleKill:
                return LanguageManager.GetText("TRIPLE KILL");
            case KillStreakType.KillingFour:
                return LanguageManager.GetText("QUAD KILL");
            case KillStreakType.Bloodthirsty5X:
                return LanguageManager.GetText("BUTCHER (5)");
            case KillStreakType.Ruthless10X:
                return LanguageManager.GetText("MERCILESS (10)");
            case KillStreakType.Brutal15X:
                return LanguageManager.GetText("BRUTAL (15)");
            case KillStreakType.Heartless20X:
                return LanguageManager.GetText("HEARTLESS (20)");
            case KillStreakType.Fierce25X:
                return LanguageManager.GetText("FIERCE (20)");
            case KillStreakType.Irrepressible25Plus:
                return LanguageManager.GetText("IRREPRESSIBLE (25+)");
            case KillStreakType.OmeletteMaster:
                return LanguageManager.GetText("OMELET MASTER");
            case KillStreakType.HeadSeries:
                return LanguageManager.GetText("HEAD SHOT STREAK");
            case KillStreakType.LeaderKiller:
                return LanguageManager.GetText("LEADER KILLER");
            default:
                return "unkown";
        }
    }
}


