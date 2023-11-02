using ScreenShare.WPF.DI.Interfaces;
using ScreenShare.WPF.MVVM.Models;
using ScreenShare.WPF.MVVM.Shell;
using ScreenShare.WPF.Utils;
using System.Windows.Input;

namespace ScreenShare.WPF.MVVM.ViewModels;

class MenuViewModel : ViewModelBase
{
	private readonly MenuModel model = new MenuModel();
	private readonly INavigation navigation;

	public string Ip
	{
		get => model.Ip;
		set
		{
			model.Ip = value;
			OnPropertyChanged();
		}
	}

	public ICommand ConnectCmd
	{
		get => model.ConnectCmd;
		set
		{
			model.ConnectCmd = value;
			OnPropertyChanged();
		}
	}

	public MenuViewModel(INavigation navigation)
	{
		ConnectCmd = new RelayCommand(ConnectClick, CanConnect);
		this.navigation = navigation;
	}

	private void ConnectClick(object? obj)
	{
		navigation.NavigateTo<ConnectionViewModel>();
	}

	private bool CanConnect(object? obj)
	{
		return true; // Ip != "";
	}

	public override void Enter() { }

	public override void Exit() { }
}