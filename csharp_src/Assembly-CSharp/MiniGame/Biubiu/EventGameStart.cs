using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("游戏开始触发", "事件")]
public struct EventGameStart : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }
}
