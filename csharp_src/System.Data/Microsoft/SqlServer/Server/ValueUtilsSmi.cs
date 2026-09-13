using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Microsoft.SqlServer.Server;

internal static class ValueUtilsSmi
{
	private const int __maxByteChunkSize = 8000;

	private const int __maxCharChunkSize = 4000;

	private const int NoLengthLimit = -1;

	private const int constBinBufferSize = 4096;

	private const int constTextBufferSize = 4096;

	private static object[] s_typeSpecificNullForSqlValue = new object[35]
	{
		SqlInt64.Null,
		SqlBinary.Null,
		SqlBoolean.Null,
		SqlString.Null,
		SqlDateTime.Null,
		SqlDecimal.Null,
		SqlDouble.Null,
		SqlBinary.Null,
		SqlInt32.Null,
		SqlMoney.Null,
		SqlString.Null,
		SqlString.Null,
		SqlString.Null,
		SqlSingle.Null,
		SqlGuid.Null,
		SqlDateTime.Null,
		SqlInt16.Null,
		SqlMoney.Null,
		SqlString.Null,
		SqlBinary.Null,
		SqlByte.Null,
		SqlBinary.Null,
		SqlString.Null,
		DBNull.Value,
		null,
		SqlXml.Null,
		null,
		null,
		null,
		null,
		null,
		DBNull.Value,
		DBNull.Value,
		DBNull.Value,
		DBNull.Value
	};

	private static readonly DateTime s_dtSmallMax = new DateTime(2079, 6, 6, 23, 59, 29, 998);

	private static readonly DateTime s_dtSmallMin = new DateTime(1899, 12, 31, 23, 59, 29, 999);

	private static readonly TimeSpan s_timeMin = TimeSpan.Zero;

	private static readonly TimeSpan s_timeMax = new TimeSpan(863999999999L);

	private const bool X = true;

	private const bool _ = false;

	private static bool[,] s_canAccessGetterDirectly = new bool[45, 35]
	{
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, true, false, false, false, false, false, false,
			true, true, true, false, false, false, false, false, true, false,
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, true, false, false, false, false, false,
			false, false, false, false, false, true, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, true, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, true, false, false, false, true,
			false, false, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, true, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, true, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, true, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			true, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, true, false, false, false, false, false, false,
			true, true, true, false, false, false, false, false, true, false,
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, true,
			false, false, false, false, false
		},
		{
			false, true, false, true, false, false, false, true, false, false,
			true, true, true, false, false, false, false, false, true, true,
			false, true, true, false, false, true, false, false, false, true,
			false, false, false, false, false
		},
		{
			false, false, false, true, false, false, false, false, false, false,
			true, true, true, false, false, false, false, false, true, false,
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, true, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, true, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false, false, false, true,
			false, true, false, false, false, false, false, false, false, true,
			false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, true, false, false, false, false, false,
			false, false, false, false, false, true, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, true, false
		},
		{
			false, false, false, false, false, false, true, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, true, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, true, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, true, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			true, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, true,
			false, false, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, true, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, true, false, false, false, false, false, false,
			true, true, true, false, false, false, false, false, true, false,
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, true, false, false, false, false, false, false,
			true, true, true, false, false, false, false, false, true, false,
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, true, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false, false, false, true,
			false, true, false, false, false, false, false, false, false, true,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, true, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, true, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, true
		},
		{
			false, true, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, false, false, false, false, false, false, true,
			false, false, false, false, false
		},
		{
			false, false, false, true, false, false, false, false, false, false,
			true, true, true, false, false, false, false, false, true, false,
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		}
	};

	private static bool[,] s_canAccessSetterDirectly = new bool[45, 35]
	{
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, true, false, false, false, false, false, false,
			true, true, true, false, false, false, false, false, true, false,
			false, false, true, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, true, false, false, false, false, false,
			false, false, false, false, false, true, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, true, false, true, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, true, false, false, false, true,
			false, false, false, false, false, false, false, true, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, true, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, true, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, true, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			true, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, true, false, false, false, false, false, false,
			true, true, true, false, false, false, false, false, true, false,
			false, false, true, true, false, true, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, true,
			false, false, false, false, false
		},
		{
			false, true, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false, false, false, true,
			false, true, false, true, false, true, false, false, false, true,
			false, false, false, false, false
		},
		{
			false, false, false, true, false, false, false, false, false, false,
			true, true, true, false, false, false, false, false, true, false,
			false, false, true, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, true, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, true, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false, false, false, true,
			false, true, false, true, false, false, false, false, false, true,
			false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, true, false, false, false, false, false,
			false, false, false, false, false, true, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, true, false, true, false
		},
		{
			false, false, false, false, false, false, true, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, true, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, true, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, true, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			true, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, true,
			false, false, false, false, false, false, false, true, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, true, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, true, false, false, false, false, false, false,
			true, true, true, false, false, false, false, false, true, false,
			false, false, true, true, false, true, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, true, false, false, false, false, false, false,
			true, true, true, false, false, false, false, false, true, false,
			false, false, true, true, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, true, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false, false, false, true,
			false, true, false, true, false, false, false, false, false, true,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, true, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, true, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, true
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false
		}
	};

	internal static bool IsDBNull(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		return IsDBNull_Unchecked(sink, getters, ordinal);
	}

	internal static bool GetBoolean(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.Boolean))
		{
			return GetBoolean_Unchecked(sink, getters, ordinal);
		}
		object value = GetValue(sink, getters, ordinal, metaData);
		if (value == null)
		{
			throw ADP.InvalidCast();
		}
		return (bool)value;
	}

	internal static byte GetByte(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.Byte))
		{
			return GetByte_Unchecked(sink, getters, ordinal);
		}
		object value = GetValue(sink, getters, ordinal, metaData);
		if (value == null)
		{
			throw ADP.InvalidCast();
		}
		return (byte)value;
	}

	private static long GetBytesConversion(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData, long fieldOffset, byte[] buffer, int bufferOffset, int length, bool throwOnNull)
	{
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		SqlBinary sqlBinary = (SqlBinary)sqlValue;
		if (sqlBinary.IsNull)
		{
			if (throwOnNull)
			{
				throw SQL.SqlNullValue();
			}
			return 0L;
		}
		if (buffer == null)
		{
			return sqlBinary.Length;
		}
		length = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength * 2, sqlBinary.Length, fieldOffset, buffer.Length, bufferOffset, length);
		Array.Copy(sqlBinary.Value, checked((int)fieldOffset), buffer, bufferOffset, length);
		return length;
	}

	internal static long GetBytes(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiExtendedMetaData metaData, long fieldOffset, byte[] buffer, int bufferOffset, int length, bool throwOnNull)
	{
		if ((-1 != metaData.MaxLength && (SqlDbType.VarChar == metaData.SqlDbType || SqlDbType.NVarChar == metaData.SqlDbType || SqlDbType.Char == metaData.SqlDbType || SqlDbType.NChar == metaData.SqlDbType)) || SqlDbType.Xml == metaData.SqlDbType)
		{
			throw SQL.NonBlobColumn(metaData.Name);
		}
		return GetBytesInternal(sink, getters, ordinal, metaData, fieldOffset, buffer, bufferOffset, length, throwOnNull);
	}

	internal static long GetBytesInternal(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData, long fieldOffset, byte[] buffer, int bufferOffset, int length, bool throwOnNull)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.ByteArray))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				if (throwOnNull)
				{
					throw SQL.SqlNullValue();
				}
				CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, 0L, fieldOffset, buffer.Length, bufferOffset, length);
				return 0L;
			}
			long bytesLength_Unchecked = GetBytesLength_Unchecked(sink, getters, ordinal);
			if (buffer == null)
			{
				return bytesLength_Unchecked;
			}
			length = ((!MetaDataUtilsSmi.IsCharOrXmlType(metaData.SqlDbType)) ? CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, bytesLength_Unchecked, fieldOffset, buffer.Length, bufferOffset, length) : CheckXetParameters(metaData.SqlDbType, metaData.MaxLength * 2, bytesLength_Unchecked, fieldOffset, buffer.Length, bufferOffset, length));
			if (length > 0)
			{
				length = GetBytes_Unchecked(sink, getters, ordinal, fieldOffset, buffer, bufferOffset, length);
			}
			return length;
		}
		return GetBytesConversion(sink, getters, ordinal, metaData, fieldOffset, buffer, bufferOffset, length, throwOnNull);
	}

	internal static long GetChars(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData, long fieldOffset, char[] buffer, int bufferOffset, int length)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.CharArray))
		{
			long charsLength_Unchecked = GetCharsLength_Unchecked(sink, getters, ordinal);
			if (buffer == null)
			{
				return charsLength_Unchecked;
			}
			length = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, charsLength_Unchecked, fieldOffset, buffer.Length, bufferOffset, length);
			if (length > 0)
			{
				length = GetChars_Unchecked(sink, getters, ordinal, fieldOffset, buffer, bufferOffset, length);
			}
			return length;
		}
		string text = (string)GetValue(sink, getters, ordinal, metaData);
		if (text == null)
		{
			throw ADP.InvalidCast();
		}
		if (buffer == null)
		{
			return text.Length;
		}
		length = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength * 2, text.Length, fieldOffset, buffer.Length, bufferOffset, length);
		text.CopyTo(checked((int)fieldOffset), buffer, bufferOffset, length);
		return length;
	}

	internal static DateTime GetDateTime(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.DateTime))
		{
			return GetDateTime_Unchecked(sink, getters, ordinal);
		}
		object value = GetValue(sink, getters, ordinal, metaData);
		if (value == null)
		{
			throw ADP.InvalidCast();
		}
		return (DateTime)value;
	}

	internal static DateTimeOffset GetDateTimeOffset(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData, bool gettersSupportKatmaiDateTime)
	{
		if (gettersSupportKatmaiDateTime)
		{
			return GetDateTimeOffset(sink, (SmiTypedGetterSetter)getters, ordinal, metaData);
		}
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		object value = GetValue(sink, getters, ordinal, metaData);
		if (value == null)
		{
			throw ADP.InvalidCast();
		}
		return (DateTimeOffset)value;
	}

	internal static DateTimeOffset GetDateTimeOffset(SmiEventSink_Default sink, SmiTypedGetterSetter getters, int ordinal, SmiMetaData metaData)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.DateTimeOffset))
		{
			return GetDateTimeOffset_Unchecked(sink, getters, ordinal);
		}
		return (DateTimeOffset)GetValue200(sink, getters, ordinal, metaData);
	}

	internal static decimal GetDecimal(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.Decimal))
		{
			return GetDecimal_PossiblyMoney(sink, getters, ordinal, metaData);
		}
		object value = GetValue(sink, getters, ordinal, metaData);
		if (value == null)
		{
			throw ADP.InvalidCast();
		}
		return (decimal)value;
	}

	internal static double GetDouble(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.Double))
		{
			return GetDouble_Unchecked(sink, getters, ordinal);
		}
		object value = GetValue(sink, getters, ordinal, metaData);
		if (value == null)
		{
			throw ADP.InvalidCast();
		}
		return (double)value;
	}

	internal static Guid GetGuid(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.Guid))
		{
			return GetGuid_Unchecked(sink, getters, ordinal);
		}
		object value = GetValue(sink, getters, ordinal, metaData);
		if (value == null)
		{
			throw ADP.InvalidCast();
		}
		return (Guid)value;
	}

	internal static short GetInt16(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.Int16))
		{
			return GetInt16_Unchecked(sink, getters, ordinal);
		}
		object value = GetValue(sink, getters, ordinal, metaData);
		if (value == null)
		{
			throw ADP.InvalidCast();
		}
		return (short)value;
	}

	internal static int GetInt32(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.Int32))
		{
			return GetInt32_Unchecked(sink, getters, ordinal);
		}
		object value = GetValue(sink, getters, ordinal, metaData);
		if (value == null)
		{
			throw ADP.InvalidCast();
		}
		return (int)value;
	}

	internal static long GetInt64(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.Int64))
		{
			return GetInt64_Unchecked(sink, getters, ordinal);
		}
		object value = GetValue(sink, getters, ordinal, metaData);
		if (value == null)
		{
			throw ADP.InvalidCast();
		}
		return (long)value;
	}

	internal static float GetSingle(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.Single))
		{
			return GetSingle_Unchecked(sink, getters, ordinal);
		}
		object value = GetValue(sink, getters, ordinal, metaData);
		if (value == null)
		{
			throw ADP.InvalidCast();
		}
		return (float)value;
	}

	internal static SqlBinary GetSqlBinary(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlBinary))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlBinary.Null;
			}
			return GetSqlBinary_Unchecked(sink, getters, ordinal);
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		return (SqlBinary)sqlValue;
	}

	internal static SqlBoolean GetSqlBoolean(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlBoolean))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlBoolean.Null;
			}
			return new SqlBoolean(GetBoolean_Unchecked(sink, getters, ordinal));
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		return (SqlBoolean)sqlValue;
	}

	internal static SqlByte GetSqlByte(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlByte))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlByte.Null;
			}
			return new SqlByte(GetByte_Unchecked(sink, getters, ordinal));
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		return (SqlByte)sqlValue;
	}

	internal static SqlBytes GetSqlBytes(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlBytes))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlBytes.Null;
			}
			long bytesLength_Unchecked = GetBytesLength_Unchecked(sink, getters, ordinal);
			if (0 <= bytesLength_Unchecked && bytesLength_Unchecked < 8000)
			{
				return new SqlBytes(GetByteArray_Unchecked(sink, getters, ordinal));
			}
			return new SqlBytes(CopyIntoNewSmiScratchStream(new SmiGettersStream(sink, getters, ordinal, metaData), sink));
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		SqlBinary sqlBinary = (SqlBinary)sqlValue;
		if (sqlBinary.IsNull)
		{
			return SqlBytes.Null;
		}
		return new SqlBytes(sqlBinary.Value);
	}

	internal static SqlChars GetSqlChars(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlChars))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlChars.Null;
			}
			return new SqlChars(GetCharArray_Unchecked(sink, getters, ordinal));
		}
		if (SqlDbType.Xml == metaData.SqlDbType)
		{
			SqlXml sqlXml_Unchecked = GetSqlXml_Unchecked(sink, getters, ordinal);
			if (sqlXml_Unchecked.IsNull)
			{
				return SqlChars.Null;
			}
			return new SqlChars(sqlXml_Unchecked.Value.ToCharArray());
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		SqlString sqlString = (SqlString)sqlValue;
		if (sqlString.IsNull)
		{
			return SqlChars.Null;
		}
		return new SqlChars(sqlString.Value.ToCharArray());
	}

	internal static SqlDateTime GetSqlDateTime(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlDateTime))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlDateTime.Null;
			}
			DateTime dateTime_Unchecked = GetDateTime_Unchecked(sink, getters, ordinal);
			return new SqlDateTime(dateTime_Unchecked);
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		return (SqlDateTime)sqlValue;
	}

	internal static SqlDecimal GetSqlDecimal(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlDecimal))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlDecimal.Null;
			}
			return GetSqlDecimal_Unchecked(sink, getters, ordinal);
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		return (SqlDecimal)sqlValue;
	}

	internal static SqlDouble GetSqlDouble(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlDouble))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlDouble.Null;
			}
			double double_Unchecked = GetDouble_Unchecked(sink, getters, ordinal);
			return new SqlDouble(double_Unchecked);
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		return (SqlDouble)sqlValue;
	}

	internal static SqlGuid GetSqlGuid(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlGuid))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlGuid.Null;
			}
			Guid guid_Unchecked = GetGuid_Unchecked(sink, getters, ordinal);
			return new SqlGuid(guid_Unchecked);
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		return (SqlGuid)sqlValue;
	}

	internal static SqlInt16 GetSqlInt16(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlInt16))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlInt16.Null;
			}
			short int16_Unchecked = GetInt16_Unchecked(sink, getters, ordinal);
			return new SqlInt16(int16_Unchecked);
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		return (SqlInt16)sqlValue;
	}

	internal static SqlInt32 GetSqlInt32(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlInt32))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlInt32.Null;
			}
			int int32_Unchecked = GetInt32_Unchecked(sink, getters, ordinal);
			return new SqlInt32(int32_Unchecked);
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		return (SqlInt32)sqlValue;
	}

	internal static SqlInt64 GetSqlInt64(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlInt64))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlInt64.Null;
			}
			long int64_Unchecked = GetInt64_Unchecked(sink, getters, ordinal);
			return new SqlInt64(int64_Unchecked);
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		return (SqlInt64)sqlValue;
	}

	internal static SqlMoney GetSqlMoney(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlMoney))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlMoney.Null;
			}
			return GetSqlMoney_Unchecked(sink, getters, ordinal);
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		return (SqlMoney)sqlValue;
	}

	internal static SqlSingle GetSqlSingle(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlSingle))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlSingle.Null;
			}
			float single_Unchecked = GetSingle_Unchecked(sink, getters, ordinal);
			return new SqlSingle(single_Unchecked);
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		return (SqlSingle)sqlValue;
	}

	internal static SqlString GetSqlString(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		SqlString result;
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlString))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlString.Null;
			}
			string string_Unchecked = GetString_Unchecked(sink, getters, ordinal);
			result = new SqlString(string_Unchecked);
		}
		else
		{
			if (SqlDbType.Xml != metaData.SqlDbType)
			{
				object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
				if (sqlValue == null)
				{
					throw ADP.InvalidCast();
				}
				return (SqlString)sqlValue;
			}
			SqlXml sqlXml_Unchecked = GetSqlXml_Unchecked(sink, getters, ordinal);
			if (sqlXml_Unchecked.IsNull)
			{
				return SqlString.Null;
			}
			result = new SqlString(sqlXml_Unchecked.Value);
		}
		return result;
	}

	internal static SqlXml GetSqlXml(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.SqlXml))
		{
			if (IsDBNull_Unchecked(sink, getters, ordinal))
			{
				return SqlXml.Null;
			}
			return GetSqlXml_Unchecked(sink, getters, ordinal);
		}
		object sqlValue = GetSqlValue(sink, getters, ordinal, metaData);
		if (sqlValue == null)
		{
			throw ADP.InvalidCast();
		}
		return (SqlXml)sqlValue;
	}

	internal static string GetString(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.String))
		{
			return GetString_Unchecked(sink, getters, ordinal);
		}
		object value = GetValue(sink, getters, ordinal, metaData);
		if (value == null)
		{
			throw ADP.InvalidCast();
		}
		return (string)value;
	}

	internal static TimeSpan GetTimeSpan(SmiEventSink_Default sink, SmiTypedGetterSetter getters, int ordinal, SmiMetaData metaData)
	{
		ThrowIfITypedGettersIsNull(sink, getters, ordinal);
		if (CanAccessGetterDirectly(metaData, ExtendedClrTypeCode.TimeSpan))
		{
			return GetTimeSpan_Unchecked(sink, getters, ordinal);
		}
		return (TimeSpan)GetValue200(sink, getters, ordinal, metaData);
	}

	internal static object GetValue200(SmiEventSink_Default sink, SmiTypedGetterSetter getters, int ordinal, SmiMetaData metaData)
	{
		object obj = null;
		if (IsDBNull_Unchecked(sink, getters, ordinal))
		{
			return DBNull.Value;
		}
		switch (metaData.SqlDbType)
		{
		case SqlDbType.Variant:
			metaData = getters.GetVariantType(sink, ordinal);
			sink.ProcessMessagesAndThrow();
			return GetValue200(sink, getters, ordinal, metaData);
		case SqlDbType.Date:
		case SqlDbType.DateTime2:
			return GetDateTime_Unchecked(sink, getters, ordinal);
		case SqlDbType.Time:
			return GetTimeSpan_Unchecked(sink, getters, ordinal);
		case SqlDbType.DateTimeOffset:
			return GetDateTimeOffset_Unchecked(sink, getters, ordinal);
		default:
			return GetValue(sink, getters, ordinal, metaData);
		}
	}

	internal static object GetValue(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		object result = null;
		if (IsDBNull_Unchecked(sink, getters, ordinal))
		{
			result = DBNull.Value;
		}
		else
		{
			switch (metaData.SqlDbType)
			{
			case SqlDbType.BigInt:
				result = GetInt64_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Binary:
				result = GetByteArray_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Bit:
				result = GetBoolean_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Char:
				result = GetString_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.DateTime:
				result = GetDateTime_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Decimal:
				result = GetSqlDecimal_Unchecked(sink, getters, ordinal).Value;
				break;
			case SqlDbType.Float:
				result = GetDouble_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Image:
				result = GetByteArray_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Int:
				result = GetInt32_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Money:
				result = GetSqlMoney_Unchecked(sink, getters, ordinal).Value;
				break;
			case SqlDbType.NChar:
				result = GetString_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.NText:
				result = GetString_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.NVarChar:
				result = GetString_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Real:
				result = GetSingle_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.UniqueIdentifier:
				result = GetGuid_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.SmallDateTime:
				result = GetDateTime_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.SmallInt:
				result = GetInt16_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.SmallMoney:
				result = GetSqlMoney_Unchecked(sink, getters, ordinal).Value;
				break;
			case SqlDbType.Text:
				result = GetString_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Timestamp:
				result = GetByteArray_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.TinyInt:
				result = GetByte_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.VarBinary:
				result = GetByteArray_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.VarChar:
				result = GetString_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Variant:
				metaData = getters.GetVariantType(sink, ordinal);
				sink.ProcessMessagesAndThrow();
				result = GetValue(sink, getters, ordinal, metaData);
				break;
			case SqlDbType.Xml:
				result = GetSqlXml_Unchecked(sink, getters, ordinal).Value;
				break;
			case SqlDbType.Udt:
				result = GetUdt_LengthChecked(sink, getters, ordinal, metaData);
				break;
			}
		}
		return result;
	}

	internal static object GetSqlValue200(SmiEventSink_Default sink, SmiTypedGetterSetter getters, int ordinal, SmiMetaData metaData)
	{
		object obj = null;
		if (IsDBNull_Unchecked(sink, getters, ordinal))
		{
			if (SqlDbType.Udt == metaData.SqlDbType)
			{
				return NullUdtInstance(metaData);
			}
			return s_typeSpecificNullForSqlValue[(int)metaData.SqlDbType];
		}
		switch (metaData.SqlDbType)
		{
		case SqlDbType.Variant:
			metaData = getters.GetVariantType(sink, ordinal);
			sink.ProcessMessagesAndThrow();
			return GetSqlValue200(sink, getters, ordinal, metaData);
		case SqlDbType.Date:
		case SqlDbType.DateTime2:
			return GetDateTime_Unchecked(sink, getters, ordinal);
		case SqlDbType.Time:
			return GetTimeSpan_Unchecked(sink, getters, ordinal);
		case SqlDbType.DateTimeOffset:
			return GetDateTimeOffset_Unchecked(sink, getters, ordinal);
		default:
			return GetSqlValue(sink, getters, ordinal, metaData);
		}
	}

	internal static object GetSqlValue(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		object result = null;
		if (IsDBNull_Unchecked(sink, getters, ordinal))
		{
			result = ((SqlDbType.Udt != metaData.SqlDbType) ? s_typeSpecificNullForSqlValue[(int)metaData.SqlDbType] : NullUdtInstance(metaData));
		}
		else
		{
			switch (metaData.SqlDbType)
			{
			case SqlDbType.BigInt:
				result = new SqlInt64(GetInt64_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.Binary:
				result = GetSqlBinary_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Bit:
				result = new SqlBoolean(GetBoolean_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.Char:
				result = new SqlString(GetString_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.DateTime:
				result = new SqlDateTime(GetDateTime_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.Decimal:
				result = GetSqlDecimal_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Float:
				result = new SqlDouble(GetDouble_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.Image:
				result = GetSqlBinary_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Int:
				result = new SqlInt32(GetInt32_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.Money:
				result = GetSqlMoney_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.NChar:
				result = new SqlString(GetString_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.NText:
				result = new SqlString(GetString_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.NVarChar:
				result = new SqlString(GetString_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.Real:
				result = new SqlSingle(GetSingle_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.UniqueIdentifier:
				result = new SqlGuid(GetGuid_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.SmallDateTime:
				result = new SqlDateTime(GetDateTime_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.SmallInt:
				result = new SqlInt16(GetInt16_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.SmallMoney:
				result = GetSqlMoney_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Text:
				result = new SqlString(GetString_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.Timestamp:
				result = GetSqlBinary_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.TinyInt:
				result = new SqlByte(GetByte_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.VarBinary:
				result = GetSqlBinary_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.VarChar:
				result = new SqlString(GetString_Unchecked(sink, getters, ordinal));
				break;
			case SqlDbType.Variant:
				metaData = getters.GetVariantType(sink, ordinal);
				sink.ProcessMessagesAndThrow();
				result = GetSqlValue(sink, getters, ordinal, metaData);
				break;
			case SqlDbType.Xml:
				result = GetSqlXml_Unchecked(sink, getters, ordinal);
				break;
			case SqlDbType.Udt:
				result = GetUdt_LengthChecked(sink, getters, ordinal, metaData);
				break;
			}
		}
		return result;
	}

	internal static object NullUdtInstance(SmiMetaData metaData)
	{
		return metaData.Type.InvokeMember("Null", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty, null, null, new object[0], CultureInfo.InvariantCulture);
	}

	internal static void SetDBNull(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, bool value)
	{
		SetDBNull_Unchecked(sink, setters, ordinal);
	}

	internal static void SetBoolean(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, bool value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.Boolean);
		SetBoolean_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetByte(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, byte value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.Byte);
		SetByte_Unchecked(sink, setters, ordinal, value);
	}

	internal static long SetBytes(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, long fieldOffset, byte[] buffer, int bufferOffset, int length)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.ByteArray);
		if (buffer == null)
		{
			throw ADP.ArgumentNull("buffer");
		}
		length = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, -1L, fieldOffset, buffer.Length, bufferOffset, length);
		if (length == 0)
		{
			fieldOffset = 0L;
			bufferOffset = 0;
		}
		return SetBytes_Unchecked(sink, setters, ordinal, fieldOffset, buffer, bufferOffset, length);
	}

	internal static long SetBytesLength(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, long length)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.ByteArray);
		if (length < 0)
		{
			throw ADP.InvalidDataLength(length);
		}
		if (metaData.MaxLength >= 0 && length > metaData.MaxLength)
		{
			length = metaData.MaxLength;
		}
		setters.SetBytesLength(sink, ordinal, length);
		sink.ProcessMessagesAndThrow();
		return length;
	}

	internal static long SetChars(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, long fieldOffset, char[] buffer, int bufferOffset, int length)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.CharArray);
		if (buffer == null)
		{
			throw ADP.ArgumentNull("buffer");
		}
		length = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, -1L, fieldOffset, buffer.Length, bufferOffset, length);
		if (length == 0)
		{
			fieldOffset = 0L;
			bufferOffset = 0;
		}
		return SetChars_Unchecked(sink, setters, ordinal, fieldOffset, buffer, bufferOffset, length);
	}

	internal static void SetDateTime(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, DateTime value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.DateTime);
		SetDateTime_Checked(sink, setters, ordinal, metaData, value);
	}

	internal static void SetDateTimeOffset(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, DateTimeOffset value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.DateTimeOffset);
		SetDateTimeOffset_Unchecked(sink, (SmiTypedGetterSetter)setters, ordinal, value);
	}

	internal static void SetDecimal(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, decimal value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.Decimal);
		SetDecimal_PossiblyMoney(sink, setters, ordinal, metaData, value);
	}

	internal static void SetDouble(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, double value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.Double);
		SetDouble_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetGuid(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, Guid value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.Guid);
		SetGuid_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetInt16(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, short value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.Int16);
		SetInt16_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetInt32(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, int value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.Int32);
		SetInt32_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetInt64(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, long value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.Int64);
		SetInt64_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetSingle(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, float value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.Single);
		SetSingle_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetSqlBinary(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlBinary value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlBinary);
		SetSqlBinary_LengthChecked(sink, setters, ordinal, metaData, value, 0);
	}

	internal static void SetSqlBoolean(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlBoolean value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlBoolean);
		SetSqlBoolean_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetSqlByte(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlByte value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlByte);
		SetSqlByte_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetSqlBytes(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlBytes value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlBytes);
		SetSqlBytes_LengthChecked(sink, setters, ordinal, metaData, value, 0);
	}

	internal static void SetSqlChars(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlChars value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlChars);
		SetSqlChars_LengthChecked(sink, setters, ordinal, metaData, value, 0);
	}

	internal static void SetSqlDateTime(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlDateTime value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlDateTime);
		SetSqlDateTime_Checked(sink, setters, ordinal, metaData, value);
	}

	internal static void SetSqlDecimal(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlDecimal value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlDecimal);
		SetSqlDecimal_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetSqlDouble(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlDouble value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlDouble);
		SetSqlDouble_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetSqlGuid(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlGuid value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlGuid);
		SetSqlGuid_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetSqlInt16(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlInt16 value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlInt16);
		SetSqlInt16_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetSqlInt32(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlInt32 value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlInt32);
		SetSqlInt32_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetSqlInt64(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlInt64 value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlInt64);
		SetSqlInt64_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetSqlMoney(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlMoney value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlMoney);
		SetSqlMoney_Checked(sink, setters, ordinal, metaData, value);
	}

	internal static void SetSqlSingle(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlSingle value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlSingle);
		SetSqlSingle_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetSqlString(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlString value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlString);
		SetSqlString_LengthChecked(sink, setters, ordinal, metaData, value, 0);
	}

	internal static void SetSqlXml(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlXml value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.SqlXml);
		SetSqlXml_Unchecked(sink, setters, ordinal, value);
	}

	internal static void SetString(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, string value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.String);
		SetString_LengthChecked(sink, setters, ordinal, metaData, value, 0);
	}

	internal static void SetTimeSpan(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, TimeSpan value)
	{
		ThrowIfInvalidSetterAccess(metaData, ExtendedClrTypeCode.TimeSpan);
		SetTimeSpan_Checked(sink, (SmiTypedGetterSetter)setters, ordinal, metaData, value);
	}

	internal static void SetCompatibleValue(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, object value, ExtendedClrTypeCode typeCode, int offset)
	{
		switch (typeCode)
		{
		case ExtendedClrTypeCode.Invalid:
			throw ADP.UnknownDataType(value.GetType());
		case ExtendedClrTypeCode.Boolean:
			SetBoolean_Unchecked(sink, setters, ordinal, (bool)value);
			break;
		case ExtendedClrTypeCode.Byte:
			SetByte_Unchecked(sink, setters, ordinal, (byte)value);
			break;
		case ExtendedClrTypeCode.Char:
		{
			char[] value2 = new char[1] { (char)value };
			SetCompatibleValue(sink, setters, ordinal, metaData, value2, ExtendedClrTypeCode.CharArray, 0);
			break;
		}
		case ExtendedClrTypeCode.DateTime:
			SetDateTime_Checked(sink, setters, ordinal, metaData, (DateTime)value);
			break;
		case ExtendedClrTypeCode.DBNull:
			SetDBNull_Unchecked(sink, setters, ordinal);
			break;
		case ExtendedClrTypeCode.Decimal:
			SetDecimal_PossiblyMoney(sink, setters, ordinal, metaData, (decimal)value);
			break;
		case ExtendedClrTypeCode.Double:
			SetDouble_Unchecked(sink, setters, ordinal, (double)value);
			break;
		case ExtendedClrTypeCode.Empty:
			SetDBNull_Unchecked(sink, setters, ordinal);
			break;
		case ExtendedClrTypeCode.Int16:
			SetInt16_Unchecked(sink, setters, ordinal, (short)value);
			break;
		case ExtendedClrTypeCode.Int32:
			SetInt32_Unchecked(sink, setters, ordinal, (int)value);
			break;
		case ExtendedClrTypeCode.Int64:
			SetInt64_Unchecked(sink, setters, ordinal, (long)value);
			break;
		case ExtendedClrTypeCode.SByte:
			throw ADP.InvalidCast();
		case ExtendedClrTypeCode.Single:
			SetSingle_Unchecked(sink, setters, ordinal, (float)value);
			break;
		case ExtendedClrTypeCode.String:
			SetString_LengthChecked(sink, setters, ordinal, metaData, (string)value, offset);
			break;
		case ExtendedClrTypeCode.UInt16:
			throw ADP.InvalidCast();
		case ExtendedClrTypeCode.UInt32:
			throw ADP.InvalidCast();
		case ExtendedClrTypeCode.UInt64:
			throw ADP.InvalidCast();
		case ExtendedClrTypeCode.Object:
			SetUdt_LengthChecked(sink, setters, ordinal, metaData, value);
			break;
		case ExtendedClrTypeCode.ByteArray:
			SetByteArray_LengthChecked(sink, setters, ordinal, metaData, (byte[])value, offset);
			break;
		case ExtendedClrTypeCode.CharArray:
			SetCharArray_LengthChecked(sink, setters, ordinal, metaData, (char[])value, offset);
			break;
		case ExtendedClrTypeCode.Guid:
			SetGuid_Unchecked(sink, setters, ordinal, (Guid)value);
			break;
		case ExtendedClrTypeCode.SqlBinary:
			SetSqlBinary_LengthChecked(sink, setters, ordinal, metaData, (SqlBinary)value, offset);
			break;
		case ExtendedClrTypeCode.SqlBoolean:
			SetSqlBoolean_Unchecked(sink, setters, ordinal, (SqlBoolean)value);
			break;
		case ExtendedClrTypeCode.SqlByte:
			SetSqlByte_Unchecked(sink, setters, ordinal, (SqlByte)value);
			break;
		case ExtendedClrTypeCode.SqlDateTime:
			SetSqlDateTime_Checked(sink, setters, ordinal, metaData, (SqlDateTime)value);
			break;
		case ExtendedClrTypeCode.SqlDouble:
			SetSqlDouble_Unchecked(sink, setters, ordinal, (SqlDouble)value);
			break;
		case ExtendedClrTypeCode.SqlGuid:
			SetSqlGuid_Unchecked(sink, setters, ordinal, (SqlGuid)value);
			break;
		case ExtendedClrTypeCode.SqlInt16:
			SetSqlInt16_Unchecked(sink, setters, ordinal, (SqlInt16)value);
			break;
		case ExtendedClrTypeCode.SqlInt32:
			SetSqlInt32_Unchecked(sink, setters, ordinal, (SqlInt32)value);
			break;
		case ExtendedClrTypeCode.SqlInt64:
			SetSqlInt64_Unchecked(sink, setters, ordinal, (SqlInt64)value);
			break;
		case ExtendedClrTypeCode.SqlMoney:
			SetSqlMoney_Checked(sink, setters, ordinal, metaData, (SqlMoney)value);
			break;
		case ExtendedClrTypeCode.SqlDecimal:
			SetSqlDecimal_Unchecked(sink, setters, ordinal, (SqlDecimal)value);
			break;
		case ExtendedClrTypeCode.SqlSingle:
			SetSqlSingle_Unchecked(sink, setters, ordinal, (SqlSingle)value);
			break;
		case ExtendedClrTypeCode.SqlString:
			SetSqlString_LengthChecked(sink, setters, ordinal, metaData, (SqlString)value, offset);
			break;
		case ExtendedClrTypeCode.SqlChars:
			SetSqlChars_LengthChecked(sink, setters, ordinal, metaData, (SqlChars)value, offset);
			break;
		case ExtendedClrTypeCode.SqlBytes:
			SetSqlBytes_LengthChecked(sink, setters, ordinal, metaData, (SqlBytes)value, offset);
			break;
		case ExtendedClrTypeCode.SqlXml:
			SetSqlXml_Unchecked(sink, setters, ordinal, (SqlXml)value);
			break;
		case ExtendedClrTypeCode.Stream:
			SetStream_Unchecked(sink, setters, ordinal, metaData, (StreamDataFeed)value);
			break;
		case ExtendedClrTypeCode.TextReader:
			SetTextReader_Unchecked(sink, setters, ordinal, metaData, (TextDataFeed)value);
			break;
		case ExtendedClrTypeCode.XmlReader:
			SetXmlReader_Unchecked(sink, setters, ordinal, ((XmlDataFeed)value)._source);
			break;
		case ExtendedClrTypeCode.DataTable:
		case ExtendedClrTypeCode.DbDataReader:
		case ExtendedClrTypeCode.IEnumerableOfSqlDataRecord:
		case ExtendedClrTypeCode.TimeSpan:
		case ExtendedClrTypeCode.DateTimeOffset:
			break;
		}
	}

	internal static void SetCompatibleValueV200(SmiEventSink_Default sink, SmiTypedGetterSetter setters, int ordinal, SmiMetaData metaData, object value, ExtendedClrTypeCode typeCode, int offset, int length, ParameterPeekAheadValue peekAhead, SqlBuffer.StorageType storageType)
	{
		if (typeCode == ExtendedClrTypeCode.DateTime)
		{
			switch (storageType)
			{
			case SqlBuffer.StorageType.DateTime2:
				SetDateTime2_Checked(sink, setters, ordinal, metaData, (DateTime)value);
				break;
			case SqlBuffer.StorageType.Date:
				SetDate_Checked(sink, setters, ordinal, metaData, (DateTime)value);
				break;
			default:
				SetDateTime_Checked(sink, setters, ordinal, metaData, (DateTime)value);
				break;
			}
		}
		else
		{
			SetCompatibleValueV200(sink, setters, ordinal, metaData, value, typeCode, offset, length, peekAhead);
		}
	}

	internal static void SetCompatibleValueV200(SmiEventSink_Default sink, SmiTypedGetterSetter setters, int ordinal, SmiMetaData metaData, object value, ExtendedClrTypeCode typeCode, int offset, int length, ParameterPeekAheadValue peekAhead)
	{
		switch (typeCode)
		{
		case ExtendedClrTypeCode.DataTable:
			SetDataTable_Unchecked(sink, setters, ordinal, metaData, (DataTable)value);
			break;
		case ExtendedClrTypeCode.DbDataReader:
			SetDbDataReader_Unchecked(sink, setters, ordinal, metaData, (DbDataReader)value);
			break;
		case ExtendedClrTypeCode.IEnumerableOfSqlDataRecord:
			SetIEnumerableOfSqlDataRecord_Unchecked(sink, setters, ordinal, metaData, (IEnumerable<SqlDataRecord>)value, peekAhead);
			break;
		case ExtendedClrTypeCode.TimeSpan:
			SetTimeSpan_Checked(sink, setters, ordinal, metaData, (TimeSpan)value);
			break;
		case ExtendedClrTypeCode.DateTimeOffset:
			SetDateTimeOffset_Unchecked(sink, setters, ordinal, (DateTimeOffset)value);
			break;
		default:
			SetCompatibleValue(sink, setters, ordinal, metaData, value, typeCode, offset);
			break;
		}
	}

	private static void SetDataTable_Unchecked(SmiEventSink_Default sink, SmiTypedGetterSetter setters, int ordinal, SmiMetaData metaData, DataTable value)
	{
		setters = setters.GetTypedGetterSetter(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		ExtendedClrTypeCode[] array = new ExtendedClrTypeCode[metaData.FieldMetaData.Count];
		for (int i = 0; i < metaData.FieldMetaData.Count; i++)
		{
			array[i] = ExtendedClrTypeCode.Invalid;
		}
		foreach (DataRow row in value.Rows)
		{
			setters.NewElement(sink);
			sink.ProcessMessagesAndThrow();
			for (int j = 0; j < metaData.FieldMetaData.Count; j++)
			{
				SmiMetaData smiMetaData = metaData.FieldMetaData[j];
				if (row.IsNull(j))
				{
					SetDBNull_Unchecked(sink, setters, j);
					continue;
				}
				object value2 = row[j];
				if (ExtendedClrTypeCode.Invalid == array[j])
				{
					array[j] = MetaDataUtilsSmi.DetermineExtendedTypeCodeForUseWithSqlDbType(smiMetaData.SqlDbType, smiMetaData.IsMultiValued, value2, smiMetaData.Type);
				}
				SetCompatibleValueV200(sink, setters, j, smiMetaData, value2, array[j], 0, -1, null);
			}
		}
		setters.EndElements(sink);
		sink.ProcessMessagesAndThrow();
	}

	internal static void FillCompatibleITypedSettersFromReader(SmiEventSink_Default sink, ITypedSettersV3 setters, SmiMetaData[] metaData, SqlDataReader reader)
	{
		for (int i = 0; i < metaData.Length; i++)
		{
			if (reader.IsDBNull(i))
			{
				SetDBNull_Unchecked(sink, setters, i);
				continue;
			}
			switch (metaData[i].SqlDbType)
			{
			case SqlDbType.BigInt:
				SetInt64_Unchecked(sink, setters, i, reader.GetInt64(i));
				break;
			case SqlDbType.Binary:
				SetSqlBytes_LengthChecked(sink, setters, i, metaData[i], reader.GetSqlBytes(i), 0);
				break;
			case SqlDbType.Bit:
				SetBoolean_Unchecked(sink, setters, i, reader.GetBoolean(i));
				break;
			case SqlDbType.Char:
				SetSqlChars_LengthChecked(sink, setters, i, metaData[i], reader.GetSqlChars(i), 0);
				break;
			case SqlDbType.DateTime:
				SetDateTime_Checked(sink, setters, i, metaData[i], reader.GetDateTime(i));
				break;
			case SqlDbType.Decimal:
				SetSqlDecimal_Unchecked(sink, setters, i, reader.GetSqlDecimal(i));
				break;
			case SqlDbType.Float:
				SetDouble_Unchecked(sink, setters, i, reader.GetDouble(i));
				break;
			case SqlDbType.Image:
				SetSqlBytes_LengthChecked(sink, setters, i, metaData[i], reader.GetSqlBytes(i), 0);
				break;
			case SqlDbType.Int:
				SetInt32_Unchecked(sink, setters, i, reader.GetInt32(i));
				break;
			case SqlDbType.Money:
				SetSqlMoney_Unchecked(sink, setters, i, metaData[i], reader.GetSqlMoney(i));
				break;
			case SqlDbType.NChar:
			case SqlDbType.NText:
			case SqlDbType.NVarChar:
				SetSqlChars_LengthChecked(sink, setters, i, metaData[i], reader.GetSqlChars(i), 0);
				break;
			case SqlDbType.Real:
				SetSingle_Unchecked(sink, setters, i, reader.GetFloat(i));
				break;
			case SqlDbType.UniqueIdentifier:
				SetGuid_Unchecked(sink, setters, i, reader.GetGuid(i));
				break;
			case SqlDbType.SmallDateTime:
				SetDateTime_Checked(sink, setters, i, metaData[i], reader.GetDateTime(i));
				break;
			case SqlDbType.SmallInt:
				SetInt16_Unchecked(sink, setters, i, reader.GetInt16(i));
				break;
			case SqlDbType.SmallMoney:
				SetSqlMoney_Checked(sink, setters, i, metaData[i], reader.GetSqlMoney(i));
				break;
			case SqlDbType.Text:
				SetSqlChars_LengthChecked(sink, setters, i, metaData[i], reader.GetSqlChars(i), 0);
				break;
			case SqlDbType.Timestamp:
				SetSqlBytes_LengthChecked(sink, setters, i, metaData[i], reader.GetSqlBytes(i), 0);
				break;
			case SqlDbType.TinyInt:
				SetByte_Unchecked(sink, setters, i, reader.GetByte(i));
				break;
			case SqlDbType.VarBinary:
				SetSqlBytes_LengthChecked(sink, setters, i, metaData[i], reader.GetSqlBytes(i), 0);
				break;
			case SqlDbType.VarChar:
				SetSqlChars_LengthChecked(sink, setters, i, metaData[i], reader.GetSqlChars(i), 0);
				break;
			case SqlDbType.Xml:
				SetSqlXml_Unchecked(sink, setters, i, reader.GetSqlXml(i));
				break;
			case SqlDbType.Variant:
			{
				object sqlValue = reader.GetSqlValue(i);
				ExtendedClrTypeCode typeCode = MetaDataUtilsSmi.DetermineExtendedTypeCode(sqlValue);
				SetCompatibleValue(sink, setters, i, metaData[i], sqlValue, typeCode, 0);
				break;
			}
			case SqlDbType.Udt:
				SetSqlBytes_LengthChecked(sink, setters, i, metaData[i], reader.GetSqlBytes(i), 0);
				break;
			default:
				throw ADP.NotSupported();
			}
		}
	}

	internal static void FillCompatibleSettersFromReader(SmiEventSink_Default sink, SmiTypedGetterSetter setters, IList<SmiExtendedMetaData> metaData, DbDataReader reader)
	{
		for (int i = 0; i < metaData.Count; i++)
		{
			if (reader.IsDBNull(i))
			{
				SetDBNull_Unchecked(sink, setters, i);
				continue;
			}
			switch (metaData[i].SqlDbType)
			{
			case SqlDbType.BigInt:
				SetInt64_Unchecked(sink, setters, i, reader.GetInt64(i));
				break;
			case SqlDbType.Binary:
				SetBytes_FromReader(sink, setters, i, metaData[i], reader, 0);
				break;
			case SqlDbType.Bit:
				SetBoolean_Unchecked(sink, setters, i, reader.GetBoolean(i));
				break;
			case SqlDbType.Char:
				SetCharsOrString_FromReader(sink, setters, i, metaData[i], reader, 0);
				break;
			case SqlDbType.DateTime:
				SetDateTime_Checked(sink, setters, i, metaData[i], reader.GetDateTime(i));
				break;
			case SqlDbType.Decimal:
				if (reader is SqlDataReader sqlDataReader4)
				{
					SetSqlDecimal_Unchecked(sink, setters, i, sqlDataReader4.GetSqlDecimal(i));
				}
				else
				{
					SetSqlDecimal_Unchecked(sink, setters, i, new SqlDecimal(reader.GetDecimal(i)));
				}
				break;
			case SqlDbType.Float:
				SetDouble_Unchecked(sink, setters, i, reader.GetDouble(i));
				break;
			case SqlDbType.Image:
				SetBytes_FromReader(sink, setters, i, metaData[i], reader, 0);
				break;
			case SqlDbType.Int:
				SetInt32_Unchecked(sink, setters, i, reader.GetInt32(i));
				break;
			case SqlDbType.Money:
				SetSqlMoney_Checked(sink, setters, i, metaData[i], new SqlMoney(reader.GetDecimal(i)));
				break;
			case SqlDbType.NChar:
			case SqlDbType.NText:
			case SqlDbType.NVarChar:
				SetCharsOrString_FromReader(sink, setters, i, metaData[i], reader, 0);
				break;
			case SqlDbType.Real:
				SetSingle_Unchecked(sink, setters, i, reader.GetFloat(i));
				break;
			case SqlDbType.UniqueIdentifier:
				SetGuid_Unchecked(sink, setters, i, reader.GetGuid(i));
				break;
			case SqlDbType.SmallDateTime:
				SetDateTime_Checked(sink, setters, i, metaData[i], reader.GetDateTime(i));
				break;
			case SqlDbType.SmallInt:
				SetInt16_Unchecked(sink, setters, i, reader.GetInt16(i));
				break;
			case SqlDbType.SmallMoney:
				SetSqlMoney_Checked(sink, setters, i, metaData[i], new SqlMoney(reader.GetDecimal(i)));
				break;
			case SqlDbType.Text:
				SetCharsOrString_FromReader(sink, setters, i, metaData[i], reader, 0);
				break;
			case SqlDbType.Timestamp:
				SetBytes_FromReader(sink, setters, i, metaData[i], reader, 0);
				break;
			case SqlDbType.TinyInt:
				SetByte_Unchecked(sink, setters, i, reader.GetByte(i));
				break;
			case SqlDbType.VarBinary:
				SetBytes_FromReader(sink, setters, i, metaData[i], reader, 0);
				break;
			case SqlDbType.VarChar:
				SetCharsOrString_FromReader(sink, setters, i, metaData[i], reader, 0);
				break;
			case SqlDbType.Xml:
				if (reader is SqlDataReader sqlDataReader5)
				{
					SetSqlXml_Unchecked(sink, setters, i, sqlDataReader5.GetSqlXml(i));
				}
				else
				{
					SetBytes_FromReader(sink, setters, i, metaData[i], reader, 0);
				}
				break;
			case SqlDbType.Variant:
			{
				SqlDataReader sqlDataReader3 = reader as SqlDataReader;
				SqlBuffer.StorageType storageType = SqlBuffer.StorageType.Empty;
				object value2;
				if (sqlDataReader3 != null)
				{
					value2 = sqlDataReader3.GetSqlValue(i);
					storageType = sqlDataReader3.GetVariantInternalStorageType(i);
				}
				else
				{
					value2 = reader.GetValue(i);
				}
				ExtendedClrTypeCode typeCode = MetaDataUtilsSmi.DetermineExtendedTypeCodeForUseWithSqlDbType(metaData[i].SqlDbType, metaData[i].IsMultiValued, value2, null);
				if (storageType == SqlBuffer.StorageType.DateTime2 || storageType == SqlBuffer.StorageType.Date)
				{
					SetCompatibleValueV200(sink, setters, i, metaData[i], value2, typeCode, 0, 0, null, storageType);
				}
				else
				{
					SetCompatibleValueV200(sink, setters, i, metaData[i], value2, typeCode, 0, 0, null);
				}
				break;
			}
			case SqlDbType.Udt:
				SetBytes_FromReader(sink, setters, i, metaData[i], reader, 0);
				break;
			case SqlDbType.Date:
			case SqlDbType.DateTime2:
				SetDateTime_Checked(sink, setters, i, metaData[i], reader.GetDateTime(i));
				break;
			case SqlDbType.Time:
				SetTimeSpan_Checked(value: (!(reader is SqlDataReader sqlDataReader2)) ? ((TimeSpan)reader.GetValue(i)) : sqlDataReader2.GetTimeSpan(i), sink: sink, setters: setters, ordinal: i, metaData: metaData[i]);
				break;
			case SqlDbType.DateTimeOffset:
			{
				DateTimeOffset value = ((!(reader is SqlDataReader sqlDataReader)) ? ((DateTimeOffset)reader.GetValue(i)) : sqlDataReader.GetDateTimeOffset(i));
				SetDateTimeOffset_Unchecked(sink, setters, i, value);
				break;
			}
			default:
				throw ADP.NotSupported();
			}
		}
	}

	internal static void FillCompatibleSettersFromRecord(SmiEventSink_Default sink, SmiTypedGetterSetter setters, SmiMetaData[] metaData, SqlDataRecord record, SmiDefaultFieldsProperty useDefaultValues)
	{
		for (int i = 0; i < metaData.Length; i++)
		{
			if (useDefaultValues != null && useDefaultValues[i])
			{
				continue;
			}
			if (record.IsDBNull(i))
			{
				SetDBNull_Unchecked(sink, setters, i);
				continue;
			}
			switch (metaData[i].SqlDbType)
			{
			case SqlDbType.BigInt:
				SetInt64_Unchecked(sink, setters, i, record.GetInt64(i));
				break;
			case SqlDbType.Binary:
				SetBytes_FromRecord(sink, setters, i, metaData[i], record, 0);
				break;
			case SqlDbType.Bit:
				SetBoolean_Unchecked(sink, setters, i, record.GetBoolean(i));
				break;
			case SqlDbType.Char:
				SetChars_FromRecord(sink, setters, i, metaData[i], record, 0);
				break;
			case SqlDbType.DateTime:
				SetDateTime_Checked(sink, setters, i, metaData[i], record.GetDateTime(i));
				break;
			case SqlDbType.Decimal:
				SetSqlDecimal_Unchecked(sink, setters, i, record.GetSqlDecimal(i));
				break;
			case SqlDbType.Float:
				SetDouble_Unchecked(sink, setters, i, record.GetDouble(i));
				break;
			case SqlDbType.Image:
				SetBytes_FromRecord(sink, setters, i, metaData[i], record, 0);
				break;
			case SqlDbType.Int:
				SetInt32_Unchecked(sink, setters, i, record.GetInt32(i));
				break;
			case SqlDbType.Money:
				SetSqlMoney_Unchecked(sink, setters, i, metaData[i], record.GetSqlMoney(i));
				break;
			case SqlDbType.NChar:
			case SqlDbType.NText:
			case SqlDbType.NVarChar:
				SetChars_FromRecord(sink, setters, i, metaData[i], record, 0);
				break;
			case SqlDbType.Real:
				SetSingle_Unchecked(sink, setters, i, record.GetFloat(i));
				break;
			case SqlDbType.UniqueIdentifier:
				SetGuid_Unchecked(sink, setters, i, record.GetGuid(i));
				break;
			case SqlDbType.SmallDateTime:
				SetDateTime_Checked(sink, setters, i, metaData[i], record.GetDateTime(i));
				break;
			case SqlDbType.SmallInt:
				SetInt16_Unchecked(sink, setters, i, record.GetInt16(i));
				break;
			case SqlDbType.SmallMoney:
				SetSqlMoney_Checked(sink, setters, i, metaData[i], record.GetSqlMoney(i));
				break;
			case SqlDbType.Text:
				SetChars_FromRecord(sink, setters, i, metaData[i], record, 0);
				break;
			case SqlDbType.Timestamp:
				SetBytes_FromRecord(sink, setters, i, metaData[i], record, 0);
				break;
			case SqlDbType.TinyInt:
				SetByte_Unchecked(sink, setters, i, record.GetByte(i));
				break;
			case SqlDbType.VarBinary:
				SetBytes_FromRecord(sink, setters, i, metaData[i], record, 0);
				break;
			case SqlDbType.VarChar:
				SetChars_FromRecord(sink, setters, i, metaData[i], record, 0);
				break;
			case SqlDbType.Xml:
				SetSqlXml_Unchecked(sink, setters, i, record.GetSqlXml(i));
				break;
			case SqlDbType.Variant:
			{
				object sqlValue = record.GetSqlValue(i);
				ExtendedClrTypeCode typeCode = MetaDataUtilsSmi.DetermineExtendedTypeCode(sqlValue);
				SetCompatibleValueV200(sink, setters, i, metaData[i], sqlValue, typeCode, 0, -1, null);
				break;
			}
			case SqlDbType.Udt:
				SetBytes_FromRecord(sink, setters, i, metaData[i], record, 0);
				break;
			case SqlDbType.Date:
			case SqlDbType.DateTime2:
				SetDateTime_Checked(sink, setters, i, metaData[i], record.GetDateTime(i));
				break;
			case SqlDbType.Time:
				SetTimeSpan_Checked(value: record?.GetTimeSpan(i) ?? ((TimeSpan)record.GetValue(i)), sink: sink, setters: setters, ordinal: i, metaData: metaData[i]);
				break;
			case SqlDbType.DateTimeOffset:
			{
				DateTimeOffset value = record?.GetDateTimeOffset(i) ?? ((DateTimeOffset)record.GetValue(i));
				SetDateTimeOffset_Unchecked(sink, setters, i, value);
				break;
			}
			default:
				throw ADP.NotSupported();
			}
		}
	}

	internal static Stream CopyIntoNewSmiScratchStream(Stream source, SmiEventSink_Default sink)
	{
		Stream stream = new MemoryStream();
		int num = (int)((!source.CanSeek || 8000 <= source.Length) ? 8000 : source.Length);
		byte[] buffer = new byte[num];
		int count;
		while ((count = source.Read(buffer, 0, num)) != 0)
		{
			stream.Write(buffer, 0, count);
		}
		stream.Flush();
		stream.Seek(0L, SeekOrigin.Begin);
		return stream;
	}

	private static object GetUdt_LengthChecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (IsDBNull_Unchecked(sink, getters, ordinal))
		{
			return metaData.Type.InvokeMember("Null", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty, null, null, new object[0], CultureInfo.InvariantCulture);
		}
		return SerializationHelperSql9.Deserialize(new SmiGettersStream(sink, getters, ordinal, metaData), metaData.Type);
	}

	private static decimal GetDecimal_PossiblyMoney(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		if (SqlDbType.Decimal == metaData.SqlDbType)
		{
			return GetSqlDecimal_Unchecked(sink, getters, ordinal).Value;
		}
		return GetSqlMoney_Unchecked(sink, getters, ordinal).Value;
	}

	private static void SetDecimal_PossiblyMoney(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, decimal value)
	{
		if (SqlDbType.Decimal == metaData.SqlDbType || SqlDbType.Variant == metaData.SqlDbType)
		{
			SetDecimal_Unchecked(sink, setters, ordinal, value);
		}
		else
		{
			SetSqlMoney_Checked(sink, setters, ordinal, metaData, new SqlMoney(value));
		}
	}

	private static void VerifyDateTimeRange(SqlDbType dbType, DateTime value)
	{
		if (SqlDbType.SmallDateTime == dbType && (s_dtSmallMax < value || s_dtSmallMin > value))
		{
			throw ADP.InvalidMetaDataValue();
		}
	}

	private static void VerifyTimeRange(SqlDbType dbType, TimeSpan value)
	{
		if (SqlDbType.Time == dbType && (s_timeMin > value || value > s_timeMax))
		{
			throw ADP.InvalidMetaDataValue();
		}
	}

	private static void SetDateTime_Checked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, DateTime value)
	{
		VerifyDateTimeRange(metaData.SqlDbType, value);
		SetDateTime_Unchecked(sink, setters, ordinal, (SqlDbType.Date == metaData.SqlDbType) ? value.Date : value);
	}

	private static void SetTimeSpan_Checked(SmiEventSink_Default sink, SmiTypedGetterSetter setters, int ordinal, SmiMetaData metaData, TimeSpan value)
	{
		VerifyTimeRange(metaData.SqlDbType, value);
		SetTimeSpan_Unchecked(sink, setters, ordinal, value);
	}

	private static void SetSqlDateTime_Checked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlDateTime value)
	{
		if (!value.IsNull)
		{
			VerifyDateTimeRange(metaData.SqlDbType, value.Value);
		}
		SetSqlDateTime_Unchecked(sink, setters, ordinal, value);
	}

	private static void SetDateTime2_Checked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, DateTime value)
	{
		VerifyDateTimeRange(metaData.SqlDbType, value);
		SetDateTime2_Unchecked(sink, setters, ordinal, metaData, value);
	}

	private static void SetDate_Checked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, DateTime value)
	{
		VerifyDateTimeRange(metaData.SqlDbType, value);
		SetDate_Unchecked(sink, setters, ordinal, metaData, value);
	}

	private static void SetSqlMoney_Checked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlMoney value)
	{
		if (!value.IsNull && SqlDbType.SmallMoney == metaData.SqlDbType)
		{
			decimal value2 = value.Value;
			if (TdsEnums.SQL_SMALL_MONEY_MIN > value2 || TdsEnums.SQL_SMALL_MONEY_MAX < value2)
			{
				throw SQL.MoneyOverflow(value2.ToString(CultureInfo.InvariantCulture));
			}
		}
		SetSqlMoney_Unchecked(sink, setters, ordinal, metaData, value);
	}

	private static void SetByteArray_LengthChecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, byte[] buffer, int offset)
	{
		int length = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, -1L, 0L, buffer.Length, offset, buffer.Length - offset);
		SetByteArray_Unchecked(sink, setters, ordinal, buffer, offset, length);
	}

	private static void SetCharArray_LengthChecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, char[] buffer, int offset)
	{
		int length = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, -1L, 0L, buffer.Length, offset, buffer.Length - offset);
		SetCharArray_Unchecked(sink, setters, ordinal, buffer, offset, length);
	}

	private static void SetSqlBinary_LengthChecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlBinary value, int offset)
	{
		int length = 0;
		if (!value.IsNull)
		{
			length = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, -1L, 0L, value.Length, offset, value.Length - offset);
		}
		SetSqlBinary_Unchecked(sink, setters, ordinal, value, offset, length);
	}

	private static void SetBytes_FromRecord(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlDataRecord record, int offset)
	{
		int num = 0;
		long num2 = record.GetBytes(ordinal, 0L, null, 0, 0);
		if (num2 > int.MaxValue)
		{
			num2 = -1L;
		}
		checked
		{
			num = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, -1L, 0L, (int)num2, offset, (int)num2);
			int num3 = ((num <= 8000 && num >= 0) ? num : 8000);
			byte[] buffer = new byte[num3];
			long num4 = 1L;
			long num5 = offset;
			for (long num6 = 0L; num < 0 || num6 < num; num6 += num4)
			{
				long bytes;
				if ((bytes = record.GetBytes(ordinal, num5, buffer, 0, num3)) == 0L)
				{
					break;
				}
				if (num4 == 0L)
				{
					break;
				}
				num4 = setters.SetBytes(sink, ordinal, num5, buffer, 0, (int)bytes);
				sink.ProcessMessagesAndThrow();
				num5 += num4;
			}
			setters.SetBytesLength(sink, ordinal, num5);
			sink.ProcessMessagesAndThrow();
		}
	}

	private static void SetBytes_FromReader(SmiEventSink_Default sink, SmiTypedGetterSetter setters, int ordinal, SmiMetaData metaData, DbDataReader reader, int offset)
	{
		int num = 0;
		num = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, -1L, 0L, -1, offset, -1);
		int num2 = 8000;
		byte[] buffer = new byte[num2];
		long num3 = 1L;
		long num4 = offset;
		checked
		{
			for (long num5 = 0L; num < 0 || num5 < num; num5 += num3)
			{
				long bytes;
				if ((bytes = reader.GetBytes(ordinal, num4, buffer, 0, num2)) == 0L)
				{
					break;
				}
				if (num3 == 0L)
				{
					break;
				}
				num3 = setters.SetBytes(sink, ordinal, num4, buffer, 0, (int)bytes);
				sink.ProcessMessagesAndThrow();
				num4 += num3;
			}
			setters.SetBytesLength(sink, ordinal, num4);
			sink.ProcessMessagesAndThrow();
		}
	}

	private static void SetSqlBytes_LengthChecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlBytes value, int offset)
	{
		int num = 0;
		if (!value.IsNull)
		{
			long num2 = value.Length;
			if (num2 > int.MaxValue)
			{
				num2 = -1L;
			}
			num = checked(CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, -1L, 0L, (int)num2, offset, (int)num2));
		}
		SetSqlBytes_Unchecked(sink, setters, ordinal, value, 0, num);
	}

	private static void SetChars_FromRecord(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlDataRecord record, int offset)
	{
		int num = 0;
		long num2 = record.GetChars(ordinal, 0L, null, 0, 0);
		if (num2 > int.MaxValue)
		{
			num2 = -1L;
		}
		checked
		{
			num = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, -1L, 0L, (int)num2, offset, (int)num2 - offset);
			int num3 = ((num <= 4000 && num >= 0) ? num : ((!MetaDataUtilsSmi.IsAnsiType(metaData.SqlDbType)) ? 4000 : 8000));
			char[] buffer = new char[num3];
			long num4 = 1L;
			long num5 = offset;
			for (long num6 = 0L; num < 0 || num6 < num; num6 += num4)
			{
				long chars;
				if ((chars = record.GetChars(ordinal, num5, buffer, 0, num3)) == 0L)
				{
					break;
				}
				if (num4 == 0L)
				{
					break;
				}
				num4 = setters.SetChars(sink, ordinal, num5, buffer, 0, (int)chars);
				sink.ProcessMessagesAndThrow();
				num5 += num4;
			}
			setters.SetCharsLength(sink, ordinal, num5);
			sink.ProcessMessagesAndThrow();
		}
	}

	private static void SetCharsOrString_FromReader(SmiEventSink_Default sink, SmiTypedGetterSetter setters, int ordinal, SmiMetaData metaData, DbDataReader reader, int offset)
	{
		bool flag = false;
		try
		{
			SetChars_FromReader(sink, setters, ordinal, metaData, reader, offset);
			flag = true;
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
		}
		if (!flag)
		{
			SetString_FromReader(sink, setters, ordinal, metaData, reader, offset);
		}
	}

	private static void SetChars_FromReader(SmiEventSink_Default sink, SmiTypedGetterSetter setters, int ordinal, SmiMetaData metaData, DbDataReader reader, int offset)
	{
		int num = 0;
		num = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, -1L, 0L, -1, offset, -1);
		int num2 = ((!MetaDataUtilsSmi.IsAnsiType(metaData.SqlDbType)) ? 4000 : 8000);
		char[] buffer = new char[num2];
		long num3 = 1L;
		long num4 = offset;
		checked
		{
			for (long num5 = 0L; num < 0 || num5 < num; num5 += num3)
			{
				long chars;
				if ((chars = reader.GetChars(ordinal, num4, buffer, 0, num2)) == 0L)
				{
					break;
				}
				if (num3 == 0L)
				{
					break;
				}
				num3 = setters.SetChars(sink, ordinal, num4, buffer, 0, (int)chars);
				sink.ProcessMessagesAndThrow();
				num4 += num3;
			}
			setters.SetCharsLength(sink, ordinal, num4);
			sink.ProcessMessagesAndThrow();
		}
	}

	private static void SetString_FromReader(SmiEventSink_Default sink, SmiTypedGetterSetter setters, int ordinal, SmiMetaData metaData, DbDataReader reader, int offset)
	{
		string text = reader.GetString(ordinal);
		int length = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, text.Length, 0L, -1, offset, -1);
		setters.SetString(sink, ordinal, text, offset, length);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlChars_LengthChecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlChars value, int offset)
	{
		int length = 0;
		if (!value.IsNull)
		{
			long num = value.Length;
			if (num > int.MaxValue)
			{
				num = -1L;
			}
			length = checked(CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, -1L, 0L, (int)num, offset, (int)num - offset));
		}
		SetSqlChars_Unchecked(sink, setters, ordinal, value, 0, length);
	}

	private static void SetSqlString_LengthChecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlString value, int offset)
	{
		if (value.IsNull)
		{
			SetDBNull_Unchecked(sink, setters, ordinal);
			return;
		}
		string value2 = value.Value;
		int length = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, -1L, 0L, value2.Length, offset, value2.Length - offset);
		SetSqlString_Unchecked(sink, setters, ordinal, metaData, value, offset, length);
	}

	private static void SetString_LengthChecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, string value, int offset)
	{
		int length = CheckXetParameters(metaData.SqlDbType, metaData.MaxLength, -1L, 0L, value.Length, offset, checked(value.Length - offset));
		SetString_Unchecked(sink, setters, ordinal, value, offset, length);
	}

	private static void SetUdt_LengthChecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, object value)
	{
		if (ADP.IsNull(value))
		{
			setters.SetDBNull(sink, ordinal);
			sink.ProcessMessagesAndThrow();
		}
		else
		{
			SerializationHelperSql9.Serialize(new SmiSettersStream(sink, setters, ordinal, metaData), value);
		}
	}

	private static void ThrowIfInvalidSetterAccess(SmiMetaData metaData, ExtendedClrTypeCode setterTypeCode)
	{
		if (!CanAccessSetterDirectly(metaData, setterTypeCode))
		{
			throw ADP.InvalidCast();
		}
	}

	private static void ThrowIfITypedGettersIsNull(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		if (IsDBNull_Unchecked(sink, getters, ordinal))
		{
			throw SQL.SqlNullValue();
		}
	}

	private static bool CanAccessGetterDirectly(SmiMetaData metaData, ExtendedClrTypeCode setterTypeCode)
	{
		bool flag = s_canAccessGetterDirectly[(int)setterTypeCode, (int)metaData.SqlDbType];
		if (flag && (ExtendedClrTypeCode.DataTable == setterTypeCode || ExtendedClrTypeCode.DbDataReader == setterTypeCode || ExtendedClrTypeCode.IEnumerableOfSqlDataRecord == setterTypeCode))
		{
			flag = metaData.IsMultiValued;
		}
		return flag;
	}

	private static bool CanAccessSetterDirectly(SmiMetaData metaData, ExtendedClrTypeCode setterTypeCode)
	{
		bool flag = s_canAccessSetterDirectly[(int)setterTypeCode, (int)metaData.SqlDbType];
		if (flag && (ExtendedClrTypeCode.DataTable == setterTypeCode || ExtendedClrTypeCode.DbDataReader == setterTypeCode || ExtendedClrTypeCode.IEnumerableOfSqlDataRecord == setterTypeCode))
		{
			flag = metaData.IsMultiValued;
		}
		return flag;
	}

	private static long PositiveMin(long first, long second)
	{
		if (first < 0)
		{
			return second;
		}
		if (second < 0)
		{
			return first;
		}
		return Math.Min(first, second);
	}

	private static int CheckXetParameters(SqlDbType dbType, long maxLength, long actualLength, long fieldOffset, int bufferLength, int bufferOffset, int length)
	{
		if (0 > fieldOffset)
		{
			throw ADP.NegativeParameter("fieldOffset");
		}
		if (bufferOffset < 0)
		{
			throw ADP.InvalidDestinationBufferIndex(bufferLength, bufferOffset, "bufferOffset");
		}
		checked
		{
			if (bufferLength < 0)
			{
				length = (int)PositiveMin(length, PositiveMin(maxLength, actualLength));
				if (length < -1)
				{
					length = -1;
				}
				return length;
			}
			if (bufferOffset > bufferLength)
			{
				throw ADP.InvalidDestinationBufferIndex(bufferLength, bufferOffset, "bufferOffset");
			}
			if (length + bufferOffset > bufferLength)
			{
				throw ADP.InvalidBufferSizeOrIndex(length, bufferOffset);
			}
			if (length < 0)
			{
				throw ADP.InvalidDataLength(length);
			}
			if (0 <= actualLength && actualLength <= fieldOffset)
			{
				return 0;
			}
		}
		length = Math.Min(length, bufferLength - bufferOffset);
		if (SqlDbType.Variant == dbType)
		{
			length = Math.Min(length, 8000);
		}
		if (0 <= actualLength)
		{
			length = (int)Math.Min(length, actualLength - fieldOffset);
		}
		else if (SqlDbType.Udt != dbType && 0 <= maxLength)
		{
			length = (int)Math.Min(length, maxLength - fieldOffset);
		}
		if (length < 0)
		{
			return 0;
		}
		return length;
	}

	private static bool IsDBNull_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		bool result = getters.IsDBNull(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return result;
	}

	private static bool GetBoolean_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		bool boolean = getters.GetBoolean(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return boolean;
	}

	private static byte GetByte_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		byte result = getters.GetByte(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return result;
	}

	private static byte[] GetByteArray_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		long bytesLength = getters.GetBytesLength(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		int num = checked((int)bytesLength);
		byte[] array = new byte[num];
		getters.GetBytes(sink, ordinal, 0L, array, 0, num);
		sink.ProcessMessagesAndThrow();
		return array;
	}

	internal static int GetBytes_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
	{
		int bytes = getters.GetBytes(sink, ordinal, fieldOffset, buffer, bufferOffset, length);
		sink.ProcessMessagesAndThrow();
		return bytes;
	}

	private static long GetBytesLength_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		long bytesLength = getters.GetBytesLength(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return bytesLength;
	}

	private static char[] GetCharArray_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		long charsLength = getters.GetCharsLength(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		int num = checked((int)charsLength);
		char[] array = new char[num];
		getters.GetChars(sink, ordinal, 0L, array, 0, num);
		sink.ProcessMessagesAndThrow();
		return array;
	}

	internal static int GetChars_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
	{
		int chars = getters.GetChars(sink, ordinal, fieldOffset, buffer, bufferOffset, length);
		sink.ProcessMessagesAndThrow();
		return chars;
	}

	private static long GetCharsLength_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		long charsLength = getters.GetCharsLength(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return charsLength;
	}

	private static DateTime GetDateTime_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		DateTime dateTime = getters.GetDateTime(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return dateTime;
	}

	private static DateTimeOffset GetDateTimeOffset_Unchecked(SmiEventSink_Default sink, SmiTypedGetterSetter getters, int ordinal)
	{
		DateTimeOffset dateTimeOffset = getters.GetDateTimeOffset(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return dateTimeOffset;
	}

	private static double GetDouble_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		double result = getters.GetDouble(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return result;
	}

	private static Guid GetGuid_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		Guid guid = getters.GetGuid(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return guid;
	}

	private static short GetInt16_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		short @int = getters.GetInt16(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return @int;
	}

	private static int GetInt32_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		int @int = getters.GetInt32(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return @int;
	}

	private static long GetInt64_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		long @int = getters.GetInt64(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return @int;
	}

	private static float GetSingle_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		float single = getters.GetSingle(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return single;
	}

	private static SqlBinary GetSqlBinary_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		return new SqlBinary(GetByteArray_Unchecked(sink, getters, ordinal));
	}

	private static SqlDecimal GetSqlDecimal_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		SqlDecimal sqlDecimal = getters.GetSqlDecimal(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return sqlDecimal;
	}

	private static SqlMoney GetSqlMoney_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		long @int = getters.GetInt64(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return SqlTypeWorkarounds.SqlMoneyCtor(@int, 1);
	}

	private static SqlXml GetSqlXml_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		return new SqlXml(CopyIntoNewSmiScratchStream(new SmiGettersStream(sink, getters, ordinal, SmiMetaData.DefaultXml), sink));
	}

	private static string GetString_Unchecked(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal)
	{
		string result = getters.GetString(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return result;
	}

	private static TimeSpan GetTimeSpan_Unchecked(SmiEventSink_Default sink, SmiTypedGetterSetter getters, int ordinal)
	{
		TimeSpan timeSpan = getters.GetTimeSpan(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		return timeSpan;
	}

	private static void SetBoolean_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, bool value)
	{
		setters.SetBoolean(sink, ordinal, value);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetByteArray_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, byte[] buffer, int bufferOffset, int length)
	{
		if (length > 0)
		{
			setters.SetBytes(sink, ordinal, 0L, buffer, bufferOffset, length);
			sink.ProcessMessagesAndThrow();
		}
		setters.SetBytesLength(sink, ordinal, length);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetStream_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metadata, StreamDataFeed feed)
	{
		long maxLength = metadata.MaxLength;
		byte[] buffer = new byte[4096];
		int num = 0;
		do
		{
			int num2 = 0;
			int num3 = 4096;
			if (maxLength > 0 && num + num3 > maxLength)
			{
				num3 = (int)(maxLength - num);
			}
			num2 = feed._source.Read(buffer, 0, num3);
			if (num2 == 0)
			{
				break;
			}
			setters.SetBytes(sink, ordinal, num, buffer, 0, num2);
			sink.ProcessMessagesAndThrow();
			num += num2;
		}
		while (maxLength <= 0 || num < maxLength);
		setters.SetBytesLength(sink, ordinal, num);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetTextReader_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metadata, TextDataFeed feed)
	{
		long maxLength = metadata.MaxLength;
		char[] buffer = new char[4096];
		int num = 0;
		do
		{
			int num2 = 0;
			int num3 = 4096;
			if (maxLength > 0 && num + num3 > maxLength)
			{
				num3 = (int)(maxLength - num);
			}
			num2 = feed._source.Read(buffer, 0, num3);
			if (num2 == 0)
			{
				break;
			}
			setters.SetChars(sink, ordinal, num, buffer, 0, num2);
			sink.ProcessMessagesAndThrow();
			num += num2;
		}
		while (maxLength <= 0 || num < maxLength);
		setters.SetCharsLength(sink, ordinal, num);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetByte_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, byte value)
	{
		setters.SetByte(sink, ordinal, value);
		sink.ProcessMessagesAndThrow();
	}

	private static int SetBytes_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
	{
		int result = setters.SetBytes(sink, ordinal, fieldOffset, buffer, bufferOffset, length);
		sink.ProcessMessagesAndThrow();
		return result;
	}

	private static void SetCharArray_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, char[] buffer, int bufferOffset, int length)
	{
		if (length > 0)
		{
			setters.SetChars(sink, ordinal, 0L, buffer, bufferOffset, length);
			sink.ProcessMessagesAndThrow();
		}
		setters.SetCharsLength(sink, ordinal, length);
		sink.ProcessMessagesAndThrow();
	}

	private static int SetChars_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
	{
		int result = setters.SetChars(sink, ordinal, fieldOffset, buffer, bufferOffset, length);
		sink.ProcessMessagesAndThrow();
		return result;
	}

	private static void SetDBNull_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal)
	{
		setters.SetDBNull(sink, ordinal);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetDecimal_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, decimal value)
	{
		setters.SetSqlDecimal(sink, ordinal, new SqlDecimal(value));
		sink.ProcessMessagesAndThrow();
	}

	private static void SetDateTime_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, DateTime value)
	{
		setters.SetDateTime(sink, ordinal, value);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetDateTime2_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, DateTime value)
	{
		setters.SetVariantMetaData(sink, ordinal, SmiMetaData.DefaultDateTime2);
		setters.SetDateTime(sink, ordinal, value);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetDate_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, DateTime value)
	{
		setters.SetVariantMetaData(sink, ordinal, SmiMetaData.DefaultDate);
		setters.SetDateTime(sink, ordinal, value);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetTimeSpan_Unchecked(SmiEventSink_Default sink, SmiTypedGetterSetter setters, int ordinal, TimeSpan value)
	{
		setters.SetTimeSpan(sink, ordinal, value);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetDateTimeOffset_Unchecked(SmiEventSink_Default sink, SmiTypedGetterSetter setters, int ordinal, DateTimeOffset value)
	{
		setters.SetDateTimeOffset(sink, ordinal, value);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetDouble_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, double value)
	{
		setters.SetDouble(sink, ordinal, value);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetGuid_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, Guid value)
	{
		setters.SetGuid(sink, ordinal, value);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetInt16_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, short value)
	{
		setters.SetInt16(sink, ordinal, value);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetInt32_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, int value)
	{
		setters.SetInt32(sink, ordinal, value);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetInt64_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, long value)
	{
		setters.SetInt64(sink, ordinal, value);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSingle_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, float value)
	{
		setters.SetSingle(sink, ordinal, value);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlBinary_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlBinary value, int offset, int length)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
		}
		else
		{
			SetByteArray_Unchecked(sink, setters, ordinal, value.Value, offset, length);
		}
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlBoolean_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlBoolean value)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
		}
		else
		{
			setters.SetBoolean(sink, ordinal, value.Value);
		}
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlByte_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlByte value)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
		}
		else
		{
			setters.SetByte(sink, ordinal, value.Value);
		}
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlBytes_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlBytes value, int offset, long length)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
			sink.ProcessMessagesAndThrow();
			return;
		}
		checked
		{
			int num = ((length <= 8000 && length >= 0) ? ((int)length) : 8000);
			byte[] buffer = new byte[num];
			long num2 = 1L;
			long num3 = offset;
			for (long num4 = 0L; length < 0 || num4 < length; num4 += num2)
			{
				long num5;
				if ((num5 = value.Read(num3, buffer, 0, num)) == 0L)
				{
					break;
				}
				if (num2 == 0L)
				{
					break;
				}
				num2 = setters.SetBytes(sink, ordinal, num3, buffer, 0, (int)num5);
				sink.ProcessMessagesAndThrow();
				num3 += num2;
			}
			setters.SetBytesLength(sink, ordinal, num3);
			sink.ProcessMessagesAndThrow();
		}
	}

	private static void SetSqlChars_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlChars value, int offset, int length)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
			sink.ProcessMessagesAndThrow();
			return;
		}
		int num = ((length <= 4000 && length >= 0) ? length : 4000);
		char[] buffer = new char[num];
		long num2 = 1L;
		long num3 = offset;
		checked
		{
			for (long num4 = 0L; length < 0 || num4 < length; num4 += num2)
			{
				long num5;
				if ((num5 = value.Read(num3, buffer, 0, num)) == 0L)
				{
					break;
				}
				if (num2 == 0L)
				{
					break;
				}
				num2 = setters.SetChars(sink, ordinal, num3, buffer, 0, (int)num5);
				sink.ProcessMessagesAndThrow();
				num3 += num2;
			}
			setters.SetCharsLength(sink, ordinal, num3);
			sink.ProcessMessagesAndThrow();
		}
	}

	private static void SetSqlDateTime_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlDateTime value)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
		}
		else
		{
			setters.SetDateTime(sink, ordinal, value.Value);
		}
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlDecimal_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlDecimal value)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
		}
		else
		{
			setters.SetSqlDecimal(sink, ordinal, value);
		}
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlDouble_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlDouble value)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
		}
		else
		{
			setters.SetDouble(sink, ordinal, value.Value);
		}
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlGuid_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlGuid value)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
		}
		else
		{
			setters.SetGuid(sink, ordinal, value.Value);
		}
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlInt16_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlInt16 value)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
		}
		else
		{
			setters.SetInt16(sink, ordinal, value.Value);
		}
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlInt32_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlInt32 value)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
		}
		else
		{
			setters.SetInt32(sink, ordinal, value.Value);
		}
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlInt64_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlInt64 value)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
		}
		else
		{
			setters.SetInt64(sink, ordinal, value.Value);
		}
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlMoney_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlMoney value)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
		}
		else
		{
			if (SqlDbType.Variant == metaData.SqlDbType)
			{
				setters.SetVariantMetaData(sink, ordinal, SmiMetaData.DefaultMoney);
				sink.ProcessMessagesAndThrow();
			}
			setters.SetInt64(sink, ordinal, SqlTypeWorkarounds.SqlMoneyToSqlInternalRepresentation(value));
		}
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlSingle_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlSingle value)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
		}
		else
		{
			setters.SetSingle(sink, ordinal, value.Value);
		}
		sink.ProcessMessagesAndThrow();
	}

	private static void SetSqlString_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData, SqlString value, int offset, int length)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
			sink.ProcessMessagesAndThrow();
			return;
		}
		if (SqlDbType.Variant == metaData.SqlDbType)
		{
			metaData = new SmiMetaData(SqlDbType.NVarChar, 4000L, 0, 0, value.LCID, value.SqlCompareOptions, null);
			setters.SetVariantMetaData(sink, ordinal, metaData);
			sink.ProcessMessagesAndThrow();
		}
		SetString_Unchecked(sink, setters, ordinal, value.Value, offset, length);
	}

	private static void SetSqlXml_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SqlXml value)
	{
		if (value.IsNull)
		{
			setters.SetDBNull(sink, ordinal);
			sink.ProcessMessagesAndThrow();
		}
		else
		{
			SetXmlReader_Unchecked(sink, setters, ordinal, value.CreateReader());
		}
	}

	private static void SetXmlReader_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, XmlReader xmlReader)
	{
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.CloseOutput = false;
		xmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
		xmlWriterSettings.Encoding = Encoding.Unicode;
		xmlWriterSettings.OmitXmlDeclaration = true;
		XmlWriter xmlWriter = XmlWriter.Create(new SmiSettersStream(sink, setters, ordinal, SmiMetaData.DefaultXml), xmlWriterSettings);
		xmlReader.Read();
		while (!xmlReader.EOF)
		{
			xmlWriter.WriteNode(xmlReader, defattr: true);
		}
		xmlWriter.Flush();
		sink.ProcessMessagesAndThrow();
	}

	private static void SetString_Unchecked(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, string value, int offset, int length)
	{
		setters.SetString(sink, ordinal, value, offset, length);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetDbDataReader_Unchecked(SmiEventSink_Default sink, SmiTypedGetterSetter setters, int ordinal, SmiMetaData metaData, DbDataReader value)
	{
		setters = setters.GetTypedGetterSetter(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		while (value.Read())
		{
			setters.NewElement(sink);
			sink.ProcessMessagesAndThrow();
			FillCompatibleSettersFromReader(sink, setters, metaData.FieldMetaData, value);
		}
		setters.EndElements(sink);
		sink.ProcessMessagesAndThrow();
	}

	private static void SetIEnumerableOfSqlDataRecord_Unchecked(SmiEventSink_Default sink, SmiTypedGetterSetter setters, int ordinal, SmiMetaData metaData, IEnumerable<SqlDataRecord> value, ParameterPeekAheadValue peekAhead)
	{
		setters = setters.GetTypedGetterSetter(sink, ordinal);
		sink.ProcessMessagesAndThrow();
		IEnumerator<SqlDataRecord> enumerator = null;
		try
		{
			SmiExtendedMetaData[] array = new SmiExtendedMetaData[metaData.FieldMetaData.Count];
			metaData.FieldMetaData.CopyTo(array, 0);
			SmiDefaultFieldsProperty useDefaultValues = (SmiDefaultFieldsProperty)metaData.ExtendedProperties[SmiPropertySelector.DefaultFields];
			int num = 1;
			if (peekAhead != null && peekAhead.FirstRecord != null)
			{
				enumerator = peekAhead.Enumerator;
				setters.NewElement(sink);
				sink.ProcessMessagesAndThrow();
				SmiTypedGetterSetter setters2 = setters;
				SmiMetaData[] metaData2 = array;
				FillCompatibleSettersFromRecord(sink, setters2, metaData2, peekAhead.FirstRecord, useDefaultValues);
				num++;
			}
			else
			{
				enumerator = value.GetEnumerator();
			}
			using (enumerator)
			{
				while (enumerator.MoveNext())
				{
					setters.NewElement(sink);
					sink.ProcessMessagesAndThrow();
					SqlDataRecord current = enumerator.Current;
					if (current.FieldCount != array.Length)
					{
						throw SQL.EnumeratedRecordFieldCountChanged(num);
					}
					for (int i = 0; i < current.FieldCount; i++)
					{
						if (!MetaDataUtilsSmi.IsCompatible(metaData.FieldMetaData[i], current.GetSqlMetaData(i)))
						{
							throw SQL.EnumeratedRecordMetaDataChanged(current.GetName(i), num);
						}
					}
					SmiTypedGetterSetter setters3 = setters;
					SmiMetaData[] metaData2 = array;
					FillCompatibleSettersFromRecord(sink, setters3, metaData2, current, useDefaultValues);
					num++;
				}
				setters.EndElements(sink);
				sink.ProcessMessagesAndThrow();
			}
		}
		finally
		{
			enumerator?.Dispose();
		}
	}
}
