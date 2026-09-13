using System.Globalization;

namespace System.Data.SqlClient;

internal sealed class ServerInfo
{
	private string _userServerName;

	internal readonly string PreRoutingServerName;

	internal string ExtendedServerName { get; private set; }

	internal string ResolvedServerName { get; private set; }

	internal string ResolvedDatabaseName { get; private set; }

	internal string UserProtocol { get; private set; }

	internal string UserServerName
	{
		get
		{
			return _userServerName;
		}
		private set
		{
			_userServerName = value;
		}
	}

	internal ServerInfo(SqlConnectionString userOptions)
		: this(userOptions, userOptions.DataSource)
	{
	}

	internal ServerInfo(SqlConnectionString userOptions, string serverName)
	{
		UserServerName = serverName ?? string.Empty;
		UserProtocol = string.Empty;
		ResolvedDatabaseName = userOptions.InitialCatalog;
		PreRoutingServerName = null;
	}

	internal ServerInfo(SqlConnectionString userOptions, RoutingInfo routing, string preRoutingServerName)
	{
		if (routing == null || routing.ServerName == null)
		{
			UserServerName = string.Empty;
		}
		else
		{
			UserServerName = string.Format(CultureInfo.InvariantCulture, "{0},{1}", routing.ServerName, routing.Port);
		}
		PreRoutingServerName = preRoutingServerName;
		UserProtocol = "tcp";
		SetDerivedNames(UserProtocol, UserServerName);
		ResolvedDatabaseName = userOptions.InitialCatalog;
	}

	internal void SetDerivedNames(string protocol, string serverName)
	{
		if (!string.IsNullOrEmpty(protocol))
		{
			ExtendedServerName = protocol + ":" + serverName;
		}
		else
		{
			ExtendedServerName = serverName;
		}
		ResolvedServerName = serverName;
	}
}
