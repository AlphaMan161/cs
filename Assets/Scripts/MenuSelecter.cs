// ILSpyBased#2
public class MenuSelecter
{
    public enum MainMenuEnum
    {
        Home = 1,
        Headquarters,
        Shop,
        Statistic,
        Comrads,
        Contracts,
        Fight,
        Clan
    }

    public enum HeadquaterMenuEnum
    {
        Appearance = 1,
        Weapon,
        Ability,
        Workshop,
        Taunt
    }

    public enum ShopMenuEnum
    {
        Appearance = 1,
        Weapon,
        Taunt,
        Enhancer
    }

    public enum ComradsMenuEnum
    {
        Friends = 1,
        Clan
    }

    public enum ClanMenuEnum
    {
        Create = 1,
        List,
        Hall
    }

    public enum ClanHallMenuEnum
    {
        Main = 1,
        Members,
        Edit,
        Invites,
        Events,
        Treasury,
        Enhancers
    }

    public enum RoomListMenuEnum
    {
        RoomList = 1,
        CreateGame,
        Maps,
        ServerList
    }

    public enum StatisticsMenuEnum
    {
        Achievement = 1,
        Rating,
        Weapon,
        Main
    }

    private static MenuSelecter hInstance;

    private MainMenuEnum mainMenuSelect = MainMenuEnum.Home;

    private HeadquaterMenuEnum headquaterMenuSelect = HeadquaterMenuEnum.Appearance;

    private ShopMenuEnum shopMenuSelect = ShopMenuEnum.Appearance;

    private ComradsMenuEnum comradsMenuSelect = ComradsMenuEnum.Friends;

    private ClanMenuEnum clanMenuSelect = ClanMenuEnum.Create;

    private ClanHallMenuEnum clanHallMenuSelect = ClanHallMenuEnum.Main;

    private RoomListMenuEnum roomListMenuSelect = RoomListMenuEnum.RoomList;

    private StatisticsMenuEnum statisticsMenuSelect = StatisticsMenuEnum.Achievement;

    private static MenuSelecter Instance
    {
        get
        {
            if (MenuSelecter.hInstance == null)
            {
                MenuSelecter.hInstance = new MenuSelecter();
            }
            return MenuSelecter.hInstance;
        }
    }

    public static MainMenuEnum MainMenuSelect
    {
        get
        {
            return MenuSelecter.Instance.mainMenuSelect;
        }
        set
        {
            if (MenuSelecter.Instance.mainMenuSelect != value)
            {
                if (MenuSelecter.Instance.mainMenuSelect == MainMenuEnum.Shop || MenuSelecter.Instance.mainMenuSelect == MainMenuEnum.Statistic)
                {
                    CharacterCameraManager.Instance.SetPlayerViewDefault(LocalUser.View);
                }
                if (value == MainMenuEnum.Statistic && StatisticManager.CurrentUser != null && StatisticManager.CurrentUser.View != null)
                {
                    CharacterCameraManager.Instance.SetPlayerViewOther(StatisticManager.CurrentUser.View);
                }
                else
                {
                    switch (value)
                    {
                        case MainMenuEnum.Shop:
                            GUIShop.IsInit = false;
                            break;
                        case MainMenuEnum.Headquarters:
                            GUIInventory.IsInit = false;
                            break;
                        case MainMenuEnum.Statistic:
                            GUIStatWeapon.IsInit = false;
                            break;
                        case MainMenuEnum.Fight:
                            if (ServersList.SelectServer != null && ServersList.SelectServer.IsConnected)
                            {
                                break;
                            }
                            if (ServersList.ServerList.Count == 1)
                            {
                                ServersList.Connect(ServersList.ServerList[0]);
                            }
                            break;
                    }
                }
            }
            MenuSelecter.Instance.mainMenuSelect = value;
        }
    }

    public static HeadquaterMenuEnum HeadquaterMenuSelect
    {
        get
        {
            return MenuSelecter.Instance.headquaterMenuSelect;
        }
        set
        {
            MenuSelecter.Instance.headquaterMenuSelect = value;
        }
    }

    public static ShopMenuEnum ShopMenuSelect
    {
        get
        {
            return MenuSelecter.Instance.shopMenuSelect;
        }
        set
        {
            MenuSelecter.Instance.shopMenuSelect = value;
        }
    }

    public static ComradsMenuEnum ComradsMenuSelect
    {
        get
        {
            return MenuSelecter.Instance.comradsMenuSelect;
        }
        set
        {
            MenuSelecter.Instance.comradsMenuSelect = value;
        }
    }

    public static ClanMenuEnum ClanMenuSelect
    {
        get
        {
            return MenuSelecter.Instance.clanMenuSelect;
        }
        set
        {
            MenuSelecter.Instance.clanMenuSelect = value;
        }
    }

    public static ClanHallMenuEnum ClanHallMenuSelect
    {
        get
        {
            return MenuSelecter.Instance.clanHallMenuSelect;
        }
        set
        {
            MenuSelecter.Instance.clanHallMenuSelect = value;
        }
    }

    public static RoomListMenuEnum RoomListMenuSelect
    {
        get
        {
            return MenuSelecter.Instance.roomListMenuSelect;
        }
        set
        {
            if (MenuSelecter.Instance.roomListMenuSelect != RoomListMenuEnum.ServerList && value == RoomListMenuEnum.ServerList)
            {
                MasterServerMonitor.Instance.Reset();
            }
            MenuSelecter.Instance.roomListMenuSelect = value;
        }
    }

    public static StatisticsMenuEnum StatisticsMenuSelect
    {
        get
        {
            return MenuSelecter.Instance.statisticsMenuSelect;
        }
        set
        {
            MenuSelecter.Instance.statisticsMenuSelect = value;
        }
    }
}


