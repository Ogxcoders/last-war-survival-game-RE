using System.Collections.Specialized;
using System.Text;

namespace WebSocketSharp.Net;

internal sealed class QueryStringCollection : NameValueCollection
{
	public override string ToString()
	{
		if (Count == 0)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		string[] allKeys = AllKeys;
		foreach (string text in allKeys)
		{
			stringBuilder.AppendFormat("{0}={1}&", text, base[text]);
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder.Length--;
		}
		return stringBuilder.ToString();
	}
}
