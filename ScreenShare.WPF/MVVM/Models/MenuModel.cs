using System.Windows.Input;

namespace ScreenShare.WPF.MVVM.Models;

internal class MenuModel
{
	public string Ip { get; set; } = "";
	public ICommand ConnectCmd { get; set; } = null!;
}