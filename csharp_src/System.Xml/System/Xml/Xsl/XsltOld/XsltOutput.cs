using System.Collections;
using System.Text;

namespace System.Xml.Xsl.XsltOld;

internal class XsltOutput : CompiledAction
{
	internal enum OutputMethod
	{
		Xml,
		Html,
		Text,
		Other,
		Unknown
	}

	private OutputMethod method = OutputMethod.Unknown;

	private int methodSId = int.MaxValue;

	private Encoding encoding = Encoding.UTF8;

	private int encodingSId = int.MaxValue;

	private string version;

	private int versionSId = int.MaxValue;

	private bool omitXmlDecl;

	private int omitXmlDeclSId = int.MaxValue;

	private bool standalone;

	private int standaloneSId = int.MaxValue;

	private string doctypePublic;

	private int doctypePublicSId = int.MaxValue;

	private string doctypeSystem;

	private int doctypeSystemSId = int.MaxValue;

	private bool indent;

	private int indentSId = int.MaxValue;

	private string mediaType = "text/html";

	private int mediaTypeSId = int.MaxValue;

	private Hashtable cdataElements;

	internal OutputMethod Method => method;

	internal bool OmitXmlDeclaration => omitXmlDecl;

	internal bool HasStandalone => standaloneSId != int.MaxValue;

	internal bool Standalone => standalone;

	internal string DoctypePublic => doctypePublic;

	internal string DoctypeSystem => doctypeSystem;

	internal Hashtable CDataElements => cdataElements;

	internal bool Indent => indent;

	internal Encoding Encoding => encoding;

	internal string MediaType => mediaType;

	internal XsltOutput CreateDerivedOutput(OutputMethod method)
	{
		XsltOutput xsltOutput = (XsltOutput)MemberwiseClone();
		xsltOutput.method = method;
		if (method == OutputMethod.Html && indentSId == int.MaxValue)
		{
			xsltOutput.indent = true;
		}
		return xsltOutput;
	}

	internal override void Compile(Compiler compiler)
	{
		CompileAttributes(compiler);
		CheckEmpty(compiler);
	}

	internal override bool CompileAttribute(Compiler compiler)
	{
		string localName = compiler.Input.LocalName;
		string value = compiler.Input.Value;
		if (Ref.Equal(localName, compiler.Atoms.Method))
		{
			if (compiler.Stylesheetid <= methodSId)
			{
				method = ParseOutputMethod(value, compiler);
				methodSId = compiler.Stylesheetid;
				if (indentSId == int.MaxValue)
				{
					indent = method == OutputMethod.Html;
				}
			}
		}
		else if (Ref.Equal(localName, compiler.Atoms.Version))
		{
			if (compiler.Stylesheetid <= versionSId)
			{
				version = value;
				versionSId = compiler.Stylesheetid;
			}
		}
		else if (Ref.Equal(localName, compiler.Atoms.Encoding))
		{
			if (compiler.Stylesheetid <= encodingSId)
			{
				try
				{
					encoding = Encoding.GetEncoding(value);
					encodingSId = compiler.Stylesheetid;
				}
				catch (NotSupportedException)
				{
				}
				catch (ArgumentException)
				{
				}
			}
		}
		else if (Ref.Equal(localName, compiler.Atoms.OmitXmlDeclaration))
		{
			if (compiler.Stylesheetid <= omitXmlDeclSId)
			{
				omitXmlDecl = compiler.GetYesNo(value);
				omitXmlDeclSId = compiler.Stylesheetid;
			}
		}
		else if (Ref.Equal(localName, compiler.Atoms.Standalone))
		{
			if (compiler.Stylesheetid <= standaloneSId)
			{
				standalone = compiler.GetYesNo(value);
				standaloneSId = compiler.Stylesheetid;
			}
		}
		else if (Ref.Equal(localName, compiler.Atoms.DocTypePublic))
		{
			if (compiler.Stylesheetid <= doctypePublicSId)
			{
				doctypePublic = value;
				doctypePublicSId = compiler.Stylesheetid;
			}
		}
		else if (Ref.Equal(localName, compiler.Atoms.DocTypeSystem))
		{
			if (compiler.Stylesheetid <= doctypeSystemSId)
			{
				doctypeSystem = value;
				doctypeSystemSId = compiler.Stylesheetid;
			}
		}
		else if (Ref.Equal(localName, compiler.Atoms.Indent))
		{
			if (compiler.Stylesheetid <= indentSId)
			{
				indent = compiler.GetYesNo(value);
				indentSId = compiler.Stylesheetid;
			}
		}
		else if (Ref.Equal(localName, compiler.Atoms.MediaType))
		{
			if (compiler.Stylesheetid <= mediaTypeSId)
			{
				mediaType = value;
				mediaTypeSId = compiler.Stylesheetid;
			}
		}
		else
		{
			if (!Ref.Equal(localName, compiler.Atoms.CDataSectionElements))
			{
				return false;
			}
			string[] array = XmlConvert.SplitString(value);
			if (cdataElements == null)
			{
				cdataElements = new Hashtable(array.Length);
			}
			for (int i = 0; i < array.Length; i++)
			{
				XmlQualifiedName xmlQualifiedName = compiler.CreateXmlQName(array[i]);
				cdataElements[xmlQualifiedName] = xmlQualifiedName;
			}
		}
		return true;
	}

	internal override void Execute(Processor processor, ActionFrame frame)
	{
	}

	private static OutputMethod ParseOutputMethod(string value, Compiler compiler)
	{
		XmlQualifiedName xmlQualifiedName = compiler.CreateXPathQName(value);
		if (xmlQualifiedName.Namespace.Length != 0)
		{
			return OutputMethod.Other;
		}
		switch (xmlQualifiedName.Name)
		{
		case "xml":
			return OutputMethod.Xml;
		case "html":
			return OutputMethod.Html;
		case "text":
			return OutputMethod.Text;
		default:
			if (compiler.ForwardCompatibility)
			{
				return OutputMethod.Unknown;
			}
			throw XsltException.Create("'{1}' is an invalid value for the '{0}' attribute.", "method", value);
		}
	}
}
