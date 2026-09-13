using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Net;

internal abstract class WebProxyDataBuilder
{
	private const char addressListDelimiter = ';';

	private const char addressListSchemeValueDelimiter = '=';

	private const char bypassListDelimiter = ';';

	private WebProxyData m_Result;

	private const string regexReserved = "#$()+.?[\\^{|";

	public WebProxyData Build()
	{
		m_Result = new WebProxyData();
		BuildInternal();
		return m_Result;
	}

	protected abstract void BuildInternal();

	protected void SetProxyAndBypassList(string addressString, string bypassListString)
	{
		if (addressString == null)
		{
			return;
		}
		addressString = addressString.Trim();
		if (!(addressString != string.Empty))
		{
			return;
		}
		if (addressString.IndexOf('=') == -1)
		{
			m_Result.proxyAddress = ParseProxyUri(addressString);
		}
		else
		{
			m_Result.proxyHostAddresses = ParseProtocolProxies(addressString);
		}
		if (bypassListString != null)
		{
			bypassListString = bypassListString.Trim();
			if (bypassListString != string.Empty)
			{
				bool bypassOnLocal = false;
				m_Result.bypassList = ParseBypassList(bypassListString, out bypassOnLocal);
				m_Result.bypassOnLocal = bypassOnLocal;
			}
		}
	}

	protected void SetAutoProxyUrl(string autoConfigUrl)
	{
		if (!string.IsNullOrEmpty(autoConfigUrl))
		{
			Uri result = null;
			if (Uri.TryCreate(autoConfigUrl, UriKind.Absolute, out result))
			{
				m_Result.scriptLocation = result;
			}
		}
	}

	protected void SetAutoDetectSettings(bool value)
	{
		m_Result.automaticallyDetectSettings = value;
	}

	private static Uri ParseProxyUri(string proxyString)
	{
		if (proxyString.IndexOf("://") == -1)
		{
			proxyString = "http://" + proxyString;
		}
		try
		{
			return new Uri(proxyString);
		}
		catch (UriFormatException)
		{
			_ = Logging.On;
			throw CreateInvalidProxyStringException(proxyString);
		}
	}

	private static Hashtable ParseProtocolProxies(string proxyListString)
	{
		string[] array = proxyListString.Split(';');
		Hashtable hashtable = new Hashtable(CaseInsensitiveAscii.StaticInstance);
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (!(text == string.Empty))
			{
				string[] array2 = text.Split('=');
				if (array2.Length != 2)
				{
					throw CreateInvalidProxyStringException(proxyListString);
				}
				array2[0] = array2[0].Trim();
				array2[1] = array2[1].Trim();
				if (array2[0] == string.Empty || array2[1] == string.Empty)
				{
					throw CreateInvalidProxyStringException(proxyListString);
				}
				hashtable[array2[0]] = ParseProxyUri(array2[1]);
			}
		}
		return hashtable;
	}

	private static FormatException CreateInvalidProxyStringException(string originalProxyString)
	{
		string message = global::SR.GetString("The system proxy settings contain an invalid proxy server setting: '{0}'.", originalProxyString);
		_ = Logging.On;
		return new FormatException(message);
	}

	private static string BypassStringEscape(string rawString)
	{
		Match match = new Regex("^(?<scheme>.*://)?(?<host>[^:]*)(?<port>:[0-9]{1,5})?$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant).Match(rawString);
		string rawString2;
		string rawString3;
		string rawString4;
		if (match.Success)
		{
			rawString2 = match.Groups["scheme"].Value;
			rawString3 = match.Groups["host"].Value;
			rawString4 = match.Groups["port"].Value;
		}
		else
		{
			rawString2 = string.Empty;
			rawString3 = rawString;
			rawString4 = string.Empty;
		}
		rawString2 = ConvertRegexReservedChars(rawString2);
		rawString3 = ConvertRegexReservedChars(rawString3);
		rawString4 = ConvertRegexReservedChars(rawString4);
		if (rawString2 == string.Empty)
		{
			rawString2 = "(?:.*://)?";
		}
		if (rawString4 == string.Empty)
		{
			rawString4 = "(?::[0-9]{1,5})?";
		}
		return "^" + rawString2 + rawString3 + rawString4 + "$";
	}

	private static string ConvertRegexReservedChars(string rawString)
	{
		if (rawString.Length == 0)
		{
			return rawString;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in rawString)
		{
			if ("#$()+.?[\\^{|".IndexOf(c) != -1)
			{
				stringBuilder.Append('\\');
			}
			else if (c == '*')
			{
				stringBuilder.Append('.');
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}

	private static ArrayList ParseBypassList(string bypassListString, out bool bypassOnLocal)
	{
		string[] array = bypassListString.Split(';');
		bypassOnLocal = false;
		if (array.Length == 0)
		{
			return null;
		}
		ArrayList arrayList = null;
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text == null)
			{
				continue;
			}
			string text2 = text.Trim();
			if (text2.Length <= 0)
			{
				continue;
			}
			if (string.Compare(text2, "<local>", StringComparison.OrdinalIgnoreCase) == 0)
			{
				bypassOnLocal = true;
				continue;
			}
			text2 = BypassStringEscape(text2);
			if (arrayList == null)
			{
				arrayList = new ArrayList();
			}
			if (!arrayList.Contains(text2))
			{
				arrayList.Add(text2);
			}
		}
		return arrayList;
	}
}
