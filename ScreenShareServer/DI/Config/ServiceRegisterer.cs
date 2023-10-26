using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScreenShareServer.DI.Interfaces;
using ScreenShareServer.DI.Services;

namespace ScreenShareServer.DI.Config;

internal class ServiceRegisterer
{
	public void RegisterServices(IServiceCollection collection)
	{
		collection.AddSingleton<ILogger, ConsoleLogger>();

		collection.AddSingleton<ICommunicationServer, TcpCommunicationServer>();

		// Emulators
		collection.AddSingleton<IMouseEmulator, MouseEmulator>();
		collection.AddSingleton<IKeyboardEmulator, KeyboardEmulator>();

		collection.AddSingleton<IScreenExtractor, ScreenExtractor>();

		collection.AddSingleton<ISerializer, JsonObjectSerializer>();
		collection.AddSingleton<IMessageSerializer, MessageSerializer>();

		collection.AddSingleton<ShareServer>();
	}
}