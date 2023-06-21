namespace WizBulbApi.Commands;

public class GetSystemConfigCommand : BulbCommand
{
    public override StateMethod Method => StateMethod.GetSystemConfig;
}
