namespace ShareScreen.Core.DI.Interfaces;

public interface ICommunication
{
	Task WriteMessage(Message msg);
	Task<Message> ReadMessage();
}