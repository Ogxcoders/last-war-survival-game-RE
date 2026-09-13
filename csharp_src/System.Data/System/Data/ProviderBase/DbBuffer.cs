using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Data.ProviderBase;

internal abstract class DbBuffer : SafeHandle
{
	private readonly int _bufferLength;

	private int BaseOffset => 0;

	public override bool IsInvalid => IntPtr.Zero == handle;

	internal int Length => _bufferLength;

	protected DbBuffer(int initialSize)
		: base(IntPtr.Zero, ownsHandle: true)
	{
		if (0 < initialSize)
		{
			_bufferLength = initialSize;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				handle = SafeNativeMethods.LocalAlloc((IntPtr)initialSize);
			}
			if (IntPtr.Zero == handle)
			{
				throw new OutOfMemoryException();
			}
		}
	}

	protected DbBuffer(IntPtr invalidHandleValue, bool ownsHandle)
		: base(invalidHandleValue, ownsHandle)
	{
	}

	internal string PtrToStringUni(int offset)
	{
		offset += BaseOffset;
		Validate(offset, 2);
		string text = null;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			text = Marshal.PtrToStringUni(ADP.IntPtrOffset(DangerousGetHandle(), offset));
			Validate(offset, 2 * (text.Length + 1));
			return text;
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal string PtrToStringUni(int offset, int length)
	{
		offset += BaseOffset;
		Validate(offset, 2 * length);
		string text = null;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			return Marshal.PtrToStringUni(ADP.IntPtrOffset(DangerousGetHandle(), offset), length);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal byte ReadByte(int offset)
	{
		offset += BaseOffset;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			return Marshal.ReadByte(DangerousGetHandle(), offset);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal byte[] ReadBytes(int offset, int length)
	{
		byte[] destination = new byte[length];
		return ReadBytes(offset, destination, 0, length);
	}

	internal byte[] ReadBytes(int offset, byte[] destination, int startIndex, int length)
	{
		offset += BaseOffset;
		Validate(offset, length);
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			Marshal.Copy(ADP.IntPtrOffset(DangerousGetHandle(), offset), destination, startIndex, length);
			return destination;
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal char ReadChar(int offset)
	{
		return (char)ReadInt16(offset);
	}

	internal char[] ReadChars(int offset, char[] destination, int startIndex, int length)
	{
		offset += BaseOffset;
		Validate(offset, 2 * length);
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			Marshal.Copy(ADP.IntPtrOffset(DangerousGetHandle(), offset), destination, startIndex, length);
			return destination;
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal double ReadDouble(int offset)
	{
		return BitConverter.Int64BitsToDouble(ReadInt64(offset));
	}

	internal short ReadInt16(int offset)
	{
		offset += BaseOffset;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			return Marshal.ReadInt16(DangerousGetHandle(), offset);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal void ReadInt16Array(int offset, short[] destination, int startIndex, int length)
	{
		offset += BaseOffset;
		Validate(offset, 2 * length);
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			Marshal.Copy(ADP.IntPtrOffset(DangerousGetHandle(), offset), destination, startIndex, length);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal int ReadInt32(int offset)
	{
		offset += BaseOffset;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			return Marshal.ReadInt32(DangerousGetHandle(), offset);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal void ReadInt32Array(int offset, int[] destination, int startIndex, int length)
	{
		offset += BaseOffset;
		Validate(offset, 4 * length);
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			Marshal.Copy(ADP.IntPtrOffset(DangerousGetHandle(), offset), destination, startIndex, length);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal long ReadInt64(int offset)
	{
		offset += BaseOffset;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			return Marshal.ReadInt64(DangerousGetHandle(), offset);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal IntPtr ReadIntPtr(int offset)
	{
		offset += BaseOffset;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			return Marshal.ReadIntPtr(DangerousGetHandle(), offset);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal unsafe float ReadSingle(int offset)
	{
		int num = ReadInt32(offset);
		return *(float*)(&num);
	}

	protected override bool ReleaseHandle()
	{
		IntPtr intPtr = handle;
		handle = IntPtr.Zero;
		if (IntPtr.Zero != intPtr)
		{
			SafeNativeMethods.LocalFree(intPtr);
		}
		return true;
	}

	private void StructureToPtr(int offset, object structure)
	{
		offset += BaseOffset;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			IntPtr ptr = ADP.IntPtrOffset(DangerousGetHandle(), offset);
			Marshal.StructureToPtr(structure, ptr, fDeleteOld: false);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal void WriteByte(int offset, byte value)
	{
		offset += BaseOffset;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			Marshal.WriteByte(DangerousGetHandle(), offset, value);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal void WriteBytes(int offset, byte[] source, int startIndex, int length)
	{
		offset += BaseOffset;
		Validate(offset, length);
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			IntPtr destination = ADP.IntPtrOffset(DangerousGetHandle(), offset);
			Marshal.Copy(source, startIndex, destination, length);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal void WriteCharArray(int offset, char[] source, int startIndex, int length)
	{
		offset += BaseOffset;
		Validate(offset, 2 * length);
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			IntPtr destination = ADP.IntPtrOffset(DangerousGetHandle(), offset);
			Marshal.Copy(source, startIndex, destination, length);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal void WriteDouble(int offset, double value)
	{
		WriteInt64(offset, BitConverter.DoubleToInt64Bits(value));
	}

	internal void WriteInt16(int offset, short value)
	{
		offset += BaseOffset;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			Marshal.WriteInt16(DangerousGetHandle(), offset, value);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal void WriteInt16Array(int offset, short[] source, int startIndex, int length)
	{
		offset += BaseOffset;
		Validate(offset, 2 * length);
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			IntPtr destination = ADP.IntPtrOffset(DangerousGetHandle(), offset);
			Marshal.Copy(source, startIndex, destination, length);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal void WriteInt32(int offset, int value)
	{
		offset += BaseOffset;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			Marshal.WriteInt32(DangerousGetHandle(), offset, value);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal void WriteInt32Array(int offset, int[] source, int startIndex, int length)
	{
		offset += BaseOffset;
		Validate(offset, 4 * length);
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			IntPtr destination = ADP.IntPtrOffset(DangerousGetHandle(), offset);
			Marshal.Copy(source, startIndex, destination, length);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal void WriteInt64(int offset, long value)
	{
		offset += BaseOffset;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			Marshal.WriteInt64(DangerousGetHandle(), offset, value);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal void WriteIntPtr(int offset, IntPtr value)
	{
		offset += BaseOffset;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			Marshal.WriteIntPtr(DangerousGetHandle(), offset, value);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal unsafe void WriteSingle(int offset, float value)
	{
		WriteInt32(offset, *(int*)(&value));
	}

	internal void ZeroMemory()
	{
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			SafeNativeMethods.ZeroMemory(DangerousGetHandle(), Length);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	internal Guid ReadGuid(int offset)
	{
		byte[] array = new byte[16];
		ReadBytes(offset, array, 0, 16);
		return new Guid(array);
	}

	internal void WriteGuid(int offset, Guid value)
	{
		StructureToPtr(offset, value);
	}

	internal DateTime ReadDate(int offset)
	{
		short[] array = new short[3];
		ReadInt16Array(offset, array, 0, 3);
		return new DateTime((ushort)array[0], (ushort)array[1], (ushort)array[2]);
	}

	internal void WriteDate(int offset, DateTime value)
	{
		short[] source = new short[3]
		{
			(short)value.Year,
			(short)value.Month,
			(short)value.Day
		};
		WriteInt16Array(offset, source, 0, 3);
	}

	internal TimeSpan ReadTime(int offset)
	{
		short[] array = new short[3];
		ReadInt16Array(offset, array, 0, 3);
		return new TimeSpan((ushort)array[0], (ushort)array[1], (ushort)array[2]);
	}

	internal void WriteTime(int offset, TimeSpan value)
	{
		short[] source = new short[3]
		{
			(short)value.Hours,
			(short)value.Minutes,
			(short)value.Seconds
		};
		WriteInt16Array(offset, source, 0, 3);
	}

	internal DateTime ReadDateTime(int offset)
	{
		short[] array = new short[6];
		ReadInt16Array(offset, array, 0, 6);
		int num = ReadInt32(offset + 12);
		return new DateTime((ushort)array[0], (ushort)array[1], (ushort)array[2], (ushort)array[3], (ushort)array[4], (ushort)array[5]).AddTicks(num / 100);
	}

	internal void WriteDateTime(int offset, DateTime value)
	{
		int value2 = (int)(value.Ticks % 10000000) * 100;
		short[] source = new short[6]
		{
			(short)value.Year,
			(short)value.Month,
			(short)value.Day,
			(short)value.Hour,
			(short)value.Minute,
			(short)value.Second
		};
		WriteInt16Array(offset, source, 0, 6);
		WriteInt32(offset + 12, value2);
	}

	internal decimal ReadNumeric(int offset)
	{
		byte[] array = new byte[20];
		ReadBytes(offset, array, 1, 19);
		int[] array2 = new int[4]
		{
			0,
			0,
			0,
			array[2] << 16
		};
		if (array[3] == 0)
		{
			array2[3] |= int.MinValue;
		}
		array2[0] = BitConverter.ToInt32(array, 4);
		array2[1] = BitConverter.ToInt32(array, 8);
		array2[2] = BitConverter.ToInt32(array, 12);
		if (BitConverter.ToInt32(array, 16) != 0)
		{
			throw ADP.NumericToDecimalOverflow();
		}
		return new decimal(array2);
	}

	internal void WriteNumeric(int offset, decimal value, byte precision)
	{
		int[] bits = decimal.GetBits(value);
		byte[] array = new byte[20]
		{
			0, precision, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0
		};
		Buffer.BlockCopy(bits, 14, array, 2, 2);
		array[3] = ((array[3] == 0) ? ((byte)1) : ((byte)0));
		Buffer.BlockCopy(bits, 0, array, 4, 12);
		array[16] = 0;
		array[17] = 0;
		array[18] = 0;
		array[19] = 0;
		WriteBytes(offset, array, 1, 19);
	}

	[Conditional("DEBUG")]
	protected void ValidateCheck(int offset, int count)
	{
		Validate(offset, count);
	}

	protected void Validate(int offset, int count)
	{
		if (offset < 0 || count < 0 || Length < checked(offset + count))
		{
			throw ADP.InternalError(ADP.InternalErrorCode.InvalidBuffer);
		}
	}
}
