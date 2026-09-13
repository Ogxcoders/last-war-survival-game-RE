using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FireworkParticleControllerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(FireworkParticleController);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 0, 0);
		Utils.RegisterFunc(L, -3, "Configure", _m_Configure);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
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
				FireworkParticleController o = new FireworkParticleController();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FireworkParticleController constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Configure(IntPtr L)
	{
		try
		{
			FireworkParticleController obj = (FireworkParticleController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float duration = (float)Lua.lua_tonumber(L, 2);
			float cycleTime = (float)Lua.lua_tonumber(L, 3);
			float normalizedProgress = (float)Lua.lua_tonumber(L, 4);
			obj.Configure(duration, cycleTime, normalizedProgress);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
