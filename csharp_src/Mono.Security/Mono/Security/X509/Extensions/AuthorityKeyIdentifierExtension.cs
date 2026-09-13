using System;
using System.Globalization;
using System.Text;

namespace Mono.Security.X509.Extensions;

public class AuthorityKeyIdentifierExtension : X509Extension
{
	private byte[] aki;

	public override string Name => "Authority Key Identifier";

	public byte[] Identifier
	{
		get
		{
			if (aki == null)
			{
				return null;
			}
			return (byte[])aki.Clone();
		}
		set
		{
			aki = value;
		}
	}

	public AuthorityKeyIdentifierExtension()
	{
		extnOid = "2.5.29.35";
	}

	public AuthorityKeyIdentifierExtension(ASN1 asn1)
		: base(asn1)
	{
	}

	public AuthorityKeyIdentifierExtension(X509Extension extension)
		: base(extension)
	{
	}

	protected override void Decode()
	{
		ASN1 aSN = new ASN1(extnValue.Value);
		if (aSN.Tag != 48)
		{
			throw new ArgumentException("Invalid AuthorityKeyIdentifier extension");
		}
		for (int i = 0; i < aSN.Count; i++)
		{
			ASN1 aSN2 = aSN[i];
			if (aSN2.Tag == 128)
			{
				aki = aSN2.Value;
			}
		}
	}

	protected override void Encode()
	{
		ASN1 aSN = new ASN1(48);
		if (aki == null)
		{
			throw new InvalidOperationException("Invalid AuthorityKeyIdentifier extension");
		}
		aSN.Add(new ASN1(128, aki));
		extnValue = new ASN1(4);
		extnValue.Add(aSN);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (aki != null)
		{
			int i = 0;
			stringBuilder.Append("KeyID=");
			for (; i < aki.Length; i++)
			{
				stringBuilder.Append(aki[i].ToString("X2", CultureInfo.InvariantCulture));
				if (i % 2 == 1)
				{
					stringBuilder.Append(" ");
				}
			}
		}
		return stringBuilder.ToString();
	}
}
