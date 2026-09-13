namespace Leopotam.EcsLite.Di;

public struct EcsWorldInject : IEcsDataInject
{
	public EcsWorld Value;

	private string _worldName;

	public static implicit operator EcsWorldInject(string worldName)
	{
		return new EcsWorldInject
		{
			_worldName = worldName
		};
	}

	void IEcsDataInject.Fill(IEcsSystems systems)
	{
		Value = systems.GetWorld(_worldName);
	}
}
