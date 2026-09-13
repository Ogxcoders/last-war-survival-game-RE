using System.Runtime.InteropServices;

namespace Joker;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct IdStruct
{
	public short Process;

	public uint Time;

	public uint Value;

	public IdStruct(uint time, short process, uint value)
	{
		Process = process;
		Time = time;
		Value = value;
	}

	public IdStruct(long id)
	{
		ulong num = (ulong)id;
		Value = (uint)(num & 0xFFFFF);
		num >>= 20;
		Time = (uint)((int)num & -1);
		num >>= 32;
		Process = (short)(num & 0xFFF);
	}

	public long ToLong()
	{
		return (long)(((((0uL | (ulong)(ushort)Process) << 32) | Time) << 20) | Value);
	}

	public static long ToLong(uint time, short process, uint value)
	{
		return (long)(((((0uL | (ulong)(ushort)process) << 32) | time) << 20) | value);
	}

	public override string ToString()
	{
		return $"process: {Process}, time: {Time}, value: {Value}";
	}
}
