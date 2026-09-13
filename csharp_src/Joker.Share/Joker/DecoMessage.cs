namespace Joker;

public class DecoMessage : IDecoMessage, IMessage
{
	public IMessage Message { get; set; }

	public void SerializeInner(MemoryBuffer buffer, bool recursive)
	{
		MessageService.Instance.Serialize(Message, buffer, recursive);
	}

	public void DeserializeInner(MemoryBuffer buffer, bool recursive)
	{
		Message = MessageService.Instance.Deserialize(buffer, recursive);
	}
}
