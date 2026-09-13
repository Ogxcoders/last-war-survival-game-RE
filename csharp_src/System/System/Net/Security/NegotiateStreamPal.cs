using System.ComponentModel;
using Microsoft.Win32.SafeHandles;

namespace System.Net.Security;

internal static class NegotiateStreamPal
{
	private const int NTE_FAIL = -2146893792;

	internal static string QueryContextClientSpecifiedSpn(SafeDeleteContext securityContext)
	{
		throw new PlatformNotSupportedException("Server implementation is not supported");
	}

	internal static string QueryContextAuthenticationPackage(SafeDeleteContext securityContext)
	{
		if (!((SafeDeleteNegoContext)securityContext).IsNtlmUsed)
		{
			return "Kerberos";
		}
		return "NTLM";
	}

	private static byte[] GssWrap(SafeGssContextHandle context, bool encrypt, byte[] buffer, int offset, int count)
	{
		global::Interop.NetSecurityNative.GssBuffer outBuffer = default(global::Interop.NetSecurityNative.GssBuffer);
		try
		{
			global::Interop.NetSecurityNative.Status minorStatus;
			global::Interop.NetSecurityNative.Status status = global::Interop.NetSecurityNative.WrapBuffer(out minorStatus, context, encrypt, buffer, offset, count, ref outBuffer);
			if (status != global::Interop.NetSecurityNative.Status.GSS_S_COMPLETE)
			{
				throw new global::Interop.NetSecurityNative.GssApiException(status, minorStatus);
			}
			return outBuffer.ToByteArray();
		}
		finally
		{
			outBuffer.Dispose();
		}
	}

	private static int GssUnwrap(SafeGssContextHandle context, byte[] buffer, int offset, int count)
	{
		global::Interop.NetSecurityNative.GssBuffer outBuffer = default(global::Interop.NetSecurityNative.GssBuffer);
		try
		{
			global::Interop.NetSecurityNative.Status minorStatus;
			global::Interop.NetSecurityNative.Status status = global::Interop.NetSecurityNative.UnwrapBuffer(out minorStatus, context, buffer, offset, count, ref outBuffer);
			if (status != global::Interop.NetSecurityNative.Status.GSS_S_COMPLETE)
			{
				throw new global::Interop.NetSecurityNative.GssApiException(status, minorStatus);
			}
			return outBuffer.Copy(buffer, offset);
		}
		finally
		{
			outBuffer.Dispose();
		}
	}

	private static bool GssInitSecurityContext(ref SafeGssContextHandle context, SafeGssCredHandle credential, bool isNtlm, SafeGssNameHandle targetName, global::Interop.NetSecurityNative.GssFlags inFlags, byte[] buffer, out byte[] outputBuffer, out uint outFlags, out int isNtlmUsed)
	{
		outputBuffer = null;
		outFlags = 0u;
		if (context == null)
		{
			context = new SafeGssContextHandle();
		}
		global::Interop.NetSecurityNative.GssBuffer token = default(global::Interop.NetSecurityNative.GssBuffer);
		global::Interop.NetSecurityNative.Status status;
		try
		{
			status = global::Interop.NetSecurityNative.InitSecContext(out var minorStatus, credential, ref context, isNtlm, targetName, (uint)inFlags, buffer, (buffer != null) ? buffer.Length : 0, ref token, out outFlags, out isNtlmUsed);
			if (status != global::Interop.NetSecurityNative.Status.GSS_S_COMPLETE && status != global::Interop.NetSecurityNative.Status.GSS_S_CONTINUE_NEEDED)
			{
				throw new global::Interop.NetSecurityNative.GssApiException(status, minorStatus);
			}
			outputBuffer = token.ToByteArray();
		}
		finally
		{
			token.Dispose();
		}
		return status == global::Interop.NetSecurityNative.Status.GSS_S_COMPLETE;
	}

	private static SecurityStatusPal EstablishSecurityContext(SafeFreeNegoCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlagsPal inFlags, SecurityBuffer inputBuffer, SecurityBuffer outputBuffer, ref ContextFlagsPal outFlags)
	{
		bool isNtlmOnly = credential.IsNtlmOnly;
		if (context == null)
		{
			context = (isNtlmOnly ? new SafeDeleteNegoContext(credential, credential.UserName) : new SafeDeleteNegoContext(credential, targetName));
		}
		SafeDeleteNegoContext safeDeleteNegoContext = (SafeDeleteNegoContext)context;
		try
		{
			global::Interop.NetSecurityNative.GssFlags interopFromContextFlagsPal = ContextFlagsAdapterPal.GetInteropFromContextFlagsPal(inFlags, isServer: false);
			SafeGssContextHandle context2 = safeDeleteNegoContext.GssContext;
			uint outFlags2;
			int isNtlmUsed;
			bool num = GssInitSecurityContext(ref context2, credential.GssCredential, isNtlmOnly, safeDeleteNegoContext.TargetName, interopFromContextFlagsPal, inputBuffer?.token, out outputBuffer.token, out outFlags2, out isNtlmUsed);
			outputBuffer.size = outputBuffer.token.Length;
			outputBuffer.offset = 0;
			outFlags = ContextFlagsAdapterPal.GetContextFlagsPalFromInterop((global::Interop.NetSecurityNative.GssFlags)outFlags2, isServer: false);
			if (safeDeleteNegoContext.GssContext == null)
			{
				safeDeleteNegoContext.SetGssContext(context2);
			}
			if (num)
			{
				safeDeleteNegoContext.SetAuthenticationPackage(Convert.ToBoolean(isNtlmUsed));
			}
			return new SecurityStatusPal((!num) ? SecurityStatusPalErrorCode.ContinueNeeded : ((safeDeleteNegoContext.IsNtlmUsed && outputBuffer.size > 0) ? SecurityStatusPalErrorCode.OK : SecurityStatusPalErrorCode.CompleteNeeded));
		}
		catch (Exception ex)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(null, ex, "EstablishSecurityContext");
			}
			return new SecurityStatusPal(SecurityStatusPalErrorCode.InternalError, ex);
		}
	}

	internal static SecurityStatusPal InitializeSecurityContext(SafeFreeCredentials credentialsHandle, ref SafeDeleteContext securityContext, string spn, ContextFlagsPal requestedContextFlags, SecurityBuffer[] inSecurityBufferArray, SecurityBuffer outSecurityBuffer, ref ContextFlagsPal contextFlags)
	{
		if (inSecurityBufferArray != null && inSecurityBufferArray.Length > 1)
		{
			throw new PlatformNotSupportedException("No support for channel binding on operating systems other than Windows.");
		}
		SafeFreeNegoCredentials obj = (SafeFreeNegoCredentials)credentialsHandle;
		if (obj.IsDefault && string.IsNullOrEmpty(spn))
		{
			throw new PlatformNotSupportedException("Target name should be non empty if default credentials are passed.");
		}
		SecurityStatusPal result = EstablishSecurityContext(obj, ref securityContext, spn, requestedContextFlags, (inSecurityBufferArray != null && inSecurityBufferArray.Length != 0) ? inSecurityBufferArray[0] : null, outSecurityBuffer, ref contextFlags);
		if (result.ErrorCode == SecurityStatusPalErrorCode.CompleteNeeded)
		{
			ContextFlagsPal contextFlagsPal = ContextFlagsPal.Confidentiality;
			if ((requestedContextFlags & contextFlagsPal) != (contextFlags & contextFlagsPal))
			{
				throw new PlatformNotSupportedException("Requested protection level is not supported with the gssapi implementation currently installed.");
			}
		}
		return result;
	}

	internal static SecurityStatusPal AcceptSecurityContext(SafeFreeCredentials credentialsHandle, ref SafeDeleteContext securityContext, ContextFlagsPal requestedContextFlags, SecurityBuffer[] inSecurityBufferArray, SecurityBuffer outSecurityBuffer, ref ContextFlagsPal contextFlags)
	{
		throw new PlatformNotSupportedException("Server implementation is not supported");
	}

	internal static Win32Exception CreateExceptionFromError(SecurityStatusPal statusCode)
	{
		return new Win32Exception(-2146893792, (statusCode.Exception != null) ? statusCode.Exception.Message : statusCode.ErrorCode.ToString());
	}

	internal static int QueryMaxTokenSize(string package)
	{
		return 0;
	}

	internal static SafeFreeCredentials AcquireDefaultCredential(string package, bool isServer)
	{
		return AcquireCredentialsHandle(package, isServer, new NetworkCredential(string.Empty, string.Empty, string.Empty));
	}

	internal static SafeFreeCredentials AcquireCredentialsHandle(string package, bool isServer, NetworkCredential credential)
	{
		if (isServer)
		{
			throw new PlatformNotSupportedException("Server implementation is not supported");
		}
		bool flag = string.IsNullOrWhiteSpace(credential.UserName) || string.IsNullOrWhiteSpace(credential.Password);
		bool flag2 = string.Equals(package, "NTLM", StringComparison.OrdinalIgnoreCase);
		if (flag2 && flag)
		{
			throw new PlatformNotSupportedException("NTLM authentication is not possible with default credentials on this platform.");
		}
		try
		{
			return flag ? new SafeFreeNegoCredentials(isNtlmOnly: false, string.Empty, string.Empty, string.Empty) : new SafeFreeNegoCredentials(flag2, credential.UserName, credential.Password, credential.Domain);
		}
		catch (Exception ex)
		{
			throw new Win32Exception(-2146893792, ex.Message);
		}
	}

	internal static SecurityStatusPal CompleteAuthToken(ref SafeDeleteContext securityContext, SecurityBuffer[] inSecurityBufferArray)
	{
		return new SecurityStatusPal(SecurityStatusPalErrorCode.OK);
	}

	internal static int Encrypt(SafeDeleteContext securityContext, byte[] buffer, int offset, int count, bool isConfidential, bool isNtlm, ref byte[] output, uint sequenceNumber)
	{
		byte[] array = GssWrap(((SafeDeleteNegoContext)securityContext).GssContext, isConfidential, buffer, offset, count);
		output = new byte[array.Length + 4];
		Array.Copy(array, 0, output, 4, array.Length);
		int num = array.Length;
		output[0] = (byte)(num & 0xFF);
		output[1] = (byte)((num >> 8) & 0xFF);
		output[2] = (byte)((num >> 16) & 0xFF);
		output[3] = (byte)((num >> 24) & 0xFF);
		return num + 4;
	}

	internal static int Decrypt(SafeDeleteContext securityContext, byte[] buffer, int offset, int count, bool isConfidential, bool isNtlm, out int newOffset, uint sequenceNumber)
	{
		if (offset < 0 || offset > ((buffer != null) ? buffer.Length : 0))
		{
			NetEventSource.Fail(securityContext, "Argument 'offset' out of range", "Decrypt");
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || count > ((buffer != null) ? (buffer.Length - offset) : 0))
		{
			NetEventSource.Fail(securityContext, "Argument 'count' out of range.", "Decrypt");
			throw new ArgumentOutOfRangeException("count");
		}
		newOffset = offset;
		return GssUnwrap(((SafeDeleteNegoContext)securityContext).GssContext, buffer, offset, count);
	}

	internal static int VerifySignature(SafeDeleteContext securityContext, byte[] buffer, int offset, int count)
	{
		if (offset < 0 || offset > ((buffer != null) ? buffer.Length : 0))
		{
			NetEventSource.Fail(securityContext, "Argument 'offset' out of range", "VerifySignature");
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || count > ((buffer != null) ? (buffer.Length - offset) : 0))
		{
			NetEventSource.Fail(securityContext, "Argument 'count' out of range.", "VerifySignature");
			throw new ArgumentOutOfRangeException("count");
		}
		return GssUnwrap(((SafeDeleteNegoContext)securityContext).GssContext, buffer, offset, count);
	}

	internal static int MakeSignature(SafeDeleteContext securityContext, byte[] buffer, int offset, int count, ref byte[] output)
	{
		byte[] array = GssWrap(((SafeDeleteNegoContext)securityContext).GssContext, encrypt: false, buffer, offset, count);
		output = new byte[array.Length + 4];
		Array.Copy(array, 0, output, 4, array.Length);
		int num = array.Length;
		output[0] = (byte)(num & 0xFF);
		output[1] = (byte)((num >> 8) & 0xFF);
		output[2] = (byte)((num >> 16) & 0xFF);
		output[3] = (byte)((num >> 24) & 0xFF);
		return num + 4;
	}
}
