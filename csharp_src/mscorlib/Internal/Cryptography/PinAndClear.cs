using System;
using System.Runtime.InteropServices;

namespace Internal.Cryptography;

internal struct PinAndClear : IDisposable
{
	private byte[] _data;

	private GCHandle _gcHandle;

	internal static PinAndClear Track(byte[] data)
	{
		return new PinAndClear
		{
			_gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned),
			_data = data
		};
	}

	public void Dispose()
	{
		Array.Clear(_data, 0, _data.Length);
		_gcHandle.Free();
	}
}
