using System;
using PVEBattleLogic.Effect;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PVEBattleLogicEffectEffectViewFacadeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(EffectViewFacade);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 32, 0, 0);
		Utils.RegisterFunc(L, -4, "Init", _m_Init_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitEffectViewArrayAccess", _m_InitEffectViewArrayAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnInitEffectViewArrayAccess", _m_UnInitEffectViewArrayAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "SyncNameId", _m_SyncNameId_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowEffect", _m_ShowEffect_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowEffectLuaArray", _m_ShowEffectLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowEffectParentLuaArray", _m_ShowEffectParentLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowEffectZeroPos", _m_ShowEffectZeroPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowEffectZeroPosLuaAccess", _m_ShowEffectZeroPosLuaAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowEffectZeroPosParentLuaAccess", _m_ShowEffectZeroPosParentLuaAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowEffectZeroRot", _m_ShowEffectZeroRot_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowEffectZeroRotLuaAccess", _m_ShowEffectZeroRotLuaAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowEffectZeroRotParentLuaAccess", _m_ShowEffectZeroRotParentLuaAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowEffectOnlyParent", _m_ShowEffectOnlyParent_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowEffectDefaultLuaAccess", _m_ShowEffectDefaultLuaAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowEffectDefaultParentLuaAccess", _m_ShowEffectDefaultParentLuaAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowHitEffectForViewTargetZeroRot", _m_ShowHitEffectForViewTargetZeroRot_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowHitEffectForViewTarget", _m_ShowHitEffectForViewTarget_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowEffectParents", _m_ShowEffectParents_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetPosition", _m_ResetPosition_xlua_st_);
		Utils.RegisterFunc(L, -4, "RemoveEffect", _m_RemoveEffect_xlua_st_);
		Utils.RegisterFunc(L, -4, "Update", _m_Update_xlua_st_);
		Utils.RegisterFunc(L, -4, "PreloadEffectGameObject", _m_PreloadEffectGameObject_xlua_st_);
		Utils.RegisterFunc(L, -4, "PreloadEffectGameObjectWhitId", _m_PreloadEffectGameObjectWhitId_xlua_st_);
		Utils.RegisterFunc(L, -4, "PreloadEffectSpriteObject", _m_PreloadEffectSpriteObject_xlua_st_);
		Utils.RegisterFunc(L, -4, "RemoveAllHitEffectByParentViewHandle", _m_RemoveAllHitEffectByParentViewHandle_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearAll", _m_ClearAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "Dispose", _m_Dispose_xlua_st_);
		Utils.RegisterFunc(L, -4, "TestLuaCSharpArray", _m_TestLuaCSharpArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "TestLuaTable", _m_TestLuaTable_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "INVALID_HANDLE", -1);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "PVEBattleLogic.Effect.EffectViewFacade does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init_xlua_st_(IntPtr L)
	{
		try
		{
			EffectViewFacade.Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitEffectViewArrayAccess_xlua_st_(IntPtr L)
	{
		try
		{
			EffectViewFacade.InitEffectViewArrayAccess((LuaArrAccess)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(LuaArrAccess)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInitEffectViewArrayAccess_xlua_st_(IntPtr L)
	{
		try
		{
			EffectViewFacade.UnInitEffectViewArrayAccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncNameId_xlua_st_(IntPtr L)
	{
		try
		{
			string viewName = Lua.lua_tostring(L, 1);
			int viewId = Lua.xlua_tointeger(L, 2);
			EffectViewFacade.SyncNameId(viewName, viewId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowEffect_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 7 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && objectTranslator.Assignable<Quaternion>(L, 5) && objectTranslator.Assignable<Transform>(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				string path = Lua.lua_tostring(L, 1);
				float time = (float)Lua.lua_tonumber(L, 2);
				int type = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val);
				objectTranslator.Get(L, 5, out Quaternion val2);
				long n = EffectViewFacade.ShowEffect(parent: (Transform)objectTranslator.GetObject(L, 6, typeof(Transform)), scale: (float)Lua.lua_tonumber(L, 7), path: path, time: time, type: type, position: val, rotation: val2);
				Lua.lua_pushint64(L, n);
				return 1;
			}
			if (num == 6 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && objectTranslator.Assignable<Quaternion>(L, 5) && objectTranslator.Assignable<Transform>(L, 6))
			{
				string path2 = Lua.lua_tostring(L, 1);
				float time2 = (float)Lua.lua_tonumber(L, 2);
				int type2 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val3);
				objectTranslator.Get(L, 5, out Quaternion val4);
				long n2 = EffectViewFacade.ShowEffect(parent: (Transform)objectTranslator.GetObject(L, 6, typeof(Transform)), path: path2, time: time2, type: type2, position: val3, rotation: val4);
				Lua.lua_pushint64(L, n2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PVEBattleLogic.Effect.EffectViewFacade.ShowEffect!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowEffectLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			long n = EffectViewFacade.ShowEffectLuaArray();
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowEffectParentLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			long n = EffectViewFacade.ShowEffectParentLuaArray((Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform)));
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowEffectZeroPos_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string path = Lua.lua_tostring(L, 1);
			float time = (float)Lua.lua_tonumber(L, 2);
			int type = Lua.xlua_tointeger(L, 3);
			objectTranslator.Get(L, 4, out Quaternion val);
			Transform parent = (Transform)objectTranslator.GetObject(L, 5, typeof(Transform));
			long n = EffectViewFacade.ShowEffectZeroPos(path, time, type, val, parent);
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowEffectZeroPosLuaAccess_xlua_st_(IntPtr L)
	{
		try
		{
			long n = EffectViewFacade.ShowEffectZeroPosLuaAccess();
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowEffectZeroPosParentLuaAccess_xlua_st_(IntPtr L)
	{
		try
		{
			long n = EffectViewFacade.ShowEffectZeroPosParentLuaAccess((Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform)));
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowEffectZeroRot_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string path = Lua.lua_tostring(L, 1);
			float time = (float)Lua.lua_tonumber(L, 2);
			int type = Lua.xlua_tointeger(L, 3);
			objectTranslator.Get(L, 4, out Vector3 val);
			Transform parent = (Transform)objectTranslator.GetObject(L, 5, typeof(Transform));
			long n = EffectViewFacade.ShowEffectZeroRot(path, time, type, val, parent);
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowEffectZeroRotLuaAccess_xlua_st_(IntPtr L)
	{
		try
		{
			long n = EffectViewFacade.ShowEffectZeroRotLuaAccess();
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowEffectZeroRotParentLuaAccess_xlua_st_(IntPtr L)
	{
		try
		{
			long n = EffectViewFacade.ShowEffectZeroRotParentLuaAccess((Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform)));
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowEffectOnlyParent_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string path = Lua.lua_tostring(L, 1);
			float time = (float)Lua.lua_tonumber(L, 2);
			int type = Lua.xlua_tointeger(L, 3);
			Transform parent = (Transform)objectTranslator.GetObject(L, 4, typeof(Transform));
			long n = EffectViewFacade.ShowEffectOnlyParent(path, time, type, parent);
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowEffectDefaultLuaAccess_xlua_st_(IntPtr L)
	{
		try
		{
			long n = EffectViewFacade.ShowEffectDefaultLuaAccess();
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowEffectDefaultParentLuaAccess_xlua_st_(IntPtr L)
	{
		try
		{
			long n = EffectViewFacade.ShowEffectDefaultParentLuaAccess((Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform)));
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowHitEffectForViewTargetZeroRot_xlua_st_(IntPtr L)
	{
		try
		{
			EffectViewFacade.ShowHitEffectForViewTargetZeroRot();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowHitEffectForViewTarget_xlua_st_(IntPtr L)
	{
		try
		{
			EffectViewFacade.ShowHitEffectForViewTarget();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowEffectParents_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) || Lua.lua_isint64(L, 1)) && objectTranslator.Assignable<Transform>(L, 2))
			{
				long objId = Lua.lua_toint64(L, 1);
				Transform parent = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				EffectViewFacade.ShowEffectParents(objId, parent);
				return 0;
			}
			if (num == 1 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) || Lua.lua_isint64(L, 1)))
			{
				EffectViewFacade.ShowEffectParents(Lua.lua_toint64(L, 1));
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PVEBattleLogic.Effect.EffectViewFacade.ShowEffectParents!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetPosition_xlua_st_(IntPtr L)
	{
		try
		{
			long objId = Lua.lua_toint64(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			EffectViewFacade.ResetPosition(objId, x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveEffect_xlua_st_(IntPtr L)
	{
		try
		{
			EffectViewFacade.RemoveEffect(Lua.lua_toint64(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update_xlua_st_(IntPtr L)
	{
		try
		{
			EffectViewFacade.Update((float)Lua.lua_tonumber(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PreloadEffectGameObject_xlua_st_(IntPtr L)
	{
		try
		{
			string path = Lua.lua_tostring(L, 1);
			int count = Lua.xlua_tointeger(L, 2);
			EffectViewFacade.PreloadEffectGameObject(path, count);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PreloadEffectGameObjectWhitId_xlua_st_(IntPtr L)
	{
		try
		{
			int viewId = Lua.xlua_tointeger(L, 1);
			int count = Lua.xlua_tointeger(L, 2);
			EffectViewFacade.PreloadEffectGameObjectWhitId(viewId, count);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PreloadEffectSpriteObject_xlua_st_(IntPtr L)
	{
		try
		{
			string path = Lua.lua_tostring(L, 1);
			int count = Lua.xlua_tointeger(L, 2);
			EffectViewFacade.PreloadEffectSpriteObject(path, count);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveAllHitEffectByParentViewHandle_xlua_st_(IntPtr L)
	{
		try
		{
			EffectViewFacade.RemoveAllHitEffectByParentViewHandle(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAll_xlua_st_(IntPtr L)
	{
		try
		{
			EffectViewFacade.ClearAll();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dispose_xlua_st_(IntPtr L)
	{
		try
		{
			EffectViewFacade.Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TestLuaCSharpArray_xlua_st_(IntPtr L)
	{
		try
		{
			EffectViewFacade.TestLuaCSharpArray((LuaArrAccess)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(LuaArrAccess)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TestLuaTable_xlua_st_(IntPtr L)
	{
		try
		{
			EffectViewFacade.TestLuaTable((LuaTable)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(LuaTable)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
