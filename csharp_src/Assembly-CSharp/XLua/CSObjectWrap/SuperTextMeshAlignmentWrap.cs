using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SuperTextMeshAlignmentWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(SuperTextMesh.Alignment), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(SuperTextMesh.Alignment), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(SuperTextMesh.Alignment), L, null, 10, 0, 0);
		Utils.RegisterObject(L, translator, -4, "TopLeft", SuperTextMesh.Alignment.TopLeft);
		Utils.RegisterObject(L, translator, -4, "TopCenter", SuperTextMesh.Alignment.TopCenter);
		Utils.RegisterObject(L, translator, -4, "TopRight", SuperTextMesh.Alignment.TopRight);
		Utils.RegisterObject(L, translator, -4, "MidLeft", SuperTextMesh.Alignment.MidLeft);
		Utils.RegisterObject(L, translator, -4, "MidCenter", SuperTextMesh.Alignment.MidCenter);
		Utils.RegisterObject(L, translator, -4, "MidRight", SuperTextMesh.Alignment.MidRight);
		Utils.RegisterObject(L, translator, -4, "BotLeft", SuperTextMesh.Alignment.BotLeft);
		Utils.RegisterObject(L, translator, -4, "BotCenter", SuperTextMesh.Alignment.BotCenter);
		Utils.RegisterObject(L, translator, -4, "BotRight", SuperTextMesh.Alignment.BotRight);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(SuperTextMesh.Alignment), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushSuperTextMeshAlignment(L, (SuperTextMesh.Alignment)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "TopLeft"))
			{
				objectTranslator.PushSuperTextMeshAlignment(L, SuperTextMesh.Alignment.TopLeft);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "TopCenter"))
			{
				objectTranslator.PushSuperTextMeshAlignment(L, SuperTextMesh.Alignment.TopCenter);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "TopRight"))
			{
				objectTranslator.PushSuperTextMeshAlignment(L, SuperTextMesh.Alignment.TopRight);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MidLeft"))
			{
				objectTranslator.PushSuperTextMeshAlignment(L, SuperTextMesh.Alignment.MidLeft);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MidCenter"))
			{
				objectTranslator.PushSuperTextMeshAlignment(L, SuperTextMesh.Alignment.MidCenter);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MidRight"))
			{
				objectTranslator.PushSuperTextMeshAlignment(L, SuperTextMesh.Alignment.MidRight);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "BotLeft"))
			{
				objectTranslator.PushSuperTextMeshAlignment(L, SuperTextMesh.Alignment.BotLeft);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "BotCenter"))
			{
				objectTranslator.PushSuperTextMeshAlignment(L, SuperTextMesh.Alignment.BotCenter);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "BotRight"))
			{
				objectTranslator.PushSuperTextMeshAlignment(L, SuperTextMesh.Alignment.BotRight);
				break;
			}
			return Lua.luaL_error(L, "invalid string for SuperTextMesh.Alignment!");
		default:
			return Lua.luaL_error(L, "invalid lua type for SuperTextMesh.Alignment! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
