using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/UnityWebRequest/Public/UploadHandler/UploadHandlerFile.h")]
public sealed class UploadHandlerFile : UploadHandler
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern IntPtr Create(UploadHandlerFile self, string filePath);

	public UploadHandlerFile(string filePath)
	{
		m_Ptr = Create(this, filePath);
	}
}
