using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common;

internal sealed class SqlDoubleStorage : DataStorage
{
	private SqlDouble[] _values;

	public SqlDoubleStorage(DataColumn column)
		: base(column, typeof(SqlDouble), SqlDouble.Null, SqlDouble.Null, StorageType.SqlDouble)
	{
	}

	public override object Aggregate(int[] records, AggregateType kind)
	{
		bool flag = false;
		try
		{
			switch (kind)
			{
			case AggregateType.Sum:
			{
				SqlDouble sqlDouble2 = 0.0;
				int[] array = records;
				foreach (int num3 in array)
				{
					if (!IsNull(num3))
					{
						sqlDouble2 += _values[num3];
						flag = true;
					}
				}
				if (flag)
				{
					return sqlDouble2;
				}
				return _nullValue;
			}
			case AggregateType.Mean:
			{
				SqlDouble sqlDouble6 = 0.0;
				int num5 = 0;
				int[] array = records;
				foreach (int num6 in array)
				{
					if (!IsNull(num6))
					{
						sqlDouble6 += _values[num6];
						num5++;
						flag = true;
					}
				}
				if (flag)
				{
					_ = (SqlDouble)0.0;
					return sqlDouble6 / num5;
				}
				return _nullValue;
			}
			case AggregateType.Var:
			case AggregateType.StDev:
			{
				int num = 0;
				SqlDouble sqlDouble3 = 0.0;
				_ = (SqlDouble)0.0;
				SqlDouble sqlDouble4 = 0.0;
				SqlDouble sqlDouble5 = 0.0;
				int[] array = records;
				foreach (int num4 in array)
				{
					if (!IsNull(num4))
					{
						sqlDouble4 += _values[num4];
						sqlDouble5 += _values[num4] * _values[num4];
						num++;
					}
				}
				if (num > 1)
				{
					sqlDouble3 = num * sqlDouble5 - sqlDouble4 * sqlDouble4;
					if (sqlDouble3 / (sqlDouble4 * sqlDouble4) < 1E-15 || sqlDouble3 < 0.0)
					{
						sqlDouble3 = 0.0;
					}
					else
					{
						sqlDouble3 /= (SqlDouble)(num * (num - 1));
					}
					if (kind == AggregateType.StDev)
					{
						return Math.Sqrt(sqlDouble3.Value);
					}
					return sqlDouble3;
				}
				return _nullValue;
			}
			case AggregateType.Min:
			{
				SqlDouble sqlDouble = SqlDouble.MaxValue;
				foreach (int num2 in records)
				{
					if (!IsNull(num2))
					{
						if (SqlDouble.LessThan(_values[num2], sqlDouble).IsTrue)
						{
							sqlDouble = _values[num2];
						}
						flag = true;
					}
				}
				if (flag)
				{
					return sqlDouble;
				}
				return _nullValue;
			}
			case AggregateType.Max:
			{
				SqlDouble sqlDouble7 = SqlDouble.MinValue;
				foreach (int num7 in records)
				{
					if (!IsNull(num7))
					{
						if (SqlDouble.GreaterThan(_values[num7], sqlDouble7).IsTrue)
						{
							sqlDouble7 = _values[num7];
						}
						flag = true;
					}
				}
				if (flag)
				{
					return sqlDouble7;
				}
				return _nullValue;
			}
			case AggregateType.First:
				if (records.Length != 0)
				{
					return _values[records[0]];
				}
				return null;
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
			throw ExprException.Overflow(typeof(SqlDouble));
		}
		throw ExceptionBuilder.AggregateException(kind, _dataType);
	}

	public override int Compare(int recordNo1, int recordNo2)
	{
		return _values[recordNo1].CompareTo(_values[recordNo2]);
	}

	public override int CompareValueTo(int recordNo, object value)
	{
		return _values[recordNo].CompareTo((SqlDouble)value);
	}

	public override object ConvertValue(object value)
	{
		if (value != null)
		{
			return SqlConvert.ConvertToSqlDouble(value);
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
		_values[record] = SqlConvert.ConvertToSqlDouble(value);
	}

	public override void SetCapacity(int capacity)
	{
		SqlDouble[] array = new SqlDouble[capacity];
		if (_values != null)
		{
			Array.Copy(_values, 0, array, 0, Math.Min(capacity, _values.Length));
		}
		_values = array;
	}

	public override object ConvertXmlToObject(string s)
	{
		SqlDouble sqlDouble = default(SqlDouble);
		StringReader input = new StringReader("<col>" + s + "</col>");
		IXmlSerializable xmlSerializable = sqlDouble;
		using (XmlTextReader reader = new XmlTextReader(input))
		{
			xmlSerializable.ReadXml(reader);
		}
		return (SqlDouble)(object)xmlSerializable;
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
		return new SqlDouble[recordCount];
	}

	protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
	{
		((SqlDouble[])store)[storeIndex] = _values[record];
		nullbits.Set(storeIndex, IsNull(record));
	}

	protected override void SetStorage(object store, BitArray nullbits)
	{
		_values = (SqlDouble[])store;
	}
}
