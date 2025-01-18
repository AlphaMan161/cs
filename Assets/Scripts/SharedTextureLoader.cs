// ILSpyBased#2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedTextureLoader
{
    private static SharedTextureLoader mInstance;

    private Queue<AssetLoader> queue = new Queue<AssetLoader>();

    private Dictionary<string, TextureAssetLoader> textureLoaders = new Dictionary<string, TextureAssetLoader>();

    public static SharedTextureLoader Instance
    {
        get
        {
            if (SharedTextureLoader.mInstance == null)
            {
                SharedTextureLoader.mInstance = new SharedTextureLoader();
            }
            return SharedTextureLoader.mInstance;
        }
        set
        {
            SharedTextureLoader.mInstance = value;
        }
    }

    public void LoadTexture(string textureID, Material sharedMaterial, RuntimeTextureLoader runtimeTextureLoader, Hashtable parameters)
    {
        this.LoadTexture(textureID, sharedMaterial, runtimeTextureLoader, parameters, true);
    }

    public void LoadTexture(string textureID, Material sharedMaterial, RuntimeTextureLoader runtimeTextureLoader, Hashtable parameters, bool replaceListeners)
    {
        parameters[(byte)151] = textureID;
        parameters[(byte)150] = runtimeTextureLoader;
        if (this.textureLoaders.ContainsKey(textureID))
        {
            if (this.textureLoaders[textureID].IsDone)
            {
                runtimeTextureLoader.OnTextureLoadedAndReady(true, this.textureLoaders[textureID].Texture, parameters);
            }
            else if (replaceListeners || (Object)this.textureLoaders[textureID].SharedMaterial == (Object)sharedMaterial)
            {
                this.textureLoaders[textureID].ReplaceCallback(new AssetLoaderCallback(parameters, new AssetLoaderCallback.AssetLoaderFinishTextureListener(this.onTextureLoadedAndReady)));
            }
            else
            {
                this.textureLoaders[textureID].AddCallback(new AssetLoaderCallback(parameters, new AssetLoaderCallback.AssetLoaderFinishTextureListener(this.onTextureLoadedAndReady)));
            }
        }
        else
        {
            this.textureLoaders[textureID] = this.DownloadTexture(textureID, sharedMaterial, parameters);
        }
    }

    public TextureAssetLoader DownloadTexture(string textureID, Material sharedMaterial, Hashtable parameters)
    {
        TextureAssetLoader textureAssetLoader = new TextureAssetLoader(LoadManager.getLoadingURL("Textures/" + textureID + ".unity3d"));
        textureAssetLoader.SharedMaterial = sharedMaterial;
        textureAssetLoader.AddCallback(new AssetLoaderCallback(parameters, new AssetLoaderCallback.AssetLoaderFinishTextureListener(this.onTextureLoadedAndReady)));
        this.AddQueue(textureAssetLoader);
        return textureAssetLoader;
    }

    private void AddQueue(AssetLoader assetLoader)
    {
        this.queue.Enqueue(assetLoader);
        if (this.queue.Count == 1)
        {
            this.ProcessQueue();
        }
    }

    private void ProcessQueue()
    {
        if (this.queue.Count >= 1)
        {
            AssetLoader assetLoader = this.queue.Peek();
            assetLoader.LoadAssetBundle(null, true, WebUrls.VERSION_TEXTURES);
        }
    }

    private void onTextureLoadedAndReady(bool success, Texture2D texture, Hashtable parameters)
    {
        if (success)
        {
            string text = (string)parameters[(byte)151];
            RuntimeTextureLoader runtimeTextureLoader = (RuntimeTextureLoader)parameters[(byte)150];
            if ((Object)runtimeTextureLoader != (Object)null)
            {
                runtimeTextureLoader.OnTextureLoadedAndReady(success, texture, parameters);
            }
        }
        if (this.queue.Count > 0)
        {
            this.queue.Dequeue();
        }
        this.ProcessQueue();
    }
}


