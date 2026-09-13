using System;
using System.Runtime.InteropServices;

namespace Mono.Btls;

internal class MonoBtlsX509StoreCtx : MonoBtlsObject
{
	internal class BoringX509StoreCtxHandle : MonoBtlsHandle
	{
		private bool dontFree;

		internal BoringX509StoreCtxHandle(IntPtr handle, bool ownsHandle = true)
			: base(handle, ownsHandle)
		{
			dontFree = !ownsHandle;
		}

		protected override bool ReleaseHandle()
		{
			if (!dontFree)
			{
				mono_btls_x509_store_ctx_free(handle);
			}
			return true;
		}
	}

	private int? verifyResult;

	internal new BoringX509StoreCtxHandle Handle => (BoringX509StoreCtxHandle)base.Handle;

	public int VerifyResult
	{
		get
		{
			if (!verifyResult.HasValue)
			{
				throw new InvalidOperationException();
			}
			return verifyResult.Value;
		}
	}

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_store_ctx_new();

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_store_ctx_from_ptr(IntPtr ctx);

	[DllImport("libmono-btls-shared")]
	private static extern MonoBtlsX509Error mono_btls_x509_store_ctx_get_error(IntPtr handle, out IntPtr error_string);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_store_ctx_get_error_depth(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_store_ctx_get_chain(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_store_ctx_init(IntPtr handle, IntPtr store, IntPtr chain);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_store_ctx_set_param(IntPtr handle, IntPtr param);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_store_ctx_verify_cert(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_store_ctx_get_by_subject(IntPtr handle, IntPtr name);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_store_ctx_get_current_cert(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_store_ctx_get_current_issuer(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_store_ctx_get_verify_param(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_store_ctx_get_untrusted(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_store_ctx_up_ref(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern void mono_btls_x509_store_ctx_free(IntPtr handle);

	internal MonoBtlsX509StoreCtx()
		: base(new BoringX509StoreCtxHandle(mono_btls_x509_store_ctx_new()))
	{
	}

	private static BoringX509StoreCtxHandle Create_internal(IntPtr store_ctx)
	{
		IntPtr intPtr = mono_btls_x509_store_ctx_from_ptr(store_ctx);
		if (intPtr == IntPtr.Zero)
		{
			throw new MonoBtlsException();
		}
		return new BoringX509StoreCtxHandle(intPtr);
	}

	internal MonoBtlsX509StoreCtx(int preverify_ok, IntPtr store_ctx)
		: base(Create_internal(store_ctx))
	{
		verifyResult = preverify_ok;
	}

	internal MonoBtlsX509StoreCtx(BoringX509StoreCtxHandle ptr, int? verifyResult)
		: base(ptr)
	{
		this.verifyResult = verifyResult;
	}

	public MonoBtlsX509Error GetError()
	{
		IntPtr error_string;
		return mono_btls_x509_store_ctx_get_error(Handle.DangerousGetHandle(), out error_string);
	}

	public int GetErrorDepth()
	{
		return mono_btls_x509_store_ctx_get_error_depth(Handle.DangerousGetHandle());
	}

	public MonoBtlsX509Exception GetException()
	{
		IntPtr error_string;
		MonoBtlsX509Error monoBtlsX509Error = mono_btls_x509_store_ctx_get_error(Handle.DangerousGetHandle(), out error_string);
		if (monoBtlsX509Error == MonoBtlsX509Error.OK)
		{
			return null;
		}
		if (error_string != IntPtr.Zero)
		{
			string message = Marshal.PtrToStringAnsi(error_string);
			return new MonoBtlsX509Exception(monoBtlsX509Error, message);
		}
		return new MonoBtlsX509Exception(monoBtlsX509Error, "Unknown verify error.");
	}

	public MonoBtlsX509Chain GetChain()
	{
		IntPtr intPtr = mono_btls_x509_store_ctx_get_chain(Handle.DangerousGetHandle());
		CheckError(intPtr != IntPtr.Zero, "GetChain");
		return new MonoBtlsX509Chain(new MonoBtlsX509Chain.BoringX509ChainHandle(intPtr));
	}

	public MonoBtlsX509Chain GetUntrusted()
	{
		IntPtr intPtr = mono_btls_x509_store_ctx_get_untrusted(Handle.DangerousGetHandle());
		CheckError(intPtr != IntPtr.Zero, "GetUntrusted");
		return new MonoBtlsX509Chain(new MonoBtlsX509Chain.BoringX509ChainHandle(intPtr));
	}

	public void Initialize(MonoBtlsX509Store store, MonoBtlsX509Chain chain)
	{
		int ret = mono_btls_x509_store_ctx_init(Handle.DangerousGetHandle(), store.Handle.DangerousGetHandle(), chain.Handle.DangerousGetHandle());
		CheckError(ret, "Initialize");
	}

	public void SetVerifyParam(MonoBtlsX509VerifyParam param)
	{
		int ret = mono_btls_x509_store_ctx_set_param(Handle.DangerousGetHandle(), param.Handle.DangerousGetHandle());
		CheckError(ret, "SetVerifyParam");
	}

	public int Verify()
	{
		verifyResult = mono_btls_x509_store_ctx_verify_cert(Handle.DangerousGetHandle());
		return verifyResult.Value;
	}

	public MonoBtlsX509 LookupBySubject(MonoBtlsX509Name name)
	{
		IntPtr intPtr = mono_btls_x509_store_ctx_get_by_subject(Handle.DangerousGetHandle(), name.Handle.DangerousGetHandle());
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new MonoBtlsX509(new MonoBtlsX509.BoringX509Handle(intPtr));
	}

	public MonoBtlsX509 GetCurrentCertificate()
	{
		IntPtr intPtr = mono_btls_x509_store_ctx_get_current_cert(Handle.DangerousGetHandle());
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new MonoBtlsX509(new MonoBtlsX509.BoringX509Handle(intPtr));
	}

	public MonoBtlsX509 GetCurrentIssuer()
	{
		IntPtr intPtr = mono_btls_x509_store_ctx_get_current_issuer(Handle.DangerousGetHandle());
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new MonoBtlsX509(new MonoBtlsX509.BoringX509Handle(intPtr));
	}

	public MonoBtlsX509VerifyParam GetVerifyParam()
	{
		IntPtr intPtr = mono_btls_x509_store_ctx_get_verify_param(Handle.DangerousGetHandle());
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new MonoBtlsX509VerifyParam(new MonoBtlsX509VerifyParam.BoringX509VerifyParamHandle(intPtr));
	}

	public MonoBtlsX509StoreCtx Copy()
	{
		IntPtr intPtr = mono_btls_x509_store_ctx_up_ref(Handle.DangerousGetHandle());
		CheckError(intPtr != IntPtr.Zero, "Copy");
		return new MonoBtlsX509StoreCtx(new BoringX509StoreCtxHandle(intPtr), verifyResult);
	}
}
