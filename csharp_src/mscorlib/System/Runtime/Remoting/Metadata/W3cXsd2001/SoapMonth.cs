using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001;

[Serializable]
[ComVisible(true)]
public sealed class SoapMonth : ISoapXsd
{
	private static readonly string[] _datetimeFormats = new string[2] { "--MM--", "--MM--zzz" };

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

	public static string XsdType => "gMonth";

	public SoapMonth()
	{
	}

	public SoapMonth(DateTime value)
	{
		_value = value;
	}

	public string GetXsdType()
	{
		return XsdType;
	}

	public static SoapMonth Parse(string value)
	{
		return new SoapMonth(DateTime.ParseExact(value, _datetimeFormats, null, DateTimeStyles.None));
	}

	public override string ToString()
	{
		return _value.ToString("--MM--", CultureInfo.InvariantCulture);
	}
}
