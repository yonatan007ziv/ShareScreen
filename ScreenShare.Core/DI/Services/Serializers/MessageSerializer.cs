using Microsoft.Extensions.Logging;
using ShareScreen.Core.DI.Interfaces;

namespace ShareScreen.Core.DI.Services.Serializers;

public class MessageSerializer : ISerializer<Message>
{
	private readonly ILogger logger;
	private readonly ISerializer serializer;

	public MessageSerializer(ILogger logger, ISerializer serializer)
	{
		this.logger = logger;
		this.serializer = serializer;
	}

	public string Serialize(Message deserialized)
	{
		return serializer.Serialize(deserialized);
	}

	public Message Deserialize(string serialized)
	{
		return serializer.Deserialize<Message>(serialized);
	}
}