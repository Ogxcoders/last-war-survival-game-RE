using System.Collections;
using System.Collections.Concurrent;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common;

internal sealed class SqlUdtStorage : DataStorage
{
	private object[] _values;

	private readonly bool _implementsIXmlSerializable;

	private readonly bool _implementsIComparable;

	private static readonly ConcurrentDictionary<Type, object> s_typeToNull = new ConcurrentDictionary<Type, object>();

	public SqlUdtStorage(DataColumn column, Type type)
		: this(column, type, GetStaticNullForUdtType(type))
	{
	}

	private SqlUdtStorage(DataColumn column, Type type, object nullValue)
		: base(column, type, nullValue, nullValue, typeof(ICloneable).IsAssignableFrom(type), DataStorage.GetStorageType(type))
	{
		_implementsIXmlSerializable = typeof(IXmlSerializable).IsAssignableFrom(type);
		_implementsIComparable = typeof(IComparable).IsAssignableFrom(type);
	}

	internal static object GetStaticNullForUdtType(Type type)
	{
		return s_typeToNull.GetOrAdd(type, delegate
		{
			PropertyInfo property = type.GetProperty("Null", BindingFlags.Static | BindingFlags.Public);
			if (property != null)
			{
				return property.GetValue(null, null);
			}
			FieldInfo field = type.GetField("Null", BindingFlags.Static | BindingFlags.Public);
			if (field != null)
			{
				return field.GetValue(null);
			}
			throw ExceptionBuilder.INullableUDTwithoutStaticNull(type.AssemblyQualifiedName);
		});
	}

	public override bool IsNull(int record)
	{
		return ((INullable)_values[record]).IsNull;
	}

	public override object Aggregate(int[] records, AggregateType kind)
	{
		throw ExceptionBuilder.AggregateException(kind, _dataType);
	}

	public override int Compare(int recordNo1, int recordNo2)
	{
		return CompareValueTo(recordNo1, _values[recordNo2]);
	}

	public override int CompareValueTo(int recordNo1, object value)
	{
		if (DBNull.Value == value)
		{
			value = _nullValue;
		}
		if (_implementsIComparable)
		{
			return ((IComparable)_values[recordNo1]).CompareTo(value);
		}
		if (_nullValue == value)
		{
			if (!((INullable)_values[recordNo1]).IsNull)
			{
				return 1;
			}
			return 0;
		}
		throw ExceptionBuilder.IComparableNotImplemented(_dataType.AssemblyQualifiedName);
	}

	public override void Copy(int recordNo1, int recordNo2)
	{
		CopyBits(recordNo1, recordNo2);
		_values[recordNo2] = _values[recordNo1];
	}

	public override object Get(int recordNo)
	{
		return _values[recordNo];
	}

	public override void Set(int recordNo, object value)
	{
		if (DBNull.Value == value)
		{
			_values[recordNo] = _nullValue;
			SetNullBit(recordNo, flag: true);
		}
		else if (value == null)
		{
			if (_isValueType)
			{
				throw ExceptionBuilder.StorageSetFailed();
			}
			_values[recordNo] = _nullValue;
			SetNullBit(recordNo, flag: true);
		}
		else
		{
			if (!_dataType.IsInstanceOfType(value))
			{
				throw ExceptionBuilder.StorageSetFailed();
			}
			_values[recordNo] = value;
			SetNullBit(recordNo, flag: false);
		}
	}

	public override void SetCapacity(int capacity)
	{
		object[] array = new object[capacity];
		if (_values != null)
		{
			Array.Copy(_values, 0, array, 0, Math.Min(capacity, _values.Length));
		}
		_values = array;
		base.SetCapacity(capacity);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override object ConvertXmlToObject(string s)
	{
		if (_implementsIXmlSerializable)
		{
			object obj = Activator.CreateInstance(_dataType, nonPublic: true);
			using XmlTextReader reader = new XmlTextReader(new StringReader("<col>" + s + "</col>"));
			((IXmlSerializable)obj).ReadXml(reader);
			return obj;
		}
		StringReader textReader = new StringReader(s);
		return ObjectStorage.GetXmlSerializer(_dataType).Deserialize(textReader);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override object ConvertXmlToObject(XmlReader xmlReader, XmlRootAttribute xmlAttrib)
	{
		if (xmlAttrib == null)
		{
			string text = xmlReader.GetAttribute("InstanceType", "urn:schemas-microsoft-com:xml-msdata");
			if (text == null)
			{
				string attribute = xmlReader.GetAttribute("InstanceType", "http://www.w3.org/2001/XMLSchema-instance");
				if (attribute != null)
				{
					text = XSDSchema.XsdtoClr(attribute).FullName;
				}
			}
			object obj = Activator.CreateInstance((text == null) ? _dataType : Type.GetType(text), nonPublic: true);
			((IXmlSerializable)obj).ReadXml(xmlReader);
			return obj;
		}
		return ObjectStorage.GetXmlSerializer(_dataType, xmlAttrib).Deserialize(xmlReader);
	}

	public override string ConvertObjectToXml(object value)
	{
		StringWriter stringWriter = new StringWriter(base.FormatProvider);
		if (_implementsIXmlSerializable)
		{
			using XmlTextWriter writer = new XmlTextWriter(stringWriter);
			((IXmlSerializable)value).WriteXml(writer);
		}
		else
		{
			ObjectStorage.GetXmlSerializer(value.GetType()).Serialize(stringWriter, value);
		}
		return stringWriter.ToString();
	}

	public override void ConvertObjectToXml(object value, XmlWriter xmlWriter, XmlRootAttribute xmlAttrib)
	{
		if (xmlAttrib == null)
		{
			((IXmlSerializable)value).WriteXml(xmlWriter);
		}
		else
		{
			ObjectStorage.GetXmlSerializer(_dataType, xmlAttrib).Serialize(xmlWriter, value);
		}
	}

	protected override object GetEmptyStorage(int recordCount)
	{
		return new object[recordCount];
	}

	protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
	{
		((object[])store)[storeIndex] = _values[record];
		nullbits.Set(storeIndex, IsNull(record));
	}

	protected override void SetStorage(object store, BitArray nullbits)
	{
		_values = (object[])store;
	}
}
