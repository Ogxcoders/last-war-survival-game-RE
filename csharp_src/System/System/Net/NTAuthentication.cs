using System.Net.Security;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net;

internal class NTAuthentication
{
	private bool _isServer;

	private SafeFreeCredentials _credentialsHandle;

	private SafeDeleteContext _securityContext;

	private string _spn;

	private int _tokenSize;

	private ContextFlagsPal _requestedContextFlags;

	private ContextFlagsPal _contextFlags;

	private bool _isCompleted;

	private string _package;

	private string _lastProtocolName;

	private string _protocolName;

	private string _clientSpecifiedSpn;

	private ChannelBinding _channelBinding;

	internal bool IsCompleted => _isCompleted;

	internal bool IsValidContext
	{
		get
		{
			if (_securityContext != null)
			{
				return !_securityContext.IsInvalid;
			}
			return false;
		}
	}

	internal string Package => _package;

	internal bool IsServer => _isServer;

	internal string ClientSpecifiedSpn
	{
		get
		{
			if (_clientSpecifiedSpn == null)
			{
				_clientSpecifiedSpn = GetClientSpecifiedSpn();
			}
			return _clientSpecifiedSpn;
		}
	}

	internal string ProtocolName
	{
		get
		{
			if (_protocolName == null)
			{
				string text = null;
				if (IsValidContext)
				{
					text = NegotiateStreamPal.QueryContextAuthenticationPackage(_securityContext);
					if (IsCompleted)
					{
						_protocolName = text;
					}
				}
				return text ?? string.Empty;
			}
			return _protocolName;
		}
	}

	internal bool IsKerberos
	{
		get
		{
			if (_lastProtocolName == null)
			{
				_lastProtocolName = ProtocolName;
			}
			return (object)_lastProtocolName == "Kerberos";
		}
	}

	internal NTAuthentication(bool isServer, string package, NetworkCredential credential, string spn, ContextFlagsPal requestedContextFlags, ChannelBinding channelBinding)
	{
		Initialize(isServer, package, credential, spn, requestedContextFlags, channelBinding);
	}

	private void Initialize(bool isServer, string package, NetworkCredential credential, string spn, ContextFlagsPal requestedContextFlags, ChannelBinding channelBinding)
	{
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Enter(this, package, spn, requestedContextFlags, "Initialize");
		}
		_tokenSize = NegotiateStreamPal.QueryMaxTokenSize(package);
		_isServer = isServer;
		_spn = spn;
		_securityContext = null;
		_requestedContextFlags = requestedContextFlags;
		_package = package;
		_channelBinding = channelBinding;
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Info(this, $"Peer SPN-> '{_spn}'", "Initialize");
		}
		if (credential == CredentialCache.DefaultCredentials)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Info(this, "using DefaultCredentials", "Initialize");
			}
			_credentialsHandle = NegotiateStreamPal.AcquireDefaultCredential(package, _isServer);
		}
		else
		{
			_credentialsHandle = NegotiateStreamPal.AcquireCredentialsHandle(package, _isServer, credential);
		}
	}

	internal SafeDeleteContext GetContext(out SecurityStatusPal status)
	{
		status = new SecurityStatusPal(SecurityStatusPalErrorCode.OK);
		if (!IsCompleted || !IsValidContext)
		{
			NetEventSource.Fail(this, "Should be called only when completed with success, currently is not!", "GetContext");
		}
		if (!IsServer)
		{
			NetEventSource.Fail(this, "The method must not be called by the client side!", "GetContext");
		}
		if (!IsValidContext)
		{
			status = new SecurityStatusPal(SecurityStatusPalErrorCode.InvalidHandle);
			return null;
		}
		return _securityContext;
	}

	internal void CloseContext()
	{
		if (_securityContext != null && !_securityContext.IsClosed)
		{
			_securityContext.Dispose();
		}
	}

	internal int VerifySignature(byte[] buffer, int offset, int count)
	{
		return NegotiateStreamPal.VerifySignature(_securityContext, buffer, offset, count);
	}

	internal int MakeSignature(byte[] buffer, int offset, int count, ref byte[] output)
	{
		return NegotiateStreamPal.MakeSignature(_securityContext, buffer, offset, count, ref output);
	}

	internal string GetOutgoingBlob(string incomingBlob)
	{
		byte[] array = null;
		if (incomingBlob != null && incomingBlob.Length > 0)
		{
			array = Convert.FromBase64String(incomingBlob);
		}
		byte[] array2 = null;
		if ((IsValidContext || IsCompleted) && array == null)
		{
			_isCompleted = true;
		}
		else
		{
			array2 = GetOutgoingBlob(array, throwOnError: true, out var _);
		}
		string result = null;
		if (array2 != null && array2.Length != 0)
		{
			result = Convert.ToBase64String(array2);
		}
		if (IsCompleted)
		{
			CloseContext();
		}
		return result;
	}

	internal byte[] GetOutgoingBlob(byte[] incomingBlob, bool thrownOnError)
	{
		SecurityStatusPal statusCode;
		return GetOutgoingBlob(incomingBlob, thrownOnError, out statusCode);
	}

	internal byte[] GetOutgoingBlob(byte[] incomingBlob, bool throwOnError, out SecurityStatusPal statusCode)
	{
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Enter(this, incomingBlob, "GetOutgoingBlob");
		}
		SecurityBuffer[] inSecurityBufferArray = null;
		if (incomingBlob != null && _channelBinding != null)
		{
			inSecurityBufferArray = new SecurityBuffer[2]
			{
				new SecurityBuffer(incomingBlob, SecurityBufferType.SECBUFFER_TOKEN),
				new SecurityBuffer(_channelBinding)
			};
		}
		else if (incomingBlob != null)
		{
			inSecurityBufferArray = new SecurityBuffer[1]
			{
				new SecurityBuffer(incomingBlob, SecurityBufferType.SECBUFFER_TOKEN)
			};
		}
		else if (_channelBinding != null)
		{
			inSecurityBufferArray = new SecurityBuffer[1]
			{
				new SecurityBuffer(_channelBinding)
			};
		}
		SecurityBuffer securityBuffer = new SecurityBuffer(_tokenSize, SecurityBufferType.SECBUFFER_TOKEN);
		bool flag = _securityContext == null;
		try
		{
			if (!_isServer)
			{
				statusCode = NegotiateStreamPal.InitializeSecurityContext(_credentialsHandle, ref _securityContext, _spn, _requestedContextFlags, inSecurityBufferArray, securityBuffer, ref _contextFlags);
				if (NetEventSource.IsEnabled)
				{
					NetEventSource.Info(this, $"SSPIWrapper.InitializeSecurityContext() returns statusCode:0x{(int)statusCode.ErrorCode:x8} ({statusCode})", "GetOutgoingBlob");
				}
				if (statusCode.ErrorCode == SecurityStatusPalErrorCode.CompleteNeeded)
				{
					statusCode = NegotiateStreamPal.CompleteAuthToken(inSecurityBufferArray: new SecurityBuffer[1] { securityBuffer }, securityContext: ref _securityContext);
					if (NetEventSource.IsEnabled)
					{
						NetEventSource.Info(this, $"SSPIWrapper.CompleteAuthToken() returns statusCode:0x{(int)statusCode.ErrorCode:x8} ({statusCode})", "GetOutgoingBlob");
					}
					securityBuffer.token = null;
				}
			}
			else
			{
				statusCode = NegotiateStreamPal.AcceptSecurityContext(_credentialsHandle, ref _securityContext, _requestedContextFlags, inSecurityBufferArray, securityBuffer, ref _contextFlags);
				if (NetEventSource.IsEnabled)
				{
					NetEventSource.Info(this, $"SSPIWrapper.AcceptSecurityContext() returns statusCode:0x{(int)statusCode.ErrorCode:x8} ({statusCode})", "GetOutgoingBlob");
				}
			}
		}
		finally
		{
			if (flag && _credentialsHandle != null)
			{
				_credentialsHandle.Dispose();
			}
		}
		if (statusCode.ErrorCode >= SecurityStatusPalErrorCode.OutOfMemory)
		{
			CloseContext();
			_isCompleted = true;
			if (throwOnError)
			{
				Exception ex = NegotiateStreamPal.CreateExceptionFromError(statusCode);
				if (NetEventSource.IsEnabled)
				{
					NetEventSource.Exit(this, ex, "GetOutgoingBlob");
				}
				throw ex;
			}
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Exit(this, $"null statusCode:0x{(int)statusCode.ErrorCode:x8} ({statusCode})", "GetOutgoingBlob");
			}
			return null;
		}
		if (flag && _credentialsHandle != null)
		{
			SSPIHandleCache.CacheCredential(_credentialsHandle);
		}
		if (statusCode.ErrorCode == SecurityStatusPalErrorCode.OK)
		{
			_isCompleted = true;
		}
		else if (NetEventSource.IsEnabled && NetEventSource.IsEnabled)
		{
			NetEventSource.Info(this, $"need continue statusCode:0x{(int)statusCode.ErrorCode:x8} ({statusCode}) _securityContext:{_securityContext}", "GetOutgoingBlob");
		}
		if (NetEventSource.IsEnabled && NetEventSource.IsEnabled)
		{
			NetEventSource.Exit(this, $"IsCompleted: {IsCompleted}", "GetOutgoingBlob");
		}
		return securityBuffer.token;
	}

	private string GetClientSpecifiedSpn()
	{
		if (!IsValidContext || !IsCompleted)
		{
			NetEventSource.Fail(this, "Trying to get the client SPN before handshaking is done!", "GetClientSpecifiedSpn");
		}
		string text = NegotiateStreamPal.QueryContextClientSpecifiedSpn(_securityContext);
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Info(this, $"The client specified SPN is [{text}]", "GetClientSpecifiedSpn");
		}
		return text;
	}
}
