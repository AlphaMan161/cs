// ILSpyBased#2
using UnityEngine;

public class TRInput
{
    public enum TRKeyCodeDefault
    {
        [EnumDisplayString("Forward")]
        Forward = 119,
        Backward = 115,
        LeftStrafe = 97,
        RightStrafe = 100,
        Jump = 0x20,
        Crouch = 306,
        RollIn = 280,
        RollOut,
        [EnumDisplayString("Weapon 1")]
        Weapon1 = 49,
        [EnumDisplayString("Weapon 2")]
        Weapon2,
        [EnumDisplayString("Weapon 3")]
        Weapon3,
        [EnumDisplayString("Weapon 4")]
        Weapon4,
        [EnumDisplayString("Weapon 5")]
        Weapon5,
        [EnumDisplayString("Weapon 6")]
        Weapon6,
        [EnumDisplayString("Weapon 7")]
        Weapon7,
        [EnumDisplayString("Enter Chat")]
        EnterChat = 13,
        [EnumDisplayString("Call Technic")]
        CallTechnic = 102,
        [EnumDisplayString("Booster 1")]
        Enhancer1 = 56,
        [EnumDisplayString("Booster 2")]
        Enhancer2,
        [EnumDisplayString("Prim. Fire")]
        Fire1 = 323,
        [EnumDisplayString("Prev. weapon")]
        QuickChange = 113,
        [EnumDisplayString("Screenshot")]
        ScreenShot = 112,
        [EnumDisplayString("Zoom")]
        Zoom = 324,
        [EnumDisplayString("Reload")]
        Reload = 114,
        [EnumDisplayString("Taunt 1")]
        Taunt1 = 101,
        [EnumDisplayString("Taunt 2")]
        Taunt2 = 116,
        [EnumDisplayString("Taunt 3")]
        Taunt3 = 121
    }

    public static KeyCode Forward = KeyCode.W;

    public static KeyCode Backward = KeyCode.S;

    public static KeyCode LeftStrafe = KeyCode.A;

    public static KeyCode RightStrafe = KeyCode.D;

    public static KeyCode Jump = KeyCode.Space;

    public static KeyCode Crouch = KeyCode.LeftControl;

    public static KeyCode Weapon1 = KeyCode.Alpha1;

    public static KeyCode Weapon2 = KeyCode.Alpha2;

    public static KeyCode Weapon3 = KeyCode.Alpha3;

    public static KeyCode Weapon4 = KeyCode.Alpha4;

    public static KeyCode Weapon5 = KeyCode.Alpha5;

    public static KeyCode Weapon6 = KeyCode.Alpha6;

    public static KeyCode Weapon7 = KeyCode.Alpha7;

    public static KeyCode EnterChat = KeyCode.Return;

    public static KeyCode CallTechnic = KeyCode.F;

    public static KeyCode Fire1 = KeyCode.Mouse0;

    public static KeyCode QuickChange = KeyCode.Q;

    public static KeyCode Taunt1 = KeyCode.E;

    public static KeyCode Taunt2 = KeyCode.T;

    public static KeyCode Taunt3 = KeyCode.Y;

    public static KeyCode ScreenShot = KeyCode.P;

    public static KeyCode Enhancer1 = KeyCode.Alpha8;

    public static KeyCode Enhancer2 = KeyCode.Alpha9;

    public static KeyCode Zoom = KeyCode.Mouse1;

    public static KeyCode Reload = KeyCode.R;

    public static KeyCode RollIn = KeyCode.PageUp;

    public static KeyCode RollOut = KeyCode.PageDown;
}


