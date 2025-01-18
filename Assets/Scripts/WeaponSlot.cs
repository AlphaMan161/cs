// ILSpyBased#2
using System;
using System.Collections.Generic;

public class WeaponSlot
{
    public delegate void PlayerWeaponSlotEventHandler(object sender, bool changeIds);

    protected int w_id1;

    protected Weapon weapon1;

    protected int w_id2;

    protected Weapon weapon2;

    protected int w_id3;

    protected Weapon weapon3;

    protected int w_id4;

    protected Weapon weapon4;

    protected int w_id5;

    protected Weapon weapon5;

    protected int w_id6;

    protected Weapon weapon6;

    protected int w_id7;

    protected Weapon weapon7;

    public int WeaponID1
    {
        get
        {
            return this.w_id1;
        }
    }

    public Weapon Weapon1
    {
        get
        {
            return this.weapon1;
        }
    }

    public int WeaponID2
    {
        get
        {
            return this.w_id2;
        }
    }

    public Weapon Weapon2
    {
        get
        {
            return this.weapon2;
        }
    }

    public int WeaponID3
    {
        get
        {
            return this.w_id3;
        }
    }

    public Weapon Weapon3
    {
        get
        {
            return this.weapon3;
        }
    }

    public int WeaponID4
    {
        get
        {
            return this.w_id4;
        }
    }

    public Weapon Weapon4
    {
        get
        {
            return this.weapon4;
        }
    }

    public int WeaponID5
    {
        get
        {
            return this.w_id5;
        }
    }

    public Weapon Weapon5
    {
        get
        {
            return this.weapon5;
        }
    }

    public int WeaponID6
    {
        get
        {
            return this.w_id6;
        }
    }

    public Weapon Weapon6
    {
        get
        {
            return this.weapon6;
        }
    }

    public int WeaponID7
    {
        get
        {
            return this.w_id7;
        }
    }

    public Weapon Weapon7
    {
        get
        {
            return this.weapon7;
        }
    }

    public WeaponSlot(Weapon[] weapons)
    {
        foreach (Weapon weapon in weapons)
        {
            switch (weapon.WeaponSlot)
            {
                case 1:
                    this.weapon1 = weapon;
                    this.w_id1 = weapon.WeaponID;
                    break;
                case 2:
                    this.weapon2 = weapon;
                    this.w_id2 = weapon.WeaponID;
                    break;
                case 3:
                    this.weapon3 = weapon;
                    this.w_id3 = weapon.WeaponID;
                    break;
                case 4:
                    this.weapon4 = weapon;
                    this.w_id4 = weapon.WeaponID;
                    break;
                case 5:
                    this.weapon5 = weapon;
                    this.w_id5 = weapon.WeaponID;
                    break;
                case 6:
                    this.weapon6 = weapon;
                    this.w_id6 = weapon.WeaponID;
                    break;
                case 7:
                    this.weapon7 = weapon;
                    this.w_id7 = weapon.WeaponID;
                    break;
            }
        }
        this.SetDefaultWeapons();
    }

    public WeaponSlot(JSONObject json)
    {
        if (Inventory.Instance.Initialized)
        {
            this.OnLoadUserInventory(Inventory.Instance, EventArgs.Empty);
        }
        Inventory.OnLoad += new Inventory.InventoryEventHandler(this.OnLoadUserInventory);
        this.w_id1 = Convert.ToInt32(json.GetField("id1").n);
        this.w_id2 = Convert.ToInt32(json.GetField("id2").n);
        this.w_id3 = Convert.ToInt32(json.GetField("id3").n);
        this.w_id4 = Convert.ToInt32(json.GetField("id4").n);
        this.w_id5 = Convert.ToInt32(json.GetField("id5").n);
        this.w_id6 = Convert.ToInt32(json.GetField("id6").n);
        this.w_id7 = Convert.ToInt32(json.GetField("id7").n);
    }

    private void OnLoadUserInventory(object sender, EventArgs arg)
    {
        List<Weapon>.Enumerator enumerator = Inventory.Instance.Weapons.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Weapon current = enumerator.Current;
                if (this.w_id1 != 0 && current.WeaponID == this.w_id1)
                {
                    this.weapon1 = current;
                }
                else if (this.w_id2 != 0 && current.WeaponID == this.w_id2)
                {
                    this.weapon2 = current;
                }
                else if (this.w_id3 != 0 && current.WeaponID == this.w_id3)
                {
                    this.weapon3 = current;
                }
                else if (this.w_id4 != 0 && current.WeaponID == this.w_id4)
                {
                    this.weapon4 = current;
                }
                else if (this.w_id5 != 0 && current.WeaponID == this.w_id5)
                {
                    this.weapon5 = current;
                }
                else if (this.w_id6 != 0 && current.WeaponID == this.w_id6)
                {
                    this.weapon6 = current;
                }
                else if (this.w_id7 != 0 && current.WeaponID == this.w_id7)
                {
                    this.weapon7 = current;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        this.SetDefaultWeapons();
    }

    protected void SetDefaultWeapons()
    {
        if (this.w_id1 == 0)
        {
            this.weapon1 = Inventory.Instance.WeaponDefault[1];
        }
        if (this.w_id2 == 0)
        {
            this.weapon2 = Inventory.Instance.WeaponDefault[2];
        }
        if (this.w_id3 == 0)
        {
            this.weapon3 = Inventory.Instance.WeaponDefault[3];
        }
        if (this.w_id4 == 0)
        {
            this.weapon4 = Inventory.Instance.WeaponDefault[4];
        }
        if (this.w_id5 == 0)
        {
            this.weapon5 = Inventory.Instance.WeaponDefault[5];
        }
        if (this.w_id6 == 0)
        {
            this.weapon6 = Inventory.Instance.WeaponDefault[6];
        }
        if (this.w_id7 == 0)
        {
            this.weapon7 = Inventory.Instance.WeaponDefault[7];
        }
    }

    public bool IsEquip(Weapon weapon)
    {
        if (weapon.WeaponID != this.w_id1 && weapon.WeaponID != this.w_id2 && weapon.WeaponID != this.w_id3 && weapon.WeaponID != this.w_id4 && weapon.WeaponID != this.w_id5 && weapon.WeaponID != this.w_id6 && weapon.WeaponID != this.w_id7)
        {
            return false;
        }
        return true;
    }

    public override string ToString()
    {
        return string.Format("[WeaponSlot: WeaponID1={0}, Weapon1={1}, WeaponID2={2}, Weapon2={3}, WeaponID3={4}, Weapon3={5}, WeaponID4={6}, Weapon4={7}, WeaponID5={8}, Weapon5={9}, WeaponID6={10}, Weapon6={11}, WeaponID7={12}, Weapon7={13}]", this.WeaponID1, this.Weapon1, this.WeaponID2, this.Weapon2, this.WeaponID3, this.Weapon3, this.WeaponID4, this.Weapon4, this.WeaponID5, this.Weapon5, this.WeaponID6, this.Weapon6, this.WeaponID7, this.Weapon7);
    }
}


