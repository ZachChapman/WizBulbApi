using System.Net;
using System.Net.Sockets;
using Toolbox;
using WizBulbApi.Commands;

namespace WizBulbApi;

public class BulbScanner : IDisposable
{
	public static int HostPort => BulbSocket.HostPort;

	private readonly int _homeId;
	private readonly IPAddress _hostIpAddress;
	private readonly MacAddress _macAddress;
	private readonly UdpClient _udpClient;

	public BulbScanner(int homeId, IPAddress hostIpAddress, MacAddress macAddress)
	{
		_homeId = homeId;
		_hostIpAddress = hostIpAddress;
		_macAddress = macAddress;

		_udpClient = new(new IPEndPoint(_hostIpAddress, HostPort));
	}

	public BulbScanner(int homeId)
	{
		_homeId = homeId;

		var networkInfo =
			NetworkHelper.GetNetworkInfo().FirstOrDefault()
			?? throw new Exception("No networks detected");

		_hostIpAddress = networkInfo.IpAddress;
		_macAddress = networkInfo.MacAddress;

		_udpClient = new(new IPEndPoint(_hostIpAddress, HostPort));
	}

	public async Task FindBulbHandlesAsync(Action<BulbHandle> onFound, int timeout = 3000)
	{
		var handles = new HashSet<BulbHandle>();

		using var repeater = new Repeater(async () =>
		{
			var (registerResponse, remoteEndPoint) = await Register();
			if(registerResponse.Result is null)
			{
				return;
			}

			var ipAddress = remoteEndPoint.Address;
			var macAddress = new MacAddress(registerResponse.Result.Mac);

			var getConfigResponse = await GetSystemConfig(ipAddress);
			var roomId = getConfigResponse?.Result?.RoomId;
			if(roomId is null)
			{
				return;
			}

			var handle = new BulbHandle(ipAddress, macAddress)
			{
				HomeId = _homeId,
				RoomId = (int)roomId,
			};

			var isAdded = handles.Add(handle);
			if(isAdded)
			{
				onFound(handle);
			}
		},
		TimeSpan.FromMilliseconds(10));

		repeater.Start();
		await Task.Delay(timeout);
		repeater.Stop();
	}

	private async Task<(BulbState, IPEndPoint)> Register()
	{
		var registration =
			new RegistrationCommand(_homeId, _hostIpAddress, _macAddress)
			.ToState()
			.ToBytes();

		await _udpClient.SendAsync(registration, registration.Length, IPAddress.Broadcast.ToString(), HostPort);
		var response = await _udpClient.ReceiveAsync();
		var state = BulbState.Parse(response.Buffer);

		return (state, response.RemoteEndPoint);
	}

	private async Task<BulbState> GetSystemConfig(IPAddress ipAddress)
	{
		var configCommand =
			new GetSystemConfigCommand()
			.ToState()
			.ToBytes();

		await _udpClient.SendAsync(configCommand, configCommand.Length, ipAddress.ToString(), HostPort);
		var response = await _udpClient.ReceiveAsync();
		var state = BulbState.Parse(response.Buffer);

		return state;
	}

	public void Dispose()
	{
		_udpClient.Dispose();
	}
}
