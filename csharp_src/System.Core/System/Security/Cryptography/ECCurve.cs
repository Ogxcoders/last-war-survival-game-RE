using System.Diagnostics;

namespace System.Security.Cryptography;

[DebuggerDisplay("ECCurve: {Oid}")]
public struct ECCurve
{
	public enum ECCurveType
	{
		Implicit,
		PrimeShortWeierstrass,
		PrimeTwistedEdwards,
		PrimeMontgomery,
		Characteristic2,
		Named
	}

	public static class NamedCurves
	{
		private const string ECDSA_P256_OID_VALUE = "1.2.840.10045.3.1.7";

		private const string ECDSA_P384_OID_VALUE = "1.3.132.0.34";

		private const string ECDSA_P521_OID_VALUE = "1.3.132.0.35";

		public static ECCurve brainpoolP160r1 => CreateFromFriendlyName("brainpoolP160r1");

		public static ECCurve brainpoolP160t1 => CreateFromFriendlyName("brainpoolP160t1");

		public static ECCurve brainpoolP192r1 => CreateFromFriendlyName("brainpoolP192r1");

		public static ECCurve brainpoolP192t1 => CreateFromFriendlyName("brainpoolP192t1");

		public static ECCurve brainpoolP224r1 => CreateFromFriendlyName("brainpoolP224r1");

		public static ECCurve brainpoolP224t1 => CreateFromFriendlyName("brainpoolP224t1");

		public static ECCurve brainpoolP256r1 => CreateFromFriendlyName("brainpoolP256r1");

		public static ECCurve brainpoolP256t1 => CreateFromFriendlyName("brainpoolP256t1");

		public static ECCurve brainpoolP320r1 => CreateFromFriendlyName("brainpoolP320r1");

		public static ECCurve brainpoolP320t1 => CreateFromFriendlyName("brainpoolP320t1");

		public static ECCurve brainpoolP384r1 => CreateFromFriendlyName("brainpoolP384r1");

		public static ECCurve brainpoolP384t1 => CreateFromFriendlyName("brainpoolP384t1");

		public static ECCurve brainpoolP512r1 => CreateFromFriendlyName("brainpoolP512r1");

		public static ECCurve brainpoolP512t1 => CreateFromFriendlyName("brainpoolP512t1");

		public static ECCurve nistP256 => CreateFromValueAndName("1.2.840.10045.3.1.7", "nistP256");

		public static ECCurve nistP384 => CreateFromValueAndName("1.3.132.0.34", "nistP384");

		public static ECCurve nistP521 => CreateFromValueAndName("1.3.132.0.35", "nistP521");
	}

	public byte[] A;

	public byte[] B;

	public ECPoint G;

	public byte[] Order;

	public byte[] Cofactor;

	public byte[] Seed;

	public ECCurveType CurveType;

	public HashAlgorithmName? Hash;

	public byte[] Polynomial;

	public byte[] Prime;

	private Oid _oid;

	public Oid Oid
	{
		get
		{
			return new Oid(_oid.Value, _oid.FriendlyName);
		}
		private set
		{
			if (value == null)
			{
				throw new ArgumentNullException("Oid");
			}
			if (string.IsNullOrEmpty(value.Value) && string.IsNullOrEmpty(value.FriendlyName))
			{
				throw new ArgumentException($"The specified Oid is not valid. The Oid.FriendlyName or Oid.Value property must be set.");
			}
			_oid = value;
		}
	}

	public bool IsPrime
	{
		get
		{
			if (CurveType != ECCurveType.PrimeShortWeierstrass && CurveType != ECCurveType.PrimeMontgomery)
			{
				return CurveType == ECCurveType.PrimeTwistedEdwards;
			}
			return true;
		}
	}

	public bool IsCharacteristic2 => CurveType == ECCurveType.Characteristic2;

	public bool IsExplicit
	{
		get
		{
			if (!IsPrime)
			{
				return IsCharacteristic2;
			}
			return true;
		}
	}

	public bool IsNamed => CurveType == ECCurveType.Named;

	private static ECCurve Create(Oid oid)
	{
		return new ECCurve
		{
			CurveType = ECCurveType.Named,
			Oid = oid
		};
	}

	public static ECCurve CreateFromOid(Oid curveOid)
	{
		return Create(new Oid(curveOid.Value, curveOid.FriendlyName));
	}

	public static ECCurve CreateFromFriendlyName(string oidFriendlyName)
	{
		if (oidFriendlyName == null)
		{
			throw new ArgumentNullException("oidFriendlyName");
		}
		return CreateFromValueAndName(null, oidFriendlyName);
	}

	public static ECCurve CreateFromValue(string oidValue)
	{
		if (oidValue == null)
		{
			throw new ArgumentNullException("oidValue");
		}
		return CreateFromValueAndName(oidValue, null);
	}

	private static ECCurve CreateFromValueAndName(string oidValue, string oidFriendlyName)
	{
		return Create(new Oid(oidValue, oidFriendlyName));
	}

	public void Validate()
	{
		if (IsNamed)
		{
			if (HasAnyExplicitParameters())
			{
				throw new CryptographicException("The specified named curve parameters are not valid. Only the Oid parameter must be set.");
			}
			if (Oid == null || (string.IsNullOrEmpty(Oid.FriendlyName) && string.IsNullOrEmpty(Oid.Value)))
			{
				throw new CryptographicException("The specified Oid is not valid. The Oid.FriendlyName or Oid.Value property must be set.");
			}
		}
		else if (IsExplicit)
		{
			bool flag = false;
			if (A == null || B == null || B.Length != A.Length || G.X == null || G.X.Length != A.Length || G.Y == null || G.Y.Length != A.Length || Order == null || Order.Length == 0 || Cofactor == null || Cofactor.Length == 0)
			{
				flag = true;
			}
			if (IsPrime)
			{
				if (!flag && (Prime == null || Prime.Length != A.Length))
				{
					flag = true;
				}
				if (flag)
				{
					throw new CryptographicException("The specified prime curve parameters are not valid. Prime, A, B, G.X, G.Y and Order are required and must be the same length, and the same length as Q.X, Q.Y and D if those are specified. Seed, Cofactor and Hash are optional. Other parameters are not allowed.");
				}
			}
			else if (IsCharacteristic2)
			{
				if (!flag && (Polynomial == null || Polynomial.Length == 0))
				{
					flag = true;
				}
				if (flag)
				{
					throw new CryptographicException("The specified Characteristic2 curve parameters are not valid. Polynomial, A, B, G.X, G.Y, and Order are required. A, B, G.X, G.Y must be the same length, and the same length as Q.X, Q.Y and D if those are specified. Seed, Cofactor and Hash are optional. Other parameters are not allowed.");
				}
			}
		}
		else if (HasAnyExplicitParameters() || Oid != null)
		{
			throw new CryptographicException($"The specified curve '{CurveType.ToString()}' or its parameters are not valid for this platform.");
		}
	}

	private bool HasAnyExplicitParameters()
	{
		if (A == null && B == null && G.X == null && G.Y == null && Order == null && Cofactor == null && Prime == null && Polynomial == null && Seed == null)
		{
			return Hash.HasValue;
		}
		return true;
	}
}
