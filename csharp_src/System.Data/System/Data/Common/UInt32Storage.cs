using System.Collections;
using System.Xml;

namespace System.Data.Common;

internal sealed class UInt32Storage : DataStorage
{
	private static readonly uint s_defaultValue;

	private uint[] _values;

	public UInt32Storage(DataColumn column)
		: base(column, typeof(uint), s_defaultValue, StorageType.UInt32)
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
				ulong num4 = s_defaultValue;
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
				long num10 = s_defaultValue;
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
						return (uint)unchecked(num10 / num11);
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
				uint num2 = uint.MaxValue;
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
				uint num13 = 0u;
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
			throw ExprException.Overflow(typeof(uint));
		}
		throw ExceptionBuilder.AggregateException(kind, _dataType);
	}

	public override int Compare(int recordNo1, int recordNo2)
	{
		uint num = _values[recordNo1];
		uint num2 = _values[recordNo2];
		if (num == s_defaultValue || num2 == s_defaultValue)
		{
			int num3 = CompareBits(recordNo1, recordNo2);
			if (num3 != 0)
			{
				return num3;
			}
		}
		if (num >= num2)
		{
			if (num <= num2)
			{
				return 0;
			}
			return 1;
		}
		return -1;
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
		uint num = _values[recordNo];
		if (s_defaultValue == num && !HasValue(recordNo))
		{
			return -1;
		}
		return num.CompareTo((uint)value);
	}

	public override object ConvertValue(object value)
	{
		if (_nullValue != value)
		{
			value = ((value == null) ? _nullValue : ((object)((IConvertible)value).ToUInt32(base.FormatProvider)));
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
		uint num = _values[record];
		if (!num.Equals(s_defaultValue))
		{
			return num;
		}
		return GetBits(record);
	}

	public override void Set(int record, object value)
	{
		if (_nullValue == value)
		{
			_values[record] = s_defaultValue;
			SetNullBit(record, flag: true);
		}
		else
		{
			_values[record] = ((IConvertible)value).ToUInt32(base.FormatProvider);
			SetNullBit(record, flag: false);
		}
	}

	public override void SetCapacity(int capacity)
	{
		uint[] array = new uint[capacity];
		if (_values != null)
		{
			Array.Copy(_values, 0, array, 0, Math.Min(capacity, _values.Length));
		}
		_values = array;
		base.SetCapacity(capacity);
	}

	public override object ConvertXmlToObject(string s)
	{
		return XmlConvert.ToUInt32(s);
	}

	public override string ConvertObjectToXml(object value)
	{
		return XmlConvert.ToString((uint)value);
	}

	protected override object GetEmptyStorage(int recordCount)
	{
		return new uint[recordCount];
	}

	protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
	{
		((uint[])store)[storeIndex] = _values[record];
		nullbits.Set(storeIndex, !HasValue(record));
	}

	protected override void SetStorage(object store, BitArray nullbits)
	{
		_values = (uint[])store;
		SetNullStorage(nullbits);
	}
}
