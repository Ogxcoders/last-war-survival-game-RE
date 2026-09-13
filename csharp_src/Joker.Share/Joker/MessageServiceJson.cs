using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Joker;

public class MessageServiceJson : MessageService
{
	protected readonly JsonSerializerSettings _settings;

	public MessageServiceJson(List<Type> messageTypes = null, JsonSerializerSettings settings = null)
		: base(messageTypes)
	{
		if (settings == null)
		{
			_settings = new JsonSerializerSettings
			{
				Formatting = Formatting.None,
				NullValueHandling = NullValueHandling.Ignore,
				DefaultValueHandling = DefaultValueHandling.Ignore
			};
		}
		else
		{
			_settings = settings;
		}
	}

	public override void Serialize(IMessage message, MemoryBuffer buffer, bool recurse = true)
	{
		int opcode = GetOpcode(message.GetType());
		buffer.Write(opcode);
		byte[] array = JsonConvert.SerializeObject(message, message.GetType(), _settings).ToUtf8();
		buffer.Write(array.Length);
		buffer.Write(array);
	}

	public override IMessage Deserialize(MemoryBuffer buffer, bool recurse = true)
	{
		int opcode = buffer.Read<int>();
		int count = buffer.Read<int>();
		byte[] bytes = buffer.Read(count);
		return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bytes), GetType(opcode), _settings) as IMessage;
	}
}
