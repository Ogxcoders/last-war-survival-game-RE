using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Transactions;

namespace System.Data.SqlClient;

internal sealed class SqlDelegatedTransaction : IPromotableSinglePhaseNotification, ITransactionPromoter
{
	private static int _objectTypeCount;

	private readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	private const int _globalTransactionsTokenVersionSizeInBytes = 4;

	private SqlInternalConnection _connection;

	private IsolationLevel _isolationLevel;

	private SqlInternalTransaction _internalTransaction;

	private Transaction _atomicTransaction;

	private bool _active;

	internal int ObjectID => _objectID;

	internal Transaction Transaction => _atomicTransaction;

	internal bool IsActive => _active;

	internal SqlDelegatedTransaction(SqlInternalConnection connection, Transaction tx)
	{
		_connection = connection;
		_atomicTransaction = tx;
		_active = false;
		System.Transactions.IsolationLevel isolationLevel = tx.IsolationLevel;
		switch (isolationLevel)
		{
		case System.Transactions.IsolationLevel.ReadCommitted:
			_isolationLevel = IsolationLevel.ReadCommitted;
			break;
		case System.Transactions.IsolationLevel.ReadUncommitted:
			_isolationLevel = IsolationLevel.ReadUncommitted;
			break;
		case System.Transactions.IsolationLevel.RepeatableRead:
			_isolationLevel = IsolationLevel.RepeatableRead;
			break;
		case System.Transactions.IsolationLevel.Serializable:
			_isolationLevel = IsolationLevel.Serializable;
			break;
		case System.Transactions.IsolationLevel.Snapshot:
			_isolationLevel = IsolationLevel.Snapshot;
			break;
		default:
			throw SQL.UnknownSysTxIsolationLevel(isolationLevel);
		}
	}

	public void Initialize()
	{
		SqlInternalConnection connection = _connection;
		SqlConnection connection2 = connection.Connection;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			if (connection.IsEnlistedInTransaction)
			{
				connection.EnlistNull();
			}
			_internalTransaction = new SqlInternalTransaction(connection, TransactionType.Delegated, null);
			connection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Begin, null, _isolationLevel, _internalTransaction, isDelegateControlRequest: true);
			if (connection.CurrentTransaction == null)
			{
				connection.DoomThisConnection();
				throw ADP.InternalError(ADP.InternalErrorCode.UnknownTransactionFailure);
			}
			_active = true;
		}
		catch (OutOfMemoryException e)
		{
			connection2.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			connection2.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			connection2.Abort(e3);
			throw;
		}
	}

	public byte[] Promote()
	{
		SqlInternalConnection validConnection = GetValidConnection();
		byte[] result = null;
		SqlConnection connection = validConnection.Connection;
		RuntimeHelpers.PrepareConstrainedRegions();
		Exception ex;
		try
		{
			lock (validConnection)
			{
				try
				{
					ValidateActiveOnConnection(validConnection);
					validConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Promote, null, IsolationLevel.Unspecified, _internalTransaction, isDelegateControlRequest: true);
					result = _connection.PromotedDTCToken;
					if (_connection.IsGlobalTransaction)
					{
						if (SysTxForGlobalTransactions.SetDistributedTransactionIdentifier == null)
						{
							throw SQL.UnsupportedSysTxForGlobalTransactions();
						}
						if (!_connection.IsGlobalTransactionsEnabledForServer)
						{
							throw SQL.GlobalTransactionsNotEnabled();
						}
						SysTxForGlobalTransactions.SetDistributedTransactionIdentifier.Invoke(_atomicTransaction, new object[2]
						{
							this,
							GetGlobalTxnIdentifierFromToken()
						});
					}
					ex = null;
				}
				catch (SqlException ex2)
				{
					ex = ex2;
					validConnection.DoomThisConnection();
				}
				catch (InvalidOperationException ex3)
				{
					ex = ex3;
					validConnection.DoomThisConnection();
				}
			}
		}
		catch (OutOfMemoryException e)
		{
			connection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			connection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			connection.Abort(e3);
			throw;
		}
		if (ex != null)
		{
			throw SQL.PromotionFailed(ex);
		}
		return result;
	}

	public void Rollback(SinglePhaseEnlistment enlistment)
	{
		SqlInternalConnection validConnection = GetValidConnection();
		SqlConnection connection = validConnection.Connection;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			lock (validConnection)
			{
				try
				{
					ValidateActiveOnConnection(validConnection);
					_active = false;
					_connection = null;
					if (!_internalTransaction.IsAborted)
					{
						validConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Rollback, null, IsolationLevel.Unspecified, _internalTransaction, isDelegateControlRequest: true);
					}
				}
				catch (SqlException)
				{
					validConnection.DoomThisConnection();
				}
				catch (InvalidOperationException)
				{
					validConnection.DoomThisConnection();
				}
			}
			validConnection.CleanupConnectionOnTransactionCompletion(_atomicTransaction);
			enlistment.Aborted();
		}
		catch (OutOfMemoryException e)
		{
			connection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			connection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			connection.Abort(e3);
			throw;
		}
	}

	public void SinglePhaseCommit(SinglePhaseEnlistment enlistment)
	{
		SqlInternalConnection validConnection = GetValidConnection();
		SqlConnection connection = validConnection.Connection;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			if (validConnection.IsConnectionDoomed)
			{
				lock (validConnection)
				{
					_active = false;
					_connection = null;
				}
				enlistment.Aborted(SQL.ConnectionDoomed());
				return;
			}
			Exception ex;
			lock (validConnection)
			{
				try
				{
					ValidateActiveOnConnection(validConnection);
					_active = false;
					_connection = null;
					validConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Commit, null, IsolationLevel.Unspecified, _internalTransaction, isDelegateControlRequest: true);
					ex = null;
				}
				catch (SqlException ex2)
				{
					ex = ex2;
					validConnection.DoomThisConnection();
				}
				catch (InvalidOperationException ex3)
				{
					ex = ex3;
					validConnection.DoomThisConnection();
				}
			}
			if (ex != null)
			{
				if (_internalTransaction.IsCommitted)
				{
					enlistment.Committed();
				}
				else if (_internalTransaction.IsAborted)
				{
					enlistment.Aborted(ex);
				}
				else
				{
					enlistment.InDoubt(ex);
				}
			}
			validConnection.CleanupConnectionOnTransactionCompletion(_atomicTransaction);
			if (ex == null)
			{
				enlistment.Committed();
			}
		}
		catch (OutOfMemoryException e)
		{
			connection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			connection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			connection.Abort(e3);
			throw;
		}
	}

	internal void TransactionEnded(Transaction transaction)
	{
		SqlInternalConnection connection = _connection;
		if (connection == null)
		{
			return;
		}
		lock (connection)
		{
			if (_atomicTransaction.Equals(transaction))
			{
				_active = false;
				_connection = null;
			}
		}
	}

	private SqlInternalConnection GetValidConnection()
	{
		SqlInternalConnection connection = _connection;
		if (connection == null)
		{
			throw ADP.ObjectDisposed(this);
		}
		return connection;
	}

	private void ValidateActiveOnConnection(SqlInternalConnection connection)
	{
		if (!_active || connection != _connection || connection.DelegatedTransaction != this)
		{
			connection?.DoomThisConnection();
			if (connection != _connection && _connection != null)
			{
				_connection.DoomThisConnection();
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnpooledObjectHasWrongOwner);
		}
	}

	private Guid GetGlobalTxnIdentifierFromToken()
	{
		byte[] array = new byte[16];
		Array.Copy(_connection.PromotedDTCToken, 4, array, 0, array.Length);
		return new Guid(array);
	}
}
