using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;

public class DerInteger : Asn1Object
{
	public const string AllowUnsafeProperty = "BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.AllowUnsafeInteger";

	private readonly byte[] bytes;

	public BigInteger Value => new BigInteger(bytes);

	public BigInteger PositiveValue => new BigInteger(1, bytes);

	internal static bool AllowUnsafe()
	{
		string environmentVariable = Platform.GetEnvironmentVariable("BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.AllowUnsafeInteger");
		if (environmentVariable != null)
		{
			return Platform.EqualsIgnoreCase("true", environmentVariable);
		}
		return false;
	}

	public static DerInteger GetInstance(object obj)
	{
		if (obj == null || obj is DerInteger)
		{
			return (DerInteger)obj;
		}
		throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
	}

	public static DerInteger GetInstance(Asn1TaggedObject obj, bool isExplicit)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		Asn1Object asn1Object = obj.GetObject();
		if (isExplicit || asn1Object is DerInteger)
		{
			return GetInstance(asn1Object);
		}
		return new DerInteger(Asn1OctetString.GetInstance(asn1Object).GetOctets());
	}

	public DerInteger(int value)
	{
		bytes = BigInteger.ValueOf(value).ToByteArray();
	}

	public DerInteger(BigInteger value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		bytes = value.ToByteArray();
	}

	public DerInteger(byte[] bytes)
	{
		if (bytes.Length > 1 && ((bytes[0] == 0 && (bytes[1] & 0x80) == 0) || (bytes[0] == byte.MaxValue && (bytes[1] & 0x80) != 0)) && !AllowUnsafe())
		{
			throw new ArgumentException("malformed integer");
		}
		this.bytes = Arrays.Clone(bytes);
	}

	internal override void Encode(DerOutputStream derOut)
	{
		derOut.WriteEncoded(2, bytes);
	}

	protected override int Asn1GetHashCode()
	{
		return Arrays.GetHashCode(bytes);
	}

	protected override bool Asn1Equals(Asn1Object asn1Object)
	{
		if (!(asn1Object is DerInteger derInteger))
		{
			return false;
		}
		return Arrays.AreEqual(bytes, derInteger.bytes);
	}

	public override string ToString()
	{
		return Value.ToString();
	}
}
