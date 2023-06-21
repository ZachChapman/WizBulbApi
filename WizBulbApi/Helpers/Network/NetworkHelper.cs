using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace WizBulbApi;

public static class NetworkHelper
{
	public static IEnumerable<NetworkInfo> GetNetworkInfo()
	{
		var networkInfos = new List<NetworkInfo>();

		// first find network interface
		foreach(var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
		{
			if(networkInterface.OperationalStatus == OperationalStatus.Up
			&& (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet
			|| networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
			{
				// then find a valid IPv4 address
				foreach(var ipAddressInformation in networkInterface.GetIPProperties().UnicastAddresses)
				{
					if(ipAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
					{
						networkInfos.Add(new()
						{
							Id = networkInterface.Id,
							Name = networkInterface.Name,
							Description = networkInterface.Description,
							IpAddress = ipAddressInformation.Address,
							MacAddress = new(networkInterface.GetPhysicalAddress().GetAddressBytes()),
						});
					}
				}
			}
		}

		return networkInfos;
	}
}
