namespace Leopotam.EcsLite;

public interface IEcsInitSystem : IEcsSystem
{
	void Init(IEcsSystems systems);
}
