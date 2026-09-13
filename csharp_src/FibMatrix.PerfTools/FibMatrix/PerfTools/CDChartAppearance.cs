using System;
using UnityEngine;

namespace FibMatrix.PerfTools;

[Serializable]
public class CDChartAppearance
{
	[Range(0f, 1f)]
	public float backroungTransparent = 0.25f;

	[Range(0f, 1f)]
	public float chartTransparent = 1f;
}
