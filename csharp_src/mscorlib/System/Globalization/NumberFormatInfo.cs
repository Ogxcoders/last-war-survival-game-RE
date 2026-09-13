using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace System.Globalization;

[Serializable]
[ComVisible(true)]
public sealed class NumberFormatInfo : ICloneable, IFormatProvider
{
	private static volatile NumberFormatInfo invariantInfo;

	internal int[] numberGroupSizes = new int[1] { 3 };

	internal int[] currencyGroupSizes = new int[1] { 3 };

	internal int[] percentGroupSizes = new int[1] { 3 };

	internal string positiveSign = "+";

	internal string negativeSign = "-";

	internal string numberDecimalSeparator = ".";

	internal string numberGroupSeparator = ",";

	internal string currencyGroupSeparator = ",";

	internal string currencyDecimalSeparator = ".";

	internal string currencySymbol = "¤";

	internal string ansiCurrencySymbol;

	internal string nanSymbol = "NaN";

	internal string positiveInfinitySymbol = "Infinity";

	internal string negativeInfinitySymbol = "-Infinity";

	internal string percentDecimalSeparator = ".";

	internal string percentGroupSeparator = ",";

	internal string percentSymbol = "%";

	internal string perMilleSymbol = "‰";

	[OptionalField(VersionAdded = 2)]
	internal string[] nativeDigits = new string[10] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

	[OptionalField(VersionAdded = 1)]
	internal int m_dataItem;

	internal int numberDecimalDigits = 2;

	internal int currencyDecimalDigits = 2;

	internal int currencyPositivePattern;

	internal int currencyNegativePattern;

	internal int numberNegativePattern = 1;

	internal int percentPositivePattern;

	internal int percentNegativePattern;

	internal int percentDecimalDigits = 2;

	[OptionalField(VersionAdded = 2)]
	internal int digitSubstitution = 1;

	internal bool isReadOnly;

	[OptionalField(VersionAdded = 1)]
	internal bool m_useUserOverride;

	[OptionalField(VersionAdded = 2)]
	internal bool m_isInvariant;

	[OptionalField(VersionAdded = 1)]
	internal bool validForParseAsNumber = true;

	[OptionalField(VersionAdded = 1)]
	internal bool validForParseAsCurrency = true;

	private const NumberStyles InvalidNumberStyles = ~(NumberStyles.Any | NumberStyles.AllowHexSpecifier);

	public static NumberFormatInfo InvariantInfo
	{
		get
		{
			if (invariantInfo == null)
			{
				invariantInfo = ReadOnly(new NumberFormatInfo
				{
					m_isInvariant = true
				});
			}
			return invariantInfo;
		}
	}

	public int CurrencyDecimalDigits
	{
		get
		{
			return currencyDecimalDigits;
		}
		set
		{
			if (value < 0 || value > 99)
			{
				throw new ArgumentOutOfRangeException("CurrencyDecimalDigits", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 99));
			}
			VerifyWritable();
			currencyDecimalDigits = value;
		}
	}

	public string CurrencyDecimalSeparator
	{
		get
		{
			return currencyDecimalSeparator;
		}
		set
		{
			VerifyWritable();
			VerifyDecimalSeparator(value, "CurrencyDecimalSeparator");
			currencyDecimalSeparator = value;
		}
	}

	public bool IsReadOnly => isReadOnly;

	public int[] CurrencyGroupSizes
	{
		get
		{
			return (int[])currencyGroupSizes.Clone();
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("CurrencyGroupSizes", Environment.GetResourceString("Object cannot be null."));
			}
			VerifyWritable();
			int[] groupSize = (int[])value.Clone();
			CheckGroupSize("CurrencyGroupSizes", groupSize);
			currencyGroupSizes = groupSize;
		}
	}

	public int[] NumberGroupSizes
	{
		get
		{
			return (int[])numberGroupSizes.Clone();
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("NumberGroupSizes", Environment.GetResourceString("Object cannot be null."));
			}
			VerifyWritable();
			int[] groupSize = (int[])value.Clone();
			CheckGroupSize("NumberGroupSizes", groupSize);
			numberGroupSizes = groupSize;
		}
	}

	public int[] PercentGroupSizes
	{
		get
		{
			return (int[])percentGroupSizes.Clone();
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("PercentGroupSizes", Environment.GetResourceString("Object cannot be null."));
			}
			VerifyWritable();
			int[] groupSize = (int[])value.Clone();
			CheckGroupSize("PercentGroupSizes", groupSize);
			percentGroupSizes = groupSize;
		}
	}

	public string CurrencyGroupSeparator
	{
		get
		{
			return currencyGroupSeparator;
		}
		set
		{
			VerifyWritable();
			VerifyGroupSeparator(value, "CurrencyGroupSeparator");
			currencyGroupSeparator = value;
		}
	}

	public string CurrencySymbol
	{
		get
		{
			return currencySymbol;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("CurrencySymbol", Environment.GetResourceString("String reference not set to an instance of a String."));
			}
			VerifyWritable();
			currencySymbol = value;
		}
	}

	public static NumberFormatInfo CurrentInfo
	{
		get
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			if (!currentCulture.m_isInherited)
			{
				NumberFormatInfo numInfo = currentCulture.numInfo;
				if (numInfo != null)
				{
					return numInfo;
				}
			}
			return (NumberFormatInfo)currentCulture.GetFormat(typeof(NumberFormatInfo));
		}
	}

	public string NaNSymbol
	{
		get
		{
			return nanSymbol;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("NaNSymbol", Environment.GetResourceString("String reference not set to an instance of a String."));
			}
			VerifyWritable();
			nanSymbol = value;
		}
	}

	public int CurrencyNegativePattern
	{
		get
		{
			return currencyNegativePattern;
		}
		set
		{
			if (value < 0 || value > 15)
			{
				throw new ArgumentOutOfRangeException("CurrencyNegativePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 15));
			}
			VerifyWritable();
			currencyNegativePattern = value;
		}
	}

	public int NumberNegativePattern
	{
		get
		{
			return numberNegativePattern;
		}
		set
		{
			if (value < 0 || value > 4)
			{
				throw new ArgumentOutOfRangeException("NumberNegativePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 4));
			}
			VerifyWritable();
			numberNegativePattern = value;
		}
	}

	public int PercentPositivePattern
	{
		get
		{
			return percentPositivePattern;
		}
		set
		{
			if (value < 0 || value > 3)
			{
				throw new ArgumentOutOfRangeException("PercentPositivePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 3));
			}
			VerifyWritable();
			percentPositivePattern = value;
		}
	}

	public int PercentNegativePattern
	{
		get
		{
			return percentNegativePattern;
		}
		set
		{
			if (value < 0 || value > 11)
			{
				throw new ArgumentOutOfRangeException("PercentNegativePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 11));
			}
			VerifyWritable();
			percentNegativePattern = value;
		}
	}

	public string NegativeInfinitySymbol
	{
		get
		{
			return negativeInfinitySymbol;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("NegativeInfinitySymbol", Environment.GetResourceString("String reference not set to an instance of a String."));
			}
			VerifyWritable();
			negativeInfinitySymbol = value;
		}
	}

	public string NegativeSign
	{
		get
		{
			return negativeSign;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("NegativeSign", Environment.GetResourceString("String reference not set to an instance of a String."));
			}
			VerifyWritable();
			negativeSign = value;
		}
	}

	public int NumberDecimalDigits
	{
		get
		{
			return numberDecimalDigits;
		}
		set
		{
			if (value < 0 || value > 99)
			{
				throw new ArgumentOutOfRangeException("NumberDecimalDigits", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 99));
			}
			VerifyWritable();
			numberDecimalDigits = value;
		}
	}

	public string NumberDecimalSeparator
	{
		get
		{
			return numberDecimalSeparator;
		}
		set
		{
			VerifyWritable();
			VerifyDecimalSeparator(value, "NumberDecimalSeparator");
			numberDecimalSeparator = value;
		}
	}

	public string NumberGroupSeparator
	{
		get
		{
			return numberGroupSeparator;
		}
		set
		{
			VerifyWritable();
			VerifyGroupSeparator(value, "NumberGroupSeparator");
			numberGroupSeparator = value;
		}
	}

	public int CurrencyPositivePattern
	{
		get
		{
			return currencyPositivePattern;
		}
		set
		{
			if (value < 0 || value > 3)
			{
				throw new ArgumentOutOfRangeException("CurrencyPositivePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 3));
			}
			VerifyWritable();
			currencyPositivePattern = value;
		}
	}

	public string PositiveInfinitySymbol
	{
		get
		{
			return positiveInfinitySymbol;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("PositiveInfinitySymbol", Environment.GetResourceString("String reference not set to an instance of a String."));
			}
			VerifyWritable();
			positiveInfinitySymbol = value;
		}
	}

	public string PositiveSign
	{
		get
		{
			return positiveSign;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("PositiveSign", Environment.GetResourceString("String reference not set to an instance of a String."));
			}
			VerifyWritable();
			positiveSign = value;
		}
	}

	public int PercentDecimalDigits
	{
		get
		{
			return percentDecimalDigits;
		}
		set
		{
			if (value < 0 || value > 99)
			{
				throw new ArgumentOutOfRangeException("PercentDecimalDigits", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 99));
			}
			VerifyWritable();
			percentDecimalDigits = value;
		}
	}

	public string PercentDecimalSeparator
	{
		get
		{
			return percentDecimalSeparator;
		}
		set
		{
			VerifyWritable();
			VerifyDecimalSeparator(value, "PercentDecimalSeparator");
			percentDecimalSeparator = value;
		}
	}

	public string PercentGroupSeparator
	{
		get
		{
			return percentGroupSeparator;
		}
		set
		{
			VerifyWritable();
			VerifyGroupSeparator(value, "PercentGroupSeparator");
			percentGroupSeparator = value;
		}
	}

	public string PercentSymbol
	{
		get
		{
			return percentSymbol;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("PercentSymbol", Environment.GetResourceString("String reference not set to an instance of a String."));
			}
			VerifyWritable();
			percentSymbol = value;
		}
	}

	public string PerMilleSymbol
	{
		get
		{
			return perMilleSymbol;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("PerMilleSymbol", Environment.GetResourceString("String reference not set to an instance of a String."));
			}
			VerifyWritable();
			perMilleSymbol = value;
		}
	}

	[ComVisible(false)]
	public string[] NativeDigits
	{
		get
		{
			return (string[])nativeDigits.Clone();
		}
		set
		{
			VerifyWritable();
			VerifyNativeDigits(value, "NativeDigits");
			nativeDigits = value;
		}
	}

	[ComVisible(false)]
	public DigitShapes DigitSubstitution
	{
		get
		{
			return (DigitShapes)digitSubstitution;
		}
		set
		{
			VerifyWritable();
			VerifyDigitSubstitution(value, "DigitSubstitution");
			digitSubstitution = (int)value;
		}
	}

	public NumberFormatInfo()
		: this(null)
	{
	}

	[OnSerializing]
	private void OnSerializing(StreamingContext ctx)
	{
		if (numberDecimalSeparator != numberGroupSeparator)
		{
			validForParseAsNumber = true;
		}
		else
		{
			validForParseAsNumber = false;
		}
		if (numberDecimalSeparator != numberGroupSeparator && numberDecimalSeparator != currencyGroupSeparator && currencyDecimalSeparator != numberGroupSeparator && currencyDecimalSeparator != currencyGroupSeparator)
		{
			validForParseAsCurrency = true;
		}
		else
		{
			validForParseAsCurrency = false;
		}
	}

	[OnDeserializing]
	private void OnDeserializing(StreamingContext ctx)
	{
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext ctx)
	{
	}

	private static void VerifyDecimalSeparator(string decSep, string propertyName)
	{
		if (decSep == null)
		{
			throw new ArgumentNullException(propertyName, Environment.GetResourceString("String reference not set to an instance of a String."));
		}
		if (decSep.Length == 0)
		{
			throw new ArgumentException(Environment.GetResourceString("Decimal separator cannot be the empty string."));
		}
	}

	private static void VerifyGroupSeparator(string groupSep, string propertyName)
	{
		if (groupSep == null)
		{
			throw new ArgumentNullException(propertyName, Environment.GetResourceString("String reference not set to an instance of a String."));
		}
	}

	private static void VerifyNativeDigits(string[] nativeDig, string propertyName)
	{
		if (nativeDig == null)
		{
			throw new ArgumentNullException(propertyName, Environment.GetResourceString("Array cannot be null."));
		}
		if (nativeDig.Length != 10)
		{
			throw new ArgumentException(Environment.GetResourceString("The NativeDigits array must contain exactly ten members."), propertyName);
		}
		for (int i = 0; i < nativeDig.Length; i++)
		{
			if (nativeDig[i] == null)
			{
				throw new ArgumentNullException(propertyName, Environment.GetResourceString("Found a null value within an array."));
			}
			if (nativeDig[i].Length != 1)
			{
				if (nativeDig[i].Length != 2)
				{
					throw new ArgumentException(Environment.GetResourceString("Each member of the NativeDigits array must be a single text element (one or more UTF16 code points) with a Unicode Nd (Number, Decimal Digit) property indicating it is a digit."), propertyName);
				}
				if (!char.IsSurrogatePair(nativeDig[i][0], nativeDig[i][1]))
				{
					throw new ArgumentException(Environment.GetResourceString("Each member of the NativeDigits array must be a single text element (one or more UTF16 code points) with a Unicode Nd (Number, Decimal Digit) property indicating it is a digit."), propertyName);
				}
			}
			if (CharUnicodeInfo.GetDecimalDigitValue(nativeDig[i], 0) != i && CharUnicodeInfo.GetUnicodeCategory(nativeDig[i], 0) != UnicodeCategory.PrivateUse)
			{
				throw new ArgumentException(Environment.GetResourceString("Each member of the NativeDigits array must be a single text element (one or more UTF16 code points) with a Unicode Nd (Number, Decimal Digit) property indicating it is a digit."), propertyName);
			}
		}
	}

	private static void VerifyDigitSubstitution(DigitShapes digitSub, string propertyName)
	{
		if ((uint)digitSub > 2u)
		{
			throw new ArgumentException(Environment.GetResourceString("The DigitSubstitution property must be of a valid member of the DigitShapes enumeration. Valid entries include Context, NativeNational or None."), propertyName);
		}
	}

	[SecuritySafeCritical]
	internal NumberFormatInfo(CultureData cultureData)
	{
		if (GlobalizationMode.Invariant)
		{
			m_isInvariant = true;
		}
		else if (cultureData != null)
		{
			cultureData.GetNFIValues(this);
			if (cultureData.IsInvariantCulture)
			{
				m_isInvariant = true;
			}
		}
	}

	private void VerifyWritable()
	{
		if (isReadOnly)
		{
			throw new InvalidOperationException(Environment.GetResourceString("Instance is read-only."));
		}
	}

	public static NumberFormatInfo GetInstance(IFormatProvider formatProvider)
	{
		if (formatProvider is CultureInfo { m_isInherited: false } cultureInfo)
		{
			NumberFormatInfo numInfo = cultureInfo.numInfo;
			if (numInfo != null)
			{
				return numInfo;
			}
			return cultureInfo.NumberFormat;
		}
		if (formatProvider is NumberFormatInfo result)
		{
			return result;
		}
		if (formatProvider != null && formatProvider.GetFormat(typeof(NumberFormatInfo)) is NumberFormatInfo result2)
		{
			return result2;
		}
		return CurrentInfo;
	}

	public object Clone()
	{
		NumberFormatInfo obj = (NumberFormatInfo)MemberwiseClone();
		obj.isReadOnly = false;
		return obj;
	}

	internal static void CheckGroupSize(string propName, int[] groupSize)
	{
		for (int i = 0; i < groupSize.Length; i++)
		{
			if (groupSize[i] < 1)
			{
				if (i == groupSize.Length - 1 && groupSize[i] == 0)
				{
					break;
				}
				throw new ArgumentException(Environment.GetResourceString("Every element in the value array should be between one and nine, except for the last element, which can be zero."), propName);
			}
			if (groupSize[i] > 9)
			{
				throw new ArgumentException(Environment.GetResourceString("Every element in the value array should be between one and nine, except for the last element, which can be zero."), propName);
			}
		}
	}

	public object GetFormat(Type formatType)
	{
		if (!(formatType == typeof(NumberFormatInfo)))
		{
			return null;
		}
		return this;
	}

	public static NumberFormatInfo ReadOnly(NumberFormatInfo nfi)
	{
		if (nfi == null)
		{
			throw new ArgumentNullException("nfi");
		}
		if (nfi.IsReadOnly)
		{
			return nfi;
		}
		NumberFormatInfo obj = (NumberFormatInfo)nfi.MemberwiseClone();
		obj.isReadOnly = true;
		return obj;
	}

	internal static void ValidateParseStyleInteger(NumberStyles style)
	{
		if ((style & ~(NumberStyles.Any | NumberStyles.AllowHexSpecifier)) != NumberStyles.None)
		{
			throw new ArgumentException(Environment.GetResourceString("An undefined NumberStyles value is being used."), "style");
		}
		if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None && (style & ~NumberStyles.HexNumber) != NumberStyles.None)
		{
			throw new ArgumentException(Environment.GetResourceString("With the AllowHexSpecifier bit set in the enum bit field, the only other valid bits that can be combined into the enum value must be a subset of those in HexNumber."));
		}
	}

	internal static void ValidateParseStyleFloatingPoint(NumberStyles style)
	{
		if ((style & ~(NumberStyles.Any | NumberStyles.AllowHexSpecifier)) != NumberStyles.None)
		{
			throw new ArgumentException(Environment.GetResourceString("An undefined NumberStyles value is being used."), "style");
		}
		if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
		{
			throw new ArgumentException(Environment.GetResourceString("The number style AllowHexSpecifier is not supported on floating point data types."));
		}
	}
}
