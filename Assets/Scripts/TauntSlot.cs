// ILSpyBased#2
public class TauntSlot
{
    public delegate void TauntSlotEventHandler(object sender, int slot);

    protected Taunt taunt0;

    protected Taunt taunt1;

    protected Taunt taunt2;

    public Taunt Taunt0
    {
        get
        {
            return this.taunt0;
        }
        set
        {
            this.taunt0 = value;
        }
    }

    public Taunt Taunt1
    {
        get
        {
            return this.taunt1;
        }
        set
        {
            this.taunt1 = value;
        }
    }

    public Taunt Taunt2
    {
        get
        {
            return this.taunt2;
        }
        set
        {
            this.taunt2 = value;
        }
    }

    public event TauntSlotEventHandler OnSet;

    public event TauntSlotEventHandler OnUnSet;

    public event TauntSlotEventHandler OnPlay;

    public virtual void Set(Taunt taunt, int slot)
    {
        if (this.IsSet(taunt))
        {
            this.UnSet(taunt);
        }
        switch (slot)
        {
            case 0:
                this.taunt0 = taunt;
                break;
            case 1:
                this.taunt1 = taunt;
                break;
            default:
                this.taunt2 = taunt;
                break;
        }
        object[] data = new object[2] {
            taunt.TauntID,
            slot
        };
        GameLogicServerNetworkController.SendChange(5, data);
        if (this.OnSet != null)
        {
            this.OnSet(taunt, slot);
        }
    }

    public virtual void Play(Taunt taunt)
    {
        if (this.OnPlay != null)
        {
            this.OnPlay(taunt, 0);
        }
    }

    public virtual void UnSet(Taunt taunt)
    {
        int num = 0;
        if (this.taunt0 != null && this.taunt0.Equals(taunt))
        {
            this.taunt0 = null;
            num = 0;
        }
        if (this.taunt1 != null && this.taunt1.Equals(taunt))
        {
            this.taunt1 = null;
            num = 1;
        }
        if (this.taunt2 != null && this.taunt2.Equals(taunt))
        {
            this.taunt2 = null;
            num = 2;
        }
        object[] data = new object[2] {
            null,
            num
        };
        GameLogicServerNetworkController.SendChange(5, data);
        if (this.OnUnSet != null)
        {
            this.OnUnSet(taunt, num);
        }
    }

    public virtual bool IsSet(Taunt taunt)
    {
        if (taunt == null)
        {
            return false;
        }
        if (!taunt.Equals(this.taunt0) && !taunt.Equals(this.taunt1) && !taunt.Equals(this.taunt2))
        {
            return false;
        }
        return true;
    }
}


