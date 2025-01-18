// dnSpy decompiler from Assembly-CSharp.dll class: MainMenu
using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnEnable()
	{
		UnityEngine.Debug.LogError("[MainMenu] Build: v0.1");
		if (LocalUser.NeedLiteRefresh)
		{
			LocalUser.RefreshLite();
		}
		LocalUser.NeedLiteRefresh = true;
		if (AchievementManager.NeedLiteRefresh)
		{
			AchievementManager.RefreshLite();
		}
		AchievementManager.NeedLiteRefresh = true;
		WebCall.Init();
		WebCall.Analitic("Loading", "MainMenu", new object[]
		{
			LocalUser.UserID
		});
		WebCall.GameAnalitic("loaded", 1);
		WebCall.CheckOffer();
		if (!(LocalUser.Name == string.Empty) && !(LocalUser.Name == string.Empty) && !(LocalUser.Name.Trim() == string.Empty))
		{
			MasterServerNetworkController.ConnectToMaster(this);
			GameLogicServerNetworkController.ConnectToGameLogic(this);
		}
		if (StatisticManager.NeedLiteRefresh)
		{
			StatisticManager.Refresh();
		}
		StatisticManager.NeedLiteRefresh = true;
		ActionRotater.ReDownload();
	}

	private void OnGUI()
	{
		DebugConsole.ShowDebug(Event.current);
		DurationManager.Instance.LateUpdate();
		if (MasterServerMonitor.Instance.Report)
		{
			return;
		}
		GUI.skin = GUISkinManager.Main;
		GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuRow01"), new GUILayoutOption[]
		{
			GUILayout.Width((float)Screen.width),
			GUILayout.Height(73f)
		});
		GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("logo"), new GUILayoutOption[]
		{
			GUILayout.Width(159f),
			GUILayout.Height(65f)
		});
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(LanguageManager.GetText("To BATTLE!"), GUISkinManager.Button.GetStyle("menuQuickBattle"), new GUILayoutOption[]
		{
			GUILayout.MinWidth(258f),
			GUILayout.Height(64f)
		}))
		{
			ServersList.ConnectQuick(this, string.Empty, MapMode.MODE.NONE);
		}
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(172f)
		});
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle(OptionsManager.SoundIsMute ? "menuSoundOff" : "menuSoundOn"), new GUILayoutOption[]
		{
			GUILayout.Width(32f),
			GUILayout.Height(32f)
		}))
		{
			OptionsManager.SoundIsMute = !OptionsManager.SoundIsMute;
		}
		GUILayout.Space(3f);
		if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("menuSetting"), new GUILayoutOption[]
		{
			GUILayout.Width(32f),
			GUILayout.Height(32f)
		}))
		{
			OptionPopup.Show = !OptionPopup.Show;
		}
		GUILayout.Space(3f);
		if (GUILayout.Button(GUIContent.none, GUISkinManager.Button.GetStyle("menuFullscreen"), new GUILayoutOption[]
		{
			GUILayout.Width(42f),
			GUILayout.Height(42f)
		}))
		{
			OptionsManager.FullScreen = !OptionsManager.FullScreen;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuRow02"), new GUILayoutOption[]
		{
			GUILayout.Width((float)Screen.width),
			GUILayout.Height(40f)
		});
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("menuRow02_inner"), new GUILayoutOption[]
		{
			GUILayout.Height(34f)
		});
		if (GUILayout.Button(LanguageManager.GetText("Home"), GUISkinManager.Button.GetStyle((MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Home) ? "menuRow02" : "menuRow02active"), new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		}))
		{
			MenuSelecter.MainMenuSelect = MenuSelecter.MainMenuEnum.Home;
		}
		GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("menuRow02"), new GUILayoutOption[]
		{
			GUILayout.Width(1f),
			GUILayout.Height(30f)
		});
		if (GUILayout.Button(LanguageManager.GetText("My HQ"), GUISkinManager.Button.GetStyle((MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Headquarters) ? "menuRow02" : "menuRow02active"), new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		}))
		{
			MenuSelecter.MainMenuSelect = MenuSelecter.MainMenuEnum.Headquarters;
		}
		GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("menuRow02"), new GUILayoutOption[]
		{
			GUILayout.Width(1f),
			GUILayout.Height(30f)
		});
		if (GUILayout.Button(LanguageManager.GetText("Shop"), GUISkinManager.Button.GetStyle((MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Shop) ? "menuRow02" : "menuRow02active"), new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		}))
		{
			MenuSelecter.MainMenuSelect = MenuSelecter.MainMenuEnum.Shop;
		}
		GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("menuRow02"), new GUILayoutOption[]
		{
			GUILayout.Width(1f),
			GUILayout.Height(30f)
		});
		if (GUILayout.Button(LanguageManager.GetText("Dossier"), GUISkinManager.Button.GetStyle((MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Statistic) ? "menuRow02" : "menuRow02active"), new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		}))
		{
			MenuSelecter.MainMenuSelect = MenuSelecter.MainMenuEnum.Statistic;
		}
		GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("menuRow02"), new GUILayoutOption[]
		{
			GUILayout.Width(1f),
			GUILayout.Height(30f)
		});
		GUIContent guicontent = new GUIContent(LanguageManager.GetText("Comrades"));
		if (((MasterServerNetworkController.Instance.FriendList != null && MasterServerNetworkController.Instance.FriendList.Request.Count > 0) || MasterServerNetworkController.NewPrivateMessages) && MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Comrads)
		{
			guicontent.image = (Texture)Resources.Load("GUI/Icons/Alert/message_alert01");
		}
		GUIContent guicontent2 = new GUIContent(LanguageManager.GetText("Clan"));
		if (ClanManager.NewRequests && MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Clan)
		{
			guicontent2.image = (Texture)Resources.Load("GUI/Icons/Alert/message_alert01");
		}
		GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("menuRow02"), new GUILayoutOption[]
		{
			GUILayout.Width(1f),
			GUILayout.Height(30f)
		});
		if (GUILayout.Button(guicontent2, GUISkinManager.Button.GetStyle((MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Clan) ? "menuRow02" : "menuRow02active"), new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		}))
		{
			MenuSelecter.MainMenuSelect = MenuSelecter.MainMenuEnum.Clan;
		}
		if (GUILayout.Button(guicontent, GUISkinManager.Button.GetStyle((MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Comrads) ? "menuRow02" : "menuRow02active"), new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		}))
		{
			MenuSelecter.MainMenuSelect = MenuSelecter.MainMenuEnum.Comrads;
		}
		GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("menuRow02"), new GUILayoutOption[]
		{
			GUILayout.Width(1f),
			GUILayout.Height(30f)
		});
		if (GUILayout.Button(LanguageManager.GetText("Battles"), GUISkinManager.Button.GetStyle((MenuSelecter.MainMenuSelect != MenuSelecter.MainMenuEnum.Fight) ? "menuRow02" : "menuRow02active"), new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		}))
		{
			MenuSelecter.MainMenuSelect = MenuSelecter.MainMenuEnum.Fight;
			ServersList.Refresh(MainNetworkController.Instance, false);
		}
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		if (MenuSelecter.MainMenuSelect == MenuSelecter.MainMenuEnum.Headquarters)
		{
			GUIInventory.OnGUI();
		}
		else if (MenuSelecter.MainMenuSelect == MenuSelecter.MainMenuEnum.Shop)
		{
			GUIShop.OnGUI();
		}
		else if (MenuSelecter.MainMenuSelect == MenuSelecter.MainMenuEnum.Fight)
		{
			GUIRoomList.OnGUI();
		}
		else if (MenuSelecter.MainMenuSelect == MenuSelecter.MainMenuEnum.Statistic)
		{
			GUIStatistic.OnGUI();
		}
		else if (MenuSelecter.MainMenuSelect == MenuSelecter.MainMenuEnum.Home)
		{
			GUIAction.OnGUI();
		}
		else if (MenuSelecter.MainMenuSelect == MenuSelecter.MainMenuEnum.Comrads)
		{
			GUIComrads.OnGUI();
		}
		else if (MenuSelecter.MainMenuSelect == MenuSelecter.MainMenuEnum.Clan)
		{
			GUIClan.OnGUI();
		}
		GUILayout.Space(13f);
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(760f)
		});
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("bottomLeft"), new GUILayoutOption[]
		{
			GUILayout.Width(327f),
			GUILayout.Height(54f)
		});
		GUILayout.Label("99", GUISkinManager.Label.GetStyle("progressLvlCur"), new GUILayoutOption[]
		{
			GUILayout.Width(38f),
			GUILayout.Height(38f)
		});
		GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), LocalUser.Level.ToString(), GUISkinManager.Text.GetStyle("progressLvlCur"), GUISkinManager.Text.GetStyle("progressLvlCurshadow"));
		GUILayout.Space(5f);
		GUIProgressBar.ProgressBar(212f, (float)LocalUser.MaxExp, (float)LocalUser.Exp);
		GUILayout.Space(5f);
		GUILayout.Label("99", GUISkinManager.Label.GetStyle("progressLvlNext"), new GUILayoutOption[]
		{
			GUILayout.Width(38f),
			GUILayout.Height(38f)
		});
		GUITextShadow.TextShadow(GUILayoutUtility.GetLastRect(), ((int)(LocalUser.Level + 1)).ToString(), GUISkinManager.Text.GetStyle("progressLvlNext"), GUISkinManager.Text.GetStyle("progressLvlNextshadow"));
		GUILayout.EndHorizontal();
		GUIHover.Hover(Event.current, LanguageManager.GetTextFormat("Exp: {0} / {1}", new object[]
		{
			LocalUser.Exp,
			LocalUser.MaxExp
		}), GUILayoutUtility.GetLastRect());
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(GUIContent.none, GUISkinManager.Backgound.GetStyle("bottomRight"), new GUILayoutOption[]
		{
			GUILayout.Width(310f),
			GUILayout.Height(68f)
		});
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(47f)
		});
		GUILayout.FlexibleSpace();
		GUILayout.Label(GUIContent.none, GUISkinManager.Ico.GetStyle("money"), new GUILayoutOption[]
		{
			GUILayout.Width(42f),
			GUILayout.Height(42f)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.Label(LocalUser.Money.ToString(), GUISkinManager.Label.GetStyle("money"), new GUILayoutOption[0]);
		GUILayout.Space(7f);
		GUILayout.Label(GUIContent.none, GUISkinManager.Separators.GetStyle("money"), new GUILayoutOption[]
		{
			GUILayout.Height(60f),
			GUILayout.Width(1f)
		});
		GUILayout.Space(7f);
		GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.MaxWidth(172f)
		});
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(LanguageManager.GetText("Recharge"), GUISkinManager.Button.GetStyle("green"), new GUILayoutOption[]
		{
			GUILayout.MinWidth(157f),
			GUILayout.Height(46f)
		}))
		{
			WebCall.BuyMoney(this);
		}
		Rect lastRect = GUILayoutUtility.GetLastRect();
		lastRect.width = 66f;
		lastRect.height = 29f;
		lastRect.x -= 1f;
		lastRect.y -= 1f;
		GUI.Label(lastRect, GUIContent.none, GUISkinManager.Label.GetStyle("action"));
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		if (LocalUser.Name == string.Empty || LocalUser.Name == string.Empty || LocalUser.Name.Trim() == string.Empty)
		{
			SetNamePopup.OnGUI();
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.F) && (UnityEngine.Input.GetKey(KeyCode.LeftAlt) || UnityEngine.Input.GetKey(KeyCode.RightAlt)))
		{
			OptionsManager.FullScreen = !OptionsManager.FullScreen;
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.ScreenShot))
		{
			ScreenshotManager.CreateAndUpload();
		}
	}
}
