// ILSpyBased#2
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ClanArmManager
{
    private static object _sync = new object();

    private static ClanArmManager hInstance = null;

    private List<ClanArm> arms = new List<ClanArm>();

    private List<ClanArm> hided = new List<ClanArm>();

    private List<ClanArm> defaultArms = new List<ClanArm>();

    private static ClanArmManager Instance
    {
        get
        {
            if (ClanArmManager.hInstance == null)
            {
                object sync = ClanArmManager._sync;
                Monitor.Enter(sync);
                try
                {
                    if (ClanArmManager.hInstance == null)
                    {
                        ClanArmManager.hInstance = new ClanArmManager();
                    }
                }
                finally
                {
                    Monitor.Exit(sync);
                }
            }
            return ClanArmManager.hInstance;
        }
    }

    public static List<ClanArm> Arms
    {
        get
        {
            return ClanArmManager.Instance.arms;
        }
    }

    public static List<ClanArm> Hided
    {
        get
        {
            return ClanArmManager.Instance.hided;
        }
    }

    public static List<ClanArm> DefaultArms
    {
        get
        {
            return ClanArmManager.Instance.defaultArms;
        }
    }

    private ClanArmManager()
    {
        Ajax.Request(WebUrls.CLAN_ARMS_URL, new AjaxRequest.AjaxHandler(this.OnInit));
    }

    private void OnInit(object result, AjaxRequest request)
    {
        JSONObject jSONObject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
        if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
        {
            JSONObject field = jSONObject.GetField("arms");
            for (int i = 0; i < field.Count; i++)
            {
                if (field[i] != null)
                {
                    ClanArm clanArm = new ClanArm(field[i]);
                    if (clanArm.Default)
                    {
                        this.defaultArms.Add(clanArm);
                    }
                    this.arms.Add(clanArm);
                }
            }
            this.hided.Add(new ClanArm(14, "https://pp.vk.me/c638416/v638416032/aae7/sfWzsfxQ3EY.jpg", false, null));
            this.hided.Add(new ClanArm(15, "https://pp.vk.me/c626123/v626123032/3b865/2Ak40IT8Syc.jpg", false, null));
            this.hided.Add(new ClanArm(16, "https://pp.vk.me/c626123/v626123032/3d355/7mPX0yRI4zk.jpg", false, null));
        }
    }

    public static ClanArm GetArm(int id)
    {
        ClanArm clanArm = ClanArmManager.Arms.Find((ClanArm x) => x.ArmID == id);
        if (clanArm != null)
        {
            return clanArm;
        }
        return ClanArmManager.Hided.Find((ClanArm x) => x.ArmID == id);
    }

    public static Texture2D GetTexture(int id)
    {
        ClanArm arm = ClanArmManager.GetArm(id);
        if (arm != null)
        {
            return arm.Ico;
        }
        return null;
    }
}


