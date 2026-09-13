using System.IO;
using System.Threading.Tasks;

namespace System.Xml;

internal class XmlUtf8RawTextWriterIndent : XmlUtf8RawTextWriter
{
	protected int indentLevel;

	protected bool newLineOnAttributes;

	protected string indentChars;

	protected bool mixedContent;

	private BitStack mixedContentStack;

	protected ConformanceLevel conformanceLevel;

	public override XmlWriterSettings Settings
	{
		get
		{
			XmlWriterSettings settings = base.Settings;
			settings.ReadOnly = false;
			settings.Indent = true;
			settings.IndentChars = indentChars;
			settings.NewLineOnAttributes = newLineOnAttributes;
			settings.ReadOnly = true;
			return settings;
		}
	}

	public XmlUtf8RawTextWriterIndent(Stream stream, XmlWriterSettings settings)
		: base(stream, settings)
	{
		Init(settings);
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		if (!mixedContent && textPos != bufPos)
		{
			WriteIndent();
		}
		base.WriteDocType(name, pubid, sysid, subset);
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		if (!mixedContent && textPos != bufPos)
		{
			WriteIndent();
		}
		indentLevel++;
		mixedContentStack.PushBit(mixedContent);
		base.WriteStartElement(prefix, localName, ns);
	}

	internal override void StartElementContent()
	{
		if (indentLevel == 1 && conformanceLevel == ConformanceLevel.Document)
		{
			mixedContent = false;
		}
		else
		{
			mixedContent = mixedContentStack.PeekBit();
		}
		base.StartElementContent();
	}

	internal override void OnRootElement(ConformanceLevel currentConformanceLevel)
	{
		conformanceLevel = currentConformanceLevel;
	}

	internal override void WriteEndElement(string prefix, string localName, string ns)
	{
		indentLevel--;
		if (!mixedContent && contentPos != bufPos && textPos != bufPos)
		{
			WriteIndent();
		}
		mixedContent = mixedContentStack.PopBit();
		base.WriteEndElement(prefix, localName, ns);
	}

	internal override void WriteFullEndElement(string prefix, string localName, string ns)
	{
		indentLevel--;
		if (!mixedContent && contentPos != bufPos && textPos != bufPos)
		{
			WriteIndent();
		}
		mixedContent = mixedContentStack.PopBit();
		base.WriteFullEndElement(prefix, localName, ns);
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		if (newLineOnAttributes)
		{
			WriteIndent();
		}
		base.WriteStartAttribute(prefix, localName, ns);
	}

	public override void WriteCData(string text)
	{
		mixedContent = true;
		base.WriteCData(text);
	}

	public override void WriteComment(string text)
	{
		if (!mixedContent && textPos != bufPos)
		{
			WriteIndent();
		}
		base.WriteComment(text);
	}

	public override void WriteProcessingInstruction(string target, string text)
	{
		if (!mixedContent && textPos != bufPos)
		{
			WriteIndent();
		}
		base.WriteProcessingInstruction(target, text);
	}

	public override void WriteEntityRef(string name)
	{
		mixedContent = true;
		base.WriteEntityRef(name);
	}

	public override void WriteCharEntity(char ch)
	{
		mixedContent = true;
		base.WriteCharEntity(ch);
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		mixedContent = true;
		base.WriteSurrogateCharEntity(lowChar, highChar);
	}

	public override void WriteWhitespace(string ws)
	{
		mixedContent = true;
		base.WriteWhitespace(ws);
	}

	public override void WriteString(string text)
	{
		mixedContent = true;
		base.WriteString(text);
	}

	public override void WriteChars(char[] buffer, int index, int count)
	{
		mixedContent = true;
		base.WriteChars(buffer, index, count);
	}

	public override void WriteRaw(char[] buffer, int index, int count)
	{
		mixedContent = true;
		base.WriteRaw(buffer, index, count);
	}

	public override void WriteRaw(string data)
	{
		mixedContent = true;
		base.WriteRaw(data);
	}

	public override void WriteBase64(byte[] buffer, int index, int count)
	{
		mixedContent = true;
		base.WriteBase64(buffer, index, count);
	}

	private void Init(XmlWriterSettings settings)
	{
		indentLevel = 0;
		indentChars = settings.IndentChars;
		newLineOnAttributes = settings.NewLineOnAttributes;
		mixedContentStack = new BitStack();
		if (!checkCharacters)
		{
			return;
		}
		if (newLineOnAttributes)
		{
			ValidateContentChars(indentChars, "IndentChars", allowOnlyWhitespace: true);
			ValidateContentChars(newLineChars, "NewLineChars", allowOnlyWhitespace: true);
			return;
		}
		ValidateContentChars(indentChars, "IndentChars", allowOnlyWhitespace: false);
		if (newLineHandling != NewLineHandling.Replace)
		{
			ValidateContentChars(newLineChars, "NewLineChars", allowOnlyWhitespace: false);
		}
	}

	private void WriteIndent()
	{
		RawText(newLineChars);
		for (int num = indentLevel; num > 0; num--)
		{
			RawText(indentChars);
		}
	}

	public override async Task WriteDocTypeAsync(string name, string pubid, string sysid, string subset)
	{
		CheckAsyncCall();
		if (!mixedContent && textPos != bufPos)
		{
			await WriteIndentAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		await base.WriteDocTypeAsync(name, pubid, sysid, subset).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task WriteStartElementAsync(string prefix, string localName, string ns)
	{
		CheckAsyncCall();
		if (!mixedContent && textPos != bufPos)
		{
			await WriteIndentAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		indentLevel++;
		mixedContentStack.PushBit(mixedContent);
		await base.WriteStartElementAsync(prefix, localName, ns).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal override async Task WriteEndElementAsync(string prefix, string localName, string ns)
	{
		CheckAsyncCall();
		indentLevel--;
		if (!mixedContent && contentPos != bufPos && textPos != bufPos)
		{
			await WriteIndentAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		mixedContent = mixedContentStack.PopBit();
		await base.WriteEndElementAsync(prefix, localName, ns).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal override async Task WriteFullEndElementAsync(string prefix, string localName, string ns)
	{
		CheckAsyncCall();
		indentLevel--;
		if (!mixedContent && contentPos != bufPos && textPos != bufPos)
		{
			await WriteIndentAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		mixedContent = mixedContentStack.PopBit();
		await base.WriteFullEndElementAsync(prefix, localName, ns).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected internal override async Task WriteStartAttributeAsync(string prefix, string localName, string ns)
	{
		CheckAsyncCall();
		if (newLineOnAttributes)
		{
			await WriteIndentAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		await base.WriteStartAttributeAsync(prefix, localName, ns).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override Task WriteCDataAsync(string text)
	{
		CheckAsyncCall();
		mixedContent = true;
		return base.WriteCDataAsync(text);
	}

	public override async Task WriteCommentAsync(string text)
	{
		CheckAsyncCall();
		if (!mixedContent && textPos != bufPos)
		{
			await WriteIndentAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		await base.WriteCommentAsync(text).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task WriteProcessingInstructionAsync(string target, string text)
	{
		CheckAsyncCall();
		if (!mixedContent && textPos != bufPos)
		{
			await WriteIndentAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		await base.WriteProcessingInstructionAsync(target, text).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override Task WriteEntityRefAsync(string name)
	{
		CheckAsyncCall();
		mixedContent = true;
		return base.WriteEntityRefAsync(name);
	}

	public override Task WriteCharEntityAsync(char ch)
	{
		CheckAsyncCall();
		mixedContent = true;
		return base.WriteCharEntityAsync(ch);
	}

	public override Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
	{
		CheckAsyncCall();
		mixedContent = true;
		return base.WriteSurrogateCharEntityAsync(lowChar, highChar);
	}

	public override Task WriteWhitespaceAsync(string ws)
	{
		CheckAsyncCall();
		mixedContent = true;
		return base.WriteWhitespaceAsync(ws);
	}

	public override Task WriteStringAsync(string text)
	{
		CheckAsyncCall();
		mixedContent = true;
		return base.WriteStringAsync(text);
	}

	public override Task WriteCharsAsync(char[] buffer, int index, int count)
	{
		CheckAsyncCall();
		mixedContent = true;
		return base.WriteCharsAsync(buffer, index, count);
	}

	public override Task WriteRawAsync(char[] buffer, int index, int count)
	{
		CheckAsyncCall();
		mixedContent = true;
		return base.WriteRawAsync(buffer, index, count);
	}

	public override Task WriteRawAsync(string data)
	{
		CheckAsyncCall();
		mixedContent = true;
		return base.WriteRawAsync(data);
	}

	public override Task WriteBase64Async(byte[] buffer, int index, int count)
	{
		CheckAsyncCall();
		mixedContent = true;
		return base.WriteBase64Async(buffer, index, count);
	}

	private async Task WriteIndentAsync()
	{
		CheckAsyncCall();
		await RawTextAsync(newLineChars).ConfigureAwait(continueOnCapturedContext: false);
		for (int i = indentLevel; i > 0; i--)
		{
			await RawTextAsync(indentChars).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
