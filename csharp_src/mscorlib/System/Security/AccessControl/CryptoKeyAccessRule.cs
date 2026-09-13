using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class CryptoKeyAccessRule : AccessRule
{
	public CryptoKeyRights CryptoKeyRights => (CryptoKeyRights)base.AccessMask;

	public CryptoKeyAccessRule(IdentityReference identity, CryptoKeyRights cryptoKeyRights, AccessControlType type)
		: base(identity, (int)cryptoKeyRights, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}

	public CryptoKeyAccessRule(string identity, CryptoKeyRights cryptoKeyRights, AccessControlType type)
		: this(new NTAccount(identity), cryptoKeyRights, type)
	{
	}
}
