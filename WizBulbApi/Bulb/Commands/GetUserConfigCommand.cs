namespace WizBulbApi.Commands;

public class GetUserConfigCommand : BulbCommand
{
    public override StateMethod Method => StateMethod.GetUserConfig;
}
