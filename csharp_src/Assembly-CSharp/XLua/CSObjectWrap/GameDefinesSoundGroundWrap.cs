using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesSoundGroundWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.SoundGround);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 8, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Music", "Music");
		Utils.RegisterObject(L, translator, -4, "Sound", "Sound");
		Utils.RegisterObject(L, translator, -4, "Effect", "Effect");
		Utils.RegisterObject(L, translator, -4, "AMBSound", "AMBSound");
		Utils.RegisterObject(L, translator, -4, "Dub", "Dub");
		Utils.RegisterObject(L, translator, -4, "Hero", "Hero");
		Utils.RegisterObject(L, translator, -4, "Timeline", "Timeline");
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 1)
			{
				GameDefines.SoundGround o = new GameDefines.SoundGround();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameDefines.SoundGround constructor!");
	}
}
