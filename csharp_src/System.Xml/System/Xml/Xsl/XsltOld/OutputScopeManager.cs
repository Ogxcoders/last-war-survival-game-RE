using System.Globalization;

namespace System.Xml.Xsl.XsltOld;

internal class OutputScopeManager
{
	private const int STACK_INCREMENT = 10;

	private HWStack elementScopesStack;

	private string defaultNS;

	private OutKeywords atoms;

	private XmlNameTable nameTable;

	private int prefixIndex;

	internal string DefaultNamespace => defaultNS;

	internal OutputScope CurrentElementScope => (OutputScope)elementScopesStack.Peek();

	internal XmlSpace XmlSpace => CurrentElementScope.Space;

	internal string XmlLang => CurrentElementScope.Lang;

	internal OutputScopeManager(XmlNameTable nameTable, OutKeywords atoms)
	{
		elementScopesStack = new HWStack(10);
		this.nameTable = nameTable;
		this.atoms = atoms;
		defaultNS = this.atoms.Empty;
		OutputScope outputScope = (OutputScope)elementScopesStack.Push();
		if (outputScope == null)
		{
			outputScope = new OutputScope();
			elementScopesStack.AddToTop(outputScope);
		}
		outputScope.Init(string.Empty, string.Empty, string.Empty, XmlSpace.None, string.Empty, mixed: false);
	}

	internal void PushNamespace(string prefix, string nspace)
	{
		CurrentElementScope.AddNamespace(prefix, nspace, defaultNS);
		if (prefix == null || prefix.Length == 0)
		{
			defaultNS = nspace;
		}
	}

	internal void PushScope(string name, string nspace, string prefix)
	{
		OutputScope currentElementScope = CurrentElementScope;
		OutputScope outputScope = (OutputScope)elementScopesStack.Push();
		if (outputScope == null)
		{
			outputScope = new OutputScope();
			elementScopesStack.AddToTop(outputScope);
		}
		outputScope.Init(name, nspace, prefix, currentElementScope.Space, currentElementScope.Lang, currentElementScope.Mixed);
	}

	internal void PopScope()
	{
		for (NamespaceDecl namespaceDecl = ((OutputScope)elementScopesStack.Pop()).Scopes; namespaceDecl != null; namespaceDecl = namespaceDecl.Next)
		{
			defaultNS = namespaceDecl.PrevDefaultNsUri;
		}
	}

	internal string ResolveNamespace(string prefix)
	{
		bool thisScope;
		return ResolveNamespace(prefix, out thisScope);
	}

	internal string ResolveNamespace(string prefix, out bool thisScope)
	{
		thisScope = true;
		if (prefix == null || prefix.Length == 0)
		{
			return defaultNS;
		}
		if (Ref.Equal(prefix, atoms.Xml))
		{
			return atoms.XmlNamespace;
		}
		if (Ref.Equal(prefix, atoms.Xmlns))
		{
			return atoms.XmlnsNamespace;
		}
		for (int num = elementScopesStack.Length - 1; num >= 0; num--)
		{
			string text = ((OutputScope)elementScopesStack[num]).ResolveAtom(prefix);
			if (text != null)
			{
				thisScope = num == elementScopesStack.Length - 1;
				return text;
			}
		}
		return null;
	}

	internal bool FindPrefix(string nspace, out string prefix)
	{
		int num = elementScopesStack.Length - 1;
		while (0 <= num)
		{
			OutputScope obj = (OutputScope)elementScopesStack[num];
			string prefix2 = null;
			if (obj.FindPrefix(nspace, out prefix2))
			{
				string text = ResolveNamespace(prefix2);
				if (text == null || !Ref.Equal(text, nspace))
				{
					break;
				}
				prefix = prefix2;
				return true;
			}
			num--;
		}
		prefix = null;
		return false;
	}

	internal string GeneratePrefix(string format)
	{
		string array;
		do
		{
			array = string.Format(CultureInfo.InvariantCulture, format, prefixIndex++);
		}
		while (nameTable.Get(array) != null);
		return nameTable.Add(array);
	}
}
