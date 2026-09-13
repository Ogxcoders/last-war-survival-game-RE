using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Xml.Xsl;

[Serializable]
internal class XslLoadException : XslTransformException
{
	private ISourceLineInfo lineInfo;

	public override string SourceUri
	{
		get
		{
			if (lineInfo == null)
			{
				return null;
			}
			return lineInfo.Uri;
		}
	}

	public override int LineNumber
	{
		get
		{
			if (lineInfo == null)
			{
				return 0;
			}
			return lineInfo.Start.Line;
		}
	}

	public override int LinePosition
	{
		get
		{
			if (lineInfo == null)
			{
				return 0;
			}
			return lineInfo.Start.Pos;
		}
	}

	protected XslLoadException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if ((bool)info.GetValue("hasLineInfo", typeof(bool)))
		{
			string uriString = (string)info.GetValue("Uri", typeof(string));
			int startLine = (int)info.GetValue("StartLine", typeof(int));
			int startPos = (int)info.GetValue("StartPos", typeof(int));
			int endLine = (int)info.GetValue("EndLine", typeof(int));
			int endPos = (int)info.GetValue("EndPos", typeof(int));
			lineInfo = new SourceLineInfo(uriString, startLine, startPos, endLine, endPos);
		}
	}

	[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue("hasLineInfo", lineInfo != null);
		if (lineInfo != null)
		{
			info.AddValue("Uri", lineInfo.Uri);
			info.AddValue("StartLine", lineInfo.Start.Line);
			info.AddValue("StartPos", lineInfo.Start.Pos);
			info.AddValue("EndLine", lineInfo.End.Line);
			info.AddValue("EndPos", lineInfo.End.Pos);
		}
	}

	internal XslLoadException(string res, params string[] args)
		: base(null, res, args)
	{
	}

	internal XslLoadException(Exception inner, ISourceLineInfo lineInfo)
		: base(inner, "XSLT compile error.", (string[])null)
	{
		SetSourceLineInfo(lineInfo);
	}

	internal void SetSourceLineInfo(ISourceLineInfo lineInfo)
	{
		this.lineInfo = lineInfo;
	}

	private static string AppendLineInfoMessage(string message, ISourceLineInfo lineInfo)
	{
		if (lineInfo != null)
		{
			string fileName = SourceLineInfo.GetFileName(lineInfo.Uri);
			string text = XslTransformException.CreateMessage("An error occurred at {0}({1},{2}).", fileName, lineInfo.Start.Line.ToString(CultureInfo.InvariantCulture), lineInfo.Start.Pos.ToString(CultureInfo.InvariantCulture));
			if (text != null && text.Length > 0)
			{
				if (message.Length > 0 && !XmlCharType.Instance.IsWhiteSpace(message[message.Length - 1]))
				{
					message += " ";
				}
				message += text;
			}
		}
		return message;
	}

	internal static string CreateMessage(ISourceLineInfo lineInfo, string res, params string[] args)
	{
		return AppendLineInfoMessage(XslTransformException.CreateMessage(res, args), lineInfo);
	}

	internal override string FormatDetailedMessage()
	{
		return AppendLineInfoMessage(Message, lineInfo);
	}
}
