// ILSpyBased#2
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class BadWorldFilter
{
    private static BadWorldFilter hInstance;

    private List<Regex> patterns = new List<Regex>();

    private List<Regex> patternsLite = new List<Regex>();

    private static BadWorldFilter Instance
    {
        get
        {
            if (BadWorldFilter.hInstance == null)
            {
                BadWorldFilter.hInstance = new BadWorldFilter();
            }
            return BadWorldFilter.hInstance;
        }
    }

    public List<Regex> Patterns
    {
        get
        {
            return this.patterns;
        }
        set
        {
            this.patterns = value;
        }
    }

    public List<Regex> PatternsLite
    {
        get
        {
            return this.patternsLite;
        }
        set
        {
            this.patternsLite = value;
        }
    }

    private BadWorldFilter()
    {
        this.Patterns = this.GetBadWords("Languages/antimat");
        this.PatternsLite = this.GetBadWords("Languages/antimatLite");
    }

    private List<Regex> GetBadWords(string fullpath)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(fullpath, typeof(TextAsset));
        if ((Object)textAsset == (Object)null)
        {
            UnityEngine.Debug.LogError("[BadWorldFilter] " + fullpath + " file not found.");
            return new List<Regex>();
        }
        StringReader stringReader = new StringReader(textAsset.text);
        List<string> list = new List<string>();
        string text;
        while ((text = stringReader.ReadLine()) != null)
        {
            if (text.Length > 2)
            {
                list.Add(text);
            }
        }
        string[] array = list.ToArray();
        List<Regex> list2 = new List<Regex>();
        for (int i = 0; i < array.Length; i++)
        {
            char[] array2 = array[i].ToCharArray();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            short num = 0;
            for (int j = 0; j < array2.Length; j++)
            {
                if (array2[j] == '[')
                {
                    stringBuilder.Append("(");
                    num = (short)(num + 1);
                }
                else if (array2[j] == ']')
                {
                    stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    stringBuilder.Append(")[\\W]*");
                    num = (short)(num - 1);
                }
                else if (num > 0)
                {
                    stringBuilder.AppendFormat("[{0}|{1}]|", array2[j].ToString().ToLower(), array2[j].ToString().ToUpper());
                }
                else
                {
                    stringBuilder.AppendFormat("[{0}|{1}][\\W]*", array2[j].ToString().ToLower(), array2[j].ToString().ToUpper());
                }
            }
            stringBuilder.Append(")");
            list2.Add(new Regex(stringBuilder.ToString()));
        }
        return list2;
    }

    public static string Check(string input)
    {
        if (!OptionsManager.EnableBadWorldFilter)
        {
            return input;
        }
        for (int i = 0; i < BadWorldFilter.Instance.patterns.Count; i++)
        {
            if (input != null)
            {
                input = BadWorldFilter.Instance.patterns[i].Replace(input, "***");
            }
        }
        return input;
    }

    public static string CheckLite(string input)
    {
        if (!OptionsManager.EnableBadWorldFilter)
        {
            return input;
        }
        for (int i = 0; i < BadWorldFilter.Instance.patternsLite.Count; i++)
        {
            if (input != null)
            {
                input = BadWorldFilter.Instance.patternsLite[i].Replace(input, "***");
            }
        }
        return input;
    }
}


