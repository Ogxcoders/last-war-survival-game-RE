using System.Collections.Generic;
using System.Net;

namespace System.Security.Cryptography.X509Certificates;

public sealed class SubjectAlternativeNameBuilder
{
	private readonly List<byte[][]> _encodedTlvs = new List<byte[][]>();

	private readonly GeneralNameEncoder _generalNameEncoder = new GeneralNameEncoder();

	public void AddEmailAddress(string emailAddress)
	{
		if (string.IsNullOrEmpty(emailAddress))
		{
			throw new ArgumentOutOfRangeException("emailAddress", "String cannot be empty or null.");
		}
		_encodedTlvs.Add(_generalNameEncoder.EncodeEmailAddress(emailAddress));
	}

	public void AddDnsName(string dnsName)
	{
		if (string.IsNullOrEmpty(dnsName))
		{
			throw new ArgumentOutOfRangeException("dnsName", "String cannot be empty or null.");
		}
		_encodedTlvs.Add(_generalNameEncoder.EncodeDnsName(dnsName));
	}

	public void AddUri(Uri uri)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		_encodedTlvs.Add(_generalNameEncoder.EncodeUri(uri));
	}

	public void AddIpAddress(IPAddress ipAddress)
	{
		if (ipAddress == null)
		{
			throw new ArgumentNullException("ipAddress");
		}
		_encodedTlvs.Add(_generalNameEncoder.EncodeIpAddress(ipAddress));
	}

	public void AddUserPrincipalName(string upn)
	{
		if (string.IsNullOrEmpty(upn))
		{
			throw new ArgumentOutOfRangeException("upn", "String cannot be empty or null.");
		}
		_encodedTlvs.Add(_generalNameEncoder.EncodeUserPrincipalName(upn));
	}

	public X509Extension Build(bool critical = false)
	{
		return new X509Extension("2.5.29.17", DerEncoder.ConstructSequence(_encodedTlvs), critical);
	}
}
