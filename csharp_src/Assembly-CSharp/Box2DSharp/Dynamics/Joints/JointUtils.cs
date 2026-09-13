using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public static class JointUtils
{
	public static void LinearStiffness(out FP stiffness, out FP damping, FP frequencyHertz, FP dampingRatio, Body bodyA, Body bodyB)
	{
		FP x = bodyA.Mass;
		FP y = bodyB.Mass;
		FP x2 = ((x > 0f && y > 0f) ? (x * y / (x + y)) : ((!(x > 0f)) ? y : x));
		FP y2 = (FP)2f * Settings.Pi * frequencyHertz;
		stiffness = x2 * y2 * y2;
		damping = (FP)2f * x2 * dampingRatio * y2;
	}

	public static void AngularStiffness(out FP stiffness, out FP damping, FP frequencyHertz, FP dampingRatio, Body bodyA, Body bodyB)
	{
		FP x = bodyA.Inertia;
		FP y = bodyB.Inertia;
		FP x2 = ((x > 0f && y > 0f) ? (x * y / (x + y)) : ((!(x > 0f)) ? y : x));
		FP y2 = (FP)2f * Settings.Pi * frequencyHertz;
		stiffness = x2 * y2 * y2;
		damping = (FP)2f * x2 * dampingRatio * y2;
	}
}
