namespace Leopotam.EcsLite.Di;

public struct EcsPoolInject<T> : IEcsDataInject where T : struct
{
	public EcsPool<T> Value;

	private string _worldName;

	public static implicit operator EcsPoolInject<T>(string worldName)
	{
		return new EcsPoolInject<T>
		{
			_worldName = worldName
		};
	}

	void IEcsDataInject.Fill(IEcsSystems systems)
	{
		Value = systems.GetWorld(_worldName).GetPool<T>();
	}
}
