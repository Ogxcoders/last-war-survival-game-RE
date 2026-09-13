namespace System.Xml.Xsl.XsltOld;

internal sealed class PrefixQName
{
	public string Prefix;

	public string Name;

	public string Namespace;

	internal void ClearPrefix()
	{
		Prefix = string.Empty;
	}

	internal void SetQName(string qname)
	{
		ParseQualifiedName(qname, out Prefix, out Name);
	}

	public static void ParseQualifiedName(string qname, out string prefix, out string local)
	{
		prefix = string.Empty;
		local = string.Empty;
		int num = ValidateNames.ParseNCName(qname);
		if (num == 0)
		{
			throw XsltException.Create("'{0}' is an invalid QName.", qname);
		}
		local = qname.Substring(0, num);
		if (num >= qname.Length)
		{
			return;
		}
		if (qname[num] == ':')
		{
			int startIndex = ++num;
			prefix = local;
			int num2 = ValidateNames.ParseNCName(qname, num);
			num += num2;
			if (num2 == 0)
			{
				throw XsltException.Create("'{0}' is an invalid QName.", qname);
			}
			local = qname.Substring(startIndex, num2);
		}
		if (num < qname.Length)
		{
			throw XsltException.Create("'{0}' is an invalid QName.", qname);
		}
	}

	public static bool ValidatePrefix(string prefix)
	{
		if (prefix.Length == 0)
		{
			return false;
		}
		return ValidateNames.ParseNCName(prefix, 0) == prefix.Length;
	}
}
