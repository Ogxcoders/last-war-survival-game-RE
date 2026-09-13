using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CitySpaceManAnimationListenerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CitySpaceManAnimationListener);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 8, 8, 8);
		Utils.RegisterFunc(L, -3, "OnAnimationEvent_PlayBegin", _m_OnAnimationEvent_PlayBegin);
		Utils.RegisterFunc(L, -3, "OnAnimationEvent_AttackBegin", _m_OnAnimationEvent_AttackBegin);
		Utils.RegisterFunc(L, -3, "OnAnimationEvent_AttackDone", _m_OnAnimationEvent_AttackDone);
		Utils.RegisterFunc(L, -3, "OnAnimationEvent_PlayEnd", _m_OnAnimationEvent_PlayEnd);
		Utils.RegisterFunc(L, -3, "OnAnimationEvent_PlaceFlag", _m_OnAnimationEvent_PlaceFlag);
		Utils.RegisterFunc(L, -3, "OnWalkLeft", _m_OnWalkLeft);
		Utils.RegisterFunc(L, -3, "OnWalkRight", _m_OnWalkRight);
		Utils.RegisterFunc(L, -3, "OnAnimationEvent_ShowTrail", _m_OnAnimationEvent_ShowTrail);
		Utils.RegisterFunc(L, -2, "animation_playBegin", _g_get_animation_playBegin);
		Utils.RegisterFunc(L, -2, "animation_attackBegin", _g_get_animation_attackBegin);
		Utils.RegisterFunc(L, -2, "animation_attackDone", _g_get_animation_attackDone);
		Utils.RegisterFunc(L, -2, "animation_playEnd", _g_get_animation_playEnd);
		Utils.RegisterFunc(L, -2, "animation_placeFlag", _g_get_animation_placeFlag);
		Utils.RegisterFunc(L, -2, "animation_walkLeft", _g_get_animation_walkLeft);
		Utils.RegisterFunc(L, -2, "animation_walkRight", _g_get_animation_walkRight);
		Utils.RegisterFunc(L, -2, "animation_showTrail", _g_get_animation_showTrail);
		Utils.RegisterFunc(L, -1, "animation_playBegin", _s_set_animation_playBegin);
		Utils.RegisterFunc(L, -1, "animation_attackBegin", _s_set_animation_attackBegin);
		Utils.RegisterFunc(L, -1, "animation_attackDone", _s_set_animation_attackDone);
		Utils.RegisterFunc(L, -1, "animation_playEnd", _s_set_animation_playEnd);
		Utils.RegisterFunc(L, -1, "animation_placeFlag", _s_set_animation_placeFlag);
		Utils.RegisterFunc(L, -1, "animation_walkLeft", _s_set_animation_walkLeft);
		Utils.RegisterFunc(L, -1, "animation_walkRight", _s_set_animation_walkRight);
		Utils.RegisterFunc(L, -1, "animation_showTrail", _s_set_animation_showTrail);
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
				CitySpaceManAnimationListener o = new CitySpaceManAnimationListener();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CitySpaceManAnimationListener constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnAnimationEvent_PlayBegin(IntPtr L)
	{
		try
		{
			((CitySpaceManAnimationListener)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnAnimationEvent_PlayBegin();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnAnimationEvent_AttackBegin(IntPtr L)
	{
		try
		{
			((CitySpaceManAnimationListener)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnAnimationEvent_AttackBegin();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnAnimationEvent_AttackDone(IntPtr L)
	{
		try
		{
			((CitySpaceManAnimationListener)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnAnimationEvent_AttackDone();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnAnimationEvent_PlayEnd(IntPtr L)
	{
		try
		{
			((CitySpaceManAnimationListener)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnAnimationEvent_PlayEnd();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnAnimationEvent_PlaceFlag(IntPtr L)
	{
		try
		{
			((CitySpaceManAnimationListener)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnAnimationEvent_PlaceFlag();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnWalkLeft(IntPtr L)
	{
		try
		{
			((CitySpaceManAnimationListener)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnWalkLeft();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnWalkRight(IntPtr L)
	{
		try
		{
			((CitySpaceManAnimationListener)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnWalkRight();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnAnimationEvent_ShowTrail(IntPtr L)
	{
		try
		{
			((CitySpaceManAnimationListener)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnAnimationEvent_ShowTrail();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animation_playBegin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CitySpaceManAnimationListener citySpaceManAnimationListener = (CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, citySpaceManAnimationListener.animation_playBegin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animation_attackBegin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CitySpaceManAnimationListener citySpaceManAnimationListener = (CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, citySpaceManAnimationListener.animation_attackBegin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animation_attackDone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CitySpaceManAnimationListener citySpaceManAnimationListener = (CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, citySpaceManAnimationListener.animation_attackDone);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animation_playEnd(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CitySpaceManAnimationListener citySpaceManAnimationListener = (CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, citySpaceManAnimationListener.animation_playEnd);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animation_placeFlag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CitySpaceManAnimationListener citySpaceManAnimationListener = (CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, citySpaceManAnimationListener.animation_placeFlag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animation_walkLeft(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CitySpaceManAnimationListener citySpaceManAnimationListener = (CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, citySpaceManAnimationListener.animation_walkLeft);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animation_walkRight(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CitySpaceManAnimationListener citySpaceManAnimationListener = (CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, citySpaceManAnimationListener.animation_walkRight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animation_showTrail(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CitySpaceManAnimationListener citySpaceManAnimationListener = (CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, citySpaceManAnimationListener.animation_showTrail);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animation_playBegin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1)).animation_playBegin = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animation_attackBegin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1)).animation_attackBegin = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animation_attackDone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1)).animation_attackDone = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animation_playEnd(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1)).animation_playEnd = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animation_placeFlag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1)).animation_placeFlag = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animation_walkLeft(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1)).animation_walkLeft = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animation_walkRight(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1)).animation_walkRight = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animation_showTrail(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CitySpaceManAnimationListener)objectTranslator.FastGetCSObj(L, 1)).animation_showTrail = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
