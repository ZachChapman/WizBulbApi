using System.Net;
using System.Net.Sockets;

namespace WizBulbApi;

public class BulbSocket : IDisposable
{
    public BulbSocket()
    {
        Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
        {
            SendTimeout = (int)Timeout.TotalMilliseconds,
            ReceiveTimeout = (int)Timeout.TotalMilliseconds,
        };
    }

    public static int HostPort => 38899;
    public Socket Socket { get; }
    public int BufferSize { get; init; } = 256;
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(2);

    public int SendTo(BulbHandle handle, BulbState state)
    {
        return Socket.SendTo(state.ToBytes(), new IPEndPoint(handle.IpAddress, HostPort));
    }

    public BulbState ReceiveFrom(BulbHandle handle)
    {
        var buffer = new byte[BufferSize];

        var ipEndPoint = (EndPoint)new IPEndPoint(handle.IpAddress, 0);

        var receivedLength = Socket.ReceiveFrom(buffer, ref ipEndPoint);

        Array.Resize(ref buffer, receivedLength);

        return BulbState.Parse(buffer);
    }

    public async Task<int> SendToAsync(BulbHandle handle, BulbState state)
    {
        return await Socket.SendToAsync(state.ToBytes(), SocketFlags.None, new IPEndPoint(handle.IpAddress, HostPort));
    }

    public async Task<BulbState> ReceiveFromAsync(BulbHandle handle)
    {
        var buffer = new byte[BufferSize];

        var ipEndPoint = (EndPoint)new IPEndPoint(handle.IpAddress, 0);

        var cts = new CancellationTokenSource();
        if(Timeout != TimeSpan.Zero)
        {
            cts.CancelAfter((int)Timeout.TotalMilliseconds);
        }

        try
        {
            var result = await Socket.ReceiveFromAsync(buffer, SocketFlags.None, ipEndPoint, cts.Token);

            var receivedLength = result.ReceivedBytes;

            Array.Resize(ref buffer, receivedLength);

            return BulbState.Parse(buffer);
        }
        catch(OperationCanceledException ex)
        {
            throw new AggregateException(ex, new SocketException(10060));
        }
    }

    public void Dispose()
    {
        Socket.Dispose();
    }
}
