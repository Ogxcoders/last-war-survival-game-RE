using System.ComponentModel;
using System.Data.ProviderBase;

namespace System.Data.Common;

internal sealed class DataRecordInternal : DbDataRecord, ICustomTypeDescriptor
{
	private SchemaInfo[] _schemaInfo;

	private object[] _values;

	private PropertyDescriptorCollection _propertyDescriptors;

	private FieldNameLookup _fieldNameLookup;

	public override int FieldCount => _schemaInfo.Length;

	public override object this[int i] => GetValue(i);

	public override object this[string name] => GetValue(GetOrdinal(name));

	internal DataRecordInternal(SchemaInfo[] schemaInfo, object[] values, PropertyDescriptorCollection descriptors, FieldNameLookup fieldNameLookup)
	{
		_schemaInfo = schemaInfo;
		_values = values;
		_propertyDescriptors = descriptors;
		_fieldNameLookup = fieldNameLookup;
	}

	public override int GetValues(object[] values)
	{
		if (values == null)
		{
			throw ADP.ArgumentNull("values");
		}
		int num = ((values.Length < _schemaInfo.Length) ? values.Length : _schemaInfo.Length);
		for (int i = 0; i < num; i++)
		{
			values[i] = _values[i];
		}
		return num;
	}

	public override string GetName(int i)
	{
		return _schemaInfo[i].name;
	}

	public override object GetValue(int i)
	{
		return _values[i];
	}

	public override string GetDataTypeName(int i)
	{
		return _schemaInfo[i].typeName;
	}

	public override Type GetFieldType(int i)
	{
		return _schemaInfo[i].type;
	}

	public override int GetOrdinal(string name)
	{
		return _fieldNameLookup.GetOrdinal(name);
	}

	public override bool GetBoolean(int i)
	{
		return (bool)_values[i];
	}

	public override byte GetByte(int i)
	{
		return (byte)_values[i];
	}

	public override long GetBytes(int i, long dataIndex, byte[] buffer, int bufferIndex, int length)
	{
		int num = 0;
		byte[] array = (byte[])_values[i];
		num = array.Length;
		if (dataIndex > int.MaxValue)
		{
			throw ADP.InvalidSourceBufferIndex(num, dataIndex, "dataIndex");
		}
		int num2 = (int)dataIndex;
		if (buffer == null)
		{
			return num;
		}
		try
		{
			if (num2 < num)
			{
				num = ((num2 + length <= num) ? length : (num - num2));
			}
			Array.Copy(array, num2, buffer, bufferIndex, num);
		}
		catch (Exception e) when (ADP.IsCatchableExceptionType(e))
		{
			num = array.Length;
			if (length < 0)
			{
				throw ADP.InvalidDataLength(length);
			}
			if (bufferIndex < 0 || bufferIndex >= buffer.Length)
			{
				throw ADP.InvalidDestinationBufferIndex(length, bufferIndex, "bufferIndex");
			}
			if (dataIndex < 0 || dataIndex >= num)
			{
				throw ADP.InvalidSourceBufferIndex(length, dataIndex, "dataIndex");
			}
			if (num + bufferIndex > buffer.Length)
			{
				throw ADP.InvalidBufferSizeOrIndex(num, bufferIndex);
			}
		}
		return num;
	}

	public override char GetChar(int i)
	{
		return ((string)_values[i])[0];
	}

	public override long GetChars(int i, long dataIndex, char[] buffer, int bufferIndex, int length)
	{
		char[] array = ((string)_values[i]).ToCharArray();
		int num = array.Length;
		if (dataIndex > int.MaxValue)
		{
			throw ADP.InvalidSourceBufferIndex(num, dataIndex, "dataIndex");
		}
		int num2 = (int)dataIndex;
		if (buffer == null)
		{
			return num;
		}
		try
		{
			if (num2 < num)
			{
				num = ((num2 + length <= num) ? length : (num - num2));
			}
			Array.Copy(array, num2, buffer, bufferIndex, num);
		}
		catch (Exception e) when (ADP.IsCatchableExceptionType(e))
		{
			num = array.Length;
			if (length < 0)
			{
				throw ADP.InvalidDataLength(length);
			}
			if (bufferIndex < 0 || bufferIndex >= buffer.Length)
			{
				throw ADP.InvalidDestinationBufferIndex(buffer.Length, bufferIndex, "bufferIndex");
			}
			if (num2 < 0 || num2 >= num)
			{
				throw ADP.InvalidSourceBufferIndex(num, dataIndex, "dataIndex");
			}
			if (num + bufferIndex > buffer.Length)
			{
				throw ADP.InvalidBufferSizeOrIndex(num, bufferIndex);
			}
		}
		return num;
	}

	public override Guid GetGuid(int i)
	{
		return (Guid)_values[i];
	}

	public override short GetInt16(int i)
	{
		return (short)_values[i];
	}

	public override int GetInt32(int i)
	{
		return (int)_values[i];
	}

	public override long GetInt64(int i)
	{
		return (long)_values[i];
	}

	public override float GetFloat(int i)
	{
		return (float)_values[i];
	}

	public override double GetDouble(int i)
	{
		return (double)_values[i];
	}

	public override string GetString(int i)
	{
		return (string)_values[i];
	}

	public override decimal GetDecimal(int i)
	{
		return (decimal)_values[i];
	}

	public override DateTime GetDateTime(int i)
	{
		return (DateTime)_values[i];
	}

	public override bool IsDBNull(int i)
	{
		object obj = _values[i];
		if (obj != null)
		{
			return Convert.IsDBNull(obj);
		}
		return true;
	}

	AttributeCollection ICustomTypeDescriptor.GetAttributes()
	{
		return new AttributeCollection((Attribute[])null);
	}

	string ICustomTypeDescriptor.GetClassName()
	{
		return null;
	}

	string ICustomTypeDescriptor.GetComponentName()
	{
		return null;
	}

	TypeConverter ICustomTypeDescriptor.GetConverter()
	{
		return null;
	}

	EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
	{
		return null;
	}

	PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
	{
		return null;
	}

	object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
	{
		return null;
	}

	EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
	{
		return new EventDescriptorCollection(null);
	}

	EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
	{
		return new EventDescriptorCollection(null);
	}

	PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
	{
		return ((ICustomTypeDescriptor)this).GetProperties((Attribute[])null);
	}

	PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
	{
		if (_propertyDescriptors == null)
		{
			_propertyDescriptors = new PropertyDescriptorCollection(null);
		}
		return _propertyDescriptors;
	}

	object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
	{
		return this;
	}
}
