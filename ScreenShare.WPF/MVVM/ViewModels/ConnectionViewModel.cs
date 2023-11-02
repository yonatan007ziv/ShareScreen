using ScreenShare.WPF.DI.Interfaces;
using ScreenShare.WPF.MVVM.Models;
using ScreenShare.WPF.MVVM.Shell;
using System.Net;
using System.Threading.Tasks;

namespace ScreenShare.WPF.MVVM.ViewModels;

internal class ConnectionViewModel : ViewModelBase
{
	private readonly ConnectionModel model = new ConnectionModel();
	private readonly IClient client;
	private readonly INavigation navigation;
	private int times = 0;

	public string ConnectingAnimation
	{
		get => model.ConnectingAnimation;
		set
		{
			model.ConnectingAnimation = value;
			OnPropertyChanged();
		}
	}

	public ConnectionViewModel(IClient client, INavigation navigation)
	{
		this.client = client;
		this.navigation = navigation;
	}

	private async void AnimationUpdate()
	{
		while (inView)
		{
			ConnectingAnimation = new string('.', times);

			times++;
			times %= 4;

			await Task.Delay(250);
		}
	}

	private Task<bool> TryConnect()
		=> client.Connect(IPAddress.Parse("127.0.0.1"), 5555);

	public override async void Enter()
	{
		AnimationUpdate();

		if (await TryConnect())
			navigation.NavigateTo<ShareViewModel>();
		else
			navigation.NavigateTo<MenuViewModel>();
	}

	public override void Exit() { }
}