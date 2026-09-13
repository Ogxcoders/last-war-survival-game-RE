using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Mono.Btls;

internal class MonoBtlsX509 : MonoBtlsObject
{
	internal class BoringX509Handle : MonoBtlsHandle
	{
		public BoringX509Handle(IntPtr handle)
			: base(handle, ownsHandle: true)
		{
		}

		protected override bool ReleaseHandle()
		{
			if (handle != IntPtr.Zero)
			{
				mono_btls_x509_free(handle);
			}
			return true;
		}

		public IntPtr StealHandle()
		{
			return Interlocked.Exchange(ref handle, IntPtr.Zero);
		}
	}

	internal new BoringX509Handle Handle => (BoringX509Handle)base.Handle;

	internal MonoBtlsX509(BoringX509Handle handle)
		: base(handle)
	{
	}

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_up_ref(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_from_data(IntPtr data, int len, MonoBtlsX509Format format);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_get_subject_name(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_get_issuer_name(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_get_subject_name_string(IntPtr handle, IntPtr buffer, int size);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_get_issuer_name_string(IntPtr handle, IntPtr buffer, int size);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_get_raw_data(IntPtr handle, IntPtr bio, MonoBtlsX509Format format);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_cmp(IntPtr a, IntPtr b);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_get_hash(IntPtr handle, out IntPtr data);

	[DllImport("libmono-btls-shared")]
	private static extern long mono_btls_x509_get_not_before(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern long mono_btls_x509_get_not_after(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_get_public_key(IntPtr handle, IntPtr bio);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_get_serial_number(IntPtr handle, IntPtr data, int size, int mono_style);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_get_version(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_get_signature_algorithm(IntPtr handle, IntPtr buffer, int size);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_get_public_key_asn1(IntPtr handle, IntPtr oid, int oid_size, out IntPtr data, out int size);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_get_public_key_parameters(IntPtr handle, IntPtr oid, int oid_size, out IntPtr data, out int size);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_get_pubkey(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_get_subject_key_identifier(IntPtr handle, out IntPtr data, out int size);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_print(IntPtr handle, IntPtr bio);

	[DllImport("libmono-btls-shared")]
	private static extern void mono_btls_x509_free(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_dup(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_add_trust_object(IntPtr handle, MonoBtlsX509Purpose purpose);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_add_reject_object(IntPtr handle, MonoBtlsX509Purpose purpose);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_add_explicit_trust(IntPtr handle, MonoBtlsX509TrustKind kind);

	internal MonoBtlsX509 Copy()
	{
		IntPtr intPtr = mono_btls_x509_up_ref(Handle.DangerousGetHandle());
		CheckError(intPtr != IntPtr.Zero, "Copy");
		return new MonoBtlsX509(new BoringX509Handle(intPtr));
	}

	internal MonoBtlsX509 Duplicate()
	{
		IntPtr intPtr = mono_btls_x509_dup(Handle.DangerousGetHandle());
		CheckError(intPtr != IntPtr.Zero, "Duplicate");
		return new MonoBtlsX509(new BoringX509Handle(intPtr));
	}

	public static MonoBtlsX509 LoadFromData(byte[] buffer, MonoBtlsX509Format format)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(buffer.Length);
		if (intPtr == IntPtr.Zero)
		{
			throw new OutOfMemoryException();
		}
		try
		{
			Marshal.Copy(buffer, 0, intPtr, buffer.Length);
			IntPtr intPtr2 = mono_btls_x509_from_data(intPtr, buffer.Length, format);
			if (intPtr2 == IntPtr.Zero)
			{
				throw new MonoBtlsException("Failed to read certificate from data.");
			}
			return new MonoBtlsX509(new BoringX509Handle(intPtr2));
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public MonoBtlsX509Name GetSubjectName()
	{
		IntPtr intPtr = mono_btls_x509_get_subject_name(Handle.DangerousGetHandle());
		CheckError(intPtr != IntPtr.Zero, "GetSubjectName");
		return new MonoBtlsX509Name(new MonoBtlsX509Name.BoringX509NameHandle(intPtr, ownsHandle: false));
	}

	public string GetSubjectNameString()
	{
		IntPtr intPtr = Marshal.AllocHGlobal(4096);
		try
		{
			int ret = mono_btls_x509_get_subject_name_string(Handle.DangerousGetHandle(), intPtr, 4096);
			CheckError(ret, "GetSubjectNameString");
			return Marshal.PtrToStringAnsi(intPtr);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public long GetSubjectNameHash()
	{
		CheckThrow();
		using MonoBtlsX509Name monoBtlsX509Name = GetSubjectName();
		return monoBtlsX509Name.GetHash();
	}

	public MonoBtlsX509Name GetIssuerName()
	{
		IntPtr intPtr = mono_btls_x509_get_issuer_name(Handle.DangerousGetHandle());
		CheckError(intPtr != IntPtr.Zero, "GetIssuerName");
		return new MonoBtlsX509Name(new MonoBtlsX509Name.BoringX509NameHandle(intPtr, ownsHandle: false));
	}

	public string GetIssuerNameString()
	{
		IntPtr intPtr = Marshal.AllocHGlobal(4096);
		try
		{
			int ret = mono_btls_x509_get_issuer_name_string(Handle.DangerousGetHandle(), intPtr, 4096);
			CheckError(ret, "GetIssuerNameString");
			return Marshal.PtrToStringAnsi(intPtr);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public byte[] GetRawData(MonoBtlsX509Format format)
	{
		using MonoBtlsBioMemory monoBtlsBioMemory = new MonoBtlsBioMemory();
		int ret = mono_btls_x509_get_raw_data(Handle.DangerousGetHandle(), monoBtlsBioMemory.Handle.DangerousGetHandle(), format);
		CheckError(ret, "GetRawData");
		return monoBtlsBioMemory.GetData();
	}

	public void GetRawData(MonoBtlsBio bio, MonoBtlsX509Format format)
	{
		CheckThrow();
		int ret = mono_btls_x509_get_raw_data(Handle.DangerousGetHandle(), bio.Handle.DangerousGetHandle(), format);
		CheckError(ret, "GetRawData");
	}

	public static int Compare(MonoBtlsX509 a, MonoBtlsX509 b)
	{
		return mono_btls_x509_cmp(a.Handle.DangerousGetHandle(), b.Handle.DangerousGetHandle());
	}

	public byte[] GetCertHash()
	{
		IntPtr data;
		int num = mono_btls_x509_get_hash(Handle.DangerousGetHandle(), out data);
		CheckError(num > 0, "GetCertHash");
		byte[] array = new byte[num];
		Marshal.Copy(data, array, 0, num);
		return array;
	}

	public DateTime GetNotBefore()
	{
		long num = mono_btls_x509_get_not_before(Handle.DangerousGetHandle());
		return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(num);
	}

	public DateTime GetNotAfter()
	{
		long num = mono_btls_x509_get_not_after(Handle.DangerousGetHandle());
		return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(num);
	}

	public byte[] GetPublicKeyData()
	{
		using MonoBtlsBioMemory monoBtlsBioMemory = new MonoBtlsBioMemory();
		int num = mono_btls_x509_get_public_key(Handle.DangerousGetHandle(), monoBtlsBioMemory.Handle.DangerousGetHandle());
		CheckError(num > 0, "GetPublicKeyData");
		return monoBtlsBioMemory.GetData();
	}

	public byte[] GetSerialNumber(bool mono_style)
	{
		int num = 256;
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		try
		{
			int num2 = mono_btls_x509_get_serial_number(Handle.DangerousGetHandle(), intPtr, num, mono_style ? 1 : 0);
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

	public int GetVersion()
	{
		return mono_btls_x509_get_version(Handle.DangerousGetHandle());
	}

	public string GetSignatureAlgorithm()
	{
		int num = 256;
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		try
		{
			int num2 = mono_btls_x509_get_signature_algorithm(Handle.DangerousGetHandle(), intPtr, num);
			CheckError(num2 > 0, "GetSignatureAlgorithm");
			return Marshal.PtrToStringAnsi(intPtr);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public AsnEncodedData GetPublicKeyAsn1()
	{
		int oid_size = 256;
		IntPtr intPtr = Marshal.AllocHGlobal(256);
		IntPtr data;
		int size;
		string text;
		try
		{
			int ret = mono_btls_x509_get_public_key_asn1(Handle.DangerousGetHandle(), intPtr, oid_size, out data, out size);
			CheckError(ret, "GetPublicKeyAsn1");
			text = Marshal.PtrToStringAnsi(intPtr);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
		try
		{
			byte[] array = new byte[size];
			Marshal.Copy(data, array, 0, size);
			return new AsnEncodedData(text.ToString(), array);
		}
		finally
		{
			if (data != IntPtr.Zero)
			{
				FreeDataPtr(data);
			}
		}
	}

	public AsnEncodedData GetPublicKeyParameters()
	{
		int oid_size = 256;
		IntPtr intPtr = Marshal.AllocHGlobal(256);
		IntPtr data;
		int size;
		string text;
		try
		{
			int ret = mono_btls_x509_get_public_key_parameters(Handle.DangerousGetHandle(), intPtr, oid_size, out data, out size);
			CheckError(ret, "GetPublicKeyParameters");
			text = Marshal.PtrToStringAnsi(intPtr);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
		try
		{
			byte[] array = new byte[size];
			Marshal.Copy(data, array, 0, size);
			return new AsnEncodedData(text.ToString(), array);
		}
		finally
		{
			if (data != IntPtr.Zero)
			{
				FreeDataPtr(data);
			}
		}
	}

	public byte[] GetSubjectKeyIdentifier()
	{
		IntPtr data = IntPtr.Zero;
		try
		{
			int size;
			int ret = mono_btls_x509_get_subject_key_identifier(Handle.DangerousGetHandle(), out data, out size);
			CheckError(ret, "GetSubjectKeyIdentifier");
			byte[] array = new byte[size];
			Marshal.Copy(data, array, 0, size);
			return array;
		}
		finally
		{
			if (data != IntPtr.Zero)
			{
				FreeDataPtr(data);
			}
		}
	}

	public MonoBtlsKey GetPublicKey()
	{
		IntPtr intPtr = mono_btls_x509_get_pubkey(Handle.DangerousGetHandle());
		CheckError(intPtr != IntPtr.Zero, "GetPublicKey");
		return new MonoBtlsKey(new MonoBtlsKey.BoringKeyHandle(intPtr));
	}

	public void Print(MonoBtlsBio bio)
	{
		int ret = mono_btls_x509_print(Handle.DangerousGetHandle(), bio.Handle.DangerousGetHandle());
		CheckError(ret, "Print");
	}

	public void ExportAsPEM(MonoBtlsBio bio, bool includeHumanReadableForm)
	{
		GetRawData(bio, MonoBtlsX509Format.PEM);
		if (!includeHumanReadableForm)
		{
			return;
		}
		Print(bio);
		byte[] certHash = GetCertHash();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("SHA1 Fingerprint=");
		for (int i = 0; i < certHash.Length; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(":");
			}
			stringBuilder.AppendFormat("{0:X2}", certHash[i]);
		}
		stringBuilder.AppendLine();
		byte[] bytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());
		bio.Write(bytes, 0, bytes.Length);
	}

	public void AddTrustObject(MonoBtlsX509Purpose purpose)
	{
		CheckThrow();
		int ret = mono_btls_x509_add_trust_object(Handle.DangerousGetHandle(), purpose);
		CheckError(ret, "AddTrustObject");
	}

	public void AddRejectObject(MonoBtlsX509Purpose purpose)
	{
		CheckThrow();
		int ret = mono_btls_x509_add_reject_object(Handle.DangerousGetHandle(), purpose);
		CheckError(ret, "AddRejectObject");
	}

	public void AddExplicitTrust(MonoBtlsX509TrustKind kind)
	{
		CheckThrow();
		int ret = mono_btls_x509_add_explicit_trust(Handle.DangerousGetHandle(), kind);
		CheckError(ret, "AddExplicitTrust");
	}
}
