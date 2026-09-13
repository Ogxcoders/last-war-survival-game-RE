namespace Joker;

public static class UtilsMask
{
	public static bool IsUpdateFlag(int mask, int flag)
	{
		return (mask & flag) == flag;
	}

	public static int UpdateFlag(int mask, int flag)
	{
		return mask | flag;
	}

	public static int ClearFlag(int mask, int flag)
	{
		return mask & ~flag;
	}

	public static int OnlyToggleFlag(int mask, int flag)
	{
		return mask ^ flag;
	}
}
