using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net;

internal sealed class Base64Stream : DelegatedStream, IEncodableStream
{
	private sealed class ReadAsyncResult : LazyAsyncResult
	{
		private readonly Base64Stream _parent;

		private readonly byte[] _buffer;

		private readonly int _offset;

		private readonly int _count;

		private int _read;

		private static readonly AsyncCallback s_onRead = OnRead;

		internal ReadAsyncResult(Base64Stream parent, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			: base(null, state, callback)
		{
			_parent = parent;
			_buffer = buffer;
			_offset = offset;
			_count = count;
		}

		private bool CompleteRead(IAsyncResult result)
		{
			_read = _parent.BaseStream.EndRead(result);
			if (_read == 0)
			{
				InvokeCallback();
				return true;
			}
			_read = _parent.DecodeBytes(_buffer, _offset, _read);
			if (_read > 0)
			{
				InvokeCallback();
				return true;
			}
			return false;
		}

		internal void Read()
		{
			IAsyncResult asyncResult;
			do
			{
				asyncResult = _parent.BaseStream.BeginRead(_buffer, _offset, _count, s_onRead, this);
			}
			while (asyncResult.CompletedSynchronously && !CompleteRead(asyncResult));
		}

		private static void OnRead(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			ReadAsyncResult readAsyncResult = (ReadAsyncResult)result.AsyncState;
			try
			{
				if (!readAsyncResult.CompleteRead(result))
				{
					readAsyncResult.Read();
				}
			}
			catch (Exception result2)
			{
				if (readAsyncResult.IsCompleted)
				{
					throw;
				}
				readAsyncResult.InvokeCallback(result2);
			}
		}

		internal static int End(IAsyncResult result)
		{
			ReadAsyncResult obj = (ReadAsyncResult)result;
			obj.InternalWaitForCompletion();
			return obj._read;
		}
	}

	private sealed class WriteAsyncResult : LazyAsyncResult
	{
		private static readonly AsyncCallback s_onWrite = OnWrite;

		private readonly Base64Stream _parent;

		private readonly byte[] _buffer;

		private readonly int _offset;

		private readonly int _count;

		private int _written;

		internal WriteAsyncResult(Base64Stream parent, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			: base(null, state, callback)
		{
			_parent = parent;
			_buffer = buffer;
			_offset = offset;
			_count = count;
		}

		internal void Write()
		{
			while (true)
			{
				_written += _parent.EncodeBytes(_buffer, _offset + _written, _count - _written, dontDeferFinalBytes: false, shouldAppendSpaceToCRLF: false);
				if (_written < _count)
				{
					IAsyncResult asyncResult = _parent.BaseStream.BeginWrite(_parent.WriteState.Buffer, 0, _parent.WriteState.Length, s_onWrite, this);
					if (asyncResult.CompletedSynchronously)
					{
						CompleteWrite(asyncResult);
						continue;
					}
					break;
				}
				InvokeCallback();
				break;
			}
		}

		private void CompleteWrite(IAsyncResult result)
		{
			_parent.BaseStream.EndWrite(result);
			_parent.WriteState.Reset();
		}

		private static void OnWrite(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			WriteAsyncResult writeAsyncResult = (WriteAsyncResult)result.AsyncState;
			try
			{
				writeAsyncResult.CompleteWrite(result);
				writeAsyncResult.Write();
			}
			catch (Exception result2)
			{
				if (writeAsyncResult.IsCompleted)
				{
					throw;
				}
				writeAsyncResult.InvokeCallback(result2);
			}
		}

		internal static void End(IAsyncResult result)
		{
			((WriteAsyncResult)result).InternalWaitForCompletion();
		}
	}

	private sealed class ReadStateInfo
	{
		internal byte Val { get; set; }

		internal byte Pos { get; set; }
	}

	private static readonly byte[] s_base64DecodeMap = new byte[256]
	{
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 62, 255, 255, 255, 63, 52, 53,
		54, 55, 56, 57, 58, 59, 60, 61, 255, 255,
		255, 255, 255, 255, 255, 0, 1, 2, 3, 4,
		5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
		15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
		25, 255, 255, 255, 255, 255, 255, 26, 27, 28,
		29, 30, 31, 32, 33, 34, 35, 36, 37, 38,
		39, 40, 41, 42, 43, 44, 45, 46, 47, 48,
		49, 50, 51, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255
	};

	private static readonly byte[] s_base64EncodeMap = new byte[65]
	{
		65, 66, 67, 68, 69, 70, 71, 72, 73, 74,
		75, 76, 77, 78, 79, 80, 81, 82, 83, 84,
		85, 86, 87, 88, 89, 90, 97, 98, 99, 100,
		101, 102, 103, 104, 105, 106, 107, 108, 109, 110,
		111, 112, 113, 114, 115, 116, 117, 118, 119, 120,
		121, 122, 48, 49, 50, 51, 52, 53, 54, 55,
		56, 57, 43, 47, 61
	};

	private readonly int _lineLength;

	private readonly Base64WriteStateInfo _writeState;

	private ReadStateInfo _readState;

	private const int SizeOfBase64EncodedChar = 4;

	private const byte InvalidBase64Value = byte.MaxValue;

	private ReadStateInfo ReadState => _readState ?? (_readState = new ReadStateInfo());

	internal Base64WriteStateInfo WriteState => _writeState;

	internal Base64Stream(Stream stream, Base64WriteStateInfo writeStateInfo)
		: base(stream)
	{
		_writeState = new Base64WriteStateInfo();
		_lineLength = writeStateInfo.MaxLineLength;
	}

	internal Base64Stream(Base64WriteStateInfo writeStateInfo)
		: base(new MemoryStream())
	{
		_lineLength = writeStateInfo.MaxLineLength;
		_writeState = writeStateInfo;
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0 || offset > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (offset + count > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		ReadAsyncResult readAsyncResult = new ReadAsyncResult(this, buffer, offset, count, callback, state);
		readAsyncResult.Read();
		return readAsyncResult;
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0 || offset > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (offset + count > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		WriteAsyncResult writeAsyncResult = new WriteAsyncResult(this, buffer, offset, count, callback, state);
		writeAsyncResult.Write();
		return writeAsyncResult;
	}

	public override void Close()
	{
		if (_writeState != null && WriteState.Length > 0)
		{
			switch (WriteState.Padding)
			{
			case 1:
				WriteState.Append(s_base64EncodeMap[WriteState.LastBits], s_base64EncodeMap[64]);
				break;
			case 2:
				WriteState.Append(s_base64EncodeMap[WriteState.LastBits], s_base64EncodeMap[64], s_base64EncodeMap[64]);
				break;
			}
			WriteState.Padding = 0;
			FlushInternal();
		}
		base.Close();
	}

	public unsafe int DecodeBytes(byte[] buffer, int offset, int count)
	{
		fixed (byte* ptr = buffer)
		{
			byte* ptr2 = ptr + offset;
			byte* ptr3 = ptr2;
			byte* ptr4 = ptr2;
			byte* ptr5 = ptr2 + count;
			while (ptr3 < ptr5)
			{
				if (*ptr3 == 13 || *ptr3 == 10 || *ptr3 == 61 || *ptr3 == 32 || *ptr3 == 9)
				{
					ptr3++;
					continue;
				}
				byte b = s_base64DecodeMap[*ptr3];
				if (b == byte.MaxValue)
				{
					throw new FormatException("An invalid character was found in the Base-64 stream.");
				}
				switch (ReadState.Pos)
				{
				case 0:
					ReadState.Val = (byte)(b << 2);
					ReadState.Pos++;
					break;
				case 1:
					*(ptr4++) = (byte)(ReadState.Val + (b >> 4));
					ReadState.Val = (byte)(b << 4);
					ReadState.Pos++;
					break;
				case 2:
					*(ptr4++) = (byte)(ReadState.Val + (b >> 2));
					ReadState.Val = (byte)(b << 6);
					ReadState.Pos++;
					break;
				case 3:
					*(ptr4++) = (byte)(ReadState.Val + b);
					ReadState.Pos = 0;
					break;
				}
				ptr3++;
			}
			return (int)(ptr4 - ptr2);
		}
	}

	public int EncodeBytes(byte[] buffer, int offset, int count)
	{
		return EncodeBytes(buffer, offset, count, dontDeferFinalBytes: true, shouldAppendSpaceToCRLF: true);
	}

	internal int EncodeBytes(byte[] buffer, int offset, int count, bool dontDeferFinalBytes, bool shouldAppendSpaceToCRLF)
	{
		WriteState.AppendHeader();
		int i = offset;
		switch (WriteState.Padding)
		{
		case 2:
			WriteState.Append(s_base64EncodeMap[WriteState.LastBits | ((buffer[i] & 0xF0) >> 4)]);
			if (count == 1)
			{
				WriteState.LastBits = (byte)((buffer[i] & 0xF) << 2);
				WriteState.Padding = 1;
				i++;
				return i - offset;
			}
			WriteState.Append(s_base64EncodeMap[((buffer[i] & 0xF) << 2) | ((buffer[i + 1] & 0xC0) >> 6)]);
			WriteState.Append(s_base64EncodeMap[buffer[i + 1] & 0x3F]);
			i += 2;
			count -= 2;
			WriteState.Padding = 0;
			break;
		case 1:
			WriteState.Append(s_base64EncodeMap[WriteState.LastBits | ((buffer[i] & 0xC0) >> 6)]);
			WriteState.Append(s_base64EncodeMap[buffer[i] & 0x3F]);
			i++;
			count--;
			WriteState.Padding = 0;
			break;
		}
		int num;
		for (num = i + (count - count % 3); i < num; i += 3)
		{
			if (_lineLength != -1 && WriteState.CurrentLineLength + 4 + _writeState.FooterLength > _lineLength)
			{
				WriteState.AppendCRLF(shouldAppendSpaceToCRLF);
			}
			WriteState.Append(s_base64EncodeMap[(buffer[i] & 0xFC) >> 2]);
			WriteState.Append(s_base64EncodeMap[((buffer[i] & 3) << 4) | ((buffer[i + 1] & 0xF0) >> 4)]);
			WriteState.Append(s_base64EncodeMap[((buffer[i + 1] & 0xF) << 2) | ((buffer[i + 2] & 0xC0) >> 6)]);
			WriteState.Append(s_base64EncodeMap[buffer[i + 2] & 0x3F]);
		}
		i = num;
		if (count % 3 != 0 && _lineLength != -1 && WriteState.CurrentLineLength + 4 + _writeState.FooterLength >= _lineLength)
		{
			WriteState.AppendCRLF(shouldAppendSpaceToCRLF);
		}
		switch (count % 3)
		{
		case 2:
			WriteState.Append(s_base64EncodeMap[(buffer[i] & 0xFC) >> 2]);
			WriteState.Append(s_base64EncodeMap[((buffer[i] & 3) << 4) | ((buffer[i + 1] & 0xF0) >> 4)]);
			if (dontDeferFinalBytes)
			{
				WriteState.Append(s_base64EncodeMap[(buffer[i + 1] & 0xF) << 2]);
				WriteState.Append(s_base64EncodeMap[64]);
				WriteState.Padding = 0;
			}
			else
			{
				WriteState.LastBits = (byte)((buffer[i + 1] & 0xF) << 2);
				WriteState.Padding = 1;
			}
			i += 2;
			break;
		case 1:
			WriteState.Append(s_base64EncodeMap[(buffer[i] & 0xFC) >> 2]);
			if (dontDeferFinalBytes)
			{
				WriteState.Append(s_base64EncodeMap[(byte)((buffer[i] & 3) << 4)]);
				WriteState.Append(s_base64EncodeMap[64]);
				WriteState.Append(s_base64EncodeMap[64]);
				WriteState.Padding = 0;
			}
			else
			{
				WriteState.LastBits = (byte)((buffer[i] & 3) << 4);
				WriteState.Padding = 2;
			}
			i++;
			break;
		}
		WriteState.AppendFooter();
		return i - offset;
	}

	public Stream GetStream()
	{
		return this;
	}

	public string GetEncodedString()
	{
		return Encoding.ASCII.GetString(WriteState.Buffer, 0, WriteState.Length);
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		if (asyncResult == null)
		{
			throw new ArgumentNullException("asyncResult");
		}
		return ReadAsyncResult.End(asyncResult);
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		if (asyncResult == null)
		{
			throw new ArgumentNullException("asyncResult");
		}
		WriteAsyncResult.End(asyncResult);
	}

	public override void Flush()
	{
		if (_writeState != null && WriteState.Length > 0)
		{
			FlushInternal();
		}
		base.Flush();
	}

	private void FlushInternal()
	{
		base.Write(WriteState.Buffer, 0, WriteState.Length);
		WriteState.Reset();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0 || offset > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (offset + count > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		int num;
		do
		{
			num = base.Read(buffer, offset, count);
			if (num == 0)
			{
				return 0;
			}
			num = DecodeBytes(buffer, offset, num);
		}
		while (num <= 0);
		return num;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0 || offset > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (offset + count > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		int num = 0;
		while (true)
		{
			num += EncodeBytes(buffer, offset + num, count - num, dontDeferFinalBytes: false, shouldAppendSpaceToCRLF: false);
			if (num < count)
			{
				FlushInternal();
				continue;
			}
			break;
		}
	}
}
