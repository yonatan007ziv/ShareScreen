using ShareScreenCore;

namespace ScreenShareServer.DI.Interfaces;

internal interface IMessageSerializer
{
	string SerializeMessage(Message deserialized);
	Message DeserializeMessage(string serialized);
}