using Microsoft.Extensions.Logging;

namespace ShareScreen.Core.DI.Services;

public class ConsoleLogger : ILogger
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
		string prefixMessage;
		ConsoleColor prefixColor;

		switch (logLevel)
		{
			default:
				prefixMessage = "";
				prefixColor = ConsoleColor.White;
				break;
			case LogLevel.Trace:
				prefixMessage = "[Trace] ";
				prefixColor = ConsoleColor.White;
				break;
			case LogLevel.Debug:
				prefixMessage = "[Debug] ";
				prefixColor = ConsoleColor.Yellow;
				break;
			case LogLevel.Information:
				prefixMessage = "[Information] ";
				prefixColor = ConsoleColor.Green;
				break;
			case LogLevel.Warning:
				prefixMessage = "[Warning] ";
				prefixColor = ConsoleColor.Yellow;
				break;
			case LogLevel.Error:
				prefixMessage = "[Error] ";
				prefixColor = ConsoleColor.Red;
				break;
			case LogLevel.Critical:
				prefixMessage = "[Fatal] ";
				prefixColor = ConsoleColor.DarkRed;
				break;
			case LogLevel.None:
				prefixMessage = "[NONE] ";
				prefixColor = ConsoleColor.White;
				break;
		}

		Console.ForegroundColor = prefixColor;
		Console.Write(prefixMessage);

		string msg = formatter(state, exception);
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine(msg);
	}
}