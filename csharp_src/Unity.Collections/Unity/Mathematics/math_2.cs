namespace Unity.Mathematics;

public class math_2
{
	public static int log2_floor(int value)
	{
		return 31 - math.lzcnt(value);
	}
}
