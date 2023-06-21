using WizBulbApi.Commands;

namespace WizBulbApi;

public static class BulbCommandExtensions
{ 
    public static BulbState ToState(this BulbCommand command) => new BulbCommandToBulbStateConverter().Convert(command);
    public static string ToJson(this BulbCommand command) => new BulbCommandToJsonConverter().Convert(command);
}
