using System.Net;
using System.Text.Json.Serialization;

namespace WizBulbApi;

public class BulbHandle
{
    [JsonConstructor]
    public BulbHandle(IPAddress ipAddress, MacAddress macAddress)
    {
        IpAddress = ipAddress;
        MacAddress = macAddress;
    }

    public BulbHandle(IPAddress ipAddress)
    {
        IpAddress = ipAddress;
    }

    public BulbHandle(string ipAddress)
    {
        IpAddress = IPAddress.Parse(ipAddress);
    }

    public BulbHandle(string ipAddress, string macAddress)
    {
        IpAddress = IPAddress.Parse(ipAddress);
        MacAddress = new(macAddress);
    }

    public BulbHandle() { }        

    public IPAddress IpAddress { get; init; }
    public MacAddress MacAddress { get; init; }
    public string Name { get; set; }
    public int HomeId { get; set; }
    public int RoomId { get; set; }

	public override bool Equals(object? obj)
    {
        if(obj is not BulbHandle other) return false;

        if(ReferenceEquals(this, other)) return true;

        return ($"{this.IpAddress}", $"{this.MacAddress}") == ($"{other.IpAddress}", $"{other.MacAddress}");
    }

    public override int GetHashCode()
    {
        return (IpAddress, MacAddress).GetHashCode();
    }

    public static bool operator ==(BulbHandle objA, BulbHandle objB) => Equals(objA, objB);
    public static bool operator !=(BulbHandle objA, BulbHandle objB) => !(objA == objB);
}
