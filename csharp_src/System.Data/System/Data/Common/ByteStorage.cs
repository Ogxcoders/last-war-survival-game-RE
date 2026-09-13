using System.Collections;
using System.Xml;

namespace System.Data.Common;

internal sealed class ByteStorage : DataStorage
{
	private const byte defaultValue = 0;

	private byte[] _values;

	internal ByteStorage(DataColumn column)
		: base(column, typeof(byte), (byte)0, StorageType.Byte)
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
				ulong num11 = 0uL;
				int[] array = records;
				foreach (int num12 in array)
				{
					if (!IsNull(num12))
					{
						num11 = checked(num11 + _values[num12]);
						flag = true;
					}
				}
				if (flag)
				{
					return num11;
				}
				return _nullValue;
			}
			case AggregateType.Mean:
			{
				long num6 = 0L;
				int num7 = 0;
				int[] array = records;
				foreach (int num8 in array)
				{
					if (!IsNull(num8))
					{
						num6 = checked(num6 + _values[num8]);
						num7++;
						flag = true;
					}
				}
				checked
				{
					if (flag)
					{
						return (byte)unchecked(num6 / num7);
					}
					return _nullValue;
				}
			}
			case AggregateType.Var:
			case AggregateType.StDev:
			{
				int num = 0;
				double num2 = 0.0;
				double num3 = 0.0;
				double num4 = 0.0;
				int[] array = records;
				foreach (int num5 in array)
				{
					if (!IsNull(num5))
					{
						num3 += (double)(int)_values[num5];
						num4 += (double)(int)_values[num5] * (double)(int)_values[num5];
						num++;
					}
				}
				if (num > 1)
				{
					num2 = (double)num * num4 - num3 * num3;
					num2 = ((!(num2 / (num3 * num3) < 1E-15) && !(num2 < 0.0)) ? (num2 / (double)(num * (num - 1))) : 0.0);
					if (kind == AggregateType.StDev)
					{
						return Math.Sqrt(num2);
					}
					return num2;
				}
				return _nullValue;
			}
			case AggregateType.Min:
			{
				byte b2 = byte.MaxValue;
				foreach (int num10 in records)
				{
					if (!IsNull(num10))
					{
						b2 = Math.Min(_values[num10], b2);
						flag = true;
					}
				}
				if (flag)
				{
					return b2;
				}
				return _nullValue;
			}
			case AggregateType.Max:
			{
				byte b = 0;
				foreach (int num9 in records)
				{
					if (!IsNull(num9))
					{
						b = Math.Max(_values[num9], b);
						flag = true;
					}
				}
				if (flag)
				{
					return b;
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
				return base.Aggregate(records, kind);
			}
		}
		catch (OverflowException)
		{
			throw ExprException.Overflow(typeof(byte));
		}
		throw ExceptionBuilder.AggregateException(kind, _dataType);
	}

	public override int Compare(int recordNo1, int recordNo2)
	{
		byte b = _values[recordNo1];
		byte b2 = _values[recordNo2];
		if (b == 0 || b2 == 0)
		{
			int num = CompareBits(recordNo1, recordNo2);
			if (num != 0)
			{
				return num;
			}
		}
		return b.CompareTo(b2);
	}

	public override int CompareValueTo(int recordNo, object value)
	{
		if (_nullValue == value)
		{
			if (IsNull(recordNo))
			{
				return 0;
			}
			return 1;
		}
		byte b = _values[recordNo];
		if (b == 0 && IsNull(recordNo))
		{
			return -1;
		}
		return b.CompareTo((byte)value);
	}

	public override object ConvertValue(object value)
	{
		if (_nullValue != value)
		{
			value = ((value == null) ? _nullValue : ((object)((IConvertible)value).ToByte(base.FormatProvider)));
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
		byte b = _values[record];
		if (b != 0)
		{
			return b;
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
			_values[record] = ((IConvertible)value).ToByte(base.FormatProvider);
			SetNullBit(record, flag: false);
		}
	}

	public override void SetCapacity(int capacity)
	{
		byte[] array = new byte[capacity];
		if (_values != null)
		{
			Array.Copy(_values, 0, array, 0, Math.Min(capacity, _values.Length));
		}
		_values = array;
		base.SetCapacity(capacity);
	}

	public override object ConvertXmlToObject(string s)
	{
		return XmlConvert.ToByte(s);
	}

	public override string ConvertObjectToXml(object value)
	{
		return XmlConvert.ToString((byte)value);
	}

	protected override object GetEmptyStorage(int recordCount)
	{
		return new byte[recordCount];
	}

	protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
	{
		((byte[])store)[storeIndex] = _values[record];
		nullbits.Set(storeIndex, IsNull(record));
	}

	protected override void SetStorage(object store, BitArray nullbits)
	{
		_values = (byte[])store;
		SetNullStorage(nullbits);
	}
}
