using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common;

public abstract class DbTransaction : MarshalByRefObject, IDbTransaction, IDisposable, IAsyncDisposable
{
	public DbConnection Connection => DbConnection;

	IDbConnection IDbTransaction.Connection => DbConnection;

	protected abstract DbConnection DbConnection { get; }

	public abstract IsolationLevel IsolationLevel { get; }

	public abstract void Commit();

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public abstract void Rollback();

	public virtual Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return Task.FromCanceled(cancellationToken);
		}
		try
		{
			Commit();
			return Task.CompletedTask;
		}
		catch (Exception exception)
		{
			return Task.FromException(exception);
		}
	}

	public virtual ValueTask DisposeAsync()
	{
		Dispose();
		return default(ValueTask);
	}

	public virtual Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return Task.FromCanceled(cancellationToken);
		}
		try
		{
			Rollback();
			return Task.CompletedTask;
		}
		catch (Exception exception)
		{
			return Task.FromException(exception);
		}
	}
}
