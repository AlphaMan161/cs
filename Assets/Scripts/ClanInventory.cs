// dnSpy decompiler from Assembly-CSharp.dll class: ClanInventory
using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class ClanInventory
{
	public ClanInventory(JSONNode json)
	{
		this.Refresh(json);
	}

	public List<Enhancer> Enhancers
	{
		get
		{
			object obj = this.lockEnhancer;
			List<Enhancer> result;
			lock (obj)
			{
				result = this.enhancers;
			}
			return result;
		}
	}

	public void Refresh(JSONNode json)
	{
		object obj = this.lockEnhancer;
		lock (obj)
		{
			foreach (JSONNode jsonnode in json.Childs)
			{
				CCItemType asInt = (CCItemType)jsonnode["it"].AsInt;
				if (asInt == CCItemType.ENHANCER)
				{
					Enhancer enhancer = new Enhancer(jsonnode);
					if (enhancer.Duration.Min <= 1u)
					{
						continue;
					}
					this.enhancers.Add(enhancer);
					Enhancer enhancer2 = ClanShopManager.Enhancers.Find((Enhancer x) => x.EnhancerID == enhancer.EnhancerID);
					if (enhancer2 != null)
					{
						enhancer2.Duration = enhancer.Duration;
						enhancer2.IsBuyed = true;
					}
				}
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"ClanInventory: ",
					jsonnode.ToString(),
					" TYPE: ",
					asInt
				}));
			}
		}
	}

	private object lockEnhancer = new object();

	private List<Enhancer> enhancers = new List<Enhancer>();
}
