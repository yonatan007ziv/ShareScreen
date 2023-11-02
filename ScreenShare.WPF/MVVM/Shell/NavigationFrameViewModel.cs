using ScreenShare.WPF.DI.Interfaces;
using ShareScreen.Core.DI.Interfaces;
using System;

namespace ScreenShare.WPF.MVVM.Shell;

class NavigationFrameViewModel : ViewModelBase, INavigation
{
	private readonly IFactory<ViewModelBase, Type> viewModelFactory;

	private ViewModelBase? currentView;
	public ViewModelBase CurrentView
	{
		get => currentView!;
		set
		{
			currentView = value;
			OnPropertyChanged();
		}
	}

	public NavigationFrameViewModel(IFactory<ViewModelBase, Type> viewModelFactory)
	{
		this.viewModelFactory = viewModelFactory;
	}

	public void NavigateTo<T>() where T : ViewModelBase
	{
		if (CurrentView != null)
		{
			CurrentView.Exit();
			CurrentView.inView = false;
		}
		CurrentView = viewModelFactory.Create(typeof(T));
		CurrentView.inView = true;
		CurrentView.Enter();
	}

	public override void Enter() { }

	public override void Exit() { }
}