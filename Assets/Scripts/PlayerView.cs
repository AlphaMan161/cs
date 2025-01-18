// ILSpyBased#2
public class PlayerView
{
    public delegate void PlayerViewEventHandler(object sender);

    protected Wear hat;

    protected Wear head;

    protected Wear mask;

    protected Wear gloves;

    protected Wear shirt;

    protected Wear pants;

    protected Wear boots;

    protected Wear backpack;

    protected Wear other;

    public Wear Hat
    {
        get
        {
            return this.hat;
        }
        set
        {
            this.hat = value;
        }
    }

    public Wear Head
    {
        get
        {
            return this.head;
        }
        set
        {
            this.head = value;
        }
    }

    public Wear Mask
    {
        get
        {
            return this.mask;
        }
        set
        {
            this.mask = value;
        }
    }

    public Wear Gloves
    {
        get
        {
            return this.gloves;
        }
        set
        {
            this.gloves = value;
        }
    }

    public Wear Shirt
    {
        get
        {
            return this.shirt;
        }
        set
        {
            this.shirt = value;
        }
    }

    public Wear Pants
    {
        get
        {
            return this.pants;
        }
        set
        {
            this.pants = value;
        }
    }

    public Wear Boots
    {
        get
        {
            return this.boots;
        }
        set
        {
            this.boots = value;
        }
    }

    public Wear Backpack
    {
        get
        {
            return this.backpack;
        }
        set
        {
            this.backpack = value;
        }
    }

    public Wear Other
    {
        get
        {
            return this.other;
        }
        set
        {
            this.other = value;
        }
    }

    public event PlayerViewEventHandler OnDreesUp;

    public event PlayerViewEventHandler OnUnDress;

    public virtual void DressUp(Wear dressed)
    {
        if (dressed.WearType == CCWearType.Hats)
        {
            this.Hat = dressed;
        }
        else if (dressed.WearType == CCWearType.Masks)
        {
            this.Mask = dressed;
        }
        else if (dressed.WearType == CCWearType.Gloves)
        {
            this.Gloves = dressed;
        }
        else if (dressed.WearType == CCWearType.Shirts)
        {
            this.Shirt = dressed;
        }
        else if (dressed.WearType == CCWearType.Pants)
        {
            this.Pants = dressed;
        }
        else if (dressed.WearType == CCWearType.Boots)
        {
            this.Boots = dressed;
        }
        else if (dressed.WearType == CCWearType.Backpacks)
        {
            this.Backpack = dressed;
        }
        else if (dressed.WearType == CCWearType.Heads)
        {
            this.Head = dressed;
        }
        else if (dressed.WearType == CCWearType.Others)
        {
            this.Other = dressed;
        }
        if (this.OnDreesUp != null)
        {
            this.OnDreesUp(dressed);
        }
    }

    public virtual void DressUpAll()
    {
        if (this.OnDreesUp != null && this.Hat != null)
        {
            this.OnDreesUp(this.Hat);
        }
        if (this.OnDreesUp != null && this.Mask != null)
        {
            this.OnDreesUp(this.Mask);
        }
        if (this.OnDreesUp != null && this.Gloves != null)
        {
            this.OnDreesUp(this.Gloves);
        }
        if (this.OnDreesUp != null && this.Shirt != null)
        {
            this.OnDreesUp(this.Shirt);
        }
        if (this.OnDreesUp != null && this.Pants != null)
        {
            this.OnDreesUp(this.Pants);
        }
        if (this.OnDreesUp != null && this.Boots != null)
        {
            this.OnDreesUp(this.Boots);
        }
        if (this.OnDreesUp != null && this.Backpack != null)
        {
            this.OnDreesUp(this.Backpack);
        }
        if (this.OnDreesUp != null && this.Head != null)
        {
            this.OnDreesUp(this.Head);
        }
        if (this.OnDreesUp != null && this.Other != null)
        {
            this.OnDreesUp(this.Other);
        }
    }

    public virtual void UnDress(Wear dressed)
    {
        if (dressed.WearType == CCWearType.Hats && dressed.Equals(this.Hat))
        {
            this.hat = null;
        }
        else if (dressed.WearType == CCWearType.Masks && dressed.Equals(this.Mask))
        {
            this.mask = null;
        }
        else if (dressed.WearType == CCWearType.Gloves && dressed.Equals(this.Gloves))
        {
            this.gloves = null;
        }
        else if (dressed.WearType == CCWearType.Shirts && dressed.Equals(this.Shirt))
        {
            this.shirt = null;
        }
        else if (dressed.WearType == CCWearType.Pants && dressed.Equals(this.Pants))
        {
            this.pants = null;
        }
        else if (dressed.WearType == CCWearType.Boots && dressed.Equals(this.Boots))
        {
            this.boots = null;
        }
        else if (dressed.WearType == CCWearType.Backpacks && dressed.Equals(this.Backpack))
        {
            this.backpack = null;
        }
        else if (dressed.WearType == CCWearType.Heads && dressed.Equals(this.Head))
        {
            this.head = null;
        }
        else if (dressed.WearType == CCWearType.Others && dressed.Equals(this.Other))
        {
            this.other = null;
        }
        if (this.OnUnDress != null)
        {
            this.OnUnDress(dressed);
        }
    }

    public virtual bool IsDressed(Wear wear)
    {
        if (wear == null)
        {
            return false;
        }
        if (this.hat != null && wear.Equals(this.hat))
        {
            return true;
        }
        if (this.mask != null && wear.Equals(this.mask))
        {
            return true;
        }
        if (this.gloves != null && wear.Equals(this.gloves))
        {
            return true;
        }
        if (this.shirt != null && wear.Equals(this.shirt))
        {
            return true;
        }
        if (this.gloves != null && wear.Equals(this.gloves))
        {
            return true;
        }
        if (this.shirt != null && wear.Equals(this.shirt))
        {
            return true;
        }
        if (this.pants != null && wear.Equals(this.pants))
        {
            return true;
        }
        if (this.boots != null && wear.Equals(this.boots))
        {
            return true;
        }
        if (this.backpack != null && wear.Equals(this.backpack))
        {
            return true;
        }
        if (this.head != null && wear.Equals(this.head))
        {
            return true;
        }
        if (this.other != null && wear.Equals(this.other))
        {
            return true;
        }
        return false;
    }
}


