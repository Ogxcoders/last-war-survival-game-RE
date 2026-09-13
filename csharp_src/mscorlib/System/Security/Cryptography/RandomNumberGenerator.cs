using System.Buffers;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography;

[ComVisible(true)]
public abstract class RandomNumberGenerator : IDisposable
{
	public static RandomNumberGenerator Create()
	{
		return Create("System.Security.Cryptography.RandomNumberGenerator");
	}

	public static RandomNumberGenerator Create(string rngName)
	{
		return (RandomNumberGenerator)CryptoConfig.CreateFromName(rngName);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public abstract void GetBytes(byte[] data);

	public virtual void GetBytes(byte[] data, int offset, int count)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("Non-negative number required."));
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("Non-negative number required."));
		}
		if (offset + count > data.Length)
		{
			throw new ArgumentException(Environment.GetResourceString("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."));
		}
		if (count > 0)
		{
			byte[] array = new byte[count];
			GetBytes(array);
			Array.Copy(array, 0, data, offset, count);
		}
	}

	public virtual void GetNonZeroBytes(byte[] data)
	{
		throw new NotImplementedException();
	}

	public static void Fill(Span<byte> data)
	{
		FillSpan(data);
	}

	internal unsafe static void FillSpan(Span<byte> data)
	{
		if (data.Length > 0)
		{
			fixed (byte* buffer = data)
			{
				Interop.GetRandomBytes(buffer, data.Length);
			}
		}
	}

	public virtual void GetBytes(Span<byte> data)
	{
		byte[] array = ArrayPool<byte>.Shared.Rent(data.Length);
		try
		{
			GetBytes(array, 0, data.Length);
			new ReadOnlySpan<byte>(array, 0, data.Length).CopyTo(data);
		}
		finally
		{
			Array.Clear(array, 0, data.Length);
			ArrayPool<byte>.Shared.Return(array);
		}
	}

	public virtual void GetNonZeroBytes(Span<byte> data)
	{
		byte[] array = ArrayPool<byte>.Shared.Rent(data.Length);
		try
		{
			GetNonZeroBytes(array);
			new ReadOnlySpan<byte>(array, 0, data.Length).CopyTo(data);
		}
		finally
		{
			Array.Clear(array, 0, data.Length);
			ArrayPool<byte>.Shared.Return(array);
		}
	}

	public static int GetInt32(int fromInclusive, int toExclusive)
	{
		if (fromInclusive >= toExclusive)
		{
			throw new ArgumentException("Range of random number does not contain at least one possibility.");
		}
		uint num = (uint)(toExclusive - fromInclusive - 1);
		if (num == 0)
		{
			return fromInclusive;
		}
		uint num2 = num;
		num2 |= num2 >> 1;
		num2 |= num2 >> 2;
		num2 |= num2 >> 4;
		num2 |= num2 >> 8;
		num2 |= num2 >> 16;
		Span<uint> span = stackalloc uint[1];
		uint num3;
		do
		{
			FillSpan(MemoryMarshal.AsBytes(span));
			num3 = num2 & span[0];
		}
		while (num3 > num);
		return (int)num3 + fromInclusive;
	}

	public static int GetInt32(int toExclusive)
	{
		if (toExclusive <= 0)
		{
			throw new ArgumentOutOfRangeException("toExclusive", "Positive number required.");
		}
		return GetInt32(0, toExclusive);
	}
}
