// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot
{
    private Vector3 origin;

    private Vector3 direction;

    private ArrayList trajectory;

    private long timeStamp;

    private long landingTimeStamp;

    private Vector3 startOrigin;

    private Vector3 startDirection;

    private WeaponType weaponType;

    private LaunchModes launchMode;

    private List<ShotTarget> targets;

    private bool crit;

    private bool hasOrigin;

    private bool hasDirection;

    public bool HasOrigin
    {
        get
        {
            return this.hasOrigin;
        }
    }

    public bool HasDirection
    {
        get
        {
            return this.hasDirection;
        }
    }

    public bool Crit
    {
        get
        {
            return this.crit;
        }
        set
        {
            this.crit = value;
        }
    }

    public long TimeStamp
    {
        get
        {
            return this.timeStamp;
        }
        set
        {
            this.timeStamp = value;
        }
    }

    public long LandingTimeStamp
    {
        get
        {
            return this.landingTimeStamp;
        }
    }

    public Vector3 Origin
    {
        get
        {
            return this.origin;
        }
    }

    public Vector3 Direction
    {
        get
        {
            return this.direction;
        }
    }

    public Vector3 StartOrigin
    {
        get
        {
            return this.startOrigin;
        }
    }

    public Vector3 StartDirection
    {
        get
        {
            return this.startDirection;
        }
    }

    public ArrayList Trajectory
    {
        get
        {
            return this.trajectory;
        }
        set
        {
            this.trajectory = value;
        }
    }

    public WeaponType WeaponType
    {
        get
        {
            return this.weaponType;
        }
    }

    public LaunchModes LaunchMode
    {
        get
        {
            return this.launchMode;
        }
        set
        {
            this.launchMode = value;
        }
    }

    public List<ShotTarget> Targets
    {
        get
        {
            if (this.targets == null)
            {
                this.targets = new List<ShotTarget>();
            }
            return this.targets;
        }
    }

    public bool HasTargets
    {
        get
        {
            return this.targets != null;
        }
    }

    public Shot(Vector3 origin, Vector3 direction, byte weaponType)
    {
        this.origin = origin;
        this.direction = direction;
        this.landingTimeStamp = -1L;
        this.startOrigin = Vector3.zero;
        this.startDirection = Vector3.zero;
        this.weaponType = (WeaponType)weaponType;
        this.launchMode = LaunchModes.SHOT;
        this.targets = null;
    }

    public void ClearTargets()
    {
        if (this.targets != null)
        {
            this.targets.Clear();
        }
    }

    public void Launch(Vector3 startOrigin, Vector3 startDirection, long launchTimeStamp, long landingTimeStamp)
    {
        this.startOrigin = startOrigin;
        this.startDirection = startDirection;
        this.timeStamp = launchTimeStamp;
        this.landingTimeStamp = landingTimeStamp;
        this.launchMode = LaunchModes.LAUNCH;
    }

    public void addPoint(Vector3 point)
    {
		
        if (this.trajectory == null)
        {
            this.trajectory = new ArrayList();
        }
        this.trajectory.Add(point);
    }

    public Hashtable ToHashtable()
    {
        Hashtable hashtable = new Hashtable();
        Hashtable hashtable2 = new Hashtable();
        hashtable2[(byte)1] = this.origin.x;
        hashtable2[(byte)2] = this.origin.y;
        hashtable2[(byte)3] = this.origin.z;
        hashtable[(byte)11] = hashtable2;
        hashtable2 = new Hashtable();
        Vector3 a = this.direction.normalized;
        a.Scale(new Vector3(127f, 127f, 127f));
        a += new Vector3(127f, 127f, 127f);
        hashtable2[(byte)19] = Convert.ToByte(a.x);
        hashtable2[(byte)20] = Convert.ToByte(a.y);
        hashtable2[(byte)21] = Convert.ToByte(a.z);
        hashtable[(byte)12] = hashtable2;
        if (this.landingTimeStamp != -1)
        {
            hashtable[(byte)9] = this.landingTimeStamp;
            hashtable2 = new Hashtable();
            hashtable2[(byte)1] = this.startOrigin.x;
            hashtable2[(byte)2] = this.startOrigin.y;
            hashtable2[(byte)3] = this.startOrigin.z;
            hashtable[(byte)14] = hashtable2;
        }
        hashtable[(byte)91] = (byte)this.weaponType;
        hashtable[(byte)8] = this.timeStamp;
        if (this.trajectory != null)
        {
            Hashtable[] array = new Hashtable[this.trajectory.Count];
            for (int i = 0; i < this.trajectory.Count; i++)
            {
                hashtable2 = new Hashtable();
                Hashtable hashtable3 = hashtable2;
                object key = (byte)1;
                Vector3 vector = (Vector3)this.trajectory[i];
                hashtable3[key] = vector.x;
                Hashtable hashtable4 = hashtable2;
                object key2 = (byte)2;
                Vector3 vector2 = (Vector3)this.trajectory[i];
                hashtable4[key2] = vector2.y;
                Hashtable hashtable5 = hashtable2;
                object key3 = (byte)3;
                Vector3 vector3 = (Vector3)this.trajectory[i];
                hashtable5[key3] = vector3.z;
                array[i] = hashtable2;
            }
            hashtable[(byte)15] = array;
        }
        if ((int)this.launchMode > 0)
        {
            hashtable[(byte)16] = (byte)this.launchMode;
        }
        if (this.targets != null)
        {
            ArrayList arrayList = new ArrayList();
            List<ShotTarget>.Enumerator enumerator = this.targets.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ShotTarget current = enumerator.Current;
                    Hashtable hashtable6 = new Hashtable();
                    hashtable6[(byte)94] = current.TargetID;
                    if (current.ItemTimeStamp != 0L && current.ItemTimeStamp != -1)
                    {
                        hashtable6[(byte)73] = (int)current.ItemTimeStamp;
                    }
                    if (current.TargetTypeDescriptor > 0)
                    {
                        hashtable6[(byte)68] = current.TargetTypeDescriptor;
                    }
                    arrayList.Add(hashtable6);
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            if (arrayList != null)
            {
                Hashtable[] array2 = new Hashtable[arrayList.Count];
                for (int j = 0; j < arrayList.Count; j++)
                {
                    array2[j] = (Hashtable)arrayList[j];
                }
                hashtable[(byte)86] = array2;
            }
        }
        return hashtable;
    }

    public static Shot FromHashtable(Hashtable shotData)
    {
        Hashtable hashtable = (Hashtable)shotData[(byte)11];
        Vector3 vector = new Vector3(0f, 0f, 0f);
        bool flag = false;
        bool flag2 = false;
        if (hashtable.ContainsKey((byte)1))
        {
            vector.x = (float)hashtable[(byte)1];
            flag = true;
        }
        if (hashtable.ContainsKey((byte)2))
        {
            vector.y = (float)hashtable[(byte)2];
            flag = true;
        }
        if (hashtable.ContainsKey((byte)3))
        {
            vector.z = (float)hashtable[(byte)3];
            flag = true;
        }
        hashtable = (Hashtable)shotData[(byte)12];
        Vector3 a = new Vector3(0f, 0f, 0f);
        if (hashtable.ContainsKey((byte)19))
        {
            a.x = (float)(int)(byte)hashtable[(byte)19];
            flag2 = true;
        }
        if (hashtable.ContainsKey((byte)20))
        {
            a.y = (float)(int)(byte)hashtable[(byte)20];
            flag2 = true;
        }
        if (hashtable.ContainsKey((byte)21))
        {
            a.z = (float)(int)(byte)hashtable[(byte)21];
            flag2 = true;
        }
        float num = 0.007874016f;
        a -= new Vector3(127f, 127f, 127f);
        a.Scale(new Vector3(num, num, num));
        byte b = (byte)shotData[(byte)91];
        Shot shot = new Shot(vector, a, b);
        shot.hasOrigin = flag;
        shot.hasDirection = flag2;
        if (shotData.ContainsKey((byte)18))
        {
            shot.crit = (bool)shotData[(byte)18];
        }
        if (shotData.ContainsKey((byte)86))
        {
            Hashtable[] array = (Hashtable[])shotData[(byte)86];
            Hashtable[] array2 = array;
            foreach (Hashtable hashtable2 in array2)
            {
                ShotTarget shotTarget = new ShotTarget();
                shotTarget.TargetID = (int)hashtable2[(byte)94];
                if (hashtable2.ContainsKey((byte)92))
                {
                    shotTarget.HealthDamage = (short)hashtable2[(byte)92];
                }
                if (hashtable2.ContainsKey((byte)93))
                {
                    shotTarget.EnergyDamage = (short)hashtable2[(byte)93];
                }
                if (hashtable2.ContainsKey((byte)73))
                {
                    shotTarget.ItemTimeStamp = (int)hashtable2[(byte)73];
                }
                if (hashtable2.ContainsKey((byte)68))
                {
                    shotTarget.TargetTypeDescriptor = (byte)hashtable2[(byte)68];
                }
                shot.Targets.Add(shotTarget);
            }
        }
        long launchTimeStamp = 0L;
        if (shotData.ContainsKey((byte)8))
        {
            launchTimeStamp = (long)shotData[(byte)8];
        }
        if (shotData.ContainsKey((byte)9))
        {
            Vector3 vector2 = default(Vector3);
            long num2 = (long)shotData[(byte)9];
            hashtable = (Hashtable)shotData[(byte)14];
            vector2.x = (float)hashtable[(byte)1];
            vector2.y = (float)hashtable[(byte)2];
            vector2.z = (float)hashtable[(byte)3];
            shot.Launch(vector2, new Vector3(0f, 0f, 0f), launchTimeStamp, num2);
        }
        else
        {
            shot.TimeStamp = launchTimeStamp;
        }
        if (shotData.ContainsKey((byte)16))
        {
            shot.LaunchMode = (LaunchModes)(byte)shotData[(byte)16];
        }
		if (shotData.ContainsKey((byte)15))
		{
			Hashtable[] array3 = (Hashtable[])shotData[(byte)15];
			for (int j = 0; j < array3.Length; j++)
			{
				hashtable = array3[j];
				float x = (float)hashtable[(byte)1];
				float y = (float)hashtable[(byte)2];
				float z = (float)hashtable[(byte)3];
				shot.addPoint(new Vector3(x, y, z));
			}
		}
        return shot;
    }
}


