using System.IO;
using System.Net;
using System.Text;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

internal class WebServiceHelper
{
	private class HeaderSerializationHelper
	{
		private SoapHeaderCollection headers;

		private XmlSerializer headerSerializer;

		public HeaderSerializationHelper(XmlSerializer headerSerializer)
		{
			headers = new SoapHeaderCollection();
			this.headerSerializer = headerSerializer;
		}

		public SoapHeaderCollection Deserialize(XmlTextReader xmlReader)
		{
			try
			{
				headerSerializer.UnknownElement += OnAddUnknownHeader;
				object[] array = (object[])headerSerializer.Deserialize(xmlReader);
				for (int i = 0; i < array.Length; i++)
				{
					SoapHeader soapHeader = (SoapHeader)array[i];
					if (soapHeader != null)
					{
						headers.Add(soapHeader);
					}
				}
				return headers;
			}
			finally
			{
				headerSerializer.UnknownElement -= OnAddUnknownHeader;
			}
		}

		private void OnAddUnknownHeader(object sender, XmlElementEventArgs e)
		{
			headers.Add(new SoapUnknownHeader(e.Element));
		}
	}

	public const string SoapEnvelopeNamespace = "http://schemas.xmlsoap.org/soap/envelope/";

	public const string Soap12EnvelopeNamespace = "http://www.w3.org/2003/05/soap-envelope";

	public const string SoapEncodingNamespace = "http://schemas.xmlsoap.org/soap/encoding/";

	public const string Soap12EncodingNamespace = "http://www.w3.org/2003/05/soap-encoding";

	private static readonly char[] trimChars;

	private static readonly bool prettyXml;

	static WebServiceHelper()
	{
		trimChars = new char[2] { '"', '\'' };
		string environmentVariable = Environment.GetEnvironmentVariable("MONO_WEBSERVICES_PRETTYXML");
		prettyXml = environmentVariable != null && environmentVariable != "no";
	}

	public static XmlTextWriter CreateXmlWriter(Stream s)
	{
		XmlTextWriter xmlTextWriter = new XmlTextWriter(s, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
		if (prettyXml)
		{
			xmlTextWriter.Formatting = Formatting.Indented;
		}
		return xmlTextWriter;
	}

	public static Encoding GetContentEncoding(string cts, out string content_type)
	{
		if (cts == null)
		{
			cts = "";
		}
		string name = "utf-8";
		int num = 0;
		int num2 = cts.IndexOf(';');
		if (num2 == -1)
		{
			content_type = cts;
		}
		else
		{
			content_type = cts.Substring(0, num2);
		}
		content_type = content_type.Trim();
		num = num2 + 1;
		while (num2 != -1)
		{
			num2 = cts.IndexOf(';', num);
			string text;
			if (num2 == -1)
			{
				text = cts.Substring(num);
			}
			else
			{
				text = cts.Substring(num, num2 - num);
				num = num2 + 1;
			}
			text = text.Trim();
			if (string.CompareOrdinal(text, 0, "charset=", 0, 8) == 0)
			{
				name = text.Substring(8);
				name = name.TrimStart(trimChars).TrimEnd(trimChars);
			}
		}
		return Encoding.GetEncoding(name);
	}

	public static string GetContextAction(string cts)
	{
		if (cts == null || cts.Length == 0)
		{
			return null;
		}
		int num = 0;
		int num2 = cts.IndexOf(';');
		num = num2 + 1;
		while (num2 != -1)
		{
			num2 = cts.IndexOf(';', num);
			string text;
			if (num2 == -1)
			{
				text = cts.Substring(num);
			}
			else
			{
				text = cts.Substring(num, num2 - num);
				num = num2 + 1;
			}
			text = text.Trim();
			string text2 = "action=";
			if (string.CompareOrdinal(text, 0, text2, 0, text2.Length) == 0)
			{
				return text.Substring(text2.Length).Trim(trimChars);
			}
		}
		return null;
	}

	public static void WriteSoapMessage(XmlTextWriter xtw, SoapMethodStubInfo method, SoapHeaderDirection dir, object bodyContent, SoapHeaderCollection headers, bool soap12)
	{
		SoapBindingUse methodUse = ((dir == SoapHeaderDirection.Fault) ? SoapBindingUse.Literal : method.Use);
		XmlSerializer bodySerializer = method.GetBodySerializer(dir, soap12);
		XmlSerializer headerSerializer = method.GetHeaderSerializer(dir);
		object[] headerValueArray = method.GetHeaderValueArray(dir, headers);
		WriteSoapMessage(xtw, methodUse, bodySerializer, headerSerializer, bodyContent, headerValueArray, soap12);
	}

	public static void WriteSoapMessage(XmlTextWriter xtw, SoapBindingUse methodUse, XmlSerializer bodySerializer, XmlSerializer headerSerializer, object bodyContent, object[] headers, bool soap12)
	{
		string ns = (soap12 ? "http://www.w3.org/2003/05/soap-envelope" : "http://schemas.xmlsoap.org/soap/envelope/");
		string value = (soap12 ? "http://www.w3.org/2003/05/soap-encoding" : "http://schemas.xmlsoap.org/soap/encoding/");
		xtw.WriteStartDocument();
		xtw.WriteStartElement("soap", "Envelope", ns);
		xtw.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
		xtw.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
		if (headers != null)
		{
			xtw.WriteStartElement("soap", "Header", ns);
			headerSerializer.Serialize(xtw, headers);
			xtw.WriteEndElement();
		}
		xtw.WriteStartElement("soap", "Body", ns);
		if (methodUse == SoapBindingUse.Encoded)
		{
			xtw.WriteAttributeString("encodingStyle", ns, value);
		}
		bodySerializer.Serialize(xtw, bodyContent);
		xtw.WriteEndElement();
		xtw.WriteEndElement();
		xtw.Flush();
	}

	public static void ReadSoapMessage(XmlTextReader xmlReader, SoapMethodStubInfo method, SoapHeaderDirection dir, bool soap12, out object body, out SoapHeaderCollection headers)
	{
		XmlSerializer bodySerializer = method.GetBodySerializer(dir, soap12: false);
		XmlSerializer headerSerializer = method.GetHeaderSerializer(dir);
		ReadSoapMessage(xmlReader, bodySerializer, headerSerializer, soap12, out body, out headers);
	}

	public static void ReadSoapMessage(XmlTextReader xmlReader, XmlSerializer bodySerializer, XmlSerializer headerSerializer, bool soap12, out object body, out SoapHeaderCollection headers)
	{
		xmlReader.MoveToContent();
		string namespaceURI = xmlReader.NamespaceURI;
		if (!(namespaceURI == "http://www.w3.org/2003/05/soap-envelope") && !(namespaceURI == "http://schemas.xmlsoap.org/soap/envelope/"))
		{
			throw new SoapException($"SOAP version mismatch. Namespace '{namespaceURI}' is not supported in this runtime profile.", VersionMismatchFaultCode(soap12));
		}
		xmlReader.ReadStartElement("Envelope", namespaceURI);
		headers = ReadHeaders(xmlReader, headerSerializer, namespaceURI);
		xmlReader.MoveToContent();
		xmlReader.ReadStartElement("Body", namespaceURI);
		xmlReader.MoveToContent();
		if (xmlReader.LocalName == "Fault" && xmlReader.NamespaceURI == namespaceURI)
		{
			bodySerializer = ((namespaceURI == "http://www.w3.org/2003/05/soap-envelope") ? Soap12Fault.Serializer : Fault.Serializer);
		}
		body = bodySerializer.Deserialize(xmlReader);
	}

	private static SoapHeaderCollection ReadHeaders(XmlTextReader xmlReader, XmlSerializer headerSerializer, string ns)
	{
		SoapHeaderCollection soapHeaderCollection = null;
		while (xmlReader.NodeType != XmlNodeType.Element || !(xmlReader.LocalName == "Body") || !(xmlReader.NamespaceURI == ns))
		{
			if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.LocalName == "Header" && xmlReader.NamespaceURI == ns && !xmlReader.IsEmptyElement && headerSerializer != null)
			{
				xmlReader.ReadStartElement();
				xmlReader.MoveToContent();
				soapHeaderCollection = new HeaderSerializationHelper(headerSerializer).Deserialize(xmlReader);
				while (xmlReader.NodeType != XmlNodeType.EndElement)
				{
					xmlReader.Skip();
				}
				xmlReader.ReadEndElement();
			}
			else
			{
				xmlReader.Skip();
			}
		}
		if (soapHeaderCollection != null)
		{
			return soapHeaderCollection;
		}
		return new SoapHeaderCollection();
	}

	public static SoapException Soap12FaultToSoapException(Soap12Fault fault)
	{
		Soap12FaultReasonText soap12FaultReasonText = ((fault.Reason != null && fault.Reason.Texts != null && fault.Reason.Texts.Length != 0) ? fault.Reason.Texts[fault.Reason.Texts.Length - 1] : null);
		XmlNode detail = ((fault.Detail == null) ? null : ((fault.Detail.Children != null && fault.Detail.Children.Length != 0) ? ((XmlNode)fault.Detail.Children[0]) : ((XmlNode)((fault.Detail.Attributes != null && fault.Detail.Attributes.Length != 0) ? fault.Detail.Attributes[0] : null))));
		SoapFaultSubCode soapFaultSubCode = Soap12Fault.GetSoapFaultSubCode(fault.Code.Subcode);
		return new SoapException(soap12FaultReasonText?.Value, fault.Code.Value, null, fault.Role, soap12FaultReasonText?.XmlLang, detail, soapFaultSubCode, null);
	}

	public static XmlQualifiedName ClientFaultCode(bool soap12)
	{
		if (!soap12)
		{
			return SoapException.ClientFaultCode;
		}
		return Soap12FaultCodes.SenderFaultCode;
	}

	public static XmlQualifiedName ServerFaultCode(bool soap12)
	{
		if (!soap12)
		{
			return SoapException.ServerFaultCode;
		}
		return Soap12FaultCodes.ReceiverFaultCode;
	}

	public static XmlQualifiedName MustUnderstandFaultCode(bool soap12)
	{
		if (!soap12)
		{
			return SoapException.MustUnderstandFaultCode;
		}
		return Soap12FaultCodes.ReceiverFaultCode;
	}

	public static XmlQualifiedName VersionMismatchFaultCode(bool soap12)
	{
		if (!soap12)
		{
			return SoapException.VersionMismatchFaultCode;
		}
		return Soap12FaultCodes.VersionMismatchFaultCode;
	}

	public static void InvalidOperation(string message, WebResponse response, Encoding enc)
	{
		if (response == null)
		{
			throw new InvalidOperationException(message);
		}
		if (enc == null)
		{
			enc = Encoding.UTF8;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(message);
		if (response.ContentLength > 0)
		{
			stringBuilder.Append("\r\nResponse error message:\r\n--\r\n");
			try
			{
				StreamReader streamReader = new StreamReader(response.GetResponseStream(), enc);
				stringBuilder.Append(streamReader.ReadToEnd());
			}
			catch (Exception)
			{
			}
		}
		throw new InvalidOperationException(stringBuilder.ToString());
	}
}
