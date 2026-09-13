using System.Data.Common;
using System.Threading;

namespace System.Data.SqlClient;

internal sealed class SqlInternalTransaction
{
	internal const long NullTransactionId = 0L;

	private TransactionState _transactionState;

	private TransactionType _transactionType;

	private long _transactionId;

	private int _openResultCount;

	private SqlInternalConnection _innerConnection;

	private bool _disposing;

	private WeakReference _parent;

	internal bool RestoreBrokenConnection { get; set; }

	internal bool ConnectionHasBeenRestored { get; set; }

	internal bool HasParentTransaction
	{
		get
		{
			if (TransactionType.LocalFromAPI != _transactionType)
			{
				if (TransactionType.LocalFromTSQL == _transactionType)
				{
					return _parent != null;
				}
				return false;
			}
			return true;
		}
	}

	internal bool IsAborted => TransactionState.Aborted == _transactionState;

	internal bool IsActive => TransactionState.Active == _transactionState;

	internal bool IsCommitted => TransactionState.Committed == _transactionState;

	internal bool IsCompleted
	{
		get
		{
			if (TransactionState.Aborted != _transactionState && TransactionState.Committed != _transactionState)
			{
				return TransactionState.Unknown == _transactionState;
			}
			return true;
		}
	}

	internal bool IsDelegated => TransactionType.Delegated == _transactionType;

	internal bool IsDistributed => TransactionType.Distributed == _transactionType;

	internal bool IsLocal
	{
		get
		{
			if (TransactionType.LocalFromTSQL != _transactionType)
			{
				return TransactionType.LocalFromAPI == _transactionType;
			}
			return true;
		}
	}

	internal bool IsOrphaned
	{
		get
		{
			if (_parent == null)
			{
				return false;
			}
			if (_parent.Target == null)
			{
				return true;
			}
			return false;
		}
	}

	internal bool IsZombied => _innerConnection == null;

	internal int OpenResultsCount => _openResultCount;

	internal SqlTransaction Parent
	{
		get
		{
			SqlTransaction result = null;
			if (_parent != null)
			{
				result = (SqlTransaction)_parent.Target;
			}
			return result;
		}
	}

	internal long TransactionId
	{
		get
		{
			return _transactionId;
		}
		set
		{
			_transactionId = value;
		}
	}

	internal SqlInternalTransaction(SqlInternalConnection innerConnection, TransactionType type, SqlTransaction outerTransaction)
		: this(innerConnection, type, outerTransaction, 0L)
	{
	}

	internal SqlInternalTransaction(SqlInternalConnection innerConnection, TransactionType type, SqlTransaction outerTransaction, long transactionId)
	{
		_innerConnection = innerConnection;
		_transactionType = type;
		if (outerTransaction != null)
		{
			_parent = new WeakReference(outerTransaction);
		}
		_transactionId = transactionId;
		RestoreBrokenConnection = false;
		ConnectionHasBeenRestored = false;
	}

	internal void Activate()
	{
		_transactionState = TransactionState.Active;
	}

	private void CheckTransactionLevelAndZombie()
	{
		try
		{
			if (!IsZombied && GetServerTransactionLevel() == 0)
			{
				Zombie();
			}
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
			Zombie();
		}
	}

	internal void CloseFromConnection()
	{
		SqlInternalConnection innerConnection = _innerConnection;
		bool flag = true;
		try
		{
			innerConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.IfRollback, null, IsolationLevel.Unspecified, null, isDelegateControlRequest: false);
		}
		catch (Exception e)
		{
			flag = ADP.IsCatchableExceptionType(e);
			throw;
		}
		finally
		{
			if (flag)
			{
				Zombie();
			}
		}
	}

	internal void Commit()
	{
		if (_innerConnection.IsLockedForBulkCopy)
		{
			throw SQL.ConnectionLockedForBcpEvent();
		}
		_innerConnection.ValidateConnectionForExecute(null);
		try
		{
			_innerConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Commit, null, IsolationLevel.Unspecified, null, isDelegateControlRequest: false);
			ZombieParent();
		}
		catch (Exception e)
		{
			if (ADP.IsCatchableExceptionType(e))
			{
				CheckTransactionLevelAndZombie();
			}
			throw;
		}
	}

	internal void Completed(TransactionState transactionState)
	{
		_transactionState = transactionState;
		Zombie();
	}

	internal int DecrementAndObtainOpenResultCount()
	{
		int num = Interlocked.Decrement(ref _openResultCount);
		if (num < 0)
		{
			throw SQL.OpenResultCountExceeded();
		}
		return num;
	}

	internal void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing && _innerConnection != null)
		{
			_disposing = true;
			Rollback();
		}
	}

	private int GetServerTransactionLevel()
	{
		using SqlCommand sqlCommand = new SqlCommand("set @out = @@trancount", (SqlConnection)_innerConnection.Owner);
		sqlCommand.Transaction = Parent;
		SqlParameter sqlParameter = new SqlParameter("@out", SqlDbType.Int);
		sqlParameter.Direction = ParameterDirection.Output;
		sqlCommand.Parameters.Add(sqlParameter);
		sqlCommand.RunExecuteReader(CommandBehavior.Default, RunBehavior.UntilDone, returnStream: false, "GetServerTransactionLevel");
		return (int)sqlParameter.Value;
	}

	internal int IncrementAndObtainOpenResultCount()
	{
		int num = Interlocked.Increment(ref _openResultCount);
		if (num < 0)
		{
			throw SQL.OpenResultCountExceeded();
		}
		return num;
	}

	internal void InitParent(SqlTransaction transaction)
	{
		_parent = new WeakReference(transaction);
	}

	internal void Rollback()
	{
		if (_innerConnection.IsLockedForBulkCopy)
		{
			throw SQL.ConnectionLockedForBcpEvent();
		}
		_innerConnection.ValidateConnectionForExecute(null);
		try
		{
			_innerConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.IfRollback, null, IsolationLevel.Unspecified, null, isDelegateControlRequest: false);
			Zombie();
		}
		catch (Exception e)
		{
			if (ADP.IsCatchableExceptionType(e))
			{
				CheckTransactionLevelAndZombie();
				if (!_disposing)
				{
					throw;
				}
				return;
			}
			throw;
		}
	}

	internal void Rollback(string transactionName)
	{
		if (_innerConnection.IsLockedForBulkCopy)
		{
			throw SQL.ConnectionLockedForBcpEvent();
		}
		_innerConnection.ValidateConnectionForExecute(null);
		if (string.IsNullOrEmpty(transactionName))
		{
			throw SQL.NullEmptyTransactionName();
		}
		try
		{
			_innerConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Rollback, transactionName, IsolationLevel.Unspecified, null, isDelegateControlRequest: false);
		}
		catch (Exception e)
		{
			if (ADP.IsCatchableExceptionType(e))
			{
				CheckTransactionLevelAndZombie();
			}
			throw;
		}
	}

	internal void Save(string savePointName)
	{
		_innerConnection.ValidateConnectionForExecute(null);
		if (string.IsNullOrEmpty(savePointName))
		{
			throw SQL.NullEmptyTransactionName();
		}
		try
		{
			_innerConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Save, savePointName, IsolationLevel.Unspecified, null, isDelegateControlRequest: false);
		}
		catch (Exception e)
		{
			if (ADP.IsCatchableExceptionType(e))
			{
				CheckTransactionLevelAndZombie();
			}
			throw;
		}
	}

	internal void Zombie()
	{
		ZombieParent();
		SqlInternalConnection innerConnection = _innerConnection;
		_innerConnection = null;
		innerConnection?.DisconnectTransaction(this);
	}

	private void ZombieParent()
	{
		if (_parent != null)
		{
			((SqlTransaction)_parent.Target)?.Zombie();
			_parent = null;
		}
	}
}
