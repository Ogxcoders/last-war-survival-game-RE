using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/UnityWebRequest/Public/UploadHandler/UploadHandlerRaw.h")]
public sealed class UploadHandlerRaw : UploadHandler
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern IntPtr Create(UploadHandlerRaw self, byte[] data);

	public UploadHandlerRaw(byte[] data)
	{
		if (data != null && data.Length == 0)
		{
			throw new ArgumentException("Cannot create a data handler without payload data");
		}
		m_Ptr = Create(this, data);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern byte[] InternalGetData();

	internal override byte[] GetData()
	{
		return InternalGetData();
	}
}
