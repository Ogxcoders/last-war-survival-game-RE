using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts;

public struct ContactImpulse
{
	public FixedArray2<FP> NormalImpulses;

	public FixedArray2<FP> TangentImpulses;

	public int Count;
}
