using System.Text.RegularExpressions;

namespace System.Net;

[Serializable]
internal class DelayedRegex
{
	private Regex _AsRegex;

	private string _AsString;

	internal Regex AsRegex
	{
		get
		{
			if (_AsRegex == null)
			{
				_AsRegex = new Regex(_AsString + "[/]?", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
			}
			return _AsRegex;
		}
	}

	internal DelayedRegex(string regexString)
	{
		if (regexString == null)
		{
			throw new ArgumentNullException("regexString");
		}
		_AsString = regexString;
	}

	internal DelayedRegex(Regex regex)
	{
		if (regex == null)
		{
			throw new ArgumentNullException("regex");
		}
		_AsRegex = regex;
	}

	public override string ToString()
	{
		if (_AsString == null)
		{
			return _AsString = _AsRegex.ToString();
		}
		return _AsString;
	}
}
