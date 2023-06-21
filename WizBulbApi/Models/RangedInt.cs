namespace WizBulbApi;

public abstract record RangedInt
{
    public RangedInt(int min, int max, int value)
    {
        if(value < min) value = min;
        if(value > max) value = max;

        Value = value;
    }

    public int Value { get; set; }

    public static implicit operator int(RangedInt rangedValue) => rangedValue.Value;
    public static implicit operator int?(RangedInt rangedValue) => rangedValue?.Value;
}
