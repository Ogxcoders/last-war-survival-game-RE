using System.Data.Common;
using System.Threading.Tasks;

namespace System.Data.ProviderBase;

internal abstract class DbConnectionBusy : DbConnectionClosed
{
	protected DbConnectionBusy(ConnectionState state)
		: base(state, hidePassword: true, allowSetConnectionString: false)
	{
	}

	internal override bool TryOpenConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions)
	{
		throw ADP.ConnectionAlreadyOpen(base.State);
	}
}
