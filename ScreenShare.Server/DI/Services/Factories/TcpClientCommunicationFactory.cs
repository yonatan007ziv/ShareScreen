using Microsoft.Extensions.Logging;
using ShareScreen.Core.DI.Interfaces;
using System.Net.Sockets;

namespace ScreenShare.Server.DI.Services.Factories;

internal class TcpClientCommunicationFactory : IFactory<TcpClientCommunication, TcpClient>
{
	private readonly ISerializer serializer;
	private readonly ILogger logger;

	public TcpClientCommunicationFactory(ISerializer serializer, ILogger logger)
	{
		this.serializer = serializer;
		this.logger = logger;
	}

	public TcpClientCommunication Create(TcpClient input)
	{
		return new TcpClientCommunication(input, serializer, logger);
	}
}