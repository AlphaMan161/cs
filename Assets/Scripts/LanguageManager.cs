// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    private static LanguageManager instance;

    private static Hashtable textTable;

    private static List<string> notExistKeys = new List<string>();

    public static LanguageManager Instance
    {
        get
        {
            if ((UnityEngine.Object)LanguageManager.instance == (UnityEngine.Object)null)
            {
                GameObject gameObject = new GameObject("Language Manager");
                LanguageManager.instance = (LanguageManager)gameObject.AddComponent(typeof(LanguageManager));
            }
            return LanguageManager.instance;
        }
    }

    private LanguageManager()
    {
    }

    public static LanguageManager GetInstance()
    {
        return LanguageManager.Instance;
    }

    public static void ChangeLang(string lang, bool changeExternal)
    {
        if (lang == "ru")
        {
            LanguageManager.LoadLanguage("default");
        }
        else if (lang == "de")
        {
            LanguageManager.LoadLanguage("default_de");
        }
        else if (lang == "es")
        {
            LanguageManager.LoadLanguage("default_es");
        }
        else if (lang == "it")
        {
            LanguageManager.LoadLanguage("default_it");
        }
        else
        {
            LanguageManager.LoadLanguage("default_eng");
        }
        if (changeExternal)
        {
            WebCall.ChangeLang(lang);
        }
    }

    public static bool LoadLanguage(string filename)
    {
        if (filename == null)
        {
            UnityEngine.Debug.Log("[LanguageManager] loading default language.");
            LanguageManager.textTable = null;
            return false;
        }
        string text = "Languages/" + filename;
        TextAsset textAsset = (TextAsset)Resources.Load(text, typeof(TextAsset));
        if ((UnityEngine.Object)textAsset == (UnityEngine.Object)null)
        {
            UnityEngine.Debug.LogError("[LanguageManager] " + text + " file not found.");
            return false;
        }
        UnityEngine.Debug.Log("[LanguageManager] loading: " + text);
        if (LanguageManager.textTable == null)
        {
            LanguageManager.textTable = new Hashtable();
        }
        LanguageManager.textTable.Clear();
        StringReader stringReader = new StringReader(textAsset.text);
        string text2 = null;
        string text3 = null;
        string text4;
        while ((text4 = stringReader.ReadLine()) != null)
        {
            if (text4.StartsWith("msgid \""))
            {
                text2 = text4.Substring(7, text4.Length - 8);
            }
            else if (text4.StartsWith("msgstr \""))
            {
                text3 = text4.Substring(8, text4.Length - 9);
                text3 = text3.Replace("\\n", '\n'.ToString());
            }
            else if (text2 != null && text3 != null)
            {
                if (LanguageManager.textTable.ContainsKey(text2))
                {
                    UnityEngine.Debug.LogError("[Language manager] Key exists: " + text2);
                }
                LanguageManager.textTable.Add(text2, text3);
                text3 = null; text2 = (text3 );
            }
        }
        stringReader.Close();
        ActionRotater.HandleOnChangeLang();
        return true;
    }

    public static string GetText(string key)
    {
        if (key != null && LanguageManager.textTable != null)
        {
            if (LanguageManager.textTable.ContainsKey(key))
            {
                string text = (string)LanguageManager.textTable[key];
                key = text;
            }
            else if (!LanguageManager.notExistKeys.Contains(key))
            {
                LanguageManager.notExistKeys.Add(key);
            }
        }
        else if (!LanguageManager.notExistKeys.Contains(key))
        {
            LanguageManager.notExistKeys.Add(key);
        }
        if (key.Length > 60)
        {
            return key.Replace("\\n", '\n'.ToString());
        }
        return key;
    }

    public static string GetTextFormat(string key, params object[] args)
    {
        return string.Format(LanguageManager.GetText(key), args);
    }

    public static void ShowNotExistsKeys()
    {
        try
        {
            UnityEngine.Debug.Log("File: " + Application.dataPath + "notExistKeys.txt");
            StreamWriter streamWriter = new StreamWriter(Application.dataPath + "notExistKeys.txt");
            UnityEngine.Debug.Log("LanguageManager not exists keys:");
            List<string>.Enumerator enumerator = LanguageManager.notExistKeys.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    streamWriter.WriteLine(current);
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            streamWriter.Close();
            UnityEngine.Debug.Log("LanguageManager not exists keys end");
        }
        catch (Exception message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}


