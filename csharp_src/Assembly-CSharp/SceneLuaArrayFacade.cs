using System.Collections.Generic;

public class SceneLuaArrayFacade
{
	private static LuaArrAccess _longArrayAccess;

	private static Dictionary<long, string> _cityBuildNameIdMap = new Dictionary<long, string>(512);

	public static void InitLongArrayAccess(LuaArrAccess longArrayAccess)
	{
		_longArrayAccess = longArrayAccess;
	}

	public static void UnInitLongArrayAccess()
	{
		_longArrayAccess = null;
	}

	internal static uint GetLongLuaArraySize()
	{
		if (_longArrayAccess != null)
		{
			return _longArrayAccess.GetArrayCapacity();
		}
		return 0u;
	}

	internal static long GetLongLuaArrayValue(int index)
	{
		if (_longArrayAccess != null)
		{
			return _longArrayAccess.GetLong(index);
		}
		return 0L;
	}

	public static void SyncCityBuildNameId(string cityBuildName, long id)
	{
		_cityBuildNameIdMap[id] = cityBuildName;
	}

	internal static bool TryGetCityBuildIdName(int buildId, int buildLevel, out string cityBuildName)
	{
		long key = ((long)buildId << 32) | buildLevel;
		if (_cityBuildNameIdMap.TryGetValue(key, out cityBuildName))
		{
			return true;
		}
		return false;
	}
}
