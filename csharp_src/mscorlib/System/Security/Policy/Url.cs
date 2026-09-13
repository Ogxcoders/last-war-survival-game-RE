using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Mono.Security;

namespace System.Security.Policy;

[Serializable]
[ComVisible(true)]
public sealed class Url : EvidenceBase, IIdentityPermissionFactory, IBuiltInEvidence
{
	private string origin_url;

	public string Value => origin_url;

	public Url(string name)
		: this(name, validated: false)
	{
	}

	internal Url(string name, bool validated)
	{
		origin_url = (validated ? name : Prepare(name));
	}

	public object Copy()
	{
		return new Url(origin_url, validated: true);
	}

	public IPermission CreateIdentityPermission(Evidence evidence)
	{
		return new UrlIdentityPermission(origin_url);
	}

	public override bool Equals(object o)
	{
		if (!(o is Url { Value: var text }))
		{
			return false;
		}
		string text2 = origin_url;
		if (text.IndexOf(Uri.SchemeDelimiter) < 0)
		{
			text = "file://" + text;
		}
		if (text2.IndexOf(Uri.SchemeDelimiter) < 0)
		{
			text2 = "file://" + text2;
		}
		return string.Compare(text, text2, ignoreCase: true, CultureInfo.InvariantCulture) == 0;
	}

	public override int GetHashCode()
	{
		string text = origin_url;
		if (text.IndexOf(Uri.SchemeDelimiter) < 0)
		{
			text = "file://" + text;
		}
		return text.GetHashCode();
	}

	public override string ToString()
	{
		SecurityElement securityElement = new SecurityElement("System.Security.Policy.Url");
		securityElement.AddAttribute("version", "1");
		securityElement.AddChild(new SecurityElement("Url", origin_url));
		return securityElement.ToString();
	}

	int IBuiltInEvidence.GetRequiredSize(bool verbose)
	{
		return ((!verbose) ? 1 : 3) + origin_url.Length;
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

	private string Prepare(string url)
	{
		if (url == null)
		{
			throw new ArgumentNullException("Url");
		}
		if (url == string.Empty)
		{
			throw new FormatException(Locale.GetText("Invalid (empty) Url"));
		}
		if (url.IndexOf(Uri.SchemeDelimiter) > 0)
		{
			if (url.StartsWith("file://"))
			{
				url = "file://" + url.Substring(7);
			}
			url = new Uri(url, dontEscape: false, reduce: false).ToString();
		}
		int num = url.Length - 1;
		if (url[num] == '/')
		{
			url = url.Substring(0, num);
		}
		return url;
	}
}
