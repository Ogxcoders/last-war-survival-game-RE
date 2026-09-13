using System.Buffers;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Compression;

public struct BrotliEncoder : IDisposable
{
	internal SafeBrotliEncoderHandle _state;

	private bool _disposed;

	public BrotliEncoder(int quality, int window)
	{
		_disposed = false;
		_state = global::Interop.Brotli.BrotliEncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
		if (_state.IsInvalid)
		{
			throw new IOException("Failed to create BrotliEncoder instance");
		}
		SetQuality(quality);
		SetWindow(window);
	}

	internal void InitializeEncoder()
	{
		EnsureNotDisposed();
		_state = global::Interop.Brotli.BrotliEncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
		if (_state.IsInvalid)
		{
			throw new IOException("Failed to create BrotliEncoder instance");
		}
	}

	internal void EnsureInitialized()
	{
		EnsureNotDisposed();
		if (_state == null)
		{
			InitializeEncoder();
		}
	}

	public void Dispose()
	{
		_disposed = true;
		_state?.Dispose();
	}

	private void EnsureNotDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("BrotliEncoder", "Can not access a closed Encoder.");
		}
	}

	internal void SetQuality(int quality)
	{
		EnsureNotDisposed();
		if (_state == null || _state.IsInvalid || _state.IsClosed)
		{
			InitializeEncoder();
		}
		if (quality < 0 || quality > 11)
		{
			throw new ArgumentOutOfRangeException("quality", global::SR.Format("Provided BrotliEncoder Quality of {0} is not between the minimum value of {1} and the maximum value of {2}", quality, 0, 11));
		}
		if (!global::Interop.Brotli.BrotliEncoderSetParameter(_state, BrotliEncoderParameter.Quality, (uint)quality))
		{
			throw new InvalidOperationException(global::SR.Format("The BrotliEncoder {0} can not be changed at current encoder state.", "Quality"));
		}
	}

	internal void SetWindow(int window)
	{
		EnsureNotDisposed();
		if (_state == null || _state.IsInvalid || _state.IsClosed)
		{
			InitializeEncoder();
		}
		if (window < 10 || window > 24)
		{
			throw new ArgumentOutOfRangeException("window", global::SR.Format("Provided BrotliEncoder Window of {0} is not between the minimum value of {1} and the maximum value of {2}", window, 10, 24));
		}
		if (!global::Interop.Brotli.BrotliEncoderSetParameter(_state, BrotliEncoderParameter.LGWin, (uint)window))
		{
			throw new InvalidOperationException(global::SR.Format("The BrotliEncoder {0} can not be changed at current encoder state.", "Window"));
		}
	}

	public static int GetMaxCompressedLength(int length)
	{
		if (length < 0 || length > 2147483132)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		if (length == 0)
		{
			return 1;
		}
		int num = length >> 24;
		int num2 = (((length & 0xFFFFFF) > 1048576) ? 4 : 3);
		int num3 = 2 + 4 * num + num2 + 1;
		return length + num3;
	}

	internal OperationStatus Flush(Memory<byte> destination, out int bytesWritten)
	{
		return Flush(destination.Span, out bytesWritten);
	}

	public OperationStatus Flush(Span<byte> destination, out int bytesWritten)
	{
		int bytesConsumed;
		return Compress(ReadOnlySpan<byte>.Empty, destination, out bytesConsumed, out bytesWritten, BrotliEncoderOperation.Flush);
	}

	internal OperationStatus Compress(ReadOnlyMemory<byte> source, Memory<byte> destination, out int bytesConsumed, out int bytesWritten, bool isFinalBlock)
	{
		return Compress(source.Span, destination.Span, out bytesConsumed, out bytesWritten, isFinalBlock);
	}

	public OperationStatus Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, bool isFinalBlock)
	{
		return Compress(source, destination, out bytesConsumed, out bytesWritten, isFinalBlock ? BrotliEncoderOperation.Finish : BrotliEncoderOperation.Process);
	}

	internal unsafe OperationStatus Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, BrotliEncoderOperation operation)
	{
		EnsureInitialized();
		bytesWritten = 0;
		bytesConsumed = 0;
		IntPtr availableOut = (IntPtr)destination.Length;
		IntPtr availableIn = (IntPtr)source.Length;
		while ((int)availableOut > 0)
		{
			fixed (byte* reference = &MemoryMarshal.GetReference(source))
			{
				fixed (byte* reference2 = &MemoryMarshal.GetReference(destination))
				{
					byte* ptr = reference;
					byte* ptr2 = reference2;
					if (!global::Interop.Brotli.BrotliEncoderCompressStream(_state, operation, ref availableIn, &ptr, ref availableOut, &ptr2, out var _))
					{
						return OperationStatus.InvalidData;
					}
					bytesConsumed += source.Length - (int)availableIn;
					bytesWritten += destination.Length - (int)availableOut;
					if ((int)availableOut == destination.Length && !global::Interop.Brotli.BrotliEncoderHasMoreOutput(_state) && (int)availableIn == 0)
					{
						return OperationStatus.Done;
					}
					source = source.Slice(source.Length - (int)availableIn);
					destination = destination.Slice(destination.Length - (int)availableOut);
				}
			}
		}
		return OperationStatus.DestinationTooSmall;
	}

	public static bool TryCompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
	{
		return TryCompress(source, destination, out bytesWritten, 11, 22);
	}

	public unsafe static bool TryCompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten, int quality, int window)
	{
		if (quality < 0 || quality > 11)
		{
			throw new ArgumentOutOfRangeException("quality", global::SR.Format("Provided BrotliEncoder Quality of {0} is not between the minimum value of {1} and the maximum value of {2}", quality, 0, 11));
		}
		if (window < 10 || window > 24)
		{
			throw new ArgumentOutOfRangeException("window", global::SR.Format("Provided BrotliEncoder Window of {0} is not between the minimum value of {1} and the maximum value of {2}", window, 10, 24));
		}
		fixed (byte* reference = &MemoryMarshal.GetReference(source))
		{
			fixed (byte* reference2 = &MemoryMarshal.GetReference(destination))
			{
				IntPtr availableOutput = (IntPtr)destination.Length;
				bool result = global::Interop.Brotli.BrotliEncoderCompress(quality, window, 0, (IntPtr)source.Length, reference, ref availableOutput, reference2);
				bytesWritten = (int)availableOutput;
				return result;
			}
		}
	}
}
