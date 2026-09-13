using System.Collections;
using System.Xml;

namespace System.Data.Common;

internal sealed class Int16Storage : DataStorage
{
	private const short defaultValue = 0;

	private short[] _values;

	internal Int16Storage(DataColumn column)
		: base(column, typeof(short), (short)0, StorageType.Int16)
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
				long num4 = 0L;
				int[] array = records;
				foreach (int num5 in array)
				{
					if (HasValue(num5))
					{
						num4 = checked(num4 + _values[num5]);
						flag = true;
					}
				}
				if (flag)
				{
					return num4;
				}
				return _nullValue;
			}
			case AggregateType.Mean:
			{
				long num10 = 0L;
				int num11 = 0;
				int[] array = records;
				foreach (int num12 in array)
				{
					if (HasValue(num12))
					{
						num10 = checked(num10 + _values[num12]);
						num11++;
						flag = true;
					}
				}
				checked
				{
					if (flag)
					{
						return (short)unchecked(num10 / num11);
					}
					return _nullValue;
				}
			}
			case AggregateType.Var:
			case AggregateType.StDev:
			{
				int num = 0;
				double num6 = 0.0;
				double num7 = 0.0;
				double num8 = 0.0;
				int[] array = records;
				foreach (int num9 in array)
				{
					if (HasValue(num9))
					{
						num7 += (double)_values[num9];
						num8 += (double)_values[num9] * (double)_values[num9];
						num++;
					}
				}
				if (num > 1)
				{
					num6 = (double)num * num8 - num7 * num7;
					num6 = ((!(num6 / (num7 * num7) < 1E-15) && !(num6 < 0.0)) ? (num6 / (double)(num * (num - 1))) : 0.0);
					if (kind == AggregateType.StDev)
					{
						return Math.Sqrt(num6);
					}
					return num6;
				}
				return _nullValue;
			}
			case AggregateType.Min:
			{
				short num2 = short.MaxValue;
				foreach (int num3 in records)
				{
					if (HasValue(num3))
					{
						num2 = Math.Min(_values[num3], num2);
						flag = true;
					}
				}
				if (flag)
				{
					return num2;
				}
				return _nullValue;
			}
			case AggregateType.Max:
			{
				short num13 = short.MinValue;
				foreach (int num14 in records)
				{
					if (HasValue(num14))
					{
						num13 = Math.Max(_values[num14], num13);
						flag = true;
					}
				}
				if (flag)
				{
					return num13;
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
					if (HasValue(records[i]))
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
			throw ExprException.Overflow(typeof(short));
		}
		throw ExceptionBuilder.AggregateException(kind, _dataType);
	}

	public override int Compare(int recordNo1, int recordNo2)
	{
		short num = _values[recordNo1];
		short num2 = _values[recordNo2];
		if (num == 0 || num2 == 0)
		{
			int num3 = CompareBits(recordNo1, recordNo2);
			if (num3 != 0)
			{
				return num3;
			}
		}
		return num - num2;
	}

	public override int CompareValueTo(int recordNo, object value)
	{
		if (_nullValue == value)
		{
			if (!HasValue(recordNo))
			{
				return 0;
			}
			return 1;
		}
		short num = _values[recordNo];
		if (num == 0 && !HasValue(recordNo))
		{
			return -1;
		}
		return num.CompareTo((short)value);
	}

	public override object ConvertValue(object value)
	{
		if (_nullValue != value)
		{
			value = ((value == null) ? _nullValue : ((object)((IConvertible)value).ToInt16(base.FormatProvider)));
		}
		return value;
	}

	public override void Copy(int recordNo1, int recordNo2)
	{
		CopyBits(recordNo1, recordNo2);
		_values[recordNo2] = _values[recordNo1];
	}

	public override object Get(int record)
	{
		short num = _values[record];
		if (num != 0)
		{
			return num;
		}
		return GetBits(record);
	}

	public override void Set(int record, object value)
	{
		if (_nullValue == value)
		{
			_values[record] = 0;
			SetNullBit(record, flag: true);
		}
		else
		{
			_values[record] = ((IConvertible)value).ToInt16(base.FormatProvider);
			SetNullBit(record, flag: false);
		}
	}

	public override void SetCapacity(int capacity)
	{
		short[] array = new short[capacity];
		if (_values != null)
		{
			Array.Copy(_values, 0, array, 0, Math.Min(capacity, _values.Length));
		}
		_values = array;
		base.SetCapacity(capacity);
	}

	public override object ConvertXmlToObject(string s)
	{
		return XmlConvert.ToInt16(s);
	}

	public override string ConvertObjectToXml(object value)
	{
		return XmlConvert.ToString((short)value);
	}

	protected override object GetEmptyStorage(int recordCount)
	{
		return new short[recordCount];
	}

	protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
	{
		((short[])store)[storeIndex] = _values[record];
		nullbits.Set(storeIndex, !HasValue(record));
	}

	protected override void SetStorage(object store, BitArray nullbits)
	{
		_values = (short[])store;
		SetNullStorage(nullbits);
	}
}
