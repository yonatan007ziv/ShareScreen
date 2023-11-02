namespace ShareScreen.Core.DI.Interfaces;

public interface ISerializer<T> where T : class
{
	string Serialize(T obj);
	T Deserialize(string obj);
}

public interface ISerializer
{
	string Serialize<T>(T obj) where T : class;
	T Deserialize<T>(string obj) where T : class;
}