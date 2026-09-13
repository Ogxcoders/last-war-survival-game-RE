using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class AttributeAction : ContainerAction
{
	private const int NameDone = 2;

	private Avt nameAvt;

	private Avt nsAvt;

	private InputScopeManager manager;

	private string name;

	private string nsUri;

	private PrefixQName qname;

	private static PrefixQName CreateAttributeQName(string name, string nsUri, InputScopeManager manager)
	{
		if (name == "xmlns")
		{
			return null;
		}
		if (nsUri == "http://www.w3.org/2000/xmlns/")
		{
			throw XsltException.Create("Elements and attributes cannot belong to the reserved namespace '{0}'.", nsUri);
		}
		PrefixQName prefixQName = new PrefixQName();
		prefixQName.SetQName(name);
		prefixQName.Namespace = ((nsUri != null) ? nsUri : manager.ResolveXPathNamespace(prefixQName.Prefix));
		if (prefixQName.Prefix.StartsWith("xml", StringComparison.Ordinal))
		{
			if (prefixQName.Prefix.Length == 3)
			{
				if (!(prefixQName.Namespace == "http://www.w3.org/XML/1998/namespace") || (!(prefixQName.Name == "lang") && !(prefixQName.Name == "space")))
				{
					prefixQName.ClearPrefix();
				}
			}
			else if (prefixQName.Prefix == "xmlns")
			{
				if (prefixQName.Namespace == "http://www.w3.org/2000/xmlns/")
				{
					throw XsltException.Create("Prefix '{0}' is not defined.", prefixQName.Prefix);
				}
				prefixQName.ClearPrefix();
			}
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
				qname = CreateAttributeQName(name, nsUri, compiler.CloneScopeManager());
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
	}

	internal override bool CompileAttribute(Compiler compiler)
	{
		string localName = compiler.Input.LocalName;
		string value = compiler.Input.Value;
		if (Ref.Equal(localName, compiler.Atoms.Name))
		{
			nameAvt = Avt.CompileAvt(compiler, value);
		}
		else
		{
			if (!Ref.Equal(localName, compiler.Atoms.Namespace))
			{
				return false;
			}
			nsAvt = Avt.CompileAvt(compiler, value);
		}
		return true;
	}

	internal override void Execute(Processor processor, ActionFrame frame)
	{
		switch (frame.State)
		{
		case 0:
			if (qname != null)
			{
				frame.CalulatedName = qname;
			}
			else
			{
				frame.CalulatedName = CreateAttributeQName((nameAvt == null) ? name : nameAvt.Evaluate(processor, frame), (nsAvt == null) ? nsUri : nsAvt.Evaluate(processor, frame), manager);
				if (frame.CalulatedName == null)
				{
					frame.Finished();
					break;
				}
			}
			goto case 2;
		case 2:
		{
			PrefixQName calulatedName = frame.CalulatedName;
			if (!processor.BeginEvent(XPathNodeType.Attribute, calulatedName.Prefix, calulatedName.Name, calulatedName.Namespace, empty: false))
			{
				frame.State = 2;
				break;
			}
			processor.PushActionFrame(frame);
			frame.State = 1;
			break;
		}
		case 1:
			if (!processor.EndEvent(XPathNodeType.Attribute))
			{
				frame.State = 1;
			}
			else
			{
				frame.Finished();
			}
			break;
		}
	}
}
