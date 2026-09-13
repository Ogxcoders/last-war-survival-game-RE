namespace Leopotam.EcsLite.Di;

public struct EcsFilterInject<TInc> : IEcsDataInject where TInc : struct, IEcsInclude
{
	public EcsFilter Value;

	public TInc Pools;

	private string _worldName;

	public static implicit operator EcsFilterInject<TInc>(string worldName)
	{
		return new EcsFilterInject<TInc>
		{
			_worldName = worldName
		};
	}

	void IEcsDataInject.Fill(IEcsSystems systems)
	{
		Pools = default(TInc);
		Value = Pools.Fill(systems.GetWorld(_worldName)).End();
	}
}
public struct EcsFilterInject<TInc, TExc> : IEcsDataInject where TInc : struct, IEcsInclude where TExc : struct, IEcsExclude
{
	public EcsFilter Value;

	public TInc Pools;

	private string _worldName;

	public static implicit operator EcsFilterInject<TInc, TExc>(string worldName)
	{
		return new EcsFilterInject<TInc, TExc>
		{
			_worldName = worldName
		};
	}

	void IEcsDataInject.Fill(IEcsSystems systems)
	{
		Pools = default(TInc);
		Value = default(TExc).Fill(Pools.Fill(systems.GetWorld(_worldName))).End();
	}
}
