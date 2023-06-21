namespace WizBulbApi;

public class GetPilotResponse : IBulbStateResponse
{
    public int SceneId { get; set; }
    public int Dimming { get; set; }
    public bool State { get; set; }
    public string? MacAddress { get; set; }
    public int Rssi { get; set; }
}
