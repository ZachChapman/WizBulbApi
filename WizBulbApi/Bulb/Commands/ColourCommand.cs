﻿namespace WizBulbApi.Commands;

public class ColourCommand : BulbCommand
{
    private readonly Colour _colour;

    public ColourCommand(Colour colour)
    {
        _colour = colour;
    }

    public override StateMethod Method => StateMethod.SetPilot;
    public int? R => _colour.Red;
    public int? G => _colour.Green;
    public int? B => _colour.Blue;
    public int? C => _colour.CoolWhite;
    public int? W => _colour.WarmWhite;
}
