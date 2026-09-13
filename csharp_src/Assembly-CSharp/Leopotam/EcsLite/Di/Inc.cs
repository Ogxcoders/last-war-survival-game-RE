namespace Leopotam.EcsLite.Di;

public struct Inc<T1> : IEcsInclude where T1 : struct
{
	public EcsPool<T1> Inc1;

	public EcsWorld.Mask Fill(EcsWorld world)
	{
		Inc1 = world.GetPool<T1>();
		return world.Filter<T1>();
	}

	public EcsWorld.Mask Exclude(EcsWorld.Mask mask)
	{
		return mask.Exc<T1>();
	}
}
public struct Inc<T1, T2> : IEcsInclude where T1 : struct where T2 : struct
{
	public EcsPool<T1> Inc1;

	public EcsPool<T2> Inc2;

	public EcsWorld.Mask Fill(EcsWorld world)
	{
		Inc1 = world.GetPool<T1>();
		Inc2 = world.GetPool<T2>();
		return world.Filter<T1>().Inc<T2>();
	}
}
public struct Inc<T1, T2, T3> : IEcsInclude where T1 : struct where T2 : struct where T3 : struct
{
	public EcsPool<T1> Inc1;

	public EcsPool<T2> Inc2;

	public EcsPool<T3> Inc3;

	public EcsWorld.Mask Fill(EcsWorld world)
	{
		Inc1 = world.GetPool<T1>();
		Inc2 = world.GetPool<T2>();
		Inc3 = world.GetPool<T3>();
		return world.Filter<T1>().Inc<T2>().Inc<T3>();
	}
}
public struct Inc<T1, T2, T3, T4> : IEcsInclude where T1 : struct where T2 : struct where T3 : struct where T4 : struct
{
	public EcsPool<T1> Inc1;

	public EcsPool<T2> Inc2;

	public EcsPool<T3> Inc3;

	public EcsPool<T4> Inc4;

	public EcsWorld.Mask Fill(EcsWorld world)
	{
		Inc1 = world.GetPool<T1>();
		Inc2 = world.GetPool<T2>();
		Inc3 = world.GetPool<T3>();
		Inc4 = world.GetPool<T4>();
		return world.Filter<T1>().Inc<T2>().Inc<T3>()
			.Inc<T4>();
	}
}
public struct Inc<T1, T2, T3, T4, T5> : IEcsInclude where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct
{
	public EcsPool<T1> Inc1;

	public EcsPool<T2> Inc2;

	public EcsPool<T3> Inc3;

	public EcsPool<T4> Inc4;

	public EcsPool<T5> Inc5;

	public EcsWorld.Mask Fill(EcsWorld world)
	{
		Inc1 = world.GetPool<T1>();
		Inc2 = world.GetPool<T2>();
		Inc3 = world.GetPool<T3>();
		Inc4 = world.GetPool<T4>();
		Inc5 = world.GetPool<T5>();
		return world.Filter<T1>().Inc<T2>().Inc<T3>()
			.Inc<T4>()
			.Inc<T5>();
	}
}
public struct Inc<T1, T2, T3, T4, T5, T6> : IEcsInclude where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct
{
	public EcsPool<T1> Inc1;

	public EcsPool<T2> Inc2;

	public EcsPool<T3> Inc3;

	public EcsPool<T4> Inc4;

	public EcsPool<T5> Inc5;

	public EcsPool<T6> Inc6;

	public EcsWorld.Mask Fill(EcsWorld world)
	{
		Inc1 = world.GetPool<T1>();
		Inc2 = world.GetPool<T2>();
		Inc3 = world.GetPool<T3>();
		Inc4 = world.GetPool<T4>();
		Inc5 = world.GetPool<T5>();
		Inc6 = world.GetPool<T6>();
		return world.Filter<T1>().Inc<T2>().Inc<T3>()
			.Inc<T4>()
			.Inc<T5>()
			.Inc<T6>();
	}
}
public struct Inc<T1, T2, T3, T4, T5, T6, T7> : IEcsInclude where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct
{
	public EcsPool<T1> Inc1;

	public EcsPool<T2> Inc2;

	public EcsPool<T3> Inc3;

	public EcsPool<T4> Inc4;

	public EcsPool<T5> Inc5;

	public EcsPool<T6> Inc6;

	public EcsPool<T7> Inc7;

	public EcsWorld.Mask Fill(EcsWorld world)
	{
		Inc1 = world.GetPool<T1>();
		Inc2 = world.GetPool<T2>();
		Inc3 = world.GetPool<T3>();
		Inc4 = world.GetPool<T4>();
		Inc5 = world.GetPool<T5>();
		Inc6 = world.GetPool<T6>();
		Inc7 = world.GetPool<T7>();
		return world.Filter<T1>().Inc<T2>().Inc<T3>()
			.Inc<T4>()
			.Inc<T5>()
			.Inc<T6>()
			.Inc<T7>();
	}
}
public struct Inc<T1, T2, T3, T4, T5, T6, T7, T8> : IEcsInclude where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct
{
	public EcsPool<T1> Inc1;

	public EcsPool<T2> Inc2;

	public EcsPool<T3> Inc3;

	public EcsPool<T4> Inc4;

	public EcsPool<T5> Inc5;

	public EcsPool<T6> Inc6;

	public EcsPool<T7> Inc7;

	public EcsPool<T8> Inc8;

	public EcsWorld.Mask Fill(EcsWorld world)
	{
		Inc1 = world.GetPool<T1>();
		Inc2 = world.GetPool<T2>();
		Inc3 = world.GetPool<T3>();
		Inc4 = world.GetPool<T4>();
		Inc5 = world.GetPool<T5>();
		Inc6 = world.GetPool<T6>();
		Inc7 = world.GetPool<T7>();
		Inc8 = world.GetPool<T8>();
		return world.Filter<T1>().Inc<T2>().Inc<T3>()
			.Inc<T4>()
			.Inc<T5>()
			.Inc<T6>()
			.Inc<T7>()
			.Inc<T8>();
	}
}
