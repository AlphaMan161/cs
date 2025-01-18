// dnSpy decompiler from Assembly-CSharp.dll class: WallManager
using System;
using System.Collections;
using UnityEngine;

public class WallManager : MonoBehaviour
{
	private static WallManager Instance
	{
		get
		{
			if (WallManager.hInstance == null)
			{
				WallManager.hInstance = (new GameObject("WallManager").AddComponent(typeof(WallManager)) as WallManager);
			}
			return WallManager.hInstance;
		}
	}

	public static string FileName
	{
		get
		{
			return WallManager.Instance.fileName;
		}
		set
		{
			WallManager.Instance.fileName = value;
			UnityEngine.Debug.Log("[WallManager] Set FileName: " + value);
		}
	}

	public static string UploadUrl
	{
		get
		{
			return WallManager.Instance.uploadUrl;
		}
		set
		{
			WallManager.Instance.uploadUrl = value;
			WallManager.Instance.isUpdatedUploadUrl = true;
			UnityEngine.Debug.Log("[WallManager] Set UploadUrl: " + value);
		}
	}

	public static void CreateAndUpload(Rect rect)
	{
		UnityEngine.Debug.LogError("CreateAndUpload Rect: " + rect.ToString());
		if ((Configuration.SType == ServerType.VK || Configuration.SType == ServerType.OD || Configuration.SType == ServerType.MM) && !WallManager.Instance.isUpdatedUploadUrl)
		{
			WebCall.NeedWallUploadUrl();
		}
		WallManager.Instance.StartCoroutine(WallManager.ICreateAndUpload(rect));
	}

	public static IEnumerator ICreateAndUpload(Rect rect)
	{
		yield return new WaitForEndOfFrame();
		Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(rect.x, rect.y, rect.width, rect.height), 0, 0);
		tex.Apply();
		byte[] bytes = tex.EncodeToPNG();
		UnityEngine.Object.Destroy(tex);
		if ((Configuration.SType == ServerType.VK || Configuration.SType == ServerType.OD || Configuration.SType == ServerType.MM) && !WallManager.Instance.isUpdatedUploadUrl)
		{
			while (!WallManager.Instance.isUpdatedUploadUrl)
			{
				yield return new WaitForSeconds(1f);
			}
		}
		WallManager.Instance.StartCoroutine(WallManager.IUpload(bytes));
		yield break;
	}

	private static IEnumerator IUpload(byte[] bytes)
	{
		if (Screen.fullScreen)
		{
			Screen.fullScreen = false;
		}
		WWWForm form = new WWWForm();
		form.AddBinaryData("photo", bytes, string.Format("screen_{0}_{1}.png", LocalUser.UserID, Time.time), "image/png");
		WWW www = new WWW(WallManager.Instance.uploadUrl, form);
		yield return www;
		if (www.error != null)
		{
			UnityEngine.Debug.LogError(www.error);
		}
		else
		{
			UnityEngine.Debug.Log("Finished Uploading Screenshot: " + www.text);
			if (Configuration.SType == ServerType.VK || Configuration.SType == ServerType.OD || Configuration.SType == ServerType.MM)
			{
				WebCall.WallUploaded(www.text);
			}
			WebCall.Analitic("Social", "WallImage Uploaded", new object[0]);
		}
		yield break;
	}

	private static WallManager hInstance = null;

	private static object syncLook = new object();

	private string fileName = "photo";

	private string uploadUrl = "http://www.cc.com.ua/upload.php";

	private bool isUpdatedUploadUrl;
}
