using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BulletMotionEditorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BulletMotionEditor);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 11, 11);
		Utils.RegisterFunc(L, -2, "SkillId", _g_get_SkillId);
		Utils.RegisterFunc(L, -2, "AttackRange", _g_get_AttackRange);
		Utils.RegisterFunc(L, -2, "Cooldown", _g_get_Cooldown);
		Utils.RegisterFunc(L, -2, "BulletEffect", _g_get_BulletEffect);
		Utils.RegisterFunc(L, -2, "HeightWidthRatio", _g_get_HeightWidthRatio);
		Utils.RegisterFunc(L, -2, "Height", _g_get_Height);
		Utils.RegisterFunc(L, -2, "FlySpeed", _g_get_FlySpeed);
		Utils.RegisterFunc(L, -2, "MotionCurve", _g_get_MotionCurve);
		Utils.RegisterFunc(L, -2, "CurveString", _g_get_CurveString);
		Utils.RegisterFunc(L, -2, "IsUltimate", _g_get_IsUltimate);
		Utils.RegisterFunc(L, -2, "Rectangle", _g_get_Rectangle);
		Utils.RegisterFunc(L, -1, "SkillId", _s_set_SkillId);
		Utils.RegisterFunc(L, -1, "AttackRange", _s_set_AttackRange);
		Utils.RegisterFunc(L, -1, "Cooldown", _s_set_Cooldown);
		Utils.RegisterFunc(L, -1, "BulletEffect", _s_set_BulletEffect);
		Utils.RegisterFunc(L, -1, "HeightWidthRatio", _s_set_HeightWidthRatio);
		Utils.RegisterFunc(L, -1, "Height", _s_set_Height);
		Utils.RegisterFunc(L, -1, "FlySpeed", _s_set_FlySpeed);
		Utils.RegisterFunc(L, -1, "MotionCurve", _s_set_MotionCurve);
		Utils.RegisterFunc(L, -1, "CurveString", _s_set_CurveString);
		Utils.RegisterFunc(L, -1, "IsUltimate", _s_set_IsUltimate);
		Utils.RegisterFunc(L, -1, "Rectangle", _s_set_Rectangle);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "CurveToString", _m_CurveToString_xlua_st_);
		Utils.RegisterFunc(L, -4, "StringToCurve", _m_StringToCurve_xlua_st_);
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
				BulletMotionEditor o = new BulletMotionEditor();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BulletMotionEditor constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CurveToString_xlua_st_(IntPtr L)
	{
		try
		{
			string str = BulletMotionEditor.CurveToString((AnimationCurve)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(AnimationCurve)));
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StringToCurve_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AnimationCurve o = BulletMotionEditor.StringToCurve(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SkillId(IntPtr L)
	{
		try
		{
			BulletMotionEditor bulletMotionEditor = (BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, bulletMotionEditor.SkillId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AttackRange(IntPtr L)
	{
		try
		{
			BulletMotionEditor bulletMotionEditor = (BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, bulletMotionEditor.AttackRange);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Cooldown(IntPtr L)
	{
		try
		{
			BulletMotionEditor bulletMotionEditor = (BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, bulletMotionEditor.Cooldown);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BulletEffect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BulletMotionEditor bulletMotionEditor = (BulletMotionEditor)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, bulletMotionEditor.BulletEffect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_HeightWidthRatio(IntPtr L)
	{
		try
		{
			BulletMotionEditor bulletMotionEditor = (BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, bulletMotionEditor.HeightWidthRatio);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Height(IntPtr L)
	{
		try
		{
			BulletMotionEditor bulletMotionEditor = (BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, bulletMotionEditor.Height);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FlySpeed(IntPtr L)
	{
		try
		{
			BulletMotionEditor bulletMotionEditor = (BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, bulletMotionEditor.FlySpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MotionCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BulletMotionEditor bulletMotionEditor = (BulletMotionEditor)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, bulletMotionEditor.MotionCurve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurveString(IntPtr L)
	{
		try
		{
			BulletMotionEditor bulletMotionEditor = (BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, bulletMotionEditor.CurveString);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsUltimate(IntPtr L)
	{
		try
		{
			BulletMotionEditor bulletMotionEditor = (BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, bulletMotionEditor.IsUltimate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Rectangle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BulletMotionEditor bulletMotionEditor = (BulletMotionEditor)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, bulletMotionEditor.Rectangle);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SkillId(IntPtr L)
	{
		try
		{
			((BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SkillId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_AttackRange(IntPtr L)
	{
		try
		{
			((BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).AttackRange = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Cooldown(IntPtr L)
	{
		try
		{
			((BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Cooldown = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_BulletEffect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BulletMotionEditor)objectTranslator.FastGetCSObj(L, 1)).BulletEffect = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_HeightWidthRatio(IntPtr L)
	{
		try
		{
			((BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HeightWidthRatio = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Height(IntPtr L)
	{
		try
		{
			((BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Height = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_FlySpeed(IntPtr L)
	{
		try
		{
			((BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FlySpeed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_MotionCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BulletMotionEditor)objectTranslator.FastGetCSObj(L, 1)).MotionCurve = (AnimationCurve)objectTranslator.GetObject(L, 2, typeof(AnimationCurve));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CurveString(IntPtr L)
	{
		try
		{
			((BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CurveString = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsUltimate(IntPtr L)
	{
		try
		{
			((BulletMotionEditor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsUltimate = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Rectangle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BulletMotionEditor bulletMotionEditor = (BulletMotionEditor)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RectInt v);
			bulletMotionEditor.Rectangle = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
