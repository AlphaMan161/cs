// dnSpy decompiler from Assembly-CSharp.dll class: GameLogicSaveController
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicSaveController
{
	private GameLogicSaveController()
	{
	}

	private static GameLogicSaveController Instance
	{
		get
		{
			if (GameLogicSaveController.hInstance == null)
			{
				object sync = GameLogicSaveController._sync;
				lock (sync)
				{
					if (GameLogicSaveController.hInstance == null)
					{
						GameLogicSaveController.hInstance = new GameLogicSaveController();
					}
				}
			}
			return GameLogicSaveController.hInstance;
		}
	}

	private bool _ProcessSave(Hashtable data)
	{
		if (data.ContainsKey(1))
		{
			UnityEngine.Debug.LogError(string.Format("Have FUFPSUserSaveKeys.MainStats:{0} type:{1}", data[1], data[1].GetType()));
			Dictionary<byte, object> dictionary = data[1] as Dictionary<byte, object>;
			if (dictionary.ContainsKey(2))
			{
				UnityEngine.Debug.Log(string.Format("uMainStat[FUFPSUserSaveKeys.MainStat_Lvl] = {0} type:{1}", dictionary[2], dictionary[2].GetType()));
				short num = Convert.ToInt16(dictionary[2]);
				LocalUser.Level += num;
				if (num > 0)
				{
				}
			}
			if (dictionary.ContainsKey(3))
			{
				UnityEngine.Debug.Log(string.Format("uMainStat[FUFPSUserSaveKeys.MainStat_Exp] = {0} type:{1}", dictionary[3], dictionary[3].GetType()));
				LocalUser.Exp += Convert.ToInt32(dictionary[3]);
			}
			if (dictionary.ContainsKey(4))
			{
				UnityEngine.Debug.Log(string.Format("uMainStat[FUFPSUserSaveKeys.MainStat_Vcur] = {0} type:{1}", dictionary[4], dictionary[4].GetType()));
				LocalUser.Money += Convert.ToInt32(dictionary[4]);
			}
			if (dictionary.ContainsKey(5))
			{
				UnityEngine.Debug.Log(string.Format("uMainStat[FUFPSUserSaveKeys.MainStat_Rcur] = {0} type:{1}", dictionary[5], dictionary[5].GetType()));
			}
			if (dictionary.ContainsKey(6))
			{
				UnityEngine.Debug.Log(string.Format("uMainStat[FUFPSUserSaveKeys.MainStat_PVPcur] = {0} type:{1}", dictionary[6], dictionary[6].GetType()));
			}
			if (dictionary.ContainsKey(7))
			{
				UnityEngine.Debug.Log(string.Format("uMainStat[FUFPSUserSaveKeys.MainStat_Room] = {0} type:{1}", dictionary[7], dictionary[7].GetType()));
			}
		}
		if (data.ContainsKey(10))
		{
			StatisticManager.UpdateWeaponLocal(data[10] as ArrayList);
			UnityEngine.Debug.Log(string.Format("Have FUFPSUserSaveKeys.StatWeaponObjList:{0} type:{1}", data[10], data[10].GetType()));
		}
		if (data.ContainsKey(11))
		{
			StatisticManager.UpdatePlayerLocal(data[11] as Dictionary<string, object>);
			UnityEngine.Debug.Log(string.Format("Have FUFPSUserSaveKeys.StatPlayerTotal:{0} type:{1}", data[11], data[11].GetType()));
		}
		if (data.ContainsKey(12))
		{
			StatisticManager.UpdateGameModeLocal(data[12] as ArrayList);
			UnityEngine.Debug.Log(string.Format("Have FUFPSUserSaveKeys.StatGameModeObjList:{0} type:{1}", data[12], data[12].GetType()));
		}
		if (data.ContainsKey(13))
		{
			StatisticManager.UpdateMapLocal(data[13] as ArrayList);
			UnityEngine.Debug.Log(string.Format("Have FUFPSUserSaveKeys.StatMapObjList2Save:{0} type:{1}", data[13], data[13].GetType()));
		}
		if (data.ContainsKey(30))
		{
			UnityEngine.Debug.Log(string.Format("Have FUFPSUserSaveKeys.uStatRating:{0} type:{1}", data[30], data[30].GetType()));
			LeagueManager.Instance.AddLocalData(data[30] as Dictionary<byte, object>);
		}
		if (data.ContainsKey(40))
		{
			UnityEngine.Debug.LogError(string.Format("Have FUFPSUserSaveKeys.uClan:{0} type:{1}", data[40], data[40].GetType()));
			Dictionary<byte, object> dictionary2 = data[40] as Dictionary<byte, object>;
			if (dictionary2.ContainsKey(42))
			{
				UnityEngine.Debug.LogError(string.Format("uClanExp[FUFPSUserSaveKeys.uClan_Exp] = {0} type:{1}", dictionary2[42], dictionary2[42].GetType()));
			}
		}
		if (data.ContainsKey(50))
		{
			Hashtable input = data[50] as Hashtable;
			AchievementManager.Instance.Update(input);
			UnityEngine.Debug.Log(string.Format("Have FUFPSUserSaveKeys.Achievement:{0} type:{1}", data[50], data[50].GetType()));
		}
		return true;
	}

	public static bool ProcessSave(Hashtable data)
	{
		return GameLogicSaveController.Instance._ProcessSave(data);
	}

	private static object _sync = new object();

	private static GameLogicSaveController hInstance = null;
}
