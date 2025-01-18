// ILSpyBased#2
using System.Collections;
using UnityEngine;

public class HandTextureLoader : RuntimeTextureLoader
{
    private void Start()
    {
        if (transform.parent.name == "SNG_Snowgun")
        {
            transform.parent.GetChild(3).GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
        }
        else if (transform.parent.name == "SG_Spas")
        {
           // transform.parent.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
        }
        else {
            transform.parent.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
        }
        base.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader = Shader.Find("Legacy Shaders/Diffuse");
        if (base.textureID != string.Empty)
        {
            base.ForceLoad();
        }
    }
}


