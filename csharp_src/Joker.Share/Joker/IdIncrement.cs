using System.Runtime.InteropServices;

namespace Joker;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct IdIncrement
{
	public uint Time;

	public uint Value;

	public IdIncrement(uint time, uint value)
	{
		Time = time;
		Value = value;
	}

	public IdIncrement(long id)
	{
		ulong num = (ulong)id;
		Value = (uint)(num & 0xFFFFFFFFu);
		num >>= 32;
		Time = (uint)(num & 0xFFFFFFFFu);
	}

	public long ToLong()
	{
		return (long)(((0uL | (ulong)Time) << 32) | Value);
	}

	public static long ToLong(uint time, uint value)
	{
		return (long)(((0uL | (ulong)time) << 32) | value);
	}

	public override string ToString()
	{
		return $"time: {Time}, value: {Value}";
	}
}
