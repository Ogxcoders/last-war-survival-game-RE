using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using System.Xml.XPath;
using System.Xml.Xsl.XsltOld.Debugger;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal sealed class Processor : IXsltProcessor
{
	internal enum ExecResult
	{
		Continue,
		Interrupt,
		Done
	}

	internal enum OutputResult
	{
		Continue,
		Interrupt,
		Overflow,
		Error,
		Ignore
	}

	internal class DebuggerFrame
	{
		internal ActionFrame actionFrame;

		internal XmlQualifiedName currentMode;
	}

	private const int StackIncrement = 10;

	private ExecResult execResult;

	private Stylesheet stylesheet;

	private RootAction rootAction;

	private Key[] keyList;

	private List<TheQuery> queryStore;

	public PermissionSet permissions;

	private XPathNavigator document;

	private HWStack actionStack;

	private HWStack debuggerStack;

	private StringBuilder sharedStringBuilder;

	private int ignoreLevel;

	private StateMachine xsm;

	private RecordBuilder builder;

	private XsltOutput output;

	private XmlNameTable nameTable = new NameTable();

	private XmlResolver resolver;

	private XsltArgumentList args;

	private Hashtable scriptExtensions;

	private ArrayList numberList;

	private TemplateLookupAction templateLookup = new TemplateLookupAction();

	private IXsltDebugger debugger;

	private Query[] queryList;

	private ArrayList sortArray;

	private Hashtable documentCache;

	private XsltCompileContext valueOfContext;

	private XsltCompileContext matchesContext;

	internal XPathNavigator Current => ((ActionFrame)actionStack.Peek())?.Node;

	internal ExecResult ExecutionResult
	{
		get
		{
			return execResult;
		}
		set
		{
			execResult = value;
		}
	}

	internal Stylesheet Stylesheet => stylesheet;

	internal XmlResolver Resolver => resolver;

	internal ArrayList SortArray => sortArray;

	internal Key[] KeyList => keyList;

	internal RootAction RootAction => rootAction;

	internal XPathNavigator Document => document;

	internal ArrayList NumberList
	{
		get
		{
			if (numberList == null)
			{
				numberList = new ArrayList();
			}
			return numberList;
		}
	}

	internal IXsltDebugger Debugger => debugger;

	internal HWStack ActionStack => actionStack;

	internal RecordBuilder Builder => builder;

	internal XsltOutput Output => output;

	internal XmlNameTable NameTable => nameTable;

	internal bool CanContinue => execResult == ExecResult.Continue;

	internal bool ExecutionDone => execResult == ExecResult.Done;

	int IXsltProcessor.StackDepth => debuggerStack.Length;

	internal XPathNavigator GetNavigator(Uri ruri)
	{
		XPathNavigator xPathNavigator = null;
		if (documentCache != null)
		{
			if (documentCache[ruri] is XPathNavigator xPathNavigator2)
			{
				return xPathNavigator2.Clone();
			}
		}
		else
		{
			documentCache = new Hashtable();
		}
		object entity = resolver.GetEntity(ruri, null, null);
		if (entity is Stream)
		{
			xPathNavigator = ((IXPathNavigable)Compiler.LoadDocument(new XmlTextReaderImpl(ruri.ToString(), (Stream)entity)
			{
				XmlResolver = resolver
			})).CreateNavigator();
		}
		else
		{
			if (!(entity is XPathNavigator))
			{
				throw XsltException.Create("Cannot resolve the referenced document '{0}'.", ruri.ToString());
			}
			xPathNavigator = (XPathNavigator)entity;
		}
		documentCache[ruri] = xPathNavigator.Clone();
		return xPathNavigator;
	}

	internal void AddSort(Sort sortinfo)
	{
		sortArray.Add(sortinfo);
	}

	internal void InitSortArray()
	{
		if (sortArray == null)
		{
			sortArray = new ArrayList();
		}
		else
		{
			sortArray.Clear();
		}
	}

	internal object GetGlobalParameter(XmlQualifiedName qname)
	{
		object obj = args.GetParam(qname.Name, qname.Namespace);
		if (obj == null)
		{
			return null;
		}
		if (!(obj is XPathNodeIterator) && !(obj is XPathNavigator) && !(obj is bool) && !(obj is double) && !(obj is string))
		{
			obj = ((!(obj is short) && !(obj is ushort) && !(obj is int) && !(obj is uint) && !(obj is long) && !(obj is ulong) && !(obj is float) && !(obj is decimal)) ? obj.ToString() : ((object)XmlConvert.ToXPathDouble(obj)));
		}
		return obj;
	}

	internal object GetExtensionObject(string nsUri)
	{
		return args.GetExtensionObject(nsUri);
	}

	internal object GetScriptObject(string nsUri)
	{
		return scriptExtensions[nsUri];
	}

	internal StringBuilder GetSharedStringBuilder()
	{
		if (sharedStringBuilder == null)
		{
			sharedStringBuilder = new StringBuilder();
		}
		else
		{
			sharedStringBuilder.Length = 0;
		}
		return sharedStringBuilder;
	}

	internal void ReleaseSharedStringBuilder()
	{
	}

	public Processor(XPathNavigator doc, XsltArgumentList args, XmlResolver resolver, Stylesheet stylesheet, List<TheQuery> queryStore, RootAction rootAction, IXsltDebugger debugger)
	{
		this.stylesheet = stylesheet;
		this.queryStore = queryStore;
		this.rootAction = rootAction;
		queryList = new Query[queryStore.Count];
		for (int i = 0; i < queryStore.Count; i++)
		{
			queryList[i] = Query.Clone(queryStore[i].CompiledQuery.QueryTree);
		}
		xsm = new StateMachine();
		document = doc;
		builder = null;
		actionStack = new HWStack(10);
		output = this.rootAction.Output;
		permissions = this.rootAction.permissions;
		this.resolver = resolver ?? XmlNullResolver.Singleton;
		this.args = args ?? new XsltArgumentList();
		this.debugger = debugger;
		if (this.debugger != null)
		{
			debuggerStack = new HWStack(10, 1000);
			templateLookup = new TemplateLookupActionDbg();
		}
		if (this.rootAction.KeyList != null)
		{
			keyList = new Key[this.rootAction.KeyList.Count];
			for (int j = 0; j < keyList.Length; j++)
			{
				keyList[j] = this.rootAction.KeyList[j].Clone();
			}
		}
		scriptExtensions = new Hashtable(this.stylesheet.ScriptObjectTypes.Count);
		foreach (DictionaryEntry scriptObjectType in this.stylesheet.ScriptObjectTypes)
		{
			string text = (string)scriptObjectType.Key;
			if (GetExtensionObject(text) != null)
			{
				throw XsltException.Create("Namespace '{0}' has a duplicate implementation.", text);
			}
			scriptExtensions.Add(text, Activator.CreateInstance((Type)scriptObjectType.Value, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null));
		}
		PushActionFrame(this.rootAction, null);
	}

	public ReaderOutput StartReader()
	{
		ReaderOutput result = new ReaderOutput(this);
		builder = new RecordBuilder(result, nameTable);
		return result;
	}

	public void Execute(Stream stream)
	{
		RecordOutput recordOutput = null;
		switch (output.Method)
		{
		case XsltOutput.OutputMethod.Text:
			recordOutput = new TextOnlyOutput(this, stream);
			break;
		case XsltOutput.OutputMethod.Xml:
		case XsltOutput.OutputMethod.Html:
		case XsltOutput.OutputMethod.Other:
		case XsltOutput.OutputMethod.Unknown:
			recordOutput = new TextOutput(this, stream);
			break;
		}
		builder = new RecordBuilder(recordOutput, nameTable);
		Execute();
	}

	public void Execute(TextWriter writer)
	{
		RecordOutput recordOutput = null;
		switch (output.Method)
		{
		case XsltOutput.OutputMethod.Text:
			recordOutput = new TextOnlyOutput(this, writer);
			break;
		case XsltOutput.OutputMethod.Xml:
		case XsltOutput.OutputMethod.Html:
		case XsltOutput.OutputMethod.Other:
		case XsltOutput.OutputMethod.Unknown:
			recordOutput = new TextOutput(this, writer);
			break;
		}
		builder = new RecordBuilder(recordOutput, nameTable);
		Execute();
	}

	public void Execute(XmlWriter writer)
	{
		builder = new RecordBuilder(new WriterOutput(this, writer), nameTable);
		Execute();
	}

	internal void Execute()
	{
		while (execResult == ExecResult.Continue)
		{
			ActionFrame actionFrame = (ActionFrame)actionStack.Peek();
			if (actionFrame == null)
			{
				builder.TheEnd();
				ExecutionResult = ExecResult.Done;
				break;
			}
			if (actionFrame.Execute(this))
			{
				actionStack.Pop();
			}
		}
		if (execResult == ExecResult.Interrupt)
		{
			execResult = ExecResult.Continue;
		}
	}

	internal ActionFrame PushNewFrame()
	{
		ActionFrame actionFrame = (ActionFrame)actionStack.Peek();
		ActionFrame actionFrame2 = (ActionFrame)actionStack.Push();
		if (actionFrame2 == null)
		{
			actionFrame2 = new ActionFrame();
			actionStack.AddToTop(actionFrame2);
		}
		if (actionFrame != null)
		{
			actionFrame2.Inherit(actionFrame);
		}
		return actionFrame2;
	}

	internal void PushActionFrame(Action action, XPathNodeIterator nodeSet)
	{
		PushNewFrame().Init(action, nodeSet);
	}

	internal void PushActionFrame(ActionFrame container)
	{
		PushActionFrame(container, container.NodeSet);
	}

	internal void PushActionFrame(ActionFrame container, XPathNodeIterator nodeSet)
	{
		PushNewFrame().Init(container, nodeSet);
	}

	internal void PushTemplateLookup(XPathNodeIterator nodeSet, XmlQualifiedName mode, Stylesheet importsOf)
	{
		templateLookup.Initialize(mode, importsOf);
		PushActionFrame(templateLookup, nodeSet);
	}

	internal string GetQueryExpression(int key)
	{
		return queryStore[key].CompiledQuery.Expression;
	}

	internal Query GetCompiledQuery(int key)
	{
		TheQuery theQuery = queryStore[key];
		theQuery.CompiledQuery.CheckErrors();
		Query query = Query.Clone(queryList[key]);
		query.SetXsltContext(new XsltCompileContext(theQuery._ScopeManager, this));
		return query;
	}

	internal Query GetValueQuery(int key)
	{
		return GetValueQuery(key, null);
	}

	internal Query GetValueQuery(int key, XsltCompileContext context)
	{
		TheQuery theQuery = queryStore[key];
		theQuery.CompiledQuery.CheckErrors();
		Query obj = queryList[key];
		if (context == null)
		{
			context = new XsltCompileContext(theQuery._ScopeManager, this);
		}
		else
		{
			context.Reinitialize(theQuery._ScopeManager, this);
		}
		obj.SetXsltContext(context);
		return obj;
	}

	private XsltCompileContext GetValueOfContext()
	{
		if (valueOfContext == null)
		{
			valueOfContext = new XsltCompileContext();
		}
		return valueOfContext;
	}

	[Conditional("DEBUG")]
	private void RecycleValueOfContext()
	{
		if (valueOfContext != null)
		{
			valueOfContext.Recycle();
		}
	}

	private XsltCompileContext GetMatchesContext()
	{
		if (matchesContext == null)
		{
			matchesContext = new XsltCompileContext();
		}
		return matchesContext;
	}

	[Conditional("DEBUG")]
	private void RecycleMatchesContext()
	{
		if (matchesContext != null)
		{
			matchesContext.Recycle();
		}
	}

	internal string ValueOf(ActionFrame context, int key)
	{
		Query valueQuery = GetValueQuery(key, GetValueOfContext());
		object obj = valueQuery.Evaluate(context.NodeSet);
		if (obj is XPathNodeIterator)
		{
			XPathNavigator xPathNavigator = valueQuery.Advance();
			return (xPathNavigator != null) ? ValueOf(xPathNavigator) : string.Empty;
		}
		return XmlConvert.ToXPathString(obj);
	}

	internal string ValueOf(XPathNavigator n)
	{
		if (stylesheet.Whitespace && n.NodeType == XPathNodeType.Element)
		{
			StringBuilder stringBuilder = GetSharedStringBuilder();
			ElementValueWithoutWS(n, stringBuilder);
			ReleaseSharedStringBuilder();
			return stringBuilder.ToString();
		}
		return n.Value;
	}

	private void ElementValueWithoutWS(XPathNavigator nav, StringBuilder builder)
	{
		bool flag = Stylesheet.PreserveWhiteSpace(this, nav);
		if (!nav.MoveToFirstChild())
		{
			return;
		}
		do
		{
			switch (nav.NodeType)
			{
			case XPathNodeType.Text:
			case XPathNodeType.SignificantWhitespace:
				builder.Append(nav.Value);
				break;
			case XPathNodeType.Whitespace:
				if (flag)
				{
					builder.Append(nav.Value);
				}
				break;
			case XPathNodeType.Element:
				ElementValueWithoutWS(nav, builder);
				break;
			}
		}
		while (nav.MoveToNext());
		nav.MoveToParent();
	}

	internal XPathNodeIterator StartQuery(XPathNodeIterator context, int key)
	{
		Query compiledQuery = GetCompiledQuery(key);
		if (compiledQuery.Evaluate(context) is XPathNodeIterator)
		{
			return new XPathSelectionIterator(context.Current, compiledQuery);
		}
		throw XsltException.Create("Expression must evaluate to a node-set.");
	}

	internal object Evaluate(ActionFrame context, int key)
	{
		return GetValueQuery(key).Evaluate(context.NodeSet);
	}

	internal object RunQuery(ActionFrame context, int key)
	{
		object obj = GetCompiledQuery(key).Evaluate(context.NodeSet);
		if (obj is XPathNodeIterator nodeIterator)
		{
			return new XPathArrayIterator(nodeIterator);
		}
		return obj;
	}

	internal string EvaluateString(ActionFrame context, int key)
	{
		object obj = Evaluate(context, key);
		string text = null;
		if (obj != null)
		{
			text = XmlConvert.ToXPathString(obj);
		}
		if (text == null)
		{
			text = string.Empty;
		}
		return text;
	}

	internal bool EvaluateBoolean(ActionFrame context, int key)
	{
		object obj = Evaluate(context, key);
		if (obj != null)
		{
			if (!(obj is XPathNavigator xPathNavigator))
			{
				return Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
			}
			return Convert.ToBoolean(xPathNavigator.Value, CultureInfo.InvariantCulture);
		}
		return false;
	}

	internal bool Matches(XPathNavigator context, int key)
	{
		Query valueQuery = GetValueQuery(key, GetMatchesContext());
		try
		{
			return valueQuery.MatchNode(context) != null;
		}
		catch (XPathException)
		{
			throw XsltException.Create("'{0}' is an invalid XSLT pattern.", GetQueryExpression(key));
		}
	}

	internal void ResetOutput()
	{
		builder.Reset();
	}

	internal bool BeginEvent(XPathNodeType nodeType, string prefix, string name, string nspace, bool empty)
	{
		return BeginEvent(nodeType, prefix, name, nspace, empty, null, search: true);
	}

	internal bool BeginEvent(XPathNodeType nodeType, string prefix, string name, string nspace, bool empty, object htmlProps, bool search)
	{
		int num = xsm.BeginOutlook(nodeType);
		if (ignoreLevel > 0 || num == 16)
		{
			ignoreLevel++;
			return true;
		}
		switch (builder.BeginEvent(num, nodeType, prefix, name, nspace, empty, htmlProps, search))
		{
		case OutputResult.Continue:
			xsm.Begin(nodeType);
			return true;
		case OutputResult.Interrupt:
			xsm.Begin(nodeType);
			ExecutionResult = ExecResult.Interrupt;
			return true;
		case OutputResult.Overflow:
			ExecutionResult = ExecResult.Interrupt;
			return false;
		case OutputResult.Error:
			ignoreLevel++;
			return true;
		case OutputResult.Ignore:
			return true;
		default:
			return true;
		}
	}

	internal bool TextEvent(string text)
	{
		return TextEvent(text, disableOutputEscaping: false);
	}

	internal bool TextEvent(string text, bool disableOutputEscaping)
	{
		if (ignoreLevel > 0)
		{
			return true;
		}
		int state = xsm.BeginOutlook(XPathNodeType.Text);
		switch (builder.TextEvent(state, text, disableOutputEscaping))
		{
		case OutputResult.Continue:
			xsm.Begin(XPathNodeType.Text);
			return true;
		case OutputResult.Interrupt:
			xsm.Begin(XPathNodeType.Text);
			ExecutionResult = ExecResult.Interrupt;
			return true;
		case OutputResult.Overflow:
			ExecutionResult = ExecResult.Interrupt;
			return false;
		case OutputResult.Error:
		case OutputResult.Ignore:
			return true;
		default:
			return true;
		}
	}

	internal bool EndEvent(XPathNodeType nodeType)
	{
		if (ignoreLevel > 0)
		{
			ignoreLevel--;
			return true;
		}
		int state = xsm.EndOutlook(nodeType);
		switch (builder.EndEvent(state, nodeType))
		{
		case OutputResult.Continue:
			xsm.End(nodeType);
			return true;
		case OutputResult.Interrupt:
			xsm.End(nodeType);
			ExecutionResult = ExecResult.Interrupt;
			return true;
		case OutputResult.Overflow:
			ExecutionResult = ExecResult.Interrupt;
			return false;
		default:
			return true;
		}
	}

	internal bool CopyBeginEvent(XPathNavigator node, bool emptyflag)
	{
		switch (node.NodeType)
		{
		case XPathNodeType.Element:
		case XPathNodeType.Attribute:
		case XPathNodeType.ProcessingInstruction:
		case XPathNodeType.Comment:
			return BeginEvent(node.NodeType, node.Prefix, node.LocalName, node.NamespaceURI, emptyflag);
		case XPathNodeType.Namespace:
			return BeginEvent(XPathNodeType.Namespace, null, node.LocalName, node.Value, empty: false);
		default:
			return true;
		}
	}

	internal bool CopyTextEvent(XPathNavigator node)
	{
		switch (node.NodeType)
		{
		case XPathNodeType.Attribute:
		case XPathNodeType.Text:
		case XPathNodeType.SignificantWhitespace:
		case XPathNodeType.Whitespace:
		case XPathNodeType.ProcessingInstruction:
		case XPathNodeType.Comment:
		{
			string value = node.Value;
			return TextEvent(value);
		}
		default:
			return true;
		}
	}

	internal bool CopyEndEvent(XPathNavigator node)
	{
		switch (node.NodeType)
		{
		case XPathNodeType.Element:
		case XPathNodeType.Attribute:
		case XPathNodeType.Namespace:
		case XPathNodeType.ProcessingInstruction:
		case XPathNodeType.Comment:
			return EndEvent(node.NodeType);
		default:
			return true;
		}
	}

	internal static bool IsRoot(XPathNavigator navigator)
	{
		if (navigator.NodeType == XPathNodeType.Root)
		{
			return true;
		}
		if (navigator.NodeType == XPathNodeType.Element)
		{
			XPathNavigator xPathNavigator = navigator.Clone();
			xPathNavigator.MoveToRoot();
			return xPathNavigator.IsSamePosition(navigator);
		}
		return false;
	}

	internal void PushOutput(RecordOutput output)
	{
		builder.OutputState = xsm.State;
		RecordBuilder next = builder;
		builder = new RecordBuilder(output, nameTable);
		builder.Next = next;
		xsm.Reset();
	}

	internal RecordOutput PopOutput()
	{
		RecordBuilder recordBuilder = builder;
		builder = recordBuilder.Next;
		xsm.State = builder.OutputState;
		recordBuilder.TheEnd();
		return recordBuilder.Output;
	}

	internal bool SetDefaultOutput(XsltOutput.OutputMethod method)
	{
		if (Output.Method != method)
		{
			output = output.CreateDerivedOutput(method);
			return true;
		}
		return false;
	}

	internal object GetVariableValue(VariableAction variable)
	{
		int varKey = variable.VarKey;
		if (variable.IsGlobal)
		{
			ActionFrame actionFrame = (ActionFrame)actionStack[0];
			object variable2 = actionFrame.GetVariable(varKey);
			if (variable2 == VariableAction.BeingComputedMark)
			{
				throw XsltException.Create("Circular reference in the definition of variable '{0}'.", variable.NameStr);
			}
			if (variable2 != null)
			{
				return variable2;
			}
			int length = actionStack.Length;
			ActionFrame actionFrame2 = PushNewFrame();
			actionFrame2.Inherit(actionFrame);
			actionFrame2.Init(variable, actionFrame.NodeSet);
			do
			{
				if (((ActionFrame)actionStack.Peek()).Execute(this))
				{
					actionStack.Pop();
				}
			}
			while (length < actionStack.Length);
			return actionFrame.GetVariable(varKey);
		}
		return ((ActionFrame)actionStack.Peek()).GetVariable(varKey);
	}

	internal void SetParameter(XmlQualifiedName name, object value)
	{
		((ActionFrame)actionStack[actionStack.Length - 2]).SetParameter(name, value);
	}

	internal void ResetParams()
	{
		((ActionFrame)actionStack[actionStack.Length - 1]).ResetParams();
	}

	internal object GetParameter(XmlQualifiedName name)
	{
		return ((ActionFrame)actionStack[actionStack.Length - 3]).GetParameter(name);
	}

	internal void PushDebuggerStack()
	{
		DebuggerFrame debuggerFrame = (DebuggerFrame)debuggerStack.Push();
		if (debuggerFrame == null)
		{
			debuggerFrame = new DebuggerFrame();
			debuggerStack.AddToTop(debuggerFrame);
		}
		debuggerFrame.actionFrame = (ActionFrame)actionStack.Peek();
	}

	internal void PopDebuggerStack()
	{
		debuggerStack.Pop();
	}

	internal void OnInstructionExecute()
	{
		((DebuggerFrame)debuggerStack.Peek()).actionFrame = (ActionFrame)actionStack.Peek();
		Debugger.OnInstructionExecute(this);
	}

	internal XmlQualifiedName GetPrevioseMode()
	{
		return ((DebuggerFrame)debuggerStack[debuggerStack.Length - 2]).currentMode;
	}

	internal void SetCurrentMode(XmlQualifiedName mode)
	{
		((DebuggerFrame)debuggerStack[debuggerStack.Length - 1]).currentMode = mode;
	}

	IStackFrame IXsltProcessor.GetStackFrame(int depth)
	{
		return ((DebuggerFrame)debuggerStack[depth]).actionFrame;
	}
}
