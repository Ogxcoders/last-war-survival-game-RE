using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Transactions;

[Serializable]
public class Transaction : IDisposable, ISerializable
{
	private delegate void AsyncCommit();

	[ThreadStatic]
	private static Transaction ambient;

	private Transaction internalTransaction;

	private IsolationLevel level;

	private TransactionInformation info;

	private ArrayList dependents = new ArrayList();

	private List<IEnlistmentNotification> volatiles;

	private List<ISinglePhaseNotification> durables;

	private IPromotableSinglePhaseNotification pspe;

	private AsyncCommit asyncCommit;

	private bool committing;

	private bool committed;

	private bool aborted;

	private TransactionScope scope;

	private Exception innerException;

	private Guid tag = Guid.NewGuid();

	internal List<IEnlistmentNotification> Volatiles
	{
		get
		{
			if (volatiles == null)
			{
				volatiles = new List<IEnlistmentNotification>();
			}
			return volatiles;
		}
	}

	internal List<ISinglePhaseNotification> Durables
	{
		get
		{
			if (durables == null)
			{
				durables = new List<ISinglePhaseNotification>();
			}
			return durables;
		}
	}

	internal IPromotableSinglePhaseNotification Pspe => pspe;

	public static Transaction Current
	{
		get
		{
			EnsureIncompleteCurrentScope();
			return CurrentInternal;
		}
		set
		{
			EnsureIncompleteCurrentScope();
			CurrentInternal = value;
		}
	}

	internal static Transaction CurrentInternal
	{
		get
		{
			return ambient;
		}
		set
		{
			ambient = value;
		}
	}

	public IsolationLevel IsolationLevel
	{
		get
		{
			EnsureIncompleteCurrentScope();
			return level;
		}
	}

	public TransactionInformation TransactionInformation
	{
		get
		{
			EnsureIncompleteCurrentScope();
			return info;
		}
	}

	public Guid PromoterType
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	private bool Aborted
	{
		get
		{
			return aborted;
		}
		set
		{
			aborted = value;
			if (aborted)
			{
				info.Status = TransactionStatus.Aborted;
			}
		}
	}

	internal TransactionScope Scope
	{
		get
		{
			return scope;
		}
		set
		{
			scope = value;
		}
	}

	internal event TransactionCompletedEventHandler TransactionCompletedInternal;

	public event TransactionCompletedEventHandler TransactionCompleted
	{
		add
		{
			if (internalTransaction != null)
			{
				internalTransaction.TransactionCompleted += value;
			}
			TransactionCompletedInternal += value;
		}
		remove
		{
			if (internalTransaction != null)
			{
				internalTransaction.TransactionCompleted -= value;
			}
			TransactionCompletedInternal -= value;
		}
	}

	internal Transaction(IsolationLevel isolationLevel)
	{
		info = new TransactionInformation();
		level = isolationLevel;
	}

	internal Transaction(Transaction other)
	{
		level = other.level;
		info = other.info;
		dependents = other.dependents;
		volatiles = other.Volatiles;
		durables = other.Durables;
		pspe = other.Pspe;
		this.TransactionCompletedInternal = other.TransactionCompletedInternal;
		internalTransaction = other;
	}

	[System.MonoTODO]
	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		throw new NotImplementedException();
	}

	public Transaction Clone()
	{
		return new Transaction(this);
	}

	public void Dispose()
	{
		if (TransactionInformation.Status == TransactionStatus.Active)
		{
			Rollback();
		}
	}

	[System.MonoTODO]
	public DependentTransaction DependentClone(DependentCloneOption cloneOption)
	{
		DependentTransaction dependentTransaction = new DependentTransaction(this, cloneOption);
		dependents.Add(dependentTransaction);
		return dependentTransaction;
	}

	[System.MonoTODO("Only SinglePhase commit supported for durable resource managers.")]
	[PermissionSet(SecurityAction.LinkDemand)]
	public Enlistment EnlistDurable(Guid resourceManagerIdentifier, IEnlistmentNotification enlistmentNotification, EnlistmentOptions enlistmentOptions)
	{
		throw new NotImplementedException("DTC unsupported, only SinglePhase commit supported for durable resource managers.");
	}

	[System.MonoTODO("Only Local Transaction Manager supported. Cannot have more than 1 durable resource per transaction. Only EnlistmentOptions.None supported yet.")]
	[PermissionSet(SecurityAction.LinkDemand)]
	public Enlistment EnlistDurable(Guid resourceManagerIdentifier, ISinglePhaseNotification singlePhaseNotification, EnlistmentOptions enlistmentOptions)
	{
		EnsureIncompleteCurrentScope();
		if (pspe != null || Durables.Count > 0)
		{
			throw new NotImplementedException("DTC unsupported, multiple durable resource managers aren't supported.");
		}
		if (enlistmentOptions != EnlistmentOptions.None)
		{
			throw new NotImplementedException("EnlistmentOptions other than None aren't supported");
		}
		Durables.Add(singlePhaseNotification);
		return new Enlistment();
	}

	public bool EnlistPromotableSinglePhase(IPromotableSinglePhaseNotification promotableSinglePhaseNotification)
	{
		EnsureIncompleteCurrentScope();
		if (pspe != null || Durables.Count > 0)
		{
			return false;
		}
		pspe = promotableSinglePhaseNotification;
		pspe.Initialize();
		return true;
	}

	public void SetDistributedTransactionIdentifier(IPromotableSinglePhaseNotification promotableNotification, Guid distributedTransactionIdentifier)
	{
		throw new NotImplementedException();
	}

	public bool EnlistPromotableSinglePhase(IPromotableSinglePhaseNotification promotableSinglePhaseNotification, Guid promoterType)
	{
		throw new NotImplementedException();
	}

	public byte[] GetPromotedToken()
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO("EnlistmentOptions being ignored")]
	public Enlistment EnlistVolatile(IEnlistmentNotification enlistmentNotification, EnlistmentOptions enlistmentOptions)
	{
		return EnlistVolatileInternal(enlistmentNotification, enlistmentOptions);
	}

	[System.MonoTODO("EnlistmentOptions being ignored")]
	public Enlistment EnlistVolatile(ISinglePhaseNotification singlePhaseNotification, EnlistmentOptions enlistmentOptions)
	{
		return EnlistVolatileInternal(singlePhaseNotification, enlistmentOptions);
	}

	private Enlistment EnlistVolatileInternal(IEnlistmentNotification notification, EnlistmentOptions options)
	{
		EnsureIncompleteCurrentScope();
		Volatiles.Add(notification);
		return new Enlistment();
	}

	[System.MonoTODO("Only Local Transaction Manager supported. Cannot have more than 1 durable resource per transaction.")]
	[PermissionSet(SecurityAction.LinkDemand)]
	public Enlistment PromoteAndEnlistDurable(Guid manager, IPromotableSinglePhaseNotification promotableNotification, ISinglePhaseNotification notification, EnlistmentOptions options)
	{
		throw new NotImplementedException("DTC unsupported, multiple durable resource managers aren't supported.");
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as Transaction);
	}

	private bool Equals(Transaction t)
	{
		if ((object)t == this)
		{
			return true;
		}
		if ((object)t == null)
		{
			return false;
		}
		if (level == t.level)
		{
			return info == t.info;
		}
		return false;
	}

	public static bool operator ==(Transaction x, Transaction y)
	{
		return x?.Equals(y) ?? ((object)y == null);
	}

	public static bool operator !=(Transaction x, Transaction y)
	{
		return !(x == y);
	}

	public override int GetHashCode()
	{
		return (int)((uint)level ^ (uint)info.GetHashCode()) ^ dependents.GetHashCode();
	}

	public void Rollback()
	{
		Rollback(null);
	}

	public void Rollback(Exception e)
	{
		EnsureIncompleteCurrentScope();
		Rollback(e, null);
	}

	internal void Rollback(Exception ex, object abortingEnlisted)
	{
		if (aborted)
		{
			FireCompleted();
			return;
		}
		if (info.Status == TransactionStatus.Committed)
		{
			throw new TransactionException("Transaction has already been committed. Cannot accept any new work.");
		}
		innerException = ex;
		SinglePhaseEnlistment singlePhaseEnlistment = new SinglePhaseEnlistment();
		foreach (IEnlistmentNotification @volatile in Volatiles)
		{
			if (@volatile != abortingEnlisted)
			{
				@volatile.Rollback(singlePhaseEnlistment);
			}
		}
		List<ISinglePhaseNotification> list = Durables;
		if (list.Count > 0 && list[0] != abortingEnlisted)
		{
			list[0].Rollback(singlePhaseEnlistment);
		}
		if (pspe != null && pspe != abortingEnlisted)
		{
			pspe.Rollback(singlePhaseEnlistment);
		}
		Aborted = true;
		FireCompleted();
	}

	protected IAsyncResult BeginCommitInternal(AsyncCallback callback)
	{
		if (committed || committing)
		{
			throw new InvalidOperationException("Commit has already been called for this transaction.");
		}
		committing = true;
		asyncCommit = DoCommit;
		return asyncCommit.BeginInvoke(callback, null);
	}

	protected void EndCommitInternal(IAsyncResult ar)
	{
		asyncCommit.EndInvoke(ar);
	}

	internal void CommitInternal()
	{
		if (committed || committing)
		{
			throw new InvalidOperationException("Commit has already been called for this transaction.");
		}
		committing = true;
		try
		{
			DoCommit();
		}
		catch (TransactionException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw new TransactionAbortedException("Transaction failed", ex2);
		}
	}

	private void DoCommit()
	{
		if (Scope != null && (!Scope.IsComplete || !Scope.IsDisposed))
		{
			Rollback(null, null);
			CheckAborted();
		}
		List<IEnlistmentNotification> list = Volatiles;
		List<ISinglePhaseNotification> list2 = Durables;
		if (list.Count == 1 && list2.Count == 0 && list[0] is ISinglePhaseNotification single)
		{
			DoSingleCommit(single);
			Complete();
			return;
		}
		if (list.Count > 0)
		{
			DoPreparePhase();
		}
		if (list2.Count > 0)
		{
			DoSingleCommit(list2[0]);
		}
		if (pspe != null)
		{
			DoSingleCommit(pspe);
		}
		if (list.Count > 0)
		{
			DoCommitPhase();
		}
		Complete();
	}

	private void Complete()
	{
		committing = false;
		committed = true;
		if (!aborted)
		{
			info.Status = TransactionStatus.Committed;
		}
		FireCompleted();
	}

	internal void InitScope(TransactionScope scope)
	{
		CheckAborted();
		if (committed)
		{
			throw new InvalidOperationException("Commit has already been called on this transaction.");
		}
		Scope = scope;
	}

	private static void PrepareCallbackWrapper(object state)
	{
		PreparingEnlistment preparingEnlistment = state as PreparingEnlistment;
		try
		{
			preparingEnlistment.EnlistmentNotification.Prepare(preparingEnlistment);
		}
		catch (Exception exception)
		{
			preparingEnlistment.Exception = exception;
			if (!preparingEnlistment.IsPrepared)
			{
				((ManualResetEvent)preparingEnlistment.WaitHandle).Set();
			}
		}
	}

	private void DoPreparePhase()
	{
		foreach (IEnlistmentNotification @volatile in Volatiles)
		{
			PreparingEnlistment preparingEnlistment = new PreparingEnlistment(this, @volatile);
			ThreadPool.QueueUserWorkItem(PrepareCallbackWrapper, preparingEnlistment);
			TimeSpan timeout = ((Scope != null) ? Scope.Timeout : TransactionManager.DefaultTimeout);
			if (!preparingEnlistment.WaitHandle.WaitOne(timeout, exitContext: true))
			{
				Aborted = true;
				throw new TimeoutException("Transaction timedout");
			}
			if (preparingEnlistment.Exception != null)
			{
				innerException = preparingEnlistment.Exception;
				Aborted = true;
				break;
			}
			if (!preparingEnlistment.IsPrepared)
			{
				Aborted = true;
				break;
			}
		}
		CheckAborted();
	}

	private void DoCommitPhase()
	{
		foreach (IEnlistmentNotification @volatile in Volatiles)
		{
			Enlistment enlistment = new Enlistment();
			@volatile.Commit(enlistment);
		}
	}

	private void DoSingleCommit(ISinglePhaseNotification single)
	{
		if (single != null)
		{
			single.SinglePhaseCommit(new SinglePhaseEnlistment(this, single));
			CheckAborted();
		}
	}

	private void DoSingleCommit(IPromotableSinglePhaseNotification single)
	{
		if (single != null)
		{
			single.SinglePhaseCommit(new SinglePhaseEnlistment(this, single));
			CheckAborted();
		}
	}

	private void CheckAborted()
	{
		if (aborted || (Scope != null && Scope.IsAborted))
		{
			throw new TransactionAbortedException("Transaction has aborted", innerException);
		}
	}

	private void FireCompleted()
	{
		if (this.TransactionCompletedInternal != null)
		{
			this.TransactionCompletedInternal(this, new TransactionEventArgs(this));
		}
	}

	private static void EnsureIncompleteCurrentScope()
	{
		if (CurrentInternal == null || CurrentInternal.Scope == null || !CurrentInternal.Scope.IsComplete)
		{
			return;
		}
		throw new InvalidOperationException("The current TransactionScope is already complete");
	}
}
