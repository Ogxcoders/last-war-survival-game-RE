using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UIUtilsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIUtils);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 32, 3, 3);
		Utils.RegisterFunc(L, -4, "GetSpecialScreenWidth", _m_GetSpecialScreenWidth_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetDescentHeightFromScreenSafeArea", _m_GetDescentHeightFromScreenSafeArea_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsPhone", _m_IsPhone_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddChildManually", _m_AddChildManually_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckGuiRaycastObjects", _m_CheckGuiRaycastObjects_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGuiRaycastObjects", _m_GetGuiRaycastObjects_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowMessage", _m_ShowMessage_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowMessages", _m_ShowMessages_xlua_st_);
		Utils.RegisterFunc(L, -4, "NewShowMessage", _m_NewShowMessage_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowMessage3Action", _m_ShowMessage3Action_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowReloadMessage", _m_ShowReloadMessage_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowLoadingMask", _m_ShowLoadingMask_xlua_st_);
		Utils.RegisterFunc(L, -4, "HideLoadingMask", _m_HideLoadingMask_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowTips", _m_ShowTips_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowLackResource", _m_ShowLackResource_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowRankView", _m_ShowRankView_xlua_st_);
		Utils.RegisterFunc(L, -4, "WordToScenePoint", _m_WordToScenePoint_xlua_st_);
		Utils.RegisterFunc(L, -4, "ScreenPointToLocalPointInRectangle", _m_ScreenPointToLocalPointInRectangle_xlua_st_);
		Utils.RegisterFunc(L, -4, "BuildSkeletonDataAsset", _m_BuildSkeletonDataAsset_xlua_st_);
		Utils.RegisterFunc(L, -4, "checkUserName", _m_checkUserName_xlua_st_);
		Utils.RegisterFunc(L, -4, "checkPwd", _m_checkPwd_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetFirstChild", _m_GetFirstChild_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetTextWithImage", _m_SetTextWithImage_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPosAtText", _m_GetPosAtText_xlua_st_);
		Utils.RegisterFunc(L, -4, "PlayAnimationReturnTime", _m_PlayAnimationReturnTime_xlua_st_);
		Utils.RegisterFunc(L, -4, "PlayCrossFadeAnimationReturnTime", _m_PlayCrossFadeAnimationReturnTime_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetCurScreenMaxRadiusSize", _m_GetCurScreenMaxRadiusSize_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPointByMeteoriteHitPlane", _m_GetPointByMeteoriteHitPlane_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetBuildPointByMeteoriteHitGlass", _m_GetBuildPointByMeteoriteHitGlass_xlua_st_);
		Utils.RegisterFunc(L, -4, "FormatServerAllianceName", _m_FormatServerAllianceName_xlua_st_);
		Utils.RegisterFunc(L, -4, "FormatAllianceAndName", _m_FormatAllianceAndName_xlua_st_);
		Utils.RegisterFunc(L, -2, "tmpWidth", _g_get_tmpWidth);
		Utils.RegisterFunc(L, -2, "leftTopScreenPoint", _g_get_leftTopScreenPoint);
		Utils.RegisterFunc(L, -2, "leftTopRectPoint", _g_get_leftTopRectPoint);
		Utils.RegisterFunc(L, -1, "tmpWidth", _s_set_tmpWidth);
		Utils.RegisterFunc(L, -1, "leftTopScreenPoint", _s_set_leftTopScreenPoint);
		Utils.RegisterFunc(L, -1, "leftTopRectPoint", _s_set_leftTopRectPoint);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UIUtils does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSpecialScreenWidth_xlua_st_(IntPtr L)
	{
		try
		{
			float specialScreenWidth = UIUtils.GetSpecialScreenWidth();
			Lua.lua_pushnumber(L, specialScreenWidth);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDescentHeightFromScreenSafeArea_xlua_st_(IntPtr L)
	{
		try
		{
			float descentHeightFromScreenSafeArea = UIUtils.GetDescentHeightFromScreenSafeArea((RectTransform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(RectTransform)));
			Lua.lua_pushnumber(L, descentHeightFromScreenSafeArea);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPhone_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = UIUtils.IsPhone();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddChildManually_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform parent = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
			Transform child = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
			UIUtils.AddChildManually(parent, child);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckGuiRaycastObjects_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			switch (Lua.lua_gettop(L))
			{
			case 0:
			{
				bool value2 = UIUtils.CheckGuiRaycastObjects();
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			case 1:
				if (objectTranslator.Assignable<Vector2>(L, 1))
				{
					objectTranslator.Get(L, 1, out Vector2 val);
					bool value = UIUtils.CheckGuiRaycastObjects(val);
					Lua.lua_pushboolean(L, value);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIUtils.CheckGuiRaycastObjects!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGuiRaycastObjects_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2 val);
			List<RaycastResult> guiRaycastObjects = UIUtils.GetGuiRaycastObjects(val);
			objectTranslator.Push(L, guiRaycastObjects);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowMessage_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 2) && objectTranslator.Assignable<Action>(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				string message = Lua.lua_tostring(L, 1);
				Action confirmAction = objectTranslator.GetDelegate<Action>(L, 2);
				Action cancelAction = objectTranslator.GetDelegate<Action>(L, 3);
				bool isChangeImg = Lua.lua_toboolean(L, 4);
				UIUtils.ShowMessage(message, confirmAction, cancelAction, isChangeImg);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 2) && objectTranslator.Assignable<Action>(L, 3))
			{
				string message2 = Lua.lua_tostring(L, 1);
				Action confirmAction2 = objectTranslator.GetDelegate<Action>(L, 2);
				Action cancelAction2 = objectTranslator.GetDelegate<Action>(L, 3);
				UIUtils.ShowMessage(message2, confirmAction2, cancelAction2);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 2))
			{
				string message3 = Lua.lua_tostring(L, 1);
				Action confirmAction3 = objectTranslator.GetDelegate<Action>(L, 2);
				UIUtils.ShowMessage(message3, confirmAction3);
				return 0;
			}
			if (num == 7 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 5) && objectTranslator.Assignable<Action>(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				string message4 = Lua.lua_tostring(L, 1);
				int buttonCount = Lua.xlua_tointeger(L, 2);
				string confirmText = Lua.lua_tostring(L, 3);
				string cancelText = Lua.lua_tostring(L, 4);
				Action confirmAction4 = objectTranslator.GetDelegate<Action>(L, 5);
				Action cancelAction3 = objectTranslator.GetDelegate<Action>(L, 6);
				bool isChangeImg2 = Lua.lua_toboolean(L, 7);
				UIUtils.ShowMessage(message4, buttonCount, confirmText, cancelText, confirmAction4, cancelAction3, isChangeImg2);
				return 0;
			}
			if (num == 6 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 5) && objectTranslator.Assignable<Action>(L, 6))
			{
				string message5 = Lua.lua_tostring(L, 1);
				int buttonCount2 = Lua.xlua_tointeger(L, 2);
				string confirmText2 = Lua.lua_tostring(L, 3);
				string cancelText2 = Lua.lua_tostring(L, 4);
				Action confirmAction5 = objectTranslator.GetDelegate<Action>(L, 5);
				Action cancelAction4 = objectTranslator.GetDelegate<Action>(L, 6);
				UIUtils.ShowMessage(message5, buttonCount2, confirmText2, cancelText2, confirmAction5, cancelAction4);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 5))
			{
				string message6 = Lua.lua_tostring(L, 1);
				int buttonCount3 = Lua.xlua_tointeger(L, 2);
				string confirmText3 = Lua.lua_tostring(L, 3);
				string cancelText3 = Lua.lua_tostring(L, 4);
				Action confirmAction6 = objectTranslator.GetDelegate<Action>(L, 5);
				UIUtils.ShowMessage(message6, buttonCount3, confirmText3, cancelText3, confirmAction6);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING))
			{
				string message7 = Lua.lua_tostring(L, 1);
				int buttonCount4 = Lua.xlua_tointeger(L, 2);
				string confirmText4 = Lua.lua_tostring(L, 3);
				string cancelText4 = Lua.lua_tostring(L, 4);
				UIUtils.ShowMessage(message7, buttonCount4, confirmText4, cancelText4);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string message8 = Lua.lua_tostring(L, 1);
				int buttonCount5 = Lua.xlua_tointeger(L, 2);
				string confirmText5 = Lua.lua_tostring(L, 3);
				UIUtils.ShowMessage(message8, buttonCount5, confirmText5);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				string message9 = Lua.lua_tostring(L, 1);
				int buttonCount6 = Lua.xlua_tointeger(L, 2);
				UIUtils.ShowMessage(message9, buttonCount6);
				return 0;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				UIUtils.ShowMessage(Lua.lua_tostring(L, 1));
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIUtils.ShowMessage!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowMessages_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 7 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 5) && objectTranslator.Assignable<Action>(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				string message = Lua.lua_tostring(L, 1);
				int buttonCount = Lua.xlua_tointeger(L, 2);
				string cancelText = Lua.lua_tostring(L, 3);
				string confirmText = Lua.lua_tostring(L, 4);
				Action confirmAction = objectTranslator.GetDelegate<Action>(L, 5);
				Action cancelAction = objectTranslator.GetDelegate<Action>(L, 6);
				bool isChangeImg = Lua.lua_toboolean(L, 7);
				UIUtils.ShowMessages(message, buttonCount, cancelText, confirmText, confirmAction, cancelAction, isChangeImg);
				return 0;
			}
			if (num == 6 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 5) && objectTranslator.Assignable<Action>(L, 6))
			{
				string message2 = Lua.lua_tostring(L, 1);
				int buttonCount2 = Lua.xlua_tointeger(L, 2);
				string cancelText2 = Lua.lua_tostring(L, 3);
				string confirmText2 = Lua.lua_tostring(L, 4);
				Action confirmAction2 = objectTranslator.GetDelegate<Action>(L, 5);
				Action cancelAction2 = objectTranslator.GetDelegate<Action>(L, 6);
				UIUtils.ShowMessages(message2, buttonCount2, cancelText2, confirmText2, confirmAction2, cancelAction2);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 5))
			{
				string message3 = Lua.lua_tostring(L, 1);
				int buttonCount3 = Lua.xlua_tointeger(L, 2);
				string cancelText3 = Lua.lua_tostring(L, 3);
				string confirmText3 = Lua.lua_tostring(L, 4);
				Action confirmAction3 = objectTranslator.GetDelegate<Action>(L, 5);
				UIUtils.ShowMessages(message3, buttonCount3, cancelText3, confirmText3, confirmAction3);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING))
			{
				string message4 = Lua.lua_tostring(L, 1);
				int buttonCount4 = Lua.xlua_tointeger(L, 2);
				string cancelText4 = Lua.lua_tostring(L, 3);
				string confirmText4 = Lua.lua_tostring(L, 4);
				UIUtils.ShowMessages(message4, buttonCount4, cancelText4, confirmText4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIUtils.ShowMessages!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NewShowMessage_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 2) && objectTranslator.Assignable<Action>(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				string message = Lua.lua_tostring(L, 1);
				Action confirmAction = objectTranslator.GetDelegate<Action>(L, 2);
				Action cancelAction = objectTranslator.GetDelegate<Action>(L, 3);
				bool isChangeImg = Lua.lua_toboolean(L, 4);
				UIUtils.NewShowMessage(message, confirmAction, cancelAction, isChangeImg);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 2) && objectTranslator.Assignable<Action>(L, 3))
			{
				string message2 = Lua.lua_tostring(L, 1);
				Action confirmAction2 = objectTranslator.GetDelegate<Action>(L, 2);
				Action cancelAction2 = objectTranslator.GetDelegate<Action>(L, 3);
				UIUtils.NewShowMessage(message2, confirmAction2, cancelAction2);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 2))
			{
				string message3 = Lua.lua_tostring(L, 1);
				Action confirmAction3 = objectTranslator.GetDelegate<Action>(L, 2);
				UIUtils.NewShowMessage(message3, confirmAction3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIUtils.NewShowMessage!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowMessage3Action_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string message = Lua.lua_tostring(L, 1);
			string leftBtnText = Lua.lua_tostring(L, 2);
			string rightBtnText = Lua.lua_tostring(L, 3);
			Action action = objectTranslator.GetDelegate<Action>(L, 4);
			Action action2 = objectTranslator.GetDelegate<Action>(L, 5);
			Action closeAction = objectTranslator.GetDelegate<Action>(L, 6);
			bool isChangeImg = Lua.lua_toboolean(L, 7);
			UIUtils.ShowMessage3Action(message, leftBtnText, rightBtnText, action, action2, closeAction, isChangeImg);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowReloadMessage_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 5 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 2) && objectTranslator.Assignable<Action>(L, 3) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string message = Lua.lua_tostring(L, 1);
				Action confirmAction = objectTranslator.GetDelegate<Action>(L, 2);
				Action cancelAction = objectTranslator.GetDelegate<Action>(L, 3);
				string rTxt = Lua.lua_tostring(L, 4);
				float countTime = (float)Lua.lua_tonumber(L, 5);
				UIUtils.ShowReloadMessage(message, confirmAction, cancelAction, rTxt, countTime);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 2) && objectTranslator.Assignable<Action>(L, 3) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING))
			{
				string message2 = Lua.lua_tostring(L, 1);
				Action confirmAction2 = objectTranslator.GetDelegate<Action>(L, 2);
				Action cancelAction2 = objectTranslator.GetDelegate<Action>(L, 3);
				string rTxt2 = Lua.lua_tostring(L, 4);
				UIUtils.ShowReloadMessage(message2, confirmAction2, cancelAction2, rTxt2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 2) && objectTranslator.Assignable<Action>(L, 3))
			{
				string message3 = Lua.lua_tostring(L, 1);
				Action confirmAction3 = objectTranslator.GetDelegate<Action>(L, 2);
				Action cancelAction3 = objectTranslator.GetDelegate<Action>(L, 3);
				UIUtils.ShowReloadMessage(message3, confirmAction3, cancelAction3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIUtils.ShowReloadMessage!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowLoadingMask_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			switch (num)
			{
			case 0:
				UIUtils.ShowLoadingMask();
				return 0;
			case 3:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && objectTranslator.Assignable<Color>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
				{
					bool animation = Lua.lua_toboolean(L, 1);
					objectTranslator.Get(L, 2, out Color val);
					UIUtils.ShowLoadingMask(isAutoTimer: Lua.lua_toboolean(L, 3), animation: animation, color: val);
					return 0;
				}
				break;
			}
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && objectTranslator.Assignable<Color>(L, 2))
			{
				bool animation2 = Lua.lua_toboolean(L, 1);
				objectTranslator.Get(L, 2, out Color val2);
				UIUtils.ShowLoadingMask(animation2, val2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIUtils.ShowLoadingMask!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HideLoadingMask_xlua_st_(IntPtr L)
	{
		try
		{
			UIUtils.HideLoadingMask();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowTips_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num >= 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 3) || objectTranslator.Assignable<object>(L, 3)))
			{
				string msgKey = Lua.lua_tostring(L, 1);
				float closeTime = (float)Lua.lua_tonumber(L, 2);
				object[] args = objectTranslator.GetParams<object>(L, 3);
				UIUtils.ShowTips(msgKey, closeTime, args);
				return 0;
			}
			if (num >= 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				string msgKey2 = Lua.lua_tostring(L, 1);
				float closeTime2 = (float)Lua.lua_tonumber(L, 2);
				UIUtils.ShowTips(msgKey2, closeTime2);
				return 0;
			}
			if (num >= 0 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				UIUtils.ShowTips(Lua.lua_tostring(L, 1), 3f);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIUtils.ShowTips!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowLackResource_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 6 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Dictionary<ResourceType, long>>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				string tipContent = Lua.lua_tostring(L, 1);
				Dictionary<ResourceType, long> resources = (Dictionary<ResourceType, long>)objectTranslator.GetObject(L, 2, typeof(Dictionary<ResourceType, long>));
				string actionName = Lua.lua_tostring(L, 3);
				Action actionCallBack = objectTranslator.GetDelegate<Action>(L, 4);
				bool tipContentIsKey = Lua.lua_toboolean(L, 5);
				bool actionNameIsKey = Lua.lua_toboolean(L, 6);
				UIUtils.ShowLackResource(tipContent, resources, actionName, actionCallBack, tipContentIsKey, actionNameIsKey);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Dictionary<ResourceType, long>>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				string tipContent2 = Lua.lua_tostring(L, 1);
				Dictionary<ResourceType, long> resources2 = (Dictionary<ResourceType, long>)objectTranslator.GetObject(L, 2, typeof(Dictionary<ResourceType, long>));
				string actionName2 = Lua.lua_tostring(L, 3);
				Action actionCallBack2 = objectTranslator.GetDelegate<Action>(L, 4);
				bool tipContentIsKey2 = Lua.lua_toboolean(L, 5);
				UIUtils.ShowLackResource(tipContent2, resources2, actionName2, actionCallBack2, tipContentIsKey2);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Dictionary<ResourceType, long>>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 4))
			{
				string tipContent3 = Lua.lua_tostring(L, 1);
				Dictionary<ResourceType, long> resources3 = (Dictionary<ResourceType, long>)objectTranslator.GetObject(L, 2, typeof(Dictionary<ResourceType, long>));
				string actionName3 = Lua.lua_tostring(L, 3);
				Action actionCallBack3 = objectTranslator.GetDelegate<Action>(L, 4);
				UIUtils.ShowLackResource(tipContent3, resources3, actionName3, actionCallBack3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIUtils.ShowLackResource!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowRankView_xlua_st_(IntPtr L)
	{
		try
		{
			UIUtils.ShowRankView();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WordToScenePoint_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			Vector2 val2 = UIUtils.WordToScenePoint(val);
			objectTranslator.PushUnityEngineVector2(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScreenPointToLocalPointInRectangle_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
			objectTranslator.Get(L, 2, out Vector2 val);
			Vector2 val2 = UIUtils.ScreenPointToLocalPointInRectangle(transform, val);
			objectTranslator.PushUnityEngineVector2(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BuildSkeletonDataAsset_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string skeletonPath = Lua.lua_tostring(L, 1);
			UIUtils.SkeletonDelegate handler = objectTranslator.GetDelegate<UIUtils.SkeletonDelegate>(L, 2);
			UIUtils.BuildSkeletonDataAsset(skeletonPath, handler);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_checkUserName_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = UIUtils.checkUserName(Lua.lua_tostring(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_checkPwd_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				bool value = UIUtils.checkPwd(Lua.lua_tostring(L, 1));
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string pwd = Lua.lua_tostring(L, 1);
				string pwd2 = Lua.lua_tostring(L, 2);
				bool value2 = UIUtils.checkPwd(pwd, pwd2);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIUtils.checkPwd!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFirstChild_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform parent = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
			string name = Lua.lua_tostring(L, 2);
			Transform firstChild = UIUtils.GetFirstChild(parent, name);
			objectTranslator.Push(L, firstChild);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTextWithImage_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Text>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Image>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Text text = (Text)objectTranslator.GetObject(L, 1, typeof(Text));
				string des = Lua.lua_tostring(L, 2);
				Image image = (Image)objectTranslator.GetObject(L, 3, typeof(Image));
				int imageWidth = Lua.xlua_tointeger(L, 4);
				UIUtils.SetTextWithImage(text, des, image, imageWidth);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Text>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Image>(L, 3))
			{
				Text text2 = (Text)objectTranslator.GetObject(L, 1, typeof(Text));
				string des2 = Lua.lua_tostring(L, 2);
				Image image2 = (Image)objectTranslator.GetObject(L, 3, typeof(Image));
				UIUtils.SetTextWithImage(text2, des2, image2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIUtils.SetTextWithImage!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPosAtText_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Canvas>(L, 1) && objectTranslator.Assignable<Text>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Canvas canvas = (Canvas)objectTranslator.GetObject(L, 1, typeof(Canvas));
				Text text = (Text)objectTranslator.GetObject(L, 2, typeof(Text));
				int charIndex = Lua.xlua_tointeger(L, 3);
				Vector3 posAtText = UIUtils.GetPosAtText(canvas, text, charIndex);
				objectTranslator.PushUnityEngineVector3(L, posAtText);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Canvas>(L, 1) && objectTranslator.Assignable<Text>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				Canvas canvas2 = (Canvas)objectTranslator.GetObject(L, 1, typeof(Canvas));
				Text text2 = (Text)objectTranslator.GetObject(L, 2, typeof(Text));
				string strFragment = Lua.lua_tostring(L, 3);
				Vector3 posAtText2 = UIUtils.GetPosAtText(canvas2, text2, strFragment);
				objectTranslator.PushUnityEngineVector3(L, posAtText2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIUtils.GetPosAtText!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAnimationReturnTime_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<SimpleAnimation>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				SimpleAnimation anim = (SimpleAnimation)objectTranslator.GetObject(L, 1, typeof(SimpleAnimation));
				string animName = Lua.lua_tostring(L, 2);
				float num2 = UIUtils.PlayAnimationReturnTime(anim, animName);
				Lua.lua_pushnumber(L, num2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Animator>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				Animator anim2 = (Animator)objectTranslator.GetObject(L, 1, typeof(Animator));
				string animName2 = Lua.lua_tostring(L, 2);
				float num3 = UIUtils.PlayAnimationReturnTime(anim2, animName2);
				Lua.lua_pushnumber(L, num3);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<GPUSkinningAnimator>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				GPUSkinningAnimator anim3 = (GPUSkinningAnimator)objectTranslator.GetObject(L, 1, typeof(GPUSkinningAnimator));
				string animName3 = Lua.lua_tostring(L, 2);
				float num4 = UIUtils.PlayAnimationReturnTime(anim3, animName3);
				Lua.lua_pushnumber(L, num4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIUtils.PlayAnimationReturnTime!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayCrossFadeAnimationReturnTime_xlua_st_(IntPtr L)
	{
		try
		{
			SimpleAnimation anim = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(SimpleAnimation));
			string animName = Lua.lua_tostring(L, 2);
			float corssTime = (float)Lua.lua_tonumber(L, 3);
			float num = UIUtils.PlayCrossFadeAnimationReturnTime(anim, animName, corssTime);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurScreenMaxRadiusSize_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector2Int curScreenMaxRadiusSize = UIUtils.GetCurScreenMaxRadiusSize();
			objectTranslator.Push(L, curScreenMaxRadiusSize);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPointByMeteoriteHitPlane_xlua_st_(IntPtr L)
	{
		try
		{
			int pointByMeteoriteHitPlane = UIUtils.GetPointByMeteoriteHitPlane();
			Lua.xlua_pushinteger(L, pointByMeteoriteHitPlane);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildPointByMeteoriteHitGlass_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointByMeteoriteHitGlass = UIUtils.GetBuildPointByMeteoriteHitGlass();
			objectTranslator.Push(L, buildPointByMeteoriteHitGlass);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FormatServerAllianceName_xlua_st_(IntPtr L)
	{
		try
		{
			int serverId = Lua.xlua_tointeger(L, 1);
			string abbr = Lua.lua_tostring(L, 2);
			string name = Lua.lua_tostring(L, 3);
			string str = UIUtils.FormatServerAllianceName(serverId, abbr, name);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FormatAllianceAndName_xlua_st_(IntPtr L)
	{
		try
		{
			string abbr = Lua.lua_tostring(L, 1);
			string name = Lua.lua_tostring(L, 2);
			string str = UIUtils.FormatAllianceAndName(abbr, name);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_tmpWidth(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, UIUtils.tmpWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_leftTopScreenPoint(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector2(L, UIUtils.leftTopScreenPoint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_leftTopRectPoint(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector2(L, UIUtils.leftTopRectPoint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tmpWidth(IntPtr L)
	{
		try
		{
			UIUtils.tmpWidth = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_leftTopScreenPoint(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector2 val);
			UIUtils.leftTopScreenPoint = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_leftTopRectPoint(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector2 val);
			UIUtils.leftTopRectPoint = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
