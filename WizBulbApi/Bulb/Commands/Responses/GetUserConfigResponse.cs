namespace WizBulbApi;

public class GetUserConfigResponse : IBulbStateResponse
{
    public int DftDim { get; set; }
    public int FadeIn { get; set; }
    public int FadeOut { get; set; }
    public bool Po { get; set; }
}
