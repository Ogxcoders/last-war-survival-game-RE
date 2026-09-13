using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics;

public struct FixtureDef
{
	public FP Density;

	public Filter Filter;

	private FP? _friction;

	public bool IsSensor;

	public FP Restitution;

	private FP? _restitutionThreshold;

	public Shape Shape;

	public object UserData;

	public FP Friction
	{
		get
		{
			return _friction.GetValueOrDefault(0.2f);
		}
		set
		{
			_friction = value;
		}
	}

	public FP RestitutionThreshold
	{
		get
		{
			return _restitutionThreshold.GetValueOrDefault(Settings.LengthUnitsPerMeter);
		}
		set
		{
			_restitutionThreshold = value;
		}
	}
}
