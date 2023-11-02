using Microsoft.Extensions.DependencyInjection;
using ScreenShare.WPF.DI.Config;
using ScreenShare.WPF.MVVM.Shell;
using System;
using System.Windows;

namespace ScreenShare.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	private readonly IServiceProvider provider;

	public App()
	{
		IServiceCollection collection = new ServiceCollection();
		new ServiceRegisterer().RegisterServices(collection);

		provider = collection.BuildServiceProvider();
	}

	private void Application_Startup(object sender, StartupEventArgs e)
	{
		provider.GetRequiredService<WindowView>().Show();
	}
}