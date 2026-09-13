using System.Resources;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Utils;

namespace System.Xml.Xsl;

[Serializable]
internal class XslTransformException : XsltException
{
	protected XslTransformException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public XslTransformException(Exception inner, string res, params string[] args)
		: base(CreateMessage(res, args), inner)
	{
	}

	public XslTransformException(string message)
		: base(CreateMessage(message, (string[])null), null)
	{
	}

	internal XslTransformException(string res, params string[] args)
		: this(null, res, args)
	{
	}

	internal static string CreateMessage(string res, params string[] args)
	{
		string text = null;
		try
		{
			text = System.Xml.Utils.Res.GetString(res, args);
		}
		catch (MissingManifestResourceException)
		{
		}
		if (text != null)
		{
			return text;
		}
		StringBuilder stringBuilder = new StringBuilder(res);
		if (args != null && args.Length != 0)
		{
			stringBuilder.Append('(');
			stringBuilder.Append(args[0]);
			for (int i = 1; i < args.Length; i++)
			{
				stringBuilder.Append(", ");
				stringBuilder.Append(args[i]);
			}
			stringBuilder.Append(')');
		}
		return stringBuilder.ToString();
	}

	internal virtual string FormatDetailedMessage()
	{
		return Message;
	}

	public override string ToString()
	{
		string text = GetType().FullName;
		string text2 = FormatDetailedMessage();
		if (text2 != null && text2.Length > 0)
		{
			text = text + ": " + text2;
		}
		if (base.InnerException != null)
		{
			text = text + " ---> " + base.InnerException.ToString() + Environment.NewLine + "   " + CreateMessage("--- End of inner exception stack trace ---");
		}
		if (StackTrace != null)
		{
			text = text + Environment.NewLine + StackTrace;
		}
		return text;
	}
}
