using Box2DSharp.Foreign;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("实体死亡", "事件")]
public struct EventEntityDie : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public int UniqueId { get; }

	public S5Game.S5GameColliderLayer Layer { get; }

	public EventEntityDie(EcsPackedEntity sender, EcsPackedEntity target, S5Game.S5GameColliderLayer layer, int uniqueId)
	{
		Sender = sender;
		Target = target;
		UniqueId = uniqueId;
		Layer = layer;
	}
}
