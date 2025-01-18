// ILSpyBased#2
using UnityEngine;

public class GUIProgressBar : MonoBehaviour
{
    public static void ProgressBar(float lineWidth, float maxValue, float curValue, GUISkin skin, string style)
    {
        GUISkin gUISkin = null;
        if ((Object)skin != (Object)null)
        {
            gUISkin = GUI.skin;
            GUI.skin = skin;
        }
        float num = (float)Mathf.FloorToInt(lineWidth * curValue / maxValue);
        float num2 = (float)(GUI.skin.GetStyle(style + "Line").border.left + GUI.skin.GetStyle(style + "Line").border.right);
        if (num < num2)
        {
            num = num2;
        }
        if (num > lineWidth - num2)
        {
            num = lineWidth - num2;
        }
        GUILayout.BeginHorizontal(GUIContent.none, style + "Background", GUILayout.Width(lineWidth));
        GUILayout.Label(GUIContent.none, style + "Line", GUILayout.Width(num));
        GUILayout.EndHorizontal();
        if ((Object)gUISkin != (Object)null)
        {
            GUI.skin = gUISkin;
        }
    }

    public static void ProgressBar(float lineWidth, float maxValue, float curValue)
    {
        GUIProgressBar.ProgressBar(lineWidth, maxValue, curValue, GUISkinManager.ProgressBar, "pb");
    }

    public static void ProgressBar(float lineWidth, float maxValue, float curValue, string style)
    {
        GUIProgressBar.ProgressBar(lineWidth, maxValue, curValue, GUISkinManager.ProgressBar, style);
    }
}


