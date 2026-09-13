using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Diagnostics;

[StructLayout(LayoutKind.Explicit, Size = 16)]
[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
internal struct EventDescriptor
{
	[FieldOffset(0)]
	private ushort m_id;

	[FieldOffset(2)]
	private byte m_version;

	[FieldOffset(3)]
	private byte m_channel;

	[FieldOffset(4)]
	private byte m_level;

	[FieldOffset(5)]
	private byte m_opcode;

	[FieldOffset(6)]
	private ushort m_task;

	[FieldOffset(8)]
	private long m_keywords;

	public int EventId => m_id;

	public byte Version => m_version;

	public byte Channel => m_channel;

	public byte Level => m_level;

	public byte Opcode => m_opcode;

	public int Task => m_task;

	public long Keywords => m_keywords;

	public EventDescriptor(int id, byte version, byte channel, byte level, byte opcode, int task, long keywords)
	{
		if (id < 0)
		{
			throw Fx.Exception.ArgumentOutOfRange("id", id, "Value Must Be Non Negative");
		}
		if (id > 65535)
		{
			throw Fx.Exception.ArgumentOutOfRange("id", id, string.Empty);
		}
		m_id = (ushort)id;
		m_version = version;
		m_channel = channel;
		m_level = level;
		m_opcode = opcode;
		m_keywords = keywords;
		if (task < 0)
		{
			throw Fx.Exception.ArgumentOutOfRange("task", task, "Value Must Be Non Negative");
		}
		if (task > 65535)
		{
			throw Fx.Exception.ArgumentOutOfRange("task", task, string.Empty);
		}
		m_task = (ushort)task;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is EventDescriptor))
		{
			return false;
		}
		return Equals((EventDescriptor)obj);
	}

	public override int GetHashCode()
	{
		return m_id ^ m_version ^ m_channel ^ m_level ^ m_opcode ^ m_task ^ (int)m_keywords;
	}

	public bool Equals(EventDescriptor other)
	{
		if (m_id != other.m_id || m_version != other.m_version || m_channel != other.m_channel || m_level != other.m_level || m_opcode != other.m_opcode || m_task != other.m_task || m_keywords != other.m_keywords)
		{
			return false;
		}
		return true;
	}

	public static bool operator ==(EventDescriptor event1, EventDescriptor event2)
	{
		return event1.Equals(event2);
	}

	public static bool operator !=(EventDescriptor event1, EventDescriptor event2)
	{
		return !event1.Equals(event2);
	}
}
