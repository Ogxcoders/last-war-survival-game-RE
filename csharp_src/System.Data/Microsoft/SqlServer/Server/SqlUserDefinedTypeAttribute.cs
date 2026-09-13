using System;
using System.Data.Common;

namespace Microsoft.SqlServer.Server;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
public sealed class SqlUserDefinedTypeAttribute : Attribute
{
	private int m_MaxByteSize;

	private bool m_IsFixedLength;

	private bool m_IsByteOrdered;

	private Format m_format;

	private string m_fName;

	internal const int YukonMaxByteSizeValue = 8000;

	private string m_ValidationMethodName;

	public int MaxByteSize
	{
		get
		{
			return m_MaxByteSize;
		}
		set
		{
			if (value < -1)
			{
				throw ADP.ArgumentOutOfRange("MaxByteSize");
			}
			m_MaxByteSize = value;
		}
	}

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

	public bool IsByteOrdered
	{
		get
		{
			return m_IsByteOrdered;
		}
		set
		{
			m_IsByteOrdered = value;
		}
	}

	public Format Format => m_format;

	public string ValidationMethodName
	{
		get
		{
			return m_ValidationMethodName;
		}
		set
		{
			m_ValidationMethodName = value;
		}
	}

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

	public SqlUserDefinedTypeAttribute(Format format)
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
