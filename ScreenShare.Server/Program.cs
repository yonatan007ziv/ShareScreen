using Microsoft.Extensions.DependencyInjection;
using ScreenShare.Server.DI.Config;
using ScreenShare.Server.DI.Services;

namespace ScreenShare.Server;

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