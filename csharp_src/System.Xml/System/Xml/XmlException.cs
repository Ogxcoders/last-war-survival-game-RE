using System.Globalization;
using System.Resources;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Xml;

[Serializable]
public class XmlException : SystemException
{
	private string res;

	private string[] args;

	private int lineNumber;

	private int linePosition;

	[OptionalField]
	private string sourceUri;

	private string message;

	public int LineNumber => lineNumber;

	public int LinePosition => linePosition;

	public string SourceUri => sourceUri;

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

	internal string ResString => res;

	protected XmlException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		res = (string)info.GetValue("res", typeof(string));
		args = (string[])info.GetValue("args", typeof(string[]));
		lineNumber = (int)info.GetValue("lineNumber", typeof(int));
		linePosition = (int)info.GetValue("linePosition", typeof(int));
		sourceUri = string.Empty;
		string text = null;
		SerializationInfoEnumerator enumerator = info.GetEnumerator();
		while (enumerator.MoveNext())
		{
			SerializationEntry current = enumerator.Current;
			string name = current.Name;
			if (!(name == "sourceUri"))
			{
				if (name == "version")
				{
					text = (string)current.Value;
				}
			}
			else
			{
				sourceUri = (string)current.Value;
			}
		}
		if (text == null)
		{
			message = CreateMessage(res, args, lineNumber, linePosition);
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
		info.AddValue("lineNumber", lineNumber);
		info.AddValue("linePosition", linePosition);
		info.AddValue("sourceUri", sourceUri);
		info.AddValue("version", "2.0");
	}

	public XmlException()
		: this(null)
	{
	}

	public XmlException(string message)
		: this(message, (Exception)null, 0, 0)
	{
	}

	public XmlException(string message, Exception innerException)
		: this(message, innerException, 0, 0)
	{
	}

	public XmlException(string message, Exception innerException, int lineNumber, int linePosition)
		: this(message, innerException, lineNumber, linePosition, null)
	{
	}

	internal XmlException(string message, Exception innerException, int lineNumber, int linePosition, string sourceUri)
		: base(FormatUserMessage(message, lineNumber, linePosition), innerException)
	{
		base.HResult = -2146232000;
		res = ((message == null) ? "An XML error has occurred." : "{0}");
		args = new string[1] { message };
		this.sourceUri = sourceUri;
		this.lineNumber = lineNumber;
		this.linePosition = linePosition;
	}

	internal XmlException(string res, string[] args)
		: this(res, args, null, 0, 0, null)
	{
	}

	internal XmlException(string res, string[] args, string sourceUri)
		: this(res, args, null, 0, 0, sourceUri)
	{
	}

	internal XmlException(string res, string arg)
		: this(res, new string[1] { arg }, null, 0, 0, null)
	{
	}

	internal XmlException(string res, string arg, string sourceUri)
		: this(res, new string[1] { arg }, null, 0, 0, sourceUri)
	{
	}

	internal XmlException(string res, string arg, IXmlLineInfo lineInfo)
		: this(res, new string[1] { arg }, lineInfo, null)
	{
	}

	internal XmlException(string res, string arg, Exception innerException, IXmlLineInfo lineInfo)
		: this(res, new string[1] { arg }, innerException, lineInfo?.LineNumber ?? 0, lineInfo?.LinePosition ?? 0, null)
	{
	}

	internal XmlException(string res, string arg, IXmlLineInfo lineInfo, string sourceUri)
		: this(res, new string[1] { arg }, lineInfo, sourceUri)
	{
	}

	internal XmlException(string res, string[] args, IXmlLineInfo lineInfo)
		: this(res, args, lineInfo, null)
	{
	}

	internal XmlException(string res, string[] args, IXmlLineInfo lineInfo, string sourceUri)
		: this(res, args, null, lineInfo?.LineNumber ?? 0, lineInfo?.LinePosition ?? 0, sourceUri)
	{
	}

	internal XmlException(string res, int lineNumber, int linePosition)
		: this(res, null, null, lineNumber, linePosition)
	{
	}

	internal XmlException(string res, string arg, int lineNumber, int linePosition)
		: this(res, new string[1] { arg }, null, lineNumber, linePosition, null)
	{
	}

	internal XmlException(string res, string arg, int lineNumber, int linePosition, string sourceUri)
		: this(res, new string[1] { arg }, null, lineNumber, linePosition, sourceUri)
	{
	}

	internal XmlException(string res, string[] args, int lineNumber, int linePosition)
		: this(res, args, null, lineNumber, linePosition, null)
	{
	}

	internal XmlException(string res, string[] args, int lineNumber, int linePosition, string sourceUri)
		: this(res, args, null, lineNumber, linePosition, sourceUri)
	{
	}

	internal XmlException(string res, string[] args, Exception innerException, int lineNumber, int linePosition)
		: this(res, args, innerException, lineNumber, linePosition, null)
	{
	}

	internal XmlException(string res, string[] args, Exception innerException, int lineNumber, int linePosition, string sourceUri)
		: base(CreateMessage(res, args, lineNumber, linePosition), innerException)
	{
		base.HResult = -2146232000;
		this.res = res;
		this.args = args;
		this.sourceUri = sourceUri;
		this.lineNumber = lineNumber;
		this.linePosition = linePosition;
	}

	private static string FormatUserMessage(string message, int lineNumber, int linePosition)
	{
		if (message == null)
		{
			return CreateMessage("An XML error has occurred.", null, lineNumber, linePosition);
		}
		if (lineNumber == 0 && linePosition == 0)
		{
			return message;
		}
		return CreateMessage("{0}", new string[1] { message }, lineNumber, linePosition);
	}

	private static string CreateMessage(string res, string[] args, int lineNumber, int linePosition)
	{
		try
		{
			string result;
			if (lineNumber == 0)
			{
				object[] array = args;
				result = Res.GetString(res, array);
			}
			else
			{
				string text = lineNumber.ToString(CultureInfo.InvariantCulture);
				string text2 = linePosition.ToString(CultureInfo.InvariantCulture);
				object[] array = args;
				result = Res.GetString(res, array);
				array = new string[3] { result, text, text2 };
				result = Res.GetString("{0} Line {1}, position {2}.", array);
			}
			return result;
		}
		catch (MissingManifestResourceException)
		{
			return "UNKNOWN(" + res + ")";
		}
	}

	internal static string[] BuildCharExceptionArgs(string data, int invCharIndex)
	{
		return BuildCharExceptionArgs(data[invCharIndex], (invCharIndex + 1 < data.Length) ? data[invCharIndex + 1] : '\0');
	}

	internal static string[] BuildCharExceptionArgs(char[] data, int invCharIndex)
	{
		return BuildCharExceptionArgs(data, data.Length, invCharIndex);
	}

	internal static string[] BuildCharExceptionArgs(char[] data, int length, int invCharIndex)
	{
		return BuildCharExceptionArgs(data[invCharIndex], (invCharIndex + 1 < length) ? data[invCharIndex + 1] : '\0');
	}

	internal static string[] BuildCharExceptionArgs(char invChar, char nextChar)
	{
		string[] array = new string[2];
		if (XmlCharType.IsHighSurrogate(invChar) && nextChar != 0)
		{
			int num = XmlCharType.CombineSurrogateChar(nextChar, invChar);
			array[0] = new string(new char[2] { invChar, nextChar });
			array[1] = string.Format(CultureInfo.InvariantCulture, "0x{0:X2}", num);
		}
		else
		{
			if (invChar == '\0')
			{
				array[0] = ".";
			}
			else
			{
				array[0] = invChar.ToString(CultureInfo.InvariantCulture);
			}
			array[1] = string.Format(CultureInfo.InvariantCulture, "0x{0:X2}", (int)invChar);
		}
		return array;
	}

	internal static bool IsCatchableException(Exception e)
	{
		if (!(e is StackOverflowException) && !(e is OutOfMemoryException) && !(e is ThreadAbortException) && !(e is ThreadInterruptedException) && !(e is NullReferenceException))
		{
			return !(e is AccessViolationException);
		}
		return false;
	}
}
