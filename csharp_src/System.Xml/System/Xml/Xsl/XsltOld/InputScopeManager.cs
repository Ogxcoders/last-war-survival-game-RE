using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class InputScopeManager
{
	private InputScope scopeStack;

	private string defaultNS = string.Empty;

	private XPathNavigator navigator;

	internal InputScope CurrentScope => scopeStack;

	internal InputScope VariableScope => scopeStack.Parent;

	public XPathNavigator Navigator => navigator;

	public string DefaultNamespace => defaultNS;

	public InputScopeManager(XPathNavigator navigator, InputScope rootScope)
	{
		this.navigator = navigator;
		scopeStack = rootScope;
	}

	internal InputScopeManager Clone()
	{
		return new InputScopeManager(navigator, null)
		{
			scopeStack = scopeStack,
			defaultNS = defaultNS
		};
	}

	internal InputScope PushScope()
	{
		scopeStack = new InputScope(scopeStack);
		return scopeStack;
	}

	internal void PopScope()
	{
		if (scopeStack != null)
		{
			for (NamespaceDecl namespaceDecl = scopeStack.Scopes; namespaceDecl != null; namespaceDecl = namespaceDecl.Next)
			{
				defaultNS = namespaceDecl.PrevDefaultNsUri;
			}
			scopeStack = scopeStack.Parent;
		}
	}

	internal void PushNamespace(string prefix, string nspace)
	{
		scopeStack.AddNamespace(prefix, nspace, defaultNS);
		if (prefix == null || prefix.Length == 0)
		{
			defaultNS = nspace;
		}
	}

	private string ResolveNonEmptyPrefix(string prefix)
	{
		if (prefix == "xml")
		{
			return "http://www.w3.org/XML/1998/namespace";
		}
		if (prefix == "xmlns")
		{
			return "http://www.w3.org/2000/xmlns/";
		}
		for (InputScope parent = scopeStack; parent != null; parent = parent.Parent)
		{
			string text = parent.ResolveNonAtom(prefix);
			if (text != null)
			{
				return text;
			}
		}
		throw XsltException.Create("Prefix '{0}' is not defined.", prefix);
	}

	public string ResolveXmlNamespace(string prefix)
	{
		if (prefix.Length == 0)
		{
			return defaultNS;
		}
		return ResolveNonEmptyPrefix(prefix);
	}

	public string ResolveXPathNamespace(string prefix)
	{
		if (prefix.Length == 0)
		{
			return string.Empty;
		}
		return ResolveNonEmptyPrefix(prefix);
	}

	internal void InsertExtensionNamespaces(string[] nsList)
	{
		for (int i = 0; i < nsList.Length; i++)
		{
			scopeStack.InsertExtensionNamespace(nsList[i]);
		}
	}

	internal bool IsExtensionNamespace(string nspace)
	{
		for (InputScope parent = scopeStack; parent != null; parent = parent.Parent)
		{
			if (parent.IsExtensionNamespace(nspace))
			{
				return true;
			}
		}
		return false;
	}

	internal void InsertExcludedNamespaces(string[] nsList)
	{
		for (int i = 0; i < nsList.Length; i++)
		{
			scopeStack.InsertExcludedNamespace(nsList[i]);
		}
	}

	internal bool IsExcludedNamespace(string nspace)
	{
		for (InputScope parent = scopeStack; parent != null; parent = parent.Parent)
		{
			if (parent.IsExcludedNamespace(nspace))
			{
				return true;
			}
		}
		return false;
	}
}
