using System.Buffers;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Compression;

public struct BrotliDecoder : IDisposable
{
	private SafeBrotliDecoderHandle _state;

	private bool _disposed;

	internal void InitializeDecoder()
	{
		_state = global::Interop.Brotli.BrotliDecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
		if (_state.IsInvalid)
		{
			throw new IOException("Failed to create BrotliDecoder instance");
		}
	}

	internal void EnsureInitialized()
	{
		EnsureNotDisposed();
		if (_state == null)
		{
			InitializeDecoder();
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
			throw new ObjectDisposedException("BrotliDecoder", "Can not access a closed Decoder.");
		}
	}

	public unsafe OperationStatus Decompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
	{
		EnsureInitialized();
		bytesConsumed = 0;
		bytesWritten = 0;
		if (global::Interop.Brotli.BrotliDecoderIsFinished(_state))
		{
			return OperationStatus.Done;
		}
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
					IntPtr totalOut;
					int num = global::Interop.Brotli.BrotliDecoderDecompressStream(_state, ref availableIn, &ptr, ref availableOut, &ptr2, out totalOut);
					if (num == 0)
					{
						return OperationStatus.InvalidData;
					}
					bytesConsumed += source.Length - (int)availableIn;
					bytesWritten += destination.Length - (int)availableOut;
					switch (num)
					{
					case 1:
						return OperationStatus.Done;
					case 3:
						return OperationStatus.DestinationTooSmall;
					}
					source = source.Slice(source.Length - (int)availableIn);
					destination = destination.Slice(destination.Length - (int)availableOut);
					if (num == 2 && source.Length == 0)
					{
						return OperationStatus.NeedMoreData;
					}
				}
			}
		}
		return OperationStatus.DestinationTooSmall;
	}

	public unsafe static bool TryDecompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
	{
		fixed (byte* reference = &MemoryMarshal.GetReference(source))
		{
			fixed (byte* reference2 = &MemoryMarshal.GetReference(destination))
			{
				IntPtr availableOutput = (IntPtr)destination.Length;
				bool result = global::Interop.Brotli.BrotliDecoderDecompress((IntPtr)source.Length, reference, ref availableOutput, reference2);
				bytesWritten = (int)availableOutput;
				return result;
			}
		}
	}
}
