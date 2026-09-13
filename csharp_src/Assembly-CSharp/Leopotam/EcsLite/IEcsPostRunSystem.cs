namespace Leopotam.EcsLite;

public interface IEcsPostRunSystem : IEcsSystem
{
	void PostRun(IEcsSystems systems);
}
