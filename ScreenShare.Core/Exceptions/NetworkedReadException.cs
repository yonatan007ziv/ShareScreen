namespace ShareScreen.Core.Exceptions;

public class NetworkedReadException : Exception
{
	public ReadExceptionType Type { get; }
	public NetworkedReadException(ReadExceptionType Type)
	{
		this.Type = Type;
	}
}

public enum ReadExceptionType
{
	IO,
	Timedout,
	OperationCancelled,
	EncryptionFailed,
	TextDecodingFailed,
	DeserializationFailed,
	Disconnected,
	Disposed,
}