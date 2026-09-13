using System;

namespace Microsoft.SqlServer.Server;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = false)]
public class SqlFacetAttribute : Attribute
{
	private bool m_IsFixedLength;

	private int m_MaxSize;

	private int m_Scale;

	private int m_Precision;

	private bool m_IsNullable;

	public bool IsFixedLength
	{
		get
		{
			return m_IsFixedLength;
		}
		set
		{
			m_IsFixedLength = value;
		}
	}

	public int MaxSize
	{
		get
		{
			return m_MaxSize;
		}
		set
		{
			m_MaxSize = value;
		}
	}

	public int Precision
	{
		get
		{
			return m_Precision;
		}
		set
		{
			m_Precision = value;
		}
	}

	public int Scale
	{
		get
		{
			return m_Scale;
		}
		set
		{
			m_Scale = value;
		}
	}

	public bool IsNullable
	{
		get
		{
			return m_IsNullable;
		}
		set
		{
			m_IsNullable = value;
		}
	}
}
