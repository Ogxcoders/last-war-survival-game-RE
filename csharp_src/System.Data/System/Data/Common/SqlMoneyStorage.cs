using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common;

internal sealed class SqlMoneyStorage : DataStorage
{
	private SqlMoney[] _values;

	public SqlMoneyStorage(DataColumn column)
		: base(column, typeof(SqlMoney), SqlMoney.Null, SqlMoney.Null, StorageType.SqlMoney)
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
				SqlDecimal sqlDecimal = 0L;
				int[] array = records;
				foreach (int num3 in array)
				{
					if (!IsNull(num3))
					{
						sqlDecimal += (SqlDecimal)_values[num3];
						flag = true;
					}
				}
				if (flag)
				{
					return sqlDecimal;
				}
				return _nullValue;
			}
			case AggregateType.Mean:
			{
				SqlDecimal sqlDecimal2 = 0L;
				int num5 = 0;
				int[] array = records;
				foreach (int num6 in array)
				{
					if (!IsNull(num6))
					{
						sqlDecimal2 += _values[num6].ToSqlDecimal();
						num5++;
						flag = true;
					}
				}
				if (flag)
				{
					_ = (SqlMoney)0L;
					return (sqlDecimal2 / num5).ToSqlMoney();
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
				SqlMoney sqlMoney = SqlMoney.MaxValue;
				foreach (int num2 in records)
				{
					if (!IsNull(num2))
					{
						if (SqlMoney.LessThan(_values[num2], sqlMoney).IsTrue)
						{
							sqlMoney = _values[num2];
						}
						flag = true;
					}
				}
				if (flag)
				{
					return sqlMoney;
				}
				return _nullValue;
			}
			case AggregateType.Max:
			{
				SqlMoney sqlMoney2 = SqlMoney.MinValue;
				foreach (int num7 in records)
				{
					if (!IsNull(num7))
					{
						if (SqlMoney.GreaterThan(_values[num7], sqlMoney2).IsTrue)
						{
							sqlMoney2 = _values[num7];
						}
						flag = true;
					}
				}
				if (flag)
				{
					return sqlMoney2;
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
			throw ExprException.Overflow(typeof(SqlMoney));
		}
		throw ExceptionBuilder.AggregateException(kind, _dataType);
	}

	public override int Compare(int recordNo1, int recordNo2)
	{
		return _values[recordNo1].CompareTo(_values[recordNo2]);
	}

	public override int CompareValueTo(int recordNo, object value)
	{
		return _values[recordNo].CompareTo((SqlMoney)value);
	}

	public override object ConvertValue(object value)
	{
		if (value != null)
		{
			return SqlConvert.ConvertToSqlMoney(value);
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
		_values[record] = SqlConvert.ConvertToSqlMoney(value);
	}

	public override void SetCapacity(int capacity)
	{
		SqlMoney[] array = new SqlMoney[capacity];
		if (_values != null)
		{
			Array.Copy(_values, 0, array, 0, Math.Min(capacity, _values.Length));
		}
		_values = array;
	}

	public override object ConvertXmlToObject(string s)
	{
		SqlMoney sqlMoney = default(SqlMoney);
		StringReader input = new StringReader("<col>" + s + "</col>");
		IXmlSerializable xmlSerializable = sqlMoney;
		using (XmlTextReader reader = new XmlTextReader(input))
		{
			xmlSerializable.ReadXml(reader);
		}
		return (SqlMoney)(object)xmlSerializable;
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
		return new SqlMoney[recordCount];
	}

	protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
	{
		((SqlMoney[])store)[storeIndex] = _values[record];
		nullbits.Set(storeIndex, IsNull(record));
	}

	protected override void SetStorage(object store, BitArray nullbits)
	{
		_values = (SqlMoney[])store;
	}
}
