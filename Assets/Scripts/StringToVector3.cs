// ILSpyBased#2
using UnityEngine;

public class StringToVector3 : MonoBehaviour
{
    public Vector3 GetVector3(string rString)
    {
        string[] array = rString.Substring(1, rString.Length - 2).Split(',');
        float x = float.Parse(array[0]);
        float y = float.Parse(array[1]);
        float z = float.Parse(array[2]);
        return new Vector3(x, y, z);
    }
}


