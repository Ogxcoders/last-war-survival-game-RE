using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTextMeshProWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TextMeshPro);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 17, 8, 4);
		Utils.RegisterFunc(L, -3, "ComputeMarginSize", _m_ComputeMarginSize);
		Utils.RegisterFunc(L, -3, "SetMask", _m_SetMask);
		Utils.RegisterFunc(L, -3, "SetVerticesDirty", _m_SetVerticesDirty);
		Utils.RegisterFunc(L, -3, "SetLayoutDirty", _m_SetLayoutDirty);
		Utils.RegisterFunc(L, -3, "SetMaterialDirty", _m_SetMaterialDirty);
		Utils.RegisterFunc(L, -3, "SetAllDirty", _m_SetAllDirty);
		Utils.RegisterFunc(L, -3, "Rebuild", _m_Rebuild);
		Utils.RegisterFunc(L, -3, "UpdateMeshPadding", _m_UpdateMeshPadding);
		Utils.RegisterFunc(L, -3, "ForceMeshUpdate", _m_ForceMeshUpdate);
		Utils.RegisterFunc(L, -3, "GetTextInfo", _m_GetTextInfo);
		Utils.RegisterFunc(L, -3, "ClearMesh", _m_ClearMesh);
		Utils.RegisterFunc(L, -3, "UpdateGeometry", _m_UpdateGeometry);
		Utils.RegisterFunc(L, -3, "UpdateVertexData", _m_UpdateVertexData);
		Utils.RegisterFunc(L, -3, "UpdateFontAsset", _m_UpdateFontAsset);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputHorizontal", _m_CalculateLayoutInputHorizontal);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputVertical", _m_CalculateLayoutInputVertical);
		Utils.RegisterFunc(L, -3, "OnPreRenderText", _e_OnPreRenderText);
		Utils.RegisterFunc(L, -2, "sortingLayerID", _g_get_sortingLayerID);
		Utils.RegisterFunc(L, -2, "sortingOrder", _g_get_sortingOrder);
		Utils.RegisterFunc(L, -2, "autoSizeTextContainer", _g_get_autoSizeTextContainer);
		Utils.RegisterFunc(L, -2, "transform", _g_get_transform);
		Utils.RegisterFunc(L, -2, "renderer", _g_get_renderer);
		Utils.RegisterFunc(L, -2, "mesh", _g_get_mesh);
		Utils.RegisterFunc(L, -2, "meshFilter", _g_get_meshFilter);
		Utils.RegisterFunc(L, -2, "maskType", _g_get_maskType);
		Utils.RegisterFunc(L, -1, "sortingLayerID", _s_set_sortingLayerID);
		Utils.RegisterFunc(L, -1, "sortingOrder", _s_set_sortingOrder);
		Utils.RegisterFunc(L, -1, "autoSizeTextContainer", _s_set_autoSizeTextContainer);
		Utils.RegisterFunc(L, -1, "maskType", _s_set_maskType);
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
				TextMeshPro o = new TextMeshPro();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TextMeshPro constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ComputeMarginSize(IntPtr L)
	{
		try
		{
			((TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ComputeMarginSize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMask(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshPro textMeshPro = (TextMeshPro)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<MaskingTypes>(L, 2) && objectTranslator.Assignable<Vector4>(L, 3))
			{
				objectTranslator.Get(L, 2, out MaskingTypes v);
				objectTranslator.Get(L, 3, out Vector4 val);
				textMeshPro.SetMask(v, val);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<MaskingTypes>(L, 2) && objectTranslator.Assignable<Vector4>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out MaskingTypes v2);
				objectTranslator.Get(L, 3, out Vector4 val2);
				float softnessX = (float)Lua.lua_tonumber(L, 4);
				float softnessY = (float)Lua.lua_tonumber(L, 5);
				textMeshPro.SetMask(v2, val2, softnessX, softnessY);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TextMeshPro.SetMask!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVerticesDirty(IntPtr L)
	{
		try
		{
			((TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetVerticesDirty();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLayoutDirty(IntPtr L)
	{
		try
		{
			((TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutDirty();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMaterialDirty(IntPtr L)
	{
		try
		{
			((TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetMaterialDirty();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAllDirty(IntPtr L)
	{
		try
		{
			((TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetAllDirty();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Rebuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshPro textMeshPro = (TextMeshPro)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CanvasUpdate v);
			textMeshPro.Rebuild(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateMeshPadding(IntPtr L)
	{
		try
		{
			((TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateMeshPadding();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceMeshUpdate(IntPtr L)
	{
		try
		{
			TextMeshPro textMeshPro = (TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				bool ignoreActiveState = Lua.lua_toboolean(L, 2);
				bool forceTextReparsing = Lua.lua_toboolean(L, 3);
				textMeshPro.ForceMeshUpdate(ignoreActiveState, forceTextReparsing);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool ignoreActiveState2 = Lua.lua_toboolean(L, 2);
				textMeshPro.ForceMeshUpdate(ignoreActiveState2);
				return 0;
			}
			if (num == 1)
			{
				textMeshPro.ForceMeshUpdate();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TextMeshPro.ForceMeshUpdate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTextInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshPro obj = (TextMeshPro)objectTranslator.FastGetCSObj(L, 1);
			string text = Lua.lua_tostring(L, 2);
			TMP_TextInfo textInfo = obj.GetTextInfo(text);
			objectTranslator.Push(L, textInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearMesh(IntPtr L)
	{
		try
		{
			TextMeshPro obj = (TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool uploadGeometry = Lua.lua_toboolean(L, 2);
			obj.ClearMesh(uploadGeometry);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateGeometry(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshPro textMeshPro = (TextMeshPro)objectTranslator.FastGetCSObj(L, 1);
			Mesh mesh = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
			int index = Lua.xlua_tointeger(L, 3);
			textMeshPro.UpdateGeometry(mesh, index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateVertexData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshPro textMeshPro = (TextMeshPro)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				textMeshPro.UpdateVertexData();
				return 0;
			case 2:
				if (objectTranslator.Assignable<TMP_VertexDataUpdateFlags>(L, 2))
				{
					objectTranslator.Get(L, 2, out TMP_VertexDataUpdateFlags v);
					textMeshPro.UpdateVertexData(v);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TextMeshPro.UpdateVertexData!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateFontAsset(IntPtr L)
	{
		try
		{
			((TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateFontAsset();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateLayoutInputHorizontal(IntPtr L)
	{
		try
		{
			((TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputHorizontal();
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
			((TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sortingLayerID(IntPtr L)
	{
		try
		{
			TextMeshPro textMeshPro = (TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, textMeshPro.sortingLayerID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sortingOrder(IntPtr L)
	{
		try
		{
			TextMeshPro textMeshPro = (TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, textMeshPro.sortingOrder);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autoSizeTextContainer(IntPtr L)
	{
		try
		{
			TextMeshPro textMeshPro = (TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, textMeshPro.autoSizeTextContainer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_transform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshPro textMeshPro = (TextMeshPro)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, textMeshPro.transform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshPro textMeshPro = (TextMeshPro)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, textMeshPro.renderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshPro textMeshPro = (TextMeshPro)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, textMeshPro.mesh);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_meshFilter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshPro textMeshPro = (TextMeshPro)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, textMeshPro.meshFilter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maskType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshPro textMeshPro = (TextMeshPro)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, textMeshPro.maskType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sortingLayerID(IntPtr L)
	{
		try
		{
			((TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sortingLayerID = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sortingOrder(IntPtr L)
	{
		try
		{
			((TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sortingOrder = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autoSizeTextContainer(IntPtr L)
	{
		try
		{
			((TextMeshPro)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).autoSizeTextContainer = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maskType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshPro textMeshPro = (TextMeshPro)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out MaskingTypes v);
			textMeshPro.maskType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnPreRenderText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TextMeshPro textMeshPro = (TextMeshPro)objectTranslator.FastGetCSObj(L, 1);
			Action<TMP_TextInfo> action = objectTranslator.GetDelegate<Action<TMP_TextInfo>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<TMPro.TMP_TextInfo>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					textMeshPro.OnPreRenderText += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					textMeshPro.OnPreRenderText -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to TMPro.TextMeshPro.OnPreRenderText!");
		return 0;
	}
}
