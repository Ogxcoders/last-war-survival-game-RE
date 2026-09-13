using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ZstdNet;

public class CompressionStream : Stream
{
	private readonly Stream innerStream;

	private readonly byte[] outputBuffer;

	private readonly int bufferSize;

	private IntPtr cStream;

	private UIntPtr pos;

	public readonly CompressionOptions Options;

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

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

	public CompressionStream(Stream stream)
		: this(stream, CompressionOptions.Default)
	{
	}

	public CompressionStream(Stream stream, int bufferSize)
		: this(stream, CompressionOptions.Default, bufferSize)
	{
	}

	public CompressionStream(Stream stream, CompressionOptions options, int bufferSize = 0)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (!stream.CanWrite)
		{
			throw new ArgumentException("Stream is not writable", "stream");
		}
		if (bufferSize < 0)
		{
			throw new ArgumentOutOfRangeException("bufferSize");
		}
		innerStream = stream;
		cStream = ExternMethods.ZSTD_createCStream().EnsureZstdSuccess();
		ExternMethods.ZSTD_CCtx_reset(cStream, ExternMethods.ZSTD_ResetDirective.ZSTD_reset_session_only).EnsureZstdSuccess();
		Options = options;
		if (options != null)
		{
			options.ApplyCompressionParams(cStream);
			if (options.Cdict != IntPtr.Zero)
			{
				ExternMethods.ZSTD_CCtx_refCDict(cStream, options.Cdict).EnsureZstdSuccess();
			}
		}
		this.bufferSize = ((bufferSize > 0) ? bufferSize : ((int)(uint)ExternMethods.ZSTD_CStreamOutSize().EnsureZstdSuccess()));
		outputBuffer = ArrayPool<byte>.Shared.Rent(this.bufferSize);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		EnsureParamsValid(buffer, offset, count);
		EnsureNotDisposed();
		WriteInternal(new Span<byte>(buffer, offset, count));
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		EnsureParamsValid(buffer, offset, count);
		EnsureNotDisposed();
		return WriteInternalAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken);
	}

	private void WriteInternal(ReadOnlySpan<byte> buffer)
	{
		if (buffer.Length == 0)
		{
			return;
		}
		ExternMethods.ZSTD_Buffer input = new ExternMethods.ZSTD_Buffer(UIntPtr.Zero, (UIntPtr)(ulong)buffer.Length);
		ExternMethods.ZSTD_Buffer output = new ExternMethods.ZSTD_Buffer(pos, (UIntPtr)(ulong)bufferSize);
		ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(outputBuffer, 0, bufferSize);
		do
		{
			if (output.IsFullyConsumed)
			{
				FlushOutputBuffer(readOnlySpan.Slice(0, (int)(uint)output.pos));
				output.pos = UIntPtr.Zero;
			}
			Compress(buffer, ref output, ref input, ExternMethods.ZSTD_EndDirective.ZSTD_e_continue);
		}
		while (!input.IsFullyConsumed);
		pos = output.pos;
	}

	private async Task WriteInternalAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
	{
		if (buffer.Length == 0)
		{
			return;
		}
		ExternMethods.ZSTD_Buffer input = new ExternMethods.ZSTD_Buffer(UIntPtr.Zero, (UIntPtr)(ulong)buffer.Length);
		ExternMethods.ZSTD_Buffer output = new ExternMethods.ZSTD_Buffer(pos, (UIntPtr)(ulong)bufferSize);
		do
		{
			if (output.IsFullyConsumed)
			{
				await FlushOutputBufferAsync(ref output, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				output.pos = UIntPtr.Zero;
			}
			Compress(buffer.Span, ref output, ref input, ExternMethods.ZSTD_EndDirective.ZSTD_e_continue);
		}
		while (!input.IsFullyConsumed);
		pos = output.pos;
	}

	private unsafe UIntPtr Compress(ReadOnlySpan<byte> buffer, ref ExternMethods.ZSTD_Buffer output, ref ExternMethods.ZSTD_Buffer input, ExternMethods.ZSTD_EndDirective directive)
	{
		fixed (byte* reference = &MemoryMarshal.GetReference(buffer))
		{
			void* value = reference;
			fixed (byte* ptr = &outputBuffer[0])
			{
				void* value2 = ptr;
				input.buffer = new IntPtr(value);
				output.buffer = new IntPtr(value2);
				return ExternMethods.ZSTD_compressStream2(cStream, ref output, ref input, directive).EnsureZstdSuccess();
			}
		}
	}

	private void FlushOutputBuffer(ReadOnlySpan<byte> outputSpan)
	{
		innerStream.Write(outputBuffer, 0, outputSpan.Length);
	}

	private Task FlushOutputBufferAsync(ref ExternMethods.ZSTD_Buffer output, CancellationToken cancellationToken)
	{
		return innerStream.WriteAsync(outputBuffer, 0, (int)(uint)output.pos, cancellationToken);
	}

	~CompressionStream()
	{
		Dispose(disposing: false);
	}

	public override void Flush()
	{
		EnsureNotDisposed();
		FlushCompressStream(ExternMethods.ZSTD_EndDirective.ZSTD_e_flush);
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		EnsureNotDisposed();
		return FlushCompressStreamAsync(ExternMethods.ZSTD_EndDirective.ZSTD_e_flush, cancellationToken);
	}

	private void FlushCompressStream(ExternMethods.ZSTD_EndDirective directive)
	{
		ReadOnlySpan<byte> empty = ReadOnlySpan<byte>.Empty;
		ExternMethods.ZSTD_Buffer input = new ExternMethods.ZSTD_Buffer(UIntPtr.Zero, UIntPtr.Zero);
		ExternMethods.ZSTD_Buffer output = new ExternMethods.ZSTD_Buffer(pos, (UIntPtr)(ulong)bufferSize);
		ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(outputBuffer, 0, bufferSize);
		do
		{
			if (output.IsFullyConsumed)
			{
				FlushOutputBuffer(readOnlySpan.Slice(0, (int)(uint)output.pos));
				output.pos = UIntPtr.Zero;
			}
		}
		while (Compress(empty, ref output, ref input, directive) != UIntPtr.Zero);
		if (output.pos != UIntPtr.Zero)
		{
			FlushOutputBuffer(readOnlySpan.Slice(0, (int)(uint)output.pos));
		}
		pos = UIntPtr.Zero;
	}

	private async Task FlushCompressStreamAsync(ExternMethods.ZSTD_EndDirective directive, CancellationToken cancellationToken)
	{
		ExternMethods.ZSTD_Buffer input = new ExternMethods.ZSTD_Buffer(UIntPtr.Zero, UIntPtr.Zero);
		ExternMethods.ZSTD_Buffer output = new ExternMethods.ZSTD_Buffer(pos, (UIntPtr)(ulong)bufferSize);
		do
		{
			if (output.IsFullyConsumed)
			{
				await FlushOutputBufferAsync(ref output, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				output.pos = UIntPtr.Zero;
			}
		}
		while (Compress(ReadOnlySpan<byte>.Empty, ref output, ref input, directive) != UIntPtr.Zero);
		if (output.pos != UIntPtr.Zero)
		{
			await FlushOutputBufferAsync(ref output, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		pos = UIntPtr.Zero;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	protected override void Dispose(bool disposing)
	{
		if (cStream == IntPtr.Zero)
		{
			return;
		}
		try
		{
			if (disposing)
			{
				FlushCompressStream(ExternMethods.ZSTD_EndDirective.ZSTD_e_end);
			}
		}
		finally
		{
			ExternMethods.ZSTD_freeCStream(cStream);
			if (outputBuffer != null)
			{
				ArrayPool<byte>.Shared.Return(outputBuffer);
			}
			cStream = IntPtr.Zero;
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
		if (cStream == IntPtr.Zero)
		{
			throw new ObjectDisposedException("CompressionStream");
		}
	}
}
