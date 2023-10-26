using ShareScreenCore;

namespace ScreenShareServer.DI.Interfaces;

internal interface ICommunicationServer
{
	void Start();
	Task AcceptPending();
	Task SendMessage(Message msg);
	Task<Message> ReceiveMessage();
}