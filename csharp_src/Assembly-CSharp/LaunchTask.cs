public class LaunchTask
{
	public readonly ELaunchTask id;

	public readonly object payload;

	public LaunchTask(ELaunchTask id, object payload = null)
	{
		this.id = id;
		this.payload = payload;
	}

	private LaunchTask()
	{
	}
}
