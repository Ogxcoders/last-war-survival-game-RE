using Box2DSharp.Collision.Collider;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("碰撞", "事件")]
public struct EventPhysicCollection : IEvent
{
	public WorldManifold Manifold;

	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public int SenderLayer { get; }

	public int TargetLayer { get; }

	public int SendFixtureType { get; }

	public int TargetFixtureType { get; }

	public EventPhysicCollection(EcsPackedEntity sender, EcsPackedEntity target, WorldManifold worldManifold, int senderLayer, int targetLayer, int senderFixtureType, int targetFixtureType)
	{
		Sender = sender;
		Target = target;
		SenderLayer = senderLayer;
		TargetLayer = targetLayer;
		Manifold = worldManifold;
		SendFixtureType = senderFixtureType;
		TargetFixtureType = targetFixtureType;
	}
}
