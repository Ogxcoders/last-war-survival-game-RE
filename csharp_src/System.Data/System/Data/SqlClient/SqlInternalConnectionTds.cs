using System.Collections.Generic;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Globalization;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace System.Data.SqlClient;

internal sealed class SqlInternalConnectionTds : SqlInternalConnection, IDisposable
{
	internal class SyncAsyncLock
	{
		private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

		internal bool CanBeReleasedFromAnyThread => _semaphore.CurrentCount == 0;

		internal void Wait(bool canReleaseFromAnyThread)
		{
			Monitor.Enter(_semaphore);
			if (canReleaseFromAnyThread || _semaphore.CurrentCount == 0)
			{
				_semaphore.Wait();
				if (canReleaseFromAnyThread)
				{
					Monitor.Exit(_semaphore);
				}
				else
				{
					_semaphore.Release();
				}
			}
		}

		internal void Wait(bool canReleaseFromAnyThread, int timeout, ref bool lockTaken)
		{
			lockTaken = false;
			bool lockTaken2 = false;
			try
			{
				Monitor.TryEnter(_semaphore, timeout, ref lockTaken2);
				if (!lockTaken2)
				{
					return;
				}
				if (canReleaseFromAnyThread || _semaphore.CurrentCount == 0)
				{
					if (_semaphore.Wait(timeout))
					{
						if (canReleaseFromAnyThread)
						{
							Monitor.Exit(_semaphore);
							lockTaken2 = false;
						}
						else
						{
							_semaphore.Release();
						}
						lockTaken = true;
					}
				}
				else
				{
					lockTaken = true;
				}
			}
			finally
			{
				if (!lockTaken && lockTaken2)
				{
					Monitor.Exit(_semaphore);
				}
			}
		}

		internal void Release()
		{
			if (_semaphore.CurrentCount == 0)
			{
				_semaphore.Release();
			}
			else
			{
				Monitor.Exit(_semaphore);
			}
		}

		internal bool ThreadMayHaveLock()
		{
			if (!Monitor.IsEntered(_semaphore))
			{
				return _semaphore.CurrentCount == 0;
			}
			return true;
		}
	}

	private readonly SqlConnectionPoolGroupProviderInfo _poolGroupProviderInfo;

	private TdsParser _parser;

	private SqlLoginAck _loginAck;

	private SqlCredential _credential;

	private FederatedAuthenticationFeatureExtensionData? _fedAuthFeatureExtensionData;

	private bool _sessionRecoveryRequested;

	internal bool _sessionRecoveryAcknowledged;

	internal SessionData _currentSessionData;

	private SessionData _recoverySessionData;

	internal bool _fedAuthRequired;

	internal bool _federatedAuthenticationRequested;

	internal bool _federatedAuthenticationAcknowledged;

	internal byte[] _accessTokenInBytes;

	private static readonly HashSet<int> s_transientErrors = new HashSet<int> { 4060, 10928, 10929, 40197, 40501, 40613 };

	private bool _fConnectionOpen;

	private bool _fResetConnection;

	private string _originalDatabase;

	private string _currentFailoverPartner;

	private string _originalLanguage;

	private string _currentLanguage;

	private int _currentPacketSize;

	private int _asyncCommandCount;

	private string _instanceName = string.Empty;

	private DbConnectionPoolIdentity _identity;

	internal SyncAsyncLock _parserLock = new SyncAsyncLock();

	private int _threadIdOwningParserLock = -1;

	private SqlConnectionTimeoutErrorInternal _timeoutErrorInternal;

	internal Guid _clientConnectionId = Guid.Empty;

	private RoutingInfo _routingInfo;

	private Guid _originalClientConnectionId = Guid.Empty;

	private string _routingDestination;

	private readonly TimeoutTimer _timeout;

	internal SessionData CurrentSessionData
	{
		get
		{
			if (_currentSessionData != null)
			{
				_currentSessionData._database = base.CurrentDatabase;
				_currentSessionData._language = _currentLanguage;
			}
			return _currentSessionData;
		}
	}

	internal SqlConnectionTimeoutErrorInternal TimeoutErrorInternal => _timeoutErrorInternal;

	internal Guid ClientConnectionId => _clientConnectionId;

	internal Guid OriginalClientConnectionId => _originalClientConnectionId;

	internal string RoutingDestination => _routingDestination;

	internal override SqlInternalTransaction CurrentTransaction => _parser.CurrentTransaction;

	internal override SqlInternalTransaction AvailableInternalTransaction
	{
		get
		{
			if (!_parser._fResetConnection)
			{
				return CurrentTransaction;
			}
			return null;
		}
	}

	internal override SqlInternalTransaction PendingTransaction => _parser.PendingTransaction;

	internal DbConnectionPoolIdentity Identity => _identity;

	internal string InstanceName => _instanceName;

	internal override bool IsLockedForBulkCopy
	{
		get
		{
			if (!Parser.MARSOn)
			{
				return Parser._physicalStateObj.BcpLock;
			}
			return false;
		}
	}

	protected internal override bool IsNonPoolableTransactionRoot
	{
		get
		{
			if (IsTransactionRoot)
			{
				if (IsKatmaiOrNewer)
				{
					return base.Pool == null;
				}
				return true;
			}
			return false;
		}
	}

	internal override bool IsKatmaiOrNewer => _parser.IsKatmaiOrNewer;

	internal int PacketSize => _currentPacketSize;

	internal TdsParser Parser => _parser;

	internal string ServerProvidedFailOverPartner => _currentFailoverPartner;

	internal SqlConnectionPoolGroupProviderInfo PoolGroupProviderInfo => _poolGroupProviderInfo;

	protected override bool ReadyToPrepareTransaction => FindLiveReader(null) == null;

	public override string ServerVersion => string.Format(null, "{0:00}.{1:00}.{2:0000}", _loginAck.majorVersion, (short)_loginAck.minorVersion, _loginAck.buildNum);

	protected override bool UnbindOnTransactionCompletion => false;

	internal bool IgnoreEnvChange => _routingInfo != null;

	internal bool ThreadHasParserLockForClose
	{
		get
		{
			return _threadIdOwningParserLock == Thread.CurrentThread.ManagedThreadId;
		}
		set
		{
			if (value)
			{
				_threadIdOwningParserLock = Thread.CurrentThread.ManagedThreadId;
			}
			else if (_threadIdOwningParserLock == Thread.CurrentThread.ManagedThreadId)
			{
				_threadIdOwningParserLock = -1;
			}
		}
	}

	internal SqlInternalConnectionTds(DbConnectionPoolIdentity identity, SqlConnectionString connectionOptions, SqlCredential credential, object providerInfo, string newPassword, SecureString newSecurePassword, bool redirectedUserInstance, SqlConnectionString userConnectionOptions = null, SessionData reconnectSessionData = null, bool applyTransientFaultHandling = false, string accessToken = null)
		: base(connectionOptions)
	{
		if (connectionOptions.ConnectRetryCount > 0)
		{
			_recoverySessionData = reconnectSessionData;
			if (reconnectSessionData == null)
			{
				_currentSessionData = new SessionData();
			}
			else
			{
				_currentSessionData = new SessionData(_recoverySessionData);
				_originalDatabase = _recoverySessionData._initialDatabase;
				_originalLanguage = _recoverySessionData._initialLanguage;
			}
		}
		if (accessToken != null)
		{
			_accessTokenInBytes = Encoding.Unicode.GetBytes(accessToken);
		}
		_identity = identity;
		_poolGroupProviderInfo = (SqlConnectionPoolGroupProviderInfo)providerInfo;
		_fResetConnection = connectionOptions.ConnectionReset;
		if (_fResetConnection && _recoverySessionData == null)
		{
			_originalDatabase = connectionOptions.InitialCatalog;
			_originalLanguage = connectionOptions.CurrentLanguage;
		}
		_timeoutErrorInternal = new SqlConnectionTimeoutErrorInternal();
		_credential = credential;
		_parserLock.Wait(canReleaseFromAnyThread: false);
		ThreadHasParserLockForClose = true;
		try
		{
			_timeout = TimeoutTimer.StartSecondsTimeout(connectionOptions.ConnectTimeout);
			int num = ((!applyTransientFaultHandling) ? 1 : (connectionOptions.ConnectRetryCount + 1));
			int num2 = connectionOptions.ConnectRetryInterval * 1000;
			for (int i = 0; i < num; i++)
			{
				try
				{
					OpenLoginEnlist(_timeout, connectionOptions, credential, newPassword, newSecurePassword, redirectedUserInstance);
					break;
				}
				catch (SqlException ex)
				{
					if (i + 1 == num || !applyTransientFaultHandling || _timeout.IsExpired || _timeout.MillisecondsRemaining < num2 || !IsTransientError(ex))
					{
						throw ex;
					}
					Thread.Sleep(num2);
				}
			}
		}
		finally
		{
			ThreadHasParserLockForClose = false;
			_parserLock.Release();
		}
	}

	private bool IsTransientError(SqlException exc)
	{
		if (exc == null)
		{
			return false;
		}
		foreach (SqlError error in exc.Errors)
		{
			if (s_transientErrors.Contains(error.Number))
			{
				return true;
			}
		}
		return false;
	}

	protected override void ChangeDatabaseInternal(string database)
	{
		database = SqlConnection.FixupDatabaseTransactionName(database);
		_parser.TdsExecuteSQLBatch("use " + database, base.ConnectionOptions.ConnectTimeout, null, _parser._physicalStateObj, sync: true);
		_parser.Run(RunBehavior.UntilDone, null, null, null, _parser._physicalStateObj);
	}

	public override void Dispose()
	{
		try
		{
			Interlocked.Exchange(ref _parser, null)?.Disconnect();
		}
		finally
		{
			_loginAck = null;
			_fConnectionOpen = false;
		}
		base.Dispose();
	}

	internal override void ValidateConnectionForExecute(SqlCommand command)
	{
		TdsParser parser = _parser;
		if (parser == null || parser.State == TdsParserState.Broken || parser.State == TdsParserState.Closed)
		{
			throw ADP.ClosedConnectionError();
		}
		SqlDataReader sqlDataReader = null;
		if (parser.MARSOn)
		{
			if (command != null)
			{
				sqlDataReader = FindLiveReader(command);
			}
		}
		else
		{
			if (_asyncCommandCount > 0)
			{
				throw SQL.MARSUnspportedOnConnection();
			}
			sqlDataReader = FindLiveReader(null);
		}
		if (sqlDataReader != null)
		{
			throw ADP.OpenReaderExists();
		}
		if (!parser.MARSOn && parser._physicalStateObj._pendingData)
		{
			parser.DrainData(parser._physicalStateObj);
		}
		parser.RollbackOrphanedAPITransactions();
	}

	internal void CheckEnlistedTransactionBinding()
	{
		Transaction enlistedTransaction = base.EnlistedTransaction;
		if (!(enlistedTransaction != null))
		{
			return;
		}
		if (base.ConnectionOptions.TransactionBinding == SqlConnectionString.TransactionBindingEnum.ExplicitUnbind)
		{
			Transaction current = Transaction.Current;
			if (enlistedTransaction.TransactionInformation.Status != TransactionStatus.Active || !enlistedTransaction.Equals(current))
			{
				throw ADP.TransactionConnectionMismatch();
			}
		}
		else if (enlistedTransaction.TransactionInformation.Status != TransactionStatus.Active)
		{
			if (!base.EnlistedTransactionDisposed)
			{
				throw ADP.TransactionCompletedButNotDisposed();
			}
			DetachTransaction(enlistedTransaction, isExplicitlyReleasing: true);
		}
	}

	internal override bool IsConnectionAlive(bool throwOnException)
	{
		return _parser._physicalStateObj.IsConnectionAlive(throwOnException);
	}

	protected override void Activate(Transaction transaction)
	{
		if (null != transaction)
		{
			if (base.ConnectionOptions.Enlist)
			{
				Enlist(transaction);
			}
		}
		else
		{
			Enlist(null);
		}
	}

	protected override void InternalDeactivate()
	{
		if (_asyncCommandCount != 0)
		{
			DoomThisConnection();
		}
		if (!IsNonPoolableTransactionRoot && _parser != null)
		{
			_parser.Deactivate(base.IsConnectionDoomed);
			if (!base.IsConnectionDoomed)
			{
				ResetConnection();
			}
		}
	}

	private void ResetConnection()
	{
		if (_fResetConnection)
		{
			_parser.PrepareResetConnection(IsTransactionRoot && !IsNonPoolableTransactionRoot);
			base.CurrentDatabase = _originalDatabase;
			_currentLanguage = _originalLanguage;
		}
	}

	internal void DecrementAsyncCount()
	{
		Interlocked.Decrement(ref _asyncCommandCount);
	}

	internal void IncrementAsyncCount()
	{
		Interlocked.Increment(ref _asyncCommandCount);
	}

	internal override void DisconnectTransaction(SqlInternalTransaction internalTransaction)
	{
		Parser?.DisconnectTransaction(internalTransaction);
	}

	internal void ExecuteTransaction(TransactionRequest transactionRequest, string name, IsolationLevel iso)
	{
		ExecuteTransaction(transactionRequest, name, iso, null, isDelegateControlRequest: false);
	}

	internal override void ExecuteTransaction(TransactionRequest transactionRequest, string name, IsolationLevel iso, SqlInternalTransaction internalTransaction, bool isDelegateControlRequest)
	{
		if (base.IsConnectionDoomed)
		{
			if (transactionRequest != TransactionRequest.Rollback && transactionRequest != TransactionRequest.IfRollback)
			{
				throw SQL.ConnectionDoomed();
			}
			return;
		}
		if ((transactionRequest == TransactionRequest.Commit || transactionRequest == TransactionRequest.Rollback || transactionRequest == TransactionRequest.IfRollback) && !Parser.MARSOn && Parser._physicalStateObj.BcpLock)
		{
			throw SQL.ConnectionLockedForBcpEvent();
		}
		string transactionName = ((name == null) ? string.Empty : name);
		ExecuteTransactionYukon(transactionRequest, transactionName, iso, internalTransaction, isDelegateControlRequest);
	}

	internal void ExecuteTransactionYukon(TransactionRequest transactionRequest, string transactionName, IsolationLevel iso, SqlInternalTransaction internalTransaction, bool isDelegateControlRequest)
	{
		TdsEnums.TransactionManagerRequestType request = TdsEnums.TransactionManagerRequestType.Begin;
		TdsEnums.TransactionManagerIsolationLevel transactionManagerIsolationLevel = TdsEnums.TransactionManagerIsolationLevel.ReadCommitted;
		transactionManagerIsolationLevel = iso switch
		{
			IsolationLevel.Unspecified => TdsEnums.TransactionManagerIsolationLevel.Unspecified, 
			IsolationLevel.ReadCommitted => TdsEnums.TransactionManagerIsolationLevel.ReadCommitted, 
			IsolationLevel.ReadUncommitted => TdsEnums.TransactionManagerIsolationLevel.ReadUncommitted, 
			IsolationLevel.RepeatableRead => TdsEnums.TransactionManagerIsolationLevel.RepeatableRead, 
			IsolationLevel.Serializable => TdsEnums.TransactionManagerIsolationLevel.Serializable, 
			IsolationLevel.Snapshot => TdsEnums.TransactionManagerIsolationLevel.Snapshot, 
			IsolationLevel.Chaos => throw SQL.NotSupportedIsolationLevel(iso), 
			_ => throw ADP.InvalidIsolationLevel(iso), 
		};
		TdsParserStateObject tdsParserStateObject = _parser._physicalStateObj;
		TdsParser parser = _parser;
		bool flag = false;
		bool releaseConnectionLock = false;
		if (!ThreadHasParserLockForClose)
		{
			_parserLock.Wait(canReleaseFromAnyThread: false);
			ThreadHasParserLockForClose = true;
			releaseConnectionLock = true;
		}
		try
		{
			switch (transactionRequest)
			{
			case TransactionRequest.Begin:
				request = TdsEnums.TransactionManagerRequestType.Begin;
				break;
			case TransactionRequest.Promote:
				request = TdsEnums.TransactionManagerRequestType.Promote;
				break;
			case TransactionRequest.Commit:
				request = TdsEnums.TransactionManagerRequestType.Commit;
				break;
			case TransactionRequest.Rollback:
			case TransactionRequest.IfRollback:
				request = TdsEnums.TransactionManagerRequestType.Rollback;
				break;
			case TransactionRequest.Save:
				request = TdsEnums.TransactionManagerRequestType.Save;
				break;
			}
			if ((internalTransaction?.RestoreBrokenConnection ?? false) && releaseConnectionLock)
			{
				Task task = internalTransaction.Parent.Connection.ValidateAndReconnect(delegate
				{
					ThreadHasParserLockForClose = false;
					_parserLock.Release();
					releaseConnectionLock = false;
				}, 0);
				if (task != null)
				{
					AsyncHelper.WaitForCompletion(task, 0);
					internalTransaction.ConnectionHasBeenRestored = true;
					return;
				}
			}
			if (internalTransaction != null && internalTransaction.IsDelegated)
			{
				if (_parser.MARSOn)
				{
					tdsParserStateObject = _parser.GetSession(this);
					flag = true;
				}
				else
				{
					_ = internalTransaction.OpenResultsCount;
				}
			}
			_parser.TdsExecuteTransactionManagerRequest(null, request, transactionName, transactionManagerIsolationLevel, base.ConnectionOptions.ConnectTimeout, internalTransaction, tdsParserStateObject, isDelegateControlRequest);
		}
		finally
		{
			if (flag)
			{
				parser.PutSession(tdsParserStateObject);
			}
			if (releaseConnectionLock)
			{
				ThreadHasParserLockForClose = false;
				_parserLock.Release();
			}
		}
	}

	internal override void DelegatedTransactionEnded()
	{
		base.DelegatedTransactionEnded();
	}

	protected override byte[] GetDTCAddress()
	{
		return _parser.GetDTCAddress(base.ConnectionOptions.ConnectTimeout, _parser.GetSession(this));
	}

	protected override void PropagateTransactionCookie(byte[] cookie)
	{
		_parser.PropagateDistributedTransaction(cookie, base.ConnectionOptions.ConnectTimeout, _parser._physicalStateObj);
	}

	private void CompleteLogin(bool enlistOK)
	{
		_parser.Run(RunBehavior.UntilDone, null, null, null, _parser._physicalStateObj);
		if (_routingInfo == null)
		{
			if (_federatedAuthenticationRequested && !_federatedAuthenticationAcknowledged)
			{
				throw SQL.ParsingError(ParsingErrorState.FedAuthNotAcknowledged);
			}
			if (!_sessionRecoveryAcknowledged)
			{
				_currentSessionData = null;
				if (_recoverySessionData != null)
				{
					throw SQL.CR_NoCRAckAtReconnection(this);
				}
			}
			if (_currentSessionData != null && _recoverySessionData == null)
			{
				_currentSessionData._initialDatabase = base.CurrentDatabase;
				_currentSessionData._initialCollation = _currentSessionData._collation;
				_currentSessionData._initialLanguage = _currentLanguage;
			}
			bool flag = _parser.EncryptionOptions == EncryptionOptions.ON;
			if (_recoverySessionData != null && _recoverySessionData._encrypted != flag)
			{
				throw SQL.CR_EncryptionChanged(this);
			}
			if (_currentSessionData != null)
			{
				_currentSessionData._encrypted = flag;
			}
			_recoverySessionData = null;
		}
		_parser._physicalStateObj.SniContext = SniContext.Snix_EnableMars;
		_parser.EnableMars();
		_fConnectionOpen = true;
		if (enlistOK && base.ConnectionOptions.Enlist)
		{
			_parser._physicalStateObj.SniContext = SniContext.Snix_AutoEnlist;
			Transaction currentTransaction = ADP.GetCurrentTransaction();
			Enlist(currentTransaction);
		}
		_parser._physicalStateObj.SniContext = SniContext.Snix_Login;
	}

	private void Login(ServerInfo server, TimeoutTimer timeout, string newPassword, SecureString newSecurePassword)
	{
		SqlLogin sqlLogin = new SqlLogin();
		base.CurrentDatabase = server.ResolvedDatabaseName;
		_currentPacketSize = base.ConnectionOptions.PacketSize;
		_currentLanguage = base.ConnectionOptions.CurrentLanguage;
		int timeout2 = 0;
		if (!timeout.IsInfinite)
		{
			long num = timeout.MillisecondsRemaining / 1000;
			if (int.MaxValue > num)
			{
				timeout2 = (int)num;
			}
		}
		sqlLogin.timeout = timeout2;
		sqlLogin.userInstance = base.ConnectionOptions.UserInstance;
		sqlLogin.hostName = base.ConnectionOptions.ObtainWorkstationId();
		sqlLogin.userName = base.ConnectionOptions.UserID;
		sqlLogin.password = base.ConnectionOptions.Password;
		sqlLogin.applicationName = base.ConnectionOptions.ApplicationName;
		sqlLogin.language = _currentLanguage;
		if (!sqlLogin.userInstance)
		{
			sqlLogin.database = base.CurrentDatabase;
			sqlLogin.attachDBFilename = base.ConnectionOptions.AttachDBFilename;
		}
		sqlLogin.serverName = server.UserServerName;
		sqlLogin.useReplication = base.ConnectionOptions.Replication;
		sqlLogin.useSSPI = base.ConnectionOptions.IntegratedSecurity;
		sqlLogin.packetSize = _currentPacketSize;
		sqlLogin.newPassword = newPassword;
		sqlLogin.readOnlyIntent = base.ConnectionOptions.ApplicationIntent == ApplicationIntent.ReadOnly;
		sqlLogin.credential = _credential;
		if (newSecurePassword != null)
		{
			sqlLogin.newSecurePassword = newSecurePassword;
		}
		TdsEnums.FeatureExtension featureExtension = TdsEnums.FeatureExtension.None;
		if (base.ConnectionOptions.ConnectRetryCount > 0)
		{
			featureExtension |= TdsEnums.FeatureExtension.SessionRecovery;
			_sessionRecoveryRequested = true;
		}
		if (_accessTokenInBytes != null)
		{
			featureExtension |= TdsEnums.FeatureExtension.FedAuth;
			_fedAuthFeatureExtensionData = new FederatedAuthenticationFeatureExtensionData
			{
				libraryType = TdsEnums.FedAuthLibrary.SecurityToken,
				fedAuthRequiredPreLoginResponse = _fedAuthRequired,
				accessToken = _accessTokenInBytes
			};
			_federatedAuthenticationRequested = true;
		}
		featureExtension |= TdsEnums.FeatureExtension.GlobalTransactions;
		_parser.TdsLogin(sqlLogin, featureExtension, _recoverySessionData, _fedAuthFeatureExtensionData);
	}

	private void LoginFailure()
	{
		if (_parser != null)
		{
			_parser.Disconnect();
		}
	}

	private void OpenLoginEnlist(TimeoutTimer timeout, SqlConnectionString connectionOptions, SqlCredential credential, string newPassword, SecureString newSecurePassword, bool redirectedUserInstance)
	{
		ServerInfo serverInfo = new ServerInfo(connectionOptions);
		bool flag;
		string failoverPartner;
		if (PoolGroupProviderInfo != null)
		{
			flag = PoolGroupProviderInfo.UseFailoverPartner;
			failoverPartner = PoolGroupProviderInfo.FailoverPartner;
		}
		else
		{
			flag = false;
			failoverPartner = base.ConnectionOptions.FailoverPartner;
		}
		_timeoutErrorInternal.SetInternalSourceType(flag ? SqlConnectionInternalSourceType.Failover : SqlConnectionInternalSourceType.Principle);
		bool flag2 = !string.IsNullOrEmpty(failoverPartner);
		try
		{
			_timeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.PreLoginBegin);
			if (flag2)
			{
				_timeoutErrorInternal.SetFailoverScenario(useFailoverServer: true);
				LoginWithFailover(flag, serverInfo, failoverPartner, newPassword, newSecurePassword, redirectedUserInstance, connectionOptions, credential, timeout);
			}
			else
			{
				_timeoutErrorInternal.SetFailoverScenario(useFailoverServer: false);
				LoginNoFailover(serverInfo, newPassword, newSecurePassword, redirectedUserInstance, connectionOptions, credential, timeout);
			}
			_timeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.PostLogin);
		}
		catch (Exception e)
		{
			if (ADP.IsCatchableExceptionType(e))
			{
				LoginFailure();
			}
			throw;
		}
		_timeoutErrorInternal.SetAllCompleteMarker();
	}

	private bool IsDoNotRetryConnectError(SqlException exc)
	{
		if (18456 != exc.Number && 18488 != exc.Number && 1346 != exc.Number)
		{
			return exc._doNotReconnect;
		}
		return true;
	}

	private void LoginNoFailover(ServerInfo serverInfo, string newPassword, SecureString newSecurePassword, bool redirectedUserInstance, SqlConnectionString connectionOptions, SqlCredential credential, TimeoutTimer timeout)
	{
		int num = 0;
		ServerInfo serverInfo2 = serverInfo;
		int num2 = 100;
		ResolveExtendedServerName(serverInfo, !redirectedUserInstance, connectionOptions);
		long num3 = 0L;
		if (connectionOptions.MultiSubnetFailover)
		{
			num3 = ((!timeout.IsInfinite) ? checked((long)(0.08f * (float)timeout.MillisecondsRemaining)) : 1200);
		}
		int num4 = 0;
		TimeoutTimer timeoutTimer = null;
		while (true)
		{
			if (connectionOptions.MultiSubnetFailover)
			{
				num4++;
				long num5 = checked(num3 * num4);
				long millisecondsRemaining = timeout.MillisecondsRemaining;
				if (num5 > millisecondsRemaining)
				{
					num5 = millisecondsRemaining;
				}
				timeoutTimer = TimeoutTimer.StartMillisecondsTimeout(num5);
			}
			if (_parser != null)
			{
				_parser.Disconnect();
			}
			_parser = new TdsParser(base.ConnectionOptions.MARS, base.ConnectionOptions.Asynchronous);
			try
			{
				AttemptOneLogin(serverInfo, newPassword, newSecurePassword, !connectionOptions.MultiSubnetFailover, connectionOptions.MultiSubnetFailover ? timeoutTimer : timeout);
				if (connectionOptions.MultiSubnetFailover && ServerProvidedFailOverPartner != null)
				{
					throw SQL.MultiSubnetFailoverWithFailoverPartner(serverProvidedFailoverPartner: true, this);
				}
				if (_routingInfo != null)
				{
					if (num > 0)
					{
						throw SQL.ROR_RecursiveRoutingNotSupported(this);
					}
					if (timeout.IsExpired)
					{
						throw SQL.ROR_TimeoutAfterRoutingInfo(this);
					}
					serverInfo = new ServerInfo(base.ConnectionOptions, _routingInfo, serverInfo.ResolvedServerName);
					_timeoutErrorInternal.SetInternalSourceType(SqlConnectionInternalSourceType.RoutingDestination);
					_originalClientConnectionId = _clientConnectionId;
					_routingDestination = serverInfo.UserServerName;
					_currentPacketSize = base.ConnectionOptions.PacketSize;
					_currentLanguage = (_originalLanguage = base.ConnectionOptions.CurrentLanguage);
					base.CurrentDatabase = (_originalDatabase = base.ConnectionOptions.InitialCatalog);
					_currentFailoverPartner = null;
					_instanceName = string.Empty;
					num++;
					continue;
				}
			}
			catch (SqlException exc)
			{
				if (_parser == null || _parser.State != TdsParserState.Closed || IsDoNotRetryConnectError(exc) || timeout.IsExpired)
				{
					throw;
				}
				if (timeout.MillisecondsRemaining <= num2)
				{
					throw;
				}
				goto IL_01f2;
			}
			break;
			IL_01f2:
			if (ServerProvidedFailOverPartner != null)
			{
				if (connectionOptions.MultiSubnetFailover)
				{
					throw SQL.MultiSubnetFailoverWithFailoverPartner(serverProvidedFailoverPartner: true, this);
				}
				_timeoutErrorInternal.ResetAndRestartPhase();
				_timeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.PreLoginBegin);
				_timeoutErrorInternal.SetInternalSourceType(SqlConnectionInternalSourceType.Failover);
				_timeoutErrorInternal.SetFailoverScenario(useFailoverServer: true);
				LoginWithFailover(useFailoverHost: true, serverInfo, ServerProvidedFailOverPartner, newPassword, newSecurePassword, redirectedUserInstance, connectionOptions, credential, timeout);
				return;
			}
			Thread.Sleep(num2);
			num2 = ((num2 < 500) ? (num2 * 2) : 1000);
		}
		if (PoolGroupProviderInfo != null)
		{
			PoolGroupProviderInfo.FailoverCheck(this, actualUseFailoverPartner: false, connectionOptions, ServerProvidedFailOverPartner);
		}
		base.CurrentDataSource = serverInfo2.UserServerName;
	}

	private void LoginWithFailover(bool useFailoverHost, ServerInfo primaryServerInfo, string failoverHost, string newPassword, SecureString newSecurePassword, bool redirectedUserInstance, SqlConnectionString connectionOptions, SqlCredential credential, TimeoutTimer timeout)
	{
		int num = 100;
		ServerInfo serverInfo = new ServerInfo(connectionOptions, failoverHost);
		ResolveExtendedServerName(primaryServerInfo, !redirectedUserInstance, connectionOptions);
		if (ServerProvidedFailOverPartner == null)
		{
			ResolveExtendedServerName(serverInfo, !redirectedUserInstance && failoverHost != primaryServerInfo.UserServerName, connectionOptions);
		}
		long num2 = checked((!timeout.IsInfinite) ? ((long)(0.08f * (float)timeout.MillisecondsRemaining)) : ((long)(0.08f * (float)ADP.TimerFromSeconds(15))));
		int num3 = 0;
		while (true)
		{
			TimeoutTimer timeout2;
			ServerInfo serverInfo2;
			checked
			{
				long num4 = num2 * (unchecked(num3 / 2) + 1);
				long millisecondsRemaining = timeout.MillisecondsRemaining;
				if (num4 > millisecondsRemaining)
				{
					num4 = millisecondsRemaining;
				}
				timeout2 = TimeoutTimer.StartMillisecondsTimeout(num4);
				if (_parser != null)
				{
					_parser.Disconnect();
				}
				_parser = new TdsParser(base.ConnectionOptions.MARS, base.ConnectionOptions.Asynchronous);
				if (useFailoverHost)
				{
					if (ServerProvidedFailOverPartner != null && serverInfo.ResolvedServerName != ServerProvidedFailOverPartner)
					{
						serverInfo.SetDerivedNames(string.Empty, ServerProvidedFailOverPartner);
					}
					serverInfo2 = serverInfo;
					_timeoutErrorInternal.SetInternalSourceType(SqlConnectionInternalSourceType.Failover);
				}
				else
				{
					serverInfo2 = primaryServerInfo;
					_timeoutErrorInternal.SetInternalSourceType(SqlConnectionInternalSourceType.Principle);
				}
			}
			try
			{
				AttemptOneLogin(serverInfo2, newPassword, newSecurePassword, ignoreSniOpenTimeout: false, timeout2, withFailover: true);
				if (_routingInfo != null)
				{
					throw SQL.ROR_UnexpectedRoutingInfo(this);
				}
			}
			catch (SqlException exc)
			{
				if (IsDoNotRetryConnectError(exc) || timeout.IsExpired)
				{
					throw;
				}
				if (base.IsConnectionDoomed)
				{
					throw;
				}
				if (1 == num3 % 2 && timeout.MillisecondsRemaining <= num)
				{
					throw;
				}
				goto IL_016c;
			}
			break;
			IL_016c:
			if (1 == num3 % 2)
			{
				Thread.Sleep(num);
				num = ((num < 500) ? (num * 2) : 1000);
			}
			num3++;
			useFailoverHost = !useFailoverHost;
		}
		if (useFailoverHost && ServerProvidedFailOverPartner == null)
		{
			throw SQL.InvalidPartnerConfiguration(failoverHost, base.CurrentDatabase);
		}
		if (PoolGroupProviderInfo != null)
		{
			PoolGroupProviderInfo.FailoverCheck(this, useFailoverHost, connectionOptions, ServerProvidedFailOverPartner);
		}
		base.CurrentDataSource = (useFailoverHost ? failoverHost : primaryServerInfo.UserServerName);
	}

	private void ResolveExtendedServerName(ServerInfo serverInfo, bool aliasLookup, SqlConnectionString options)
	{
		if (serverInfo.ExtendedServerName == null)
		{
			string userServerName = serverInfo.UserServerName;
			string userProtocol = serverInfo.UserProtocol;
			serverInfo.SetDerivedNames(userProtocol, userServerName);
		}
	}

	private void AttemptOneLogin(ServerInfo serverInfo, string newPassword, SecureString newSecurePassword, bool ignoreSniOpenTimeout, TimeoutTimer timeout, bool withFailover = false)
	{
		_routingInfo = null;
		_parser._physicalStateObj.SniContext = SniContext.Snix_Connect;
		_parser.Connect(serverInfo, this, ignoreSniOpenTimeout, timeout.LegacyTimerExpire, base.ConnectionOptions.Encrypt, base.ConnectionOptions.TrustServerCertificate, base.ConnectionOptions.IntegratedSecurity, withFailover);
		_timeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.ConsumePreLoginHandshake);
		_timeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.LoginBegin);
		_parser._physicalStateObj.SniContext = SniContext.Snix_Login;
		Login(serverInfo, timeout, newPassword, newSecurePassword);
		_timeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.ProcessConnectionAuth);
		_timeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.PostLogin);
		CompleteLogin(!base.ConnectionOptions.Pooling);
		_timeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.PostLogin);
	}

	protected override object ObtainAdditionalLocksForClose()
	{
		bool num = !ThreadHasParserLockForClose;
		if (num)
		{
			_parserLock.Wait(canReleaseFromAnyThread: false);
			ThreadHasParserLockForClose = true;
		}
		return num;
	}

	protected override void ReleaseAdditionalLocksForClose(object lockToken)
	{
		if ((bool)lockToken)
		{
			ThreadHasParserLockForClose = false;
			_parserLock.Release();
		}
	}

	internal bool GetSessionAndReconnectIfNeeded(SqlConnection parent, int timeout = 0)
	{
		if (ThreadHasParserLockForClose)
		{
			return false;
		}
		_parserLock.Wait(canReleaseFromAnyThread: false);
		ThreadHasParserLockForClose = true;
		bool releaseConnectionLock = true;
		try
		{
			Task task = parent.ValidateAndReconnect(delegate
			{
				ThreadHasParserLockForClose = false;
				_parserLock.Release();
				releaseConnectionLock = false;
			}, timeout);
			if (task != null)
			{
				AsyncHelper.WaitForCompletion(task, timeout);
				return true;
			}
			return false;
		}
		finally
		{
			if (releaseConnectionLock)
			{
				ThreadHasParserLockForClose = false;
				_parserLock.Release();
			}
		}
	}

	internal void BreakConnection()
	{
		SqlConnection connection = base.Connection;
		DoomThisConnection();
		connection?.Close();
	}

	internal void OnEnvChange(SqlEnvChange rec)
	{
		switch (rec.type)
		{
		case 1:
			if (!_fConnectionOpen && _recoverySessionData == null)
			{
				_originalDatabase = rec.newValue;
			}
			base.CurrentDatabase = rec.newValue;
			break;
		case 2:
			if (!_fConnectionOpen && _recoverySessionData == null)
			{
				_originalLanguage = rec.newValue;
			}
			_currentLanguage = rec.newValue;
			break;
		case 4:
			_currentPacketSize = int.Parse(rec.newValue, CultureInfo.InvariantCulture);
			break;
		case 7:
			if (_currentSessionData != null)
			{
				_currentSessionData._collation = rec.newCollation;
			}
			break;
		case 13:
			if (base.ConnectionOptions.ApplicationIntent == ApplicationIntent.ReadOnly)
			{
				throw SQL.ROR_FailoverNotSupportedServer(this);
			}
			_currentFailoverPartner = rec.newValue;
			break;
		case 15:
			base.PromotedDTCToken = rec.newBinValue;
			break;
		case 18:
			if (_currentSessionData != null)
			{
				_currentSessionData.Reset();
			}
			break;
		case 19:
			_instanceName = rec.newValue;
			break;
		case 20:
			if (string.IsNullOrEmpty(rec.newRoutingInfo.ServerName) || rec.newRoutingInfo.Protocol != 0 || rec.newRoutingInfo.Port == 0)
			{
				throw SQL.ROR_InvalidRoutingInfo(this);
			}
			_routingInfo = rec.newRoutingInfo;
			break;
		case 3:
		case 5:
		case 6:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 14:
		case 16:
		case 17:
			break;
		}
	}

	internal void OnLoginAck(SqlLoginAck rec)
	{
		_loginAck = rec;
		if (_recoverySessionData != null && _recoverySessionData._tdsVersion != rec.tdsVersion)
		{
			throw SQL.CR_TDSVersionNotPreserved(this);
		}
		if (_currentSessionData != null)
		{
			_currentSessionData._tdsVersion = rec.tdsVersion;
		}
	}

	internal void OnFeatureExtAck(int featureId, byte[] data)
	{
		if (_routingInfo != null)
		{
			return;
		}
		switch (featureId)
		{
		case 1:
		{
			if (!_sessionRecoveryRequested)
			{
				throw SQL.ParsingError();
			}
			_sessionRecoveryAcknowledged = true;
			int num = 0;
			while (num < data.Length)
			{
				byte b = data[num];
				num++;
				byte b2 = data[num];
				num++;
				int num2;
				if (b2 == byte.MaxValue)
				{
					num2 = BitConverter.ToInt32(data, num);
					num += 4;
				}
				else
				{
					num2 = b2;
				}
				byte[] array = new byte[num2];
				Buffer.BlockCopy(data, num, array, 0, num2);
				num += num2;
				if (_recoverySessionData == null)
				{
					_currentSessionData._initialState[b] = array;
					continue;
				}
				_currentSessionData._delta[b] = new SessionStateRecord
				{
					_data = array,
					_dataLength = num2,
					_recoverable = true,
					_version = 0u
				};
				_currentSessionData._deltaDirty = true;
			}
			break;
		}
		case 5:
			if (data.Length < 1)
			{
				throw SQL.ParsingError();
			}
			base.IsGlobalTransaction = true;
			if (1 == data[0])
			{
				base.IsGlobalTransactionsEnabledForServer = true;
			}
			break;
		case 2:
			if (!_federatedAuthenticationRequested)
			{
				throw SQL.ParsingErrorFeatureId(ParsingErrorState.UnrequestedFeatureAckReceived, featureId);
			}
			if (_fedAuthFeatureExtensionData.Value.libraryType == TdsEnums.FedAuthLibrary.SecurityToken)
			{
				if (data.Length != 0)
				{
					throw SQL.ParsingError(ParsingErrorState.FedAuthFeatureAckContainsExtraData);
				}
				_federatedAuthenticationAcknowledged = true;
				break;
			}
			throw SQL.ParsingErrorLibraryType(ParsingErrorState.FedAuthFeatureAckUnknownLibraryType, (int)_fedAuthFeatureExtensionData.Value.libraryType);
		default:
			throw SQL.ParsingError();
		}
	}

	internal override bool TryReplaceConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions)
	{
		return TryOpenConnectionInternal(outerConnection, connectionFactory, retry, userOptions);
	}
}
