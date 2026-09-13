using System;
using System.Collections.Generic;

namespace Joker;

public class MessageServiceMemoryPack : MessageService
{
	public MessageServiceMemoryPack(List<Type> messageTypes = null)
		: base(messageTypes)
	{
	}

	public override void Serialize(IMessage message, MemoryBuffer buffer, bool recurse = true)
	{
	}

	public override IMessage Deserialize(MemoryBuffer buffer, bool recurse = true)
	{
		return null;
	}
}
