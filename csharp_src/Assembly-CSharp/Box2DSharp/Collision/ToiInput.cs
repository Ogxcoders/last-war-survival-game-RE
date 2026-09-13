using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public struct ToiInput
{
	public DistanceProxy ProxyA;

	public DistanceProxy ProxyB;

	public Sweep SweepA;

	public Sweep SweepB;

	public FP Tmax;
}
