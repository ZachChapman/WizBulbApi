using System.Text.Json;
using WizBulbApi.Commands;

namespace WizBulbApi;

public class BulbCommandToJsonConverter : IConverter<BulbCommand, string>
{
    public string Convert(BulbCommand command)
    {
        return JsonSerializer.Serialize(command, command.GetType(), new BulbStateJsonSerializerOptions());
    }
}
