using System.Xml;

namespace System.Web.Services.Protocols;

public abstract class MimeFormatter
{
	public static MimeFormatter CreateInstance(Type type, object initializer)
	{
		MimeFormatter obj = (MimeFormatter)Activator.CreateInstance(type);
		obj.Initialize(initializer);
		return obj;
	}

	public abstract object GetInitializer(LogicalMethodInfo methodInfo);

	public static object GetInitializer(Type type, LogicalMethodInfo methodInfo)
	{
		return ((MimeFormatter)Activator.CreateInstance(type)).GetInitializer(methodInfo);
	}

	public virtual object[] GetInitializers(LogicalMethodInfo[] methodInfos)
	{
		object[] array = new object[methodInfos.Length];
		for (int i = 0; i < methodInfos.Length; i++)
		{
			array[i] = GetInitializer(methodInfos[i]);
		}
		return array;
	}

	public static object[] GetInitializers(Type type, LogicalMethodInfo[] methodInfos)
	{
		return ((MimeFormatter)Activator.CreateInstance(type)).GetInitializers(methodInfos);
	}

	public abstract void Initialize(object initializer);

	internal static object StringToObj(Type type, string value)
	{
		if (type.IsEnum)
		{
			return Enum.Parse(type, value);
		}
		switch (Type.GetTypeCode(type))
		{
		case TypeCode.Boolean:
			return XmlConvert.ToBoolean(value);
		case TypeCode.Byte:
			return XmlConvert.ToByte(value);
		case TypeCode.DateTime:
			return XmlConvert.ToDateTime(value);
		case TypeCode.Decimal:
			return XmlConvert.ToDecimal(value);
		case TypeCode.Double:
			return XmlConvert.ToDouble(value);
		case TypeCode.Int16:
			return XmlConvert.ToInt16(value);
		case TypeCode.Int32:
			return XmlConvert.ToInt32(value);
		case TypeCode.Int64:
			return XmlConvert.ToInt64(value);
		case TypeCode.SByte:
			return XmlConvert.ToSByte(value);
		case TypeCode.Single:
			return XmlConvert.ToSingle(value);
		case TypeCode.UInt16:
			return XmlConvert.ToUInt16(value);
		case TypeCode.UInt32:
			return XmlConvert.ToUInt32(value);
		case TypeCode.UInt64:
			return XmlConvert.ToUInt64(value);
		case TypeCode.String:
			return value;
		case TypeCode.Char:
			if (value.Length != 1)
			{
				throw new InvalidOperationException("Invalid char value");
			}
			return value[0];
		default:
			throw new InvalidOperationException("Type not supported");
		}
	}

	internal static string ObjToString(object value)
	{
		if (value == null)
		{
			return "";
		}
		switch (Type.GetTypeCode(value.GetType()))
		{
		case TypeCode.Boolean:
			return XmlConvert.ToString((bool)value);
		case TypeCode.Byte:
			return XmlConvert.ToString((byte)value);
		case TypeCode.Char:
			return XmlConvert.ToString((char)value);
		case TypeCode.DateTime:
			return XmlConvert.ToString((DateTime)value);
		case TypeCode.Decimal:
			return XmlConvert.ToString((decimal)value);
		case TypeCode.Double:
			return XmlConvert.ToString((double)value);
		case TypeCode.Int16:
			return XmlConvert.ToString((short)value);
		case TypeCode.Int32:
			return XmlConvert.ToString((int)value);
		case TypeCode.Int64:
			return XmlConvert.ToString((long)value);
		case TypeCode.SByte:
			return XmlConvert.ToString((sbyte)value);
		case TypeCode.Single:
			return XmlConvert.ToString((float)value);
		case TypeCode.UInt16:
			return XmlConvert.ToString((ushort)value);
		case TypeCode.UInt32:
			return XmlConvert.ToString((uint)value);
		case TypeCode.UInt64:
			return XmlConvert.ToString((ulong)value);
		case TypeCode.String:
			return value as string;
		default:
			if (value.GetType().IsEnum)
			{
				return value.ToString();
			}
			throw new InvalidOperationException("Type not supported");
		}
	}
}
