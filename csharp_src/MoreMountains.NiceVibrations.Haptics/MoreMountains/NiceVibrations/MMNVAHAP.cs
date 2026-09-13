using System;
using System.Collections.Generic;

namespace MoreMountains.NiceVibrations;

[Serializable]
public class MMNVAHAP
{
	public float Version;

	public MMNVAHAPMetadata Metadata;

	public List<MMNVAHAPPattern> Pattern;

	public static float Remap(float x, float A, float B, float C, float D)
	{
		return C + (x - A) / (B - A) * (D - C);
	}
}
