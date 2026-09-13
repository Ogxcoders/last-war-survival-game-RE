namespace Leopotam.EcsLite;

public interface IEcsPreInitSystem : IEcsSystem
{
	void PreInit(IEcsSystems systems);
}
