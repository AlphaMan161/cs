// ILSpyBased#2
using SimpleJSON;

public class WeekItem
{
    private int id;

    private CCItemType type;

    private int item_id;

    private string desc = string.Empty;

    public int ID
    {
        get
        {
            return this.id;
        }
    }

    public CCItemType Type
    {
        get
        {
            return this.type;
        }
    }

    public int ItemID
    {
        get
        {
            return this.item_id;
        }
    }

    public string Description
    {
        get
        {
            return this.desc;
        }
    }

    public WeekItem(JSONNode obj)
    {
        this.id = obj["id"].AsInt;
        this.type = (CCItemType)obj["it"].AsInt;
        this.item_id = obj["ii"].AsInt;
        this.desc = obj["d"].Value;
    }

    public override string ToString()
    {
        return string.Format("[WeekItem: ID={0}, Type={1}, itemID={2}, Description={3}]", this.ID, this.Type, this.ItemID, this.Description);
    }
}


