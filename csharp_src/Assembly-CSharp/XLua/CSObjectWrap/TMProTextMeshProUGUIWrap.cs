using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTextMeshProUGUIWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TextMeshProUGUI);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 22, 5, 2);
		Utils.RegisterFunc(L, -3, "ComputeMarginSize", _m_ComputeMarginSize);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputHorizontal", _m_CalculateLayoutInputHorizontal);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputVertical", _m_CalculateLayoutInputVertical);
		Utils.RegisterFunc(L, -3, "SetVerticesDirty", _m_SetVerticesDirty);
		Utils.RegisterFunc(L, -3, "SetLayoutDirty", _m_SetLayoutDirty);
		Utils.RegisterFunc(L, -3, "SetMaterialDirty", _m_SetMaterialDirty);
		Utils.RegisterFunc(L, -3, "SetAllDirty", _m_SetAllDirty);
		Utils.RegisterFunc(L, -3, "Rebuild", _m_Rebuild);
		Utils.RegisterFunc(L, -3, "GetModifiedMaterial", _m_GetModifiedMaterial);
		Utils.RegisterFunc(L, -3, "RecalculateClipping", _m_RecalculateClipping);
		Utils.RegisterFunc(L, -3, "Cull", _m_Cull);
		Utils.RegisterFunc(L, -3, "UpdateMeshPadding", _m_UpdateMeshPadding);
		Utils.RegisterFunc(L, -3, "ForceMeshUpdate", _m_ForceMeshUpdate);
		Utils.RegisterFunc(L, -3, "GetTextInfo", _m_GetTextInfo);
		Utils.RegisterFunc(L, -3, "ClearMesh", _m_ClearMesh);
		Utils.RegisterFunc(L, -3, "UpdateGeometry", _m_UpdateGeometry);
		Utils.RegisterFunc(L, -3, "UpdateVertexData", _m_UpdateVertexData);
		Utils.RegisterFunc(L, -3, "UpdateFontAsset", _m_UpdateFontAsset);
		Utils.RegisterFunc(L, -3, "SetLocalText", _m_SetLocalText);
		Utils.RegisterFunc(L, -3, "Native_SetText", _m_Native_SetText);
		Utils.RegisterFunc(L, -3, "SetText_NotNative", _m_SetText_NotNative);
		Utils.RegisterFunc(L, -3, "OnPreRenderText", _e_OnPreRenderText);
		Utils.RegisterFunc(L, -2, "materialForRendering", _g_get_materialForRendering);
		Utils.RegisterFunc(L, -2, "autoSizeTextContainer", _g_get_autoSizeTextContainer);
		Utils.RegisterFunc(L, -2, "mesh", _g_get_mesh);
		Utils.RegisterFunc(L, -2, "canvasRenderer", _g_get_canvasRenderer);
		Utils.RegisterFunc(L, -2, "maskOffset", _g_get_maskOffset);
		Utils.RegisterFunc(L, -1, "autoSizeTextContainer", _s_set_autoSizeTextContainer);
		Utils.RegisterFunc(L, -1, "maskOffset", _s_set_maskOffset);
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
				TextMeshProUGUI o = new TextMeshProUGUI();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TextMeshProUGUI constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ComputeMarginSize(IntPtr L)
	{
		try
		{
			((TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ComputeMarginSize();
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
			((TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputHorizontal();
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
			((TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVerticesDirty(IntPtr L)
	{
		try
		{
			((TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetVerticesDirty();
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
			((TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutDirty();
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
			((TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetMaterialDirty();
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
			((TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetAllDirty();
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
			TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CanvasUpdate v);
			textMeshProUGUI.Rebuild(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetModifiedMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)objectTranslator.FastGetCSObj(L, 1);
			Material baseMaterial = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
			Material modifiedMaterial = textMeshProUGUI.GetModifiedMaterial(baseMaterial);
			objectTranslator.Push(L, modifiedMaterial);
			return 1;
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
			((TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RecalculateClipping();
			return 0;
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
			TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Rect v);
			bool validRect = Lua.lua_toboolean(L, 3);
			textMeshProUGUI.Cull(v, validRect);
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
			((TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateMeshPadding();
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
			TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				bool ignoreActiveState = Lua.lua_toboolean(L, 2);
				bool forceTextReparsing = Lua.lua_toboolean(L, 3);
				textMeshProUGUI.ForceMeshUpdate(ignoreActiveState, forceTextReparsing);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool ignoreActiveState2 = Lua.lua_toboolean(L, 2);
				textMeshProUGUI.ForceMeshUpdate(ignoreActiveState2);
				return 0;
			}
			if (num == 1)
			{
				textMeshProUGUI.ForceMeshUpdate();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TextMeshProUGUI.ForceMeshUpdate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTextInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProUGUI obj = (TextMeshProUGUI)objectTranslator.FastGetCSObj(L, 1);
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
			((TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearMesh();
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
			TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)objectTranslator.FastGetCSObj(L, 1);
			Mesh mesh = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
			int index = Lua.xlua_tointeger(L, 3);
			textMeshProUGUI.UpdateGeometry(mesh, index);
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
			TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				textMeshProUGUI.UpdateVertexData();
				return 0;
			case 2:
				if (objectTranslator.Assignable<TMP_VertexDataUpdateFlags>(L, 2))
				{
					objectTranslator.Get(L, 2, out TMP_VertexDataUpdateFlags v);
					textMeshProUGUI.UpdateVertexData(v);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TextMeshProUGUI.UpdateVertexData!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateFontAsset(IntPtr L)
	{
		try
		{
			((TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateFontAsset();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLocalText(IntPtr L)
	{
		try
		{
			((TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLocalText();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Native_SetText(IntPtr L)
	{
		try
		{
			TextMeshProUGUI text = (TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string value = Lua.lua_tostring(L, 2);
			text.Native_SetText(value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetText_NotNative(IntPtr L)
	{
		try
		{
			TextMeshProUGUI text = (TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string value = Lua.lua_tostring(L, 2);
			text.SetText_NotNative(value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_materialForRendering(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, textMeshProUGUI.materialForRendering);
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
			TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, textMeshProUGUI.autoSizeTextContainer);
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
			TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, textMeshProUGUI.mesh);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_canvasRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, textMeshProUGUI.canvasRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maskOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector4(L, textMeshProUGUI.maskOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autoSizeTextContainer(IntPtr L)
	{
		try
		{
			((TextMeshProUGUI)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).autoSizeTextContainer = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maskOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector4 val);
			textMeshProUGUI.maskOffset = val;
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
			TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)objectTranslator.FastGetCSObj(L, 1);
			Action<TMP_TextInfo> action = objectTranslator.GetDelegate<Action<TMP_TextInfo>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<TMPro.TMP_TextInfo>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					textMeshProUGUI.OnPreRenderText += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					textMeshProUGUI.OnPreRenderText -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to TMPro.TextMeshProUGUI.OnPreRenderText!");
		return 0;
	}
}
