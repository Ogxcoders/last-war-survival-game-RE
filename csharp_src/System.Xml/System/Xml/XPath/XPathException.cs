using System.Resources;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Xml.XPath;

[Serializable]
public class XPathException : SystemException
{
	private string res;

	private string[] args;

	private string message;

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

	protected XPathException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		res = (string)info.GetValue("res", typeof(string));
		args = (string[])info.GetValue("args", typeof(string[]));
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
		info.AddValue("version", "2.0");
	}

	public XPathException()
		: this(string.Empty, (Exception)null)
	{
	}

	public XPathException(string message)
		: this(message, (Exception)null)
	{
	}

	public XPathException(string message, Exception innerException)
		: this("{0}", new string[1] { message }, innerException)
	{
	}

	internal static XPathException Create(string res)
	{
		return new XPathException(res, (string[])null);
	}

	internal static XPathException Create(string res, string arg)
	{
		return new XPathException(res, new string[1] { arg });
	}

	internal static XPathException Create(string res, string arg, string arg2)
	{
		return new XPathException(res, new string[2] { arg, arg2 });
	}

	internal static XPathException Create(string res, string arg, Exception innerException)
	{
		return new XPathException(res, new string[1] { arg }, innerException);
	}

	private XPathException(string res, string[] args)
		: this(res, args, null)
	{
	}

	private XPathException(string res, string[] args, Exception inner)
		: base(CreateMessage(res, args), inner)
	{
		base.HResult = -2146231997;
		this.res = res;
		this.args = args;
	}

	private static string CreateMessage(string res, string[] args)
	{
		try
		{
			string text = Res.GetString(res, args);
			if (text == null)
			{
				text = "UNKNOWN(" + res + ")";
			}
			return text;
		}
		catch (MissingManifestResourceException)
		{
			return "UNKNOWN(" + res + ")";
		}
	}
}
