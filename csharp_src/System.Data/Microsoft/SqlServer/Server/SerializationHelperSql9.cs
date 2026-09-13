using System;
using System.Collections;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Server;

internal class SerializationHelperSql9
{
	[ThreadStatic]
	private static Hashtable s_types2Serializers;

	private SerializationHelperSql9()
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static int SizeInBytes(Type t)
	{
		return SizeInBytes(Activator.CreateInstance(t));
	}

	internal static int SizeInBytes(object instance)
	{
		GetFormat(instance.GetType());
		DummyStream dummyStream = new DummyStream();
		GetSerializer(instance.GetType()).Serialize(dummyStream, instance);
		return (int)dummyStream.Length;
	}

	internal static void Serialize(Stream s, object instance)
	{
		GetSerializer(instance.GetType()).Serialize(s, instance);
	}

	internal static object Deserialize(Stream s, Type resultType)
	{
		return GetSerializer(resultType).Deserialize(s);
	}

	private static Format GetFormat(Type t)
	{
		return GetUdtAttribute(t).Format;
	}

	private static Serializer GetSerializer(Type t)
	{
		if (s_types2Serializers == null)
		{
			s_types2Serializers = new Hashtable();
		}
		Serializer serializer = (Serializer)s_types2Serializers[t];
		if (serializer == null)
		{
			serializer = GetNewSerializer(t);
			s_types2Serializers[t] = serializer;
		}
		return serializer;
	}

	internal static int GetUdtMaxLength(Type t)
	{
		SqlUdtInfo fromType = SqlUdtInfo.GetFromType(t);
		if (Format.Native == fromType.SerializationFormat)
		{
			return SizeInBytes(t);
		}
		return fromType.MaxByteSize;
	}

	private static object[] GetCustomAttributes(Type t)
	{
		return t.GetCustomAttributes(typeof(SqlUserDefinedTypeAttribute), inherit: false);
	}

	internal static SqlUserDefinedTypeAttribute GetUdtAttribute(Type t)
	{
		SqlUserDefinedTypeAttribute sqlUserDefinedTypeAttribute = null;
		object[] customAttributes = GetCustomAttributes(t);
		if (customAttributes != null && customAttributes.Length == 1)
		{
			return (SqlUserDefinedTypeAttribute)customAttributes[0];
		}
		throw InvalidUdtException.Create(t, "no UDT attribute");
	}

	private static Serializer GetNewSerializer(Type t)
	{
		GetUdtAttribute(t);
		Format format = GetFormat(t);
		return format switch
		{
			Format.Native => new NormalizedSerializer(t), 
			Format.UserDefined => new BinarySerializeSerializer(t), 
			_ => throw ADP.InvalidUserDefinedTypeSerializationFormat(format), 
		};
	}
}
