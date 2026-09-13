using System.Collections.Generic;
using System.ComponentModel;

namespace System.Runtime;

internal static class TypeHelper
{
	public static readonly Type ArrayType = typeof(Array);

	public static readonly Type BoolType = typeof(bool);

	public static readonly Type GenericCollectionType = typeof(ICollection<>);

	public static readonly Type ByteType = typeof(byte);

	public static readonly Type SByteType = typeof(sbyte);

	public static readonly Type CharType = typeof(char);

	public static readonly Type ShortType = typeof(short);

	public static readonly Type UShortType = typeof(ushort);

	public static readonly Type IntType = typeof(int);

	public static readonly Type UIntType = typeof(uint);

	public static readonly Type LongType = typeof(long);

	public static readonly Type ULongType = typeof(ulong);

	public static readonly Type FloatType = typeof(float);

	public static readonly Type DoubleType = typeof(double);

	public static readonly Type DecimalType = typeof(decimal);

	public static readonly Type ExceptionType = typeof(Exception);

	public static readonly Type NullableType = typeof(Nullable<>);

	public static readonly Type ObjectType = typeof(object);

	public static readonly Type StringType = typeof(string);

	public static readonly Type TypeType = typeof(Type);

	public static readonly Type VoidType = typeof(void);

	public static bool AreTypesCompatible(object source, Type destinationType)
	{
		if (source == null)
		{
			if (destinationType.IsValueType)
			{
				return IsNullableType(destinationType);
			}
			return true;
		}
		return AreTypesCompatible(source.GetType(), destinationType);
	}

	public static bool AreTypesCompatible(Type sourceType, Type destinationType)
	{
		if ((object)sourceType == destinationType)
		{
			return true;
		}
		if (!IsImplicitNumericConversion(sourceType, destinationType) && !IsImplicitReferenceConversion(sourceType, destinationType) && !IsImplicitBoxingConversion(sourceType, destinationType))
		{
			return IsImplicitNullableConversion(sourceType, destinationType);
		}
		return true;
	}

	public static bool AreReferenceTypesCompatible(Type sourceType, Type destinationType)
	{
		if ((object)sourceType == destinationType)
		{
			return true;
		}
		return IsImplicitReferenceConversion(sourceType, destinationType);
	}

	public static IEnumerable<Type> GetCompatibleTypes(IEnumerable<Type> enumerable, Type targetType)
	{
		foreach (Type item in enumerable)
		{
			if (AreTypesCompatible(item, targetType))
			{
				yield return item;
			}
		}
	}

	public static bool ContainsCompatibleType(IEnumerable<Type> enumerable, Type targetType)
	{
		foreach (Type item in enumerable)
		{
			if (AreTypesCompatible(item, targetType))
			{
				return true;
			}
		}
		return false;
	}

	public static T Convert<T>(object source)
	{
		if (source is T)
		{
			return (T)source;
		}
		if (source == null)
		{
			if (typeof(T).IsValueType && !IsNullableType(typeof(T)))
			{
				throw Fx.Exception.AsError(new InvalidCastException(InternalSR.CannotConvertObject(source, typeof(T))));
			}
			return default(T);
		}
		if (TryNumericConversion<T>(source, out var result))
		{
			return result;
		}
		throw Fx.Exception.AsError(new InvalidCastException(InternalSR.CannotConvertObject(source, typeof(T))));
	}

	public static IEnumerable<Type> GetImplementedTypes(Type type)
	{
		Dictionary<Type, object> dictionary = new Dictionary<Type, object>();
		GetImplementedTypesHelper(type, dictionary);
		return dictionary.Keys;
	}

	private static void GetImplementedTypesHelper(Type type, Dictionary<Type, object> typesEncountered)
	{
		if (!typesEncountered.ContainsKey(type))
		{
			typesEncountered.Add(type, type);
			Type[] interfaces = type.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				GetImplementedTypesHelper(interfaces[i], typesEncountered);
			}
			Type baseType = type.BaseType;
			while (baseType != null && baseType != ObjectType)
			{
				GetImplementedTypesHelper(baseType, typesEncountered);
				baseType = baseType.BaseType;
			}
		}
	}

	private static bool IsImplicitNumericConversion(Type source, Type destination)
	{
		TypeCode typeCode = Type.GetTypeCode(source);
		TypeCode typeCode2 = Type.GetTypeCode(destination);
		switch (typeCode)
		{
		case TypeCode.SByte:
			switch (typeCode2)
			{
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return true;
			default:
				return false;
			}
		case TypeCode.Byte:
			if ((uint)(typeCode2 - 7) <= 8u)
			{
				return true;
			}
			return false;
		case TypeCode.Int16:
			switch (typeCode2)
			{
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return true;
			default:
				return false;
			}
		case TypeCode.UInt16:
			if ((uint)(typeCode2 - 9) <= 6u)
			{
				return true;
			}
			return false;
		case TypeCode.Int32:
			if (typeCode2 == TypeCode.Int64 || (uint)(typeCode2 - 13) <= 2u)
			{
				return true;
			}
			return false;
		case TypeCode.UInt32:
			if ((uint)(typeCode2 - 10) <= 5u)
			{
				return true;
			}
			return false;
		case TypeCode.Int64:
		case TypeCode.UInt64:
			if ((uint)(typeCode2 - 13) <= 2u)
			{
				return true;
			}
			return false;
		case TypeCode.Char:
			if ((uint)(typeCode2 - 8) <= 7u)
			{
				return true;
			}
			return false;
		case TypeCode.Single:
			return typeCode2 == TypeCode.Double;
		default:
			return false;
		}
	}

	private static bool IsImplicitReferenceConversion(Type sourceType, Type destinationType)
	{
		return destinationType.IsAssignableFrom(sourceType);
	}

	private static bool IsImplicitBoxingConversion(Type sourceType, Type destinationType)
	{
		if (sourceType.IsValueType && (destinationType == ObjectType || destinationType == typeof(ValueType)))
		{
			return true;
		}
		if (sourceType.IsEnum && destinationType == typeof(Enum))
		{
			return true;
		}
		return false;
	}

	private static bool IsImplicitNullableConversion(Type sourceType, Type destinationType)
	{
		if (!IsNullableType(destinationType))
		{
			return false;
		}
		destinationType = destinationType.GetGenericArguments()[0];
		if (IsNullableType(sourceType))
		{
			sourceType = sourceType.GetGenericArguments()[0];
		}
		return AreTypesCompatible(sourceType, destinationType);
	}

	private static bool IsNullableType(Type type)
	{
		if (type.IsGenericType)
		{
			return type.GetGenericTypeDefinition() == NullableType;
		}
		return false;
	}

	private static bool TryNumericConversion<T>(object source, out T result)
	{
		TypeCode typeCode = Type.GetTypeCode(source.GetType());
		TypeCode typeCode2 = Type.GetTypeCode(typeof(T));
		switch (typeCode)
		{
		case TypeCode.SByte:
		{
			sbyte b = (sbyte)source;
			switch (typeCode2)
			{
			case TypeCode.Int16:
				result = (T)(object)(short)b;
				return true;
			case TypeCode.Int32:
				result = (T)(object)(int)b;
				return true;
			case TypeCode.Int64:
				result = (T)(object)(long)b;
				return true;
			case TypeCode.Single:
				result = (T)(object)(float)b;
				return true;
			case TypeCode.Double:
				result = (T)(object)(double)b;
				return true;
			case TypeCode.Decimal:
				result = (T)(object)(decimal)b;
				return true;
			}
			break;
		}
		case TypeCode.Byte:
		{
			byte b2 = (byte)source;
			switch (typeCode2)
			{
			case TypeCode.Int16:
				result = (T)(object)(short)b2;
				return true;
			case TypeCode.UInt16:
				result = (T)(object)(ushort)b2;
				return true;
			case TypeCode.Int32:
				result = (T)(object)(int)b2;
				return true;
			case TypeCode.UInt32:
				result = (T)(object)(uint)b2;
				return true;
			case TypeCode.Int64:
				result = (T)(object)(long)b2;
				return true;
			case TypeCode.UInt64:
				result = (T)(object)(ulong)b2;
				return true;
			case TypeCode.Single:
				result = (T)(object)(float)(int)b2;
				return true;
			case TypeCode.Double:
				result = (T)(object)(double)(int)b2;
				return true;
			case TypeCode.Decimal:
				result = (T)(object)(decimal)b2;
				return true;
			}
			break;
		}
		case TypeCode.Int16:
		{
			short num6 = (short)source;
			switch (typeCode2)
			{
			case TypeCode.Int32:
				result = (T)(object)(int)num6;
				return true;
			case TypeCode.Int64:
				result = (T)(object)(long)num6;
				return true;
			case TypeCode.Single:
				result = (T)(object)(float)num6;
				return true;
			case TypeCode.Double:
				result = (T)(object)(double)num6;
				return true;
			case TypeCode.Decimal:
				result = (T)(object)(decimal)num6;
				return true;
			}
			break;
		}
		case TypeCode.UInt16:
		{
			ushort num5 = (ushort)source;
			switch (typeCode2)
			{
			case TypeCode.Int32:
				result = (T)(object)(int)num5;
				return true;
			case TypeCode.UInt32:
				result = (T)(object)(uint)num5;
				return true;
			case TypeCode.Int64:
				result = (T)(object)(long)num5;
				return true;
			case TypeCode.UInt64:
				result = (T)(object)(ulong)num5;
				return true;
			case TypeCode.Single:
				result = (T)(object)(float)(int)num5;
				return true;
			case TypeCode.Double:
				result = (T)(object)(double)(int)num5;
				return true;
			case TypeCode.Decimal:
				result = (T)(object)(decimal)num5;
				return true;
			}
			break;
		}
		case TypeCode.Int32:
		{
			int num4 = (int)source;
			switch (typeCode2)
			{
			case TypeCode.Int64:
				result = (T)(object)(long)num4;
				return true;
			case TypeCode.Single:
				result = (T)(object)(float)num4;
				return true;
			case TypeCode.Double:
				result = (T)(object)(double)num4;
				return true;
			case TypeCode.Decimal:
				result = (T)(object)(decimal)num4;
				return true;
			}
			break;
		}
		case TypeCode.UInt32:
		{
			uint num3 = (uint)source;
			switch (typeCode2)
			{
			case TypeCode.UInt32:
				result = (T)(object)num3;
				return true;
			case TypeCode.Int64:
				result = (T)(object)(long)num3;
				return true;
			case TypeCode.UInt64:
				result = (T)(object)(ulong)num3;
				return true;
			case TypeCode.Single:
				result = (T)(object)(float)num3;
				return true;
			case TypeCode.Double:
				result = (T)(object)(double)num3;
				return true;
			case TypeCode.Decimal:
				result = (T)(object)(decimal)num3;
				return true;
			}
			break;
		}
		case TypeCode.Int64:
		{
			long num2 = (long)source;
			switch (typeCode2)
			{
			case TypeCode.Single:
				result = (T)(object)(float)num2;
				return true;
			case TypeCode.Double:
				result = (T)(object)(double)num2;
				return true;
			case TypeCode.Decimal:
				result = (T)(object)(decimal)num2;
				return true;
			}
			break;
		}
		case TypeCode.UInt64:
		{
			ulong num = (ulong)source;
			switch (typeCode2)
			{
			case TypeCode.Single:
				result = (T)(object)(float)num;
				return true;
			case TypeCode.Double:
				result = (T)(object)(double)num;
				return true;
			case TypeCode.Decimal:
				result = (T)(object)(decimal)num;
				return true;
			}
			break;
		}
		case TypeCode.Char:
		{
			char c = (char)source;
			switch (typeCode2)
			{
			case TypeCode.UInt16:
				result = (T)(object)(ushort)c;
				return true;
			case TypeCode.Int32:
				result = (T)(object)(int)c;
				return true;
			case TypeCode.UInt32:
				result = (T)(object)(uint)c;
				return true;
			case TypeCode.Int64:
				result = (T)(object)(long)c;
				return true;
			case TypeCode.UInt64:
				result = (T)(object)(ulong)c;
				return true;
			case TypeCode.Single:
				result = (T)(object)(float)(int)c;
				return true;
			case TypeCode.Double:
				result = (T)(object)(double)(int)c;
				return true;
			case TypeCode.Decimal:
				result = (T)(object)(decimal)c;
				return true;
			}
			break;
		}
		case TypeCode.Single:
			if (typeCode2 == TypeCode.Double)
			{
				result = (T)(object)(double)(float)source;
				return true;
			}
			break;
		}
		result = default(T);
		return false;
	}

	public static object GetDefaultValueForType(Type type)
	{
		if (!type.IsValueType)
		{
			return null;
		}
		if (type.IsEnum)
		{
			Array values = Enum.GetValues(type);
			if (values.Length > 0)
			{
				return values.GetValue(0);
			}
		}
		return Activator.CreateInstance(type);
	}

	public static bool IsNullableValueType(Type type)
	{
		if (type.IsValueType)
		{
			return IsNullableType(type);
		}
		return false;
	}

	public static bool IsNonNullableValueType(Type type)
	{
		if (!type.IsValueType)
		{
			return false;
		}
		if (type.IsGenericType)
		{
			return false;
		}
		return type != StringType;
	}

	public static bool ShouldFilterProperty(PropertyDescriptor property, Attribute[] attributes)
	{
		if (attributes == null || attributes.Length == 0)
		{
			return false;
		}
		foreach (Attribute attribute in attributes)
		{
			Attribute attribute2 = property.Attributes[attribute.GetType()];
			if (attribute2 == null)
			{
				if (!attribute.IsDefaultAttribute())
				{
					return true;
				}
			}
			else if (!attribute.Match(attribute2))
			{
				return true;
			}
		}
		return false;
	}
}
