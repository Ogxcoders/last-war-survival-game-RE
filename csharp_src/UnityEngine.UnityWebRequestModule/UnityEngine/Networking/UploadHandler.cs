using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/UnityWebRequest/Public/UploadHandler/UploadHandler.h")]
public class UploadHandler : IDisposable
{
	[NonSerialized]
	internal IntPtr m_Ptr;

	public byte[] data => GetData();

	public string contentType
	{
		get
		{
			return GetContentType();
		}
		set
		{
			SetContentType(value);
		}
	}

	public float progress => GetProgress();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private extern void Release();

	internal UploadHandler()
	{
	}

	~UploadHandler()
	{
		Dispose();
	}

	public void Dispose()
	{
		if (m_Ptr != IntPtr.Zero)
		{
			Release();
			m_Ptr = IntPtr.Zero;
		}
	}

	internal virtual byte[] GetData()
	{
		return null;
	}

	internal virtual string GetContentType()
	{
		return InternalGetContentType();
	}

	internal virtual void SetContentType(string newContentType)
	{
		InternalSetContentType(newContentType);
	}

	internal virtual float GetProgress()
	{
		return InternalGetProgress();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetContentType")]
	private extern string InternalGetContentType();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("SetContentType")]
	private extern void InternalSetContentType(string newContentType);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetProgress")]
	private extern float InternalGetProgress();
}
