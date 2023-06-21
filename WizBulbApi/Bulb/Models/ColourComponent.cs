namespace WizBulbApi;

/// <summary> The single component of a colour. </summary>
/// <remarks> Range: 0 - 255 </remarks>
public record ColourComponent : RangedInt
{
    public static int Min => 0;
    public static int Max => 255;

    public ColourComponent(int value) : base(Min, Max, value) { }

    public static implicit operator ColourComponent(int value) => new(value);
}
