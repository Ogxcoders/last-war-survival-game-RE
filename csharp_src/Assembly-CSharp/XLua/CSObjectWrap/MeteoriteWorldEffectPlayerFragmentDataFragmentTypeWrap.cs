using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MeteoriteWorldEffectPlayerFragmentDataFragmentTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(MeteoriteWorldEffectPlayer.FragmentData.FragmentType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(MeteoriteWorldEffectPlayer.FragmentData.FragmentType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(MeteoriteWorldEffectPlayer.FragmentData.FragmentType), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Small", MeteoriteWorldEffectPlayer.FragmentData.FragmentType.Small);
		Utils.RegisterObject(L, translator, -4, "Middle", MeteoriteWorldEffectPlayer.FragmentData.FragmentType.Middle);
		Utils.RegisterObject(L, translator, -4, "Daddy", MeteoriteWorldEffectPlayer.FragmentData.FragmentType.Daddy);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(MeteoriteWorldEffectPlayer.FragmentData.FragmentType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushMeteoriteWorldEffectPlayerFragmentDataFragmentType(L, (MeteoriteWorldEffectPlayer.FragmentData.FragmentType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Small"))
			{
				objectTranslator.PushMeteoriteWorldEffectPlayerFragmentDataFragmentType(L, MeteoriteWorldEffectPlayer.FragmentData.FragmentType.Small);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Middle"))
			{
				objectTranslator.PushMeteoriteWorldEffectPlayerFragmentDataFragmentType(L, MeteoriteWorldEffectPlayer.FragmentData.FragmentType.Middle);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Daddy"))
			{
				objectTranslator.PushMeteoriteWorldEffectPlayerFragmentDataFragmentType(L, MeteoriteWorldEffectPlayer.FragmentData.FragmentType.Daddy);
				break;
			}
			return Lua.luaL_error(L, "invalid string for MeteoriteWorldEffectPlayer.FragmentData.FragmentType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for MeteoriteWorldEffectPlayer.FragmentData.FragmentType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
