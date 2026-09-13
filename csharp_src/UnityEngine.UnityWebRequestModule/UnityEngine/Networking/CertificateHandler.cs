using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Networking;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/UnityWebRequest/Public/CertificateHandler/CertificateHandlerScript.h")]
public class CertificateHandler : IDisposable
{
	[NonSerialized]
	internal IntPtr m_Ptr;

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern IntPtr Create(CertificateHandler obj);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private extern void Release();

	protected CertificateHandler()
	{
		m_Ptr = Create(this);
	}

	~CertificateHandler()
	{
		Dispose();
	}

	protected virtual bool ValidateCertificate(byte[] certificateData)
	{
		return false;
	}

	[RequiredByNativeCode]
	internal bool ValidateCertificateNative(byte[] certificateData)
	{
		return ValidateCertificate(certificateData);
	}

	public void Dispose()
	{
		if (m_Ptr != IntPtr.Zero)
		{
			Release();
			m_Ptr = IntPtr.Zero;
		}
	}
}
