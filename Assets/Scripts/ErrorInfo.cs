// ILSpyBased#2
public class ErrorInfo
{
    public enum CODE
    {
        NONE,
        FRIEND_ROOM_CONNECT = 0x400,
        LIST_ROOM_IS_EMPTY,
        SERVER_DISCONNECTED,
        SERVER_CONNECTION_ERROR,
        ROOM_IS_FULL,
        SERVERS_IS_DOWN,
        SERVER_NOT_RESPOND,
        SERVER_CONNECTION_LOST_ERROR_105,
        SERVER_HIGH_PING,
        SERVER_AFK,
        SERVER_MOVE_CHEATING,
        SERVER_TIME_CHEATING,
        SERVER_INVALID_PASSWORD,
        SERVER_MAP_NOT_ALLOWED,
        SERVER_NOT_ALLOWED,
        NO_AVAILABLE_SERVERS,
        SERVER_PLAYER_KICKED,
        SYSTEM_ERROR = 100,
        MISSING_MONEY,
        MISSING_MONEY_TREASURY,
        GAME_MODE_LOW_LEVEL = 400,
        USER_NAME_INVALID = 300,
        USER_NAME_LEN,
        USER_NAME_EXISTS,
        CLAN_NAME = 350,
        CLAN_NAME_LEN,
        CLAN_NAME_EXIST,
        CLAN_TAG,
        CLAN_TAG_LEN,
        CLAN_TAG_EXIST,
        CLAN_USER_LVL_LESS,
        CLAN_CREATE_YOU_ARE_IN_CLAN,
        CLAN_MEMBER_MAX_COUNT,
        CLAN_URL,
        CLAN_DESC,
        CLAN_ACCESS_DISABLE
    }

    public enum TYPE
    {
        NONE,
        BUY_WEAR,
        BUY_WEAPON,
        BUY_ABILITY,
        SERVER_CONNECT,
        BUY_STAT_CLEAR,
        BUY_OTHER,
        CLAN_JOIN
    }
}


