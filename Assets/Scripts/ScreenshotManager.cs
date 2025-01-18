// ILSpyBased#2
using System.Collections;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{
    private static ScreenshotManager hInstance = null;

    private static object syncLook = new object();

    private string fileName = "file1";

    private string uploadUrl = "http://www.cc.com.ua/upload.php";

    private bool isUpdatedUploadUrl;

    private static ScreenshotManager Instance
    {
        get
        {
            if ((Object)ScreenshotManager.hInstance == (Object)null)
            {
                ScreenshotManager.hInstance = (new GameObject("ScreenshotManager").AddComponent(typeof(ScreenshotManager)) as ScreenshotManager);
            }
            return ScreenshotManager.hInstance;
        }
    }

    public static string FileName
    {
        get
        {
            return ScreenshotManager.Instance.fileName;
        }
        set
        {
            ScreenshotManager.Instance.fileName = value;
            UnityEngine.Debug.Log("[ScreenshotManager] Set FileName: " + value);
        }
    }

    public static string UploadUrl
    {
        get
        {
            return ScreenshotManager.Instance.uploadUrl;
        }
        set
        {
            ScreenshotManager.Instance.uploadUrl = value;
            ScreenshotManager.Instance.isUpdatedUploadUrl = true;
            UnityEngine.Debug.Log("[ScreenshotManager] Set UploadUrl: " + value);
        }
    }

    public static void CreateAndUpload()
    {
        UnityEngine.Debug.Log("[ScreenshotManager] CreateAndUpload");
        if ((Configuration.SType == ServerType.VK || Configuration.SType == ServerType.OD || Configuration.SType == ServerType.MM) && !ScreenshotManager.Instance.isUpdatedUploadUrl)
        {
            UnityEngine.Debug.Log("[ScreenshotManager] WebCall.NeedUploadUrl");
            WebCall.NeedUploadUrl();
        }
        ScreenshotManager.Instance.StartCoroutine(ScreenshotManager.ICreateAndUpload());
    }

    public static IEnumerator ICreateAndUpload()
    {
        UnityEngine.Debug.Log("[ScreenshotManager] ICreateAndUpload");
        yield return (object)new WaitForEndOfFrame();
        float width = (float)Screen.width;
        float height = (float)Screen.height;
        Texture2D tex = new Texture2D((int)width, (int)height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();
        UnityEngine.Object.DestroyImmediate(tex);
        if ((Configuration.SType == ServerType.VK || Configuration.SType == ServerType.OD || Configuration.SType == ServerType.MM) && !ScreenshotManager.Instance.isUpdatedUploadUrl)
        {
            while (!ScreenshotManager.Instance.isUpdatedUploadUrl)
            {
                yield return (object)new WaitForSeconds(1f);
            }
        }
        ScreenshotManager.Instance.StartCoroutine(ScreenshotManager.IUpload(bytes));
    }

    private static Texture2D AddWaterInfo(Texture2D tex)
    {
        string text = "Hello";
        int num = 5;
        Color[] pixels = tex.GetPixels(0, tex.height - 1, text.Length * num, 1);
        for (int i = 0; i < text.Length; i++)
        {
            float num2 = 0f;
            float num3 = 0f;
            float num4 = 0f;
            for (int j = 1; j < num; j++)
            {
                num2 += pixels[i + j].r;
                num3 += pixels[i + j].g;
                num4 += pixels[i + j].b;
            }
            num2 /= (float)(num - 1);
            num3 /= (float)(num - 1);
            num4 /= (float)(num - 1);
            int num5 = text[i];
            int num6 = num5 % 10;
            int num7 = num5 / 10 % 10;
            int num8 = num5 / 100;
            float num9 = (float)num6 / 255f;
            float num10 = (float)num7 / 255f;
            float num11 = (float)num8 / 255f;
            num2 = num9;
            num3 = num10;
            num4 = num11;
            pixels[i * num].r = num2;
            pixels[i * num].g = num3;
            pixels[i * num].b = num4;
        }
        tex.SetPixels(0, tex.height - 1, text.Length * num, 1, pixels);
        return tex;
    }

    private static IEnumerator IUpload(byte[] bytes)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData(ScreenshotManager.Instance.fileName, bytes, string.Format("screen_{0}_{1}.png", LocalUser.UserID, Time.time), "image/png");
        WWW www = new WWW(ScreenshotManager.Instance.uploadUrl, form);
        yield return (object)www;
        if (www.error != null)
        {
            UnityEngine.Debug.LogError(www.error);
        }
        else
        {
            UnityEngine.Debug.Log("Finished Uploading Screenshot: " + www.text);
            if (Configuration.SType == ServerType.VK || Configuration.SType == ServerType.OD || Configuration.SType == ServerType.MM)
            {
                WebCall.ScreenUploaded(www.text);
            }
            WebCall.Analitic("Social", "ScreenShot Uploaded");
        }
    }
}


