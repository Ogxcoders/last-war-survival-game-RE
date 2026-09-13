using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class ElementAction : ContainerAction
{
	private const int NameDone = 2;

	private Avt nameAvt;

	private Avt nsAvt;

	private bool empty;

	private InputScopeManager manager;

	private string name;

	private string nsUri;

	private PrefixQName qname;

	internal ElementAction()
	{
	}

	private static PrefixQName CreateElementQName(string name, string nsUri, InputScopeManager manager)
	{
		if (nsUri == "http://www.w3.org/2000/xmlns/")
		{
			throw XsltException.Create("Elements and attributes cannot belong to the reserved namespace '{0}'.", nsUri);
		}
		PrefixQName prefixQName = new PrefixQName();
		prefixQName.SetQName(name);
		if (nsUri == null)
		{
			prefixQName.Namespace = manager.ResolveXmlNamespace(prefixQName.Prefix);
		}
		else
		{
			prefixQName.Namespace = nsUri;
		}
		return prefixQName;
	}

	internal override void Compile(Compiler compiler)
	{
		CompileAttributes(compiler);
		CheckRequiredAttribute(compiler, nameAvt, "name");
		name = CompiledAction.PrecalculateAvt(ref nameAvt);
		nsUri = CompiledAction.PrecalculateAvt(ref nsAvt);
		if (nameAvt == null && nsAvt == null)
		{
			if (name != "xmlns")
			{
				qname = CreateElementQName(name, nsUri, compiler.CloneScopeManager());
			}
		}
		else
		{
			manager = compiler.CloneScopeManager();
		}
		if (compiler.Recurse())
		{
			CompileTemplate(compiler);
			compiler.ToParent();
		}
		empty = containedActions == null;
	}

	internal override bool CompileAttribute(Compiler compiler)
	{
		string localName = compiler.Input.LocalName;
		string value = compiler.Input.Value;
		if (Ref.Equal(localName, compiler.Atoms.Name))
		{
			nameAvt = Avt.CompileAvt(compiler, value);
		}
		else if (Ref.Equal(localName, compiler.Atoms.Namespace))
		{
			nsAvt = Avt.CompileAvt(compiler, value);
		}
		else
		{
			if (!Ref.Equal(localName, compiler.Atoms.UseAttributeSets))
			{
				return false;
			}
			AddAction(compiler.CreateUseAttributeSetsAction());
		}
		return true;
	}

	internal override void Execute(Processor processor, ActionFrame frame)
	{
		switch (frame.State)
		{
		default:
			return;
		case 0:
			if (qname != null)
			{
				frame.CalulatedName = qname;
			}
			else
			{
				frame.CalulatedName = CreateElementQName((nameAvt == null) ? name : nameAvt.Evaluate(processor, frame), (nsAvt == null) ? nsUri : nsAvt.Evaluate(processor, frame), manager);
			}
			goto case 2;
		case 2:
		{
			PrefixQName calulatedName = frame.CalulatedName;
			if (!processor.BeginEvent(XPathNodeType.Element, calulatedName.Prefix, calulatedName.Name, calulatedName.Namespace, empty))
			{
				frame.State = 2;
				return;
			}
			if (!empty)
			{
				processor.PushActionFrame(frame);
				frame.State = 1;
				return;
			}
			break;
		}
		case 1:
			break;
		}
		if (!processor.EndEvent(XPathNodeType.Element))
		{
			frame.State = 1;
		}
		else
		{
			frame.Finished();
		}
	}
}
