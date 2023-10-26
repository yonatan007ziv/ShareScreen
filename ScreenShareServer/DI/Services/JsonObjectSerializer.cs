using Microsoft.Extensions.Logging;
using ScreenShareServer.DI.Interfaces;
using System.Text.Json;

namespace ScreenShareServer.DI.Services;

internal class JsonObjectSerializer : ISerializer
{
	private readonly ILogger logger;

	public JsonObjectSerializer(ILogger logger)
	{
		this.logger = logger;
	}

	public T Deserialize<T>(string obj) where T : class
	{
		try
		{
			return JsonSerializer.Deserialize<T>(obj)
				?? throw new JsonException();
		}
		catch (JsonException ex)
		{
			logger.LogError("JsonException Occured while Desrializing string: {serializedString}\n{exceptionString}", obj, ex.ToString());
			return null;
		}
	}

	public string Serialize<T>(T obj) where T : class
	{
		try
		{
			return JsonSerializer.Serialize(obj)
				?? throw new JsonException();
		}
		catch (JsonException ex)
		{
			logger.LogError("JsonException Occured while Serializing Data: {deserializedData}\n{exceptionString}", obj.ToString(), ex.ToString());
			return null;
		}
	}
}