namespace WizBulbApi.WinUI;

public static class ViewTypeRegister
{
    public static void Register()
    {
        ViewTypes.Scenes = typeof(ScenesView);
    }
}
