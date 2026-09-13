using System.Data.SqlTypes;
using System.Diagnostics;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient;

internal class TdsRecordBufferSetter : SmiRecordBuffer
{
	private TdsValueSetter[] _fieldSetters;

	private TdsParserStateObject _stateObj;

	private SmiMetaData _metaData;

	internal override bool CanGet => false;

	internal override bool CanSet => true;

	internal TdsRecordBufferSetter(TdsParserStateObject stateObj, SmiMetaData md)
	{
		_fieldSetters = new TdsValueSetter[md.FieldMetaData.Count];
		for (int i = 0; i < md.FieldMetaData.Count; i++)
		{
			_fieldSetters[i] = new TdsValueSetter(stateObj, md.FieldMetaData[i]);
		}
		_stateObj = stateObj;
		_metaData = md;
	}

	public override void SetDBNull(SmiEventSink sink, int ordinal)
	{
		_fieldSetters[ordinal].SetDBNull();
	}

	public override void SetBoolean(SmiEventSink sink, int ordinal, bool value)
	{
		_fieldSetters[ordinal].SetBoolean(value);
	}

	public override void SetByte(SmiEventSink sink, int ordinal, byte value)
	{
		_fieldSetters[ordinal].SetByte(value);
	}

	public override int SetBytes(SmiEventSink sink, int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
	{
		return _fieldSetters[ordinal].SetBytes(fieldOffset, buffer, bufferOffset, length);
	}

	public override void SetBytesLength(SmiEventSink sink, int ordinal, long length)
	{
		_fieldSetters[ordinal].SetBytesLength(length);
	}

	public override int SetChars(SmiEventSink sink, int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
	{
		return _fieldSetters[ordinal].SetChars(fieldOffset, buffer, bufferOffset, length);
	}

	public override void SetCharsLength(SmiEventSink sink, int ordinal, long length)
	{
		_fieldSetters[ordinal].SetCharsLength(length);
	}

	public override void SetString(SmiEventSink sink, int ordinal, string value, int offset, int length)
	{
		_fieldSetters[ordinal].SetString(value, offset, length);
	}

	public override void SetInt16(SmiEventSink sink, int ordinal, short value)
	{
		_fieldSetters[ordinal].SetInt16(value);
	}

	public override void SetInt32(SmiEventSink sink, int ordinal, int value)
	{
		_fieldSetters[ordinal].SetInt32(value);
	}

	public override void SetInt64(SmiEventSink sink, int ordinal, long value)
	{
		_fieldSetters[ordinal].SetInt64(value);
	}

	public override void SetSingle(SmiEventSink sink, int ordinal, float value)
	{
		_fieldSetters[ordinal].SetSingle(value);
	}

	public override void SetDouble(SmiEventSink sink, int ordinal, double value)
	{
		_fieldSetters[ordinal].SetDouble(value);
	}

	public override void SetSqlDecimal(SmiEventSink sink, int ordinal, SqlDecimal value)
	{
		_fieldSetters[ordinal].SetSqlDecimal(value);
	}

	public override void SetDateTime(SmiEventSink sink, int ordinal, DateTime value)
	{
		_fieldSetters[ordinal].SetDateTime(value);
	}

	public override void SetGuid(SmiEventSink sink, int ordinal, Guid value)
	{
		_fieldSetters[ordinal].SetGuid(value);
	}

	public override void SetTimeSpan(SmiEventSink sink, int ordinal, TimeSpan value)
	{
		_fieldSetters[ordinal].SetTimeSpan(value);
	}

	public override void SetDateTimeOffset(SmiEventSink sink, int ordinal, DateTimeOffset value)
	{
		_fieldSetters[ordinal].SetDateTimeOffset(value);
	}

	public override void SetVariantMetaData(SmiEventSink sink, int ordinal, SmiMetaData metaData)
	{
		_fieldSetters[ordinal].SetVariantType(metaData);
	}

	internal override void NewElement(SmiEventSink sink)
	{
		_stateObj.WriteByte(1);
	}

	internal override void EndElements(SmiEventSink sink)
	{
		_stateObj.WriteByte(0);
	}

	[Conditional("DEBUG")]
	private void CheckWritingToColumn(int ordinal)
	{
	}

	[Conditional("DEBUG")]
	private void SkipPossibleDefaultedColumns(int targetColumn)
	{
	}

	[Conditional("DEBUG")]
	internal void CheckSettingColumn(int ordinal)
	{
	}
}
