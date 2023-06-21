using Toolbox;

namespace WizBulbApi;

public class SceneCategory : Enumeration<SceneCategory>
{
    public SceneCategory(int index, string displayName) : base(index)
    {
        DisplayName = displayName;
    }

    public string DisplayName { get; }

    public static readonly SceneCategory White = new(0, "White");
    public static readonly SceneCategory Simple = new(1, "Simple");
    public static readonly SceneCategory Dynamic = new(2, "Dynamic");
    public static readonly SceneCategory Progressive = new(3, "Progressive");
    public static readonly SceneCategory Celebration = new(4, "Celebration");
    public static readonly SceneCategory Rhythm = new(5, "Rhythm");
}
