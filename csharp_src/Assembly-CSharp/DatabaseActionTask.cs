using SQLite4Unity3d;

internal abstract class DatabaseActionTask : IQueuedThreadTask
{
	protected SQLiteConnection dbConnection;

	public volatile bool Processed;

	protected internal DatabaseActionTask(SQLiteConnection dbConnection)
	{
		this.dbConnection = dbConnection;
	}

	public virtual void Process()
	{
		Processed = true;
	}

	protected internal abstract void CallBack();
}
