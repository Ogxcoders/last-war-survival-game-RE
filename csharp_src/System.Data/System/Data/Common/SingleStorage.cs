using System.Collections;
using System.Xml;

namespace System.Data.Common;

internal sealed class SingleStorage : DataStorage
{
	private const float defaultValue = 0f;

	private float[] _values;

	public SingleStorage(DataColumn column)
		: base(column, typeof(float), 0f, StorageType.Single)
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
				float num13 = 0f;
				int[] array = records;
				foreach (int num14 in array)
				{
					if (!IsNull(num14))
					{
						num13 += _values[num14];
						flag = true;
					}
				}
				if (flag)
				{
					return num13;
				}
				return _nullValue;
			}
			case AggregateType.Mean:
			{
				double num6 = 0.0;
				int num7 = 0;
				int[] array = records;
				foreach (int num8 in array)
				{
					if (!IsNull(num8))
					{
						num6 += (double)_values[num8];
						num7++;
						flag = true;
					}
				}
				if (flag)
				{
					return (float)(num6 / (double)num7);
				}
				return _nullValue;
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
						num3 += (double)_values[num5];
						num4 += (double)_values[num5] * (double)_values[num5];
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
				float num11 = float.MaxValue;
				foreach (int num12 in records)
				{
					if (!IsNull(num12))
					{
						num11 = Math.Min(_values[num12], num11);
						flag = true;
					}
				}
				if (flag)
				{
					return num11;
				}
				return _nullValue;
			}
			case AggregateType.Max:
			{
				float num9 = float.MinValue;
				foreach (int num10 in records)
				{
					if (!IsNull(num10))
					{
						num9 = Math.Max(_values[num10], num9);
						flag = true;
					}
				}
				if (flag)
				{
					return num9;
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
			throw ExprException.Overflow(typeof(float));
		}
		throw ExceptionBuilder.AggregateException(kind, _dataType);
	}

	public override int Compare(int recordNo1, int recordNo2)
	{
		float num = _values[recordNo1];
		float num2 = _values[recordNo2];
		if (num == 0f || num2 == 0f)
		{
			int num3 = CompareBits(recordNo1, recordNo2);
			if (num3 != 0)
			{
				return num3;
			}
		}
		return num.CompareTo(num2);
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
		float num = _values[recordNo];
		if (0f == num && IsNull(recordNo))
		{
			return -1;
		}
		return num.CompareTo((float)value);
	}

	public override object ConvertValue(object value)
	{
		if (_nullValue != value)
		{
			value = ((value == null) ? _nullValue : ((object)((IConvertible)value).ToSingle(base.FormatProvider)));
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
		float num = _values[record];
		if (num != 0f)
		{
			return num;
		}
		return GetBits(record);
	}

	public override void Set(int record, object value)
	{
		if (_nullValue == value)
		{
			_values[record] = 0f;
			SetNullBit(record, flag: true);
		}
		else
		{
			_values[record] = ((IConvertible)value).ToSingle(base.FormatProvider);
			SetNullBit(record, flag: false);
		}
	}

	public override void SetCapacity(int capacity)
	{
		float[] array = new float[capacity];
		if (_values != null)
		{
			Array.Copy(_values, 0, array, 0, Math.Min(capacity, _values.Length));
		}
		_values = array;
		base.SetCapacity(capacity);
	}

	public override object ConvertXmlToObject(string s)
	{
		return XmlConvert.ToSingle(s);
	}

	public override string ConvertObjectToXml(object value)
	{
		return XmlConvert.ToString((float)value);
	}

	protected override object GetEmptyStorage(int recordCount)
	{
		return new float[recordCount];
	}

	protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
	{
		((float[])store)[storeIndex] = _values[record];
		nullbits.Set(storeIndex, IsNull(record));
	}

	protected override void SetStorage(object store, BitArray nullbits)
	{
		_values = (float[])store;
		SetNullStorage(nullbits);
	}
}
