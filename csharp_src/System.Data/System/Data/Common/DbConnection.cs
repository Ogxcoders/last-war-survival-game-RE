using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace System.Data.Common;

public abstract class DbConnection : Component, IDbConnection, IDisposable, IAsyncDisposable
{
	internal bool _suppressStateChangeForReconnection;

	[DefaultValue("")]
	[SettingsBindable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[RecommendedAsConfigurable(true)]
	public abstract string ConnectionString { get; set; }

	public virtual int ConnectionTimeout => 15;

	public abstract string Database { get; }

	public abstract string DataSource { get; }

	protected virtual DbProviderFactory DbProviderFactory => null;

	internal DbProviderFactory ProviderFactory => DbProviderFactory;

	[Browsable(false)]
	public abstract string ServerVersion { get; }

	[Browsable(false)]
	public abstract ConnectionState State { get; }

	public virtual event StateChangeEventHandler StateChange;

	protected abstract DbTransaction BeginDbTransaction(IsolationLevel isolationLevel);

	public DbTransaction BeginTransaction()
	{
		return BeginDbTransaction(IsolationLevel.Unspecified);
	}

	public DbTransaction BeginTransaction(IsolationLevel isolationLevel)
	{
		return BeginDbTransaction(isolationLevel);
	}

	IDbTransaction IDbConnection.BeginTransaction()
	{
		return BeginDbTransaction(IsolationLevel.Unspecified);
	}

	IDbTransaction IDbConnection.BeginTransaction(IsolationLevel isolationLevel)
	{
		return BeginDbTransaction(isolationLevel);
	}

	public abstract void Close();

	public abstract void ChangeDatabase(string databaseName);

	public DbCommand CreateCommand()
	{
		return CreateDbCommand();
	}

	IDbCommand IDbConnection.CreateCommand()
	{
		return CreateDbCommand();
	}

	protected abstract DbCommand CreateDbCommand();

	public virtual void EnlistTransaction(Transaction transaction)
	{
		throw ADP.NotSupported();
	}

	public virtual DataTable GetSchema()
	{
		throw ADP.NotSupported();
	}

	public virtual DataTable GetSchema(string collectionName)
	{
		throw ADP.NotSupported();
	}

	public virtual DataTable GetSchema(string collectionName, string[] restrictionValues)
	{
		throw ADP.NotSupported();
	}

	protected virtual void OnStateChange(StateChangeEventArgs stateChange)
	{
		if (!_suppressStateChangeForReconnection)
		{
			this.StateChange?.Invoke(this, stateChange);
		}
	}

	public abstract void Open();

	public Task OpenAsync()
	{
		return OpenAsync(CancellationToken.None);
	}

	public virtual Task OpenAsync(CancellationToken cancellationToken)
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return Task.FromCanceled(cancellationToken);
		}
		try
		{
			Open();
			return Task.CompletedTask;
		}
		catch (Exception exception)
		{
			return Task.FromException(exception);
		}
	}

	protected virtual ValueTask<DbTransaction> BeginDbTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return new ValueTask<DbTransaction>(Task.FromCanceled<DbTransaction>(cancellationToken));
		}
		try
		{
			return new ValueTask<DbTransaction>(BeginDbTransaction(isolationLevel));
		}
		catch (Exception exception)
		{
			return new ValueTask<DbTransaction>(Task.FromException<DbTransaction>(exception));
		}
	}

	public ValueTask<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return BeginDbTransactionAsync(IsolationLevel.Unspecified, cancellationToken);
	}

	public ValueTask<DbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default(CancellationToken))
	{
		return BeginDbTransactionAsync(isolationLevel, cancellationToken);
	}

	public virtual Task CloseAsync()
	{
		try
		{
			Close();
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

	public virtual Task ChangeDatabaseAsync(string databaseName, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return Task.FromCanceled(cancellationToken);
		}
		try
		{
			ChangeDatabase(databaseName);
			return Task.CompletedTask;
		}
		catch (Exception exception)
		{
			return Task.FromException(exception);
		}
	}
}
