namespace WizBulbApi;

/// <summary> The brightness of the bulb. </summary>
/// <remarks> Range: 10 - 100 </remarks>
public record Brightness : RangedInt
{
    /// <summary> 100% </summary>
    public static readonly Brightness Full = new(100);
    /// <summary> 75% </summary>
    public static readonly Brightness High = new(75);
    /// <summary> 50% </summary>
    public static readonly Brightness Mid = new(50);
    /// <summary> 25% </summary>
    public static readonly Brightness Low = new(25);
    /// <summary> 10% </summary>
    public static readonly Brightness Dim = new(10);
    /// <summary> 0% </summary>
    public static readonly Brightness None = new(0);

    public static int Min => 10;
    public static int Max => 100;

    public Brightness(int value) : base(Min, Max, value) { }

    public static implicit operator Brightness(int value) => new(value);
}
