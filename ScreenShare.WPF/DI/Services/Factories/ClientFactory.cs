using Microsoft.Extensions.Logging;
using ScreenShare.WPF.DI.Interfaces;
using ShareScreen.Core.DI.Interfaces;

namespace ScreenShare.WPF.DI.Services.Factories;

class ClientFactory : IFactory<IClient>
{
	private readonly ISerializer serializer;
	private readonly ILogger logger;

	public ClientFactory(ISerializer serializer, ILogger logger)
	{
		this.serializer = serializer;
		this.logger = logger;
	}

	public IClient Create()
	{
		return new TcpClientCommunication(serializer, logger);
	}
}