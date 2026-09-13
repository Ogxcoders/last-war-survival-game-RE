using System;
using UnityEngine;

namespace FibMatrix.PerfTools;

[Serializable]
public class CDParticleMonitor
{
	[Range(0.01f, 5f)]
	public float sampleInterval = 0.5f;
}
