namespace UnityEngine.VFX;

internal enum VFXTaskType
{
	None = 0,
	Spawner = 268435456,
	Initialize = 536870912,
	Update = 805306368,
	Output = 1073741824,
	CameraSort = 805306369,
	ParticlePointOutput = 1073741824,
	ParticleLineOutput = 1073741825,
	ParticleQuadOutput = 1073741826,
	ParticleHexahedronOutput = 1073741827,
	ParticleMeshOutput = 1073741828,
	ParticleTriangleOutput = 1073741829,
	ParticleOctagonOutput = 1073741830,
	ConstantRateSpawner = 268435456,
	BurstSpawner = 268435457,
	PeriodicBurstSpawner = 268435458,
	VariableRateSpawner = 268435459,
	CustomCallbackSpawner = 268435460,
	SetAttributeSpawner = 268435461
}
