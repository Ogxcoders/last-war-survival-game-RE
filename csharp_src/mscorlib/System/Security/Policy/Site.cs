using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Mono.Security;

namespace System.Security.Policy;

[Serializable]
[ComVisible(true)]
public sealed class Site : EvidenceBase, IIdentityPermissionFactory, IBuiltInEvidence
{
	internal string origin_site;

	public string Name => origin_site;

	public Site(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("url");
		}
		if (!IsValid(name))
		{
			throw new ArgumentException(Locale.GetText("name is not valid"));
		}
		origin_site = name;
	}

	public static Site CreateFromUrl(string url)
	{
		if (url == null)
		{
			throw new ArgumentNullException("url");
		}
		if (url.Length == 0)
		{
			throw new FormatException(Locale.GetText("Empty URL."));
		}
		return new Site(UrlToSite(url) ?? throw new ArgumentException(string.Format(Locale.GetText("Invalid URL '{0}'."), url), "url"));
	}

	public object Copy()
	{
		return new Site(origin_site);
	}

	public IPermission CreateIdentityPermission(Evidence evidence)
	{
		return new SiteIdentityPermission(origin_site);
	}

	public override bool Equals(object o)
	{
		if (!(o is Site site))
		{
			return false;
		}
		return string.Compare(site.Name, origin_site, ignoreCase: true, CultureInfo.InvariantCulture) == 0;
	}

	public override int GetHashCode()
	{
		return origin_site.GetHashCode();
	}

	public override string ToString()
	{
		SecurityElement securityElement = new SecurityElement("System.Security.Policy.Site");
		securityElement.AddAttribute("version", "1");
		securityElement.AddChild(new SecurityElement("Name", origin_site));
		return securityElement.ToString();
	}

	int IBuiltInEvidence.GetRequiredSize(bool verbose)
	{
		return ((!verbose) ? 1 : 3) + origin_site.Length;
	}

	[MonoTODO("IBuiltInEvidence")]
	int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
	{
		return 0;
	}

	[MonoTODO("IBuiltInEvidence")]
	int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
	{
		return 0;
	}

	internal static bool IsValid(string name)
	{
		if (name == string.Empty)
		{
			return false;
		}
		if (name.Length == 1 && name == ".")
		{
			return false;
		}
		string[] array = name.Split('.');
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i];
			if (i == 0 && text == "*")
			{
				continue;
			}
			string text2 = text;
			for (int j = 0; j < text2.Length; j++)
			{
				int num = Convert.ToInt32(text2[j]);
				switch (num)
				{
				case 33:
				case 35:
				case 36:
				case 37:
				case 38:
				case 39:
				case 40:
				case 41:
				case 45:
					continue;
				}
				if ((num < 48 || num > 57) && (num < 64 || num > 90) && (num < 94 || num > 95) && (num < 97 || num > 123) && (num < 125 || num > 126))
				{
					return false;
				}
			}
		}
		return true;
	}

	internal static string UrlToSite(string url)
	{
		if (url == null)
		{
			return null;
		}
		Uri uri = new Uri(url);
		if (uri.Scheme == Uri.UriSchemeFile)
		{
			return null;
		}
		string host = uri.Host;
		if (!IsValid(host))
		{
			return null;
		}
		return host;
	}
}
