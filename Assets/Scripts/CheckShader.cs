// ILSpyBased#2
using UnityEngine;

public class CheckShader : MonoBehaviour
{
    private void FixedUpdate()
    {
        GameObject gameObject = base.gameObject;
        string text = string.Format("m:{0} t:{1} c:{2} shader:{3}", gameObject.GetComponent<Renderer>().material.name, gameObject.GetComponent<Renderer>().material.mainTexture, gameObject.GetComponent<Renderer>().material.color, gameObject.GetComponent<Renderer>().material.shader);
    }
}


