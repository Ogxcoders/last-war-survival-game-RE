using System;
using DG.Tweening;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DGTweeningDOTweenAnimationTargetTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(DOTweenAnimation.TargetType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(DOTweenAnimation.TargetType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(DOTweenAnimation.TargetType), L, null, 17, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Unset", DOTweenAnimation.TargetType.Unset);
		Utils.RegisterObject(L, translator, -4, "Camera", DOTweenAnimation.TargetType.Camera);
		Utils.RegisterObject(L, translator, -4, "CanvasGroup", DOTweenAnimation.TargetType.CanvasGroup);
		Utils.RegisterObject(L, translator, -4, "Image", DOTweenAnimation.TargetType.Image);
		Utils.RegisterObject(L, translator, -4, "Light", DOTweenAnimation.TargetType.Light);
		Utils.RegisterObject(L, translator, -4, "RectTransform", DOTweenAnimation.TargetType.RectTransform);
		Utils.RegisterObject(L, translator, -4, "Renderer", DOTweenAnimation.TargetType.Renderer);
		Utils.RegisterObject(L, translator, -4, "SpriteRenderer", DOTweenAnimation.TargetType.SpriteRenderer);
		Utils.RegisterObject(L, translator, -4, "Rigidbody", DOTweenAnimation.TargetType.Rigidbody);
		Utils.RegisterObject(L, translator, -4, "Rigidbody2D", DOTweenAnimation.TargetType.Rigidbody2D);
		Utils.RegisterObject(L, translator, -4, "Text", DOTweenAnimation.TargetType.Text);
		Utils.RegisterObject(L, translator, -4, "Transform", DOTweenAnimation.TargetType.Transform);
		Utils.RegisterObject(L, translator, -4, "tk2dBaseSprite", DOTweenAnimation.TargetType.tk2dBaseSprite);
		Utils.RegisterObject(L, translator, -4, "tk2dTextMesh", DOTweenAnimation.TargetType.tk2dTextMesh);
		Utils.RegisterObject(L, translator, -4, "TextMeshPro", DOTweenAnimation.TargetType.TextMeshPro);
		Utils.RegisterObject(L, translator, -4, "TextMeshProUGUI", DOTweenAnimation.TargetType.TextMeshProUGUI);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(DOTweenAnimation.TargetType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, (DOTweenAnimation.TargetType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Unset"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.Unset);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Camera"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.Camera);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "CanvasGroup"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.CanvasGroup);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Image"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.Image);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Light"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.Light);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "RectTransform"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.RectTransform);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Renderer"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.Renderer);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "SpriteRenderer"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.SpriteRenderer);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Rigidbody"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.Rigidbody);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Rigidbody2D"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.Rigidbody2D);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Text"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.Text);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Transform"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.Transform);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "tk2dBaseSprite"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.tk2dBaseSprite);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "tk2dTextMesh"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.tk2dTextMesh);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "TextMeshPro"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.TextMeshPro);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "TextMeshProUGUI"))
			{
				objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, DOTweenAnimation.TargetType.TextMeshProUGUI);
				break;
			}
			return Lua.luaL_error(L, "invalid string for DG.Tweening.DOTweenAnimation.TargetType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for DG.Tweening.DOTweenAnimation.TargetType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
