// ILSpyBased#2
using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Field)]
public class EnumDisplayStringAttribute : Attribute
{
    public string StringValue
    {
        get;
        private set;
    }

    public EnumDisplayStringAttribute(string stringValue)
    {
        this.StringValue = stringValue;
    }

    public static string ToDisplayString(TRInput.TRKeyCodeDefault value)
    {
        Type type = value.GetType();
        FieldInfo field = type.GetField(value.ToString());
        EnumDisplayStringAttribute[] array = field.GetCustomAttributes(typeof(EnumDisplayStringAttribute), false) as EnumDisplayStringAttribute[];
        string key = value.ToString();
        if (array != null && array.Length > 0)
        {
            key = array[0].StringValue;
        }
        return LanguageManager.GetText(key);
    }
}


