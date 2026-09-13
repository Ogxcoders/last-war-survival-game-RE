using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/UnityWebRequest/Public/DownloadHandler/DownloadHandlerScript.h")]
public class DownloadHandlerScript : DownloadHandler
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern IntPtr Create(DownloadHandlerScript obj);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern IntPtr CreatePreallocated(DownloadHandlerScript obj, byte[] preallocatedBuffer);

	private void InternalCreateScript()
	{
		m_Ptr = Create(this);
	}

	private void InternalCreateScript(byte[] preallocatedBuffer)
	{
		m_Ptr = CreatePreallocated(this, preallocatedBuffer);
	}

	public DownloadHandlerScript()
	{
		InternalCreateScript();
	}

	public DownloadHandlerScript(byte[] preallocatedBuffer)
	{
		if (preallocatedBuffer == null || preallocatedBuffer.Length < 1)
		{
			throw new ArgumentException("Cannot create a preallocated-buffer DownloadHandlerScript backed by a null or zero-length array");
		}
		InternalCreateScript(preallocatedBuffer);
	}
}
