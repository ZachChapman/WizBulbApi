namespace WizBulbApi;

public class GetSystemConfigResponse : IBulbStateResponse
{
    public string ModuleName { get; set; }
    public int HomeId { get; set; }
    public int GroupId { get; set; }
    public int RoomId { get; set; }
    public string FwVersion { get; set; }
    public string IpAddress { get; set; }
    public string MacAddress { get; set; }
}
