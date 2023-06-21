namespace WizBulbApi;

public class BulbStateToResponseConverter : IConverter<BulbState, IBulbStateResponse>
{
    public IBulbStateResponse Convert(BulbState state)
    {
        if(state.Error is not null)
        {
            return new InvalidStateResponse();
        }

        IBulbStateResponse response = state.Method switch
        {
            StateMethod.Registration => new RegistrationResponse(),
            StateMethod.Pulse => new PulseResponse(),
            //StateMethod.FirstBeat => throw new NotImplementedException(),
            StateMethod.GetPilot => GetGetPilotResponse(state),
            StateMethod.SetPilot => new SetPilotResponse(),
            //StateMethod.SyncPilot => throw new NotImplementedException(),
            StateMethod.GetSystemConfig => new GetSystemConfigResponse(),
            StateMethod.SetSystemConfig => new SetSystemConfigResponse(),
            StateMethod.GetUserConfig => new GetUserConfigResponse(),
            StateMethod.SetUserConfig => new SetUserConfigResponse(),
            _ => throw new NotImplementedException(),
        };

        return new MapsterMapper.Mapper().Map(state.Result, response, typeof(StateResult), response.GetType()) as IBulbStateResponse;
    }

    private static GetPilotResponse GetGetPilotResponse(BulbState state)
    {
        return state.Result switch
        {
            { R: not null } => new GetPilotColourResponse(),
            { Speed: not null } => new GetPilotSceneResponse(),
            { Temp: not null } => new GetPilotTemperatureResponse(),
            _ => throw new NotImplementedException(),
        };
    }
}
