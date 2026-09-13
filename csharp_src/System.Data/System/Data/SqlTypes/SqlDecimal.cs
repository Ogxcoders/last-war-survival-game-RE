using System.Data.Common;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[Serializable]
[XmlSchemaProvider("GetXsdType")]
public struct SqlDecimal : INullable, IComparable, IXmlSerializable
{
	internal byte _bStatus;

	internal byte _bLen;

	internal byte _bPrec;

	internal byte _bScale;

	internal uint _data1;

	internal uint _data2;

	internal uint _data3;

	internal uint _data4;

	private static readonly byte s_NUMERIC_MAX_PRECISION = 38;

	public static readonly byte MaxPrecision = s_NUMERIC_MAX_PRECISION;

	public static readonly byte MaxScale = s_NUMERIC_MAX_PRECISION;

	private static readonly byte s_bNullMask = 1;

	private static readonly byte s_bIsNull = 0;

	private static readonly byte s_bNotNull = 1;

	private static readonly byte s_bReverseNullMask = (byte)(~s_bNullMask);

	private static readonly byte s_bSignMask = 2;

	private static readonly byte s_bPositive = 0;

	private static readonly byte s_bNegative = 2;

	private static readonly byte s_bReverseSignMask = (byte)(~s_bSignMask);

	private static readonly uint s_uiZero = 0u;

	private static readonly int s_cNumeMax = 4;

	private static readonly long s_lInt32Base = 4294967296L;

	private static readonly ulong s_ulInt32Base = 4294967296uL;

	private static readonly ulong s_ulInt32BaseForMod = s_ulInt32Base - 1;

	internal static readonly ulong s_llMax = 9223372036854775807uL;

	private static readonly uint s_ulBase10 = 10u;

	private static readonly double s_DUINT_BASE = s_lInt32Base;

	private static readonly double s_DUINT_BASE2 = s_DUINT_BASE * s_DUINT_BASE;

	private static readonly double s_DUINT_BASE3 = s_DUINT_BASE2 * s_DUINT_BASE;

	private static readonly double s_DMAX_NUME = 1E+38;

	private static readonly uint s_DBL_DIG = 17u;

	private static readonly byte s_cNumeDivScaleMin = 6;

	private static readonly uint[] s_rgulShiftBase = new uint[9] { 10u, 100u, 1000u, 10000u, 100000u, 1000000u, 10000000u, 100000000u, 1000000000u };

	private static readonly uint[] s_decimalHelpersLo = new uint[38]
	{
		10u, 100u, 1000u, 10000u, 100000u, 1000000u, 10000000u, 100000000u, 1000000000u, 1410065408u,
		1215752192u, 3567587328u, 1316134912u, 276447232u, 2764472320u, 1874919424u, 1569325056u, 2808348672u, 2313682944u, 1661992960u,
		3735027712u, 2990538752u, 4135583744u, 2701131776u, 1241513984u, 3825205248u, 3892314112u, 268435456u, 2684354560u, 1073741824u,
		2147483648u, 0u, 0u, 0u, 0u, 0u, 0u, 0u
	};

	private static readonly uint[] s_decimalHelpersMid = new uint[38]
	{
		0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 2u,
		23u, 232u, 2328u, 23283u, 232830u, 2328306u, 23283064u, 232830643u, 2328306436u, 1808227885u,
		902409669u, 434162106u, 46653770u, 466537709u, 370409800u, 3704098002u, 2681241660u, 1042612833u, 1836193738u, 1182068202u,
		3230747430u, 2242703233u, 952195850u, 932023908u, 730304488u, 3008077584u, 16004768u, 160047680u
	};

	private static readonly uint[] s_decimalHelpersHi = new uint[38]
	{
		0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u,
		0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 5u,
		54u, 542u, 5421u, 54210u, 542101u, 5421010u, 54210108u, 542101086u, 1126043566u, 2670501072u,
		935206946u, 762134875u, 3326381459u, 3199043520u, 1925664130u, 2076772117u, 3587851993u, 1518781562u
	};

	private static readonly uint[] s_decimalHelpersHiHi = new uint[38]
	{
		0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u,
		0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u,
		0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 1u, 12u,
		126u, 1262u, 12621u, 126217u, 1262177u, 12621774u, 126217744u, 1262177448u
	};

	private const int HelperTableStartIndexLo = 5;

	private const int HelperTableStartIndexMid = 15;

	private const int HelperTableStartIndexHi = 24;

	private const int HelperTableStartIndexHiHi = 33;

	private static readonly byte[] s_rgCLenFromPrec = new byte[38]
	{
		1, 1, 1, 1, 1, 1, 1, 1, 1, 2,
		2, 2, 2, 2, 2, 2, 2, 2, 2, 3,
		3, 3, 3, 3, 3, 3, 3, 3, 4, 4,
		4, 4, 4, 4, 4, 4, 4, 4
	};

	private static readonly uint s_ulT1 = 10u;

	private static readonly uint s_ulT2 = 100u;

	private static readonly uint s_ulT3 = 1000u;

	private static readonly uint s_ulT4 = 10000u;

	private static readonly uint s_ulT5 = 100000u;

	private static readonly uint s_ulT6 = 1000000u;

	private static readonly uint s_ulT7 = 10000000u;

	private static readonly uint s_ulT8 = 100000000u;

	private static readonly uint s_ulT9 = 1000000000u;

	private static readonly ulong s_dwlT10 = 10000000000uL;

	private static readonly ulong s_dwlT11 = 100000000000uL;

	private static readonly ulong s_dwlT12 = 1000000000000uL;

	private static readonly ulong s_dwlT13 = 10000000000000uL;

	private static readonly ulong s_dwlT14 = 100000000000000uL;

	private static readonly ulong s_dwlT15 = 1000000000000000uL;

	private static readonly ulong s_dwlT16 = 10000000000000000uL;

	private static readonly ulong s_dwlT17 = 100000000000000000uL;

	private static readonly ulong s_dwlT18 = 1000000000000000000uL;

	private static readonly ulong s_dwlT19 = 10000000000000000000uL;

	public static readonly SqlDecimal Null = new SqlDecimal(fNull: true);

	public static readonly SqlDecimal MinValue = Parse("-99999999999999999999999999999999999999");

	public static readonly SqlDecimal MaxValue = Parse("99999999999999999999999999999999999999");

	public bool IsNull => (_bStatus & s_bNullMask) == s_bIsNull;

	public decimal Value => ToDecimal();

	public bool IsPositive
	{
		get
		{
			if (IsNull)
			{
				throw new SqlNullValueException();
			}
			return (_bStatus & s_bSignMask) == s_bPositive;
		}
	}

	public byte Precision
	{
		get
		{
			if (IsNull)
			{
				throw new SqlNullValueException();
			}
			return _bPrec;
		}
	}

	public byte Scale
	{
		get
		{
			if (IsNull)
			{
				throw new SqlNullValueException();
			}
			return _bScale;
		}
	}

	public int[] Data
	{
		get
		{
			if (IsNull)
			{
				throw new SqlNullValueException();
			}
			return new int[4]
			{
				(int)_data1,
				(int)_data2,
				(int)_data3,
				(int)_data4
			};
		}
	}

	public byte[] BinData
	{
		get
		{
			if (IsNull)
			{
				throw new SqlNullValueException();
			}
			int data = (int)_data1;
			int data2 = (int)_data2;
			int data3 = (int)_data3;
			int data4 = (int)_data4;
			byte[] obj = new byte[16]
			{
				(byte)(data & 0xFF),
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			data >>= 8;
			obj[1] = (byte)(data & 0xFF);
			data >>= 8;
			obj[2] = (byte)(data & 0xFF);
			data >>= 8;
			obj[3] = (byte)(data & 0xFF);
			obj[4] = (byte)(data2 & 0xFF);
			data2 >>= 8;
			obj[5] = (byte)(data2 & 0xFF);
			data2 >>= 8;
			obj[6] = (byte)(data2 & 0xFF);
			data2 >>= 8;
			obj[7] = (byte)(data2 & 0xFF);
			obj[8] = (byte)(data3 & 0xFF);
			data3 >>= 8;
			obj[9] = (byte)(data3 & 0xFF);
			data3 >>= 8;
			obj[10] = (byte)(data3 & 0xFF);
			data3 >>= 8;
			obj[11] = (byte)(data3 & 0xFF);
			obj[12] = (byte)(data4 & 0xFF);
			data4 >>= 8;
			obj[13] = (byte)(data4 & 0xFF);
			data4 >>= 8;
			obj[14] = (byte)(data4 & 0xFF);
			data4 >>= 8;
			obj[15] = (byte)(data4 & 0xFF);
			return obj;
		}
	}

	private byte CalculatePrecision()
	{
		uint[] array;
		uint num2;
		int num;
		if (_data4 != 0)
		{
			num = 33;
			array = s_decimalHelpersHiHi;
			num2 = _data4;
		}
		else if (_data3 != 0)
		{
			num = 24;
			array = s_decimalHelpersHi;
			num2 = _data3;
		}
		else if (_data2 != 0)
		{
			num = 15;
			array = s_decimalHelpersMid;
			num2 = _data2;
		}
		else
		{
			num = 5;
			array = s_decimalHelpersLo;
			num2 = _data1;
		}
		if (num2 < array[num])
		{
			num -= 2;
			if (num2 < array[num])
			{
				num -= 2;
				num = ((num2 >= array[num]) ? (num + 1) : (num - 1));
			}
			else
			{
				num++;
			}
		}
		else
		{
			num += 2;
			num = ((num2 >= array[num]) ? (num + 1) : (num - 1));
		}
		if (num2 >= array[num])
		{
			num++;
			if (num == 37 && num2 >= array[num])
			{
				num++;
			}
		}
		byte b = (byte)(num + 1);
		if (b > 1 && VerifyPrecision((byte)(b - 1)))
		{
			b--;
		}
		return Math.Max(b, _bScale);
	}

	private bool VerifyPrecision(byte precision)
	{
		int num = checked(precision - 1);
		if (_data4 < s_decimalHelpersHiHi[num])
		{
			return true;
		}
		if (_data4 == s_decimalHelpersHiHi[num])
		{
			if (_data3 < s_decimalHelpersHi[num])
			{
				return true;
			}
			if (_data3 == s_decimalHelpersHi[num])
			{
				if (_data2 < s_decimalHelpersMid[num])
				{
					return true;
				}
				if (_data2 == s_decimalHelpersMid[num] && _data1 < s_decimalHelpersLo[num])
				{
					return true;
				}
			}
		}
		return false;
	}

	private SqlDecimal(bool fNull)
	{
		_bLen = (_bPrec = (_bScale = 0));
		_bStatus = 0;
		_data1 = (_data2 = (_data3 = (_data4 = s_uiZero)));
	}

	public SqlDecimal(decimal value)
	{
		_bStatus = s_bNotNull;
		int[] bits = decimal.GetBits(value);
		uint num = (uint)bits[3];
		_data1 = (uint)bits[0];
		_data2 = (uint)bits[1];
		_data3 = (uint)bits[2];
		_data4 = s_uiZero;
		_bStatus |= (byte)(((num & 0x80000000u) == 2147483648u) ? s_bNegative : 0);
		if (_data3 != 0)
		{
			_bLen = 3;
		}
		else if (_data2 != 0)
		{
			_bLen = 2;
		}
		else
		{
			_bLen = 1;
		}
		_bScale = (byte)((int)(num & 0xFF0000) >> 16);
		_bPrec = 0;
		_bPrec = CalculatePrecision();
	}

	public SqlDecimal(int value)
	{
		_bStatus = s_bNotNull;
		uint data = (uint)value;
		if (value < 0)
		{
			_bStatus |= s_bNegative;
			if (value != int.MinValue)
			{
				data = (uint)(-value);
			}
		}
		_data1 = data;
		_data2 = (_data3 = (_data4 = s_uiZero));
		_bLen = 1;
		_bPrec = BGetPrecUI4(_data1);
		_bScale = 0;
	}

	public SqlDecimal(long value)
	{
		_bStatus = s_bNotNull;
		ulong num = (ulong)value;
		if (value < 0)
		{
			_bStatus |= s_bNegative;
			if (value != long.MinValue)
			{
				num = (ulong)(-value);
			}
		}
		_data1 = (uint)num;
		_data2 = (uint)(num >> 32);
		_data3 = (_data4 = 0u);
		_bLen = (byte)((_data2 == 0) ? 1u : 2u);
		_bPrec = BGetPrecUI8(num);
		_bScale = 0;
	}

	public SqlDecimal(byte bPrecision, byte bScale, bool fPositive, int[] bits)
	{
		CheckValidPrecScale(bPrecision, bScale);
		if (bits == null)
		{
			throw new ArgumentNullException("bits");
		}
		if (bits.Length != 4)
		{
			throw new ArgumentException(SQLResource.InvalidArraySizeMessage, "bits");
		}
		_bPrec = bPrecision;
		_bScale = bScale;
		_data1 = (uint)bits[0];
		_data2 = (uint)bits[1];
		_data3 = (uint)bits[2];
		_data4 = (uint)bits[3];
		_bLen = 1;
		for (int num = 3; num >= 0; num--)
		{
			if (bits[num] != 0)
			{
				_bLen = (byte)(num + 1);
				break;
			}
		}
		_bStatus = s_bNotNull;
		if (!fPositive)
		{
			_bStatus |= s_bNegative;
		}
		if (FZero())
		{
			SetPositive();
		}
		if (bPrecision < CalculatePrecision())
		{
			throw new OverflowException(SQLResource.ArithOverflowMessage);
		}
	}

	public SqlDecimal(byte bPrecision, byte bScale, bool fPositive, int data1, int data2, int data3, int data4)
	{
		CheckValidPrecScale(bPrecision, bScale);
		_bPrec = bPrecision;
		_bScale = bScale;
		_data1 = (uint)data1;
		_data2 = (uint)data2;
		_data3 = (uint)data3;
		_data4 = (uint)data4;
		_bLen = 1;
		if (data4 == 0)
		{
			if (data3 == 0)
			{
				if (data2 == 0)
				{
					_bLen = 1;
				}
				else
				{
					_bLen = 2;
				}
			}
			else
			{
				_bLen = 3;
			}
		}
		else
		{
			_bLen = 4;
		}
		_bStatus = s_bNotNull;
		if (!fPositive)
		{
			_bStatus |= s_bNegative;
		}
		if (FZero())
		{
			SetPositive();
		}
		if (bPrecision < CalculatePrecision())
		{
			throw new OverflowException(SQLResource.ArithOverflowMessage);
		}
	}

	public SqlDecimal(double dVal)
		: this(fNull: false)
	{
		_bStatus = s_bNotNull;
		if (dVal < 0.0)
		{
			dVal = 0.0 - dVal;
			_bStatus |= s_bNegative;
		}
		if (dVal >= s_DMAX_NUME)
		{
			throw new OverflowException(SQLResource.ArithOverflowMessage);
		}
		double num = Math.Floor(dVal);
		double num2 = dVal - num;
		_bPrec = s_NUMERIC_MAX_PRECISION;
		_bLen = 1;
		if (num > 0.0)
		{
			dVal = Math.Floor(num / s_DUINT_BASE);
			_data1 = (uint)(num - dVal * s_DUINT_BASE);
			num = dVal;
			if (num > 0.0)
			{
				dVal = Math.Floor(num / s_DUINT_BASE);
				_data2 = (uint)(num - dVal * s_DUINT_BASE);
				num = dVal;
				_bLen++;
				if (num > 0.0)
				{
					dVal = Math.Floor(num / s_DUINT_BASE);
					_data3 = (uint)(num - dVal * s_DUINT_BASE);
					num = dVal;
					_bLen++;
					if (num > 0.0)
					{
						dVal = Math.Floor(num / s_DUINT_BASE);
						_data4 = (uint)(num - dVal * s_DUINT_BASE);
						num = dVal;
						_bLen++;
					}
				}
			}
		}
		uint num3 = (uint)((!FZero()) ? CalculatePrecision() : 0);
		if (num3 > s_DBL_DIG)
		{
			uint num4 = num3 - s_DBL_DIG;
			uint num5;
			do
			{
				num5 = DivByULong(10u);
				num4--;
			}
			while (num4 != 0);
			num4 = num3 - s_DBL_DIG;
			if (num5 >= 5)
			{
				AddULong(1u);
				num3 = CalculatePrecision() + num4;
			}
			do
			{
				MultByULong(10u);
				num4--;
			}
			while (num4 != 0);
		}
		_bScale = (byte)((num3 < s_DBL_DIG) ? (s_DBL_DIG - num3) : 0u);
		_bPrec = (byte)(num3 + _bScale);
		if (_bScale > 0)
		{
			num3 = _bScale;
			do
			{
				uint num6 = ((num3 >= 9) ? 9u : num3);
				num2 *= (double)s_rgulShiftBase[num6 - 1];
				num3 -= num6;
				MultByULong(s_rgulShiftBase[num6 - 1]);
				AddULong((uint)num2);
				num2 -= Math.Floor(num2);
			}
			while (num3 != 0);
		}
		if (num2 >= 0.5)
		{
			AddULong(1u);
		}
		if (FZero())
		{
			SetPositive();
		}
	}

	private SqlDecimal(uint[] rglData, byte bLen, byte bPrec, byte bScale, bool fPositive)
	{
		CheckValidPrecScale(bPrec, bScale);
		_bLen = bLen;
		_bPrec = bPrec;
		_bScale = bScale;
		_data1 = rglData[0];
		_data2 = rglData[1];
		_data3 = rglData[2];
		_data4 = rglData[3];
		_bStatus = s_bNotNull;
		if (!fPositive)
		{
			_bStatus |= s_bNegative;
		}
		if (FZero())
		{
			SetPositive();
		}
	}

	private void SetPositive()
	{
		_bStatus &= s_bReverseSignMask;
	}

	private void SetSignBit(bool fPositive)
	{
		_bStatus = (byte)(fPositive ? (_bStatus & s_bReverseSignMask) : (_bStatus | s_bNegative));
	}

	public override string ToString()
	{
		if (IsNull)
		{
			return SQLResource.NullString;
		}
		uint[] array = new uint[4] { _data1, _data2, _data3, _data4 };
		int ciulU = _bLen;
		char[] array2 = new char[s_NUMERIC_MAX_PRECISION + 1];
		int num = 0;
		while (ciulU > 1 || array[0] != 0)
		{
			MpDiv1(array, ref ciulU, s_ulBase10, out var iulR);
			array2[num++] = ChFromDigit(iulR);
		}
		while (num <= _bScale)
		{
			array2[num++] = ChFromDigit(0u);
		}
		int num2 = 0;
		int num3 = 0;
		if (_bScale > 0)
		{
			num2 = 1;
		}
		char[] array3;
		if (IsPositive)
		{
			array3 = new char[num2 + num];
		}
		else
		{
			array3 = new char[num2 + num + 1];
			array3[num3++] = '-';
		}
		while (num > 0)
		{
			if (num-- == _bScale)
			{
				array3[num3++] = '.';
			}
			array3[num3++] = array2[num];
		}
		return new string(array3);
	}

	public static SqlDecimal Parse(string s)
	{
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		if (s == SQLResource.NullString)
		{
			return Null;
		}
		SqlDecimal result = Null;
		char[] array = s.ToCharArray();
		int num = array.Length;
		int num2 = -1;
		int num3 = 0;
		result._bPrec = 1;
		result._bScale = 0;
		result.SetToZero();
		while (num != 0 && array[num - 1] == ' ')
		{
			num--;
		}
		if (num == 0)
		{
			throw new FormatException(SQLResource.FormatMessage);
		}
		while (array[num3] == ' ')
		{
			num3++;
			num--;
		}
		if (array[num3] == '-')
		{
			result.SetSignBit(fPositive: false);
			num3++;
			num--;
		}
		else
		{
			result.SetSignBit(fPositive: true);
			if (array[num3] == '+')
			{
				num3++;
				num--;
			}
		}
		while (num > 2 && array[num3] == '0')
		{
			num3++;
			num--;
		}
		if (2 == num && '0' == array[num3] && '.' == array[num3 + 1])
		{
			array[num3] = '.';
			array[num3 + 1] = '0';
		}
		if (num == 0 || num > s_NUMERIC_MAX_PRECISION + 1)
		{
			throw new FormatException(SQLResource.FormatMessage);
		}
		while (num > 1 && array[num3] == '0')
		{
			num3++;
			num--;
		}
		int i;
		for (i = 0; i < num; i++)
		{
			char c = array[num3];
			num3++;
			if (c >= '0' && c <= '9')
			{
				c = (char)(c - 48);
				result.MultByULong(s_ulBase10);
				result.AddULong(c);
				continue;
			}
			if (c == '.' && num2 < 0)
			{
				num2 = i;
				continue;
			}
			throw new FormatException(SQLResource.FormatMessage);
		}
		if (num2 < 0)
		{
			result._bPrec = (byte)i;
			result._bScale = 0;
		}
		else
		{
			result._bPrec = (byte)(i - 1);
			result._bScale = (byte)(result._bPrec - num2);
		}
		if (result._bPrec > s_NUMERIC_MAX_PRECISION)
		{
			throw new FormatException(SQLResource.FormatMessage);
		}
		if (result._bPrec == 0)
		{
			throw new FormatException(SQLResource.FormatMessage);
		}
		if (result.FZero())
		{
			result.SetPositive();
		}
		return result;
	}

	public double ToDouble()
	{
		if (IsNull)
		{
			throw new SqlNullValueException();
		}
		double num = 0.0;
		num = _data4;
		num = num * (double)s_lInt32Base + (double)_data3;
		num = num * (double)s_lInt32Base + (double)_data2;
		num = num * (double)s_lInt32Base + (double)_data1;
		num /= Math.Pow(10.0, (int)_bScale);
		if (!IsPositive)
		{
			return 0.0 - num;
		}
		return num;
	}

	private decimal ToDecimal()
	{
		if (IsNull)
		{
			throw new SqlNullValueException();
		}
		if (_data4 != 0 || _bScale > 28)
		{
			throw new OverflowException(SQLResource.ConversionOverflowMessage);
		}
		return new decimal((int)_data1, (int)_data2, (int)_data3, !IsPositive, _bScale);
	}

	public static implicit operator SqlDecimal(decimal x)
	{
		return new SqlDecimal(x);
	}

	public static explicit operator SqlDecimal(double x)
	{
		return new SqlDecimal(x);
	}

	public static implicit operator SqlDecimal(long x)
	{
		return new SqlDecimal(new decimal(x));
	}

	public static explicit operator decimal(SqlDecimal x)
	{
		return x.Value;
	}

	public static SqlDecimal operator -(SqlDecimal x)
	{
		if (x.IsNull)
		{
			return Null;
		}
		SqlDecimal result = x;
		if (result.FZero())
		{
			result.SetPositive();
		}
		else
		{
			result.SetSignBit(!result.IsPositive);
		}
		return result;
	}

	public static SqlDecimal operator +(SqlDecimal x, SqlDecimal y)
	{
		if (x.IsNull || y.IsNull)
		{
			return Null;
		}
		bool flag = true;
		bool isPositive = x.IsPositive;
		bool flag2 = y.IsPositive;
		int bScale = x._bScale;
		int bScale2 = y._bScale;
		int num = Math.Max(x._bPrec - bScale, y._bPrec - bScale2);
		int num2 = Math.Max(bScale, bScale2);
		int val = num + num2 + 1;
		val = Math.Min(MaxPrecision, val);
		if (val - num < num2)
		{
			num2 = val - num;
		}
		if (bScale != num2)
		{
			x.AdjustScale(num2 - bScale, fRound: true);
		}
		if (bScale2 != num2)
		{
			y.AdjustScale(num2 - bScale2, fRound: true);
		}
		if (!isPositive)
		{
			isPositive = !isPositive;
			flag2 = !flag2;
			flag = !flag;
		}
		int num3 = x._bLen;
		int bLen = y._bLen;
		uint[] array = new uint[4] { x._data1, x._data2, x._data3, x._data4 };
		uint[] array2 = new uint[4] { y._data1, y._data2, y._data3, y._data4 };
		byte bLen2;
		if (flag2)
		{
			ulong num4 = 0uL;
			int i;
			for (i = 0; i < num3 || i < bLen; i++)
			{
				if (i < num3)
				{
					num4 += array[i];
				}
				if (i < bLen)
				{
					num4 += array2[i];
				}
				array[i] = (uint)num4;
				num4 >>= 32;
			}
			if (num4 != 0L)
			{
				if (i == s_cNumeMax)
				{
					throw new OverflowException(SQLResource.ArithOverflowMessage);
				}
				array[i] = (uint)num4;
				i++;
			}
			bLen2 = (byte)i;
		}
		else
		{
			int num5 = 0;
			if (x.LAbsCmp(y) < 0)
			{
				flag = !flag;
				uint[] array3 = array2;
				array2 = array;
				array = array3;
				num3 = bLen;
				bLen = x._bLen;
			}
			ulong num4 = s_ulInt32Base;
			for (int i = 0; i < num3 || i < bLen; i++)
			{
				if (i < num3)
				{
					num4 += array[i];
				}
				if (i < bLen)
				{
					num4 -= array2[i];
				}
				array[i] = (uint)num4;
				if (array[i] != 0)
				{
					num5 = i;
				}
				num4 >>= 32;
				num4 += s_ulInt32BaseForMod;
			}
			bLen2 = (byte)(num5 + 1);
		}
		SqlDecimal result = new SqlDecimal(array, bLen2, (byte)val, (byte)num2, flag);
		if (result.FGt10_38() || result.CalculatePrecision() > s_NUMERIC_MAX_PRECISION)
		{
			throw new OverflowException(SQLResource.ArithOverflowMessage);
		}
		if (result.FZero())
		{
			result.SetPositive();
		}
		return result;
	}

	public static SqlDecimal operator -(SqlDecimal x, SqlDecimal y)
	{
		return x + -y;
	}

	public static SqlDecimal operator *(SqlDecimal x, SqlDecimal y)
	{
		if (x.IsNull || y.IsNull)
		{
			return Null;
		}
		int bLen = y._bLen;
		int num = x._bScale + y._bScale;
		int num2 = num;
		int num3 = x._bPrec - x._bScale + (y._bPrec - y._bScale) + 1;
		int num4 = num2 + num3;
		if (num4 > s_NUMERIC_MAX_PRECISION)
		{
			num4 = s_NUMERIC_MAX_PRECISION;
		}
		if (num2 > s_NUMERIC_MAX_PRECISION)
		{
			num2 = s_NUMERIC_MAX_PRECISION;
		}
		num2 = Math.Min(num4 - num3, num2);
		num2 = Math.Max(num2, Math.Min(num, s_cNumeDivScaleMin));
		int num5 = num2 - num;
		bool fPositive = x.IsPositive == y.IsPositive;
		uint[] array = new uint[4] { x._data1, x._data2, x._data3, x._data4 };
		uint[] array2 = new uint[4] { y._data1, y._data2, y._data3, y._data4 };
		uint[] array3 = new uint[9];
		int num6 = 0;
		for (int i = 0; i < x._bLen; i++)
		{
			uint num7 = array[i];
			ulong num8 = 0uL;
			num6 = i;
			for (int j = 0; j < bLen; j++)
			{
				ulong num9 = num8 + array3[num6];
				ulong num10 = array2[j];
				num8 = num7 * num10;
				num8 += num9;
				num9 = ((num8 >= num9) ? 0 : s_ulInt32Base);
				array3[num6++] = (uint)num8;
				num8 = (num8 >> 32) + num9;
			}
			if (num8 != 0L)
			{
				array3[num6++] = (uint)num8;
			}
		}
		while (array3[num6] == 0 && num6 > 0)
		{
			num6--;
		}
		int ciulU = num6 + 1;
		SqlDecimal result;
		if (num5 != 0)
		{
			if (num5 < 0)
			{
				uint num11;
				uint iulR;
				do
				{
					if (num5 <= -9)
					{
						num11 = s_rgulShiftBase[8];
						num5 += 9;
					}
					else
					{
						num11 = s_rgulShiftBase[-num5 - 1];
						num5 = 0;
					}
					MpDiv1(array3, ref ciulU, num11, out iulR);
				}
				while (num5 != 0);
				if (ciulU > s_cNumeMax)
				{
					throw new OverflowException(SQLResource.ArithOverflowMessage);
				}
				for (num6 = ciulU; num6 < s_cNumeMax; num6++)
				{
					array3[num6] = 0u;
				}
				result = new SqlDecimal(array3, (byte)ciulU, (byte)num4, (byte)num2, fPositive);
				if (result.FGt10_38())
				{
					throw new OverflowException(SQLResource.ArithOverflowMessage);
				}
				if (iulR >= num11 / 2)
				{
					result.AddULong(1u);
				}
				if (result.FZero())
				{
					result.SetPositive();
				}
				return result;
			}
			if (ciulU > s_cNumeMax)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			for (num6 = ciulU; num6 < s_cNumeMax; num6++)
			{
				array3[num6] = 0u;
			}
			result = new SqlDecimal(array3, (byte)ciulU, (byte)num4, (byte)num, fPositive);
			if (result.FZero())
			{
				result.SetPositive();
			}
			result.AdjustScale(num5, fRound: true);
			return result;
		}
		if (ciulU > s_cNumeMax)
		{
			throw new OverflowException(SQLResource.ArithOverflowMessage);
		}
		for (num6 = ciulU; num6 < s_cNumeMax; num6++)
		{
			array3[num6] = 0u;
		}
		result = new SqlDecimal(array3, (byte)ciulU, (byte)num4, (byte)num2, fPositive);
		if (result.FGt10_38())
		{
			throw new OverflowException(SQLResource.ArithOverflowMessage);
		}
		if (result.FZero())
		{
			result.SetPositive();
		}
		return result;
	}

	public static SqlDecimal operator /(SqlDecimal x, SqlDecimal y)
	{
		if (x.IsNull || y.IsNull)
		{
			return Null;
		}
		if (y.FZero())
		{
			throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
		}
		bool fPositive = x.IsPositive == y.IsPositive;
		int num = Math.Max(x._bScale + y._bPrec + 1, s_cNumeDivScaleMin);
		int val = x._bPrec - x._bScale + y._bScale;
		int num2 = num + x._bPrec + y._bPrec + 1;
		int val2 = Math.Min(num, s_cNumeDivScaleMin);
		val = Math.Min(val, s_NUMERIC_MAX_PRECISION);
		num2 = val + num;
		if (num2 > s_NUMERIC_MAX_PRECISION)
		{
			num2 = s_NUMERIC_MAX_PRECISION;
		}
		num = Math.Min(num2 - val, num);
		num = Math.Max(num, val2);
		int digits = num - x._bScale + y._bScale;
		x.AdjustScale(digits, fRound: true);
		uint[] rgulU = new uint[4] { x._data1, x._data2, x._data3, x._data4 };
		uint[] rgulD = new uint[4] { y._data1, y._data2, y._data3, y._data4 };
		uint[] rgulR = new uint[s_cNumeMax + 1];
		uint[] array = new uint[s_cNumeMax];
		MpDiv(rgulU, x._bLen, rgulD, y._bLen, array, out var ciulQ, rgulR, out var _);
		ZeroToMaxLen(array, ciulQ);
		SqlDecimal result = new SqlDecimal(array, (byte)ciulQ, (byte)num2, (byte)num, fPositive);
		if (result.FZero())
		{
			result.SetPositive();
		}
		return result;
	}

	public static explicit operator SqlDecimal(SqlBoolean x)
	{
		if (!x.IsNull)
		{
			return new SqlDecimal(x.ByteValue);
		}
		return Null;
	}

	public static implicit operator SqlDecimal(SqlByte x)
	{
		if (!x.IsNull)
		{
			return new SqlDecimal(x.Value);
		}
		return Null;
	}

	public static implicit operator SqlDecimal(SqlInt16 x)
	{
		if (!x.IsNull)
		{
			return new SqlDecimal(x.Value);
		}
		return Null;
	}

	public static implicit operator SqlDecimal(SqlInt32 x)
	{
		if (!x.IsNull)
		{
			return new SqlDecimal(x.Value);
		}
		return Null;
	}

	public static implicit operator SqlDecimal(SqlInt64 x)
	{
		if (!x.IsNull)
		{
			return new SqlDecimal(x.Value);
		}
		return Null;
	}

	public static implicit operator SqlDecimal(SqlMoney x)
	{
		if (!x.IsNull)
		{
			return new SqlDecimal(x.ToDecimal());
		}
		return Null;
	}

	public static explicit operator SqlDecimal(SqlSingle x)
	{
		if (!x.IsNull)
		{
			return new SqlDecimal(x.Value);
		}
		return Null;
	}

	public static explicit operator SqlDecimal(SqlDouble x)
	{
		if (!x.IsNull)
		{
			return new SqlDecimal(x.Value);
		}
		return Null;
	}

	public static explicit operator SqlDecimal(SqlString x)
	{
		if (!x.IsNull)
		{
			return Parse(x.Value);
		}
		return Null;
	}

	[Conditional("DEBUG")]
	private void AssertValid()
	{
		if (!IsNull)
		{
			_ = (new uint[4] { _data1, _data2, _data3, _data4 })[_bLen - 1];
			for (int i = _bLen; i < s_cNumeMax; i++)
			{
			}
		}
	}

	private static void ZeroToMaxLen(uint[] rgulData, int cUI4sCur)
	{
		switch (cUI4sCur)
		{
		case 1:
			rgulData[1] = (rgulData[2] = (rgulData[3] = 0u));
			break;
		case 2:
			rgulData[2] = (rgulData[3] = 0u);
			break;
		case 3:
			rgulData[3] = 0u;
			break;
		}
	}

	private static byte CLenFromPrec(byte bPrec)
	{
		return s_rgCLenFromPrec[bPrec - 1];
	}

	private bool FZero()
	{
		if (_data1 == 0)
		{
			return _bLen <= 1;
		}
		return false;
	}

	private bool FGt10_38()
	{
		if ((long)_data4 >= 1262177448L && _bLen == 4)
		{
			if ((long)_data4 <= 1262177448L && (long)_data3 <= 1518781562L)
			{
				if ((ulong)_data3 == 1518781562)
				{
					return (long)_data2 >= 160047680L;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool FGt10_38(uint[] rglData)
	{
		if ((long)rglData[3] >= 1262177448L)
		{
			if ((long)rglData[3] <= 1262177448L && (long)rglData[2] <= 1518781562L)
			{
				if ((ulong)rglData[2] == 1518781562)
				{
					return (long)rglData[1] >= 160047680L;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private static byte BGetPrecUI4(uint value)
	{
		int num = ((value < s_ulT4) ? ((value >= s_ulT2) ? ((value >= s_ulT3) ? 4 : 3) : ((value < s_ulT1) ? 1 : 2)) : ((value >= s_ulT8) ? ((value >= s_ulT9) ? 10 : 9) : ((value >= s_ulT6) ? ((value >= s_ulT7) ? 8 : 7) : ((value >= s_ulT5) ? 6 : 5))));
		return (byte)num;
	}

	private static byte BGetPrecUI8(uint ulU0, uint ulU1)
	{
		return BGetPrecUI8(ulU0 + ((ulong)ulU1 << 32));
	}

	private static byte BGetPrecUI8(ulong dwlVal)
	{
		int num;
		if (dwlVal >= s_ulT8)
		{
			num = ((dwlVal < s_dwlT16) ? ((dwlVal < s_dwlT12) ? ((dwlVal >= s_dwlT10) ? ((dwlVal >= s_dwlT11) ? 12 : 11) : ((dwlVal >= s_ulT9) ? 10 : 9)) : ((dwlVal >= s_dwlT14) ? ((dwlVal >= s_dwlT15) ? 16 : 15) : ((dwlVal >= s_dwlT13) ? 14 : 13))) : ((dwlVal >= s_dwlT18) ? ((dwlVal >= s_dwlT19) ? 20 : 19) : ((dwlVal >= s_dwlT17) ? 18 : 17)));
		}
		else
		{
			uint num2 = (uint)dwlVal;
			num = ((num2 < s_ulT4) ? ((num2 >= s_ulT2) ? ((num2 >= s_ulT3) ? 4 : 3) : ((num2 < s_ulT1) ? 1 : 2)) : ((num2 >= s_ulT6) ? ((num2 >= s_ulT7) ? 8 : 7) : ((num2 >= s_ulT5) ? 6 : 5)));
		}
		return (byte)num;
	}

	private void AddULong(uint ulAdd)
	{
		ulong num = ulAdd;
		int bLen = _bLen;
		uint[] array = new uint[4] { _data1, _data2, _data3, _data4 };
		int num2 = 0;
		do
		{
			num += array[num2];
			array[num2] = (uint)num;
			num >>= 32;
			if (num == 0L)
			{
				StoreFromWorkingArray(array);
				return;
			}
			num2++;
		}
		while (num2 < bLen);
		if (num2 == s_cNumeMax)
		{
			throw new OverflowException(SQLResource.ArithOverflowMessage);
		}
		array[num2] = (uint)num;
		_bLen++;
		if (FGt10_38(array))
		{
			throw new OverflowException(SQLResource.ArithOverflowMessage);
		}
		StoreFromWorkingArray(array);
	}

	private void MultByULong(uint uiMultiplier)
	{
		int bLen = _bLen;
		ulong num = 0uL;
		ulong num2 = 0uL;
		uint[] array = new uint[4] { _data1, _data2, _data3, _data4 };
		for (int i = 0; i < bLen; i++)
		{
			num2 = (ulong)array[i] * (ulong)uiMultiplier;
			num += num2;
			num2 = ((num >= num2) ? 0 : s_ulInt32Base);
			array[i] = (uint)num;
			num = (num >> 32) + num2;
		}
		if (num != 0L)
		{
			if (bLen == s_cNumeMax)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			array[bLen] = (uint)num;
			_bLen++;
		}
		if (FGt10_38(array))
		{
			throw new OverflowException(SQLResource.ArithOverflowMessage);
		}
		StoreFromWorkingArray(array);
	}

	private uint DivByULong(uint iDivisor)
	{
		ulong num = iDivisor;
		ulong num2 = 0uL;
		uint num3 = 0u;
		bool flag = true;
		if (num == 0L)
		{
			throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
		}
		uint[] array = new uint[4] { _data1, _data2, _data3, _data4 };
		for (int num4 = _bLen; num4 > 0; num4--)
		{
			num2 = (num2 << 32) + array[num4 - 1];
			num3 = (array[num4 - 1] = (uint)(num2 / num));
			num2 %= num;
			if (flag && num3 == 0)
			{
				_bLen--;
			}
			else
			{
				flag = false;
			}
		}
		StoreFromWorkingArray(array);
		if (flag)
		{
			_bLen = 1;
		}
		return (uint)num2;
	}

	internal void AdjustScale(int digits, bool fRound)
	{
		bool flag = false;
		int num = digits;
		if (num + _bScale < 0)
		{
			throw new SqlTruncateException();
		}
		if (num + _bScale > s_NUMERIC_MAX_PRECISION)
		{
			throw new OverflowException(SQLResource.ArithOverflowMessage);
		}
		byte bScale = (byte)(num + _bScale);
		byte bPrec = (byte)Math.Min(s_NUMERIC_MAX_PRECISION, Math.Max(1, num + _bPrec));
		if (num > 0)
		{
			_bScale = bScale;
			_bPrec = bPrec;
			while (num > 0)
			{
				uint uiMultiplier;
				if (num >= 9)
				{
					uiMultiplier = s_rgulShiftBase[8];
					num -= 9;
				}
				else
				{
					uiMultiplier = s_rgulShiftBase[num - 1];
					num = 0;
				}
				MultByULong(uiMultiplier);
			}
		}
		else if (num < 0)
		{
			uint uiMultiplier;
			uint num2;
			do
			{
				if (num <= -9)
				{
					uiMultiplier = s_rgulShiftBase[8];
					num += 9;
				}
				else
				{
					uiMultiplier = s_rgulShiftBase[-num - 1];
					num = 0;
				}
				num2 = DivByULong(uiMultiplier);
			}
			while (num < 0);
			flag = num2 >= uiMultiplier / 2;
			_bScale = bScale;
			_bPrec = bPrec;
		}
		if (flag && fRound)
		{
			AddULong(1u);
		}
		else if (FZero())
		{
			SetPositive();
		}
	}

	public static SqlDecimal AdjustScale(SqlDecimal n, int digits, bool fRound)
	{
		if (n.IsNull)
		{
			return Null;
		}
		SqlDecimal result = n;
		result.AdjustScale(digits, fRound);
		return result;
	}

	public static SqlDecimal ConvertToPrecScale(SqlDecimal n, int precision, int scale)
	{
		CheckValidPrecScale(precision, scale);
		if (n.IsNull)
		{
			return Null;
		}
		SqlDecimal result = n;
		int digits = scale - result._bScale;
		result.AdjustScale(digits, fRound: true);
		byte b = CLenFromPrec((byte)precision);
		if (b < result._bLen)
		{
			throw new SqlTruncateException();
		}
		if (b == result._bLen && precision < result.CalculatePrecision())
		{
			throw new SqlTruncateException();
		}
		result._bPrec = (byte)precision;
		return result;
	}

	private int LAbsCmp(SqlDecimal snumOp)
	{
		int bLen = snumOp._bLen;
		int bLen2 = _bLen;
		if (bLen != bLen2)
		{
			if (bLen2 <= bLen)
			{
				return -1;
			}
			return 1;
		}
		uint[] array = new uint[4] { _data1, _data2, _data3, _data4 };
		uint[] array2 = new uint[4] { snumOp._data1, snumOp._data2, snumOp._data3, snumOp._data4 };
		int num = bLen - 1;
		do
		{
			if (array[num] != array2[num])
			{
				if (array[num] <= array2[num])
				{
					return -1;
				}
				return 1;
			}
			num--;
		}
		while (num >= 0);
		return 0;
	}

	private static void MpMove(uint[] rgulS, int ciulS, uint[] rgulD, out int ciulD)
	{
		ciulD = ciulS;
		for (int i = 0; i < ciulS; i++)
		{
			rgulD[i] = rgulS[i];
		}
	}

	private static void MpSet(uint[] rgulD, out int ciulD, uint iulN)
	{
		ciulD = 1;
		rgulD[0] = iulN;
	}

	private static void MpNormalize(uint[] rgulU, ref int ciulU)
	{
		while (ciulU > 1 && rgulU[ciulU - 1] == 0)
		{
			ciulU--;
		}
	}

	private static void MpMul1(uint[] piulD, ref int ciulD, uint iulX)
	{
		uint num = 0u;
		int i;
		for (i = 0; i < ciulD; i++)
		{
			ulong num2 = piulD[i];
			ulong x = num + num2 * iulX;
			num = HI(x);
			piulD[i] = LO(x);
		}
		if (num != 0)
		{
			piulD[i] = num;
			ciulD++;
		}
	}

	private static void MpDiv1(uint[] rgulU, ref int ciulU, uint iulD, out uint iulR)
	{
		uint num = 0u;
		ulong num2 = iulD;
		int num3 = ciulU;
		while (num3 > 0)
		{
			num3--;
			ulong num4 = ((ulong)num << 32) + rgulU[num3];
			rgulU[num3] = (uint)(num4 / num2);
			num = (uint)(num4 - rgulU[num3] * num2);
		}
		iulR = num;
		MpNormalize(rgulU, ref ciulU);
	}

	internal static ulong DWL(uint lo, uint hi)
	{
		return lo + ((ulong)hi << 32);
	}

	private static uint HI(ulong x)
	{
		return (uint)(x >> 32);
	}

	private static uint LO(ulong x)
	{
		return (uint)x;
	}

	private static void MpDiv(uint[] rgulU, int ciulU, uint[] rgulD, int ciulD, uint[] rgulQ, out int ciulQ, uint[] rgulR, out int ciulR)
	{
		if (ciulD == 1 && rgulD[0] == 0)
		{
			ciulQ = (ciulR = 0);
			return;
		}
		if (ciulU == 1 && ciulD == 1)
		{
			MpSet(rgulQ, out ciulQ, rgulU[0] / rgulD[0]);
			MpSet(rgulR, out ciulR, rgulU[0] % rgulD[0]);
			return;
		}
		if (ciulD > ciulU)
		{
			MpMove(rgulU, ciulU, rgulR, out ciulR);
			MpSet(rgulQ, out ciulQ, 0u);
			return;
		}
		if (ciulU <= 2)
		{
			ulong num = DWL(rgulU[0], rgulU[1]);
			ulong num2 = rgulD[0];
			if (ciulD > 1)
			{
				num2 += (ulong)rgulD[1] << 32;
			}
			ulong x = num / num2;
			rgulQ[0] = LO(x);
			rgulQ[1] = HI(x);
			ciulQ = ((HI(x) == 0) ? 1 : 2);
			x = num % num2;
			rgulR[0] = LO(x);
			rgulR[1] = HI(x);
			ciulR = ((HI(x) == 0) ? 1 : 2);
			return;
		}
		if (ciulD == 1)
		{
			MpMove(rgulU, ciulU, rgulQ, out ciulQ);
			MpDiv1(rgulQ, ref ciulQ, rgulD[0], out var iulR);
			rgulR[0] = iulR;
			ciulR = 1;
			return;
		}
		ciulQ = (ciulR = 0);
		if (rgulU != rgulR)
		{
			MpMove(rgulU, ciulU, rgulR, out ciulR);
		}
		ciulQ = ciulU - ciulD + 1;
		uint num3 = rgulD[ciulD - 1];
		rgulR[ciulU] = 0u;
		int num4 = ciulU;
		uint num5 = (uint)(s_ulInt32Base / (ulong)((long)num3 + 1L));
		if (num5 > 1)
		{
			MpMul1(rgulD, ref ciulD, num5);
			num3 = rgulD[ciulD - 1];
			MpMul1(rgulR, ref ciulR, num5);
		}
		uint num6 = rgulD[ciulD - 2];
		do
		{
			ulong num7 = DWL(rgulR[num4 - 1], rgulR[num4]);
			uint num8 = (uint)((num3 != rgulR[num4]) ? (num7 / num3) : (s_ulInt32Base - 1));
			ulong num9 = num8;
			uint num10 = (uint)(num7 - num9 * num3);
			while (num6 * num9 > DWL(rgulR[num4 - 2], num10))
			{
				num8--;
				if (num10 >= 0 - num3)
				{
					break;
				}
				num10 += num3;
				num9 = num8;
			}
			num7 = s_ulInt32Base;
			ulong num11 = 0uL;
			int num12 = 0;
			int num13 = num4 - ciulD;
			while (num12 < ciulD)
			{
				ulong num14 = rgulD[num12];
				num11 += num8 * num14;
				num7 += (ulong)((long)rgulR[num13] - (long)LO(num11));
				num11 = HI(num11);
				rgulR[num13] = LO(num7);
				num7 = HI(num7) + s_ulInt32Base - 1;
				num12++;
				num13++;
			}
			num7 += rgulR[num13] - num11;
			rgulR[num13] = LO(num7);
			rgulQ[num4 - ciulD] = num8;
			if (HI(num7) == 0)
			{
				rgulQ[num4 - ciulD] = num8 - 1;
				uint num15 = 0u;
				num12 = 0;
				num13 = num4 - ciulD;
				while (num12 < ciulD)
				{
					num7 = (ulong)((long)rgulD[num12] + (long)rgulR[num13] + num15);
					num15 = HI(num7);
					rgulR[num13] = LO(num7);
					num12++;
					num13++;
				}
				rgulR[num13] += num15;
			}
			num4--;
		}
		while (num4 >= ciulD);
		MpNormalize(rgulQ, ref ciulQ);
		ciulR = ciulD;
		MpNormalize(rgulR, ref ciulR);
		if (num5 > 1)
		{
			MpDiv1(rgulD, ref ciulD, num5, out var iulR2);
			MpDiv1(rgulR, ref ciulR, num5, out iulR2);
		}
	}

	private EComparison CompareNm(SqlDecimal snumOp)
	{
		int num = (IsPositive ? 1 : (-1));
		int num2 = (snumOp.IsPositive ? 1 : (-1));
		if (num != num2)
		{
			if (num != 1)
			{
				return EComparison.LT;
			}
			return EComparison.GT;
		}
		SqlDecimal sqlDecimal = this;
		SqlDecimal snumOp2 = snumOp;
		int num3 = _bScale - snumOp._bScale;
		if (num3 < 0)
		{
			try
			{
				sqlDecimal.AdjustScale(-num3, fRound: true);
			}
			catch (OverflowException)
			{
				return (num > 0) ? EComparison.GT : EComparison.LT;
			}
		}
		else if (num3 > 0)
		{
			try
			{
				snumOp2.AdjustScale(num3, fRound: true);
			}
			catch (OverflowException)
			{
				return (num <= 0) ? EComparison.GT : EComparison.LT;
			}
		}
		int num4 = sqlDecimal.LAbsCmp(snumOp2);
		if (num4 == 0)
		{
			return EComparison.EQ;
		}
		if (num * num4 < 0)
		{
			return EComparison.LT;
		}
		return EComparison.GT;
	}

	private static void CheckValidPrecScale(byte bPrec, byte bScale)
	{
		if (bPrec < 1 || bPrec > MaxPrecision || bScale < 0 || bScale > MaxScale || bScale > bPrec)
		{
			throw new SqlTypeException(SQLResource.InvalidPrecScaleMessage);
		}
	}

	private static void CheckValidPrecScale(int iPrec, int iScale)
	{
		if (iPrec < 1 || iPrec > MaxPrecision || iScale < 0 || iScale > MaxScale || iScale > iPrec)
		{
			throw new SqlTypeException(SQLResource.InvalidPrecScaleMessage);
		}
	}

	public static SqlBoolean operator ==(SqlDecimal x, SqlDecimal y)
	{
		if (!x.IsNull && !y.IsNull)
		{
			return new SqlBoolean(x.CompareNm(y) == EComparison.EQ);
		}
		return SqlBoolean.Null;
	}

	public static SqlBoolean operator !=(SqlDecimal x, SqlDecimal y)
	{
		return !(x == y);
	}

	public static SqlBoolean operator <(SqlDecimal x, SqlDecimal y)
	{
		if (!x.IsNull && !y.IsNull)
		{
			return new SqlBoolean(x.CompareNm(y) == EComparison.LT);
		}
		return SqlBoolean.Null;
	}

	public static SqlBoolean operator >(SqlDecimal x, SqlDecimal y)
	{
		if (!x.IsNull && !y.IsNull)
		{
			return new SqlBoolean(x.CompareNm(y) == EComparison.GT);
		}
		return SqlBoolean.Null;
	}

	public static SqlBoolean operator <=(SqlDecimal x, SqlDecimal y)
	{
		if (x.IsNull || y.IsNull)
		{
			return SqlBoolean.Null;
		}
		EComparison eComparison = x.CompareNm(y);
		return new SqlBoolean(eComparison == EComparison.LT || eComparison == EComparison.EQ);
	}

	public static SqlBoolean operator >=(SqlDecimal x, SqlDecimal y)
	{
		if (x.IsNull || y.IsNull)
		{
			return SqlBoolean.Null;
		}
		EComparison eComparison = x.CompareNm(y);
		return new SqlBoolean(eComparison == EComparison.GT || eComparison == EComparison.EQ);
	}

	public static SqlDecimal Add(SqlDecimal x, SqlDecimal y)
	{
		return x + y;
	}

	public static SqlDecimal Subtract(SqlDecimal x, SqlDecimal y)
	{
		return x - y;
	}

	public static SqlDecimal Multiply(SqlDecimal x, SqlDecimal y)
	{
		return x * y;
	}

	public static SqlDecimal Divide(SqlDecimal x, SqlDecimal y)
	{
		return x / y;
	}

	public static SqlBoolean Equals(SqlDecimal x, SqlDecimal y)
	{
		return x == y;
	}

	public static SqlBoolean NotEquals(SqlDecimal x, SqlDecimal y)
	{
		return x != y;
	}

	public static SqlBoolean LessThan(SqlDecimal x, SqlDecimal y)
	{
		return x < y;
	}

	public static SqlBoolean GreaterThan(SqlDecimal x, SqlDecimal y)
	{
		return x > y;
	}

	public static SqlBoolean LessThanOrEqual(SqlDecimal x, SqlDecimal y)
	{
		return x <= y;
	}

	public static SqlBoolean GreaterThanOrEqual(SqlDecimal x, SqlDecimal y)
	{
		return x >= y;
	}

	public SqlBoolean ToSqlBoolean()
	{
		return (SqlBoolean)this;
	}

	public SqlByte ToSqlByte()
	{
		return (SqlByte)this;
	}

	public SqlDouble ToSqlDouble()
	{
		return this;
	}

	public SqlInt16 ToSqlInt16()
	{
		return (SqlInt16)this;
	}

	public SqlInt32 ToSqlInt32()
	{
		return (SqlInt32)this;
	}

	public SqlInt64 ToSqlInt64()
	{
		return (SqlInt64)this;
	}

	public SqlMoney ToSqlMoney()
	{
		return (SqlMoney)this;
	}

	public SqlSingle ToSqlSingle()
	{
		return this;
	}

	public SqlString ToSqlString()
	{
		return (SqlString)this;
	}

	private static char ChFromDigit(uint uiDigit)
	{
		return (char)(uiDigit + 48);
	}

	private void StoreFromWorkingArray(uint[] rguiData)
	{
		_data1 = rguiData[0];
		_data2 = rguiData[1];
		_data3 = rguiData[2];
		_data4 = rguiData[3];
	}

	private void SetToZero()
	{
		_bLen = 1;
		_data1 = (_data2 = (_data3 = (_data4 = 0u)));
		_bStatus = (byte)(s_bNotNull | s_bPositive);
	}

	private void MakeInteger(out bool fFraction)
	{
		int num = _bScale;
		fFraction = false;
		while (num > 0)
		{
			uint num2;
			if (num >= 9)
			{
				num2 = DivByULong(s_rgulShiftBase[8]);
				num -= 9;
			}
			else
			{
				num2 = DivByULong(s_rgulShiftBase[num - 1]);
				num = 0;
			}
			if (num2 != 0)
			{
				fFraction = true;
			}
		}
		_bScale = 0;
	}

	public static SqlDecimal Abs(SqlDecimal n)
	{
		if (n.IsNull)
		{
			return Null;
		}
		n.SetPositive();
		return n;
	}

	public static SqlDecimal Ceiling(SqlDecimal n)
	{
		if (n.IsNull)
		{
			return Null;
		}
		if (n._bScale == 0)
		{
			return n;
		}
		n.MakeInteger(out var fFraction);
		if (fFraction && n.IsPositive)
		{
			n.AddULong(1u);
		}
		if (n.FZero())
		{
			n.SetPositive();
		}
		return n;
	}

	public static SqlDecimal Floor(SqlDecimal n)
	{
		if (n.IsNull)
		{
			return Null;
		}
		if (n._bScale == 0)
		{
			return n;
		}
		n.MakeInteger(out var fFraction);
		if (fFraction && !n.IsPositive)
		{
			n.AddULong(1u);
		}
		if (n.FZero())
		{
			n.SetPositive();
		}
		return n;
	}

	public static SqlInt32 Sign(SqlDecimal n)
	{
		if (n.IsNull)
		{
			return SqlInt32.Null;
		}
		if (n == new SqlDecimal(0))
		{
			return SqlInt32.Zero;
		}
		if (!n.IsNull)
		{
			if (!n.IsPositive)
			{
				return new SqlInt32(-1);
			}
			return new SqlInt32(1);
		}
		return SqlInt32.Null;
	}

	private static SqlDecimal Round(SqlDecimal n, int lPosition, bool fTruncate)
	{
		if (n.IsNull)
		{
			return Null;
		}
		if (lPosition >= 0)
		{
			lPosition = Math.Min(s_NUMERIC_MAX_PRECISION, lPosition);
			if (lPosition >= n._bScale)
			{
				return n;
			}
		}
		else
		{
			lPosition = Math.Max(-s_NUMERIC_MAX_PRECISION, lPosition);
			if (lPosition < n._bScale - n._bPrec)
			{
				n.SetToZero();
				return n;
			}
		}
		uint num = 0u;
		int num2 = Math.Abs(lPosition - n._bScale);
		uint num3 = 1u;
		while (num2 > 0)
		{
			if (num2 >= 9)
			{
				num = n.DivByULong(s_rgulShiftBase[8]);
				num3 = s_rgulShiftBase[8];
				num2 -= 9;
			}
			else
			{
				num = n.DivByULong(s_rgulShiftBase[num2 - 1]);
				num3 = s_rgulShiftBase[num2 - 1];
				num2 = 0;
			}
		}
		if (num3 > 1)
		{
			num /= num3 / 10;
		}
		if (n.FZero() && (fTruncate || num < 5))
		{
			n.SetPositive();
			return n;
		}
		if (num >= 5 && !fTruncate)
		{
			n.AddULong(1u);
		}
		num2 = Math.Abs(lPosition - n._bScale);
		while (num2-- > 0)
		{
			n.MultByULong(s_ulBase10);
		}
		return n;
	}

	public static SqlDecimal Round(SqlDecimal n, int position)
	{
		return Round(n, position, fTruncate: false);
	}

	public static SqlDecimal Truncate(SqlDecimal n, int position)
	{
		return Round(n, position, fTruncate: true);
	}

	public static SqlDecimal Power(SqlDecimal n, double exp)
	{
		if (n.IsNull)
		{
			return Null;
		}
		_ = n.Precision;
		int scale = n.Scale;
		double x = n.ToDouble();
		n = new SqlDecimal(Math.Pow(x, exp));
		n.AdjustScale(scale - n.Scale, fRound: true);
		n._bPrec = MaxPrecision;
		return n;
	}

	public int CompareTo(object value)
	{
		if (value is SqlDecimal value2)
		{
			return CompareTo(value2);
		}
		throw ADP.WrongType(value.GetType(), typeof(SqlDecimal));
	}

	public int CompareTo(SqlDecimal value)
	{
		if (IsNull)
		{
			if (!value.IsNull)
			{
				return -1;
			}
			return 0;
		}
		if (value.IsNull)
		{
			return 1;
		}
		if (this < value)
		{
			return -1;
		}
		if (this > value)
		{
			return 1;
		}
		return 0;
	}

	public override bool Equals(object value)
	{
		if (!(value is SqlDecimal sqlDecimal))
		{
			return false;
		}
		if (sqlDecimal.IsNull || IsNull)
		{
			if (sqlDecimal.IsNull)
			{
				return IsNull;
			}
			return false;
		}
		return (this == sqlDecimal).Value;
	}

	public override int GetHashCode()
	{
		if (IsNull)
		{
			return 0;
		}
		SqlDecimal sqlDecimal = this;
		int num = sqlDecimal.CalculatePrecision();
		sqlDecimal.AdjustScale(s_NUMERIC_MAX_PRECISION - num, fRound: true);
		int bLen = sqlDecimal._bLen;
		int num2 = 0;
		int[] data = sqlDecimal.Data;
		for (int i = 0; i < bLen; i++)
		{
			int num3 = (num2 >> 28) & 0xFF;
			num2 <<= 4;
			num2 = num2 ^ data[i] ^ num3;
		}
		return num2;
	}

	XmlSchema IXmlSerializable.GetSchema()
	{
		return null;
	}

	void IXmlSerializable.ReadXml(XmlReader reader)
	{
		string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
		if (attribute != null && XmlConvert.ToBoolean(attribute))
		{
			reader.ReadElementString();
			_bStatus = (byte)(s_bReverseNullMask & _bStatus);
			return;
		}
		SqlDecimal sqlDecimal = Parse(reader.ReadElementString());
		_bStatus = sqlDecimal._bStatus;
		_bLen = sqlDecimal._bLen;
		_bPrec = sqlDecimal._bPrec;
		_bScale = sqlDecimal._bScale;
		_data1 = sqlDecimal._data1;
		_data2 = sqlDecimal._data2;
		_data3 = sqlDecimal._data3;
		_data4 = sqlDecimal._data4;
	}

	void IXmlSerializable.WriteXml(XmlWriter writer)
	{
		if (IsNull)
		{
			writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
		}
		else
		{
			writer.WriteString(ToString());
		}
	}

	public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
	{
		return new XmlQualifiedName("decimal", "http://www.w3.org/2001/XMLSchema");
	}
}
