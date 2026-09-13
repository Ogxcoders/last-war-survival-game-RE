using System;

namespace SimpleProto;

public sealed class ProtoBufAttribute : Attribute
{
	private int m_index;

	public int Index
	{
		get
		{
			return m_index;
		}
		set
		{
			m_index = value;
		}
	}

	public ProtoBufAttribute(int index)
	{
		m_index = index;
	}
}
