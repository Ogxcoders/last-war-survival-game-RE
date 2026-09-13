using System.Collections;
using System.Globalization;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl.XsltOld;

internal class ContainerAction : CompiledAction
{
	internal ArrayList containedActions;

	internal CopyCodeAction lastCopyCodeAction;

	private int maxid;

	protected const int ProcessingChildren = 1;

	internal override void Compile(Compiler compiler)
	{
		throw new NotImplementedException();
	}

	internal void CompileStylesheetAttributes(Compiler compiler)
	{
		NavigatorInput input = compiler.Input;
		string localName = input.LocalName;
		string text = null;
		string text2 = null;
		if (input.MoveToFirstAttribute())
		{
			do
			{
				string namespaceURI = input.NamespaceURI;
				string localName2 = input.LocalName;
				if (namespaceURI.Length != 0)
				{
					continue;
				}
				if (Ref.Equal(localName2, input.Atoms.Version))
				{
					text2 = input.Value;
					if (1.0 <= XmlConvert.ToXPathDouble(text2))
					{
						compiler.ForwardCompatibility = text2 != "1.0";
					}
					else if (!compiler.ForwardCompatibility)
					{
						throw XsltException.Create("'{1}' is an invalid value for the '{0}' attribute.", "version", text2);
					}
				}
				else if (Ref.Equal(localName2, input.Atoms.ExtensionElementPrefixes))
				{
					compiler.InsertExtensionNamespace(input.Value);
				}
				else if (Ref.Equal(localName2, input.Atoms.ExcludeResultPrefixes))
				{
					compiler.InsertExcludedNamespace(input.Value);
				}
				else if (!Ref.Equal(localName2, input.Atoms.Id))
				{
					text = localName2;
				}
			}
			while (input.MoveToNextAttribute());
			input.ToParent();
		}
		if (text2 == null)
		{
			throw XsltException.Create("Missing mandatory attribute '{0}'.", "version");
		}
		if (text != null && !compiler.ForwardCompatibility)
		{
			throw XsltException.Create("'{0}' is an invalid attribute for the '{1}' element.", text, localName);
		}
	}

	internal void CompileSingleTemplate(Compiler compiler)
	{
		NavigatorInput input = compiler.Input;
		string text = null;
		if (input.MoveToFirstAttribute())
		{
			do
			{
				string namespaceURI = input.NamespaceURI;
				string localName = input.LocalName;
				if (Ref.Equal(namespaceURI, input.Atoms.UriXsl) && Ref.Equal(localName, input.Atoms.Version))
				{
					text = input.Value;
				}
			}
			while (input.MoveToNextAttribute());
			input.ToParent();
		}
		if (text == null)
		{
			if (Ref.Equal(input.LocalName, input.Atoms.Stylesheet) && input.NamespaceURI == "http://www.w3.org/TR/WD-xsl")
			{
				throw XsltException.Create("The 'http://www.w3.org/TR/WD-xsl' namespace is no longer supported.");
			}
			throw XsltException.Create("Stylesheet must start either with an 'xsl:stylesheet' or an 'xsl:transform' element, or with a literal result element that has an 'xsl:version' attribute, where prefix 'xsl' denotes the 'http://www.w3.org/1999/XSL/Transform' namespace.");
		}
		compiler.AddTemplate(compiler.CreateSingleTemplateAction());
	}

	protected void CompileDocument(Compiler compiler, bool inInclude)
	{
		NavigatorInput input = compiler.Input;
		while (input.NodeType != XPathNodeType.Element)
		{
			if (!compiler.Advance())
			{
				throw XsltException.Create("Stylesheet must start either with an 'xsl:stylesheet' or an 'xsl:transform' element, or with a literal result element that has an 'xsl:version' attribute, where prefix 'xsl' denotes the 'http://www.w3.org/1999/XSL/Transform' namespace.");
			}
		}
		if (Ref.Equal(input.NamespaceURI, input.Atoms.UriXsl))
		{
			if (!Ref.Equal(input.LocalName, input.Atoms.Stylesheet) && !Ref.Equal(input.LocalName, input.Atoms.Transform))
			{
				throw XsltException.Create("Stylesheet must start either with an 'xsl:stylesheet' or an 'xsl:transform' element, or with a literal result element that has an 'xsl:version' attribute, where prefix 'xsl' denotes the 'http://www.w3.org/1999/XSL/Transform' namespace.");
			}
			compiler.PushNamespaceScope();
			CompileStylesheetAttributes(compiler);
			CompileTopLevelElements(compiler);
			if (!inInclude)
			{
				CompileImports(compiler);
			}
		}
		else
		{
			compiler.PushLiteralScope();
			CompileSingleTemplate(compiler);
		}
		compiler.PopScope();
	}

	internal Stylesheet CompileImport(Compiler compiler, Uri uri, int id)
	{
		NavigatorInput navigatorInput = compiler.ResolveDocument(uri);
		compiler.PushInputDocument(navigatorInput);
		try
		{
			compiler.PushStylesheet(new Stylesheet());
			compiler.Stylesheetid = id;
			CompileDocument(compiler, inInclude: false);
		}
		catch (XsltCompileException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new XsltCompileException(inner, navigatorInput.BaseURI, navigatorInput.LineNumber, navigatorInput.LinePosition);
		}
		finally
		{
			compiler.PopInputDocument();
		}
		return compiler.PopStylesheet();
	}

	private void CompileImports(Compiler compiler)
	{
		ArrayList imports = compiler.CompiledStylesheet.Imports;
		int stylesheetid = compiler.Stylesheetid;
		int num = imports.Count - 1;
		while (0 <= num)
		{
			Uri uri = imports[num] as Uri;
			imports[num] = CompileImport(compiler, uri, ++maxid);
			num--;
		}
		compiler.Stylesheetid = stylesheetid;
	}

	private void CompileInclude(Compiler compiler)
	{
		Uri uri = compiler.ResolveUri(compiler.GetSingleAttribute(compiler.Input.Atoms.Href));
		string text = uri.ToString();
		if (compiler.IsCircularReference(text))
		{
			throw XsltException.Create("Stylesheet '{0}' cannot directly or indirectly include or import itself.", text);
		}
		NavigatorInput navigatorInput = compiler.ResolveDocument(uri);
		compiler.PushInputDocument(navigatorInput);
		try
		{
			CompileDocument(compiler, inInclude: true);
		}
		catch (XsltCompileException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new XsltCompileException(inner, navigatorInput.BaseURI, navigatorInput.LineNumber, navigatorInput.LinePosition);
		}
		finally
		{
			compiler.PopInputDocument();
		}
		CheckEmpty(compiler);
	}

	internal void CompileNamespaceAlias(Compiler compiler)
	{
		NavigatorInput input = compiler.Input;
		string localName = input.LocalName;
		string text = null;
		string text2 = null;
		string text3 = null;
		string prefix = null;
		if (input.MoveToFirstAttribute())
		{
			do
			{
				string namespaceURI = input.NamespaceURI;
				string localName2 = input.LocalName;
				if (namespaceURI.Length == 0)
				{
					if (Ref.Equal(localName2, input.Atoms.StylesheetPrefix))
					{
						text3 = input.Value;
						text = compiler.GetNsAlias(ref text3);
					}
					else if (Ref.Equal(localName2, input.Atoms.ResultPrefix))
					{
						prefix = input.Value;
						text2 = compiler.GetNsAlias(ref prefix);
					}
					else if (!compiler.ForwardCompatibility)
					{
						throw XsltException.Create("'{0}' is an invalid attribute for the '{1}' element.", localName2, localName);
					}
				}
			}
			while (input.MoveToNextAttribute());
			input.ToParent();
		}
		CheckRequiredAttribute(compiler, text, "stylesheet-prefix");
		CheckRequiredAttribute(compiler, text2, "result-prefix");
		CheckEmpty(compiler);
		compiler.AddNamespaceAlias(text, new NamespaceInfo(prefix, text2, compiler.Stylesheetid));
	}

	internal void CompileKey(Compiler compiler)
	{
		NavigatorInput input = compiler.Input;
		string localName = input.LocalName;
		int num = -1;
		int num2 = -1;
		XmlQualifiedName xmlQualifiedName = null;
		if (input.MoveToFirstAttribute())
		{
			do
			{
				string namespaceURI = input.NamespaceURI;
				string localName2 = input.LocalName;
				string value = input.Value;
				if (namespaceURI.Length == 0)
				{
					if (Ref.Equal(localName2, input.Atoms.Name))
					{
						xmlQualifiedName = compiler.CreateXPathQName(value);
					}
					else if (Ref.Equal(localName2, input.Atoms.Match))
					{
						num = compiler.AddQuery(value, allowVar: false, allowKey: false, isPattern: true);
					}
					else if (Ref.Equal(localName2, input.Atoms.Use))
					{
						num2 = compiler.AddQuery(value, allowVar: false, allowKey: false, isPattern: false);
					}
					else if (!compiler.ForwardCompatibility)
					{
						throw XsltException.Create("'{0}' is an invalid attribute for the '{1}' element.", localName2, localName);
					}
				}
			}
			while (input.MoveToNextAttribute());
			input.ToParent();
		}
		CheckRequiredAttribute(compiler, num != -1, "match");
		CheckRequiredAttribute(compiler, num2 != -1, "use");
		CheckRequiredAttribute(compiler, xmlQualifiedName != null, "name");
		compiler.InsertKey(xmlQualifiedName, num, num2);
	}

	protected void CompileDecimalFormat(Compiler compiler)
	{
		NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
		DecimalFormat decimalFormat = new DecimalFormat(numberFormatInfo, '#', '0', ';');
		XmlQualifiedName xmlQualifiedName = null;
		NavigatorInput input = compiler.Input;
		if (input.MoveToFirstAttribute())
		{
			do
			{
				if (input.Prefix.Length != 0)
				{
					continue;
				}
				string localName = input.LocalName;
				string value = input.Value;
				if (Ref.Equal(localName, input.Atoms.Name))
				{
					xmlQualifiedName = compiler.CreateXPathQName(value);
				}
				else if (Ref.Equal(localName, input.Atoms.DecimalSeparator))
				{
					numberFormatInfo.NumberDecimalSeparator = value;
				}
				else if (Ref.Equal(localName, input.Atoms.GroupingSeparator))
				{
					numberFormatInfo.NumberGroupSeparator = value;
				}
				else if (Ref.Equal(localName, input.Atoms.Infinity))
				{
					numberFormatInfo.PositiveInfinitySymbol = value;
				}
				else if (Ref.Equal(localName, input.Atoms.MinusSign))
				{
					numberFormatInfo.NegativeSign = value;
				}
				else if (Ref.Equal(localName, input.Atoms.NaN))
				{
					numberFormatInfo.NaNSymbol = value;
				}
				else if (Ref.Equal(localName, input.Atoms.Percent))
				{
					numberFormatInfo.PercentSymbol = value;
				}
				else if (Ref.Equal(localName, input.Atoms.PerMille))
				{
					numberFormatInfo.PerMilleSymbol = value;
				}
				else if (Ref.Equal(localName, input.Atoms.Digit))
				{
					if (CheckAttribute(value.Length == 1, compiler))
					{
						decimalFormat.digit = value[0];
					}
				}
				else if (Ref.Equal(localName, input.Atoms.ZeroDigit))
				{
					if (CheckAttribute(value.Length == 1, compiler))
					{
						decimalFormat.zeroDigit = value[0];
					}
				}
				else if (Ref.Equal(localName, input.Atoms.PatternSeparator) && CheckAttribute(value.Length == 1, compiler))
				{
					decimalFormat.patternSeparator = value[0];
				}
			}
			while (input.MoveToNextAttribute());
			input.ToParent();
		}
		numberFormatInfo.NegativeInfinitySymbol = numberFormatInfo.NegativeSign + numberFormatInfo.PositiveInfinitySymbol;
		if (xmlQualifiedName == null)
		{
			xmlQualifiedName = new XmlQualifiedName();
		}
		compiler.AddDecimalFormat(xmlQualifiedName, decimalFormat);
		CheckEmpty(compiler);
	}

	internal bool CheckAttribute(bool valid, Compiler compiler)
	{
		if (!valid)
		{
			if (!compiler.ForwardCompatibility)
			{
				throw XsltException.Create("'{1}' is an invalid value for the '{0}' attribute.", compiler.Input.LocalName, compiler.Input.Value);
			}
			return false;
		}
		return true;
	}

	protected void CompileSpace(Compiler compiler, bool preserve)
	{
		string[] array = XmlConvert.SplitString(compiler.GetSingleAttribute(compiler.Input.Atoms.Elements));
		for (int i = 0; i < array.Length; i++)
		{
			double priority = NameTest(array[i]);
			compiler.CompiledStylesheet.AddSpace(compiler, array[i], priority, preserve);
		}
		CheckEmpty(compiler);
	}

	private double NameTest(string name)
	{
		if (name == "*")
		{
			return -0.5;
		}
		int num = name.Length - 2;
		if (0 <= num && name[num] == ':' && name[num + 1] == '*')
		{
			if (!PrefixQName.ValidatePrefix(name.Substring(0, num)))
			{
				throw XsltException.Create("'{1}' is an invalid value for the '{0}' attribute.", "elements", name);
			}
			return -0.25;
		}
		PrefixQName.ParseQualifiedName(name, out var _, out var _);
		return 0.0;
	}

	protected void CompileTopLevelElements(Compiler compiler)
	{
		if (!compiler.Recurse())
		{
			return;
		}
		NavigatorInput input = compiler.Input;
		bool flag = false;
		do
		{
			switch (input.NodeType)
			{
			case XPathNodeType.Element:
			{
				string localName = input.LocalName;
				string namespaceURI = input.NamespaceURI;
				if (Ref.Equal(namespaceURI, input.Atoms.UriXsl))
				{
					if (Ref.Equal(localName, input.Atoms.Import))
					{
						if (flag)
						{
							throw XsltException.Create("'xsl:import' instructions must precede all other element children of an 'xsl:stylesheet' element.");
						}
						Uri uri = compiler.ResolveUri(compiler.GetSingleAttribute(compiler.Input.Atoms.Href));
						string text = uri.ToString();
						if (compiler.IsCircularReference(text))
						{
							throw XsltException.Create("Stylesheet '{0}' cannot directly or indirectly include or import itself.", text);
						}
						compiler.CompiledStylesheet.Imports.Add(uri);
						CheckEmpty(compiler);
						break;
					}
					if (Ref.Equal(localName, input.Atoms.Include))
					{
						flag = true;
						CompileInclude(compiler);
						break;
					}
					flag = true;
					compiler.PushNamespaceScope();
					if (Ref.Equal(localName, input.Atoms.StripSpace))
					{
						CompileSpace(compiler, preserve: false);
					}
					else if (Ref.Equal(localName, input.Atoms.PreserveSpace))
					{
						CompileSpace(compiler, preserve: true);
					}
					else if (Ref.Equal(localName, input.Atoms.Output))
					{
						CompileOutput(compiler);
					}
					else if (Ref.Equal(localName, input.Atoms.Key))
					{
						CompileKey(compiler);
					}
					else if (Ref.Equal(localName, input.Atoms.DecimalFormat))
					{
						CompileDecimalFormat(compiler);
					}
					else if (Ref.Equal(localName, input.Atoms.NamespaceAlias))
					{
						CompileNamespaceAlias(compiler);
					}
					else if (Ref.Equal(localName, input.Atoms.AttributeSet))
					{
						compiler.AddAttributeSet(compiler.CreateAttributeSetAction());
					}
					else if (Ref.Equal(localName, input.Atoms.Variable))
					{
						VariableAction variableAction = compiler.CreateVariableAction(VariableType.GlobalVariable);
						if (variableAction != null)
						{
							AddAction(variableAction);
						}
					}
					else if (Ref.Equal(localName, input.Atoms.Param))
					{
						VariableAction variableAction2 = compiler.CreateVariableAction(VariableType.GlobalParameter);
						if (variableAction2 != null)
						{
							AddAction(variableAction2);
						}
					}
					else if (Ref.Equal(localName, input.Atoms.Template))
					{
						compiler.AddTemplate(compiler.CreateTemplateAction());
					}
					else if (!compiler.ForwardCompatibility)
					{
						throw compiler.UnexpectedKeyword();
					}
					compiler.PopScope();
				}
				else if (namespaceURI.Length == 0)
				{
					throw XsltException.Create("Top-level element '{0}' may not have a null namespace URI.", input.Name);
				}
				break;
			}
			default:
				throw XsltException.Create("The contents of '{0}' are invalid.", "stylesheet");
			case XPathNodeType.SignificantWhitespace:
			case XPathNodeType.Whitespace:
			case XPathNodeType.ProcessingInstruction:
			case XPathNodeType.Comment:
				break;
			}
		}
		while (compiler.Advance());
		compiler.ToParent();
	}

	protected void CompileTemplate(Compiler compiler)
	{
		do
		{
			CompileOnceTemplate(compiler);
		}
		while (compiler.Advance());
	}

	protected void CompileOnceTemplate(Compiler compiler)
	{
		NavigatorInput input = compiler.Input;
		if (input.NodeType == XPathNodeType.Element)
		{
			string namespaceURI = input.NamespaceURI;
			if (Ref.Equal(namespaceURI, input.Atoms.UriXsl))
			{
				compiler.PushNamespaceScope();
				CompileInstruction(compiler);
				compiler.PopScope();
				return;
			}
			compiler.PushLiteralScope();
			compiler.InsertExtensionNamespace();
			if (compiler.IsExtensionNamespace(namespaceURI))
			{
				AddAction(compiler.CreateNewInstructionAction());
			}
			else
			{
				CompileLiteral(compiler);
			}
			compiler.PopScope();
		}
		else
		{
			CompileLiteral(compiler);
		}
	}

	private void CompileInstruction(Compiler compiler)
	{
		NavigatorInput input = compiler.Input;
		CompiledAction compiledAction = null;
		string localName = input.LocalName;
		if (Ref.Equal(localName, input.Atoms.ApplyImports))
		{
			compiledAction = compiler.CreateApplyImportsAction();
		}
		else if (Ref.Equal(localName, input.Atoms.ApplyTemplates))
		{
			compiledAction = compiler.CreateApplyTemplatesAction();
		}
		else if (Ref.Equal(localName, input.Atoms.Attribute))
		{
			compiledAction = compiler.CreateAttributeAction();
		}
		else if (Ref.Equal(localName, input.Atoms.CallTemplate))
		{
			compiledAction = compiler.CreateCallTemplateAction();
		}
		else if (Ref.Equal(localName, input.Atoms.Choose))
		{
			compiledAction = compiler.CreateChooseAction();
		}
		else if (Ref.Equal(localName, input.Atoms.Comment))
		{
			compiledAction = compiler.CreateCommentAction();
		}
		else if (Ref.Equal(localName, input.Atoms.Copy))
		{
			compiledAction = compiler.CreateCopyAction();
		}
		else if (Ref.Equal(localName, input.Atoms.CopyOf))
		{
			compiledAction = compiler.CreateCopyOfAction();
		}
		else if (Ref.Equal(localName, input.Atoms.Element))
		{
			compiledAction = compiler.CreateElementAction();
		}
		else
		{
			if (Ref.Equal(localName, input.Atoms.Fallback))
			{
				return;
			}
			if (Ref.Equal(localName, input.Atoms.ForEach))
			{
				compiledAction = compiler.CreateForEachAction();
			}
			else if (Ref.Equal(localName, input.Atoms.If))
			{
				compiledAction = compiler.CreateIfAction(IfAction.ConditionType.ConditionIf);
			}
			else if (Ref.Equal(localName, input.Atoms.Message))
			{
				compiledAction = compiler.CreateMessageAction();
			}
			else if (Ref.Equal(localName, input.Atoms.Number))
			{
				compiledAction = compiler.CreateNumberAction();
			}
			else if (Ref.Equal(localName, input.Atoms.ProcessingInstruction))
			{
				compiledAction = compiler.CreateProcessingInstructionAction();
			}
			else if (Ref.Equal(localName, input.Atoms.Text))
			{
				compiledAction = compiler.CreateTextAction();
			}
			else if (Ref.Equal(localName, input.Atoms.ValueOf))
			{
				compiledAction = compiler.CreateValueOfAction();
			}
			else if (Ref.Equal(localName, input.Atoms.Variable))
			{
				compiledAction = compiler.CreateVariableAction(VariableType.LocalVariable);
			}
			else
			{
				if (!compiler.ForwardCompatibility)
				{
					throw compiler.UnexpectedKeyword();
				}
				compiledAction = compiler.CreateNewInstructionAction();
			}
		}
		AddAction(compiledAction);
	}

	private void CompileLiteral(Compiler compiler)
	{
		switch (compiler.Input.NodeType)
		{
		case XPathNodeType.Element:
			AddEvent(compiler.CreateBeginEvent());
			CompileLiteralAttributesAndNamespaces(compiler);
			if (compiler.Recurse())
			{
				CompileTemplate(compiler);
				compiler.ToParent();
			}
			AddEvent(new EndEvent(XPathNodeType.Element));
			break;
		case XPathNodeType.Text:
		case XPathNodeType.SignificantWhitespace:
			AddEvent(compiler.CreateTextEvent());
			break;
		case XPathNodeType.Attribute:
		case XPathNodeType.Namespace:
		case XPathNodeType.Whitespace:
		case XPathNodeType.ProcessingInstruction:
		case XPathNodeType.Comment:
			break;
		}
	}

	private void CompileLiteralAttributesAndNamespaces(Compiler compiler)
	{
		NavigatorInput input = compiler.Input;
		if (input.Navigator.MoveToAttribute("use-attribute-sets", input.Atoms.UriXsl))
		{
			AddAction(compiler.CreateUseAttributeSetsAction());
			input.Navigator.MoveToParent();
		}
		compiler.InsertExcludedNamespace();
		if (input.MoveToFirstNamespace())
		{
			do
			{
				string value = input.Value;
				if (!(value == "http://www.w3.org/1999/XSL/Transform") && !compiler.IsExcludedNamespace(value) && !compiler.IsExtensionNamespace(value) && !compiler.IsNamespaceAlias(value))
				{
					AddEvent(new NamespaceEvent(input));
				}
			}
			while (input.MoveToNextNamespace());
			input.ToParent();
		}
		if (!input.MoveToFirstAttribute())
		{
			return;
		}
		do
		{
			if (!Ref.Equal(input.NamespaceURI, input.Atoms.UriXsl))
			{
				AddEvent(compiler.CreateBeginEvent());
				AddEvents(compiler.CompileAvt(input.Value));
				AddEvent(new EndEvent(XPathNodeType.Attribute));
			}
		}
		while (input.MoveToNextAttribute());
		input.ToParent();
	}

	private void CompileOutput(Compiler compiler)
	{
		compiler.RootAction.Output.Compile(compiler);
	}

	internal void AddAction(Action action)
	{
		if (containedActions == null)
		{
			containedActions = new ArrayList();
		}
		containedActions.Add(action);
		lastCopyCodeAction = null;
	}

	private void EnsureCopyCodeAction()
	{
		if (lastCopyCodeAction == null)
		{
			CopyCodeAction action = new CopyCodeAction();
			AddAction(action);
			lastCopyCodeAction = action;
		}
	}

	protected void AddEvent(Event copyEvent)
	{
		EnsureCopyCodeAction();
		lastCopyCodeAction.AddEvent(copyEvent);
	}

	protected void AddEvents(ArrayList copyEvents)
	{
		EnsureCopyCodeAction();
		lastCopyCodeAction.AddEvents(copyEvents);
	}

	internal override void Execute(Processor processor, ActionFrame frame)
	{
		switch (frame.State)
		{
		case 0:
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

	internal Action GetAction(int actionIndex)
	{
		if (containedActions != null && actionIndex < containedActions.Count)
		{
			return (Action)containedActions[actionIndex];
		}
		return null;
	}

	internal void CheckDuplicateParams(XmlQualifiedName name)
	{
		if (containedActions == null)
		{
			return;
		}
		foreach (CompiledAction containedAction in containedActions)
		{
			if (containedAction is WithParamAction withParamAction && withParamAction.Name == name)
			{
				throw XsltException.Create("Value of parameter '{0}' cannot be specified more than once within a single 'xsl:call-template' or 'xsl:apply-templates' element.", name.ToString());
			}
		}
	}

	internal override void ReplaceNamespaceAlias(Compiler compiler)
	{
		if (containedActions != null)
		{
			_ = containedActions.Count;
			for (int i = 0; i < containedActions.Count; i++)
			{
				((Action)containedActions[i]).ReplaceNamespaceAlias(compiler);
			}
		}
	}
}
