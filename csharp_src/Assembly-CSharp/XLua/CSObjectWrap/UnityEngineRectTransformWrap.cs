using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRectTransformWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(RectTransform);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 40, 9, 8);
		Utils.RegisterFunc(L, -3, "ForceUpdateRectTransforms", _m_ForceUpdateRectTransforms);
		Utils.RegisterFunc(L, -3, "GetLocalCorners", _m_GetLocalCorners);
		Utils.RegisterFunc(L, -3, "GetWorldCorners", _m_GetWorldCorners);
		Utils.RegisterFunc(L, -3, "SetInsetAndSizeFromParentEdge", _m_SetInsetAndSizeFromParentEdge);
		Utils.RegisterFunc(L, -3, "SetSizeWithCurrentAnchors", _m_SetSizeWithCurrentAnchors);
		Utils.RegisterFunc(L, -3, "DOAnchorPos", _m_DOAnchorPos);
		Utils.RegisterFunc(L, -3, "DOAnchorPosX", _m_DOAnchorPosX);
		Utils.RegisterFunc(L, -3, "DOAnchorPosY", _m_DOAnchorPosY);
		Utils.RegisterFunc(L, -3, "DOAnchorPos3D", _m_DOAnchorPos3D);
		Utils.RegisterFunc(L, -3, "DOAnchorPos3DX", _m_DOAnchorPos3DX);
		Utils.RegisterFunc(L, -3, "DOAnchorPos3DY", _m_DOAnchorPos3DY);
		Utils.RegisterFunc(L, -3, "DOAnchorPos3DZ", _m_DOAnchorPos3DZ);
		Utils.RegisterFunc(L, -3, "DOAnchorMax", _m_DOAnchorMax);
		Utils.RegisterFunc(L, -3, "DOAnchorMin", _m_DOAnchorMin);
		Utils.RegisterFunc(L, -3, "DOPivot", _m_DOPivot);
		Utils.RegisterFunc(L, -3, "DOPivotX", _m_DOPivotX);
		Utils.RegisterFunc(L, -3, "DOPivotY", _m_DOPivotY);
		Utils.RegisterFunc(L, -3, "DOSizeDelta", _m_DOSizeDelta);
		Utils.RegisterFunc(L, -3, "DOPunchAnchorPos", _m_DOPunchAnchorPos);
		Utils.RegisterFunc(L, -3, "DOShakeAnchorPos", _m_DOShakeAnchorPos);
		Utils.RegisterFunc(L, -3, "DOJumpAnchorPos", _m_DOJumpAnchorPos);
		Utils.RegisterFunc(L, -3, "Set_offsetMax", _m_Set_offsetMax);
		Utils.RegisterFunc(L, -3, "Get_offsetMax", _m_Get_offsetMax);
		Utils.RegisterFunc(L, -3, "Set_offsetMin", _m_Set_offsetMin);
		Utils.RegisterFunc(L, -3, "Get_offsetMin", _m_Get_offsetMin);
		Utils.RegisterFunc(L, -3, "Set_anchorMin", _m_Set_anchorMin);
		Utils.RegisterFunc(L, -3, "Get_anchorMin", _m_Get_anchorMin);
		Utils.RegisterFunc(L, -3, "Set_anchorMax", _m_Set_anchorMax);
		Utils.RegisterFunc(L, -3, "Get_anchorMax", _m_Get_anchorMax);
		Utils.RegisterFunc(L, -3, "Set_anchoredPosition", _m_Set_anchoredPosition);
		Utils.RegisterFunc(L, -3, "Get_anchoredPosition", _m_Get_anchoredPosition);
		Utils.RegisterFunc(L, -3, "Set_pivot", _m_Set_pivot);
		Utils.RegisterFunc(L, -3, "Get_pivot", _m_Get_pivot);
		Utils.RegisterFunc(L, -3, "Set_sizeDelta", _m_Set_sizeDelta);
		Utils.RegisterFunc(L, -3, "Set_sizeDelta_x", _m_Set_sizeDelta_x);
		Utils.RegisterFunc(L, -3, "Set_sizeDelta_y", _m_Set_sizeDelta_y);
		Utils.RegisterFunc(L, -3, "Get_sizeDelta", _m_Get_sizeDelta);
		Utils.RegisterFunc(L, -3, "Get_sizeDelta_x", _m_Get_sizeDelta_x);
		Utils.RegisterFunc(L, -3, "Get_sizeDelta_y", _m_Get_sizeDelta_y);
		Utils.RegisterFunc(L, -3, "Get_worldCorners_x", _m_Get_worldCorners_x);
		Utils.RegisterFunc(L, -2, "rect", _g_get_rect);
		Utils.RegisterFunc(L, -2, "anchorMin", _g_get_anchorMin);
		Utils.RegisterFunc(L, -2, "anchorMax", _g_get_anchorMax);
		Utils.RegisterFunc(L, -2, "anchoredPosition", _g_get_anchoredPosition);
		Utils.RegisterFunc(L, -2, "sizeDelta", _g_get_sizeDelta);
		Utils.RegisterFunc(L, -2, "pivot", _g_get_pivot);
		Utils.RegisterFunc(L, -2, "anchoredPosition3D", _g_get_anchoredPosition3D);
		Utils.RegisterFunc(L, -2, "offsetMin", _g_get_offsetMin);
		Utils.RegisterFunc(L, -2, "offsetMax", _g_get_offsetMax);
		Utils.RegisterFunc(L, -1, "anchorMin", _s_set_anchorMin);
		Utils.RegisterFunc(L, -1, "anchorMax", _s_set_anchorMax);
		Utils.RegisterFunc(L, -1, "anchoredPosition", _s_set_anchoredPosition);
		Utils.RegisterFunc(L, -1, "sizeDelta", _s_set_sizeDelta);
		Utils.RegisterFunc(L, -1, "pivot", _s_set_pivot);
		Utils.RegisterFunc(L, -1, "anchoredPosition3D", _s_set_anchoredPosition3D);
		Utils.RegisterFunc(L, -1, "offsetMin", _s_set_offsetMin);
		Utils.RegisterFunc(L, -1, "offsetMax", _s_set_offsetMax);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "reapplyDrivenProperties", _e_reapplyDrivenProperties);
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
				RectTransform o = new RectTransform();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceUpdateRectTransforms(IntPtr L)
	{
		try
		{
			((RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceUpdateRectTransforms();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLocalCorners(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			Vector3[] fourCornersArray = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
			rectTransform.GetLocalCorners(fourCornersArray);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldCorners(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			Vector3[] fourCornersArray = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
			rectTransform.GetWorldCorners(fourCornersArray);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetInsetAndSizeFromParentEdge(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RectTransform.Edge val);
			float inset = (float)Lua.lua_tonumber(L, 3);
			float size = (float)Lua.lua_tonumber(L, 4);
			rectTransform.SetInsetAndSizeFromParentEdge(val, inset, size);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSizeWithCurrentAnchors(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RectTransform.Axis val);
			float size = (float)Lua.lua_tonumber(L, 3);
			rectTransform.SetSizeWithCurrentAnchors(val, size);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOAnchorPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector2, Vector2, VectorOptions> o = target.DOAnchorPos(val, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector2, Vector2, VectorOptions> o2 = target.DOAnchorPos(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.DOAnchorPos!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOAnchorPosX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector2, Vector2, VectorOptions> o = target.DOAnchorPosX(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector2, Vector2, VectorOptions> o2 = target.DOAnchorPosX(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.DOAnchorPosX!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOAnchorPosY(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector2, Vector2, VectorOptions> o = target.DOAnchorPosY(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector2, Vector2, VectorOptions> o2 = target.DOAnchorPosY(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.DOAnchorPosY!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOAnchorPos3D(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOAnchorPos3D(val, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = target.DOAnchorPos3D(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.DOAnchorPos3D!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOAnchorPos3DX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOAnchorPos3DX(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = target.DOAnchorPos3DX(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.DOAnchorPos3DX!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOAnchorPos3DY(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOAnchorPos3DY(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = target.DOAnchorPos3DY(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.DOAnchorPos3DY!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOAnchorPos3DZ(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOAnchorPos3DZ(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = target.DOAnchorPos3DZ(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.DOAnchorPos3DZ!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOAnchorMax(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector2, Vector2, VectorOptions> o = target.DOAnchorMax(val, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector2, Vector2, VectorOptions> o2 = target.DOAnchorMax(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.DOAnchorMax!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOAnchorMin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector2, Vector2, VectorOptions> o = target.DOAnchorMin(val, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector2, Vector2, VectorOptions> o2 = target.DOAnchorMin(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.DOAnchorMin!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPivot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			TweenerCore<Vector2, Vector2, VectorOptions> o = DOTweenModuleUI.DOPivot(duration: (float)Lua.lua_tonumber(L, 3), target: target, endValue: val);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPivotX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<Vector2, Vector2, VectorOptions> o = target.DOPivotX(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPivotY(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<Vector2, Vector2, VectorOptions> o = target.DOPivotY(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOSizeDelta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector2, Vector2, VectorOptions> o = target.DOSizeDelta(val, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector2, Vector2, VectorOptions> o2 = target.DOSizeDelta(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.DOSizeDelta!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPunchAnchorPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				int vibrato = Lua.xlua_tointeger(L, 4);
				float elasticity = (float)Lua.lua_tonumber(L, 5);
				bool snapping = Lua.lua_toboolean(L, 6);
				Tweener o = target.DOPunchAnchorPos(val, duration, vibrato, elasticity, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				int vibrato2 = Lua.xlua_tointeger(L, 4);
				float elasticity2 = (float)Lua.lua_tonumber(L, 5);
				Tweener o2 = target.DOPunchAnchorPos(val2, duration2, vibrato2, elasticity2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val3);
				float duration3 = (float)Lua.lua_tonumber(L, 3);
				int vibrato3 = Lua.xlua_tointeger(L, 4);
				Tweener o3 = target.DOPunchAnchorPos(val3, duration3, vibrato3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val4);
				float duration4 = (float)Lua.lua_tonumber(L, 3);
				Tweener o4 = target.DOPunchAnchorPos(val4, duration4);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.DOPunchAnchorPos!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOShakeAnchorPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 7 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				float duration = (float)Lua.lua_tonumber(L, 2);
				float strength = (float)Lua.lua_tonumber(L, 3);
				int vibrato = Lua.xlua_tointeger(L, 4);
				float randomness = (float)Lua.lua_tonumber(L, 5);
				bool snapping = Lua.lua_toboolean(L, 6);
				bool fadeOut = Lua.lua_toboolean(L, 7);
				Tweener o = target.DOShakeAnchorPos(duration, strength, vibrato, randomness, snapping, fadeOut);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				float duration2 = (float)Lua.lua_tonumber(L, 2);
				float strength2 = (float)Lua.lua_tonumber(L, 3);
				int vibrato2 = Lua.xlua_tointeger(L, 4);
				float randomness2 = (float)Lua.lua_tonumber(L, 5);
				bool snapping2 = Lua.lua_toboolean(L, 6);
				Tweener o2 = target.DOShakeAnchorPos(duration2, strength2, vibrato2, randomness2, snapping2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float duration3 = (float)Lua.lua_tonumber(L, 2);
				float strength3 = (float)Lua.lua_tonumber(L, 3);
				int vibrato3 = Lua.xlua_tointeger(L, 4);
				float randomness3 = (float)Lua.lua_tonumber(L, 5);
				Tweener o3 = target.DOShakeAnchorPos(duration3, strength3, vibrato3, randomness3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float duration4 = (float)Lua.lua_tonumber(L, 2);
				float strength4 = (float)Lua.lua_tonumber(L, 3);
				int vibrato4 = Lua.xlua_tointeger(L, 4);
				Tweener o4 = target.DOShakeAnchorPos(duration4, strength4, vibrato4);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float duration5 = (float)Lua.lua_tonumber(L, 2);
				float strength5 = (float)Lua.lua_tonumber(L, 3);
				Tweener o5 = target.DOShakeAnchorPos(duration5, strength5);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float duration6 = (float)Lua.lua_tonumber(L, 2);
				Tweener o6 = target.DOShakeAnchorPos(duration6);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 7 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				float duration7 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector2 val);
				int vibrato5 = Lua.xlua_tointeger(L, 4);
				float randomness4 = (float)Lua.lua_tonumber(L, 5);
				bool snapping3 = Lua.lua_toboolean(L, 6);
				bool fadeOut2 = Lua.lua_toboolean(L, 7);
				Tweener o7 = target.DOShakeAnchorPos(duration7, val, vibrato5, randomness4, snapping3, fadeOut2);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				float duration8 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector2 val2);
				int vibrato6 = Lua.xlua_tointeger(L, 4);
				float randomness5 = (float)Lua.lua_tonumber(L, 5);
				bool snapping4 = Lua.lua_toboolean(L, 6);
				Tweener o8 = target.DOShakeAnchorPos(duration8, val2, vibrato6, randomness5, snapping4);
				objectTranslator.Push(L, o8);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float duration9 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector2 val3);
				int vibrato7 = Lua.xlua_tointeger(L, 4);
				float randomness6 = (float)Lua.lua_tonumber(L, 5);
				Tweener o9 = target.DOShakeAnchorPos(duration9, val3, vibrato7, randomness6);
				objectTranslator.Push(L, o9);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float duration10 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector2 val4);
				int vibrato8 = Lua.xlua_tointeger(L, 4);
				Tweener o10 = target.DOShakeAnchorPos(duration10, val4, vibrato8);
				objectTranslator.Push(L, o10);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector2>(L, 3))
			{
				float duration11 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector2 val5);
				Tweener o11 = target.DOShakeAnchorPos(duration11, val5);
				objectTranslator.Push(L, o11);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.DOShakeAnchorPos!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOJumpAnchorPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform target = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				float jumpPower = (float)Lua.lua_tonumber(L, 3);
				int numJumps = Lua.xlua_tointeger(L, 4);
				float duration = (float)Lua.lua_tonumber(L, 5);
				bool snapping = Lua.lua_toboolean(L, 6);
				Sequence o = target.DOJumpAnchorPos(val, jumpPower, numJumps, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				float jumpPower2 = (float)Lua.lua_tonumber(L, 3);
				int numJumps2 = Lua.xlua_tointeger(L, 4);
				float duration2 = (float)Lua.lua_tonumber(L, 5);
				Sequence o2 = target.DOJumpAnchorPos(val2, jumpPower2, numJumps2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.DOJumpAnchorPos!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_offsetMax(IntPtr L)
	{
		try
		{
			RectTransform rt = (RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			rt.Set_offsetMax(x, y);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_offsetMax(IntPtr L)
	{
		try
		{
			((RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_offsetMax(out var x, out var y);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_offsetMin(IntPtr L)
	{
		try
		{
			RectTransform rt = (RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			rt.Set_offsetMin(x, y);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_offsetMin(IntPtr L)
	{
		try
		{
			((RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_offsetMin(out var x, out var y);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_anchorMin(IntPtr L)
	{
		try
		{
			RectTransform rt = (RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			rt.Set_anchorMin(x, y);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_anchorMin(IntPtr L)
	{
		try
		{
			((RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_anchorMin(out var x, out var y);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_anchorMax(IntPtr L)
	{
		try
		{
			RectTransform rt = (RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			rt.Set_anchorMax(x, y);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_anchorMax(IntPtr L)
	{
		try
		{
			((RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_anchorMax(out var x, out var y);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_anchoredPosition(IntPtr L)
	{
		try
		{
			RectTransform rt = (RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			rt.Set_anchoredPosition(x, y);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_anchoredPosition(IntPtr L)
	{
		try
		{
			((RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_anchoredPosition(out var x, out var y);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_pivot(IntPtr L)
	{
		try
		{
			RectTransform rt = (RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			rt.Set_pivot(x, y);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_pivot(IntPtr L)
	{
		try
		{
			((RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_pivot(out var x, out var y);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_sizeDelta(IntPtr L)
	{
		try
		{
			RectTransform rt = (RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			rt.Set_sizeDelta(x, y);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_sizeDelta_x(IntPtr L)
	{
		try
		{
			RectTransform rt = (RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			rt.Set_sizeDelta_x(x);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_sizeDelta_y(IntPtr L)
	{
		try
		{
			RectTransform rt = (RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float y = (float)Lua.lua_tonumber(L, 2);
			rt.Set_sizeDelta_y(y);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_sizeDelta(IntPtr L)
	{
		try
		{
			((RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_sizeDelta(out var x, out var y);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_sizeDelta_x(IntPtr L)
	{
		try
		{
			((RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_sizeDelta_x(out var x);
			Lua.lua_pushnumber(L, x);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_sizeDelta_y(IntPtr L)
	{
		try
		{
			((RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_sizeDelta_y(out var y);
			Lua.lua_pushnumber(L, y);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_worldCorners_x(IntPtr L)
	{
		try
		{
			((RectTransform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_worldCorners_x(out var minX, out var maxX);
			Lua.lua_pushnumber(L, minX);
			Lua.lua_pushnumber(L, maxX);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, rectTransform.rect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_anchorMin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, rectTransform.anchorMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_anchorMax(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, rectTransform.anchorMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_anchoredPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, rectTransform.anchoredPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sizeDelta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, rectTransform.sizeDelta);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pivot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, rectTransform.pivot);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_anchoredPosition3D(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, rectTransform.anchoredPosition3D);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_offsetMin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, rectTransform.offsetMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_offsetMax(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, rectTransform.offsetMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_anchorMin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			rectTransform.anchorMin = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_anchorMax(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			rectTransform.anchorMax = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_anchoredPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			rectTransform.anchoredPosition = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sizeDelta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			rectTransform.sizeDelta = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pivot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			rectTransform.pivot = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_anchoredPosition3D(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			rectTransform.anchoredPosition3D = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_offsetMin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			rectTransform.offsetMin = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_offsetMax(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			rectTransform.offsetMax = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_reapplyDrivenProperties(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			RectTransform.ReapplyDrivenProperties reapplyDrivenProperties = objectTranslator.GetDelegate<RectTransform.ReapplyDrivenProperties>(L, 2);
			if (reapplyDrivenProperties == null)
			{
				return Lua.luaL_error(L, "#2 need UnityEngine.RectTransform.ReapplyDrivenProperties!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				RectTransform.reapplyDrivenProperties += reapplyDrivenProperties;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				RectTransform.reapplyDrivenProperties -= reapplyDrivenProperties;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectTransform.reapplyDrivenProperties!");
	}
}
