// ILSpyBased#2
public static class ClanHelper
{
    public static string GetDesc(this ClanTreasuryEventType t)
    {
        switch (t)
        {
            case ClanTreasuryEventType.Add:
                return LanguageManager.GetText("(treasury refill)");
            case ClanTreasuryEventType.GetExpandClanMember:
                return LanguageManager.GetText("(clan quantity refill)");
            case ClanTreasuryEventType.GetChangeName:
                return LanguageManager.GetText("(name changed)");
            case ClanTreasuryEventType.GetChangeTag:
                return LanguageManager.GetText("(tag changed)");
            case ClanTreasuryEventType.GetChangeArm:
                return LanguageManager.GetText("(arms changed)");
            case ClanTreasuryEventType.BuyEnhancer:
                return LanguageManager.GetText("(buy enhancer)");
            default:
                return string.Empty;
        }
    }
}


