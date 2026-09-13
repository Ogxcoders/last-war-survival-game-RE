using System.IO;
using System.Reflection;
using System.Xml.XPath;
using System.Xml.XmlConfiguration;

namespace System.Xml.Xsl;

public sealed class XslCompiledTransform
{
	private readonly bool enable_debug;

	private object debugger;

	private XmlWriterSettings output_settings = new XmlWriterSettings();

	private XslTransform impl = new XslTransform();

	[System.MonoTODO]
	public XmlWriterSettings OutputSettings => output_settings;

	public XslCompiledTransform()
		: this(enableDebug: false)
	{
	}

	public XslCompiledTransform(bool enableDebug)
	{
		enable_debug = enableDebug;
		if (enable_debug)
		{
			debugger = new NoOperationDebugger();
		}
		output_settings.ConformanceLevel = ConformanceLevel.Fragment;
	}

	public void Transform(string inputUri, string resultsFile)
	{
		using Stream results = File.Create(resultsFile);
		Transform(new XPathDocument(inputUri, XmlSpace.Preserve), null, results);
	}

	public void Transform(string inputUri, XmlWriter results)
	{
		Transform(inputUri, null, results);
	}

	public void Transform(string inputUri, XsltArgumentList arguments, Stream results)
	{
		Transform(new XPathDocument(inputUri, XmlSpace.Preserve), arguments, results);
	}

	public void Transform(string inputUri, XsltArgumentList arguments, TextWriter results)
	{
		Transform(new XPathDocument(inputUri, XmlSpace.Preserve), arguments, results);
	}

	public void Transform(string inputUri, XsltArgumentList arguments, XmlWriter results)
	{
		Transform(new XPathDocument(inputUri, XmlSpace.Preserve), arguments, results);
	}

	public void Transform(XmlReader input, XmlWriter results)
	{
		Transform(input, null, results);
	}

	public void Transform(XmlReader input, XsltArgumentList arguments, Stream results)
	{
		Transform(new XPathDocument(input, XmlSpace.Preserve), arguments, results);
	}

	public void Transform(XmlReader input, XsltArgumentList arguments, TextWriter results)
	{
		Transform(new XPathDocument(input, XmlSpace.Preserve), arguments, results);
	}

	public void Transform(XmlReader input, XsltArgumentList arguments, XmlWriter results)
	{
		Transform(input, arguments, results, null);
	}

	public void Transform(IXPathNavigable input, XsltArgumentList arguments, TextWriter results)
	{
		Transform(input.CreateNavigator(), arguments, results);
	}

	public void Transform(IXPathNavigable input, XsltArgumentList arguments, Stream results)
	{
		using StreamWriter output = new StreamWriter(results);
		Transform(input.CreateNavigator(), arguments, output);
	}

	public void Transform(IXPathNavigable input, XmlWriter results)
	{
		Transform(input, null, results);
	}

	public void Transform(IXPathNavigable input, XsltArgumentList arguments, XmlWriter results)
	{
		Transform(input.CreateNavigator(), arguments, results, null);
	}

	public void Transform(IXPathNavigable input, XsltArgumentList arguments, XmlWriter results, XmlResolver documentResolver)
	{
		Transform(input.CreateNavigator(), arguments, results, documentResolver);
	}

	public void Transform(XmlReader input, XsltArgumentList arguments, XmlWriter results, XmlResolver documentResolver)
	{
		Transform(new XPathDocument(input, XmlSpace.Preserve).CreateNavigator(), arguments, results, documentResolver);
	}

	private void Transform(XPathNavigator input, XsltArgumentList args, XmlWriter output, XmlResolver resolver)
	{
		impl.Transform(input, args, output, resolver);
	}

	private void Transform(XPathNavigator input, XsltArgumentList args, TextWriter output)
	{
		impl.Transform(input, args, output);
	}

	private XmlReader GetXmlReader(string url)
	{
		XmlResolver xmlResolver = new XmlUrlResolver();
		Uri uri = xmlResolver.ResolveUri(null, url);
		Stream input = xmlResolver.GetEntity(uri, null, typeof(Stream)) as Stream;
		return new XmlValidatingReader(new XmlTextReader(uri.ToString(), input)
		{
			XmlResolver = xmlResolver
		})
		{
			XmlResolver = xmlResolver,
			ValidationType = ValidationType.None
		};
	}

	public void Load(string stylesheetUri)
	{
		using XmlReader stylesheet = GetXmlReader(stylesheetUri);
		Load(stylesheet);
	}

	public void Load(XmlReader stylesheet)
	{
		Load(stylesheet, XsltSettings.Default, XsltConfigSection.CreateDefaultResolver());
	}

	public void Load(IXPathNavigable stylesheet)
	{
		Load(stylesheet.CreateNavigator(), XsltSettings.Default, XsltConfigSection.CreateDefaultResolver());
	}

	public void Load(IXPathNavigable stylesheet, XsltSettings settings, XmlResolver stylesheetResolver)
	{
		if (settings.EnableScript)
		{
			throw new NotSupportedException("'msxsl:script' element is not supported on this framework because it does not support run-time code generation");
		}
		impl.Load(stylesheet, stylesheetResolver);
	}

	public void Load(XmlReader stylesheet, XsltSettings settings, XmlResolver stylesheetResolver)
	{
		Load(new XPathDocument(stylesheet, XmlSpace.Preserve), settings, stylesheetResolver);
	}

	public void Load(string stylesheetUri, XsltSettings settings, XmlResolver stylesheetResolver)
	{
		Load(new XPathDocument(stylesheetUri, XmlSpace.Preserve).CreateNavigator(), settings, stylesheetResolver);
	}

	public void Load(MethodInfo executeMethod, byte[] queryData, Type[] earlyBoundTypes)
	{
		throw new NotImplementedException();
	}

	public void Load(Type compiledStylesheet)
	{
		throw new NotImplementedException();
	}
}
