using System.Resources;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Xml.Schema;

[Serializable]
public class XmlSchemaException : SystemException
{
	private string res;

	private string[] args;

	private string sourceUri;

	private int lineNumber;

	private int linePosition;

	[NonSerialized]
	private XmlSchemaObject sourceSchemaObject;

	private string message;

	internal string GetRes => res;

	internal string[] Args => args;

	public string SourceUri => sourceUri;

	public int LineNumber => lineNumber;

	public int LinePosition => linePosition;

	public XmlSchemaObject SourceSchemaObject => sourceSchemaObject;

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

	protected XmlSchemaException(SerializationInfo info, StreamingContext context)
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
			message = CreateMessage(res, args);
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

	public XmlSchemaException()
		: this(null)
	{
	}

	public XmlSchemaException(string message)
		: this(message, (Exception)null, 0, 0)
	{
	}

	public XmlSchemaException(string message, Exception innerException)
		: this(message, innerException, 0, 0)
	{
	}

	public XmlSchemaException(string message, Exception innerException, int lineNumber, int linePosition)
		: this((message == null) ? "A schema error occurred." : "{0}", new string[1] { message }, innerException, null, lineNumber, linePosition, null)
	{
	}

	internal XmlSchemaException(string res, string[] args)
		: this(res, args, null, null, 0, 0, null)
	{
	}

	internal XmlSchemaException(string res, string arg)
		: this(res, new string[1] { arg }, null, null, 0, 0, null)
	{
	}

	internal XmlSchemaException(string res, string arg, string sourceUri, int lineNumber, int linePosition)
		: this(res, new string[1] { arg }, null, sourceUri, lineNumber, linePosition, null)
	{
	}

	internal XmlSchemaException(string res, string sourceUri, int lineNumber, int linePosition)
		: this(res, null, null, sourceUri, lineNumber, linePosition, null)
	{
	}

	internal XmlSchemaException(string res, string[] args, string sourceUri, int lineNumber, int linePosition)
		: this(res, args, null, sourceUri, lineNumber, linePosition, null)
	{
	}

	internal XmlSchemaException(string res, XmlSchemaObject source)
		: this(res, (string[])null, source)
	{
	}

	internal XmlSchemaException(string res, string arg, XmlSchemaObject source)
		: this(res, new string[1] { arg }, source)
	{
	}

	internal XmlSchemaException(string res, string[] args, XmlSchemaObject source)
		: this(res, args, null, source.SourceUri, source.LineNumber, source.LinePosition, source)
	{
	}

	internal XmlSchemaException(string res, string[] args, Exception innerException, string sourceUri, int lineNumber, int linePosition, XmlSchemaObject source)
		: base(CreateMessage(res, args), innerException)
	{
		base.HResult = -2146231999;
		this.res = res;
		this.args = args;
		this.sourceUri = sourceUri;
		this.lineNumber = lineNumber;
		this.linePosition = linePosition;
		sourceSchemaObject = source;
	}

	internal static string CreateMessage(string res, string[] args)
	{
		try
		{
			return Res.GetString(res, args);
		}
		catch (MissingManifestResourceException)
		{
			return "UNKNOWN(" + res + ")";
		}
	}

	internal void SetSource(string sourceUri, int lineNumber, int linePosition)
	{
		this.sourceUri = sourceUri;
		this.lineNumber = lineNumber;
		this.linePosition = linePosition;
	}

	internal void SetSchemaObject(XmlSchemaObject source)
	{
		sourceSchemaObject = source;
	}

	internal void SetSource(XmlSchemaObject source)
	{
		sourceSchemaObject = source;
		sourceUri = source.SourceUri;
		lineNumber = source.LineNumber;
		linePosition = source.LinePosition;
	}

	internal void SetResourceId(string resourceId)
	{
		res = resourceId;
	}
}
