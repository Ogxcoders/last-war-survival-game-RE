using System.Threading;

namespace System.Transactions;

public class PreparingEnlistment : Enlistment
{
	private bool prepared;

	private Transaction tx;

	private IEnlistmentNotification enlisted;

	private WaitHandle waitHandle;

	private Exception ex;

	internal bool IsPrepared => prepared;

	internal WaitHandle WaitHandle => waitHandle;

	internal IEnlistmentNotification EnlistmentNotification => enlisted;

	internal Exception Exception
	{
		get
		{
			return ex;
		}
		set
		{
			ex = value;
		}
	}

	internal PreparingEnlistment(Transaction tx, IEnlistmentNotification enlisted)
	{
		this.tx = tx;
		this.enlisted = enlisted;
		waitHandle = new ManualResetEvent(initialState: false);
	}

	public void ForceRollback()
	{
		ForceRollback(null);
	}

	internal override void InternalOnDone()
	{
		Prepared();
	}

	[System.MonoTODO]
	public void ForceRollback(Exception e)
	{
		tx.Rollback(e, enlisted);
		((ManualResetEvent)waitHandle).Set();
	}

	[System.MonoTODO]
	public void Prepared()
	{
		prepared = true;
		((ManualResetEvent)waitHandle).Set();
	}

	[System.MonoTODO]
	public byte[] RecoveryInformation()
	{
		throw new NotImplementedException();
	}
}
