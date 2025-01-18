// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoader
{
    protected short attempt;

    protected AssetBundle assetBundle;

    protected WWW downloader;

    protected bool isDone;

    protected string url;

    protected List<AssetLoaderCallback> listeners = new List<AssetLoaderCallback>();

    public AssetBundle AssetBundle
    {
        get
        {
            return this.assetBundle;
        }
    }

    public bool IsDone
    {
        get
        {
            return this.isDone;
        }
    }

    public AssetLoader(string url)
    {
        this.isDone = false;
        this.url = url;
    }

    public void LoadAssetBundle(AssetLoaderCallback assetLoaderCallback, bool cached, int version)
    {
        if (Configuration.DebugLoadManager)
        {
            UnityEngine.Debug.Log("[LoadManager] LoadAssetBundle url: " + this.url);
        }
        if (cached)
        {
            this.downloader = WWW.LoadFromCacheOrDownload(this.url, version);
        }
        else
        {
            this.downloader = new WWW(this.url);
        }
        LoadManager.Instance.StartCoroutine(this.LoadAssetIdle());
    }

    public void Update()
    {
        if (this.downloader != null)
        {
            List<AssetLoaderCallback>.Enumerator enumerator = this.listeners.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AssetLoaderCallback current = enumerator.Current;
                    current.CallProgress(this.downloader.progress);
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
        }
    }

    protected IEnumerator LoadAssetIdle()
    {
        if (this.downloader.error != null)
        {
            this.FinishError();
        }
        else
        {
            if (this.listeners[0].HasProgressListener)
            {
                LoadManager.Instance.AddAssetLoader(this);
            }
            yield return (object)this.downloader;
            yield return (object)new WaitForSeconds(0.5f);
            if (this.downloader.error != null)
            {
                this.FinishError();
            }
            else
            {
                this.Finish();
            }
        }
    }

    public void ReplaceCallback(AssetLoaderCallback assetLoaderCallback)
    {
        this.listeners[0] = assetLoaderCallback;
    }

    public void AddCallback(AssetLoaderCallback assetLoaderCallback)
    {
        this.listeners.Add(assetLoaderCallback);
    }

    protected virtual void Finish()
    {
        if (Configuration.DebugLoadManager)
        {
            UnityEngine.Debug.Log("[LoadManager] Finish url: " + this.downloader.url);
        }
        if ((UnityEngine.Object)PlayerManager.Instance != (UnityEngine.Object)null && Datameter.enabled)
        {
            Datameter.TextureCounter++;
            Datameter.TextureSizeCounter += (float)PlayerManager.GetObjectSize(this.downloader.assetBundle);
        }
        this.assetBundle = this.downloader.assetBundle;
        this.isDone = true;
        this.downloader.Dispose();
        LoadManager.Instance.RemoveAssetLoader(this);
        List<AssetLoaderCallback>.Enumerator enumerator = this.listeners.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AssetLoaderCallback current = enumerator.Current;
                current.CallFinish(true, this.assetBundle);
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    protected virtual void FinishError()
    {
        UnityEngine.Debug.LogError(this.downloader.error);
        this.downloader.Dispose();
        LoadManager.Instance.RemoveAssetLoader(this);
        if (this.attempt > Ajax.ERROR_ATTEMPT)
        {
            List<AssetLoaderCallback>.Enumerator enumerator = this.listeners.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AssetLoaderCallback current = enumerator.Current;
                    current.CallFinish(false, null);
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
        }
        else
        {
            LoadManager.Instance.StartCoroutine(this.LoadAssetBundleAgain());
        }
    }

    protected IEnumerator LoadAssetBundleAgain()
    {
        yield return (object)new WaitForSeconds(Ajax.ERROR_WAIT_SEC);
        this.attempt++;
        this.downloader = new WWW(this.url);
        LoadManager.Instance.StartCoroutine(this.LoadAssetIdle());
    }

    public void Dispose()
    {
        if (this.downloader != null)
        {
            UnityEngine.Debug.Log("DISPOSE!!!");
            if ((UnityEngine.Object)this.assetBundle != (UnityEngine.Object)null)
            {
                UnityEngine.Debug.Log("UNLOAD ASSET BUNDLE!!!");
                this.assetBundle.Unload(true);
            }
            this.downloader.Dispose();
        }
    }
}


