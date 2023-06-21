using System.Text.Json;

namespace WizBulbApi;

public class BulbStateToJsonConverter : IDualConverter<BulbState, string>
{
    public string Convert(BulbState state)
    {
        return JsonSerializer.Serialize(state, new BulbStateJsonSerializerOptions());
    }

    public BulbState ConvertBack(string stateJson)
    {
        return JsonSerializer.Deserialize<BulbState>(stateJson, new BulbStateJsonSerializerOptions())!;
    }
}
