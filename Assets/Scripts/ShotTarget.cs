// ILSpyBased#2
using System;
using UnityEngine;

public class ShotTarget
{
    public int TargetID;

    public long ItemTimeStamp;

    public int HealthDamage;

    public int EnergyDamage;

    public Transform TargetTransform;

    public ShotTargetType TargetType;

    private DirectTarget direct;

    public PlayerHitZone HitZone;

    public bool Direct
    {
        get
        {
            return this.direct == DirectTarget.DIRECT;
        }
        set
        {
            this.direct = (DirectTarget)(value ? 8 : 0);
        }
    }

    public byte TargetTypeDescriptor
    {
        get
        {
            return Convert.ToByte((int)this.TargetType | (int)this.direct | (int)this.HitZone);
        }
        set
        {
            this.TargetType = (ShotTargetType)(value & 7);
            this.direct = (DirectTarget)(value & 0x10);
            this.HitZone = (PlayerHitZone)(value & 0x30);
        }
    }
}


