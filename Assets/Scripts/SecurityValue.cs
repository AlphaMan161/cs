// ILSpyBased#2
public class SecurityValue
{
    private int code;

    private int mask;

    public SecurityValue(int value, int mask)
    {
        this.code = value << 2;
    }

    public bool Check(int value)
    {
        return value << 2 == this.code;
    }

    public int GetCode()
    {
        return this.code;
    }
}


