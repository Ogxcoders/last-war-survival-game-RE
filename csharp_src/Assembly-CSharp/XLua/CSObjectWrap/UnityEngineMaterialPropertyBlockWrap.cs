using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineMaterialPropertyBlockWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MaterialPropertyBlock);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 23, 1, 0);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "SetFloat", _m_SetFloat);
		Utils.RegisterFunc(L, -3, "SetInt", _m_SetInt);
		Utils.RegisterFunc(L, -3, "SetVector", _m_SetVector);
		Utils.RegisterFunc(L, -3, "SetColor", _m_SetColor);
		Utils.RegisterFunc(L, -3, "SetMatrix", _m_SetMatrix);
		Utils.RegisterFunc(L, -3, "SetBuffer", _m_SetBuffer);
		Utils.RegisterFunc(L, -3, "SetTexture", _m_SetTexture);
		Utils.RegisterFunc(L, -3, "SetConstantBuffer", _m_SetConstantBuffer);
		Utils.RegisterFunc(L, -3, "SetFloatArray", _m_SetFloatArray);
		Utils.RegisterFunc(L, -3, "SetVectorArray", _m_SetVectorArray);
		Utils.RegisterFunc(L, -3, "SetMatrixArray", _m_SetMatrixArray);
		Utils.RegisterFunc(L, -3, "GetFloat", _m_GetFloat);
		Utils.RegisterFunc(L, -3, "GetInt", _m_GetInt);
		Utils.RegisterFunc(L, -3, "GetVector", _m_GetVector);
		Utils.RegisterFunc(L, -3, "GetColor", _m_GetColor);
		Utils.RegisterFunc(L, -3, "GetMatrix", _m_GetMatrix);
		Utils.RegisterFunc(L, -3, "GetTexture", _m_GetTexture);
		Utils.RegisterFunc(L, -3, "GetFloatArray", _m_GetFloatArray);
		Utils.RegisterFunc(L, -3, "GetVectorArray", _m_GetVectorArray);
		Utils.RegisterFunc(L, -3, "GetMatrixArray", _m_GetMatrixArray);
		Utils.RegisterFunc(L, -3, "CopySHCoefficientArraysFrom", _m_CopySHCoefficientArraysFrom);
		Utils.RegisterFunc(L, -3, "CopyProbeOcclusionArrayFrom", _m_CopyProbeOcclusionArrayFrom);
		Utils.RegisterFunc(L, -2, "isEmpty", _g_get_isEmpty);
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
				MaterialPropertyBlock o = new MaterialPropertyBlock();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((MaterialPropertyBlock)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFloat(IntPtr L)
	{
		try
		{
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				float value = (float)Lua.lua_tonumber(L, 3);
				materialPropertyBlock.SetFloat(nameID, value);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				float value2 = (float)Lua.lua_tonumber(L, 3);
				materialPropertyBlock.SetFloat(name, value2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.SetFloat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetInt(IntPtr L)
	{
		try
		{
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				int value = Lua.xlua_tointeger(L, 3);
				materialPropertyBlock.SetInt(nameID, value);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				int value2 = Lua.xlua_tointeger(L, 3);
				materialPropertyBlock.SetInt(name, value2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.SetInt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVector(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector4>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out Vector4 val);
				materialPropertyBlock.SetVector(nameID, val);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector4>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out Vector4 val2);
				materialPropertyBlock.SetVector(name, val2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.SetVector!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Color>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out Color val);
				materialPropertyBlock.SetColor(nameID, val);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Color>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out Color val2);
				materialPropertyBlock.SetColor(name, val2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.SetColor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Matrix4x4>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out Matrix4x4 v);
				materialPropertyBlock.SetMatrix(nameID, v);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Matrix4x4>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out Matrix4x4 v2);
				materialPropertyBlock.SetMatrix(name, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.SetMatrix!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBuffer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<ComputeBuffer>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				ComputeBuffer value = (ComputeBuffer)objectTranslator.GetObject(L, 3, typeof(ComputeBuffer));
				materialPropertyBlock.SetBuffer(nameID, value);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<ComputeBuffer>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				ComputeBuffer value2 = (ComputeBuffer)objectTranslator.GetObject(L, 3, typeof(ComputeBuffer));
				materialPropertyBlock.SetBuffer(name, value2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.SetBuffer!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Texture>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Texture value = (Texture)objectTranslator.GetObject(L, 3, typeof(Texture));
				materialPropertyBlock.SetTexture(nameID, value);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Texture>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				Texture value2 = (Texture)objectTranslator.GetObject(L, 3, typeof(Texture));
				materialPropertyBlock.SetTexture(name, value2);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<RenderTexture>(L, 3) && objectTranslator.Assignable<RenderTextureSubElement>(L, 4))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				RenderTexture value3 = (RenderTexture)objectTranslator.GetObject(L, 3, typeof(RenderTexture));
				objectTranslator.Get(L, 4, out RenderTextureSubElement v);
				materialPropertyBlock.SetTexture(nameID2, value3, v);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<RenderTexture>(L, 3) && objectTranslator.Assignable<RenderTextureSubElement>(L, 4))
			{
				string name2 = Lua.lua_tostring(L, 2);
				RenderTexture value4 = (RenderTexture)objectTranslator.GetObject(L, 3, typeof(RenderTexture));
				objectTranslator.Get(L, 4, out RenderTextureSubElement v2);
				materialPropertyBlock.SetTexture(name2, value4, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.SetTexture!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetConstantBuffer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<ComputeBuffer>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				ComputeBuffer value = (ComputeBuffer)objectTranslator.GetObject(L, 3, typeof(ComputeBuffer));
				int offset = Lua.xlua_tointeger(L, 4);
				int size = Lua.xlua_tointeger(L, 5);
				materialPropertyBlock.SetConstantBuffer(nameID, value, offset, size);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<ComputeBuffer>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string name = Lua.lua_tostring(L, 2);
				ComputeBuffer value2 = (ComputeBuffer)objectTranslator.GetObject(L, 3, typeof(ComputeBuffer));
				int offset2 = Lua.xlua_tointeger(L, 4);
				int size2 = Lua.xlua_tointeger(L, 5);
				materialPropertyBlock.SetConstantBuffer(name, value2, offset2, size2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.SetConstantBuffer!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFloatArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<float>>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				List<float> values = (List<float>)objectTranslator.GetObject(L, 3, typeof(List<float>));
				materialPropertyBlock.SetFloatArray(nameID, values);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<float[]>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				float[] values2 = (float[])objectTranslator.GetObject(L, 3, typeof(float[]));
				materialPropertyBlock.SetFloatArray(nameID2, values2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<float>>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				List<float> values3 = (List<float>)objectTranslator.GetObject(L, 3, typeof(List<float>));
				materialPropertyBlock.SetFloatArray(name, values3);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<float[]>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				float[] values4 = (float[])objectTranslator.GetObject(L, 3, typeof(float[]));
				materialPropertyBlock.SetFloatArray(name2, values4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.SetFloatArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVectorArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Vector4>>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				List<Vector4> values = (List<Vector4>)objectTranslator.GetObject(L, 3, typeof(List<Vector4>));
				materialPropertyBlock.SetVectorArray(nameID, values);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector4[]>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				Vector4[] values2 = (Vector4[])objectTranslator.GetObject(L, 3, typeof(Vector4[]));
				materialPropertyBlock.SetVectorArray(nameID2, values2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Vector4>>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				List<Vector4> values3 = (List<Vector4>)objectTranslator.GetObject(L, 3, typeof(List<Vector4>));
				materialPropertyBlock.SetVectorArray(name, values3);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector4[]>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				Vector4[] values4 = (Vector4[])objectTranslator.GetObject(L, 3, typeof(Vector4[]));
				materialPropertyBlock.SetVectorArray(name2, values4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.SetVectorArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMatrixArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Matrix4x4>>(L, 3))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				List<Matrix4x4> values = (List<Matrix4x4>)objectTranslator.GetObject(L, 3, typeof(List<Matrix4x4>));
				materialPropertyBlock.SetMatrixArray(nameID, values);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Matrix4x4[]>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				Matrix4x4[] values2 = (Matrix4x4[])objectTranslator.GetObject(L, 3, typeof(Matrix4x4[]));
				materialPropertyBlock.SetMatrixArray(nameID2, values2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Matrix4x4>>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				List<Matrix4x4> values3 = (List<Matrix4x4>)objectTranslator.GetObject(L, 3, typeof(List<Matrix4x4>));
				materialPropertyBlock.SetMatrixArray(name, values3);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Matrix4x4[]>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				Matrix4x4[] values4 = (Matrix4x4[])objectTranslator.GetObject(L, 3, typeof(Matrix4x4[]));
				materialPropertyBlock.SetMatrixArray(name2, values4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.SetMatrixArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFloat(IntPtr L)
	{
		try
		{
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				float num2 = materialPropertyBlock.GetFloat(nameID);
				Lua.lua_pushnumber(L, num2);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				float num3 = materialPropertyBlock.GetFloat(name);
				Lua.lua_pushnumber(L, num3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.GetFloat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInt(IntPtr L)
	{
		try
		{
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				int value = materialPropertyBlock.GetInt(nameID);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				int value2 = materialPropertyBlock.GetInt(name);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.GetInt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetVector(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Vector4 vector = materialPropertyBlock.GetVector(nameID);
				objectTranslator.PushUnityEngineVector4(L, vector);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Vector4 vector2 = materialPropertyBlock.GetVector(name);
				objectTranslator.PushUnityEngineVector4(L, vector2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.GetVector!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Color color = materialPropertyBlock.GetColor(nameID);
				objectTranslator.PushUnityEngineColor(L, color);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Color color2 = materialPropertyBlock.GetColor(name);
				objectTranslator.PushUnityEngineColor(L, color2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.GetColor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Matrix4x4 matrix = materialPropertyBlock.GetMatrix(nameID);
				objectTranslator.Push(L, matrix);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Matrix4x4 matrix2 = materialPropertyBlock.GetMatrix(name);
				objectTranslator.Push(L, matrix2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.GetMatrix!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Texture texture = materialPropertyBlock.GetTexture(nameID);
				objectTranslator.Push(L, texture);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Texture texture2 = materialPropertyBlock.GetTexture(name);
				objectTranslator.Push(L, texture2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.GetTexture!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFloatArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				float[] floatArray = materialPropertyBlock.GetFloatArray(nameID);
				objectTranslator.Push(L, floatArray);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				float[] floatArray2 = materialPropertyBlock.GetFloatArray(name);
				objectTranslator.Push(L, floatArray2);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<float>>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				List<float> values = (List<float>)objectTranslator.GetObject(L, 3, typeof(List<float>));
				materialPropertyBlock.GetFloatArray(nameID2, values);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<float>>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				List<float> values2 = (List<float>)objectTranslator.GetObject(L, 3, typeof(List<float>));
				materialPropertyBlock.GetFloatArray(name2, values2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.GetFloatArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetVectorArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Vector4[] vectorArray = materialPropertyBlock.GetVectorArray(nameID);
				objectTranslator.Push(L, vectorArray);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Vector4[] vectorArray2 = materialPropertyBlock.GetVectorArray(name);
				objectTranslator.Push(L, vectorArray2);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Vector4>>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				List<Vector4> values = (List<Vector4>)objectTranslator.GetObject(L, 3, typeof(List<Vector4>));
				materialPropertyBlock.GetVectorArray(nameID2, values);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Vector4>>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				List<Vector4> values2 = (List<Vector4>)objectTranslator.GetObject(L, 3, typeof(List<Vector4>));
				materialPropertyBlock.GetVectorArray(name2, values2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.GetVectorArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMatrixArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameID = Lua.xlua_tointeger(L, 2);
				Matrix4x4[] matrixArray = materialPropertyBlock.GetMatrixArray(nameID);
				objectTranslator.Push(L, matrixArray);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				Matrix4x4[] matrixArray2 = materialPropertyBlock.GetMatrixArray(name);
				objectTranslator.Push(L, matrixArray2);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Matrix4x4>>(L, 3))
			{
				int nameID2 = Lua.xlua_tointeger(L, 2);
				List<Matrix4x4> values = (List<Matrix4x4>)objectTranslator.GetObject(L, 3, typeof(List<Matrix4x4>));
				materialPropertyBlock.GetMatrixArray(nameID2, values);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<Matrix4x4>>(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				List<Matrix4x4> values2 = (List<Matrix4x4>)objectTranslator.GetObject(L, 3, typeof(List<Matrix4x4>));
				materialPropertyBlock.GetMatrixArray(name2, values2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.GetMatrixArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CopySHCoefficientArraysFrom(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<List<SphericalHarmonicsL2>>(L, 2))
			{
				List<SphericalHarmonicsL2> lightProbes = (List<SphericalHarmonicsL2>)objectTranslator.GetObject(L, 2, typeof(List<SphericalHarmonicsL2>));
				materialPropertyBlock.CopySHCoefficientArraysFrom(lightProbes);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<SphericalHarmonicsL2[]>(L, 2))
			{
				SphericalHarmonicsL2[] lightProbes2 = (SphericalHarmonicsL2[])objectTranslator.GetObject(L, 2, typeof(SphericalHarmonicsL2[]));
				materialPropertyBlock.CopySHCoefficientArraysFrom(lightProbes2);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<List<SphericalHarmonicsL2>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				List<SphericalHarmonicsL2> lightProbes3 = (List<SphericalHarmonicsL2>)objectTranslator.GetObject(L, 2, typeof(List<SphericalHarmonicsL2>));
				int sourceStart = Lua.xlua_tointeger(L, 3);
				int destStart = Lua.xlua_tointeger(L, 4);
				int count = Lua.xlua_tointeger(L, 5);
				materialPropertyBlock.CopySHCoefficientArraysFrom(lightProbes3, sourceStart, destStart, count);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<SphericalHarmonicsL2[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				SphericalHarmonicsL2[] lightProbes4 = (SphericalHarmonicsL2[])objectTranslator.GetObject(L, 2, typeof(SphericalHarmonicsL2[]));
				int sourceStart2 = Lua.xlua_tointeger(L, 3);
				int destStart2 = Lua.xlua_tointeger(L, 4);
				int count2 = Lua.xlua_tointeger(L, 5);
				materialPropertyBlock.CopySHCoefficientArraysFrom(lightProbes4, sourceStart2, destStart2, count2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.CopySHCoefficientArraysFrom!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CopyProbeOcclusionArrayFrom(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<List<Vector4>>(L, 2))
			{
				List<Vector4> occlusionProbes = (List<Vector4>)objectTranslator.GetObject(L, 2, typeof(List<Vector4>));
				materialPropertyBlock.CopyProbeOcclusionArrayFrom(occlusionProbes);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector4[]>(L, 2))
			{
				Vector4[] occlusionProbes2 = (Vector4[])objectTranslator.GetObject(L, 2, typeof(Vector4[]));
				materialPropertyBlock.CopyProbeOcclusionArrayFrom(occlusionProbes2);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<List<Vector4>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				List<Vector4> occlusionProbes3 = (List<Vector4>)objectTranslator.GetObject(L, 2, typeof(List<Vector4>));
				int sourceStart = Lua.xlua_tointeger(L, 3);
				int destStart = Lua.xlua_tointeger(L, 4);
				int count = Lua.xlua_tointeger(L, 5);
				materialPropertyBlock.CopyProbeOcclusionArrayFrom(occlusionProbes3, sourceStart, destStart, count);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Vector4[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				Vector4[] occlusionProbes4 = (Vector4[])objectTranslator.GetObject(L, 2, typeof(Vector4[]));
				int sourceStart2 = Lua.xlua_tointeger(L, 3);
				int destStart2 = Lua.xlua_tointeger(L, 4);
				int count2 = Lua.xlua_tointeger(L, 5);
				materialPropertyBlock.CopyProbeOcclusionArrayFrom(occlusionProbes4, sourceStart2, destStart2, count2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MaterialPropertyBlock.CopyProbeOcclusionArrayFrom!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isEmpty(IntPtr L)
	{
		try
		{
			MaterialPropertyBlock materialPropertyBlock = (MaterialPropertyBlock)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, materialPropertyBlock.isEmpty);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
