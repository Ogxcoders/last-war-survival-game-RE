using System.Data.Common;
using System.Threading.Tasks;
using System.Transactions;

namespace System.Data.ProviderBase;

internal abstract class DbConnectionClosed : DbConnectionInternal
{
	public override string ServerVersion
	{
		get
		{
			throw ADP.ClosedConnectionError();
		}
	}

	protected DbConnectionClosed(ConnectionState state, bool hidePassword, bool allowSetConnectionString)
		: base(state, hidePassword, allowSetConnectionString)
	{
	}

	public override DbTransaction BeginTransaction(IsolationLevel il)
	{
		throw ADP.ClosedConnectionError();
	}

	public override void ChangeDatabase(string database)
	{
		throw ADP.ClosedConnectionError();
	}

	internal override void CloseConnection(DbConnection owningObject, DbConnectionFactory connectionFactory)
	{
	}

	protected override void Deactivate()
	{
		ADP.ClosedConnectionError();
	}

	protected internal override DataTable GetSchema(DbConnectionFactory factory, DbConnectionPoolGroup poolGroup, DbConnection outerConnection, string collectionName, string[] restrictions)
	{
		throw ADP.ClosedConnectionError();
	}

	protected override DbReferenceCollection CreateReferenceCollection()
	{
		throw ADP.ClosedConnectionError();
	}

	internal override bool TryOpenConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions)
	{
		return TryOpenConnectionInternal(outerConnection, connectionFactory, retry, userOptions);
	}

	protected override void Activate(Transaction transaction)
	{
		throw ADP.ClosedConnectionError();
	}

	public override void EnlistTransaction(Transaction transaction)
	{
		throw ADP.ClosedConnectionError();
	}
}
