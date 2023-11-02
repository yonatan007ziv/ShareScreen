using Microsoft.Extensions.Logging;
using ShareScreen.Core.DI.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;

namespace ShareScreen.Core.DI.Services.Serializers;

public class BitmapSerializer : ISerializer<Bitmap>
{
	private readonly ILogger logger;

	public BitmapSerializer(ILogger logger)
	{
		this.logger = logger;
	}

	/*
		File.WriteAllText("D:/SERIALIZATION.txt", result);

		string txt = Encoding.UTF8.GetString(bytes);
		File.WriteAllText("D:/DESERIALIZATION.txt", txt); 
	 */
	public string Serialize(Bitmap obj)
	{
		MemoryStream stream = new MemoryStream();
		obj.Save(stream, ImageFormat.Png);
		stream.Position = 0;
		string result = Convert.ToBase64String(stream.ToArray());
		stream.Dispose();
		Deserialize(result);
		return result;
	}

	public Bitmap Deserialize(string obj)
	{
		byte[] bytes = Convert.FromBase64String(obj);
		MemoryStream stream = new MemoryStream(bytes);
		Bitmap result = new Bitmap(stream);
		stream.Dispose();

		return result;
	}
}