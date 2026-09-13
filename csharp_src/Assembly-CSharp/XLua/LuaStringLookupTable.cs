using System.Collections.Generic;
using UnityEngine;

namespace XLua;

public static class LuaStringLookupTable
{
	private static Dictionary<int, string> _lookupTable;

	public static void Init()
	{
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetLuaStringTable");
		if (luaTable == null)
		{
			Debug.LogError("#LuaStringLUT# LuaStringLookupTable init failed!");
			return;
		}
		_lookupTable = new Dictionary<int, string>(256);
		luaTable.ForEach(delegate(int key, string value)
		{
			_lookupTable.Add(key, value);
		});
		luaTable.Dispose();
	}

	public static string Get(int id)
	{
		if (_lookupTable != null && _lookupTable.TryGetValue(id, out var value))
		{
			return value;
		}
		return string.Empty;
	}
}
