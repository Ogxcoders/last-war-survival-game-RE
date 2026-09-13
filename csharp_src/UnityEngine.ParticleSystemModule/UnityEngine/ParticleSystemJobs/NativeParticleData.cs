namespace UnityEngine.ParticleSystemJobs;

internal struct NativeParticleData
{
	internal struct Array3
	{
		internal unsafe float* x;

		internal unsafe float* y;

		internal unsafe float* z;
	}

	internal struct Array4
	{
		internal unsafe float* x;

		internal unsafe float* y;

		internal unsafe float* z;

		internal unsafe float* w;
	}

	internal int count;

	internal Array3 positions;

	internal Array3 velocities;

	internal Array3 rotations;

	internal Array3 rotationalSpeeds;

	internal Array3 sizes;

	internal unsafe void* startColors;

	internal unsafe void* aliveTimePercent;

	internal unsafe void* inverseStartLifetimes;

	internal unsafe void* randomSeeds;

	internal Array4 customData1;

	internal Array4 customData2;
}
