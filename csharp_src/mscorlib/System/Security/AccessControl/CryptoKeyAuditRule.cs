using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class CryptoKeyAuditRule : AuditRule
{
	public CryptoKeyRights CryptoKeyRights => (CryptoKeyRights)base.AccessMask;

	public CryptoKeyAuditRule(IdentityReference identity, CryptoKeyRights cryptoKeyRights, AuditFlags flags)
		: base(identity, (int)cryptoKeyRights, isInherited: false, InheritanceFlags.None, PropagationFlags.None, flags)
	{
	}

	public CryptoKeyAuditRule(string identity, CryptoKeyRights cryptoKeyRights, AuditFlags flags)
		: this(new NTAccount(identity), cryptoKeyRights, flags)
	{
	}
}
