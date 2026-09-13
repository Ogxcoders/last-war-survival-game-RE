using System.Collections;

namespace System.Xml.Xsl.XsltOld;

internal class InputScope : DocumentScope
{
	private InputScope parent;

	private bool forwardCompatibility;

	private bool canHaveApplyImports;

	private Hashtable variables;

	private Hashtable extensionNamespaces;

	private Hashtable excludedNamespaces;

	internal InputScope Parent => parent;

	internal Hashtable Variables => variables;

	internal bool ForwardCompatibility
	{
		get
		{
			return forwardCompatibility;
		}
		set
		{
			forwardCompatibility = value;
		}
	}

	internal bool CanHaveApplyImports
	{
		get
		{
			return canHaveApplyImports;
		}
		set
		{
			canHaveApplyImports = value;
		}
	}

	internal InputScope(InputScope parent)
	{
		Init(parent);
	}

	internal void Init(InputScope parent)
	{
		scopes = null;
		this.parent = parent;
		if (this.parent != null)
		{
			forwardCompatibility = this.parent.forwardCompatibility;
			canHaveApplyImports = this.parent.canHaveApplyImports;
		}
	}

	internal void InsertExtensionNamespace(string nspace)
	{
		if (extensionNamespaces == null)
		{
			extensionNamespaces = new Hashtable();
		}
		extensionNamespaces[nspace] = null;
	}

	internal bool IsExtensionNamespace(string nspace)
	{
		if (extensionNamespaces == null)
		{
			return false;
		}
		return extensionNamespaces.Contains(nspace);
	}

	internal void InsertExcludedNamespace(string nspace)
	{
		if (excludedNamespaces == null)
		{
			excludedNamespaces = new Hashtable();
		}
		excludedNamespaces[nspace] = null;
	}

	internal bool IsExcludedNamespace(string nspace)
	{
		if (excludedNamespaces == null)
		{
			return false;
		}
		return excludedNamespaces.Contains(nspace);
	}

	internal void InsertVariable(VariableAction variable)
	{
		if (variables == null)
		{
			variables = new Hashtable();
		}
		variables[variable.Name] = variable;
	}

	internal int GetVeriablesCount()
	{
		if (variables == null)
		{
			return 0;
		}
		return variables.Count;
	}

	public VariableAction ResolveVariable(XmlQualifiedName qname)
	{
		for (InputScope inputScope = this; inputScope != null; inputScope = inputScope.Parent)
		{
			if (inputScope.Variables != null)
			{
				VariableAction variableAction = (VariableAction)inputScope.Variables[qname];
				if (variableAction != null)
				{
					return variableAction;
				}
			}
		}
		return null;
	}

	public VariableAction ResolveGlobalVariable(XmlQualifiedName qname)
	{
		InputScope inputScope = null;
		for (InputScope inputScope2 = this; inputScope2 != null; inputScope2 = inputScope2.Parent)
		{
			inputScope = inputScope2;
		}
		return inputScope.ResolveVariable(qname);
	}
}
