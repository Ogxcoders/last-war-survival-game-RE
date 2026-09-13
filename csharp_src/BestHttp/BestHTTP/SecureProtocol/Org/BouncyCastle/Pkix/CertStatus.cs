using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix;

public class CertStatus
{
	public const int Unrevoked = 11;

	public const int Undetermined = 12;

	private int status = 11;

	private DateTimeObject revocationDate;

	public DateTimeObject RevocationDate
	{
		get
		{
			return revocationDate;
		}
		set
		{
			revocationDate = value;
		}
	}

	public int Status
	{
		get
		{
			return status;
		}
		set
		{
			status = value;
		}
	}
}
