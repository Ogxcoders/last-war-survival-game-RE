using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient;

internal class TdsParameterSetter : SmiTypedGetterSetter
{
	private TdsRecordBufferSetter _target;

	internal override bool CanGet => false;

	internal override bool CanSet => true;

	internal TdsParameterSetter(TdsParserStateObject stateObj, SmiMetaData md)
	{
		_target = new TdsRecordBufferSetter(stateObj, md);
	}

	internal override SmiTypedGetterSetter GetTypedGetterSetter(SmiEventSink sink, int ordinal)
	{
		return _target;
	}

	public override void SetDBNull(SmiEventSink sink, int ordinal)
	{
		_target.EndElements(sink);
	}
}
