using System.Runtime.CompilerServices;

public class LuaJitType
{
	public static uint LJ_TISNUM;

	public const uint LJ_TNUMX = 4294967282u;

	public static bool LJ_DUALNUM;

	public static bool GC64;

	static LuaJitType()
	{
		LJ_TISNUM = 4294967282u;
		LJ_DUALNUM = true;
		GC64 = false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsIntType(ref LuaJitTValue tv)
	{
		if (!GC64)
		{
			if (LJ_DUALNUM)
			{
				return tv.it == LJ_TISNUM;
			}
			return false;
		}
		return (uint)(tv.it64 >> 47) == LJ_TISNUM;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double GetDouble(ref LuaJitTValue tv)
	{
		if (IsIntType(ref tv))
		{
			return tv.i;
		}
		return tv.n;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetInt(ref LuaJitTValue tv)
	{
		if (IsIntType(ref tv))
		{
			return tv.i;
		}
		return (int)tv.n;
	}
}
