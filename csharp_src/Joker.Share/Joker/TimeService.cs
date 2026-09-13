namespace Joker;

public class TimeService : Singleton<TimeService>, IService, IUpdateService
{
	public float RunningTime { get; private set; }

	public float DeltaTime { get; private set; }

	public long Frame { get; private set; }

	public void Awake()
	{
	}

	public void Startup()
	{
	}

	public void Shutdown()
	{
	}

	public void Destroy()
	{
	}

	public void Update(float deltaTime)
	{
		DeltaTime = deltaTime;
		RunningTime += DeltaTime;
		Frame++;
	}
}
