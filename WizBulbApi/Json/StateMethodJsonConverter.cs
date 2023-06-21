using System.Text.Json;
using System.Text.Json.Serialization;

namespace WizBulbApi;

public sealed class StateMethodJsonConverter : JsonStringEnumConverter
{
    public StateMethodJsonConverter() : base(JsonNamingPolicy.CamelCase, true)
    {
    }
}
