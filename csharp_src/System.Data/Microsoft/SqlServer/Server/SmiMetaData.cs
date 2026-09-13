using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;

namespace Microsoft.SqlServer.Server;

internal class SmiMetaData
{
	private SqlDbType _databaseType;

	private long _maxLength;

	private byte _precision;

	private byte _scale;

	private long _localeId;

	private SqlCompareOptions _compareOptions;

	private Type _clrType;

	private string _udtAssemblyQualifiedName;

	private bool _isMultiValued;

	private IList<SmiExtendedMetaData> _fieldMetaData;

	private SmiMetaDataPropertyCollection _extendedProperties;

	internal const long UnlimitedMaxLengthIndicator = -1L;

	internal const long MaxUnicodeCharacters = 4000L;

	internal const long MaxANSICharacters = 8000L;

	internal const long MaxBinaryLength = 8000L;

	internal const int MinPrecision = 1;

	internal const int MinScale = 0;

	internal const int MaxTimeScale = 7;

	internal static readonly DateTime MaxSmallDateTime = new DateTime(2079, 6, 6, 23, 59, 29, 998);

	internal static readonly DateTime MinSmallDateTime = new DateTime(1899, 12, 31, 23, 59, 29, 999);

	internal static readonly SqlMoney MaxSmallMoney = new SqlMoney(214748.3647m);

	internal static readonly SqlMoney MinSmallMoney = new SqlMoney(-214748.3648m);

	internal const SqlCompareOptions DefaultStringCompareOptions = SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth;

	internal const long MaxNameLength = 128L;

	private static readonly IList<SmiExtendedMetaData> s_emptyFieldList = new List<SmiExtendedMetaData>().AsReadOnly();

	private static byte[] s_maxLenFromPrecision = new byte[38]
	{
		5, 5, 5, 5, 5, 5, 5, 5, 5, 9,
		9, 9, 9, 9, 9, 9, 9, 9, 9, 13,
		13, 13, 13, 13, 13, 13, 13, 13, 17, 17,
		17, 17, 17, 17, 17, 17, 17, 17
	};

	private static byte[] s_maxVarTimeLenOffsetFromScale = new byte[8] { 2, 2, 2, 1, 1, 0, 0, 0 };

	internal static readonly SmiMetaData DefaultBigInt = new SmiMetaData(SqlDbType.BigInt, 8L, 19, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultBinary = new SmiMetaData(SqlDbType.Binary, 1L, 0, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultBit = new SmiMetaData(SqlDbType.Bit, 1L, 1, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultChar_NoCollation = new SmiMetaData(SqlDbType.Char, 1L, 0, 0, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth);

	internal static readonly SmiMetaData DefaultDateTime = new SmiMetaData(SqlDbType.DateTime, 8L, 23, 3, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultDecimal = new SmiMetaData(SqlDbType.Decimal, 9L, 18, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultFloat = new SmiMetaData(SqlDbType.Float, 8L, 53, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultImage = new SmiMetaData(SqlDbType.Image, -1L, 0, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultInt = new SmiMetaData(SqlDbType.Int, 4L, 10, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultMoney = new SmiMetaData(SqlDbType.Money, 8L, 19, 4, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultNChar_NoCollation = new SmiMetaData(SqlDbType.NChar, 1L, 0, 0, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth);

	internal static readonly SmiMetaData DefaultNText_NoCollation = new SmiMetaData(SqlDbType.NText, -1L, 0, 0, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth);

	internal static readonly SmiMetaData DefaultNVarChar_NoCollation = new SmiMetaData(SqlDbType.NVarChar, 4000L, 0, 0, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth);

	internal static readonly SmiMetaData DefaultReal = new SmiMetaData(SqlDbType.Real, 4L, 24, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultUniqueIdentifier = new SmiMetaData(SqlDbType.UniqueIdentifier, 16L, 0, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultSmallDateTime = new SmiMetaData(SqlDbType.SmallDateTime, 4L, 16, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultSmallInt = new SmiMetaData(SqlDbType.SmallInt, 2L, 5, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultSmallMoney = new SmiMetaData(SqlDbType.SmallMoney, 4L, 10, 4, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultText_NoCollation = new SmiMetaData(SqlDbType.Text, -1L, 0, 0, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth);

	internal static readonly SmiMetaData DefaultTimestamp = new SmiMetaData(SqlDbType.Timestamp, 8L, 0, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultTinyInt = new SmiMetaData(SqlDbType.TinyInt, 1L, 3, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultVarBinary = new SmiMetaData(SqlDbType.VarBinary, 8000L, 0, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultVarChar_NoCollation = new SmiMetaData(SqlDbType.VarChar, 8000L, 0, 0, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth);

	internal static readonly SmiMetaData DefaultVariant = new SmiMetaData(SqlDbType.Variant, 8016L, 0, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultXml = new SmiMetaData(SqlDbType.Xml, -1L, 0, 0, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth);

	internal static readonly SmiMetaData DefaultUdt_NoType = new SmiMetaData(SqlDbType.Udt, 0L, 0, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultStructured = new SmiMetaData(SqlDbType.Structured, 0L, 0, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultDate = new SmiMetaData(SqlDbType.Date, 3L, 10, 0, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultTime = new SmiMetaData(SqlDbType.Time, 5L, 0, 7, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultDateTime2 = new SmiMetaData(SqlDbType.DateTime2, 8L, 0, 7, SqlCompareOptions.None);

	internal static readonly SmiMetaData DefaultDateTimeOffset = new SmiMetaData(SqlDbType.DateTimeOffset, 10L, 0, 7, SqlCompareOptions.None);

	private static SmiMetaData[] s_defaultValues = new SmiMetaData[35]
	{
		DefaultBigInt, DefaultBinary, DefaultBit, DefaultChar_NoCollation, DefaultDateTime, DefaultDecimal, DefaultFloat, DefaultImage, DefaultInt, DefaultMoney,
		DefaultNChar_NoCollation, DefaultNText_NoCollation, DefaultNVarChar_NoCollation, DefaultReal, DefaultUniqueIdentifier, DefaultSmallDateTime, DefaultSmallInt, DefaultSmallMoney, DefaultText_NoCollation, DefaultTimestamp,
		DefaultTinyInt, DefaultVarBinary, DefaultVarChar_NoCollation, DefaultVariant, DefaultNVarChar_NoCollation, DefaultXml, DefaultNVarChar_NoCollation, DefaultNVarChar_NoCollation, DefaultNVarChar_NoCollation, DefaultUdt_NoType,
		DefaultStructured, DefaultDate, DefaultTime, DefaultDateTime2, DefaultDateTimeOffset
	};

	private static string[] s_typeNameByDatabaseType = new string[35]
	{
		"bigint",
		"binary",
		"bit",
		"char",
		"datetime",
		"decimal",
		"float",
		"image",
		"int",
		"money",
		"nchar",
		"ntext",
		"nvarchar",
		"real",
		"uniqueidentifier",
		"smalldatetime",
		"smallint",
		"smallmoney",
		"text",
		"timestamp",
		"tinyint",
		"varbinary",
		"varchar",
		"sql_variant",
		null,
		"xml",
		null,
		null,
		null,
		string.Empty,
		string.Empty,
		"date",
		"time",
		"datetime2",
		"datetimeoffset"
	};

	internal static SmiMetaData DefaultChar => new SmiMetaData(DefaultChar_NoCollation.SqlDbType, DefaultChar_NoCollation.MaxLength, DefaultChar_NoCollation.Precision, DefaultChar_NoCollation.Scale, CultureInfo.CurrentCulture.LCID, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, null);

	internal static SmiMetaData DefaultNChar => new SmiMetaData(DefaultNChar_NoCollation.SqlDbType, DefaultNChar_NoCollation.MaxLength, DefaultNChar_NoCollation.Precision, DefaultNChar_NoCollation.Scale, CultureInfo.CurrentCulture.LCID, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, null);

	internal static SmiMetaData DefaultNText => new SmiMetaData(DefaultNText_NoCollation.SqlDbType, DefaultNText_NoCollation.MaxLength, DefaultNText_NoCollation.Precision, DefaultNText_NoCollation.Scale, CultureInfo.CurrentCulture.LCID, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, null);

	internal static SmiMetaData DefaultNVarChar => new SmiMetaData(DefaultNVarChar_NoCollation.SqlDbType, DefaultNVarChar_NoCollation.MaxLength, DefaultNVarChar_NoCollation.Precision, DefaultNVarChar_NoCollation.Scale, CultureInfo.CurrentCulture.LCID, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, null);

	internal static SmiMetaData DefaultText => new SmiMetaData(DefaultText_NoCollation.SqlDbType, DefaultText_NoCollation.MaxLength, DefaultText_NoCollation.Precision, DefaultText_NoCollation.Scale, CultureInfo.CurrentCulture.LCID, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, null);

	internal static SmiMetaData DefaultVarChar => new SmiMetaData(DefaultVarChar_NoCollation.SqlDbType, DefaultVarChar_NoCollation.MaxLength, DefaultVarChar_NoCollation.Precision, DefaultVarChar_NoCollation.Scale, CultureInfo.CurrentCulture.LCID, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, null);

	internal SqlCompareOptions CompareOptions => _compareOptions;

	internal long LocaleId => _localeId;

	internal long MaxLength => _maxLength;

	internal byte Precision => _precision;

	internal byte Scale => _scale;

	internal SqlDbType SqlDbType => _databaseType;

	internal Type Type
	{
		get
		{
			if (null == _clrType && SqlDbType.Udt == _databaseType && _udtAssemblyQualifiedName != null)
			{
				_clrType = Type.GetType(_udtAssemblyQualifiedName, throwOnError: true);
			}
			return _clrType;
		}
	}

	internal Type TypeWithoutThrowing
	{
		get
		{
			if (null == _clrType && SqlDbType.Udt == _databaseType && _udtAssemblyQualifiedName != null)
			{
				_clrType = Type.GetType(_udtAssemblyQualifiedName, throwOnError: false);
			}
			return _clrType;
		}
	}

	internal string TypeName
	{
		get
		{
			string text = null;
			if (SqlDbType.Udt == _databaseType)
			{
				return Type.FullName;
			}
			return s_typeNameByDatabaseType[(int)_databaseType];
		}
	}

	internal string AssemblyQualifiedName
	{
		get
		{
			string result = null;
			if (SqlDbType.Udt == _databaseType)
			{
				if (_udtAssemblyQualifiedName == null && _clrType != null)
				{
					_udtAssemblyQualifiedName = _clrType.AssemblyQualifiedName;
				}
				result = _udtAssemblyQualifiedName;
			}
			return result;
		}
	}

	internal bool IsMultiValued => _isMultiValued;

	internal IList<SmiExtendedMetaData> FieldMetaData => _fieldMetaData;

	internal SmiMetaDataPropertyCollection ExtendedProperties => _extendedProperties;

	internal SmiMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType)
		: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, isMultiValued: false, null, null)
	{
	}

	internal SmiMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, bool isMultiValued, IList<SmiExtendedMetaData> fieldTypes, SmiMetaDataPropertyCollection extendedProperties)
		: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, null, isMultiValued, fieldTypes, extendedProperties)
	{
	}

	internal SmiMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, string udtAssemblyQualifiedName, bool isMultiValued, IList<SmiExtendedMetaData> fieldTypes, SmiMetaDataPropertyCollection extendedProperties)
	{
		SetDefaultsForType(dbType);
		switch (dbType)
		{
		case SqlDbType.Binary:
		case SqlDbType.VarBinary:
			_maxLength = maxLength;
			break;
		case SqlDbType.Char:
		case SqlDbType.NChar:
		case SqlDbType.NVarChar:
		case SqlDbType.VarChar:
			_maxLength = maxLength;
			_localeId = localeId;
			_compareOptions = compareOptions;
			break;
		case SqlDbType.NText:
		case SqlDbType.Text:
			_localeId = localeId;
			_compareOptions = compareOptions;
			break;
		case SqlDbType.Decimal:
			_precision = precision;
			_scale = scale;
			_maxLength = s_maxLenFromPrecision[precision - 1];
			break;
		case SqlDbType.Udt:
			_clrType = userDefinedType;
			if (null != userDefinedType)
			{
				_maxLength = SerializationHelperSql9.GetUdtMaxLength(userDefinedType);
			}
			else
			{
				_maxLength = maxLength;
			}
			_udtAssemblyQualifiedName = udtAssemblyQualifiedName;
			break;
		case SqlDbType.Structured:
			if (fieldTypes != null)
			{
				_fieldMetaData = new List<SmiExtendedMetaData>(fieldTypes).AsReadOnly();
			}
			_isMultiValued = isMultiValued;
			_maxLength = _fieldMetaData.Count;
			break;
		case SqlDbType.Time:
			_scale = scale;
			_maxLength = 5 - s_maxVarTimeLenOffsetFromScale[scale];
			break;
		case SqlDbType.DateTime2:
			_scale = scale;
			_maxLength = 8 - s_maxVarTimeLenOffsetFromScale[scale];
			break;
		case SqlDbType.DateTimeOffset:
			_scale = scale;
			_maxLength = 10 - s_maxVarTimeLenOffsetFromScale[scale];
			break;
		}
		if (extendedProperties != null)
		{
			extendedProperties.SetReadOnly();
			_extendedProperties = extendedProperties;
		}
	}

	internal bool IsValidMaxLengthForCtorGivenType(SqlDbType dbType, long maxLength)
	{
		bool result = true;
		switch (dbType)
		{
		case SqlDbType.Binary:
			result = 0 < maxLength && 8000 >= maxLength;
			break;
		case SqlDbType.VarBinary:
			result = -1 == maxLength || (0 < maxLength && 8000 >= maxLength);
			break;
		case SqlDbType.Char:
			result = 0 < maxLength && 8000 >= maxLength;
			break;
		case SqlDbType.NChar:
			result = 0 < maxLength && 4000 >= maxLength;
			break;
		case SqlDbType.NVarChar:
			result = -1 == maxLength || (0 < maxLength && 4000 >= maxLength);
			break;
		case SqlDbType.VarChar:
			result = -1 == maxLength || (0 < maxLength && 8000 >= maxLength);
			break;
		}
		return result;
	}

	internal static bool IsSupportedDbType(SqlDbType dbType)
	{
		if (SqlDbType.BigInt > dbType || SqlDbType.Xml < dbType)
		{
			if (SqlDbType.Udt <= dbType)
			{
				return SqlDbType.DateTimeOffset >= dbType;
			}
			return false;
		}
		return true;
	}

	internal static SmiMetaData GetDefaultForType(SqlDbType dbType)
	{
		return s_defaultValues[(int)dbType];
	}

	private SmiMetaData(SqlDbType sqlDbType, long maxLength, byte precision, byte scale, SqlCompareOptions compareOptions)
	{
		_databaseType = sqlDbType;
		_maxLength = maxLength;
		_precision = precision;
		_scale = scale;
		_compareOptions = compareOptions;
		_localeId = 0L;
		_clrType = null;
		_isMultiValued = false;
		_fieldMetaData = s_emptyFieldList;
		_extendedProperties = SmiMetaDataPropertyCollection.EmptyInstance;
	}

	private void SetDefaultsForType(SqlDbType dbType)
	{
		SmiMetaData defaultForType = GetDefaultForType(dbType);
		_databaseType = dbType;
		_maxLength = defaultForType.MaxLength;
		_precision = defaultForType.Precision;
		_scale = defaultForType.Scale;
		_localeId = defaultForType.LocaleId;
		_compareOptions = defaultForType.CompareOptions;
		_clrType = null;
		_isMultiValued = defaultForType._isMultiValued;
		_fieldMetaData = defaultForType._fieldMetaData;
		_extendedProperties = defaultForType._extendedProperties;
	}
}
