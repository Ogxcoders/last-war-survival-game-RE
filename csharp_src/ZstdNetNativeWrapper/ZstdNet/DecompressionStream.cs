using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ZstdNet;

public class DecompressionStream : Stream
{
	private readonly Stream innerStream;

	private readonly byte[] inputBuffer;

	private readonly int bufferSize;

	private IntPtr dStream;

	private UIntPtr pos;

	private UIntPtr size;

	public readonly DecompressionOptions Options;

	public override bool CanRead => true;

	public override bool CanSeek => false;

	public override bool CanWrite => false;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

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

	public DecompressionStream(Stream stream)
		: this(stream, null)
	{
	}

	public DecompressionStream(Stream stream, int bufferSize)
		: this(stream, null, bufferSize)
	{
	}

	public DecompressionStream(Stream stream, DecompressionOptions options, int bufferSize = 0)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (!stream.CanRead)
		{
			throw new ArgumentException("Stream is not readable", "stream");
		}
		if (bufferSize < 0)
		{
			throw new ArgumentOutOfRangeException("bufferSize");
		}
		innerStream = stream;
		dStream = ExternMethods.ZSTD_createDStream().EnsureZstdSuccess();
		ExternMethods.ZSTD_DCtx_reset(dStream, ExternMethods.ZSTD_ResetDirective.ZSTD_reset_session_only).EnsureZstdSuccess();
		Options = options;
		if (options != null)
		{
			options.ApplyDecompressionParams(dStream);
			if (options.Ddict != IntPtr.Zero)
			{
				ExternMethods.ZSTD_DCtx_refDDict(dStream, options.Ddict).EnsureZstdSuccess();
			}
		}
		this.bufferSize = ((bufferSize > 0) ? bufferSize : ((int)(uint)ExternMethods.ZSTD_DStreamInSize().EnsureZstdSuccess()));
		inputBuffer = ArrayPool<byte>.Shared.Rent(this.bufferSize);
		pos = (size = (UIntPtr)(ulong)this.bufferSize);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		EnsureParamsValid(buffer, offset, count);
		EnsureNotDisposed();
		return ReadInternal(new Span<byte>(buffer, offset, count));
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		EnsureParamsValid(buffer, offset, count);
		EnsureNotDisposed();
		return ReadInternalAsync(new Memory<byte>(buffer, offset, count), cancellationToken);
	}

	private int ReadInternal(Span<byte> buffer)
	{
		ExternMethods.ZSTD_Buffer input = new ExternMethods.ZSTD_Buffer(pos, size);
		ExternMethods.ZSTD_Buffer output = new ExternMethods.ZSTD_Buffer(UIntPtr.Zero, (UIntPtr)(ulong)buffer.Length);
		Span<byte> inputSpan = new Span<byte>(inputBuffer, 0, bufferSize);
		while (!output.IsFullyConsumed && (!input.IsFullyConsumed || FillInputBuffer(inputSpan, ref input) > 0))
		{
			Decompress(buffer, ref output, ref input);
		}
		pos = input.pos;
		size = input.size;
		return (int)(uint)output.pos;
	}

	private async Task<int> ReadInternalAsync(Memory<byte> buffer, CancellationToken cancellationToken)
	{
		ExternMethods.ZSTD_Buffer input = new ExternMethods.ZSTD_Buffer(pos, size);
		ExternMethods.ZSTD_Buffer output = new ExternMethods.ZSTD_Buffer(UIntPtr.Zero, (UIntPtr)(ulong)buffer.Length);
		while (!output.IsFullyConsumed)
		{
			if (input.IsFullyConsumed)
			{
				int num;
				if ((num = await innerStream.ReadAsync(inputBuffer, 0, bufferSize, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)) == 0)
				{
					break;
				}
				input.size = (UIntPtr)(ulong)num;
				input.pos = UIntPtr.Zero;
			}
			Decompress(buffer.Span, ref output, ref input);
		}
		pos = input.pos;
		size = input.size;
		return (int)(uint)output.pos;
	}

	private unsafe void Decompress(Span<byte> buffer, ref ExternMethods.ZSTD_Buffer output, ref ExternMethods.ZSTD_Buffer input)
	{
		fixed (byte* ptr = &inputBuffer[0])
		{
			void* value = ptr;
			fixed (byte* reference = &MemoryMarshal.GetReference(buffer))
			{
				void* value2 = reference;
				input.buffer = new IntPtr(value);
				output.buffer = new IntPtr(value2);
				ExternMethods.ZSTD_decompressStream(dStream, ref output, ref input).EnsureZstdSuccess();
			}
		}
	}

	private int FillInputBuffer(Span<byte> inputSpan, ref ExternMethods.ZSTD_Buffer input)
	{
		int num = innerStream.Read(inputBuffer, 0, inputSpan.Length);
		input.size = (UIntPtr)(ulong)num;
		input.pos = UIntPtr.Zero;
		return num;
	}

	~DecompressionStream()
	{
		Dispose(disposing: false);
	}

	public override void Flush()
	{
		throw new NotSupportedException();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	protected override void Dispose(bool disposing)
	{
		if (!(dStream == IntPtr.Zero))
		{
			ExternMethods.ZSTD_freeDStream(dStream);
			if (inputBuffer != null)
			{
				ArrayPool<byte>.Shared.Return(inputBuffer);
			}
			dStream = IntPtr.Zero;
		}
	}

	private void EnsureParamsValid(byte[] buffer, int offset, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (count > buffer.Length - offset)
		{
			throw new ArgumentException("The sum of offset and count is greater than the buffer length");
		}
	}

	private void EnsureNotDisposed()
	{
		if (dStream == IntPtr.Zero)
		{
			throw new ObjectDisposedException("DecompressionStream");
		}
	}
}
