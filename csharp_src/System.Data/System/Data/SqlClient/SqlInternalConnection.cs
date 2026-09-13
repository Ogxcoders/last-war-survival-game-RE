using System.Data.Common;
using System.Data.ProviderBase;
using System.Threading;
using System.Transactions;

namespace System.Data.SqlClient;

internal abstract class SqlInternalConnection : DbConnectionInternal
{
	internal enum TransactionRequest
	{
		Begin,
		Promote,
		Commit,
		Rollback,
		IfRollback,
		Save
	}

	private readonly SqlConnectionString _connectionOptions;

	private bool _isEnlistedInTransaction;

	private byte[] _promotedDTCToken;

	private byte[] _whereAbouts;

	private bool _isGlobalTransaction;

	private bool _isGlobalTransactionEnabledForServer;

	private static readonly Guid _globalTransactionTMID = new Guid("1c742caf-6680-40ea-9c26-6b6846079764");

	internal string CurrentDatabase { get; set; }

	internal string CurrentDataSource { get; set; }

	internal SqlDelegatedTransaction DelegatedTransaction { get; set; }

	internal SqlConnection Connection => (SqlConnection)base.Owner;

	internal SqlConnectionString ConnectionOptions => _connectionOptions;

	internal abstract SqlInternalTransaction CurrentTransaction { get; }

	internal virtual SqlInternalTransaction AvailableInternalTransaction => CurrentTransaction;

	internal abstract SqlInternalTransaction PendingTransaction { get; }

	protected internal override bool IsNonPoolableTransactionRoot => IsTransactionRoot;

	internal override bool IsTransactionRoot => DelegatedTransaction?.IsActive ?? false;

	internal bool HasLocalTransaction => CurrentTransaction?.IsLocal ?? false;

	internal bool HasLocalTransactionFromAPI => CurrentTransaction?.HasParentTransaction ?? false;

	internal bool IsEnlistedInTransaction => _isEnlistedInTransaction;

	internal abstract bool IsLockedForBulkCopy { get; }

	internal abstract bool IsKatmaiOrNewer { get; }

	internal byte[] PromotedDTCToken
	{
		get
		{
			return _promotedDTCToken;
		}
		set
		{
			_promotedDTCToken = value;
		}
	}

	internal bool IsGlobalTransaction
	{
		get
		{
			return _isGlobalTransaction;
		}
		set
		{
			_isGlobalTransaction = value;
		}
	}

	internal bool IsGlobalTransactionsEnabledForServer
	{
		get
		{
			return _isGlobalTransactionEnabledForServer;
		}
		set
		{
			_isGlobalTransactionEnabledForServer = value;
		}
	}

	internal SqlInternalConnection(SqlConnectionString connectionOptions)
	{
		_connectionOptions = connectionOptions;
	}

	public override DbTransaction BeginTransaction(IsolationLevel iso)
	{
		return BeginSqlTransaction(iso, null, shouldReconnect: false);
	}

	internal virtual SqlTransaction BeginSqlTransaction(IsolationLevel iso, string transactionName, bool shouldReconnect)
	{
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Connection.Statistics);
			ValidateConnectionForExecute(null);
			if (HasLocalTransactionFromAPI)
			{
				throw ADP.ParallelTransactionsNotSupported(Connection);
			}
			if (iso == IsolationLevel.Unspecified)
			{
				iso = IsolationLevel.ReadCommitted;
			}
			SqlTransaction sqlTransaction = new SqlTransaction(this, Connection, iso, AvailableInternalTransaction);
			sqlTransaction.InternalTransaction.RestoreBrokenConnection = shouldReconnect;
			ExecuteTransaction(TransactionRequest.Begin, transactionName, iso, sqlTransaction.InternalTransaction, isDelegateControlRequest: false);
			sqlTransaction.InternalTransaction.RestoreBrokenConnection = false;
			return sqlTransaction;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public override void ChangeDatabase(string database)
	{
		if (string.IsNullOrEmpty(database))
		{
			throw ADP.EmptyDatabaseName();
		}
		ValidateConnectionForExecute(null);
		ChangeDatabaseInternal(database);
	}

	protected abstract void ChangeDatabaseInternal(string database);

	protected override void CleanupTransactionOnCompletion(Transaction transaction)
	{
		DelegatedTransaction?.TransactionEnded(transaction);
	}

	protected override DbReferenceCollection CreateReferenceCollection()
	{
		return new SqlReferenceCollection();
	}

	protected override void Deactivate()
	{
		try
		{
			((SqlReferenceCollection)base.ReferenceCollection)?.Deactivate();
			InternalDeactivate();
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
			DoomThisConnection();
		}
	}

	internal abstract void DisconnectTransaction(SqlInternalTransaction internalTransaction);

	public override void Dispose()
	{
		_whereAbouts = null;
		base.Dispose();
	}

	protected void Enlist(Transaction tx)
	{
		if (null == tx)
		{
			if (IsEnlistedInTransaction)
			{
				EnlistNull();
				return;
			}
			Transaction enlistedTransaction = base.EnlistedTransaction;
			if (enlistedTransaction != null && enlistedTransaction.TransactionInformation.Status != TransactionStatus.Active)
			{
				EnlistNull();
			}
		}
		else if (!tx.Equals(base.EnlistedTransaction))
		{
			EnlistNonNull(tx);
		}
	}

	private void EnlistNonNull(Transaction tx)
	{
		bool flag = false;
		SqlDelegatedTransaction sqlDelegatedTransaction = new SqlDelegatedTransaction(this, tx);
		try
		{
			flag = ((!_isGlobalTransaction) ? tx.EnlistPromotableSinglePhase(sqlDelegatedTransaction) : ((!(SysTxForGlobalTransactions.EnlistPromotableSinglePhase == null)) ? ((bool)SysTxForGlobalTransactions.EnlistPromotableSinglePhase.Invoke(tx, new object[2] { sqlDelegatedTransaction, _globalTransactionTMID })) : tx.EnlistPromotableSinglePhase(sqlDelegatedTransaction)));
			if (flag)
			{
				DelegatedTransaction = sqlDelegatedTransaction;
			}
		}
		catch (SqlException ex)
		{
			if (ex.Class >= 20)
			{
				throw;
			}
			if (this is SqlInternalConnectionTds { Parser: var parser } && (parser == null || parser.State != TdsParserState.OpenLoggedIn))
			{
				throw;
			}
		}
		if (!flag)
		{
			byte[] array = null;
			if (_isGlobalTransaction)
			{
				if (SysTxForGlobalTransactions.GetPromotedToken == null)
				{
					throw SQL.UnsupportedSysTxForGlobalTransactions();
				}
				array = (byte[])SysTxForGlobalTransactions.GetPromotedToken.Invoke(tx, null);
			}
			else
			{
				if (_whereAbouts == null)
				{
					byte[] dTCAddress = GetDTCAddress();
					if (dTCAddress == null)
					{
						throw SQL.CannotGetDTCAddress();
					}
					_whereAbouts = dTCAddress;
				}
				array = GetTransactionCookie(tx, _whereAbouts);
			}
			PropagateTransactionCookie(array);
			_isEnlistedInTransaction = true;
		}
		base.EnlistedTransaction = tx;
	}

	internal void EnlistNull()
	{
		PropagateTransactionCookie(null);
		_isEnlistedInTransaction = false;
		base.EnlistedTransaction = null;
	}

	public override void EnlistTransaction(Transaction transaction)
	{
		ValidateConnectionForExecute(null);
		if (HasLocalTransaction)
		{
			throw ADP.LocalTransactionPresent();
		}
		if (null != transaction && transaction.Equals(base.EnlistedTransaction))
		{
			return;
		}
		try
		{
			Enlist(transaction);
		}
		catch (OutOfMemoryException e)
		{
			Connection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			Connection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			Connection.Abort(e3);
			throw;
		}
	}

	internal abstract void ExecuteTransaction(TransactionRequest transactionRequest, string name, IsolationLevel iso, SqlInternalTransaction internalTransaction, bool isDelegateControlRequest);

	internal SqlDataReader FindLiveReader(SqlCommand command)
	{
		SqlDataReader result = null;
		SqlReferenceCollection sqlReferenceCollection = (SqlReferenceCollection)base.ReferenceCollection;
		if (sqlReferenceCollection != null)
		{
			result = sqlReferenceCollection.FindLiveReader(command);
		}
		return result;
	}

	internal SqlCommand FindLiveCommand(TdsParserStateObject stateObj)
	{
		SqlCommand result = null;
		SqlReferenceCollection sqlReferenceCollection = (SqlReferenceCollection)base.ReferenceCollection;
		if (sqlReferenceCollection != null)
		{
			result = sqlReferenceCollection.FindLiveCommand(stateObj);
		}
		return result;
	}

	protected abstract byte[] GetDTCAddress();

	private static byte[] GetTransactionCookie(Transaction transaction, byte[] whereAbouts)
	{
		byte[] result = null;
		if (null != transaction)
		{
			result = TransactionInterop.GetExportCookie(transaction, whereAbouts);
		}
		return result;
	}

	protected virtual void InternalDeactivate()
	{
	}

	internal void OnError(SqlException exception, bool breakConnection, Action<Action> wrapCloseInAction = null)
	{
		if (breakConnection)
		{
			DoomThisConnection();
		}
		SqlConnection connection = Connection;
		if (connection != null)
		{
			connection.OnError(exception, breakConnection, wrapCloseInAction);
		}
		else if (exception.Class >= 11)
		{
			throw exception;
		}
	}

	protected abstract void PropagateTransactionCookie(byte[] transactionCookie);

	internal abstract void ValidateConnectionForExecute(SqlCommand command);
}
