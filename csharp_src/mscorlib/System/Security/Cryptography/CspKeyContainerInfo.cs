using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace System.Security.Cryptography;

[ComVisible(true)]
public sealed class CspKeyContainerInfo
{
	private CspParameters _params;

	internal bool _random;

	public bool Accessible => true;

	public CryptoKeySecurity CryptoKeySecurity => null;

	public bool Exportable => true;

	public bool HardwareDevice => false;

	public string KeyContainerName => _params.KeyContainerName;

	public KeyNumber KeyNumber => (KeyNumber)_params.KeyNumber;

	public bool MachineKeyStore => false;

	public bool Protected => false;

	public string ProviderName => _params.ProviderName;

	public int ProviderType => _params.ProviderType;

	public bool RandomlyGenerated => _random;

	public bool Removable => false;

	public string UniqueKeyContainerName => _params.ProviderName + "\\" + _params.KeyContainerName;

	public CspKeyContainerInfo(CspParameters parameters)
	{
		_params = parameters;
		_random = true;
	}
}
