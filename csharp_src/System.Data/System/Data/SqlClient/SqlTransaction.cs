using System.Data.Common;

namespace System.Data.SqlClient;

public sealed class SqlTransaction : DbTransaction
{
	private static readonly DiagnosticListener s_diagnosticListener = new DiagnosticListener("SqlClientDiagnosticListener");

	internal readonly IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

	private SqlInternalTransaction _internalTransaction;

	private SqlConnection _connection;

	private bool _isFromAPI;

	public new SqlConnection Connection
	{
		get
		{
			if (IsZombied)
			{
				return null;
			}
			return _connection;
		}
	}

	protected override DbConnection DbConnection => Connection;

	internal SqlInternalTransaction InternalTransaction => _internalTransaction;

	public override IsolationLevel IsolationLevel
	{
		get
		{
			ZombieCheck();
			return _isolationLevel;
		}
	}

	private bool IsYukonPartialZombie
	{
		get
		{
			if (_internalTransaction != null)
			{
				return _internalTransaction.IsCompleted;
			}
			return false;
		}
	}

	internal bool IsZombied
	{
		get
		{
			if (_internalTransaction != null)
			{
				return _internalTransaction.IsCompleted;
			}
			return true;
		}
	}

	internal SqlStatistics Statistics
	{
		get
		{
			if (_connection != null && _connection.StatisticsEnabled)
			{
				return _connection.Statistics;
			}
			return null;
		}
	}

	internal SqlTransaction(SqlInternalConnection internalConnection, SqlConnection con, IsolationLevel iso, SqlInternalTransaction internalTransaction)
	{
		_isolationLevel = iso;
		_connection = con;
		if (internalTransaction == null)
		{
			_internalTransaction = new SqlInternalTransaction(internalConnection, TransactionType.LocalFromAPI, this);
			return;
		}
		_internalTransaction = internalTransaction;
		_internalTransaction.InitParent(this);
	}

	public override void Commit()
	{
		Exception ex = null;
		Guid operationId = s_diagnosticListener.WriteTransactionCommitBefore(_isolationLevel, _connection, "Commit");
		ZombieCheck();
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_isFromAPI = true;
			_internalTransaction.Commit();
		}
		catch (Exception ex2)
		{
			ex = ex2;
			throw;
		}
		finally
		{
			if (ex != null)
			{
				s_diagnosticListener.WriteTransactionCommitError(operationId, _isolationLevel, _connection, ex, "Commit");
			}
			else
			{
				s_diagnosticListener.WriteTransactionCommitAfter(operationId, _isolationLevel, _connection, "Commit");
			}
			_isFromAPI = false;
			SqlStatistics.StopTimer(statistics);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !IsZombied && !IsYukonPartialZombie)
		{
			_internalTransaction.Dispose();
		}
		base.Dispose(disposing);
	}

	public override void Rollback()
	{
		Exception ex = null;
		Guid operationId = s_diagnosticListener.WriteTransactionRollbackBefore(_isolationLevel, _connection, null, "Rollback");
		if (IsYukonPartialZombie)
		{
			_internalTransaction = null;
			return;
		}
		ZombieCheck();
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_isFromAPI = true;
			_internalTransaction.Rollback();
		}
		catch (Exception ex2)
		{
			ex = ex2;
			throw;
		}
		finally
		{
			if (ex != null)
			{
				s_diagnosticListener.WriteTransactionRollbackError(operationId, _isolationLevel, _connection, null, ex, "Rollback");
			}
			else
			{
				s_diagnosticListener.WriteTransactionRollbackAfter(operationId, _isolationLevel, _connection, null, "Rollback");
			}
			_isFromAPI = false;
			SqlStatistics.StopTimer(statistics);
		}
	}

	public void Rollback(string transactionName)
	{
		Exception ex = null;
		Guid operationId = s_diagnosticListener.WriteTransactionRollbackBefore(_isolationLevel, _connection, transactionName, "Rollback");
		ZombieCheck();
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_isFromAPI = true;
			_internalTransaction.Rollback(transactionName);
		}
		catch (Exception ex2)
		{
			ex = ex2;
			throw;
		}
		finally
		{
			if (ex != null)
			{
				s_diagnosticListener.WriteTransactionRollbackError(operationId, _isolationLevel, _connection, transactionName, ex, "Rollback");
			}
			else
			{
				s_diagnosticListener.WriteTransactionRollbackAfter(operationId, _isolationLevel, _connection, transactionName, "Rollback");
			}
			_isFromAPI = false;
			SqlStatistics.StopTimer(statistics);
		}
	}

	public void Save(string savePointName)
	{
		ZombieCheck();
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_internalTransaction.Save(savePointName);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	internal void Zombie()
	{
		if (!(_connection.InnerConnection is SqlInternalConnection) || _isFromAPI)
		{
			_internalTransaction = null;
		}
	}

	private void ZombieCheck()
	{
		if (IsZombied)
		{
			if (IsYukonPartialZombie)
			{
				_internalTransaction = null;
			}
			throw ADP.TransactionZombied(this);
		}
	}
}
