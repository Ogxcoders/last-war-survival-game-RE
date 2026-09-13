using System.Text;

namespace Joker;

public class MessageServiceJsonDumpReplay : MessageService
{
	public MessageServiceJsonDumpReplay()
		: base(null)
	{
	}

	public override void Serialize(IMessage message, MemoryBuffer buffer, bool recurse = true)
	{
		MessageServiceJsonWithDump.MessageItem messageItem = message as MessageServiceJsonWithDump.MessageItem;
		buffer.Write(messageItem.OpCode);
		buffer.Write(messageItem.Length);
		buffer.Write(messageItem.Message.ToUtf8());
	}

	public override IMessage Deserialize(MemoryBuffer buffer, bool recurse = true)
	{
		MessageServiceJsonWithDump.MessageItem messageItem = new MessageServiceJsonWithDump.MessageItem();
		int opCode = buffer.Read<int>();
		int num = buffer.Read<int>();
		byte[] bytes = buffer.Read(num);
		string message = Encoding.UTF8.GetString(bytes);
		messageItem.IsSend = false;
		messageItem.OpCode = opCode;
		messageItem.Length = num;
		messageItem.Message = message;
		return messageItem;
	}
}
