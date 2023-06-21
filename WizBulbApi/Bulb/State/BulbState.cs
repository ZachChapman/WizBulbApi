namespace WizBulbApi;

public class BulbState
{
    public StateMethod? Method { get; set; }
    public StateParams? Params { get; set; }
    public StateResult? Result { get; set; }
    public StateError? Error { get; set; }
    public int? Id { get; set; }

    public static BulbState Parse(byte[] bytes) => new BulbStateToBytesConverter().ConvertBack(bytes);
}
