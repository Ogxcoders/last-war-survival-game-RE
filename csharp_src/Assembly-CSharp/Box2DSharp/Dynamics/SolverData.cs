namespace Box2DSharp.Dynamics;

public ref struct SolverData
{
	public readonly TimeStep Step;

	public readonly Position[] Positions;

	public readonly Velocity[] Velocities;

	public SolverData(in TimeStep step, Position[] positions, Velocity[] velocities)
	{
		Step = step;
		Positions = positions;
		Velocities = velocities;
	}
}
