// dnSpy decompiler from Assembly-CSharp.dll class: StatisticManager
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticManager
{
	public StatisticManager()
	{
		this.userList = new Dictionary<int, UserRatingAdvanced>();
		this.userList.Add(LocalUser.UserID, new UserRatingAdvanced());
		AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.USER_INFO_VIEW_URL + "&ui=" + LocalUser.UserID, LocalUser.UserID);
		ajaxRequest.OnComplete += this.OnLoad;
		Ajax.Request(ajaxRequest);
	}

	public static event StatisticManager.StatisticEventHandler OnChange;

	private static StatisticManager Instance
	{
		get
		{
			if (StatisticManager.hInstance == null)
			{
				StatisticManager.hInstance = new StatisticManager();
			}
			return StatisticManager.hInstance;
		}
	}

	public static UserRatingAdvanced CurrentUser
	{
		get
		{
			return StatisticManager.Instance.currentUser;
		}
	}

	public static bool NeedLiteRefresh
	{
		get
		{
			return StatisticManager.Instance.needLiteRefresh;
		}
		set
		{
			StatisticManager.Instance.needLiteRefresh = value;
		}
	}

	public static void Refresh()
	{
		if (StatisticManager.Instance.currentUser == null)
		{
			return;
		}
		AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.USER_INFO_VIEW_URL + "&ui=" + LocalUser.UserID, LocalUser.UserID);
		ajaxRequest.OnComplete += StatisticManager.Instance.OnLoad;
		Ajax.Request(ajaxRequest);
	}

	private void OnLoad(object result, AjaxRequest request)
	{
		JSONObject jsonobject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
		if (jsonobject.GetField("result").type == JSONObject.Type.BOOL && jsonobject.GetField("result").b)
		{
			int num;
			if (request.Tag.GetType() == typeof(int))
			{
				num = Convert.ToInt32(request.Tag);
				this.userList[num].Update(jsonobject, LocalUser.UserID == num);
			}
			else
			{
				UserRatingAdvanced userRatingAdvanced = new UserRatingAdvanced(jsonobject, false);
				num = userRatingAdvanced.UserID;
				this.userList.Add(num, userRatingAdvanced);
				string key = request.Tag.ToString();
				this.socialUidList.Add(key, num);
			}
			this.currentUser = this.userList[num];
			if (num == LocalUser.UserID)
			{
				this.localUser = this.userList[num];
			}
			CharacterCameraManager.Instance.SetPlayerViewOther(this.currentUser.View);
			if (StatisticManager.OnChange != null)
			{
				StatisticManager.OnChange(this.currentUser);
			}
			return;
		}
		throw new Exception("LocalUser not init: " + result);
	}

	public static void SetLocal()
	{
		StatisticManager.Instance.currentUser = StatisticManager.Instance.localUser;
		CharacterCameraManager.Instance.SetPlayerViewOther(StatisticManager.Instance.currentUser.View);
		if (StatisticManager.OnChange != null)
		{
			StatisticManager.OnChange(StatisticManager.Instance.currentUser);
		}
	}

	public static void View(string uid, ServerType st)
	{
		if (StatisticManager.Instance.socialUidList.ContainsKey(uid))
		{
			StatisticManager.View(StatisticManager.Instance.socialUidList[uid]);
		}
		else
		{
			if (MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Statistic)
			{
				MenuSelecter.MainMenuSelect = MenuSelecter.MainMenuEnum.Statistic;
			}
			if (MenuSelecter.StatisticsMenuSelect != MenuSelecter.StatisticsMenuEnum.Main && MenuSelecter.StatisticsMenuSelect != MenuSelecter.StatisticsMenuEnum.Weapon)
			{
				MenuSelecter.StatisticsMenuSelect = MenuSelecter.StatisticsMenuEnum.Main;
			}
			AjaxRequest ajaxRequest = new AjaxRequest(string.Concat(new string[]
			{
				WebUrls.USER_INFO_VIEW_URL,
				"&uid=",
				uid,
				"&st=",
				st.ToString()
			}), uid);
			ajaxRequest.OnComplete += StatisticManager.Instance.OnLoad;
			Ajax.Request(ajaxRequest);
		}
	}

	public static void View(int user_id)
	{
		if (MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Statistic)
		{
			MenuSelecter.MainMenuSelect = MenuSelecter.MainMenuEnum.Statistic;
		}
		if (MenuSelecter.StatisticsMenuSelect != MenuSelecter.StatisticsMenuEnum.Main && MenuSelecter.StatisticsMenuSelect != MenuSelecter.StatisticsMenuEnum.Weapon)
		{
			MenuSelecter.StatisticsMenuSelect = MenuSelecter.StatisticsMenuEnum.Main;
		}
		if (StatisticManager.Instance.userList.ContainsKey(user_id))
		{
			StatisticManager.Instance.currentUser = StatisticManager.Instance.userList[user_id];
			CharacterCameraManager.Instance.SetPlayerViewOther(StatisticManager.Instance.currentUser.View);
			if (StatisticManager.OnChange != null)
			{
				StatisticManager.OnChange(StatisticManager.Instance.currentUser);
			}
		}
		else
		{
			StatisticManager.Instance.userList.Add(user_id, new UserRatingAdvanced());
			AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.USER_INFO_VIEW_URL + "&ui=" + user_id, user_id);
			ajaxRequest.OnComplete += StatisticManager.Instance.OnLoad;
			Ajax.Request(ajaxRequest);
		}
	}

	public static void ClearStatistic(int type)
	{
		if (30 > LocalUser.Money)
		{
			ErrorInfo.CODE code = ErrorInfo.CODE.MISSING_MONEY;
			code.AddNotification(ErrorInfo.TYPE.BUY_STAT_CLEAR);
			return;
		}
		string text = LanguageManager.GetText("Do you really want to reset the weapon statistics?\\nThis action can't be cancelled.");
		if (type == 2)
		{
			text = LanguageManager.GetText("Do you really want to reset the character statistics?\\nThis action can't be cancelled.");
		}
		else if (type == 3)
		{
			text = LanguageManager.GetText("Do you really want to reset game modes statistics?\\nThis action can't be cancelled.");
		}
		else if (type == 4)
		{
			text = LanguageManager.GetText("Do you really want to reset maps statistics?\\nThis action can't be cancelled.");
		}
		NotificationWindow.Add(new Notification(Notification.Type.NOTIFICATION, LanguageManager.GetText("Statistics reset"), text, LanguageManager.GetText("Reset"), new Notification.ButtonClick(StatisticManager.OnClearStatisticConfirmed), type));
	}

	private static void OnClearStatisticConfirmed(object type)
	{
		AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.STATISTIC_CLEAR_URL + "&t=" + type, type);
		ajaxRequest.OnComplete += StatisticManager.Instance.OnClearStatistic;
		Ajax.Request(ajaxRequest);
	}

	private void OnClearStatistic(object result, AjaxRequest request)
	{
		UnityEngine.Debug.LogError("[StatisticManager] OnClearStatistic: " + result);
		JSONObject jsonobject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
		if (jsonobject.GetField("result").type == JSONObject.Type.BOOL && jsonobject.GetField("result").b)
		{
			LocalUser.Money -= 30;
			StatisticManager.Refresh();
			string data = null;
			if (jsonobject.GetField("req") != null)
			{
				data = jsonobject.GetField("req").str;
			}
			GameLogicServerNetworkController.SendChange(12, data);
			return;
		}
		throw new Exception("LocalUser not init: " + result);
	}

	public static void UpdateWeaponLocal(ArrayList data)
	{
		if (StatisticManager.Instance.userList.ContainsKey(LocalUser.UserID))
		{
			StatisticManager.Instance.userList[LocalUser.UserID].AddWeaponStat(data);
		}
	}

	public static void UpdatePlayerLocal(Dictionary<string, object> data)
	{
		if (StatisticManager.Instance.userList.ContainsKey(LocalUser.UserID))
		{
			StatisticManager.Instance.userList[LocalUser.UserID].AddPlayerStat(data);
		}
	}

	public static void UpdateGameModeLocal(ArrayList data)
	{
		if (StatisticManager.Instance.userList.ContainsKey(LocalUser.UserID))
		{
			StatisticManager.Instance.userList[LocalUser.UserID].AddGameModeStat(data);
		}
	}

	public static void UpdateMapLocal(ArrayList data)
	{
		if (StatisticManager.Instance.userList.ContainsKey(LocalUser.UserID))
		{
			StatisticManager.Instance.userList[LocalUser.UserID].AddMapStat(data);
		}
	}

	private static StatisticManager hInstance;

	private UserRatingAdvanced localUser = new UserRatingAdvanced();

	private UserRatingAdvanced currentUser;

	private Dictionary<int, UserRatingAdvanced> userList = new Dictionary<int, UserRatingAdvanced>();

	private bool needLiteRefresh = true;

	private Dictionary<string, int> socialUidList = new Dictionary<string, int>();

	public delegate void StatisticEventHandler(object sender);
}
