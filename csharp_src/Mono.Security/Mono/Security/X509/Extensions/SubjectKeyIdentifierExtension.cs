using System;
using System.Globalization;
using System.Text;

namespace Mono.Security.X509.Extensions;

public class SubjectKeyIdentifierExtension : X509Extension
{
	private byte[] ski;

	public override string Name => "Subject Key Identifier";

	public byte[] Identifier
	{
		get
		{
			if (ski == null)
			{
				return null;
			}
			return (byte[])ski.Clone();
		}
		set
		{
			ski = value;
		}
	}

	public SubjectKeyIdentifierExtension()
	{
		extnOid = "2.5.29.14";
	}

	public SubjectKeyIdentifierExtension(ASN1 asn1)
		: base(asn1)
	{
	}

	public SubjectKeyIdentifierExtension(X509Extension extension)
		: base(extension)
	{
	}

	protected override void Decode()
	{
		ASN1 aSN = new ASN1(extnValue.Value);
		if (aSN.Tag != 4)
		{
			throw new ArgumentException("Invalid SubjectKeyIdentifier extension");
		}
		ski = aSN.Value;
	}

	protected override void Encode()
	{
		if (ski == null)
		{
			throw new InvalidOperationException("Invalid SubjectKeyIdentifier extension");
		}
		ASN1 asn = new ASN1(4, ski);
		extnValue = new ASN1(4);
		extnValue.Add(asn);
	}

	public override string ToString()
	{
		if (ski == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < ski.Length; i++)
		{
			stringBuilder.Append(ski[i].ToString("X2", CultureInfo.InvariantCulture));
			if (i % 2 == 1)
			{
				stringBuilder.Append(" ");
			}
		}
		return stringBuilder.ToString();
	}
}
