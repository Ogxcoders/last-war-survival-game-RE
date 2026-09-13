using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SpineUnitySkeletonGraphicWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SkeletonGraphic);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 20, 30, 20);
		Utils.RegisterFunc(L, -3, "Rebuild", _m_Rebuild);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "LateUpdate", _m_LateUpdate);
		Utils.RegisterFunc(L, -3, "OnBecameVisible", _m_OnBecameVisible);
		Utils.RegisterFunc(L, -3, "OnBecameInvisible", _m_OnBecameInvisible);
		Utils.RegisterFunc(L, -3, "ReapplySeparatorSlotNames", _m_ReapplySeparatorSlotNames);
		Utils.RegisterFunc(L, -3, "GetLastMesh", _m_GetLastMesh);
		Utils.RegisterFunc(L, -3, "MatchRectTransformWithBounds", _m_MatchRectTransformWithBounds);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "TrimRenderers", _m_TrimRenderers);
		Utils.RegisterFunc(L, -3, "Initialize", _m_Initialize);
		Utils.RegisterFunc(L, -3, "UpdateMesh", _m_UpdateMesh);
		Utils.RegisterFunc(L, -3, "HasMultipleSubmeshInstructions", _m_HasMultipleSubmeshInstructions);
		Utils.RegisterFunc(L, -3, "OnRebuild", _e_OnRebuild);
		Utils.RegisterFunc(L, -3, "OnMeshAndMaterialsUpdated", _e_OnMeshAndMaterialsUpdated);
		Utils.RegisterFunc(L, -3, "BeforeApply", _e_BeforeApply);
		Utils.RegisterFunc(L, -3, "UpdateLocal", _e_UpdateLocal);
		Utils.RegisterFunc(L, -3, "UpdateWorld", _e_UpdateWorld);
		Utils.RegisterFunc(L, -3, "UpdateComplete", _e_UpdateComplete);
		Utils.RegisterFunc(L, -3, "OnPostProcessVertices", _e_OnPostProcessVertices);
		Utils.RegisterFunc(L, -2, "SkeletonDataAsset", _g_get_SkeletonDataAsset);
		Utils.RegisterFunc(L, -2, "UpdateMode", _g_get_UpdateMode);
		Utils.RegisterFunc(L, -2, "SeparatorParts", _g_get_SeparatorParts);
		Utils.RegisterFunc(L, -2, "CustomTextureOverride", _g_get_CustomTextureOverride);
		Utils.RegisterFunc(L, -2, "CustomMaterialOverride", _g_get_CustomMaterialOverride);
		Utils.RegisterFunc(L, -2, "OverrideTexture", _g_get_OverrideTexture);
		Utils.RegisterFunc(L, -2, "mainTexture", _g_get_mainTexture);
		Utils.RegisterFunc(L, -2, "Skeleton", _g_get_Skeleton);
		Utils.RegisterFunc(L, -2, "SkeletonData", _g_get_SkeletonData);
		Utils.RegisterFunc(L, -2, "IsValid", _g_get_IsValid);
		Utils.RegisterFunc(L, -2, "AnimationState", _g_get_AnimationState);
		Utils.RegisterFunc(L, -2, "MeshGenerator", _g_get_MeshGenerator);
		Utils.RegisterFunc(L, -2, "skeletonDataAsset", _g_get_skeletonDataAsset);
		Utils.RegisterFunc(L, -2, "additiveMaterial", _g_get_additiveMaterial);
		Utils.RegisterFunc(L, -2, "multiplyMaterial", _g_get_multiplyMaterial);
		Utils.RegisterFunc(L, -2, "screenMaterial", _g_get_screenMaterial);
		Utils.RegisterFunc(L, -2, "initialSkinName", _g_get_initialSkinName);
		Utils.RegisterFunc(L, -2, "initialFlipX", _g_get_initialFlipX);
		Utils.RegisterFunc(L, -2, "initialFlipY", _g_get_initialFlipY);
		Utils.RegisterFunc(L, -2, "startingAnimation", _g_get_startingAnimation);
		Utils.RegisterFunc(L, -2, "startingLoop", _g_get_startingLoop);
		Utils.RegisterFunc(L, -2, "timeScale", _g_get_timeScale);
		Utils.RegisterFunc(L, -2, "freeze", _g_get_freeze);
		Utils.RegisterFunc(L, -2, "updateWhenInvisible", _g_get_updateWhenInvisible);
		Utils.RegisterFunc(L, -2, "unscaledTime", _g_get_unscaledTime);
		Utils.RegisterFunc(L, -2, "allowMultipleCanvasRenderers", _g_get_allowMultipleCanvasRenderers);
		Utils.RegisterFunc(L, -2, "canvasRenderers", _g_get_canvasRenderers);
		Utils.RegisterFunc(L, -2, "separatorSlots", _g_get_separatorSlots);
		Utils.RegisterFunc(L, -2, "enableSeparatorSlots", _g_get_enableSeparatorSlots);
		Utils.RegisterFunc(L, -2, "updateSeparatorPartLocation", _g_get_updateSeparatorPartLocation);
		Utils.RegisterFunc(L, -1, "UpdateMode", _s_set_UpdateMode);
		Utils.RegisterFunc(L, -1, "OverrideTexture", _s_set_OverrideTexture);
		Utils.RegisterFunc(L, -1, "Skeleton", _s_set_Skeleton);
		Utils.RegisterFunc(L, -1, "skeletonDataAsset", _s_set_skeletonDataAsset);
		Utils.RegisterFunc(L, -1, "additiveMaterial", _s_set_additiveMaterial);
		Utils.RegisterFunc(L, -1, "multiplyMaterial", _s_set_multiplyMaterial);
		Utils.RegisterFunc(L, -1, "screenMaterial", _s_set_screenMaterial);
		Utils.RegisterFunc(L, -1, "initialSkinName", _s_set_initialSkinName);
		Utils.RegisterFunc(L, -1, "initialFlipX", _s_set_initialFlipX);
		Utils.RegisterFunc(L, -1, "initialFlipY", _s_set_initialFlipY);
		Utils.RegisterFunc(L, -1, "startingAnimation", _s_set_startingAnimation);
		Utils.RegisterFunc(L, -1, "startingLoop", _s_set_startingLoop);
		Utils.RegisterFunc(L, -1, "timeScale", _s_set_timeScale);
		Utils.RegisterFunc(L, -1, "freeze", _s_set_freeze);
		Utils.RegisterFunc(L, -1, "updateWhenInvisible", _s_set_updateWhenInvisible);
		Utils.RegisterFunc(L, -1, "unscaledTime", _s_set_unscaledTime);
		Utils.RegisterFunc(L, -1, "allowMultipleCanvasRenderers", _s_set_allowMultipleCanvasRenderers);
		Utils.RegisterFunc(L, -1, "canvasRenderers", _s_set_canvasRenderers);
		Utils.RegisterFunc(L, -1, "enableSeparatorSlots", _s_set_enableSeparatorSlots);
		Utils.RegisterFunc(L, -1, "updateSeparatorPartLocation", _s_set_updateSeparatorPartLocation);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 0, 0);
		Utils.RegisterFunc(L, -4, "NewSkeletonGraphicGameObject", _m_NewSkeletonGraphicGameObject_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddSkeletonGraphicComponent", _m_AddSkeletonGraphicComponent_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "SeparatorPartGameObjectName", "Part");
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
				SkeletonGraphic o = new SkeletonGraphic();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonGraphic constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NewSkeletonGraphicGameObject_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonDataAsset skeletonDataAsset = (SkeletonDataAsset)objectTranslator.GetObject(L, 1, typeof(SkeletonDataAsset));
			Transform parent = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
			Material material = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
			SkeletonGraphic o = SkeletonGraphic.NewSkeletonGraphicGameObject(skeletonDataAsset, parent, material);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddSkeletonGraphicComponent_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.GetObject(L, 1, typeof(GameObject));
			SkeletonDataAsset skeletonDataAsset = (SkeletonDataAsset)objectTranslator.GetObject(L, 2, typeof(SkeletonDataAsset));
			Material material = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
			SkeletonGraphic o = SkeletonGraphic.AddSkeletonGraphicComponent(gameObject, skeletonDataAsset, material);
			objectTranslator.Push(L, o);
			return 1;
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
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CanvasUpdate v);
			skeletonGraphic.Rebuild(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				skeletonGraphic.Update();
				return 0;
			case 2:
				if (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
				{
					float deltaTime = (float)Lua.lua_tonumber(L, 2);
					skeletonGraphic.Update(deltaTime);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonGraphic.Update!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LateUpdate(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LateUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBecameVisible(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnBecameVisible();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBecameInvisible(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnBecameInvisible();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReapplySeparatorSlotNames(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ReapplySeparatorSlotNames();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLastMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh lastMesh = ((SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1)).GetLastMesh();
			objectTranslator.Push(L, lastMesh);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MatchRectTransformWithBounds(IntPtr L)
	{
		try
		{
			bool value = ((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).MatchRectTransformWithBounds();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TrimRenderers(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TrimRenderers();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Initialize(IntPtr L)
	{
		try
		{
			SkeletonGraphic obj = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool overwrite = Lua.lua_toboolean(L, 2);
			obj.Initialize(overwrite);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateMesh(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool keepRendererCount = Lua.lua_toboolean(L, 2);
				skeletonGraphic.UpdateMesh(keepRendererCount);
				return 0;
			}
			if (num == 1)
			{
				skeletonGraphic.UpdateMesh();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonGraphic.UpdateMesh!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasMultipleSubmeshInstructions(IntPtr L)
	{
		try
		{
			bool value = ((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HasMultipleSubmeshInstructions();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SkeletonDataAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.SkeletonDataAsset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UpdateMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.UpdateMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SeparatorParts(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.SeparatorParts);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CustomTextureOverride(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.CustomTextureOverride);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CustomMaterialOverride(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.CustomMaterialOverride);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OverrideTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.OverrideTexture);
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
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.mainTexture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Skeleton(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.Skeleton);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SkeletonData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.SkeletonData);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsValid(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skeletonGraphic.IsValid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AnimationState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.AnimationState);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MeshGenerator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.MeshGenerator);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_skeletonDataAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.skeletonDataAsset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_additiveMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.additiveMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_multiplyMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.multiplyMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_screenMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.screenMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_initialSkinName(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, skeletonGraphic.initialSkinName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_initialFlipX(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skeletonGraphic.initialFlipX);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_initialFlipY(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skeletonGraphic.initialFlipY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startingAnimation(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, skeletonGraphic.startingAnimation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startingLoop(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skeletonGraphic.startingLoop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_timeScale(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, skeletonGraphic.timeScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_freeze(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skeletonGraphic.freeze);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_updateWhenInvisible(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.updateWhenInvisible);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_unscaledTime(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skeletonGraphic.unscaledTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allowMultipleCanvasRenderers(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skeletonGraphic.allowMultipleCanvasRenderers);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_canvasRenderers(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.canvasRenderers);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_separatorSlots(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonGraphic.separatorSlots);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enableSeparatorSlots(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skeletonGraphic.enableSeparatorSlots);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_updateSeparatorPartLocation(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skeletonGraphic.updateSeparatorPartLocation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_UpdateMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out UpdateMode v);
			skeletonGraphic.UpdateMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OverrideTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1)).OverrideTexture = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Skeleton(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1)).Skeleton = (Skeleton)objectTranslator.GetObject(L, 2, typeof(Skeleton));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_skeletonDataAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1)).skeletonDataAsset = (SkeletonDataAsset)objectTranslator.GetObject(L, 2, typeof(SkeletonDataAsset));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_additiveMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1)).additiveMaterial = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_multiplyMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1)).multiplyMaterial = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_screenMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1)).screenMaterial = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_initialSkinName(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).initialSkinName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_initialFlipX(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).initialFlipX = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_initialFlipY(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).initialFlipY = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startingAnimation(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).startingAnimation = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startingLoop(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).startingLoop = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_timeScale(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).timeScale = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_freeze(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).freeze = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_updateWhenInvisible(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out UpdateMode v);
			skeletonGraphic.updateWhenInvisible = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_unscaledTime(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).unscaledTime = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allowMultipleCanvasRenderers(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allowMultipleCanvasRenderers = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_canvasRenderers(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1)).canvasRenderers = (List<CanvasRenderer>)objectTranslator.GetObject(L, 2, typeof(List<CanvasRenderer>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enableSeparatorSlots(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enableSeparatorSlots = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_updateSeparatorPartLocation(IntPtr L)
	{
		try
		{
			((SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).updateSeparatorPartLocation = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnRebuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			SkeletonGraphic.SkeletonRendererDelegate skeletonRendererDelegate = objectTranslator.GetDelegate<SkeletonGraphic.SkeletonRendererDelegate>(L, 3);
			if (skeletonRendererDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need Spine.Unity.SkeletonGraphic.SkeletonRendererDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					skeletonGraphic.OnRebuild += skeletonRendererDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					skeletonGraphic.OnRebuild -= skeletonRendererDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonGraphic.OnRebuild!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnMeshAndMaterialsUpdated(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			SkeletonGraphic.SkeletonRendererDelegate skeletonRendererDelegate = objectTranslator.GetDelegate<SkeletonGraphic.SkeletonRendererDelegate>(L, 3);
			if (skeletonRendererDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need Spine.Unity.SkeletonGraphic.SkeletonRendererDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					skeletonGraphic.OnMeshAndMaterialsUpdated += skeletonRendererDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					skeletonGraphic.OnMeshAndMaterialsUpdated -= skeletonRendererDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonGraphic.OnMeshAndMaterialsUpdated!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_BeforeApply(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			UpdateBonesDelegate updateBonesDelegate = objectTranslator.GetDelegate<UpdateBonesDelegate>(L, 3);
			if (updateBonesDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need Spine.Unity.UpdateBonesDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					skeletonGraphic.BeforeApply += updateBonesDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					skeletonGraphic.BeforeApply -= updateBonesDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonGraphic.BeforeApply!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_UpdateLocal(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			UpdateBonesDelegate updateBonesDelegate = objectTranslator.GetDelegate<UpdateBonesDelegate>(L, 3);
			if (updateBonesDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need Spine.Unity.UpdateBonesDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					skeletonGraphic.UpdateLocal += updateBonesDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					skeletonGraphic.UpdateLocal -= updateBonesDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonGraphic.UpdateLocal!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_UpdateWorld(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			UpdateBonesDelegate updateBonesDelegate = objectTranslator.GetDelegate<UpdateBonesDelegate>(L, 3);
			if (updateBonesDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need Spine.Unity.UpdateBonesDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					skeletonGraphic.UpdateWorld += updateBonesDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					skeletonGraphic.UpdateWorld -= updateBonesDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonGraphic.UpdateWorld!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_UpdateComplete(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			UpdateBonesDelegate updateBonesDelegate = objectTranslator.GetDelegate<UpdateBonesDelegate>(L, 3);
			if (updateBonesDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need Spine.Unity.UpdateBonesDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					skeletonGraphic.UpdateComplete += updateBonesDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					skeletonGraphic.UpdateComplete -= updateBonesDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonGraphic.UpdateComplete!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnPostProcessVertices(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)objectTranslator.FastGetCSObj(L, 1);
			MeshGeneratorDelegate meshGeneratorDelegate = objectTranslator.GetDelegate<MeshGeneratorDelegate>(L, 3);
			if (meshGeneratorDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need Spine.Unity.MeshGeneratorDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					skeletonGraphic.OnPostProcessVertices += meshGeneratorDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					skeletonGraphic.OnPostProcessVertices -= meshGeneratorDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonGraphic.OnPostProcessVertices!");
		return 0;
	}
}
