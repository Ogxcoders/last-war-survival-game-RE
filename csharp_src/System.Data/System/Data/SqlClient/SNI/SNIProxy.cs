using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace System.Data.SqlClient.SNI;

internal class SNIProxy
{
	internal class SspiClientContextResult
	{
		internal const uint OK = 0u;

		internal const uint Failed = 1u;

		internal const uint KerberosTicketMissing = 2u;
	}

	private const int DefaultSqlServerPort = 1433;

	private const int DefaultSqlServerDacPort = 1434;

	private const string SqlServerSpnHeader = "MSSQLSvc";

	public static readonly SNIProxy Singleton = new SNIProxy();

	public void Terminate()
	{
	}

	public uint EnableSsl(SNIHandle handle, uint options)
	{
		try
		{
			return handle.EnableSsl(options);
		}
		catch (Exception sniException)
		{
			return SNICommon.ReportSNIError(SNIProviders.SSL_PROV, 31u, sniException);
		}
	}

	public uint DisableSsl(SNIHandle handle)
	{
		handle.DisableSsl();
		return 0u;
	}

	public void GenSspiClientContext(SspiClientContextStatus sspiClientContextStatus, byte[] receivedBuff, ref byte[] sendBuff, byte[] serverName)
	{
		SafeDeleteContext securityContext = sspiClientContextStatus.SecurityContext;
		ContextFlagsPal contextFlags = sspiClientContextStatus.ContextFlags;
		SafeFreeCredentials credentialsHandle = sspiClientContextStatus.CredentialsHandle;
		string package = "Negotiate";
		if (securityContext == null)
		{
			credentialsHandle = NegotiateStreamPal.AcquireDefaultCredential(package, isServer: false);
		}
		SecurityBuffer[] array = null;
		array = ((receivedBuff == null) ? new SecurityBuffer[0] : new SecurityBuffer[1]
		{
			new SecurityBuffer(receivedBuff, SecurityBufferType.SECBUFFER_TOKEN)
		});
		SecurityBuffer securityBuffer = new SecurityBuffer(NegotiateStreamPal.QueryMaxTokenSize(package), SecurityBufferType.SECBUFFER_TOKEN);
		ContextFlagsPal requestedContextFlags = ContextFlagsPal.Delegate | ContextFlagsPal.MutualAuth | ContextFlagsPal.Confidentiality | ContextFlagsPal.Connection;
		string spn = Encoding.UTF8.GetString(serverName);
		SecurityStatusPal securityStatusPal = NegotiateStreamPal.InitializeSecurityContext(credentialsHandle, ref securityContext, spn, requestedContextFlags, array, securityBuffer, ref contextFlags);
		if (securityStatusPal.ErrorCode == SecurityStatusPalErrorCode.CompleteNeeded || securityStatusPal.ErrorCode == SecurityStatusPalErrorCode.CompAndContinue)
		{
			array = new SecurityBuffer[1] { securityBuffer };
			securityStatusPal = NegotiateStreamPal.CompleteAuthToken(ref securityContext, array);
			securityBuffer.token = null;
		}
		sendBuff = securityBuffer.token;
		if (sendBuff == null)
		{
			sendBuff = Array.Empty<byte>();
		}
		sspiClientContextStatus.SecurityContext = securityContext;
		sspiClientContextStatus.ContextFlags = contextFlags;
		sspiClientContextStatus.CredentialsHandle = credentialsHandle;
		if (IsErrorStatus(securityStatusPal.ErrorCode))
		{
			if (securityStatusPal.ErrorCode == SecurityStatusPalErrorCode.InternalError)
			{
				throw new InvalidOperationException(SQLMessage.KerberosTicketMissingError() + "\n" + securityStatusPal);
			}
			throw new InvalidOperationException(SQLMessage.SSPIGenerateError() + "\n" + securityStatusPal);
		}
	}

	private static bool IsErrorStatus(SecurityStatusPalErrorCode errorCode)
	{
		if (errorCode != SecurityStatusPalErrorCode.NotSet && errorCode != SecurityStatusPalErrorCode.OK && errorCode != SecurityStatusPalErrorCode.ContinueNeeded && errorCode != SecurityStatusPalErrorCode.CompleteNeeded && errorCode != SecurityStatusPalErrorCode.CompAndContinue && errorCode != SecurityStatusPalErrorCode.ContextExpired && errorCode != SecurityStatusPalErrorCode.CredentialsNeeded)
		{
			return errorCode != SecurityStatusPalErrorCode.Renegotiate;
		}
		return false;
	}

	public uint InitializeSspiPackage(ref uint maxLength)
	{
		throw new PlatformNotSupportedException();
	}

	public uint SetConnectionBufferSize(SNIHandle handle, uint bufferSize)
	{
		handle.SetBufferSize((int)bufferSize);
		return 0u;
	}

	public uint PacketGetData(SNIPacket packet, byte[] inBuff, ref uint dataSize)
	{
		int dataSize2 = 0;
		packet.GetData(inBuff, ref dataSize2);
		dataSize = (uint)dataSize2;
		return 0u;
	}

	public uint ReadSyncOverAsync(SNIHandle handle, out SNIPacket packet, int timeout)
	{
		return handle.Receive(out packet, timeout);
	}

	public uint GetConnectionId(SNIHandle handle, ref Guid clientConnectionId)
	{
		clientConnectionId = handle.ConnectionId;
		return 0u;
	}

	public uint WritePacket(SNIHandle handle, SNIPacket packet, bool sync)
	{
		SNIPacket sNIPacket = packet.Clone();
		uint result;
		if (sync)
		{
			result = handle.Send(sNIPacket);
			sNIPacket.Dispose();
		}
		else
		{
			result = handle.SendAsync(sNIPacket, disposePacketAfterSendAsync: true);
		}
		return result;
	}

	public SNIHandle CreateConnectionHandle(object callbackObject, string fullServerName, bool ignoreSniOpenTimeout, long timerExpire, out byte[] instanceName, ref byte[] spnBuffer, bool flushCache, bool async, bool parallel, bool isIntegratedSecurity)
	{
		instanceName = new byte[1];
		bool error;
		string localDBDataSource = GetLocalDBDataSource(fullServerName, out error);
		if (error)
		{
			return null;
		}
		fullServerName = localDBDataSource ?? fullServerName;
		DataSource dataSource = DataSource.ParseServerName(fullServerName);
		if (dataSource == null)
		{
			return null;
		}
		SNIHandle result = null;
		switch (dataSource.ConnectionProtocol)
		{
		case DataSource.Protocol.TCP:
		case DataSource.Protocol.None:
		case DataSource.Protocol.Admin:
			result = CreateTcpHandle(dataSource, timerExpire, callbackObject, parallel);
			break;
		case DataSource.Protocol.NP:
			result = CreateNpHandle(dataSource, timerExpire, callbackObject, parallel);
			break;
		}
		if (isIntegratedSecurity)
		{
			try
			{
				spnBuffer = GetSqlServerSPN(dataSource);
			}
			catch (Exception sniException)
			{
				SNILoadHandle.SingletonInstance.LastError = new SNIError(SNIProviders.INVALID_PROV, 44u, sniException);
			}
		}
		return result;
	}

	private static byte[] GetSqlServerSPN(DataSource dataSource)
	{
		string serverName = dataSource.ServerName;
		string portOrInstanceName = null;
		if (dataSource.Port != -1)
		{
			portOrInstanceName = dataSource.Port.ToString();
		}
		else if (!string.IsNullOrWhiteSpace(dataSource.InstanceName))
		{
			portOrInstanceName = dataSource.InstanceName;
		}
		else if (dataSource.ConnectionProtocol == DataSource.Protocol.TCP)
		{
			portOrInstanceName = 1433.ToString();
		}
		return GetSqlServerSPN(serverName, portOrInstanceName);
	}

	private static byte[] GetSqlServerSPN(string hostNameOrAddress, string portOrInstanceName)
	{
		IPHostEntry iPHostEntry = null;
		string text;
		try
		{
			iPHostEntry = Dns.GetHostEntry(hostNameOrAddress);
		}
		catch (SocketException)
		{
		}
		finally
		{
			text = iPHostEntry?.HostName ?? hostNameOrAddress;
		}
		string text2 = "MSSQLSvc/" + text;
		if (!string.IsNullOrWhiteSpace(portOrInstanceName))
		{
			text2 = text2 + ":" + portOrInstanceName;
		}
		return Encoding.UTF8.GetBytes(text2);
	}

	private SNITCPHandle CreateTcpHandle(DataSource details, long timerExpire, object callbackObject, bool parallel)
	{
		string serverName = details.ServerName;
		if (string.IsNullOrWhiteSpace(serverName))
		{
			SNILoadHandle.SingletonInstance.LastError = new SNIError(SNIProviders.TCP_PROV, 0u, 25u, string.Empty);
			return null;
		}
		int num = -1;
		bool flag = details.ConnectionProtocol == DataSource.Protocol.Admin;
		if (!details.IsSsrpRequired)
		{
			num = ((details.Port == -1) ? (flag ? 1434 : 1433) : details.Port);
		}
		else
		{
			try
			{
				num = (flag ? SSRP.GetDacPortByInstanceName(serverName, details.InstanceName) : SSRP.GetPortByInstanceName(serverName, details.InstanceName));
			}
			catch (SocketException sniException)
			{
				SNILoadHandle.SingletonInstance.LastError = new SNIError(SNIProviders.TCP_PROV, 25u, sniException);
				return null;
			}
		}
		return new SNITCPHandle(serverName, num, timerExpire, callbackObject, parallel);
	}

	private SNINpHandle CreateNpHandle(DataSource details, long timerExpire, object callbackObject, bool parallel)
	{
		if (parallel)
		{
			SNICommon.ReportSNIError(SNIProviders.NP_PROV, 0u, 49u, string.Empty);
			return null;
		}
		return new SNINpHandle(details.PipeHostName, details.PipeName, timerExpire, callbackObject);
	}

	public uint ReadAsync(SNIHandle handle, out SNIPacket packet)
	{
		packet = null;
		return handle.ReceiveAsync(ref packet);
	}

	public void PacketSetData(SNIPacket packet, byte[] data, int length)
	{
		packet.SetData(data, length);
	}

	public void PacketRelease(SNIPacket packet)
	{
		packet.Release();
	}

	public uint CheckConnection(SNIHandle handle)
	{
		return handle.CheckConnection();
	}

	public SNIError GetLastError()
	{
		return SNILoadHandle.SingletonInstance.LastError;
	}

	private string GetLocalDBDataSource(string fullServerName, out bool error)
	{
		string result = null;
		bool error2;
		string localDBInstance = DataSource.GetLocalDBInstance(fullServerName, out error2);
		if (error2)
		{
			error = true;
			return null;
		}
		if (!string.IsNullOrEmpty(localDBInstance))
		{
			result = LocalDB.GetLocalDBConnectionString(localDBInstance);
			if (fullServerName == null)
			{
				error = true;
				return null;
			}
		}
		error = false;
		return result;
	}
}
