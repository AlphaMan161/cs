// ILSpyBased#2
using System.Collections;
using UnityEngine;

public class RuntimeTextureSwitch : RuntimeTextureLoader
{
    public string textureIDList = string.Empty;

    public void Switch(int id)
    {
        if (id == 0)
        {
            base.LoadTexture(base.textureID);
        }
        else
        {
            string[] array = this.textureIDList.Split(',');
            if (id <= array.Length)
            {
                base.LoadTexture(array[id - 1]);
            }
        }
    }

    public override void OnTextureLoaded(bool success, AssetBundle assetBundle, Hashtable parameters)
    {
        if (success)
        {
            base.StartCoroutine(base.ApplyTextureAsync(success, assetBundle, parameters));
        }
    }
}


