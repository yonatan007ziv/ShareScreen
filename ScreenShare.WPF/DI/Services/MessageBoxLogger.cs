using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace ScreenShare.WPF.DI.Services;

class MessageBoxLogger : ILogger
{
	public IDisposable? BeginScope<TState>(TState state) where TState : notnull
	{
		throw new NotImplementedException();
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return true;
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
	{
		MessageBox.Show(formatter(state, exception));
	}
}