using MapsterMapper;
using WizBulbApi.Commands;

namespace WizBulbApi;

public class BulbCommandToBulbStateConverter : IConverter<BulbCommand, BulbState>
{
    public BulbState Convert(BulbCommand command)
    {
        return new BulbState
        {
            Method = command.Method,
            Params = new Mapper().Map<StateParams>(command),
        };
    }
}
