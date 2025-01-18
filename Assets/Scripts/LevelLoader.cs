// ILSpyBased#2
public class LevelLoader
{
    private static AssetLoader mInstance;

    public static AssetLoader Loader
    {
        get
        {
            return LevelLoader.mInstance;
        }
        set
        {
            LevelLoader.mInstance = value;
        }
    }
}


