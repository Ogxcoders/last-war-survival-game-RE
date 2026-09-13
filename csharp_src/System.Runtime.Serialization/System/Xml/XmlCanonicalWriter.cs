using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml;

internal sealed class XmlCanonicalWriter
{
	private class AttributeSorter : IComparer
	{
		private XmlCanonicalWriter writer;

		public AttributeSorter(XmlCanonicalWriter writer)
		{
			this.writer = writer;
		}

		public void Sort()
		{
			object[] array = new object[writer.attributeCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = i;
			}
			Array.Sort(array, this);
			Attribute[] array2 = new Attribute[writer.attributes.Length];
			for (int j = 0; j < array.Length; j++)
			{
				array2[j] = writer.attributes[(int)array[j]];
			}
			writer.attributes = array2;
		}

		public int Compare(object obj1, object obj2)
		{
			int num = (int)obj1;
			int num2 = (int)obj2;
			return writer.Compare(ref writer.attributes[num], ref writer.attributes[num2]);
		}
	}

	private struct Scope
	{
		public int xmlnsAttributeCount;

		public int xmlnsOffset;
	}

	private struct Element
	{
		public int prefixOffset;

		public int prefixLength;

		public int localNameOffset;

		public int localNameLength;
	}

	private struct Attribute
	{
		public int prefixOffset;

		public int prefixLength;

		public int localNameOffset;

		public int localNameLength;

		public int nsOffset;

		public int nsLength;

		public int offset;

		public int length;
	}

	private struct XmlnsAttribute
	{
		public int prefixOffset;

		public int prefixLength;

		public int nsOffset;

		public int nsLength;

		public bool referred;
	}

	private XmlUTF8NodeWriter writer;

	private MemoryStream elementStream;

	private byte[] elementBuffer;

	private XmlUTF8NodeWriter elementWriter;

	private bool inStartElement;

	private int depth;

	private Scope[] scopes;

	private int xmlnsAttributeCount;

	private XmlnsAttribute[] xmlnsAttributes;

	private int attributeCount;

	private Attribute[] attributes;

	private Attribute attribute;

	private Element element;

	private byte[] xmlnsBuffer;

	private int xmlnsOffset;

	private const int maxBytesPerChar = 3;

	private int xmlnsStartOffset;

	private bool includeComments;

	private string[] inclusivePrefixes;

	private const string xmlnsNamespace = "http://www.w3.org/2000/xmlns/";

	private static readonly bool[] isEscapedAttributeChar = new bool[64]
	{
		true, true, true, true, true, true, true, true, true, true,
		true, true, true, true, true, true, true, true, true, true,
		true, true, true, true, true, true, true, true, true, true,
		true, true, false, false, true, false, false, false, true, false,
		false, false, false, false, false, false, false, false, false, false,
		false, false, false, false, false, false, false, false, false, false,
		true, false, false, false
	};

	private static readonly bool[] isEscapedElementChar = new bool[64]
	{
		true, true, true, true, true, true, true, true, true, false,
		false, true, true, true, true, true, true, true, true, true,
		true, true, true, true, true, true, true, true, true, true,
		true, true, false, false, false, false, false, false, true, false,
		false, false, false, false, false, false, false, false, false, false,
		false, false, false, false, false, false, false, false, false, false,
		true, false, true, false
	};

	public void SetOutput(Stream stream, bool includeComments, string[] inclusivePrefixes)
	{
		if (stream == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
		}
		if (writer == null)
		{
			writer = new XmlUTF8NodeWriter(isEscapedAttributeChar, isEscapedElementChar);
		}
		writer.SetOutput(stream, ownsStream: false, null);
		if (elementStream == null)
		{
			elementStream = new MemoryStream();
		}
		if (elementWriter == null)
		{
			elementWriter = new XmlUTF8NodeWriter(isEscapedAttributeChar, isEscapedElementChar);
		}
		elementWriter.SetOutput(elementStream, ownsStream: false, null);
		if (xmlnsAttributes == null)
		{
			xmlnsAttributeCount = 0;
			xmlnsOffset = 0;
			WriteXmlnsAttribute("xml", "http://www.w3.org/XML/1998/namespace");
			WriteXmlnsAttribute("xmlns", "http://www.w3.org/2000/xmlns/");
			WriteXmlnsAttribute(string.Empty, string.Empty);
			xmlnsStartOffset = xmlnsOffset;
			for (int i = 0; i < 3; i++)
			{
				xmlnsAttributes[i].referred = true;
			}
		}
		else
		{
			xmlnsAttributeCount = 3;
			xmlnsOffset = xmlnsStartOffset;
		}
		depth = 0;
		inStartElement = false;
		this.includeComments = includeComments;
		this.inclusivePrefixes = null;
		if (inclusivePrefixes == null)
		{
			return;
		}
		this.inclusivePrefixes = new string[inclusivePrefixes.Length];
		for (int j = 0; j < inclusivePrefixes.Length; j++)
		{
			if (inclusivePrefixes[j] == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument(SR.GetString("The inclusive namespace prefix collection cannot contain null as one of the items."));
			}
			this.inclusivePrefixes[j] = inclusivePrefixes[j];
		}
	}

	public void Flush()
	{
		ThrowIfClosed();
		writer.Flush();
	}

	public void Close()
	{
		if (writer != null)
		{
			writer.Close();
		}
		if (elementWriter != null)
		{
			elementWriter.Close();
		}
		if (elementStream != null && elementStream.Length > 512)
		{
			elementStream = null;
		}
		elementBuffer = null;
		if (scopes != null && scopes.Length > 16)
		{
			scopes = null;
		}
		if (attributes != null && attributes.Length > 16)
		{
			attributes = null;
		}
		if (xmlnsBuffer != null && xmlnsBuffer.Length > 1024)
		{
			xmlnsAttributes = null;
			xmlnsBuffer = null;
		}
		inclusivePrefixes = null;
	}

	public void WriteDeclaration()
	{
	}

	public void WriteComment(string value)
	{
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		ThrowIfClosed();
		if (includeComments)
		{
			writer.WriteComment(value);
		}
	}

	private void StartElement()
	{
		if (scopes == null)
		{
			scopes = new Scope[4];
		}
		else if (depth == scopes.Length)
		{
			Scope[] destinationArray = new Scope[depth * 2];
			Array.Copy(scopes, destinationArray, depth);
			scopes = destinationArray;
		}
		scopes[depth].xmlnsAttributeCount = xmlnsAttributeCount;
		scopes[depth].xmlnsOffset = xmlnsOffset;
		depth++;
		inStartElement = true;
		attributeCount = 0;
		elementStream.Position = 0L;
	}

	private void EndElement()
	{
		depth--;
		xmlnsAttributeCount = scopes[depth].xmlnsAttributeCount;
		xmlnsOffset = scopes[depth].xmlnsOffset;
	}

	public void WriteStartElement(string prefix, string localName)
	{
		if (prefix == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("prefix");
		}
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
		}
		ThrowIfClosed();
		bool num = depth == 0;
		StartElement();
		element.prefixOffset = elementWriter.Position + 1;
		element.prefixLength = Encoding.UTF8.GetByteCount(prefix);
		element.localNameOffset = element.prefixOffset + element.prefixLength + ((element.prefixLength != 0) ? 1 : 0);
		element.localNameLength = Encoding.UTF8.GetByteCount(localName);
		elementWriter.WriteStartElement(prefix, localName);
		if (!num || inclusivePrefixes == null)
		{
			return;
		}
		for (int i = 0; i < scopes[0].xmlnsAttributeCount; i++)
		{
			if (IsInclusivePrefix(ref xmlnsAttributes[i]))
			{
				XmlnsAttribute xmlnsAttribute = xmlnsAttributes[i];
				AddXmlnsAttribute(ref xmlnsAttribute);
			}
		}
	}

	public void WriteStartElement(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
	{
		if (prefixBuffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("prefixBuffer"));
		}
		if (prefixOffset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixOffset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (prefixOffset > prefixBuffer.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixOffset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", prefixBuffer.Length)));
		}
		if (prefixLength < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixLength", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (prefixLength > prefixBuffer.Length - prefixOffset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixLength", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", prefixBuffer.Length - prefixOffset)));
		}
		if (localNameBuffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localNameBuffer"));
		}
		if (localNameOffset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameOffset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (localNameOffset > localNameBuffer.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameOffset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", localNameBuffer.Length)));
		}
		if (localNameLength < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameLength", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (localNameLength > localNameBuffer.Length - localNameOffset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameLength", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", localNameBuffer.Length - localNameOffset)));
		}
		ThrowIfClosed();
		bool num = depth == 0;
		StartElement();
		element.prefixOffset = elementWriter.Position + 1;
		element.prefixLength = prefixLength;
		element.localNameOffset = element.prefixOffset + prefixLength + ((prefixLength != 0) ? 1 : 0);
		element.localNameLength = localNameLength;
		elementWriter.WriteStartElement(prefixBuffer, prefixOffset, prefixLength, localNameBuffer, localNameOffset, localNameLength);
		if (!num || inclusivePrefixes == null)
		{
			return;
		}
		for (int i = 0; i < scopes[0].xmlnsAttributeCount; i++)
		{
			if (IsInclusivePrefix(ref xmlnsAttributes[i]))
			{
				XmlnsAttribute xmlnsAttribute = xmlnsAttributes[i];
				AddXmlnsAttribute(ref xmlnsAttribute);
			}
		}
	}

	private bool IsInclusivePrefix(ref XmlnsAttribute xmlnsAttribute)
	{
		for (int i = 0; i < inclusivePrefixes.Length; i++)
		{
			if (inclusivePrefixes[i].Length == xmlnsAttribute.prefixLength && string.Compare(Encoding.UTF8.GetString(xmlnsBuffer, xmlnsAttribute.prefixOffset, xmlnsAttribute.prefixLength), inclusivePrefixes[i], StringComparison.Ordinal) == 0)
			{
				return true;
			}
		}
		return false;
	}

	public void WriteEndStartElement(bool isEmpty)
	{
		ThrowIfClosed();
		elementWriter.Flush();
		elementBuffer = elementStream.GetBuffer();
		inStartElement = false;
		ResolvePrefixes();
		writer.WriteStartElement(elementBuffer, element.prefixOffset, element.prefixLength, elementBuffer, element.localNameOffset, element.localNameLength);
		for (int i = scopes[depth - 1].xmlnsAttributeCount; i < xmlnsAttributeCount; i++)
		{
			int num = i - 1;
			bool flag = false;
			while (num >= 0)
			{
				if (Equals(xmlnsBuffer, xmlnsAttributes[i].prefixOffset, xmlnsAttributes[i].prefixLength, xmlnsBuffer, xmlnsAttributes[num].prefixOffset, xmlnsAttributes[num].prefixLength))
				{
					if (!Equals(xmlnsBuffer, xmlnsAttributes[i].nsOffset, xmlnsAttributes[i].nsLength, xmlnsBuffer, xmlnsAttributes[num].nsOffset, xmlnsAttributes[num].nsLength))
					{
						break;
					}
					if (xmlnsAttributes[num].referred)
					{
						flag = true;
						break;
					}
				}
				num--;
			}
			if (!flag)
			{
				WriteXmlnsAttribute(ref xmlnsAttributes[i]);
			}
		}
		if (attributeCount > 0)
		{
			if (attributeCount > 1)
			{
				SortAttributes();
			}
			for (int j = 0; j < attributeCount; j++)
			{
				writer.WriteText(elementBuffer, attributes[j].offset, attributes[j].length);
			}
		}
		writer.WriteEndStartElement(isEmpty: false);
		if (isEmpty)
		{
			writer.WriteEndElement(elementBuffer, element.prefixOffset, element.prefixLength, elementBuffer, element.localNameOffset, element.localNameLength);
			EndElement();
		}
		elementBuffer = null;
	}

	public void WriteEndElement(string prefix, string localName)
	{
		if (prefix == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("prefix");
		}
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
		}
		ThrowIfClosed();
		writer.WriteEndElement(prefix, localName);
		EndElement();
	}

	private void EnsureXmlnsBuffer(int byteCount)
	{
		if (xmlnsBuffer == null)
		{
			xmlnsBuffer = new byte[Math.Max(byteCount, 128)];
		}
		else if (xmlnsOffset + byteCount > xmlnsBuffer.Length)
		{
			byte[] dst = new byte[Math.Max(xmlnsOffset + byteCount, xmlnsBuffer.Length * 2)];
			Buffer.BlockCopy(xmlnsBuffer, 0, dst, 0, xmlnsOffset);
			xmlnsBuffer = dst;
		}
	}

	public void WriteXmlnsAttribute(string prefix, string ns)
	{
		if (prefix == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("prefix");
		}
		if (ns == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("ns");
		}
		ThrowIfClosed();
		if (prefix.Length > int.MaxValue - ns.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("ns", SR.GetString("The combined length of the prefix and namespace must not be greater than {0}.", 715827882)));
		}
		int num = prefix.Length + ns.Length;
		if (num > 715827882)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("ns", SR.GetString("The combined length of the prefix and namespace must not be greater than {0}.", 715827882)));
		}
		EnsureXmlnsBuffer(num * 3);
		XmlnsAttribute xmlnsAttribute = default(XmlnsAttribute);
		xmlnsAttribute.prefixOffset = xmlnsOffset;
		xmlnsAttribute.prefixLength = Encoding.UTF8.GetBytes(prefix, 0, prefix.Length, xmlnsBuffer, xmlnsOffset);
		xmlnsOffset += xmlnsAttribute.prefixLength;
		xmlnsAttribute.nsOffset = xmlnsOffset;
		xmlnsAttribute.nsLength = Encoding.UTF8.GetBytes(ns, 0, ns.Length, xmlnsBuffer, xmlnsOffset);
		xmlnsOffset += xmlnsAttribute.nsLength;
		xmlnsAttribute.referred = false;
		AddXmlnsAttribute(ref xmlnsAttribute);
	}

	public void WriteXmlnsAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] nsBuffer, int nsOffset, int nsLength)
	{
		if (prefixBuffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("prefixBuffer"));
		}
		if (prefixOffset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixOffset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (prefixOffset > prefixBuffer.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixOffset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", prefixBuffer.Length)));
		}
		if (prefixLength < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixLength", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (prefixLength > prefixBuffer.Length - prefixOffset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixLength", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", prefixBuffer.Length - prefixOffset)));
		}
		if (nsBuffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("nsBuffer"));
		}
		if (nsOffset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("nsOffset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (nsOffset > nsBuffer.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("nsOffset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", nsBuffer.Length)));
		}
		if (nsLength < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("nsLength", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (nsLength > nsBuffer.Length - nsOffset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("nsLength", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", nsBuffer.Length - nsOffset)));
		}
		ThrowIfClosed();
		if (prefixLength > int.MaxValue - nsLength)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("nsLength", SR.GetString("The combined length of the prefix and namespace must not be greater than {0}.", int.MaxValue)));
		}
		EnsureXmlnsBuffer(prefixLength + nsLength);
		XmlnsAttribute xmlnsAttribute = default(XmlnsAttribute);
		xmlnsAttribute.prefixOffset = xmlnsOffset;
		xmlnsAttribute.prefixLength = prefixLength;
		Buffer.BlockCopy(prefixBuffer, prefixOffset, xmlnsBuffer, xmlnsOffset, prefixLength);
		xmlnsOffset += prefixLength;
		xmlnsAttribute.nsOffset = xmlnsOffset;
		xmlnsAttribute.nsLength = nsLength;
		Buffer.BlockCopy(nsBuffer, nsOffset, xmlnsBuffer, xmlnsOffset, nsLength);
		xmlnsOffset += nsLength;
		xmlnsAttribute.referred = false;
		AddXmlnsAttribute(ref xmlnsAttribute);
	}

	public void WriteStartAttribute(string prefix, string localName)
	{
		if (prefix == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("prefix");
		}
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
		}
		ThrowIfClosed();
		attribute.offset = elementWriter.Position;
		attribute.length = 0;
		attribute.prefixOffset = attribute.offset + 1;
		attribute.prefixLength = Encoding.UTF8.GetByteCount(prefix);
		attribute.localNameOffset = attribute.prefixOffset + attribute.prefixLength + ((attribute.prefixLength != 0) ? 1 : 0);
		attribute.localNameLength = Encoding.UTF8.GetByteCount(localName);
		attribute.nsOffset = 0;
		attribute.nsLength = 0;
		elementWriter.WriteStartAttribute(prefix, localName);
	}

	public void WriteStartAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
	{
		if (prefixBuffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("prefixBuffer"));
		}
		if (prefixOffset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixOffset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (prefixOffset > prefixBuffer.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixOffset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", prefixBuffer.Length)));
		}
		if (prefixLength < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixLength", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (prefixLength > prefixBuffer.Length - prefixOffset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixLength", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", prefixBuffer.Length - prefixOffset)));
		}
		if (localNameBuffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localNameBuffer"));
		}
		if (localNameOffset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameOffset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (localNameOffset > localNameBuffer.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameOffset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", localNameBuffer.Length)));
		}
		if (localNameLength < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameLength", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (localNameLength > localNameBuffer.Length - localNameOffset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameLength", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", localNameBuffer.Length - localNameOffset)));
		}
		ThrowIfClosed();
		attribute.offset = elementWriter.Position;
		attribute.length = 0;
		attribute.prefixOffset = attribute.offset + 1;
		attribute.prefixLength = prefixLength;
		attribute.localNameOffset = attribute.prefixOffset + prefixLength + ((prefixLength != 0) ? 1 : 0);
		attribute.localNameLength = localNameLength;
		attribute.nsOffset = 0;
		attribute.nsLength = 0;
		elementWriter.WriteStartAttribute(prefixBuffer, prefixOffset, prefixLength, localNameBuffer, localNameOffset, localNameLength);
	}

	public void WriteEndAttribute()
	{
		ThrowIfClosed();
		elementWriter.WriteEndAttribute();
		attribute.length = elementWriter.Position - attribute.offset;
		AddAttribute(ref attribute);
	}

	public void WriteCharEntity(int ch)
	{
		ThrowIfClosed();
		if (ch <= 65535)
		{
			char[] chars = new char[1] { (char)ch };
			WriteEscapedText(chars, 0, 1);
		}
		else
		{
			WriteText(ch);
		}
	}

	public void WriteEscapedText(string value)
	{
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		ThrowIfClosed();
		if (depth > 0)
		{
			if (inStartElement)
			{
				elementWriter.WriteEscapedText(value);
			}
			else
			{
				writer.WriteEscapedText(value);
			}
		}
	}

	public void WriteEscapedText(byte[] chars, int offset, int count)
	{
		if (chars == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > chars.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", chars.Length)));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > chars.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", chars.Length - offset)));
		}
		ThrowIfClosed();
		if (depth > 0)
		{
			if (inStartElement)
			{
				elementWriter.WriteEscapedText(chars, offset, count);
			}
			else
			{
				writer.WriteEscapedText(chars, offset, count);
			}
		}
	}

	public void WriteEscapedText(char[] chars, int offset, int count)
	{
		ThrowIfClosed();
		if (depth > 0)
		{
			if (inStartElement)
			{
				elementWriter.WriteEscapedText(chars, offset, count);
			}
			else
			{
				writer.WriteEscapedText(chars, offset, count);
			}
		}
	}

	public void WriteText(int ch)
	{
		ThrowIfClosed();
		if (inStartElement)
		{
			elementWriter.WriteText(ch);
		}
		else
		{
			writer.WriteText(ch);
		}
	}

	public void WriteText(byte[] chars, int offset, int count)
	{
		ThrowIfClosed();
		if (chars == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > chars.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", chars.Length)));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > chars.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", chars.Length - offset)));
		}
		if (inStartElement)
		{
			elementWriter.WriteText(chars, offset, count);
		}
		else
		{
			writer.WriteText(chars, offset, count);
		}
	}

	public void WriteText(string value)
	{
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
		}
		if (value.Length > 0)
		{
			if (inStartElement)
			{
				elementWriter.WriteText(value);
			}
			else
			{
				writer.WriteText(value);
			}
		}
	}

	public void WriteText(char[] chars, int offset, int count)
	{
		ThrowIfClosed();
		if (chars == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > chars.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", chars.Length)));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > chars.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", chars.Length - offset)));
		}
		if (inStartElement)
		{
			elementWriter.WriteText(chars, offset, count);
		}
		else
		{
			writer.WriteText(chars, offset, count);
		}
	}

	private void ThrowIfClosed()
	{
		if (writer == null)
		{
			ThrowClosed();
		}
	}

	private void ThrowClosed()
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ObjectDisposedException(GetType().ToString()));
	}

	private void WriteXmlnsAttribute(ref XmlnsAttribute xmlnsAttribute)
	{
		if (xmlnsAttribute.referred)
		{
			writer.WriteXmlnsAttribute(xmlnsBuffer, xmlnsAttribute.prefixOffset, xmlnsAttribute.prefixLength, xmlnsBuffer, xmlnsAttribute.nsOffset, xmlnsAttribute.nsLength);
		}
	}

	private void SortAttributes()
	{
		if (attributeCount < 16)
		{
			for (int i = 0; i < attributeCount - 1; i++)
			{
				int num = i;
				for (int j = i + 1; j < attributeCount; j++)
				{
					if (Compare(ref attributes[j], ref attributes[num]) < 0)
					{
						num = j;
					}
				}
				if (num != i)
				{
					Attribute attribute = attributes[i];
					attributes[i] = attributes[num];
					attributes[num] = attribute;
				}
			}
		}
		else
		{
			new AttributeSorter(this).Sort();
		}
	}

	private void AddAttribute(ref Attribute attribute)
	{
		if (attributes == null)
		{
			attributes = new Attribute[4];
		}
		else if (attributeCount == attributes.Length)
		{
			Attribute[] destinationArray = new Attribute[attributeCount * 2];
			Array.Copy(attributes, destinationArray, attributeCount);
			attributes = destinationArray;
		}
		attributes[attributeCount] = attribute;
		attributeCount++;
	}

	private void AddXmlnsAttribute(ref XmlnsAttribute xmlnsAttribute)
	{
		if (xmlnsAttributes == null)
		{
			xmlnsAttributes = new XmlnsAttribute[4];
		}
		else if (xmlnsAttributes.Length == xmlnsAttributeCount)
		{
			XmlnsAttribute[] destinationArray = new XmlnsAttribute[xmlnsAttributeCount * 2];
			Array.Copy(xmlnsAttributes, destinationArray, xmlnsAttributeCount);
			xmlnsAttributes = destinationArray;
		}
		if (depth > 0 && inclusivePrefixes != null && IsInclusivePrefix(ref xmlnsAttribute))
		{
			xmlnsAttribute.referred = true;
		}
		if (depth == 0)
		{
			xmlnsAttributes[xmlnsAttributeCount++] = xmlnsAttribute;
			return;
		}
		int i = scopes[depth - 1].xmlnsAttributeCount;
		bool flag = true;
		for (; i < xmlnsAttributeCount; i++)
		{
			int num = Compare(ref xmlnsAttribute, ref xmlnsAttributes[i]);
			if (num <= 0)
			{
				if (num == 0)
				{
					xmlnsAttributes[i] = xmlnsAttribute;
					flag = false;
				}
				break;
			}
		}
		if (flag)
		{
			Array.Copy(xmlnsAttributes, i, xmlnsAttributes, i + 1, xmlnsAttributeCount - i);
			xmlnsAttributes[i] = xmlnsAttribute;
			xmlnsAttributeCount++;
		}
	}

	private void ResolvePrefix(int prefixOffset, int prefixLength, out int nsOffset, out int nsLength)
	{
		int num = scopes[depth - 1].xmlnsAttributeCount;
		int num2 = xmlnsAttributeCount - 1;
		while (!Equals(elementBuffer, prefixOffset, prefixLength, xmlnsBuffer, xmlnsAttributes[num2].prefixOffset, xmlnsAttributes[num2].prefixLength))
		{
			num2--;
		}
		nsOffset = xmlnsAttributes[num2].nsOffset;
		nsLength = xmlnsAttributes[num2].nsLength;
		if (num2 < num)
		{
			if (!xmlnsAttributes[num2].referred)
			{
				XmlnsAttribute xmlnsAttribute = xmlnsAttributes[num2];
				xmlnsAttribute.referred = true;
				AddXmlnsAttribute(ref xmlnsAttribute);
			}
		}
		else
		{
			xmlnsAttributes[num2].referred = true;
		}
	}

	private void ResolvePrefix(ref Attribute attribute)
	{
		if (attribute.prefixLength != 0)
		{
			ResolvePrefix(attribute.prefixOffset, attribute.prefixLength, out attribute.nsOffset, out attribute.nsLength);
		}
	}

	private void ResolvePrefixes()
	{
		ResolvePrefix(element.prefixOffset, element.prefixLength, out var _, out var _);
		for (int i = 0; i < attributeCount; i++)
		{
			ResolvePrefix(ref attributes[i]);
		}
	}

	private int Compare(ref XmlnsAttribute xmlnsAttribute1, ref XmlnsAttribute xmlnsAttribute2)
	{
		return Compare(xmlnsBuffer, xmlnsAttribute1.prefixOffset, xmlnsAttribute1.prefixLength, xmlnsAttribute2.prefixOffset, xmlnsAttribute2.prefixLength);
	}

	private int Compare(ref Attribute attribute1, ref Attribute attribute2)
	{
		int num = Compare(xmlnsBuffer, attribute1.nsOffset, attribute1.nsLength, attribute2.nsOffset, attribute2.nsLength);
		if (num == 0)
		{
			num = Compare(elementBuffer, attribute1.localNameOffset, attribute1.localNameLength, attribute2.localNameOffset, attribute2.localNameLength);
		}
		return num;
	}

	private int Compare(byte[] buffer, int offset1, int length1, int offset2, int length2)
	{
		if (offset1 == offset2)
		{
			return length1 - length2;
		}
		return Compare(buffer, offset1, length1, buffer, offset2, length2);
	}

	private int Compare(byte[] buffer1, int offset1, int length1, byte[] buffer2, int offset2, int length2)
	{
		int num = Math.Min(length1, length2);
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			if (num2 != 0)
			{
				break;
			}
			num2 = buffer1[offset1 + i] - buffer2[offset2 + i];
		}
		if (num2 == 0)
		{
			num2 = length1 - length2;
		}
		return num2;
	}

	private bool Equals(byte[] buffer1, int offset1, int length1, byte[] buffer2, int offset2, int length2)
	{
		if (length1 != length2)
		{
			return false;
		}
		for (int i = 0; i < length1; i++)
		{
			if (buffer1[offset1 + i] != buffer2[offset2 + i])
			{
				return false;
			}
		}
		return true;
	}
}
