using System.Globalization;
using System.Net;

namespace System.Security.Cryptography.X509Certificates;

internal sealed class GeneralNameEncoder
{
	private enum GeneralNameTag : byte
	{
		OtherName = 160,
		Rfc822Name = 129,
		DnsName = 130,
		X400Address = 131,
		DirectoryName = 132,
		EdiPartyName = 133,
		Uri = 134,
		IpAddress = 135,
		RegisteredId = 136
	}

	private static readonly IdnMapping s_idnMapping = new IdnMapping();

	internal byte[][] EncodeEmailAddress(string emailAddress)
	{
		byte[][] array = DerEncoder.SegmentedEncodeIA5String(emailAddress.ToCharArray());
		array[0][0] = 129;
		return array;
	}

	internal byte[][] EncodeDnsName(string dnsName)
	{
		byte[][] array = DerEncoder.SegmentedEncodeIA5String(s_idnMapping.GetAscii(dnsName).ToCharArray());
		array[0][0] = 130;
		return array;
	}

	internal byte[][] EncodeUri(Uri uri)
	{
		byte[][] array = DerEncoder.SegmentedEncodeIA5String(uri.AbsoluteUri.ToCharArray());
		array[0][0] = 134;
		return array;
	}

	internal byte[][] EncodeIpAddress(IPAddress address)
	{
		byte[][] array = DerEncoder.SegmentedEncodeOctetString(address.GetAddressBytes());
		array[0][0] = 135;
		return array;
	}

	internal byte[][] EncodeUserPrincipalName(string upn)
	{
		byte[][] array = DerEncoder.SegmentedEncodeUtf8String(upn.ToCharArray());
		byte[][] array2 = DerEncoder.ConstructSegmentedSequence(array);
		array2[0][0] = 160;
		byte[][] array3 = DerEncoder.ConstructSegmentedSequence(DerEncoder.SegmentedEncodeOid("1.3.6.1.4.1.311.20.2.3"), array2);
		array3[0][0] = 160;
		return array3;
	}
}
