using System.Text.Json;

namespace WizBulbApi;

public class BulbResponseToJsonConverter : IConverter<IBulbStateResponse, string>
{
    public string Convert(IBulbStateResponse response)
    {
        return JsonSerializer.Serialize(response, response.GetType());
    }
}
