using System;

namespace Box2DSharp.Dynamics.Joints;

public class JointDef : IDisposable
{
	public Body BodyA;

	public Body BodyB;

	public bool CollideConnected;

	public JointType JointType;

	public object UserData;

	public virtual void Dispose()
	{
		BodyA = null;
		BodyB = null;
		UserData = null;
	}
}
