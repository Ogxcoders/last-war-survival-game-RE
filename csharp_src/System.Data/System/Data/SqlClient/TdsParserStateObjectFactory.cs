using System.Data.SqlClient.SNI;

namespace System.Data.SqlClient;

internal sealed class TdsParserStateObjectFactory
{
	public static readonly TdsParserStateObjectFactory Singleton = new TdsParserStateObjectFactory();

	public static bool UseManagedSNI => true;

	public EncryptionOptions EncryptionOptions => SNILoadHandle.SingletonInstance.Options;

	public uint SNIStatus => SNILoadHandle.SingletonInstance.Status;

	public TdsParserStateObject CreateTdsParserStateObject(TdsParser parser)
	{
		return new TdsParserStateObjectManaged(parser);
	}

	internal TdsParserStateObject CreateSessionObject(TdsParser tdsParser, TdsParserStateObject _pMarsPhysicalConObj, bool v)
	{
		return new TdsParserStateObjectManaged(tdsParser, _pMarsPhysicalConObj, async: true);
	}
}
