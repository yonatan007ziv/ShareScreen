using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScreenShare.WPF.DI.Interfaces;
using ScreenShare.WPF.DI.Services;
using ScreenShare.WPF.DI.Services.Factories;
using ScreenShare.WPF.MVVM.Shell;
using ScreenShare.WPF.MVVM.ViewModels;
using ShareScreen.Core;
using ShareScreen.Core.DI.Interfaces;
using ShareScreen.Core.DI.Services.Serializers;
using System;
using System.Drawing;

namespace ScreenShare.WPF.DI.Config;

internal class ServiceRegisterer
{
	public void RegisterServices(IServiceCollection collection)
	{
		collection.AddSingleton<IClient, TcpClientCommunication>();

		collection.AddSingleton<ISerializer<Bitmap>, BitmapSerializer>();
		collection.AddSingleton<ISerializer<Message>, MessageSerializer>();
		collection.AddSingleton<ISerializer, JSONSerializer>();
		collection.AddSingleton<ILogger, MessageBoxLogger>();
		collection.AddSingleton<INavigation, NavigationFrameViewModel>();

		// Factories
		collection.AddSingleton<IFactory<IClient>, ClientFactory>();
		collection.AddSingleton<IFactory<ScreenShareClientHandler>, ScreenShareClientHandlerFactory>();
		collection.AddSingleton<IFactory<ViewModelBase, Type>, ViewModelFactory>();

		RegisterViews(collection);
	}

	private void RegisterViews(IServiceCollection collection)
	{
		collection.AddSingleton<WindowView>(provider => new WindowView { DataContext = provider.GetRequiredService<WindowViewModel>() });

		collection.AddSingleton<WindowViewModel>();
		collection.AddSingleton<MenuViewModel>();
		collection.AddSingleton<ConnectionViewModel>();
		collection.AddSingleton<ShareViewModel>();
	}
}