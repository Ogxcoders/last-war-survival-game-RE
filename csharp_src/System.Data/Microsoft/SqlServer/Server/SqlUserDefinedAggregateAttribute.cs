using System;
using System.Data.Common;

namespace Microsoft.SqlServer.Server;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class SqlUserDefinedAggregateAttribute : Attribute
{
	private int m_MaxByteSize;

	private bool m_fInvariantToDup;

	private bool m_fInvariantToNulls;

	private bool m_fInvariantToOrder = true;

	private bool m_fNullIfEmpty;

	private Format m_format;

	private string m_fName;

	public const int MaxByteSizeValue = 8000;

	public int MaxByteSize
	{
		get
		{
			return m_MaxByteSize;
		}
		set
		{
			if (value < -1 || value > 8000)
			{
				throw ADP.ArgumentOutOfRange(Res.GetString("range: 0-8000"), "MaxByteSize", value);
			}
			m_MaxByteSize = value;
		}
	}

	public bool IsInvariantToDuplicates
	{
		get
		{
			return m_fInvariantToDup;
		}
		set
		{
			m_fInvariantToDup = value;
		}
	}

	public bool IsInvariantToNulls
	{
		get
		{
			return m_fInvariantToNulls;
		}
		set
		{
			m_fInvariantToNulls = value;
		}
	}

	public bool IsInvariantToOrder
	{
		get
		{
			return m_fInvariantToOrder;
		}
		set
		{
			m_fInvariantToOrder = value;
		}
	}

	public bool IsNullIfEmpty
	{
		get
		{
			return m_fNullIfEmpty;
		}
		set
		{
			m_fNullIfEmpty = value;
		}
	}

	public Format Format => m_format;

	public string Name
	{
		get
		{
			return m_fName;
		}
		set
		{
			m_fName = value;
		}
	}

	public SqlUserDefinedAggregateAttribute(Format format)
	{
		switch (format)
		{
		case Format.Unknown:
			throw ADP.NotSupportedUserDefinedTypeSerializationFormat(format, "format");
		case Format.Native:
		case Format.UserDefined:
			m_format = format;
			break;
		default:
			throw ADP.InvalidUserDefinedTypeSerializationFormat(format);
		}
	}
}
