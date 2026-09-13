using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class NewInstructionAction : ContainerAction
{
	private string name;

	private string parent;

	private bool fallback;

	internal override void Compile(Compiler compiler)
	{
		XPathNavigator xPathNavigator = compiler.Input.Navigator.Clone();
		name = xPathNavigator.Name;
		xPathNavigator.MoveToParent();
		parent = xPathNavigator.Name;
		if (compiler.Recurse())
		{
			CompileSelectiveTemplate(compiler);
			compiler.ToParent();
		}
	}

	internal void CompileSelectiveTemplate(Compiler compiler)
	{
		NavigatorInput input = compiler.Input;
		do
		{
			if (Ref.Equal(input.NamespaceURI, input.Atoms.UriXsl) && Ref.Equal(input.LocalName, input.Atoms.Fallback))
			{
				fallback = true;
				if (compiler.Recurse())
				{
					CompileTemplate(compiler);
					compiler.ToParent();
				}
			}
		}
		while (compiler.Advance());
	}

	internal override void Execute(Processor processor, ActionFrame frame)
	{
		switch (frame.State)
		{
		default:
			return;
		case 0:
			if (!fallback)
			{
				throw XsltException.Create("'{0}' is not a recognized extension element.", name);
			}
			if (containedActions != null && containedActions.Count > 0)
			{
				processor.PushActionFrame(frame);
				frame.State = 1;
				return;
			}
			break;
		case 1:
			break;
		}
		frame.Finished();
	}
}
