// ILSpyBased#2
using UnityEngine;

public class TrajectoryNode
{
    public Vector3 WorldPosition;

    public float PathPosition;

    public TrajectoryNode(Vector3 position, float pathPosition)
    {
        this.WorldPosition = position;
        this.PathPosition = pathPosition;
    }
}


