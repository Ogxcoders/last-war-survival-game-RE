namespace System.Data.SqlClient;

internal sealed class SQLMessage
{
	private SQLMessage()
	{
	}

	internal static string CultureIdError()
	{
		return global::SR.GetString("The Collation specified by SQL Server is not supported.");
	}

	internal static string EncryptionNotSupportedByClient()
	{
		return global::SR.GetString("The instance of SQL Server you attempted to connect to requires encryption but this machine does not support it.");
	}

	internal static string EncryptionNotSupportedByServer()
	{
		return global::SR.GetString("The instance of SQL Server you attempted to connect to does not support encryption.");
	}

	internal static string OperationCancelled()
	{
		return global::SR.GetString("Operation cancelled by user.");
	}

	internal static string SevereError()
	{
		return global::SR.GetString("A severe error occurred on the current command.  The results, if any, should be discarded.");
	}

	internal static string SSPIInitializeError()
	{
		return global::SR.GetString("Cannot initialize SSPI package.");
	}

	internal static string SSPIGenerateError()
	{
		return global::SR.GetString("Failed to generate SSPI context.");
	}

	internal static string SqlServerBrowserNotAccessible()
	{
		return global::SR.GetString("Cannot connect to SQL Server Browser. Ensure SQL Server Browser has been started.");
	}

	internal static string KerberosTicketMissingError()
	{
		return global::SR.GetString("Cannot authenticate using Kerberos. Ensure Kerberos has been initialized on the client with 'kinit' and a Service Principal Name has been registered for the SQL Server to allow Kerberos authentication.");
	}

	internal static string Timeout()
	{
		return global::SR.GetString("Timeout expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.");
	}

	internal static string Timeout_PreLogin_Begin()
	{
		return global::SR.GetString("Connection Timeout Expired.  The timeout period elapsed at the start of the pre-login phase.  This could be because of insufficient time provided for connection timeout.");
	}

	internal static string Timeout_PreLogin_InitializeConnection()
	{
		return global::SR.GetString("Connection Timeout Expired.  The timeout period elapsed while attempting to create and initialize a socket to the server.  This could be either because the server was unreachable or unable to respond back in time.");
	}

	internal static string Timeout_PreLogin_SendHandshake()
	{
		return global::SR.GetString("Connection Timeout Expired.  The timeout period elapsed while making a pre-login handshake request.  This could be because the server was unable to respond back in time.");
	}

	internal static string Timeout_PreLogin_ConsumeHandshake()
	{
		return global::SR.GetString("Connection Timeout Expired.  The timeout period elapsed while attempting to consume the pre-login handshake acknowledgement.  This could be because the pre-login handshake failed or the server was unable to respond back in time.");
	}

	internal static string Timeout_Login_Begin()
	{
		return global::SR.GetString("Connection Timeout Expired.  The timeout period elapsed at the start of the login phase.  This could be because of insufficient time provided for connection timeout.");
	}

	internal static string Timeout_Login_ProcessConnectionAuth()
	{
		return global::SR.GetString("Connection Timeout Expired.  The timeout period elapsed while attempting to authenticate the login.  This could be because the server failed to authenticate the user or the server was unable to respond back in time.");
	}

	internal static string Timeout_PostLogin()
	{
		return global::SR.GetString("Connection Timeout Expired.  The timeout period elapsed during the post-login phase.  The connection could have timed out while waiting for server to complete the login process and respond; Or it could have timed out while attempting to create multiple active connections.");
	}

	internal static string Timeout_FailoverInfo()
	{
		return global::SR.GetString("This failure occurred while attempting to connect to the {0} server.");
	}

	internal static string Timeout_RoutingDestination()
	{
		return global::SR.GetString("This failure occurred while attempting to connect to the routing destination. The duration spent while attempting to connect to the original server was - [Pre-Login] initialization={0}; handshake={1}; [Login] initialization={2}; authentication={3}; [Post-Login] complete={4};  ");
	}

	internal static string Duration_PreLogin_Begin(long PreLoginBeginDuration)
	{
		return global::SR.GetString("The duration spent while attempting to connect to this server was - [Pre-Login] initialization={0};", PreLoginBeginDuration);
	}

	internal static string Duration_PreLoginHandshake(long PreLoginBeginDuration, long PreLoginHandshakeDuration)
	{
		return global::SR.GetString("The duration spent while attempting to connect to this server was - [Pre-Login] initialization={0}; handshake={1}; ", PreLoginBeginDuration, PreLoginHandshakeDuration);
	}

	internal static string Duration_Login_Begin(long PreLoginBeginDuration, long PreLoginHandshakeDuration, long LoginBeginDuration)
	{
		return global::SR.GetString("The duration spent while attempting to connect to this server was - [Pre-Login] initialization={0}; handshake={1}; [Login] initialization={2}; ", PreLoginBeginDuration, PreLoginHandshakeDuration, LoginBeginDuration);
	}

	internal static string Duration_Login_ProcessConnectionAuth(long PreLoginBeginDuration, long PreLoginHandshakeDuration, long LoginBeginDuration, long LoginAuthDuration)
	{
		return global::SR.GetString("The duration spent while attempting to connect to this server was - [Pre-Login] initialization={0}; handshake={1}; [Login] initialization={2}; authentication={3}; ", PreLoginBeginDuration, PreLoginHandshakeDuration, LoginBeginDuration, LoginAuthDuration);
	}

	internal static string Duration_PostLogin(long PreLoginBeginDuration, long PreLoginHandshakeDuration, long LoginBeginDuration, long LoginAuthDuration, long PostLoginDuration)
	{
		return global::SR.GetString("The duration spent while attempting to connect to this server was - [Pre-Login] initialization={0}; handshake={1}; [Login] initialization={2}; authentication={3}; [Post-Login] complete={4}; ", PreLoginBeginDuration, PreLoginHandshakeDuration, LoginBeginDuration, LoginAuthDuration, PostLoginDuration);
	}

	internal static string UserInstanceFailure()
	{
		return global::SR.GetString("A user instance was requested in the connection string but the server specified does not support this option.");
	}

	internal static string PreloginError()
	{
		return global::SR.GetString("A connection was successfully established with the server, but then an error occurred during the pre-login handshake.");
	}

	internal static string ExClientConnectionId()
	{
		return global::SR.GetString("ClientConnectionId:{0}");
	}

	internal static string ExErrorNumberStateClass()
	{
		return global::SR.GetString("Error Number:{0},State:{1},Class:{2}");
	}

	internal static string ExOriginalClientConnectionId()
	{
		return global::SR.GetString("ClientConnectionId before routing:{0}");
	}

	internal static string ExRoutingDestination()
	{
		return global::SR.GetString("Routing Destination:{0}");
	}
}
