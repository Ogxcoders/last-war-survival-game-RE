using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineMaterialWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Material);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 61, 11, 10);
		Utils.RegisterFunc(L, -3, "HasProperty", _m_HasProperty);
		Utils.RegisterFunc(L, -3, "EnableKeyword", _m_EnableKeyword);
		Utils.RegisterFunc(L, -3, "DisableKeyword", _m_DisableKeyword);
		Utils.RegisterFunc(L, -3, "IsKeywordEnabled", _m_IsKeywordEnabled);
		Utils.RegisterFunc(L, -3, "SetShaderPassEnabled", _m_SetShaderPassEnabled);
		Utils.RegisterFunc(L, -3, "GetShaderPassEnabled", _m_GetShaderPassEnabled);
		Utils.RegisterFunc(L, -3, "GetPassName", _m_GetPassName);
		Utils.RegisterFunc(L, -3, "FindPass", _m_FindPass);
		Utils.RegisterFunc(L, -3, "SetOverrideTag", _m_SetOverrideTag);
		Utils.RegisterFunc(L, -3, "GetTag", _m_GetTag);
		Utils.RegisterFunc(L, -3, "Lerp", _m_Lerp);
		Utils.RegisterFunc(L, -3, "SetPass", _m_SetPass);
		Utils.RegisterFunc(L, -3, "CopyPropertiesFromMaterial", _m_CopyPropertiesFromMaterial);
		Utils.RegisterFunc(L, -3, "ComputeCRC", _m_ComputeCRC);
		Utils.RegisterFunc(L, -3, "GetTexturePropertyNames", _m_GetTexturePropertyNames);
		Utils.RegisterFunc(L, -3, "GetTexturePropertyNameIDs", _m_GetTexturePropertyNameIDs);
		Utils.RegisterFunc(L, -3, "SetFloat", _m_SetFloat);
		Utils.RegisterFunc(L, -3, "SetInt", _m_SetInt);
		Utils.RegisterFunc(L, -3, "SetColor", _m_SetColor);
		Utils.RegisterFunc(L, -3, "SetVector", _m_SetVector);
		Utils.RegisterFunc(L, -3, "SetMatrix", _m_SetMatrix);
		Utils.RegisterFunc(L, -3, "SetTexture", _m_SetTexture);
		Utils.RegisterFunc(L, -3, "SetBuffer", _m_SetBuffer);
		Utils.RegisterFunc(L, -3, "SetConstantBuffer", _m_SetConstantBuffer);
		Utils.RegisterFunc(L, -3, "SetFloatArray", _m_SetFloatArray);
		Utils.RegisterFunc(L, -3, "SetColorArray", _m_SetColorArray);
		Utils.RegisterFunc(L, -3, "SetVectorArray", _m_SetVectorArray);
		Utils.RegisterFunc(L, -3, "SetMatrixArray", _m_SetMatrixArray);
		Utils.RegisterFunc(L, -3, "GetFloat", _m_GetFloat);
		Utils.RegisterFunc(L, -3, "GetInt", _m_GetInt);
		Utils.RegisterFunc(L, -3, "GetColor", _m_GetColor);
		Utils.RegisterFunc(L, -3, "GetVector", _m_GetVector);
		Utils.RegisterFunc(L, -3, "GetMatrix", _m_GetMatrix);
		Utils.RegisterFunc(L, -3, "GetTexture", _m_GetTexture);
		Utils.RegisterFunc(L, -3, "GetFloatArray", _m_GetFloatArray);
		Utils.RegisterFunc(L, -3, "GetColorArray", _m_GetColorArray);
		Utils.RegisterFunc(L, -3, "GetVectorArray", _m_GetVectorArray);
		Utils.RegisterFunc(L, -3, "GetMatrixArray", _m_GetMatrixArray);
		Utils.RegisterFunc(L, -3, "SetTextureOffset", _m_SetTextureOffset);
		Utils.RegisterFunc(L, -3, "SetTextureScale", _m_SetTextureScale);
		Utils.RegisterFunc(L, -3, "GetTextureOffset", _m_GetTextureOffset);
		Utils.RegisterFunc(L, -3, "GetTextureScale", _m_GetTextureScale);
		Utils.RegisterFunc(L, -3, "DOColor", _m_DOColor);
		Utils.RegisterFunc(L, -3, "DOFade", _m_DOFade);
		Utils.RegisterFunc(L, -3, "DOFloat", _m_DOFloat);
		Utils.RegisterFunc(L, -3, "DOOffset", _m_DOOffset);
		Utils.RegisterFunc(L, -3, "DOTiling", _m_DOTiling);
		Utils.RegisterFunc(L, -3, "DOVector", _m_DOVector);
		Utils.RegisterFunc(L, -3, "DOBlendableColor", _m_DOBlendableColor);
		Utils.RegisterFunc(L, -3, "DOComplete", _m_DOComplete);
		Utils.RegisterFunc(L, -3, "DOKill", _m_DOKill);
		Utils.RegisterFunc(L, -3, "DOFlip", _m_DOFlip);
		Utils.RegisterFunc(L, -3, "DOGoto", _m_DOGoto);
		Utils.RegisterFunc(L, -3, "DOPause", _m_DOPause);
		Utils.RegisterFunc(L, -3, "DOPlay", _m_DOPlay);
		Utils.RegisterFunc(L, -3, "DOPlayBackwards", _m_DOPlayBackwards);
		Utils.RegisterFunc(L, -3, "DOPlayForward", _m_DOPlayForward);
		Utils.RegisterFunc(L, -3, "DORestart", _m_DORestart);
		Utils.RegisterFunc(L, -3, "DORewind", _m_DORewind);
		Utils.RegisterFunc(L, -3, "DOSmoothRewind", _m_DOSmoothRewind);
		Utils.RegisterFunc(L, -3, "DOTogglePause", _m_DOTogglePause);
		Utils.RegisterFunc(L, -2, "shader", _g_get_shader);
		Utils.RegisterFunc(L, -2, "color", _g_get_color);
		Utils.RegisterFunc(L, -2, "mainTexture", _g_get_mainTexture);
		Utils.RegisterFunc(L, -2, "mainTextureOffset", _g_get_mainTextureOffset);
		Utils.RegisterFunc(L, -2, "mainTextureScale", _g_get_mainTextureScale);
		Utils.RegisterFunc(L, -2, "renderQueue", _g_get_renderQueue);
		Utils.RegisterFunc(L, -2, "globalIlluminationFlags", _g_get_globalIlluminationFlags);
		Utils.RegisterFunc(L, -2, "doubleSidedGI", _g_get_doubleSidedGI);
		Utils.RegisterFunc(L, -2, "enableInstancing", _g_get_enableInstancing);
		Utils.RegisterFunc(L, -2, "passCount", _g_get_passCount);
		Utils.RegisterFunc(L, -2, "shaderKeywords", _g_get_shaderKeywords);
		Utils.RegisterFunc(L, -1, "shader", _s_set_shader);
		Utils.RegisterFunc(L, -1, "color", _s_set_color);
		Utils.RegisterFunc(L, -1, "mainTexture", _s_set_mainTexture);
		Utils.RegisterFunc(L, -1, "mainTextureOffset", _s_set_mainTextureOffset);
		Utils.RegisterFunc(L, -1, "mainTextureScale", _s_set_mainTextureScale);
		Utils.RegisterFunc(L, -1, "renderQueue", _s_set_renderQueue);
		Utils.RegisterFunc(L, -1, "globalIlluminationFlags", _s_set_globalIlluminationFlags);
		Utils.RegisterFunc(L, -1, "doubleSidedGI", _s_set_doubleSidedGI);
		Utils.RegisterFunc(L, -1, "enableInstancing", _s_set_enableInstancing);
		Utils.RegisterFunc(L, -1, "shaderKeywords", _s_set_shaderKeywords);
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
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<Shader>(L, 2))
			{
				Material o = new Material((Shader)objectTranslator.GetObject(L, 2, typeof(Shader)));
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<Material>(L, 2))
			{
				Material o2 = new Material((Material)objectTranslator.GetObject(L, 2, typeof(Material)));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasProperty(IntPtr L)
	{
		try
		{
			Material material = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				bool value = material.HasProperty(nameID);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				bool value2 = material.HasProperty(name);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.HasProperty!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnableKeyword(IntPtr L)
	{
		try
		{
			Material obj = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string keyword = Lua.lua_tostring(L, 2);
			obj.EnableKeyword(keyword);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DisableKeyword(IntPtr L)
	{
		try
		{
			Material obj = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string keyword = Lua.lua_tostring(L, 2);
			obj.DisableKeyword(keyword);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsKeywordEnabled(IntPtr L)
	{
		try
		{
			Material obj = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string keyword = Lua.lua_tostring(L, 2);
			bool value = obj.IsKeywordEnabled(keyword);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetShaderPassEnabled(IntPtr L)
	{
		try
		{
			Material obj = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string passName = Lua.lua_tostring(L, 2);
			bool enabled = Lua.lua_toboolean(L, 3);
			obj.SetShaderPassEnabled(passName, enabled);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetShaderPassEnabled(IntPtr L)
	{
		try
		{
			Material obj = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string passName = Lua.lua_tostring(L, 2);
			bool shaderPassEnabled = obj.GetShaderPassEnabled(passName);
			Lua.lua_pushboolean(L, shaderPassEnabled);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPassName(IntPtr L)
	{
		try
		{
			Material obj = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pass = Lua.xlua_tointeger(L, 2);
			string passName = obj.GetPassName(pass);
			Lua.lua_pushstring(L, passName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindPass(IntPtr L)
	{
		try
		{
			Material obj = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string passName = Lua.lua_tostring(L, 2);
			int value = obj.FindPass(passName);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOverrideTag(IntPtr L)
	{
		try
		{
			Material obj = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string tag = Lua.lua_tostring(L, 2);
			string val = Lua.lua_tostring(L, 3);
			obj.SetOverrideTag(tag, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTag(IntPtr L)
	{
		try
		{
			Material material = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string tag = Lua.lua_tostring(L, 2);
				bool searchFallbacks = Lua.lua_toboolean(L, 3);
				string tag2 = material.GetTag(tag, searchFallbacks);
				Lua.lua_pushstring(L, tag2);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING))
			{
				string tag3 = Lua.lua_tostring(L, 2);
				bool searchFallbacks2 = Lua.lua_toboolean(L, 3);
				string defaultValue = Lua.lua_tostring(L, 4);
				string tag4 = material.GetTag(tag3, searchFallbacks2, defaultValue);
				Lua.lua_pushstring(L, tag4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetTag!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Lerp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			Material start = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
			Material end = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
			float t = (float)Lua.lua_tonumber(L, 4);
			material.Lerp(start, end, t);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPass(IntPtr L)
	{
		try
		{
			Material obj = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pass = Lua.xlua_tointeger(L, 2);
			bool value = obj.SetPass(pass);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CopyPropertiesFromMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			Material mat = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
			material.CopyPropertiesFromMaterial(mat);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ComputeCRC(IntPtr L)
	{
		try
		{
			int value = ((Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ComputeCRC();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTexturePropertyNames(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				string[] texturePropertyNames = material.GetTexturePropertyNames();
				objectTranslator.Push(L, texturePropertyNames);
				return 1;
			}
			case 2:
				if (objectTranslator.Assignable<List<string>>(L, 2))
				{
					List<string> outNames = (List<string>)objectTranslator.GetObject(L, 2, typeof(List<string>));
					material.GetTexturePropertyNames(outNames);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetTexturePropertyNames!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTexturePropertyNameIDs(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				int[] texturePropertyNameIDs = material.GetTexturePropertyNameIDs();
				objectTranslator.Push(L, texturePropertyNameIDs);
				return 1;
			}
			case 2:
				if (objectTranslator.Assignable<List<int>>(L, 2))
				{
					List<int> outNames = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
					material.GetTexturePropertyNameIDs(outNames);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetTexturePropertyNameIDs!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFloat(IntPtr L)
	{
		try
		{
			Material material = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				float value = (float)Lua.lua_tonumber(L, 3);
				material.SetFloat(nameID, value);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				float value2 = (float)Lua.lua_tonumber(L, 3);
				material.SetFloat(name, value2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetFloat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetInt(IntPtr L)
	{
		try
		{
			Material material = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				int value = Lua.xlua_tointeger(L, 3);
				material.SetInt(nameID, value);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				int value2 = Lua.xlua_tointeger(L, 3);
				material.SetInt(name, value2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetInt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Color>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out Color val);
				material.SetColor(nameID, val);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Color>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out Color val2);
				material.SetColor(name, val2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetColor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVector(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector4>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out Vector4 val);
				material.SetVector(nameID, val);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector4>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out Vector4 val2);
				material.SetVector(name, val2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetVector!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Matrix4x4>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out Matrix4x4 v);
				material.SetMatrix(nameID, v);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Matrix4x4>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out Matrix4x4 v2);
				material.SetMatrix(name, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetMatrix!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Texture>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Texture value = (Texture)objectTranslator.GetObject(L, 3, typeof(Texture));
				material.SetTexture(nameID, value);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Texture>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				Texture value2 = (Texture)objectTranslator.GetObject(L, 3, typeof(Texture));
				material.SetTexture(name, value2);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<RenderTexture>(L, 3) && objectTranslator.Assignable<RenderTextureSubElement>(L, 4))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				RenderTexture value3 = (RenderTexture)objectTranslator.GetObject(L, 3, typeof(RenderTexture));
				objectTranslator.Get(L, 4, out RenderTextureSubElement v);
				material.SetTexture(nameID2, value3, v);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<RenderTexture>(L, 3) && objectTranslator.Assignable<RenderTextureSubElement>(L, 4))
			{
				string name2 = Lua.lua_tostring(L, 2);
				RenderTexture value4 = (RenderTexture)objectTranslator.GetObject(L, 3, typeof(RenderTexture));
				objectTranslator.Get(L, 4, out RenderTextureSubElement v2);
				material.SetTexture(name2, value4, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetTexture!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBuffer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<ComputeBuffer>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				ComputeBuffer value = (ComputeBuffer)objectTranslator.GetObject(L, 3, typeof(ComputeBuffer));
				material.SetBuffer(nameID, value);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<ComputeBuffer>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				ComputeBuffer value2 = (ComputeBuffer)objectTranslator.GetObject(L, 3, typeof(ComputeBuffer));
				material.SetBuffer(name, value2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetBuffer!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetConstantBuffer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<ComputeBuffer>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				ComputeBuffer value = (ComputeBuffer)objectTranslator.GetObject(L, 3, typeof(ComputeBuffer));
				int offset = Lua.xlua_tointeger(L, 4);
				int size = Lua.xlua_tointeger(L, 5);
				material.SetConstantBuffer(nameID, value, offset, size);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<ComputeBuffer>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string name = Lua.lua_tostring(L, 2);
				ComputeBuffer value2 = (ComputeBuffer)objectTranslator.GetObject(L, 3, typeof(ComputeBuffer));
				int offset2 = Lua.xlua_tointeger(L, 4);
				int size2 = Lua.xlua_tointeger(L, 5);
				material.SetConstantBuffer(name, value2, offset2, size2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetConstantBuffer!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFloatArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<float>>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				List<float> values = (List<float>)objectTranslator.GetObject(L, 3, typeof(List<float>));
				material.SetFloatArray(nameID, values);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<float[]>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				float[] values2 = (float[])objectTranslator.GetObject(L, 3, typeof(float[]));
				material.SetFloatArray(nameID2, values2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<float>>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				List<float> values3 = (List<float>)objectTranslator.GetObject(L, 3, typeof(List<float>));
				material.SetFloatArray(name, values3);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<float[]>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				float[] values4 = (float[])objectTranslator.GetObject(L, 3, typeof(float[]));
				material.SetFloatArray(name2, values4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetFloatArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetColorArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Color>>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				List<Color> values = (List<Color>)objectTranslator.GetObject(L, 3, typeof(List<Color>));
				material.SetColorArray(nameID, values);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Color[]>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				Color[] values2 = (Color[])objectTranslator.GetObject(L, 3, typeof(Color[]));
				material.SetColorArray(nameID2, values2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Color>>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				List<Color> values3 = (List<Color>)objectTranslator.GetObject(L, 3, typeof(List<Color>));
				material.SetColorArray(name, values3);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Color[]>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				Color[] values4 = (Color[])objectTranslator.GetObject(L, 3, typeof(Color[]));
				material.SetColorArray(name2, values4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetColorArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVectorArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Vector4>>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				List<Vector4> values = (List<Vector4>)objectTranslator.GetObject(L, 3, typeof(List<Vector4>));
				material.SetVectorArray(nameID, values);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector4[]>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				Vector4[] values2 = (Vector4[])objectTranslator.GetObject(L, 3, typeof(Vector4[]));
				material.SetVectorArray(nameID2, values2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Vector4>>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				List<Vector4> values3 = (List<Vector4>)objectTranslator.GetObject(L, 3, typeof(List<Vector4>));
				material.SetVectorArray(name, values3);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector4[]>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				Vector4[] values4 = (Vector4[])objectTranslator.GetObject(L, 3, typeof(Vector4[]));
				material.SetVectorArray(name2, values4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetVectorArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMatrixArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Matrix4x4>>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				List<Matrix4x4> values = (List<Matrix4x4>)objectTranslator.GetObject(L, 3, typeof(List<Matrix4x4>));
				material.SetMatrixArray(nameID, values);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Matrix4x4[]>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				Matrix4x4[] values2 = (Matrix4x4[])objectTranslator.GetObject(L, 3, typeof(Matrix4x4[]));
				material.SetMatrixArray(nameID2, values2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Matrix4x4>>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				List<Matrix4x4> values3 = (List<Matrix4x4>)objectTranslator.GetObject(L, 3, typeof(List<Matrix4x4>));
				material.SetMatrixArray(name, values3);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Matrix4x4[]>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				Matrix4x4[] values4 = (Matrix4x4[])objectTranslator.GetObject(L, 3, typeof(Matrix4x4[]));
				material.SetMatrixArray(name2, values4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetMatrixArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFloat(IntPtr L)
	{
		try
		{
			Material material = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				float num2 = material.GetFloat(nameID);
				Lua.lua_pushnumber(L, num2);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				float num3 = material.GetFloat(name);
				Lua.lua_pushnumber(L, num3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetFloat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInt(IntPtr L)
	{
		try
		{
			Material material = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				int value = material.GetInt(nameID);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				int value2 = material.GetInt(name);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetInt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Color color = material.GetColor(nameID);
				objectTranslator.PushUnityEngineColor(L, color);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Color color2 = material.GetColor(name);
				objectTranslator.PushUnityEngineColor(L, color2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetColor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetVector(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Vector4 vector = material.GetVector(nameID);
				objectTranslator.PushUnityEngineVector4(L, vector);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Vector4 vector2 = material.GetVector(name);
				objectTranslator.PushUnityEngineVector4(L, vector2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetVector!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Matrix4x4 matrix = material.GetMatrix(nameID);
				objectTranslator.Push(L, matrix);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Matrix4x4 matrix2 = material.GetMatrix(name);
				objectTranslator.Push(L, matrix2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetMatrix!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Texture texture = material.GetTexture(nameID);
				objectTranslator.Push(L, texture);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Texture texture2 = material.GetTexture(name);
				objectTranslator.Push(L, texture2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetTexture!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFloatArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				float[] floatArray = material.GetFloatArray(nameID);
				objectTranslator.Push(L, floatArray);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				float[] floatArray2 = material.GetFloatArray(name);
				objectTranslator.Push(L, floatArray2);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<float>>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				List<float> values = (List<float>)objectTranslator.GetObject(L, 3, typeof(List<float>));
				material.GetFloatArray(nameID2, values);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<float>>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				List<float> values2 = (List<float>)objectTranslator.GetObject(L, 3, typeof(List<float>));
				material.GetFloatArray(name2, values2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetFloatArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetColorArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Color[] colorArray = material.GetColorArray(nameID);
				objectTranslator.Push(L, colorArray);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Color[] colorArray2 = material.GetColorArray(name);
				objectTranslator.Push(L, colorArray2);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Color>>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				List<Color> values = (List<Color>)objectTranslator.GetObject(L, 3, typeof(List<Color>));
				material.GetColorArray(nameID2, values);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Color>>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				List<Color> values2 = (List<Color>)objectTranslator.GetObject(L, 3, typeof(List<Color>));
				material.GetColorArray(name2, values2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetColorArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetVectorArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Vector4[] vectorArray = material.GetVectorArray(nameID);
				objectTranslator.Push(L, vectorArray);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Vector4[] vectorArray2 = material.GetVectorArray(name);
				objectTranslator.Push(L, vectorArray2);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Vector4>>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				List<Vector4> values = (List<Vector4>)objectTranslator.GetObject(L, 3, typeof(List<Vector4>));
				material.GetVectorArray(nameID2, values);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Vector4>>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				List<Vector4> values2 = (List<Vector4>)objectTranslator.GetObject(L, 3, typeof(List<Vector4>));
				material.GetVectorArray(name2, values2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetVectorArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMatrixArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Matrix4x4[] matrixArray = material.GetMatrixArray(nameID);
				objectTranslator.Push(L, matrixArray);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Matrix4x4[] matrixArray2 = material.GetMatrixArray(name);
				objectTranslator.Push(L, matrixArray2);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Matrix4x4>>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				List<Matrix4x4> values = (List<Matrix4x4>)objectTranslator.GetObject(L, 3, typeof(List<Matrix4x4>));
				material.GetMatrixArray(nameID2, values);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Matrix4x4>>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				List<Matrix4x4> values2 = (List<Matrix4x4>)objectTranslator.GetObject(L, 3, typeof(List<Matrix4x4>));
				material.GetMatrixArray(name2, values2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetMatrixArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTextureOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector2>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out Vector2 val);
				material.SetTextureOffset(nameID, val);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector2>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out Vector2 val2);
				material.SetTextureOffset(name, val2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetTextureOffset!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTextureScale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector2>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out Vector2 val);
				material.SetTextureScale(nameID, val);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector2>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out Vector2 val2);
				material.SetTextureScale(name, val2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.SetTextureScale!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTextureOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Vector2 textureOffset = material.GetTextureOffset(nameID);
				objectTranslator.PushUnityEngineVector2(L, textureOffset);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Vector2 textureOffset2 = material.GetTextureOffset(name);
				objectTranslator.PushUnityEngineVector2(L, textureOffset2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetTextureOffset!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTextureScale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Vector2 textureScale = material.GetTextureScale(nameID);
				objectTranslator.PushUnityEngineVector2(L, textureScale);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Vector2 textureScale2 = material.GetTextureScale(name);
				objectTranslator.PushUnityEngineVector2(L, textureScale2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.GetTextureScale!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material target = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Color>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Color val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Color, Color, ColorOptions> o = target.DOColor(val, duration);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Color>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Color val2);
				int propertyID = Lua.xlua_tointeger(L, 3);
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<Color, Color, ColorOptions> o2 = target.DOColor(val2, propertyID, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Color>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Color val3);
				string property = Lua.lua_tostring(L, 3);
				float duration3 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<Color, Color, ColorOptions> o3 = target.DOColor(val3, property, duration3);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.DOColor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOFade(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material target = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Color, Color, ColorOptions> o = target.DOFade(endValue, duration);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				int propertyID = Lua.xlua_tointeger(L, 3);
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<Color, Color, ColorOptions> o2 = target.DOFade(endValue2, propertyID, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float endValue3 = (float)Lua.lua_tonumber(L, 2);
				string property = Lua.lua_tostring(L, 3);
				float duration3 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<Color, Color, ColorOptions> o3 = target.DOFade(endValue3, property, duration3);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.DOFade!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOFloat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material target = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				int propertyID = Lua.xlua_tointeger(L, 3);
				float duration = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<float, float, FloatOptions> o = target.DOFloat(endValue, propertyID, duration);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				string property = Lua.lua_tostring(L, 3);
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<float, float, FloatOptions> o2 = target.DOFloat(endValue2, property, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.DOFloat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material target = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector2, Vector2, VectorOptions> o = target.DOOffset(val, duration);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				string property = Lua.lua_tostring(L, 3);
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<Vector2, Vector2, VectorOptions> o2 = target.DOOffset(val2, property, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.DOOffset!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOTiling(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material target = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector2, Vector2, VectorOptions> o = target.DOTiling(val, duration);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				string property = Lua.lua_tostring(L, 3);
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<Vector2, Vector2, VectorOptions> o2 = target.DOTiling(val2, property, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.DOTiling!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOVector(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material target = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector4>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector4 val);
				int propertyID = Lua.xlua_tointeger(L, 3);
				float duration = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<Vector4, Vector4, VectorOptions> o = target.DOVector(val, propertyID, duration);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector4>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector4 val2);
				string property = Lua.lua_tostring(L, 3);
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<Vector4, Vector4, VectorOptions> o2 = target.DOVector(val2, property, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.DOVector!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOBlendableColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material target = (Material)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Color>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Color val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				Tweener o = target.DOBlendableColor(val, duration);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Color>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Color val2);
				int propertyID = Lua.xlua_tointeger(L, 3);
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				Tweener o2 = target.DOBlendableColor(val2, propertyID, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Color>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Color val3);
				string property = Lua.lua_tostring(L, 3);
				float duration3 = (float)Lua.lua_tonumber(L, 4);
				Tweener o3 = target.DOBlendableColor(val3, property, duration3);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.DOBlendableColor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOComplete(IntPtr L)
	{
		try
		{
			Material target = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool withCallbacks = Lua.lua_toboolean(L, 2);
				int value = target.DOComplete(withCallbacks);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 1)
			{
				int value2 = target.DOComplete();
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.DOComplete!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOKill(IntPtr L)
	{
		try
		{
			Material target = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool complete = Lua.lua_toboolean(L, 2);
				int value = target.DOKill(complete);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 1)
			{
				int value2 = target.DOKill();
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.DOKill!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOFlip(IntPtr L)
	{
		try
		{
			int value = ((Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOFlip();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOGoto(IntPtr L)
	{
		try
		{
			Material target = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				float to = (float)Lua.lua_tonumber(L, 2);
				bool andPlay = Lua.lua_toboolean(L, 3);
				int value = target.DOGoto(to, andPlay);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float to2 = (float)Lua.lua_tonumber(L, 2);
				int value2 = target.DOGoto(to2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.DOGoto!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPause(IntPtr L)
	{
		try
		{
			int value = ((Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOPause();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlay(IntPtr L)
	{
		try
		{
			int value = ((Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOPlay();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlayBackwards(IntPtr L)
	{
		try
		{
			int value = ((Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOPlayBackwards();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlayForward(IntPtr L)
	{
		try
		{
			int value = ((Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOPlayForward();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DORestart(IntPtr L)
	{
		try
		{
			Material target = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool includeDelay = Lua.lua_toboolean(L, 2);
				int value = target.DORestart(includeDelay);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 1)
			{
				int value2 = target.DORestart();
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.DORestart!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DORewind(IntPtr L)
	{
		try
		{
			Material target = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool includeDelay = Lua.lua_toboolean(L, 2);
				int value = target.DORewind(includeDelay);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 1)
			{
				int value2 = target.DORewind();
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Material.DORewind!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOSmoothRewind(IntPtr L)
	{
		try
		{
			int value = ((Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOSmoothRewind();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOTogglePause(IntPtr L)
	{
		try
		{
			int value = ((Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOTogglePause();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, material.shader);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_color(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, material.color);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mainTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, material.mainTexture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mainTextureOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, material.mainTextureOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mainTextureScale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, material.mainTextureScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderQueue(IntPtr L)
	{
		try
		{
			Material material = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, material.renderQueue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_globalIlluminationFlags(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, material.globalIlluminationFlags);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_doubleSidedGI(IntPtr L)
	{
		try
		{
			Material material = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, material.doubleSidedGI);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enableInstancing(IntPtr L)
	{
		try
		{
			Material material = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, material.enableInstancing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_passCount(IntPtr L)
	{
		try
		{
			Material material = (Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, material.passCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shaderKeywords(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, material.shaderKeywords);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Material)objectTranslator.FastGetCSObj(L, 1)).shader = (Shader)objectTranslator.GetObject(L, 2, typeof(Shader));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_color(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			material.color = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mainTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Material)objectTranslator.FastGetCSObj(L, 1)).mainTexture = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mainTextureOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			material.mainTextureOffset = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mainTextureScale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			material.mainTextureScale = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderQueue(IntPtr L)
	{
		try
		{
			((Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).renderQueue = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_globalIlluminationFlags(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material material = (Material)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out MaterialGlobalIlluminationFlags v);
			material.globalIlluminationFlags = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_doubleSidedGI(IntPtr L)
	{
		try
		{
			((Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).doubleSidedGI = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enableInstancing(IntPtr L)
	{
		try
		{
			((Material)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enableInstancing = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shaderKeywords(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Material)objectTranslator.FastGetCSObj(L, 1)).shaderKeywords = (string[])objectTranslator.GetObject(L, 2, typeof(string[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
