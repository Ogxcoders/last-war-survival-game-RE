using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001;

[Serializable]
[ComVisible(true)]
public sealed class SoapMonthDay : ISoapXsd
{
	private static readonly string[] _datetimeFormats = new string[2] { "--MM-dd", "--MM-ddzzz" };

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

	public static string XsdType => "gMonthDay";

	public SoapMonthDay()
	{
	}

	public SoapMonthDay(DateTime value)
	{
		_value = value;
	}

	public string GetXsdType()
	{
		return XsdType;
	}

	public static SoapMonthDay Parse(string value)
	{
		return new SoapMonthDay(DateTime.ParseExact(value, _datetimeFormats, null, DateTimeStyles.None));
	}

	public override string ToString()
	{
		return _value.ToString("--MM-dd", CultureInfo.InvariantCulture);
	}
}
