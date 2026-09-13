using Unity.Collections;

namespace UnityEngine.ParticleSystemJobs;

public struct ParticleSystemNativeArray4
{
	public NativeArray<float> x;

	public NativeArray<float> y;

	public NativeArray<float> z;

	public NativeArray<float> w;

	public Vector4 this[int index]
	{
		get
		{
			return new Vector4(x[index], y[index], z[index], w[index]);
		}
		set
		{
			x[index] = value.x;
			y[index] = value.y;
			z[index] = value.z;
			w[index] = value.w;
		}
	}
}
