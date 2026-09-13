using System;
using UnityEngine.Profiling;

namespace UnityEngine.Rendering;

public class ProfilingSampler
{
	internal CustomSampler sampler { get; private set; }

	internal CustomSampler inlineSampler { get; private set; }

	public string name { get; private set; }

	public bool enableRecording
	{
		set
		{
		}
	}

	public float gpuElapsedTime => 0f;

	public int gpuSampleCount => 0;

	public float cpuElapsedTime => 0f;

	public int cpuSampleCount => 0;

	public float inlineCpuElapsedTime => 0f;

	public int inlineCpuSampleCount => 0;

	public static ProfilingSampler Get<TEnum>(TEnum marker) where TEnum : Enum
	{
		TProfilingSampler<TEnum>.samples.TryGetValue(marker, out var value);
		return value;
	}

	public ProfilingSampler(string name)
	{
		sampler = CustomSampler.Create("Dummy_" + name);
		inlineSampler = CustomSampler.Create("Inl_" + name);
		this.name = name;
	}

	internal bool IsValid()
	{
		if (sampler != null)
		{
			return inlineSampler != null;
		}
		return false;
	}

	private ProfilingSampler()
	{
	}
}
