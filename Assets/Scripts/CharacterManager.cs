// dnSpy decompiler from Assembly-CSharp.dll class: CharacterManager
using System;
using System.Collections;
using UnityEngine;

public class CharacterManager
{
	private CharacterManager()
	{
	}

	public static event CharacterManager.CharacterManagerHandler OnLoad;

	public static event CharacterManager.CharacterManagerProgressHandler OnLoadProgress;

	public static event CharacterManager.CharacterManagerHandler OnError;

	public static CharacterManager Instance
	{
		get
		{
			if (CharacterManager.hInstance == null)
			{
				CharacterManager.hInstance = new CharacterManager();
			}
			return CharacterManager.hInstance;
		}
	}

	public void Init()
	{
		UnityEngine.Debug.Log("[CharacterManager] Init " + Time.time);
		CharacterManager.characterLoader = new AssetLoader(LoadManager.getLoadingURL(WebUrls.CHARACTER_URL));
		CharacterManager.characterLoader.AddCallback(new AssetLoaderCallback(null, new AssetLoaderCallback.AssetLoaderFinishListener(this.OnCharacterLoaded), new AssetLoaderCallback.AssetLoaderProgressListener(this.OnCharacterLoadProgress)));
		CharacterManager.characterLoader.LoadAssetBundle(null, false, 0);
	}

	private void OnCharacterLoadProgress(float progress)
	{
		if (CharacterManager.OnLoadProgress != null)
		{
			CharacterManager.OnLoadProgress(this, progress);
		}
	}

	private void OnCharacterLoaded(bool success, AssetBundle assetBundle, Hashtable parameters)
	{
		if (assetBundle == null)
		{
			string text = "OnCharacter not init: empty!";
			if (CharacterManager.OnError == null)
			{
				throw new Exception("[CharacterManager] " + text);
			}
			CharacterManager.OnError(text);
		}
		if (success)
		{
			this.characterBundle = assetBundle;
			if (CharacterManager.OnLoad != null)
			{
				CharacterManager.OnLoad(this);
			}
		}
		else
		{
			string text2 = "OnCharacter not init: couldn't load!";
			if (CharacterManager.OnError == null)
			{
				throw new Exception("[CharacterManager] " + text2);
			}
			CharacterManager.OnError(text2);
		}
	}

	public GameObject GetPlayer()
	{
		return (GameObject)this.characterBundle.LoadAsset("Fighter_locomotion");
	}

	public GameObject GetPlayerEnemy()
	{
		return (GameObject)this.characterBundle.LoadAsset("Enemy_Fighter_locomotion");
	}

	public void GetPlayerEnemyAsync(CharacterManager.OnCharacterAssetLoaded OnLoaded, Hashtable data, int actorID)
	{
		if (PlayerManager.Instance != null)
		{
			PlayerManager.Instance.StartCoroutine(this.LoadCharacterAssetAsync(this.characterBundle, "Enemy_Fighter_locomotion", OnLoaded, data, actorID));
		}
	}

	protected IEnumerator LoadCharacterAssetAsync(AssetBundle assetBundle, string assetName, CharacterManager.OnCharacterAssetLoaded OnLoaded, Hashtable data, int actorID)
	{
		AssetBundleRequest assetLoadRequest = assetBundle.LoadAssetAsync(assetName);
		yield return assetLoadRequest;
		GameObject characterObject = assetLoadRequest.asset as GameObject;
		OnLoaded(characterObject, data, actorID);
		yield break;
	}

	public GameObject GetDamageInfo()
	{
		return (GameObject)this.characterBundle.LoadAsset("DamageInfo");
	}

	public GameObject GetPrefab(string name)
	{
		return (GameObject)this.characterBundle.LoadAsset(name);
	}

	private static CharacterManager hInstance;

	private AssetBundle characterBundle;

	private static AssetLoader characterLoader;

	public delegate void CharacterManagerHandler(object sender);

	public delegate void CharacterManagerProgressHandler(object sender, float progress);

	public delegate void OnCharacterAssetLoaded(GameObject characterObject, Hashtable data, int actorID);
}
