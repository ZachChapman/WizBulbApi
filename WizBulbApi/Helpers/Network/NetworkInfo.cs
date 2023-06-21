using System.Net;

namespace WizBulbApi;

public class NetworkInfo
{
	public string Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public IPAddress IpAddress { get; set; }
	public MacAddress MacAddress { get; set; }
}
