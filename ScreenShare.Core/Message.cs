namespace ShareScreen.Core;

public class Message
{
	public MessageType Type { get; set; }
	public string Content { get; set; }

	public Message(MessageType type, string content)
	{
		Type = type;
		Content = content;
	}

	public override string ToString()
	{
		return $"MessageType: {Type}\nContent: {Content}";
	}
}