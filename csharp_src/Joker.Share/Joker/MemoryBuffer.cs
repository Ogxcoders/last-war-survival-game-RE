using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Joker;

public class MemoryBuffer : MemoryStream, IBufferWriter<byte>
{
	private int origin;

	private static readonly ConcurrentQueue<MemoryBuffer> _msPool = new ConcurrentQueue<MemoryBuffer>();

	public ReadOnlyMemory<byte> WrittenMemory => GetBuffer().AsMemory(origin, (int)Position);

	public ReadOnlySpan<byte> WrittenSpan => GetBuffer().AsSpan(origin, (int)Position);

	public MemoryBuffer()
	{
	}

	public MemoryBuffer(int capacity)
		: base(capacity)
	{
	}

	public MemoryBuffer(byte[] buffer)
		: base(buffer)
	{
	}

	public MemoryBuffer(byte[] buffer, int index, int length)
		: base(buffer, index, length)
	{
		origin = index;
	}

	public static MemoryBuffer Fetch(int size = 0)
	{
		if (_msPool.TryDequeue(out var result))
		{
			return result;
		}
		return new MemoryBuffer(size);
	}

	public static void Recycle(MemoryBuffer memoryBuffer)
	{
		if (memoryBuffer.Capacity <= 131072 && _msPool.Count <= 16)
		{
			memoryBuffer.Seek(0L, SeekOrigin.Begin);
			memoryBuffer.SetLength(0L);
			_msPool.Enqueue(memoryBuffer);
		}
	}

	public void Write<T>(T value) where T : struct
	{
		Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(GetSpan(Unsafe.SizeOf<T>())), value);
		Advance(Unsafe.SizeOf<T>());
	}

	public new void Write(byte[] data, int offset = 0, int count = -1)
	{
		if (count < 0)
		{
			count = data.Length - offset;
		}
		count = Math.Min(count, data.Length - offset);
		Unsafe.CopyBlockUnaligned(ref MemoryMarshal.GetReference(GetSpan(count)), ref data[offset], (uint)count);
		Advance(count);
	}

	public T Read<T>() where T : struct
	{
		if (Length - Position < Unsafe.SizeOf<T>())
		{
			throw new Exception("Read T error: buffer length not enough");
		}
		T result = Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(GetSpan(Unsafe.SizeOf<T>())));
		Advance(Unsafe.SizeOf<T>());
		return result;
	}

	public byte[] Read(int count)
	{
		if (Length - Position < count)
		{
			throw new Exception("Read T error: buffer length not enough");
		}
		Span<byte> span = GetSpan(count);
		byte[] array = new byte[count];
		Unsafe.CopyBlockUnaligned(ref MemoryMarshal.GetReference<byte>(array), ref MemoryMarshal.GetReference(span), (uint)count);
		Advance(count);
		return array;
	}

	public void Advance(int count)
	{
		long num = Position + count;
		if (num > Length)
		{
			SetLength(num);
		}
		Position = num;
	}

	public Memory<byte> GetMemory(int sizeHint = 0)
	{
		if (Length - Position < sizeHint)
		{
			SetLength(Position + sizeHint);
		}
		return GetBuffer().AsMemory((int)Position + origin, (int)(Length - Position));
	}

	public Span<byte> GetSpan(int sizeHint = 0)
	{
		if (Length - Position < sizeHint)
		{
			SetLength(Position + sizeHint);
		}
		return GetBuffer().AsSpan((int)Position + origin, (int)(Length - Position));
	}
}
