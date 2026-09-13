using Unity.Collections;

namespace UnityEngine.ParticleSystemJobs;

public struct ParticleSystemNativeArray3
{
	public NativeArray<float> x;

	public NativeArray<float> y;

	public NativeArray<float> z;

	public Vector3 this[int index]
	{
		get
		{
			return new Vector3(x[index], y[index], z[index]);
		}
		set
		{
			x[index] = value.x;
			y[index] = value.y;
			z[index] = value.z;
		}
	}
}
