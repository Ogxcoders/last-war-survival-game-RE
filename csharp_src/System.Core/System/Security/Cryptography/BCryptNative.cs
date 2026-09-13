namespace System.Security.Cryptography;

internal static class BCryptNative
{
	internal static class AlgorithmName
	{
		public const string ECDH = "ECDH";

		public const string ECDHP256 = "ECDH_P256";

		public const string ECDHP384 = "ECDH_P384";

		public const string ECDHP521 = "ECDH_P521";

		public const string ECDsa = "ECDSA";

		public const string ECDsaP256 = "ECDSA_P256";

		public const string ECDsaP384 = "ECDSA_P384";

		public const string ECDsaP521 = "ECDSA_P521";

		public const string MD5 = "MD5";

		public const string Sha1 = "SHA1";

		public const string Sha256 = "SHA256";

		public const string Sha384 = "SHA384";

		public const string Sha512 = "SHA512";

		internal const string Rsa = "RSA";
	}
}
