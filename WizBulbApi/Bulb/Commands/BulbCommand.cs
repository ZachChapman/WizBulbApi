namespace WizBulbApi.Commands;

public abstract class BulbCommand 
{
    public abstract StateMethod Method { get; }

    public static implicit operator BulbState(BulbCommand command) => command.ToState();
}
