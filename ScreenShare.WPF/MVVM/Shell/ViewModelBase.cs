using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScreenShare.WPF.MVVM.Shell;

abstract class ViewModelBase : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public bool inView;

	public void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public abstract void Enter();
	public abstract void Exit();
}