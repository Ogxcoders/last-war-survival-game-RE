using System;
using System.IO;

namespace Joker;

public class PacketParser
{
	private readonly CircularBuffer buffer;

	private int packetSize;

	private ParserState state;

	private readonly AService service;

	private readonly byte[] cache = new byte[8];

	public const int PacketSizeLength = 4;

	public PacketParser(CircularBuffer buffer, AService service)
	{
		this.buffer = buffer;
		this.service = service;
	}

	public bool Parse(out MemoryBuffer memoryBuffer)
	{
		while (true)
		{
			switch (state)
			{
			case ParserState.PacketSize:
				if (buffer.Length < 4)
				{
					memoryBuffer = null;
					return false;
				}
				buffer.Read(cache, 0, 4);
				packetSize = BitConverter.ToInt32(cache, 0);
				if (packetSize > 1048560 || packetSize < 2)
				{
					throw new Exception($"recv packet size error, 可能是外网探测端口: {packetSize}");
				}
				break;
			case ParserState.PacketBody:
				if (buffer.Length < packetSize)
				{
					memoryBuffer = null;
					return false;
				}
				memoryBuffer = service.Fetch(packetSize);
				buffer.Read(memoryBuffer, packetSize);
				memoryBuffer.Seek(0L, SeekOrigin.Begin);
				state = ParserState.PacketSize;
				return true;
			default:
				throw new ArgumentOutOfRangeException();
			}
			state = ParserState.PacketBody;
		}
	}
}
