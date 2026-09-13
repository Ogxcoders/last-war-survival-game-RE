using System.Globalization;
using System.Runtime.Serialization;

namespace System.Text;

internal struct SurrogateChar
{
	private char lowChar;

	private char highChar;

	public const int MinValue = 65536;

	public const int MaxValue = 1114111;

	private const char surHighMin = '\ud800';

	private const char surHighMax = '\udbff';

	private const char surLowMin = '\udc00';

	private const char surLowMax = '\udfff';

	public char LowChar => lowChar;

	public char HighChar => highChar;

	public int Char => (lowChar - 56320) | ((highChar - 55296 << 10) + 65536);

	public SurrogateChar(int ch)
	{
		if (ch < 65536 || ch > 1114111)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Surrogate char '0x{0}' not valid. Surrogate chars range from 0x10000 to 0x10FFFF.", ch.ToString("X", CultureInfo.InvariantCulture)), "ch"));
		}
		lowChar = (char)(((ch - 65536) & 0x3FF) + 56320);
		highChar = (char)(((ch - 65536 >> 10) & 0x3FF) + 55296);
	}

	public SurrogateChar(char lowChar, char highChar)
	{
		if (lowChar < '\udc00' || lowChar > '\udfff')
		{
			object[] array = new object[1];
			int num = lowChar;
			array[0] = num.ToString("X", CultureInfo.InvariantCulture);
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Low surrogate char '0x{0}' not valid. Low surrogate chars range from 0xDC00 to 0xDFFF.", array), "lowChar"));
		}
		if (highChar < '\ud800' || highChar > '\udbff')
		{
			object[] array2 = new object[1];
			int num = highChar;
			array2[0] = num.ToString("X", CultureInfo.InvariantCulture);
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("High surrogate char '0x{0}' not valid. High surrogate chars range from 0xD800 to 0xDBFF.", array2), "highChar"));
		}
		this.lowChar = lowChar;
		this.highChar = highChar;
	}
}
