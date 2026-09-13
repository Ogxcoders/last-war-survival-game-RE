using System.IO;

internal abstract class FileActionTask : IQueuedThreadTask
{
	public volatile bool Processed;

	protected readonly string FilePath;

	protected readonly string DirectoryPath;

	protected internal FileActionTask(string id)
	{
		string rootDirectory = FileContentHelper.GetRootDirectory();
		FilePath = Path.Combine(rootDirectory, id + ".bin");
		DirectoryPath = Path.GetDirectoryName(FilePath);
	}

	public virtual void Process()
	{
		Processed = true;
	}

	protected internal abstract void CallBack();
}
