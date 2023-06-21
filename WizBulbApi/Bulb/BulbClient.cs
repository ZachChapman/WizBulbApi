namespace WizBulbApi;

public class BulbClient
{
	private readonly BulbSocket _socket;

	public BulbClient(BulbHandle handle)
	{
		Handle = handle;
		_socket = new BulbSocket();
	}

	public BulbHandle Handle { get; }

	public async Task<BulbState?> SendStateAsync(BulbState state)
	{
		await _socket.SendToAsync(Handle, state);
		var responseState = await _socket.ReceiveFromAsync(Handle);

		responseState.IsValid(throwIfInvalid: true, respondingTo: state);

		return responseState;
	}

	public async Task<BulbState?> TrySendStateAsync(BulbState state)
	{
		try
		{
			await _socket.SendToAsync(Handle, state);
			var responseState = await _socket.ReceiveFromAsync(Handle);
			if(!responseState.IsValid())
			{
				return default;
			}

			return responseState;
		}
		catch { }

		return default;
	}
}
