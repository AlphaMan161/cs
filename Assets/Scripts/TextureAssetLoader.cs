// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAssetLoader : AssetLoader
{
    public Texture2D Texture;

    public Material SharedMaterial;

    public TextureAssetLoader(string url)
        : base(url)
    {
    }

    protected override void Finish()
    {
        if (Configuration.DebugLoadManager)
        {
            UnityEngine.Debug.Log("[LoadManager] Finish url: " + base.downloader.url);
        }
        if ((UnityEngine.Object)PlayerManager.Instance != (UnityEngine.Object)null && Datameter.enabled)
        {
            Datameter.TextureCounter++;
            Datameter.TextureSizeCounter += (float)PlayerManager.GetObjectSize(base.downloader.assetBundle);
        }
        base.assetBundle = base.downloader.assetBundle;
        base.downloader.Dispose();
        LoadManager.Instance.RemoveAssetLoader(this);
        AssetLoaderCallback assetLoaderCallback = base.listeners[0];
        RuntimeTextureLoader runtimeTextureLoader = (RuntimeTextureLoader)assetLoaderCallback.Parameters[(byte)150];
        if ((UnityEngine.Object)runtimeTextureLoader != (UnityEngine.Object)null)
        {
            runtimeTextureLoader.StartCoroutine(this.LoadTextureAssetAsync(base.assetBundle));
        }
    }

    protected IEnumerator LoadTextureAssetAsync(AssetBundle assetBundle)
    {
        AssetBundleRequest assetLoadRequest = assetBundle.LoadAllAssetsAsync();
        yield return (object)assetLoadRequest;
        this.Texture = (assetLoadRequest.allAssets[0] as Texture2D);
        base.isDone = true;
        List<AssetLoaderCallback>.Enumerator enumerator = base.listeners.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AssetLoaderCallback listener = enumerator.Current;
                listener.CallFinishTexture(true, this.Texture);
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    protected override void FinishError()
    {
        UnityEngine.Debug.LogError(base.downloader.error);
        base.downloader.Dispose();
        LoadManager.Instance.RemoveAssetLoader(this);
        if (base.attempt > Ajax.ERROR_ATTEMPT)
        {
            List<AssetLoaderCallback>.Enumerator enumerator = base.listeners.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AssetLoaderCallback current = enumerator.Current;
                    current.CallFinishTexture(false, null);
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
        }
        else
        {
            LoadManager.Instance.StartCoroutine(base.LoadAssetBundleAgain());
        }
    }
}


