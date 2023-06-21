using System.Drawing;

namespace WizBulbApi;

public record Colour
{
    public Colour(ColourComponent red, ColourComponent green, ColourComponent blue, ColourComponent? coolWhite = default, ColourComponent? warmWhite = default)
    {
        Red = red;
        Green = green;
        Blue = blue;
        CoolWhite = coolWhite ?? 0;
        WarmWhite = warmWhite ?? 0;
    }

    public Colour(int red, int green, int blue, int? coolWhite = default, int? warmWhite = default)
    {
        Red = red;
        Green = green;
        Blue = blue;
        CoolWhite = coolWhite ?? 0;
        WarmWhite = warmWhite ?? 0;
    }

    public Colour(Color color)
    {
        Red = color.R;
        Green = color.G;
        Blue = color.B;
    }

    public ColourComponent Red { get; set; } = new(0);
    public ColourComponent Green { get; set; } = new(0);
    public ColourComponent Blue { get; set; } = new(0);
    public ColourComponent CoolWhite { get; set; } = new(0);
    public ColourComponent WarmWhite { get; set; } = new(0);

    public static implicit operator Colour(Color color) => new(color);
    public static implicit operator Color(Colour colour) => Color.FromArgb(255, colour.Red, colour.Green, colour.Blue);
    public static implicit operator Colour((int red, int green, int blue, int? coolWhite, int? warmWhite) values) => new(values.red, values.green, values.blue, values.coolWhite, values.warmWhite);
}
