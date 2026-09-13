using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient;

internal class SqlUdtInfo
{
	internal readonly Format SerializationFormat;

	internal readonly bool IsByteOrdered;

	internal readonly bool IsFixedLength;

	internal readonly int MaxByteSize;

	internal readonly string Name;

	internal readonly string ValidationMethodName;

	[ThreadStatic]
	private static Dictionary<Type, SqlUdtInfo> s_types2UdtInfo;

	private SqlUdtInfo(SqlUserDefinedTypeAttribute attr)
	{
		SerializationFormat = attr.Format;
		IsByteOrdered = attr.IsByteOrdered;
		IsFixedLength = attr.IsFixedLength;
		MaxByteSize = attr.MaxByteSize;
		Name = attr.Name;
		ValidationMethodName = attr.ValidationMethodName;
	}

	internal static SqlUdtInfo GetFromType(Type target)
	{
		return TryGetFromType(target) ?? throw InvalidUdtException.Create(target, "no UDT attribute");
	}

	internal static SqlUdtInfo TryGetFromType(Type target)
	{
		if (s_types2UdtInfo == null)
		{
			s_types2UdtInfo = new Dictionary<Type, SqlUdtInfo>();
		}
		SqlUdtInfo value = null;
		if (!s_types2UdtInfo.TryGetValue(target, out value))
		{
			object[] customAttributes = target.GetCustomAttributes(typeof(SqlUserDefinedTypeAttribute), inherit: false);
			if (customAttributes != null && customAttributes.Length == 1)
			{
				value = new SqlUdtInfo((SqlUserDefinedTypeAttribute)customAttributes[0]);
			}
			s_types2UdtInfo.Add(target, value);
		}
		return value;
	}
}
