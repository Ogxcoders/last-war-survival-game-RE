using System.Diagnostics;
using System.Globalization;

namespace System.Numerics;

[Serializable]
public readonly struct BigInteger : IFormattable, IComparable, IComparable<BigInteger>, IEquatable<BigInteger>
{
	private enum GetBytesMode
	{
		AllocateArray,
		Count,
		Span
	}

	private const int knMaskHighBit = int.MinValue;

	private const uint kuMaskHighBit = 2147483648u;

	private const int kcbitUint = 32;

	private const int kcbitUlong = 64;

	private const int DecimalScaleFactorMask = 16711680;

	private const int DecimalSignMask = int.MinValue;

	internal readonly int _sign;

	internal readonly uint[] _bits;

	private static readonly BigInteger s_bnMinInt = new BigInteger(-1, new uint[1] { 2147483648u });

	private static readonly BigInteger s_bnOneInt = new BigInteger(1);

	private static readonly BigInteger s_bnZeroInt = new BigInteger(0);

	private static readonly BigInteger s_bnMinusOneInt = new BigInteger(-1);

	private static readonly byte[] s_success = Array.Empty<byte>();

	public static BigInteger Zero => s_bnZeroInt;

	public static BigInteger One => s_bnOneInt;

	public static BigInteger MinusOne => s_bnMinusOneInt;

	public bool IsPowerOfTwo
	{
		get
		{
			if (_bits == null)
			{
				if ((_sign & (_sign - 1)) == 0)
				{
					return _sign != 0;
				}
				return false;
			}
			if (_sign != 1)
			{
				return false;
			}
			int num = _bits.Length - 1;
			if ((_bits[num] & (_bits[num] - 1)) != 0)
			{
				return false;
			}
			while (--num >= 0)
			{
				if (_bits[num] != 0)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool IsZero => _sign == 0;

	public bool IsOne
	{
		get
		{
			if (_sign == 1)
			{
				return _bits == null;
			}
			return false;
		}
	}

	public bool IsEven
	{
		get
		{
			if (_bits != null)
			{
				return (_bits[0] & 1) == 0;
			}
			return (_sign & 1) == 0;
		}
	}

	public int Sign => (_sign >> 31) - (-_sign >> 31);

	public BigInteger(int value)
	{
		if (value == int.MinValue)
		{
			this = s_bnMinInt;
			return;
		}
		_sign = value;
		_bits = null;
	}

	[CLSCompliant(false)]
	public BigInteger(uint value)
	{
		if (value <= int.MaxValue)
		{
			_sign = (int)value;
			_bits = null;
		}
		else
		{
			_sign = 1;
			_bits = new uint[1];
			_bits[0] = value;
		}
	}

	public BigInteger(long value)
	{
		if (int.MinValue < value && value <= int.MaxValue)
		{
			_sign = (int)value;
			_bits = null;
			return;
		}
		if (value == int.MinValue)
		{
			this = s_bnMinInt;
			return;
		}
		ulong num = 0uL;
		if (value < 0)
		{
			num = (ulong)(-value);
			_sign = -1;
		}
		else
		{
			num = (ulong)value;
			_sign = 1;
		}
		if (num <= uint.MaxValue)
		{
			_bits = new uint[1];
			_bits[0] = (uint)num;
		}
		else
		{
			_bits = new uint[2];
			_bits[0] = (uint)num;
			_bits[1] = (uint)(num >> 32);
		}
	}

	[CLSCompliant(false)]
	public BigInteger(ulong value)
	{
		if (value <= int.MaxValue)
		{
			_sign = (int)value;
			_bits = null;
		}
		else if (value <= uint.MaxValue)
		{
			_sign = 1;
			_bits = new uint[1];
			_bits[0] = (uint)value;
		}
		else
		{
			_sign = 1;
			_bits = new uint[2];
			_bits[0] = (uint)value;
			_bits[1] = (uint)(value >> 32);
		}
	}

	public BigInteger(float value)
		: this((double)value)
	{
	}

	public BigInteger(double value)
	{
		if (!double.IsFinite(value))
		{
			if (double.IsInfinity(value))
			{
				throw new OverflowException("BigInteger cannot represent infinity.");
			}
			throw new OverflowException("The value is not a number.");
		}
		_sign = 0;
		_bits = null;
		NumericsHelpers.GetDoubleParts(value, out var sign, out var exp, out var man, out var _);
		if (man == 0L)
		{
			this = Zero;
			return;
		}
		if (exp <= 0)
		{
			if (exp <= -64)
			{
				this = Zero;
				return;
			}
			this = man >> -exp;
			if (sign < 0)
			{
				_sign = -_sign;
			}
			return;
		}
		if (exp <= 11)
		{
			this = man << exp;
			if (sign < 0)
			{
				_sign = -_sign;
			}
			return;
		}
		man <<= 11;
		exp -= 11;
		int num = (exp - 1) / 32 + 1;
		int num2 = num * 32 - exp;
		_bits = new uint[num + 2];
		_bits[num + 1] = (uint)(man >> num2 + 32);
		_bits[num] = (uint)(man >> num2);
		if (num2 > 0)
		{
			_bits[num - 1] = (uint)((int)man << 32 - num2);
		}
		_sign = sign;
	}

	public BigInteger(decimal value)
	{
		int[] bits = decimal.GetBits(decimal.Truncate(value));
		int num = 3;
		while (num > 0 && bits[num - 1] == 0)
		{
			num--;
		}
		switch (num)
		{
		case 0:
			this = s_bnZeroInt;
			return;
		case 1:
			if (bits[0] > 0)
			{
				_sign = bits[0];
				_sign *= (((bits[3] & int.MinValue) == 0) ? 1 : (-1));
				_bits = null;
				return;
			}
			break;
		}
		_bits = new uint[num];
		_bits[0] = (uint)bits[0];
		if (num > 1)
		{
			_bits[1] = (uint)bits[1];
		}
		if (num > 2)
		{
			_bits[2] = (uint)bits[2];
		}
		_sign = (((bits[3] & int.MinValue) == 0) ? 1 : (-1));
	}

	[CLSCompliant(false)]
	public BigInteger(byte[] value)
		: this(new ReadOnlySpan<byte>(value ?? throw new ArgumentNullException("value")))
	{
	}

	public BigInteger(ReadOnlySpan<byte> value, bool isUnsigned = false, bool isBigEndian = false)
	{
		int num = value.Length;
		bool flag;
		if (num > 0)
		{
			byte num2 = (isBigEndian ? value[0] : value[num - 1]);
			flag = (num2 & 0x80) != 0 && !isUnsigned;
			if (num2 == 0)
			{
				if (isBigEndian)
				{
					int i;
					for (i = 1; i < num && value[i] == 0; i++)
					{
					}
					value = value.Slice(i);
					num = value.Length;
				}
				else
				{
					num -= 2;
					while (num >= 0 && value[num] == 0)
					{
						num--;
					}
					num++;
				}
			}
		}
		else
		{
			flag = false;
		}
		if (num == 0)
		{
			_sign = 0;
			_bits = null;
			return;
		}
		if (num <= 4)
		{
			_sign = (flag ? (-1) : 0);
			if (isBigEndian)
			{
				for (int j = 0; j < num; j++)
				{
					_sign = (_sign << 8) | value[j];
				}
			}
			else
			{
				for (int num3 = num - 1; num3 >= 0; num3--)
				{
					_sign = (_sign << 8) | value[num3];
				}
			}
			_bits = null;
			if (_sign < 0 && !flag)
			{
				_bits = new uint[1] { (uint)_sign };
				_sign = 1;
			}
			if (_sign == int.MinValue)
			{
				this = s_bnMinInt;
			}
			return;
		}
		int num4 = num % 4;
		int num5 = num / 4 + ((num4 != 0) ? 1 : 0);
		uint[] array = new uint[num5];
		int num6 = num - 1;
		int k;
		if (isBigEndian)
		{
			int num7 = num - 4;
			for (k = 0; k < num5 - ((num4 != 0) ? 1 : 0); k++)
			{
				for (int l = 0; l < 4; l++)
				{
					byte b = value[num7];
					array[k] = (array[k] << 8) | b;
					num7++;
				}
				num7 -= 8;
			}
		}
		else
		{
			int num7 = 3;
			for (k = 0; k < num5 - ((num4 != 0) ? 1 : 0); k++)
			{
				for (int m = 0; m < 4; m++)
				{
					byte b2 = value[num7];
					array[k] = (array[k] << 8) | b2;
					num7--;
				}
				num7 += 8;
			}
		}
		if (num4 != 0)
		{
			if (flag)
			{
				array[num5 - 1] = uint.MaxValue;
			}
			if (isBigEndian)
			{
				for (int num7 = 0; num7 < num4; num7++)
				{
					byte b3 = value[num7];
					array[k] = (array[k] << 8) | b3;
				}
			}
			else
			{
				for (int num7 = num6; num7 >= num - num4; num7--)
				{
					byte b4 = value[num7];
					array[k] = (array[k] << 8) | b4;
				}
			}
		}
		if (flag)
		{
			NumericsHelpers.DangerousMakeTwosComplement(array);
			int num8 = array.Length - 1;
			while (num8 >= 0 && array[num8] == 0)
			{
				num8--;
			}
			num8++;
			if (num8 == 1)
			{
				switch (array[0])
				{
				case 1u:
					this = s_bnMinusOneInt;
					return;
				case 2147483648u:
					this = s_bnMinInt;
					return;
				}
				if ((int)array[0] > 0)
				{
					_sign = -1 * (int)array[0];
					_bits = null;
					return;
				}
			}
			if (num8 != array.Length)
			{
				_sign = -1;
				_bits = new uint[num8];
				Array.Copy(array, 0, _bits, 0, num8);
			}
			else
			{
				_sign = -1;
				_bits = array;
			}
		}
		else
		{
			_sign = 1;
			_bits = array;
		}
	}

	internal BigInteger(int n, uint[] rgu)
	{
		_sign = n;
		_bits = rgu;
	}

	internal BigInteger(uint[] value, bool negative)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		int num = value.Length;
		while (num > 0 && value[num - 1] == 0)
		{
			num--;
		}
		switch (num)
		{
		case 0:
			this = s_bnZeroInt;
			break;
		case 1:
			if (value[0] < 2147483648u)
			{
				_sign = (int)(negative ? (0 - value[0]) : value[0]);
				_bits = null;
				if (_sign == int.MinValue)
				{
					this = s_bnMinInt;
				}
				break;
			}
			goto default;
		default:
			_sign = ((!negative) ? 1 : (-1));
			_bits = new uint[num];
			Array.Copy(value, 0, _bits, 0, num);
			break;
		}
	}

	private BigInteger(uint[] value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		int num = value.Length;
		bool flag = num > 0 && (value[num - 1] & 0x80000000u) == 2147483648u;
		while (num > 0 && value[num - 1] == 0)
		{
			num--;
		}
		switch (num)
		{
		case 0:
			this = s_bnZeroInt;
			return;
		case 1:
			if ((int)value[0] < 0 && !flag)
			{
				_bits = new uint[1];
				_bits[0] = value[0];
				_sign = 1;
			}
			else if (int.MinValue == (int)value[0])
			{
				this = s_bnMinInt;
			}
			else
			{
				_sign = (int)value[0];
				_bits = null;
			}
			return;
		}
		if (!flag)
		{
			if (num != value.Length)
			{
				_sign = 1;
				_bits = new uint[num];
				Array.Copy(value, 0, _bits, 0, num);
			}
			else
			{
				_sign = 1;
				_bits = value;
			}
			return;
		}
		NumericsHelpers.DangerousMakeTwosComplement(value);
		int num2 = value.Length;
		while (num2 > 0 && value[num2 - 1] == 0)
		{
			num2--;
		}
		if (num2 == 1 && (int)value[0] > 0)
		{
			if (value[0] == 1)
			{
				this = s_bnMinusOneInt;
				return;
			}
			if (value[0] == 2147483648u)
			{
				this = s_bnMinInt;
				return;
			}
			_sign = -1 * (int)value[0];
			_bits = null;
		}
		else if (num2 != value.Length)
		{
			_sign = -1;
			_bits = new uint[num2];
			Array.Copy(value, 0, _bits, 0, num2);
		}
		else
		{
			_sign = -1;
			_bits = value;
		}
	}

	public static BigInteger Parse(string value)
	{
		return Parse(value, NumberStyles.Integer);
	}

	public static BigInteger Parse(string value, NumberStyles style)
	{
		return Parse(value, style, NumberFormatInfo.CurrentInfo);
	}

	public static BigInteger Parse(string value, IFormatProvider provider)
	{
		return Parse(value, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
	}

	public static BigInteger Parse(string value, NumberStyles style, IFormatProvider provider)
	{
		return BigNumber.ParseBigInteger(value, style, NumberFormatInfo.GetInstance(provider));
	}

	public static bool TryParse(string value, out BigInteger result)
	{
		return TryParse(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
	}

	public static bool TryParse(string value, NumberStyles style, IFormatProvider provider, out BigInteger result)
	{
		return BigNumber.TryParseBigInteger(value, style, NumberFormatInfo.GetInstance(provider), out result);
	}

	public static BigInteger Parse(ReadOnlySpan<char> value, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)
	{
		return BigNumber.ParseBigInteger(value, style, NumberFormatInfo.GetInstance(provider));
	}

	public static bool TryParse(ReadOnlySpan<char> value, out BigInteger result)
	{
		return BigNumber.TryParseBigInteger(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
	}

	public static bool TryParse(ReadOnlySpan<char> value, NumberStyles style, IFormatProvider provider, out BigInteger result)
	{
		return BigNumber.TryParseBigInteger(value, style, NumberFormatInfo.GetInstance(provider), out result);
	}

	public static int Compare(BigInteger left, BigInteger right)
	{
		return left.CompareTo(right);
	}

	public static BigInteger Abs(BigInteger value)
	{
		if (!(value >= Zero))
		{
			return -value;
		}
		return value;
	}

	public static BigInteger Add(BigInteger left, BigInteger right)
	{
		return left + right;
	}

	public static BigInteger Subtract(BigInteger left, BigInteger right)
	{
		return left - right;
	}

	public static BigInteger Multiply(BigInteger left, BigInteger right)
	{
		return left * right;
	}

	public static BigInteger Divide(BigInteger dividend, BigInteger divisor)
	{
		return dividend / divisor;
	}

	public static BigInteger Remainder(BigInteger dividend, BigInteger divisor)
	{
		return dividend % divisor;
	}

	public static BigInteger DivRem(BigInteger dividend, BigInteger divisor, out BigInteger remainder)
	{
		bool flag = dividend._bits == null;
		bool flag2 = divisor._bits == null;
		if (flag && flag2)
		{
			remainder = dividend._sign % divisor._sign;
			return dividend._sign / divisor._sign;
		}
		if (flag)
		{
			remainder = dividend;
			return s_bnZeroInt;
		}
		if (flag2)
		{
			uint remainder2;
			uint[] value = BigIntegerCalculator.Divide(dividend._bits, NumericsHelpers.Abs(divisor._sign), out remainder2);
			remainder = ((dividend._sign < 0) ? (-1 * remainder2) : remainder2);
			return new BigInteger(value, (dividend._sign < 0) ^ (divisor._sign < 0));
		}
		if (dividend._bits.Length < divisor._bits.Length)
		{
			remainder = dividend;
			return s_bnZeroInt;
		}
		uint[] remainder3;
		uint[] value2 = BigIntegerCalculator.Divide(dividend._bits, divisor._bits, out remainder3);
		remainder = new BigInteger(remainder3, dividend._sign < 0);
		return new BigInteger(value2, (dividend._sign < 0) ^ (divisor._sign < 0));
	}

	public static BigInteger Negate(BigInteger value)
	{
		return -value;
	}

	public static double Log(BigInteger value)
	{
		return Log(value, Math.E);
	}

	public static double Log(BigInteger value, double baseValue)
	{
		if (value._sign < 0 || baseValue == 1.0)
		{
			return double.NaN;
		}
		if (baseValue == double.PositiveInfinity)
		{
			if (!value.IsOne)
			{
				return double.NaN;
			}
			return 0.0;
		}
		if (baseValue == 0.0 && !value.IsOne)
		{
			return double.NaN;
		}
		if (value._bits == null)
		{
			return Math.Log(value._sign, baseValue);
		}
		long num = value._bits[value._bits.Length - 1];
		ulong num2 = ((value._bits.Length > 1) ? value._bits[value._bits.Length - 2] : 0u);
		ulong num3 = ((value._bits.Length > 2) ? value._bits[value._bits.Length - 3] : 0u);
		int num4 = NumericsHelpers.CbitHighZero((uint)num);
		long num5 = (long)value._bits.Length * 32L - num4;
		return Math.Log((ulong)(num << 32 + num4) | (num2 << num4) | (num3 >> 32 - num4), baseValue) + (double)(num5 - 64) / Math.Log(baseValue, 2.0);
	}

	public static double Log10(BigInteger value)
	{
		return Log(value, 10.0);
	}

	public static BigInteger GreatestCommonDivisor(BigInteger left, BigInteger right)
	{
		bool flag = left._bits == null;
		bool flag2 = right._bits == null;
		if (flag && flag2)
		{
			return BigIntegerCalculator.Gcd(NumericsHelpers.Abs(left._sign), NumericsHelpers.Abs(right._sign));
		}
		if (flag)
		{
			if (left._sign == 0)
			{
				return new BigInteger(right._bits, negative: false);
			}
			return BigIntegerCalculator.Gcd(right._bits, NumericsHelpers.Abs(left._sign));
		}
		if (flag2)
		{
			if (right._sign == 0)
			{
				return new BigInteger(left._bits, negative: false);
			}
			return BigIntegerCalculator.Gcd(left._bits, NumericsHelpers.Abs(right._sign));
		}
		if (BigIntegerCalculator.Compare(left._bits, right._bits) < 0)
		{
			return GreatestCommonDivisor(right._bits, left._bits);
		}
		return GreatestCommonDivisor(left._bits, right._bits);
	}

	private static BigInteger GreatestCommonDivisor(uint[] leftBits, uint[] rightBits)
	{
		if (rightBits.Length == 1)
		{
			uint right = BigIntegerCalculator.Remainder(leftBits, rightBits[0]);
			return BigIntegerCalculator.Gcd(rightBits[0], right);
		}
		if (rightBits.Length == 2)
		{
			uint[] array = BigIntegerCalculator.Remainder(leftBits, rightBits);
			ulong left = ((ulong)rightBits[1] << 32) | rightBits[0];
			ulong right2 = ((ulong)array[1] << 32) | array[0];
			return BigIntegerCalculator.Gcd(left, right2);
		}
		return new BigInteger(BigIntegerCalculator.Gcd(leftBits, rightBits), negative: false);
	}

	public static BigInteger Max(BigInteger left, BigInteger right)
	{
		if (left.CompareTo(right) < 0)
		{
			return right;
		}
		return left;
	}

	public static BigInteger Min(BigInteger left, BigInteger right)
	{
		if (left.CompareTo(right) <= 0)
		{
			return left;
		}
		return right;
	}

	public static BigInteger ModPow(BigInteger value, BigInteger exponent, BigInteger modulus)
	{
		if (exponent.Sign < 0)
		{
			throw new ArgumentOutOfRangeException("exponent", "The number must be greater than or equal to zero.");
		}
		bool flag = value._bits == null;
		bool flag2 = exponent._bits == null;
		if (modulus._bits == null)
		{
			uint num = ((flag && flag2) ? BigIntegerCalculator.Pow(NumericsHelpers.Abs(value._sign), NumericsHelpers.Abs(exponent._sign), NumericsHelpers.Abs(modulus._sign)) : (flag ? BigIntegerCalculator.Pow(NumericsHelpers.Abs(value._sign), exponent._bits, NumericsHelpers.Abs(modulus._sign)) : (flag2 ? BigIntegerCalculator.Pow(value._bits, NumericsHelpers.Abs(exponent._sign), NumericsHelpers.Abs(modulus._sign)) : BigIntegerCalculator.Pow(value._bits, exponent._bits, NumericsHelpers.Abs(modulus._sign)))));
			return (value._sign < 0 && !exponent.IsEven) ? (-1 * num) : num;
		}
		return new BigInteger((flag && flag2) ? BigIntegerCalculator.Pow(NumericsHelpers.Abs(value._sign), NumericsHelpers.Abs(exponent._sign), modulus._bits) : (flag ? BigIntegerCalculator.Pow(NumericsHelpers.Abs(value._sign), exponent._bits, modulus._bits) : (flag2 ? BigIntegerCalculator.Pow(value._bits, NumericsHelpers.Abs(exponent._sign), modulus._bits) : BigIntegerCalculator.Pow(value._bits, exponent._bits, modulus._bits))), value._sign < 0 && !exponent.IsEven);
	}

	public static BigInteger Pow(BigInteger value, int exponent)
	{
		if (exponent < 0)
		{
			throw new ArgumentOutOfRangeException("exponent", "The number must be greater than or equal to zero.");
		}
		switch (exponent)
		{
		case 0:
			return s_bnOneInt;
		case 1:
			return value;
		default:
		{
			bool flag = value._bits == null;
			if (flag)
			{
				if (value._sign == 1)
				{
					return value;
				}
				if (value._sign == -1)
				{
					if ((exponent & 1) == 0)
					{
						return s_bnOneInt;
					}
					return value;
				}
				if (value._sign == 0)
				{
					return value;
				}
			}
			return new BigInteger(flag ? BigIntegerCalculator.Pow(NumericsHelpers.Abs(value._sign), NumericsHelpers.Abs(exponent)) : BigIntegerCalculator.Pow(value._bits, NumericsHelpers.Abs(exponent)), value._sign < 0 && (exponent & 1) != 0);
		}
		}
	}

	public override int GetHashCode()
	{
		if (_bits == null)
		{
			return _sign;
		}
		int num = _sign;
		int num2 = _bits.Length;
		while (--num2 >= 0)
		{
			num = NumericsHelpers.CombineHash(num, (int)_bits[num2]);
		}
		return num;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is BigInteger))
		{
			return false;
		}
		return Equals((BigInteger)obj);
	}

	public bool Equals(long other)
	{
		if (_bits == null)
		{
			return _sign == other;
		}
		int num;
		if ((_sign ^ other) < 0 || (num = _bits.Length) > 2)
		{
			return false;
		}
		ulong num2 = (ulong)((other < 0) ? (-other) : other);
		if (num == 1)
		{
			return _bits[0] == num2;
		}
		return NumericsHelpers.MakeUlong(_bits[1], _bits[0]) == num2;
	}

	[CLSCompliant(false)]
	public bool Equals(ulong other)
	{
		if (_sign < 0)
		{
			return false;
		}
		if (_bits == null)
		{
			return (ulong)_sign == other;
		}
		int num = _bits.Length;
		if (num > 2)
		{
			return false;
		}
		if (num == 1)
		{
			return _bits[0] == other;
		}
		return NumericsHelpers.MakeUlong(_bits[1], _bits[0]) == other;
	}

	public bool Equals(BigInteger other)
	{
		if (_sign != other._sign)
		{
			return false;
		}
		if (_bits == other._bits)
		{
			return true;
		}
		if (_bits == null || other._bits == null)
		{
			return false;
		}
		int num = _bits.Length;
		if (num != other._bits.Length)
		{
			return false;
		}
		return GetDiffLength(_bits, other._bits, num) == 0;
	}

	public int CompareTo(long other)
	{
		if (_bits == null)
		{
			return ((long)_sign).CompareTo(other);
		}
		int num;
		if ((_sign ^ other) < 0 || (num = _bits.Length) > 2)
		{
			return _sign;
		}
		ulong value = (ulong)((other < 0) ? (-other) : other);
		ulong num2 = ((num == 2) ? NumericsHelpers.MakeUlong(_bits[1], _bits[0]) : _bits[0]);
		return _sign * num2.CompareTo(value);
	}

	[CLSCompliant(false)]
	public int CompareTo(ulong other)
	{
		if (_sign < 0)
		{
			return -1;
		}
		if (_bits == null)
		{
			return ((ulong)_sign).CompareTo(other);
		}
		int num = _bits.Length;
		if (num > 2)
		{
			return 1;
		}
		return ((num == 2) ? NumericsHelpers.MakeUlong(_bits[1], _bits[0]) : _bits[0]).CompareTo(other);
	}

	public int CompareTo(BigInteger other)
	{
		if ((_sign ^ other._sign) < 0)
		{
			if (_sign >= 0)
			{
				return 1;
			}
			return -1;
		}
		if (_bits == null)
		{
			if (other._bits == null)
			{
				if (_sign >= other._sign)
				{
					if (_sign <= other._sign)
					{
						return 0;
					}
					return 1;
				}
				return -1;
			}
			return -other._sign;
		}
		int num;
		int num2;
		if (other._bits == null || (num = _bits.Length) > (num2 = other._bits.Length))
		{
			return _sign;
		}
		if (num < num2)
		{
			return -_sign;
		}
		int diffLength = GetDiffLength(_bits, other._bits, num);
		if (diffLength == 0)
		{
			return 0;
		}
		if (_bits[diffLength - 1] >= other._bits[diffLength - 1])
		{
			return _sign;
		}
		return -_sign;
	}

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		if (!(obj is BigInteger))
		{
			throw new ArgumentException("The parameter must be a BigInteger.", "obj");
		}
		return CompareTo((BigInteger)obj);
	}

	public byte[] ToByteArray()
	{
		return ToByteArray(isUnsigned: false, isBigEndian: false);
	}

	public byte[] ToByteArray(bool isUnsigned = false, bool isBigEndian = false)
	{
		int bytesWritten = 0;
		return TryGetBytes(GetBytesMode.AllocateArray, default(Span<byte>), isUnsigned, isBigEndian, ref bytesWritten);
	}

	public bool TryWriteBytes(Span<byte> destination, out int bytesWritten, bool isUnsigned = false, bool isBigEndian = false)
	{
		bytesWritten = 0;
		if (TryGetBytes(GetBytesMode.Span, destination, isUnsigned, isBigEndian, ref bytesWritten) == null)
		{
			bytesWritten = 0;
			return false;
		}
		return true;
	}

	internal bool TryWriteOrCountBytes(Span<byte> destination, out int bytesWritten, bool isUnsigned = false, bool isBigEndian = false)
	{
		bytesWritten = 0;
		return TryGetBytes(GetBytesMode.Span, destination, isUnsigned, isBigEndian, ref bytesWritten) != null;
	}

	public int GetByteCount(bool isUnsigned = false)
	{
		int bytesWritten = 0;
		TryGetBytes(GetBytesMode.Count, default(Span<byte>), isUnsigned, isBigEndian: false, ref bytesWritten);
		return bytesWritten;
	}

	private byte[] TryGetBytes(GetBytesMode mode, Span<byte> destination, bool isUnsigned, bool isBigEndian, ref int bytesWritten)
	{
		int sign = _sign;
		if (sign == 0)
		{
			switch (mode)
			{
			case GetBytesMode.AllocateArray:
				return new byte[1];
			case GetBytesMode.Count:
				bytesWritten = 1;
				return null;
			default:
				bytesWritten = 1;
				if (destination.Length != 0)
				{
					destination[0] = 0;
					return s_success;
				}
				return null;
			}
		}
		if (isUnsigned && sign < 0)
		{
			throw new OverflowException("Negative values do not have an unsigned representation.");
		}
		int i = 0;
		uint[] bits = _bits;
		byte b;
		uint num;
		if (bits == null)
		{
			b = (byte)((sign < 0) ? 255u : 0u);
			num = (uint)sign;
		}
		else if (sign == -1)
		{
			b = byte.MaxValue;
			for (; bits[i] == 0; i++)
			{
			}
			num = ~bits[^1];
			if (bits.Length - 1 == i)
			{
				num++;
			}
		}
		else
		{
			b = 0;
			num = bits[^1];
		}
		byte b2;
		int num2;
		if ((b2 = (byte)(num >> 24)) != b)
		{
			num2 = 3;
		}
		else if ((b2 = (byte)(num >> 16)) != b)
		{
			num2 = 2;
		}
		else if ((b2 = (byte)(num >> 8)) != b)
		{
			num2 = 1;
		}
		else
		{
			b2 = (byte)num;
			num2 = 0;
		}
		bool flag = (b2 & 0x80) != (b & 0x80) && !isUnsigned;
		int num3 = num2 + 1 + (flag ? 1 : 0);
		if (bits != null)
		{
			num3 = checked(4 * (bits.Length - 1) + num3);
		}
		byte[] result;
		switch (mode)
		{
		case GetBytesMode.AllocateArray:
			destination = (result = new byte[num3]);
			break;
		case GetBytesMode.Count:
			bytesWritten = num3;
			return null;
		default:
			bytesWritten = num3;
			if (destination.Length < num3)
			{
				return null;
			}
			result = s_success;
			break;
		}
		int num4 = (isBigEndian ? (num3 - 1) : 0);
		int num5 = ((!isBigEndian) ? 1 : (-1));
		if (bits != null)
		{
			for (int j = 0; j < bits.Length - 1; j++)
			{
				uint num6 = bits[j];
				if (sign == -1)
				{
					num6 = ~num6;
					if (j <= i)
					{
						num6++;
					}
				}
				destination[num4] = (byte)num6;
				num4 += num5;
				destination[num4] = (byte)(num6 >> 8);
				num4 += num5;
				destination[num4] = (byte)(num6 >> 16);
				num4 += num5;
				destination[num4] = (byte)(num6 >> 24);
				num4 += num5;
			}
		}
		destination[num4] = (byte)num;
		if (num2 != 0)
		{
			num4 += num5;
			destination[num4] = (byte)(num >> 8);
			if (num2 != 1)
			{
				num4 += num5;
				destination[num4] = (byte)(num >> 16);
				if (num2 != 2)
				{
					num4 += num5;
					destination[num4] = (byte)(num >> 24);
				}
			}
		}
		if (flag)
		{
			num4 += num5;
			destination[num4] = b;
		}
		return result;
	}

	private uint[] ToUInt32Array()
	{
		if (_bits == null && _sign == 0)
		{
			return new uint[1];
		}
		uint[] array;
		uint num;
		if (_bits == null)
		{
			array = new uint[1] { (uint)_sign };
			num = ((_sign < 0) ? uint.MaxValue : 0u);
		}
		else if (_sign == -1)
		{
			array = (uint[])_bits.Clone();
			NumericsHelpers.DangerousMakeTwosComplement(array);
			num = uint.MaxValue;
		}
		else
		{
			array = _bits;
			num = 0u;
		}
		int num2 = array.Length - 1;
		while (num2 > 0 && array[num2] == num)
		{
			num2--;
		}
		bool flag = (array[num2] & 0x80000000u) != (num & 0x80000000u);
		uint[] array2 = new uint[num2 + 1 + (flag ? 1 : 0)];
		Array.Copy(array, 0, array2, 0, num2 + 1);
		if (flag)
		{
			array2[^1] = num;
		}
		return array2;
	}

	public override string ToString()
	{
		return BigNumber.FormatBigInteger(this, null, NumberFormatInfo.CurrentInfo);
	}

	public string ToString(IFormatProvider provider)
	{
		return BigNumber.FormatBigInteger(this, null, NumberFormatInfo.GetInstance(provider));
	}

	public string ToString(string format)
	{
		return BigNumber.FormatBigInteger(this, format, NumberFormatInfo.CurrentInfo);
	}

	public string ToString(string format, IFormatProvider provider)
	{
		return BigNumber.FormatBigInteger(this, format, NumberFormatInfo.GetInstance(provider));
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider provider = null)
	{
		return BigNumber.TryFormatBigInteger(this, format, NumberFormatInfo.GetInstance(provider), destination, out charsWritten);
	}

	private static BigInteger Add(uint[] leftBits, int leftSign, uint[] rightBits, int rightSign)
	{
		bool flag = leftBits == null;
		bool flag2 = rightBits == null;
		if (flag && flag2)
		{
			return (long)leftSign + (long)rightSign;
		}
		if (flag)
		{
			return new BigInteger(BigIntegerCalculator.Add(rightBits, NumericsHelpers.Abs(leftSign)), leftSign < 0);
		}
		if (flag2)
		{
			return new BigInteger(BigIntegerCalculator.Add(leftBits, NumericsHelpers.Abs(rightSign)), leftSign < 0);
		}
		if (leftBits.Length < rightBits.Length)
		{
			return new BigInteger(BigIntegerCalculator.Add(rightBits, leftBits), leftSign < 0);
		}
		return new BigInteger(BigIntegerCalculator.Add(leftBits, rightBits), leftSign < 0);
	}

	public static BigInteger operator -(BigInteger left, BigInteger right)
	{
		if (left._sign < 0 != right._sign < 0)
		{
			return Add(left._bits, left._sign, right._bits, -1 * right._sign);
		}
		return Subtract(left._bits, left._sign, right._bits, right._sign);
	}

	private static BigInteger Subtract(uint[] leftBits, int leftSign, uint[] rightBits, int rightSign)
	{
		bool flag = leftBits == null;
		bool flag2 = rightBits == null;
		if (flag && flag2)
		{
			return (long)leftSign - (long)rightSign;
		}
		if (flag)
		{
			return new BigInteger(BigIntegerCalculator.Subtract(rightBits, NumericsHelpers.Abs(leftSign)), leftSign >= 0);
		}
		if (flag2)
		{
			return new BigInteger(BigIntegerCalculator.Subtract(leftBits, NumericsHelpers.Abs(rightSign)), leftSign < 0);
		}
		if (BigIntegerCalculator.Compare(leftBits, rightBits) < 0)
		{
			return new BigInteger(BigIntegerCalculator.Subtract(rightBits, leftBits), leftSign >= 0);
		}
		return new BigInteger(BigIntegerCalculator.Subtract(leftBits, rightBits), leftSign < 0);
	}

	public static implicit operator BigInteger(byte value)
	{
		return new BigInteger(value);
	}

	[CLSCompliant(false)]
	public static implicit operator BigInteger(sbyte value)
	{
		return new BigInteger(value);
	}

	public static implicit operator BigInteger(short value)
	{
		return new BigInteger(value);
	}

	[CLSCompliant(false)]
	public static implicit operator BigInteger(ushort value)
	{
		return new BigInteger(value);
	}

	public static implicit operator BigInteger(int value)
	{
		return new BigInteger(value);
	}

	[CLSCompliant(false)]
	public static implicit operator BigInteger(uint value)
	{
		return new BigInteger(value);
	}

	public static implicit operator BigInteger(long value)
	{
		return new BigInteger(value);
	}

	[CLSCompliant(false)]
	public static implicit operator BigInteger(ulong value)
	{
		return new BigInteger(value);
	}

	public static explicit operator BigInteger(float value)
	{
		return new BigInteger(value);
	}

	public static explicit operator BigInteger(double value)
	{
		return new BigInteger(value);
	}

	public static explicit operator BigInteger(decimal value)
	{
		return new BigInteger(value);
	}

	public static explicit operator byte(BigInteger value)
	{
		return checked((byte)(int)value);
	}

	[CLSCompliant(false)]
	public static explicit operator sbyte(BigInteger value)
	{
		return checked((sbyte)(int)value);
	}

	public static explicit operator short(BigInteger value)
	{
		return checked((short)(int)value);
	}

	[CLSCompliant(false)]
	public static explicit operator ushort(BigInteger value)
	{
		return checked((ushort)(int)value);
	}

	public static explicit operator int(BigInteger value)
	{
		if (value._bits == null)
		{
			return value._sign;
		}
		if (value._bits.Length > 1)
		{
			throw new OverflowException("Value was either too large or too small for an Int32.");
		}
		if (value._sign > 0)
		{
			return checked((int)value._bits[0]);
		}
		if (value._bits[0] > 2147483648u)
		{
			throw new OverflowException("Value was either too large or too small for an Int32.");
		}
		return (int)(0 - value._bits[0]);
	}

	[CLSCompliant(false)]
	public static explicit operator uint(BigInteger value)
	{
		if (value._bits == null)
		{
			return checked((uint)value._sign);
		}
		if (value._bits.Length > 1 || value._sign < 0)
		{
			throw new OverflowException("Value was either too large or too small for a UInt32.");
		}
		return value._bits[0];
	}

	public static explicit operator long(BigInteger value)
	{
		if (value._bits == null)
		{
			return value._sign;
		}
		int num = value._bits.Length;
		if (num > 2)
		{
			throw new OverflowException("Value was either too large or too small for an Int64.");
		}
		ulong num2 = ((num <= 1) ? value._bits[0] : NumericsHelpers.MakeUlong(value._bits[1], value._bits[0]));
		long num3 = (long)((value._sign > 0) ? num2 : (0L - num2));
		if ((num3 > 0 && value._sign > 0) || (num3 < 0 && value._sign < 0))
		{
			return num3;
		}
		throw new OverflowException("Value was either too large or too small for an Int64.");
	}

	[CLSCompliant(false)]
	public static explicit operator ulong(BigInteger value)
	{
		if (value._bits == null)
		{
			return checked((ulong)value._sign);
		}
		int num = value._bits.Length;
		if (num > 2 || value._sign < 0)
		{
			throw new OverflowException("Value was either too large or too small for a UInt64.");
		}
		if (num > 1)
		{
			return NumericsHelpers.MakeUlong(value._bits[1], value._bits[0]);
		}
		return value._bits[0];
	}

	public static explicit operator float(BigInteger value)
	{
		return (float)(double)value;
	}

	public static explicit operator double(BigInteger value)
	{
		int sign = value._sign;
		uint[] bits = value._bits;
		if (bits == null)
		{
			return sign;
		}
		int num = bits.Length;
		if (num > 32)
		{
			if (sign == 1)
			{
				return double.PositiveInfinity;
			}
			return double.NegativeInfinity;
		}
		long num2 = bits[num - 1];
		ulong num3 = ((num > 1) ? bits[num - 2] : 0u);
		ulong num4 = ((num > 2) ? bits[num - 3] : 0u);
		int num5 = NumericsHelpers.CbitHighZero((uint)num2);
		int exp = (num - 2) * 32 - num5;
		ulong man = (ulong)(num2 << 32 + num5) | (num3 << num5) | (num4 >> 32 - num5);
		return NumericsHelpers.GetDoubleFromParts(sign, exp, man);
	}

	public static explicit operator decimal(BigInteger value)
	{
		if (value._bits == null)
		{
			return value._sign;
		}
		int num = value._bits.Length;
		if (num > 3)
		{
			throw new OverflowException("Value was either too large or too small for a Decimal.");
		}
		int lo = 0;
		int mid = 0;
		int hi = 0;
		if (num > 2)
		{
			hi = (int)value._bits[2];
		}
		if (num > 1)
		{
			mid = (int)value._bits[1];
		}
		if (num > 0)
		{
			lo = (int)value._bits[0];
		}
		return new decimal(lo, mid, hi, value._sign < 0, 0);
	}

	public static BigInteger operator &(BigInteger left, BigInteger right)
	{
		if (left.IsZero || right.IsZero)
		{
			return Zero;
		}
		if (left._bits == null && right._bits == null)
		{
			return left._sign & right._sign;
		}
		uint[] array = left.ToUInt32Array();
		uint[] array2 = right.ToUInt32Array();
		uint[] array3 = new uint[Math.Max(array.Length, array2.Length)];
		uint num = ((left._sign < 0) ? uint.MaxValue : 0u);
		uint num2 = ((right._sign < 0) ? uint.MaxValue : 0u);
		for (int i = 0; i < array3.Length; i++)
		{
			uint num3 = ((i < array.Length) ? array[i] : num);
			uint num4 = ((i < array2.Length) ? array2[i] : num2);
			array3[i] = num3 & num4;
		}
		return new BigInteger(array3);
	}

	public static BigInteger operator |(BigInteger left, BigInteger right)
	{
		if (left.IsZero)
		{
			return right;
		}
		if (right.IsZero)
		{
			return left;
		}
		if (left._bits == null && right._bits == null)
		{
			return left._sign | right._sign;
		}
		uint[] array = left.ToUInt32Array();
		uint[] array2 = right.ToUInt32Array();
		uint[] array3 = new uint[Math.Max(array.Length, array2.Length)];
		uint num = ((left._sign < 0) ? uint.MaxValue : 0u);
		uint num2 = ((right._sign < 0) ? uint.MaxValue : 0u);
		for (int i = 0; i < array3.Length; i++)
		{
			uint num3 = ((i < array.Length) ? array[i] : num);
			uint num4 = ((i < array2.Length) ? array2[i] : num2);
			array3[i] = num3 | num4;
		}
		return new BigInteger(array3);
	}

	public static BigInteger operator ^(BigInteger left, BigInteger right)
	{
		if (left._bits == null && right._bits == null)
		{
			return left._sign ^ right._sign;
		}
		uint[] array = left.ToUInt32Array();
		uint[] array2 = right.ToUInt32Array();
		uint[] array3 = new uint[Math.Max(array.Length, array2.Length)];
		uint num = ((left._sign < 0) ? uint.MaxValue : 0u);
		uint num2 = ((right._sign < 0) ? uint.MaxValue : 0u);
		for (int i = 0; i < array3.Length; i++)
		{
			uint num3 = ((i < array.Length) ? array[i] : num);
			uint num4 = ((i < array2.Length) ? array2[i] : num2);
			array3[i] = num3 ^ num4;
		}
		return new BigInteger(array3);
	}

	public static BigInteger operator <<(BigInteger value, int shift)
	{
		if (shift == 0)
		{
			return value;
		}
		if (shift == int.MinValue)
		{
			return value >> int.MaxValue >> 1;
		}
		if (shift < 0)
		{
			return value >> -shift;
		}
		int num = shift / 32;
		int num2 = shift - num * 32;
		uint[] xd;
		int xl;
		bool partsForBitManipulation = GetPartsForBitManipulation(ref value, out xd, out xl);
		uint[] array = new uint[xl + num + 1];
		if (num2 == 0)
		{
			for (int i = 0; i < xl; i++)
			{
				array[i + num] = xd[i];
			}
		}
		else
		{
			int num3 = 32 - num2;
			uint num4 = 0u;
			int j;
			for (j = 0; j < xl; j++)
			{
				uint num5 = xd[j];
				array[j + num] = (num5 << num2) | num4;
				num4 = num5 >> num3;
			}
			array[j + num] = num4;
		}
		return new BigInteger(array, partsForBitManipulation);
	}

	public static BigInteger operator >>(BigInteger value, int shift)
	{
		if (shift == 0)
		{
			return value;
		}
		if (shift == int.MinValue)
		{
			return value << int.MaxValue << 1;
		}
		if (shift < 0)
		{
			return value << -shift;
		}
		int num = shift / 32;
		int num2 = shift - num * 32;
		uint[] xd;
		int xl;
		bool partsForBitManipulation = GetPartsForBitManipulation(ref value, out xd, out xl);
		if (partsForBitManipulation)
		{
			if (shift >= 32 * xl)
			{
				return MinusOne;
			}
			uint[] array = new uint[xl];
			Array.Copy(xd, 0, array, 0, xl);
			xd = array;
			NumericsHelpers.DangerousMakeTwosComplement(xd);
		}
		int num3 = xl - num;
		if (num3 < 0)
		{
			num3 = 0;
		}
		uint[] array2 = new uint[num3];
		if (num2 == 0)
		{
			for (int num4 = xl - 1; num4 >= num; num4--)
			{
				array2[num4 - num] = xd[num4];
			}
		}
		else
		{
			int num5 = 32 - num2;
			uint num6 = 0u;
			for (int num7 = xl - 1; num7 >= num; num7--)
			{
				uint num8 = xd[num7];
				if (partsForBitManipulation && num7 == xl - 1)
				{
					array2[num7 - num] = (num8 >> num2) | (uint)(-1 << num5);
				}
				else
				{
					array2[num7 - num] = (num8 >> num2) | num6;
				}
				num6 = num8 << num5;
			}
		}
		if (partsForBitManipulation)
		{
			NumericsHelpers.DangerousMakeTwosComplement(array2);
		}
		return new BigInteger(array2, partsForBitManipulation);
	}

	public static BigInteger operator ~(BigInteger value)
	{
		return -(value + One);
	}

	public static BigInteger operator -(BigInteger value)
	{
		return new BigInteger(-value._sign, value._bits);
	}

	public static BigInteger operator +(BigInteger value)
	{
		return value;
	}

	public static BigInteger operator ++(BigInteger value)
	{
		return value + One;
	}

	public static BigInteger operator --(BigInteger value)
	{
		return value - One;
	}

	public static BigInteger operator +(BigInteger left, BigInteger right)
	{
		if (left._sign < 0 != right._sign < 0)
		{
			return Subtract(left._bits, left._sign, right._bits, -1 * right._sign);
		}
		return Add(left._bits, left._sign, right._bits, right._sign);
	}

	public static BigInteger operator *(BigInteger left, BigInteger right)
	{
		bool flag = left._bits == null;
		bool flag2 = right._bits == null;
		if (flag && flag2)
		{
			return (long)left._sign * (long)right._sign;
		}
		if (flag)
		{
			return new BigInteger(BigIntegerCalculator.Multiply(right._bits, NumericsHelpers.Abs(left._sign)), (left._sign < 0) ^ (right._sign < 0));
		}
		if (flag2)
		{
			return new BigInteger(BigIntegerCalculator.Multiply(left._bits, NumericsHelpers.Abs(right._sign)), (left._sign < 0) ^ (right._sign < 0));
		}
		if (left._bits == right._bits)
		{
			return new BigInteger(BigIntegerCalculator.Square(left._bits), (left._sign < 0) ^ (right._sign < 0));
		}
		if (left._bits.Length < right._bits.Length)
		{
			return new BigInteger(BigIntegerCalculator.Multiply(right._bits, left._bits), (left._sign < 0) ^ (right._sign < 0));
		}
		return new BigInteger(BigIntegerCalculator.Multiply(left._bits, right._bits), (left._sign < 0) ^ (right._sign < 0));
	}

	public static BigInteger operator /(BigInteger dividend, BigInteger divisor)
	{
		bool flag = dividend._bits == null;
		bool flag2 = divisor._bits == null;
		if (flag && flag2)
		{
			return dividend._sign / divisor._sign;
		}
		if (flag)
		{
			return s_bnZeroInt;
		}
		if (flag2)
		{
			return new BigInteger(BigIntegerCalculator.Divide(dividend._bits, NumericsHelpers.Abs(divisor._sign)), (dividend._sign < 0) ^ (divisor._sign < 0));
		}
		if (dividend._bits.Length < divisor._bits.Length)
		{
			return s_bnZeroInt;
		}
		return new BigInteger(BigIntegerCalculator.Divide(dividend._bits, divisor._bits), (dividend._sign < 0) ^ (divisor._sign < 0));
	}

	public static BigInteger operator %(BigInteger dividend, BigInteger divisor)
	{
		bool flag = dividend._bits == null;
		bool flag2 = divisor._bits == null;
		if (flag && flag2)
		{
			return dividend._sign % divisor._sign;
		}
		if (flag)
		{
			return dividend;
		}
		if (flag2)
		{
			uint num = BigIntegerCalculator.Remainder(dividend._bits, NumericsHelpers.Abs(divisor._sign));
			return (dividend._sign < 0) ? (-1 * num) : num;
		}
		if (dividend._bits.Length < divisor._bits.Length)
		{
			return dividend;
		}
		return new BigInteger(BigIntegerCalculator.Remainder(dividend._bits, divisor._bits), dividend._sign < 0);
	}

	public static bool operator <(BigInteger left, BigInteger right)
	{
		return left.CompareTo(right) < 0;
	}

	public static bool operator <=(BigInteger left, BigInteger right)
	{
		return left.CompareTo(right) <= 0;
	}

	public static bool operator >(BigInteger left, BigInteger right)
	{
		return left.CompareTo(right) > 0;
	}

	public static bool operator >=(BigInteger left, BigInteger right)
	{
		return left.CompareTo(right) >= 0;
	}

	public static bool operator ==(BigInteger left, BigInteger right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(BigInteger left, BigInteger right)
	{
		return !left.Equals(right);
	}

	public static bool operator <(BigInteger left, long right)
	{
		return left.CompareTo(right) < 0;
	}

	public static bool operator <=(BigInteger left, long right)
	{
		return left.CompareTo(right) <= 0;
	}

	public static bool operator >(BigInteger left, long right)
	{
		return left.CompareTo(right) > 0;
	}

	public static bool operator >=(BigInteger left, long right)
	{
		return left.CompareTo(right) >= 0;
	}

	public static bool operator ==(BigInteger left, long right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(BigInteger left, long right)
	{
		return !left.Equals(right);
	}

	public static bool operator <(long left, BigInteger right)
	{
		return right.CompareTo(left) > 0;
	}

	public static bool operator <=(long left, BigInteger right)
	{
		return right.CompareTo(left) >= 0;
	}

	public static bool operator >(long left, BigInteger right)
	{
		return right.CompareTo(left) < 0;
	}

	public static bool operator >=(long left, BigInteger right)
	{
		return right.CompareTo(left) <= 0;
	}

	public static bool operator ==(long left, BigInteger right)
	{
		return right.Equals(left);
	}

	public static bool operator !=(long left, BigInteger right)
	{
		return !right.Equals(left);
	}

	[CLSCompliant(false)]
	public static bool operator <(BigInteger left, ulong right)
	{
		return left.CompareTo(right) < 0;
	}

	[CLSCompliant(false)]
	public static bool operator <=(BigInteger left, ulong right)
	{
		return left.CompareTo(right) <= 0;
	}

	[CLSCompliant(false)]
	public static bool operator >(BigInteger left, ulong right)
	{
		return left.CompareTo(right) > 0;
	}

	[CLSCompliant(false)]
	public static bool operator >=(BigInteger left, ulong right)
	{
		return left.CompareTo(right) >= 0;
	}

	[CLSCompliant(false)]
	public static bool operator ==(BigInteger left, ulong right)
	{
		return left.Equals(right);
	}

	[CLSCompliant(false)]
	public static bool operator !=(BigInteger left, ulong right)
	{
		return !left.Equals(right);
	}

	[CLSCompliant(false)]
	public static bool operator <(ulong left, BigInteger right)
	{
		return right.CompareTo(left) > 0;
	}

	[CLSCompliant(false)]
	public static bool operator <=(ulong left, BigInteger right)
	{
		return right.CompareTo(left) >= 0;
	}

	[CLSCompliant(false)]
	public static bool operator >(ulong left, BigInteger right)
	{
		return right.CompareTo(left) < 0;
	}

	[CLSCompliant(false)]
	public static bool operator >=(ulong left, BigInteger right)
	{
		return right.CompareTo(left) <= 0;
	}

	[CLSCompliant(false)]
	public static bool operator ==(ulong left, BigInteger right)
	{
		return right.Equals(left);
	}

	[CLSCompliant(false)]
	public static bool operator !=(ulong left, BigInteger right)
	{
		return !right.Equals(left);
	}

	private static bool GetPartsForBitManipulation(ref BigInteger x, out uint[] xd, out int xl)
	{
		if (x._bits == null)
		{
			if (x._sign < 0)
			{
				xd = new uint[1] { (uint)(-x._sign) };
			}
			else
			{
				xd = new uint[1] { (uint)x._sign };
			}
		}
		else
		{
			xd = x._bits;
		}
		xl = ((x._bits == null) ? 1 : x._bits.Length);
		return x._sign < 0;
	}

	internal static int GetDiffLength(uint[] rgu1, uint[] rgu2, int cu)
	{
		int num = cu;
		while (--num >= 0)
		{
			if (rgu1[num] != rgu2[num])
			{
				return num + 1;
			}
		}
		return 0;
	}

	[Conditional("DEBUG")]
	private void AssertValid()
	{
		_ = _bits;
	}
}
