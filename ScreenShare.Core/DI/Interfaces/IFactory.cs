namespace ShareScreen.Core.DI.Interfaces;

public interface IFactory<T> where T : class
{
	T Create();
}

public interface IFactory<T, InputA> where T : class
{
	T Create(InputA input);
}