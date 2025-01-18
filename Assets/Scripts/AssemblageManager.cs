// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Threading;

public class AssemblageManager
{
    public delegate void AssemblageEventHandler(object sender);

    private static AssemblageManager hInstance = null;

    private static object syncLook = new object();

    private List<Assemblage> assemblages = new List<Assemblage>();

    private static AssemblageManager Instance
    {
        get
        {
            if (AssemblageManager.hInstance == null)
            {
                object obj = AssemblageManager.syncLook;
                Monitor.Enter(obj);
                try
                {
                    if (AssemblageManager.hInstance == null)
                    {
                        AssemblageManager.hInstance = new AssemblageManager();
                    }
                }
                finally
                {
                    Monitor.Exit(obj);
                }
            }
            return AssemblageManager.hInstance;
        }
    }

    public static event AssemblageEventHandler OnLoad;

    public static event AssemblageEventHandler OnError;

    public static void Init()
    {
        Ajax.Request(WebUrls.ASSEMBLAGE_URL, new AjaxRequest.AjaxHandler(AssemblageManager.Instance.OnInit));
    }

    private void OnInit(object result, AjaxRequest request)
    {
        JSONNode jSONNode = JSON.Parse(result.ToString());
        if (jSONNode["result"] != (object)null && jSONNode["result"].AsBool)
        {
            if (jSONNode["week"] != (object)null)
            {
                WeekItemsManager.Init(jSONNode["week"]);
            }
            foreach (JSONNode child in jSONNode["assemblage"].Childs)
            {
                this.assemblages.Add(new Assemblage(child));
            }
            if (AssemblageManager.OnLoad != null)
            {
                AssemblageManager.OnLoad(result);
            }
            return;
        }
        if (AssemblageManager.OnError != null)
        {
            AssemblageManager.OnError(result);
            return;
        }
        throw new Exception("[AssemblageManager] OnInit not init: " + result);
    }

    public static Assemblage GetAssemblage(Wear wear)
    {
        List<Assemblage>.Enumerator enumerator = AssemblageManager.Instance.assemblages.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Assemblage current = enumerator.Current;
                if (current.Contain(wear))
                {
                    return current;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        return null;
    }

    public static Assemblage GetAssemblage(Weapon weapon)
    {
        List<Assemblage>.Enumerator enumerator = AssemblageManager.Instance.assemblages.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Assemblage current = enumerator.Current;
                if (current.Contain(weapon))
                {
                    return current;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        return null;
    }
}


