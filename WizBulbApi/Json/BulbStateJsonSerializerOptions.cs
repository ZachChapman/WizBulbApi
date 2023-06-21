using System.Text.Json;
using System.Text.Json.Serialization;

namespace WizBulbApi;

public class BulbStateJsonSerializerOptions
{
    public static implicit operator JsonSerializerOptions(BulbStateJsonSerializerOptions options)
    {
        return new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new StateMethodJsonConverter(),
            },
        };
    }
}