using Toolbox;

namespace WizBulbApi;

public class Scene : Enumeration<Scene>
{
    public Scene(int index, string displayName, SceneCategory category) : base(index)
    {
        DisplayName = displayName;
        Category = category;
    }

    public int Index => _Index;
    public string DisplayName { get; }
    public SceneCategory Category { get; }

    //public static readonly Scene CustomWhite = new(-1, "Custom white", SceneCategory.White);
    public static readonly Scene Custom = new(0, "Custom colour", SceneCategory.Simple);
    public static readonly Scene Ocean = new(1, "Ocean", SceneCategory.Dynamic);
    public static readonly Scene Romance = new(2, "Romance", SceneCategory.Dynamic);
    public static readonly Scene Sunset = new(3, "Sunset", SceneCategory.Dynamic);
    public static readonly Scene Party = new(4, "Party", SceneCategory.Dynamic);
    public static readonly Scene Fireplace = new(5, "Fireplace", SceneCategory.Dynamic);
    public static readonly Scene Cozy = new(6, "Cozy", SceneCategory.Simple);
    public static readonly Scene Forest = new(7, "Forest", SceneCategory.Dynamic);
    public static readonly Scene PastelColours = new(8, "Pastel colours", SceneCategory.Dynamic);
    public static readonly Scene Wakeup = new(9, "Wakeup", SceneCategory.Progressive);
    public static readonly Scene Bedtime = new(10, "Bedtime", SceneCategory.Progressive);
    public static readonly Scene WarmWhite = new(11, "Warm white", SceneCategory.White);
    public static readonly Scene Daylight = new(12, "Daylight", SceneCategory.White);
    public static readonly Scene CoolWhite = new(13, "Cool white", SceneCategory.White);
    public static readonly Scene NightLight = new(14, "Night light", SceneCategory.White);
    public static readonly Scene Focus = new(15, "Focus", SceneCategory.Simple);
    public static readonly Scene Relax = new(16, "Relax", SceneCategory.Simple);
    public static readonly Scene TrueColours = new(17, "True colours", SceneCategory.Simple);
    public static readonly Scene TvTime = new(18, "TV time", SceneCategory.Simple);
    public static readonly Scene PlantGrowth = new(19, "Plant growth", SceneCategory.Simple);
    public static readonly Scene Spring = new(20, "Spring", SceneCategory.Dynamic);
    public static readonly Scene Summer = new(21, "Summer", SceneCategory.Dynamic);
    public static readonly Scene Fall = new(22, "Fall", SceneCategory.Dynamic);
    public static readonly Scene DeepDive = new(23, "Deep dive", SceneCategory.Dynamic);
    public static readonly Scene Jungle = new(24, "Jungle", SceneCategory.Dynamic);
    public static readonly Scene Mojito = new(25, "Mojito", SceneCategory.Dynamic);
    public static readonly Scene Club = new(26, "Club", SceneCategory.Dynamic);
    public static readonly Scene Christmas = new(27, "Christmas", SceneCategory.Celebration);
    public static readonly Scene Halloween = new(28, "Halloween", SceneCategory.Celebration);
    public static readonly Scene Candlelight = new(29, "Candlelight", SceneCategory.Dynamic);
    public static readonly Scene GoldenWhite = new(30, "Golden white", SceneCategory.Dynamic);
    public static readonly Scene Pulse = new(31, "Pulse", SceneCategory.Dynamic);
    public static readonly Scene Steampunk = new(32, "Steampunk", SceneCategory.Dynamic);
    public static readonly Scene Diwali = new(33, "Diwali", SceneCategory.Celebration);
    public static readonly Scene Alarm = new(35, "Alarm", SceneCategory.White);
    public static readonly Scene Rhythm = new(1000, "Rhythm", SceneCategory.Rhythm);
}
