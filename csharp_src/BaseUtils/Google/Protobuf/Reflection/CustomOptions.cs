using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection;

public sealed class CustomOptions
{
	private static readonly object[] EmptyParameters = new object[0];

	private readonly IDictionary<int, IExtensionValue> values;

	internal CustomOptions(IDictionary<int, IExtensionValue> values)
	{
		this.values = values;
	}

	public bool TryGetBool(int field, out bool value)
	{
		return TryGetPrimitiveValue<bool>(field, out value);
	}

	public bool TryGetInt32(int field, out int value)
	{
		return TryGetPrimitiveValue<int>(field, out value);
	}

	public bool TryGetInt64(int field, out long value)
	{
		return TryGetPrimitiveValue<long>(field, out value);
	}

	public bool TryGetFixed32(int field, out uint value)
	{
		return TryGetUInt32(field, out value);
	}

	public bool TryGetFixed64(int field, out ulong value)
	{
		return TryGetUInt64(field, out value);
	}

	public bool TryGetSFixed32(int field, out int value)
	{
		return TryGetInt32(field, out value);
	}

	public bool TryGetSFixed64(int field, out long value)
	{
		return TryGetInt64(field, out value);
	}

	public bool TryGetSInt32(int field, out int value)
	{
		return TryGetPrimitiveValue<int>(field, out value);
	}

	public bool TryGetSInt64(int field, out long value)
	{
		return TryGetPrimitiveValue<long>(field, out value);
	}

	public bool TryGetUInt32(int field, out uint value)
	{
		return TryGetPrimitiveValue<uint>(field, out value);
	}

	public bool TryGetUInt64(int field, out ulong value)
	{
		return TryGetPrimitiveValue<ulong>(field, out value);
	}

	public bool TryGetFloat(int field, out float value)
	{
		return TryGetPrimitiveValue<float>(field, out value);
	}

	public bool TryGetDouble(int field, out double value)
	{
		return TryGetPrimitiveValue<double>(field, out value);
	}

	public bool TryGetString(int field, out string value)
	{
		return TryGetPrimitiveValue<string>(field, out value);
	}

	public bool TryGetBytes(int field, out ByteString value)
	{
		return TryGetPrimitiveValue<ByteString>(field, out value);
	}

	public bool TryGetMessage<T>(int field, out T value) where T : class, IMessage, new()
	{
		if (values == null)
		{
			value = null;
			return false;
		}
		if (values.TryGetValue(field, out var value2))
		{
			if (value2 is ExtensionValue<T>)
			{
				ByteString data = (value2 as ExtensionValue<T>).GetValue().ToByteString();
				value = new T();
				value.MergeFrom(data);
				return true;
			}
			if (value2 is RepeatedExtensionValue<T>)
			{
				RepeatedExtensionValue<T> repeatedExtensionValue = value2 as RepeatedExtensionValue<T>;
				value = (from v in repeatedExtensionValue.GetValue()
					select v.ToByteString()).Aggregate(new T(), delegate(T t, ByteString b)
				{
					t.MergeFrom(b);
					return t;
				});
				return true;
			}
		}
		value = null;
		return false;
	}

	private bool TryGetPrimitiveValue<T>(int field, out T value)
	{
		if (values == null)
		{
			value = default(T);
			return false;
		}
		if (values.TryGetValue(field, out var value2))
		{
			if (value2 is ExtensionValue<T>)
			{
				ExtensionValue<T> extensionValue = value2 as ExtensionValue<T>;
				value = extensionValue.GetValue();
				return true;
			}
			if (value2 is RepeatedExtensionValue<T>)
			{
				RepeatedExtensionValue<T> repeatedExtensionValue = value2 as RepeatedExtensionValue<T>;
				if (repeatedExtensionValue.GetValue().Count != 0)
				{
					RepeatedField<T> value3 = repeatedExtensionValue.GetValue();
					value = value3[value3.Count - 1];
					return true;
				}
			}
			else
			{
				Type type = value2.GetType();
				if (type.GetGenericTypeDefinition() == typeof(ExtensionValue<>))
				{
					TypeInfo typeInfo = type.GetTypeInfo();
					Type[] genericTypeArguments = typeInfo.GenericTypeArguments;
					if (genericTypeArguments.Length == 1 && genericTypeArguments[0].GetTypeInfo().IsEnum)
					{
						value = (T)typeInfo.GetDeclaredMethod("GetValue").Invoke(value2, EmptyParameters);
						return true;
					}
				}
				else if (type.GetGenericTypeDefinition() == typeof(RepeatedExtensionValue<>))
				{
					TypeInfo typeInfo2 = type.GetTypeInfo();
					Type[] genericTypeArguments2 = typeInfo2.GenericTypeArguments;
					if (genericTypeArguments2.Length == 1 && genericTypeArguments2[0].GetTypeInfo().IsEnum)
					{
						IList list = (IList)typeInfo2.GetDeclaredMethod("GetValue").Invoke(value2, EmptyParameters);
						if (list.Count != 0)
						{
							value = (T)list[list.Count - 1];
							return true;
						}
					}
				}
			}
		}
		value = default(T);
		return false;
	}
}
