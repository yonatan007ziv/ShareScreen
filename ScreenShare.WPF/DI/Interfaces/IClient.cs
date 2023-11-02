
using ShareScreen.Core.DI.Interfaces;
using System.Net;
using System.Threading.Tasks;

namespace ScreenShare.WPF.DI.Interfaces;

public interface IClient : ICommunication
{
	Task<bool> Connect(IPAddress addr, int port);
	void Disconnect();
}