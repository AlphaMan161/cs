// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONObject : Nullable
{
    public enum Type
    {
        NULL,
        STRING,
        NUMBER,
        OBJECT,
        ARRAY,
        BOOL
    }

    private const int MAX_DEPTH = 1000;

    private const string INFINITY = "\"INFINITY\"";

    private const string NEGINFINITY = "\"NEGINFINITY\"";

    public JSONObject parent;

    public Type type;

    public ArrayList list = new ArrayList();

    public ArrayList keys = new ArrayList();

    public string str;

    public double n;

    public bool b;

    public string input_str = string.Empty;

    public bool isContainer
    {
        get
        {
            return this.type == Type.ARRAY || this.type == Type.OBJECT;
        }
    }

    public int Count
    {
        get
        {
            return this.list.Count;
        }
    }

    public static JSONObject nullJO
    {
        get
        {
            return new JSONObject(Type.NULL);
        }
    }

    public static JSONObject obj
    {
        get
        {
            return new JSONObject(Type.OBJECT);
        }
    }

    public static JSONObject arr
    {
        get
        {
            return new JSONObject(Type.ARRAY);
        }
    }

    public JSONObject this[int index]
    {
        get
        {
            if (this.list.Count > index)
            {
                return (JSONObject)this.list[index];
            }
            return null;
        }
        set
        {
            if (this.list.Count > index)
            {
                this.list[index] = value;
            }
        }
    }

    public JSONObject this[string index]
    {
        get
        {
            return this.GetField(index);
        }
        set
        {
            this.SetField(index, value);
        }
    }

    public JSONObject(Type t)
    {
        this.type = t;
        switch (t)
        {
            case Type.ARRAY:
                this.list = new ArrayList();
                break;
            case Type.OBJECT:
                this.list = new ArrayList();
                this.keys = new ArrayList();
                break;
        }
    }

    public JSONObject(bool b)
    {
        this.type = Type.BOOL;
        this.b = b;
    }

    public JSONObject(float f)
    {
        this.type = Type.NUMBER;
        this.n = (double)f;
    }

    public JSONObject(Dictionary<string, string> dic)
    {
        this.type = Type.OBJECT;
        Dictionary<string, string>.Enumerator enumerator = dic.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, string> current = enumerator.Current;
                this.keys.Add(current.Key);
                this.list.Add(new JSONObject {
                    type = Type.STRING,
                    str = current.Value
                });
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public JSONObject()
    {
    }

    public JSONObject(string str)
    {
        if (str != null)
        {
            this.input_str = str;
            str = str.Replace("\\n", "\n");
            str = str.Replace("\\", string.Empty);
            if (str.Length > 0)
            {
                if (string.Compare(str, "true", true) == 0)
                {
                    this.type = Type.BOOL;
                    this.b = true;
                }
                else if (string.Compare(str, "false", true) == 0)
                {
                    this.type = Type.BOOL;
                    this.b = false;
                }
                else if (str == "null")
                {
                    this.type = Type.NULL;
                }
                else if (str == "\"INFINITY\"")
                {
                    this.type = Type.NUMBER;
                    this.n = double.PositiveInfinity;
                }
                else if (str == "\"NEGINFINITY\"")
                {
                    this.type = Type.NUMBER;
                    this.n = double.NegativeInfinity;
                }
                else if (str[0] == '"')
                {
                    this.type = Type.STRING;
                    this.str = str.Substring(1, str.Length - 2);
                }
                else
                {
                    try
                    {
                        this.n = Convert.ToDouble(str);
                        this.type = Type.NUMBER;
                    }
                    catch (FormatException)
                    {
                        int num = 0;
                        switch (str[0])
                        {
                            case '{':
                                this.type = Type.OBJECT;
                                this.keys = new ArrayList();
                                this.list = new ArrayList();
                                break;
                            case '[':
                                this.type = Type.ARRAY;
                                this.list = new ArrayList();
                                break;
                            default:
                                this.type = Type.NULL;
                                UnityEngine.Debug.LogWarning("improper JSON formatting:" + str);
                                return;
                        }
                        int num2 = 0;
                        bool flag = false;
                        bool flag2 = false;
                        for (int i = 1; i < str.Length; i++)
                        {
                            if (str[i] == '\\' || str[i] == '\t' || str[i] == '\n' || str[i] == '\r')
                            {
                                i++;
                            }
                            else
                            {
                                if (str[i] == '"')
                                {
                                    flag = !flag;
                                }
                                else if (str[i] == '[' || str[i] == '{')
                                {
                                    num2++;
                                }
                                if (num2 == 0 && !flag)
                                {
                                    if (str[i] == ':' && !flag2)
                                    {
                                        flag2 = true;
                                        try
                                        {
                                            this.keys.Add(str.Substring(num + 2, i - num - 3));
                                        }
                                        catch
                                        {
                                            UnityEngine.Debug.Log(i + " - " + str.Length + " - " + str);
                                        }
                                        num = i;
                                    }
                                    if (str[i] == ',')
                                    {
                                        flag2 = false;
                                        this.list.Add(new JSONObject(str.Substring(num + 1, i - num - 1)));
                                        num = i;
                                    }
                                    if (str[i] == ']' || str[i] == '}')
                                    {
                                        this.list.Add(new JSONObject(str.Substring(num + 1, i - num - 1)));
                                    }
                                }
                                if (str[i] == ']' || str[i] == '}')
                                {
                                    num2--;
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            this.type = Type.NULL;
        }
    }

    public void Absorb(JSONObject obj)
    {
        this.list.AddRange(obj.list);
        this.keys.AddRange(obj.keys);
        this.str = obj.str;
        this.n = obj.n;
        this.b = obj.b;
        this.type = obj.type;
    }

    public void Add(bool val)
    {
        this.Add(new JSONObject(val));
    }

    public void Add(float val)
    {
        this.Add(new JSONObject(val));
    }

    public void Add(int val)
    {
        this.Add(new JSONObject((float)val));
    }

    public void Add(JSONObject obj)
    {
        if ((bool)obj)
        {
            if (this.type != Type.ARRAY)
            {
                this.type = Type.ARRAY;
                UnityEngine.Debug.LogWarning("tried to add an object to a non-array JSONObject.  We'll do it for you, but you might be doing something wrong.");
            }
            this.list.Add(obj);
        }
    }

    public void AddField(string name, bool val)
    {
        this.AddField(name, new JSONObject(val));
    }

    public void AddField(string name, float val)
    {
        this.AddField(name, new JSONObject(val));
    }

    public void AddField(string name, int val)
    {
        this.AddField(name, new JSONObject((float)val));
    }

    public void AddField(string name, string val)
    {
        this.AddField(name, new JSONObject {
            type = Type.STRING,
            str = val
        });
    }

    public void AddField(string name, JSONObject obj)
    {
        if ((bool)obj)
        {
            if (this.type != Type.OBJECT)
            {
                this.type = Type.OBJECT;
                UnityEngine.Debug.LogWarning("tried to add a field to a non-object JSONObject.  We'll do it for you, but you might be doing something wrong.");
            }
            this.keys.Add(name);
            this.list.Add(obj);
        }
    }

    public void SetField(string name, JSONObject obj)
    {
        if (this.HasField(name))
        {
            this.list.Remove(this[name]);
            this.keys.Remove(name);
        }
        this.AddField(name, obj);
    }

    public JSONObject GetField(string name)
    {
        if (this.type == Type.OBJECT)
        {
            for (int i = 0; i < this.keys.Count; i++)
            {
                if ((string)this.keys[i] == name)
                {
                    return (JSONObject)this.list[i];
                }
            }
        }
        return null;
    }

    public bool HasField(string name)
    {
        if (this.type == Type.OBJECT)
        {
            for (int i = 0; i < this.keys.Count; i++)
            {
                if ((string)this.keys[i] == name)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Clear()
    {
        this.type = Type.NULL;
        this.list.Clear();
        this.keys.Clear();
        this.str = string.Empty;
        this.n = 0.0;
        this.b = false;
    }

    public JSONObject Copy()
    {
        return new JSONObject(this.print());
    }

    public void Merge(JSONObject obj)
    {
        JSONObject.MergeRecur(this, obj);
    }

    private static void MergeRecur(JSONObject left, JSONObject right)
    {
        if (left.type == Type.NULL)
        {
            left.Absorb(right);
        }
        else if (left.type == Type.OBJECT && right.type == Type.OBJECT)
        {
            for (int i = 0; i < right.list.Count; i++)
            {
                string text = (string)right.keys[i];
                if (right[i].isContainer)
                {
                    if (left.HasField(text))
                    {
                        JSONObject.MergeRecur(left[text], right[i]);
                    }
                    else
                    {
                        left.AddField(text, right[i]);
                    }
                }
                else if (left.HasField(text))
                {
                    left.SetField(text, right[i]);
                }
                else
                {
                    left.AddField(text, right[i]);
                }
            }
        }
        else if (left.type == Type.ARRAY && right.type == Type.ARRAY)
        {
            if (right.Count > left.Count)
            {
                UnityEngine.Debug.LogError("Cannot merge arrays when right object has more elements");
            }
            else
            {
                for (int j = 0; j < right.list.Count; j++)
                {
                    if (left[j].type == right[j].type)
                    {
                        if (left[j].isContainer)
                        {
                            JSONObject.MergeRecur(left[j], right[j]);
                        }
                        else
                        {
                            left[j] = right[j];
                        }
                    }
                }
            }
        }
    }

    public string print()
    {
        return this.print(0);
    }

    public string print(int depth)
    {
        if (depth++ > 1000)
        {
            UnityEngine.Debug.Log("reached max depth!");
            return string.Empty;
        }
        string text = string.Empty;
        switch (this.type)
        {
            case Type.STRING:
                text = "\"" + this.str + "\"";
                break;
            case Type.NUMBER:
                text = ((this.n != double.PositiveInfinity) ? ((this.n != double.NegativeInfinity) ? (text + this.n) : "\"NEGINFINITY\"") : "\"INFINITY\"");
                break;
            case Type.OBJECT:
                if (this.list.Count > 0)
                {
                    text = "{";
                    text += "\n";
                    depth++;
                    for (int i = 0; i < this.list.Count; i++)
                    {
                        string str = (string)this.keys[i];
                        JSONObject jSONObject = (JSONObject)this.list[i];
                        if ((bool)jSONObject)
                        {
                            for (int j = 0; j < depth; j++)
                            {
                                text += "\t";
                            }
                            text = text + "\"" + str + "\":";
                            text = text + jSONObject.print(depth) + ",";
                            text += "\n";
                        }
                    }
                    text = text.Substring(0, text.Length - 1);
                    text = text.Substring(0, text.Length - 1);
                    text += "}";
                }
                else
                {
                    text = "null";
                }
                break;
            case Type.ARRAY:
                if (this.list.Count > 0)
                {
                    text = "[";
                    text += "\n";
                    depth++;
                    foreach (JSONObject item in this.list)
                    {
                        if ((bool)item)
                        {
                            for (int k = 0; k < depth; k++)
                            {
                                text += "\t";
                            }
                            text = text + item.print(depth) + ",";
                            text += "\n";
                        }
                    }
                    text = text.Substring(0, text.Length - 1);
                    text = text.Substring(0, text.Length - 1);
                    text += "]";
                }
                else
                {
                    text = "null";
                }
                break;
            case Type.BOOL:
                text = ((!this.b) ? "false" : "true");
                break;
            case Type.NULL:
                text = "null";
                break;
        }
        return text;
    }

    public override string ToString()
    {
        return this.print();
    }

    public Dictionary<string, string> ToDictionary()
    {
        if (this.type == Type.OBJECT)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < this.list.Count; i++)
            {
                JSONObject jSONObject = (JSONObject)this.list[i];
                switch (jSONObject.type)
                {
                    case Type.STRING:
                        dictionary.Add((string)this.keys[i], jSONObject.str);
                        break;
                    case Type.NUMBER:
                        dictionary.Add((string)this.keys[i], jSONObject.n + string.Empty);
                        break;
                    case Type.BOOL:
                        dictionary.Add((string)this.keys[i], jSONObject.b + string.Empty);
                        break;
                    default:
                        UnityEngine.Debug.LogWarning("Omitting object: " + (string)this.keys[i] + " in dictionary conversion");
                        break;
                }
            }
            return dictionary;
        }
        UnityEngine.Debug.LogWarning("Tried to turn non-Object JSONObject into a dictionary");
        return null;
    }
}


