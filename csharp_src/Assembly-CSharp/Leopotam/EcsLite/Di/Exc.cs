using System.Runtime.InteropServices;

namespace Leopotam.EcsLite.Di;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Exc<T1> : IEcsExclude where T1 : struct
{
	public EcsWorld.Mask Fill(EcsWorld.Mask mask)
	{
		return mask.Exc<T1>();
	}
}
[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Exc<T1, T2> : IEcsExclude where T1 : struct where T2 : struct
{
	public EcsWorld.Mask Fill(EcsWorld.Mask mask)
	{
		return mask.Exc<T1>().Exc<T2>();
	}
}
[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Exc<T1, T2, T3> : IEcsExclude where T1 : struct where T2 : struct where T3 : struct
{
	public EcsWorld.Mask Fill(EcsWorld.Mask mask)
	{
		return mask.Exc<T1>().Exc<T2>().Exc<T3>();
	}
}
[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Exc<T1, T2, T3, T4> : IEcsExclude where T1 : struct where T2 : struct where T3 : struct where T4 : struct
{
	public EcsWorld.Mask Fill(EcsWorld.Mask mask)
	{
		return mask.Exc<T1>().Exc<T2>().Exc<T3>()
			.Exc<T4>();
	}
}
