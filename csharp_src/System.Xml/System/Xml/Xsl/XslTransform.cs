using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Xml.XPath;
using System.Xml.XmlConfiguration;
using System.Xml.Xsl.XsltOld;
using System.Xml.Xsl.XsltOld.Debugger;

namespace System.Xml.Xsl;

[Obsolete("This class has been deprecated. Please use System.Xml.Xsl.XslCompiledTransform instead. http://go.microsoft.com/fwlink/?linkid=14202")]
public sealed class XslTransform
{
	private class DebuggerAddapter : IXsltDebugger
	{
		private object unknownDebugger;

		private MethodInfo getBltIn;

		private MethodInfo onCompile;

		private MethodInfo onExecute;

		public DebuggerAddapter(object unknownDebugger)
		{
			this.unknownDebugger = unknownDebugger;
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			Type type = unknownDebugger.GetType();
			getBltIn = type.GetMethod("GetBuiltInTemplatesUri", bindingAttr);
			onCompile = type.GetMethod("OnInstructionCompile", bindingAttr);
			onExecute = type.GetMethod("OnInstructionExecute", bindingAttr);
		}

		public string GetBuiltInTemplatesUri()
		{
			if (getBltIn == null)
			{
				return null;
			}
			return (string)getBltIn.Invoke(unknownDebugger, new object[0]);
		}

		public void OnInstructionCompile(XPathNavigator styleSheetNavigator)
		{
			if (onCompile != null)
			{
				onCompile.Invoke(unknownDebugger, new object[1] { styleSheetNavigator });
			}
		}

		public void OnInstructionExecute(IXsltProcessor xsltProcessor)
		{
			if (onExecute != null)
			{
				onExecute.Invoke(unknownDebugger, new object[1] { xsltProcessor });
			}
		}
	}

	private XmlResolver _documentResolver;

	private bool isDocumentResolverSet;

	private Stylesheet _CompiledStylesheet;

	private List<TheQuery> _QueryStore;

	private RootAction _RootAction;

	private IXsltDebugger debugger;

	private XmlResolver _DocumentResolver
	{
		get
		{
			if (isDocumentResolverSet)
			{
				return _documentResolver;
			}
			return XsltConfigSection.CreateDefaultResolver();
		}
	}

	public XmlResolver XmlResolver
	{
		set
		{
			_documentResolver = value;
			isDocumentResolverSet = true;
		}
	}

	internal IXsltDebugger Debugger => debugger;

	public XslTransform()
	{
	}

	public void Load(XmlReader stylesheet)
	{
		Load(stylesheet, XsltConfigSection.CreateDefaultResolver());
	}

	public void Load(XmlReader stylesheet, XmlResolver resolver)
	{
		Load(new XPathDocument(stylesheet, XmlSpace.Preserve), resolver);
	}

	public void Load(IXPathNavigable stylesheet)
	{
		Load(stylesheet, XsltConfigSection.CreateDefaultResolver());
	}

	public void Load(IXPathNavigable stylesheet, XmlResolver resolver)
	{
		if (stylesheet == null)
		{
			throw new ArgumentNullException("stylesheet");
		}
		Load(stylesheet.CreateNavigator(), resolver);
	}

	public void Load(XPathNavigator stylesheet)
	{
		if (stylesheet == null)
		{
			throw new ArgumentNullException("stylesheet");
		}
		Load(stylesheet, XsltConfigSection.CreateDefaultResolver());
	}

	public void Load(XPathNavigator stylesheet, XmlResolver resolver)
	{
		if (stylesheet == null)
		{
			throw new ArgumentNullException("stylesheet");
		}
		Compile(stylesheet, resolver, null);
	}

	public void Load(string url)
	{
		XmlTextReaderImpl xmlTextReaderImpl = new XmlTextReaderImpl(url);
		Evidence evidence = XmlSecureResolver.CreateEvidenceForUrl(xmlTextReaderImpl.BaseURI);
		Compile(Compiler.LoadDocument(xmlTextReaderImpl).CreateNavigator(), XsltConfigSection.CreateDefaultResolver(), evidence);
	}

	public void Load(string url, XmlResolver resolver)
	{
		XmlTextReaderImpl xmlTextReaderImpl = new XmlTextReaderImpl(url);
		xmlTextReaderImpl.XmlResolver = resolver;
		Evidence evidence = XmlSecureResolver.CreateEvidenceForUrl(xmlTextReaderImpl.BaseURI);
		Compile(Compiler.LoadDocument(xmlTextReaderImpl).CreateNavigator(), resolver, evidence);
	}

	public void Load(IXPathNavigable stylesheet, XmlResolver resolver, Evidence evidence)
	{
		if (stylesheet == null)
		{
			throw new ArgumentNullException("stylesheet");
		}
		Load(stylesheet.CreateNavigator(), resolver, evidence);
	}

	public void Load(XmlReader stylesheet, XmlResolver resolver, Evidence evidence)
	{
		if (stylesheet == null)
		{
			throw new ArgumentNullException("stylesheet");
		}
		Load(new XPathDocument(stylesheet, XmlSpace.Preserve), resolver, evidence);
	}

	public void Load(XPathNavigator stylesheet, XmlResolver resolver, Evidence evidence)
	{
		if (stylesheet == null)
		{
			throw new ArgumentNullException("stylesheet");
		}
		Compile(stylesheet, resolver, evidence);
	}

	private void CheckCommand()
	{
		if (_CompiledStylesheet == null)
		{
			throw new InvalidOperationException(Res.GetString("No stylesheet was loaded."));
		}
	}

	public XmlReader Transform(XPathNavigator input, XsltArgumentList args, XmlResolver resolver)
	{
		CheckCommand();
		return new Processor(input, args, resolver, _CompiledStylesheet, _QueryStore, _RootAction, debugger).StartReader();
	}

	public XmlReader Transform(XPathNavigator input, XsltArgumentList args)
	{
		return Transform(input, args, _DocumentResolver);
	}

	public void Transform(XPathNavigator input, XsltArgumentList args, XmlWriter output, XmlResolver resolver)
	{
		CheckCommand();
		new Processor(input, args, resolver, _CompiledStylesheet, _QueryStore, _RootAction, debugger).Execute(output);
	}

	public void Transform(XPathNavigator input, XsltArgumentList args, XmlWriter output)
	{
		Transform(input, args, output, _DocumentResolver);
	}

	public void Transform(XPathNavigator input, XsltArgumentList args, Stream output, XmlResolver resolver)
	{
		CheckCommand();
		new Processor(input, args, resolver, _CompiledStylesheet, _QueryStore, _RootAction, debugger).Execute(output);
	}

	public void Transform(XPathNavigator input, XsltArgumentList args, Stream output)
	{
		Transform(input, args, output, _DocumentResolver);
	}

	public void Transform(XPathNavigator input, XsltArgumentList args, TextWriter output, XmlResolver resolver)
	{
		CheckCommand();
		new Processor(input, args, resolver, _CompiledStylesheet, _QueryStore, _RootAction, debugger).Execute(output);
	}

	public void Transform(XPathNavigator input, XsltArgumentList args, TextWriter output)
	{
		CheckCommand();
		new Processor(input, args, _DocumentResolver, _CompiledStylesheet, _QueryStore, _RootAction, debugger).Execute(output);
	}

	public XmlReader Transform(IXPathNavigable input, XsltArgumentList args, XmlResolver resolver)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		return Transform(input.CreateNavigator(), args, resolver);
	}

	public XmlReader Transform(IXPathNavigable input, XsltArgumentList args)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		return Transform(input.CreateNavigator(), args, _DocumentResolver);
	}

	public void Transform(IXPathNavigable input, XsltArgumentList args, TextWriter output, XmlResolver resolver)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		Transform(input.CreateNavigator(), args, output, resolver);
	}

	public void Transform(IXPathNavigable input, XsltArgumentList args, TextWriter output)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		Transform(input.CreateNavigator(), args, output, _DocumentResolver);
	}

	public void Transform(IXPathNavigable input, XsltArgumentList args, Stream output, XmlResolver resolver)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		Transform(input.CreateNavigator(), args, output, resolver);
	}

	public void Transform(IXPathNavigable input, XsltArgumentList args, Stream output)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		Transform(input.CreateNavigator(), args, output, _DocumentResolver);
	}

	public void Transform(IXPathNavigable input, XsltArgumentList args, XmlWriter output, XmlResolver resolver)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		Transform(input.CreateNavigator(), args, output, resolver);
	}

	public void Transform(IXPathNavigable input, XsltArgumentList args, XmlWriter output)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		Transform(input.CreateNavigator(), args, output, _DocumentResolver);
	}

	public void Transform(string inputfile, string outputfile, XmlResolver resolver)
	{
		FileStream fileStream = null;
		try
		{
			XPathDocument input = new XPathDocument(inputfile);
			fileStream = new FileStream(outputfile, FileMode.Create, FileAccess.ReadWrite);
			Transform(input, null, fileStream, resolver);
		}
		finally
		{
			fileStream?.Close();
		}
	}

	public void Transform(string inputfile, string outputfile)
	{
		Transform(inputfile, outputfile, _DocumentResolver);
	}

	private void Compile(XPathNavigator stylesheet, XmlResolver resolver, Evidence evidence)
	{
		Compiler compiler = ((Debugger == null) ? new Compiler() : new DbgCompiler(Debugger));
		NavigatorInput input = new NavigatorInput(stylesheet);
		compiler.Compile(input, resolver ?? XmlNullResolver.Singleton, evidence);
		_CompiledStylesheet = compiler.CompiledStylesheet;
		_QueryStore = compiler.QueryStore;
		_RootAction = compiler.RootAction;
	}

	internal XslTransform(object debugger)
	{
		if (debugger != null)
		{
			this.debugger = new DebuggerAddapter(debugger);
		}
	}
}
