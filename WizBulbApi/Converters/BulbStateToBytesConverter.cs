using System.Text;
using System.Text.Json;

namespace WizBulbApi;

public class BulbStateToBytesConverter : IDualConverter<BulbState, byte[]>
{
    public byte[] Convert(BulbState state)
    {
        return Encoding.UTF8.GetBytes(state.ToJson());
    }

    public BulbState ConvertBack(byte[] bytes)
    {
        return JsonSerializer.Deserialize<BulbState>(bytes, new BulbStateJsonSerializerOptions())!;
    }
}
