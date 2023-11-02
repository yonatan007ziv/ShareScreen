using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScreenShare.Server.DI.Interfaces;
using ScreenShare.Server.DI.Services;
using ScreenShare.Server.DI.Services.Factories;
using ScreenShare.Server.DI.Services.InputEmulators.KeyboardEmulators;
using ScreenShare.Server.DI.Services.InputEmulators.MouseEmulators;
using ScreenShare.Server.DI.Services.ScreenAnalyzers;
using ShareScreen.Core;
using ShareScreen.Core.DI.Interfaces;
using ShareScreen.Core.DI.Services;
using ShareScreen.Core.DI.Services.Serializers;
using System.Drawing;
using System.Net.Sockets;

namespace ScreenShare.Server.DI.Config;

internal class ServiceRegisterer
{
	public void RegisterServices(IServiceCollection collection)
	{
		collection.AddSingleton<IServer, TcpServer>();

		// Emulators
		collection.AddSingleton<IMouseEmulator, WindowsMouseEmulator>();
		collection.AddSingleton<IKeyboardEmulator, WindowsKeyboardEmulator>();

		collection.AddSingleton<IScreenAnalyzer, WindowsScreenAnalyzer>();

		collection.AddSingleton<ISerializer, JSONSerializer>();
		collection.AddSingleton<ISerializer<Bitmap>, BitmapSerializer>();
		collection.AddSingleton<ISerializer<Message>, MessageSerializer>();

		collection.AddSingleton<ShareServer>();

		// Loggers
		collection.AddSingleton<ILogger, ConsoleLogger>();

		// Factories
		collection.AddSingleton<IFactory<TcpClientCommunication, TcpClient>, TcpClientCommunicationFactory>();
	}
}