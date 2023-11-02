using Microsoft.Extensions.Logging;
using ScreenShare.WPF.DI.Interfaces;
using ScreenShare.WPF.MVVM.Models;
using ScreenShare.WPF.MVVM.Shell;
using ShareScreen.Core;
using ShareScreen.Core.DI.Interfaces;
using System.Drawing;
using System.Threading.Tasks;

namespace ScreenShare.WPF.MVVM.ViewModels;

internal class ShareViewModel : ViewModelBase
{
	private readonly ShareModel model = new ShareModel();
	private readonly IClient client;
	private readonly ILogger logger;
	private readonly ISerializer<Bitmap> bitmapSerializer;

	public Bitmap ScreenFrame
	{
		get => model.ScreenFrame;
		set
		{
			model.ScreenFrame = value;
			OnPropertyChanged();
		}
	}

	public ShareViewModel(IClient client, ILogger logger, ISerializer<Bitmap> bitmapSerializer)
	{
		this.client = client;
		this.logger = logger;
		this.bitmapSerializer = bitmapSerializer;
	}

	private async void MessageLoop()
	{
		while (inView)
		{
			Message msg = await client.ReadMessage();
			Decode(msg);
			await Task.Delay(1000 / 120);
		}
	}

	private void Decode(Message msg)
	{
		switch (msg.Type)
		{
			case MessageType.Frame:
				SetFrame(msg.Content);
				break;
		}
	}

	private void SetFrame(string frame)
	{
		ScreenFrame = bitmapSerializer.Deserialize(frame);
	}

	public override void Enter()
	{
		MessageLoop();
	}

	public override void Exit() { }
}