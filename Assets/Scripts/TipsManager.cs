// ILSpyBased#2
using System.Collections.Generic;
using UnityEngine;

public class TipsManager
{
    private static TipsManager hInstance;

    private List<string> tipsList;

    private List<object> tipsParams;

    private float TipsRotate = 10f;

    private float NextChange;

    private int currentTip;

    private static TipsManager Instance
    {
        get
        {
            if (TipsManager.hInstance == null)
            {
                TipsManager.hInstance = new TipsManager();
            }
            return TipsManager.hInstance;
        }
    }

    public static string GetTip()
    {
        if (Time.time > TipsManager.Instance.NextChange)
        {
            TipsManager.Instance.NextChange = Time.time + TipsManager.Instance.TipsRotate;
            TipsManager.Instance.currentTip = Random.Range(0, 17);
        }
        return TipsManager.Instance.TipByNumber(TipsManager.Instance.currentTip);
    }

    private string TipByNumber(int num)
    {
        switch (num)
        {
            case 0:
                return LanguageManager.GetText("To enter the full screen mode please press \"Alt + F\"");
            case 1:
                return LanguageManager.GetText("During the game you can change the weapon with the mouse scroll");
            case 2:
                return LanguageManager.GetTextFormat("To make a screenshot press \"{0}\"", TRInput.ScreenShot);
            case 3:
                return LanguageManager.GetText("Press \"TAB\" to check the statistics of the round");
            case 4:
                return LanguageManager.GetText("You will get the money for completing the achievements!");
            case 5:
                return LanguageManager.GetText("You can always buy stronger weapon in the SHOP");
            case 6:
                return LanguageManager.GetText("Invite your friends and get money for that!");
            case 7:
                return LanguageManager.GetText("You can get unique abilities in Training hall of your Headquaters!");
            case 8:
                return LanguageManager.GetText("Sniper rifles deal as much damage as your target is far away");
            case 9:
                return LanguageManager.GetText("Ammunition ended and there is no time for reload? Change the weapon!");
            case 10:
                return LanguageManager.GetText("Shooting from shotgun gives you ability to jump thanks to its reciol");
            case 11:
                return LanguageManager.GetText("Remember - machine gun needs some time to spin before shooting!");
            case 12:
                return LanguageManager.GetText("Improve yourself in the Training hall of your Headquaters!");
            case 13:
                return LanguageManager.GetTextFormat("Press \"{0}\" to salute your killed enemy and other players", TRInput.Taunt1);
            case 14:
                return LanguageManager.GetText("If there is some lags during the game try to descrease the video settings");
            case 15:
                return LanguageManager.GetText("Complete the achievements and get the money for that!");
            case 16:
                return LanguageManager.GetText("NEVER give the links of your game pages to ANYONE. Also don't give the links which can be copied in the game by pressing the right button of the mouse!");
            default:
                return string.Empty;
        }
    }
}


