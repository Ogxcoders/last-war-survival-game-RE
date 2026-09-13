using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs;

public class AsymmetricKeyEntry : Pkcs12Entry
{
	private readonly AsymmetricKeyParameter key;

	public AsymmetricKeyParameter Key => key;

	public AsymmetricKeyEntry(AsymmetricKeyParameter key)
		: base(Platform.CreateHashtable())
	{
		this.key = key;
	}

	[Obsolete]
	public AsymmetricKeyEntry(AsymmetricKeyParameter key, Hashtable attributes)
		: base(attributes)
	{
		this.key = key;
	}

	public AsymmetricKeyEntry(AsymmetricKeyParameter key, IDictionary attributes)
		: base(attributes)
	{
		this.key = key;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is AsymmetricKeyEntry asymmetricKeyEntry))
		{
			return false;
		}
		return key.Equals(asymmetricKeyEntry.key);
	}

	public override int GetHashCode()
	{
		return ~key.GetHashCode();
	}
}
