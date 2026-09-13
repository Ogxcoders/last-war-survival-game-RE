using System.Globalization;
using System.IO;
using System.Text;

namespace System.Xml;

internal class XmlTextEncoder
{
	private TextWriter textWriter;

	private bool inAttribute;

	private char quoteChar;

	private StringBuilder attrValue;

	private bool cacheAttrValue;

	private XmlCharType xmlCharType;

	internal char QuoteChar
	{
		set
		{
			quoteChar = value;
		}
	}

	internal string AttributeValue
	{
		get
		{
			if (cacheAttrValue)
			{
				return attrValue.ToString();
			}
			return string.Empty;
		}
	}

	internal XmlTextEncoder(TextWriter textWriter)
	{
		this.textWriter = textWriter;
		quoteChar = '"';
		xmlCharType = XmlCharType.Instance;
	}

	internal void StartAttribute(bool cacheAttrValue)
	{
		inAttribute = true;
		this.cacheAttrValue = cacheAttrValue;
		if (cacheAttrValue)
		{
			if (attrValue == null)
			{
				attrValue = new StringBuilder();
			}
			else
			{
				attrValue.Length = 0;
			}
		}
	}

	internal void EndAttribute()
	{
		if (cacheAttrValue)
		{
			attrValue.Length = 0;
		}
		inAttribute = false;
		cacheAttrValue = false;
	}

	internal void WriteSurrogateChar(char lowChar, char highChar)
	{
		if (!XmlCharType.IsLowSurrogate(lowChar) || !XmlCharType.IsHighSurrogate(highChar))
		{
			throw XmlConvert.CreateInvalidSurrogatePairException(lowChar, highChar);
		}
		textWriter.Write(highChar);
		textWriter.Write(lowChar);
	}

	internal void Write(char[] array, int offset, int count)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (0 > offset)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (0 > count)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (count > array.Length - offset)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (cacheAttrValue)
		{
			attrValue.Append(array, offset, count);
		}
		int num = offset + count;
		int i = offset;
		char c = '\0';
		while (true)
		{
			int num2 = i;
			for (; i < num; i++)
			{
				if ((xmlCharType.charProperties[(uint)(c = array[i])] & 0x80) == 0)
				{
					break;
				}
			}
			if (num2 < i)
			{
				textWriter.Write(array, num2, i - num2);
			}
			if (i == num)
			{
				break;
			}
			switch (c)
			{
			case '\t':
				textWriter.Write(c);
				break;
			case '\n':
			case '\r':
				if (inAttribute)
				{
					WriteCharEntityImpl(c);
				}
				else
				{
					textWriter.Write(c);
				}
				break;
			case '<':
				WriteEntityRefImpl("lt");
				break;
			case '>':
				WriteEntityRefImpl("gt");
				break;
			case '&':
				WriteEntityRefImpl("amp");
				break;
			case '\'':
				if (inAttribute && quoteChar == c)
				{
					WriteEntityRefImpl("apos");
				}
				else
				{
					textWriter.Write('\'');
				}
				break;
			case '"':
				if (inAttribute && quoteChar == c)
				{
					WriteEntityRefImpl("quot");
				}
				else
				{
					textWriter.Write('"');
				}
				break;
			default:
				if (XmlCharType.IsHighSurrogate(c))
				{
					if (i + 1 >= num)
					{
						throw new ArgumentException(Res.GetString("The second character surrogate pair is not in the input buffer to be written."));
					}
					WriteSurrogateChar(array[++i], c);
				}
				else
				{
					if (XmlCharType.IsLowSurrogate(c))
					{
						throw XmlConvert.CreateInvalidHighSurrogateCharException(c);
					}
					WriteCharEntityImpl(c);
				}
				break;
			}
			i++;
		}
	}

	internal void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		if (!XmlCharType.IsLowSurrogate(lowChar) || !XmlCharType.IsHighSurrogate(highChar))
		{
			throw XmlConvert.CreateInvalidSurrogatePairException(lowChar, highChar);
		}
		int num = XmlCharType.CombineSurrogateChar(lowChar, highChar);
		if (cacheAttrValue)
		{
			attrValue.Append(highChar);
			attrValue.Append(lowChar);
		}
		textWriter.Write("&#x");
		textWriter.Write(num.ToString("X", NumberFormatInfo.InvariantInfo));
		textWriter.Write(';');
	}

	internal void Write(string text)
	{
		if (text == null)
		{
			return;
		}
		if (cacheAttrValue)
		{
			attrValue.Append(text);
		}
		int length = text.Length;
		int i = 0;
		int num = 0;
		char c = '\0';
		while (true)
		{
			if (i < length && (xmlCharType.charProperties[(uint)(c = text[i])] & 0x80) != 0)
			{
				i++;
				continue;
			}
			if (i == length)
			{
				textWriter.Write(text);
				return;
			}
			if (inAttribute)
			{
				if (c != '\t')
				{
					break;
				}
				i++;
			}
			else
			{
				if (c != '\t' && c != '\n' && c != '\r' && c != '"' && c != '\'')
				{
					break;
				}
				i++;
			}
		}
		char[] helperBuffer = new char[256];
		while (true)
		{
			if (num < i)
			{
				WriteStringFragment(text, num, i - num, helperBuffer);
			}
			if (i == length)
			{
				break;
			}
			switch (c)
			{
			case '\t':
				textWriter.Write(c);
				break;
			case '\n':
			case '\r':
				if (inAttribute)
				{
					WriteCharEntityImpl(c);
				}
				else
				{
					textWriter.Write(c);
				}
				break;
			case '<':
				WriteEntityRefImpl("lt");
				break;
			case '>':
				WriteEntityRefImpl("gt");
				break;
			case '&':
				WriteEntityRefImpl("amp");
				break;
			case '\'':
				if (inAttribute && quoteChar == c)
				{
					WriteEntityRefImpl("apos");
				}
				else
				{
					textWriter.Write('\'');
				}
				break;
			case '"':
				if (inAttribute && quoteChar == c)
				{
					WriteEntityRefImpl("quot");
				}
				else
				{
					textWriter.Write('"');
				}
				break;
			default:
				if (XmlCharType.IsHighSurrogate(c))
				{
					if (i + 1 >= length)
					{
						throw XmlConvert.CreateInvalidSurrogatePairException(text[i], c);
					}
					WriteSurrogateChar(text[++i], c);
				}
				else
				{
					if (XmlCharType.IsLowSurrogate(c))
					{
						throw XmlConvert.CreateInvalidHighSurrogateCharException(c);
					}
					WriteCharEntityImpl(c);
				}
				break;
			}
			i++;
			num = i;
			for (; i < length; i++)
			{
				if ((xmlCharType.charProperties[(uint)(c = text[i])] & 0x80) == 0)
				{
					break;
				}
			}
		}
	}

	internal void WriteRawWithSurrogateChecking(string text)
	{
		if (text == null)
		{
			return;
		}
		if (cacheAttrValue)
		{
			attrValue.Append(text);
		}
		int length = text.Length;
		int num = 0;
		char c = '\0';
		while (true)
		{
			if (num < length && ((xmlCharType.charProperties[(uint)(c = text[num])] & 0x10) != 0 || c < ' '))
			{
				num++;
				continue;
			}
			if (num == length)
			{
				break;
			}
			if (XmlCharType.IsHighSurrogate(c))
			{
				if (num + 1 >= length)
				{
					throw new ArgumentException(Res.GetString("The surrogate pair is invalid. Missing a low surrogate character."));
				}
				char c2 = text[num + 1];
				if (!XmlCharType.IsLowSurrogate(c2))
				{
					throw XmlConvert.CreateInvalidSurrogatePairException(c2, c);
				}
				num += 2;
			}
			else
			{
				if (XmlCharType.IsLowSurrogate(c))
				{
					throw XmlConvert.CreateInvalidHighSurrogateCharException(c);
				}
				num++;
			}
		}
		textWriter.Write(text);
	}

	internal void WriteRaw(string value)
	{
		if (cacheAttrValue)
		{
			attrValue.Append(value);
		}
		textWriter.Write(value);
	}

	internal void WriteRaw(char[] array, int offset, int count)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (0 > count)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (0 > offset)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count > array.Length - offset)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (cacheAttrValue)
		{
			attrValue.Append(array, offset, count);
		}
		textWriter.Write(array, offset, count);
	}

	internal void WriteCharEntity(char ch)
	{
		if (XmlCharType.IsSurrogate(ch))
		{
			throw new ArgumentException(Res.GetString("The surrogate pair is invalid. Missing a low surrogate character."));
		}
		int num = ch;
		string text = num.ToString("X", NumberFormatInfo.InvariantInfo);
		if (cacheAttrValue)
		{
			attrValue.Append("&#x");
			attrValue.Append(text);
			attrValue.Append(';');
		}
		WriteCharEntityImpl(text);
	}

	internal void WriteEntityRef(string name)
	{
		if (cacheAttrValue)
		{
			attrValue.Append('&');
			attrValue.Append(name);
			attrValue.Append(';');
		}
		WriteEntityRefImpl(name);
	}

	internal void Flush()
	{
	}

	private void WriteStringFragment(string str, int offset, int count, char[] helperBuffer)
	{
		int num = helperBuffer.Length;
		while (count > 0)
		{
			int num2 = count;
			if (num2 > num)
			{
				num2 = num;
			}
			str.CopyTo(offset, helperBuffer, 0, num2);
			textWriter.Write(helperBuffer, 0, num2);
			offset += num2;
			count -= num2;
		}
	}

	private void WriteCharEntityImpl(char ch)
	{
		int num = ch;
		WriteCharEntityImpl(num.ToString("X", NumberFormatInfo.InvariantInfo));
	}

	private void WriteCharEntityImpl(string strVal)
	{
		textWriter.Write("&#x");
		textWriter.Write(strVal);
		textWriter.Write(';');
	}

	private void WriteEntityRefImpl(string name)
	{
		textWriter.Write('&');
		textWriter.Write(name);
		textWriter.Write(';');
	}
}
