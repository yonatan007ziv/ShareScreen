using System;
using System.Windows.Input;

namespace ScreenShare.WPF.Utils;

class RelayCommand : ICommand
{
	private readonly Action<object?> execute;
	private readonly Predicate<object?> canExecute;

	public event EventHandler? CanExecuteChanged;

	public RelayCommand(Action<object?> execute, Predicate<object?> canExecute)
	{
		this.execute = execute;
		this.canExecute = canExecute;
	}

	public bool CanExecute(object? parameter)
	{
		return canExecute(parameter);
	}

	public void Execute(object? parameter)
	{
		execute(parameter);
	}
}