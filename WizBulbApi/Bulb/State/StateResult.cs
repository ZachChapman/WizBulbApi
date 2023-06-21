namespace WizBulbApi;

public class StateResult : StateParams
{
    /// <summary> Whether a method call was successful. Not always used. </summary>
    public bool? Success { get; set; }

    /// <summary> WiFi signal strength, in negative dBm. </summary>
    /// <remarks> (Received Signal Strength Indicator) </remarks>
    public int? Rssi { get; set; }
}
