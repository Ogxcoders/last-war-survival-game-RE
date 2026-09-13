using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Xml.Xsl;

[Serializable]
public class XsltCompileException : XsltException
{
	protected XsltCompileException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
	}

	public XsltCompileException()
	{
	}

	public XsltCompileException(string message)
		: base(message)
	{
	}

	public XsltCompileException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public XsltCompileException(Exception inner, string sourceUri, int lineNumber, int linePosition)
		: base((lineNumber != 0) ? "XSLT compile error at {0}({1},{2}). See InnerException for details." : "XSLT compile error.", new string[3]
		{
			sourceUri,
			lineNumber.ToString(CultureInfo.InvariantCulture),
			linePosition.ToString(CultureInfo.InvariantCulture)
		}, sourceUri, lineNumber, linePosition, inner)
	{
	}
}
