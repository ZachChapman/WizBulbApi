using System.Text.Json;
using System.Text.Json.Serialization;

namespace WizBulbApi;

public class StringJsonConverter<T> : JsonConverter<T>
{
    private readonly Func<string, T> _construction;

    public StringJsonConverter(Func<string, T> construction)
    {
        _construction = construction;
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return _construction(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
