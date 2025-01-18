// ILSpyBased#2
using System.Collections.Generic;
using UnityEngine;

public class Trajectory
{
    public Dictionary<int, TrajectoryNode> nodes;

    private float trajectoryLength;

    private float currentPosition;

    private int currentNodeIndex;

    public Dictionary<int, TrajectoryNode> Nodes
    {
        get
        {
            return this.nodes;
        }
    }

    public float TrajectoryLength
    {
        get
        {
            return this.trajectoryLength;
        }
    }

    public Trajectory()
    {
        this.currentNodeIndex = 0;
        this.currentPosition = 0f;
        this.trajectoryLength = 0f;
        this.nodes = new Dictionary<int, TrajectoryNode>();
    }

    public void AddNode(Vector3 nodePosition)
    {
        if (this.nodes.Count > 0)
        {
            this.trajectoryLength += (this.nodes[this.nodes.Count - 1].WorldPosition - nodePosition).magnitude;
        }
        this.nodes.Add(this.nodes.Count, new TrajectoryNode(nodePosition, this.trajectoryLength));
    }

    public new void Finalize()
    {
        if (this.nodes.Count >= 1)
        {
            this.trajectoryLength += (this.nodes[0].WorldPosition - this.nodes[this.nodes.Count - 1].WorldPosition).magnitude;
        }
    }

    public Vector3 Move(Vector3 currentPosition, float speed)
    {
        Vector3 vector = currentPosition;
        if (this.nodes.Count < 1)
        {
            return vector;
        }
        if (this.nodes.Count == 1)
        {
            return this.nodes[0].WorldPosition;
        }
        Vector3 worldPosition = this.nodes[this.currentNodeIndex].WorldPosition;
        int num = this.currentNodeIndex + 1;
        if (num >= this.nodes.Count)
        {
            num = 0;
        }
        Vector3 worldPosition2 = this.nodes[num].WorldPosition;
        Vector3 a = worldPosition2 - currentPosition;
        float magnitude = a.magnitude;
        if (magnitude <= speed)
        {
            vector = worldPosition2;
            speed -= magnitude;
            this.currentNodeIndex = num;
            num = this.currentNodeIndex + 1;
            if (num >= this.nodes.Count)
            {
                num = 0;
            }
        }
        a.Normalize();
        return vector + a * speed;
    }

    public Vector3 GetPosition(float percent)
    {
        Vector3 result = new Vector3(0f, 0f, 0f);
        if (this.nodes.Count < 1)
        {
            return result;
        }
        if (this.nodes.Count == 1)
        {
            return this.nodes[0].WorldPosition;
        }
        float num = percent * this.trajectoryLength;
        int num2 = 0;
        while (this.nodes[num2].PathPosition < num)
        {
            num2++;
            if (num2 >= this.nodes.Count)
            {
                return result;
            }
        }
        if (num2 == 0)
        {
            return this.nodes[0].WorldPosition;
        }
        int key = num2 - 1;
        Vector3 worldPosition = this.nodes[num2].WorldPosition;
        Vector3 worldPosition2 = this.nodes[key].WorldPosition;
        float magnitude = (worldPosition - worldPosition2).magnitude;
        float num3 = num - this.nodes[key].PathPosition;
        float num4 = num3 / magnitude;
        return worldPosition2 * (1f - num4) + worldPosition * num4;
    }
}


