using System.Collections;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class Stylesheet
{
	private class WhitespaceElement
	{
		private int key;

		private double priority;

		private bool preserveSpace;

		internal double Priority => priority;

		internal int Key => key;

		internal bool PreserveSpace => preserveSpace;

		internal WhitespaceElement(int Key, double priority, bool PreserveSpace)
		{
			key = Key;
			this.priority = priority;
			preserveSpace = PreserveSpace;
		}

		internal void ReplaceValue(bool PreserveSpace)
		{
			preserveSpace = PreserveSpace;
		}
	}

	private ArrayList imports = new ArrayList();

	private Hashtable modeManagers;

	private Hashtable templateNameTable = new Hashtable();

	private Hashtable attributeSetTable;

	private int templateCount;

	private Hashtable queryKeyTable;

	private ArrayList whitespaceList;

	private bool whitespace;

	private Hashtable scriptObjectTypes = new Hashtable();

	private TemplateManager templates;

	internal bool Whitespace => whitespace;

	internal ArrayList Imports => imports;

	internal Hashtable AttributeSetTable => attributeSetTable;

	internal Hashtable ScriptObjectTypes => scriptObjectTypes;

	internal void AddSpace(Compiler compiler, string query, double Priority, bool PreserveSpace)
	{
		WhitespaceElement whitespaceElement;
		if (queryKeyTable != null)
		{
			if (queryKeyTable.Contains(query))
			{
				whitespaceElement = (WhitespaceElement)queryKeyTable[query];
				whitespaceElement.ReplaceValue(PreserveSpace);
				return;
			}
		}
		else
		{
			queryKeyTable = new Hashtable();
			whitespaceList = new ArrayList();
		}
		whitespaceElement = new WhitespaceElement(compiler.AddQuery(query), Priority, PreserveSpace);
		queryKeyTable[query] = whitespaceElement;
		whitespaceList.Add(whitespaceElement);
	}

	internal void SortWhiteSpace()
	{
		if (queryKeyTable != null)
		{
			for (int i = 0; i < whitespaceList.Count; i++)
			{
				for (int num = whitespaceList.Count - 1; num > i; num--)
				{
					WhitespaceElement whitespaceElement = (WhitespaceElement)whitespaceList[num - 1];
					WhitespaceElement whitespaceElement2 = (WhitespaceElement)whitespaceList[num];
					if (whitespaceElement2.Priority < whitespaceElement.Priority)
					{
						whitespaceList[num - 1] = whitespaceElement2;
						whitespaceList[num] = whitespaceElement;
					}
				}
			}
			whitespace = true;
		}
		if (imports == null)
		{
			return;
		}
		for (int num2 = imports.Count - 1; num2 >= 0; num2--)
		{
			Stylesheet stylesheet = (Stylesheet)imports[num2];
			if (stylesheet.Whitespace)
			{
				stylesheet.SortWhiteSpace();
				whitespace = true;
			}
		}
	}

	internal bool PreserveWhiteSpace(Processor proc, XPathNavigator node)
	{
		if (whitespaceList != null)
		{
			int num = whitespaceList.Count - 1;
			while (0 <= num)
			{
				WhitespaceElement whitespaceElement = (WhitespaceElement)whitespaceList[num];
				if (proc.Matches(node, whitespaceElement.Key))
				{
					return whitespaceElement.PreserveSpace;
				}
				num--;
			}
		}
		if (imports != null)
		{
			for (int num2 = imports.Count - 1; num2 >= 0; num2--)
			{
				if (!((Stylesheet)imports[num2]).PreserveWhiteSpace(proc, node))
				{
					return false;
				}
			}
		}
		return true;
	}

	internal void AddAttributeSet(AttributeSetAction attributeSet)
	{
		if (attributeSetTable == null)
		{
			attributeSetTable = new Hashtable();
		}
		if (!attributeSetTable.ContainsKey(attributeSet.Name))
		{
			attributeSetTable[attributeSet.Name] = attributeSet;
		}
		else
		{
			((AttributeSetAction)attributeSetTable[attributeSet.Name]).Merge(attributeSet);
		}
	}

	internal void AddTemplate(TemplateAction template)
	{
		XmlQualifiedName xmlQualifiedName = template.Mode;
		if (template.Name != null)
		{
			if (templateNameTable.ContainsKey(template.Name))
			{
				throw XsltException.Create("'{0}' is a duplicate template name.", template.Name.ToString());
			}
			templateNameTable[template.Name] = template;
		}
		if (template.MatchKey == -1)
		{
			return;
		}
		if (modeManagers == null)
		{
			modeManagers = new Hashtable();
		}
		if (xmlQualifiedName == null)
		{
			xmlQualifiedName = XmlQualifiedName.Empty;
		}
		TemplateManager templateManager = (TemplateManager)modeManagers[xmlQualifiedName];
		if (templateManager == null)
		{
			templateManager = new TemplateManager(this, xmlQualifiedName);
			modeManagers[xmlQualifiedName] = templateManager;
			if (xmlQualifiedName.IsEmpty)
			{
				templates = templateManager;
			}
		}
		template.TemplateId = ++templateCount;
		templateManager.AddTemplate(template);
	}

	internal void ProcessTemplates()
	{
		if (modeManagers != null)
		{
			IDictionaryEnumerator enumerator = modeManagers.GetEnumerator();
			while (enumerator.MoveNext())
			{
				((TemplateManager)enumerator.Value).ProcessTemplates();
			}
		}
		if (imports != null)
		{
			for (int num = imports.Count - 1; num >= 0; num--)
			{
				((Stylesheet)imports[num]).ProcessTemplates();
			}
		}
	}

	internal void ReplaceNamespaceAlias(Compiler compiler)
	{
		if (modeManagers != null)
		{
			IDictionaryEnumerator enumerator = modeManagers.GetEnumerator();
			while (enumerator.MoveNext())
			{
				TemplateManager templateManager = (TemplateManager)enumerator.Value;
				if (templateManager.templates != null)
				{
					for (int i = 0; i < templateManager.templates.Count; i++)
					{
						((TemplateAction)templateManager.templates[i]).ReplaceNamespaceAlias(compiler);
					}
				}
			}
		}
		if (templateNameTable != null)
		{
			IDictionaryEnumerator enumerator2 = templateNameTable.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				((TemplateAction)enumerator2.Value).ReplaceNamespaceAlias(compiler);
			}
		}
		if (imports != null)
		{
			for (int num = imports.Count - 1; num >= 0; num--)
			{
				((Stylesheet)imports[num]).ReplaceNamespaceAlias(compiler);
			}
		}
	}

	internal TemplateAction FindTemplate(Processor processor, XPathNavigator navigator, XmlQualifiedName mode)
	{
		TemplateAction templateAction = null;
		if (modeManagers != null)
		{
			TemplateManager templateManager = (TemplateManager)modeManagers[mode];
			if (templateManager != null)
			{
				templateAction = templateManager.FindTemplate(processor, navigator);
			}
		}
		if (templateAction == null)
		{
			templateAction = FindTemplateImports(processor, navigator, mode);
		}
		return templateAction;
	}

	internal TemplateAction FindTemplateImports(Processor processor, XPathNavigator navigator, XmlQualifiedName mode)
	{
		TemplateAction templateAction = null;
		if (imports != null)
		{
			for (int num = imports.Count - 1; num >= 0; num--)
			{
				templateAction = ((Stylesheet)imports[num]).FindTemplate(processor, navigator, mode);
				if (templateAction != null)
				{
					return templateAction;
				}
			}
		}
		return templateAction;
	}

	internal TemplateAction FindTemplate(Processor processor, XPathNavigator navigator)
	{
		TemplateAction templateAction = null;
		if (templates != null)
		{
			templateAction = templates.FindTemplate(processor, navigator);
		}
		if (templateAction == null)
		{
			templateAction = FindTemplateImports(processor, navigator);
		}
		return templateAction;
	}

	internal TemplateAction FindTemplate(XmlQualifiedName name)
	{
		TemplateAction templateAction = null;
		if (templateNameTable != null)
		{
			templateAction = (TemplateAction)templateNameTable[name];
		}
		if (templateAction == null && imports != null)
		{
			for (int num = imports.Count - 1; num >= 0; num--)
			{
				templateAction = ((Stylesheet)imports[num]).FindTemplate(name);
				if (templateAction != null)
				{
					return templateAction;
				}
			}
		}
		return templateAction;
	}

	internal TemplateAction FindTemplateImports(Processor processor, XPathNavigator navigator)
	{
		TemplateAction templateAction = null;
		if (imports != null)
		{
			for (int num = imports.Count - 1; num >= 0; num--)
			{
				templateAction = ((Stylesheet)imports[num]).FindTemplate(processor, navigator);
				if (templateAction != null)
				{
					return templateAction;
				}
			}
		}
		return templateAction;
	}
}
