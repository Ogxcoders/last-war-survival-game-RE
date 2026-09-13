using System.Runtime.CompilerServices;
using Microsoft.Win32.SafeHandles;

namespace System.Runtime.InteropServices;

public abstract class SafeBuffer : SafeHandleZeroOrMinusOneIsInvalid
{
	private static readonly UIntPtr Uninitialized = ((UIntPtr.Size == 4) ? ((UIntPtr)uint.MaxValue) : ((UIntPtr)ulong.MaxValue));

	private UIntPtr _numBytes;

	[CLSCompliant(false)]
	public ulong ByteLength
	{
		get
		{
			if (_numBytes == Uninitialized)
			{
				throw NotInitialized();
			}
			return (ulong)_numBytes;
		}
	}

	protected SafeBuffer(bool ownsHandle)
		: base(ownsHandle)
	{
		_numBytes = Uninitialized;
	}

	[CLSCompliant(false)]
	public void Initialize(ulong numBytes)
	{
		if (IntPtr.Size == 4 && numBytes > uint.MaxValue)
		{
			throw new ArgumentOutOfRangeException("numBytes", "The number of bytes cannot exceed the virtual address space on a 32 bit machine.");
		}
		if (numBytes >= (ulong)Uninitialized)
		{
			throw new ArgumentOutOfRangeException("numBytes", "The length of the buffer must be less than the maximum UIntPtr value for your platform.");
		}
		_numBytes = (UIntPtr)numBytes;
	}

	[CLSCompliant(false)]
	public void Initialize(uint numElements, uint sizeOfEachElement)
	{
		if (IntPtr.Size == 4 && numElements * sizeOfEachElement > uint.MaxValue)
		{
			throw new ArgumentOutOfRangeException("numBytes", "The number of bytes cannot exceed the virtual address space on a 32 bit machine.");
		}
		if (numElements * sizeOfEachElement >= (ulong)Uninitialized)
		{
			throw new ArgumentOutOfRangeException("numElements", "The length of the buffer must be less than the maximum UIntPtr value for your platform.");
		}
		_numBytes = (UIntPtr)checked(numElements * sizeOfEachElement);
	}

	[CLSCompliant(false)]
	public void Initialize<T>(uint numElements) where T : struct
	{
		Initialize(numElements, AlignedSizeOf<T>());
	}

	[CLSCompliant(false)]
	public unsafe void AcquirePointer(ref byte* pointer)
	{
		if (_numBytes == Uninitialized)
		{
			throw NotInitialized();
		}
		pointer = null;
		bool success = false;
		DangerousAddRef(ref success);
		pointer = (byte*)(void*)handle;
	}

	public void ReleasePointer()
	{
		if (_numBytes == Uninitialized)
		{
			throw NotInitialized();
		}
		DangerousRelease();
	}

	[CLSCompliant(false)]
	public unsafe T Read<T>(ulong byteOffset) where T : struct
	{
		if (_numBytes == Uninitialized)
		{
			throw NotInitialized();
		}
		uint num = SizeOf<T>();
		byte* ptr = (byte*)(void*)handle + byteOffset;
		SpaceCheck(ptr, num);
		T source = default(T);
		bool success = false;
		try
		{
			DangerousAddRef(ref success);
			fixed (byte* dest = &Unsafe.As<T, byte>(ref source))
			{
				Buffer.Memmove(dest, ptr, num);
				return source;
			}
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	[CLSCompliant(false)]
	public unsafe void ReadArray<T>(ulong byteOffset, T[] array, int index, int count) where T : struct
	{
		if (array == null)
		{
			throw new ArgumentNullException("array", "Buffer cannot be null.");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
		}
		if (array.Length - index < count)
		{
			throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
		}
		if (_numBytes == Uninitialized)
		{
			throw NotInitialized();
		}
		uint num = SizeOf<T>();
		uint num2 = AlignedSizeOf<T>();
		byte* ptr = (byte*)(void*)handle + byteOffset;
		SpaceCheck(ptr, checked((ulong)(num2 * count)));
		bool success = false;
		try
		{
			DangerousAddRef(ref success);
			if (count <= 0)
			{
				return;
			}
			fixed (byte* ptr2 = &Unsafe.As<T, byte>(ref array[index]))
			{
				for (int i = 0; i < count; i++)
				{
					Buffer.Memmove(ptr2 + num * i, ptr + num2 * i, num);
				}
			}
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	[CLSCompliant(false)]
	public unsafe void Write<T>(ulong byteOffset, T value) where T : struct
	{
		if (_numBytes == Uninitialized)
		{
			throw NotInitialized();
		}
		uint num = SizeOf<T>();
		byte* ptr = (byte*)(void*)handle + byteOffset;
		SpaceCheck(ptr, num);
		bool success = false;
		try
		{
			DangerousAddRef(ref success);
			fixed (byte* src = &Unsafe.As<T, byte>(ref value))
			{
				Buffer.Memmove(ptr, src, num);
			}
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	[CLSCompliant(false)]
	public unsafe void WriteArray<T>(ulong byteOffset, T[] array, int index, int count) where T : struct
	{
		if (array == null)
		{
			throw new ArgumentNullException("array", "Buffer cannot be null.");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
		}
		if (array.Length - index < count)
		{
			throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
		}
		if (_numBytes == Uninitialized)
		{
			throw NotInitialized();
		}
		uint num = SizeOf<T>();
		uint num2 = AlignedSizeOf<T>();
		byte* ptr = (byte*)(void*)handle + byteOffset;
		SpaceCheck(ptr, checked((ulong)(num2 * count)));
		bool success = false;
		try
		{
			DangerousAddRef(ref success);
			if (count <= 0)
			{
				return;
			}
			fixed (byte* ptr2 = &Unsafe.As<T, byte>(ref array[index]))
			{
				for (int i = 0; i < count; i++)
				{
					Buffer.Memmove(ptr + num2 * i, ptr2 + num * i, num);
				}
			}
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	private unsafe void SpaceCheck(byte* ptr, ulong sizeInBytes)
	{
		if ((ulong)_numBytes < sizeInBytes)
		{
			NotEnoughRoom();
		}
		if ((ulong)(ptr - (byte*)(void*)handle) > (ulong)_numBytes - sizeInBytes)
		{
			NotEnoughRoom();
		}
	}

	private static void NotEnoughRoom()
	{
		throw new ArgumentException("Not enough space available in the buffer.");
	}

	private static InvalidOperationException NotInitialized()
	{
		return new InvalidOperationException("You must call Initialize on this object instance before using it.");
	}

	internal static uint AlignedSizeOf<T>() where T : struct
	{
		uint num = SizeOf<T>();
		if (num == 1 || num == 2)
		{
			return num;
		}
		return (uint)((num + 3) & -4);
	}

	internal static uint SizeOf<T>() where T : struct
	{
		if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			throw new ArgumentException("The specified Type must be a struct containing no references.");
		}
		return (uint)Unsafe.SizeOf<T>();
	}
}
