namespace Joker;

public interface IDecoMessage : IMessage
{
	IMessage Message { get; }

	void SerializeInner(MemoryBuffer buffer, bool recursive);

	void DeserializeInner(MemoryBuffer buffer, bool recursive);
}
