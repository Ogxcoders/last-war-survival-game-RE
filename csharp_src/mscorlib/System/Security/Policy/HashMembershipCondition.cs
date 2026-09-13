using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Mono.Security.Cryptography;

namespace System.Security.Policy;

[Serializable]
[ComVisible(true)]
public sealed class HashMembershipCondition : IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable, IDeserializationCallback, ISerializable
{
	private readonly int version = 1;

	private HashAlgorithm hash_algorithm;

	private byte[] hash_value;

	public HashAlgorithm HashAlgorithm
	{
		get
		{
			if (hash_algorithm == null)
			{
				hash_algorithm = new SHA1Managed();
			}
			return hash_algorithm;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("HashAlgorithm");
			}
			hash_algorithm = value;
		}
	}

	public byte[] HashValue
	{
		get
		{
			if (hash_value == null)
			{
				throw new ArgumentException(Locale.GetText("No HashValue available."));
			}
			return (byte[])hash_value.Clone();
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("HashValue");
			}
			hash_value = (byte[])value.Clone();
		}
	}

	internal HashMembershipCondition()
	{
	}

	public HashMembershipCondition(HashAlgorithm hashAlg, byte[] value)
	{
		if (hashAlg == null)
		{
			throw new ArgumentNullException("hashAlg");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		hash_algorithm = hashAlg;
		hash_value = (byte[])value.Clone();
	}

	public bool Check(Evidence evidence)
	{
		if (evidence == null)
		{
			return false;
		}
		IEnumerator hostEnumerator = evidence.GetHostEnumerator();
		while (hostEnumerator.MoveNext())
		{
			if (hostEnumerator.Current is Hash hash)
			{
				if (!Compare(hash_value, hash.GenerateHash(hash_algorithm)))
				{
					break;
				}
				return true;
			}
		}
		return false;
	}

	public IMembershipCondition Copy()
	{
		return new HashMembershipCondition(hash_algorithm, hash_value);
	}

	public override bool Equals(object o)
	{
		if (!(o is HashMembershipCondition hashMembershipCondition))
		{
			return false;
		}
		if (hashMembershipCondition.HashAlgorithm == hash_algorithm)
		{
			return Compare(hash_value, hashMembershipCondition.hash_value);
		}
		return false;
	}

	public SecurityElement ToXml()
	{
		return ToXml(null);
	}

	public SecurityElement ToXml(PolicyLevel level)
	{
		SecurityElement securityElement = MembershipConditionHelper.Element(typeof(HashMembershipCondition), version);
		securityElement.AddAttribute("HashValue", CryptoConvert.ToHex(HashValue));
		securityElement.AddAttribute("HashAlgorithm", hash_algorithm.GetType().FullName);
		return securityElement;
	}

	public void FromXml(SecurityElement e)
	{
		FromXml(e, null);
	}

	public void FromXml(SecurityElement e, PolicyLevel level)
	{
		MembershipConditionHelper.CheckSecurityElement(e, "e", version, version);
		hash_value = CryptoConvert.FromHex(e.Attribute("HashValue"));
		string text = e.Attribute("HashAlgorithm");
		hash_algorithm = ((text == null) ? null : HashAlgorithm.Create(text));
	}

	public override int GetHashCode()
	{
		int num = hash_algorithm.GetType().GetHashCode();
		if (hash_value != null)
		{
			byte[] array = hash_value;
			foreach (byte b in array)
			{
				num ^= b;
			}
		}
		return num;
	}

	public override string ToString()
	{
		Type type = HashAlgorithm.GetType();
		return $"Hash - {type.FullName} {type.Assembly} = {CryptoConvert.ToHex(HashValue)}";
	}

	private bool Compare(byte[] expected, byte[] actual)
	{
		if (expected.Length != actual.Length)
		{
			return false;
		}
		int num = expected.Length;
		for (int i = 0; i < num; i++)
		{
			if (expected[i] != actual[i])
			{
				return false;
			}
		}
		return true;
	}

	[MonoTODO("fx 2.0")]
	void IDeserializationCallback.OnDeserialization(object sender)
	{
	}

	[MonoTODO("fx 2.0")]
	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
