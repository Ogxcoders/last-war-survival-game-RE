using System.Globalization;
using System.Text;

namespace System.Xml;

internal class CharEntityEncoderFallbackBuffer : EncoderFallbackBuffer
{
	private CharEntityEncoderFallback parent;

	private string charEntity = string.Empty;

	private int charEntityIndex = -1;

	public override int Remaining
	{
		get
		{
			if (charEntityIndex == -1)
			{
				return 0;
			}
			return charEntity.Length - charEntityIndex;
		}
	}

	internal CharEntityEncoderFallbackBuffer(CharEntityEncoderFallback parent)
	{
		this.parent = parent;
	}

	public override bool Fallback(char charUnknown, int index)
	{
		if (charEntityIndex >= 0)
		{
			new EncoderExceptionFallback().CreateFallbackBuffer().Fallback(charUnknown, index);
		}
		if (parent.CanReplaceAt(index))
		{
			charEntity = string.Format(CultureInfo.InvariantCulture, "&#x{0:X};", new object[1] { (int)charUnknown });
			charEntityIndex = 0;
			return true;
		}
		new EncoderExceptionFallback().CreateFallbackBuffer().Fallback(charUnknown, index);
		return false;
	}

	public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index)
	{
		if (!char.IsSurrogatePair(charUnknownHigh, charUnknownLow))
		{
			throw XmlConvert.CreateInvalidSurrogatePairException(charUnknownHigh, charUnknownLow);
		}
		if (charEntityIndex >= 0)
		{
			new EncoderExceptionFallback().CreateFallbackBuffer().Fallback(charUnknownHigh, charUnknownLow, index);
		}
		if (parent.CanReplaceAt(index))
		{
			charEntity = string.Format(CultureInfo.InvariantCulture, "&#x{0:X};", new object[1] { SurrogateCharToUtf32(charUnknownHigh, charUnknownLow) });
			charEntityIndex = 0;
			return true;
		}
		new EncoderExceptionFallback().CreateFallbackBuffer().Fallback(charUnknownHigh, charUnknownLow, index);
		return false;
	}

	public override char GetNextChar()
	{
		if (charEntityIndex == charEntity.Length)
		{
			charEntityIndex = -1;
		}
		if (charEntityIndex == -1)
		{
			return '\0';
		}
		return charEntity[charEntityIndex++];
	}

	public override bool MovePrevious()
	{
		if (charEntityIndex == -1)
		{
			return false;
		}
		if (charEntityIndex > 0)
		{
			charEntityIndex--;
			return true;
		}
		return false;
	}

	public override void Reset()
	{
		charEntityIndex = -1;
	}

	private int SurrogateCharToUtf32(char highSurrogate, char lowSurrogate)
	{
		return XmlCharType.CombineSurrogateChar(lowSurrogate, highSurrogate);
	}
}
