using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SceneSpriteSliderWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SceneSpriteSlider);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 0, 0);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
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
				SceneSpriteSlider o = new SceneSpriteSlider();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SceneSpriteSlider constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			SceneSpriteSlider obj = (SceneSpriteSlider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long maxValue = Lua.lua_toint64(L, 2);
			long curValue = Lua.lua_toint64(L, 3);
			obj.Init(maxValue, curValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
