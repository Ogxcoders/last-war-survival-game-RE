using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

public abstract class X509NameEntryConverter
{
	protected Asn1Object ConvertHexEncoded(string hexString, int offset)
	{
		return Asn1Object.FromByteArray(Hex.Decode(hexString.Substring(offset)));
	}

	protected bool CanBePrintable(string str)
	{
		return DerPrintableString.IsPrintableString(str);
	}

	public abstract Asn1Object GetConvertedValue(DerObjectIdentifier oid, string value);
}
