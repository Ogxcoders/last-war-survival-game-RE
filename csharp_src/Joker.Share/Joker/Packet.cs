using System.IO;

namespace Joker;

public struct Packet
{
	public const int MinPacketSize = 2;

	public const int OpcodeLength = 2;

	public const int ActorIdIndex = 0;

	public const int ActorIdLength = 16;

	public ushort Opcode;

	public long ActorId;

	public MemoryStream MemoryStream;
}
