// ILSpyBased#2
public struct EnhancerValue
{
    public string Key;

    public int Value;

    public EnhancerValueType Type;

    public EnhancerValue(string in_key, int in_value, EnhancerValueType in_type)
    {
        this.Key = in_key;
        this.Value = in_value;
        this.Type = in_type;
    }
}


