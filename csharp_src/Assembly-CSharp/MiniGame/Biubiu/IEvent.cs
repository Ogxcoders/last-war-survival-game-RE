using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public interface IEvent
{
	EcsPackedEntity Sender { get; }

	EcsPackedEntity Target { get; }
}
