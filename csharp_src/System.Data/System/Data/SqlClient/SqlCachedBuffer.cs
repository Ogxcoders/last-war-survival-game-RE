using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;

namespace System.Data.SqlClient;

internal sealed class SqlCachedBuffer : INullable
{
	public static readonly SqlCachedBuffer Null = new SqlCachedBuffer();

	private const int _maxChunkSize = 2048;

	private List<byte[]> _cachedBytes;

	internal List<byte[]> CachedBytes => _cachedBytes;

	public bool IsNull
	{
		get
		{
			if (_cachedBytes != null)
			{
				return false;
			}
			return true;
		}
	}

	private SqlCachedBuffer()
	{
	}

	private SqlCachedBuffer(List<byte[]> cachedBytes)
	{
		_cachedBytes = cachedBytes;
	}

	internal static bool TryCreate(SqlMetaDataPriv metadata, TdsParser parser, TdsParserStateObject stateObj, out SqlCachedBuffer buffer)
	{
		int num = 0;
		List<byte[]> list = new List<byte[]>();
		buffer = null;
		if (!parser.TryPlpBytesLeft(stateObj, out var left))
		{
			return false;
		}
		while (left != 0L)
		{
			do
			{
				num = (int)((left > 2048) ? 2048 : left);
				byte[] buff = new byte[num];
				if (!stateObj.TryReadPlpBytes(ref buff, 0, num, out num))
				{
					return false;
				}
				if (list.Count == 0)
				{
					AddByteOrderMark(buff, list);
				}
				list.Add(buff);
				left -= (ulong)num;
			}
			while (left != 0);
			if (!parser.TryPlpBytesLeft(stateObj, out left))
			{
				return false;
			}
			if (left == 0)
			{
				break;
			}
		}
		buffer = new SqlCachedBuffer(list);
		return true;
	}

	private static void AddByteOrderMark(byte[] byteArr, List<byte[]> cachedBytes)
	{
		if (byteArr.Length < 2 || byteArr[0] != 223 || byteArr[1] != byte.MaxValue)
		{
			cachedBytes.Add(TdsEnums.XMLUNICODEBOMBYTES);
		}
	}

	internal Stream ToStream()
	{
		return new SqlCachedStream(this);
	}

	public override string ToString()
	{
		if (IsNull)
		{
			throw new SqlNullValueException();
		}
		if (_cachedBytes.Count == 0)
		{
			return string.Empty;
		}
		return new SqlXml(ToStream()).Value;
	}

	internal SqlString ToSqlString()
	{
		if (IsNull)
		{
			return SqlString.Null;
		}
		return new SqlString(ToString());
	}

	internal SqlXml ToSqlXml()
	{
		return new SqlXml(ToStream());
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal XmlReader ToXmlReader()
	{
		return SqlTypeWorkarounds.SqlXmlCreateSqlXmlReader(ToStream());
	}
}
