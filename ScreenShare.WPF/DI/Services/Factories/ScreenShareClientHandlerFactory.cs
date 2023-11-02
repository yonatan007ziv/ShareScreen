using ScreenShare.WPF.DI.Interfaces;
using ShareScreen.Core.DI.Interfaces;
using System.Drawing;

namespace ScreenShare.WPF.DI.Services.Factories;

class ScreenShareClientHandlerFactory : IFactory<ScreenShareClientHandler>
{
	private readonly IFactory<IClient> clientFactory;
	private readonly ISerializer<Bitmap> bitmapSerializer;

	public ScreenShareClientHandlerFactory(IFactory<IClient> clientFactory, ISerializer<Bitmap> bitmapSerializer)
	{
		this.clientFactory = clientFactory;
		this.bitmapSerializer = bitmapSerializer;
	}

	public ScreenShareClientHandler Create()
	{
		return new ScreenShareClientHandler(clientFactory.Create(), bitmapSerializer);
	}
}