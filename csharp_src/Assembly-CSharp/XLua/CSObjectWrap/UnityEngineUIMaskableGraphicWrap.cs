using System;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIMaskableGraphicWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MaskableGraphic);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 6, 3, 3);
		Utils.RegisterFunc(L, -3, "GetModifiedMaterial", _m_GetModifiedMaterial);
		Utils.RegisterFunc(L, -3, "Cull", _m_Cull);
		Utils.RegisterFunc(L, -3, "SetClipRect", _m_SetClipRect);
		Utils.RegisterFunc(L, -3, "SetClipSoftness", _m_SetClipSoftness);
		Utils.RegisterFunc(L, -3, "RecalculateClipping", _m_RecalculateClipping);
		Utils.RegisterFunc(L, -3, "RecalculateMasking", _m_RecalculateMasking);
		Utils.RegisterFunc(L, -2, "onCullStateChanged", _g_get_onCullStateChanged);
		Utils.RegisterFunc(L, -2, "maskable", _g_get_maskable);
		Utils.RegisterFunc(L, -2, "isMaskingGraphic", _g_get_isMaskingGraphic);
		Utils.RegisterFunc(L, -1, "onCullStateChanged", _s_set_onCullStateChanged);
		Utils.RegisterFunc(L, -1, "maskable", _s_set_maskable);
		Utils.RegisterFunc(L, -1, "isMaskingGraphic", _s_set_isMaskingGraphic);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.MaskableGraphic does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetModifiedMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaskableGraphic maskableGraphic = (MaskableGraphic)objectTranslator.FastGetCSObj(L, 1);
			Material baseMaterial = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
			Material modifiedMaterial = maskableGraphic.GetModifiedMaterial(baseMaterial);
			objectTranslator.Push(L, modifiedMaterial);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Cull(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaskableGraphic maskableGraphic = (MaskableGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Rect v);
			bool validRect = Lua.lua_toboolean(L, 3);
			maskableGraphic.Cull(v, validRect);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetClipRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaskableGraphic maskableGraphic = (MaskableGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Rect v);
			bool validRect = Lua.lua_toboolean(L, 3);
			maskableGraphic.SetClipRect(v, validRect);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetClipSoftness(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaskableGraphic maskableGraphic = (MaskableGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			maskableGraphic.SetClipSoftness(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecalculateClipping(IntPtr L)
	{
		try
		{
			((MaskableGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RecalculateClipping();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecalculateMasking(IntPtr L)
	{
		try
		{
			((MaskableGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RecalculateMasking();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onCullStateChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaskableGraphic maskableGraphic = (MaskableGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, maskableGraphic.onCullStateChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maskable(IntPtr L)
	{
		try
		{
			MaskableGraphic maskableGraphic = (MaskableGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, maskableGraphic.maskable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isMaskingGraphic(IntPtr L)
	{
		try
		{
			MaskableGraphic maskableGraphic = (MaskableGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, maskableGraphic.isMaskingGraphic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onCullStateChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((MaskableGraphic)objectTranslator.FastGetCSObj(L, 1)).onCullStateChanged = (MaskableGraphic.CullStateChangedEvent)objectTranslator.GetObject(L, 2, typeof(MaskableGraphic.CullStateChangedEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maskable(IntPtr L)
	{
		try
		{
			((MaskableGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maskable = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isMaskingGraphic(IntPtr L)
	{
		try
		{
			((MaskableGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isMaskingGraphic = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
