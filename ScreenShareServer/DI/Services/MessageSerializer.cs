using Microsoft.Extensions.Logging;
using ScreenShareServer.DI.Interfaces;
using ShareScreenCore;

namespace ScreenShareServer.DI.Services;

internal class MessageSerializer : IMessageSerializer
{
	private readonly ILogger logger;
	private readonly ISerializer serializer;

	public MessageSerializer(ILogger logger, ISerializer serializer)
    {
		this.logger = logger;
		this.serializer = serializer;
	}

    public Message DeserializeMessage(string serialized)
	{
		return serializer.Deserialize<Message>(serialized);
	}

	public string SerializeMessage(Message deserialized)
	{
		return serializer.Serialize(deserialized);
	}
}