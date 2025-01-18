// dnSpy decompiler from Assembly-CSharp.dll class: WebUrls
using System;

public static class WebUrls
{
	public static string MAIN_SERVER
	{
		get
		{
			if (WebUrls.mainServer == string.Empty)
			{
				WebUrls.mainServer = "http://127.0.0.1:5000/ajax.php?";
			/*	WebUrls.mainServer = "https://od-contra.pentagames.net/ajax.php?";
				string text = "https://";
				if (!Configuration.EnableSSL)
				{
					text = "http://";
				}
				if (Configuration.SType == ServerType.DEBUG_LOCAL)
				{
					WebUrls.mainServer = text + "www.cc.com.ua/ajax.php?";
				}
				else if (Configuration.SType == ServerType.DEV_LOCAL)
				{
					WebUrls.mainServer = text + "www.cc.dev/ajax.php?";
				}
				else if (Configuration.SType == ServerType.VK)
				{
					WebUrls.mainServer = text + "127.0.0.1:5000/ajax.php?";
				}
				else if (Configuration.SType == ServerType.OD)
				{
					WebUrls.mainServer = text + "od-contra.pentagames.net/ajax.php?";
				}
				else if (Configuration.SType == ServerType.MM)
				{
					WebUrls.mainServer = text + "mm-contra.pentagames.net/ajax.php?";
				}
				else if (Configuration.SType == ServerType.FACEBOOK)
				{
					WebUrls.mainServer = text + "fb-contra.pentagames.net/ajax.php?";
				}
				else if (Configuration.SType == ServerType.KONGREGATE)
				{
					WebUrls.mainServer = text + "kg-contra.pentagames.net/ajax.php?";
				}
				else if (Configuration.SType == ServerType.DEV)
				{
					WebUrls.mainServer = text + "dev.contra-city.com/ajax.php?";
				} */
			}
			return WebUrls.mainServer;
		}
	}

	private static string SESSION_AUTH
	{
		get
        {      //https://vk-contra.pentagames.net/ajax.php?ccid=10457382&cckey=242982ba7c7e79ed089496c8a3fd2d34&page=auth&act=g
            //https://vk-contra.pentagames.net/ajax.php?ccid=10457382&cckey=58e9459091a41884152f441cf2e585d0&page=auth&act=g 
            //https://od-contra.pentagames.net/ajax.php?ccid=10194748&cckey=dc6bf36c3c7ee1707cf07a847d1679f3&page=auth&act=g 
			//return "ccid=7463776&cckey=7972b586ca51072c9136fd42ea888500&";
            return Configuration.SessionAuth;
		}
	}

	public static string LOGIN_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=account&action=login";
		}
	}

	public static string AUTH_CHECK_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=auth&act=g";
		}
	}

	public static string USER_INFO_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=i&ai=1";
		}
	}

	public static string USER_INFO_EXTENDED_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=i&v=1&w=1&t=1";
		}
	}

	public static string USER_INFO_VIEW_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=i&v=1&w=1";
		}
	}

	public static string USER_INVENTORY
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=inv";
		}
	}

	public static string USER_INFO_UID
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=uid";
		}
	}

	public static string USER_SET_VIEW_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=sview";
		}
	}

	public static string USER_SET_WEAPON_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=sweap";
		}
	}

	public static string USER_SET_TAUNT_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=staunt";
		}
	}

	public static string ASSEMBLAGE_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=shop&act=assemb";
		}
	}

	public static string SHOP_WEAR_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=shop&act=wear";
		}
	}

	public static string SHOP_WEAPON_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=shop&act=weap";
		}
	}

	public static string SHOP_ITEMS_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=shop&act=items";
		}
	}

	public static string BUY_WEAR_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=buy&act=bwear";
		}
	}

	public static string BUY_WEAPON_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=buy&act=bweap";
		}
	}

	public static string BUY_TAUNT_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=buy&act=btaunt";
		}
	}

	public static string BUY_ENHANCER_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=buy&act=benh";
		}
	}

	public static string BUY_WEAPON_UPGRADE_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=buy&act=bweapupg";
		}
	}

	public static string GET_WEAPON_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=shop&act=weapinf";
		}
	}

	public static string ABILITY_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=abil";
		}
	}

	public static string BUY_ABILITY_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=buy&act=babil";
		}
	}

	public static string EVENT_CONFIRM
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=cev";
		}
	}

	public static string MAP_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=map";
		}
	}

	public static string MAP_TRY_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=tmap";
		}
	}

	public static string MAP_BUY_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=buy&act=bmap";
		}
	}

	public static string ACHIEVEMENT_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=ach";
		}
	}

	public static string ACHIEVEMENT_LITE_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=pl&act=ach&lite=1";
		}
	}

	public static string ACTION_IMAGE_URL
	{
		get
		{
			return "https://cs317017.userapi.com/v317017793/6359/EFAwm2PAZxI.jpg";
		}
	}

	public static string TOP_RATING_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=stats&act=rat";
		}
	}

	public static string TOP_RATING_LEAGUE_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=stats&act=league";
		}
	}

	public static string TOP_YESTERDAY_BEST_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=stats&act=ybest";
		}
	}

	public static string CHECK_NAME_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=account&action=cname";
		}
	}

	public static string CHANGE_NAME_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=account&action=cname&set=1";
		}
	}

	public static string CHANGE_NAME_PAYED_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=account&action=cpname";
		}
	}

	public static string CHARACTER_URL
	{
		get
		{
            return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=assets";
        }
	}

	public static string SEARCH_PLAYER_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=account&action=searcname";
		}
	}

	public static string ACTION_BANNERS_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=shop&act=act";
		}
	}

	public static string STATISTIC_CLEAR_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=stats&act=reset";
		}
	}

	public static string CLAN_GET_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=g";
		}
	}

	public static string CLAN_SEARCH_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=src";
		}
	}

	public static string CLAN_GET_EXTRA_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=gextra";
		}
	}

	public static string CLAN_MEMBERS_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=m";
		}
	}

	public static string CLAN_INIVTES_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=inv";
		}
	}

	public static string CLAN_CREATE_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=create";
		}
	}

	public static string CLAN_DELETE_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=del";
		}
	}

	public static string CLAN_JOIN_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=join";
		}
	}

	public static string CLAN_ACCEPT_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=accept";
		}
	}

	public static string CLAN_REJECT_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=reject";
		}
	}

	public static string CLAN_BUY_REQUEST_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=buyReq";
		}
	}

	public static string CLAN_EXPAND_MEMBER_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=expand";
		}
	}

	public static string CLAN_REMOVE_MEMBER_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=remove";
		}
	}

	public static string CLAN_BUY_ENHANCER_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=bench";
		}
	}

	public static string CLAN_LEAVE_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=leave";
		}
	}

	public static string CLAN_ARMS_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=arms";
		}
	}

	public static string CLAN_CHANGE_NAME_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=cname";
		}
	}

	public static string CLAN_CHANGE_TAG_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=ctag";
		}
	}

	public static string CLAN_CHANGE_ARM_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=carm";
		}
	}

	public static string CLAN_CHANGE_PAGE_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=curl";
		}
	}

	public static string CLAN_CHANGE_DESC_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=cdesc";
		}
	}

	public static string CLAN_CHANGE_OWNER_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=cowner";
		}
	}

	public static string CLAN_CHANGE_ACCESS_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=caccess";
		}
	}

	public static string CLAN_CHANGE_ACCESS_LVL_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=caccesslvl";
		}
	}

	public static string CLAN_ADD_MONEY_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=amoney";
		}
	}

	public static string CLAN_CHANGE_KOEF_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=ckoef";
		}
	}

	public static string CLAN_GET_EVENT_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=gevnt";
		}
	}

	public static string CLAN_DEL_EVENT_URL
	{
		get
		{
			return WebUrls.MAIN_SERVER + WebUrls.SESSION_AUTH + "page=clan&act=delevnt";
		}
	}

	public static string GetMapLoadingUrl(string map_name)
	{
		return map_name + ".unity3d" + WebUrls.VERSION_MAP;
	}

	private static string mainServer = string.Empty;

	private static string VERSION_CHARACTER = "?20180717";

	private static string VERSION_MAP = "?20180726";

	public static int VERSION_TEXTURES = 1;
}
