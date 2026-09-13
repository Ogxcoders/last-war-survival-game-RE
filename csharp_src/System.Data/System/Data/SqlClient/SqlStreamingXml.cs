using System.Data.Common;
using System.IO;
using System.Text;
using System.Xml;

namespace System.Data.SqlClient;

internal sealed class SqlStreamingXml
{
	private int _columnOrdinal;

	private SqlDataReader _reader;

	private XmlReader _xmlReader;

	private XmlWriter _xmlWriter;

	private StringWriter _strWriter;

	private long _charsRemoved;

	public int ColumnOrdinal => _columnOrdinal;

	public SqlStreamingXml(int i, SqlDataReader reader)
	{
		_columnOrdinal = i;
		_reader = reader;
	}

	public void Close()
	{
		((IDisposable)_xmlWriter).Dispose();
		((IDisposable)_xmlReader).Dispose();
		_reader = null;
		_xmlReader = null;
		_xmlWriter = null;
		_strWriter = null;
	}

	public long GetChars(long dataIndex, char[] buffer, int bufferIndex, int length)
	{
		if (_xmlReader == null)
		{
			SqlStream sqlStream = new SqlStream(_columnOrdinal, _reader, addByteOrderMark: true, processAllRows: false, advanceReader: false);
			_xmlReader = sqlStream.ToXmlReader();
			_strWriter = new StringWriter((IFormatProvider)null);
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.CloseOutput = true;
			xmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
			_xmlWriter = XmlWriter.Create(_strWriter, xmlWriterSettings);
		}
		int num = 0;
		int num2 = 0;
		if (dataIndex < _charsRemoved)
		{
			throw ADP.NonSeqByteAccess(dataIndex, _charsRemoved, "GetChars");
		}
		if (dataIndex > _charsRemoved)
		{
			num = (int)(dataIndex - _charsRemoved);
		}
		if (buffer == null)
		{
			return -1L;
		}
		StringBuilder stringBuilder = _strWriter.GetStringBuilder();
		while (!_xmlReader.EOF && stringBuilder.Length < length + num)
		{
			WriteXmlElement();
			if (num > 0)
			{
				num2 = ((stringBuilder.Length < num) ? stringBuilder.Length : num);
				stringBuilder.Remove(0, num2);
				num -= num2;
				_charsRemoved += num2;
			}
		}
		if (num > 0)
		{
			num2 = ((stringBuilder.Length < num) ? stringBuilder.Length : num);
			stringBuilder.Remove(0, num2);
			num -= num2;
			_charsRemoved += num2;
		}
		if (stringBuilder.Length == 0)
		{
			return 0L;
		}
		num2 = ((stringBuilder.Length < length) ? stringBuilder.Length : length);
		for (int i = 0; i < num2; i++)
		{
			buffer[bufferIndex + i] = stringBuilder[i];
		}
		stringBuilder.Remove(0, num2);
		_charsRemoved += num2;
		return num2;
	}

	private void WriteXmlElement()
	{
		if (_xmlReader.EOF)
		{
			return;
		}
		bool canReadValueChunk = _xmlReader.CanReadValueChunk;
		char[] array = null;
		_xmlReader.Read();
		switch (_xmlReader.NodeType)
		{
		case XmlNodeType.Element:
			_xmlWriter.WriteStartElement(_xmlReader.Prefix, _xmlReader.LocalName, _xmlReader.NamespaceURI);
			_xmlWriter.WriteAttributes(_xmlReader, defattr: true);
			if (_xmlReader.IsEmptyElement)
			{
				_xmlWriter.WriteEndElement();
			}
			break;
		case XmlNodeType.Text:
			if (canReadValueChunk)
			{
				if (array == null)
				{
					array = new char[1024];
				}
				int count;
				while ((count = _xmlReader.ReadValueChunk(array, 0, 1024)) > 0)
				{
					_xmlWriter.WriteChars(array, 0, count);
				}
			}
			else
			{
				_xmlWriter.WriteString(_xmlReader.Value);
			}
			break;
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
			_xmlWriter.WriteWhitespace(_xmlReader.Value);
			break;
		case XmlNodeType.CDATA:
			_xmlWriter.WriteCData(_xmlReader.Value);
			break;
		case XmlNodeType.EntityReference:
			_xmlWriter.WriteEntityRef(_xmlReader.Name);
			break;
		case XmlNodeType.ProcessingInstruction:
		case XmlNodeType.XmlDeclaration:
			_xmlWriter.WriteProcessingInstruction(_xmlReader.Name, _xmlReader.Value);
			break;
		case XmlNodeType.DocumentType:
			_xmlWriter.WriteDocType(_xmlReader.Name, _xmlReader.GetAttribute("PUBLIC"), _xmlReader.GetAttribute("SYSTEM"), _xmlReader.Value);
			break;
		case XmlNodeType.Comment:
			_xmlWriter.WriteComment(_xmlReader.Value);
			break;
		case XmlNodeType.EndElement:
			_xmlWriter.WriteFullEndElement();
			break;
		}
		_xmlWriter.Flush();
	}
}
