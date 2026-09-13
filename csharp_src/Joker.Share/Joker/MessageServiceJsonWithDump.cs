using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Joker;

public class MessageServiceJsonWithDump : MessageServiceJson
{
	public class MessageItem : IMessage
	{
		public bool IsSend;

		public double Time;

		public int OpCode;

		public int Length;

		public string Message;
	}

	public class MessageDump
	{
		public List<MessageItem> Items = new List<MessageItem>();
	}

	private const string kMessageDumpFile = "message-dump.json";

	private MessageDump _messageDump = new MessageDump();

	private Stopwatch _timer = new Stopwatch();

	public MessageServiceJsonWithDump(List<Type> messageTypes = null, JsonSerializerSettings settings = null)
		: base(messageTypes, settings)
	{
		_timer.Start();
	}

	public override void Shutdown()
	{
		base.Shutdown();
		string contents = JsonConvert.SerializeObject(_messageDump);
		File.WriteAllText("message_dump_" + DateTime.Now.ToString("MMdd_HHmmss") + "_" + Guid.NewGuid().ToString("") + ".json", contents);
	}

	public override void Serialize(IMessage message, MemoryBuffer buffer, bool recurse = true)
	{
		int opcode = GetOpcode(message.GetType());
		buffer.Write(opcode);
		string text = JsonConvert.SerializeObject(message, message.GetType(), Formatting.Indented, _settings);
		byte[] array = text.ToUtf8();
		buffer.Write(array.Length);
		buffer.Write(array);
		MessageItem item = new MessageItem
		{
			IsSend = true,
			Time = (double)_timer.ElapsedTicks / (double)Stopwatch.Frequency,
			OpCode = opcode,
			Length = array.Length,
			Message = text
		};
		_messageDump.Items.Add(item);
	}

	public override IMessage Deserialize(MemoryBuffer buffer, bool recurse = true)
	{
		int num = buffer.Read<int>();
		int num2 = buffer.Read<int>();
		byte[] bytes = buffer.Read(num2);
		string text = Encoding.UTF8.GetString(bytes);
		IMessage result = JsonConvert.DeserializeObject(text, GetType(num), _settings) as IMessage;
		MessageItem item = new MessageItem
		{
			IsSend = false,
			Time = (double)_timer.ElapsedTicks / (double)Stopwatch.Frequency,
			OpCode = num,
			Length = num2,
			Message = text
		};
		_messageDump.Items.Add(item);
		return result;
	}
}
