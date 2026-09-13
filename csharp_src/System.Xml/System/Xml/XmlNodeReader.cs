using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml;

public class XmlNodeReader : XmlReader, IXmlNamespaceResolver
{
	private XmlNodeReaderNavigator readerNav;

	private XmlNodeType nodeType;

	private int curDepth;

	private ReadState readState;

	private bool fEOF;

	private bool bResolveEntity;

	private bool bStartFromDocument;

	private bool bInReadBinary;

	private ReadContentAsBinaryHelper readBinaryHelper;

	public override XmlNodeType NodeType
	{
		get
		{
			if (!IsInReadingStates())
			{
				return XmlNodeType.None;
			}
			return nodeType;
		}
	}

	public override string Name
	{
		get
		{
			if (!IsInReadingStates())
			{
				return string.Empty;
			}
			return readerNav.Name;
		}
	}

	public override string LocalName
	{
		get
		{
			if (!IsInReadingStates())
			{
				return string.Empty;
			}
			return readerNav.LocalName;
		}
	}

	public override string NamespaceURI
	{
		get
		{
			if (!IsInReadingStates())
			{
				return string.Empty;
			}
			return readerNav.NamespaceURI;
		}
	}

	public override string Prefix
	{
		get
		{
			if (!IsInReadingStates())
			{
				return string.Empty;
			}
			return readerNav.Prefix;
		}
	}

	public override bool HasValue
	{
		get
		{
			if (!IsInReadingStates())
			{
				return false;
			}
			return readerNav.HasValue;
		}
	}

	public override string Value
	{
		get
		{
			if (!IsInReadingStates())
			{
				return string.Empty;
			}
			return readerNav.Value;
		}
	}

	public override int Depth => curDepth;

	public override string BaseURI => readerNav.BaseURI;

	public override bool CanResolveEntity => true;

	public override bool IsEmptyElement
	{
		get
		{
			if (!IsInReadingStates())
			{
				return false;
			}
			return readerNav.IsEmptyElement;
		}
	}

	public override bool IsDefault
	{
		get
		{
			if (!IsInReadingStates())
			{
				return false;
			}
			return readerNav.IsDefault;
		}
	}

	public override XmlSpace XmlSpace
	{
		get
		{
			if (!IsInReadingStates())
			{
				return XmlSpace.None;
			}
			return readerNav.XmlSpace;
		}
	}

	public override string XmlLang
	{
		get
		{
			if (!IsInReadingStates())
			{
				return string.Empty;
			}
			return readerNav.XmlLang;
		}
	}

	public override IXmlSchemaInfo SchemaInfo
	{
		get
		{
			if (!IsInReadingStates())
			{
				return null;
			}
			return readerNav.SchemaInfo;
		}
	}

	public override int AttributeCount
	{
		get
		{
			if (!IsInReadingStates() || nodeType == XmlNodeType.EndElement)
			{
				return 0;
			}
			return readerNav.AttributeCount;
		}
	}

	public override bool EOF
	{
		get
		{
			if (readState != ReadState.Closed)
			{
				return fEOF;
			}
			return false;
		}
	}

	public override ReadState ReadState => readState;

	public override bool HasAttributes => AttributeCount > 0;

	public override XmlNameTable NameTable => readerNav.NameTable;

	public override bool CanReadBinaryContent => true;

	internal override IDtdInfo DtdInfo => readerNav.Document.DtdSchemaInfo;

	public XmlNodeReader(XmlNode node)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		readerNav = new XmlNodeReaderNavigator(node);
		curDepth = 0;
		readState = ReadState.Initial;
		fEOF = false;
		nodeType = XmlNodeType.None;
		bResolveEntity = false;
		bStartFromDocument = false;
	}

	internal bool IsInReadingStates()
	{
		return readState == ReadState.Interactive;
	}

	public override string GetAttribute(string name)
	{
		if (!IsInReadingStates())
		{
			return null;
		}
		return readerNav.GetAttribute(name);
	}

	public override string GetAttribute(string name, string namespaceURI)
	{
		if (!IsInReadingStates())
		{
			return null;
		}
		string ns = ((namespaceURI == null) ? string.Empty : namespaceURI);
		return readerNav.GetAttribute(name, ns);
	}

	public override string GetAttribute(int attributeIndex)
	{
		if (!IsInReadingStates())
		{
			throw new ArgumentOutOfRangeException("attributeIndex");
		}
		return readerNav.GetAttribute(attributeIndex);
	}

	public override bool MoveToAttribute(string name)
	{
		if (!IsInReadingStates())
		{
			return false;
		}
		readerNav.ResetMove(ref curDepth, ref nodeType);
		if (readerNav.MoveToAttribute(name))
		{
			curDepth++;
			nodeType = readerNav.NodeType;
			if (bInReadBinary)
			{
				FinishReadBinary();
			}
			return true;
		}
		readerNav.RollBackMove(ref curDepth);
		return false;
	}

	public override bool MoveToAttribute(string name, string namespaceURI)
	{
		if (!IsInReadingStates())
		{
			return false;
		}
		readerNav.ResetMove(ref curDepth, ref nodeType);
		string namespaceURI2 = ((namespaceURI == null) ? string.Empty : namespaceURI);
		if (readerNav.MoveToAttribute(name, namespaceURI2))
		{
			curDepth++;
			nodeType = readerNav.NodeType;
			if (bInReadBinary)
			{
				FinishReadBinary();
			}
			return true;
		}
		readerNav.RollBackMove(ref curDepth);
		return false;
	}

	public override void MoveToAttribute(int attributeIndex)
	{
		if (!IsInReadingStates())
		{
			throw new ArgumentOutOfRangeException("attributeIndex");
		}
		readerNav.ResetMove(ref curDepth, ref nodeType);
		try
		{
			if (AttributeCount <= 0)
			{
				throw new ArgumentOutOfRangeException("attributeIndex");
			}
			readerNav.MoveToAttribute(attributeIndex);
			if (bInReadBinary)
			{
				FinishReadBinary();
			}
		}
		catch
		{
			readerNav.RollBackMove(ref curDepth);
			throw;
		}
		curDepth++;
		nodeType = readerNav.NodeType;
	}

	public override bool MoveToFirstAttribute()
	{
		if (!IsInReadingStates())
		{
			return false;
		}
		readerNav.ResetMove(ref curDepth, ref nodeType);
		if (AttributeCount > 0)
		{
			readerNav.MoveToAttribute(0);
			curDepth++;
			nodeType = readerNav.NodeType;
			if (bInReadBinary)
			{
				FinishReadBinary();
			}
			return true;
		}
		readerNav.RollBackMove(ref curDepth);
		return false;
	}

	public override bool MoveToNextAttribute()
	{
		if (!IsInReadingStates() || nodeType == XmlNodeType.EndElement)
		{
			return false;
		}
		readerNav.LogMove(curDepth);
		readerNav.ResetToAttribute(ref curDepth);
		if (readerNav.MoveToNextAttribute(ref curDepth))
		{
			nodeType = readerNav.NodeType;
			if (bInReadBinary)
			{
				FinishReadBinary();
			}
			return true;
		}
		readerNav.RollBackMove(ref curDepth);
		return false;
	}

	public override bool MoveToElement()
	{
		if (!IsInReadingStates())
		{
			return false;
		}
		readerNav.LogMove(curDepth);
		readerNav.ResetToAttribute(ref curDepth);
		if (readerNav.MoveToElement())
		{
			curDepth--;
			nodeType = readerNav.NodeType;
			if (bInReadBinary)
			{
				FinishReadBinary();
			}
			return true;
		}
		readerNav.RollBackMove(ref curDepth);
		return false;
	}

	public override bool Read()
	{
		return Read(fSkipChildren: false);
	}

	private bool Read(bool fSkipChildren)
	{
		if (fEOF)
		{
			return false;
		}
		if (readState == ReadState.Initial)
		{
			if (readerNav.NodeType == XmlNodeType.Document || readerNav.NodeType == XmlNodeType.DocumentFragment)
			{
				bStartFromDocument = true;
				if (!ReadNextNode(fSkipChildren))
				{
					readState = ReadState.Error;
					return false;
				}
			}
			ReSetReadingMarks();
			readState = ReadState.Interactive;
			nodeType = readerNav.NodeType;
			curDepth = 0;
			return true;
		}
		if (bInReadBinary)
		{
			FinishReadBinary();
		}
		if (readerNav.CreatedOnAttribute)
		{
			return false;
		}
		ReSetReadingMarks();
		if (ReadNextNode(fSkipChildren))
		{
			return true;
		}
		if (readState == ReadState.Initial || readState == ReadState.Interactive)
		{
			readState = ReadState.Error;
		}
		if (readState == ReadState.EndOfFile)
		{
			nodeType = XmlNodeType.None;
		}
		return false;
	}

	private bool ReadNextNode(bool fSkipChildren)
	{
		if (readState != ReadState.Interactive && readState != ReadState.Initial)
		{
			nodeType = XmlNodeType.None;
			return false;
		}
		bool num = !fSkipChildren;
		XmlNodeType xmlNodeType = readerNav.NodeType;
		if (num && nodeType != XmlNodeType.EndElement && nodeType != XmlNodeType.EndEntity && (xmlNodeType == XmlNodeType.Element || (xmlNodeType == XmlNodeType.EntityReference && bResolveEntity) || ((readerNav.NodeType == XmlNodeType.Document || readerNav.NodeType == XmlNodeType.DocumentFragment) && readState == ReadState.Initial)))
		{
			if (readerNav.MoveToFirstChild())
			{
				nodeType = readerNav.NodeType;
				curDepth++;
				if (bResolveEntity)
				{
					bResolveEntity = false;
				}
				return true;
			}
			if (readerNav.NodeType == XmlNodeType.Element && !readerNav.IsEmptyElement)
			{
				nodeType = XmlNodeType.EndElement;
				return true;
			}
			if (readerNav.NodeType == XmlNodeType.EntityReference && bResolveEntity)
			{
				bResolveEntity = false;
				nodeType = XmlNodeType.EndEntity;
				return true;
			}
			return ReadForward(fSkipChildren);
		}
		if (readerNav.NodeType == XmlNodeType.EntityReference && bResolveEntity)
		{
			if (readerNav.MoveToFirstChild())
			{
				nodeType = readerNav.NodeType;
				curDepth++;
			}
			else
			{
				nodeType = XmlNodeType.EndEntity;
			}
			bResolveEntity = false;
			return true;
		}
		return ReadForward(fSkipChildren);
	}

	private void SetEndOfFile()
	{
		fEOF = true;
		readState = ReadState.EndOfFile;
		nodeType = XmlNodeType.None;
	}

	private bool ReadAtZeroLevel(bool fSkipChildren)
	{
		if (!fSkipChildren && nodeType != XmlNodeType.EndElement && readerNav.NodeType == XmlNodeType.Element && !readerNav.IsEmptyElement)
		{
			nodeType = XmlNodeType.EndElement;
			return true;
		}
		SetEndOfFile();
		return false;
	}

	private bool ReadForward(bool fSkipChildren)
	{
		if (readState == ReadState.Error)
		{
			return false;
		}
		if (!bStartFromDocument && curDepth == 0)
		{
			return ReadAtZeroLevel(fSkipChildren);
		}
		if (readerNav.MoveToNext())
		{
			nodeType = readerNav.NodeType;
			return true;
		}
		if (curDepth == 0)
		{
			return ReadAtZeroLevel(fSkipChildren);
		}
		if (readerNav.MoveToParent())
		{
			if (readerNav.NodeType == XmlNodeType.Element)
			{
				curDepth--;
				nodeType = XmlNodeType.EndElement;
				return true;
			}
			if (readerNav.NodeType == XmlNodeType.EntityReference)
			{
				curDepth--;
				nodeType = XmlNodeType.EndEntity;
				return true;
			}
			return true;
		}
		return false;
	}

	private void ReSetReadingMarks()
	{
		readerNav.ResetMove(ref curDepth, ref nodeType);
	}

	public override void Close()
	{
		readState = ReadState.Closed;
	}

	public override void Skip()
	{
		Read(fSkipChildren: true);
	}

	public override string ReadString()
	{
		if (NodeType == XmlNodeType.EntityReference && bResolveEntity && !Read())
		{
			throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
		}
		return base.ReadString();
	}

	public override string LookupNamespace(string prefix)
	{
		if (!IsInReadingStates())
		{
			return null;
		}
		string text = readerNav.LookupNamespace(prefix);
		if (text != null && text.Length == 0)
		{
			return null;
		}
		return text;
	}

	public override void ResolveEntity()
	{
		if (!IsInReadingStates() || nodeType != XmlNodeType.EntityReference)
		{
			throw new InvalidOperationException(Res.GetString("The node is not an expandable 'EntityReference' node."));
		}
		bResolveEntity = true;
	}

	public override bool ReadAttributeValue()
	{
		if (!IsInReadingStates())
		{
			return false;
		}
		if (readerNav.ReadAttributeValue(ref curDepth, ref bResolveEntity, ref nodeType))
		{
			bInReadBinary = false;
			return true;
		}
		return false;
	}

	public override int ReadContentAsBase64(byte[] buffer, int index, int count)
	{
		if (readState != ReadState.Interactive)
		{
			return 0;
		}
		if (!bInReadBinary)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
		}
		bInReadBinary = false;
		int result = readBinaryHelper.ReadContentAsBase64(buffer, index, count);
		bInReadBinary = true;
		return result;
	}

	public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
	{
		if (readState != ReadState.Interactive)
		{
			return 0;
		}
		if (!bInReadBinary)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
		}
		bInReadBinary = false;
		int result = readBinaryHelper.ReadContentAsBinHex(buffer, index, count);
		bInReadBinary = true;
		return result;
	}

	public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
	{
		if (readState != ReadState.Interactive)
		{
			return 0;
		}
		if (!bInReadBinary)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
		}
		bInReadBinary = false;
		int result = readBinaryHelper.ReadElementContentAsBase64(buffer, index, count);
		bInReadBinary = true;
		return result;
	}

	public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
	{
		if (readState != ReadState.Interactive)
		{
			return 0;
		}
		if (!bInReadBinary)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
		}
		bInReadBinary = false;
		int result = readBinaryHelper.ReadElementContentAsBinHex(buffer, index, count);
		bInReadBinary = true;
		return result;
	}

	private void FinishReadBinary()
	{
		bInReadBinary = false;
		readBinaryHelper.Finish();
	}

	IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
	{
		return readerNav.GetNamespacesInScope(scope);
	}

	string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
	{
		return readerNav.LookupPrefix(namespaceName);
	}

	string IXmlNamespaceResolver.LookupNamespace(string prefix)
	{
		if (!IsInReadingStates())
		{
			return readerNav.DefaultLookupNamespace(prefix);
		}
		string text = readerNav.LookupNamespace(prefix);
		if (text != null)
		{
			text = readerNav.NameTable.Add(text);
		}
		return text;
	}
}
