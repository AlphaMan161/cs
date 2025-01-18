// ILSpyBased#2
public class MapMode
{
    public enum MODE : byte
    {
        NONE,
        DEATHMATCH,
        TEAM_DEATHMATCH,
        CAPTURE_THE_FLAG = 4,
        CONTROL_POINTS = 8,
        TOWER_DEFENSE = 0x10,
        ESCORT = 0x20,
        ZOMBIE = 0x40
    }
}


