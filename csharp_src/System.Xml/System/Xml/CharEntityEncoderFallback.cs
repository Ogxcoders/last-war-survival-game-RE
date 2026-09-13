using System.Text;

namespace System.Xml;

internal class CharEntityEncoderFallback : EncoderFallback
{
	private CharEntityEncoderFallbackBuffer fallbackBuffer;

	private int[] textContentMarks;

	private int endMarkPos;

	private int curMarkPos;

	private int startOffset;

	public override int MaxCharCount => 12;

	internal int StartOffset
	{
		get
		{
			return startOffset;
		}
		set
		{
			startOffset = value;
		}
	}

	internal CharEntityEncoderFallback()
	{
	}

	public override EncoderFallbackBuffer CreateFallbackBuffer()
	{
		if (fallbackBuffer == null)
		{
			fallbackBuffer = new CharEntityEncoderFallbackBuffer(this);
		}
		return fallbackBuffer;
	}

	internal void Reset(int[] textContentMarks, int endMarkPos)
	{
		this.textContentMarks = textContentMarks;
		this.endMarkPos = endMarkPos;
		curMarkPos = 0;
	}

	internal bool CanReplaceAt(int index)
	{
		int i = curMarkPos;
		for (int num = startOffset + index; i < endMarkPos && num >= textContentMarks[i + 1]; i++)
		{
		}
		curMarkPos = i;
		return (i & 1) != 0;
	}
}
