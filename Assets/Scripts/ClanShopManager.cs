// dnSpy decompiler from Assembly-CSharp.dll class: ClanShopManager
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ClanShopManager
{
	private static ClanShopManager Instance
	{
		get
		{
			if (ClanShopManager.hInstance == null)
			{
				object sync = ClanShopManager._sync;
				lock (sync)
				{
					if (ClanShopManager.hInstance == null)
					{
						ClanShopManager.hInstance = new ClanShopManager();
					}
				}
			}
			return ClanShopManager.hInstance;
		}
	}

	public static List<Enhancer> Enhancers
	{
		get
		{
			object obj = ClanShopManager.Instance.lockEnhancer;
			List<Enhancer> result;
			lock (obj)
			{
				result = ClanShopManager.Instance.enhancers;
			}
			return result;
		}
	}

	public static void AddClanEnhancer(Enhancer enhancer)
	{
		Enhancer enhancer2 = ClanShopManager.Instance.enhancers.Find((Enhancer x) => x.EnhancerID == enhancer.EnhancerID);
		if (enhancer2 != null)
		{
			return;
		}
		object obj = ClanShopManager.Instance.lockEnhancer;
		lock (obj)
		{
			ClanShopManager.Instance.enhancers.Add(enhancer);
		}
	}

	public static void BuyEnhancer(Enhancer enhancer)
	{
		AjaxRequest ajaxRequest = new AjaxRequest(string.Format("{0}&cid={1}&id={2}&dur={3}", new object[]
		{
			WebUrls.CLAN_BUY_ENHANCER_URL,
			LocalUser.Clan.ClanID,
			enhancer.EnhancerID,
			(int)enhancer.Shop_Cost.SelectedDuration
		}), enhancer);
		ajaxRequest.OnComplete += ClanShopManager.Instance.OnBuyEnhancer;
		Ajax.Request(ajaxRequest);
	}

	private void OnBuyEnhancer(object result, AjaxRequest request)
	{
		JSONNode jsonnode = JSON.Parse(result.ToString());
		if (jsonnode["result"] != null && jsonnode["result"].AsBool)
		{
			Enhancer enhancer = request.Tag as Enhancer;
			if (MasterServerNetworkController.Instance != null && MasterServerNetworkController.Connected)
			{
				Hashtable hashtable = new Hashtable();
				hashtable[0] = (int)enhancer.EnhancerID;
				hashtable[1] = (int)enhancer.Shop_Cost.SelectedDuration;
				MasterServerNetworkController.SendClanEvent(ClanEventCode.BuyEnhancer, LocalUser.Clan.ClanID, hashtable);
			}
			else
			{
				ClanManager.OnChangeEnhancer(LocalUser.Clan.ClanID, (int)enhancer.EnhancerID, (int)enhancer.Shop_Cost.SelectedDuration);
			}
			WebCall.Analitic("BuyEnhancer", string.Format("C_{0}_{1}", enhancer.EnhancerID, enhancer.Shop_Cost.SelectedDuration), new object[0]);
		}
	}

	private static object _sync = new object();

	private static ClanShopManager hInstance = null;

	private object lockEnhancer = new object();

	private List<Enhancer> enhancers = new List<Enhancer>();
}
