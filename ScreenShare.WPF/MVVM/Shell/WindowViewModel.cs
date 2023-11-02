using ScreenShare.WPF.DI.Interfaces;
using ScreenShare.WPF.MVVM.ViewModels;

namespace ScreenShare.WPF.MVVM.Shell;

class WindowViewModel : ViewModelBase
{
	public INavigation Navigation { get; set; }

	public WindowViewModel(INavigation navigation)
	{
		Navigation = navigation;
		Navigation.NavigateTo<MenuViewModel>();
	}

	public override void Enter() { }

	public override void Exit() { }
}