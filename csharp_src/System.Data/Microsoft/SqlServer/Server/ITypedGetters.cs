using System;
using System.Data;
using System.Data.SqlTypes;

namespace Microsoft.SqlServer.Server;

internal interface ITypedGetters
{
	bool IsDBNull(int ordinal);

	SqlDbType GetVariantType(int ordinal);

	bool GetBoolean(int ordinal);

	byte GetByte(int ordinal);

	long GetBytes(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length);

	char GetChar(int ordinal);

	long GetChars(int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length);

	short GetInt16(int ordinal);

	int GetInt32(int ordinal);

	long GetInt64(int ordinal);

	float GetFloat(int ordinal);

	double GetDouble(int ordinal);

	string GetString(int ordinal);

	decimal GetDecimal(int ordinal);

	DateTime GetDateTime(int ordinal);

	Guid GetGuid(int ordinal);

	SqlBoolean GetSqlBoolean(int ordinal);

	SqlByte GetSqlByte(int ordinal);

	SqlInt16 GetSqlInt16(int ordinal);

	SqlInt32 GetSqlInt32(int ordinal);

	SqlInt64 GetSqlInt64(int ordinal);

	SqlSingle GetSqlSingle(int ordinal);

	SqlDouble GetSqlDouble(int ordinal);

	SqlMoney GetSqlMoney(int ordinal);

	SqlDateTime GetSqlDateTime(int ordinal);

	SqlDecimal GetSqlDecimal(int ordinal);

	SqlString GetSqlString(int ordinal);

	SqlBinary GetSqlBinary(int ordinal);

	SqlGuid GetSqlGuid(int ordinal);

	SqlChars GetSqlChars(int ordinal);

	SqlBytes GetSqlBytes(int ordinal);

	SqlXml GetSqlXml(int ordinal);

	SqlBytes GetSqlBytesRef(int ordinal);

	SqlChars GetSqlCharsRef(int ordinal);

	SqlXml GetSqlXmlRef(int ordinal);
}
