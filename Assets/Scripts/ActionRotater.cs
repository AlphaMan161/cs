// dnSpy decompiler from Assembly-CSharp.dll class: ActionRotater
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionRotater
{
	private ActionRotater()
	{
		this.defaultAction = new global::Action(LanguageManager.GetText("inviteBanner"), LanguageManager.GetText("Invite"), new global::Action.ActionClick(WebCall.InviteFriend), null);
		this.action = this.defaultAction;
		this.defaultLoadingAction = new global::Action(LanguageManager.GetText("inviteBannerSmall"));
		this.loadingAction = this.defaultLoadingAction;
		if (ActionRotater.offerEnabled)
		{
			this.actionList.Add(new global::Action(LanguageManager.GetText("offerBanner"), LanguageManager.GetText("Click HERE!"), new global::Action.ActionClick(WebCall.OpenOfferWindow), null));
		}
	}

	public static event ActionRotater.ActionRotaterHandler OnLoad;

	public static event ActionRotater.ActionRotaterHandler OnError;

	private static ActionRotater Instance
	{
		get
		{
			if (ActionRotater.hInstance == null)
			{
				ActionRotater.hInstance = new ActionRotater();
			}
			return ActionRotater.hInstance;
		}
	}

	public static global::Action Action
	{
		get
		{
			if (Time.time > ActionRotater.nextRotateTime)
			{
				ActionRotater.Instance.Rotate();
			}
			if (ActionRotater.Instance.action != null && ActionRotater.Instance.action.IsLoaded)
			{
				return ActionRotater.Instance.action;
			}
			return ActionRotater.Instance.defaultAction;
		}
	}

	public static global::Action LoadingAction
	{
		get
		{
			if (Time.time > ActionRotater.nextRotateLoadingTime)
			{
				ActionRotater.Instance.RotateLoading();
			}
			if (ActionRotater.Instance.loadingAction != null && ActionRotater.Instance.loadingAction.IsLoaded)
			{
				return ActionRotater.Instance.loadingAction;
			}
			return ActionRotater.Instance.defaultLoadingAction;
		}
	}

	private bool Rotate()
	{
		if (this.actionList.Count <= 1)
		{
			if (this.actionList.Count == 1)
			{
				this.action = this.actionList[0];
			}
			else
			{
				this.action = this.defaultAction;
			}
			ActionRotater.nextRotateTime = float.MaxValue;
			return false;
		}
		global::Action action;
		do
		{
			action = this.actionList[UnityEngine.Random.Range(0, this.actionList.Count)];
		}
		while (action == this.action);
		if (action.IsLoaded)
		{
			this.action = action;
		}
		ActionRotater.nextRotateTime = Time.time + ActionRotater.RotateOffset;
		return true;
	}

	private bool RotateLoading()
	{
		if (this.loadingActionList.Count <= 1)
		{
			if (this.loadingActionList.Count == 1)
			{
				this.loadingAction = this.loadingActionList[0];
			}
			else
			{
				this.loadingAction = this.defaultLoadingAction;
			}
			ActionRotater.nextRotateLoadingTime = float.MaxValue;
			return false;
		}
		global::Action action = this.loadingActionList[UnityEngine.Random.Range(0, this.loadingActionList.Count)];
		if (action.IsLoaded)
		{
			this.loadingAction = action;
		}
		ActionRotater.nextRotateLoadingTime = Time.time + ActionRotater.RotateOffset;
		return true;
	}

	public static void HandleOnChangeLang()
	{
		ActionRotater.Instance.defaultAction = new global::Action(LanguageManager.GetText("inviteBanner"), LanguageManager.GetText("Invite"), new global::Action.ActionClick(WebCall.InviteFriend), null);
		ActionRotater.Instance.Rotate();
	}

	public static void Init()
	{
		Ajax.Request(WebUrls.ACTION_BANNERS_URL, new AjaxRequest.AjaxHandler(ActionRotater.Instance.OnInit));
	}

	private void OnInit(object result, AjaxRequest request)
	{
		this.actionList = new List<global::Action>();
		this.loadingActionList = new List<global::Action>();
		if (result == null)
		{
			return;
		}
		JSONObject jsonobject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
		this.isLoaded = true;
		if (jsonobject.GetField("result").type == JSONObject.Type.BOOL && jsonobject.GetField("result").b)
		{
			if (jsonobject.GetField("bp") != null)
			{
				BankActionPackageManager.Init(jsonobject.GetField("bp"));
			}
			JSONObject field = jsonobject.GetField("a");
			if (field != null && field.type == JSONObject.Type.ARRAY)
			{
				for (int i = 0; i < field.Count; i++)
				{
					if (field[i].type != JSONObject.Type.NULL)
					{
						string str = field[i].GetField("t").str;
						string str2 = field[i].GetField("im").str;
						string str3 = field[i].GetField("ims").str;
						int num = Convert.ToInt32(field[i].GetField("ct").n);
						global::Action.ActionClick callback = null;
						object callbackParam = null;
						if (num == 1)
						{
							callback = new global::Action.ActionClick(WebCall.InviteFriend);
						}
						else if (num == 2)
						{
							callback = new global::Action.ActionClick(WebCall.BuyMoney);
						}
						else if (num == 3)
						{
							callback = new global::Action.ActionClick(WebCall.OpenUrl);
							if (field[i].GetField("cp") != null)
							{
								callbackParam = field[i].GetField("cp").str;
							}
						}
						if (str2 != string.Empty)
						{
							this.actionList.Add(new global::Action(str2, str, callback, callbackParam));
						}
						if (str3 != string.Empty)
						{
							this.loadingActionList.Add(new global::Action(str3));
						}
					}
				}
				this.Rotate();
				this.RotateLoading();
			}
			if (ActionRotater.OnLoad != null)
			{
				ActionRotater.OnLoad(this);
			}
		}
		else
		{
			if (ActionRotater.OnError == null)
			{
				throw new Exception("[ActionRotater] OnInit not init: " + result);
			}
			ActionRotater.OnError(result);
		}
	}

	public static void ReDownload()
	{
		if (!ActionRotater.Instance.defaultAction.IsLoaded)
		{
			ActionRotater.Instance.defaultAction.ReDownload();
		}
		if (!ActionRotater.Instance.defaultLoadingAction.IsLoaded)
		{
			ActionRotater.Instance.defaultLoadingAction.ReDownload();
		}
		foreach (global::Action action in ActionRotater.Instance.actionList)
		{
			if (!action.IsLoaded)
			{
				action.ReDownload();
			}
		}
		if (!ActionRotater.Instance.isLoaded)
		{
			ActionRotater.Init();
			return;
		}
	}

	public static void AddOfferAction()
	{
		if (ActionRotater.hInstance == null)
		{
			UnityEngine.Debug.Log("AddOffer");
			if (!ActionRotater.offerEnabled)
			{
				ActionRotater.Instance.actionList.Add(new global::Action(LanguageManager.GetText("offerBanner"), LanguageManager.GetText("Click HERE!"), new global::Action.ActionClick(WebCall.OpenOfferWindow), null));
			}
		}
		ActionRotater.offerEnabled = true;
	}

	public static bool OfferEnabled
	{
		get
		{
			return ActionRotater.offerEnabled;
		}
	}

	private static ActionRotater hInstance;

	private global::Action defaultAction;

	private global::Action action;

	private List<global::Action> actionList = new List<global::Action>();

	private List<global::Action> loadingActionList = new List<global::Action>();

	private global::Action defaultLoadingAction;

	private global::Action loadingAction;

	private bool isLoaded;

	private static float RotateOffset = 5f;

	private static float nextRotateTime;

	private static float nextRotateLoadingTime;

	private static bool offerEnabled;

	public delegate void ActionRotaterHandler(object sender);
}
