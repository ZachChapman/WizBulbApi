using System.Runtime.Serialization;

namespace WizBulbApi;

public class InvalidBulbStateException : Exception
{
    public InvalidBulbStateException() : base("State error: Unknown error.") { }

    public InvalidBulbStateException(StateError stateError)
        : base(stateError is null
            ? "State error: Unknown error."
            : $"State error: {stateError.Message}. ({stateError.Code})") { }

    public InvalidBulbStateException(StateError stateError, string message)
        : base(stateError is null
            ? "State error: Unknown error."
            : $"State error: {stateError.Message}. ({stateError.Code}) {message}") { }

    public InvalidBulbStateException(string? message) : base(message) { }

    public InvalidBulbStateException(string? message, Exception? innerException) : base(message, innerException) { }

    protected InvalidBulbStateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
