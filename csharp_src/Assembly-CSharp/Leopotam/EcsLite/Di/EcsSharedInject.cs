namespace Leopotam.EcsLite.Di;

public struct EcsSharedInject<T> : IEcsDataInject where T : class
{
	public T Value;

	void IEcsDataInject.Fill(IEcsSystems systems)
	{
		Value = systems.GetWorld().GetShared<T>();
	}
}
