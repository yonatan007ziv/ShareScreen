namespace ShareScreenCore;

public class Message
{
    public MessageType Type { get; set; }
    public object Content { get; set; }

    public Message(MessageType type, object content)
    {
        Type = type;
        Content = content;
	}

	public override string ToString()
	{
        return $"MessageType: {Type}\nContent: {Content}";
	}
}