// ILSpyBased#2
using UnityEngine;

public class ControlPoint
{
    public GameObject controlPointObject;

    public int Team = -1;

    public ControlPoint(GameObject controlPointObject, int Team)
    {
        this.controlPointObject = controlPointObject;
        this.Team = Team;
    }

    public ControlPoint(GameObject controlPointObject)
    {
        this.controlPointObject = controlPointObject;
    }
}


