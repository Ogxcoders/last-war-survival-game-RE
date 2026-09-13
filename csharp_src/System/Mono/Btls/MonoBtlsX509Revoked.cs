using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Mono.Btls;

internal class MonoBtlsX509Revoked : MonoBtlsObject
{
	internal class BoringX509RevokedHandle : MonoBtlsHandle
	{
		public BoringX509RevokedHandle(IntPtr handle)
			: base(handle, ownsHandle: true)
		{
		}

		protected override bool ReleaseHandle()
		{
			if (handle != IntPtr.Zero)
			{
				mono_btls_x509_revoked_free(handle);
			}
			return true;
		}

		public IntPtr StealHandle()
		{
			return Interlocked.Exchange(ref handle, IntPtr.Zero);
		}
	}

	internal new BoringX509RevokedHandle Handle => (BoringX509RevokedHandle)base.Handle;

	internal MonoBtlsX509Revoked(BoringX509RevokedHandle handle)
		: base(handle)
	{
	}

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_revoked_get_serial_number(IntPtr handle, IntPtr data, int size);

	[DllImport("libmono-btls-shared")]
	private static extern long mono_btls_x509_revoked_get_revocation_date(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_revoked_get_reason(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_revoked_get_sequence(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern void mono_btls_x509_revoked_free(IntPtr handle);

	public byte[] GetSerialNumber()
	{
		int num = 256;
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		try
		{
			int num2 = mono_btls_x509_revoked_get_serial_number(Handle.DangerousGetHandle(), intPtr, num);
			CheckError(num2 > 0, "GetSerialNumber");
			byte[] array = new byte[num2];
			Marshal.Copy(intPtr, array, 0, num2);
			return array;
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	public DateTime GetRevocationDate()
	{
		long num = mono_btls_x509_revoked_get_revocation_date(Handle.DangerousGetHandle());
		return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(num);
	}

	public int GetReason()
	{
		return mono_btls_x509_revoked_get_reason(Handle.DangerousGetHandle());
	}

	public int GetSequence()
	{
		return mono_btls_x509_revoked_get_sequence(Handle.DangerousGetHandle());
	}
}
