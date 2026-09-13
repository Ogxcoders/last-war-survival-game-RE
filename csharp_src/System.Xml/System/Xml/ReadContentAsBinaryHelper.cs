using System.Threading.Tasks;

namespace System.Xml;

internal class ReadContentAsBinaryHelper
{
	private enum State
	{
		None,
		InReadContent,
		InReadElementContent
	}

	private XmlReader reader;

	private State state;

	private int valueOffset;

	private bool isEnd;

	private bool canReadValueChunk;

	private char[] valueChunk;

	private int valueChunkLength;

	private IncrementalReadDecoder decoder;

	private Base64Decoder base64Decoder;

	private BinHexDecoder binHexDecoder;

	private const int ChunkSize = 256;

	internal ReadContentAsBinaryHelper(XmlReader reader)
	{
		this.reader = reader;
		canReadValueChunk = reader.CanReadValueChunk;
		if (canReadValueChunk)
		{
			valueChunk = new char[256];
		}
	}

	internal static ReadContentAsBinaryHelper CreateOrReset(ReadContentAsBinaryHelper helper, XmlReader reader)
	{
		if (helper == null)
		{
			return new ReadContentAsBinaryHelper(reader);
		}
		helper.Reset();
		return helper;
	}

	internal int ReadContentAsBase64(byte[] buffer, int index, int count)
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
		switch (state)
		{
		case State.None:
			if (!reader.CanReadContentAs())
			{
				throw reader.CreateReadContentAsException("ReadContentAsBase64");
			}
			if (!Init())
			{
				return 0;
			}
			break;
		case State.InReadContent:
			if (decoder == base64Decoder)
			{
				return ReadContentAsBinary(buffer, index, count);
			}
			break;
		case State.InReadElementContent:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		default:
			return 0;
		}
		InitBase64Decoder();
		return ReadContentAsBinary(buffer, index, count);
	}

	internal int ReadContentAsBinHex(byte[] buffer, int index, int count)
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
		switch (state)
		{
		case State.None:
			if (!reader.CanReadContentAs())
			{
				throw reader.CreateReadContentAsException("ReadContentAsBinHex");
			}
			if (!Init())
			{
				return 0;
			}
			break;
		case State.InReadContent:
			if (decoder == binHexDecoder)
			{
				return ReadContentAsBinary(buffer, index, count);
			}
			break;
		case State.InReadElementContent:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		default:
			return 0;
		}
		InitBinHexDecoder();
		return ReadContentAsBinary(buffer, index, count);
	}

	internal int ReadElementContentAsBase64(byte[] buffer, int index, int count)
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
		switch (state)
		{
		case State.None:
			if (reader.NodeType != XmlNodeType.Element)
			{
				throw reader.CreateReadElementContentAsException("ReadElementContentAsBase64");
			}
			if (!InitOnElement())
			{
				return 0;
			}
			break;
		case State.InReadContent:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		case State.InReadElementContent:
			if (decoder == base64Decoder)
			{
				return ReadElementContentAsBinary(buffer, index, count);
			}
			break;
		default:
			return 0;
		}
		InitBase64Decoder();
		return ReadElementContentAsBinary(buffer, index, count);
	}

	internal int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
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
		switch (state)
		{
		case State.None:
			if (reader.NodeType != XmlNodeType.Element)
			{
				throw reader.CreateReadElementContentAsException("ReadElementContentAsBinHex");
			}
			if (!InitOnElement())
			{
				return 0;
			}
			break;
		case State.InReadContent:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		case State.InReadElementContent:
			if (decoder == binHexDecoder)
			{
				return ReadElementContentAsBinary(buffer, index, count);
			}
			break;
		default:
			return 0;
		}
		InitBinHexDecoder();
		return ReadElementContentAsBinary(buffer, index, count);
	}

	internal void Finish()
	{
		if (state != State.None)
		{
			while (MoveToNextContentNode(moveIfOnContentNode: true))
			{
			}
			if (state == State.InReadElementContent)
			{
				if (reader.NodeType != XmlNodeType.EndElement)
				{
					throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
				}
				reader.Read();
			}
		}
		Reset();
	}

	internal void Reset()
	{
		state = State.None;
		isEnd = false;
		valueOffset = 0;
	}

	private bool Init()
	{
		if (!MoveToNextContentNode(moveIfOnContentNode: false))
		{
			return false;
		}
		state = State.InReadContent;
		isEnd = false;
		return true;
	}

	private bool InitOnElement()
	{
		bool isEmptyElement = reader.IsEmptyElement;
		reader.Read();
		if (isEmptyElement)
		{
			return false;
		}
		if (!MoveToNextContentNode(moveIfOnContentNode: false))
		{
			if (reader.NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
			}
			reader.Read();
			return false;
		}
		state = State.InReadElementContent;
		isEnd = false;
		return true;
	}

	private void InitBase64Decoder()
	{
		if (base64Decoder == null)
		{
			base64Decoder = new Base64Decoder();
		}
		else
		{
			base64Decoder.Reset();
		}
		decoder = base64Decoder;
	}

	private void InitBinHexDecoder()
	{
		if (binHexDecoder == null)
		{
			binHexDecoder = new BinHexDecoder();
		}
		else
		{
			binHexDecoder.Reset();
		}
		decoder = binHexDecoder;
	}

	private int ReadContentAsBinary(byte[] buffer, int index, int count)
	{
		if (isEnd)
		{
			Reset();
			return 0;
		}
		decoder.SetNextOutputBuffer(buffer, index, count);
		do
		{
			if (canReadValueChunk)
			{
				while (true)
				{
					if (valueOffset < valueChunkLength)
					{
						int num = decoder.Decode(valueChunk, valueOffset, valueChunkLength - valueOffset);
						valueOffset += num;
					}
					if (decoder.IsFull)
					{
						return decoder.DecodedCount;
					}
					if ((valueChunkLength = reader.ReadValueChunk(valueChunk, 0, 256)) == 0)
					{
						break;
					}
					valueOffset = 0;
				}
			}
			else
			{
				string value = reader.Value;
				int num2 = decoder.Decode(value, valueOffset, value.Length - valueOffset);
				valueOffset += num2;
				if (decoder.IsFull)
				{
					return decoder.DecodedCount;
				}
			}
			valueOffset = 0;
		}
		while (MoveToNextContentNode(moveIfOnContentNode: true));
		isEnd = true;
		return decoder.DecodedCount;
	}

	private int ReadElementContentAsBinary(byte[] buffer, int index, int count)
	{
		if (count == 0)
		{
			return 0;
		}
		int num = ReadContentAsBinary(buffer, index, count);
		if (num > 0)
		{
			return num;
		}
		if (reader.NodeType != XmlNodeType.EndElement)
		{
			throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
		}
		reader.Read();
		state = State.None;
		return 0;
	}

	private bool MoveToNextContentNode(bool moveIfOnContentNode)
	{
		do
		{
			switch (reader.NodeType)
			{
			case XmlNodeType.Attribute:
				return !moveIfOnContentNode;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				if (!moveIfOnContentNode)
				{
					return true;
				}
				goto IL_0078;
			case XmlNodeType.EntityReference:
				if (!reader.CanResolveEntity)
				{
					break;
				}
				reader.ResolveEntity();
				goto IL_0078;
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.EndEntity:
				goto IL_0078;
			}
			return false;
			IL_0078:
			moveIfOnContentNode = false;
		}
		while (reader.Read());
		return false;
	}

	internal async Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count)
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
		switch (state)
		{
		case State.None:
			if (!reader.CanReadContentAs())
			{
				throw reader.CreateReadContentAsException("ReadContentAsBase64");
			}
			if (!(await InitAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				return 0;
			}
			break;
		case State.InReadContent:
			if (decoder == base64Decoder)
			{
				return await ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			}
			break;
		case State.InReadElementContent:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		default:
			return 0;
		}
		InitBase64Decoder();
		return await ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal async Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count)
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
		switch (state)
		{
		case State.None:
			if (!reader.CanReadContentAs())
			{
				throw reader.CreateReadContentAsException("ReadContentAsBinHex");
			}
			if (!(await InitAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				return 0;
			}
			break;
		case State.InReadContent:
			if (decoder == binHexDecoder)
			{
				return await ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			}
			break;
		case State.InReadElementContent:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		default:
			return 0;
		}
		InitBinHexDecoder();
		return await ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal async Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count)
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
		switch (state)
		{
		case State.None:
			if (reader.NodeType != XmlNodeType.Element)
			{
				throw reader.CreateReadElementContentAsException("ReadElementContentAsBase64");
			}
			if (!(await InitOnElementAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				return 0;
			}
			break;
		case State.InReadContent:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		case State.InReadElementContent:
			if (decoder == base64Decoder)
			{
				return await ReadElementContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			}
			break;
		default:
			return 0;
		}
		InitBase64Decoder();
		return await ReadElementContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal async Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count)
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
		switch (state)
		{
		case State.None:
			if (reader.NodeType != XmlNodeType.Element)
			{
				throw reader.CreateReadElementContentAsException("ReadElementContentAsBinHex");
			}
			if (!(await InitOnElementAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				return 0;
			}
			break;
		case State.InReadContent:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		case State.InReadElementContent:
			if (decoder == binHexDecoder)
			{
				return await ReadElementContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			}
			break;
		default:
			return 0;
		}
		InitBinHexDecoder();
		return await ReadElementContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal async Task FinishAsync()
	{
		if (state != State.None)
		{
			while (await MoveToNextContentNodeAsync(moveIfOnContentNode: true).ConfigureAwait(continueOnCapturedContext: false))
			{
			}
			if (state == State.InReadElementContent)
			{
				if (reader.NodeType != XmlNodeType.EndElement)
				{
					throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
				}
				await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		Reset();
	}

	private async Task<bool> InitAsync()
	{
		if (!(await MoveToNextContentNodeAsync(moveIfOnContentNode: false).ConfigureAwait(continueOnCapturedContext: false)))
		{
			return false;
		}
		state = State.InReadContent;
		isEnd = false;
		return true;
	}

	private async Task<bool> InitOnElementAsync()
	{
		bool isEmpty = reader.IsEmptyElement;
		await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (isEmpty)
		{
			return false;
		}
		if (!(await MoveToNextContentNodeAsync(moveIfOnContentNode: false).ConfigureAwait(continueOnCapturedContext: false)))
		{
			if (reader.NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
			}
			await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			return false;
		}
		state = State.InReadElementContent;
		isEnd = false;
		return true;
	}

	private async Task<int> ReadContentAsBinaryAsync(byte[] buffer, int index, int count)
	{
		if (isEnd)
		{
			Reset();
			return 0;
		}
		decoder.SetNextOutputBuffer(buffer, index, count);
		do
		{
			if (canReadValueChunk)
			{
				while (true)
				{
					if (valueOffset < valueChunkLength)
					{
						int num = decoder.Decode(valueChunk, valueOffset, valueChunkLength - valueOffset);
						valueOffset += num;
					}
					if (decoder.IsFull)
					{
						return decoder.DecodedCount;
					}
					if ((valueChunkLength = await reader.ReadValueChunkAsync(valueChunk, 0, 256).ConfigureAwait(continueOnCapturedContext: false)) == 0)
					{
						break;
					}
					valueOffset = 0;
				}
			}
			else
			{
				string text = await reader.GetValueAsync().ConfigureAwait(continueOnCapturedContext: false);
				int num2 = decoder.Decode(text, valueOffset, text.Length - valueOffset);
				valueOffset += num2;
				if (decoder.IsFull)
				{
					return decoder.DecodedCount;
				}
			}
			valueOffset = 0;
		}
		while (await MoveToNextContentNodeAsync(moveIfOnContentNode: true).ConfigureAwait(continueOnCapturedContext: false));
		isEnd = true;
		return decoder.DecodedCount;
	}

	private async Task<int> ReadElementContentAsBinaryAsync(byte[] buffer, int index, int count)
	{
		if (count == 0)
		{
			return 0;
		}
		int num = await ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		if (num > 0)
		{
			return num;
		}
		if (reader.NodeType != XmlNodeType.EndElement)
		{
			throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
		}
		await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		state = State.None;
		return 0;
	}

	private async Task<bool> MoveToNextContentNodeAsync(bool moveIfOnContentNode)
	{
		do
		{
			switch (reader.NodeType)
			{
			case XmlNodeType.Attribute:
				return !moveIfOnContentNode;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				if (!moveIfOnContentNode)
				{
					return true;
				}
				goto IL_00a5;
			case XmlNodeType.EntityReference:
				if (!reader.CanResolveEntity)
				{
					break;
				}
				reader.ResolveEntity();
				goto IL_00a5;
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.EndEntity:
				goto IL_00a5;
			}
			return false;
			IL_00a5:
			moveIfOnContentNode = false;
		}
		while (await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false));
		return false;
	}
}
