using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;

namespace Microsoft.SqlServer.Server;

internal class MetaDataUtilsSmi
{
	internal const SqlDbType InvalidSqlDbType = (SqlDbType)(-1);

	internal const long InvalidMaxLength = -2L;

	private static readonly SqlDbType[] s_extendedTypeCodeToSqlDbTypeMap = new SqlDbType[43]
	{
		(SqlDbType)(-1),
		SqlDbType.Bit,
		SqlDbType.TinyInt,
		SqlDbType.NVarChar,
		SqlDbType.DateTime,
		(SqlDbType)(-1),
		SqlDbType.Decimal,
		SqlDbType.Float,
		(SqlDbType)(-1),
		SqlDbType.SmallInt,
		SqlDbType.Int,
		SqlDbType.BigInt,
		(SqlDbType)(-1),
		SqlDbType.Real,
		SqlDbType.NVarChar,
		(SqlDbType)(-1),
		(SqlDbType)(-1),
		(SqlDbType)(-1),
		(SqlDbType)(-1),
		SqlDbType.VarBinary,
		SqlDbType.NVarChar,
		SqlDbType.UniqueIdentifier,
		SqlDbType.VarBinary,
		SqlDbType.Bit,
		SqlDbType.TinyInt,
		SqlDbType.DateTime,
		SqlDbType.Float,
		SqlDbType.UniqueIdentifier,
		SqlDbType.SmallInt,
		SqlDbType.Int,
		SqlDbType.BigInt,
		SqlDbType.Money,
		SqlDbType.Decimal,
		SqlDbType.Real,
		SqlDbType.NVarChar,
		SqlDbType.NVarChar,
		SqlDbType.VarBinary,
		SqlDbType.Xml,
		SqlDbType.Structured,
		SqlDbType.Structured,
		SqlDbType.Structured,
		SqlDbType.Time,
		SqlDbType.DateTimeOffset
	};

	private static readonly Dictionary<Type, ExtendedClrTypeCode> s_typeToExtendedTypeCodeMap = CreateTypeToExtendedTypeCodeMap();

	private static Dictionary<Type, ExtendedClrTypeCode> CreateTypeToExtendedTypeCodeMap()
	{
		return new Dictionary<Type, ExtendedClrTypeCode>(42)
		{
			{
				typeof(bool),
				ExtendedClrTypeCode.Boolean
			},
			{
				typeof(byte),
				ExtendedClrTypeCode.Byte
			},
			{
				typeof(char),
				ExtendedClrTypeCode.Char
			},
			{
				typeof(DateTime),
				ExtendedClrTypeCode.DateTime
			},
			{
				typeof(DBNull),
				ExtendedClrTypeCode.DBNull
			},
			{
				typeof(decimal),
				ExtendedClrTypeCode.Decimal
			},
			{
				typeof(double),
				ExtendedClrTypeCode.Double
			},
			{
				typeof(short),
				ExtendedClrTypeCode.Int16
			},
			{
				typeof(int),
				ExtendedClrTypeCode.Int32
			},
			{
				typeof(long),
				ExtendedClrTypeCode.Int64
			},
			{
				typeof(sbyte),
				ExtendedClrTypeCode.SByte
			},
			{
				typeof(float),
				ExtendedClrTypeCode.Single
			},
			{
				typeof(string),
				ExtendedClrTypeCode.String
			},
			{
				typeof(ushort),
				ExtendedClrTypeCode.UInt16
			},
			{
				typeof(uint),
				ExtendedClrTypeCode.UInt32
			},
			{
				typeof(ulong),
				ExtendedClrTypeCode.UInt64
			},
			{
				typeof(object),
				ExtendedClrTypeCode.Object
			},
			{
				typeof(byte[]),
				ExtendedClrTypeCode.ByteArray
			},
			{
				typeof(char[]),
				ExtendedClrTypeCode.CharArray
			},
			{
				typeof(Guid),
				ExtendedClrTypeCode.Guid
			},
			{
				typeof(SqlBinary),
				ExtendedClrTypeCode.SqlBinary
			},
			{
				typeof(SqlBoolean),
				ExtendedClrTypeCode.SqlBoolean
			},
			{
				typeof(SqlByte),
				ExtendedClrTypeCode.SqlByte
			},
			{
				typeof(SqlDateTime),
				ExtendedClrTypeCode.SqlDateTime
			},
			{
				typeof(SqlDouble),
				ExtendedClrTypeCode.SqlDouble
			},
			{
				typeof(SqlGuid),
				ExtendedClrTypeCode.SqlGuid
			},
			{
				typeof(SqlInt16),
				ExtendedClrTypeCode.SqlInt16
			},
			{
				typeof(SqlInt32),
				ExtendedClrTypeCode.SqlInt32
			},
			{
				typeof(SqlInt64),
				ExtendedClrTypeCode.SqlInt64
			},
			{
				typeof(SqlMoney),
				ExtendedClrTypeCode.SqlMoney
			},
			{
				typeof(SqlDecimal),
				ExtendedClrTypeCode.SqlDecimal
			},
			{
				typeof(SqlSingle),
				ExtendedClrTypeCode.SqlSingle
			},
			{
				typeof(SqlString),
				ExtendedClrTypeCode.SqlString
			},
			{
				typeof(SqlChars),
				ExtendedClrTypeCode.SqlChars
			},
			{
				typeof(SqlBytes),
				ExtendedClrTypeCode.SqlBytes
			},
			{
				typeof(SqlXml),
				ExtendedClrTypeCode.SqlXml
			},
			{
				typeof(DataTable),
				ExtendedClrTypeCode.DataTable
			},
			{
				typeof(DbDataReader),
				ExtendedClrTypeCode.DbDataReader
			},
			{
				typeof(IEnumerable<SqlDataRecord>),
				ExtendedClrTypeCode.IEnumerableOfSqlDataRecord
			},
			{
				typeof(TimeSpan),
				ExtendedClrTypeCode.TimeSpan
			},
			{
				typeof(DateTimeOffset),
				ExtendedClrTypeCode.DateTimeOffset
			}
		};
	}

	internal static bool IsCharOrXmlType(SqlDbType type)
	{
		if (!IsUnicodeType(type) && !IsAnsiType(type))
		{
			return type == SqlDbType.Xml;
		}
		return true;
	}

	internal static bool IsUnicodeType(SqlDbType type)
	{
		if (type != SqlDbType.NChar && type != SqlDbType.NVarChar)
		{
			return type == SqlDbType.NText;
		}
		return true;
	}

	internal static bool IsAnsiType(SqlDbType type)
	{
		if (type != SqlDbType.Char && type != SqlDbType.VarChar)
		{
			return type == SqlDbType.Text;
		}
		return true;
	}

	internal static bool IsBinaryType(SqlDbType type)
	{
		if (type != SqlDbType.Binary && type != SqlDbType.VarBinary)
		{
			return type == SqlDbType.Image;
		}
		return true;
	}

	internal static bool IsPlpFormat(SmiMetaData metaData)
	{
		if (metaData.MaxLength != -1 && metaData.SqlDbType != SqlDbType.Image && metaData.SqlDbType != SqlDbType.NText && metaData.SqlDbType != SqlDbType.Text)
		{
			return metaData.SqlDbType == SqlDbType.Udt;
		}
		return true;
	}

	internal static ExtendedClrTypeCode DetermineExtendedTypeCodeForUseWithSqlDbType(SqlDbType dbType, bool isMultiValued, object value, Type udtType)
	{
		ExtendedClrTypeCode extendedClrTypeCode = ExtendedClrTypeCode.Invalid;
		if (value == null)
		{
			extendedClrTypeCode = ExtendedClrTypeCode.Empty;
		}
		else if (DBNull.Value == value)
		{
			extendedClrTypeCode = ExtendedClrTypeCode.DBNull;
		}
		else
		{
			switch (dbType)
			{
			case SqlDbType.BigInt:
				if (value.GetType() == typeof(long))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.Int64;
				}
				else if (value.GetType() == typeof(SqlInt64))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlInt64;
				}
				break;
			case SqlDbType.Binary:
			case SqlDbType.Image:
			case SqlDbType.Timestamp:
			case SqlDbType.VarBinary:
				if (value.GetType() == typeof(byte[]))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.ByteArray;
				}
				else if (value.GetType() == typeof(SqlBinary))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlBinary;
				}
				else if (value.GetType() == typeof(SqlBytes))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlBytes;
				}
				else if (value.GetType() == typeof(StreamDataFeed))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.Stream;
				}
				break;
			case SqlDbType.Bit:
				if (value.GetType() == typeof(bool))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.Boolean;
				}
				else if (value.GetType() == typeof(SqlBoolean))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlBoolean;
				}
				break;
			case SqlDbType.Char:
			case SqlDbType.NChar:
			case SqlDbType.NText:
			case SqlDbType.NVarChar:
			case SqlDbType.Text:
			case SqlDbType.VarChar:
				if (value.GetType() == typeof(string))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.String;
				}
				if (value.GetType() == typeof(TextDataFeed))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.TextReader;
				}
				else if (value.GetType() == typeof(SqlString))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlString;
				}
				else if (value.GetType() == typeof(char[]))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.CharArray;
				}
				else if (value.GetType() == typeof(SqlChars))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlChars;
				}
				else if (value.GetType() == typeof(char))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.Char;
				}
				break;
			case SqlDbType.DateTime:
			case SqlDbType.SmallDateTime:
			case SqlDbType.Date:
			case SqlDbType.DateTime2:
				if (value.GetType() == typeof(DateTime))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.DateTime;
				}
				else if (value.GetType() == typeof(SqlDateTime))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlDateTime;
				}
				break;
			case SqlDbType.Decimal:
				if (value.GetType() == typeof(decimal))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.Decimal;
				}
				else if (value.GetType() == typeof(SqlDecimal))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlDecimal;
				}
				break;
			case SqlDbType.Real:
				if (value.GetType() == typeof(float))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.Single;
				}
				else if (value.GetType() == typeof(SqlSingle))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlSingle;
				}
				break;
			case SqlDbType.Int:
				if (value.GetType() == typeof(int))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.Int32;
				}
				else if (value.GetType() == typeof(SqlInt32))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlInt32;
				}
				break;
			case SqlDbType.Money:
			case SqlDbType.SmallMoney:
				if (value.GetType() == typeof(SqlMoney))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlMoney;
				}
				else if (value.GetType() == typeof(decimal))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.Decimal;
				}
				break;
			case SqlDbType.Float:
				if (value.GetType() == typeof(SqlDouble))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlDouble;
				}
				else if (value.GetType() == typeof(double))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.Double;
				}
				break;
			case SqlDbType.UniqueIdentifier:
				if (value.GetType() == typeof(SqlGuid))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlGuid;
				}
				else if (value.GetType() == typeof(Guid))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.Guid;
				}
				break;
			case SqlDbType.SmallInt:
				if (value.GetType() == typeof(short))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.Int16;
				}
				else if (value.GetType() == typeof(SqlInt16))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlInt16;
				}
				break;
			case SqlDbType.TinyInt:
				if (value.GetType() == typeof(byte))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.Byte;
				}
				else if (value.GetType() == typeof(SqlByte))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlByte;
				}
				break;
			case SqlDbType.Variant:
				extendedClrTypeCode = DetermineExtendedTypeCode(value);
				if (ExtendedClrTypeCode.SqlXml == extendedClrTypeCode)
				{
					extendedClrTypeCode = ExtendedClrTypeCode.Invalid;
				}
				break;
			case SqlDbType.Udt:
				extendedClrTypeCode = ((!(null == udtType) && !(value.GetType() == udtType)) ? ExtendedClrTypeCode.Invalid : ExtendedClrTypeCode.Object);
				break;
			case SqlDbType.Time:
				if (value.GetType() == typeof(TimeSpan))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.TimeSpan;
				}
				break;
			case SqlDbType.DateTimeOffset:
				if (value.GetType() == typeof(DateTimeOffset))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.DateTimeOffset;
				}
				break;
			case SqlDbType.Xml:
				if (value.GetType() == typeof(SqlXml))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.SqlXml;
				}
				if (value.GetType() == typeof(XmlDataFeed))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.XmlReader;
				}
				else if (value.GetType() == typeof(string))
				{
					extendedClrTypeCode = ExtendedClrTypeCode.String;
				}
				break;
			case SqlDbType.Structured:
				if (isMultiValued)
				{
					if (value is DataTable)
					{
						extendedClrTypeCode = ExtendedClrTypeCode.DataTable;
					}
					else if (value is IEnumerable<SqlDataRecord>)
					{
						extendedClrTypeCode = ExtendedClrTypeCode.IEnumerableOfSqlDataRecord;
					}
					else if (value is DbDataReader)
					{
						extendedClrTypeCode = ExtendedClrTypeCode.DbDataReader;
					}
				}
				break;
			}
		}
		return extendedClrTypeCode;
	}

	internal static ExtendedClrTypeCode DetermineExtendedTypeCodeFromType(Type clrType)
	{
		if (!s_typeToExtendedTypeCodeMap.TryGetValue(clrType, out var value))
		{
			return ExtendedClrTypeCode.Invalid;
		}
		return value;
	}

	internal static ExtendedClrTypeCode DetermineExtendedTypeCode(object value)
	{
		if (value == null)
		{
			return ExtendedClrTypeCode.Empty;
		}
		return DetermineExtendedTypeCodeFromType(value.GetType());
	}

	internal static SqlDbType InferSqlDbTypeFromTypeCode(ExtendedClrTypeCode typeCode)
	{
		return s_extendedTypeCodeToSqlDbTypeMap[(int)(typeCode + 1)];
	}

	internal static SqlDbType InferSqlDbTypeFromType(Type type)
	{
		ExtendedClrTypeCode extendedClrTypeCode = DetermineExtendedTypeCodeFromType(type);
		if (ExtendedClrTypeCode.Invalid == extendedClrTypeCode)
		{
			return (SqlDbType)(-1);
		}
		return InferSqlDbTypeFromTypeCode(extendedClrTypeCode);
	}

	internal static SqlDbType InferSqlDbTypeFromType_Katmai(Type type)
	{
		SqlDbType sqlDbType = InferSqlDbTypeFromType(type);
		if (SqlDbType.DateTime == sqlDbType)
		{
			sqlDbType = SqlDbType.DateTime2;
		}
		return sqlDbType;
	}

	internal static SqlMetaData SmiExtendedMetaDataToSqlMetaData(SmiExtendedMetaData source)
	{
		if (SqlDbType.Xml == source.SqlDbType)
		{
			return new SqlMetaData(source.Name, source.SqlDbType, source.MaxLength, source.Precision, source.Scale, source.LocaleId, source.CompareOptions, source.TypeSpecificNamePart1, source.TypeSpecificNamePart2, source.TypeSpecificNamePart3, partialLength: true, source.Type);
		}
		return new SqlMetaData(source.Name, source.SqlDbType, source.MaxLength, source.Precision, source.Scale, source.LocaleId, source.CompareOptions, null);
	}

	internal static SmiExtendedMetaData SqlMetaDataToSmiExtendedMetaData(SqlMetaData source)
	{
		string text = null;
		string text2 = null;
		string text3 = null;
		if (SqlDbType.Xml == source.SqlDbType)
		{
			text = source.XmlSchemaCollectionDatabase;
			text2 = source.XmlSchemaCollectionOwningSchema;
			text3 = source.XmlSchemaCollectionName;
		}
		else if (SqlDbType.Udt == source.SqlDbType)
		{
			string serverTypeName = source.ServerTypeName;
			if (serverTypeName != null)
			{
				string[] array = SqlParameter.ParseTypeName(serverTypeName, isUdtTypeName: true);
				if (1 == array.Length)
				{
					text3 = array[0];
				}
				else if (2 == array.Length)
				{
					text2 = array[0];
					text3 = array[1];
				}
				else
				{
					if (3 != array.Length)
					{
						throw ADP.ArgumentOutOfRange("typeName");
					}
					text = array[0];
					text2 = array[1];
					text3 = array[2];
				}
				if ((!string.IsNullOrEmpty(text) && 255 < text.Length) || (!string.IsNullOrEmpty(text2) && 255 < text2.Length) || (!string.IsNullOrEmpty(text3) && 255 < text3.Length))
				{
					throw ADP.ArgumentOutOfRange("typeName");
				}
			}
		}
		return new SmiExtendedMetaData(source.SqlDbType, source.MaxLength, source.Precision, source.Scale, source.LocaleId, source.CompareOptions, null, source.Name, text, text2, text3);
	}

	internal static bool IsCompatible(SmiMetaData firstMd, SqlMetaData secondMd)
	{
		if (firstMd.SqlDbType == secondMd.SqlDbType && firstMd.MaxLength == secondMd.MaxLength && firstMd.Precision == secondMd.Precision && firstMd.Scale == secondMd.Scale && firstMd.CompareOptions == secondMd.CompareOptions && firstMd.LocaleId == secondMd.LocaleId && firstMd.SqlDbType != SqlDbType.Structured)
		{
			return !firstMd.IsMultiValued;
		}
		return false;
	}

	internal static SmiExtendedMetaData SmiMetaDataFromDataColumn(DataColumn column, DataTable parent)
	{
		SqlDbType sqlDbType = InferSqlDbTypeFromType_Katmai(column.DataType);
		if ((SqlDbType)(-1) == sqlDbType)
		{
			throw SQL.UnsupportedColumnTypeForSqlProvider(column.ColumnName, column.DataType.Name);
		}
		long num = AdjustMaxLength(sqlDbType, column.MaxLength);
		if (-2 == num)
		{
			throw SQL.InvalidColumnMaxLength(column.ColumnName, num);
		}
		checked
		{
			byte b;
			byte b4;
			if (column.DataType == typeof(SqlDecimal))
			{
				b = 0;
				byte b2 = 0;
				foreach (DataRow row in parent.Rows)
				{
					object obj = row[column];
					if (obj is DBNull)
					{
						continue;
					}
					SqlDecimal sqlDecimal = (SqlDecimal)obj;
					if (!sqlDecimal.IsNull)
					{
						byte b3 = (byte)(sqlDecimal.Precision - sqlDecimal.Scale);
						if (b3 > b2)
						{
							b2 = b3;
						}
						if (sqlDecimal.Scale > b)
						{
							b = sqlDecimal.Scale;
						}
					}
				}
				b4 = (byte)(b2 + b);
				if (SqlDecimal.MaxPrecision < b4)
				{
					throw SQL.InvalidTableDerivedPrecisionForTvp(column.ColumnName, b4);
				}
				if (b4 == 0)
				{
					b4 = 1;
				}
			}
			else
			{
				switch (sqlDbType)
				{
				case SqlDbType.Time:
				case SqlDbType.DateTime2:
				case SqlDbType.DateTimeOffset:
					b4 = 0;
					b = SmiMetaData.DefaultTime.Scale;
					break;
				case SqlDbType.Decimal:
				{
					b = 0;
					byte b5 = 0;
					foreach (DataRow row2 in parent.Rows)
					{
						object obj2 = row2[column];
						if (!(obj2 is DBNull))
						{
							SqlDecimal sqlDecimal2 = (decimal)obj2;
							byte b6 = (byte)(sqlDecimal2.Precision - sqlDecimal2.Scale);
							if (b6 > b5)
							{
								b5 = b6;
							}
							if (sqlDecimal2.Scale > b)
							{
								b = sqlDecimal2.Scale;
							}
						}
					}
					b4 = (byte)(b5 + b);
					if (SqlDecimal.MaxPrecision < b4)
					{
						throw SQL.InvalidTableDerivedPrecisionForTvp(column.ColumnName, b4);
					}
					if (b4 == 0)
					{
						b4 = 1;
					}
					break;
				}
				default:
					b4 = 0;
					b = 0;
					break;
				}
			}
			CultureInfo cultureInfo = ((parent != null) ? parent.Locale : CultureInfo.CurrentCulture);
			return new SmiExtendedMetaData(sqlDbType, num, b4, b, cultureInfo.LCID, SmiMetaData.DefaultNVarChar.CompareOptions, null, isMultiValued: false, null, null, column.ColumnName, null, null, null);
		}
	}

	internal static long AdjustMaxLength(SqlDbType dbType, long maxLength)
	{
		if (-1 != maxLength)
		{
			if (maxLength < 0)
			{
				maxLength = -2L;
			}
			switch (dbType)
			{
			case SqlDbType.Binary:
				if (maxLength > 8000)
				{
					maxLength = -2L;
				}
				break;
			case SqlDbType.Char:
				if (maxLength > 8000)
				{
					maxLength = -2L;
				}
				break;
			case SqlDbType.NChar:
				if (maxLength > 4000)
				{
					maxLength = -2L;
				}
				break;
			case SqlDbType.NVarChar:
				if (4000 < maxLength)
				{
					maxLength = -1L;
				}
				break;
			case SqlDbType.VarBinary:
				if (8000 < maxLength)
				{
					maxLength = -1L;
				}
				break;
			case SqlDbType.VarChar:
				if (8000 < maxLength)
				{
					maxLength = -1L;
				}
				break;
			}
		}
		return maxLength;
	}

	internal static SmiExtendedMetaData SmiMetaDataFromSchemaTableRow(DataRow schemaRow)
	{
		string text = "";
		object obj = schemaRow[SchemaTableColumn.ColumnName];
		if (DBNull.Value != obj)
		{
			text = (string)obj;
		}
		obj = schemaRow[SchemaTableColumn.DataType];
		if (DBNull.Value == obj)
		{
			throw SQL.NullSchemaTableDataTypeNotSupported(text);
		}
		Type type = (Type)obj;
		SqlDbType sqlDbType = InferSqlDbTypeFromType_Katmai(type);
		if ((SqlDbType)(-1) == sqlDbType)
		{
			if (!(typeof(object) == type))
			{
				throw SQL.UnsupportedColumnTypeForSqlProvider(text, type.ToString());
			}
			sqlDbType = SqlDbType.VarBinary;
		}
		long num = 0L;
		byte b = 0;
		byte b2 = 0;
		switch (sqlDbType)
		{
		case SqlDbType.Binary:
		case SqlDbType.VarBinary:
			obj = schemaRow[SchemaTableColumn.ColumnSize];
			if (DBNull.Value == obj)
			{
				num = ((SqlDbType.Binary != sqlDbType) ? (-1) : 8000);
				break;
			}
			num = Convert.ToInt64(obj, null);
			if (num > 8000)
			{
				num = -1L;
			}
			if (num >= 0 || (num == -1 && SqlDbType.Binary != sqlDbType))
			{
				break;
			}
			throw SQL.InvalidColumnMaxLength(text, num);
		case SqlDbType.Char:
		case SqlDbType.VarChar:
			obj = schemaRow[SchemaTableColumn.ColumnSize];
			if (DBNull.Value == obj)
			{
				num = ((SqlDbType.Char != sqlDbType) ? (-1) : 8000);
				break;
			}
			num = Convert.ToInt64(obj, null);
			if (num > 8000)
			{
				num = -1L;
			}
			if (num >= 0 || (num == -1 && SqlDbType.Char != sqlDbType))
			{
				break;
			}
			throw SQL.InvalidColumnMaxLength(text, num);
		case SqlDbType.NChar:
		case SqlDbType.NVarChar:
			obj = schemaRow[SchemaTableColumn.ColumnSize];
			if (DBNull.Value == obj)
			{
				num = ((SqlDbType.NChar != sqlDbType) ? (-1) : 4000);
				break;
			}
			num = Convert.ToInt64(obj, null);
			if (num > 4000)
			{
				num = -1L;
			}
			if (num >= 0 || (num == -1 && SqlDbType.NChar != sqlDbType))
			{
				break;
			}
			throw SQL.InvalidColumnMaxLength(text, num);
		case SqlDbType.Decimal:
			obj = schemaRow[SchemaTableColumn.NumericPrecision];
			b = ((DBNull.Value != obj) ? Convert.ToByte(obj, null) : SmiMetaData.DefaultDecimal.Precision);
			obj = schemaRow[SchemaTableColumn.NumericScale];
			b2 = ((DBNull.Value != obj) ? Convert.ToByte(obj, null) : SmiMetaData.DefaultDecimal.Scale);
			if (b < 1 || b > SqlDecimal.MaxPrecision || b2 < 0 || b2 > SqlDecimal.MaxScale || b2 > b)
			{
				throw SQL.InvalidColumnPrecScale();
			}
			break;
		case SqlDbType.Time:
		case SqlDbType.DateTime2:
		case SqlDbType.DateTimeOffset:
			obj = schemaRow[SchemaTableColumn.NumericScale];
			b2 = ((DBNull.Value != obj) ? Convert.ToByte(obj, null) : SmiMetaData.DefaultTime.Scale);
			if (b2 > 7)
			{
				throw SQL.InvalidColumnPrecScale();
			}
			if (b2 < 0)
			{
				b2 = SmiMetaData.DefaultTime.Scale;
			}
			break;
		default:
			throw SQL.UnsupportedColumnTypeForSqlProvider(text, type.ToString());
		case SqlDbType.BigInt:
		case SqlDbType.Bit:
		case SqlDbType.DateTime:
		case SqlDbType.Float:
		case SqlDbType.Image:
		case SqlDbType.Int:
		case SqlDbType.Money:
		case SqlDbType.NText:
		case SqlDbType.Real:
		case SqlDbType.UniqueIdentifier:
		case SqlDbType.SmallDateTime:
		case SqlDbType.SmallInt:
		case SqlDbType.SmallMoney:
		case SqlDbType.Text:
		case SqlDbType.Timestamp:
		case SqlDbType.TinyInt:
		case SqlDbType.Variant:
		case SqlDbType.Xml:
		case SqlDbType.Date:
			break;
		}
		return new SmiExtendedMetaData(sqlDbType, num, b, b2, CultureInfo.CurrentCulture.LCID, SmiMetaData.GetDefaultForType(sqlDbType).CompareOptions, null, isMultiValued: false, null, null, text, null, null, null);
	}
}
