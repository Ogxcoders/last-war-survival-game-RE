using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.SqlClient.SNI;

internal sealed class SslOverTdsStream : Stream
{
	private readonly Stream _stream;

	private int _packetBytes;

	private bool _encapsulate;

	private const int PACKET_SIZE_WITHOUT_HEADER = 4088;

	private const int PRELOGIN_PACKET_TYPE = 18;

	public override long Position
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public override bool CanRead => _stream.CanRead;

	public override bool CanWrite => _stream.CanWrite;

	public override bool CanSeek => false;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public SslOverTdsStream(Stream stream)
	{
		_stream = stream;
		_encapsulate = true;
	}

	public void FinishHandshake()
	{
		_encapsulate = false;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return ReadInternal(buffer, offset, count, CancellationToken.None, async: false).GetAwaiter().GetResult();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		WriteInternal(buffer, offset, count, CancellationToken.None, async: false).Wait();
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken token)
	{
		return WriteInternal(buffer, offset, count, token, async: true);
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken token)
	{
		return ReadInternal(buffer, offset, count, token, async: true);
	}

	private async Task<int> ReadInternal(byte[] buffer, int offset, int count, CancellationToken token, bool async)
	{
		int num = 0;
		byte[] packetData = new byte[(count < 8) ? 8 : count];
		int num3;
		if (_encapsulate)
		{
			if (_packetBytes == 0)
			{
				while (num < 8)
				{
					int num2 = num;
					num3 = ((!async) ? _stream.Read(packetData, num, 8 - num) : (await _stream.ReadAsync(packetData, num, 8 - num, token).ConfigureAwait(continueOnCapturedContext: false)));
					num = num2 + num3;
				}
				_packetBytes = (packetData[2] << 8) | packetData[3];
				_packetBytes -= 8;
			}
			if (count > _packetBytes)
			{
				count = _packetBytes;
			}
		}
		num3 = ((!async) ? _stream.Read(packetData, 0, count) : (await _stream.ReadAsync(packetData, 0, count, token).ConfigureAwait(continueOnCapturedContext: false)));
		num = num3;
		if (_encapsulate)
		{
			_packetBytes -= num;
		}
		Buffer.BlockCopy(packetData, 0, buffer, offset, num);
		return num;
	}

	private async Task WriteInternal(byte[] buffer, int offset, int count, CancellationToken token, bool async)
	{
		int currentOffset = offset;
		while (count > 0)
		{
			int currentCount;
			if (_encapsulate)
			{
				currentCount = ((count <= 4088) ? count : 4088);
				count -= currentCount;
				byte[] array = new byte[8 + currentCount];
				array[0] = 18;
				array[1] = ((count <= 0) ? ((byte)1) : ((byte)0));
				array[2] = (byte)((currentCount + 8) / 256);
				array[3] = (byte)((currentCount + 8) % 256);
				array[4] = 0;
				array[5] = 0;
				array[6] = 0;
				array[7] = 0;
				for (int i = 8; i < array.Length; i++)
				{
					array[i] = buffer[currentOffset + (i - 8)];
				}
				if (async)
				{
					await _stream.WriteAsync(array, 0, array.Length, token).ConfigureAwait(continueOnCapturedContext: false);
				}
				else
				{
					_stream.Write(array, 0, array.Length);
				}
			}
			else
			{
				currentCount = count;
				count = 0;
				if (async)
				{
					await _stream.WriteAsync(buffer, currentOffset, currentCount, token).ConfigureAwait(continueOnCapturedContext: false);
				}
				else
				{
					_stream.Write(buffer, currentOffset, currentCount);
				}
			}
			if (async)
			{
				await _stream.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				_stream.Flush();
			}
			currentOffset += currentCount;
		}
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void Flush()
	{
		if (!(_stream is PipeStream))
		{
			_stream.Flush();
		}
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}
}
