// ILSpyBased#2
using UnityEngine;

public class GUITextShadow
{
    public static void TextShadow(Rect rect, string text, GUIStyle normal, GUIStyle shadow)
    {
        rect.x -= 1f;
        GUI.Box(rect, text, shadow);
        rect.x += 2f;
        GUI.Box(rect, text, shadow);
        rect.y -= 1f;
        rect.x -= 1f;
        GUI.Box(rect, text, shadow);
        rect.y += 2f;
        GUI.Box(rect, text, shadow);
        rect.y = rect.y + 1f - 2f;
        GUI.Box(rect, text, normal);
    }
}


