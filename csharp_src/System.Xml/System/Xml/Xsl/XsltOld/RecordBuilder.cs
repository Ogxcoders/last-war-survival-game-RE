using System.Collections;
using System.Text;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal sealed class RecordBuilder
{
	private int outputState;

	private RecordBuilder next;

	private RecordOutput output;

	private XmlNameTable nameTable;

	private OutKeywords atoms;

	private OutputScopeManager scopeManager;

	private BuilderInfo mainNode = new BuilderInfo();

	private ArrayList attributeList = new ArrayList();

	private int attributeCount;

	private ArrayList namespaceList = new ArrayList();

	private int namespaceCount;

	private BuilderInfo dummy = new BuilderInfo();

	private BuilderInfo currentInfo;

	private bool popScope;

	private int recordState;

	private int recordDepth;

	private const int NoRecord = 0;

	private const int SomeRecord = 1;

	private const int HaveRecord = 2;

	private const char s_Minus = '-';

	private const string s_Space = " ";

	private const string s_SpaceMinus = " -";

	private const char s_Question = '?';

	private const char s_Greater = '>';

	private const string s_SpaceGreater = " >";

	private const string PrefixFormat = "xp_{0}";

	internal int OutputState
	{
		get
		{
			return outputState;
		}
		set
		{
			outputState = value;
		}
	}

	internal RecordBuilder Next
	{
		get
		{
			return next;
		}
		set
		{
			next = value;
		}
	}

	internal RecordOutput Output => output;

	internal BuilderInfo MainNode => mainNode;

	internal ArrayList AttributeList => attributeList;

	internal int AttributeCount => attributeCount;

	internal OutputScopeManager Manager => scopeManager;

	internal RecordBuilder(RecordOutput output, XmlNameTable nameTable)
	{
		this.output = output;
		this.nameTable = ((nameTable != null) ? nameTable : new NameTable());
		atoms = new OutKeywords(this.nameTable);
		scopeManager = new OutputScopeManager(this.nameTable, atoms);
	}

	private void ValueAppend(string s, bool disableOutputEscaping)
	{
		currentInfo.ValueAppend(s, disableOutputEscaping);
	}

	private bool CanOutput(int state)
	{
		if (recordState == 0 || (state & 0x2000) == 0)
		{
			return true;
		}
		recordState = 2;
		FinalizeRecord();
		SetEmptyFlag(state);
		return output.RecordDone(this) == Processor.OutputResult.Continue;
	}

	internal Processor.OutputResult BeginEvent(int state, XPathNodeType nodeType, string prefix, string name, string nspace, bool empty, object htmlProps, bool search)
	{
		if (!CanOutput(state))
		{
			return Processor.OutputResult.Overflow;
		}
		AdjustDepth(state);
		ResetRecord(state);
		PopElementScope();
		prefix = ((prefix != null) ? nameTable.Add(prefix) : atoms.Empty);
		name = ((name != null) ? nameTable.Add(name) : atoms.Empty);
		nspace = ((nspace != null) ? nameTable.Add(nspace) : atoms.Empty);
		switch (nodeType)
		{
		case XPathNodeType.Element:
			mainNode.htmlProps = htmlProps as HtmlElementProps;
			mainNode.search = search;
			BeginElement(prefix, name, nspace, empty);
			break;
		case XPathNodeType.Attribute:
			BeginAttribute(prefix, name, nspace, htmlProps, search);
			break;
		case XPathNodeType.Namespace:
			BeginNamespace(name, nspace);
			break;
		case XPathNodeType.ProcessingInstruction:
			if (!BeginProcessingInstruction(prefix, name, nspace))
			{
				return Processor.OutputResult.Error;
			}
			break;
		case XPathNodeType.Comment:
			BeginComment();
			break;
		}
		return CheckRecordBegin(state);
	}

	internal Processor.OutputResult TextEvent(int state, string text, bool disableOutputEscaping)
	{
		if (!CanOutput(state))
		{
			return Processor.OutputResult.Overflow;
		}
		AdjustDepth(state);
		ResetRecord(state);
		PopElementScope();
		if ((state & 0x2000) != 0)
		{
			currentInfo.Depth = recordDepth;
			currentInfo.NodeType = XmlNodeType.Text;
		}
		ValueAppend(text, disableOutputEscaping);
		return CheckRecordBegin(state);
	}

	internal Processor.OutputResult EndEvent(int state, XPathNodeType nodeType)
	{
		if (!CanOutput(state))
		{
			return Processor.OutputResult.Overflow;
		}
		AdjustDepth(state);
		PopElementScope();
		popScope = (state & 0x10000) != 0;
		if ((state & 0x1000) != 0 && mainNode.IsEmptyTag)
		{
			return Processor.OutputResult.Continue;
		}
		ResetRecord(state);
		if ((state & 0x2000) != 0 && nodeType == XPathNodeType.Element)
		{
			EndElement();
		}
		return CheckRecordEnd(state);
	}

	internal void Reset()
	{
		if (recordState == 2)
		{
			recordState = 0;
		}
	}

	internal void TheEnd()
	{
		if (recordState == 1)
		{
			recordState = 2;
			FinalizeRecord();
			output.RecordDone(this);
		}
		output.TheEnd();
	}

	private int FindAttribute(string name, string nspace, ref string prefix)
	{
		for (int i = 0; i < attributeCount; i++)
		{
			BuilderInfo builderInfo = (BuilderInfo)attributeList[i];
			if (Ref.Equal(builderInfo.LocalName, name))
			{
				if (Ref.Equal(builderInfo.NamespaceURI, nspace))
				{
					return i;
				}
				if (Ref.Equal(builderInfo.Prefix, prefix))
				{
					prefix = string.Empty;
				}
			}
		}
		return -1;
	}

	private void BeginElement(string prefix, string name, string nspace, bool empty)
	{
		currentInfo.NodeType = XmlNodeType.Element;
		currentInfo.Prefix = prefix;
		currentInfo.LocalName = name;
		currentInfo.NamespaceURI = nspace;
		currentInfo.Depth = recordDepth;
		currentInfo.IsEmptyTag = empty;
		scopeManager.PushScope(name, nspace, prefix);
	}

	private void EndElement()
	{
		OutputScope currentElementScope = scopeManager.CurrentElementScope;
		currentInfo.NodeType = XmlNodeType.EndElement;
		currentInfo.Prefix = currentElementScope.Prefix;
		currentInfo.LocalName = currentElementScope.Name;
		currentInfo.NamespaceURI = currentElementScope.Namespace;
		currentInfo.Depth = recordDepth;
	}

	private int NewAttribute()
	{
		if (attributeCount >= attributeList.Count)
		{
			attributeList.Add(new BuilderInfo());
		}
		return attributeCount++;
	}

	private void BeginAttribute(string prefix, string name, string nspace, object htmlAttrProps, bool search)
	{
		int num = FindAttribute(name, nspace, ref prefix);
		if (num == -1)
		{
			num = NewAttribute();
		}
		BuilderInfo builderInfo = (BuilderInfo)attributeList[num];
		builderInfo.Initialize(prefix, name, nspace);
		builderInfo.Depth = recordDepth;
		builderInfo.NodeType = XmlNodeType.Attribute;
		builderInfo.htmlAttrProps = htmlAttrProps as HtmlAttributeProps;
		builderInfo.search = search;
		currentInfo = builderInfo;
	}

	private void BeginNamespace(string name, string nspace)
	{
		bool thisScope = false;
		if (Ref.Equal(name, atoms.Empty))
		{
			if (!Ref.Equal(nspace, scopeManager.DefaultNamespace) && !Ref.Equal(mainNode.NamespaceURI, atoms.Empty))
			{
				DeclareNamespace(nspace, name);
			}
		}
		else
		{
			string text = scopeManager.ResolveNamespace(name, out thisScope);
			if (text != null)
			{
				if (!Ref.Equal(nspace, text) && !thisScope)
				{
					DeclareNamespace(nspace, name);
				}
			}
			else
			{
				DeclareNamespace(nspace, name);
			}
		}
		currentInfo = dummy;
		currentInfo.NodeType = XmlNodeType.Attribute;
	}

	private bool BeginProcessingInstruction(string prefix, string name, string nspace)
	{
		currentInfo.NodeType = XmlNodeType.ProcessingInstruction;
		currentInfo.Prefix = prefix;
		currentInfo.LocalName = name;
		currentInfo.NamespaceURI = nspace;
		currentInfo.Depth = recordDepth;
		return true;
	}

	private void BeginComment()
	{
		currentInfo.NodeType = XmlNodeType.Comment;
		currentInfo.Depth = recordDepth;
	}

	private void AdjustDepth(int state)
	{
		switch (state & 0x300)
		{
		case 256:
			recordDepth++;
			break;
		case 512:
			recordDepth--;
			break;
		}
	}

	private void ResetRecord(int state)
	{
		if ((state & 0x2000) != 0)
		{
			attributeCount = 0;
			namespaceCount = 0;
			currentInfo = mainNode;
			currentInfo.Initialize(atoms.Empty, atoms.Empty, atoms.Empty);
			currentInfo.NodeType = XmlNodeType.None;
			currentInfo.IsEmptyTag = false;
			currentInfo.htmlProps = null;
			currentInfo.htmlAttrProps = null;
		}
	}

	private void PopElementScope()
	{
		if (popScope)
		{
			scopeManager.PopScope();
			popScope = false;
		}
	}

	private Processor.OutputResult CheckRecordBegin(int state)
	{
		if ((state & 0x4000) != 0)
		{
			recordState = 2;
			FinalizeRecord();
			SetEmptyFlag(state);
			return output.RecordDone(this);
		}
		recordState = 1;
		return Processor.OutputResult.Continue;
	}

	private Processor.OutputResult CheckRecordEnd(int state)
	{
		if ((state & 0x4000) != 0)
		{
			recordState = 2;
			FinalizeRecord();
			SetEmptyFlag(state);
			return output.RecordDone(this);
		}
		return Processor.OutputResult.Continue;
	}

	private void SetEmptyFlag(int state)
	{
		if ((state & 0x400) != 0)
		{
			mainNode.IsEmptyTag = false;
		}
	}

	private void AnalyzeSpaceLang()
	{
		for (int i = 0; i < attributeCount; i++)
		{
			BuilderInfo builderInfo = (BuilderInfo)attributeList[i];
			if (Ref.Equal(builderInfo.Prefix, atoms.Xml))
			{
				OutputScope currentElementScope = scopeManager.CurrentElementScope;
				if (Ref.Equal(builderInfo.LocalName, atoms.Lang))
				{
					currentElementScope.Lang = builderInfo.Value;
				}
				else if (Ref.Equal(builderInfo.LocalName, atoms.Space))
				{
					currentElementScope.Space = TranslateXmlSpace(builderInfo.Value);
				}
			}
		}
	}

	private void FixupElement()
	{
		if (Ref.Equal(mainNode.NamespaceURI, atoms.Empty))
		{
			mainNode.Prefix = atoms.Empty;
		}
		if (Ref.Equal(mainNode.Prefix, atoms.Empty))
		{
			if (!Ref.Equal(mainNode.NamespaceURI, scopeManager.DefaultNamespace))
			{
				DeclareNamespace(mainNode.NamespaceURI, mainNode.Prefix);
			}
		}
		else
		{
			bool thisScope = false;
			string text = scopeManager.ResolveNamespace(mainNode.Prefix, out thisScope);
			if (text != null)
			{
				if (!Ref.Equal(mainNode.NamespaceURI, text))
				{
					if (thisScope)
					{
						mainNode.Prefix = GetPrefixForNamespace(mainNode.NamespaceURI);
					}
					else
					{
						DeclareNamespace(mainNode.NamespaceURI, mainNode.Prefix);
					}
				}
			}
			else
			{
				DeclareNamespace(mainNode.NamespaceURI, mainNode.Prefix);
			}
		}
		scopeManager.CurrentElementScope.Prefix = mainNode.Prefix;
	}

	private void FixupAttributes(int attributeCount)
	{
		for (int i = 0; i < attributeCount; i++)
		{
			BuilderInfo builderInfo = (BuilderInfo)attributeList[i];
			if (Ref.Equal(builderInfo.NamespaceURI, atoms.Empty))
			{
				builderInfo.Prefix = atoms.Empty;
				continue;
			}
			if (Ref.Equal(builderInfo.Prefix, atoms.Empty))
			{
				builderInfo.Prefix = GetPrefixForNamespace(builderInfo.NamespaceURI);
				continue;
			}
			bool thisScope = false;
			string text = scopeManager.ResolveNamespace(builderInfo.Prefix, out thisScope);
			if (text != null)
			{
				if (!Ref.Equal(builderInfo.NamespaceURI, text))
				{
					if (thisScope)
					{
						builderInfo.Prefix = GetPrefixForNamespace(builderInfo.NamespaceURI);
					}
					else
					{
						DeclareNamespace(builderInfo.NamespaceURI, builderInfo.Prefix);
					}
				}
			}
			else
			{
				DeclareNamespace(builderInfo.NamespaceURI, builderInfo.Prefix);
			}
		}
	}

	private void AppendNamespaces()
	{
		for (int num = namespaceCount - 1; num >= 0; num--)
		{
			((BuilderInfo)attributeList[NewAttribute()]).Initialize((BuilderInfo)namespaceList[num]);
		}
	}

	private void AnalyzeComment()
	{
		StringBuilder stringBuilder = null;
		string value = mainNode.Value;
		bool flag = false;
		int i = 0;
		int num = 0;
		for (; i < value.Length; i++)
		{
			if (value[i] == '-')
			{
				if (flag)
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(value, num, i, 2 * value.Length);
					}
					else
					{
						stringBuilder.Append(value, num, i - num);
					}
					stringBuilder.Append(" -");
					num = i + 1;
				}
				flag = true;
			}
			else
			{
				flag = false;
			}
		}
		if (stringBuilder != null)
		{
			if (num < value.Length)
			{
				stringBuilder.Append(value, num, value.Length - num);
			}
			if (flag)
			{
				stringBuilder.Append(" ");
			}
			mainNode.Value = stringBuilder.ToString();
		}
		else if (flag)
		{
			mainNode.ValueAppend(" ", disableEscaping: false);
		}
	}

	private void AnalyzeProcessingInstruction()
	{
		StringBuilder stringBuilder = null;
		string value = mainNode.Value;
		bool flag = false;
		int i = 0;
		int num = 0;
		for (; i < value.Length; i++)
		{
			switch (value[i])
			{
			case '?':
				flag = true;
				break;
			case '>':
				if (flag)
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(value, num, i, 2 * value.Length);
					}
					else
					{
						stringBuilder.Append(value, num, i - num);
					}
					stringBuilder.Append(" >");
					num = i + 1;
				}
				flag = false;
				break;
			default:
				flag = false;
				break;
			}
		}
		if (stringBuilder != null)
		{
			if (num < value.Length)
			{
				stringBuilder.Append(value, num, value.Length - num);
			}
			mainNode.Value = stringBuilder.ToString();
		}
	}

	private void FinalizeRecord()
	{
		switch (mainNode.NodeType)
		{
		case XmlNodeType.Element:
		{
			int num = attributeCount;
			FixupElement();
			FixupAttributes(num);
			AnalyzeSpaceLang();
			AppendNamespaces();
			break;
		}
		case XmlNodeType.Comment:
			AnalyzeComment();
			break;
		case XmlNodeType.ProcessingInstruction:
			AnalyzeProcessingInstruction();
			break;
		}
	}

	private int NewNamespace()
	{
		if (namespaceCount >= namespaceList.Count)
		{
			namespaceList.Add(new BuilderInfo());
		}
		return namespaceCount++;
	}

	private void DeclareNamespace(string nspace, string prefix)
	{
		int index = NewNamespace();
		BuilderInfo builderInfo = (BuilderInfo)namespaceList[index];
		if (prefix == atoms.Empty)
		{
			builderInfo.Initialize(atoms.Empty, atoms.Xmlns, atoms.XmlnsNamespace);
		}
		else
		{
			builderInfo.Initialize(atoms.Xmlns, prefix, atoms.XmlnsNamespace);
		}
		builderInfo.Depth = recordDepth;
		builderInfo.NodeType = XmlNodeType.Attribute;
		builderInfo.Value = nspace;
		scopeManager.PushNamespace(prefix, nspace);
	}

	private string DeclareNewNamespace(string nspace)
	{
		string text = scopeManager.GeneratePrefix("xp_{0}");
		DeclareNamespace(nspace, text);
		return text;
	}

	internal string GetPrefixForNamespace(string nspace)
	{
		string prefix = null;
		if (scopeManager.FindPrefix(nspace, out prefix))
		{
			return prefix;
		}
		return DeclareNewNamespace(nspace);
	}

	private static XmlSpace TranslateXmlSpace(string space)
	{
		if (space == "default")
		{
			return XmlSpace.Default;
		}
		if (space == "preserve")
		{
			return XmlSpace.Preserve;
		}
		return XmlSpace.None;
	}
}
