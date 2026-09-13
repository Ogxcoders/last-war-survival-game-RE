using System.Runtime.CompilerServices;
using Unity.Mathematics;

public static class MathematicsExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Overlaps(this int4 half4, int4 other)
	{
		if (other.x < half4.z && other.z > half4.x && other.y < half4.w)
		{
			return other.w > half4.y;
		}
		return false;
	}
}
