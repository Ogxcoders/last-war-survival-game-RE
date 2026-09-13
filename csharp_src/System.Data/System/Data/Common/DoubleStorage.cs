using System.Collections;
using System.Xml;

namespace System.Data.Common;

internal sealed class DoubleStorage : DataStorage
{
	private const double defaultValue = 0.0;

	private double[] _values;

	internal DoubleStorage(DataColumn column)
		: base(column, typeof(double), 0.0, StorageType.Double)
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
				double num13 = 0.0;
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
						num6 += _values[num8];
						num7++;
						flag = true;
					}
				}
				if (flag)
				{
					return num6 / (double)num7;
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
						num3 += _values[num5];
						num4 += _values[num5] * _values[num5];
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
				double num11 = double.MaxValue;
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
				double num9 = double.MinValue;
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
			throw ExprException.Overflow(typeof(double));
		}
		throw ExceptionBuilder.AggregateException(kind, _dataType);
	}

	public override int Compare(int recordNo1, int recordNo2)
	{
		double num = _values[recordNo1];
		double num2 = _values[recordNo2];
		if (num == 0.0 || num2 == 0.0)
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
		double num = _values[recordNo];
		if (0.0 == num && IsNull(recordNo))
		{
			return -1;
		}
		return num.CompareTo((double)value);
	}

	public override object ConvertValue(object value)
	{
		if (_nullValue != value)
		{
			value = ((value == null) ? _nullValue : ((object)((IConvertible)value).ToDouble(base.FormatProvider)));
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
		double num = _values[record];
		if (num != 0.0)
		{
			return num;
		}
		return GetBits(record);
	}

	public override void Set(int record, object value)
	{
		if (_nullValue == value)
		{
			_values[record] = 0.0;
			SetNullBit(record, flag: true);
		}
		else
		{
			_values[record] = ((IConvertible)value).ToDouble(base.FormatProvider);
			SetNullBit(record, flag: false);
		}
	}

	public override void SetCapacity(int capacity)
	{
		double[] array = new double[capacity];
		if (_values != null)
		{
			Array.Copy(_values, 0, array, 0, Math.Min(capacity, _values.Length));
		}
		_values = array;
		base.SetCapacity(capacity);
	}

	public override object ConvertXmlToObject(string s)
	{
		return XmlConvert.ToDouble(s);
	}

	public override string ConvertObjectToXml(object value)
	{
		return XmlConvert.ToString((double)value);
	}

	protected override object GetEmptyStorage(int recordCount)
	{
		return new double[recordCount];
	}

	protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
	{
		((double[])store)[storeIndex] = _values[record];
		nullbits.Set(storeIndex, IsNull(record));
	}

	protected override void SetStorage(object store, BitArray nullbits)
	{
		_values = (double[])store;
		SetNullStorage(nullbits);
	}
}
