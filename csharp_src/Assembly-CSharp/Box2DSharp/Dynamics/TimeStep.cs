using Box2DSharp.Common;

namespace Box2DSharp.Dynamics;

public struct TimeStep
{
	public FP Dt;

	public FP InvDt;

	public FP DtRatio;

	public int VelocityIterations;

	public int PositionIterations;

	public bool WarmStarting;
}
