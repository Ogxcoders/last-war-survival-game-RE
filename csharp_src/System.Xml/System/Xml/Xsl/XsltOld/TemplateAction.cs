using System.Xml.XPath;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class TemplateAction : TemplateBaseAction
{
	private int matchKey = -1;

	private XmlQualifiedName name;

	private double priority = double.NaN;

	private XmlQualifiedName mode;

	private int templateId;

	private bool replaceNSAliasesDone;

	internal int MatchKey => matchKey;

	internal XmlQualifiedName Name => name;

	internal double Priority => priority;

	internal XmlQualifiedName Mode => mode;

	internal int TemplateId
	{
		get
		{
			return templateId;
		}
		set
		{
			templateId = value;
		}
	}

	internal override void Compile(Compiler compiler)
	{
		CompileAttributes(compiler);
		if (matchKey == -1)
		{
			if (name == null)
			{
				throw XsltException.Create("The 'xsl:template' instruction must have the 'match' and/or 'name' attribute present.");
			}
			if (mode != null)
			{
				throw XsltException.Create("An 'xsl:template' element without a 'match' attribute cannot have a 'mode' attribute.");
			}
		}
		compiler.BeginTemplate(this);
		if (compiler.Recurse())
		{
			CompileParameters(compiler);
			CompileTemplate(compiler);
			compiler.ToParent();
		}
		compiler.EndTemplate();
		AnalyzePriority(compiler);
	}

	internal virtual void CompileSingle(Compiler compiler)
	{
		matchKey = compiler.AddQuery("/", allowVar: false, allowKey: true, isPattern: true);
		priority = 0.5;
		CompileOnceTemplate(compiler);
	}

	internal override bool CompileAttribute(Compiler compiler)
	{
		string localName = compiler.Input.LocalName;
		string value = compiler.Input.Value;
		if (Ref.Equal(localName, compiler.Atoms.Match))
		{
			matchKey = compiler.AddQuery(value, allowVar: false, allowKey: true, isPattern: true);
		}
		else if (Ref.Equal(localName, compiler.Atoms.Name))
		{
			name = compiler.CreateXPathQName(value);
		}
		else if (Ref.Equal(localName, compiler.Atoms.Priority))
		{
			priority = XmlConvert.ToXPathDouble(value);
			if (double.IsNaN(priority) && !compiler.ForwardCompatibility)
			{
				throw XsltException.Create("'{1}' is an invalid value for the '{0}' attribute.", "priority", value);
			}
		}
		else
		{
			if (!Ref.Equal(localName, compiler.Atoms.Mode))
			{
				return false;
			}
			if (compiler.AllowBuiltInMode && value == "*")
			{
				mode = Compiler.BuiltInMode;
			}
			else
			{
				mode = compiler.CreateXPathQName(value);
			}
		}
		return true;
	}

	private void AnalyzePriority(Compiler compiler)
	{
		_ = compiler.Input;
		if (double.IsNaN(priority) && matchKey != -1)
		{
			TheQuery theQuery = compiler.QueryStore[MatchKey];
			CompiledXpathExpr compiledQuery = theQuery.CompiledQuery;
			Query query;
			for (query = compiledQuery.QueryTree; query is UnionExpr unionExpr; query = unionExpr.qy1)
			{
				TemplateAction templateAction = CloneWithoutName();
				compiler.QueryStore.Add(new TheQuery(new CompiledXpathExpr(unionExpr.qy2, compiledQuery.Expression, needContext: false), theQuery._ScopeManager));
				templateAction.matchKey = compiler.QueryStore.Count - 1;
				templateAction.priority = unionExpr.qy2.XsltDefaultPriority;
				compiler.AddTemplate(templateAction);
			}
			if (compiledQuery.QueryTree != query)
			{
				compiler.QueryStore[MatchKey] = new TheQuery(new CompiledXpathExpr(query, compiledQuery.Expression, needContext: false), theQuery._ScopeManager);
			}
			priority = query.XsltDefaultPriority;
		}
	}

	protected void CompileParameters(Compiler compiler)
	{
		NavigatorInput input = compiler.Input;
		do
		{
			switch (input.NodeType)
			{
			case XPathNodeType.Element:
				if (Ref.Equal(input.NamespaceURI, input.Atoms.UriXsl) && Ref.Equal(input.LocalName, input.Atoms.Param))
				{
					compiler.PushNamespaceScope();
					AddAction(compiler.CreateVariableAction(VariableType.LocalParameter));
					compiler.PopScope();
					break;
				}
				return;
			case XPathNodeType.Text:
				return;
			case XPathNodeType.SignificantWhitespace:
				AddEvent(compiler.CreateTextEvent());
				break;
			}
		}
		while (input.Advance());
	}

	private TemplateAction CloneWithoutName()
	{
		return new TemplateAction
		{
			containedActions = containedActions,
			mode = mode,
			variableCount = variableCount,
			replaceNSAliasesDone = true
		};
	}

	internal override void ReplaceNamespaceAlias(Compiler compiler)
	{
		if (!replaceNSAliasesDone)
		{
			base.ReplaceNamespaceAlias(compiler);
			replaceNSAliasesDone = true;
		}
	}

	internal override void Execute(Processor processor, ActionFrame frame)
	{
		switch (frame.State)
		{
		case 0:
			if (variableCount > 0)
			{
				frame.AllocateVariables(variableCount);
			}
			if (containedActions != null && containedActions.Count > 0)
			{
				processor.PushActionFrame(frame);
				frame.State = 1;
			}
			else
			{
				frame.Finished();
			}
			break;
		case 1:
			frame.Finished();
			break;
		}
	}
}
