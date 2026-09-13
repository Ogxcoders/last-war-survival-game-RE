using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class XsltCompileContext : XsltContext
{
	private abstract class XsltFunctionImpl : IXsltContextFunction
	{
		private int minargs;

		private int maxargs;

		private XPathResultType returnType;

		private XPathResultType[] argTypes;

		public int Minargs => minargs;

		public int Maxargs => maxargs;

		public XPathResultType ReturnType => returnType;

		public XPathResultType[] ArgTypes => argTypes;

		public XsltFunctionImpl()
		{
		}

		public XsltFunctionImpl(int minArgs, int maxArgs, XPathResultType returnType, XPathResultType[] argTypes)
		{
			Init(minArgs, maxArgs, returnType, argTypes);
		}

		protected void Init(int minArgs, int maxArgs, XPathResultType returnType, XPathResultType[] argTypes)
		{
			minargs = minArgs;
			maxargs = maxArgs;
			this.returnType = returnType;
			this.argTypes = argTypes;
		}

		public abstract object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext);

		public static XPathNodeIterator ToIterator(object argument)
		{
			return (argument as XPathNodeIterator) ?? throw XsltException.Create("Cannot convert the operand to a node-set.");
		}

		public static XPathNavigator ToNavigator(object argument)
		{
			return (argument as XPathNavigator) ?? throw XsltException.Create("Cannot convert the operand to 'Result tree fragment'.");
		}

		private static string IteratorToString(XPathNodeIterator it)
		{
			if (it.MoveNext())
			{
				return it.Current.Value;
			}
			return string.Empty;
		}

		public static string ToString(object argument)
		{
			if (argument is XPathNodeIterator it)
			{
				return IteratorToString(it);
			}
			return XmlConvert.ToXPathString(argument);
		}

		public static bool ToBoolean(object argument)
		{
			if (argument is XPathNodeIterator it)
			{
				return Convert.ToBoolean(IteratorToString(it), CultureInfo.InvariantCulture);
			}
			if (argument is XPathNavigator xPathNavigator)
			{
				return Convert.ToBoolean(xPathNavigator.ToString(), CultureInfo.InvariantCulture);
			}
			return Convert.ToBoolean(argument, CultureInfo.InvariantCulture);
		}

		public static double ToNumber(object argument)
		{
			if (argument is XPathNodeIterator it)
			{
				return XmlConvert.ToXPathDouble(IteratorToString(it));
			}
			if (argument is XPathNavigator xPathNavigator)
			{
				return XmlConvert.ToXPathDouble(xPathNavigator.ToString());
			}
			return XmlConvert.ToXPathDouble(argument);
		}

		private static object ToNumeric(object argument, TypeCode typeCode)
		{
			return Convert.ChangeType(ToNumber(argument), typeCode, CultureInfo.InvariantCulture);
		}

		public static object ConvertToXPathType(object val, XPathResultType xt, TypeCode typeCode)
		{
			switch (xt)
			{
			case XPathResultType.String:
				if (typeCode == TypeCode.String)
				{
					return ToString(val);
				}
				return ToNavigator(val);
			case XPathResultType.Number:
				return ToNumeric(val, typeCode);
			case XPathResultType.Boolean:
				return ToBoolean(val);
			case XPathResultType.NodeSet:
				return ToIterator(val);
			case XPathResultType.Any:
			case XPathResultType.Error:
				return val;
			default:
				return val;
			}
		}
	}

	private class FuncCurrent : XsltFunctionImpl
	{
		public FuncCurrent()
			: base(0, 0, XPathResultType.NodeSet, new XPathResultType[0])
		{
		}

		public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			return ((XsltCompileContext)xsltContext).Current();
		}
	}

	private class FuncUnEntityUri : XsltFunctionImpl
	{
		public FuncUnEntityUri()
			: base(1, 1, XPathResultType.String, new XPathResultType[1] { XPathResultType.String })
		{
		}

		public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			throw XsltException.Create("'{0}()' is an unsupported XSLT function.", "unparsed-entity-uri");
		}
	}

	private class FuncGenerateId : XsltFunctionImpl
	{
		public FuncGenerateId()
			: base(0, 1, XPathResultType.String, new XPathResultType[1] { XPathResultType.NodeSet })
		{
		}

		public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			if (args.Length != 0)
			{
				XPathNodeIterator xPathNodeIterator = XsltFunctionImpl.ToIterator(args[0]);
				if (xPathNodeIterator.MoveNext())
				{
					return xPathNodeIterator.Current.UniqueId;
				}
				return string.Empty;
			}
			return docContext.UniqueId;
		}
	}

	private class FuncSystemProp : XsltFunctionImpl
	{
		public FuncSystemProp()
			: base(1, 1, XPathResultType.String, new XPathResultType[1] { XPathResultType.String })
		{
		}

		public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			return ((XsltCompileContext)xsltContext).SystemProperty(XsltFunctionImpl.ToString(args[0]));
		}
	}

	private class FuncElementAvailable : XsltFunctionImpl
	{
		public FuncElementAvailable()
			: base(1, 1, XPathResultType.Boolean, new XPathResultType[1] { XPathResultType.String })
		{
		}

		public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			return ((XsltCompileContext)xsltContext).ElementAvailable(XsltFunctionImpl.ToString(args[0]));
		}
	}

	private class FuncFunctionAvailable : XsltFunctionImpl
	{
		public FuncFunctionAvailable()
			: base(1, 1, XPathResultType.Boolean, new XPathResultType[1] { XPathResultType.String })
		{
		}

		public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			return ((XsltCompileContext)xsltContext).FunctionAvailable(XsltFunctionImpl.ToString(args[0]));
		}
	}

	private class FuncDocument : XsltFunctionImpl
	{
		public FuncDocument()
			: base(1, 2, XPathResultType.NodeSet, new XPathResultType[2]
			{
				XPathResultType.Any,
				XPathResultType.NodeSet
			})
		{
		}

		public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			string baseUri = null;
			if (args.Length == 2)
			{
				XPathNodeIterator xPathNodeIterator = XsltFunctionImpl.ToIterator(args[1]);
				baseUri = ((!xPathNodeIterator.MoveNext()) ? string.Empty : xPathNodeIterator.Current.BaseURI);
			}
			try
			{
				return ((XsltCompileContext)xsltContext).Document(args[0], baseUri);
			}
			catch (Exception e)
			{
				if (!XmlException.IsCatchableException(e))
				{
					throw;
				}
				return XPathEmptyIterator.Instance;
			}
		}
	}

	private class FuncKey : XsltFunctionImpl
	{
		public FuncKey()
			: base(2, 2, XPathResultType.NodeSet, new XPathResultType[2]
			{
				XPathResultType.String,
				XPathResultType.Any
			})
		{
		}

		public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			XsltCompileContext xsltCompileContext = (XsltCompileContext)xsltContext;
			PrefixQName.ParseQualifiedName(XsltFunctionImpl.ToString(args[0]), out var prefix, out var local);
			string ns = xsltContext.LookupNamespace(prefix);
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(local, ns);
			XPathNavigator xPathNavigator = docContext.Clone();
			xPathNavigator.MoveToRoot();
			ArrayList arrayList = null;
			Key[] keyList = xsltCompileContext.processor.KeyList;
			foreach (Key key in keyList)
			{
				if (!(key.Name == xmlQualifiedName))
				{
					continue;
				}
				Hashtable hashtable = key.GetKeys(xPathNavigator);
				if (hashtable == null)
				{
					hashtable = xsltCompileContext.BuildKeyTable(key, xPathNavigator);
					key.AddKey(xPathNavigator, hashtable);
				}
				if (args[1] is XPathNodeIterator xPathNodeIterator)
				{
					XPathNodeIterator xPathNodeIterator2 = xPathNodeIterator.Clone();
					while (xPathNodeIterator2.MoveNext())
					{
						arrayList = AddToList(arrayList, (ArrayList)hashtable[xPathNodeIterator2.Current.Value]);
					}
				}
				else
				{
					arrayList = AddToList(arrayList, (ArrayList)hashtable[XsltFunctionImpl.ToString(args[1])]);
				}
			}
			if (arrayList == null)
			{
				return XPathEmptyIterator.Instance;
			}
			if (arrayList[0] is XPathNavigator)
			{
				return new XPathArrayIterator(arrayList);
			}
			return new XPathMultyIterator(arrayList);
		}

		private static ArrayList AddToList(ArrayList resultCollection, ArrayList newList)
		{
			if (newList == null)
			{
				return resultCollection;
			}
			if (resultCollection == null)
			{
				return newList;
			}
			if (!(resultCollection[0] is ArrayList))
			{
				ArrayList value = resultCollection;
				resultCollection = new ArrayList();
				resultCollection.Add(value);
			}
			resultCollection.Add(newList);
			return resultCollection;
		}
	}

	private class FuncFormatNumber : XsltFunctionImpl
	{
		public FuncFormatNumber()
			: base(2, 3, XPathResultType.String, new XPathResultType[3]
			{
				XPathResultType.Number,
				XPathResultType.String,
				XPathResultType.String
			})
		{
		}

		public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			DecimalFormat decimalFormat = ((XsltCompileContext)xsltContext).ResolveFormatName((args.Length == 3) ? XsltFunctionImpl.ToString(args[2]) : null);
			return DecimalFormatter.Format(XsltFunctionImpl.ToNumber(args[0]), XsltFunctionImpl.ToString(args[1]), decimalFormat);
		}
	}

	private class FuncNodeSet : XsltFunctionImpl
	{
		public FuncNodeSet()
			: base(1, 1, XPathResultType.NodeSet, new XPathResultType[1] { XPathResultType.String })
		{
		}

		public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			return new XPathSingletonIterator(XsltFunctionImpl.ToNavigator(args[0]));
		}
	}

	private class FuncExtension : XsltFunctionImpl
	{
		private object extension;

		private MethodInfo method;

		private TypeCode[] typeCodes;

		private PermissionSet permissions;

		public FuncExtension(object extension, MethodInfo method, PermissionSet permissions)
		{
			this.extension = extension;
			this.method = method;
			this.permissions = permissions;
			XPathResultType xPathType = GetXPathType(method.ReturnType);
			ParameterInfo[] parameters = method.GetParameters();
			int num = parameters.Length;
			int maxArgs = parameters.Length;
			typeCodes = new TypeCode[parameters.Length];
			XPathResultType[] array = new XPathResultType[parameters.Length];
			bool flag = true;
			int num2 = parameters.Length - 1;
			while (0 <= num2)
			{
				typeCodes[num2] = Type.GetTypeCode(parameters[num2].ParameterType);
				array[num2] = GetXPathType(parameters[num2].ParameterType);
				if (flag)
				{
					if (parameters[num2].IsOptional)
					{
						num--;
					}
					else
					{
						flag = false;
					}
				}
				num2--;
			}
			Init(num, maxArgs, xPathType, array);
		}

		public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			int num = args.Length - 1;
			while (0 <= num)
			{
				args[num] = XsltFunctionImpl.ConvertToXPathType(args[num], base.ArgTypes[num], typeCodes[num]);
				num--;
			}
			if (permissions != null)
			{
				permissions.PermitOnly();
			}
			return method.Invoke(extension, args);
		}
	}

	private InputScopeManager manager;

	private Processor processor;

	private static Hashtable s_FunctionTable = CreateFunctionTable();

	private static IXsltContextFunction s_FuncNodeSet = new FuncNodeSet();

	private const string f_NodeSet = "node-set";

	private const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	public override string DefaultNamespace => string.Empty;

	public override bool Whitespace => processor.Stylesheet.Whitespace;

	internal XsltCompileContext(InputScopeManager manager, Processor processor)
		: base(dummy: false)
	{
		this.manager = manager;
		this.processor = processor;
	}

	internal XsltCompileContext()
		: base(dummy: false)
	{
	}

	internal void Recycle()
	{
		manager = null;
		processor = null;
	}

	internal void Reinitialize(InputScopeManager manager, Processor processor)
	{
		this.manager = manager;
		this.processor = processor;
	}

	public override int CompareDocument(string baseUri, string nextbaseUri)
	{
		return string.Compare(baseUri, nextbaseUri, StringComparison.Ordinal);
	}

	public override string LookupNamespace(string prefix)
	{
		return manager.ResolveXPathNamespace(prefix);
	}

	public override IXsltContextVariable ResolveVariable(string prefix, string name)
	{
		string ns = LookupNamespace(prefix);
		XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(name, ns);
		IXsltContextVariable xsltContextVariable = manager.VariableScope.ResolveVariable(xmlQualifiedName);
		if (xsltContextVariable == null)
		{
			throw XsltException.Create("The variable or parameter '{0}' is either not defined or it is out of scope.", xmlQualifiedName.ToString());
		}
		return xsltContextVariable;
	}

	internal object EvaluateVariable(VariableAction variable)
	{
		object variableValue = processor.GetVariableValue(variable);
		if (variableValue == null && !variable.IsGlobal)
		{
			VariableAction variableAction = manager.VariableScope.ResolveGlobalVariable(variable.Name);
			if (variableAction != null)
			{
				variableValue = processor.GetVariableValue(variableAction);
			}
		}
		if (variableValue == null)
		{
			throw XsltException.Create("The variable or parameter '{0}' is either not defined or it is out of scope.", variable.Name.ToString());
		}
		return variableValue;
	}

	public override bool PreserveWhitespace(XPathNavigator node)
	{
		node = node.Clone();
		node.MoveToParent();
		return processor.Stylesheet.PreserveWhiteSpace(processor, node);
	}

	private MethodInfo FindBestMethod(MethodInfo[] methods, bool ignoreCase, bool publicOnly, string name, XPathResultType[] argTypes)
	{
		int num = methods.Length;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			if (string.Compare(name, methods[i].Name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0 && (!publicOnly || methods[i].GetBaseDefinition().IsPublic))
			{
				methods[num2++] = methods[i];
			}
		}
		num = num2;
		if (num == 0)
		{
			return null;
		}
		if (argTypes == null)
		{
			return methods[0];
		}
		num2 = 0;
		for (int j = 0; j < num; j++)
		{
			if (methods[j].GetParameters().Length == argTypes.Length)
			{
				methods[num2++] = methods[j];
			}
		}
		num = num2;
		if (num <= 1)
		{
			return methods[0];
		}
		num2 = 0;
		for (int k = 0; k < num; k++)
		{
			bool flag = true;
			ParameterInfo[] parameters = methods[k].GetParameters();
			for (int l = 0; l < parameters.Length; l++)
			{
				XPathResultType xPathResultType = argTypes[l];
				if (xPathResultType != XPathResultType.Any)
				{
					XPathResultType xPathType = GetXPathType(parameters[l].ParameterType);
					if (xPathType != xPathResultType && xPathType != XPathResultType.Any)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				methods[num2++] = methods[k];
			}
		}
		num = num2;
		return methods[0];
	}

	private IXsltContextFunction GetExtentionMethod(string ns, string name, XPathResultType[] argTypes, out object extension)
	{
		FuncExtension result = null;
		extension = processor.GetScriptObject(ns);
		if (extension != null)
		{
			MethodInfo methodInfo = FindBestMethod(extension.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), ignoreCase: true, publicOnly: false, name, argTypes);
			if (methodInfo != null)
			{
				result = new FuncExtension(extension, methodInfo, null);
			}
			return result;
		}
		extension = processor.GetExtensionObject(ns);
		if (extension != null)
		{
			MethodInfo methodInfo2 = FindBestMethod(extension.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), ignoreCase: false, publicOnly: true, name, argTypes);
			if (methodInfo2 != null)
			{
				result = new FuncExtension(extension, methodInfo2, processor.permissions);
			}
			return result;
		}
		return null;
	}

	public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] argTypes)
	{
		IXsltContextFunction xsltContextFunction = null;
		if (prefix.Length == 0)
		{
			xsltContextFunction = s_FunctionTable[name] as IXsltContextFunction;
		}
		else
		{
			string text = LookupNamespace(prefix);
			if (text == "urn:schemas-microsoft-com:xslt" && name == "node-set")
			{
				xsltContextFunction = s_FuncNodeSet;
			}
			else
			{
				xsltContextFunction = GetExtentionMethod(text, name, argTypes, out var extension);
				if (extension == null)
				{
					throw XsltException.Create("Cannot find the script or external object that implements prefix '{0}'.", prefix);
				}
			}
		}
		if (xsltContextFunction == null)
		{
			throw XsltException.Create("'{0}()' is an unknown XSLT function.", name);
		}
		if (argTypes.Length < xsltContextFunction.Minargs || xsltContextFunction.Maxargs < argTypes.Length)
		{
			throw XsltException.Create("XSLT function '{0}()' has the wrong number of arguments.", name, argTypes.Length.ToString(CultureInfo.InvariantCulture));
		}
		return xsltContextFunction;
	}

	private Uri ComposeUri(string thisUri, string baseUri)
	{
		XmlResolver resolver = processor.Resolver;
		Uri baseUri2 = null;
		if (baseUri.Length != 0)
		{
			baseUri2 = resolver.ResolveUri(null, baseUri);
		}
		return resolver.ResolveUri(baseUri2, thisUri);
	}

	private XPathNodeIterator Document(object arg0, string baseUri)
	{
		if (processor.permissions != null)
		{
			processor.permissions.PermitOnly();
		}
		if (arg0 is XPathNodeIterator xPathNodeIterator)
		{
			ArrayList arrayList = new ArrayList();
			Hashtable hashtable = new Hashtable();
			while (xPathNodeIterator.MoveNext())
			{
				Uri uri = ComposeUri(xPathNodeIterator.Current.Value, baseUri ?? xPathNodeIterator.Current.BaseURI);
				if (!hashtable.ContainsKey(uri))
				{
					hashtable.Add(uri, null);
					arrayList.Add(processor.GetNavigator(uri));
				}
			}
			return new XPathArrayIterator(arrayList);
		}
		return new XPathSingletonIterator(processor.GetNavigator(ComposeUri(XmlConvert.ToXPathString(arg0), baseUri ?? manager.Navigator.BaseURI)));
	}

	private Hashtable BuildKeyTable(Key key, XPathNavigator root)
	{
		Hashtable hashtable = new Hashtable();
		string queryExpression = processor.GetQueryExpression(key.MatchKey);
		Query compiledQuery = processor.GetCompiledQuery(key.MatchKey);
		Query compiledQuery2 = processor.GetCompiledQuery(key.UseKey);
		XPathNodeIterator xPathNodeIterator = root.SelectDescendants(XPathNodeType.All, matchSelf: false);
		while (xPathNodeIterator.MoveNext())
		{
			XPathNavigator current = xPathNodeIterator.Current;
			EvaluateKey(current, compiledQuery, queryExpression, compiledQuery2, hashtable);
			if (current.MoveToFirstAttribute())
			{
				do
				{
					EvaluateKey(current, compiledQuery, queryExpression, compiledQuery2, hashtable);
				}
				while (current.MoveToNextAttribute());
				current.MoveToParent();
			}
		}
		return hashtable;
	}

	private static void AddKeyValue(Hashtable keyTable, string key, XPathNavigator value, bool checkDuplicates)
	{
		ArrayList arrayList = (ArrayList)keyTable[key];
		if (arrayList == null)
		{
			arrayList = new ArrayList();
			keyTable.Add(key, arrayList);
		}
		else if (checkDuplicates && value.ComparePosition((XPathNavigator)arrayList[arrayList.Count - 1]) == XmlNodeOrder.Same)
		{
			return;
		}
		arrayList.Add(value.Clone());
	}

	private static void EvaluateKey(XPathNavigator node, Query matchExpr, string matchStr, Query useExpr, Hashtable keyTable)
	{
		try
		{
			if (matchExpr.MatchNode(node) == null)
			{
				return;
			}
		}
		catch (XPathException)
		{
			throw XsltException.Create("'{0}' is an invalid XSLT pattern.", matchStr);
		}
		object obj = useExpr.Evaluate(new XPathSingletonIterator(node, moved: true));
		if (obj is XPathNodeIterator xPathNodeIterator)
		{
			bool checkDuplicates = false;
			while (xPathNodeIterator.MoveNext())
			{
				AddKeyValue(keyTable, xPathNodeIterator.Current.Value, node, checkDuplicates);
				checkDuplicates = true;
			}
		}
		else
		{
			string key = XmlConvert.ToXPathString(obj);
			AddKeyValue(keyTable, key, node, checkDuplicates: false);
		}
	}

	private DecimalFormat ResolveFormatName(string formatName)
	{
		string ns = string.Empty;
		string local = string.Empty;
		if (formatName != null)
		{
			PrefixQName.ParseQualifiedName(formatName, out var prefix, out local);
			ns = LookupNamespace(prefix);
		}
		DecimalFormat decimalFormat = processor.RootAction.GetDecimalFormat(new XmlQualifiedName(local, ns));
		if (decimalFormat == null)
		{
			if (formatName != null)
			{
				throw XsltException.Create("Decimal format '{0}' is not defined.", formatName);
			}
			decimalFormat = new DecimalFormat(new NumberFormatInfo(), '#', '0', ';');
		}
		return decimalFormat;
	}

	private bool ElementAvailable(string qname)
	{
		PrefixQName.ParseQualifiedName(qname, out var prefix, out var local);
		if (manager.ResolveXmlNamespace(prefix) == "http://www.w3.org/1999/XSL/Transform")
		{
			switch (local)
			{
			default:
				return local == "variable";
			case "apply-imports":
			case "apply-templates":
			case "attribute":
			case "call-template":
			case "choose":
			case "comment":
			case "copy":
			case "copy-of":
			case "element":
			case "fallback":
			case "for-each":
			case "if":
			case "message":
			case "number":
			case "processing-instruction":
			case "text":
			case "value-of":
				return true;
			}
		}
		return false;
	}

	private bool FunctionAvailable(string qname)
	{
		PrefixQName.ParseQualifiedName(qname, out var prefix, out var local);
		string text = LookupNamespace(prefix);
		if (text == "urn:schemas-microsoft-com:xslt")
		{
			return local == "node-set";
		}
		if (text.Length == 0)
		{
			switch (local)
			{
			default:
				if (s_FunctionTable[local] != null)
				{
					return local != "unparsed-entity-uri";
				}
				return false;
			case "last":
			case "position":
			case "name":
			case "namespace-uri":
			case "local-name":
			case "count":
			case "id":
			case "string":
			case "concat":
			case "starts-with":
			case "contains":
			case "substring-before":
			case "substring-after":
			case "substring":
			case "string-length":
			case "normalize-space":
			case "translate":
			case "boolean":
			case "not":
			case "true":
			case "false":
			case "lang":
			case "number":
			case "sum":
			case "floor":
			case "ceiling":
			case "round":
				return true;
			}
		}
		object extension;
		return GetExtentionMethod(text, local, null, out extension) != null;
	}

	private XPathNodeIterator Current()
	{
		XPathNavigator current = processor.Current;
		if (current != null)
		{
			return new XPathSingletonIterator(current.Clone());
		}
		return XPathEmptyIterator.Instance;
	}

	private string SystemProperty(string qname)
	{
		string result = string.Empty;
		PrefixQName.ParseQualifiedName(qname, out var prefix, out var local);
		string text = LookupNamespace(prefix);
		if (text == "http://www.w3.org/1999/XSL/Transform")
		{
			switch (local)
			{
			case "version":
				result = "1";
				break;
			case "vendor":
				result = "Microsoft";
				break;
			case "vendor-url":
				result = "http://www.microsoft.com";
				break;
			}
			return result;
		}
		if (text == null && prefix != null)
		{
			throw XsltException.Create("Prefix '{0}' is not defined.", prefix);
		}
		return string.Empty;
	}

	public static XPathResultType GetXPathType(Type type)
	{
		switch (Type.GetTypeCode(type))
		{
		case TypeCode.String:
			return XPathResultType.String;
		case TypeCode.Boolean:
			return XPathResultType.Boolean;
		case TypeCode.Object:
			if (typeof(XPathNavigator).IsAssignableFrom(type) || typeof(IXPathNavigable).IsAssignableFrom(type))
			{
				return XPathResultType.String;
			}
			if (typeof(XPathNodeIterator).IsAssignableFrom(type))
			{
				return XPathResultType.NodeSet;
			}
			return XPathResultType.Any;
		case TypeCode.DateTime:
			return XPathResultType.Error;
		default:
			return XPathResultType.Number;
		}
	}

	private static Hashtable CreateFunctionTable()
	{
		return new Hashtable(10)
		{
			["current"] = new FuncCurrent(),
			["unparsed-entity-uri"] = new FuncUnEntityUri(),
			["generate-id"] = new FuncGenerateId(),
			["system-property"] = new FuncSystemProp(),
			["element-available"] = new FuncElementAvailable(),
			["function-available"] = new FuncFunctionAvailable(),
			["document"] = new FuncDocument(),
			["key"] = new FuncKey(),
			["format-number"] = new FuncFormatNumber()
		};
	}
}
