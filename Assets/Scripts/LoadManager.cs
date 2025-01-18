using System;
using System.Collections;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
	private void FixedUpdate()
	{
		if (LoadManager.assetLoaders == null)
		{
			return;
		}
		foreach (object obj in LoadManager.assetLoaders)
		{
			AssetLoader assetLoader = (AssetLoader)obj;
			assetLoader.Update();
		}
	}

	public void AddAssetLoader(AssetLoader assetLoader)
	{
		if (LoadManager.assetLoaders == null)
		{
			LoadManager.assetLoaders = new ArrayList();
		}
		LoadManager.assetLoaders.Add(assetLoader);
	}

	public void RemoveAssetLoader(AssetLoader assetLoader)
	{
		LoadManager.assetLoaders.Remove(assetLoader);
	}

	public static LoadManager Instance
	{
		get
		{
			if (LoadManager.mInstance == null)
			{
				LoadManager.mInstance = (new GameObject("LoadManager").AddComponent(typeof(LoadManager)) as LoadManager);
			}
			return LoadManager.mInstance;
		}
	}

	public static string getLoadingURL(string url)
	{
		string result = string.Empty;
        /*	if (url.IndexOf("file://") == 0 || url.IndexOf("http://") == 0)
            {
                result = url;
            }
            if (Application.platform == (RuntimePlatform)3 || Application.platform == (RuntimePlatform)5)
            {
                result = Application.dataPath + "/AssetBundles/" + url;
            }
            else if (Application.platform == null || Application.platform == (RuntimePlatform)7)
            {
                result = "file://" + Application.dataPath + "/../AssetBundles/" + url;
            }
            else if (Application.platform == (RuntimePlatform)11)
            {
                result = "jar:file://" + Application.dataPath + "!/assets/" + url;
            } */
        return AuthManager.AssetBandle + url;
            //"https://sincere-games.ru/server_103/ABPC/" + url;
        //"https://static.pentagames.net/CC/AssetBundles/" + url;  //result;
    }

    private static LoadManager mInstance;

	private static ArrayList assetLoaders;
}
