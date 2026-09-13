using System.Collections;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient;

public sealed class SqlConnection : DbConnection, ICloneable, IDbConnection, IDisposable
{
	private class OpenAsyncRetry
	{
		private SqlConnection _parent;

		private TaskCompletionSource<DbConnectionInternal> _retry;

		private TaskCompletionSource<object> _result;

		private CancellationTokenRegistration _registration;

		public OpenAsyncRetry(SqlConnection parent, TaskCompletionSource<DbConnectionInternal> retry, TaskCompletionSource<object> result, CancellationTokenRegistration registration)
		{
			_parent = parent;
			_retry = retry;
			_result = result;
			_registration = registration;
		}

		internal void Retry(Task<DbConnectionInternal> retryTask)
		{
			_registration.Dispose();
			try
			{
				SqlStatistics statistics = null;
				try
				{
					statistics = SqlStatistics.StartTimer(_parent.Statistics);
					if (retryTask.IsFaulted)
					{
						_ = retryTask.Exception.InnerException;
						_parent.CloseInnerConnection();
						_parent._currentCompletion = null;
						_result.SetException(retryTask.Exception.InnerException);
						return;
					}
					if (retryTask.IsCanceled)
					{
						_parent.CloseInnerConnection();
						_parent._currentCompletion = null;
						_result.SetCanceled();
						return;
					}
					bool flag;
					lock (_parent.InnerConnection)
					{
						flag = _parent.TryOpen(_retry);
					}
					if (flag)
					{
						_parent._currentCompletion = null;
						_result.SetResult(null);
					}
					else
					{
						_parent.CloseInnerConnection();
						_parent._currentCompletion = null;
						_result.SetException(ADP.ExceptionWithStackTrace(ADP.InternalError(ADP.InternalErrorCode.CompletedConnectReturnedPending)));
					}
				}
				finally
				{
					SqlStatistics.StopTimer(statistics);
				}
			}
			catch (Exception exception)
			{
				_parent.CloseInnerConnection();
				_parent._currentCompletion = null;
				_result.SetException(exception);
			}
		}
	}

	private bool _AsyncCommandInProgress;

	internal SqlStatistics _statistics;

	private bool _collectstats;

	private bool _fireInfoMessageEventOnUserErrors;

	private Tuple<TaskCompletionSource<DbConnectionInternal>, Task> _currentCompletion;

	private SqlCredential _credential;

	private string _connectionString;

	private int _connectRetryCount;

	private string _accessToken;

	private object _reconnectLock = new object();

	internal Task _currentReconnectionTask;

	private Task _asyncWaitingForReconnection;

	private Guid _originalConnectionId = Guid.Empty;

	private CancellationTokenSource _reconnectionCancellationSource;

	internal SessionData _recoverySessionData;

	internal new bool _suppressStateChangeForReconnection;

	private int _reconnectCount;

	private static readonly DiagnosticListener s_diagnosticListener = new DiagnosticListener("SqlClientDiagnosticListener");

	internal bool _applyTransientFaultHandling;

	private static readonly DbConnectionFactory s_connectionFactory = SqlConnectionFactory.SingletonInstance;

	private DbConnectionOptions _userConnectionOptions;

	private DbConnectionPoolGroup _poolGroup;

	private DbConnectionInternal _innerConnection;

	private int _closeCount;

	public bool StatisticsEnabled
	{
		get
		{
			return _collectstats;
		}
		set
		{
			if (value)
			{
				if (ConnectionState.Open == State)
				{
					if (_statistics == null)
					{
						_statistics = new SqlStatistics();
						ADP.TimerCurrent(out _statistics._openTimestamp);
					}
					Parser.Statistics = _statistics;
				}
			}
			else if (_statistics != null && ConnectionState.Open == State)
			{
				Parser.Statistics = null;
				ADP.TimerCurrent(out _statistics._closeTimestamp);
			}
			_collectstats = value;
		}
	}

	internal bool AsyncCommandInProgress
	{
		get
		{
			return _AsyncCommandInProgress;
		}
		set
		{
			_AsyncCommandInProgress = value;
		}
	}

	internal SqlConnectionString.TransactionBindingEnum TransactionBinding => ((SqlConnectionString)ConnectionOptions).TransactionBinding;

	internal SqlConnectionString.TypeSystem TypeSystem => ((SqlConnectionString)ConnectionOptions).TypeSystemVersion;

	internal Version TypeSystemAssemblyVersion => ((SqlConnectionString)ConnectionOptions).TypeSystemAssemblyVersion;

	internal int ConnectRetryInterval => ((SqlConnectionString)ConnectionOptions).ConnectRetryInterval;

	public override string ConnectionString
	{
		get
		{
			return ConnectionString_Get();
		}
		set
		{
			if (_credential != null || _accessToken != null)
			{
				SqlConnectionString connectionOptions = new SqlConnectionString(value);
				if (_credential != null)
				{
					CheckAndThrowOnInvalidCombinationOfConnectionStringAndSqlCredential(connectionOptions);
				}
				else
				{
					CheckAndThrowOnInvalidCombinationOfConnectionOptionAndAccessToken(connectionOptions);
				}
			}
			ConnectionString_Set(new SqlConnectionPoolKey(value, _credential, _accessToken));
			_connectionString = value;
			CacheConnectionStringProperties();
		}
	}

	public override int ConnectionTimeout => ((SqlConnectionString)ConnectionOptions)?.ConnectTimeout ?? 15;

	public string AccessToken
	{
		get
		{
			_ = _accessToken;
			SqlConnectionString sqlConnectionString = (SqlConnectionString)UserConnectionOptions;
			if (!InnerConnection.ShouldHidePassword || sqlConnectionString == null || sqlConnectionString.PersistSecurityInfo)
			{
				return _accessToken;
			}
			return null;
		}
		set
		{
			if (!InnerConnection.AllowSetConnectionString)
			{
				throw ADP.OpenConnectionPropertySet("AccessToken", InnerConnection.State);
			}
			if (value != null)
			{
				CheckAndThrowOnInvalidCombinationOfConnectionOptionAndAccessToken((SqlConnectionString)ConnectionOptions);
			}
			ConnectionString_Set(new SqlConnectionPoolKey(_connectionString, _credential, value));
			_accessToken = value;
		}
	}

	public override string Database
	{
		get
		{
			if (!(InnerConnection is SqlInternalConnection { CurrentDatabase: var currentDatabase }))
			{
				SqlConnectionString sqlConnectionString = (SqlConnectionString)ConnectionOptions;
				return (sqlConnectionString != null) ? sqlConnectionString.InitialCatalog : "";
			}
			return currentDatabase;
		}
	}

	public override string DataSource
	{
		get
		{
			if (!(InnerConnection is SqlInternalConnection { CurrentDataSource: var currentDataSource }))
			{
				SqlConnectionString sqlConnectionString = (SqlConnectionString)ConnectionOptions;
				return (sqlConnectionString != null) ? sqlConnectionString.DataSource : "";
			}
			return currentDataSource;
		}
	}

	public int PacketSize
	{
		get
		{
			if (!(InnerConnection is SqlInternalConnectionTds { PacketSize: var packetSize }))
			{
				return ((SqlConnectionString)ConnectionOptions)?.PacketSize ?? 8000;
			}
			return packetSize;
		}
	}

	public Guid ClientConnectionId
	{
		get
		{
			if (InnerConnection is SqlInternalConnectionTds sqlInternalConnectionTds)
			{
				return sqlInternalConnectionTds.ClientConnectionId;
			}
			Task currentReconnectionTask = _currentReconnectionTask;
			if (currentReconnectionTask != null && !currentReconnectionTask.IsCompleted)
			{
				return _originalConnectionId;
			}
			return Guid.Empty;
		}
	}

	public override string ServerVersion => GetOpenTdsConnection().ServerVersion;

	public override ConnectionState State
	{
		get
		{
			Task currentReconnectionTask = _currentReconnectionTask;
			if (currentReconnectionTask != null && !currentReconnectionTask.IsCompleted)
			{
				return ConnectionState.Open;
			}
			return InnerConnection.State;
		}
	}

	internal SqlStatistics Statistics => _statistics;

	public string WorkstationId => ((SqlConnectionString)ConnectionOptions)?.WorkstationId ?? Environment.MachineName;

	public SqlCredential Credential
	{
		get
		{
			SqlCredential result = _credential;
			SqlConnectionString sqlConnectionString = (SqlConnectionString)UserConnectionOptions;
			if (InnerConnection.ShouldHidePassword && sqlConnectionString != null && !sqlConnectionString.PersistSecurityInfo)
			{
				result = null;
			}
			return result;
		}
		set
		{
			if (!InnerConnection.AllowSetConnectionString)
			{
				throw ADP.OpenConnectionPropertySet("Credential", InnerConnection.State);
			}
			if (value != null)
			{
				CheckAndThrowOnInvalidCombinationOfConnectionStringAndSqlCredential((SqlConnectionString)ConnectionOptions);
				if (_accessToken != null)
				{
					throw ADP.InvalidMixedUsageOfCredentialAndAccessToken();
				}
			}
			_credential = value;
			ConnectionString_Set(new SqlConnectionPoolKey(_connectionString, _credential, _accessToken));
		}
	}

	protected override DbProviderFactory DbProviderFactory => SqlClientFactory.Instance;

	public bool FireInfoMessageEventOnUserErrors
	{
		get
		{
			return _fireInfoMessageEventOnUserErrors;
		}
		set
		{
			_fireInfoMessageEventOnUserErrors = value;
		}
	}

	internal int ReconnectCount => _reconnectCount;

	internal bool ForceNewConnection { get; set; }

	internal bool HasLocalTransaction => GetOpenTdsConnection().HasLocalTransaction;

	internal bool HasLocalTransactionFromAPI
	{
		get
		{
			Task currentReconnectionTask = _currentReconnectionTask;
			if (currentReconnectionTask != null && !currentReconnectionTask.IsCompleted)
			{
				return false;
			}
			return GetOpenTdsConnection().HasLocalTransactionFromAPI;
		}
	}

	internal bool IsKatmaiOrNewer
	{
		get
		{
			if (_currentReconnectionTask != null)
			{
				return true;
			}
			return GetOpenTdsConnection().IsKatmaiOrNewer;
		}
	}

	internal TdsParser Parser => GetOpenTdsConnection().Parser;

	internal int CloseCount => _closeCount;

	internal DbConnectionFactory ConnectionFactory => s_connectionFactory;

	internal DbConnectionOptions ConnectionOptions => PoolGroup?.ConnectionOptions;

	internal DbConnectionInternal InnerConnection => _innerConnection;

	internal DbConnectionPoolGroup PoolGroup
	{
		get
		{
			return _poolGroup;
		}
		set
		{
			_poolGroup = value;
		}
	}

	internal DbConnectionOptions UserConnectionOptions => _userConnectionOptions;

	[System.MonoTODO]
	public SqlCredential Credentials
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public event SqlInfoMessageEventHandler InfoMessage;

	public SqlConnection(string connectionString)
		: this()
	{
		ConnectionString = connectionString;
		CacheConnectionStringProperties();
	}

	public SqlConnection(string connectionString, SqlCredential credential)
		: this()
	{
		ConnectionString = connectionString;
		if (credential != null)
		{
			SqlConnectionString opt = (SqlConnectionString)ConnectionOptions;
			if (UsesClearUserIdOrPassword(opt))
			{
				throw ADP.InvalidMixedArgumentOfSecureAndClearCredential();
			}
			if (UsesIntegratedSecurity(opt))
			{
				throw ADP.InvalidMixedArgumentOfSecureCredentialAndIntegratedSecurity();
			}
			Credential = credential;
		}
		CacheConnectionStringProperties();
	}

	private SqlConnection(SqlConnection connection)
	{
		GC.SuppressFinalize(this);
		CopyFrom(connection);
		_connectionString = connection._connectionString;
		if (connection._credential != null)
		{
			SecureString secureString = connection._credential.Password.Copy();
			secureString.MakeReadOnly();
			_credential = new SqlCredential(connection._credential.UserId, secureString);
		}
		_accessToken = connection._accessToken;
		CacheConnectionStringProperties();
	}

	private void CacheConnectionStringProperties()
	{
		if (ConnectionOptions is SqlConnectionString sqlConnectionString)
		{
			_connectRetryCount = sqlConnectionString.ConnectRetryCount;
		}
	}

	private bool UsesIntegratedSecurity(SqlConnectionString opt)
	{
		return opt?.IntegratedSecurity ?? false;
	}

	private bool UsesClearUserIdOrPassword(SqlConnectionString opt)
	{
		bool result = false;
		if (opt != null)
		{
			result = !string.IsNullOrEmpty(opt.UserID) || !string.IsNullOrEmpty(opt.Password);
		}
		return result;
	}

	private void CheckAndThrowOnInvalidCombinationOfConnectionStringAndSqlCredential(SqlConnectionString connectionOptions)
	{
		if (UsesClearUserIdOrPassword(connectionOptions))
		{
			throw ADP.InvalidMixedUsageOfSecureAndClearCredential();
		}
		if (UsesIntegratedSecurity(connectionOptions))
		{
			throw ADP.InvalidMixedUsageOfSecureCredentialAndIntegratedSecurity();
		}
	}

	private void CheckAndThrowOnInvalidCombinationOfConnectionOptionAndAccessToken(SqlConnectionString connectionOptions)
	{
		if (UsesClearUserIdOrPassword(connectionOptions))
		{
			throw ADP.InvalidMixedUsageOfAccessTokenAndUserIDPassword();
		}
		if (UsesIntegratedSecurity(connectionOptions))
		{
			throw ADP.InvalidMixedUsageOfAccessTokenAndIntegratedSecurity();
		}
		if (_credential != null)
		{
			throw ADP.InvalidMixedUsageOfCredentialAndAccessToken();
		}
	}

	protected override void OnStateChange(StateChangeEventArgs stateChange)
	{
		if (!_suppressStateChangeForReconnection)
		{
			base.OnStateChange(stateChange);
		}
	}

	public new SqlTransaction BeginTransaction()
	{
		return BeginTransaction(IsolationLevel.Unspecified, null);
	}

	public new SqlTransaction BeginTransaction(IsolationLevel iso)
	{
		return BeginTransaction(iso, null);
	}

	public SqlTransaction BeginTransaction(string transactionName)
	{
		return BeginTransaction(IsolationLevel.Unspecified, transactionName);
	}

	protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
	{
		SqlTransaction result = BeginTransaction(isolationLevel);
		GC.KeepAlive(this);
		return result;
	}

	public SqlTransaction BeginTransaction(IsolationLevel iso, string transactionName)
	{
		WaitForPendingReconnection();
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			bool shouldReconnect = true;
			SqlTransaction sqlTransaction;
			do
			{
				sqlTransaction = GetOpenTdsConnection().BeginSqlTransaction(iso, transactionName, shouldReconnect);
				shouldReconnect = false;
			}
			while (sqlTransaction.InternalTransaction.ConnectionHasBeenRestored);
			GC.KeepAlive(this);
			return sqlTransaction;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public override void ChangeDatabase(string database)
	{
		SqlStatistics statistics = null;
		RepairInnerConnection();
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			InnerConnection.ChangeDatabase(database);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public static void ClearAllPools()
	{
		SqlConnectionFactory.SingletonInstance.ClearAllPools();
	}

	public static void ClearPool(SqlConnection connection)
	{
		ADP.CheckArgumentNull(connection, "connection");
		DbConnectionOptions userConnectionOptions = connection.UserConnectionOptions;
		if (userConnectionOptions != null)
		{
			SqlConnectionFactory.SingletonInstance.ClearPool(connection);
		}
	}

	private void CloseInnerConnection()
	{
		InnerConnection.CloseConnection(this, ConnectionFactory);
	}

	public override void Close()
	{
		ConnectionState state = State;
		Guid operationId = default(Guid);
		Guid clientConnectionId = default(Guid);
		if (state != ConnectionState.Closed)
		{
			operationId = s_diagnosticListener.WriteConnectionCloseBefore(this, "Close");
			clientConnectionId = ClientConnectionId;
		}
		SqlStatistics statistics = null;
		Exception ex = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			Task currentReconnectionTask = _currentReconnectionTask;
			if (currentReconnectionTask != null && !currentReconnectionTask.IsCompleted)
			{
				_reconnectionCancellationSource?.Cancel();
				AsyncHelper.WaitForCompletion(currentReconnectionTask, 0, null, rethrowExceptions: false);
				if (State != ConnectionState.Open)
				{
					OnStateChange(DbConnectionInternal.StateChangeClosed);
				}
			}
			CancelOpenAndWait();
			CloseInnerConnection();
			GC.SuppressFinalize(this);
			if (Statistics != null)
			{
				ADP.TimerCurrent(out _statistics._closeTimestamp);
			}
		}
		catch (Exception ex2)
		{
			ex = ex2;
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			if (state != ConnectionState.Closed)
			{
				if (ex != null)
				{
					s_diagnosticListener.WriteConnectionCloseError(operationId, clientConnectionId, this, ex, "Close");
				}
				else
				{
					s_diagnosticListener.WriteConnectionCloseAfter(operationId, clientConnectionId, this, "Close");
				}
			}
		}
	}

	public new SqlCommand CreateCommand()
	{
		return new SqlCommand(null, this);
	}

	private void DisposeMe(bool disposing)
	{
		_credential = null;
		_accessToken = null;
		if (!disposing && InnerConnection is SqlInternalConnectionTds sqlInternalConnectionTds && !sqlInternalConnectionTds.ConnectionOptions.Pooling)
		{
			TdsParser parser = sqlInternalConnectionTds.Parser;
			if (parser != null && parser._physicalStateObj != null)
			{
				parser._physicalStateObj.DecrementPendingCallbacks(release: false);
			}
		}
	}

	public override void Open()
	{
		Guid operationId = s_diagnosticListener.WriteConnectionOpenBefore(this, "Open");
		PrepareStatisticsForNewConnection();
		SqlStatistics statistics = null;
		Exception ex = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			if (!TryOpen(null))
			{
				throw ADP.InternalError(ADP.InternalErrorCode.SynchronousConnectReturnedPending);
			}
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
				s_diagnosticListener.WriteConnectionOpenError(operationId, this, ex, "Open");
			}
			else
			{
				s_diagnosticListener.WriteConnectionOpenAfter(operationId, this, "Open");
			}
		}
	}

	internal void RegisterWaitingForReconnect(Task waitingTask)
	{
		if (!((SqlConnectionString)ConnectionOptions).MARS)
		{
			Interlocked.CompareExchange(ref _asyncWaitingForReconnection, waitingTask, null);
			if (_asyncWaitingForReconnection != waitingTask)
			{
				throw SQL.MARSUnspportedOnConnection();
			}
		}
	}

	private async Task ReconnectAsync(int timeout)
	{
		_ = 1;
		try
		{
			long commandTimeoutExpiration = 0L;
			if (timeout > 0)
			{
				commandTimeoutExpiration = ADP.TimerCurrent() + ADP.TimerFromSeconds(timeout);
			}
			CancellationToken ctoken = (_reconnectionCancellationSource = new CancellationTokenSource()).Token;
			int retryCount = _connectRetryCount;
			for (int attempt = 0; attempt < retryCount; attempt++)
			{
				if (ctoken.IsCancellationRequested)
				{
					break;
				}
				try
				{
					try
					{
						ForceNewConnection = true;
						await OpenAsync(ctoken).ConfigureAwait(continueOnCapturedContext: false);
						_reconnectCount++;
						break;
					}
					finally
					{
						ForceNewConnection = false;
					}
				}
				catch (SqlException innerException)
				{
					if (attempt == retryCount - 1)
					{
						throw SQL.CR_AllAttemptsFailed(innerException, _originalConnectionId);
					}
					if (timeout > 0 && ADP.TimerRemaining(commandTimeoutExpiration) < ADP.TimerFromSeconds(ConnectRetryInterval))
					{
						throw SQL.CR_NextAttemptWillExceedQueryTimeout(innerException, _originalConnectionId);
					}
				}
				await Task.Delay(1000 * ConnectRetryInterval, ctoken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		finally
		{
			_recoverySessionData = null;
			_suppressStateChangeForReconnection = false;
		}
	}

	internal Task ValidateAndReconnect(Action beforeDisconnect, int timeout)
	{
		Task task = _currentReconnectionTask;
		while (task != null && task.IsCompleted)
		{
			Interlocked.CompareExchange(ref _currentReconnectionTask, null, task);
			task = _currentReconnectionTask;
		}
		if (task == null)
		{
			if (_connectRetryCount > 0)
			{
				SqlInternalConnectionTds openTdsConnection = GetOpenTdsConnection();
				if (openTdsConnection._sessionRecoveryAcknowledged && !openTdsConnection.Parser._physicalStateObj.ValidateSNIConnection())
				{
					if (openTdsConnection.Parser._sessionPool != null && openTdsConnection.Parser._sessionPool.ActiveSessionsCount > 0)
					{
						beforeDisconnect?.Invoke();
						OnError(SQL.CR_UnrecoverableClient(ClientConnectionId), breakConnection: true, null);
					}
					SessionData currentSessionData = openTdsConnection.CurrentSessionData;
					if (currentSessionData._unrecoverableStatesCount == 0)
					{
						bool flag = false;
						lock (_reconnectLock)
						{
							openTdsConnection.CheckEnlistedTransactionBinding();
							task = _currentReconnectionTask;
							if (task == null)
							{
								if (currentSessionData._unrecoverableStatesCount == 0)
								{
									_originalConnectionId = ClientConnectionId;
									_recoverySessionData = currentSessionData;
									beforeDisconnect?.Invoke();
									try
									{
										_suppressStateChangeForReconnection = true;
										openTdsConnection.DoomThisConnection();
									}
									catch (SqlException)
									{
									}
									task = (_currentReconnectionTask = Task.Run(() => ReconnectAsync(timeout)));
								}
							}
							else
							{
								flag = true;
							}
						}
						if (flag)
						{
							beforeDisconnect?.Invoke();
						}
					}
					else
					{
						beforeDisconnect?.Invoke();
						OnError(SQL.CR_UnrecoverableServer(ClientConnectionId), breakConnection: true, null);
					}
				}
			}
		}
		else
		{
			beforeDisconnect?.Invoke();
		}
		return task;
	}

	private void WaitForPendingReconnection()
	{
		Task currentReconnectionTask = _currentReconnectionTask;
		if (currentReconnectionTask != null && !currentReconnectionTask.IsCompleted)
		{
			AsyncHelper.WaitForCompletion(currentReconnectionTask, 0, null, rethrowExceptions: false);
		}
	}

	private void CancelOpenAndWait()
	{
		Tuple<TaskCompletionSource<DbConnectionInternal>, Task> currentCompletion = _currentCompletion;
		if (currentCompletion != null)
		{
			currentCompletion.Item1.TrySetCanceled();
			((IAsyncResult)currentCompletion.Item2).AsyncWaitHandle.WaitOne();
		}
	}

	public override Task OpenAsync(CancellationToken cancellationToken)
	{
		Guid operationId = s_diagnosticListener.WriteConnectionOpenBefore(this, "OpenAsync");
		PrepareStatisticsForNewConnection();
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			TaskCompletionSource<DbConnectionInternal> taskCompletionSource = new TaskCompletionSource<DbConnectionInternal>(ADP.GetCurrentTransaction());
			TaskCompletionSource<object> taskCompletionSource2 = new TaskCompletionSource<object>();
			if (s_diagnosticListener.IsEnabled("System.Data.SqlClient.WriteConnectionOpenAfter") || s_diagnosticListener.IsEnabled("System.Data.SqlClient.WriteConnectionOpenError"))
			{
				taskCompletionSource2.Task.ContinueWith(delegate(Task<object> t)
				{
					if (t.Exception != null)
					{
						s_diagnosticListener.WriteConnectionOpenError(operationId, this, t.Exception, "OpenAsync");
					}
					else
					{
						s_diagnosticListener.WriteConnectionOpenAfter(operationId, this, "OpenAsync");
					}
				}, TaskScheduler.Default);
			}
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource2.SetCanceled();
				return taskCompletionSource2.Task;
			}
			bool flag;
			try
			{
				flag = TryOpen(taskCompletionSource);
			}
			catch (Exception ex)
			{
				s_diagnosticListener.WriteConnectionOpenError(operationId, this, ex, "OpenAsync");
				taskCompletionSource2.SetException(ex);
				return taskCompletionSource2.Task;
			}
			if (flag)
			{
				taskCompletionSource2.SetResult(null);
				return taskCompletionSource2.Task;
			}
			CancellationTokenRegistration registration = default(CancellationTokenRegistration);
			if (cancellationToken.CanBeCanceled)
			{
				registration = cancellationToken.Register(delegate(object s)
				{
					((TaskCompletionSource<DbConnectionInternal>)s).TrySetCanceled();
				}, taskCompletionSource);
			}
			OpenAsyncRetry openAsyncRetry = new OpenAsyncRetry(this, taskCompletionSource, taskCompletionSource2, registration);
			_currentCompletion = new Tuple<TaskCompletionSource<DbConnectionInternal>, Task>(taskCompletionSource, taskCompletionSource2.Task);
			taskCompletionSource.Task.ContinueWith(openAsyncRetry.Retry, TaskScheduler.Default);
			return taskCompletionSource2.Task;
		}
		catch (Exception ex2)
		{
			s_diagnosticListener.WriteConnectionOpenError(operationId, this, ex2, "OpenAsync");
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public override DataTable GetSchema()
	{
		return GetSchema(DbMetaDataCollectionNames.MetaDataCollections, null);
	}

	public override DataTable GetSchema(string collectionName)
	{
		return GetSchema(collectionName, null);
	}

	public override DataTable GetSchema(string collectionName, string[] restrictionValues)
	{
		return InnerConnection.GetSchema(ConnectionFactory, PoolGroup, this, collectionName, restrictionValues);
	}

	private void PrepareStatisticsForNewConnection()
	{
		if (StatisticsEnabled || s_diagnosticListener.IsEnabled("System.Data.SqlClient.WriteCommandAfter") || s_diagnosticListener.IsEnabled("System.Data.SqlClient.WriteConnectionOpenAfter"))
		{
			if (_statistics == null)
			{
				_statistics = new SqlStatistics();
			}
			else
			{
				_statistics.ContinueOnNewConnection();
			}
		}
	}

	private bool TryOpen(TaskCompletionSource<DbConnectionInternal> retry)
	{
		SqlConnectionString sqlConnectionString = (SqlConnectionString)ConnectionOptions;
		_applyTransientFaultHandling = retry == null && sqlConnectionString != null && sqlConnectionString.ConnectRetryCount > 0;
		if (ForceNewConnection)
		{
			if (!InnerConnection.TryReplaceConnection(this, ConnectionFactory, retry, UserConnectionOptions))
			{
				return false;
			}
		}
		else if (!InnerConnection.TryOpenConnection(this, ConnectionFactory, retry, UserConnectionOptions))
		{
			return false;
		}
		SqlInternalConnectionTds sqlInternalConnectionTds = (SqlInternalConnectionTds)InnerConnection;
		if (!sqlInternalConnectionTds.ConnectionOptions.Pooling)
		{
			GC.ReRegisterForFinalize(this);
		}
		SqlStatistics statistics = _statistics;
		if (StatisticsEnabled || (s_diagnosticListener.IsEnabled("System.Data.SqlClient.WriteCommandAfter") && statistics != null))
		{
			ADP.TimerCurrent(out _statistics._openTimestamp);
			sqlInternalConnectionTds.Parser.Statistics = _statistics;
		}
		else
		{
			sqlInternalConnectionTds.Parser.Statistics = null;
			_statistics = null;
		}
		return true;
	}

	internal void ValidateConnectionForExecute(string method, SqlCommand command)
	{
		Task asyncWaitingForReconnection = _asyncWaitingForReconnection;
		if (asyncWaitingForReconnection != null)
		{
			if (!asyncWaitingForReconnection.IsCompleted)
			{
				throw SQL.MARSUnspportedOnConnection();
			}
			Interlocked.CompareExchange(ref _asyncWaitingForReconnection, null, asyncWaitingForReconnection);
		}
		if (_currentReconnectionTask != null)
		{
			Task currentReconnectionTask = _currentReconnectionTask;
			if (currentReconnectionTask != null && !currentReconnectionTask.IsCompleted)
			{
				return;
			}
		}
		GetOpenTdsConnection(method).ValidateConnectionForExecute(command);
	}

	internal static string FixupDatabaseTransactionName(string name)
	{
		if (!string.IsNullOrEmpty(name))
		{
			return SqlServerEscapeHelper.EscapeIdentifier(name);
		}
		return name;
	}

	internal void OnError(SqlException exception, bool breakConnection, Action<Action> wrapCloseInAction)
	{
		if (breakConnection && ConnectionState.Open == State)
		{
			if (wrapCloseInAction != null)
			{
				int capturedCloseCount = _closeCount;
				Action obj = delegate
				{
					if (capturedCloseCount == _closeCount)
					{
						Close();
					}
				};
				wrapCloseInAction(obj);
			}
			else
			{
				Close();
			}
		}
		if (exception.Class >= 11)
		{
			throw exception;
		}
		OnInfoMessage(new SqlInfoMessageEventArgs(exception));
	}

	internal SqlInternalConnectionTds GetOpenTdsConnection()
	{
		if (!(InnerConnection is SqlInternalConnectionTds result))
		{
			throw ADP.ClosedConnectionError();
		}
		return result;
	}

	internal SqlInternalConnectionTds GetOpenTdsConnection(string method)
	{
		if (!(InnerConnection is SqlInternalConnectionTds result))
		{
			throw ADP.OpenConnectionRequired(method, InnerConnection.State);
		}
		return result;
	}

	internal void OnInfoMessage(SqlInfoMessageEventArgs imevent)
	{
		OnInfoMessage(imevent, out var _);
	}

	internal void OnInfoMessage(SqlInfoMessageEventArgs imevent, out bool notified)
	{
		SqlInfoMessageEventHandler sqlInfoMessageEventHandler = this.InfoMessage;
		if (sqlInfoMessageEventHandler != null)
		{
			notified = true;
			try
			{
				sqlInfoMessageEventHandler(this, imevent);
				return;
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableOrSecurityExceptionType(e))
				{
					throw;
				}
				return;
			}
		}
		notified = false;
	}

	public static void ChangePassword(string connectionString, string newPassword)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			throw SQL.ChangePasswordArgumentMissing("newPassword");
		}
		if (string.IsNullOrEmpty(newPassword))
		{
			throw SQL.ChangePasswordArgumentMissing("newPassword");
		}
		if (128 < newPassword.Length)
		{
			throw ADP.InvalidArgumentLength("newPassword", 128);
		}
		SqlConnectionString sqlConnectionString = SqlConnectionFactory.FindSqlConnectionOptions(new SqlConnectionPoolKey(connectionString, null, null));
		if (sqlConnectionString.IntegratedSecurity)
		{
			throw SQL.ChangePasswordConflictsWithSSPI();
		}
		if (!string.IsNullOrEmpty(sqlConnectionString.AttachDBFilename))
		{
			throw SQL.ChangePasswordUseOfUnallowedKey("attachdbfilename");
		}
		ChangePassword(connectionString, sqlConnectionString, null, newPassword, null);
	}

	public static void ChangePassword(string connectionString, SqlCredential credential, SecureString newSecurePassword)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			throw SQL.ChangePasswordArgumentMissing("connectionString");
		}
		if (credential == null)
		{
			throw SQL.ChangePasswordArgumentMissing("credential");
		}
		if (newSecurePassword == null || newSecurePassword.Length == 0)
		{
			throw SQL.ChangePasswordArgumentMissing("newSecurePassword");
		}
		if (!newSecurePassword.IsReadOnly())
		{
			throw ADP.MustBeReadOnly("newSecurePassword");
		}
		if (128 < newSecurePassword.Length)
		{
			throw ADP.InvalidArgumentLength("newSecurePassword", 128);
		}
		SqlConnectionString sqlConnectionString = SqlConnectionFactory.FindSqlConnectionOptions(new SqlConnectionPoolKey(connectionString, null, null));
		if (!string.IsNullOrEmpty(sqlConnectionString.UserID) || !string.IsNullOrEmpty(sqlConnectionString.Password))
		{
			throw ADP.InvalidMixedArgumentOfSecureAndClearCredential();
		}
		if (sqlConnectionString.IntegratedSecurity)
		{
			throw SQL.ChangePasswordConflictsWithSSPI();
		}
		if (!string.IsNullOrEmpty(sqlConnectionString.AttachDBFilename))
		{
			throw SQL.ChangePasswordUseOfUnallowedKey("attachdbfilename");
		}
		ChangePassword(connectionString, sqlConnectionString, credential, null, newSecurePassword);
	}

	private static void ChangePassword(string connectionString, SqlConnectionString connectionOptions, SqlCredential credential, string newPassword, SecureString newSecurePassword)
	{
		SqlInternalConnectionTds sqlInternalConnectionTds = null;
		try
		{
			sqlInternalConnectionTds = new SqlInternalConnectionTds(null, connectionOptions, credential, null, newPassword, newSecurePassword, redirectedUserInstance: false);
		}
		finally
		{
			sqlInternalConnectionTds?.Dispose();
		}
		SqlConnectionPoolKey key = new SqlConnectionPoolKey(connectionString, null, null);
		SqlConnectionFactory.SingletonInstance.ClearPool(key);
	}

	internal void RegisterForConnectionCloseNotification<T>(ref Task<T> outerTask, object value, int tag)
	{
		outerTask = outerTask.ContinueWith(delegate(Task<T> task)
		{
			RemoveWeakReference(value);
			return task;
		}, TaskScheduler.Default).Unwrap();
	}

	public void ResetStatistics()
	{
		if (Statistics != null)
		{
			Statistics.Reset();
			if (ConnectionState.Open == State)
			{
				ADP.TimerCurrent(out _statistics._openTimestamp);
			}
		}
	}

	public IDictionary RetrieveStatistics()
	{
		if (Statistics != null)
		{
			UpdateStatistics();
			return Statistics.GetDictionary();
		}
		return new SqlStatistics().GetDictionary();
	}

	private void UpdateStatistics()
	{
		if (ConnectionState.Open == State)
		{
			ADP.TimerCurrent(out _statistics._closeTimestamp);
		}
		Statistics.UpdateStatistics();
	}

	object ICloneable.Clone()
	{
		return new SqlConnection(this);
	}

	private void CopyFrom(SqlConnection connection)
	{
		ADP.CheckArgumentNull(connection, "connection");
		_userConnectionOptions = connection.UserConnectionOptions;
		_poolGroup = connection.PoolGroup;
		if (DbConnectionClosedNeverOpened.SingletonInstance == connection._innerConnection)
		{
			_innerConnection = DbConnectionClosedNeverOpened.SingletonInstance;
		}
		else
		{
			_innerConnection = DbConnectionClosedPreviouslyOpened.SingletonInstance;
		}
	}

	private Assembly ResolveTypeAssembly(AssemblyName asmRef, bool throwOnError)
	{
		if (string.Compare(asmRef.Name, "Microsoft.SqlServer.Types", StringComparison.OrdinalIgnoreCase) == 0)
		{
			asmRef.Version = TypeSystemAssemblyVersion;
		}
		try
		{
			return Assembly.Load(asmRef);
		}
		catch (Exception e)
		{
			if (throwOnError || !ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
			return null;
		}
	}

	internal void CheckGetExtendedUDTInfo(SqlMetaDataPriv metaData, bool fThrow)
	{
		if (metaData.udtType == null)
		{
			metaData.udtType = Type.GetType(metaData.udtAssemblyQualifiedName, (AssemblyName asmRef) => ResolveTypeAssembly(asmRef, fThrow), null, fThrow);
			if (fThrow && metaData.udtType == null)
			{
				throw SQL.UDTUnexpectedResult(metaData.udtAssemblyQualifiedName);
			}
		}
	}

	internal object GetUdtValue(object value, SqlMetaDataPriv metaData, bool returnDBNull)
	{
		if (returnDBNull && ADP.IsNull(value))
		{
			return DBNull.Value;
		}
		if (ADP.IsNull(value))
		{
			return metaData.udtType.InvokeMember("Null", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty, null, null, new object[0], CultureInfo.InvariantCulture);
		}
		return SerializationHelperSql9.Deserialize(new MemoryStream((byte[])value), metaData.udtType);
	}

	internal byte[] GetBytes(object o)
	{
		Format format = Format.Native;
		int maxSize;
		return GetBytes(o, out format, out maxSize);
	}

	internal byte[] GetBytes(object o, out Format format, out int maxSize)
	{
		SqlUdtInfo infoFromType = GetInfoFromType(o.GetType());
		maxSize = infoFromType.MaxByteSize;
		format = infoFromType.SerializationFormat;
		if (maxSize < -1 || maxSize >= 65535)
		{
			throw new InvalidOperationException(o.GetType()?.ToString() + ": invalid Size");
		}
		using MemoryStream memoryStream = new MemoryStream((maxSize >= 0) ? maxSize : 0);
		SerializationHelperSql9.Serialize(memoryStream, o);
		return memoryStream.ToArray();
	}

	private SqlUdtInfo GetInfoFromType(Type t)
	{
		Type type = t;
		do
		{
			SqlUdtInfo sqlUdtInfo = SqlUdtInfo.TryGetFromType(t);
			if (sqlUdtInfo != null)
			{
				return sqlUdtInfo;
			}
			t = t.BaseType;
		}
		while (t != null);
		throw SQL.UDTInvalidSqlType(type.AssemblyQualifiedName);
	}

	public SqlConnection()
	{
		GC.SuppressFinalize(this);
		_innerConnection = DbConnectionClosedNeverOpened.SingletonInstance;
	}

	private string ConnectionString_Get()
	{
		bool shouldHidePassword = InnerConnection.ShouldHidePassword;
		DbConnectionOptions userConnectionOptions = UserConnectionOptions;
		if (userConnectionOptions == null)
		{
			return "";
		}
		return userConnectionOptions.UsersConnectionString(shouldHidePassword);
	}

	private void ConnectionString_Set(DbConnectionPoolKey key)
	{
		DbConnectionOptions userConnectionOptions = null;
		DbConnectionPoolGroup connectionPoolGroup = ConnectionFactory.GetConnectionPoolGroup(key, null, ref userConnectionOptions);
		DbConnectionInternal innerConnection = InnerConnection;
		bool flag = innerConnection.AllowSetConnectionString;
		if (flag)
		{
			flag = SetInnerConnectionFrom(DbConnectionClosedBusy.SingletonInstance, innerConnection);
			if (flag)
			{
				_userConnectionOptions = userConnectionOptions;
				_poolGroup = connectionPoolGroup;
				_innerConnection = DbConnectionClosedNeverOpened.SingletonInstance;
			}
		}
		if (!flag)
		{
			throw ADP.OpenConnectionPropertySet("ConnectionString", innerConnection.State);
		}
	}

	internal void Abort(Exception e)
	{
		DbConnectionInternal innerConnection = _innerConnection;
		if (ConnectionState.Open == innerConnection.State)
		{
			Interlocked.CompareExchange(ref _innerConnection, DbConnectionClosedPreviouslyOpened.SingletonInstance, innerConnection);
			innerConnection.DoomThisConnection();
		}
	}

	internal void AddWeakReference(object value, int tag)
	{
		InnerConnection.AddWeakReference(value, tag);
	}

	protected override DbCommand CreateDbCommand()
	{
		DbCommand dbCommand = ConnectionFactory.ProviderFactory.CreateCommand();
		dbCommand.Connection = this;
		return dbCommand;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_userConnectionOptions = null;
			_poolGroup = null;
			Close();
		}
		DisposeMe(disposing);
		base.Dispose(disposing);
	}

	private void RepairInnerConnection()
	{
		WaitForPendingReconnection();
		if (_connectRetryCount != 0 && InnerConnection is SqlInternalConnectionTds sqlInternalConnectionTds)
		{
			sqlInternalConnectionTds.ValidateConnectionForExecute(null);
			sqlInternalConnectionTds.GetSessionAndReconnectIfNeeded(this);
		}
	}

	public override void EnlistTransaction(Transaction transaction)
	{
		Transaction enlistedTransaction = InnerConnection.EnlistedTransaction;
		if (enlistedTransaction != null)
		{
			if (enlistedTransaction.Equals(transaction))
			{
				return;
			}
			if (enlistedTransaction.TransactionInformation.Status == TransactionStatus.Active)
			{
				throw ADP.TransactionPresent();
			}
		}
		RepairInnerConnection();
		InnerConnection.EnlistTransaction(transaction);
		GC.KeepAlive(this);
	}

	internal void NotifyWeakReference(int message)
	{
		InnerConnection.NotifyWeakReference(message);
	}

	internal void PermissionDemand()
	{
		DbConnectionOptions dbConnectionOptions = PoolGroup?.ConnectionOptions;
		if (dbConnectionOptions == null || dbConnectionOptions.IsEmpty)
		{
			throw ADP.NoConnectionString();
		}
		_ = UserConnectionOptions;
	}

	internal void RemoveWeakReference(object value)
	{
		InnerConnection.RemoveWeakReference(value);
	}

	internal void SetInnerConnectionEvent(DbConnectionInternal to)
	{
		ConnectionState connectionState = _innerConnection.State & ConnectionState.Open;
		ConnectionState connectionState2 = to.State & ConnectionState.Open;
		if (connectionState != connectionState2 && connectionState2 == ConnectionState.Closed)
		{
			_closeCount++;
		}
		_innerConnection = to;
		if (connectionState == ConnectionState.Closed && ConnectionState.Open == connectionState2)
		{
			OnStateChange(DbConnectionInternal.StateChangeOpen);
		}
		else if (ConnectionState.Open == connectionState && connectionState2 == ConnectionState.Closed)
		{
			OnStateChange(DbConnectionInternal.StateChangeClosed);
		}
		else if (connectionState != connectionState2)
		{
			OnStateChange(new StateChangeEventArgs(connectionState, connectionState2));
		}
	}

	internal bool SetInnerConnectionFrom(DbConnectionInternal to, DbConnectionInternal from)
	{
		return from == Interlocked.CompareExchange(ref _innerConnection, to, from);
	}

	internal void SetInnerConnectionTo(DbConnectionInternal to)
	{
		_innerConnection = to;
	}
}
