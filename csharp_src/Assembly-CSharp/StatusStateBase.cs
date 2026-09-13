using Protobuf;

public abstract class StatusStateBase
{
	public enum StateType
	{
		Continue = 1,
		Finish
	}

	protected long endTime;

	protected float timer;

	public int statusId = -1;

	public long startTime;

	protected StatusStateBase(int statusId, long startTime, long endTime)
	{
		this.statusId = statusId;
		this.endTime = endTime;
		this.startTime = startTime;
		timer = 0f;
	}

	public abstract void Start();

	public abstract StateType Update(float deltaTime);

	public virtual void UpdateStatus(Status newData)
	{
		endTime = newData.ExpireTime;
	}

	public virtual void Dispose()
	{
		endTime = 0L;
		timer = 0f;
	}
}
