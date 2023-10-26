namespace ScreenShareServer.DI.Interfaces;

internal interface ISerializer
{
	string Serialize<T>(T obj) where T : class;
	T Deserialize<T>(string obj) where T : class;
}