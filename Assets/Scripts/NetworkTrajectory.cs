// ILSpyBased#2
using System.Collections;

public class NetworkTrajectory
{
    private Shot shot;

    private double timeStamp;

    public Shot Shot
    {
        get
        {
            return this.shot;
        }
        set
        {
        }
    }

    public double TimeStamp
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

    private NetworkTrajectory()
    {
    }

    public void Load(NetworkTrajectory ntraj)
    {
        this.shot = ntraj.Shot;
        this.timeStamp = ntraj.timeStamp;
    }

    public void Update(NetworkTrajectory ntraj)
    {
        this.shot = ntraj.Shot;
        this.timeStamp = ntraj.timeStamp;
    }

    public static NetworkTrajectory Clone(NetworkTrajectory ntraj)
    {
        NetworkTrajectory networkTrajectory = new NetworkTrajectory();
        networkTrajectory.Load(ntraj);
        return networkTrajectory;
    }

    public Hashtable ToHashtable()
    {
        return new Hashtable();
    }

    public static NetworkTrajectory FromShot(Shot shot)
    {
        NetworkTrajectory networkTrajectory = new NetworkTrajectory();
        networkTrajectory.shot = shot;
        return networkTrajectory;
    }
}


