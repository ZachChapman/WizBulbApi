using Microsoft.UI.Xaml.Data;

namespace WizBulbApi.WinUI;

public class SceneIdConverter : IValueConverter
{
    public SceneIdConvertTo ConvertTo { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(ConvertTo is SceneIdConvertTo.DisplayName)
        {
            if(value is int sceneId)
            {
                return Scene.FromIndex(sceneId)?.DisplayName;
            }
        }

        if(ConvertTo is SceneIdConvertTo.Icon)
        {
            if(value is int sceneId)
            {
                return GetIcon(sceneId);
            }
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }

    private static string? GetIcon(int sceneId)
    {
        return new Dictionary<int, string>
        {
            //{ Scene.CustomWhite, "\ue734" },
            { Scene.Custom, "\uf53f" },
            { Scene.Ocean, "\uf773" },
            { Scene.Romance, "\uf004" },
            { Scene.Sunset, "\uf185" },
            { Scene.Party, "\uf001" },
            { Scene.Fireplace, "\uf06d" },
            { Scene.Cozy, "\uf7b6" },
            { Scene.Forest, "\uf1bb" },
            { Scene.PastelColours, "\uf5c3" },
            { Scene.Wakeup, "\uf017" },
            { Scene.Bedtime, "\uf236" },
            { Scene.WarmWhite, "\uf0eb" },
            { Scene.Daylight, "\uf185" },
            { Scene.CoolWhite, "\uf2dc" },
            { Scene.NightLight, "\uf186" },
            { Scene.Focus, "\uf5da" },
            { Scene.Relax, "\uf5bb" },
            { Scene.TrueColours, "\uf53f" },
            { Scene.TvTime, "\uf26c" },
            { Scene.PlantGrowth, "\uf4d8" },
            { Scene.Spring, "\uf06c" },
            { Scene.Summer, "\uf185" },
            { Scene.Fall, "\uf06c" },
            { Scene.DeepDive, "\uf578" },
            { Scene.Jungle, "\uf1bb" },
            { Scene.Mojito, "\ue4f4" },
            { Scene.Club, "\uf130" },
            { Scene.Christmas, "\uf06b" },
            { Scene.Halloween, "\uf6e2" },
            { Scene.Diwali, "\ue734" },
            { Scene.Candlelight, "\uf06d"},
            { Scene.GoldenWhite, "\ue734" },
            { Scene.Alarm, "\uf12a" },
            { Scene.Pulse, "\uf83e" },
            { Scene.Steampunk, "\uf530" },
            { Scene.Rhythm, "\uf734" },
        }
        .GetValueOrDefault(sceneId);
    }
}

public enum SceneIdConvertTo
{
    DisplayName,
    Icon,
}