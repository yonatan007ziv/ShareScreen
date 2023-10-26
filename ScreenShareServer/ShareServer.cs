using Microsoft.Extensions.Logging;
using ScreenShareServer.DI.Interfaces;
using ScreenShareServer.DI.Services;
using ShareScreenCore;
using System.Drawing;

namespace ScreenShareServer;

internal class ShareServer
{
	private const int TargetFPS = 30;

	private readonly ICommunicationServer communicationServer;
	private readonly ILogger logger;
	private readonly IScreenExtractor screenExtractor;
	private readonly IMouseEmulator mouse;
	private readonly IKeyboardEmulator keyboard;

	public ShareServer(ILogger logger, ICommunicationServer communicationServer, IScreenExtractor screenExtractor, IMouseEmulator mouse, IKeyboardEmulator keyboard)
	{
		this.logger = logger;
		this.communicationServer = communicationServer;
		this.screenExtractor = screenExtractor;
		this.mouse = mouse;
		this.keyboard = keyboard;
	}

	public async Task Start()
	{
		logger.LogInformation("Starting ShareServer...");
		communicationServer.Start();
		logger.LogInformation("Started ShareServer");

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
			byte[] bitmapData;
			using (MemoryStream stream = new MemoryStream())
			{
				bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
				bitmapData = stream.ToArray();
			}

			Message msg = new Message(MessageType.Frame, bitmapData);
			await communicationServer.SendMessage(msg);
			await Task.Delay(1000 / TargetFPS);
		}
	}

	private async Task MessageLoop()
	{
		logger.LogInformation("Entered MessageLoop");
		while (true)
		{
			Message Received = await communicationServer.ReceiveMessage();
			Decode(Received);
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