using System.Threading;

namespace System.Data.SqlClient.SNI;

internal class SNILoadHandle
{
	public static readonly SNILoadHandle SingletonInstance = new SNILoadHandle();

	public readonly EncryptionOptions _encryptionOption;

	public ThreadLocal<SNIError> _lastError = new ThreadLocal<SNIError>(() => new SNIError(SNIProviders.INVALID_PROV, 0u, 0u, string.Empty));

	private readonly uint _status;

	public SNIError LastError
	{
		get
		{
			return _lastError.Value;
		}
		set
		{
			_lastError.Value = value;
		}
	}

	public uint Status => _status;

	public EncryptionOptions Options => _encryptionOption;
}
