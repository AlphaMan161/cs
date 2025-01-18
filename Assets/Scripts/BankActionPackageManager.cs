// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class BankActionPackageManager
{
    private static BankActionPackageManager hInstance;

    private List<BankActionPackage> packageList = new List<BankActionPackage>();

    private static BankActionPackageManager Instance
    {
        get
        {
            if (BankActionPackageManager.hInstance == null)
            {
                BankActionPackageManager.hInstance = new BankActionPackageManager();
            }
            return BankActionPackageManager.hInstance;
        }
    }

    public static List<BankActionPackage> PackageList
    {
        get
        {
            return BankActionPackageManager.Instance.packageList;
        }
    }

    private BankActionPackageManager()
    {
    }

    public static void Init(JSONObject json)
    {
        UnityEngine.Debug.Log("[BankActionPackageManager] Init " + Time.time);
        if (json.type == JSONObject.Type.ARRAY)
        {
            for (int i = 0; i < json.Count; i++)
            {
                if (json[i].type != 0)
                {
                    BankActionPackage bankActionPackage = new BankActionPackage(json[i].GetField("i").str, Convert.ToInt16(json[i].GetField("id").n), Convert.ToUInt32(json[i].GetField("ed").n), new BankActionPackage.BankActionPackageHandler(WebCall.BankPackage));
                    bankActionPackage.OnEndtime += new BankActionPackage.BankActionPackageHandler(BankActionPackageManager.OnEndtime);
                    BankActionPackageManager.Instance.packageList.Add(bankActionPackage);
                }
            }
        }
    }

    private static void OnEndtime(object obj)
    {
        BankActionPackage bankActionPackage = obj as BankActionPackage;
        bankActionPackage.OnEndtime -= new BankActionPackage.BankActionPackageHandler(BankActionPackageManager.OnEndtime);
        BankActionPackageManager.Instance.packageList.Remove(bankActionPackage);
        UnityEngine.Debug.LogError("OnEndTime " + bankActionPackage.PackageID + " time: " + Time.time);
    }
}


