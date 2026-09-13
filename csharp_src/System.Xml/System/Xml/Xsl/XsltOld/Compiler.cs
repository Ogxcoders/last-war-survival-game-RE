using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;
using System.Xml.Xsl.Xslt;
using System.Xml.Xsl.XsltOld.Debugger;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class Compiler
{
	internal class ErrorXPathExpression : CompiledXpathExpr
	{
		private string baseUri;

		private int lineNumber;

		private int linePosition;

		public ErrorXPathExpression(string expression, string baseUri, int lineNumber, int linePosition)
			: base(null, expression, needContext: false)
		{
			this.baseUri = baseUri;
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
		}

		public override XPathExpression Clone()
		{
			return this;
		}

		public override void CheckErrors()
		{
			throw new XsltException("'{0}' is an invalid XPath expression.", new string[1] { Expression }, baseUri, linePosition, lineNumber, null);
		}
	}

	internal const int InvalidQueryKey = -1;

	internal const double RootPriority = 0.5;

	internal StringBuilder AvtStringBuilder = new StringBuilder();

	private int stylesheetid;

	private InputScope rootScope;

	private XmlResolver xmlResolver;

	private TemplateBaseAction currentTemplate;

	private XmlQualifiedName currentMode;

	private Hashtable globalNamespaceAliasTable;

	private Stack stylesheets;

	private HybridDictionary documentURIs = new HybridDictionary();

	private NavigatorInput input;

	private KeywordsTable atoms;

	private InputScopeManager scopeManager;

	internal Stylesheet stylesheet;

	internal Stylesheet rootStylesheet;

	private RootAction rootAction;

	private List<TheQuery> queryStore;

	private QueryBuilder queryBuilder = new QueryBuilder();

	private int rtfCount;

	public bool AllowBuiltInMode;

	public static XmlQualifiedName BuiltInMode = new XmlQualifiedName("*", string.Empty);

	private Hashtable[] _typeDeclsByLang = new Hashtable[3]
	{
		new Hashtable(),
		new Hashtable(),
		new Hashtable()
	};

	private ArrayList scriptFiles = new ArrayList();

	private static string[] _defaultNamespaces = new string[7] { "System", "System.Collections", "System.Text", "System.Text.RegularExpressions", "System.Xml", "System.Xml.Xsl", "System.Xml.XPath" };

	private static int scriptClassCounter = 0;

	internal KeywordsTable Atoms => atoms;

	internal int Stylesheetid
	{
		get
		{
			return stylesheetid;
		}
		set
		{
			stylesheetid = value;
		}
	}

	internal NavigatorInput Document => input;

	internal NavigatorInput Input => input;

	internal Stylesheet CompiledStylesheet => stylesheet;

	internal RootAction RootAction
	{
		get
		{
			return rootAction;
		}
		set
		{
			rootAction = value;
			currentTemplate = rootAction;
		}
	}

	internal List<TheQuery> QueryStore => queryStore;

	public virtual IXsltDebugger Debugger => null;

	internal bool ForwardCompatibility
	{
		get
		{
			return scopeManager.CurrentScope.ForwardCompatibility;
		}
		set
		{
			scopeManager.CurrentScope.ForwardCompatibility = value;
		}
	}

	internal bool CanHaveApplyImports
	{
		get
		{
			return scopeManager.CurrentScope.CanHaveApplyImports;
		}
		set
		{
			scopeManager.CurrentScope.CanHaveApplyImports = value;
		}
	}

	protected InputScopeManager ScopeManager => scopeManager;

	internal string DefaultNamespace => scopeManager.DefaultNamespace;

	internal XmlQualifiedName CurrentMode => currentMode;

	internal bool Advance()
	{
		return Document.Advance();
	}

	internal bool Recurse()
	{
		return Document.Recurse();
	}

	internal bool ToParent()
	{
		return Document.ToParent();
	}

	internal string GetUnicRtfId()
	{
		rtfCount++;
		return rtfCount.ToString(CultureInfo.InvariantCulture);
	}

	internal void Compile(NavigatorInput input, XmlResolver xmlResolver, Evidence evidence)
	{
		evidence = null;
		this.xmlResolver = xmlResolver;
		PushInputDocument(input);
		rootScope = scopeManager.PushScope();
		queryStore = new List<TheQuery>();
		try
		{
			rootStylesheet = new Stylesheet();
			PushStylesheet(rootStylesheet);
			try
			{
				CreateRootAction();
			}
			catch (XsltCompileException)
			{
				throw;
			}
			catch (Exception inner)
			{
				throw new XsltCompileException(inner, Input.BaseURI, Input.LineNumber, Input.LinePosition);
			}
			stylesheet.ProcessTemplates();
			rootAction.PorcessAttributeSets(rootStylesheet);
			stylesheet.SortWhiteSpace();
			if (evidence != null)
			{
				rootAction.permissions = SecurityManager.GetStandardSandbox(evidence);
			}
			if (globalNamespaceAliasTable != null)
			{
				stylesheet.ReplaceNamespaceAlias(this);
				rootAction.ReplaceNamespaceAlias(this);
			}
		}
		finally
		{
			PopInputDocument();
		}
	}

	internal void InsertExtensionNamespace(string value)
	{
		string[] array = ResolvePrefixes(value);
		if (array != null)
		{
			scopeManager.InsertExtensionNamespaces(array);
		}
	}

	internal void InsertExcludedNamespace(string value)
	{
		string[] array = ResolvePrefixes(value);
		if (array != null)
		{
			scopeManager.InsertExcludedNamespaces(array);
		}
	}

	internal void InsertExtensionNamespace()
	{
		InsertExtensionNamespace(Input.Navigator.GetAttribute(Input.Atoms.ExtensionElementPrefixes, Input.Atoms.UriXsl));
	}

	internal void InsertExcludedNamespace()
	{
		InsertExcludedNamespace(Input.Navigator.GetAttribute(Input.Atoms.ExcludeResultPrefixes, Input.Atoms.UriXsl));
	}

	internal bool IsExtensionNamespace(string nspace)
	{
		return scopeManager.IsExtensionNamespace(nspace);
	}

	internal bool IsExcludedNamespace(string nspace)
	{
		return scopeManager.IsExcludedNamespace(nspace);
	}

	internal void PushLiteralScope()
	{
		PushNamespaceScope();
		string attribute = Input.Navigator.GetAttribute(Atoms.Version, Atoms.UriXsl);
		if (attribute.Length != 0)
		{
			ForwardCompatibility = attribute != "1.0";
		}
	}

	internal void PushNamespaceScope()
	{
		scopeManager.PushScope();
		NavigatorInput navigatorInput = Input;
		if (navigatorInput.MoveToFirstNamespace())
		{
			do
			{
				scopeManager.PushNamespace(navigatorInput.LocalName, navigatorInput.Value);
			}
			while (navigatorInput.MoveToNextNamespace());
			navigatorInput.ToParent();
		}
	}

	internal virtual void PopScope()
	{
		currentTemplate.ReleaseVariableSlots(scopeManager.CurrentScope.GetVeriablesCount());
		scopeManager.PopScope();
	}

	internal InputScopeManager CloneScopeManager()
	{
		return scopeManager.Clone();
	}

	internal int InsertVariable(VariableAction variable)
	{
		InputScope inputScope = ((!variable.IsGlobal) ? scopeManager.VariableScope : rootScope);
		VariableAction variableAction = inputScope.ResolveVariable(variable.Name);
		if (variableAction != null)
		{
			if (!variableAction.IsGlobal)
			{
				throw XsltException.Create("Variable or parameter '{0}' was duplicated within the same scope.", variable.NameStr);
			}
			if (variable.IsGlobal)
			{
				if (variable.Stylesheetid == variableAction.Stylesheetid)
				{
					throw XsltException.Create("Variable or parameter '{0}' was duplicated within the same scope.", variable.NameStr);
				}
				if (variable.Stylesheetid < variableAction.Stylesheetid)
				{
					inputScope.InsertVariable(variable);
					return variableAction.VarKey;
				}
				return -1;
			}
		}
		inputScope.InsertVariable(variable);
		return currentTemplate.AllocateVariableSlot();
	}

	internal void AddNamespaceAlias(string StylesheetURI, NamespaceInfo AliasInfo)
	{
		if (globalNamespaceAliasTable == null)
		{
			globalNamespaceAliasTable = new Hashtable();
		}
		if (!(globalNamespaceAliasTable[StylesheetURI] is NamespaceInfo namespaceInfo) || AliasInfo.stylesheetId <= namespaceInfo.stylesheetId)
		{
			globalNamespaceAliasTable[StylesheetURI] = AliasInfo;
		}
	}

	internal bool IsNamespaceAlias(string StylesheetURI)
	{
		if (globalNamespaceAliasTable == null)
		{
			return false;
		}
		return globalNamespaceAliasTable.Contains(StylesheetURI);
	}

	internal NamespaceInfo FindNamespaceAlias(string StylesheetURI)
	{
		if (globalNamespaceAliasTable != null)
		{
			return (NamespaceInfo)globalNamespaceAliasTable[StylesheetURI];
		}
		return null;
	}

	internal string ResolveXmlNamespace(string prefix)
	{
		return scopeManager.ResolveXmlNamespace(prefix);
	}

	internal string ResolveXPathNamespace(string prefix)
	{
		return scopeManager.ResolveXPathNamespace(prefix);
	}

	internal void InsertKey(XmlQualifiedName name, int MatchKey, int UseKey)
	{
		rootAction.InsertKey(name, MatchKey, UseKey);
	}

	internal void AddDecimalFormat(XmlQualifiedName name, DecimalFormat formatinfo)
	{
		rootAction.AddDecimalFormat(name, formatinfo);
	}

	private string[] ResolvePrefixes(string tokens)
	{
		if (tokens == null || tokens.Length == 0)
		{
			return null;
		}
		string[] array = XmlConvert.SplitString(tokens);
		try
		{
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				array[i] = scopeManager.ResolveXmlNamespace((text == "#default") ? string.Empty : text);
			}
			return array;
		}
		catch (XsltException)
		{
			if (!ForwardCompatibility)
			{
				throw;
			}
			return null;
		}
	}

	internal bool GetYesNo(string value)
	{
		if (value == "yes")
		{
			return true;
		}
		if (value == "no")
		{
			return false;
		}
		throw XsltException.Create("'{1}' is an invalid value for the '{0}' attribute.", Input.LocalName, value);
	}

	internal string GetSingleAttribute(string attributeAtom)
	{
		NavigatorInput navigatorInput = Input;
		string localName = navigatorInput.LocalName;
		string text = null;
		if (navigatorInput.MoveToFirstAttribute())
		{
			do
			{
				string namespaceURI = navigatorInput.NamespaceURI;
				string localName2 = navigatorInput.LocalName;
				if (namespaceURI.Length == 0)
				{
					if (Ref.Equal(localName2, attributeAtom))
					{
						text = navigatorInput.Value;
					}
					else if (!ForwardCompatibility)
					{
						throw XsltException.Create("'{0}' is an invalid attribute for the '{1}' element.", localName2, localName);
					}
				}
			}
			while (navigatorInput.MoveToNextAttribute());
			navigatorInput.ToParent();
		}
		if (text == null)
		{
			throw XsltException.Create("Missing mandatory attribute '{0}'.", attributeAtom);
		}
		return text;
	}

	internal XmlQualifiedName CreateXPathQName(string qname)
	{
		PrefixQName.ParseQualifiedName(qname, out var prefix, out var local);
		return new XmlQualifiedName(local, scopeManager.ResolveXPathNamespace(prefix));
	}

	internal XmlQualifiedName CreateXmlQName(string qname)
	{
		PrefixQName.ParseQualifiedName(qname, out var prefix, out var local);
		return new XmlQualifiedName(local, scopeManager.ResolveXmlNamespace(prefix));
	}

	internal static XPathDocument LoadDocument(XmlTextReaderImpl reader)
	{
		reader.EntityHandling = EntityHandling.ExpandEntities;
		reader.XmlValidatingReaderCompatibilityMode = true;
		try
		{
			return new XPathDocument(reader, XmlSpace.Preserve);
		}
		finally
		{
			reader.Close();
		}
	}

	private void AddDocumentURI(string href)
	{
		documentURIs.Add(href, null);
	}

	private void RemoveDocumentURI(string href)
	{
		documentURIs.Remove(href);
	}

	internal bool IsCircularReference(string href)
	{
		return documentURIs.Contains(href);
	}

	internal Uri ResolveUri(string relativeUri)
	{
		string baseURI = Input.BaseURI;
		Uri uri = xmlResolver.ResolveUri((baseURI.Length != 0) ? xmlResolver.ResolveUri(null, baseURI) : null, relativeUri);
		if (uri == null)
		{
			throw XsltException.Create("Cannot resolve the referenced document '{0}'.", relativeUri);
		}
		return uri;
	}

	internal NavigatorInput ResolveDocument(Uri absoluteUri)
	{
		object entity = xmlResolver.GetEntity(absoluteUri, null, null);
		string text = absoluteUri.ToString();
		if (entity is Stream)
		{
			return new NavigatorInput(LoadDocument(new XmlTextReaderImpl(text, (Stream)entity)
			{
				XmlResolver = xmlResolver
			}).CreateNavigator(), text, rootScope);
		}
		if (entity is XPathNavigator)
		{
			return new NavigatorInput((XPathNavigator)entity, text, rootScope);
		}
		throw XsltException.Create("Cannot resolve the referenced document '{0}'.", text);
	}

	internal void PushInputDocument(NavigatorInput newInput)
	{
		string href = newInput.Href;
		AddDocumentURI(href);
		newInput.Next = input;
		input = newInput;
		atoms = input.Atoms;
		scopeManager = input.InputScopeManager;
	}

	internal void PopInputDocument()
	{
		NavigatorInput navigatorInput = input;
		input = navigatorInput.Next;
		navigatorInput.Next = null;
		if (input != null)
		{
			atoms = input.Atoms;
			scopeManager = input.InputScopeManager;
		}
		else
		{
			atoms = null;
			scopeManager = null;
		}
		RemoveDocumentURI(navigatorInput.Href);
		navigatorInput.Close();
	}

	internal void PushStylesheet(Stylesheet stylesheet)
	{
		if (stylesheets == null)
		{
			stylesheets = new Stack();
		}
		stylesheets.Push(stylesheet);
		this.stylesheet = stylesheet;
	}

	internal Stylesheet PopStylesheet()
	{
		Stylesheet result = (Stylesheet)stylesheets.Pop();
		stylesheet = (Stylesheet)stylesheets.Peek();
		return result;
	}

	internal void AddAttributeSet(AttributeSetAction attributeSet)
	{
		stylesheet.AddAttributeSet(attributeSet);
	}

	internal void AddTemplate(TemplateAction template)
	{
		stylesheet.AddTemplate(template);
	}

	internal void BeginTemplate(TemplateAction template)
	{
		currentTemplate = template;
		currentMode = template.Mode;
		CanHaveApplyImports = template.MatchKey != -1;
	}

	internal void EndTemplate()
	{
		currentTemplate = rootAction;
	}

	internal int AddQuery(string xpathQuery)
	{
		return AddQuery(xpathQuery, allowVar: true, allowKey: true, isPattern: false);
	}

	internal int AddQuery(string xpathQuery, bool allowVar, bool allowKey, bool isPattern)
	{
		CompiledXpathExpr compiledQuery;
		try
		{
			compiledQuery = new CompiledXpathExpr(isPattern ? queryBuilder.BuildPatternQuery(xpathQuery, allowVar, allowKey) : queryBuilder.Build(xpathQuery, allowVar, allowKey), xpathQuery, needContext: false);
		}
		catch (XPathException inner)
		{
			if (!ForwardCompatibility)
			{
				throw XsltException.Create("'{0}' is an invalid XPath expression.", new string[1] { xpathQuery }, inner);
			}
			compiledQuery = new ErrorXPathExpression(xpathQuery, Input.BaseURI, Input.LineNumber, Input.LinePosition);
		}
		queryStore.Add(new TheQuery(compiledQuery, scopeManager));
		return queryStore.Count - 1;
	}

	internal int AddStringQuery(string xpathQuery)
	{
		string xpathQuery2 = (XmlCharType.Instance.IsOnlyWhitespace(xpathQuery) ? xpathQuery : ("string(" + xpathQuery + ")"));
		return AddQuery(xpathQuery2);
	}

	internal int AddBooleanQuery(string xpathQuery)
	{
		string xpathQuery2 = (XmlCharType.Instance.IsOnlyWhitespace(xpathQuery) ? xpathQuery : ("boolean(" + xpathQuery + ")"));
		return AddQuery(xpathQuery2);
	}

	private static string GenerateUniqueClassName()
	{
		return "ScriptClass_" + Interlocked.Increment(ref scriptClassCounter);
	}

	private static void ValidateExtensionNamespace(string nsUri)
	{
		if (nsUri.Length == 0 || nsUri == "http://www.w3.org/1999/XSL/Transform")
		{
			throw XsltException.Create("Extension namespace cannot be 'null' or an XSLT namespace URI.");
		}
		XmlConvert.ToUri(nsUri);
	}

	public string GetNsAlias(ref string prefix)
	{
		if (prefix == "#default")
		{
			prefix = string.Empty;
			return DefaultNamespace;
		}
		if (!PrefixQName.ValidatePrefix(prefix))
		{
			throw XsltException.Create("'{1}' is an invalid value for the '{0}' attribute.", input.LocalName, prefix);
		}
		return ResolveXPathNamespace(prefix);
	}

	private static void getTextLex(string avt, ref int start, StringBuilder lex)
	{
		int length = avt.Length;
		int i;
		char c;
		for (i = start; i < length; lex.Append(c), i++)
		{
			c = avt[i];
			switch (c)
			{
			case '{':
				if (i + 1 < length && avt[i + 1] == '{')
				{
					i++;
					continue;
				}
				break;
			case '}':
				if (i + 1 < length && avt[i + 1] == '}')
				{
					i++;
					continue;
				}
				throw XsltException.Create("Right curly brace in the attribute value template '{0}' must be doubled.", avt);
			default:
				continue;
			}
			break;
		}
		start = i;
	}

	private static void getXPathLex(string avt, ref int start, StringBuilder lex)
	{
		int length = avt.Length;
		int num = 0;
		for (int i = start + 1; i < length; i++)
		{
			char c = avt[i];
			switch (num)
			{
			case 0:
				switch (c)
				{
				case '{':
					throw XsltException.Create("AVT cannot be nested in AVT '{0}'.", avt);
				case '}':
					i++;
					if (i == start + 2)
					{
						throw XsltException.Create("XPath Expression in AVT cannot be empty: '{0}'.", avt);
					}
					lex.Append(avt, start + 1, i - start - 2);
					start = i;
					return;
				case '\'':
					num = 1;
					break;
				case '"':
					num = 2;
					break;
				}
				break;
			case 1:
				if (c == '\'')
				{
					num = 0;
				}
				break;
			case 2:
				if (c == '"')
				{
					num = 0;
				}
				break;
			}
		}
		throw XsltException.Create((num == 0) ? "The braces are not closed in AVT expression '{0}'." : "The literal in AVT expression is not correctly closed '{0}'.", avt);
	}

	private static bool GetNextAvtLex(string avt, ref int start, StringBuilder lex, out bool isAvt)
	{
		isAvt = false;
		if (start == avt.Length)
		{
			return false;
		}
		lex.Length = 0;
		getTextLex(avt, ref start, lex);
		if (lex.Length == 0)
		{
			isAvt = true;
			getXPathLex(avt, ref start, lex);
		}
		return true;
	}

	internal ArrayList CompileAvt(string avtText, out bool constant)
	{
		ArrayList arrayList = new ArrayList();
		constant = true;
		int start = 0;
		bool isAvt;
		while (GetNextAvtLex(avtText, ref start, AvtStringBuilder, out isAvt))
		{
			string text = AvtStringBuilder.ToString();
			if (isAvt)
			{
				arrayList.Add(new AvtEvent(AddStringQuery(text)));
				constant = false;
			}
			else
			{
				arrayList.Add(new TextEvent(text));
			}
		}
		return arrayList;
	}

	internal ArrayList CompileAvt(string avtText)
	{
		bool constant;
		return CompileAvt(avtText, out constant);
	}

	public virtual ApplyImportsAction CreateApplyImportsAction()
	{
		ApplyImportsAction applyImportsAction = new ApplyImportsAction();
		applyImportsAction.Compile(this);
		return applyImportsAction;
	}

	public virtual ApplyTemplatesAction CreateApplyTemplatesAction()
	{
		ApplyTemplatesAction applyTemplatesAction = new ApplyTemplatesAction();
		applyTemplatesAction.Compile(this);
		return applyTemplatesAction;
	}

	public virtual AttributeAction CreateAttributeAction()
	{
		AttributeAction attributeAction = new AttributeAction();
		attributeAction.Compile(this);
		return attributeAction;
	}

	public virtual AttributeSetAction CreateAttributeSetAction()
	{
		AttributeSetAction attributeSetAction = new AttributeSetAction();
		attributeSetAction.Compile(this);
		return attributeSetAction;
	}

	public virtual CallTemplateAction CreateCallTemplateAction()
	{
		CallTemplateAction callTemplateAction = new CallTemplateAction();
		callTemplateAction.Compile(this);
		return callTemplateAction;
	}

	public virtual ChooseAction CreateChooseAction()
	{
		ChooseAction chooseAction = new ChooseAction();
		chooseAction.Compile(this);
		return chooseAction;
	}

	public virtual CommentAction CreateCommentAction()
	{
		CommentAction commentAction = new CommentAction();
		commentAction.Compile(this);
		return commentAction;
	}

	public virtual CopyAction CreateCopyAction()
	{
		CopyAction copyAction = new CopyAction();
		copyAction.Compile(this);
		return copyAction;
	}

	public virtual CopyOfAction CreateCopyOfAction()
	{
		CopyOfAction copyOfAction = new CopyOfAction();
		copyOfAction.Compile(this);
		return copyOfAction;
	}

	public virtual ElementAction CreateElementAction()
	{
		ElementAction elementAction = new ElementAction();
		elementAction.Compile(this);
		return elementAction;
	}

	public virtual ForEachAction CreateForEachAction()
	{
		ForEachAction forEachAction = new ForEachAction();
		forEachAction.Compile(this);
		return forEachAction;
	}

	public virtual IfAction CreateIfAction(IfAction.ConditionType type)
	{
		IfAction ifAction = new IfAction(type);
		ifAction.Compile(this);
		return ifAction;
	}

	public virtual MessageAction CreateMessageAction()
	{
		MessageAction messageAction = new MessageAction();
		messageAction.Compile(this);
		return messageAction;
	}

	public virtual NewInstructionAction CreateNewInstructionAction()
	{
		NewInstructionAction newInstructionAction = new NewInstructionAction();
		newInstructionAction.Compile(this);
		return newInstructionAction;
	}

	public virtual NumberAction CreateNumberAction()
	{
		NumberAction numberAction = new NumberAction();
		numberAction.Compile(this);
		return numberAction;
	}

	public virtual ProcessingInstructionAction CreateProcessingInstructionAction()
	{
		ProcessingInstructionAction processingInstructionAction = new ProcessingInstructionAction();
		processingInstructionAction.Compile(this);
		return processingInstructionAction;
	}

	public virtual void CreateRootAction()
	{
		RootAction = new RootAction();
		RootAction.Compile(this);
	}

	public virtual SortAction CreateSortAction()
	{
		SortAction sortAction = new SortAction();
		sortAction.Compile(this);
		return sortAction;
	}

	public virtual TemplateAction CreateTemplateAction()
	{
		TemplateAction templateAction = new TemplateAction();
		templateAction.Compile(this);
		return templateAction;
	}

	public virtual TemplateAction CreateSingleTemplateAction()
	{
		TemplateAction templateAction = new TemplateAction();
		templateAction.CompileSingle(this);
		return templateAction;
	}

	public virtual TextAction CreateTextAction()
	{
		TextAction textAction = new TextAction();
		textAction.Compile(this);
		return textAction;
	}

	public virtual UseAttributeSetsAction CreateUseAttributeSetsAction()
	{
		UseAttributeSetsAction useAttributeSetsAction = new UseAttributeSetsAction();
		useAttributeSetsAction.Compile(this);
		return useAttributeSetsAction;
	}

	public virtual ValueOfAction CreateValueOfAction()
	{
		ValueOfAction valueOfAction = new ValueOfAction();
		valueOfAction.Compile(this);
		return valueOfAction;
	}

	public virtual VariableAction CreateVariableAction(VariableType type)
	{
		VariableAction variableAction = new VariableAction(type);
		variableAction.Compile(this);
		if (variableAction.VarKey != -1)
		{
			return variableAction;
		}
		return null;
	}

	public virtual WithParamAction CreateWithParamAction()
	{
		WithParamAction withParamAction = new WithParamAction();
		withParamAction.Compile(this);
		return withParamAction;
	}

	public virtual BeginEvent CreateBeginEvent()
	{
		return new BeginEvent(this);
	}

	public virtual TextEvent CreateTextEvent()
	{
		return new TextEvent(this);
	}

	public XsltException UnexpectedKeyword()
	{
		XPathNavigator xPathNavigator = Input.Navigator.Clone();
		string name = xPathNavigator.Name;
		xPathNavigator.MoveToParent();
		string name2 = xPathNavigator.Name;
		return XsltException.Create("'{0}' cannot be a child of the '{1}' element.", name, name2);
	}
}
