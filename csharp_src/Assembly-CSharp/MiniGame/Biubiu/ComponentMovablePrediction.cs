using MiniGame.Core;

namespace MiniGame.Biubiu;

public struct ComponentMovablePrediction
{
	public int FrameSync;

	public float TimeSimulate;

	public float TimePrediction;

	public FloatVector3 Position;

	public FloatVector3 PositionSyn;

	public FloatVector3 PositionPrediction;

	public FloatVector3 Forward;
}
