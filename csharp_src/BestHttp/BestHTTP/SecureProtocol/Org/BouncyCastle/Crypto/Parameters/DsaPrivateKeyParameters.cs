using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

public class DsaPrivateKeyParameters : DsaKeyParameters
{
	private readonly BigInteger x;

	public BigInteger X => x;

	public DsaPrivateKeyParameters(BigInteger x, DsaParameters parameters)
		: base(isPrivate: true, parameters)
	{
		if (x == null)
		{
			throw new ArgumentNullException("x");
		}
		this.x = x;
	}

	public override bool Equals(object obj)
	{
		if (obj == this)
		{
			return true;
		}
		if (!(obj is DsaPrivateKeyParameters other))
		{
			return false;
		}
		return Equals(other);
	}

	protected bool Equals(DsaPrivateKeyParameters other)
	{
		if (x.Equals(other.x))
		{
			return Equals((DsaKeyParameters)other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return x.GetHashCode() ^ base.GetHashCode();
	}
}
