using Microsoft.Extensions.Logging;
using ScreenShare.Server.DI.Interfaces;
using ShareScreen.Core;
using ShareScreen.Core.DI.Interfaces;
using System.Drawing;

namespace ScreenShare.Server.DI.Services;

internal class ShareServer
{
	private const int TargetFPS = 60;

	private readonly IServer communicationServer;
	private readonly ILogger logger;
	private readonly IScreenAnalyzer screenExtractor;
	private readonly IMouseEmulator mouse;
	private readonly IKeyboardEmulator keyboard;
	private readonly ISerializer<Bitmap> bitmapSerializer;

	public ShareServer(ILogger logger,
		IServer communicationServer,
		IScreenAnalyzer screenExtractor,
		IMouseEmulator mouse,
		IKeyboardEmulator keyboard,
		ISerializer<Bitmap> bitmapSerializer)
	{
		this.logger = logger;
		this.communicationServer = communicationServer;
		this.screenExtractor = screenExtractor;
		this.mouse = mouse;
		this.keyboard = keyboard;
		this.bitmapSerializer = bitmapSerializer;
	}

	public async Task Start()
	{
		communicationServer.Start();

		logger.LogInformation("Waiting For Client...");
		await communicationServer.AcceptPending();
		logger.LogInformation("Accepted Client");
		FrameSenderLoop();
		await MessageLoop();
	}

	private async void FrameSenderLoop()
	{
		while (true)
		{
			Bitmap bitmap = screenExtractor.GetScreen();
			Message msg = new Message(MessageType.Frame, bitmapSerializer.Serialize(bitmap));
			await communicationServer.WriteMessage(msg);
			await Task.Delay(1000 / TargetFPS);
		}
	}

	private async Task MessageLoop()
	{
		logger.LogInformation("Entered MessageLoop");
		while (true)
		{
			Message? received = await communicationServer.ReadMessage();
			if (received == null)
				break;

			Decode(received);
		}
	}

	private void Decode(Message msg)
	{
		switch (msg.Type)
		{
			case MessageType.SwitchMonitor:
				break;
			case MessageType.MouseAction:
				MouseAction(msg);
				break;
			case MessageType.KeyboardAction:
				KeyboardAction(msg);
				break;
		}
	}

	private void MouseAction(Message msg)
	{
		throw new NotImplementedException();
	}

	private void KeyboardAction(Message msg)
	{
		throw new NotImplementedException();
	}
}