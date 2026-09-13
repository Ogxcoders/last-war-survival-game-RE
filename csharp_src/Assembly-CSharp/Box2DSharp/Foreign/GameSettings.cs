using Box2DSharp.Common;

namespace Box2DSharp.Foreign;

public class GameSettings
{
	public FVector2 Gravity;

	public int VelocityIterations;

	public int PositionIterations;

	public bool EnableSleep;

	public bool EnableSubStepping;

	public bool EnableWarmStarting = true;

	public bool EnableContinuous = true;

	public bool Pause;

	public GameSettings()
	{
		Reset();
	}

	private void Reset()
	{
		Gravity = new FVector2(0, -10);
		VelocityIterations = 8;
		PositionIterations = 3;
		EnableSleep = true;
		EnableSubStepping = false;
		EnableWarmStarting = true;
		EnableContinuous = true;
		Pause = false;
	}
}
