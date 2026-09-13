using System;
using System.Runtime.InteropServices;

namespace Mono;

internal class CFData : CFObject
{
	public IntPtr Length => CFDataGetLength(base.Handle);

	public IntPtr Bytes => CFDataGetBytePtr(base.Handle);

	public byte this[long idx]
	{
		get
		{
			if (idx < 0 || (ulong)idx > (ulong)(long)Length)
			{
				throw new ArgumentException("idx");
			}
			return Marshal.ReadByte(new IntPtr(Bytes.ToInt64() + idx));
		}
		set
		{
			throw new NotImplementedException("NSData arrays can not be modified, use an NSMutableData instead");
		}
	}

	public CFData(IntPtr handle, bool own)
		: base(handle, own)
	{
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFDataCreate(IntPtr allocator, IntPtr bytes, IntPtr length);

	public unsafe static CFData FromData(byte[] buffer)
	{
		fixed (byte* ptr = buffer)
		{
			return FromData((IntPtr)ptr, (IntPtr)buffer.Length);
		}
	}

	public static CFData FromData(IntPtr buffer, IntPtr length)
	{
		return new CFData(CFDataCreate(IntPtr.Zero, buffer, length), own: true);
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	internal static extern IntPtr CFDataGetLength(IntPtr theData);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	internal static extern IntPtr CFDataGetBytePtr(IntPtr theData);
}
