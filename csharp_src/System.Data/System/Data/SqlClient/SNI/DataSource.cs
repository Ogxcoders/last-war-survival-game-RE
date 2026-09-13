using System.IO;
using System.Text;

namespace System.Data.SqlClient.SNI;

internal class DataSource
{
	internal enum Protocol
	{
		TCP,
		NP,
		None,
		Admin
	}

	private const char CommaSeparator = ',';

	private const char BackSlashSeparator = '\\';

	private const string DefaultHostName = "localhost";

	private const string DefaultSqlServerInstanceName = "mssqlserver";

	private const string PipeBeginning = "\\\\";

	private const string PipeToken = "pipe";

	private const string LocalDbHost = "(localdb)";

	private const string NamedPipeInstanceNameHeader = "mssql$";

	private const string DefaultPipeName = "sql\\query";

	internal Protocol ConnectionProtocol = Protocol.None;

	private string _workingDataSource;

	private string _dataSourceAfterTrimmingProtocol;

	internal string ServerName { get; private set; }

	internal int Port { get; private set; } = -1;

	public string InstanceName { get; internal set; }

	public string PipeName { get; internal set; }

	public string PipeHostName { get; internal set; }

	internal bool IsBadDataSource { get; private set; }

	internal bool IsSsrpRequired { get; private set; }

	private DataSource(string dataSource)
	{
		_workingDataSource = dataSource.Trim().ToLowerInvariant();
		int num = _workingDataSource.IndexOf(':');
		PopulateProtocol();
		_dataSourceAfterTrimmingProtocol = ((num > -1 && ConnectionProtocol != Protocol.None) ? _workingDataSource.Substring(num + 1).Trim() : _workingDataSource);
		if (_dataSourceAfterTrimmingProtocol.Contains("/"))
		{
			if (ConnectionProtocol == Protocol.None)
			{
				ReportSNIError(SNIProviders.INVALID_PROV);
			}
			else if (ConnectionProtocol == Protocol.NP)
			{
				ReportSNIError(SNIProviders.NP_PROV);
			}
			else if (ConnectionProtocol == Protocol.TCP)
			{
				ReportSNIError(SNIProviders.TCP_PROV);
			}
		}
	}

	private void PopulateProtocol()
	{
		string[] array = _workingDataSource.Split(':');
		if (array.Length <= 1)
		{
			ConnectionProtocol = Protocol.None;
			return;
		}
		switch (array[0].Trim())
		{
		case "tcp":
			ConnectionProtocol = Protocol.TCP;
			break;
		case "np":
			ConnectionProtocol = Protocol.NP;
			break;
		case "admin":
			ConnectionProtocol = Protocol.Admin;
			break;
		default:
			ConnectionProtocol = Protocol.None;
			break;
		}
	}

	public static string GetLocalDBInstance(string dataSource, out bool error)
	{
		string result = null;
		string[] array = dataSource.ToLowerInvariant().Split('\\');
		error = false;
		if (array.Length == 2 && "(localdb)".Equals(array[0].TrimStart()))
		{
			if (string.IsNullOrWhiteSpace(array[1]))
			{
				SNILoadHandle.SingletonInstance.LastError = new SNIError(SNIProviders.INVALID_PROV, 0u, 51u, string.Empty);
				error = true;
				return null;
			}
			result = array[1].Trim();
		}
		return result;
	}

	public static DataSource ParseServerName(string dataSource)
	{
		DataSource dataSource2 = new DataSource(dataSource);
		if (dataSource2.IsBadDataSource)
		{
			return null;
		}
		if (dataSource2.InferNamedPipesInformation())
		{
			return dataSource2;
		}
		if (dataSource2.IsBadDataSource)
		{
			return null;
		}
		if (dataSource2.InferConnectionDetails())
		{
			return dataSource2;
		}
		return null;
	}

	private void InferLocalServerName()
	{
		if (string.IsNullOrEmpty(ServerName) || IsLocalHost(ServerName))
		{
			ServerName = ((ConnectionProtocol == Protocol.Admin) ? Environment.MachineName : "localhost");
		}
	}

	private bool InferConnectionDetails()
	{
		string[] array = _dataSourceAfterTrimmingProtocol.Split('\\', ',');
		ServerName = array[0].Trim();
		int num = _dataSourceAfterTrimmingProtocol.IndexOf(',');
		int num2 = _dataSourceAfterTrimmingProtocol.IndexOf('\\');
		if (num > -1)
		{
			string text = ((num2 <= -1) ? array[1].Trim() : ((num > num2) ? array[2].Trim() : array[1].Trim()));
			if (string.IsNullOrEmpty(text))
			{
				ReportSNIError(SNIProviders.INVALID_PROV);
				return false;
			}
			if (ConnectionProtocol == Protocol.None)
			{
				ConnectionProtocol = Protocol.TCP;
			}
			else if (ConnectionProtocol != Protocol.TCP)
			{
				ReportSNIError(SNIProviders.INVALID_PROV);
				return false;
			}
			if (!int.TryParse(text, out var result))
			{
				ReportSNIError(SNIProviders.TCP_PROV);
				return false;
			}
			if (result < 1)
			{
				ReportSNIError(SNIProviders.TCP_PROV);
				return false;
			}
			Port = result;
		}
		else if (num2 > -1)
		{
			InstanceName = array[1].Trim();
			if (string.IsNullOrWhiteSpace(InstanceName))
			{
				ReportSNIError(SNIProviders.INVALID_PROV);
				return false;
			}
			if ("mssqlserver".Equals(InstanceName))
			{
				ReportSNIError(SNIProviders.INVALID_PROV);
				return false;
			}
			IsSsrpRequired = true;
		}
		InferLocalServerName();
		return true;
	}

	private void ReportSNIError(SNIProviders provider)
	{
		SNILoadHandle.SingletonInstance.LastError = new SNIError(provider, 0u, 25u, string.Empty);
		IsBadDataSource = true;
	}

	private bool InferNamedPipesInformation()
	{
		if (_dataSourceAfterTrimmingProtocol.StartsWith("\\\\") || ConnectionProtocol == Protocol.NP)
		{
			if (!_dataSourceAfterTrimmingProtocol.Contains('\\'))
			{
				string pipeHostName = (ServerName = _dataSourceAfterTrimmingProtocol);
				PipeHostName = pipeHostName;
				InferLocalServerName();
				PipeName = "sql\\query";
				return true;
			}
			try
			{
				string[] array = _dataSourceAfterTrimmingProtocol.Split('\\');
				if (array.Length < 6)
				{
					ReportSNIError(SNIProviders.NP_PROV);
					return false;
				}
				string text = array[2];
				if (string.IsNullOrEmpty(text))
				{
					ReportSNIError(SNIProviders.NP_PROV);
					return false;
				}
				if (!"pipe".Equals(array[3]))
				{
					ReportSNIError(SNIProviders.NP_PROV);
					return false;
				}
				if (array[4].StartsWith("mssql$"))
				{
					InstanceName = array[4].Substring("mssql$".Length);
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 4; i < array.Length - 1; i++)
				{
					stringBuilder.Append(array[i]);
					stringBuilder.Append(Path.DirectorySeparatorChar);
				}
				stringBuilder.Append(array[^1]);
				PipeName = stringBuilder.ToString();
				if (string.IsNullOrWhiteSpace(InstanceName) && !"sql\\query".Equals(PipeName))
				{
					InstanceName = "pipe" + PipeName;
				}
				ServerName = (IsLocalHost(text) ? Environment.MachineName : text);
				PipeHostName = text;
			}
			catch (UriFormatException)
			{
				ReportSNIError(SNIProviders.NP_PROV);
				return false;
			}
			if (ConnectionProtocol == Protocol.None)
			{
				ConnectionProtocol = Protocol.NP;
			}
			else if (ConnectionProtocol != Protocol.NP)
			{
				ReportSNIError(SNIProviders.NP_PROV);
				return false;
			}
			return true;
		}
		return false;
	}

	private static bool IsLocalHost(string serverName)
	{
		if (!".".Equals(serverName) && !"(local)".Equals(serverName))
		{
			return "localhost".Equals(serverName);
		}
		return true;
	}
}
