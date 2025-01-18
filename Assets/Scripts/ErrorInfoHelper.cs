// ILSpyBased#2
public static class ErrorInfoHelper
{
    public static string GetDescription(this ErrorInfo.CODE code)
    {
        switch (code)
        {
            case ErrorInfo.CODE.GAME_MODE_LOW_LEVEL:
                return LanguageManager.GetText("This game mode becomes available at level 10.");
            case ErrorInfo.CODE.SYSTEM_ERROR:
                return LanguageManager.GetText("An unknown error occurred. Please, let our Support Team know about it.");
            case ErrorInfo.CODE.MISSING_MONEY:
                return LanguageManager.GetText("Not enough money");
            case ErrorInfo.CODE.MISSING_MONEY_TREASURY:
                return LanguageManager.GetText("Not enough money in treasury");
            case ErrorInfo.CODE.USER_NAME_INVALID:
                return LanguageManager.GetText("Please, choose another name.");
            case ErrorInfo.CODE.USER_NAME_LEN:
                return LanguageManager.GetText("Name must contain 3-16 characters.");
            case ErrorInfo.CODE.USER_NAME_EXISTS:
                return LanguageManager.GetText("This name is already in use by another comrade.");
            case ErrorInfo.CODE.FRIEND_ROOM_CONNECT:
                return LanguageManager.GetText("Unable to join the room.");
            case ErrorInfo.CODE.LIST_ROOM_IS_EMPTY:
                return LanguageManager.GetText("The battles list is empty. You may create new battle.");
            case ErrorInfo.CODE.SERVER_DISCONNECTED:
                return LanguageManager.GetText("Server doesn’t respond. Retry to connect a little bit later.");
            case ErrorInfo.CODE.SERVER_CONNECTION_ERROR:
                return LanguageManager.GetText("Server doesn’t respond. Retry to connect a little bit later.");
            case ErrorInfo.CODE.SERVER_INVALID_PASSWORD:
                return LanguageManager.GetText("Incorrect password");
            case ErrorInfo.CODE.SERVER_MAP_NOT_ALLOWED:
                return LanguageManager.GetText("This map not buyed");
            case ErrorInfo.CODE.SERVER_NOT_ALLOWED:
                return LanguageManager.GetText("No servers are available right now. Please retry to connect a little bit later.");
            case ErrorInfo.CODE.NO_AVAILABLE_SERVERS:
                return LanguageManager.GetText("This server is for low level players.");
            case ErrorInfo.CODE.ROOM_IS_FULL:
                return LanguageManager.GetText("The players’ quantity limit is reached for this room.");
            case ErrorInfo.CODE.SERVER_PLAYER_KICKED:
                return LanguageManager.GetText("Your have been kicked from this room.");
            case ErrorInfo.CODE.SERVERS_IS_DOWN:
                return LanguageManager.GetText("Server is not available right now. Please retry to connect a little bit later.");
            case ErrorInfo.CODE.SERVER_NOT_RESPOND:
                return LanguageManager.GetText("Unable to connect to the server. Please retry to connect a little bit later.");
            case ErrorInfo.CODE.SERVER_CONNECTION_LOST_ERROR_105:
                return LanguageManager.GetText("Data transfer problem. Connection interrupted.");
            case ErrorInfo.CODE.SERVER_HIGH_PING:
                return LanguageManager.GetText("Data transfer problem. Server respond delay is too high. Connection interrupted.");
            case ErrorInfo.CODE.SERVER_AFK:
                return LanguageManager.GetText("Data expectation timeout problem. Connection interrupted.");
            case ErrorInfo.CODE.SERVER_MOVE_CHEATING:
                return LanguageManager.GetText("Unknown connection error. Please, let our Support Team know about it.");
            case ErrorInfo.CODE.SERVER_TIME_CHEATING:
                return LanguageManager.GetText("Unknown joining error. Please let our Support Team know about it.");
            case ErrorInfo.CODE.CLAN_NAME:
                return LanguageManager.GetText("Clan name is invalid");
            case ErrorInfo.CODE.CLAN_NAME_LEN:
                return LanguageManager.GetText("Clan name should consist of 3 - 16 symbols");
            case ErrorInfo.CODE.CLAN_NAME_EXIST:
                return LanguageManager.GetText("This clan name is already existed");
            case ErrorInfo.CODE.CLAN_TAG:
                return LanguageManager.GetText("Clan tag is invalid");
            case ErrorInfo.CODE.CLAN_TAG_LEN:
                return LanguageManager.GetText("Clan tag should consist of 2 - 6 symbols");
            case ErrorInfo.CODE.CLAN_TAG_EXIST:
                return LanguageManager.GetText("This clan tag is already existed");
            case ErrorInfo.CODE.CLAN_USER_LVL_LESS:
                return LanguageManager.GetText("You must be on level 15");
            case ErrorInfo.CODE.CLAN_ACCESS_DISABLE:
                return LanguageManager.GetText("Selection is closed");
            case ErrorInfo.CODE.CLAN_CREATE_YOU_ARE_IN_CLAN:
                return LanguageManager.GetText("You can't create the clan as you are already in clan");
            case ErrorInfo.CODE.CLAN_MEMBER_MAX_COUNT:
                return LanguageManager.GetText("Cannot join because max count");
            case ErrorInfo.CODE.CLAN_URL:
                return LanguageManager.GetText("Clan homepage link is invalid");
            case ErrorInfo.CODE.CLAN_DESC:
                return LanguageManager.GetText("Clan description is invalid");
            default:
                return string.Empty;
        }
    }

    public static Notification AddNotification(this ErrorInfo.CODE code, ErrorInfo.TYPE type)
    {
        Notification notification = null;
        switch (code)
        {
            case ErrorInfo.CODE.CLAN_USER_LVL_LESS:
            case ErrorInfo.CODE.CLAN_ACCESS_DISABLE:
                notification = new Notification(Notification.Type.NOTIFICATION, LanguageManager.GetText("Attention!"), code.GetDescription());
                break;
            case ErrorInfo.CODE.MISSING_MONEY:
                notification = new Notification(Notification.Type.ERROR, LanguageManager.GetText("Cannot purchase"), code.GetDescription(), LanguageManager.GetText("Add more"), new Notification.ButtonClick(WebCall.BuyMoney), null);
                break;
            case ErrorInfo.CODE.MISSING_MONEY_TREASURY:
                notification = new Notification(Notification.Type.ERROR, LanguageManager.GetText("Cannot purchase"), code.GetDescription(), LanguageManager.GetText("Add more"), new Notification.ButtonClick(WebCall.BuyMoney), null);
                break;
            case ErrorInfo.CODE.GAME_MODE_LOW_LEVEL:
                notification = new Notification(Notification.Type.NOTIFICATION, LanguageManager.GetText("Attention!"), code.GetDescription());
                break;
            case ErrorInfo.CODE.FRIEND_ROOM_CONNECT:
                notification = new Notification(Notification.Type.NOTIFICATION, LanguageManager.GetText("Attention!"), code.GetDescription());
                break;
            case ErrorInfo.CODE.LIST_ROOM_IS_EMPTY:
                notification = new Notification(Notification.Type.NOTIFICATION, LanguageManager.GetText("Attention!"), code.GetDescription());
                break;
            case ErrorInfo.CODE.SERVER_DISCONNECTED:
                notification = new Notification(Notification.Type.ERROR, LanguageManager.GetText("Connection problem"), code.GetDescription());
                break;
            case ErrorInfo.CODE.SERVER_CONNECTION_ERROR:
                notification = new Notification(Notification.Type.ERROR, LanguageManager.GetText("Connection problem"), code.GetDescription());
                break;
            case ErrorInfo.CODE.SERVER_INVALID_PASSWORD:
            case ErrorInfo.CODE.SERVER_MAP_NOT_ALLOWED:
            case ErrorInfo.CODE.SERVER_NOT_ALLOWED:
            case ErrorInfo.CODE.NO_AVAILABLE_SERVERS:
            case ErrorInfo.CODE.SERVER_PLAYER_KICKED:
                notification = new Notification(Notification.Type.ERROR, LanguageManager.GetText("Connection problem"), code.GetDescription());
                break;
            case ErrorInfo.CODE.ROOM_IS_FULL:
                notification = new Notification(Notification.Type.ERROR, LanguageManager.GetText("The room is full"), code.GetDescription());
                break;
            case ErrorInfo.CODE.SERVERS_IS_DOWN:
                notification = new Notification(Notification.Type.ERROR, LanguageManager.GetText("Error"), code.GetDescription());
                break;
            case ErrorInfo.CODE.SERVER_NOT_RESPOND:
                notification = new Notification(Notification.Type.ERROR, LanguageManager.GetText("Server problem"), code.GetDescription());
                break;
            case ErrorInfo.CODE.SERVER_CONNECTION_LOST_ERROR_105:
                notification = new Notification(Notification.Type.CONNECTION_LOST, LanguageManager.GetText("Loss of connection"), code.GetDescription(), LanguageManager.GetText("My HQ"), null, null);
                break;
            case ErrorInfo.CODE.SERVER_HIGH_PING:
                notification = new Notification(Notification.Type.CONNECTION_LOST, LanguageManager.GetText("Loss of connection"), code.GetDescription(), LanguageManager.GetText("My HQ"), null, null);
                break;
            case ErrorInfo.CODE.SERVER_AFK:
                notification = new Notification(Notification.Type.CONNECTION_LOST, LanguageManager.GetText("Loss of connection"), code.GetDescription(), LanguageManager.GetText("My HQ"), null, null);
                break;
            case ErrorInfo.CODE.SERVER_MOVE_CHEATING:
                notification = new Notification(Notification.Type.CONNECTION_LOST, LanguageManager.GetText("Loss of connection"), code.GetDescription(), LanguageManager.GetText("My HQ"), null, null);
                break;
            case ErrorInfo.CODE.SERVER_TIME_CHEATING:
                notification = new Notification(Notification.Type.CONNECTION_LOST, LanguageManager.GetText("Loss of connection"), code.GetDescription(), LanguageManager.GetText("My HQ"), null, null);
                break;
        }
        if (notification != null)
        {
            NotificationWindow.Add(notification);
        }
        return notification;
    }
}


