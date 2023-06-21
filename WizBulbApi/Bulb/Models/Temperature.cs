namespace WizBulbApi;

/// <summary> The temperature of the bulb. </summary>
/// <remarks> Range: 2200 - 6500 </remarks>
public record Temperature : RangedInt
{
    public static int Min => 2200;
    public static int Max => 6500;

    public Temperature(int value) : base(Min, Max, value) { }

    public static implicit operator Temperature(int value) => new(value);
}
