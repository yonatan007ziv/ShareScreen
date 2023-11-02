using ScreenShare.WPF.MVVM.Shell;

namespace ScreenShare.WPF.DI.Interfaces;

interface INavigation
{
	void NavigateTo<T>() where T : ViewModelBase;
}