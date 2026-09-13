using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesQualitySettingWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.QualitySetting);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 11, 0, 0);
		Utils.RegisterObject(L, translator, -4, "PostProcess_Bloom", "QualitySetting.PostProcess.Bloom");
		Utils.RegisterObject(L, translator, -4, "PostProcess_ColorAdjustments", "QualitySetting.PostProcess.ColorAdjustments");
		Utils.RegisterObject(L, translator, -4, "PostProcess_Vignette", "QualitySetting.PostProcess.Vignette");
		Utils.RegisterObject(L, translator, -4, "PostProcess_Tonemapping", "QualitySetting.PostProcess.Tonemapping");
		Utils.RegisterObject(L, translator, -4, "PostProcess_LiftGammaGain", "QualitySetting.PostProcess.LiftGammaGain");
		Utils.RegisterObject(L, translator, -4, "PostProcess_DepthOfField", "QualitySetting.PostProcess.DepthOfField");
		Utils.RegisterObject(L, translator, -4, "Resolution", "QualitySetting.Resolution");
		Utils.RegisterObject(L, translator, -4, "FPS", "QualitySetting.FPS");
		Utils.RegisterObject(L, translator, -4, "Terrain", "QualitySetting.Terrain");
		Utils.RegisterObject(L, translator, -4, "ShaderLOD", "QualitySetting.ShaderLOD");
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "GameDefines.QualitySetting does not have a constructor!");
	}
}
