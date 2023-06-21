namespace WizBulbApi;

/// <summary> The speed of the animation. </summary>
/// <remarks> Range: 10 - 200 </remarks>
public record Speed : RangedInt
{
    public static int Min => 10;
    public static int Max => 200;

    public Speed(int value) : base(Min, Max, value) { }

    public static implicit operator Speed(int value) => new(value);
}
