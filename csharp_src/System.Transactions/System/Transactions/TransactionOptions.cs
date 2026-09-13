namespace System.Transactions;

public struct TransactionOptions
{
	private IsolationLevel level;

	private TimeSpan timeout;

	public IsolationLevel IsolationLevel
	{
		get
		{
			return level;
		}
		set
		{
			level = value;
		}
	}

	public TimeSpan Timeout
	{
		get
		{
			return timeout;
		}
		set
		{
			timeout = value;
		}
	}

	internal TransactionOptions(IsolationLevel level, TimeSpan timeout)
	{
		this.level = level;
		this.timeout = timeout;
	}

	public static bool operator ==(TransactionOptions x, TransactionOptions y)
	{
		if (x.level == y.level)
		{
			return x.timeout == y.timeout;
		}
		return false;
	}

	public static bool operator !=(TransactionOptions x, TransactionOptions y)
	{
		if (x.level == y.level)
		{
			return x.timeout != y.timeout;
		}
		return true;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is TransactionOptions))
		{
			return false;
		}
		return this == (TransactionOptions)obj;
	}

	public override int GetHashCode()
	{
		return (int)level ^ timeout.GetHashCode();
	}
}
