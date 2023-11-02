using ShareScreen.Core.DI.Interfaces;

namespace ScreenShare.Server.DI.Interfaces;

internal interface IServer : ICommunication
{
	void Start();
	Task AcceptPending();
}