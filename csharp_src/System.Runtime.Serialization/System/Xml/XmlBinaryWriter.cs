using System.IO;
using System.Runtime.Serialization;
using System.Security;

namespace System.Xml;

internal class XmlBinaryWriter : XmlBaseWriter, IXmlBinaryWriterInitializer
{
	private XmlBinaryNodeWriter writer;

	private char[] chars;

	private byte[] bytes;

	public void SetOutput(Stream stream, IXmlDictionary dictionary, XmlBinaryWriterSession session, bool ownsStream)
	{
		if (stream == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("stream"));
		}
		if (writer == null)
		{
			writer = new XmlBinaryNodeWriter();
		}
		writer.SetOutput(stream, dictionary, session, ownsStream);
		SetOutput(writer);
	}

	protected override XmlSigningNodeWriter CreateSigningNodeWriter()
	{
		return new XmlSigningNodeWriter(text: false);
	}

	protected override void WriteTextNode(XmlDictionaryReader reader, bool attribute)
	{
		Type valueType = reader.ValueType;
		if (valueType == typeof(string))
		{
			if (reader.TryGetValueAsDictionaryString(out var value))
			{
				WriteString(value);
			}
			else if (reader.CanReadValueChunk)
			{
				if (chars == null)
				{
					chars = new char[256];
				}
				int count;
				while ((count = reader.ReadValueChunk(chars, 0, chars.Length)) > 0)
				{
					WriteChars(chars, 0, count);
				}
			}
			else
			{
				WriteString(reader.Value);
			}
			if (!attribute)
			{
				reader.Read();
			}
		}
		else if (valueType == typeof(byte[]))
		{
			if (reader.CanReadBinaryContent)
			{
				if (bytes == null)
				{
					bytes = new byte[384];
				}
				int count2;
				while ((count2 = reader.ReadValueAsBase64(bytes, 0, bytes.Length)) > 0)
				{
					WriteBase64(bytes, 0, count2);
				}
			}
			else
			{
				WriteString(reader.Value);
			}
			if (!attribute)
			{
				reader.Read();
			}
		}
		else if (valueType == typeof(int))
		{
			WriteValue(reader.ReadContentAsInt());
		}
		else if (valueType == typeof(long))
		{
			WriteValue(reader.ReadContentAsLong());
		}
		else if (valueType == typeof(bool))
		{
			WriteValue(reader.ReadContentAsBoolean());
		}
		else if (valueType == typeof(double))
		{
			WriteValue(reader.ReadContentAsDouble());
		}
		else if (valueType == typeof(DateTime))
		{
			WriteValue(reader.ReadContentAsDateTime());
		}
		else if (valueType == typeof(float))
		{
			WriteValue(reader.ReadContentAsFloat());
		}
		else if (valueType == typeof(decimal))
		{
			WriteValue(reader.ReadContentAsDecimal());
		}
		else if (valueType == typeof(UniqueId))
		{
			WriteValue(reader.ReadContentAsUniqueId());
		}
		else if (valueType == typeof(Guid))
		{
			WriteValue(reader.ReadContentAsGuid());
		}
		else if (valueType == typeof(TimeSpan))
		{
			WriteValue(reader.ReadContentAsTimeSpan());
		}
		else
		{
			WriteValue(reader.ReadContentAsObject());
		}
	}

	private void WriteStartArray(string prefix, string localName, string namespaceUri, int count)
	{
		StartArray(count);
		writer.WriteArrayNode();
		WriteStartElement(prefix, localName, namespaceUri);
		WriteEndElement();
	}

	private void WriteStartArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, int count)
	{
		StartArray(count);
		writer.WriteArrayNode();
		WriteStartElement(prefix, localName, namespaceUri);
		WriteEndElement();
	}

	private void WriteEndArray()
	{
		EndArray();
	}

	[SecurityCritical]
	private unsafe void UnsafeWriteArray(string prefix, string localName, string namespaceUri, XmlBinaryNodeType nodeType, int count, byte* array, byte* arrayMax)
	{
		WriteStartArray(prefix, localName, namespaceUri, count);
		writer.UnsafeWriteArray(nodeType, count, array, arrayMax);
		WriteEndArray();
	}

	[SecurityCritical]
	private unsafe void UnsafeWriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, XmlBinaryNodeType nodeType, int count, byte* array, byte* arrayMax)
	{
		WriteStartArray(prefix, localName, namespaceUri, count);
		writer.UnsafeWriteArray(nodeType, count, array, arrayMax);
		WriteEndArray();
	}

	private void CheckArray(Array array, int offset, int count)
	{
		if (array == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("array"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > array.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", array.Length)));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > array.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", array.Length - offset)));
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, bool[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (bool* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.BoolTextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, bool[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (bool* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.BoolTextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, short[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (short* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.Int16TextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, short[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (short* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.Int16TextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, int[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (int* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.Int32TextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, int[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (int* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.Int32TextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, long[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (long* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.Int64TextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, long[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (long* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.Int64TextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, float[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (float* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.FloatTextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, float[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (float* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.FloatTextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, double[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (double* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.DoubleTextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, double[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (double* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.DoubleTextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, decimal[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (decimal* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.DecimalTextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, decimal[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			fixed (decimal* ptr = &array[offset])
			{
				UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.DecimalTextWithEndElement, count, (byte*)ptr, (byte*)(ptr + count));
			}
		}
	}

	public override void WriteArray(string prefix, string localName, string namespaceUri, DateTime[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			WriteStartArray(prefix, localName, namespaceUri, count);
			writer.WriteDateTimeArray(array, offset, count);
			WriteEndArray();
		}
	}

	public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, DateTime[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			WriteStartArray(prefix, localName, namespaceUri, count);
			writer.WriteDateTimeArray(array, offset, count);
			WriteEndArray();
		}
	}

	public override void WriteArray(string prefix, string localName, string namespaceUri, Guid[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			WriteStartArray(prefix, localName, namespaceUri, count);
			writer.WriteGuidArray(array, offset, count);
			WriteEndArray();
		}
	}

	public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, Guid[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			WriteStartArray(prefix, localName, namespaceUri, count);
			writer.WriteGuidArray(array, offset, count);
			WriteEndArray();
		}
	}

	public override void WriteArray(string prefix, string localName, string namespaceUri, TimeSpan[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			WriteStartArray(prefix, localName, namespaceUri, count);
			writer.WriteTimeSpanArray(array, offset, count);
			WriteEndArray();
		}
	}

	public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, TimeSpan[] array, int offset, int count)
	{
		if (base.Signing)
		{
			base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
			return;
		}
		CheckArray(array, offset, count);
		if (count > 0)
		{
			WriteStartArray(prefix, localName, namespaceUri, count);
			writer.WriteTimeSpanArray(array, offset, count);
			WriteEndArray();
		}
	}
}
