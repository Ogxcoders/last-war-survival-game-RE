using System.Data.Common;
using System.Threading.Tasks;

namespace System.Data.ProviderBase;

internal sealed class DbConnectionClosedConnecting : DbConnectionBusy
{
	internal static readonly DbConnectionInternal SingletonInstance = new DbConnectionClosedConnecting();

	private DbConnectionClosedConnecting()
		: base(ConnectionState.Connecting)
	{
	}

	internal override void CloseConnection(DbConnection owningObject, DbConnectionFactory connectionFactory)
	{
		connectionFactory.SetInnerConnectionTo(owningObject, DbConnectionClosedPreviouslyOpened.SingletonInstance);
	}

	internal override bool TryReplaceConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions)
	{
		return TryOpenConnection(outerConnection, connectionFactory, retry, userOptions);
	}

	internal override bool TryOpenConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions)
	{
		if (retry == null || !retry.Task.IsCompleted)
		{
			throw ADP.ConnectionAlreadyOpen(base.State);
		}
		DbConnectionInternal result = retry.Task.Result;
		if (result == null)
		{
			connectionFactory.SetInnerConnectionTo(outerConnection, this);
			throw ADP.InternalConnectionError(ADP.ConnectionError.GetConnectionReturnsNull);
		}
		connectionFactory.SetInnerConnectionEvent(outerConnection, result);
		return true;
	}
}
