using System;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIGridLayoutGroupWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GridLayoutGroup);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 6, 6);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputHorizontal", _m_CalculateLayoutInputHorizontal);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputVertical", _m_CalculateLayoutInputVertical);
		Utils.RegisterFunc(L, -3, "SetLayoutHorizontal", _m_SetLayoutHorizontal);
		Utils.RegisterFunc(L, -3, "SetLayoutVertical", _m_SetLayoutVertical);
		Utils.RegisterFunc(L, -2, "startCorner", _g_get_startCorner);
		Utils.RegisterFunc(L, -2, "startAxis", _g_get_startAxis);
		Utils.RegisterFunc(L, -2, "cellSize", _g_get_cellSize);
		Utils.RegisterFunc(L, -2, "spacing", _g_get_spacing);
		Utils.RegisterFunc(L, -2, "constraint", _g_get_constraint);
		Utils.RegisterFunc(L, -2, "constraintCount", _g_get_constraintCount);
		Utils.RegisterFunc(L, -1, "startCorner", _s_set_startCorner);
		Utils.RegisterFunc(L, -1, "startAxis", _s_set_startAxis);
		Utils.RegisterFunc(L, -1, "cellSize", _s_set_cellSize);
		Utils.RegisterFunc(L, -1, "spacing", _s_set_spacing);
		Utils.RegisterFunc(L, -1, "constraint", _s_set_constraint);
		Utils.RegisterFunc(L, -1, "constraintCount", _s_set_constraintCount);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.GridLayoutGroup does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateLayoutInputHorizontal(IntPtr L)
	{
		try
		{
			((GridLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputHorizontal();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateLayoutInputVertical(IntPtr L)
	{
		try
		{
			((GridLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLayoutHorizontal(IntPtr L)
	{
		try
		{
			((GridLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutHorizontal();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLayoutVertical(IntPtr L)
	{
		try
		{
			((GridLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startCorner(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIGridLayoutGroupCorner(L, gridLayoutGroup.startCorner);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startAxis(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIGridLayoutGroupAxis(L, gridLayoutGroup.startAxis);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cellSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, gridLayoutGroup.cellSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spacing(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, gridLayoutGroup.spacing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_constraint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIGridLayoutGroupConstraint(L, gridLayoutGroup.constraint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_constraintCount(IntPtr L)
	{
		try
		{
			GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, gridLayoutGroup.constraintCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startCorner(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out GridLayoutGroup.Corner val);
			gridLayoutGroup.startCorner = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startAxis(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out GridLayoutGroup.Axis val);
			gridLayoutGroup.startAxis = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cellSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			gridLayoutGroup.cellSize = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_spacing(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			gridLayoutGroup.spacing = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_constraint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out GridLayoutGroup.Constraint val);
			gridLayoutGroup.constraint = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_constraintCount(IntPtr L)
	{
		try
		{
			((GridLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).constraintCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
