using System;

namespace Joker;

public interface IMessageService : IService
{
	void Serialize(IMessage message, MemoryBuffer buffer, bool recurse = true);

	IMessage Deserialize(MemoryBuffer buffer, bool recurse = true);

	IMessage CloneMessage(IMessage message, bool recurse = true);

	int GetOpcode(Type type);

	Type GetType(int opcode);

	Type GetResponseType(Type requestType);
}
