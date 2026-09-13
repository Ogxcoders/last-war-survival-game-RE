using System.Globalization;
using System.Resources;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Utils;

namespace System.Xml.Xsl;

[Serializable]
public class XsltException : SystemException
{
	private string res;

	private string[] args;

	private string sourceUri;

	private int lineNumber;

	private int linePosition;

	private string message;

	public virtual string SourceUri => sourceUri;

	public virtual int LineNumber => lineNumber;

	public virtual int LinePosition => linePosition;

	public override string Message
	{
		get
		{
			if (message != null)
			{
				return message;
			}
			return base.Message;
		}
	}

	protected XsltException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		res = (string)info.GetValue("res", typeof(string));
		args = (string[])info.GetValue("args", typeof(string[]));
		sourceUri = (string)info.GetValue("sourceUri", typeof(string));
		lineNumber = (int)info.GetValue("lineNumber", typeof(int));
		linePosition = (int)info.GetValue("linePosition", typeof(int));
		string text = null;
		SerializationInfoEnumerator enumerator = info.GetEnumerator();
		while (enumerator.MoveNext())
		{
			SerializationEntry current = enumerator.Current;
			if (current.Name == "version")
			{
				text = (string)current.Value;
			}
		}
		if (text == null)
		{
			message = CreateMessage(res, args, sourceUri, lineNumber, linePosition);
		}
		else
		{
			message = null;
		}
	}

	[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue("res", res);
		info.AddValue("args", args);
		info.AddValue("sourceUri", sourceUri);
		info.AddValue("lineNumber", lineNumber);
		info.AddValue("linePosition", linePosition);
		info.AddValue("version", "2.0");
	}

	public XsltException()
		: this(string.Empty, null)
	{
	}

	public XsltException(string message)
		: this(message, null)
	{
	}

	public XsltException(string message, Exception innerException)
		: this("{0}", new string[1] { message }, null, 0, 0, innerException)
	{
	}

	internal static XsltException Create(string res, params string[] args)
	{
		return new XsltException(res, args, null, 0, 0, null);
	}

	internal static XsltException Create(string res, string[] args, Exception inner)
	{
		return new XsltException(res, args, null, 0, 0, inner);
	}

	internal XsltException(string res, string[] args, string sourceUri, int lineNumber, int linePosition, Exception inner)
		: base(CreateMessage(res, args, sourceUri, lineNumber, linePosition), inner)
	{
		base.HResult = -2146231998;
		this.res = res;
		this.sourceUri = sourceUri;
		this.lineNumber = lineNumber;
		this.linePosition = linePosition;
	}

	private static string CreateMessage(string res, string[] args, string sourceUri, int lineNumber, int linePosition)
	{
		try
		{
			string text = FormatMessage(res, args);
			if (res != "XSLT compile error at {0}({1},{2}). See InnerException for details." && lineNumber != 0)
			{
				text = text + " " + FormatMessage("An error occurred at {0}({1},{2}).", sourceUri, lineNumber.ToString(CultureInfo.InvariantCulture), linePosition.ToString(CultureInfo.InvariantCulture));
			}
			return text;
		}
		catch (MissingManifestResourceException)
		{
			return "UNKNOWN(" + res + ")";
		}
	}

	private static string FormatMessage(string key, params string[] args)
	{
		string text = System.Xml.Utils.Res.GetString(key);
		if (text != null && args != null)
		{
			text = string.Format(CultureInfo.InvariantCulture, text, args);
		}
		return text;
	}
}
