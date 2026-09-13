using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001;

[Serializable]
[ComVisible(true)]
public sealed class SoapDay : ISoapXsd
{
	private static readonly string[] _datetimeFormats = new string[2] { "---dd", "---ddzzz" };

	private DateTime _value;

	public DateTime Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}

	public static string XsdType => "gDay";

	public SoapDay()
	{
	}

	public SoapDay(DateTime value)
	{
		_value = value;
	}

	public string GetXsdType()
	{
		return XsdType;
	}

	public static SoapDay Parse(string value)
	{
		return new SoapDay(DateTime.ParseExact(value, _datetimeFormats, null, DateTimeStyles.None));
	}

	public override string ToString()
	{
		return _value.ToString("---dd", CultureInfo.InvariantCulture);
	}
}
