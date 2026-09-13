using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace System.Data.ProviderBase;

internal abstract class DbConnectionInternal
{
	internal static readonly StateChangeEventArgs StateChangeClosed = new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Closed);

	internal static readonly StateChangeEventArgs StateChangeOpen = new StateChangeEventArgs(ConnectionState.Closed, ConnectionState.Open);

	private readonly bool _allowSetConnectionString;

	private readonly bool _hidePassword;

	private readonly ConnectionState _state;

	private readonly WeakReference _owningObject = new WeakReference(null, trackResurrection: false);

	private DbConnectionPool _connectionPool;

	private DbReferenceCollection _referenceCollection;

	private int _pooledCount;

	private bool _connectionIsDoomed;

	private bool _cannotBePooled;

	private DateTime _createTime;

	private bool _isInStasis;

	private Transaction _enlistedTransaction;

	private Transaction _enlistedTransactionOriginal;

	internal bool AllowSetConnectionString => _allowSetConnectionString;

	internal bool CanBePooled
	{
		get
		{
			if (!_connectionIsDoomed && !_cannotBePooled)
			{
				return !_owningObject.IsAlive;
			}
			return false;
		}
	}

	protected internal bool IsConnectionDoomed => _connectionIsDoomed;

	internal bool IsEmancipated
	{
		get
		{
			if (_pooledCount < 1)
			{
				return !_owningObject.IsAlive;
			}
			return false;
		}
	}

	internal bool IsInPool => _pooledCount == 1;

	protected internal object Owner => _owningObject.Target;

	internal DbConnectionPool Pool => _connectionPool;

	protected internal DbReferenceCollection ReferenceCollection => _referenceCollection;

	public abstract string ServerVersion { get; }

	public virtual string ServerVersionNormalized
	{
		get
		{
			throw ADP.NotSupported();
		}
	}

	public bool ShouldHidePassword => _hidePassword;

	public ConnectionState State => _state;

	protected internal Transaction EnlistedTransaction
	{
		get
		{
			return _enlistedTransaction;
		}
		set
		{
			Transaction enlistedTransaction = _enlistedTransaction;
			if ((!(null == enlistedTransaction) || !(null != value)) && (!(null != enlistedTransaction) || enlistedTransaction.Equals(value)))
			{
				return;
			}
			Transaction transaction = null;
			Transaction transaction2 = null;
			try
			{
				if (null != value)
				{
					transaction = value.Clone();
				}
				lock (this)
				{
					transaction2 = Interlocked.Exchange(ref _enlistedTransaction, transaction);
					_enlistedTransactionOriginal = value;
					value = transaction;
					transaction = null;
				}
			}
			finally
			{
				if (null != transaction2 && (object)transaction2 != _enlistedTransaction)
				{
					transaction2.Dispose();
				}
				if (null != transaction && (object)transaction != _enlistedTransaction)
				{
					transaction.Dispose();
				}
			}
			if (null != value)
			{
				TransactionOutcomeEnlist(value);
			}
		}
	}

	protected bool EnlistedTransactionDisposed
	{
		get
		{
			try
			{
				Transaction enlistedTransactionOriginal = _enlistedTransactionOriginal;
				return enlistedTransactionOriginal != null && enlistedTransactionOriginal.TransactionInformation == null;
			}
			catch (ObjectDisposedException)
			{
				return true;
			}
		}
	}

	internal bool IsTxRootWaitingForTxEnd => _isInStasis;

	protected virtual bool UnbindOnTransactionCompletion => true;

	protected internal virtual bool IsNonPoolableTransactionRoot => false;

	internal virtual bool IsTransactionRoot => false;

	protected virtual bool ReadyToPrepareTransaction => true;

	protected DbConnectionInternal()
		: this(ConnectionState.Open, hidePassword: true, allowSetConnectionString: false)
	{
	}

	internal DbConnectionInternal(ConnectionState state, bool hidePassword, bool allowSetConnectionString)
	{
		_allowSetConnectionString = allowSetConnectionString;
		_hidePassword = hidePassword;
		_state = state;
	}

	internal void AddWeakReference(object value, int tag)
	{
		if (_referenceCollection == null)
		{
			_referenceCollection = CreateReferenceCollection();
			if (_referenceCollection == null)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.CreateReferenceCollectionReturnedNull);
			}
		}
		_referenceCollection.Add(value, tag);
	}

	public abstract DbTransaction BeginTransaction(IsolationLevel il);

	public virtual void ChangeDatabase(string value)
	{
		throw ADP.MethodNotImplemented("ChangeDatabase");
	}

	internal virtual void PrepareForReplaceConnection()
	{
	}

	protected virtual void PrepareForCloseConnection()
	{
	}

	protected virtual object ObtainAdditionalLocksForClose()
	{
		return null;
	}

	protected virtual void ReleaseAdditionalLocksForClose(object lockToken)
	{
	}

	protected virtual DbReferenceCollection CreateReferenceCollection()
	{
		throw ADP.InternalError(ADP.InternalErrorCode.AttemptingToConstructReferenceCollectionOnStaticObject);
	}

	protected abstract void Deactivate();

	internal void DeactivateConnection()
	{
		if (!_connectionIsDoomed && Pool.UseLoadBalancing && DateTime.UtcNow.Ticks - _createTime.Ticks > Pool.LoadBalanceTimeout.Ticks)
		{
			DoNotPoolThisConnection();
		}
		Deactivate();
	}

	protected internal void DoNotPoolThisConnection()
	{
		_cannotBePooled = true;
	}

	protected internal void DoomThisConnection()
	{
		_connectionIsDoomed = true;
	}

	protected internal virtual DataTable GetSchema(DbConnectionFactory factory, DbConnectionPoolGroup poolGroup, DbConnection outerConnection, string collectionName, string[] restrictions)
	{
		return factory.GetMetaDataFactory(poolGroup, this).GetSchema(outerConnection, collectionName, restrictions);
	}

	internal void MakeNonPooledObject(object owningObject)
	{
		_connectionPool = null;
		_owningObject.Target = owningObject;
		_pooledCount = -1;
	}

	internal void MakePooledConnection(DbConnectionPool connectionPool)
	{
		_createTime = DateTime.UtcNow;
		_connectionPool = connectionPool;
	}

	internal void NotifyWeakReference(int message)
	{
		ReferenceCollection?.Notify(message);
	}

	internal virtual void OpenConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory)
	{
		if (!TryOpenConnection(outerConnection, connectionFactory, null, null))
		{
			throw ADP.InternalError(ADP.InternalErrorCode.SynchronousConnectReturnedPending);
		}
	}

	internal virtual bool TryOpenConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions)
	{
		throw ADP.ConnectionAlreadyOpen(State);
	}

	internal virtual bool TryReplaceConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions)
	{
		throw ADP.MethodNotImplemented("TryReplaceConnection");
	}

	protected bool TryOpenConnectionInternal(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions)
	{
		if (connectionFactory.SetInnerConnectionFrom(outerConnection, DbConnectionClosedConnecting.SingletonInstance, this))
		{
			DbConnectionInternal connection = null;
			try
			{
				connectionFactory.PermissionDemand(outerConnection);
				if (!connectionFactory.TryGetConnection(outerConnection, retry, userOptions, this, out connection))
				{
					return false;
				}
			}
			catch
			{
				connectionFactory.SetInnerConnectionTo(outerConnection, this);
				throw;
			}
			if (connection == null)
			{
				connectionFactory.SetInnerConnectionTo(outerConnection, this);
				throw ADP.InternalConnectionError(ADP.ConnectionError.GetConnectionReturnsNull);
			}
			connectionFactory.SetInnerConnectionEvent(outerConnection, connection);
		}
		return true;
	}

	internal void PrePush(object expectedOwner)
	{
		if (expectedOwner == null)
		{
			if (_owningObject.Target != null)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.UnpooledObjectHasOwner);
			}
		}
		else if (_owningObject.Target != expectedOwner)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnpooledObjectHasWrongOwner);
		}
		if (_pooledCount != 0)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.PushingObjectSecondTime);
		}
		_pooledCount++;
		_owningObject.Target = null;
	}

	internal void PostPop(object newOwner)
	{
		if (_owningObject.Target != null)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.PooledObjectHasOwner);
		}
		_owningObject.Target = newOwner;
		_pooledCount--;
		if (Pool != null)
		{
			if (_pooledCount != 0)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.PooledObjectInPoolMoreThanOnce);
			}
		}
		else if (-1 != _pooledCount)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.NonPooledObjectUsedMoreThanOnce);
		}
	}

	internal void RemoveWeakReference(object value)
	{
		ReferenceCollection?.Remove(value);
	}

	internal virtual bool IsConnectionAlive(bool throwOnException = false)
	{
		return true;
	}

	protected abstract void Activate(Transaction transaction);

	internal void ActivateConnection(Transaction transaction)
	{
		Activate(transaction);
	}

	internal virtual void CloseConnection(DbConnection owningObject, DbConnectionFactory connectionFactory)
	{
		if (!connectionFactory.SetInnerConnectionFrom(owningObject, DbConnectionOpenBusy.SingletonInstance, this))
		{
			return;
		}
		lock (this)
		{
			object lockToken = ObtainAdditionalLocksForClose();
			try
			{
				PrepareForCloseConnection();
				DbConnectionPool pool = Pool;
				DetachCurrentTransactionIfEnded();
				if (pool != null)
				{
					pool.PutObject(this, owningObject);
					return;
				}
				Deactivate();
				_owningObject.Target = null;
				if (IsTransactionRoot)
				{
					SetInStasis();
				}
				else
				{
					Dispose();
				}
			}
			finally
			{
				ReleaseAdditionalLocksForClose(lockToken);
				connectionFactory.SetInnerConnectionEvent(owningObject, DbConnectionClosedPreviouslyOpened.SingletonInstance);
			}
		}
	}

	internal virtual void DelegatedTransactionEnded()
	{
		if (1 == _pooledCount)
		{
			TerminateStasis(returningToPool: true);
			Deactivate();
			DbConnectionPool pool = Pool;
			if (pool == null)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.PooledObjectWithoutPool);
			}
			pool.PutObjectFromTransactedPool(this);
		}
		else if (-1 == _pooledCount && !_owningObject.IsAlive)
		{
			TerminateStasis(returningToPool: false);
			Deactivate();
			Dispose();
		}
	}

	public virtual void Dispose()
	{
		_connectionPool = null;
		_connectionIsDoomed = true;
		_enlistedTransactionOriginal = null;
		Transaction transaction = Interlocked.Exchange(ref _enlistedTransaction, null);
		if (transaction != null)
		{
			transaction.Dispose();
		}
	}

	public abstract void EnlistTransaction(Transaction transaction);

	protected virtual void CleanupTransactionOnCompletion(Transaction transaction)
	{
	}

	internal void DetachCurrentTransactionIfEnded()
	{
		Transaction enlistedTransaction = EnlistedTransaction;
		if (enlistedTransaction != null)
		{
			bool flag;
			try
			{
				flag = enlistedTransaction.TransactionInformation.Status != TransactionStatus.Active;
			}
			catch (TransactionException)
			{
				flag = true;
			}
			if (flag)
			{
				DetachTransaction(enlistedTransaction, isExplicitlyReleasing: true);
			}
		}
	}

	internal void DetachTransaction(Transaction transaction, bool isExplicitlyReleasing)
	{
		lock (this)
		{
			DbConnection dbConnection = (DbConnection)Owner;
			if (!isExplicitlyReleasing && !UnbindOnTransactionCompletion && dbConnection != null)
			{
				return;
			}
			Transaction enlistedTransaction = _enlistedTransaction;
			if (enlistedTransaction != null && transaction.Equals(enlistedTransaction))
			{
				EnlistedTransaction = null;
				if (IsTxRootWaitingForTxEnd)
				{
					DelegatedTransactionEnded();
				}
			}
		}
	}

	internal void CleanupConnectionOnTransactionCompletion(Transaction transaction)
	{
		DetachTransaction(transaction, isExplicitlyReleasing: false);
		Pool?.TransactionEnded(transaction, this);
	}

	private void TransactionCompletedEvent(object sender, TransactionEventArgs e)
	{
		Transaction transaction = e.Transaction;
		CleanupTransactionOnCompletion(transaction);
		CleanupConnectionOnTransactionCompletion(transaction);
	}

	private void TransactionOutcomeEnlist(Transaction transaction)
	{
		transaction.TransactionCompleted += TransactionCompletedEvent;
	}

	internal void SetInStasis()
	{
		_isInStasis = true;
	}

	private void TerminateStasis(bool returningToPool)
	{
		_isInStasis = false;
	}
}
