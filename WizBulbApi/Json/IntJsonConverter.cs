using System.Text.Json;
using System.Text.Json.Serialization;

namespace WizBulbApi;

public class IntJsonConverter<T> : JsonConverter<T>
{
    private readonly Func<int, T> _construction;

    public IntJsonConverter(Func<int, T> construction)
    {
        _construction = construction;
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => _construction(reader.GetInt32());

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
}