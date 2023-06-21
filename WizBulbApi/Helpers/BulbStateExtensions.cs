using System.Drawing;

namespace WizBulbApi;

public static class BulbStateExtensions
{
    public static string ToJson(this BulbState state) => new BulbStateToJsonConverter().Convert(state);
    public static byte[] ToBytes(this BulbState state) => new BulbStateToBytesConverter().Convert(state);
    public static bool IsValid(this BulbState? state, bool throwIfInvalid = false, BulbState? respondingTo = default)
    {
        var isValid = state is not null && state.Error is null;

        if(!isValid && throwIfInvalid)
        {
            if(respondingTo is not null)
            {
                var message = $"[Responding to: {respondingTo!.ToJson()}]";

                throw state?.Error is null
                    ? new InvalidBulbStateException()
                    : new InvalidBulbStateException(state.Error, message);
            }

            throw state?.Error is null
               ? new InvalidBulbStateException()
               : new InvalidBulbStateException(state.Error);
        }

        return isValid;
    }
    public static Color? GetColor(this BulbState state)
    {
        if(state is null) return default;
        if(state.Result is null) return default;

        var result = state.Result;

        if(result.R is null) return default;

        return Color.FromArgb(255, (int)result.R!, (int)result.G!, (int)result.B!);
    }
    public static IBulbStateResponse ToResponse(this BulbState state) => new BulbStateToResponseConverter().Convert(state);
}
