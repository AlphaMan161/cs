// ILSpyBased#2
using UnityEngine;

public static class TRInputHelper
{
    public static void ResetDefault()
    {
        TRInput.Forward = KeyCode.W;
        TRInput.Backward = KeyCode.S;
        TRInput.LeftStrafe = KeyCode.A;
        TRInput.RightStrafe = KeyCode.D;
        TRInput.Jump = KeyCode.Space;
        TRInput.Crouch = KeyCode.LeftControl;
        TRInput.Weapon1 = KeyCode.Alpha1;
        TRInput.Weapon2 = KeyCode.Alpha2;
        TRInput.Weapon3 = KeyCode.Alpha3;
        TRInput.Weapon4 = KeyCode.Alpha4;
        TRInput.Weapon5 = KeyCode.Alpha5;
        TRInput.Weapon6 = KeyCode.Alpha6;
        TRInput.Weapon7 = KeyCode.Alpha7;
        TRInput.EnterChat = KeyCode.Return;
        TRInput.CallTechnic = KeyCode.F;
        TRInput.Fire1 = KeyCode.Mouse0;
        TRInput.QuickChange = KeyCode.Q;
        TRInput.Taunt1 = KeyCode.E;
        TRInput.Taunt2 = KeyCode.T;
        TRInput.Taunt3 = KeyCode.Y;
        TRInput.Enhancer1 = KeyCode.Alpha8;
        TRInput.Enhancer2 = KeyCode.Alpha9;
        TRInput.Zoom = KeyCode.Mouse1;
        TRInput.Reload = KeyCode.R;
        TRInput.ScreenShot = KeyCode.P;
        TRInput.RollIn = KeyCode.PageUp;
        TRInput.RollOut = KeyCode.PageDown;
    }

    public static bool SetButton(TRInput.TRKeyCodeDefault button, KeyCode keyCode)
    {
        switch (button)
        {
            case TRInput.TRKeyCodeDefault.Forward:
                TRInput.Forward = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Backward:
                TRInput.Backward = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.LeftStrafe:
                TRInput.LeftStrafe = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.RightStrafe:
                TRInput.RightStrafe = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Jump:
                TRInput.Jump = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Crouch:
                TRInput.Crouch = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Weapon1:
                TRInput.Weapon1 = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Weapon2:
                TRInput.Weapon2 = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Weapon3:
                TRInput.Weapon3 = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Weapon4:
                TRInput.Weapon4 = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Weapon5:
                TRInput.Weapon5 = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Weapon6:
                TRInput.Weapon6 = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Weapon7:
                TRInput.Weapon7 = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.EnterChat:
                TRInput.EnterChat = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.CallTechnic:
                TRInput.CallTechnic = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Fire1:
                TRInput.Fire1 = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.QuickChange:
                TRInput.QuickChange = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Taunt1:
                TRInput.Taunt1 = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Taunt2:
                TRInput.Taunt2 = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Taunt3:
                TRInput.Taunt3 = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.ScreenShot:
                TRInput.ScreenShot = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Enhancer1:
                TRInput.Enhancer1 = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Enhancer2:
                TRInput.Enhancer2 = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Zoom:
                TRInput.Zoom = keyCode;
                break;
            case TRInput.TRKeyCodeDefault.Reload:
                TRInput.Reload = keyCode;
                break;
        }
        return true;
    }

    public static string ToDisplayString(this TRInput.TRKeyCodeDefault e)
    {
        return EnumDisplayStringAttribute.ToDisplayString(e);
    }

    public static string ToDisplayString(this KeyCode e)
    {
        switch (e)
        {
            case KeyCode.Mouse0:
                return LanguageManager.GetText("Left Mouse");
            case KeyCode.Mouse1:
                return LanguageManager.GetText("Right Mouse");
            case KeyCode.Mouse2:
                return LanguageManager.GetText("Middle Mouse");
            case KeyCode.Alpha1:
                return "1";
            case KeyCode.Alpha2:
                return "2";
            case KeyCode.Alpha3:
                return "3";
            case KeyCode.Alpha4:
                return "4";
            case KeyCode.Alpha5:
                return "5";
            case KeyCode.Alpha6:
                return "6";
            case KeyCode.Alpha7:
                return "7";
            case KeyCode.Alpha8:
                return "8";
            case KeyCode.Alpha9:
                return "9";
            case KeyCode.Alpha0:
                return "0";
            default:
                return e.ToString();
        }
    }
}


