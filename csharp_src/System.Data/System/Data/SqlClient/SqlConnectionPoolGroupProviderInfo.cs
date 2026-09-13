using System.Data.ProviderBase;

namespace System.Data.SqlClient;

internal sealed class SqlConnectionPoolGroupProviderInfo : DbConnectionPoolGroupProviderInfo
{
	private string _alias;

	private string _failoverPartner;

	private bool _useFailoverPartner;

	internal string FailoverPartner => _failoverPartner;

	internal bool UseFailoverPartner => _useFailoverPartner;

	internal SqlConnectionPoolGroupProviderInfo(SqlConnectionString connectionOptions)
	{
		_failoverPartner = connectionOptions.FailoverPartner;
		if (string.IsNullOrEmpty(_failoverPartner))
		{
			_failoverPartner = null;
		}
	}

	internal void AliasCheck(string server)
	{
		if (!(_alias != server))
		{
			return;
		}
		lock (this)
		{
			if (_alias == null)
			{
				_alias = server;
			}
			else if (_alias != server)
			{
				base.PoolGroup.Clear();
				_alias = server;
			}
		}
	}

	internal void FailoverCheck(SqlInternalConnection connection, bool actualUseFailoverPartner, SqlConnectionString userConnectionOptions, string actualFailoverPartner)
	{
		if (UseFailoverPartner != actualUseFailoverPartner)
		{
			base.PoolGroup.Clear();
			_useFailoverPartner = actualUseFailoverPartner;
		}
		if (_useFailoverPartner || !(_failoverPartner != actualFailoverPartner))
		{
			return;
		}
		lock (this)
		{
			if (_failoverPartner != actualFailoverPartner)
			{
				_failoverPartner = actualFailoverPartner;
			}
		}
	}
}
