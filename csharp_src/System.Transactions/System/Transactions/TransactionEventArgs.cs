namespace System.Transactions;

public class TransactionEventArgs : EventArgs
{
	private Transaction transaction;

	public Transaction Transaction => transaction;

	public TransactionEventArgs()
	{
	}

	internal TransactionEventArgs(Transaction transaction)
		: this()
	{
		this.transaction = transaction;
	}
}
