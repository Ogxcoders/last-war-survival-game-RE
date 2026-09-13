using System.Data.ProviderBase;

namespace System.Data.SqlClient;

internal sealed class SqlConnectionPoolProviderInfo : DbConnectionPoolProviderInfo
{
	private string _instanceName;

	internal string InstanceName
	{
		get
		{
			return _instanceName;
		}
		set
		{
			_instanceName = value;
		}
	}
}
