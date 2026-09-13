using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Mono.Btls;

internal static class MonoBtlsUtils
{
	private static byte[] emailOid = new byte[9] { 42, 134, 72, 134, 247, 13, 1, 9, 1 };

	private const X500DistinguishedNameFlags AllFlags = X500DistinguishedNameFlags.Reversed | X500DistinguishedNameFlags.UseSemicolons | X500DistinguishedNameFlags.DoNotUsePlusSign | X500DistinguishedNameFlags.DoNotUseQuotes | X500DistinguishedNameFlags.UseCommas | X500DistinguishedNameFlags.UseNewLines | X500DistinguishedNameFlags.UseUTF8Encoding | X500DistinguishedNameFlags.UseT61Encoding | X500DistinguishedNameFlags.ForceUTF8Encoding;

	public static bool Compare(byte[] a, byte[] b)
	{
		if (a.Length != b.Length)
		{
			return false;
		}
		for (int i = 0; i < a.Length; i++)
		{
			if (a[i] != b[i])
			{
				return false;
			}
		}
		return true;
	}

	private static bool AppendEntry(StringBuilder sb, MonoBtlsX509Name name, int index, string separator, bool quotes)
	{
		MonoBtlsX509NameEntryType monoBtlsX509NameEntryType = name.GetEntryType(index);
		if (monoBtlsX509NameEntryType < MonoBtlsX509NameEntryType.Unknown)
		{
			return false;
		}
		if (monoBtlsX509NameEntryType == MonoBtlsX509NameEntryType.Unknown && Compare(name.GetEntryOidData(index), emailOid))
		{
			monoBtlsX509NameEntryType = MonoBtlsX509NameEntryType.Email;
		}
		int tag;
		string text = name.GetEntryValue(index, out tag);
		if (text == null)
		{
			return false;
		}
		string entryOid = name.GetEntryOid(index);
		if (entryOid == null)
		{
			return false;
		}
		if (sb.Length > 0)
		{
			sb.Append(separator);
		}
		switch (monoBtlsX509NameEntryType)
		{
		case MonoBtlsX509NameEntryType.CountryName:
			sb.Append("C=");
			break;
		case MonoBtlsX509NameEntryType.OrganizationName:
			sb.Append("O=");
			break;
		case MonoBtlsX509NameEntryType.OrganizationalUnitName:
			sb.Append("OU=");
			break;
		case MonoBtlsX509NameEntryType.CommonName:
			sb.Append("CN=");
			break;
		case MonoBtlsX509NameEntryType.LocalityName:
			sb.Append("L=");
			break;
		case MonoBtlsX509NameEntryType.StateOrProvinceName:
			sb.Append("S=");
			break;
		case MonoBtlsX509NameEntryType.StreetAddress:
			sb.Append("STREET=");
			break;
		case MonoBtlsX509NameEntryType.DomainComponent:
			sb.Append("DC=");
			break;
		case MonoBtlsX509NameEntryType.UserId:
			sb.Append("UID=");
			break;
		case MonoBtlsX509NameEntryType.Email:
			sb.Append("E=");
			break;
		case MonoBtlsX509NameEntryType.DnQualifier:
			sb.Append("dnQualifier=");
			break;
		case MonoBtlsX509NameEntryType.Title:
			sb.Append("T=");
			break;
		case MonoBtlsX509NameEntryType.Surname:
			sb.Append("SN=");
			break;
		case MonoBtlsX509NameEntryType.GivenName:
			sb.Append("G=");
			break;
		case MonoBtlsX509NameEntryType.Initial:
			sb.Append("I=");
			break;
		case MonoBtlsX509NameEntryType.SerialNumber:
			sb.Append("SERIALNUMBER=");
			break;
		default:
			sb.Append("OID.");
			sb.Append(entryOid);
			sb.Append("=");
			break;
		}
		char[] anyOf = new char[7] { ',', '+', '"', '\\', '<', '>', ';' };
		if (quotes && tag != 30 && (text.IndexOfAny(anyOf, 0, text.Length) > 0 || text.StartsWith(" ") || text.EndsWith(" ")))
		{
			text = "\"" + text + "\"";
		}
		sb.Append(text);
		return true;
	}

	private static string GetSeparator(X500DistinguishedNameFlags flag)
	{
		if ((flag & X500DistinguishedNameFlags.UseSemicolons) != X500DistinguishedNameFlags.None)
		{
			return "; ";
		}
		if ((flag & X500DistinguishedNameFlags.UseCommas) != X500DistinguishedNameFlags.None)
		{
			return ", ";
		}
		if ((flag & X500DistinguishedNameFlags.UseNewLines) != X500DistinguishedNameFlags.None)
		{
			return Environment.NewLine;
		}
		return ", ";
	}

	public static string FormatName(MonoBtlsX509Name name, X500DistinguishedNameFlags flag)
	{
		if (flag != X500DistinguishedNameFlags.None && (flag & (X500DistinguishedNameFlags.Reversed | X500DistinguishedNameFlags.UseSemicolons | X500DistinguishedNameFlags.DoNotUsePlusSign | X500DistinguishedNameFlags.DoNotUseQuotes | X500DistinguishedNameFlags.UseCommas | X500DistinguishedNameFlags.UseNewLines | X500DistinguishedNameFlags.UseUTF8Encoding | X500DistinguishedNameFlags.UseT61Encoding | X500DistinguishedNameFlags.ForceUTF8Encoding)) == 0)
		{
			throw new ArgumentException("flag");
		}
		if (name.GetEntryCount() == 0)
		{
			return string.Empty;
		}
		bool reversed = (flag & X500DistinguishedNameFlags.Reversed) != 0;
		bool quotes = (flag & X500DistinguishedNameFlags.DoNotUseQuotes) == 0;
		string separator = GetSeparator(flag);
		return FormatName(name, reversed, separator, quotes);
	}

	public static string FormatName(MonoBtlsX509Name name, bool reversed, string separator, bool quotes)
	{
		int entryCount = name.GetEntryCount();
		StringBuilder stringBuilder = new StringBuilder();
		if (reversed)
		{
			for (int num = entryCount - 1; num >= 0; num--)
			{
				AppendEntry(stringBuilder, name, num, separator, quotes);
			}
		}
		else
		{
			for (int i = 0; i < entryCount; i++)
			{
				AppendEntry(stringBuilder, name, i, separator, quotes);
			}
		}
		return stringBuilder.ToString();
	}
}
