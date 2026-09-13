using System.Collections.Generic;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient;

public sealed class SqlCommand : DbCommand, ICloneable, IDbCommand, IDisposable
{
	private enum EXECTYPE
	{
		UNPREPARED,
		PREPAREPENDING,
		PREPARED
	}

	private class CachedAsyncState
	{
		private int _cachedAsyncCloseCount = -1;

		private TaskCompletionSource<object> _cachedAsyncResult;

		private SqlConnection _cachedAsyncConnection;

		private SqlDataReader _cachedAsyncReader;

		private RunBehavior _cachedRunBehavior = RunBehavior.ReturnImmediately;

		private string _cachedSetOptions;

		private string _cachedEndMethod;

		internal SqlDataReader CachedAsyncReader => _cachedAsyncReader;

		internal RunBehavior CachedRunBehavior => _cachedRunBehavior;

		internal string CachedSetOptions => _cachedSetOptions;

		internal bool PendingAsyncOperation => _cachedAsyncResult != null;

		internal string EndMethodName => _cachedEndMethod;

		internal CachedAsyncState()
		{
		}

		internal bool IsActiveConnectionValid(SqlConnection activeConnection)
		{
			if (_cachedAsyncConnection == activeConnection)
			{
				return _cachedAsyncCloseCount == activeConnection.CloseCount;
			}
			return false;
		}

		internal void ResetAsyncState()
		{
			_cachedAsyncCloseCount = -1;
			_cachedAsyncResult = null;
			if (_cachedAsyncConnection != null)
			{
				_cachedAsyncConnection.AsyncCommandInProgress = false;
				_cachedAsyncConnection = null;
			}
			_cachedAsyncReader = null;
			_cachedRunBehavior = RunBehavior.ReturnImmediately;
			_cachedSetOptions = null;
			_cachedEndMethod = null;
		}

		internal void SetActiveConnectionAndResult(TaskCompletionSource<object> completion, string endMethod, SqlConnection activeConnection)
		{
			TdsParser tdsParser = activeConnection?.Parser;
			if (tdsParser == null || tdsParser.State == TdsParserState.Closed || tdsParser.State == TdsParserState.Broken)
			{
				throw ADP.ClosedConnectionError();
			}
			_cachedAsyncCloseCount = activeConnection.CloseCount;
			_cachedAsyncResult = completion;
			if (!tdsParser.MARSOn && activeConnection.AsyncCommandInProgress)
			{
				throw SQL.MARSUnspportedOnConnection();
			}
			_cachedAsyncConnection = activeConnection;
			_cachedAsyncConnection.AsyncCommandInProgress = true;
			_cachedEndMethod = endMethod;
		}

		internal void SetAsyncReaderState(SqlDataReader ds, RunBehavior runBehavior, string optionSettings)
		{
			_cachedAsyncReader = ds;
			_cachedRunBehavior = runBehavior;
			_cachedSetOptions = optionSettings;
		}
	}

	private enum ProcParamsColIndex
	{
		ParameterName,
		ParameterType,
		DataType,
		ManagedDataType,
		CharacterMaximumLength,
		NumericPrecision,
		NumericScale,
		TypeCatalogName,
		TypeSchemaName,
		TypeName,
		XmlSchemaCollectionCatalogName,
		XmlSchemaCollectionSchemaName,
		XmlSchemaCollectionName,
		UdtTypeName,
		DateTimeScale
	}

	private string _commandText;

	private CommandType _commandType;

	private int _commandTimeout = 30;

	private UpdateRowSource _updatedRowSource = UpdateRowSource.Both;

	private bool _designTimeInvisible;

	internal SqlDependency _sqlDep;

	private static readonly DiagnosticListener _diagnosticListener = new DiagnosticListener("SqlClientDiagnosticListener");

	private bool _parentOperationStarted;

	private bool _inPrepare;

	private int _prepareHandle = -1;

	private bool _hiddenPrepare;

	private int _preparedConnectionCloseCount = -1;

	private int _preparedConnectionReconnectCount = -1;

	private SqlParameterCollection _parameters;

	private SqlConnection _activeConnection;

	private bool _dirty;

	private EXECTYPE _execType;

	private _SqlRPC[] _rpcArrayOf1;

	private _SqlMetaDataSet _cachedMetaData;

	private TaskCompletionSource<object> _reconnectionCompletionSource;

	private CachedAsyncState _cachedAsyncState;

	internal int _rowsAffected = -1;

	private SqlNotificationRequest _notification;

	private SqlTransaction _transaction;

	private StatementCompletedEventHandler _statementCompletedEventHandler;

	private TdsParserStateObject _stateObj;

	private volatile bool _pendingCancel;

	private bool _batchRPCMode;

	private List<_SqlRPC> _RPCList;

	private _SqlRPC[] _SqlRPCBatchArray;

	private List<SqlParameterCollection> _parameterCollectionList;

	private int _currentlyExecutingBatch;

	internal static readonly string[] PreKatmaiProcParamsNames = new string[15]
	{
		"PARAMETER_NAME", "PARAMETER_TYPE", "DATA_TYPE", null, "CHARACTER_MAXIMUM_LENGTH", "NUMERIC_PRECISION", "NUMERIC_SCALE", "UDT_CATALOG", "UDT_SCHEMA", "TYPE_NAME",
		"XML_CATALOGNAME", "XML_SCHEMANAME", "XML_SCHEMACOLLECTIONNAME", "UDT_NAME", null
	};

	internal static readonly string[] KatmaiProcParamsNames = new string[15]
	{
		"PARAMETER_NAME", "PARAMETER_TYPE", null, "MANAGED_DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH", "NUMERIC_PRECISION", "NUMERIC_SCALE", "TYPE_CATALOG_NAME", "TYPE_SCHEMA_NAME", "TYPE_NAME",
		"XML_CATALOGNAME", "XML_SCHEMANAME", "XML_SCHEMACOLLECTIONNAME", null, "SS_DATETIME_PRECISION"
	};

	internal bool InPrepare => _inPrepare;

	private CachedAsyncState cachedAsyncState
	{
		get
		{
			if (_cachedAsyncState == null)
			{
				_cachedAsyncState = new CachedAsyncState();
			}
			return _cachedAsyncState;
		}
	}

	public new SqlConnection Connection
	{
		get
		{
			return _activeConnection;
		}
		set
		{
			if (_activeConnection != value && _activeConnection != null && cachedAsyncState.PendingAsyncOperation)
			{
				throw SQL.CannotModifyPropertyAsyncOperationInProgress("Connection");
			}
			if (_transaction != null && _transaction.Connection == null)
			{
				_transaction = null;
			}
			if (IsPrepared && _activeConnection != value && _activeConnection != null)
			{
				try
				{
					Unprepare();
				}
				catch (Exception)
				{
				}
				finally
				{
					_prepareHandle = -1;
					_execType = EXECTYPE.UNPREPARED;
				}
			}
			_activeConnection = value;
		}
	}

	protected override DbConnection DbConnection
	{
		get
		{
			return Connection;
		}
		set
		{
			Connection = (SqlConnection)value;
		}
	}

	private SqlInternalConnectionTds InternalTdsConnection => (SqlInternalConnectionTds)_activeConnection.InnerConnection;

	public SqlNotificationRequest Notification
	{
		get
		{
			return _notification;
		}
		set
		{
			_sqlDep = null;
			_notification = value;
		}
	}

	internal SqlStatistics Statistics
	{
		get
		{
			if (_activeConnection != null && (_activeConnection.StatisticsEnabled || _diagnosticListener.IsEnabled("System.Data.SqlClient.WriteCommandAfter")))
			{
				return _activeConnection.Statistics;
			}
			return null;
		}
	}

	public new SqlTransaction Transaction
	{
		get
		{
			if (_transaction != null && _transaction.Connection == null)
			{
				_transaction = null;
			}
			return _transaction;
		}
		set
		{
			if (_transaction != value && _activeConnection != null && cachedAsyncState.PendingAsyncOperation)
			{
				throw SQL.CannotModifyPropertyAsyncOperationInProgress("Transaction");
			}
			_transaction = value;
		}
	}

	protected override DbTransaction DbTransaction
	{
		get
		{
			return Transaction;
		}
		set
		{
			Transaction = (SqlTransaction)value;
		}
	}

	public override string CommandText
	{
		get
		{
			string commandText = _commandText;
			if (commandText == null)
			{
				return ADP.StrEmpty;
			}
			return commandText;
		}
		set
		{
			if (_commandText != value)
			{
				PropertyChanging();
				_commandText = value;
			}
		}
	}

	public override int CommandTimeout
	{
		get
		{
			return _commandTimeout;
		}
		set
		{
			if (value < 0)
			{
				throw ADP.InvalidCommandTimeout(value, "CommandTimeout");
			}
			if (value != _commandTimeout)
			{
				PropertyChanging();
				_commandTimeout = value;
			}
		}
	}

	public override CommandType CommandType
	{
		get
		{
			CommandType commandType = _commandType;
			if (commandType == (CommandType)0)
			{
				return CommandType.Text;
			}
			return commandType;
		}
		set
		{
			if (_commandType != value)
			{
				switch (value)
				{
				case CommandType.Text:
				case CommandType.StoredProcedure:
					PropertyChanging();
					_commandType = value;
					break;
				case CommandType.TableDirect:
					throw SQL.NotSupportedCommandType(value);
				default:
					throw ADP.InvalidCommandType(value);
				}
			}
		}
	}

	public override bool DesignTimeVisible
	{
		get
		{
			return !_designTimeInvisible;
		}
		set
		{
			_designTimeInvisible = !value;
		}
	}

	public new SqlParameterCollection Parameters
	{
		get
		{
			if (_parameters == null)
			{
				_parameters = new SqlParameterCollection();
			}
			return _parameters;
		}
	}

	protected override DbParameterCollection DbParameterCollection => Parameters;

	public override UpdateRowSource UpdatedRowSource
	{
		get
		{
			return _updatedRowSource;
		}
		set
		{
			if ((uint)value <= 3u)
			{
				_updatedRowSource = value;
				return;
			}
			throw ADP.InvalidUpdateRowSource(value);
		}
	}

	internal _SqlMetaDataSet MetaData => _cachedMetaData;

	internal TdsParserStateObject StateObject => _stateObj;

	private bool IsPrepared => _execType != EXECTYPE.UNPREPARED;

	private bool IsUserPrepared
	{
		get
		{
			if (IsPrepared && !_hiddenPrepare)
			{
				return !IsDirty;
			}
			return false;
		}
	}

	internal bool IsDirty
	{
		get
		{
			SqlConnection activeConnection = _activeConnection;
			if (IsPrepared)
			{
				if (!_dirty && (_parameters == null || !_parameters.IsDirty))
				{
					if (activeConnection != null)
					{
						if (activeConnection.CloseCount == _preparedConnectionCloseCount)
						{
							return activeConnection.ReconnectCount != _preparedConnectionReconnectCount;
						}
						return true;
					}
					return false;
				}
				return true;
			}
			return false;
		}
		set
		{
			_dirty = value && IsPrepared;
			if (_parameters != null)
			{
				_parameters.IsDirty = _dirty;
			}
			_cachedMetaData = null;
		}
	}

	internal int InternalRecordsAffected
	{
		get
		{
			return _rowsAffected;
		}
		set
		{
			if (-1 == _rowsAffected)
			{
				_rowsAffected = value;
			}
			else if (0 < value)
			{
				_rowsAffected += value;
			}
		}
	}

	internal bool BatchRPCMode
	{
		get
		{
			return _batchRPCMode;
		}
		set
		{
			_batchRPCMode = value;
			if (!_batchRPCMode)
			{
				ClearBatchCommand();
				return;
			}
			if (_RPCList == null)
			{
				_RPCList = new List<_SqlRPC>();
			}
			if (_parameterCollectionList == null)
			{
				_parameterCollectionList = new List<SqlParameterCollection>();
			}
		}
	}

	[System.MonoTODO]
	public bool NotificationAutoEnlist
	{
		get
		{
			return Notification != null;
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public event StatementCompletedEventHandler StatementCompleted
	{
		add
		{
			_statementCompletedEventHandler = (StatementCompletedEventHandler)Delegate.Combine(_statementCompletedEventHandler, value);
		}
		remove
		{
			_statementCompletedEventHandler = (StatementCompletedEventHandler)Delegate.Remove(_statementCompletedEventHandler, value);
		}
	}

	public SqlCommand()
	{
		GC.SuppressFinalize(this);
	}

	public SqlCommand(string cmdText)
		: this()
	{
		CommandText = cmdText;
	}

	public SqlCommand(string cmdText, SqlConnection connection)
		: this()
	{
		CommandText = cmdText;
		Connection = connection;
	}

	public SqlCommand(string cmdText, SqlConnection connection, SqlTransaction transaction)
		: this()
	{
		CommandText = cmdText;
		Connection = connection;
		Transaction = transaction;
	}

	private SqlCommand(SqlCommand from)
		: this()
	{
		CommandText = from.CommandText;
		CommandTimeout = from.CommandTimeout;
		CommandType = from.CommandType;
		Connection = from.Connection;
		DesignTimeVisible = from.DesignTimeVisible;
		Transaction = from.Transaction;
		UpdatedRowSource = from.UpdatedRowSource;
		SqlParameterCollection parameters = Parameters;
		foreach (object parameter in from.Parameters)
		{
			parameters.Add((parameter is ICloneable) ? (parameter as ICloneable).Clone() : parameter);
		}
	}

	public void ResetCommandTimeout()
	{
		if (30 != _commandTimeout)
		{
			PropertyChanging();
			_commandTimeout = 30;
		}
	}

	internal void OnStatementCompleted(int recordCount)
	{
		if (0 > recordCount)
		{
			return;
		}
		StatementCompletedEventHandler statementCompletedEventHandler = _statementCompletedEventHandler;
		if (statementCompletedEventHandler == null)
		{
			return;
		}
		try
		{
			statementCompletedEventHandler(this, new StatementCompletedEventArgs(recordCount));
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableOrSecurityExceptionType(e))
			{
				throw;
			}
		}
	}

	private void PropertyChanging()
	{
		IsDirty = true;
	}

	public override void Prepare()
	{
		_pendingCancel = false;
		SqlStatistics sqlStatistics = null;
		sqlStatistics = SqlStatistics.StartTimer(Statistics);
		if ((IsPrepared && !IsDirty) || CommandType == CommandType.StoredProcedure || (CommandType.Text == CommandType && GetParameterCount(_parameters) == 0))
		{
			if (Statistics != null)
			{
				Statistics.SafeIncrement(ref Statistics._prepares);
			}
			_hiddenPrepare = false;
		}
		else
		{
			ValidateCommand(async: false, "Prepare");
			bool flag = true;
			try
			{
				GetStateObject();
				if (_parameters != null)
				{
					int count = _parameters.Count;
					for (int i = 0; i < count; i++)
					{
						_parameters[i].Prepare(this);
					}
				}
				InternalPrepare();
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
					_hiddenPrepare = false;
					ReliablePutStateObject();
				}
			}
		}
		SqlStatistics.StopTimer(sqlStatistics);
	}

	private void InternalPrepare()
	{
		if (IsDirty)
		{
			Unprepare();
			IsDirty = false;
		}
		_execType = EXECTYPE.PREPAREPENDING;
		_preparedConnectionCloseCount = _activeConnection.CloseCount;
		_preparedConnectionReconnectCount = _activeConnection.ReconnectCount;
		if (Statistics != null)
		{
			Statistics.SafeIncrement(ref Statistics._prepares);
		}
	}

	internal void Unprepare()
	{
		_execType = EXECTYPE.PREPAREPENDING;
		if (_activeConnection.CloseCount != _preparedConnectionCloseCount || _activeConnection.ReconnectCount != _preparedConnectionReconnectCount)
		{
			_prepareHandle = -1;
		}
		_cachedMetaData = null;
	}

	public override void Cancel()
	{
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			TaskCompletionSource<object> reconnectionCompletionSource = _reconnectionCompletionSource;
			if ((reconnectionCompletionSource != null && reconnectionCompletionSource.TrySetCanceled()) || _activeConnection == null || !(_activeConnection.InnerConnection is SqlInternalConnectionTds sqlInternalConnectionTds))
			{
				return;
			}
			lock (sqlInternalConnectionTds)
			{
				if (sqlInternalConnectionTds != _activeConnection.InnerConnection as SqlInternalConnectionTds)
				{
					return;
				}
				TdsParser parser = sqlInternalConnectionTds.Parser;
				if (parser != null && !_pendingCancel)
				{
					_pendingCancel = true;
					TdsParserStateObject stateObj = _stateObj;
					if (stateObj != null)
					{
						stateObj.Cancel(this);
					}
					else
					{
						sqlInternalConnectionTds.FindLiveReader(this)?.Cancel(this);
					}
				}
			}
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public new SqlParameter CreateParameter()
	{
		return new SqlParameter();
	}

	protected override DbParameter CreateDbParameter()
	{
		return CreateParameter();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_cachedMetaData = null;
		}
		base.Dispose(disposing);
	}

	public override object ExecuteScalar()
	{
		_pendingCancel = false;
		Guid operationId = _diagnosticListener.WriteCommandBefore(this, "ExecuteScalar");
		SqlStatistics statistics = null;
		Exception ex = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			SqlDataReader ds = RunExecuteReader(CommandBehavior.Default, RunBehavior.ReturnImmediately, returnStream: true, "ExecuteScalar");
			return CompleteExecuteScalar(ds, returnSqlValue: false);
		}
		catch (Exception ex2)
		{
			ex = ex2;
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			if (ex != null)
			{
				_diagnosticListener.WriteCommandError(operationId, this, ex, "ExecuteScalar");
			}
			else
			{
				_diagnosticListener.WriteCommandAfter(operationId, this, "ExecuteScalar");
			}
		}
	}

	private object CompleteExecuteScalar(SqlDataReader ds, bool returnSqlValue)
	{
		object result = null;
		try
		{
			if (ds.Read() && ds.FieldCount > 0)
			{
				result = ((!returnSqlValue) ? ds.GetValue(0) : ds.GetSqlValue(0));
			}
		}
		finally
		{
			ds.Close();
		}
		return result;
	}

	public override int ExecuteNonQuery()
	{
		_pendingCancel = false;
		Guid operationId = _diagnosticListener.WriteCommandBefore(this, "ExecuteNonQuery");
		SqlStatistics statistics = null;
		Exception ex = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			InternalExecuteNonQuery(null, sendToPipe: false, CommandTimeout, asyncWrite: false, "ExecuteNonQuery");
			return _rowsAffected;
		}
		catch (Exception ex2)
		{
			ex = ex2;
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			if (ex != null)
			{
				_diagnosticListener.WriteCommandError(operationId, this, ex, "ExecuteNonQuery");
			}
			else
			{
				_diagnosticListener.WriteCommandAfter(operationId, this, "ExecuteNonQuery");
			}
		}
	}

	public IAsyncResult BeginExecuteNonQuery()
	{
		return BeginExecuteNonQuery(null, null);
	}

	public IAsyncResult BeginExecuteNonQuery(AsyncCallback callback, object stateObject)
	{
		_pendingCancel = false;
		ValidateAsyncCommand();
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			TaskCompletionSource<object> completion = new TaskCompletionSource<object>(stateObject);
			try
			{
				Task task = InternalExecuteNonQuery(completion, sendToPipe: false, CommandTimeout, asyncWrite: true, "BeginExecuteNonQuery");
				cachedAsyncState.SetActiveConnectionAndResult(completion, "EndExecuteNonQuery", _activeConnection);
				if (task != null)
				{
					AsyncHelper.ContinueTask(task, completion, delegate
					{
						BeginExecuteNonQueryInternalReadStage(completion);
					});
				}
				else
				{
					BeginExecuteNonQueryInternalReadStage(completion);
				}
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableOrSecurityExceptionType(e))
				{
					throw;
				}
				ReliablePutStateObject();
				throw;
			}
			if (callback != null)
			{
				completion.Task.ContinueWith(delegate(Task<object> t)
				{
					callback(t);
				}, TaskScheduler.Default);
			}
			return completion.Task;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	private void BeginExecuteNonQueryInternalReadStage(TaskCompletionSource<object> completion)
	{
		try
		{
			_stateObj.ReadSni(completion);
		}
		catch (Exception)
		{
			if (_cachedAsyncState != null)
			{
				_cachedAsyncState.ResetAsyncState();
			}
			ReliablePutStateObject();
			throw;
		}
	}

	private void VerifyEndExecuteState(Task completionTask, string endMethod)
	{
		if (completionTask.IsCanceled)
		{
			if (_stateObj == null)
			{
				throw SQL.CR_ReconnectionCancelled();
			}
			_stateObj.Parser.State = TdsParserState.Broken;
			_stateObj.Parser.Connection.BreakConnection();
			_stateObj.Parser.ThrowExceptionAndWarning(_stateObj);
		}
		else if (completionTask.IsFaulted)
		{
			throw completionTask.Exception.InnerException;
		}
		if (cachedAsyncState.EndMethodName == null)
		{
			throw ADP.MethodCalledTwice(endMethod);
		}
		if (endMethod != cachedAsyncState.EndMethodName)
		{
			throw ADP.MismatchedAsyncResult(cachedAsyncState.EndMethodName, endMethod);
		}
		if (_activeConnection.State != ConnectionState.Open || !cachedAsyncState.IsActiveConnectionValid(_activeConnection))
		{
			throw ADP.ClosedConnectionError();
		}
	}

	private void WaitForAsyncResults(IAsyncResult asyncResult)
	{
		_ = (Task)asyncResult;
		if (!asyncResult.IsCompleted)
		{
			asyncResult.AsyncWaitHandle.WaitOne();
		}
		_stateObj._networkPacketTaskSource = null;
		_activeConnection.GetOpenTdsConnection().DecrementAsyncCount();
	}

	private void ThrowIfReconnectionHasBeenCanceled()
	{
		if (_stateObj == null)
		{
			TaskCompletionSource<object> reconnectionCompletionSource = _reconnectionCompletionSource;
			if (reconnectionCompletionSource != null && reconnectionCompletionSource.Task.IsCanceled)
			{
				throw SQL.CR_ReconnectionCancelled();
			}
		}
	}

	public int EndExecuteNonQuery(IAsyncResult asyncResult)
	{
		Exception exception = ((Task)asyncResult).Exception;
		if (exception != null)
		{
			if (cachedAsyncState != null)
			{
				cachedAsyncState.ResetAsyncState();
			}
			ReliablePutStateObject();
			throw exception.InnerException;
		}
		ThrowIfReconnectionHasBeenCanceled();
		lock (_stateObj)
		{
			return EndExecuteNonQueryInternal(asyncResult);
		}
	}

	private int EndExecuteNonQueryInternal(IAsyncResult asyncResult)
	{
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			VerifyEndExecuteState((Task)asyncResult, "EndExecuteNonQuery");
			WaitForAsyncResults(asyncResult);
			bool flag = true;
			try
			{
				CheckThrowSNIException();
				if (CommandType.Text == CommandType && GetParameterCount(_parameters) == 0)
				{
					try
					{
						if (!_stateObj.Parser.TryRun(RunBehavior.UntilDone, this, null, null, _stateObj, out var _))
						{
							throw SQL.SynchronousCallMayNotPend();
						}
					}
					finally
					{
						cachedAsyncState.ResetAsyncState();
					}
				}
				else
				{
					CompleteAsyncExecuteReader()?.Close();
				}
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
					PutStateObject();
				}
			}
			return _rowsAffected;
		}
		catch (Exception e2)
		{
			if (cachedAsyncState != null)
			{
				cachedAsyncState.ResetAsyncState();
			}
			if (ADP.IsCatchableExceptionType(e2))
			{
				ReliablePutStateObject();
			}
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	private Task InternalExecuteNonQuery(TaskCompletionSource<object> completion, bool sendToPipe, int timeout, bool asyncWrite = false, [CallerMemberName] string methodName = "")
	{
		bool async = completion != null;
		SqlStatistics statistics = Statistics;
		_rowsAffected = -1;
		ValidateCommand(async, methodName);
		CheckNotificationStateAndAutoEnlist();
		Task task = null;
		if (!BatchRPCMode && CommandType.Text == CommandType && GetParameterCount(_parameters) == 0)
		{
			if (statistics != null)
			{
				if (!IsDirty && IsPrepared)
				{
					statistics.SafeIncrement(ref statistics._preparedExecs);
				}
				else
				{
					statistics.SafeIncrement(ref statistics._unpreparedExecs);
				}
			}
			task = RunExecuteNonQueryTds(methodName, async, timeout, asyncWrite);
		}
		else
		{
			SqlDataReader reader = RunExecuteReader(CommandBehavior.Default, RunBehavior.UntilDone, returnStream: false, completion, timeout, out task, asyncWrite, methodName);
			if (reader != null)
			{
				if (task != null)
				{
					task = AsyncHelper.CreateContinuationTask(task, delegate
					{
						reader.Close();
					});
				}
				else
				{
					reader.Close();
				}
			}
		}
		return task;
	}

	public XmlReader ExecuteXmlReader()
	{
		_pendingCancel = false;
		Guid operationId = _diagnosticListener.WriteCommandBefore(this, "ExecuteXmlReader");
		SqlStatistics statistics = null;
		Exception ex = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			SqlDataReader ds = RunExecuteReader(CommandBehavior.SequentialAccess, RunBehavior.ReturnImmediately, returnStream: true, "ExecuteXmlReader");
			return CompleteXmlReader(ds);
		}
		catch (Exception ex2)
		{
			ex = ex2;
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			if (ex != null)
			{
				_diagnosticListener.WriteCommandError(operationId, this, ex, "ExecuteXmlReader");
			}
			else
			{
				_diagnosticListener.WriteCommandAfter(operationId, this, "ExecuteXmlReader");
			}
		}
	}

	public IAsyncResult BeginExecuteXmlReader()
	{
		return BeginExecuteXmlReader(null, null);
	}

	public IAsyncResult BeginExecuteXmlReader(AsyncCallback callback, object stateObject)
	{
		_pendingCancel = false;
		ValidateAsyncCommand();
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			TaskCompletionSource<object> completion = new TaskCompletionSource<object>(stateObject);
			Task task;
			try
			{
				RunExecuteReader(CommandBehavior.SequentialAccess, RunBehavior.ReturnImmediately, returnStream: true, completion, CommandTimeout, out task, asyncWrite: true, "BeginExecuteXmlReader");
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableOrSecurityExceptionType(e))
				{
					throw;
				}
				ReliablePutStateObject();
				throw;
			}
			cachedAsyncState.SetActiveConnectionAndResult(completion, "EndExecuteXmlReader", _activeConnection);
			if (task != null)
			{
				AsyncHelper.ContinueTask(task, completion, delegate
				{
					BeginExecuteXmlReaderInternalReadStage(completion);
				});
			}
			else
			{
				BeginExecuteXmlReaderInternalReadStage(completion);
			}
			if (callback != null)
			{
				completion.Task.ContinueWith(delegate(Task<object> t)
				{
					callback(t);
				}, TaskScheduler.Default);
			}
			return completion.Task;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	private void BeginExecuteXmlReaderInternalReadStage(TaskCompletionSource<object> completion)
	{
		try
		{
			_stateObj.ReadSni(completion);
		}
		catch (Exception exception)
		{
			if (_cachedAsyncState != null)
			{
				_cachedAsyncState.ResetAsyncState();
			}
			ReliablePutStateObject();
			completion.TrySetException(exception);
		}
	}

	public XmlReader EndExecuteXmlReader(IAsyncResult asyncResult)
	{
		Exception exception = ((Task)asyncResult).Exception;
		if (exception != null)
		{
			if (cachedAsyncState != null)
			{
				cachedAsyncState.ResetAsyncState();
			}
			ReliablePutStateObject();
			throw exception.InnerException;
		}
		ThrowIfReconnectionHasBeenCanceled();
		lock (_stateObj)
		{
			return EndExecuteXmlReaderInternal(asyncResult);
		}
	}

	private XmlReader EndExecuteXmlReaderInternal(IAsyncResult asyncResult)
	{
		try
		{
			return CompleteXmlReader(InternalEndExecuteReader(asyncResult, "EndExecuteXmlReader"), async: true);
		}
		catch (Exception e)
		{
			if (cachedAsyncState != null)
			{
				cachedAsyncState.ResetAsyncState();
			}
			if (ADP.IsCatchableExceptionType(e))
			{
				ReliablePutStateObject();
			}
			throw;
		}
	}

	private XmlReader CompleteXmlReader(SqlDataReader ds, bool async = false)
	{
		XmlReader xmlReader = null;
		SmiExtendedMetaData[] internalSmiMetaData = ds.GetInternalSmiMetaData();
		if (internalSmiMetaData != null && internalSmiMetaData.Length == 1 && (internalSmiMetaData[0].SqlDbType == SqlDbType.NText || internalSmiMetaData[0].SqlDbType == SqlDbType.NVarChar || internalSmiMetaData[0].SqlDbType == SqlDbType.Xml))
		{
			try
			{
				xmlReader = new SqlStream(ds, addByteOrderMark: true, internalSmiMetaData[0].SqlDbType != SqlDbType.Xml).ToXmlReader(async);
			}
			catch (Exception e)
			{
				if (ADP.IsCatchableExceptionType(e))
				{
					ds.Close();
				}
				throw;
			}
		}
		if (xmlReader == null)
		{
			ds.Close();
			throw SQL.NonXmlResult();
		}
		return xmlReader;
	}

	protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
	{
		return ExecuteReader(behavior);
	}

	public new SqlDataReader ExecuteReader()
	{
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			return ExecuteReader(CommandBehavior.Default);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public new SqlDataReader ExecuteReader(CommandBehavior behavior)
	{
		_pendingCancel = false;
		Guid operationId = _diagnosticListener.WriteCommandBefore(this, "ExecuteReader");
		SqlStatistics statistics = null;
		Exception ex = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			return RunExecuteReader(behavior, RunBehavior.ReturnImmediately, returnStream: true, "ExecuteReader");
		}
		catch (Exception ex2)
		{
			ex = ex2;
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			if (ex != null)
			{
				_diagnosticListener.WriteCommandError(operationId, this, ex, "ExecuteReader");
			}
			else
			{
				_diagnosticListener.WriteCommandAfter(operationId, this, "ExecuteReader");
			}
		}
	}

	public SqlDataReader EndExecuteReader(IAsyncResult asyncResult)
	{
		Exception exception = ((Task)asyncResult).Exception;
		if (exception != null)
		{
			if (cachedAsyncState != null)
			{
				cachedAsyncState.ResetAsyncState();
			}
			ReliablePutStateObject();
			throw exception.InnerException;
		}
		ThrowIfReconnectionHasBeenCanceled();
		lock (_stateObj)
		{
			return EndExecuteReaderInternal(asyncResult);
		}
	}

	private SqlDataReader EndExecuteReaderInternal(IAsyncResult asyncResult)
	{
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			return InternalEndExecuteReader(asyncResult, "EndExecuteReader");
		}
		catch (Exception e)
		{
			if (cachedAsyncState != null)
			{
				cachedAsyncState.ResetAsyncState();
			}
			if (ADP.IsCatchableExceptionType(e))
			{
				ReliablePutStateObject();
			}
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	internal IAsyncResult BeginExecuteReader(CommandBehavior behavior, AsyncCallback callback, object stateObject)
	{
		_pendingCancel = false;
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			TaskCompletionSource<object> completion = new TaskCompletionSource<object>(stateObject);
			ValidateAsyncCommand();
			Task task = null;
			try
			{
				RunExecuteReader(behavior, RunBehavior.ReturnImmediately, returnStream: true, completion, CommandTimeout, out task, asyncWrite: true, "BeginExecuteReader");
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableOrSecurityExceptionType(e))
				{
					throw;
				}
				ReliablePutStateObject();
				throw;
			}
			cachedAsyncState.SetActiveConnectionAndResult(completion, "EndExecuteReader", _activeConnection);
			if (task != null)
			{
				AsyncHelper.ContinueTask(task, completion, delegate
				{
					BeginExecuteReaderInternalReadStage(completion);
				});
			}
			else
			{
				BeginExecuteReaderInternalReadStage(completion);
			}
			if (callback != null)
			{
				completion.Task.ContinueWith(delegate(Task<object> t)
				{
					callback(t);
				}, TaskScheduler.Default);
			}
			return completion.Task;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	private void BeginExecuteReaderInternalReadStage(TaskCompletionSource<object> completion)
	{
		try
		{
			_stateObj.ReadSni(completion);
		}
		catch (Exception exception)
		{
			if (_cachedAsyncState != null)
			{
				_cachedAsyncState.ResetAsyncState();
			}
			ReliablePutStateObject();
			completion.TrySetException(exception);
		}
	}

	private SqlDataReader InternalEndExecuteReader(IAsyncResult asyncResult, string endMethod)
	{
		VerifyEndExecuteState((Task)asyncResult, endMethod);
		WaitForAsyncResults(asyncResult);
		CheckThrowSNIException();
		return CompleteAsyncExecuteReader();
	}

	public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
	{
		Guid operationId = _diagnosticListener.WriteCommandBefore(this, "ExecuteNonQueryAsync");
		TaskCompletionSource<int> source = new TaskCompletionSource<int>();
		CancellationTokenRegistration registration = default(CancellationTokenRegistration);
		if (cancellationToken.CanBeCanceled)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				source.SetCanceled();
				return source.Task;
			}
			registration = cancellationToken.Register(delegate(object s)
			{
				((SqlCommand)s).CancelIgnoreFailure();
			}, this);
		}
		Task<int> outerTask = source.Task;
		try
		{
			RegisterForConnectionCloseNotification(ref outerTask);
			Task<int>.Factory.FromAsync(BeginExecuteNonQuery, EndExecuteNonQuery, null).ContinueWith(delegate(Task<int> t)
			{
				registration.Dispose();
				if (t.IsFaulted)
				{
					Exception innerException = t.Exception.InnerException;
					_diagnosticListener.WriteCommandError(operationId, this, innerException, "ExecuteNonQueryAsync");
					source.SetException(innerException);
				}
				else
				{
					if (t.IsCanceled)
					{
						source.SetCanceled();
					}
					else
					{
						source.SetResult(t.Result);
					}
					_diagnosticListener.WriteCommandAfter(operationId, this, "ExecuteNonQueryAsync");
				}
			}, TaskScheduler.Default);
		}
		catch (Exception ex)
		{
			_diagnosticListener.WriteCommandError(operationId, this, ex, "ExecuteNonQueryAsync");
			source.SetException(ex);
		}
		return outerTask;
	}

	protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
	{
		return ExecuteReaderAsync(behavior, cancellationToken).ContinueWith((Func<Task<SqlDataReader>, DbDataReader>)delegate(Task<SqlDataReader> result)
		{
			if (result.IsFaulted)
			{
				throw result.Exception.InnerException;
			}
			return result.Result;
		}, CancellationToken.None, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
	}

	public new Task<SqlDataReader> ExecuteReaderAsync()
	{
		return ExecuteReaderAsync(CommandBehavior.Default, CancellationToken.None);
	}

	public new Task<SqlDataReader> ExecuteReaderAsync(CommandBehavior behavior)
	{
		return ExecuteReaderAsync(behavior, CancellationToken.None);
	}

	public new Task<SqlDataReader> ExecuteReaderAsync(CancellationToken cancellationToken)
	{
		return ExecuteReaderAsync(CommandBehavior.Default, cancellationToken);
	}

	public new Task<SqlDataReader> ExecuteReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
	{
		Guid operationId = default(Guid);
		if (!_parentOperationStarted)
		{
			operationId = _diagnosticListener.WriteCommandBefore(this, "ExecuteReaderAsync");
		}
		TaskCompletionSource<SqlDataReader> source = new TaskCompletionSource<SqlDataReader>();
		CancellationTokenRegistration registration = default(CancellationTokenRegistration);
		if (cancellationToken.CanBeCanceled)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				source.SetCanceled();
				return source.Task;
			}
			registration = cancellationToken.Register(delegate(object s)
			{
				((SqlCommand)s).CancelIgnoreFailure();
			}, this);
		}
		Task<SqlDataReader> outerTask = source.Task;
		try
		{
			RegisterForConnectionCloseNotification(ref outerTask);
			Task<SqlDataReader>.Factory.FromAsync(BeginExecuteReader, EndExecuteReader, behavior, null).ContinueWith(delegate(Task<SqlDataReader> t)
			{
				registration.Dispose();
				if (t.IsFaulted)
				{
					Exception innerException = t.Exception.InnerException;
					if (!_parentOperationStarted)
					{
						_diagnosticListener.WriteCommandError(operationId, this, innerException, "ExecuteReaderAsync");
					}
					source.SetException(innerException);
				}
				else
				{
					if (t.IsCanceled)
					{
						source.SetCanceled();
					}
					else
					{
						source.SetResult(t.Result);
					}
					if (!_parentOperationStarted)
					{
						_diagnosticListener.WriteCommandAfter(operationId, this, "ExecuteReaderAsync");
					}
				}
			}, TaskScheduler.Default);
		}
		catch (Exception ex)
		{
			if (!_parentOperationStarted)
			{
				_diagnosticListener.WriteCommandError(operationId, this, ex, "ExecuteReaderAsync");
			}
			source.SetException(ex);
		}
		return outerTask;
	}

	public override Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
	{
		_parentOperationStarted = true;
		Guid operationId = _diagnosticListener.WriteCommandBefore(this, "ExecuteScalarAsync");
		return ExecuteReaderAsync(cancellationToken).ContinueWith(delegate(Task<SqlDataReader> executeTask)
		{
			TaskCompletionSource<object> source = new TaskCompletionSource<object>();
			if (executeTask.IsCanceled)
			{
				source.SetCanceled();
			}
			else if (executeTask.IsFaulted)
			{
				_diagnosticListener.WriteCommandError(operationId, this, executeTask.Exception.InnerException, "ExecuteScalarAsync");
				source.SetException(executeTask.Exception.InnerException);
			}
			else
			{
				SqlDataReader reader = executeTask.Result;
				reader.ReadAsync(cancellationToken).ContinueWith(delegate(Task<bool> readTask)
				{
					try
					{
						if (readTask.IsCanceled)
						{
							reader.Dispose();
							source.SetCanceled();
						}
						else if (readTask.IsFaulted)
						{
							reader.Dispose();
							_diagnosticListener.WriteCommandError(operationId, this, readTask.Exception.InnerException, "ExecuteScalarAsync");
							source.SetException(readTask.Exception.InnerException);
						}
						else
						{
							Exception ex = null;
							object result = null;
							try
							{
								if (readTask.Result && reader.FieldCount > 0)
								{
									try
									{
										result = reader.GetValue(0);
									}
									catch (Exception ex2)
									{
										ex = ex2;
									}
								}
							}
							finally
							{
								reader.Dispose();
							}
							if (ex != null)
							{
								_diagnosticListener.WriteCommandError(operationId, this, ex, "ExecuteScalarAsync");
								source.SetException(ex);
							}
							else
							{
								_diagnosticListener.WriteCommandAfter(operationId, this, "ExecuteScalarAsync");
								source.SetResult(result);
							}
						}
					}
					catch (Exception exception)
					{
						source.SetException(exception);
					}
				}, TaskScheduler.Default);
			}
			_parentOperationStarted = false;
			return source.Task;
		}, TaskScheduler.Default).Unwrap();
	}

	public Task<XmlReader> ExecuteXmlReaderAsync()
	{
		return ExecuteXmlReaderAsync(CancellationToken.None);
	}

	public Task<XmlReader> ExecuteXmlReaderAsync(CancellationToken cancellationToken)
	{
		Guid operationId = _diagnosticListener.WriteCommandBefore(this, "ExecuteXmlReaderAsync");
		TaskCompletionSource<XmlReader> source = new TaskCompletionSource<XmlReader>();
		CancellationTokenRegistration registration = default(CancellationTokenRegistration);
		if (cancellationToken.CanBeCanceled)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				source.SetCanceled();
				return source.Task;
			}
			registration = cancellationToken.Register(delegate(object s)
			{
				((SqlCommand)s).CancelIgnoreFailure();
			}, this);
		}
		Task<XmlReader> outerTask = source.Task;
		try
		{
			RegisterForConnectionCloseNotification(ref outerTask);
			Task<XmlReader>.Factory.FromAsync(BeginExecuteXmlReader, EndExecuteXmlReader, null).ContinueWith(delegate(Task<XmlReader> t)
			{
				registration.Dispose();
				if (t.IsFaulted)
				{
					Exception innerException = t.Exception.InnerException;
					_diagnosticListener.WriteCommandError(operationId, this, innerException, "ExecuteXmlReaderAsync");
					source.SetException(innerException);
				}
				else
				{
					if (t.IsCanceled)
					{
						source.SetCanceled();
					}
					else
					{
						source.SetResult(t.Result);
					}
					_diagnosticListener.WriteCommandAfter(operationId, this, "ExecuteXmlReaderAsync");
				}
			}, TaskScheduler.Default);
		}
		catch (Exception ex)
		{
			_diagnosticListener.WriteCommandError(operationId, this, ex, "ExecuteXmlReaderAsync");
			source.SetException(ex);
		}
		return outerTask;
	}

	private static string UnquoteProcedurePart(string part)
	{
		if (part != null && 2 <= part.Length && '[' == part[0] && ']' == part[part.Length - 1])
		{
			part = part.Substring(1, part.Length - 2);
			part = part.Replace("]]", "]");
		}
		return part;
	}

	private static string UnquoteProcedureName(string name, out object groupNumber)
	{
		groupNumber = null;
		string text = name;
		if (text != null)
		{
			if (char.IsDigit(text[text.Length - 1]))
			{
				int num = text.LastIndexOf(';');
				if (num != -1)
				{
					string s = text.Substring(num + 1);
					int result = 0;
					if (int.TryParse(s, out result))
					{
						groupNumber = result;
						text = text.Substring(0, num);
					}
				}
			}
			text = UnquoteProcedurePart(text);
		}
		return text;
	}

	internal void DeriveParameters()
	{
		switch (CommandType)
		{
		case CommandType.Text:
			throw ADP.DeriveParametersNotSupported(this);
		case CommandType.TableDirect:
			throw ADP.DeriveParametersNotSupported(this);
		default:
			throw ADP.InvalidCommandType(CommandType);
		case CommandType.StoredProcedure:
		{
			ValidateCommand(async: false, "DeriveParameters");
			string[] array = MultipartIdentifier.ParseMultipartIdentifier(CommandText, "[\"", "]\"", "SqlCommand.DeriveParameters failed because the SqlCommand.CommandText property value is an invalid multipart name", ThrowOnEmptyMultipartName: false);
			if (array[3] == null || string.IsNullOrEmpty(array[3]))
			{
				throw ADP.NoStoredProcedureExists(CommandText);
			}
			SqlCommand sqlCommand = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(array[0]))
			{
				SqlCommandSet.BuildStoredProcedureName(stringBuilder, array[0]);
				stringBuilder.Append(".");
			}
			if (string.IsNullOrEmpty(array[1]))
			{
				array[1] = Connection.Database;
			}
			SqlCommandSet.BuildStoredProcedureName(stringBuilder, array[1]);
			stringBuilder.Append(".");
			string[] array2;
			bool flag;
			if (Connection.IsKatmaiOrNewer)
			{
				stringBuilder.Append("[sys].[").Append("sp_procedure_params_100_managed").Append("]");
				array2 = KatmaiProcParamsNames;
				flag = true;
			}
			else
			{
				stringBuilder.Append("[sys].[").Append("sp_procedure_params_managed").Append("]");
				array2 = PreKatmaiProcParamsNames;
				flag = false;
			}
			sqlCommand = new SqlCommand(stringBuilder.ToString(), Connection, Transaction)
			{
				CommandType = CommandType.StoredProcedure
			};
			sqlCommand.Parameters.Add(new SqlParameter("@procedure_name", SqlDbType.NVarChar, 255));
			sqlCommand.Parameters[0].Value = UnquoteProcedureName(array[3], out var groupNumber);
			if (groupNumber != null)
			{
				sqlCommand.Parameters.Add(new SqlParameter("@group_number", SqlDbType.Int)).Value = groupNumber;
			}
			if (!string.IsNullOrEmpty(array[2]))
			{
				sqlCommand.Parameters.Add(new SqlParameter("@procedure_schema", SqlDbType.NVarChar, 255)).Value = UnquoteProcedurePart(array[2]);
			}
			SqlDataReader sqlDataReader = null;
			List<SqlParameter> list = new List<SqlParameter>();
			bool flag2 = true;
			try
			{
				sqlDataReader = sqlCommand.ExecuteReader();
				SqlParameter sqlParameter = null;
				while (sqlDataReader.Read())
				{
					sqlParameter = new SqlParameter
					{
						ParameterName = (string)sqlDataReader[array2[0]]
					};
					if (flag)
					{
						sqlParameter.SqlDbType = (SqlDbType)(short)sqlDataReader[array2[3]];
						switch (sqlParameter.SqlDbType)
						{
						case SqlDbType.Image:
						case SqlDbType.Timestamp:
							sqlParameter.SqlDbType = SqlDbType.VarBinary;
							break;
						case SqlDbType.NText:
							sqlParameter.SqlDbType = SqlDbType.NVarChar;
							break;
						case SqlDbType.Text:
							sqlParameter.SqlDbType = SqlDbType.VarChar;
							break;
						}
					}
					else
					{
						sqlParameter.SqlDbType = MetaType.GetSqlDbTypeFromOleDbType((short)sqlDataReader[array2[2]], ADP.IsNull(sqlDataReader[array2[9]]) ? ADP.StrEmpty : ((string)sqlDataReader[array2[9]]));
					}
					if (sqlDataReader[array2[4]] is int num)
					{
						if (num == 0 && (sqlParameter.SqlDbType == SqlDbType.NVarChar || sqlParameter.SqlDbType == SqlDbType.VarBinary || sqlParameter.SqlDbType == SqlDbType.VarChar))
						{
							num = -1;
						}
						sqlParameter.Size = num;
					}
					sqlParameter.Direction = ParameterDirectionFromOleDbDirection((short)sqlDataReader[array2[1]]);
					if (sqlParameter.SqlDbType == SqlDbType.Decimal)
					{
						sqlParameter.ScaleInternal = (byte)((short)sqlDataReader[array2[6]] & 0xFF);
						sqlParameter.PrecisionInternal = (byte)((short)sqlDataReader[array2[5]] & 0xFF);
					}
					if (SqlDbType.Udt == sqlParameter.SqlDbType)
					{
						string text = ((!flag) ? ((string)sqlDataReader[array2[13]]) : ((string)sqlDataReader[array2[9]]));
						sqlParameter.UdtTypeName = sqlDataReader[array2[7]]?.ToString() + "." + sqlDataReader[array2[8]]?.ToString() + "." + text;
					}
					if (SqlDbType.Structured == sqlParameter.SqlDbType)
					{
						sqlParameter.TypeName = sqlDataReader[array2[7]]?.ToString() + "." + sqlDataReader[array2[8]]?.ToString() + "." + sqlDataReader[array2[9]];
					}
					if (SqlDbType.Xml == sqlParameter.SqlDbType)
					{
						object obj = sqlDataReader[array2[10]];
						sqlParameter.XmlSchemaCollectionDatabase = (ADP.IsNull(obj) ? string.Empty : ((string)obj));
						obj = sqlDataReader[array2[11]];
						sqlParameter.XmlSchemaCollectionOwningSchema = (ADP.IsNull(obj) ? string.Empty : ((string)obj));
						obj = sqlDataReader[array2[12]];
						sqlParameter.XmlSchemaCollectionName = (ADP.IsNull(obj) ? string.Empty : ((string)obj));
					}
					if (MetaType._IsVarTime(sqlParameter.SqlDbType))
					{
						object obj2 = sqlDataReader[array2[14]];
						if (obj2 is int)
						{
							sqlParameter.ScaleInternal = (byte)((int)obj2 & 0xFF);
						}
					}
					list.Add(sqlParameter);
				}
			}
			catch (Exception e)
			{
				flag2 = ADP.IsCatchableExceptionType(e);
				throw;
			}
			finally
			{
				if (flag2)
				{
					sqlDataReader?.Close();
					sqlCommand.Connection = null;
				}
			}
			if (list.Count == 0)
			{
				throw ADP.NoStoredProcedureExists(CommandText);
			}
			Parameters.Clear();
			{
				foreach (SqlParameter item in list)
				{
					_parameters.Add(item);
				}
				break;
			}
		}
		}
	}

	private ParameterDirection ParameterDirectionFromOleDbDirection(short oledbDirection)
	{
		return oledbDirection switch
		{
			2 => ParameterDirection.InputOutput, 
			3 => ParameterDirection.Output, 
			4 => ParameterDirection.ReturnValue, 
			_ => ParameterDirection.Input, 
		};
	}

	private void CheckNotificationStateAndAutoEnlist()
	{
		if (Notification != null && _sqlDep != null)
		{
			if (_sqlDep.Options == null)
			{
				SqlDependency.IdentityUserNamePair identityUserNamePair = null;
				SqlInternalConnectionTds sqlInternalConnectionTds = _activeConnection.InnerConnection as SqlInternalConnectionTds;
				identityUserNamePair = ((sqlInternalConnectionTds.Identity == null) ? new SqlDependency.IdentityUserNamePair(null, sqlInternalConnectionTds.ConnectionOptions.UserID) : new SqlDependency.IdentityUserNamePair(sqlInternalConnectionTds.Identity, null));
				Notification.Options = SqlDependency.GetDefaultComposedOptions(_activeConnection.DataSource, InternalTdsConnection.ServerProvidedFailOverPartner, identityUserNamePair, _activeConnection.Database);
			}
			Notification.UserData = _sqlDep.ComputeHashAndAddToDispatcher(this);
			_sqlDep.AddToServerList(_activeConnection.DataSource);
		}
	}

	private Task RunExecuteNonQueryTds(string methodName, bool async, int timeout, bool asyncWrite)
	{
		bool flag = true;
		try
		{
			Task task = _activeConnection.ValidateAndReconnect(null, timeout);
			if (task != null)
			{
				long reconnectionStart = ADP.TimerCurrent();
				if (async)
				{
					TaskCompletionSource<object> completion = new TaskCompletionSource<object>();
					_activeConnection.RegisterWaitingForReconnect(completion.Task);
					_reconnectionCompletionSource = completion;
					CancellationTokenSource timeoutCTS = new CancellationTokenSource();
					AsyncHelper.SetTimeoutException(completion, timeout, SQL.CR_ReconnectTimeout, timeoutCTS.Token);
					AsyncHelper.ContinueTask(task, completion, delegate
					{
						if (!completion.Task.IsCompleted)
						{
							Interlocked.CompareExchange(ref _reconnectionCompletionSource, null, completion);
							timeoutCTS.Cancel();
							Task task2 = RunExecuteNonQueryTds(methodName, async, TdsParserStaticMethods.GetRemainingTimeout(timeout, reconnectionStart), asyncWrite);
							if (task2 == null)
							{
								completion.SetResult(null);
							}
							else
							{
								AsyncHelper.ContinueTask(task2, completion, delegate
								{
									completion.SetResult(null);
								});
							}
						}
					}, null, null, null, null, _activeConnection);
					return completion.Task;
				}
				AsyncHelper.WaitForCompletion(task, timeout, delegate
				{
					throw SQL.CR_ReconnectTimeout();
				});
				timeout = TdsParserStaticMethods.GetRemainingTimeout(timeout, reconnectionStart);
			}
			if (asyncWrite)
			{
				_activeConnection.AddWeakReference(this, 2);
			}
			GetStateObject();
			_stateObj.Parser.TdsExecuteSQLBatch(CommandText, timeout, Notification, _stateObj, sync: true);
			NotifyDependency();
			bool dataReady;
			if (async)
			{
				_activeConnection.GetOpenTdsConnection(methodName).IncrementAsyncCount();
			}
			else if (!_stateObj.Parser.TryRun(RunBehavior.UntilDone, this, null, null, _stateObj, out dataReady))
			{
				throw SQL.SynchronousCallMayNotPend();
			}
		}
		catch (Exception e)
		{
			flag = ADP.IsCatchableExceptionType(e);
			throw;
		}
		finally
		{
			if (flag && !async)
			{
				PutStateObject();
			}
		}
		return null;
	}

	internal SqlDataReader RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, [CallerMemberName] string method = "")
	{
		Task task;
		return RunExecuteReader(cmdBehavior, runBehavior, returnStream, null, CommandTimeout, out task, asyncWrite: false, method);
	}

	internal SqlDataReader RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, TaskCompletionSource<object> completion, int timeout, out Task task, bool asyncWrite = false, [CallerMemberName] string method = "")
	{
		bool flag = completion != null;
		task = null;
		_rowsAffected = -1;
		if ((CommandBehavior.SingleRow & cmdBehavior) != CommandBehavior.Default)
		{
			cmdBehavior |= CommandBehavior.SingleResult;
		}
		ValidateCommand(flag, method);
		CheckNotificationStateAndAutoEnlist();
		SqlStatistics statistics = Statistics;
		if (statistics != null)
		{
			if ((!IsDirty && IsPrepared && !_hiddenPrepare) || (IsPrepared && _execType == EXECTYPE.PREPAREPENDING))
			{
				statistics.SafeIncrement(ref statistics._preparedExecs);
			}
			else
			{
				statistics.SafeIncrement(ref statistics._unpreparedExecs);
			}
		}
		return RunExecuteReaderTds(cmdBehavior, runBehavior, returnStream, flag, timeout, out task, asyncWrite && flag);
	}

	private SqlDataReader RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, bool async, int timeout, out Task task, bool asyncWrite, SqlDataReader ds = null)
	{
		if (ds == null && returnStream)
		{
			ds = new SqlDataReader(this, cmdBehavior);
		}
		Task task2 = _activeConnection.ValidateAndReconnect(null, timeout);
		if (task2 != null)
		{
			long reconnectionStart = ADP.TimerCurrent();
			if (async)
			{
				TaskCompletionSource<object> completion = new TaskCompletionSource<object>();
				_activeConnection.RegisterWaitingForReconnect(completion.Task);
				_reconnectionCompletionSource = completion;
				CancellationTokenSource timeoutCTS = new CancellationTokenSource();
				AsyncHelper.SetTimeoutException(completion, timeout, SQL.CR_ReconnectTimeout, timeoutCTS.Token);
				AsyncHelper.ContinueTask(task2, completion, delegate
				{
					if (!completion.Task.IsCompleted)
					{
						Interlocked.CompareExchange(ref _reconnectionCompletionSource, null, completion);
						timeoutCTS.Cancel();
						RunExecuteReaderTds(cmdBehavior, runBehavior, returnStream, async, TdsParserStaticMethods.GetRemainingTimeout(timeout, reconnectionStart), out var task4, asyncWrite, ds);
						if (task4 == null)
						{
							completion.SetResult(null);
						}
						else
						{
							AsyncHelper.ContinueTask(task4, completion, delegate
							{
								completion.SetResult(null);
							});
						}
					}
				}, null, null, null, null, _activeConnection);
				task = completion.Task;
				return ds;
			}
			AsyncHelper.WaitForCompletion(task2, timeout, delegate
			{
				throw SQL.CR_ReconnectTimeout();
			});
			timeout = TdsParserStaticMethods.GetRemainingTimeout(timeout, reconnectionStart);
		}
		bool inSchema = (cmdBehavior & CommandBehavior.SchemaOnly) != 0;
		_SqlRPC rpc = null;
		task = null;
		string optionSettings = null;
		bool flag = true;
		bool flag2 = false;
		if (async)
		{
			_activeConnection.GetOpenTdsConnection().IncrementAsyncCount();
			flag2 = true;
		}
		try
		{
			if (asyncWrite)
			{
				_activeConnection.AddWeakReference(this, 2);
			}
			GetStateObject();
			Task task3 = null;
			if (BatchRPCMode)
			{
				task3 = _stateObj.Parser.TdsExecuteRPC(_SqlRPCBatchArray, timeout, inSchema, Notification, _stateObj, CommandType.StoredProcedure == CommandType, !asyncWrite);
			}
			else if (CommandType.Text == CommandType && GetParameterCount(_parameters) == 0)
			{
				string text = GetCommandText(cmdBehavior) + GetResetOptionsString(cmdBehavior);
				task3 = _stateObj.Parser.TdsExecuteSQLBatch(text, timeout, Notification, _stateObj, !asyncWrite);
			}
			else if (CommandType.Text == CommandType)
			{
				if (IsDirty)
				{
					if (_execType == EXECTYPE.PREPARED)
					{
						_hiddenPrepare = true;
					}
					Unprepare();
					IsDirty = false;
				}
				if (_execType == EXECTYPE.PREPARED)
				{
					rpc = BuildExecute(inSchema);
				}
				else if (_execType == EXECTYPE.PREPAREPENDING)
				{
					rpc = BuildPrepExec(cmdBehavior);
					_execType = EXECTYPE.PREPARED;
					_preparedConnectionCloseCount = _activeConnection.CloseCount;
					_preparedConnectionReconnectCount = _activeConnection.ReconnectCount;
					_inPrepare = true;
				}
				else
				{
					BuildExecuteSql(cmdBehavior, null, _parameters, ref rpc);
				}
				rpc.options = 2;
				task3 = _stateObj.Parser.TdsExecuteRPC(_rpcArrayOf1, timeout, inSchema, Notification, _stateObj, CommandType.StoredProcedure == CommandType, !asyncWrite);
			}
			else
			{
				BuildRPC(inSchema, _parameters, ref rpc);
				optionSettings = GetSetOptionsString(cmdBehavior);
				if (optionSettings != null)
				{
					_stateObj.Parser.TdsExecuteSQLBatch(optionSettings, timeout, Notification, _stateObj, sync: true);
					if (!_stateObj.Parser.TryRun(RunBehavior.UntilDone, this, null, null, _stateObj, out var _))
					{
						throw SQL.SynchronousCallMayNotPend();
					}
					optionSettings = GetResetOptionsString(cmdBehavior);
				}
				task3 = _stateObj.Parser.TdsExecuteRPC(_rpcArrayOf1, timeout, inSchema, Notification, _stateObj, CommandType.StoredProcedure == CommandType, !asyncWrite);
			}
			if (async)
			{
				flag2 = false;
				if (task3 != null)
				{
					task = AsyncHelper.CreateContinuationTask(task3, delegate
					{
						_activeConnection.GetOpenTdsConnection();
						cachedAsyncState.SetAsyncReaderState(ds, runBehavior, optionSettings);
					}, null, delegate
					{
						_activeConnection.GetOpenTdsConnection().DecrementAsyncCount();
					});
				}
				else
				{
					cachedAsyncState.SetAsyncReaderState(ds, runBehavior, optionSettings);
				}
			}
			else
			{
				FinishExecuteReader(ds, runBehavior, optionSettings);
			}
		}
		catch (Exception e)
		{
			flag = ADP.IsCatchableExceptionType(e);
			if (flag2 && _activeConnection.InnerConnection is SqlInternalConnectionTds sqlInternalConnectionTds)
			{
				sqlInternalConnectionTds.DecrementAsyncCount();
			}
			throw;
		}
		finally
		{
			if (flag && !async)
			{
				PutStateObject();
			}
		}
		return ds;
	}

	private SqlDataReader CompleteAsyncExecuteReader()
	{
		SqlDataReader cachedAsyncReader = cachedAsyncState.CachedAsyncReader;
		bool flag = true;
		try
		{
			FinishExecuteReader(cachedAsyncReader, cachedAsyncState.CachedRunBehavior, cachedAsyncState.CachedSetOptions);
			return cachedAsyncReader;
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
				cachedAsyncState.ResetAsyncState();
				PutStateObject();
			}
		}
	}

	private void FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, string resetOptionsString)
	{
		NotifyDependency();
		if (runBehavior == RunBehavior.UntilDone)
		{
			try
			{
				if (!_stateObj.Parser.TryRun(RunBehavior.UntilDone, this, ds, null, _stateObj, out var _))
				{
					throw SQL.SynchronousCallMayNotPend();
				}
			}
			catch (Exception e)
			{
				if (ADP.IsCatchableExceptionType(e))
				{
					if (_inPrepare)
					{
						_inPrepare = false;
						IsDirty = true;
						_execType = EXECTYPE.PREPAREPENDING;
					}
					if (ds != null)
					{
						try
						{
							ds.Close();
						}
						catch (Exception)
						{
						}
					}
				}
				throw;
			}
		}
		if (ds == null)
		{
			return;
		}
		ds.Bind(_stateObj);
		_stateObj = null;
		ds.ResetOptionsString = resetOptionsString;
		_activeConnection.AddWeakReference(ds, 1);
		try
		{
			_cachedMetaData = ds.MetaData;
			ds.IsInitialized = true;
		}
		catch (Exception e2)
		{
			if (ADP.IsCatchableExceptionType(e2))
			{
				if (_inPrepare)
				{
					_inPrepare = false;
					IsDirty = true;
					_execType = EXECTYPE.PREPAREPENDING;
				}
				try
				{
					ds.Close();
				}
				catch (Exception)
				{
				}
			}
			throw;
		}
	}

	private void RegisterForConnectionCloseNotification<T>(ref Task<T> outerTask)
	{
		(_activeConnection ?? throw ADP.ClosedConnectionError()).RegisterForConnectionCloseNotification(ref outerTask, this, 2);
	}

	private void ValidateCommand(bool async, [CallerMemberName] string method = "")
	{
		if (_activeConnection == null)
		{
			throw ADP.ConnectionRequired(method);
		}
		if (_activeConnection.InnerConnection is SqlInternalConnectionTds { Parser: var parser })
		{
			if (parser == null || parser.State == TdsParserState.Closed)
			{
				throw ADP.OpenConnectionRequired(method, ConnectionState.Closed);
			}
			if (parser.State != TdsParserState.OpenLoggedIn)
			{
				throw ADP.OpenConnectionRequired(method, ConnectionState.Broken);
			}
		}
		else
		{
			if (_activeConnection.State == ConnectionState.Closed)
			{
				throw ADP.OpenConnectionRequired(method, ConnectionState.Closed);
			}
			if (_activeConnection.State == ConnectionState.Broken)
			{
				throw ADP.OpenConnectionRequired(method, ConnectionState.Broken);
			}
		}
		ValidateAsyncCommand();
		_activeConnection.ValidateConnectionForExecute(method, this);
		if (_transaction != null && _transaction.Connection == null)
		{
			_transaction = null;
		}
		if (_activeConnection.HasLocalTransactionFromAPI && _transaction == null)
		{
			throw ADP.TransactionRequired(method);
		}
		if (_transaction != null && _activeConnection != _transaction.Connection)
		{
			throw ADP.TransactionConnectionMismatch();
		}
		if (string.IsNullOrEmpty(CommandText))
		{
			throw ADP.CommandTextRequired(method);
		}
	}

	private void ValidateAsyncCommand()
	{
		if (cachedAsyncState.PendingAsyncOperation)
		{
			if (cachedAsyncState.IsActiveConnectionValid(_activeConnection))
			{
				throw SQL.PendingBeginXXXExists();
			}
			_stateObj = null;
			cachedAsyncState.ResetAsyncState();
		}
	}

	private void GetStateObject(TdsParser parser = null)
	{
		if (_pendingCancel)
		{
			_pendingCancel = false;
			throw SQL.OperationCancelled();
		}
		if (parser == null)
		{
			parser = _activeConnection.Parser;
			if (parser == null || parser.State == TdsParserState.Broken || parser.State == TdsParserState.Closed)
			{
				throw ADP.ClosedConnectionError();
			}
		}
		TdsParserStateObject session = parser.GetSession(this);
		session.StartSession(this);
		_stateObj = session;
		if (_pendingCancel)
		{
			_pendingCancel = false;
			throw SQL.OperationCancelled();
		}
	}

	private void ReliablePutStateObject()
	{
		PutStateObject();
	}

	private void PutStateObject()
	{
		TdsParserStateObject stateObj = _stateObj;
		_stateObj = null;
		stateObj?.CloseSession();
	}

	internal void OnDoneProc()
	{
		if (BatchRPCMode)
		{
			_SqlRPCBatchArray[_currentlyExecutingBatch].cumulativeRecordsAffected = _rowsAffected;
			_SqlRPCBatchArray[_currentlyExecutingBatch].recordsAffected = ((0 < _currentlyExecutingBatch && 0 <= _rowsAffected) ? (_rowsAffected - Math.Max(_SqlRPCBatchArray[_currentlyExecutingBatch - 1].cumulativeRecordsAffected, 0)) : _rowsAffected);
			_SqlRPCBatchArray[_currentlyExecutingBatch].errorsIndexStart = ((0 < _currentlyExecutingBatch) ? _SqlRPCBatchArray[_currentlyExecutingBatch - 1].errorsIndexEnd : 0);
			_SqlRPCBatchArray[_currentlyExecutingBatch].errorsIndexEnd = _stateObj.ErrorCount;
			_SqlRPCBatchArray[_currentlyExecutingBatch].errors = _stateObj._errors;
			_SqlRPCBatchArray[_currentlyExecutingBatch].warningsIndexStart = ((0 < _currentlyExecutingBatch) ? _SqlRPCBatchArray[_currentlyExecutingBatch - 1].warningsIndexEnd : 0);
			_SqlRPCBatchArray[_currentlyExecutingBatch].warningsIndexEnd = _stateObj.WarningCount;
			_SqlRPCBatchArray[_currentlyExecutingBatch].warnings = _stateObj._warnings;
			_currentlyExecutingBatch++;
		}
	}

	internal void OnReturnStatus(int status)
	{
		if (_inPrepare)
		{
			return;
		}
		SqlParameterCollection sqlParameterCollection = _parameters;
		if (BatchRPCMode)
		{
			sqlParameterCollection = ((_parameterCollectionList.Count <= _currentlyExecutingBatch) ? null : _parameterCollectionList[_currentlyExecutingBatch]);
		}
		int parameterCount = GetParameterCount(sqlParameterCollection);
		for (int i = 0; i < parameterCount; i++)
		{
			SqlParameter sqlParameter = sqlParameterCollection[i];
			if (sqlParameter.Direction == ParameterDirection.ReturnValue)
			{
				object value = sqlParameter.Value;
				if (value != null && value.GetType() == typeof(SqlInt32))
				{
					sqlParameter.Value = new SqlInt32(status);
				}
				else
				{
					sqlParameter.Value = status;
				}
				break;
			}
		}
	}

	internal void OnReturnValue(SqlReturnValue rec, TdsParserStateObject stateObj)
	{
		if (_inPrepare)
		{
			if (!rec.value.IsNull)
			{
				_prepareHandle = rec.value.Int32;
			}
			_inPrepare = false;
			return;
		}
		SqlParameterCollection currentParameterCollection = GetCurrentParameterCollection();
		int parameterCount = GetParameterCount(currentParameterCollection);
		SqlParameter parameterForOutputValueExtraction = GetParameterForOutputValueExtraction(currentParameterCollection, rec.parameter, parameterCount);
		if (parameterForOutputValueExtraction == null)
		{
			return;
		}
		_ = parameterForOutputValueExtraction.Value;
		if (SqlDbType.Udt == parameterForOutputValueExtraction.SqlDbType)
		{
			object obj = null;
			try
			{
				Connection.CheckGetExtendedUDTInfo(rec, fThrow: true);
				obj = ((!rec.value.IsNull) ? ((object)rec.value.ByteArray) : ((object)DBNull.Value));
				parameterForOutputValueExtraction.Value = Connection.GetUdtValue(obj, rec, returnDBNull: false);
				return;
			}
			catch (FileNotFoundException udtLoadError)
			{
				parameterForOutputValueExtraction.SetUdtLoadError(udtLoadError);
				return;
			}
			catch (FileLoadException udtLoadError2)
			{
				parameterForOutputValueExtraction.SetUdtLoadError(udtLoadError2);
				return;
			}
		}
		parameterForOutputValueExtraction.SetSqlBuffer(rec.value);
		MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(rec.type, isMultiValued: false);
		if (rec.type == SqlDbType.Decimal)
		{
			parameterForOutputValueExtraction.ScaleInternal = rec.scale;
			parameterForOutputValueExtraction.PrecisionInternal = rec.precision;
		}
		else if (metaTypeFromSqlDbType.IsVarTime)
		{
			parameterForOutputValueExtraction.ScaleInternal = rec.scale;
		}
		else if (rec.type == SqlDbType.Xml && parameterForOutputValueExtraction.Value is SqlCachedBuffer sqlCachedBuffer)
		{
			parameterForOutputValueExtraction.Value = sqlCachedBuffer.ToString();
		}
		if (rec.collation != null)
		{
			parameterForOutputValueExtraction.Collation = rec.collation;
		}
	}

	private SqlParameterCollection GetCurrentParameterCollection()
	{
		if (BatchRPCMode)
		{
			if (_parameterCollectionList.Count > _currentlyExecutingBatch)
			{
				return _parameterCollectionList[_currentlyExecutingBatch];
			}
			return null;
		}
		return _parameters;
	}

	private SqlParameter GetParameterForOutputValueExtraction(SqlParameterCollection parameters, string paramName, int paramCount)
	{
		SqlParameter sqlParameter = null;
		bool flag = false;
		if (paramName == null)
		{
			for (int i = 0; i < paramCount; i++)
			{
				sqlParameter = parameters[i];
				if (sqlParameter.Direction == ParameterDirection.ReturnValue)
				{
					flag = true;
					break;
				}
			}
		}
		else
		{
			for (int j = 0; j < paramCount; j++)
			{
				sqlParameter = parameters[j];
				if (sqlParameter.Direction != ParameterDirection.Input && sqlParameter.Direction != ParameterDirection.ReturnValue && paramName == sqlParameter.ParameterNameFixed)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			return sqlParameter;
		}
		return null;
	}

	private void GetRPCObject(int paramCount, ref _SqlRPC rpc)
	{
		if (rpc == null)
		{
			if (_rpcArrayOf1 == null)
			{
				_rpcArrayOf1 = new _SqlRPC[1];
				_rpcArrayOf1[0] = new _SqlRPC();
			}
			rpc = _rpcArrayOf1[0];
		}
		rpc.ProcID = 0;
		rpc.rpcName = null;
		rpc.options = 0;
		if (rpc.parameters == null || rpc.parameters.Length < paramCount)
		{
			rpc.parameters = new SqlParameter[paramCount];
		}
		else if (rpc.parameters.Length > paramCount)
		{
			rpc.parameters[paramCount] = null;
		}
		if (rpc.paramoptions == null || rpc.paramoptions.Length < paramCount)
		{
			rpc.paramoptions = new byte[paramCount];
			return;
		}
		for (int i = 0; i < paramCount; i++)
		{
			rpc.paramoptions[i] = 0;
		}
	}

	private void SetUpRPCParameters(_SqlRPC rpc, int startCount, bool inSchema, SqlParameterCollection parameters)
	{
		int parameterCount = GetParameterCount(parameters);
		int num = startCount;
		_ = _activeConnection.Parser;
		for (int i = 0; i < parameterCount; i++)
		{
			SqlParameter sqlParameter = parameters[i];
			sqlParameter.Validate(i, CommandType.StoredProcedure == CommandType);
			if (!sqlParameter.ValidateTypeLengths().IsPlp && sqlParameter.Direction != ParameterDirection.Output)
			{
				sqlParameter.FixStreamDataForNonPLP();
			}
			if (ShouldSendParameter(sqlParameter))
			{
				rpc.parameters[num] = sqlParameter;
				if (sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Output)
				{
					rpc.paramoptions[num] = 1;
				}
				if (sqlParameter.Direction != ParameterDirection.Output && sqlParameter.Value == null && (!inSchema || SqlDbType.Structured == sqlParameter.SqlDbType))
				{
					rpc.paramoptions[num] |= 2;
				}
				num++;
			}
		}
	}

	private _SqlRPC BuildPrepExec(CommandBehavior behavior)
	{
		int num = 3;
		int num2 = CountSendableParameters(_parameters);
		_SqlRPC rpc = null;
		GetRPCObject(num2 + num, ref rpc);
		rpc.ProcID = 13;
		rpc.rpcName = "sp_prepexec";
		SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.Int);
		sqlParameter.Direction = ParameterDirection.InputOutput;
		sqlParameter.Value = _prepareHandle;
		rpc.parameters[0] = sqlParameter;
		rpc.paramoptions[0] = 1;
		string text = BuildParamList(_stateObj.Parser, _parameters);
		sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, text.Length);
		sqlParameter.Value = text;
		rpc.parameters[1] = sqlParameter;
		string commandText = GetCommandText(behavior);
		sqlParameter = new SqlParameter(null, (commandText.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, commandText.Length);
		sqlParameter.Value = commandText;
		rpc.parameters[2] = sqlParameter;
		SetUpRPCParameters(rpc, num, inSchema: false, _parameters);
		return rpc;
	}

	private static bool ShouldSendParameter(SqlParameter p)
	{
		ParameterDirection direction = p.Direction;
		if ((uint)(direction - 1) > 2u)
		{
			_ = 6;
			return false;
		}
		return true;
	}

	private int CountSendableParameters(SqlParameterCollection parameters)
	{
		int num = 0;
		if (parameters != null)
		{
			int count = parameters.Count;
			for (int i = 0; i < count; i++)
			{
				if (ShouldSendParameter(parameters[i]))
				{
					num++;
				}
			}
		}
		return num;
	}

	private int GetParameterCount(SqlParameterCollection parameters)
	{
		return parameters?.Count ?? 0;
	}

	private void BuildRPC(bool inSchema, SqlParameterCollection parameters, ref _SqlRPC rpc)
	{
		int paramCount = CountSendableParameters(parameters);
		GetRPCObject(paramCount, ref rpc);
		rpc.rpcName = CommandText;
		SetUpRPCParameters(rpc, 0, inSchema, parameters);
	}

	private _SqlRPC BuildExecute(bool inSchema)
	{
		int num = 1;
		int num2 = CountSendableParameters(_parameters);
		_SqlRPC rpc = null;
		GetRPCObject(num2 + num, ref rpc);
		rpc.ProcID = 12;
		rpc.rpcName = "sp_execute";
		SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.Int);
		sqlParameter.Value = _prepareHandle;
		rpc.parameters[0] = sqlParameter;
		SetUpRPCParameters(rpc, num, inSchema, _parameters);
		return rpc;
	}

	private void BuildExecuteSql(CommandBehavior behavior, string commandText, SqlParameterCollection parameters, ref _SqlRPC rpc)
	{
		int num = CountSendableParameters(parameters);
		int num2 = ((num <= 0) ? 1 : 2);
		GetRPCObject(num + num2, ref rpc);
		rpc.ProcID = 10;
		rpc.rpcName = "sp_executesql";
		if (commandText == null)
		{
			commandText = GetCommandText(behavior);
		}
		SqlParameter sqlParameter = new SqlParameter(null, (commandText.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, commandText.Length);
		sqlParameter.Value = commandText;
		rpc.parameters[0] = sqlParameter;
		if (num > 0)
		{
			string text = BuildParamList(_stateObj.Parser, BatchRPCMode ? parameters : _parameters);
			sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, text.Length);
			sqlParameter.Value = text;
			rpc.parameters[1] = sqlParameter;
			bool inSchema = (behavior & CommandBehavior.SchemaOnly) != 0;
			SetUpRPCParameters(rpc, num2, inSchema, parameters);
		}
	}

	internal string BuildParamList(TdsParser parser, SqlParameterCollection parameters)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		int num = 0;
		num = parameters.Count;
		for (int i = 0; i < num; i++)
		{
			SqlParameter sqlParameter = parameters[i];
			sqlParameter.Validate(i, CommandType.StoredProcedure == CommandType);
			if (!ShouldSendParameter(sqlParameter))
			{
				continue;
			}
			if (flag)
			{
				stringBuilder.Append(',');
			}
			stringBuilder.Append(sqlParameter.ParameterNameFixed);
			MetaType metaType = sqlParameter.InternalMetaType;
			stringBuilder.Append(" ");
			if (metaType.SqlDbType == SqlDbType.Udt)
			{
				string udtTypeName = sqlParameter.UdtTypeName;
				if (string.IsNullOrEmpty(udtTypeName))
				{
					throw SQL.MustSetUdtTypeNameForUdtParams();
				}
				stringBuilder.Append(ParseAndQuoteIdentifier(udtTypeName, isUdtTypeName: true));
			}
			else if (metaType.SqlDbType == SqlDbType.Structured)
			{
				string typeName = sqlParameter.TypeName;
				if (string.IsNullOrEmpty(typeName))
				{
					throw SQL.MustSetTypeNameForParam(metaType.TypeName, sqlParameter.ParameterNameFixed);
				}
				stringBuilder.Append(ParseAndQuoteIdentifier(typeName, isUdtTypeName: false));
				stringBuilder.Append(" READONLY");
			}
			else
			{
				metaType = sqlParameter.ValidateTypeLengths();
				if (!metaType.IsPlp && sqlParameter.Direction != ParameterDirection.Output)
				{
					sqlParameter.FixStreamDataForNonPLP();
				}
				stringBuilder.Append(metaType.TypeName);
			}
			flag = true;
			if (metaType.SqlDbType == SqlDbType.Decimal)
			{
				byte b = sqlParameter.GetActualPrecision();
				byte actualScale = sqlParameter.GetActualScale();
				stringBuilder.Append('(');
				if (b == 0)
				{
					b = 29;
				}
				stringBuilder.Append(b);
				stringBuilder.Append(',');
				stringBuilder.Append(actualScale);
				stringBuilder.Append(')');
			}
			else if (metaType.IsVarTime)
			{
				byte actualScale2 = sqlParameter.GetActualScale();
				stringBuilder.Append('(');
				stringBuilder.Append(actualScale2);
				stringBuilder.Append(')');
			}
			else if (!metaType.IsFixed && !metaType.IsLong && metaType.SqlDbType != SqlDbType.Timestamp && metaType.SqlDbType != SqlDbType.Udt && SqlDbType.Structured != metaType.SqlDbType)
			{
				int num2 = sqlParameter.Size;
				stringBuilder.Append('(');
				if (metaType.IsAnsiType)
				{
					object coercedValue = sqlParameter.GetCoercedValue();
					string text = null;
					if (coercedValue != null && DBNull.Value != coercedValue)
					{
						text = coercedValue as string;
						if (text == null)
						{
							SqlString sqlString = ((coercedValue is SqlString) ? ((SqlString)coercedValue) : SqlString.Null);
							if (!sqlString.IsNull)
							{
								text = sqlString.Value;
							}
						}
					}
					if (text != null)
					{
						int encodingCharLength = parser.GetEncodingCharLength(text, sqlParameter.GetActualSize(), sqlParameter.Offset, null);
						if (encodingCharLength > num2)
						{
							num2 = encodingCharLength;
						}
					}
				}
				if (num2 == 0)
				{
					num2 = (metaType.IsSizeInCharacters ? 4000 : 8000);
				}
				stringBuilder.Append(num2);
				stringBuilder.Append(')');
			}
			else if (metaType.IsPlp && metaType.SqlDbType != SqlDbType.Xml && metaType.SqlDbType != SqlDbType.Udt)
			{
				stringBuilder.Append("(max) ");
			}
			if (sqlParameter.Direction != ParameterDirection.Input)
			{
				stringBuilder.Append(" output");
			}
		}
		return stringBuilder.ToString();
	}

	private string ParseAndQuoteIdentifier(string identifier, bool isUdtTypeName)
	{
		string[] array = SqlParameter.ParseTypeName(identifier, isUdtTypeName);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			if (0 < stringBuilder.Length)
			{
				stringBuilder.Append('.');
			}
			if (array[i] != null && array[i].Length != 0)
			{
				stringBuilder.Append(ADP.BuildQuotedString("[", "]", array[i]));
			}
		}
		return stringBuilder.ToString();
	}

	private string GetSetOptionsString(CommandBehavior behavior)
	{
		string text = null;
		if (CommandBehavior.SchemaOnly == (behavior & CommandBehavior.SchemaOnly) || CommandBehavior.KeyInfo == (behavior & CommandBehavior.KeyInfo))
		{
			text = " SET FMTONLY OFF;";
			if (CommandBehavior.KeyInfo == (behavior & CommandBehavior.KeyInfo))
			{
				text += " SET NO_BROWSETABLE ON;";
			}
			if (CommandBehavior.SchemaOnly == (behavior & CommandBehavior.SchemaOnly))
			{
				text += " SET FMTONLY ON;";
			}
		}
		return text;
	}

	private string GetResetOptionsString(CommandBehavior behavior)
	{
		string text = null;
		if (CommandBehavior.SchemaOnly == (behavior & CommandBehavior.SchemaOnly))
		{
			text += " SET FMTONLY OFF;";
		}
		if (CommandBehavior.KeyInfo == (behavior & CommandBehavior.KeyInfo))
		{
			text += " SET NO_BROWSETABLE OFF;";
		}
		return text;
	}

	private string GetCommandText(CommandBehavior behavior)
	{
		return GetSetOptionsString(behavior) + CommandText;
	}

	internal void CheckThrowSNIException()
	{
		_stateObj?.CheckThrowSNIException();
	}

	internal void OnConnectionClosed()
	{
		_stateObj?.OnConnectionClosed();
	}

	internal void ClearBatchCommand()
	{
		_RPCList?.Clear();
		if (_parameterCollectionList != null)
		{
			_parameterCollectionList.Clear();
		}
		_SqlRPCBatchArray = null;
		_currentlyExecutingBatch = 0;
	}

	internal void AddBatchCommand(string commandText, SqlParameterCollection parameters, CommandType cmdType)
	{
		_SqlRPC rpc = new _SqlRPC();
		CommandText = commandText;
		CommandType = cmdType;
		GetStateObject();
		if (cmdType == CommandType.StoredProcedure)
		{
			BuildRPC(inSchema: false, parameters, ref rpc);
		}
		else
		{
			BuildExecuteSql(CommandBehavior.Default, commandText, parameters, ref rpc);
		}
		_RPCList.Add(rpc);
		_parameterCollectionList.Add(parameters);
		ReliablePutStateObject();
	}

	internal int ExecuteBatchRPCCommand()
	{
		_SqlRPCBatchArray = _RPCList.ToArray();
		_currentlyExecutingBatch = 0;
		return ExecuteNonQuery();
	}

	internal int? GetRecordsAffected(int commandIndex)
	{
		return _SqlRPCBatchArray[commandIndex].recordsAffected;
	}

	internal SqlException GetErrors(int commandIndex)
	{
		SqlException result = null;
		int num = _SqlRPCBatchArray[commandIndex].errorsIndexEnd - _SqlRPCBatchArray[commandIndex].errorsIndexStart;
		if (0 < num)
		{
			SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
			for (int i = _SqlRPCBatchArray[commandIndex].errorsIndexStart; i < _SqlRPCBatchArray[commandIndex].errorsIndexEnd; i++)
			{
				sqlErrorCollection.Add(_SqlRPCBatchArray[commandIndex].errors[i]);
			}
			for (int j = _SqlRPCBatchArray[commandIndex].warningsIndexStart; j < _SqlRPCBatchArray[commandIndex].warningsIndexEnd; j++)
			{
				sqlErrorCollection.Add(_SqlRPCBatchArray[commandIndex].warnings[j]);
			}
			result = SqlException.CreateException(sqlErrorCollection, Connection.ServerVersion, Connection.ClientConnectionId);
		}
		return result;
	}

	internal new void CancelIgnoreFailure()
	{
		try
		{
			Cancel();
		}
		catch (Exception)
		{
		}
	}

	private void NotifyDependency()
	{
		if (_sqlDep != null)
		{
			_sqlDep.StartTimer(Notification);
		}
	}

	object ICloneable.Clone()
	{
		return Clone();
	}

	public SqlCommand Clone()
	{
		return new SqlCommand(this);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginExecuteReader()
	{
		return BeginExecuteReader(CommandBehavior.Default, null, null);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject)
	{
		return BeginExecuteReader(CommandBehavior.Default, callback, stateObject);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject, CommandBehavior behavior)
	{
		return BeginExecuteReader(behavior, callback, stateObject);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginExecuteReader(CommandBehavior behavior)
	{
		return BeginExecuteReader(behavior, null, null);
	}
}
