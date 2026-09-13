using Leopotam.EcsLite;

namespace MiniGame.Core;

public interface IGameUniqueIDRegister
{
	EcsPackedEntity UniqueIDManager { get; set; }
}
