namespace Mono.Security.X509;

internal class SafeBag
{
	private string _bagOID;

	private ASN1 _asn1;

	public string BagOID => _bagOID;

	public ASN1 ASN1 => _asn1;

	public SafeBag(string bagOID, ASN1 asn1)
	{
		_bagOID = bagOID;
		_asn1 = asn1;
	}
}
