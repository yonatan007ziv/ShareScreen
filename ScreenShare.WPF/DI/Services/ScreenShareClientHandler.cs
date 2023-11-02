using ScreenShare.WPF.DI.Interfaces;
using ShareScreen.Core;
using ShareScreen.Core.DI.Interfaces;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;

namespace ScreenShare.WPF.DI.Services;

public class ScreenShareClientHandler
{
	private readonly IClient client;
	private readonly ISerializer<Bitmap> bitmapSerializer;

	public ScreenShareClientHandler(IClient client, ISerializer<Bitmap> bitmapSerializer)
	{
		this.client = client;
		this.bitmapSerializer = bitmapSerializer;

		Connect(IPAddress.Parse("127.0.0.1"), 5555); // temp obviously;
	}

	private async Task<bool> Connect(IPAddress address, int port)
	{
		return await client.Connect(address, port);
	}

	public async Task<Bitmap> GetFrame()
	{
		Message? msg = await client.ReadMessage();

		if (msg == null)
			return new Bitmap(1920, 1080);
		return bitmapSerializer.Deserialize(msg.Content);
	}
}