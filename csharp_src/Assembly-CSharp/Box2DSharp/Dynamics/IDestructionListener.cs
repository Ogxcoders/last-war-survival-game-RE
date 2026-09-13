using Box2DSharp.Dynamics.Joints;

namespace Box2DSharp.Dynamics;

public interface IDestructionListener
{
	void SayGoodbye(Joint joint);

	void SayGoodbye(Fixture fixture);
}
