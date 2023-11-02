using Microsoft.Extensions.DependencyInjection;
using ScreenShare.WPF.MVVM.Shell;
using ShareScreen.Core.DI.Interfaces;
using System;

namespace ScreenShare.WPF.DI.Services.Factories;

internal class ViewModelFactory : IFactory<ViewModelBase, Type>
{
	private readonly IServiceProvider provider;

	public ViewModelFactory(IServiceProvider provider)
	{
		this.provider = provider;
	}

	public ViewModelBase Create(Type viewModel)
	{
		return (ViewModelBase)provider.GetRequiredService(viewModel);
	}
}