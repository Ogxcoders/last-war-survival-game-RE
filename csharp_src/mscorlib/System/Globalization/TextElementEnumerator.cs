using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Globalization;

[Serializable]
[ComVisible(true)]
public class TextElementEnumerator : IEnumerator
{
	private string str;

	private int index;

	private int startIndex;

	[NonSerialized]
	private int strLen;

	[NonSerialized]
	private int currTextElementLen;

	[OptionalField(VersionAdded = 2)]
	private UnicodeCategory uc;

	[OptionalField(VersionAdded = 2)]
	private int charLen;

	private int endIndex;

	private int nextTextElementLen;

	public object Current => GetTextElement();

	public int ElementIndex
	{
		get
		{
			if (index == startIndex)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Enumeration has not started. Call MoveNext."));
			}
			return index - currTextElementLen;
		}
	}

	internal TextElementEnumerator(string str, int startIndex, int strLen)
	{
		this.str = str;
		this.startIndex = startIndex;
		this.strLen = strLen;
		Reset();
	}

	[OnDeserializing]
	private void OnDeserializing(StreamingContext ctx)
	{
		charLen = -1;
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext ctx)
	{
		strLen = endIndex + 1;
		currTextElementLen = nextTextElementLen;
		if (charLen == -1)
		{
			uc = CharUnicodeInfo.InternalGetUnicodeCategory(str, index, out charLen);
		}
	}

	[OnSerializing]
	private void OnSerializing(StreamingContext ctx)
	{
		endIndex = strLen - 1;
		nextTextElementLen = currTextElementLen;
	}

	public bool MoveNext()
	{
		if (index >= strLen)
		{
			index = strLen + 1;
			return false;
		}
		currTextElementLen = StringInfo.GetCurrentTextElementLen(str, index, strLen, ref uc, ref charLen);
		index += currTextElementLen;
		return true;
	}

	public string GetTextElement()
	{
		if (index == startIndex)
		{
			throw new InvalidOperationException(Environment.GetResourceString("Enumeration has not started. Call MoveNext."));
		}
		if (index > strLen)
		{
			throw new InvalidOperationException(Environment.GetResourceString("Enumeration already finished."));
		}
		return str.Substring(index - currTextElementLen, currTextElementLen);
	}

	public void Reset()
	{
		index = startIndex;
		if (index < strLen)
		{
			uc = CharUnicodeInfo.InternalGetUnicodeCategory(str, index, out charLen);
		}
	}
}
