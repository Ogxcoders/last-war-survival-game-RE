using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Utils;

namespace System.Xml.Xsl.XsltOld;

internal class ReaderOutput : XmlReader, RecordOutput
{
	private class XmlEncoder
	{
		private StringBuilder buffer;

		private XmlTextEncoder encoder;

		public char QuoteChar => '"';

		private void Init()
		{
			buffer = new StringBuilder();
			encoder = new XmlTextEncoder(new StringWriter(buffer, CultureInfo.InvariantCulture));
		}

		public string AtributeInnerXml(string value)
		{
			if (encoder == null)
			{
				Init();
			}
			buffer.Length = 0;
			encoder.StartAttribute(cacheAttrValue: false);
			encoder.Write(value);
			encoder.EndAttribute();
			return buffer.ToString();
		}

		public string AtributeOuterXml(string name, string value)
		{
			if (encoder == null)
			{
				Init();
			}
			buffer.Length = 0;
			buffer.Append(name);
			buffer.Append('=');
			buffer.Append(QuoteChar);
			encoder.StartAttribute(cacheAttrValue: false);
			encoder.Write(value);
			encoder.EndAttribute();
			buffer.Append(QuoteChar);
			return buffer.ToString();
		}
	}

	private Processor processor;

	private XmlNameTable nameTable;

	private RecordBuilder builder;

	private BuilderInfo mainNode;

	private ArrayList attributeList;

	private int attributeCount;

	private BuilderInfo attributeValue;

	private OutputScopeManager manager;

	private int currentIndex;

	private BuilderInfo currentInfo;

	private ReadState state;

	private bool haveRecord;

	private static BuilderInfo s_DefaultInfo = new BuilderInfo();

	private XmlEncoder encoder = new XmlEncoder();

	private XmlCharType xmlCharType = XmlCharType.Instance;

	public override XmlNodeType NodeType => currentInfo.NodeType;

	public override string Name
	{
		get
		{
			string prefix = Prefix;
			string localName = LocalName;
			if (prefix != null && prefix.Length > 0)
			{
				if (localName.Length > 0)
				{
					return nameTable.Add(prefix + ":" + localName);
				}
				return prefix;
			}
			return localName;
		}
	}

	public override string LocalName => currentInfo.LocalName;

	public override string NamespaceURI => currentInfo.NamespaceURI;

	public override string Prefix => currentInfo.Prefix;

	public override bool HasValue => XmlReader.HasValueInternal(NodeType);

	public override string Value => currentInfo.Value;

	public override int Depth => currentInfo.Depth;

	public override string BaseURI => string.Empty;

	public override bool IsEmptyElement => currentInfo.IsEmptyTag;

	public override char QuoteChar => encoder.QuoteChar;

	public override bool IsDefault => false;

	public override XmlSpace XmlSpace
	{
		get
		{
			if (manager == null)
			{
				return XmlSpace.None;
			}
			return manager.XmlSpace;
		}
	}

	public override string XmlLang
	{
		get
		{
			if (manager == null)
			{
				return string.Empty;
			}
			return manager.XmlLang;
		}
	}

	public override int AttributeCount => attributeCount;

	public override string this[int i] => GetAttribute(i);

	public override string this[string name] => GetAttribute(name);

	public override string this[string name, string namespaceURI] => GetAttribute(name, namespaceURI);

	public override bool EOF => state == ReadState.EndOfFile;

	public override ReadState ReadState => state;

	public override XmlNameTable NameTable => nameTable;

	internal ReaderOutput(Processor processor)
	{
		this.processor = processor;
		nameTable = processor.NameTable;
		Reset();
	}

	public override string GetAttribute(string name)
	{
		if (FindAttribute(name, out var attrIndex))
		{
			return ((BuilderInfo)attributeList[attrIndex]).Value;
		}
		return null;
	}

	public override string GetAttribute(string localName, string namespaceURI)
	{
		if (FindAttribute(localName, namespaceURI, out var attrIndex))
		{
			return ((BuilderInfo)attributeList[attrIndex]).Value;
		}
		return null;
	}

	public override string GetAttribute(int i)
	{
		return GetBuilderInfo(i).Value;
	}

	public override bool MoveToAttribute(string name)
	{
		if (FindAttribute(name, out var attrIndex))
		{
			SetAttribute(attrIndex);
			return true;
		}
		return false;
	}

	public override bool MoveToAttribute(string localName, string namespaceURI)
	{
		if (FindAttribute(localName, namespaceURI, out var attrIndex))
		{
			SetAttribute(attrIndex);
			return true;
		}
		return false;
	}

	public override void MoveToAttribute(int i)
	{
		if (i < 0 || attributeCount <= i)
		{
			throw new ArgumentOutOfRangeException("i");
		}
		SetAttribute(i);
	}

	public override bool MoveToFirstAttribute()
	{
		if (attributeCount <= 0)
		{
			return false;
		}
		SetAttribute(0);
		return true;
	}

	public override bool MoveToNextAttribute()
	{
		if (currentIndex + 1 < attributeCount)
		{
			SetAttribute(currentIndex + 1);
			return true;
		}
		return false;
	}

	public override bool MoveToElement()
	{
		if (NodeType == XmlNodeType.Attribute || currentInfo == attributeValue)
		{
			SetMainNode();
			return true;
		}
		return false;
	}

	public override bool Read()
	{
		if (state != ReadState.Interactive)
		{
			if (state != ReadState.Initial)
			{
				return false;
			}
			state = ReadState.Interactive;
		}
		while (true)
		{
			if (haveRecord)
			{
				processor.ResetOutput();
				haveRecord = false;
			}
			processor.Execute();
			if (haveRecord)
			{
				switch (NodeType)
				{
				case XmlNodeType.Text:
					if (!xmlCharType.IsOnlyWhitespace(Value))
					{
						break;
					}
					currentInfo.NodeType = XmlNodeType.Whitespace;
					goto IL_007b;
				case XmlNodeType.Whitespace:
					goto IL_007b;
				}
			}
			else
			{
				state = ReadState.EndOfFile;
				Reset();
			}
			break;
			IL_007b:
			if (Value.Length != 0)
			{
				if (XmlSpace == XmlSpace.Preserve)
				{
					currentInfo.NodeType = XmlNodeType.SignificantWhitespace;
				}
				break;
			}
		}
		return haveRecord;
	}

	public override void Close()
	{
		processor = null;
		state = ReadState.Closed;
		Reset();
	}

	public override string ReadString()
	{
		string text = string.Empty;
		if (NodeType == XmlNodeType.Element || NodeType == XmlNodeType.Attribute || currentInfo == attributeValue)
		{
			if (mainNode.IsEmptyTag)
			{
				return text;
			}
			if (!Read())
			{
				throw new InvalidOperationException(System.Xml.Utils.Res.GetString("Operation is not valid due to the current state of the object."));
			}
		}
		StringBuilder stringBuilder = null;
		bool flag = true;
		while (true)
		{
			XmlNodeType nodeType = NodeType;
			if (nodeType != XmlNodeType.Text && (uint)(nodeType - 13) > 1u)
			{
				break;
			}
			if (flag)
			{
				text = Value;
				flag = false;
			}
			else
			{
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(text);
				}
				stringBuilder.Append(Value);
			}
			if (!Read())
			{
				throw new InvalidOperationException(System.Xml.Utils.Res.GetString("Operation is not valid due to the current state of the object."));
			}
		}
		if (stringBuilder != null)
		{
			return stringBuilder.ToString();
		}
		return text;
	}

	public override string ReadInnerXml()
	{
		if (ReadState == ReadState.Interactive)
		{
			if (NodeType == XmlNodeType.Element && !IsEmptyElement)
			{
				StringOutput stringOutput = new StringOutput(processor);
				stringOutput.OmitXmlDecl();
				int depth = Depth;
				Read();
				while (depth < Depth)
				{
					stringOutput.RecordDone(builder);
					Read();
				}
				Read();
				stringOutput.TheEnd();
				return stringOutput.Result;
			}
			if (NodeType == XmlNodeType.Attribute)
			{
				return encoder.AtributeInnerXml(Value);
			}
			Read();
		}
		return string.Empty;
	}

	public override string ReadOuterXml()
	{
		if (ReadState == ReadState.Interactive)
		{
			if (NodeType == XmlNodeType.Element)
			{
				StringOutput stringOutput = new StringOutput(processor);
				stringOutput.OmitXmlDecl();
				bool isEmptyElement = IsEmptyElement;
				int depth = Depth;
				stringOutput.RecordDone(builder);
				Read();
				while (depth < Depth)
				{
					stringOutput.RecordDone(builder);
					Read();
				}
				if (!isEmptyElement)
				{
					stringOutput.RecordDone(builder);
					Read();
				}
				stringOutput.TheEnd();
				return stringOutput.Result;
			}
			if (NodeType == XmlNodeType.Attribute)
			{
				return encoder.AtributeOuterXml(Name, Value);
			}
			Read();
		}
		return string.Empty;
	}

	public override string LookupNamespace(string prefix)
	{
		prefix = nameTable.Get(prefix);
		if (manager != null && prefix != null)
		{
			return manager.ResolveNamespace(prefix);
		}
		return null;
	}

	public override void ResolveEntity()
	{
		if (NodeType != XmlNodeType.EntityReference)
		{
			throw new InvalidOperationException(System.Xml.Utils.Res.GetString("Operation is not valid due to the current state of the object."));
		}
	}

	public override bool ReadAttributeValue()
	{
		if (ReadState != ReadState.Interactive || NodeType != XmlNodeType.Attribute)
		{
			return false;
		}
		if (attributeValue == null)
		{
			attributeValue = new BuilderInfo();
			attributeValue.NodeType = XmlNodeType.Text;
		}
		if (currentInfo == attributeValue)
		{
			return false;
		}
		attributeValue.Value = currentInfo.Value;
		attributeValue.Depth = currentInfo.Depth + 1;
		currentInfo = attributeValue;
		return true;
	}

	public Processor.OutputResult RecordDone(RecordBuilder record)
	{
		builder = record;
		mainNode = record.MainNode;
		attributeList = record.AttributeList;
		attributeCount = record.AttributeCount;
		manager = record.Manager;
		haveRecord = true;
		SetMainNode();
		return Processor.OutputResult.Interrupt;
	}

	public void TheEnd()
	{
	}

	private void SetMainNode()
	{
		currentIndex = -1;
		currentInfo = mainNode;
	}

	private void SetAttribute(int attrib)
	{
		currentIndex = attrib;
		currentInfo = (BuilderInfo)attributeList[attrib];
	}

	private BuilderInfo GetBuilderInfo(int attrib)
	{
		if (attrib < 0 || attributeCount <= attrib)
		{
			throw new ArgumentOutOfRangeException("attrib");
		}
		return (BuilderInfo)attributeList[attrib];
	}

	private bool FindAttribute(string localName, string namespaceURI, out int attrIndex)
	{
		if (namespaceURI == null)
		{
			namespaceURI = string.Empty;
		}
		if (localName == null)
		{
			localName = string.Empty;
		}
		for (int i = 0; i < attributeCount; i++)
		{
			BuilderInfo builderInfo = (BuilderInfo)attributeList[i];
			if (builderInfo.NamespaceURI == namespaceURI && builderInfo.LocalName == localName)
			{
				attrIndex = i;
				return true;
			}
		}
		attrIndex = -1;
		return false;
	}

	private bool FindAttribute(string name, out int attrIndex)
	{
		if (name == null)
		{
			name = string.Empty;
		}
		for (int i = 0; i < attributeCount; i++)
		{
			if (((BuilderInfo)attributeList[i]).Name == name)
			{
				attrIndex = i;
				return true;
			}
		}
		attrIndex = -1;
		return false;
	}

	private void Reset()
	{
		currentIndex = -1;
		currentInfo = s_DefaultInfo;
		mainNode = s_DefaultInfo;
		manager = null;
	}

	[Conditional("DEBUG")]
	private void CheckCurrentInfo()
	{
	}
}
