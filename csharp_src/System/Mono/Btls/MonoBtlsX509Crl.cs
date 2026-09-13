using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Mono.Btls;

internal class MonoBtlsX509Crl : MonoBtlsObject
{
	internal class BoringX509CrlHandle : MonoBtlsHandle
	{
		public BoringX509CrlHandle(IntPtr handle)
			: base(handle, ownsHandle: true)
		{
		}

		protected override bool ReleaseHandle()
		{
			if (handle != IntPtr.Zero)
			{
				mono_btls_x509_crl_free(handle);
			}
			return true;
		}

		public IntPtr StealHandle()
		{
			return Interlocked.Exchange(ref handle, IntPtr.Zero);
		}
	}

	internal new BoringX509CrlHandle Handle => (BoringX509CrlHandle)base.Handle;

	internal MonoBtlsX509Crl(BoringX509CrlHandle handle)
		: base(handle)
	{
	}

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_crl_ref(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_crl_from_data(IntPtr data, int len, MonoBtlsX509Format format);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_crl_get_by_cert(IntPtr handle, IntPtr x509);

	[DllImport("libmono-btls-shared")]
	private unsafe static extern IntPtr mono_btls_x509_crl_get_by_serial(IntPtr handle, void* serial, int len);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_crl_get_revoked_count(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_crl_get_revoked(IntPtr handle, int index);

	[DllImport("libmono-btls-shared")]
	private static extern long mono_btls_x509_crl_get_last_update(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern long mono_btls_x509_crl_get_next_update(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern long mono_btls_x509_crl_get_version(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_crl_get_issuer(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern void mono_btls_x509_crl_free(IntPtr handle);

	public static MonoBtlsX509Crl LoadFromData(byte[] buffer, MonoBtlsX509Format format)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(buffer.Length);
		if (intPtr == IntPtr.Zero)
		{
			throw new OutOfMemoryException();
		}
		try
		{
			Marshal.Copy(buffer, 0, intPtr, buffer.Length);
			IntPtr intPtr2 = mono_btls_x509_crl_from_data(intPtr, buffer.Length, format);
			if (intPtr2 == IntPtr.Zero)
			{
				throw new MonoBtlsException("Failed to read CRL from data.");
			}
			return new MonoBtlsX509Crl(new BoringX509CrlHandle(intPtr2));
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public MonoBtlsX509Revoked GetByCert(MonoBtlsX509 x509)
	{
		IntPtr intPtr = mono_btls_x509_crl_get_by_cert(Handle.DangerousGetHandle(), x509.Handle.DangerousGetHandle());
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new MonoBtlsX509Revoked(new MonoBtlsX509Revoked.BoringX509RevokedHandle(intPtr));
	}

	public unsafe MonoBtlsX509Revoked GetBySerial(byte[] serial)
	{
		fixed (byte* ptr = serial)
		{
			void* serial2 = ptr;
			IntPtr intPtr = mono_btls_x509_crl_get_by_serial(Handle.DangerousGetHandle(), serial2, serial.Length);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return new MonoBtlsX509Revoked(new MonoBtlsX509Revoked.BoringX509RevokedHandle(intPtr));
		}
	}

	public int GetRevokedCount()
	{
		return mono_btls_x509_crl_get_revoked_count(Handle.DangerousGetHandle());
	}

	public MonoBtlsX509Revoked GetRevoked(int index)
	{
		if (index >= GetRevokedCount())
		{
			throw new ArgumentOutOfRangeException();
		}
		IntPtr intPtr = mono_btls_x509_crl_get_revoked(Handle.DangerousGetHandle(), index);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new MonoBtlsX509Revoked(new MonoBtlsX509Revoked.BoringX509RevokedHandle(intPtr));
	}

	public DateTime GetLastUpdate()
	{
		long num = mono_btls_x509_crl_get_last_update(Handle.DangerousGetHandle());
		return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(num);
	}

	public DateTime GetNextUpdate()
	{
		long num = mono_btls_x509_crl_get_next_update(Handle.DangerousGetHandle());
		return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(num);
	}

	public long GetVersion()
	{
		return mono_btls_x509_crl_get_version(Handle.DangerousGetHandle());
	}

	public MonoBtlsX509Name GetIssuerName()
	{
		IntPtr intPtr = mono_btls_x509_crl_get_issuer(Handle.DangerousGetHandle());
		CheckError(intPtr != IntPtr.Zero, "GetIssuerName");
		return new MonoBtlsX509Name(new MonoBtlsX509Name.BoringX509NameHandle(intPtr, ownsHandle: false));
	}
}
