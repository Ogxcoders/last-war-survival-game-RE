using System.Threading.Tasks;

namespace System.Xml;

internal class XmlCharCheckingReader : XmlWrappingReader
{
	private enum State
	{
		Initial,
		InReadBinary,
		Error,
		Interactive
	}

	private State state;

	private bool checkCharacters;

	private bool ignoreWhitespace;

	private bool ignoreComments;

	private bool ignorePis;

	private DtdProcessing dtdProcessing;

	private XmlNodeType lastNodeType;

	private XmlCharType xmlCharType;

	private ReadContentAsBinaryHelper readBinaryHelper;

	public override XmlReaderSettings Settings
	{
		get
		{
			XmlReaderSettings settings = reader.Settings;
			settings = ((settings != null) ? settings.Clone() : new XmlReaderSettings());
			if (checkCharacters)
			{
				settings.CheckCharacters = true;
			}
			if (ignoreWhitespace)
			{
				settings.IgnoreWhitespace = true;
			}
			if (ignoreComments)
			{
				settings.IgnoreComments = true;
			}
			if (ignorePis)
			{
				settings.IgnoreProcessingInstructions = true;
			}
			if (dtdProcessing != (DtdProcessing)(-1))
			{
				settings.DtdProcessing = dtdProcessing;
			}
			settings.ReadOnly = true;
			return settings;
		}
	}

	public override ReadState ReadState
	{
		get
		{
			switch (state)
			{
			case State.Initial:
				if (reader.ReadState != ReadState.Closed)
				{
					return ReadState.Initial;
				}
				return ReadState.Closed;
			case State.Error:
				return ReadState.Error;
			default:
				return reader.ReadState;
			}
		}
	}

	public override bool CanReadBinaryContent => true;

	internal XmlCharCheckingReader(XmlReader reader, bool checkCharacters, bool ignoreWhitespace, bool ignoreComments, bool ignorePis, DtdProcessing dtdProcessing)
		: base(reader)
	{
		state = State.Initial;
		this.checkCharacters = checkCharacters;
		this.ignoreWhitespace = ignoreWhitespace;
		this.ignoreComments = ignoreComments;
		this.ignorePis = ignorePis;
		this.dtdProcessing = dtdProcessing;
		lastNodeType = XmlNodeType.None;
		if (checkCharacters)
		{
			xmlCharType = XmlCharType.Instance;
		}
	}

	public override bool MoveToAttribute(string name)
	{
		if (state == State.InReadBinary)
		{
			FinishReadBinary();
		}
		return reader.MoveToAttribute(name);
	}

	public override bool MoveToAttribute(string name, string ns)
	{
		if (state == State.InReadBinary)
		{
			FinishReadBinary();
		}
		return reader.MoveToAttribute(name, ns);
	}

	public override void MoveToAttribute(int i)
	{
		if (state == State.InReadBinary)
		{
			FinishReadBinary();
		}
		reader.MoveToAttribute(i);
	}

	public override bool MoveToFirstAttribute()
	{
		if (state == State.InReadBinary)
		{
			FinishReadBinary();
		}
		return reader.MoveToFirstAttribute();
	}

	public override bool MoveToNextAttribute()
	{
		if (state == State.InReadBinary)
		{
			FinishReadBinary();
		}
		return reader.MoveToNextAttribute();
	}

	public override bool MoveToElement()
	{
		if (state == State.InReadBinary)
		{
			FinishReadBinary();
		}
		return reader.MoveToElement();
	}

	public override bool Read()
	{
		switch (state)
		{
		case State.Initial:
			state = State.Interactive;
			if (reader.ReadState != ReadState.Initial)
			{
				break;
			}
			goto case State.Interactive;
		case State.Error:
			return false;
		case State.InReadBinary:
			FinishReadBinary();
			state = State.Interactive;
			goto case State.Interactive;
		case State.Interactive:
			if (!reader.Read())
			{
				return false;
			}
			break;
		default:
			return false;
		}
		XmlNodeType nodeType = reader.NodeType;
		if (!checkCharacters)
		{
			switch (nodeType)
			{
			case XmlNodeType.Comment:
				if (ignoreComments)
				{
					return Read();
				}
				break;
			case XmlNodeType.Whitespace:
				if (ignoreWhitespace)
				{
					return Read();
				}
				break;
			case XmlNodeType.ProcessingInstruction:
				if (ignorePis)
				{
					return Read();
				}
				break;
			case XmlNodeType.DocumentType:
				if (dtdProcessing == DtdProcessing.Prohibit)
				{
					Throw("For security reasons DTD is prohibited in this XML document. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method.", string.Empty);
				}
				else if (dtdProcessing == DtdProcessing.Ignore)
				{
					return Read();
				}
				break;
			}
			return true;
		}
		switch (nodeType)
		{
		case XmlNodeType.Element:
			if (!checkCharacters)
			{
				break;
			}
			ValidateQName(reader.Prefix, reader.LocalName);
			if (reader.MoveToFirstAttribute())
			{
				do
				{
					ValidateQName(reader.Prefix, reader.LocalName);
					CheckCharacters(reader.Value);
				}
				while (reader.MoveToNextAttribute());
				reader.MoveToElement();
			}
			break;
		case XmlNodeType.Text:
		case XmlNodeType.CDATA:
			if (checkCharacters)
			{
				CheckCharacters(reader.Value);
			}
			break;
		case XmlNodeType.EntityReference:
			if (checkCharacters)
			{
				ValidateQName(reader.Name);
			}
			break;
		case XmlNodeType.ProcessingInstruction:
			if (ignorePis)
			{
				return Read();
			}
			if (checkCharacters)
			{
				ValidateQName(reader.Name);
				CheckCharacters(reader.Value);
			}
			break;
		case XmlNodeType.Comment:
			if (ignoreComments)
			{
				return Read();
			}
			if (checkCharacters)
			{
				CheckCharacters(reader.Value);
			}
			break;
		case XmlNodeType.DocumentType:
			if (dtdProcessing == DtdProcessing.Prohibit)
			{
				Throw("For security reasons DTD is prohibited in this XML document. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method.", string.Empty);
			}
			else if (dtdProcessing == DtdProcessing.Ignore)
			{
				return Read();
			}
			if (checkCharacters)
			{
				ValidateQName(reader.Name);
				CheckCharacters(reader.Value);
				string attribute = reader.GetAttribute("SYSTEM");
				if (attribute != null)
				{
					CheckCharacters(attribute);
				}
				attribute = reader.GetAttribute("PUBLIC");
				int invCharIndex;
				if (attribute != null && (invCharIndex = xmlCharType.IsPublicId(attribute)) >= 0)
				{
					Throw("'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(attribute, invCharIndex));
				}
			}
			break;
		case XmlNodeType.Whitespace:
			if (ignoreWhitespace)
			{
				return Read();
			}
			if (checkCharacters)
			{
				CheckWhitespace(reader.Value);
			}
			break;
		case XmlNodeType.SignificantWhitespace:
			if (checkCharacters)
			{
				CheckWhitespace(reader.Value);
			}
			break;
		case XmlNodeType.EndElement:
			if (checkCharacters)
			{
				ValidateQName(reader.Prefix, reader.LocalName);
			}
			break;
		}
		lastNodeType = nodeType;
		return true;
	}

	public override bool ReadAttributeValue()
	{
		if (state == State.InReadBinary)
		{
			FinishReadBinary();
		}
		return reader.ReadAttributeValue();
	}

	public override int ReadContentAsBase64(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (state != State.InReadBinary)
		{
			if (base.CanReadBinaryContent && !checkCharacters)
			{
				readBinaryHelper = null;
				state = State.InReadBinary;
				return base.ReadContentAsBase64(buffer, index, count);
			}
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
		}
		else if (readBinaryHelper == null)
		{
			return base.ReadContentAsBase64(buffer, index, count);
		}
		state = State.Interactive;
		int result = readBinaryHelper.ReadContentAsBase64(buffer, index, count);
		state = State.InReadBinary;
		return result;
	}

	public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (state != State.InReadBinary)
		{
			if (base.CanReadBinaryContent && !checkCharacters)
			{
				readBinaryHelper = null;
				state = State.InReadBinary;
				return base.ReadContentAsBinHex(buffer, index, count);
			}
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
		}
		else if (readBinaryHelper == null)
		{
			return base.ReadContentAsBinHex(buffer, index, count);
		}
		state = State.Interactive;
		int result = readBinaryHelper.ReadContentAsBinHex(buffer, index, count);
		state = State.InReadBinary;
		return result;
	}

	public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (buffer.Length - index < count)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (state != State.InReadBinary)
		{
			if (base.CanReadBinaryContent && !checkCharacters)
			{
				readBinaryHelper = null;
				state = State.InReadBinary;
				return base.ReadElementContentAsBase64(buffer, index, count);
			}
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
		}
		else if (readBinaryHelper == null)
		{
			return base.ReadElementContentAsBase64(buffer, index, count);
		}
		state = State.Interactive;
		int result = readBinaryHelper.ReadElementContentAsBase64(buffer, index, count);
		state = State.InReadBinary;
		return result;
	}

	public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (buffer.Length - index < count)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (state != State.InReadBinary)
		{
			if (base.CanReadBinaryContent && !checkCharacters)
			{
				readBinaryHelper = null;
				state = State.InReadBinary;
				return base.ReadElementContentAsBinHex(buffer, index, count);
			}
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
		}
		else if (readBinaryHelper == null)
		{
			return base.ReadElementContentAsBinHex(buffer, index, count);
		}
		state = State.Interactive;
		int result = readBinaryHelper.ReadElementContentAsBinHex(buffer, index, count);
		state = State.InReadBinary;
		return result;
	}

	private void Throw(string res, string arg)
	{
		state = State.Error;
		throw new XmlException(res, arg, (IXmlLineInfo)null);
	}

	private void Throw(string res, string[] args)
	{
		state = State.Error;
		throw new XmlException(res, args, (IXmlLineInfo)null);
	}

	private void CheckWhitespace(string value)
	{
		int invCharIndex;
		if ((invCharIndex = xmlCharType.IsOnlyWhitespaceWithPos(value)) != -1)
		{
			Throw("The Whitespace or SignificantWhitespace node can contain only XML white space characters. '{0}' is not an XML white space character.", XmlException.BuildCharExceptionArgs(value, invCharIndex));
		}
	}

	private void ValidateQName(string name)
	{
		ValidateNames.ParseQNameThrow(name, out var _, out var _);
	}

	private void ValidateQName(string prefix, string localName)
	{
		try
		{
			if (prefix.Length > 0)
			{
				ValidateNames.ParseNCNameThrow(prefix);
			}
			ValidateNames.ParseNCNameThrow(localName);
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	private void CheckCharacters(string value)
	{
		XmlConvert.VerifyCharData(value, ExceptionType.ArgumentException, ExceptionType.XmlException);
	}

	private void FinishReadBinary()
	{
		state = State.Interactive;
		if (readBinaryHelper != null)
		{
			readBinaryHelper.Finish();
		}
	}

	public override async Task<bool> ReadAsync()
	{
		switch (state)
		{
		case State.Initial:
			state = State.Interactive;
			if (reader.ReadState != ReadState.Initial)
			{
				break;
			}
			goto case State.Interactive;
		case State.Error:
			return false;
		case State.InReadBinary:
			await FinishReadBinaryAsync().ConfigureAwait(continueOnCapturedContext: false);
			state = State.Interactive;
			goto case State.Interactive;
		case State.Interactive:
			if (!(await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				return false;
			}
			break;
		default:
			return false;
		}
		XmlNodeType nodeType = reader.NodeType;
		if (!checkCharacters)
		{
			switch (nodeType)
			{
			case XmlNodeType.Comment:
				if (ignoreComments)
				{
					return await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				break;
			case XmlNodeType.Whitespace:
				if (ignoreWhitespace)
				{
					return await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				break;
			case XmlNodeType.ProcessingInstruction:
				if (ignorePis)
				{
					return await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				break;
			case XmlNodeType.DocumentType:
				if (dtdProcessing == DtdProcessing.Prohibit)
				{
					Throw("For security reasons DTD is prohibited in this XML document. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method.", string.Empty);
				}
				else if (dtdProcessing == DtdProcessing.Ignore)
				{
					return await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				break;
			}
			return true;
		}
		switch (nodeType)
		{
		case XmlNodeType.Element:
			if (!checkCharacters)
			{
				break;
			}
			ValidateQName(reader.Prefix, reader.LocalName);
			if (reader.MoveToFirstAttribute())
			{
				do
				{
					ValidateQName(reader.Prefix, reader.LocalName);
					CheckCharacters(reader.Value);
				}
				while (reader.MoveToNextAttribute());
				reader.MoveToElement();
			}
			break;
		case XmlNodeType.Text:
		case XmlNodeType.CDATA:
			if (checkCharacters)
			{
				CheckCharacters(await reader.GetValueAsync().ConfigureAwait(continueOnCapturedContext: false));
			}
			break;
		case XmlNodeType.EntityReference:
			if (checkCharacters)
			{
				ValidateQName(reader.Name);
			}
			break;
		case XmlNodeType.ProcessingInstruction:
			if (ignorePis)
			{
				return await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			if (checkCharacters)
			{
				ValidateQName(reader.Name);
				CheckCharacters(reader.Value);
			}
			break;
		case XmlNodeType.Comment:
			if (ignoreComments)
			{
				return await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			if (checkCharacters)
			{
				CheckCharacters(reader.Value);
			}
			break;
		case XmlNodeType.DocumentType:
			if (dtdProcessing == DtdProcessing.Prohibit)
			{
				Throw("For security reasons DTD is prohibited in this XML document. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method.", string.Empty);
			}
			else if (dtdProcessing == DtdProcessing.Ignore)
			{
				return await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			if (checkCharacters)
			{
				ValidateQName(reader.Name);
				CheckCharacters(reader.Value);
				string attribute = reader.GetAttribute("SYSTEM");
				if (attribute != null)
				{
					CheckCharacters(attribute);
				}
				attribute = reader.GetAttribute("PUBLIC");
				int invCharIndex;
				if (attribute != null && (invCharIndex = xmlCharType.IsPublicId(attribute)) >= 0)
				{
					Throw("'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(attribute, invCharIndex));
				}
			}
			break;
		case XmlNodeType.Whitespace:
			if (ignoreWhitespace)
			{
				return await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			if (checkCharacters)
			{
				CheckWhitespace(await reader.GetValueAsync().ConfigureAwait(continueOnCapturedContext: false));
			}
			break;
		case XmlNodeType.SignificantWhitespace:
			if (checkCharacters)
			{
				CheckWhitespace(await reader.GetValueAsync().ConfigureAwait(continueOnCapturedContext: false));
			}
			break;
		case XmlNodeType.EndElement:
			if (checkCharacters)
			{
				ValidateQName(reader.Prefix, reader.LocalName);
			}
			break;
		}
		lastNodeType = nodeType;
		return true;
	}

	public override async Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (state != State.InReadBinary)
		{
			if (base.CanReadBinaryContent && !checkCharacters)
			{
				readBinaryHelper = null;
				state = State.InReadBinary;
				return await base.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			}
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
		}
		else if (readBinaryHelper == null)
		{
			return await base.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		}
		state = State.Interactive;
		int result = await readBinaryHelper.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		state = State.InReadBinary;
		return result;
	}

	public override async Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (state != State.InReadBinary)
		{
			if (base.CanReadBinaryContent && !checkCharacters)
			{
				readBinaryHelper = null;
				state = State.InReadBinary;
				return await base.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			}
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
		}
		else if (readBinaryHelper == null)
		{
			return await base.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		}
		state = State.Interactive;
		int result = await readBinaryHelper.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		state = State.InReadBinary;
		return result;
	}

	public override async Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (buffer.Length - index < count)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (state != State.InReadBinary)
		{
			if (base.CanReadBinaryContent && !checkCharacters)
			{
				readBinaryHelper = null;
				state = State.InReadBinary;
				return await base.ReadElementContentAsBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			}
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
		}
		else if (readBinaryHelper == null)
		{
			return await base.ReadElementContentAsBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		}
		state = State.Interactive;
		int result = await readBinaryHelper.ReadElementContentAsBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		state = State.InReadBinary;
		return result;
	}

	public override async Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (buffer.Length - index < count)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (state != State.InReadBinary)
		{
			if (base.CanReadBinaryContent && !checkCharacters)
			{
				readBinaryHelper = null;
				state = State.InReadBinary;
				return await base.ReadElementContentAsBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			}
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
		}
		else if (readBinaryHelper == null)
		{
			return await base.ReadElementContentAsBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		}
		state = State.Interactive;
		int result = await readBinaryHelper.ReadElementContentAsBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		state = State.InReadBinary;
		return result;
	}

	private async Task FinishReadBinaryAsync()
	{
		state = State.Interactive;
		if (readBinaryHelper != null)
		{
			await readBinaryHelper.FinishAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
