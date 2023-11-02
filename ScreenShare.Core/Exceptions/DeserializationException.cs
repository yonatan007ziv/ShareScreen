namespace ShareScreen.Core.Exceptions;

public class DeserializationException : Exception
{
	public DeserializationException(string serialized)
		: base($"Exception while Deserializing: {serialized}")
	{

	}
}