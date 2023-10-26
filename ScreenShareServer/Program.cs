using Microsoft.Extensions.DependencyInjection;
using ScreenShareServer.DI.Config;
using ScreenShareServer.DI.Interfaces;
using ScreenShareServer.DI.Services;
using ShareScreenCore;
using System.Drawing;

namespace ScreenShareServer;

internal class Program
{
	public static async Task Main()
	{
		IServiceCollection collection = new ServiceCollection();
		new ServiceRegisterer().RegisterServices(collection);

		IServiceProvider provider = collection.BuildServiceProvider();

        await provider.GetRequiredService<ShareServer>().Start();
	}
}