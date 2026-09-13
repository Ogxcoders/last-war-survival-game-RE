using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/UnityWebRequest/Public/DownloadHandler/DownloadHandlerBuffer.h")]
public sealed class DownloadHandlerBuffer : DownloadHandler
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern IntPtr Create(DownloadHandlerBuffer obj);

	private void InternalCreateBuffer()
	{
		m_Ptr = Create(this);
	}

	public DownloadHandlerBuffer()
	{
		InternalCreateBuffer();
	}

	protected override byte[] GetData()
	{
		return InternalGetData();
	}

	private byte[] InternalGetData()
	{
		return DownloadHandler.InternalGetByteArray(this);
	}

	public static string GetContent(UnityWebRequest www)
	{
		return DownloadHandler.GetCheckedDownloader<DownloadHandlerBuffer>(www).text;
	}
}
