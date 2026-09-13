namespace System.ComponentModel.Design;

public class DesignerTransactionCloseEventArgs : EventArgs
{
	public bool TransactionCommitted { get; }

	public bool LastTransaction { get; }

	[Obsolete("This constructor is obsolete. Use DesignerTransactionCloseEventArgs(bool, bool) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
	public DesignerTransactionCloseEventArgs(bool commit)
		: this(commit, lastTransaction: true)
	{
	}

	public DesignerTransactionCloseEventArgs(bool commit, bool lastTransaction)
	{
		TransactionCommitted = commit;
		LastTransaction = lastTransaction;
	}
}
