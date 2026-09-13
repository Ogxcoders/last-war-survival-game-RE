using System.Collections.Generic;

internal class FloatComparer : IEqualityComparer<float>
{
	bool IEqualityComparer<float>.Equals(float x, float y)
	{
		float num = x - y;
		num = ((num > 0f) ? num : (0f - num));
		return num < 0.015f;
	}

	int IEqualityComparer<float>.GetHashCode(float obj)
	{
		return obj.GetHashCode();
	}
}
