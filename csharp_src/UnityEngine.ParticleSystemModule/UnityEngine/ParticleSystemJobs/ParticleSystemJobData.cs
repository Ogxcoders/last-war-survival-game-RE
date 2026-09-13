using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs;

public struct ParticleSystemJobData
{
	public int count { get; }

	public ParticleSystemNativeArray3 positions { get; }

	public ParticleSystemNativeArray3 velocities { get; }

	public ParticleSystemNativeArray3 rotations { get; }

	public ParticleSystemNativeArray3 rotationalSpeeds { get; }

	public ParticleSystemNativeArray3 sizes { get; }

	public NativeArray<Color32> startColors { get; }

	public NativeArray<float> aliveTimePercent { get; }

	public NativeArray<float> inverseStartLifetimes { get; }

	public NativeArray<uint> randomSeeds { get; }

	public ParticleSystemNativeArray4 customData1 { get; }

	public ParticleSystemNativeArray4 customData2 { get; }

	internal unsafe ParticleSystemJobData(ref NativeParticleData nativeData)
	{
		this = default(ParticleSystemJobData);
		count = nativeData.count;
		positions = CreateNativeArray3(ref nativeData.positions, count);
		velocities = CreateNativeArray3(ref nativeData.velocities, count);
		rotations = CreateNativeArray3(ref nativeData.rotations, count);
		rotationalSpeeds = CreateNativeArray3(ref nativeData.rotationalSpeeds, count);
		sizes = CreateNativeArray3(ref nativeData.sizes, count);
		startColors = CreateNativeArray<Color32>(nativeData.startColors, count);
		aliveTimePercent = CreateNativeArray<float>(nativeData.aliveTimePercent, count);
		inverseStartLifetimes = CreateNativeArray<float>(nativeData.inverseStartLifetimes, count);
		randomSeeds = CreateNativeArray<uint>(nativeData.randomSeeds, count);
		customData1 = CreateNativeArray4(ref nativeData.customData1, count);
		customData2 = CreateNativeArray4(ref nativeData.customData2, count);
	}

	internal unsafe NativeArray<T> CreateNativeArray<T>(void* src, int count) where T : struct
	{
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(src, count, Allocator.Invalid);
	}

	internal unsafe ParticleSystemNativeArray3 CreateNativeArray3(ref NativeParticleData.Array3 ptrs, int count)
	{
		return new ParticleSystemNativeArray3
		{
			x = CreateNativeArray<float>(ptrs.x, count),
			y = CreateNativeArray<float>(ptrs.y, count),
			z = CreateNativeArray<float>(ptrs.z, count)
		};
	}

	internal unsafe ParticleSystemNativeArray4 CreateNativeArray4(ref NativeParticleData.Array4 ptrs, int count)
	{
		return new ParticleSystemNativeArray4
		{
			x = CreateNativeArray<float>(ptrs.x, count),
			y = CreateNativeArray<float>(ptrs.y, count),
			z = CreateNativeArray<float>(ptrs.z, count),
			w = CreateNativeArray<float>(ptrs.w, count)
		};
	}
}
