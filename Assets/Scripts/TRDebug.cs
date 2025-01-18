// ILSpyBased#2
using UnityEngine;

public static class TRDebug
{
    public static void ShowDebug(this WWW www)
    {
        UnityEngine.Debug.Log(string.Format("[WWW DEBUG] URL: {0}\nERROR: {1}\nTEXT: {2}", www.url, www.error, www.text));
    }

    public static void ShowDebug(this WWW www, string name)
    {
        UnityEngine.Debug.Log(string.Format("[WWW DEBUG: {0}]\nURL: {1}\nERROR: {2}\nTEXT: {3}", name, www.url, www.error, www.text));
    }
}


