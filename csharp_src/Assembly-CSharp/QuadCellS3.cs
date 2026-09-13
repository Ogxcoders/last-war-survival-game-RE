using System.Runtime.CompilerServices;

public struct QuadCellS3
{
	public float layer0;

	public float layer0Prev;

	public float layer1;

	public float time;

	public QuadCellS3(float v1 = 0f, float v2 = 0f, float v3 = 0f)
	{
		layer0 = v1;
		layer0Prev = v1;
		layer1 = v2;
		time = v3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float Layer0Value(float now)
	{
		float num = now - time;
		if (!(num > 1f))
		{
			return layer0Prev + num / 1f * (layer0 - layer0Prev);
		}
		return layer0;
	}
}
