namespace WizBulbApi;

public static class BulbStateResponseExtensions
{
    public static string ToJson(this IBulbStateResponse response) => new BulbResponseToJsonConverter().Convert(response);
}
