using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common;

internal sealed class SqlBooleanStorage : DataStorage
{
	private SqlBoolean[] _values;

	public SqlBooleanStorage(DataColumn column)
		: base(column, typeof(SqlBoolean), SqlBoolean.Null, SqlBoolean.Null, StorageType.SqlBoolean)
	{
	}

	public override object Aggregate(int[] records, AggregateType kind)
	{
		bool flag = false;
		try
		{
			switch (kind)
			{
			case AggregateType.Min:
			{
				SqlBoolean sqlBoolean2 = true;
				foreach (int num3 in records)
				{
					if (!IsNull(num3))
					{
						sqlBoolean2 = SqlBoolean.And(_values[num3], sqlBoolean2);
						flag = true;
					}
				}
				if (flag)
				{
					return sqlBoolean2;
				}
				return _nullValue;
			}
			case AggregateType.Max:
			{
				SqlBoolean sqlBoolean = false;
				foreach (int num2 in records)
				{
					if (!IsNull(num2))
					{
						sqlBoolean = SqlBoolean.Or(_values[num2], sqlBoolean);
						flag = true;
					}
				}
				if (flag)
				{
					return sqlBoolean;
				}
				return _nullValue;
			}
			case AggregateType.First:
				if (records.Length != 0)
				{
					return _values[records[0]];
				}
				return _nullValue;
			case AggregateType.Count:
			{
				int num = 0;
				for (int i = 0; i < records.Length; i++)
				{
					if (!IsNull(records[i]))
					{
						num++;
					}
				}
				return num;
			}
			}
		}
		catch (OverflowException)
		{
			throw ExprException.Overflow(typeof(SqlBoolean));
		}
		throw ExceptionBuilder.AggregateException(kind, _dataType);
	}

	public override int Compare(int recordNo1, int recordNo2)
	{
		return _values[recordNo1].CompareTo(_values[recordNo2]);
	}

	public override int CompareValueTo(int recordNo, object value)
	{
		return _values[recordNo].CompareTo((SqlBoolean)value);
	}

	public override object ConvertValue(object value)
	{
		if (value != null)
		{
			return SqlConvert.ConvertToSqlBoolean(value);
		}
		return _nullValue;
	}

	public override void Copy(int recordNo1, int recordNo2)
	{
		_values[recordNo2] = _values[recordNo1];
	}

	public override object Get(int record)
	{
		return _values[record];
	}

	public override bool IsNull(int record)
	{
		return _values[record].IsNull;
	}

	public override void Set(int record, object value)
	{
		_values[record] = SqlConvert.ConvertToSqlBoolean(value);
	}

	public override void SetCapacity(int capacity)
	{
		SqlBoolean[] array = new SqlBoolean[capacity];
		if (_values != null)
		{
			Array.Copy(_values, 0, array, 0, Math.Min(capacity, _values.Length));
		}
		_values = array;
	}

	public override object ConvertXmlToObject(string s)
	{
		SqlBoolean sqlBoolean = default(SqlBoolean);
		StringReader input = new StringReader("<col>" + s + "</col>");
		IXmlSerializable xmlSerializable = sqlBoolean;
		using (XmlTextReader reader = new XmlTextReader(input))
		{
			xmlSerializable.ReadXml(reader);
		}
		return (SqlBoolean)(object)xmlSerializable;
	}

	public override string ConvertObjectToXml(object value)
	{
		StringWriter stringWriter = new StringWriter(base.FormatProvider);
		using (XmlTextWriter writer = new XmlTextWriter(stringWriter))
		{
			((IXmlSerializable)value).WriteXml(writer);
		}
		return stringWriter.ToString();
	}

	protected override object GetEmptyStorage(int recordCount)
	{
		return new SqlBoolean[recordCount];
	}

	protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
	{
		((SqlBoolean[])store)[storeIndex] = _values[record];
		nullbits.Set(storeIndex, IsNull(record));
	}

	protected override void SetStorage(object store, BitArray nullbits)
	{
		_values = (SqlBoolean[])store;
	}
}
