// ILSpyBased#2
using System.Collections;
using UnityEngine;

public class AssetLoaderCallback
{
    public delegate void AssetLoaderFinishTextureListener(bool success, Texture2D texture, Hashtable parameters);

    public delegate void AssetLoaderFinishListener(bool success, AssetBundle assetBundle, Hashtable parameters);

    public delegate void AssetLoaderProgressListener(float progress);

    private AssetLoaderFinishListener assetLoaderFinishListener;

    private AssetLoaderFinishTextureListener assetLoaderFinishTextureListener;

    private AssetLoaderProgressListener assetLoaderProgressListener;

    private Hashtable parameters;

    public Hashtable Parameters
    {
        get
        {
            return this.parameters;
        }
    }

    public bool HasProgressListener
    {
        get
        {
            return this.assetLoaderProgressListener != null;
        }
    }

    public AssetLoaderCallback(Hashtable parameters, AssetLoaderFinishListener assetLoaderFinishListener, AssetLoaderProgressListener assetLoaderProgressListener)
    {
        this.assetLoaderProgressListener = assetLoaderProgressListener;
        this.assetLoaderFinishListener = assetLoaderFinishListener;
        this.parameters = parameters;
    }

    public AssetLoaderCallback(Hashtable parameters, AssetLoaderFinishTextureListener assetLoaderFinishTextureListener)
    {
        this.assetLoaderFinishTextureListener = assetLoaderFinishTextureListener;
        this.parameters = parameters;
    }

    public AssetLoaderCallback(Hashtable parameters, AssetLoaderFinishListener assetLoaderFinishListener)
    {
        this.assetLoaderFinishListener = assetLoaderFinishListener;
        this.parameters = parameters;
    }

    public void CallFinish(bool success, AssetBundle assetBundle)
    {
        if (this.assetLoaderFinishListener != null)
        {
            this.assetLoaderFinishListener(success, assetBundle, this.parameters);
        }
    }

    public void CallFinishTexture(bool success, Texture2D texture)
    {
        if (this.assetLoaderFinishTextureListener == null)
        {
            UnityEngine.Debug.LogError("assetLoaderFinishTextureLisener is NULL!!!");
        }
        else
        {
            this.assetLoaderFinishTextureListener(success, texture, this.parameters);
        }
    }

    public void CallProgress(float progress)
    {
        if (this.HasProgressListener)
        {
            this.assetLoaderProgressListener(progress);
        }
    }
}


