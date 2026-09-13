using Box2DSharp.Common;

namespace Box2DSharp.Ropes;

public class RopeTuning
{
	public StretchingModel StretchingModel;

	public BendingModel BendingModel;

	public FP Damping;

	public FP StretchStiffness;

	public FP StretchHertz;

	public FP StretchDamping;

	public FP BendStiffness;

	public FP BendHertz;

	public FP BendDamping;

	public bool Isometric;

	public bool FixedEffectiveMass;

	public bool WarmStart;

	public RopeTuning()
	{
		StretchingModel = StretchingModel.PbdStretchingModel;
		BendingModel = BendingModel.PbdAngleBendingModel;
		Damping = 0f;
		StretchStiffness = 1f;
		StretchHertz = 1f;
		StretchDamping = 0f;
		BendStiffness = 0.5f;
		BendHertz = 1f;
		BendDamping = 0f;
		Isometric = false;
		FixedEffectiveMass = false;
		WarmStart = false;
	}
}
