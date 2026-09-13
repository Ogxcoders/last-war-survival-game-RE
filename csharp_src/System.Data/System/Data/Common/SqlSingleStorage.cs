using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common;

internal sealed class SqlSingleStorage : DataStorage
{
	private SqlSingle[] _values;

	public SqlSingleStorage(DataColumn column)
		: base(column, typeof(SqlSingle), SqlSingle.Null, SqlSingle.Null, StorageType.SqlSingle)
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
				SqlSingle sqlSingle2 = 0f;
				int[] array = records;
				foreach (int num3 in array)
				{
					if (!IsNull(num3))
					{
						sqlSingle2 += _values[num3];
						flag = true;
					}
				}
				if (flag)
				{
					return sqlSingle2;
				}
				return _nullValue;
			}
			case AggregateType.Mean:
			{
				SqlDouble sqlDouble4 = 0.0;
				int num5 = 0;
				int[] array = records;
				foreach (int num6 in array)
				{
					if (!IsNull(num6))
					{
						sqlDouble4 += _values[num6].ToSqlDouble();
						num5++;
						flag = true;
					}
				}
				if (flag)
				{
					_ = (SqlSingle)0f;
					return (sqlDouble4 / num5).ToSqlSingle();
				}
				return _nullValue;
			}
			case AggregateType.Var:
			case AggregateType.StDev:
			{
				int num = 0;
				SqlDouble sqlDouble = 0.0;
				_ = (SqlDouble)0.0;
				SqlDouble sqlDouble2 = 0.0;
				SqlDouble sqlDouble3 = 0.0;
				int[] array = records;
				foreach (int num4 in array)
				{
					if (!IsNull(num4))
					{
						sqlDouble2 += _values[num4].ToSqlDouble();
						sqlDouble3 += _values[num4].ToSqlDouble() * _values[num4].ToSqlDouble();
						num++;
					}
				}
				if (num > 1)
				{
					sqlDouble = num * sqlDouble3 - sqlDouble2 * sqlDouble2;
					if (sqlDouble / (sqlDouble2 * sqlDouble2) < 1E-15 || sqlDouble < 0.0)
					{
						sqlDouble = 0.0;
					}
					else
					{
						sqlDouble /= (SqlDouble)(num * (num - 1));
					}
					if (kind == AggregateType.StDev)
					{
						return Math.Sqrt(sqlDouble.Value);
					}
					return sqlDouble;
				}
				return _nullValue;
			}
			case AggregateType.Min:
			{
				SqlSingle sqlSingle = SqlSingle.MaxValue;
				foreach (int num2 in records)
				{
					if (!IsNull(num2))
					{
						if (SqlSingle.LessThan(_values[num2], sqlSingle).IsTrue)
						{
							sqlSingle = _values[num2];
						}
						flag = true;
					}
				}
				if (flag)
				{
					return sqlSingle;
				}
				return _nullValue;
			}
			case AggregateType.Max:
			{
				SqlSingle sqlSingle3 = SqlSingle.MinValue;
				foreach (int num7 in records)
				{
					if (!IsNull(num7))
					{
						if (SqlSingle.GreaterThan(_values[num7], sqlSingle3).IsTrue)
						{
							sqlSingle3 = _values[num7];
						}
						flag = true;
					}
				}
				if (flag)
				{
					return sqlSingle3;
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
			throw ExprException.Overflow(typeof(SqlSingle));
		}
		throw ExceptionBuilder.AggregateException(kind, _dataType);
	}

	public override int Compare(int recordNo1, int recordNo2)
	{
		return _values[recordNo1].CompareTo(_values[recordNo2]);
	}

	public override int CompareValueTo(int recordNo, object value)
	{
		return _values[recordNo].CompareTo((SqlSingle)value);
	}

	public override object ConvertValue(object value)
	{
		if (value != null)
		{
			return SqlConvert.ConvertToSqlSingle(value);
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
		_values[record] = SqlConvert.ConvertToSqlSingle(value);
	}

	public override void SetCapacity(int capacity)
	{
		SqlSingle[] array = new SqlSingle[capacity];
		if (_values != null)
		{
			Array.Copy(_values, 0, array, 0, Math.Min(capacity, _values.Length));
		}
		_values = array;
	}

	public override object ConvertXmlToObject(string s)
	{
		SqlSingle sqlSingle = default(SqlSingle);
		StringReader input = new StringReader("<col>" + s + "</col>");
		IXmlSerializable xmlSerializable = sqlSingle;
		using (XmlTextReader reader = new XmlTextReader(input))
		{
			xmlSerializable.ReadXml(reader);
		}
		return (SqlSingle)(object)xmlSerializable;
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
		return new SqlSingle[recordCount];
	}

	protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
	{
		((SqlSingle[])store)[storeIndex] = _values[record];
		nullbits.Set(storeIndex, IsNull(record));
	}

	protected override void SetStorage(object store, BitArray nullbits)
	{
		_values = (SqlSingle[])store;
	}
}
