using System;
using System.Data.SqlTypes;

namespace Microsoft.SqlServer.Server;

internal interface ITypedSetters
{
	void SetDBNull(int ordinal);

	void SetBoolean(int ordinal, bool value);

	void SetByte(int ordinal, byte value);

	void SetBytes(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length);

	void SetChar(int ordinal, char value);

	void SetChars(int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length);

	void SetInt16(int ordinal, short value);

	void SetInt32(int ordinal, int value);

	void SetInt64(int ordinal, long value);

	void SetFloat(int ordinal, float value);

	void SetDouble(int ordinal, double value);

	[Obsolete("Not supported as of SMI v2.  Will be removed when v1 support dropped.  Use setter with offset.")]
	void SetString(int ordinal, string value);

	void SetString(int ordinal, string value, int offset);

	void SetDecimal(int ordinal, decimal value);

	void SetDateTime(int ordinal, DateTime value);

	void SetGuid(int ordinal, Guid value);

	void SetSqlBoolean(int ordinal, SqlBoolean value);

	void SetSqlByte(int ordinal, SqlByte value);

	void SetSqlInt16(int ordinal, SqlInt16 value);

	void SetSqlInt32(int ordinal, SqlInt32 value);

	void SetSqlInt64(int ordinal, SqlInt64 value);

	void SetSqlSingle(int ordinal, SqlSingle value);

	void SetSqlDouble(int ordinal, SqlDouble value);

	void SetSqlMoney(int ordinal, SqlMoney value);

	void SetSqlDateTime(int ordinal, SqlDateTime value);

	void SetSqlDecimal(int ordinal, SqlDecimal value);

	[Obsolete("Not supported as of SMI v2.  Will be removed when v1 support dropped.  Use setter with offset.")]
	void SetSqlString(int ordinal, SqlString value);

	void SetSqlString(int ordinal, SqlString value, int offset);

	[Obsolete("Not supported as of SMI v2.  Will be removed when v1 support dropped.  Use setter with offset.")]
	void SetSqlBinary(int ordinal, SqlBinary value);

	void SetSqlBinary(int ordinal, SqlBinary value, int offset);

	void SetSqlGuid(int ordinal, SqlGuid value);

	[Obsolete("Not supported as of SMI v2.  Will be removed when v1 support dropped.  Use setter with offset.")]
	void SetSqlChars(int ordinal, SqlChars value);

	void SetSqlChars(int ordinal, SqlChars value, int offset);

	[Obsolete("Not supported as of SMI v2.  Will be removed when v1 support dropped.  Use setter with offset.")]
	void SetSqlBytes(int ordinal, SqlBytes value);

	void SetSqlBytes(int ordinal, SqlBytes value, int offset);

	void SetSqlXml(int ordinal, SqlXml value);
}
