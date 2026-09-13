using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace System.Data.ProviderBase;

internal sealed class DbConnectionPool
{
	private enum State
	{
		Initializing,
		Running,
		ShuttingDown
	}

	private sealed class TransactedConnectionList : List<DbConnectionInternal>
	{
		private Transaction _transaction;

		internal TransactedConnectionList(int initialAllocation, Transaction tx)
			: base(initialAllocation)
		{
			_transaction = tx;
		}

		internal void Dispose()
		{
			if (null != _transaction)
			{
				_transaction.Dispose();
			}
		}
	}

	private sealed class PendingGetConnection
	{
		public long DueTime { get; private set; }

		public DbConnection Owner { get; private set; }

		public TaskCompletionSource<DbConnectionInternal> Completion { get; private set; }

		public DbConnectionOptions UserOptions { get; private set; }

		public PendingGetConnection(long dueTime, DbConnection owner, TaskCompletionSource<DbConnectionInternal> completion, DbConnectionOptions userOptions)
		{
			DueTime = dueTime;
			Owner = owner;
			Completion = completion;
		}
	}

	private sealed class TransactedConnectionPool
	{
		private Dictionary<Transaction, TransactedConnectionList> _transactedCxns;

		private DbConnectionPool _pool;

		private static int _objectTypeCount;

		internal readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

		internal int ObjectID => _objectID;

		internal DbConnectionPool Pool => _pool;

		internal TransactedConnectionPool(DbConnectionPool pool)
		{
			_pool = pool;
			_transactedCxns = new Dictionary<Transaction, TransactedConnectionList>();
		}

		internal DbConnectionInternal GetTransactedObject(Transaction transaction)
		{
			DbConnectionInternal result = null;
			bool flag = false;
			TransactedConnectionList value;
			lock (_transactedCxns)
			{
				flag = _transactedCxns.TryGetValue(transaction, out value);
			}
			if (flag)
			{
				lock (value)
				{
					int num = value.Count - 1;
					if (0 <= num)
					{
						result = value[num];
						value.RemoveAt(num);
					}
				}
			}
			return result;
		}

		internal void PutTransactedObject(Transaction transaction, DbConnectionInternal transactedObject)
		{
			bool flag = false;
			TransactedConnectionList value;
			lock (_transactedCxns)
			{
				if (flag = _transactedCxns.TryGetValue(transaction, out value))
				{
					lock (value)
					{
						value.Add(transactedObject);
					}
				}
			}
			if (flag)
			{
				return;
			}
			Transaction transaction2 = null;
			TransactedConnectionList transactedConnectionList = null;
			try
			{
				transaction2 = transaction.Clone();
				transactedConnectionList = new TransactedConnectionList(2, transaction2);
				lock (_transactedCxns)
				{
					if (flag = _transactedCxns.TryGetValue(transaction, out value))
					{
						lock (value)
						{
							value.Add(transactedObject);
							return;
						}
					}
					transactedConnectionList.Add(transactedObject);
					_transactedCxns.Add(transaction2, transactedConnectionList);
					transaction2 = null;
				}
			}
			finally
			{
				if (null != transaction2)
				{
					if (transactedConnectionList != null)
					{
						transactedConnectionList.Dispose();
					}
					else
					{
						transaction2.Dispose();
					}
				}
			}
		}

		internal void TransactionEnded(Transaction transaction, DbConnectionInternal transactedObject)
		{
			int num = -1;
			lock (_transactedCxns)
			{
				if (_transactedCxns.TryGetValue(transaction, out var value))
				{
					bool flag = false;
					lock (value)
					{
						num = value.IndexOf(transactedObject);
						if (num >= 0)
						{
							value.RemoveAt(num);
						}
						if (0 >= value.Count)
						{
							_transactedCxns.Remove(transaction);
							flag = true;
						}
					}
					if (flag)
					{
						value.Dispose();
					}
				}
			}
			if (0 <= num)
			{
				Pool.PutObjectFromTransactedPool(transactedObject);
			}
		}
	}

	private sealed class PoolWaitHandles
	{
		private readonly Semaphore _poolSemaphore;

		private readonly ManualResetEvent _errorEvent;

		private readonly Semaphore _creationSemaphore;

		private readonly WaitHandle[] _handlesWithCreate;

		private readonly WaitHandle[] _handlesWithoutCreate;

		internal Semaphore CreationSemaphore => _creationSemaphore;

		internal ManualResetEvent ErrorEvent => _errorEvent;

		internal Semaphore PoolSemaphore => _poolSemaphore;

		internal PoolWaitHandles()
		{
			_poolSemaphore = new Semaphore(0, 1048576);
			_errorEvent = new ManualResetEvent(initialState: false);
			_creationSemaphore = new Semaphore(1, 1);
			_handlesWithCreate = new WaitHandle[3] { _poolSemaphore, _errorEvent, _creationSemaphore };
			_handlesWithoutCreate = new WaitHandle[2] { _poolSemaphore, _errorEvent };
		}

		internal WaitHandle[] GetHandles(bool withCreate)
		{
			if (!withCreate)
			{
				return _handlesWithoutCreate;
			}
			return _handlesWithCreate;
		}
	}

	private const int MAX_Q_SIZE = 1048576;

	private const int SEMAPHORE_HANDLE = 0;

	private const int ERROR_HANDLE = 1;

	private const int CREATION_HANDLE = 2;

	private const int BOGUS_HANDLE = 3;

	private const int ERROR_WAIT_DEFAULT = 5000;

	private static readonly Random s_random = new Random(5101977);

	private readonly int _cleanupWait;

	private readonly DbConnectionPoolIdentity _identity;

	private readonly DbConnectionFactory _connectionFactory;

	private readonly DbConnectionPoolGroup _connectionPoolGroup;

	private readonly DbConnectionPoolGroupOptions _connectionPoolGroupOptions;

	private DbConnectionPoolProviderInfo _connectionPoolProviderInfo;

	private State _state;

	private readonly ConcurrentStack<DbConnectionInternal> _stackOld = new ConcurrentStack<DbConnectionInternal>();

	private readonly ConcurrentStack<DbConnectionInternal> _stackNew = new ConcurrentStack<DbConnectionInternal>();

	private readonly ConcurrentQueue<PendingGetConnection> _pendingOpens = new ConcurrentQueue<PendingGetConnection>();

	private int _pendingOpensWaiting;

	private readonly WaitCallback _poolCreateRequest;

	private int _waitCount;

	private readonly PoolWaitHandles _waitHandles;

	private Exception _resError;

	private volatile bool _errorOccurred;

	private int _errorWait;

	private Timer _errorTimer;

	private Timer _cleanupTimer;

	private readonly TransactedConnectionPool _transactedConnectionPool;

	private readonly List<DbConnectionInternal> _objectList;

	private int _totalObjects;

	private int CreationTimeout => PoolGroupOptions.CreationTimeout;

	internal int Count => _totalObjects;

	internal DbConnectionFactory ConnectionFactory => _connectionFactory;

	internal bool ErrorOccurred => _errorOccurred;

	private bool HasTransactionAffinity => PoolGroupOptions.HasTransactionAffinity;

	internal TimeSpan LoadBalanceTimeout => PoolGroupOptions.LoadBalanceTimeout;

	private bool NeedToReplenish
	{
		get
		{
			if (State.Running != _state)
			{
				return false;
			}
			int count = Count;
			if (count >= MaxPoolSize)
			{
				return false;
			}
			if (count < MinPoolSize)
			{
				return true;
			}
			int num = _stackNew.Count + _stackOld.Count;
			int waitCount = _waitCount;
			if (num >= waitCount)
			{
				if (num == waitCount)
				{
					return count > 1;
				}
				return false;
			}
			return true;
		}
	}

	internal DbConnectionPoolIdentity Identity => _identity;

	internal bool IsRunning => State.Running == _state;

	private int MaxPoolSize => PoolGroupOptions.MaxPoolSize;

	private int MinPoolSize => PoolGroupOptions.MinPoolSize;

	internal DbConnectionPoolGroup PoolGroup => _connectionPoolGroup;

	internal DbConnectionPoolGroupOptions PoolGroupOptions => _connectionPoolGroupOptions;

	internal DbConnectionPoolProviderInfo ProviderInfo => _connectionPoolProviderInfo;

	internal bool UseLoadBalancing => PoolGroupOptions.UseLoadBalancing;

	private bool UsingIntegrateSecurity
	{
		get
		{
			if (_identity != null)
			{
				return DbConnectionPoolIdentity.NoIdentity != _identity;
			}
			return false;
		}
	}

	internal DbConnectionPool(DbConnectionFactory connectionFactory, DbConnectionPoolGroup connectionPoolGroup, DbConnectionPoolIdentity identity, DbConnectionPoolProviderInfo connectionPoolProviderInfo)
	{
		if (identity != null && identity.IsRestricted)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.AttemptingToPoolOnRestrictedToken);
		}
		_state = State.Initializing;
		lock (s_random)
		{
			_cleanupWait = s_random.Next(12, 24) * 10 * 1000;
		}
		_connectionFactory = connectionFactory;
		_connectionPoolGroup = connectionPoolGroup;
		_connectionPoolGroupOptions = connectionPoolGroup.PoolGroupOptions;
		_connectionPoolProviderInfo = connectionPoolProviderInfo;
		_identity = identity;
		_waitHandles = new PoolWaitHandles();
		_errorWait = 5000;
		_errorTimer = null;
		_objectList = new List<DbConnectionInternal>(MaxPoolSize);
		_transactedConnectionPool = new TransactedConnectionPool(this);
		_poolCreateRequest = PoolCreateRequest;
		_state = State.Running;
	}

	private void CleanupCallback(object state)
	{
		while (Count > MinPoolSize && _waitHandles.PoolSemaphore.WaitOne(0))
		{
			if (_stackOld.TryPop(out var result))
			{
				bool flag = true;
				lock (result)
				{
					if (result.IsTransactionRoot)
					{
						flag = false;
					}
				}
				if (flag)
				{
					DestroyObject(result);
				}
				else
				{
					result.SetInStasis();
				}
				continue;
			}
			_waitHandles.PoolSemaphore.Release(1);
			break;
		}
		if (_waitHandles.PoolSemaphore.WaitOne(0))
		{
			DbConnectionInternal result2;
			while (_stackNew.TryPop(out result2))
			{
				_stackOld.Push(result2);
			}
			_waitHandles.PoolSemaphore.Release(1);
		}
		QueuePoolCreateRequest();
	}

	internal void Clear()
	{
		DbConnectionInternal result;
		lock (_objectList)
		{
			int count = _objectList.Count;
			for (int i = 0; i < count; i++)
			{
				result = _objectList[i];
				result?.DoNotPoolThisConnection();
			}
		}
		while (_stackNew.TryPop(out result))
		{
			DestroyObject(result);
		}
		while (_stackOld.TryPop(out result))
		{
			DestroyObject(result);
		}
		ReclaimEmancipatedObjects();
	}

	private Timer CreateCleanupTimer()
	{
		return ADP.UnsafeCreateTimer(CleanupCallback, null, _cleanupWait, _cleanupWait);
	}

	private DbConnectionInternal CreateObject(DbConnection owningObject, DbConnectionOptions userOptions, DbConnectionInternal oldConnection)
	{
		DbConnectionInternal dbConnectionInternal = null;
		try
		{
			dbConnectionInternal = _connectionFactory.CreatePooledConnection(this, owningObject, _connectionPoolGroup.ConnectionOptions, _connectionPoolGroup.PoolKey, userOptions);
			if (dbConnectionInternal == null)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.CreateObjectReturnedNull);
			}
			if (!dbConnectionInternal.CanBePooled)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.NewObjectCannotBePooled);
			}
			dbConnectionInternal.PrePush(null);
			lock (_objectList)
			{
				if (oldConnection != null && oldConnection.Pool == this)
				{
					_objectList.Remove(oldConnection);
				}
				_objectList.Add(dbConnectionInternal);
				_totalObjects = _objectList.Count;
			}
			if (oldConnection != null)
			{
				DbConnectionPool pool = oldConnection.Pool;
				if (pool != null && pool != this)
				{
					lock (pool._objectList)
					{
						pool._objectList.Remove(oldConnection);
						pool._totalObjects = pool._objectList.Count;
					}
				}
			}
			_errorWait = 5000;
			return dbConnectionInternal;
		}
		catch (Exception ex)
		{
			if (!ADP.IsCatchableExceptionType(ex))
			{
				throw;
			}
			dbConnectionInternal = null;
			_resError = ex;
			Timer timer = new Timer(ErrorCallback, null, -1, -1);
			try
			{
			}
			finally
			{
				_waitHandles.ErrorEvent.Set();
				_errorOccurred = true;
				_errorTimer = timer;
				timer.Change(_errorWait, _errorWait);
			}
			if (30000 < _errorWait)
			{
				_errorWait = 60000;
			}
			else
			{
				_errorWait *= 2;
			}
			throw;
		}
	}

	private void DeactivateObject(DbConnectionInternal obj)
	{
		obj.DeactivateConnection();
		bool flag = false;
		bool flag2 = false;
		if (obj.IsConnectionDoomed)
		{
			flag2 = true;
		}
		else
		{
			lock (obj)
			{
				if (_state == State.ShuttingDown)
				{
					if (obj.IsTransactionRoot)
					{
						obj.SetInStasis();
					}
					else
					{
						flag2 = true;
					}
				}
				else if (obj.IsNonPoolableTransactionRoot)
				{
					obj.SetInStasis();
				}
				else if (obj.CanBePooled)
				{
					Transaction enlistedTransaction = obj.EnlistedTransaction;
					if (null != enlistedTransaction)
					{
						_transactedConnectionPool.PutTransactedObject(enlistedTransaction, obj);
					}
					else
					{
						flag = true;
					}
				}
				else if (obj.IsTransactionRoot && !obj.IsConnectionDoomed)
				{
					obj.SetInStasis();
				}
				else
				{
					flag2 = true;
				}
			}
		}
		if (flag)
		{
			PutNewObject(obj);
		}
		else if (flag2)
		{
			DestroyObject(obj);
			QueuePoolCreateRequest();
		}
	}

	internal void DestroyObject(DbConnectionInternal obj)
	{
		if (!obj.IsTxRootWaitingForTxEnd)
		{
			lock (_objectList)
			{
				_objectList.Remove(obj);
				_totalObjects = _objectList.Count;
			}
			obj.Dispose();
		}
	}

	private void ErrorCallback(object state)
	{
		_errorOccurred = false;
		_waitHandles.ErrorEvent.Reset();
		Timer errorTimer = _errorTimer;
		_errorTimer = null;
		errorTimer?.Dispose();
	}

	private Exception TryCloneCachedException()
	{
		if (_resError == null)
		{
			return null;
		}
		if (_resError is SqlException ex)
		{
			return ex.InternalClone();
		}
		return _resError;
	}

	private void WaitForPendingOpen()
	{
		PendingGetConnection result;
		do
		{
			bool flag = false;
			try
			{
				try
				{
				}
				finally
				{
					flag = Interlocked.CompareExchange(ref _pendingOpensWaiting, 1, 0) == 0;
				}
				if (!flag)
				{
					break;
				}
				while (_pendingOpens.TryDequeue(out result))
				{
					if (!result.Completion.Task.IsCompleted)
					{
						uint waitForMultipleObjectsTimeout = (uint)((result.DueTime != -1) ? Math.Max(ADP.TimerRemainingMilliseconds(result.DueTime), 0L) : uint.MaxValue);
						DbConnectionInternal connection = null;
						bool flag2 = false;
						Exception ex = null;
						try
						{
							bool allowCreate = true;
							bool onlyOneCheckConnection = false;
							ADP.SetCurrentTransaction(result.Completion.Task.AsyncState as Transaction);
							flag2 = !TryGetConnection(result.Owner, waitForMultipleObjectsTimeout, allowCreate, onlyOneCheckConnection, result.UserOptions, out connection);
						}
						catch (Exception ex2)
						{
							ex = ex2;
						}
						if (ex != null)
						{
							result.Completion.TrySetException(ex);
						}
						else if (flag2)
						{
							result.Completion.TrySetException(ADP.ExceptionWithStackTrace(ADP.PooledOpenTimeout()));
						}
						else if (!result.Completion.TrySetResult(connection))
						{
							PutObject(connection, result.Owner);
						}
					}
				}
			}
			finally
			{
				if (flag)
				{
					Interlocked.Exchange(ref _pendingOpensWaiting, 0);
				}
			}
		}
		while (_pendingOpens.TryPeek(out result));
	}

	internal bool TryGetConnection(DbConnection owningObject, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions, out DbConnectionInternal connection)
	{
		uint num = 0u;
		bool allowCreate = false;
		if (retry == null)
		{
			num = (uint)CreationTimeout;
			if (num == 0)
			{
				num = uint.MaxValue;
			}
			allowCreate = true;
		}
		if (_state != State.Running)
		{
			connection = null;
			return true;
		}
		bool onlyOneCheckConnection = true;
		if (TryGetConnection(owningObject, num, allowCreate, onlyOneCheckConnection, userOptions, out connection))
		{
			return true;
		}
		if (retry == null)
		{
			return true;
		}
		PendingGetConnection item = new PendingGetConnection((CreationTimeout == 0) ? (-1) : (ADP.TimerCurrent() + ADP.TimerFromSeconds(CreationTimeout / 1000)), owningObject, retry, userOptions);
		_pendingOpens.Enqueue(item);
		if (_pendingOpensWaiting == 0)
		{
			Thread thread = new Thread(WaitForPendingOpen);
			thread.IsBackground = true;
			thread.Start();
		}
		connection = null;
		return false;
	}

	private bool TryGetConnection(DbConnection owningObject, uint waitForMultipleObjectsTimeout, bool allowCreate, bool onlyOneCheckConnection, DbConnectionOptions userOptions, out DbConnectionInternal connection)
	{
		DbConnectionInternal dbConnectionInternal = null;
		Transaction transaction = null;
		if (HasTransactionAffinity)
		{
			dbConnectionInternal = GetFromTransactedPool(out transaction);
		}
		if (dbConnectionInternal == null)
		{
			Interlocked.Increment(ref _waitCount);
			do
			{
				int num = 3;
				try
				{
					try
					{
					}
					finally
					{
						num = WaitHandle.WaitAny(_waitHandles.GetHandles(allowCreate), (int)waitForMultipleObjectsTimeout);
					}
					switch (num)
					{
					case 258:
						Interlocked.Decrement(ref _waitCount);
						connection = null;
						return false;
					case 1:
						Interlocked.Decrement(ref _waitCount);
						throw TryCloneCachedException();
					case 2:
						try
						{
							dbConnectionInternal = UserCreateRequest(owningObject, userOptions);
						}
						catch
						{
							if (dbConnectionInternal == null)
							{
								Interlocked.Decrement(ref _waitCount);
							}
							throw;
						}
						finally
						{
							if (dbConnectionInternal != null)
							{
								Interlocked.Decrement(ref _waitCount);
							}
						}
						if (dbConnectionInternal == null && Count >= MaxPoolSize && MaxPoolSize != 0 && !ReclaimEmancipatedObjects())
						{
							allowCreate = false;
						}
						break;
					case 0:
						Interlocked.Decrement(ref _waitCount);
						dbConnectionInternal = GetFromGeneralPool();
						if (dbConnectionInternal == null || dbConnectionInternal.IsConnectionAlive())
						{
							break;
						}
						DestroyObject(dbConnectionInternal);
						dbConnectionInternal = null;
						if (onlyOneCheckConnection)
						{
							if (!_waitHandles.CreationSemaphore.WaitOne((int)waitForMultipleObjectsTimeout))
							{
								connection = null;
								return false;
							}
							try
							{
								dbConnectionInternal = UserCreateRequest(owningObject, userOptions);
							}
							finally
							{
								_waitHandles.CreationSemaphore.Release(1);
							}
						}
						break;
					default:
						Interlocked.Decrement(ref _waitCount);
						throw ADP.InternalError(ADP.InternalErrorCode.UnexpectedWaitAnyResult);
					}
				}
				finally
				{
					if (2 == num)
					{
						_waitHandles.CreationSemaphore.Release(1);
					}
				}
			}
			while (dbConnectionInternal == null);
		}
		if (dbConnectionInternal != null)
		{
			PrepareConnection(owningObject, dbConnectionInternal, transaction);
		}
		connection = dbConnectionInternal;
		return true;
	}

	private void PrepareConnection(DbConnection owningObject, DbConnectionInternal obj, Transaction transaction)
	{
		lock (obj)
		{
			obj.PostPop(owningObject);
		}
		try
		{
			obj.ActivateConnection(transaction);
		}
		catch
		{
			PutObject(obj, owningObject);
			throw;
		}
	}

	internal DbConnectionInternal ReplaceConnection(DbConnection owningObject, DbConnectionOptions userOptions, DbConnectionInternal oldConnection)
	{
		DbConnectionInternal dbConnectionInternal = UserCreateRequest(owningObject, userOptions, oldConnection);
		if (dbConnectionInternal != null)
		{
			PrepareConnection(owningObject, dbConnectionInternal, oldConnection.EnlistedTransaction);
			oldConnection.PrepareForReplaceConnection();
			oldConnection.DeactivateConnection();
			oldConnection.Dispose();
		}
		return dbConnectionInternal;
	}

	private DbConnectionInternal GetFromGeneralPool()
	{
		DbConnectionInternal result = null;
		if (!_stackNew.TryPop(out result) && !_stackOld.TryPop(out result))
		{
			result = null;
		}
		return result;
	}

	private DbConnectionInternal GetFromTransactedPool(out Transaction transaction)
	{
		transaction = ADP.GetCurrentTransaction();
		DbConnectionInternal dbConnectionInternal = null;
		if (null != transaction && _transactedConnectionPool != null)
		{
			dbConnectionInternal = _transactedConnectionPool.GetTransactedObject(transaction);
			if (dbConnectionInternal != null)
			{
				if (dbConnectionInternal.IsTransactionRoot)
				{
					try
					{
						dbConnectionInternal.IsConnectionAlive(throwOnException: true);
					}
					catch
					{
						DestroyObject(dbConnectionInternal);
						throw;
					}
				}
				else if (!dbConnectionInternal.IsConnectionAlive())
				{
					DestroyObject(dbConnectionInternal);
					dbConnectionInternal = null;
				}
			}
		}
		return dbConnectionInternal;
	}

	private void PoolCreateRequest(object state)
	{
		if (State.Running != _state)
		{
			return;
		}
		if (!_pendingOpens.IsEmpty && _pendingOpensWaiting == 0)
		{
			Thread thread = new Thread(WaitForPendingOpen);
			thread.IsBackground = true;
			thread.Start();
		}
		ReclaimEmancipatedObjects();
		if (ErrorOccurred || !NeedToReplenish || (UsingIntegrateSecurity && !_identity.Equals(DbConnectionPoolIdentity.GetCurrent())))
		{
			return;
		}
		int num = 3;
		try
		{
			try
			{
			}
			finally
			{
				num = WaitHandle.WaitAny(_waitHandles.GetHandles(withCreate: true), CreationTimeout);
			}
			if (2 == num)
			{
				if (ErrorOccurred)
				{
					return;
				}
				while (NeedToReplenish)
				{
					DbConnectionInternal dbConnectionInternal;
					try
					{
						dbConnectionInternal = CreateObject(null, null, null);
					}
					catch
					{
						break;
					}
					if (dbConnectionInternal != null)
					{
						PutNewObject(dbConnectionInternal);
						continue;
					}
					break;
				}
			}
			else if (258 == num)
			{
				QueuePoolCreateRequest();
			}
		}
		finally
		{
			if (2 == num)
			{
				_waitHandles.CreationSemaphore.Release(1);
			}
		}
	}

	internal void PutNewObject(DbConnectionInternal obj)
	{
		_stackNew.Push(obj);
		_waitHandles.PoolSemaphore.Release(1);
	}

	internal void PutObject(DbConnectionInternal obj, object owningObject)
	{
		lock (obj)
		{
			obj.PrePush(owningObject);
		}
		DeactivateObject(obj);
	}

	internal void PutObjectFromTransactedPool(DbConnectionInternal obj)
	{
		if (_state == State.Running && obj.CanBePooled)
		{
			PutNewObject(obj);
			return;
		}
		DestroyObject(obj);
		QueuePoolCreateRequest();
	}

	private void QueuePoolCreateRequest()
	{
		if (State.Running == _state)
		{
			ThreadPool.QueueUserWorkItem(_poolCreateRequest);
		}
	}

	private bool ReclaimEmancipatedObjects()
	{
		bool result = false;
		List<DbConnectionInternal> list = new List<DbConnectionInternal>();
		int count;
		lock (_objectList)
		{
			count = _objectList.Count;
			for (int i = 0; i < count; i++)
			{
				DbConnectionInternal dbConnectionInternal = _objectList[i];
				if (dbConnectionInternal == null)
				{
					continue;
				}
				bool lockTaken = false;
				try
				{
					Monitor.TryEnter(dbConnectionInternal, ref lockTaken);
					if (lockTaken && dbConnectionInternal.IsEmancipated)
					{
						dbConnectionInternal.PrePush(null);
						list.Add(dbConnectionInternal);
					}
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(dbConnectionInternal);
					}
				}
			}
		}
		count = list.Count;
		for (int j = 0; j < count; j++)
		{
			DbConnectionInternal dbConnectionInternal2 = list[j];
			result = true;
			dbConnectionInternal2.DetachCurrentTransactionIfEnded();
			DeactivateObject(dbConnectionInternal2);
		}
		return result;
	}

	internal void Startup()
	{
		_cleanupTimer = CreateCleanupTimer();
		if (NeedToReplenish)
		{
			QueuePoolCreateRequest();
		}
	}

	internal void Shutdown()
	{
		_state = State.ShuttingDown;
		Timer cleanupTimer = _cleanupTimer;
		_cleanupTimer = null;
		cleanupTimer?.Dispose();
	}

	internal void TransactionEnded(Transaction transaction, DbConnectionInternal transactedObject)
	{
		_transactedConnectionPool?.TransactionEnded(transaction, transactedObject);
	}

	private DbConnectionInternal UserCreateRequest(DbConnection owningObject, DbConnectionOptions userOptions, DbConnectionInternal oldConnection = null)
	{
		DbConnectionInternal result = null;
		if (ErrorOccurred)
		{
			throw TryCloneCachedException();
		}
		if ((oldConnection != null || Count < MaxPoolSize || MaxPoolSize == 0) && (oldConnection != null || (Count & 1) == 1 || !ReclaimEmancipatedObjects()))
		{
			result = CreateObject(owningObject, userOptions, oldConnection);
		}
		return result;
	}
}
