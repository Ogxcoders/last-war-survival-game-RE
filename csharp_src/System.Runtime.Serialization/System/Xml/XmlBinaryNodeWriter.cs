using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace System.Xml;

internal class XmlBinaryNodeWriter : XmlStreamNodeWriter
{
	private struct AttributeValue
	{
		private string captureText;

		private XmlDictionaryString captureXText;

		private MemoryStream captureStream;

		public void Clear()
		{
			captureText = null;
			captureXText = null;
			captureStream = null;
		}

		public void WriteText(string s)
		{
			if (captureStream != null)
			{
				captureText = XmlConverter.Base64Encoding.GetString(captureStream.GetBuffer(), 0, (int)captureStream.Length);
				captureStream = null;
			}
			if (captureXText != null)
			{
				captureText = captureXText.Value;
				captureXText = null;
			}
			if (captureText == null || captureText.Length == 0)
			{
				captureText = s;
			}
			else
			{
				captureText += s;
			}
		}

		public void WriteText(XmlDictionaryString s)
		{
			if (captureText != null || captureStream != null)
			{
				WriteText(s.Value);
			}
			else
			{
				captureXText = s;
			}
		}

		public void WriteBase64Text(byte[] trailBytes, int trailByteCount, byte[] buffer, int offset, int count)
		{
			if (captureText != null || captureXText != null)
			{
				if (trailByteCount > 0)
				{
					WriteText(XmlConverter.Base64Encoding.GetString(trailBytes, 0, trailByteCount));
				}
				WriteText(XmlConverter.Base64Encoding.GetString(buffer, offset, count));
				return;
			}
			if (captureStream == null)
			{
				captureStream = new MemoryStream();
			}
			if (trailByteCount > 0)
			{
				captureStream.Write(trailBytes, 0, trailByteCount);
			}
			captureStream.Write(buffer, offset, count);
		}

		public void WriteTo(XmlBinaryNodeWriter writer)
		{
			if (captureText != null)
			{
				writer.WriteText(captureText);
				captureText = null;
			}
			else if (captureXText != null)
			{
				writer.WriteText(captureXText);
				captureXText = null;
			}
			else if (captureStream != null)
			{
				writer.WriteBase64Text(null, 0, captureStream.GetBuffer(), 0, (int)captureStream.Length);
				captureStream = null;
			}
			else
			{
				writer.WriteEmptyText();
			}
		}
	}

	private IXmlDictionary dictionary;

	private XmlBinaryWriterSession session;

	private bool inAttribute;

	private bool inList;

	private bool wroteAttributeValue;

	private AttributeValue attributeValue;

	private const int maxBytesPerChar = 3;

	private int textNodeOffset;

	public void SetOutput(Stream stream, IXmlDictionary dictionary, XmlBinaryWriterSession session, bool ownsStream)
	{
		this.dictionary = dictionary;
		this.session = session;
		inAttribute = false;
		inList = false;
		attributeValue.Clear();
		textNodeOffset = -1;
		SetOutput(stream, ownsStream, null);
	}

	private void WriteNode(XmlBinaryNodeType nodeType)
	{
		WriteByte((byte)nodeType);
		textNodeOffset = -1;
	}

	private void WroteAttributeValue()
	{
		if (wroteAttributeValue && !inList)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Only a single typed value may be written inside an attribute or content.")));
		}
		wroteAttributeValue = true;
	}

	private void WriteTextNode(XmlBinaryNodeType nodeType)
	{
		if (inAttribute)
		{
			WroteAttributeValue();
		}
		WriteByte((byte)nodeType);
		textNodeOffset = base.BufferOffset - 1;
	}

	private byte[] GetTextNodeBuffer(int size, out int offset)
	{
		if (inAttribute)
		{
			WroteAttributeValue();
		}
		byte[] result = GetBuffer(size, out offset);
		textNodeOffset = offset;
		return result;
	}

	private void WriteTextNodeWithLength(XmlBinaryNodeType nodeType, int length)
	{
		int num;
		byte[] textNodeBuffer = GetTextNodeBuffer(5, out num);
		if (length < 256)
		{
			textNodeBuffer[num] = (byte)nodeType;
			textNodeBuffer[num + 1] = (byte)length;
			Advance(2);
		}
		else if (length < 65536)
		{
			textNodeBuffer[num] = (byte)(nodeType + 2);
			textNodeBuffer[num + 1] = (byte)length;
			length >>= 8;
			textNodeBuffer[num + 2] = (byte)length;
			Advance(3);
		}
		else
		{
			textNodeBuffer[num] = (byte)(nodeType + 4);
			textNodeBuffer[num + 1] = (byte)length;
			length >>= 8;
			textNodeBuffer[num + 2] = (byte)length;
			length >>= 8;
			textNodeBuffer[num + 3] = (byte)length;
			length >>= 8;
			textNodeBuffer[num + 4] = (byte)length;
			Advance(5);
		}
	}

	private void WriteTextNodeWithInt64(XmlBinaryNodeType nodeType, long value)
	{
		int num;
		byte[] textNodeBuffer = GetTextNodeBuffer(9, out num);
		textNodeBuffer[num] = (byte)nodeType;
		textNodeBuffer[num + 1] = (byte)value;
		value >>= 8;
		textNodeBuffer[num + 2] = (byte)value;
		value >>= 8;
		textNodeBuffer[num + 3] = (byte)value;
		value >>= 8;
		textNodeBuffer[num + 4] = (byte)value;
		value >>= 8;
		textNodeBuffer[num + 5] = (byte)value;
		value >>= 8;
		textNodeBuffer[num + 6] = (byte)value;
		value >>= 8;
		textNodeBuffer[num + 7] = (byte)value;
		value >>= 8;
		textNodeBuffer[num + 8] = (byte)value;
		Advance(9);
	}

	public override void WriteDeclaration()
	{
	}

	public override void WriteStartElement(string prefix, string localName)
	{
		if (prefix.Length == 0)
		{
			WriteNode(XmlBinaryNodeType.MinElement);
			WriteName(localName);
			return;
		}
		char c = prefix[0];
		if (prefix.Length == 1 && c >= 'a' && c <= 'z')
		{
			WritePrefixNode(XmlBinaryNodeType.PrefixElementA, c - 97);
			WriteName(localName);
		}
		else
		{
			WriteNode(XmlBinaryNodeType.Element);
			WriteName(prefix);
			WriteName(localName);
		}
	}

	private void WritePrefixNode(XmlBinaryNodeType nodeType, int ch)
	{
		WriteNode(nodeType + ch);
	}

	public override void WriteStartElement(string prefix, XmlDictionaryString localName)
	{
		if (!TryGetKey(localName, out var key))
		{
			WriteStartElement(prefix, localName.Value);
			return;
		}
		if (prefix.Length == 0)
		{
			WriteNode(XmlBinaryNodeType.ShortDictionaryElement);
			WriteDictionaryString(localName, key);
			return;
		}
		char c = prefix[0];
		if (prefix.Length == 1 && c >= 'a' && c <= 'z')
		{
			WritePrefixNode(XmlBinaryNodeType.PrefixDictionaryElementA, c - 97);
			WriteDictionaryString(localName, key);
		}
		else
		{
			WriteNode(XmlBinaryNodeType.DictionaryElement);
			WriteName(prefix);
			WriteDictionaryString(localName, key);
		}
	}

	public override void WriteEndStartElement(bool isEmpty)
	{
		if (isEmpty)
		{
			WriteEndElement();
		}
	}

	public override void WriteEndElement(string prefix, string localName)
	{
		WriteEndElement();
	}

	private void WriteEndElement()
	{
		if (textNodeOffset != -1)
		{
			byte[] streamBuffer = base.StreamBuffer;
			XmlBinaryNodeType xmlBinaryNodeType = (XmlBinaryNodeType)streamBuffer[textNodeOffset];
			streamBuffer[textNodeOffset] = (byte)(xmlBinaryNodeType + 1);
			textNodeOffset = -1;
		}
		else
		{
			WriteNode(XmlBinaryNodeType.EndElement);
		}
	}

	public override void WriteStartAttribute(string prefix, string localName)
	{
		if (prefix.Length == 0)
		{
			WriteNode(XmlBinaryNodeType.MinAttribute);
			WriteName(localName);
		}
		else
		{
			char c = prefix[0];
			if (prefix.Length == 1 && c >= 'a' && c <= 'z')
			{
				WritePrefixNode(XmlBinaryNodeType.PrefixAttributeA, c - 97);
				WriteName(localName);
			}
			else
			{
				WriteNode(XmlBinaryNodeType.Attribute);
				WriteName(prefix);
				WriteName(localName);
			}
		}
		inAttribute = true;
		wroteAttributeValue = false;
	}

	public override void WriteStartAttribute(string prefix, XmlDictionaryString localName)
	{
		if (!TryGetKey(localName, out var key))
		{
			WriteStartAttribute(prefix, localName.Value);
			return;
		}
		if (prefix.Length == 0)
		{
			WriteNode(XmlBinaryNodeType.ShortDictionaryAttribute);
			WriteDictionaryString(localName, key);
		}
		else
		{
			char c = prefix[0];
			if (prefix.Length == 1 && c >= 'a' && c <= 'z')
			{
				WritePrefixNode(XmlBinaryNodeType.PrefixDictionaryAttributeA, c - 97);
				WriteDictionaryString(localName, key);
			}
			else
			{
				WriteNode(XmlBinaryNodeType.DictionaryAttribute);
				WriteName(prefix);
				WriteDictionaryString(localName, key);
			}
		}
		inAttribute = true;
		wroteAttributeValue = false;
	}

	public override void WriteEndAttribute()
	{
		inAttribute = false;
		if (!wroteAttributeValue)
		{
			attributeValue.WriteTo(this);
		}
		textNodeOffset = -1;
	}

	public override void WriteXmlnsAttribute(string prefix, string ns)
	{
		if (prefix.Length == 0)
		{
			WriteNode(XmlBinaryNodeType.ShortXmlnsAttribute);
			WriteName(ns);
		}
		else
		{
			WriteNode(XmlBinaryNodeType.XmlnsAttribute);
			WriteName(prefix);
			WriteName(ns);
		}
	}

	public override void WriteXmlnsAttribute(string prefix, XmlDictionaryString ns)
	{
		if (!TryGetKey(ns, out var key))
		{
			WriteXmlnsAttribute(prefix, ns.Value);
		}
		else if (prefix.Length == 0)
		{
			WriteNode(XmlBinaryNodeType.ShortDictionaryXmlnsAttribute);
			WriteDictionaryString(ns, key);
		}
		else
		{
			WriteNode(XmlBinaryNodeType.DictionaryXmlnsAttribute);
			WriteName(prefix);
			WriteDictionaryString(ns, key);
		}
	}

	private bool TryGetKey(XmlDictionaryString s, out int key)
	{
		key = -1;
		if (s.Dictionary == dictionary)
		{
			key = s.Key * 2;
			return true;
		}
		if (dictionary != null && dictionary.TryLookup(s, out var result))
		{
			key = result.Key * 2;
			return true;
		}
		if (session == null)
		{
			return false;
		}
		if (!session.TryLookup(s, out var key2) && !session.TryAdd(s, out key2))
		{
			return false;
		}
		key = key2 * 2 + 1;
		return true;
	}

	private void WriteDictionaryString(XmlDictionaryString s, int key)
	{
		WriteMultiByteInt32(key);
	}

	[SecuritySafeCritical]
	private unsafe void WriteName(string s)
	{
		int length = s.Length;
		if (length == 0)
		{
			WriteByte(0);
			return;
		}
		fixed (char* chars = s)
		{
			UnsafeWriteName(chars, length);
		}
	}

	[SecurityCritical]
	private unsafe void UnsafeWriteName(char* chars, int charCount)
	{
		if (charCount < 42)
		{
			int num;
			byte[] array = GetBuffer(1 + charCount * 3, out num);
			int num2 = UnsafeGetUTF8Chars(chars, charCount, array, num + 1);
			array[num] = (byte)num2;
			Advance(1 + num2);
		}
		else
		{
			int i = UnsafeGetUTF8Length(chars, charCount);
			WriteMultiByteInt32(i);
			UnsafeWriteUTF8Chars(chars, charCount);
		}
	}

	private void WriteMultiByteInt32(int i)
	{
		int num;
		byte[] array = GetBuffer(5, out num);
		int num2 = num;
		while ((i & 0xFFFFFF80u) != 0L)
		{
			array[num++] = (byte)((i & 0x7F) | 0x80);
			i >>= 7;
		}
		array[num++] = (byte)i;
		Advance(num - num2);
	}

	public override void WriteComment(string value)
	{
		WriteNode(XmlBinaryNodeType.Comment);
		WriteName(value);
	}

	public override void WriteCData(string value)
	{
		WriteText(value);
	}

	private void WriteEmptyText()
	{
		WriteTextNode(XmlBinaryNodeType.EmptyText);
	}

	public override void WriteBoolText(bool value)
	{
		if (value)
		{
			WriteTextNode(XmlBinaryNodeType.TrueText);
		}
		else
		{
			WriteTextNode(XmlBinaryNodeType.FalseText);
		}
	}

	public override void WriteInt32Text(int value)
	{
		if (value >= -128 && value < 128)
		{
			switch (value)
			{
			case 0:
				WriteTextNode(XmlBinaryNodeType.MinText);
				return;
			case 1:
				WriteTextNode(XmlBinaryNodeType.OneText);
				return;
			}
			int num;
			byte[] textNodeBuffer = GetTextNodeBuffer(2, out num);
			textNodeBuffer[num] = 136;
			textNodeBuffer[num + 1] = (byte)value;
			Advance(2);
		}
		else if (value >= -32768 && value < 32768)
		{
			int num2;
			byte[] textNodeBuffer2 = GetTextNodeBuffer(3, out num2);
			textNodeBuffer2[num2] = 138;
			textNodeBuffer2[num2 + 1] = (byte)value;
			value >>= 8;
			textNodeBuffer2[num2 + 2] = (byte)value;
			Advance(3);
		}
		else
		{
			int num3;
			byte[] textNodeBuffer3 = GetTextNodeBuffer(5, out num3);
			textNodeBuffer3[num3] = 140;
			textNodeBuffer3[num3 + 1] = (byte)value;
			value >>= 8;
			textNodeBuffer3[num3 + 2] = (byte)value;
			value >>= 8;
			textNodeBuffer3[num3 + 3] = (byte)value;
			value >>= 8;
			textNodeBuffer3[num3 + 4] = (byte)value;
			Advance(5);
		}
	}

	public override void WriteInt64Text(long value)
	{
		if (value >= int.MinValue && value <= int.MaxValue)
		{
			WriteInt32Text((int)value);
		}
		else
		{
			WriteTextNodeWithInt64(XmlBinaryNodeType.Int64Text, value);
		}
	}

	public override void WriteUInt64Text(ulong value)
	{
		if (value <= long.MaxValue)
		{
			WriteInt64Text((long)value);
		}
		else
		{
			WriteTextNodeWithInt64(XmlBinaryNodeType.UInt64Text, (long)value);
		}
	}

	private void WriteInt64(long value)
	{
		int num;
		byte[] array = GetBuffer(8, out num);
		array[num] = (byte)value;
		value >>= 8;
		array[num + 1] = (byte)value;
		value >>= 8;
		array[num + 2] = (byte)value;
		value >>= 8;
		array[num + 3] = (byte)value;
		value >>= 8;
		array[num + 4] = (byte)value;
		value >>= 8;
		array[num + 5] = (byte)value;
		value >>= 8;
		array[num + 6] = (byte)value;
		value >>= 8;
		array[num + 7] = (byte)value;
		Advance(8);
	}

	public override void WriteBase64Text(byte[] trailBytes, int trailByteCount, byte[] base64Buffer, int base64Offset, int base64Count)
	{
		if (inAttribute)
		{
			attributeValue.WriteBase64Text(trailBytes, trailByteCount, base64Buffer, base64Offset, base64Count);
			return;
		}
		int num = trailByteCount + base64Count;
		if (num > 0)
		{
			WriteTextNodeWithLength(XmlBinaryNodeType.Bytes8Text, num);
			if (trailByteCount > 0)
			{
				int num2;
				byte[] array = GetBuffer(trailByteCount, out num2);
				for (int i = 0; i < trailByteCount; i++)
				{
					array[num2 + i] = trailBytes[i];
				}
				Advance(trailByteCount);
			}
			if (base64Count > 0)
			{
				WriteBytes(base64Buffer, base64Offset, base64Count);
			}
		}
		else
		{
			WriteEmptyText();
		}
	}

	public override void WriteText(XmlDictionaryString value)
	{
		if (inAttribute)
		{
			attributeValue.WriteText(value);
			return;
		}
		if (!TryGetKey(value, out var key))
		{
			WriteText(value.Value);
			return;
		}
		WriteTextNode(XmlBinaryNodeType.DictionaryText);
		WriteDictionaryString(value, key);
	}

	[SecuritySafeCritical]
	public unsafe override void WriteText(string value)
	{
		if (inAttribute)
		{
			attributeValue.WriteText(value);
		}
		else if (value.Length > 0)
		{
			fixed (char* chars = value)
			{
				UnsafeWriteText(chars, value.Length);
			}
		}
		else
		{
			WriteEmptyText();
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteText(char[] chars, int offset, int count)
	{
		if (inAttribute)
		{
			attributeValue.WriteText(new string(chars, offset, count));
		}
		else if (count > 0)
		{
			fixed (char* chars2 = &chars[offset])
			{
				UnsafeWriteText(chars2, count);
			}
		}
		else
		{
			WriteEmptyText();
		}
	}

	public override void WriteText(byte[] chars, int charOffset, int charCount)
	{
		WriteTextNodeWithLength(XmlBinaryNodeType.Chars8Text, charCount);
		WriteBytes(chars, charOffset, charCount);
	}

	[SecurityCritical]
	private unsafe void UnsafeWriteText(char* chars, int charCount)
	{
		if (charCount == 1)
		{
			switch (*chars)
			{
			case '0':
				WriteTextNode(XmlBinaryNodeType.MinText);
				return;
			case '1':
				WriteTextNode(XmlBinaryNodeType.OneText);
				return;
			}
		}
		if (charCount <= 85)
		{
			int num;
			byte[] array = GetBuffer(2 + charCount * 3, out num);
			int num2 = UnsafeGetUTF8Chars(chars, charCount, array, num + 2);
			if (num2 / 2 <= charCount)
			{
				array[num] = 152;
			}
			else
			{
				array[num] = 182;
				num2 = UnsafeGetUnicodeChars(chars, charCount, array, num + 2);
			}
			textNodeOffset = num;
			array[num + 1] = (byte)num2;
			Advance(2 + num2);
		}
		else
		{
			int num3 = UnsafeGetUTF8Length(chars, charCount);
			if (num3 / 2 > charCount)
			{
				WriteTextNodeWithLength(XmlBinaryNodeType.UnicodeChars8Text, charCount * 2);
				UnsafeWriteUnicodeChars(chars, charCount);
			}
			else
			{
				WriteTextNodeWithLength(XmlBinaryNodeType.Chars8Text, num3);
				UnsafeWriteUTF8Chars(chars, charCount);
			}
		}
	}

	public override void WriteEscapedText(string value)
	{
		WriteText(value);
	}

	public override void WriteEscapedText(XmlDictionaryString value)
	{
		WriteText(value);
	}

	public override void WriteEscapedText(char[] chars, int offset, int count)
	{
		WriteText(chars, offset, count);
	}

	public override void WriteEscapedText(byte[] chars, int offset, int count)
	{
		WriteText(chars, offset, count);
	}

	public override void WriteCharEntity(int ch)
	{
		if (ch > 65535)
		{
			SurrogateChar surrogateChar = new SurrogateChar(ch);
			char[] chars = new char[2] { surrogateChar.HighChar, surrogateChar.LowChar };
			WriteText(chars, 0, 2);
		}
		else
		{
			char[] chars2 = new char[1] { (char)ch };
			WriteText(chars2, 0, 1);
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteFloatText(float f)
	{
		long value;
		if (f >= -9.223372E+18f && f <= 9.223372E+18f && (float)(value = (long)f) == f)
		{
			WriteInt64Text(value);
			return;
		}
		int num;
		byte[] textNodeBuffer = GetTextNodeBuffer(5, out num);
		byte* ptr = (byte*)(&f);
		textNodeBuffer[num] = 144;
		textNodeBuffer[num + 1] = *ptr;
		textNodeBuffer[num + 2] = ptr[1];
		textNodeBuffer[num + 3] = ptr[2];
		textNodeBuffer[num + 4] = ptr[3];
		Advance(5);
	}

	[SecuritySafeCritical]
	public unsafe override void WriteDoubleText(double d)
	{
		float value;
		if (d >= -3.4028234663852886E+38 && d <= 3.4028234663852886E+38 && (double)(value = (float)d) == d)
		{
			WriteFloatText(value);
			return;
		}
		int num;
		byte[] textNodeBuffer = GetTextNodeBuffer(9, out num);
		byte* ptr = (byte*)(&d);
		textNodeBuffer[num] = 146;
		textNodeBuffer[num + 1] = *ptr;
		textNodeBuffer[num + 2] = ptr[1];
		textNodeBuffer[num + 3] = ptr[2];
		textNodeBuffer[num + 4] = ptr[3];
		textNodeBuffer[num + 5] = ptr[4];
		textNodeBuffer[num + 6] = ptr[5];
		textNodeBuffer[num + 7] = ptr[6];
		textNodeBuffer[num + 8] = ptr[7];
		Advance(9);
	}

	[SecuritySafeCritical]
	public unsafe override void WriteDecimalText(decimal d)
	{
		int num;
		byte[] textNodeBuffer = GetTextNodeBuffer(17, out num);
		byte* ptr = (byte*)(&d);
		textNodeBuffer[num++] = 148;
		for (int i = 0; i < 16; i++)
		{
			textNodeBuffer[num++] = ptr[i];
		}
		Advance(17);
	}

	public override void WriteDateTimeText(DateTime dt)
	{
		WriteTextNodeWithInt64(XmlBinaryNodeType.DateTimeText, dt.ToBinary());
	}

	public override void WriteUniqueIdText(UniqueId value)
	{
		if (value.IsGuid)
		{
			int num;
			byte[] textNodeBuffer = GetTextNodeBuffer(17, out num);
			textNodeBuffer[num] = 172;
			value.TryGetGuid(textNodeBuffer, num + 1);
			Advance(17);
		}
		else
		{
			WriteText(value.ToString());
		}
	}

	public override void WriteGuidText(Guid guid)
	{
		int num;
		byte[] textNodeBuffer = GetTextNodeBuffer(17, out num);
		textNodeBuffer[num] = 176;
		Buffer.BlockCopy(guid.ToByteArray(), 0, textNodeBuffer, num + 1, 16);
		Advance(17);
	}

	public override void WriteTimeSpanText(TimeSpan value)
	{
		WriteTextNodeWithInt64(XmlBinaryNodeType.TimeSpanText, value.Ticks);
	}

	public override void WriteStartListText()
	{
		inList = true;
		WriteNode(XmlBinaryNodeType.StartListText);
	}

	public override void WriteListSeparator()
	{
	}

	public override void WriteEndListText()
	{
		inList = false;
		wroteAttributeValue = true;
		WriteNode(XmlBinaryNodeType.EndListText);
	}

	public void WriteArrayNode()
	{
		WriteNode(XmlBinaryNodeType.Array);
	}

	private void WriteArrayInfo(XmlBinaryNodeType nodeType, int count)
	{
		WriteNode(nodeType);
		WriteMultiByteInt32(count);
	}

	[SecurityCritical]
	public unsafe void UnsafeWriteArray(XmlBinaryNodeType nodeType, int count, byte* array, byte* arrayMax)
	{
		WriteArrayInfo(nodeType, count);
		UnsafeWriteArray(array, (int)(arrayMax - array));
	}

	[SecurityCritical]
	private unsafe void UnsafeWriteArray(byte* array, int byteCount)
	{
		UnsafeWriteBytes(array, byteCount);
	}

	public void WriteDateTimeArray(DateTime[] array, int offset, int count)
	{
		WriteArrayInfo(XmlBinaryNodeType.DateTimeTextWithEndElement, count);
		for (int i = 0; i < count; i++)
		{
			WriteInt64(array[offset + i].ToBinary());
		}
	}

	public void WriteGuidArray(Guid[] array, int offset, int count)
	{
		WriteArrayInfo(XmlBinaryNodeType.GuidTextWithEndElement, count);
		for (int i = 0; i < count; i++)
		{
			byte[] byteBuffer = array[offset + i].ToByteArray();
			WriteBytes(byteBuffer, 0, 16);
		}
	}

	public void WriteTimeSpanArray(TimeSpan[] array, int offset, int count)
	{
		WriteArrayInfo(XmlBinaryNodeType.TimeSpanTextWithEndElement, count);
		for (int i = 0; i < count; i++)
		{
			WriteInt64(array[offset + i].Ticks);
		}
	}

	public override void WriteQualifiedName(string prefix, XmlDictionaryString localName)
	{
		if (prefix.Length == 0)
		{
			WriteText(localName);
			return;
		}
		char c = prefix[0];
		if (prefix.Length == 1 && c >= 'a' && c <= 'z' && TryGetKey(localName, out var key))
		{
			WriteTextNode(XmlBinaryNodeType.QNameDictionaryText);
			WriteByte((byte)(c - 97));
			WriteDictionaryString(localName, key);
		}
		else
		{
			WriteText(prefix);
			WriteText(":");
			WriteText(localName);
		}
	}

	protected override void FlushBuffer()
	{
		base.FlushBuffer();
		textNodeOffset = -1;
	}

	public override void Close()
	{
		base.Close();
		attributeValue.Clear();
	}
}
