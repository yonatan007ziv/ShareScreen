using ScreenShare.Server.DI.Interfaces;
using ShareScreen.Core;
using ShareScreen.Core.DI.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace ScreenShare.Server.DI.Services;

internal class TcpServer : IServer
{
	private TcpListener listener;
	private TcpClientCommunication? client;
	private readonly IFactory<TcpClientCommunication, TcpClient> tcpClientCommunicationFactory;

	public TcpServer(IFactory<TcpClientCommunication, TcpClient> tcpClientCommunicationFactory)
	{
		listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5555);

		this.tcpClientCommunicationFactory = tcpClientCommunicationFactory;
	}

	public void Start()
	{
		listener.Start();
	}

	public async Task AcceptPending()
	{
		await listener.AcceptTcpClientAsync().ContinueWith(t => client = tcpClientCommunicationFactory.Create(t.Result));
	}

	public async Task WriteMessage(Message msg)
	{
		if (client == null)
			return;

		await client.WriteMessage(msg);
	}

	public async Task<Message?> ReadMessage()
	{
		if (client == null)
			return null;

		return await client.ReadMessage();
	}
}