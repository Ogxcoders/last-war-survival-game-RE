using System.IO;

namespace System.Xml.Xsl;

internal class QueryReaderSettings
{
	private bool validatingReader;

	private XmlReaderSettings xmlReaderSettings;

	private XmlNameTable xmlNameTable;

	private EntityHandling entityHandling;

	private bool namespaces;

	private bool normalization;

	private bool prohibitDtd;

	private WhitespaceHandling whitespaceHandling;

	private XmlResolver xmlResolver;

	public XmlNameTable NameTable
	{
		get
		{
			if (xmlReaderSettings == null)
			{
				return xmlNameTable;
			}
			return xmlReaderSettings.NameTable;
		}
	}

	public QueryReaderSettings(XmlNameTable xmlNameTable)
	{
		xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.NameTable = xmlNameTable;
		xmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;
		xmlReaderSettings.XmlResolver = null;
		xmlReaderSettings.DtdProcessing = DtdProcessing.Prohibit;
		xmlReaderSettings.CloseInput = true;
	}

	public QueryReaderSettings(XmlReader reader)
	{
		if (reader is XmlValidatingReader xmlValidatingReader)
		{
			validatingReader = true;
			reader = xmlValidatingReader.Impl.Reader;
		}
		xmlReaderSettings = reader.Settings;
		if (xmlReaderSettings != null)
		{
			xmlReaderSettings = xmlReaderSettings.Clone();
			xmlReaderSettings.NameTable = reader.NameTable;
			xmlReaderSettings.CloseInput = true;
			xmlReaderSettings.LineNumberOffset = 0;
			xmlReaderSettings.LinePositionOffset = 0;
			if (reader is XmlTextReaderImpl xmlTextReaderImpl)
			{
				xmlReaderSettings.XmlResolver = xmlTextReaderImpl.GetResolver();
			}
			return;
		}
		xmlNameTable = reader.NameTable;
		if (reader is XmlTextReader { Impl: var impl })
		{
			entityHandling = impl.EntityHandling;
			namespaces = impl.Namespaces;
			normalization = impl.Normalization;
			prohibitDtd = impl.DtdProcessing == DtdProcessing.Prohibit;
			whitespaceHandling = impl.WhitespaceHandling;
			xmlResolver = impl.GetResolver();
		}
		else
		{
			entityHandling = EntityHandling.ExpandEntities;
			namespaces = true;
			normalization = true;
			prohibitDtd = true;
			whitespaceHandling = WhitespaceHandling.All;
			xmlResolver = null;
		}
	}

	public XmlReader CreateReader(Stream stream, string baseUri)
	{
		XmlReader xmlReader = ((xmlReaderSettings == null) ? new XmlTextReaderImpl(baseUri, stream, xmlNameTable)
		{
			EntityHandling = entityHandling,
			Namespaces = namespaces,
			Normalization = normalization,
			DtdProcessing = ((!prohibitDtd) ? DtdProcessing.Parse : DtdProcessing.Prohibit),
			WhitespaceHandling = whitespaceHandling,
			XmlResolver = xmlResolver
		} : XmlReader.Create(stream, xmlReaderSettings, baseUri));
		if (validatingReader)
		{
			xmlReader = new XmlValidatingReader(xmlReader);
		}
		return xmlReader;
	}
}
