using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineShaderWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Shader);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 15, 4, 1);
		Utils.RegisterFunc(L, -3, "GetDependency", _m_GetDependency);
		Utils.RegisterFunc(L, -3, "FindPassTagValue", _m_FindPassTagValue);
		Utils.RegisterFunc(L, -3, "GetPropertyCount", _m_GetPropertyCount);
		Utils.RegisterFunc(L, -3, "FindPropertyIndex", _m_FindPropertyIndex);
		Utils.RegisterFunc(L, -3, "GetPropertyName", _m_GetPropertyName);
		Utils.RegisterFunc(L, -3, "GetPropertyNameId", _m_GetPropertyNameId);
		Utils.RegisterFunc(L, -3, "GetPropertyType", _m_GetPropertyType);
		Utils.RegisterFunc(L, -3, "GetPropertyDescription", _m_GetPropertyDescription);
		Utils.RegisterFunc(L, -3, "GetPropertyFlags", _m_GetPropertyFlags);
		Utils.RegisterFunc(L, -3, "GetPropertyAttributes", _m_GetPropertyAttributes);
		Utils.RegisterFunc(L, -3, "GetPropertyDefaultFloatValue", _m_GetPropertyDefaultFloatValue);
		Utils.RegisterFunc(L, -3, "GetPropertyDefaultVectorValue", _m_GetPropertyDefaultVectorValue);
		Utils.RegisterFunc(L, -3, "GetPropertyRangeLimits", _m_GetPropertyRangeLimits);
		Utils.RegisterFunc(L, -3, "GetPropertyTextureDimension", _m_GetPropertyTextureDimension);
		Utils.RegisterFunc(L, -3, "GetPropertyTextureDefaultName", _m_GetPropertyTextureDefaultName);
		Utils.RegisterFunc(L, -2, "maximumLOD", _g_get_maximumLOD);
		Utils.RegisterFunc(L, -2, "isSupported", _g_get_isSupported);
		Utils.RegisterFunc(L, -2, "renderQueue", _g_get_renderQueue);
		Utils.RegisterFunc(L, -2, "passCount", _g_get_passCount);
		Utils.RegisterFunc(L, -1, "maximumLOD", _s_set_maximumLOD);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 27, 2, 2);
		Utils.RegisterFunc(L, -4, "Find", _m_Find_xlua_st_);
		Utils.RegisterFunc(L, -4, "EnableKeyword", _m_EnableKeyword_xlua_st_);
		Utils.RegisterFunc(L, -4, "DisableKeyword", _m_DisableKeyword_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsKeywordEnabled", _m_IsKeywordEnabled_xlua_st_);
		Utils.RegisterFunc(L, -4, "WarmupAllShaders", _m_WarmupAllShaders_xlua_st_);
		Utils.RegisterFunc(L, -4, "PropertyToID", _m_PropertyToID_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGlobalFloat", _m_SetGlobalFloat_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGlobalInt", _m_SetGlobalInt_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGlobalVector", _m_SetGlobalVector_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGlobalColor", _m_SetGlobalColor_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGlobalMatrix", _m_SetGlobalMatrix_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGlobalTexture", _m_SetGlobalTexture_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGlobalBuffer", _m_SetGlobalBuffer_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGlobalConstantBuffer", _m_SetGlobalConstantBuffer_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGlobalFloatArray", _m_SetGlobalFloatArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGlobalVectorArray", _m_SetGlobalVectorArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGlobalMatrixArray", _m_SetGlobalMatrixArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGlobalFloat", _m_GetGlobalFloat_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGlobalInt", _m_GetGlobalInt_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGlobalVector", _m_GetGlobalVector_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGlobalColor", _m_GetGlobalColor_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGlobalMatrix", _m_GetGlobalMatrix_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGlobalTexture", _m_GetGlobalTexture_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGlobalFloatArray", _m_GetGlobalFloatArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGlobalVectorArray", _m_GetGlobalVectorArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGlobalMatrixArray", _m_GetGlobalMatrixArray_xlua_st_);
		Utils.RegisterFunc(L, -2, "globalMaximumLOD", _g_get_globalMaximumLOD);
		Utils.RegisterFunc(L, -2, "globalRenderPipeline", _g_get_globalRenderPipeline);
		Utils.RegisterFunc(L, -1, "globalMaximumLOD", _s_set_globalMaximumLOD);
		Utils.RegisterFunc(L, -1, "globalRenderPipeline", _s_set_globalRenderPipeline);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.Shader does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Find_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shader o = Shader.Find(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnableKeyword_xlua_st_(IntPtr L)
	{
		try
		{
			Shader.EnableKeyword(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DisableKeyword_xlua_st_(IntPtr L)
	{
		try
		{
			Shader.DisableKeyword(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsKeywordEnabled_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = Shader.IsKeywordEnabled(Lua.lua_tostring(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WarmupAllShaders_xlua_st_(IntPtr L)
	{
		try
		{
			Shader.WarmupAllShaders();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PropertyToID_xlua_st_(IntPtr L)
	{
		try
		{
			int value = Shader.PropertyToID(Lua.lua_tostring(L, 1));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDependency(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shader obj = (Shader)objectTranslator.FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			Shader dependency = obj.GetDependency(name);
			objectTranslator.Push(L, dependency);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindPassTagValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shader shader = (Shader)objectTranslator.FastGetCSObj(L, 1);
			int passIndex = Lua.xlua_tointeger(L, 2);
			objectTranslator.Get(L, 3, out ShaderTagId v);
			ShaderTagId shaderTagId = shader.FindPassTagValue(passIndex, v);
			objectTranslator.Push(L, shaderTagId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalFloat_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 1);
				float value = (float)Lua.lua_tonumber(L, 2);
				Shader.SetGlobalFloat(nameID, value);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				string name = Lua.lua_tostring(L, 1);
				float value2 = (float)Lua.lua_tonumber(L, 2);
				Shader.SetGlobalFloat(name, value2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.SetGlobalFloat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalInt_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 1);
				int value = Lua.xlua_tointeger(L, 2);
				Shader.SetGlobalInt(nameID, value);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				string name = Lua.lua_tostring(L, 1);
				int value2 = Lua.xlua_tointeger(L, 2);
				Shader.SetGlobalInt(name, value2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.SetGlobalInt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalVector_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<Vector4>(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 1);
				objectTranslator.Get(L, 2, out Vector4 val);
				Shader.SetGlobalVector(nameID, val);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector4>(L, 2))
			{
				string name = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out Vector4 val2);
				Shader.SetGlobalVector(name, val2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.SetGlobalVector!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalColor_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<Color>(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 1);
				objectTranslator.Get(L, 2, out Color val);
				Shader.SetGlobalColor(nameID, val);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Color>(L, 2))
			{
				string name = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out Color val2);
				Shader.SetGlobalColor(name, val2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.SetGlobalColor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalMatrix_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 1);
				objectTranslator.Get(L, 2, out Matrix4x4 v);
				Shader.SetGlobalMatrix(nameID, v);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Matrix4x4>(L, 2))
			{
				string name = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out Matrix4x4 v2);
				Shader.SetGlobalMatrix(name, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.SetGlobalMatrix!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalTexture_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<Texture>(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 1);
				Texture value = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				Shader.SetGlobalTexture(nameID, value);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Texture>(L, 2))
			{
				string name = Lua.lua_tostring(L, 1);
				Texture value2 = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				Shader.SetGlobalTexture(name, value2);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<RenderTexture>(L, 2) && objectTranslator.Assignable<RenderTextureSubElement>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 1);
				RenderTexture value3 = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				objectTranslator.Get(L, 3, out RenderTextureSubElement v);
				Shader.SetGlobalTexture(nameID2, value3, v);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<RenderTexture>(L, 2) && objectTranslator.Assignable<RenderTextureSubElement>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 1);
				RenderTexture value4 = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				objectTranslator.Get(L, 3, out RenderTextureSubElement v2);
				Shader.SetGlobalTexture(name2, value4, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.SetGlobalTexture!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalBuffer_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<ComputeBuffer>(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 1);
				ComputeBuffer value = (ComputeBuffer)objectTranslator.GetObject(L, 2, typeof(ComputeBuffer));
				Shader.SetGlobalBuffer(nameID, value);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<ComputeBuffer>(L, 2))
			{
				string name = Lua.lua_tostring(L, 1);
				ComputeBuffer value2 = (ComputeBuffer)objectTranslator.GetObject(L, 2, typeof(ComputeBuffer));
				Shader.SetGlobalBuffer(name, value2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.SetGlobalBuffer!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalConstantBuffer_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int nameID = Lua.xlua_tointeger(L, 1);
			ComputeBuffer value = (ComputeBuffer)objectTranslator.GetObject(L, 2, typeof(ComputeBuffer));
			int offset = Lua.xlua_tointeger(L, 3);
			int size = Lua.xlua_tointeger(L, 4);
			Shader.SetGlobalConstantBuffer(nameID, value, offset, size);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalFloatArray_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<List<float>>(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 1);
				List<float> values = (List<float>)objectTranslator.GetObject(L, 2, typeof(List<float>));
				Shader.SetGlobalFloatArray(nameID, values);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<float[]>(L, 2))
			{
				int nameID2 = Lua.xlua_tointeger(L, 1);
				float[] values2 = (float[])objectTranslator.GetObject(L, 2, typeof(float[]));
				Shader.SetGlobalFloatArray(nameID2, values2);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<float>>(L, 2))
			{
				string name = Lua.lua_tostring(L, 1);
				List<float> values3 = (List<float>)objectTranslator.GetObject(L, 2, typeof(List<float>));
				Shader.SetGlobalFloatArray(name, values3);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<float[]>(L, 2))
			{
				string name2 = Lua.lua_tostring(L, 1);
				float[] values4 = (float[])objectTranslator.GetObject(L, 2, typeof(float[]));
				Shader.SetGlobalFloatArray(name2, values4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.SetGlobalFloatArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalVectorArray_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<List<Vector4>>(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 1);
				List<Vector4> values = (List<Vector4>)objectTranslator.GetObject(L, 2, typeof(List<Vector4>));
				Shader.SetGlobalVectorArray(nameID, values);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<Vector4[]>(L, 2))
			{
				int nameID2 = Lua.xlua_tointeger(L, 1);
				Vector4[] values2 = (Vector4[])objectTranslator.GetObject(L, 2, typeof(Vector4[]));
				Shader.SetGlobalVectorArray(nameID2, values2);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Vector4>>(L, 2))
			{
				string name = Lua.lua_tostring(L, 1);
				List<Vector4> values3 = (List<Vector4>)objectTranslator.GetObject(L, 2, typeof(List<Vector4>));
				Shader.SetGlobalVectorArray(name, values3);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector4[]>(L, 2))
			{
				string name2 = Lua.lua_tostring(L, 1);
				Vector4[] values4 = (Vector4[])objectTranslator.GetObject(L, 2, typeof(Vector4[]));
				Shader.SetGlobalVectorArray(name2, values4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.SetGlobalVectorArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalMatrixArray_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<List<Matrix4x4>>(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 1);
				List<Matrix4x4> values = (List<Matrix4x4>)objectTranslator.GetObject(L, 2, typeof(List<Matrix4x4>));
				Shader.SetGlobalMatrixArray(nameID, values);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<Matrix4x4[]>(L, 2))
			{
				int nameID2 = Lua.xlua_tointeger(L, 1);
				Matrix4x4[] values2 = (Matrix4x4[])objectTranslator.GetObject(L, 2, typeof(Matrix4x4[]));
				Shader.SetGlobalMatrixArray(nameID2, values2);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Matrix4x4>>(L, 2))
			{
				string name = Lua.lua_tostring(L, 1);
				List<Matrix4x4> values3 = (List<Matrix4x4>)objectTranslator.GetObject(L, 2, typeof(List<Matrix4x4>));
				Shader.SetGlobalMatrixArray(name, values3);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Matrix4x4[]>(L, 2))
			{
				string name2 = Lua.lua_tostring(L, 1);
				Matrix4x4[] values4 = (Matrix4x4[])objectTranslator.GetObject(L, 2, typeof(Matrix4x4[]));
				Shader.SetGlobalMatrixArray(name2, values4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.SetGlobalMatrixArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGlobalFloat_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				float globalFloat = Shader.GetGlobalFloat(Lua.xlua_tointeger(L, 1));
				Lua.lua_pushnumber(L, globalFloat);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				float globalFloat2 = Shader.GetGlobalFloat(Lua.lua_tostring(L, 1));
				Lua.lua_pushnumber(L, globalFloat2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.GetGlobalFloat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGlobalInt_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				int globalInt = Shader.GetGlobalInt(Lua.xlua_tointeger(L, 1));
				Lua.xlua_pushinteger(L, globalInt);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				int globalInt2 = Shader.GetGlobalInt(Lua.lua_tostring(L, 1));
				Lua.xlua_pushinteger(L, globalInt2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.GetGlobalInt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGlobalVector_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				Vector4 globalVector = Shader.GetGlobalVector(Lua.xlua_tointeger(L, 1));
				objectTranslator.PushUnityEngineVector4(L, globalVector);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				Vector4 globalVector2 = Shader.GetGlobalVector(Lua.lua_tostring(L, 1));
				objectTranslator.PushUnityEngineVector4(L, globalVector2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.GetGlobalVector!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGlobalColor_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				Color globalColor = Shader.GetGlobalColor(Lua.xlua_tointeger(L, 1));
				objectTranslator.PushUnityEngineColor(L, globalColor);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				Color globalColor2 = Shader.GetGlobalColor(Lua.lua_tostring(L, 1));
				objectTranslator.PushUnityEngineColor(L, globalColor2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.GetGlobalColor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGlobalMatrix_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				Matrix4x4 globalMatrix = Shader.GetGlobalMatrix(Lua.xlua_tointeger(L, 1));
				objectTranslator.Push(L, globalMatrix);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				Matrix4x4 globalMatrix2 = Shader.GetGlobalMatrix(Lua.lua_tostring(L, 1));
				objectTranslator.Push(L, globalMatrix2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.GetGlobalMatrix!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGlobalTexture_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				Texture globalTexture = Shader.GetGlobalTexture(Lua.xlua_tointeger(L, 1));
				objectTranslator.Push(L, globalTexture);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				Texture globalTexture2 = Shader.GetGlobalTexture(Lua.lua_tostring(L, 1));
				objectTranslator.Push(L, globalTexture2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.GetGlobalTexture!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGlobalFloatArray_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				float[] globalFloatArray = Shader.GetGlobalFloatArray(Lua.xlua_tointeger(L, 1));
				objectTranslator.Push(L, globalFloatArray);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				float[] globalFloatArray2 = Shader.GetGlobalFloatArray(Lua.lua_tostring(L, 1));
				objectTranslator.Push(L, globalFloatArray2);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<List<float>>(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 1);
				List<float> values = (List<float>)objectTranslator.GetObject(L, 2, typeof(List<float>));
				Shader.GetGlobalFloatArray(nameID, values);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<float>>(L, 2))
			{
				string name = Lua.lua_tostring(L, 1);
				List<float> values2 = (List<float>)objectTranslator.GetObject(L, 2, typeof(List<float>));
				Shader.GetGlobalFloatArray(name, values2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.GetGlobalFloatArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGlobalVectorArray_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				Vector4[] globalVectorArray = Shader.GetGlobalVectorArray(Lua.xlua_tointeger(L, 1));
				objectTranslator.Push(L, globalVectorArray);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				Vector4[] globalVectorArray2 = Shader.GetGlobalVectorArray(Lua.lua_tostring(L, 1));
				objectTranslator.Push(L, globalVectorArray2);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<List<Vector4>>(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 1);
				List<Vector4> values = (List<Vector4>)objectTranslator.GetObject(L, 2, typeof(List<Vector4>));
				Shader.GetGlobalVectorArray(nameID, values);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Vector4>>(L, 2))
			{
				string name = Lua.lua_tostring(L, 1);
				List<Vector4> values2 = (List<Vector4>)objectTranslator.GetObject(L, 2, typeof(List<Vector4>));
				Shader.GetGlobalVectorArray(name, values2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.GetGlobalVectorArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGlobalMatrixArray_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				Matrix4x4[] globalMatrixArray = Shader.GetGlobalMatrixArray(Lua.xlua_tointeger(L, 1));
				objectTranslator.Push(L, globalMatrixArray);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				Matrix4x4[] globalMatrixArray2 = Shader.GetGlobalMatrixArray(Lua.lua_tostring(L, 1));
				objectTranslator.Push(L, globalMatrixArray2);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<List<Matrix4x4>>(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 1);
				List<Matrix4x4> values = (List<Matrix4x4>)objectTranslator.GetObject(L, 2, typeof(List<Matrix4x4>));
				Shader.GetGlobalMatrixArray(nameID, values);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Matrix4x4>>(L, 2))
			{
				string name = Lua.lua_tostring(L, 1);
				List<Matrix4x4> values2 = (List<Matrix4x4>)objectTranslator.GetObject(L, 2, typeof(List<Matrix4x4>));
				Shader.GetGlobalMatrixArray(name, values2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Shader.GetGlobalMatrixArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropertyCount(IntPtr L)
	{
		try
		{
			int propertyCount = ((Shader)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPropertyCount();
			Lua.xlua_pushinteger(L, propertyCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindPropertyIndex(IntPtr L)
	{
		try
		{
			Shader obj = (Shader)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string propertyName = Lua.lua_tostring(L, 2);
			int value = obj.FindPropertyIndex(propertyName);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropertyName(IntPtr L)
	{
		try
		{
			Shader obj = (Shader)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int propertyIndex = Lua.xlua_tointeger(L, 2);
			string propertyName = obj.GetPropertyName(propertyIndex);
			Lua.lua_pushstring(L, propertyName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropertyNameId(IntPtr L)
	{
		try
		{
			Shader obj = (Shader)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int propertyIndex = Lua.xlua_tointeger(L, 2);
			int propertyNameId = obj.GetPropertyNameId(propertyIndex);
			Lua.xlua_pushinteger(L, propertyNameId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropertyType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shader obj = (Shader)objectTranslator.FastGetCSObj(L, 1);
			int propertyIndex = Lua.xlua_tointeger(L, 2);
			ShaderPropertyType propertyType = obj.GetPropertyType(propertyIndex);
			objectTranslator.Push(L, propertyType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropertyDescription(IntPtr L)
	{
		try
		{
			Shader obj = (Shader)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int propertyIndex = Lua.xlua_tointeger(L, 2);
			string propertyDescription = obj.GetPropertyDescription(propertyIndex);
			Lua.lua_pushstring(L, propertyDescription);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropertyFlags(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shader obj = (Shader)objectTranslator.FastGetCSObj(L, 1);
			int propertyIndex = Lua.xlua_tointeger(L, 2);
			ShaderPropertyFlags propertyFlags = obj.GetPropertyFlags(propertyIndex);
			objectTranslator.Push(L, propertyFlags);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropertyAttributes(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shader obj = (Shader)objectTranslator.FastGetCSObj(L, 1);
			int propertyIndex = Lua.xlua_tointeger(L, 2);
			string[] propertyAttributes = obj.GetPropertyAttributes(propertyIndex);
			objectTranslator.Push(L, propertyAttributes);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropertyDefaultFloatValue(IntPtr L)
	{
		try
		{
			Shader obj = (Shader)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int propertyIndex = Lua.xlua_tointeger(L, 2);
			float propertyDefaultFloatValue = obj.GetPropertyDefaultFloatValue(propertyIndex);
			Lua.lua_pushnumber(L, propertyDefaultFloatValue);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropertyDefaultVectorValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shader obj = (Shader)objectTranslator.FastGetCSObj(L, 1);
			int propertyIndex = Lua.xlua_tointeger(L, 2);
			Vector4 propertyDefaultVectorValue = obj.GetPropertyDefaultVectorValue(propertyIndex);
			objectTranslator.PushUnityEngineVector4(L, propertyDefaultVectorValue);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropertyRangeLimits(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shader obj = (Shader)objectTranslator.FastGetCSObj(L, 1);
			int propertyIndex = Lua.xlua_tointeger(L, 2);
			Vector2 propertyRangeLimits = obj.GetPropertyRangeLimits(propertyIndex);
			objectTranslator.PushUnityEngineVector2(L, propertyRangeLimits);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropertyTextureDimension(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shader obj = (Shader)objectTranslator.FastGetCSObj(L, 1);
			int propertyIndex = Lua.xlua_tointeger(L, 2);
			TextureDimension propertyTextureDimension = obj.GetPropertyTextureDimension(propertyIndex);
			objectTranslator.Push(L, propertyTextureDimension);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropertyTextureDefaultName(IntPtr L)
	{
		try
		{
			Shader obj = (Shader)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int propertyIndex = Lua.xlua_tointeger(L, 2);
			string propertyTextureDefaultName = obj.GetPropertyTextureDefaultName(propertyIndex);
			Lua.lua_pushstring(L, propertyTextureDefaultName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maximumLOD(IntPtr L)
	{
		try
		{
			Shader shader = (Shader)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, shader.maximumLOD);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_globalMaximumLOD(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Shader.globalMaximumLOD);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isSupported(IntPtr L)
	{
		try
		{
			Shader shader = (Shader)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, shader.isSupported);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_globalRenderPipeline(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Shader.globalRenderPipeline);
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
			Shader shader = (Shader)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, shader.renderQueue);
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
			Shader shader = (Shader)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, shader.passCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maximumLOD(IntPtr L)
	{
		try
		{
			((Shader)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maximumLOD = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_globalMaximumLOD(IntPtr L)
	{
		try
		{
			Shader.globalMaximumLOD = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_globalRenderPipeline(IntPtr L)
	{
		try
		{
			Shader.globalRenderPipeline = Lua.lua_tostring(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
