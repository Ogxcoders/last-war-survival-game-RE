using System.Text;
using System.Threading.Tasks;

namespace System.Xml;

internal class XmlCharCheckingWriter : XmlWrappingWriter
{
	private bool checkValues;

	private bool checkNames;

	private bool replaceNewLines;

	private string newLineChars;

	private XmlCharType xmlCharType;

	public override XmlWriterSettings Settings
	{
		get
		{
			XmlWriterSettings settings = writer.Settings;
			settings = ((settings != null) ? settings.Clone() : new XmlWriterSettings());
			if (checkValues)
			{
				settings.CheckCharacters = true;
			}
			if (replaceNewLines)
			{
				settings.NewLineHandling = NewLineHandling.Replace;
				settings.NewLineChars = newLineChars;
			}
			settings.ReadOnly = true;
			return settings;
		}
	}

	internal XmlCharCheckingWriter(XmlWriter baseWriter, bool checkValues, bool checkNames, bool replaceNewLines, string newLineChars)
		: base(baseWriter)
	{
		this.checkValues = checkValues;
		this.checkNames = checkNames;
		this.replaceNewLines = replaceNewLines;
		this.newLineChars = newLineChars;
		if (checkValues)
		{
			xmlCharType = XmlCharType.Instance;
		}
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		if (checkNames)
		{
			ValidateQName(name);
		}
		if (checkValues)
		{
			int invCharPos;
			if (pubid != null && (invCharPos = xmlCharType.IsPublicId(pubid)) >= 0)
			{
				throw XmlConvert.CreateInvalidCharException(pubid, invCharPos);
			}
			if (sysid != null)
			{
				CheckCharacters(sysid);
			}
			if (subset != null)
			{
				CheckCharacters(subset);
			}
		}
		if (replaceNewLines)
		{
			sysid = ReplaceNewLines(sysid);
			pubid = ReplaceNewLines(pubid);
			subset = ReplaceNewLines(subset);
		}
		writer.WriteDocType(name, pubid, sysid, subset);
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		if (checkNames)
		{
			if (localName == null || localName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
			}
			ValidateNCName(localName);
			if (prefix != null && prefix.Length > 0)
			{
				ValidateNCName(prefix);
			}
		}
		writer.WriteStartElement(prefix, localName, ns);
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		if (checkNames)
		{
			if (localName == null || localName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
			}
			ValidateNCName(localName);
			if (prefix != null && prefix.Length > 0)
			{
				ValidateNCName(prefix);
			}
		}
		writer.WriteStartAttribute(prefix, localName, ns);
	}

	public override void WriteCData(string text)
	{
		if (text != null)
		{
			if (checkValues)
			{
				CheckCharacters(text);
			}
			if (replaceNewLines)
			{
				text = ReplaceNewLines(text);
			}
			int num;
			while ((num = text.IndexOf("]]>", StringComparison.Ordinal)) >= 0)
			{
				writer.WriteCData(text.Substring(0, num + 2));
				text = text.Substring(num + 2);
			}
		}
		writer.WriteCData(text);
	}

	public override void WriteComment(string text)
	{
		if (text != null)
		{
			if (checkValues)
			{
				CheckCharacters(text);
				text = InterleaveInvalidChars(text, '-', '-');
			}
			if (replaceNewLines)
			{
				text = ReplaceNewLines(text);
			}
		}
		writer.WriteComment(text);
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
		if (checkNames)
		{
			ValidateNCName(name);
		}
		if (text != null)
		{
			if (checkValues)
			{
				CheckCharacters(text);
				text = InterleaveInvalidChars(text, '?', '>');
			}
			if (replaceNewLines)
			{
				text = ReplaceNewLines(text);
			}
		}
		writer.WriteProcessingInstruction(name, text);
	}

	public override void WriteEntityRef(string name)
	{
		if (checkNames)
		{
			ValidateQName(name);
		}
		writer.WriteEntityRef(name);
	}

	public override void WriteWhitespace(string ws)
	{
		if (ws == null)
		{
			ws = string.Empty;
		}
		int invCharIndex;
		if (checkNames && (invCharIndex = xmlCharType.IsOnlyWhitespaceWithPos(ws)) != -1)
		{
			object[] args = XmlException.BuildCharExceptionArgs(ws, invCharIndex);
			throw new ArgumentException(Res.GetString("The Whitespace or SignificantWhitespace node can contain only XML white space characters. '{0}' is not an XML white space character.", args));
		}
		if (replaceNewLines)
		{
			ws = ReplaceNewLines(ws);
		}
		writer.WriteWhitespace(ws);
	}

	public override void WriteString(string text)
	{
		if (text != null)
		{
			if (checkValues)
			{
				CheckCharacters(text);
			}
			if (replaceNewLines && WriteState != WriteState.Attribute)
			{
				text = ReplaceNewLines(text);
			}
		}
		writer.WriteString(text);
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		writer.WriteSurrogateCharEntity(lowChar, highChar);
	}

	public override void WriteChars(char[] buffer, int index, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (count > buffer.Length - index)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (checkValues)
		{
			CheckCharacters(buffer, index, count);
		}
		if (replaceNewLines && WriteState != WriteState.Attribute)
		{
			string text = ReplaceNewLines(buffer, index, count);
			if (text != null)
			{
				WriteString(text);
				return;
			}
		}
		writer.WriteChars(buffer, index, count);
	}

	public override void WriteNmToken(string name)
	{
		if (checkNames)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
			}
			XmlConvert.VerifyNMTOKEN(name);
		}
		writer.WriteNmToken(name);
	}

	public override void WriteName(string name)
	{
		if (checkNames)
		{
			XmlConvert.VerifyQName(name, ExceptionType.XmlException);
		}
		writer.WriteName(name);
	}

	public override void WriteQualifiedName(string localName, string ns)
	{
		if (checkNames)
		{
			ValidateNCName(localName);
		}
		writer.WriteQualifiedName(localName, ns);
	}

	private void CheckCharacters(string str)
	{
		XmlConvert.VerifyCharData(str, ExceptionType.ArgumentException);
	}

	private void CheckCharacters(char[] data, int offset, int len)
	{
		XmlConvert.VerifyCharData(data, offset, len, ExceptionType.ArgumentException);
	}

	private void ValidateNCName(string ncname)
	{
		if (ncname.Length == 0)
		{
			throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
		}
		int num = ValidateNames.ParseNCName(ncname, 0);
		if (num != ncname.Length)
		{
			string name = ((num == 0) ? "Name cannot begin with the '{0}' character, hexadecimal value {1}." : "The '{0}' character, hexadecimal value {1}, cannot be included in a name.");
			object[] args = XmlException.BuildCharExceptionArgs(ncname, num);
			throw new ArgumentException(Res.GetString(name, args));
		}
	}

	private void ValidateQName(string name)
	{
		if (name.Length == 0)
		{
			throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
		}
		int colonOffset;
		int num = ValidateNames.ParseQName(name, 0, out colonOffset);
		if (num != name.Length)
		{
			string name2 = ((num == 0 || (colonOffset > -1 && num == colonOffset + 1)) ? "Name cannot begin with the '{0}' character, hexadecimal value {1}." : "The '{0}' character, hexadecimal value {1}, cannot be included in a name.");
			object[] args = XmlException.BuildCharExceptionArgs(name, num);
			throw new ArgumentException(Res.GetString(name2, args));
		}
	}

	private string ReplaceNewLines(string str)
	{
		if (str == null)
		{
			return null;
		}
		StringBuilder stringBuilder = null;
		int num = 0;
		int i;
		for (i = 0; i < str.Length; i++)
		{
			char c;
			if ((c = str[i]) >= ' ')
			{
				continue;
			}
			if (c == '\n')
			{
				if (newLineChars == "\n")
				{
					continue;
				}
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(str.Length + 5);
				}
				stringBuilder.Append(str, num, i - num);
			}
			else
			{
				if (c != '\r')
				{
					continue;
				}
				if (i + 1 < str.Length && str[i + 1] == '\n')
				{
					if (newLineChars == "\r\n")
					{
						i++;
						continue;
					}
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(str.Length + 5);
					}
					stringBuilder.Append(str, num, i - num);
					i++;
				}
				else
				{
					if (newLineChars == "\r")
					{
						continue;
					}
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(str.Length + 5);
					}
					stringBuilder.Append(str, num, i - num);
				}
			}
			stringBuilder.Append(newLineChars);
			num = i + 1;
		}
		if (stringBuilder == null)
		{
			return str;
		}
		stringBuilder.Append(str, num, i - num);
		return stringBuilder.ToString();
	}

	private string ReplaceNewLines(char[] data, int offset, int len)
	{
		if (data == null)
		{
			return null;
		}
		StringBuilder stringBuilder = null;
		int num = offset;
		int num2 = offset + len;
		int i;
		for (i = offset; i < num2; i++)
		{
			char c;
			if ((c = data[i]) >= ' ')
			{
				continue;
			}
			if (c == '\n')
			{
				if (newLineChars == "\n")
				{
					continue;
				}
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(len + 5);
				}
				stringBuilder.Append(data, num, i - num);
			}
			else
			{
				if (c != '\r')
				{
					continue;
				}
				if (i + 1 < num2 && data[i + 1] == '\n')
				{
					if (newLineChars == "\r\n")
					{
						i++;
						continue;
					}
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(len + 5);
					}
					stringBuilder.Append(data, num, i - num);
					i++;
				}
				else
				{
					if (newLineChars == "\r")
					{
						continue;
					}
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(len + 5);
					}
					stringBuilder.Append(data, num, i - num);
				}
			}
			stringBuilder.Append(newLineChars);
			num = i + 1;
		}
		if (stringBuilder == null)
		{
			return null;
		}
		stringBuilder.Append(data, num, i - num);
		return stringBuilder.ToString();
	}

	private string InterleaveInvalidChars(string text, char invChar1, char invChar2)
	{
		StringBuilder stringBuilder = null;
		int num = 0;
		int i;
		for (i = 0; i < text.Length; i++)
		{
			if (text[i] == invChar2 && i > 0 && text[i - 1] == invChar1)
			{
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(text.Length + 5);
				}
				stringBuilder.Append(text, num, i - num);
				stringBuilder.Append(' ');
				num = i;
			}
		}
		if (stringBuilder == null)
		{
			if (i != 0 && text[i - 1] == invChar1)
			{
				return text + " ";
			}
			return text;
		}
		stringBuilder.Append(text, num, i - num);
		if (i > 0 && text[i - 1] == invChar1)
		{
			stringBuilder.Append(' ');
		}
		return stringBuilder.ToString();
	}

	public override Task WriteDocTypeAsync(string name, string pubid, string sysid, string subset)
	{
		if (checkNames)
		{
			ValidateQName(name);
		}
		if (checkValues)
		{
			int invCharPos;
			if (pubid != null && (invCharPos = xmlCharType.IsPublicId(pubid)) >= 0)
			{
				throw XmlConvert.CreateInvalidCharException(pubid, invCharPos);
			}
			if (sysid != null)
			{
				CheckCharacters(sysid);
			}
			if (subset != null)
			{
				CheckCharacters(subset);
			}
		}
		if (replaceNewLines)
		{
			sysid = ReplaceNewLines(sysid);
			pubid = ReplaceNewLines(pubid);
			subset = ReplaceNewLines(subset);
		}
		return writer.WriteDocTypeAsync(name, pubid, sysid, subset);
	}

	public override Task WriteStartElementAsync(string prefix, string localName, string ns)
	{
		if (checkNames)
		{
			if (localName == null || localName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
			}
			ValidateNCName(localName);
			if (prefix != null && prefix.Length > 0)
			{
				ValidateNCName(prefix);
			}
		}
		return writer.WriteStartElementAsync(prefix, localName, ns);
	}

	protected internal override Task WriteStartAttributeAsync(string prefix, string localName, string ns)
	{
		if (checkNames)
		{
			if (localName == null || localName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
			}
			ValidateNCName(localName);
			if (prefix != null && prefix.Length > 0)
			{
				ValidateNCName(prefix);
			}
		}
		return writer.WriteStartAttributeAsync(prefix, localName, ns);
	}

	public override async Task WriteCDataAsync(string text)
	{
		if (text != null)
		{
			if (checkValues)
			{
				CheckCharacters(text);
			}
			if (replaceNewLines)
			{
				text = ReplaceNewLines(text);
			}
			while (true)
			{
				int num;
				int i = (num = text.IndexOf("]]>", StringComparison.Ordinal));
				if (num < 0)
				{
					break;
				}
				await writer.WriteCDataAsync(text.Substring(0, i + 2)).ConfigureAwait(continueOnCapturedContext: false);
				text = text.Substring(i + 2);
			}
		}
		await writer.WriteCDataAsync(text).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override Task WriteCommentAsync(string text)
	{
		if (text != null)
		{
			if (checkValues)
			{
				CheckCharacters(text);
				text = InterleaveInvalidChars(text, '-', '-');
			}
			if (replaceNewLines)
			{
				text = ReplaceNewLines(text);
			}
		}
		return writer.WriteCommentAsync(text);
	}

	public override Task WriteProcessingInstructionAsync(string name, string text)
	{
		if (checkNames)
		{
			ValidateNCName(name);
		}
		if (text != null)
		{
			if (checkValues)
			{
				CheckCharacters(text);
				text = InterleaveInvalidChars(text, '?', '>');
			}
			if (replaceNewLines)
			{
				text = ReplaceNewLines(text);
			}
		}
		return writer.WriteProcessingInstructionAsync(name, text);
	}

	public override Task WriteEntityRefAsync(string name)
	{
		if (checkNames)
		{
			ValidateQName(name);
		}
		return writer.WriteEntityRefAsync(name);
	}

	public override Task WriteWhitespaceAsync(string ws)
	{
		if (ws == null)
		{
			ws = string.Empty;
		}
		int invCharIndex;
		if (checkNames && (invCharIndex = xmlCharType.IsOnlyWhitespaceWithPos(ws)) != -1)
		{
			object[] args = XmlException.BuildCharExceptionArgs(ws, invCharIndex);
			throw new ArgumentException(Res.GetString("The Whitespace or SignificantWhitespace node can contain only XML white space characters. '{0}' is not an XML white space character.", args));
		}
		if (replaceNewLines)
		{
			ws = ReplaceNewLines(ws);
		}
		return writer.WriteWhitespaceAsync(ws);
	}

	public override Task WriteStringAsync(string text)
	{
		if (text != null)
		{
			if (checkValues)
			{
				CheckCharacters(text);
			}
			if (replaceNewLines && WriteState != WriteState.Attribute)
			{
				text = ReplaceNewLines(text);
			}
		}
		return writer.WriteStringAsync(text);
	}

	public override Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
	{
		return writer.WriteSurrogateCharEntityAsync(lowChar, highChar);
	}

	public override Task WriteCharsAsync(char[] buffer, int index, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (count > buffer.Length - index)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (checkValues)
		{
			CheckCharacters(buffer, index, count);
		}
		if (replaceNewLines && WriteState != WriteState.Attribute)
		{
			string text = ReplaceNewLines(buffer, index, count);
			if (text != null)
			{
				return WriteStringAsync(text);
			}
		}
		return writer.WriteCharsAsync(buffer, index, count);
	}

	public override Task WriteNmTokenAsync(string name)
	{
		if (checkNames)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
			}
			XmlConvert.VerifyNMTOKEN(name);
		}
		return writer.WriteNmTokenAsync(name);
	}

	public override Task WriteNameAsync(string name)
	{
		if (checkNames)
		{
			XmlConvert.VerifyQName(name, ExceptionType.XmlException);
		}
		return writer.WriteNameAsync(name);
	}

	public override Task WriteQualifiedNameAsync(string localName, string ns)
	{
		if (checkNames)
		{
			ValidateNCName(localName);
		}
		return writer.WriteQualifiedNameAsync(localName, ns);
	}
}
