using ScreenShareServer.DI.Interfaces;
using ShareScreenCore;
using System.Net;
using System.Net.Sockets;

namespace ScreenShareServer.DI.Services;

internal class TcpCommunicationServer : ICommunicationServer
{
    private TcpListener listener;
    private readonly IMessageSerializer serializer;

    public TcpCommunicationServer(IMessageSerializer serializer)
    {
        listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5555);

        this.serializer = serializer;
    }

    public void Start()
    {
        listener.Start();
    }

    public Task AcceptPending()
    {
        return listener.AcceptTcpClientAsync();
    }

    public Task<Message> ReceiveMessage()
    {
        return Task.FromResult(new Message(MessageType.MouseAction, new Vector2(100, 100)));
    }

    public Task SendMessage(Message msg)
    {
        return Task.CompletedTask;
    }
}