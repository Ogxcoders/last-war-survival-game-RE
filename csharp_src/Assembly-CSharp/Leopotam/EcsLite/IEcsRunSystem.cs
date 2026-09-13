namespace Leopotam.EcsLite;

public interface IEcsRunSystem : IEcsSystem
{
	void Run(IEcsSystems systems);
}
