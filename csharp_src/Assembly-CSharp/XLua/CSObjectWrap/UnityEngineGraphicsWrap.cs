using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineGraphicsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Graphics);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 23, 5, 1);
		Utils.RegisterFunc(L, -4, "ClearRandomWriteTargets", _m_ClearRandomWriteTargets_xlua_st_);
		Utils.RegisterFunc(L, -4, "ExecuteCommandBuffer", _m_ExecuteCommandBuffer_xlua_st_);
		Utils.RegisterFunc(L, -4, "ExecuteCommandBufferAsync", _m_ExecuteCommandBufferAsync_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetRenderTarget", _m_SetRenderTarget_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetRandomWriteTarget", _m_SetRandomWriteTarget_xlua_st_);
		Utils.RegisterFunc(L, -4, "CopyTexture", _m_CopyTexture_xlua_st_);
		Utils.RegisterFunc(L, -4, "ConvertTexture", _m_ConvertTexture_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateAsyncGraphicsFence", _m_CreateAsyncGraphicsFence_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateGraphicsFence", _m_CreateGraphicsFence_xlua_st_);
		Utils.RegisterFunc(L, -4, "WaitOnAsyncGraphicsFence", _m_WaitOnAsyncGraphicsFence_xlua_st_);
		Utils.RegisterFunc(L, -4, "DrawTexture", _m_DrawTexture_xlua_st_);
		Utils.RegisterFunc(L, -4, "DrawMeshNow", _m_DrawMeshNow_xlua_st_);
		Utils.RegisterFunc(L, -4, "DrawMesh", _m_DrawMesh_xlua_st_);
		Utils.RegisterFunc(L, -4, "DrawMeshInstanced", _m_DrawMeshInstanced_xlua_st_);
		Utils.RegisterFunc(L, -4, "DrawMeshInstancedProcedural", _m_DrawMeshInstancedProcedural_xlua_st_);
		Utils.RegisterFunc(L, -4, "DrawMeshInstancedIndirect", _m_DrawMeshInstancedIndirect_xlua_st_);
		Utils.RegisterFunc(L, -4, "DrawProceduralNow", _m_DrawProceduralNow_xlua_st_);
		Utils.RegisterFunc(L, -4, "DrawProceduralIndirectNow", _m_DrawProceduralIndirectNow_xlua_st_);
		Utils.RegisterFunc(L, -4, "DrawProcedural", _m_DrawProcedural_xlua_st_);
		Utils.RegisterFunc(L, -4, "DrawProceduralIndirect", _m_DrawProceduralIndirect_xlua_st_);
		Utils.RegisterFunc(L, -4, "Blit", _m_Blit_xlua_st_);
		Utils.RegisterFunc(L, -4, "BlitMultiTap", _m_BlitMultiTap_xlua_st_);
		Utils.RegisterFunc(L, -2, "activeColorGamut", _g_get_activeColorGamut);
		Utils.RegisterFunc(L, -2, "activeTier", _g_get_activeTier);
		Utils.RegisterFunc(L, -2, "preserveFramebufferAlpha", _g_get_preserveFramebufferAlpha);
		Utils.RegisterFunc(L, -2, "activeColorBuffer", _g_get_activeColorBuffer);
		Utils.RegisterFunc(L, -2, "activeDepthBuffer", _g_get_activeDepthBuffer);
		Utils.RegisterFunc(L, -1, "activeTier", _s_set_activeTier);
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
				Graphics o = new Graphics();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearRandomWriteTargets_xlua_st_(IntPtr L)
	{
		try
		{
			Graphics.ClearRandomWriteTargets();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExecuteCommandBuffer_xlua_st_(IntPtr L)
	{
		try
		{
			Graphics.ExecuteCommandBuffer((CommandBuffer)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(CommandBuffer)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExecuteCommandBufferAsync_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CommandBuffer buffer = (CommandBuffer)objectTranslator.GetObject(L, 1, typeof(CommandBuffer));
			objectTranslator.Get(L, 2, out ComputeQueueType v);
			Graphics.ExecuteCommandBufferAsync(buffer, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRenderTarget_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<RenderTargetSetup>(L, 1))
			{
				objectTranslator.Get(L, 1, out RenderTargetSetup v);
				Graphics.SetRenderTarget(v);
				return 0;
			}
			if (num == 1 && objectTranslator.Assignable<RenderTexture>(L, 1))
			{
				Graphics.SetRenderTarget((RenderTexture)objectTranslator.GetObject(L, 1, typeof(RenderTexture)));
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<RenderTexture>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				RenderTexture rt = (RenderTexture)objectTranslator.GetObject(L, 1, typeof(RenderTexture));
				int mipLevel = Lua.xlua_tointeger(L, 2);
				Graphics.SetRenderTarget(rt, mipLevel);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<RenderBuffer[]>(L, 1) && objectTranslator.Assignable<RenderBuffer>(L, 2))
			{
				RenderBuffer[] colorBuffers = (RenderBuffer[])objectTranslator.GetObject(L, 1, typeof(RenderBuffer[]));
				objectTranslator.Get(L, 2, out RenderBuffer v2);
				Graphics.SetRenderTarget(colorBuffers, v2);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<RenderBuffer>(L, 1) && objectTranslator.Assignable<RenderBuffer>(L, 2))
			{
				objectTranslator.Get(L, 1, out RenderBuffer v3);
				objectTranslator.Get(L, 2, out RenderBuffer v4);
				Graphics.SetRenderTarget(v3, v4);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<RenderTexture>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<CubemapFace>(L, 3))
			{
				RenderTexture rt2 = (RenderTexture)objectTranslator.GetObject(L, 1, typeof(RenderTexture));
				int mipLevel2 = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out CubemapFace v5);
				Graphics.SetRenderTarget(rt2, mipLevel2, v5);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<RenderBuffer>(L, 1) && objectTranslator.Assignable<RenderBuffer>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out RenderBuffer v6);
				objectTranslator.Get(L, 2, out RenderBuffer v7);
				int mipLevel3 = Lua.xlua_tointeger(L, 3);
				Graphics.SetRenderTarget(v6, v7, mipLevel3);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<RenderTexture>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<CubemapFace>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				RenderTexture rt3 = (RenderTexture)objectTranslator.GetObject(L, 1, typeof(RenderTexture));
				int mipLevel4 = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out CubemapFace v8);
				Graphics.SetRenderTarget(depthSlice: Lua.xlua_tointeger(L, 4), rt: rt3, mipLevel: mipLevel4, face: v8);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<RenderBuffer>(L, 1) && objectTranslator.Assignable<RenderBuffer>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<CubemapFace>(L, 4))
			{
				objectTranslator.Get(L, 1, out RenderBuffer v9);
				objectTranslator.Get(L, 2, out RenderBuffer v10);
				int mipLevel5 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out CubemapFace v11);
				Graphics.SetRenderTarget(v9, v10, mipLevel5, v11);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<RenderBuffer>(L, 1) && objectTranslator.Assignable<RenderBuffer>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<CubemapFace>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out RenderBuffer v12);
				objectTranslator.Get(L, 2, out RenderBuffer v13);
				int mipLevel6 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out CubemapFace v14);
				int depthSlice = Lua.xlua_tointeger(L, 5);
				Graphics.SetRenderTarget(v12, v13, mipLevel6, v14, depthSlice);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.SetRenderTarget!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRandomWriteTarget_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<RenderTexture>(L, 2))
			{
				int index = Lua.xlua_tointeger(L, 1);
				RenderTexture uav = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				Graphics.SetRandomWriteTarget(index, uav);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<ComputeBuffer>(L, 2))
			{
				int index2 = Lua.xlua_tointeger(L, 1);
				ComputeBuffer uav2 = (ComputeBuffer)objectTranslator.GetObject(L, 2, typeof(ComputeBuffer));
				Graphics.SetRandomWriteTarget(index2, uav2);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<ComputeBuffer>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int index3 = Lua.xlua_tointeger(L, 1);
				ComputeBuffer uav3 = (ComputeBuffer)objectTranslator.GetObject(L, 2, typeof(ComputeBuffer));
				bool preserveCounterValue = Lua.lua_toboolean(L, 3);
				Graphics.SetRandomWriteTarget(index3, uav3, preserveCounterValue);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.SetRandomWriteTarget!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CopyTexture_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<Texture>(L, 2))
			{
				Texture src = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				Texture dst = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				Graphics.CopyTexture(src, dst);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Texture>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Texture>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Texture src2 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				int srcElement = Lua.xlua_tointeger(L, 2);
				Texture dst2 = (Texture)objectTranslator.GetObject(L, 3, typeof(Texture));
				int dstElement = Lua.xlua_tointeger(L, 4);
				Graphics.CopyTexture(src2, srcElement, dst2, dstElement);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Texture>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Texture>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				Texture src3 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				int srcElement2 = Lua.xlua_tointeger(L, 2);
				int srcMip = Lua.xlua_tointeger(L, 3);
				Texture dst3 = (Texture)objectTranslator.GetObject(L, 4, typeof(Texture));
				int dstElement2 = Lua.xlua_tointeger(L, 5);
				int dstMip = Lua.xlua_tointeger(L, 6);
				Graphics.CopyTexture(src3, srcElement2, srcMip, dst3, dstElement2, dstMip);
				return 0;
			}
			if (num == 12 && objectTranslator.Assignable<Texture>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<Texture>(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 12))
			{
				Texture src4 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				int srcElement3 = Lua.xlua_tointeger(L, 2);
				int srcMip2 = Lua.xlua_tointeger(L, 3);
				int srcX = Lua.xlua_tointeger(L, 4);
				int srcY = Lua.xlua_tointeger(L, 5);
				int srcWidth = Lua.xlua_tointeger(L, 6);
				int srcHeight = Lua.xlua_tointeger(L, 7);
				Texture dst4 = (Texture)objectTranslator.GetObject(L, 8, typeof(Texture));
				int dstElement3 = Lua.xlua_tointeger(L, 9);
				int dstMip2 = Lua.xlua_tointeger(L, 10);
				int dstX = Lua.xlua_tointeger(L, 11);
				int dstY = Lua.xlua_tointeger(L, 12);
				Graphics.CopyTexture(src4, srcElement3, srcMip2, srcX, srcY, srcWidth, srcHeight, dst4, dstElement3, dstMip2, dstX, dstY);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.CopyTexture!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConvertTexture_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<Texture>(L, 2))
			{
				Texture src = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				Texture dst = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				bool value = Graphics.ConvertTexture(src, dst);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Texture>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Texture>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Texture src2 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				int srcElement = Lua.xlua_tointeger(L, 2);
				Texture dst2 = (Texture)objectTranslator.GetObject(L, 3, typeof(Texture));
				int dstElement = Lua.xlua_tointeger(L, 4);
				bool value2 = Graphics.ConvertTexture(src2, srcElement, dst2, dstElement);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.ConvertTexture!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateAsyncGraphicsFence_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			switch (Lua.lua_gettop(L))
			{
			case 0:
			{
				GraphicsFence graphicsFence2 = Graphics.CreateAsyncGraphicsFence();
				objectTranslator.Push(L, graphicsFence2);
				return 1;
			}
			case 1:
				if (objectTranslator.Assignable<SynchronisationStage>(L, 1))
				{
					objectTranslator.Get(L, 1, out SynchronisationStage v);
					GraphicsFence graphicsFence = Graphics.CreateAsyncGraphicsFence(v);
					objectTranslator.Push(L, graphicsFence);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.CreateAsyncGraphicsFence!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateGraphicsFence_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out GraphicsFenceType v);
			objectTranslator.Get(L, 2, out SynchronisationStageFlags v2);
			GraphicsFence graphicsFence = Graphics.CreateGraphicsFence(v, v2);
			objectTranslator.Push(L, graphicsFence);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WaitOnAsyncGraphicsFence_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<GraphicsFence>(L, 1))
			{
				objectTranslator.Get(L, 1, out GraphicsFence v);
				Graphics.WaitOnAsyncGraphicsFence(v);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<GraphicsFence>(L, 1) && objectTranslator.Assignable<SynchronisationStage>(L, 2))
			{
				objectTranslator.Get(L, 1, out GraphicsFence v2);
				objectTranslator.Get(L, 2, out SynchronisationStage v3);
				Graphics.WaitOnAsyncGraphicsFence(v2, v3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.WaitOnAsyncGraphicsFence!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DrawTexture_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Rect>(L, 1) && objectTranslator.Assignable<Texture>(L, 2))
			{
				objectTranslator.Get(L, 1, out Rect v);
				Texture texture = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				Graphics.DrawTexture(v, texture);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Rect>(L, 1) && objectTranslator.Assignable<Texture>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 1, out Rect v2);
				Texture texture2 = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				int leftBorder = Lua.xlua_tointeger(L, 3);
				int rightBorder = Lua.xlua_tointeger(L, 4);
				int topBorder = Lua.xlua_tointeger(L, 5);
				int bottomBorder = Lua.xlua_tointeger(L, 6);
				Graphics.DrawTexture(v2, texture2, leftBorder, rightBorder, topBorder, bottomBorder);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Rect>(L, 1) && objectTranslator.Assignable<Texture>(L, 2) && objectTranslator.Assignable<Material>(L, 3))
			{
				objectTranslator.Get(L, 1, out Rect v3);
				Texture texture3 = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				Material mat = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				Graphics.DrawTexture(v3, texture3, mat);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Rect>(L, 1) && objectTranslator.Assignable<Texture>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Rect v4);
				Texture texture4 = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				Material mat2 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				int pass = Lua.xlua_tointeger(L, 4);
				Graphics.DrawTexture(v4, texture4, mat2, pass);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<Rect>(L, 1) && objectTranslator.Assignable<Texture>(L, 2) && objectTranslator.Assignable<Rect>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				objectTranslator.Get(L, 1, out Rect v5);
				Texture texture5 = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				objectTranslator.Get(L, 3, out Rect v6);
				int leftBorder2 = Lua.xlua_tointeger(L, 4);
				int rightBorder2 = Lua.xlua_tointeger(L, 5);
				int topBorder2 = Lua.xlua_tointeger(L, 6);
				int bottomBorder2 = Lua.xlua_tointeger(L, 7);
				Graphics.DrawTexture(v5, texture5, v6, leftBorder2, rightBorder2, topBorder2, bottomBorder2);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<Rect>(L, 1) && objectTranslator.Assignable<Texture>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Material>(L, 7))
			{
				objectTranslator.Get(L, 1, out Rect v7);
				Texture texture6 = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				int leftBorder3 = Lua.xlua_tointeger(L, 3);
				int rightBorder3 = Lua.xlua_tointeger(L, 4);
				int topBorder3 = Lua.xlua_tointeger(L, 5);
				int bottomBorder3 = Lua.xlua_tointeger(L, 6);
				Material mat3 = (Material)objectTranslator.GetObject(L, 7, typeof(Material));
				Graphics.DrawTexture(v7, texture6, leftBorder3, rightBorder3, topBorder3, bottomBorder3, mat3);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Rect>(L, 1) && objectTranslator.Assignable<Texture>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Material>(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				objectTranslator.Get(L, 1, out Rect v8);
				Texture texture7 = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				int leftBorder4 = Lua.xlua_tointeger(L, 3);
				int rightBorder4 = Lua.xlua_tointeger(L, 4);
				int topBorder4 = Lua.xlua_tointeger(L, 5);
				int bottomBorder4 = Lua.xlua_tointeger(L, 6);
				Material mat4 = (Material)objectTranslator.GetObject(L, 7, typeof(Material));
				int pass2 = Lua.xlua_tointeger(L, 8);
				Graphics.DrawTexture(v8, texture7, leftBorder4, rightBorder4, topBorder4, bottomBorder4, mat4, pass2);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Rect>(L, 1) && objectTranslator.Assignable<Texture>(L, 2) && objectTranslator.Assignable<Rect>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<Color>(L, 8))
			{
				objectTranslator.Get(L, 1, out Rect v9);
				Texture texture8 = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				objectTranslator.Get(L, 3, out Rect v10);
				int leftBorder5 = Lua.xlua_tointeger(L, 4);
				int rightBorder5 = Lua.xlua_tointeger(L, 5);
				int topBorder5 = Lua.xlua_tointeger(L, 6);
				int bottomBorder5 = Lua.xlua_tointeger(L, 7);
				objectTranslator.Get(L, 8, out Color val);
				Graphics.DrawTexture(v9, texture8, v10, leftBorder5, rightBorder5, topBorder5, bottomBorder5, val);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Rect>(L, 1) && objectTranslator.Assignable<Texture>(L, 2) && objectTranslator.Assignable<Rect>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<Material>(L, 8))
			{
				objectTranslator.Get(L, 1, out Rect v11);
				Texture texture9 = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				objectTranslator.Get(L, 3, out Rect v12);
				int leftBorder6 = Lua.xlua_tointeger(L, 4);
				int rightBorder6 = Lua.xlua_tointeger(L, 5);
				int topBorder6 = Lua.xlua_tointeger(L, 6);
				int bottomBorder6 = Lua.xlua_tointeger(L, 7);
				Material mat5 = (Material)objectTranslator.GetObject(L, 8, typeof(Material));
				Graphics.DrawTexture(v11, texture9, v12, leftBorder6, rightBorder6, topBorder6, bottomBorder6, mat5);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Rect>(L, 1) && objectTranslator.Assignable<Texture>(L, 2) && objectTranslator.Assignable<Rect>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<Material>(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9))
			{
				objectTranslator.Get(L, 1, out Rect v13);
				Texture texture10 = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				objectTranslator.Get(L, 3, out Rect v14);
				int leftBorder7 = Lua.xlua_tointeger(L, 4);
				int rightBorder7 = Lua.xlua_tointeger(L, 5);
				int topBorder7 = Lua.xlua_tointeger(L, 6);
				int bottomBorder7 = Lua.xlua_tointeger(L, 7);
				Material mat6 = (Material)objectTranslator.GetObject(L, 8, typeof(Material));
				int pass3 = Lua.xlua_tointeger(L, 9);
				Graphics.DrawTexture(v13, texture10, v14, leftBorder7, rightBorder7, topBorder7, bottomBorder7, mat6, pass3);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Rect>(L, 1) && objectTranslator.Assignable<Texture>(L, 2) && objectTranslator.Assignable<Rect>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<Color>(L, 8) && objectTranslator.Assignable<Material>(L, 9))
			{
				objectTranslator.Get(L, 1, out Rect v15);
				Texture texture11 = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				objectTranslator.Get(L, 3, out Rect v16);
				int leftBorder8 = Lua.xlua_tointeger(L, 4);
				int rightBorder8 = Lua.xlua_tointeger(L, 5);
				int topBorder8 = Lua.xlua_tointeger(L, 6);
				int bottomBorder8 = Lua.xlua_tointeger(L, 7);
				objectTranslator.Get(L, 8, out Color val2);
				Material mat7 = (Material)objectTranslator.GetObject(L, 9, typeof(Material));
				Graphics.DrawTexture(v15, texture11, v16, leftBorder8, rightBorder8, topBorder8, bottomBorder8, val2, mat7);
				return 0;
			}
			if (num == 10 && objectTranslator.Assignable<Rect>(L, 1) && objectTranslator.Assignable<Texture>(L, 2) && objectTranslator.Assignable<Rect>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<Color>(L, 8) && objectTranslator.Assignable<Material>(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10))
			{
				objectTranslator.Get(L, 1, out Rect v17);
				Texture texture12 = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
				objectTranslator.Get(L, 3, out Rect v18);
				int leftBorder9 = Lua.xlua_tointeger(L, 4);
				int rightBorder9 = Lua.xlua_tointeger(L, 5);
				int topBorder9 = Lua.xlua_tointeger(L, 6);
				int bottomBorder9 = Lua.xlua_tointeger(L, 7);
				objectTranslator.Get(L, 8, out Color val3);
				Material mat8 = (Material)objectTranslator.GetObject(L, 9, typeof(Material));
				int pass4 = Lua.xlua_tointeger(L, 10);
				Graphics.DrawTexture(v17, texture12, v18, leftBorder9, rightBorder9, topBorder9, bottomBorder9, val3, mat8, pass4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.DrawTexture!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DrawMeshNow_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2))
			{
				Mesh mesh = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v);
				Graphics.DrawMeshNow(mesh, v);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Mesh mesh2 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v2);
				Graphics.DrawMeshNow(materialIndex: Lua.xlua_tointeger(L, 3), mesh: mesh2, matrix: v2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3))
			{
				Mesh mesh3 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Vector3 val);
				objectTranslator.Get(L, 3, out Quaternion val2);
				Graphics.DrawMeshNow(mesh3, val, val2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Mesh mesh4 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Quaternion val4);
				Graphics.DrawMeshNow(materialIndex: Lua.xlua_tointeger(L, 4), mesh: mesh4, position: val3, rotation: val4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.DrawMeshNow!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DrawMesh_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Mesh mesh = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 3, typeof(Material)), layer: Lua.xlua_tointeger(L, 4), mesh: mesh, matrix: v);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<Material>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				Mesh mesh2 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Vector3 val);
				objectTranslator.Get(L, 3, out Quaternion val2);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 4, typeof(Material)), layer: Lua.xlua_tointeger(L, 5), mesh: mesh2, position: val, rotation: val2);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Camera>(L, 5))
			{
				Mesh mesh3 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v2);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 3, typeof(Material)), layer: Lua.xlua_tointeger(L, 4), camera: (Camera)objectTranslator.GetObject(L, 5, typeof(Camera)), mesh: mesh3, matrix: v2);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Camera>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				Mesh mesh4 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v3);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 3, typeof(Material)), layer: Lua.xlua_tointeger(L, 4), camera: (Camera)objectTranslator.GetObject(L, 5, typeof(Camera)), submeshIndex: Lua.xlua_tointeger(L, 6), mesh: mesh4, matrix: v3);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<Material>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6))
			{
				Mesh mesh5 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Quaternion val4);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 4, typeof(Material)), layer: Lua.xlua_tointeger(L, 5), camera: (Camera)objectTranslator.GetObject(L, 6, typeof(Camera)), mesh: mesh5, position: val3, rotation: val4);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<Material>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				Mesh mesh6 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Vector3 val5);
				objectTranslator.Get(L, 3, out Quaternion val6);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 4, typeof(Material)), layer: Lua.xlua_tointeger(L, 5), camera: (Camera)objectTranslator.GetObject(L, 6, typeof(Camera)), submeshIndex: Lua.xlua_tointeger(L, 7), mesh: mesh6, position: val5, rotation: val6);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Camera>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7))
			{
				Mesh mesh7 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v4);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 3, typeof(Material)), layer: Lua.xlua_tointeger(L, 4), camera: (Camera)objectTranslator.GetObject(L, 5, typeof(Camera)), submeshIndex: Lua.xlua_tointeger(L, 6), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock)), mesh: mesh7, matrix: v4);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Camera>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				Mesh mesh8 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v5);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 3, typeof(Material)), layer: Lua.xlua_tointeger(L, 4), camera: (Camera)objectTranslator.GetObject(L, 5, typeof(Camera)), submeshIndex: Lua.xlua_tointeger(L, 6), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock)), castShadows: Lua.lua_toboolean(L, 8), mesh: mesh8, matrix: v5);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Camera>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9))
			{
				Mesh mesh9 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v6);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 3, typeof(Material)), layer: Lua.xlua_tointeger(L, 4), camera: (Camera)objectTranslator.GetObject(L, 5, typeof(Camera)), submeshIndex: Lua.xlua_tointeger(L, 6), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock)), castShadows: Lua.lua_toboolean(L, 8), receiveShadows: Lua.lua_toboolean(L, 9), mesh: mesh9, matrix: v6);
				return 0;
			}
			if (num == 10 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Camera>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 10))
			{
				Mesh mesh10 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v7);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 3, typeof(Material)), layer: Lua.xlua_tointeger(L, 4), camera: (Camera)objectTranslator.GetObject(L, 5, typeof(Camera)), submeshIndex: Lua.xlua_tointeger(L, 6), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock)), castShadows: Lua.lua_toboolean(L, 8), receiveShadows: Lua.lua_toboolean(L, 9), useLightProbes: Lua.lua_toboolean(L, 10), mesh: mesh10, matrix: v7);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<Material>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8))
			{
				Mesh mesh11 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Vector3 val7);
				objectTranslator.Get(L, 3, out Quaternion val8);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 4, typeof(Material)), layer: Lua.xlua_tointeger(L, 5), camera: (Camera)objectTranslator.GetObject(L, 6, typeof(Camera)), submeshIndex: Lua.xlua_tointeger(L, 7), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock)), mesh: mesh11, position: val7, rotation: val8);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Camera>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8))
			{
				Mesh mesh12 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v8);
				Material material = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				int layer = Lua.xlua_tointeger(L, 4);
				Camera camera = (Camera)objectTranslator.GetObject(L, 5, typeof(Camera));
				int submeshIndex = Lua.xlua_tointeger(L, 6);
				MaterialPropertyBlock properties = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v9);
				Graphics.DrawMesh(mesh12, v8, material, layer, camera, submeshIndex, properties, v9);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<Material>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9))
			{
				Mesh mesh13 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Vector3 val9);
				objectTranslator.Get(L, 3, out Quaternion val10);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 4, typeof(Material)), layer: Lua.xlua_tointeger(L, 5), camera: (Camera)objectTranslator.GetObject(L, 6, typeof(Camera)), submeshIndex: Lua.xlua_tointeger(L, 7), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock)), castShadows: Lua.lua_toboolean(L, 9), mesh: mesh13, position: val9, rotation: val10);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Camera>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9))
			{
				Mesh mesh14 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v10);
				Material material2 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				int layer2 = Lua.xlua_tointeger(L, 4);
				Camera camera2 = (Camera)objectTranslator.GetObject(L, 5, typeof(Camera));
				int submeshIndex2 = Lua.xlua_tointeger(L, 6);
				MaterialPropertyBlock properties2 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v11);
				Graphics.DrawMesh(receiveShadows: Lua.lua_toboolean(L, 9), mesh: mesh14, matrix: v10, material: material2, layer: layer2, camera: camera2, submeshIndex: submeshIndex2, properties: properties2, castShadows: v11);
				return 0;
			}
			if (num == 10 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<Material>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 10))
			{
				Mesh mesh15 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Vector3 val11);
				objectTranslator.Get(L, 3, out Quaternion val12);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 4, typeof(Material)), layer: Lua.xlua_tointeger(L, 5), camera: (Camera)objectTranslator.GetObject(L, 6, typeof(Camera)), submeshIndex: Lua.xlua_tointeger(L, 7), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock)), castShadows: Lua.lua_toboolean(L, 9), receiveShadows: Lua.lua_toboolean(L, 10), mesh: mesh15, position: val11, rotation: val12);
				return 0;
			}
			if (num == 11 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<Material>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 10) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 11))
			{
				Mesh mesh16 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Vector3 val13);
				objectTranslator.Get(L, 3, out Quaternion val14);
				Graphics.DrawMesh(material: (Material)objectTranslator.GetObject(L, 4, typeof(Material)), layer: Lua.xlua_tointeger(L, 5), camera: (Camera)objectTranslator.GetObject(L, 6, typeof(Camera)), submeshIndex: Lua.xlua_tointeger(L, 7), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock)), castShadows: Lua.lua_toboolean(L, 9), receiveShadows: Lua.lua_toboolean(L, 10), useLightProbes: Lua.lua_toboolean(L, 11), mesh: mesh16, position: val13, rotation: val14);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<Material>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8) && objectTranslator.Assignable<ShadowCastingMode>(L, 9))
			{
				Mesh mesh17 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Vector3 val15);
				objectTranslator.Get(L, 3, out Quaternion val16);
				Material material3 = (Material)objectTranslator.GetObject(L, 4, typeof(Material));
				int layer3 = Lua.xlua_tointeger(L, 5);
				Camera camera3 = (Camera)objectTranslator.GetObject(L, 6, typeof(Camera));
				int submeshIndex3 = Lua.xlua_tointeger(L, 7);
				MaterialPropertyBlock properties3 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 9, out ShadowCastingMode v12);
				Graphics.DrawMesh(mesh17, val15, val16, material3, layer3, camera3, submeshIndex3, properties3, v12);
				return 0;
			}
			if (num == 10 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<Material>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8) && objectTranslator.Assignable<ShadowCastingMode>(L, 9) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 10))
			{
				Mesh mesh18 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Vector3 val17);
				objectTranslator.Get(L, 3, out Quaternion val18);
				Material material4 = (Material)objectTranslator.GetObject(L, 4, typeof(Material));
				int layer4 = Lua.xlua_tointeger(L, 5);
				Camera camera4 = (Camera)objectTranslator.GetObject(L, 6, typeof(Camera));
				int submeshIndex4 = Lua.xlua_tointeger(L, 7);
				MaterialPropertyBlock properties4 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 9, out ShadowCastingMode v13);
				Graphics.DrawMesh(receiveShadows: Lua.lua_toboolean(L, 10), mesh: mesh18, position: val17, rotation: val18, material: material4, layer: layer4, camera: camera4, submeshIndex: submeshIndex4, properties: properties4, castShadows: v13);
				return 0;
			}
			if (num == 10 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Camera>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && objectTranslator.Assignable<Transform>(L, 10))
			{
				Mesh mesh19 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v14);
				Material material5 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				int layer5 = Lua.xlua_tointeger(L, 4);
				Camera camera5 = (Camera)objectTranslator.GetObject(L, 5, typeof(Camera));
				int submeshIndex5 = Lua.xlua_tointeger(L, 6);
				MaterialPropertyBlock properties5 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v15);
				Graphics.DrawMesh(receiveShadows: Lua.lua_toboolean(L, 9), probeAnchor: (Transform)objectTranslator.GetObject(L, 10, typeof(Transform)), mesh: mesh19, matrix: v14, material: material5, layer: layer5, camera: camera5, submeshIndex: submeshIndex5, properties: properties5, castShadows: v15);
				return 0;
			}
			if (num == 11 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Camera>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && objectTranslator.Assignable<Transform>(L, 10) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 11))
			{
				Mesh mesh20 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v16);
				Material material6 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				int layer6 = Lua.xlua_tointeger(L, 4);
				Camera camera6 = (Camera)objectTranslator.GetObject(L, 5, typeof(Camera));
				int submeshIndex6 = Lua.xlua_tointeger(L, 6);
				MaterialPropertyBlock properties6 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v17);
				Graphics.DrawMesh(receiveShadows: Lua.lua_toboolean(L, 9), probeAnchor: (Transform)objectTranslator.GetObject(L, 10, typeof(Transform)), useLightProbes: Lua.lua_toboolean(L, 11), mesh: mesh20, matrix: v16, material: material6, layer: layer6, camera: camera6, submeshIndex: submeshIndex6, properties: properties6, castShadows: v17);
				return 0;
			}
			if (num == 11 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<Material>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8) && objectTranslator.Assignable<ShadowCastingMode>(L, 9) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 10) && objectTranslator.Assignable<Transform>(L, 11))
			{
				Mesh mesh21 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Vector3 val19);
				objectTranslator.Get(L, 3, out Quaternion val20);
				Material material7 = (Material)objectTranslator.GetObject(L, 4, typeof(Material));
				int layer7 = Lua.xlua_tointeger(L, 5);
				Camera camera7 = (Camera)objectTranslator.GetObject(L, 6, typeof(Camera));
				int submeshIndex7 = Lua.xlua_tointeger(L, 7);
				MaterialPropertyBlock properties7 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 9, out ShadowCastingMode v18);
				Graphics.DrawMesh(receiveShadows: Lua.lua_toboolean(L, 10), probeAnchor: (Transform)objectTranslator.GetObject(L, 11, typeof(Transform)), mesh: mesh21, position: val19, rotation: val20, material: material7, layer: layer7, camera: camera7, submeshIndex: submeshIndex7, properties: properties7, castShadows: v18);
				return 0;
			}
			if (num == 11 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Camera>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && objectTranslator.Assignable<Transform>(L, 10) && objectTranslator.Assignable<LightProbeUsage>(L, 11))
			{
				Mesh mesh22 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v19);
				Material material8 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				int layer8 = Lua.xlua_tointeger(L, 4);
				Camera camera8 = (Camera)objectTranslator.GetObject(L, 5, typeof(Camera));
				int submeshIndex8 = Lua.xlua_tointeger(L, 6);
				MaterialPropertyBlock properties8 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v20);
				bool receiveShadows = Lua.lua_toboolean(L, 9);
				Transform probeAnchor = (Transform)objectTranslator.GetObject(L, 10, typeof(Transform));
				objectTranslator.Get(L, 11, out LightProbeUsage v21);
				Graphics.DrawMesh(mesh22, v19, material8, layer8, camera8, submeshIndex8, properties8, v20, receiveShadows, probeAnchor, v21);
				return 0;
			}
			if (num == 12 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<Material>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8) && objectTranslator.Assignable<ShadowCastingMode>(L, 9) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 10) && objectTranslator.Assignable<Transform>(L, 11) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 12))
			{
				Mesh mesh23 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Vector3 val21);
				objectTranslator.Get(L, 3, out Quaternion val22);
				Material material9 = (Material)objectTranslator.GetObject(L, 4, typeof(Material));
				int layer9 = Lua.xlua_tointeger(L, 5);
				Camera camera9 = (Camera)objectTranslator.GetObject(L, 6, typeof(Camera));
				int submeshIndex9 = Lua.xlua_tointeger(L, 7);
				MaterialPropertyBlock properties9 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 9, out ShadowCastingMode v22);
				Graphics.DrawMesh(receiveShadows: Lua.lua_toboolean(L, 10), probeAnchor: (Transform)objectTranslator.GetObject(L, 11, typeof(Transform)), useLightProbes: Lua.lua_toboolean(L, 12), mesh: mesh23, position: val21, rotation: val22, material: material9, layer: layer9, camera: camera9, submeshIndex: submeshIndex9, properties: properties9, castShadows: v22);
				return 0;
			}
			if (num == 12 && objectTranslator.Assignable<Mesh>(L, 1) && objectTranslator.Assignable<Matrix4x4>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Camera>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && objectTranslator.Assignable<Transform>(L, 10) && objectTranslator.Assignable<LightProbeUsage>(L, 11) && objectTranslator.Assignable<LightProbeProxyVolume>(L, 12))
			{
				Mesh mesh24 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				objectTranslator.Get(L, 2, out Matrix4x4 v23);
				Material material10 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				int layer10 = Lua.xlua_tointeger(L, 4);
				Camera camera10 = (Camera)objectTranslator.GetObject(L, 5, typeof(Camera));
				int submeshIndex10 = Lua.xlua_tointeger(L, 6);
				MaterialPropertyBlock properties10 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v24);
				bool receiveShadows2 = Lua.lua_toboolean(L, 9);
				Transform probeAnchor2 = (Transform)objectTranslator.GetObject(L, 10, typeof(Transform));
				objectTranslator.Get(L, 11, out LightProbeUsage v25);
				Graphics.DrawMesh(lightProbeProxyVolume: (LightProbeProxyVolume)objectTranslator.GetObject(L, 12, typeof(LightProbeProxyVolume)), mesh: mesh24, matrix: v23, material: material10, layer: layer10, camera: camera10, submeshIndex: submeshIndex10, properties: properties10, castShadows: v24, receiveShadows: receiveShadows2, probeAnchor: probeAnchor2, lightProbeUsage: v25);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.DrawMesh!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DrawMeshInstanced_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Matrix4x4[]>(L, 4))
			{
				Mesh mesh = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex = Lua.xlua_tointeger(L, 2);
				Material material = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				Matrix4x4[] matrices = (Matrix4x4[])objectTranslator.GetObject(L, 4, typeof(Matrix4x4[]));
				Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<List<Matrix4x4>>(L, 4))
			{
				Mesh mesh2 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex2 = Lua.xlua_tointeger(L, 2);
				Material material2 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				List<Matrix4x4> matrices2 = (List<Matrix4x4>)objectTranslator.GetObject(L, 4, typeof(List<Matrix4x4>));
				Graphics.DrawMeshInstanced(mesh2, submeshIndex2, material2, matrices2);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Matrix4x4[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				Mesh mesh3 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex3 = Lua.xlua_tointeger(L, 2);
				Material material3 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				Matrix4x4[] matrices3 = (Matrix4x4[])objectTranslator.GetObject(L, 4, typeof(Matrix4x4[]));
				int count = Lua.xlua_tointeger(L, 5);
				Graphics.DrawMeshInstanced(mesh3, submeshIndex3, material3, matrices3, count);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<List<Matrix4x4>>(L, 4) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 5))
			{
				Mesh mesh4 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex4 = Lua.xlua_tointeger(L, 2);
				Material material4 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				List<Matrix4x4> matrices4 = (List<Matrix4x4>)objectTranslator.GetObject(L, 4, typeof(List<Matrix4x4>));
				MaterialPropertyBlock properties = (MaterialPropertyBlock)objectTranslator.GetObject(L, 5, typeof(MaterialPropertyBlock));
				Graphics.DrawMeshInstanced(mesh4, submeshIndex4, material4, matrices4, properties);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Matrix4x4[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6))
			{
				Mesh mesh5 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex5 = Lua.xlua_tointeger(L, 2);
				Material material5 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				Matrix4x4[] matrices5 = (Matrix4x4[])objectTranslator.GetObject(L, 4, typeof(Matrix4x4[]));
				int count2 = Lua.xlua_tointeger(L, 5);
				MaterialPropertyBlock properties2 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock));
				Graphics.DrawMeshInstanced(mesh5, submeshIndex5, material5, matrices5, count2, properties2);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<List<Matrix4x4>>(L, 4) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 5) && objectTranslator.Assignable<ShadowCastingMode>(L, 6))
			{
				Mesh mesh6 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex6 = Lua.xlua_tointeger(L, 2);
				Material material6 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				List<Matrix4x4> matrices6 = (List<Matrix4x4>)objectTranslator.GetObject(L, 4, typeof(List<Matrix4x4>));
				MaterialPropertyBlock properties3 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 5, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 6, out ShadowCastingMode v);
				Graphics.DrawMeshInstanced(mesh6, submeshIndex6, material6, matrices6, properties3, v);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Matrix4x4[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6) && objectTranslator.Assignable<ShadowCastingMode>(L, 7))
			{
				Mesh mesh7 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex7 = Lua.xlua_tointeger(L, 2);
				Material material7 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				Matrix4x4[] matrices7 = (Matrix4x4[])objectTranslator.GetObject(L, 4, typeof(Matrix4x4[]));
				int count3 = Lua.xlua_tointeger(L, 5);
				MaterialPropertyBlock properties4 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 7, out ShadowCastingMode v2);
				Graphics.DrawMeshInstanced(mesh7, submeshIndex7, material7, matrices7, count3, properties4, v2);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<List<Matrix4x4>>(L, 4) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 5) && objectTranslator.Assignable<ShadowCastingMode>(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				Mesh mesh8 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex8 = Lua.xlua_tointeger(L, 2);
				Material material8 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				List<Matrix4x4> matrices8 = (List<Matrix4x4>)objectTranslator.GetObject(L, 4, typeof(List<Matrix4x4>));
				MaterialPropertyBlock properties5 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 5, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 6, out ShadowCastingMode v3);
				Graphics.DrawMeshInstanced(receiveShadows: Lua.lua_toboolean(L, 7), mesh: mesh8, submeshIndex: submeshIndex8, material: material8, matrices: matrices8, properties: properties5, castShadows: v3);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Matrix4x4[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6) && objectTranslator.Assignable<ShadowCastingMode>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				Mesh mesh9 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex9 = Lua.xlua_tointeger(L, 2);
				Material material9 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				Matrix4x4[] matrices9 = (Matrix4x4[])objectTranslator.GetObject(L, 4, typeof(Matrix4x4[]));
				int count4 = Lua.xlua_tointeger(L, 5);
				MaterialPropertyBlock properties6 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 7, out ShadowCastingMode v4);
				Graphics.DrawMeshInstanced(receiveShadows: Lua.lua_toboolean(L, 8), mesh: mesh9, submeshIndex: submeshIndex9, material: material9, matrices: matrices9, count: count4, properties: properties6, castShadows: v4);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<List<Matrix4x4>>(L, 4) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 5) && objectTranslator.Assignable<ShadowCastingMode>(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				Mesh mesh10 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex10 = Lua.xlua_tointeger(L, 2);
				Material material10 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				List<Matrix4x4> matrices10 = (List<Matrix4x4>)objectTranslator.GetObject(L, 4, typeof(List<Matrix4x4>));
				MaterialPropertyBlock properties7 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 5, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 6, out ShadowCastingMode v5);
				Graphics.DrawMeshInstanced(receiveShadows: Lua.lua_toboolean(L, 7), layer: Lua.xlua_tointeger(L, 8), mesh: mesh10, submeshIndex: submeshIndex10, material: material10, matrices: matrices10, properties: properties7, castShadows: v5);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Matrix4x4[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6) && objectTranslator.Assignable<ShadowCastingMode>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9))
			{
				Mesh mesh11 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex11 = Lua.xlua_tointeger(L, 2);
				Material material11 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				Matrix4x4[] matrices11 = (Matrix4x4[])objectTranslator.GetObject(L, 4, typeof(Matrix4x4[]));
				int count5 = Lua.xlua_tointeger(L, 5);
				MaterialPropertyBlock properties8 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 7, out ShadowCastingMode v6);
				Graphics.DrawMeshInstanced(receiveShadows: Lua.lua_toboolean(L, 8), layer: Lua.xlua_tointeger(L, 9), mesh: mesh11, submeshIndex: submeshIndex11, material: material11, matrices: matrices11, count: count5, properties: properties8, castShadows: v6);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<List<Matrix4x4>>(L, 4) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 5) && objectTranslator.Assignable<ShadowCastingMode>(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && objectTranslator.Assignable<Camera>(L, 9))
			{
				Mesh mesh12 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex12 = Lua.xlua_tointeger(L, 2);
				Material material12 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				List<Matrix4x4> matrices12 = (List<Matrix4x4>)objectTranslator.GetObject(L, 4, typeof(List<Matrix4x4>));
				MaterialPropertyBlock properties9 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 5, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 6, out ShadowCastingMode v7);
				Graphics.DrawMeshInstanced(receiveShadows: Lua.lua_toboolean(L, 7), layer: Lua.xlua_tointeger(L, 8), camera: (Camera)objectTranslator.GetObject(L, 9, typeof(Camera)), mesh: mesh12, submeshIndex: submeshIndex12, material: material12, matrices: matrices12, properties: properties9, castShadows: v7);
				return 0;
			}
			if (num == 10 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Matrix4x4[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6) && objectTranslator.Assignable<ShadowCastingMode>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && objectTranslator.Assignable<Camera>(L, 10))
			{
				Mesh mesh13 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex13 = Lua.xlua_tointeger(L, 2);
				Material material13 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				Matrix4x4[] matrices13 = (Matrix4x4[])objectTranslator.GetObject(L, 4, typeof(Matrix4x4[]));
				int count6 = Lua.xlua_tointeger(L, 5);
				MaterialPropertyBlock properties10 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 7, out ShadowCastingMode v8);
				Graphics.DrawMeshInstanced(receiveShadows: Lua.lua_toboolean(L, 8), layer: Lua.xlua_tointeger(L, 9), camera: (Camera)objectTranslator.GetObject(L, 10, typeof(Camera)), mesh: mesh13, submeshIndex: submeshIndex13, material: material13, matrices: matrices13, count: count6, properties: properties10, castShadows: v8);
				return 0;
			}
			if (num == 10 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<List<Matrix4x4>>(L, 4) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 5) && objectTranslator.Assignable<ShadowCastingMode>(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && objectTranslator.Assignable<Camera>(L, 9) && objectTranslator.Assignable<LightProbeUsage>(L, 10))
			{
				Mesh mesh14 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex14 = Lua.xlua_tointeger(L, 2);
				Material material14 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				List<Matrix4x4> matrices14 = (List<Matrix4x4>)objectTranslator.GetObject(L, 4, typeof(List<Matrix4x4>));
				MaterialPropertyBlock properties11 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 5, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 6, out ShadowCastingMode v9);
				bool receiveShadows = Lua.lua_toboolean(L, 7);
				int layer = Lua.xlua_tointeger(L, 8);
				Camera camera = (Camera)objectTranslator.GetObject(L, 9, typeof(Camera));
				objectTranslator.Get(L, 10, out LightProbeUsage v10);
				Graphics.DrawMeshInstanced(mesh14, submeshIndex14, material14, matrices14, properties11, v9, receiveShadows, layer, camera, v10);
				return 0;
			}
			if (num == 11 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Matrix4x4[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6) && objectTranslator.Assignable<ShadowCastingMode>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && objectTranslator.Assignable<Camera>(L, 10) && objectTranslator.Assignable<LightProbeUsage>(L, 11))
			{
				Mesh mesh15 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex15 = Lua.xlua_tointeger(L, 2);
				Material material15 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				Matrix4x4[] matrices15 = (Matrix4x4[])objectTranslator.GetObject(L, 4, typeof(Matrix4x4[]));
				int count7 = Lua.xlua_tointeger(L, 5);
				MaterialPropertyBlock properties12 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 7, out ShadowCastingMode v11);
				bool receiveShadows2 = Lua.lua_toboolean(L, 8);
				int layer2 = Lua.xlua_tointeger(L, 9);
				Camera camera2 = (Camera)objectTranslator.GetObject(L, 10, typeof(Camera));
				objectTranslator.Get(L, 11, out LightProbeUsage v12);
				Graphics.DrawMeshInstanced(mesh15, submeshIndex15, material15, matrices15, count7, properties12, v11, receiveShadows2, layer2, camera2, v12);
				return 0;
			}
			if (num == 11 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<List<Matrix4x4>>(L, 4) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 5) && objectTranslator.Assignable<ShadowCastingMode>(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && objectTranslator.Assignable<Camera>(L, 9) && objectTranslator.Assignable<LightProbeUsage>(L, 10) && objectTranslator.Assignable<LightProbeProxyVolume>(L, 11))
			{
				Mesh mesh16 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex16 = Lua.xlua_tointeger(L, 2);
				Material material16 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				List<Matrix4x4> matrices16 = (List<Matrix4x4>)objectTranslator.GetObject(L, 4, typeof(List<Matrix4x4>));
				MaterialPropertyBlock properties13 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 5, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 6, out ShadowCastingMode v13);
				bool receiveShadows3 = Lua.lua_toboolean(L, 7);
				int layer3 = Lua.xlua_tointeger(L, 8);
				Camera camera3 = (Camera)objectTranslator.GetObject(L, 9, typeof(Camera));
				objectTranslator.Get(L, 10, out LightProbeUsage v14);
				Graphics.DrawMeshInstanced(lightProbeProxyVolume: (LightProbeProxyVolume)objectTranslator.GetObject(L, 11, typeof(LightProbeProxyVolume)), mesh: mesh16, submeshIndex: submeshIndex16, material: material16, matrices: matrices16, properties: properties13, castShadows: v13, receiveShadows: receiveShadows3, layer: layer3, camera: camera3, lightProbeUsage: v14);
				return 0;
			}
			if (num == 12 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Matrix4x4[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6) && objectTranslator.Assignable<ShadowCastingMode>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && objectTranslator.Assignable<Camera>(L, 10) && objectTranslator.Assignable<LightProbeUsage>(L, 11) && objectTranslator.Assignable<LightProbeProxyVolume>(L, 12))
			{
				Mesh mesh17 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex17 = Lua.xlua_tointeger(L, 2);
				Material material17 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				Matrix4x4[] matrices17 = (Matrix4x4[])objectTranslator.GetObject(L, 4, typeof(Matrix4x4[]));
				int count8 = Lua.xlua_tointeger(L, 5);
				MaterialPropertyBlock properties14 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 7, out ShadowCastingMode v15);
				bool receiveShadows4 = Lua.lua_toboolean(L, 8);
				int layer4 = Lua.xlua_tointeger(L, 9);
				Camera camera4 = (Camera)objectTranslator.GetObject(L, 10, typeof(Camera));
				objectTranslator.Get(L, 11, out LightProbeUsage v16);
				Graphics.DrawMeshInstanced(lightProbeProxyVolume: (LightProbeProxyVolume)objectTranslator.GetObject(L, 12, typeof(LightProbeProxyVolume)), mesh: mesh17, submeshIndex: submeshIndex17, material: material17, matrices: matrices17, count: count8, properties: properties14, castShadows: v15, receiveShadows: receiveShadows4, layer: layer4, camera: camera4, lightProbeUsage: v16);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.DrawMeshInstanced!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DrawMeshInstancedProcedural_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 12 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6) && objectTranslator.Assignable<ShadowCastingMode>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && objectTranslator.Assignable<Camera>(L, 10) && objectTranslator.Assignable<LightProbeUsage>(L, 11) && objectTranslator.Assignable<LightProbeProxyVolume>(L, 12))
			{
				Mesh mesh = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex = Lua.xlua_tointeger(L, 2);
				Material material = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val);
				int count = Lua.xlua_tointeger(L, 5);
				MaterialPropertyBlock properties = (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 7, out ShadowCastingMode v);
				bool receiveShadows = Lua.lua_toboolean(L, 8);
				int layer = Lua.xlua_tointeger(L, 9);
				Camera camera = (Camera)objectTranslator.GetObject(L, 10, typeof(Camera));
				objectTranslator.Get(L, 11, out LightProbeUsage v2);
				Graphics.DrawMeshInstancedProcedural(lightProbeProxyVolume: (LightProbeProxyVolume)objectTranslator.GetObject(L, 12, typeof(LightProbeProxyVolume)), mesh: mesh, submeshIndex: submeshIndex, material: material, bounds: val, count: count, properties: properties, castShadows: v, receiveShadows: receiveShadows, layer: layer, camera: camera, lightProbeUsage: v2);
				return 0;
			}
			if (num == 11 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6) && objectTranslator.Assignable<ShadowCastingMode>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && objectTranslator.Assignable<Camera>(L, 10) && objectTranslator.Assignable<LightProbeUsage>(L, 11))
			{
				Mesh mesh2 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex2 = Lua.xlua_tointeger(L, 2);
				Material material2 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val2);
				int count2 = Lua.xlua_tointeger(L, 5);
				MaterialPropertyBlock properties2 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 7, out ShadowCastingMode v3);
				bool receiveShadows2 = Lua.lua_toboolean(L, 8);
				int layer2 = Lua.xlua_tointeger(L, 9);
				Camera camera2 = (Camera)objectTranslator.GetObject(L, 10, typeof(Camera));
				objectTranslator.Get(L, 11, out LightProbeUsage v4);
				Graphics.DrawMeshInstancedProcedural(mesh2, submeshIndex2, material2, val2, count2, properties2, v3, receiveShadows2, layer2, camera2, v4);
				return 0;
			}
			if (num == 10 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6) && objectTranslator.Assignable<ShadowCastingMode>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && objectTranslator.Assignable<Camera>(L, 10))
			{
				Mesh mesh3 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex3 = Lua.xlua_tointeger(L, 2);
				Material material3 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val3);
				int count3 = Lua.xlua_tointeger(L, 5);
				MaterialPropertyBlock properties3 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 7, out ShadowCastingMode v5);
				Graphics.DrawMeshInstancedProcedural(receiveShadows: Lua.lua_toboolean(L, 8), layer: Lua.xlua_tointeger(L, 9), camera: (Camera)objectTranslator.GetObject(L, 10, typeof(Camera)), mesh: mesh3, submeshIndex: submeshIndex3, material: material3, bounds: val3, count: count3, properties: properties3, castShadows: v5);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6) && objectTranslator.Assignable<ShadowCastingMode>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9))
			{
				Mesh mesh4 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex4 = Lua.xlua_tointeger(L, 2);
				Material material4 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val4);
				int count4 = Lua.xlua_tointeger(L, 5);
				MaterialPropertyBlock properties4 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 7, out ShadowCastingMode v6);
				Graphics.DrawMeshInstancedProcedural(receiveShadows: Lua.lua_toboolean(L, 8), layer: Lua.xlua_tointeger(L, 9), mesh: mesh4, submeshIndex: submeshIndex4, material: material4, bounds: val4, count: count4, properties: properties4, castShadows: v6);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6) && objectTranslator.Assignable<ShadowCastingMode>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				Mesh mesh5 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex5 = Lua.xlua_tointeger(L, 2);
				Material material5 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val5);
				int count5 = Lua.xlua_tointeger(L, 5);
				MaterialPropertyBlock properties5 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 7, out ShadowCastingMode v7);
				Graphics.DrawMeshInstancedProcedural(receiveShadows: Lua.lua_toboolean(L, 8), mesh: mesh5, submeshIndex: submeshIndex5, material: material5, bounds: val5, count: count5, properties: properties5, castShadows: v7);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6) && objectTranslator.Assignable<ShadowCastingMode>(L, 7))
			{
				Mesh mesh6 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex6 = Lua.xlua_tointeger(L, 2);
				Material material6 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val6);
				int count6 = Lua.xlua_tointeger(L, 5);
				MaterialPropertyBlock properties6 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 7, out ShadowCastingMode v8);
				Graphics.DrawMeshInstancedProcedural(mesh6, submeshIndex6, material6, val6, count6, properties6, v8);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 6))
			{
				Mesh mesh7 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex7 = Lua.xlua_tointeger(L, 2);
				Material material7 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val7);
				Graphics.DrawMeshInstancedProcedural(count: Lua.xlua_tointeger(L, 5), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 6, typeof(MaterialPropertyBlock)), mesh: mesh7, submeshIndex: submeshIndex7, material: material7, bounds: val7);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				Mesh mesh8 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex8 = Lua.xlua_tointeger(L, 2);
				Material material8 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val8);
				Graphics.DrawMeshInstancedProcedural(count: Lua.xlua_tointeger(L, 5), mesh: mesh8, submeshIndex: submeshIndex8, material: material8, bounds: val8);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.DrawMeshInstancedProcedural!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DrawMeshInstancedIndirect_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5))
			{
				Mesh mesh = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex = Lua.xlua_tointeger(L, 2);
				Material material = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val);
				Graphics.DrawMeshInstancedIndirect(bufferWithArgs: (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer)), mesh: mesh, submeshIndex: submeshIndex, material: material, bounds: val);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				Mesh mesh2 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex2 = Lua.xlua_tointeger(L, 2);
				Material material2 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val2);
				Graphics.DrawMeshInstancedIndirect(bufferWithArgs: (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer)), argsOffset: Lua.xlua_tointeger(L, 6), mesh: mesh2, submeshIndex: submeshIndex2, material: material2, bounds: val2);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7))
			{
				Mesh mesh3 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex3 = Lua.xlua_tointeger(L, 2);
				Material material3 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val3);
				Graphics.DrawMeshInstancedIndirect(bufferWithArgs: (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer)), argsOffset: Lua.xlua_tointeger(L, 6), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock)), mesh: mesh3, submeshIndex: submeshIndex3, material: material3, bounds: val3);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8))
			{
				Mesh mesh4 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex4 = Lua.xlua_tointeger(L, 2);
				Material material4 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val4);
				ComputeBuffer bufferWithArgs = (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer));
				int argsOffset = Lua.xlua_tointeger(L, 6);
				MaterialPropertyBlock properties = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v);
				Graphics.DrawMeshInstancedIndirect(mesh4, submeshIndex4, material4, val4, bufferWithArgs, argsOffset, properties, v);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9))
			{
				Mesh mesh5 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex5 = Lua.xlua_tointeger(L, 2);
				Material material5 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val5);
				ComputeBuffer bufferWithArgs2 = (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer));
				int argsOffset2 = Lua.xlua_tointeger(L, 6);
				MaterialPropertyBlock properties2 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v2);
				Graphics.DrawMeshInstancedIndirect(receiveShadows: Lua.lua_toboolean(L, 9), mesh: mesh5, submeshIndex: submeshIndex5, material: material5, bounds: val5, bufferWithArgs: bufferWithArgs2, argsOffset: argsOffset2, properties: properties2, castShadows: v2);
				return 0;
			}
			if (num == 10 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10))
			{
				Mesh mesh6 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex6 = Lua.xlua_tointeger(L, 2);
				Material material6 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val6);
				ComputeBuffer bufferWithArgs3 = (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer));
				int argsOffset3 = Lua.xlua_tointeger(L, 6);
				MaterialPropertyBlock properties3 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v3);
				Graphics.DrawMeshInstancedIndirect(receiveShadows: Lua.lua_toboolean(L, 9), layer: Lua.xlua_tointeger(L, 10), mesh: mesh6, submeshIndex: submeshIndex6, material: material6, bounds: val6, bufferWithArgs: bufferWithArgs3, argsOffset: argsOffset3, properties: properties3, castShadows: v3);
				return 0;
			}
			if (num == 11 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && objectTranslator.Assignable<Camera>(L, 11))
			{
				Mesh mesh7 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex7 = Lua.xlua_tointeger(L, 2);
				Material material7 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val7);
				ComputeBuffer bufferWithArgs4 = (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer));
				int argsOffset4 = Lua.xlua_tointeger(L, 6);
				MaterialPropertyBlock properties4 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v4);
				Graphics.DrawMeshInstancedIndirect(receiveShadows: Lua.lua_toboolean(L, 9), layer: Lua.xlua_tointeger(L, 10), camera: (Camera)objectTranslator.GetObject(L, 11, typeof(Camera)), mesh: mesh7, submeshIndex: submeshIndex7, material: material7, bounds: val7, bufferWithArgs: bufferWithArgs4, argsOffset: argsOffset4, properties: properties4, castShadows: v4);
				return 0;
			}
			if (num == 12 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && objectTranslator.Assignable<Camera>(L, 11) && objectTranslator.Assignable<LightProbeUsage>(L, 12))
			{
				Mesh mesh8 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex8 = Lua.xlua_tointeger(L, 2);
				Material material8 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val8);
				ComputeBuffer bufferWithArgs5 = (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer));
				int argsOffset5 = Lua.xlua_tointeger(L, 6);
				MaterialPropertyBlock properties5 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v5);
				bool receiveShadows = Lua.lua_toboolean(L, 9);
				int layer = Lua.xlua_tointeger(L, 10);
				Camera camera = (Camera)objectTranslator.GetObject(L, 11, typeof(Camera));
				objectTranslator.Get(L, 12, out LightProbeUsage v6);
				Graphics.DrawMeshInstancedIndirect(mesh8, submeshIndex8, material8, val8, bufferWithArgs5, argsOffset5, properties5, v5, receiveShadows, layer, camera, v6);
				return 0;
			}
			if (num == 13 && objectTranslator.Assignable<Mesh>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Material>(L, 3) && objectTranslator.Assignable<Bounds>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && objectTranslator.Assignable<Camera>(L, 11) && objectTranslator.Assignable<LightProbeUsage>(L, 12) && objectTranslator.Assignable<LightProbeProxyVolume>(L, 13))
			{
				Mesh mesh9 = (Mesh)objectTranslator.GetObject(L, 1, typeof(Mesh));
				int submeshIndex9 = Lua.xlua_tointeger(L, 2);
				Material material9 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				objectTranslator.Get(L, 4, out Bounds val9);
				ComputeBuffer bufferWithArgs6 = (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer));
				int argsOffset6 = Lua.xlua_tointeger(L, 6);
				MaterialPropertyBlock properties6 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v7);
				bool receiveShadows2 = Lua.lua_toboolean(L, 9);
				int layer2 = Lua.xlua_tointeger(L, 10);
				Camera camera2 = (Camera)objectTranslator.GetObject(L, 11, typeof(Camera));
				objectTranslator.Get(L, 12, out LightProbeUsage v8);
				Graphics.DrawMeshInstancedIndirect(lightProbeProxyVolume: (LightProbeProxyVolume)objectTranslator.GetObject(L, 13, typeof(LightProbeProxyVolume)), mesh: mesh9, submeshIndex: submeshIndex9, material: material9, bounds: val9, bufferWithArgs: bufferWithArgs6, argsOffset: argsOffset6, properties: properties6, castShadows: v7, receiveShadows: receiveShadows2, layer: layer2, camera: camera2, lightProbeUsage: v8);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.DrawMeshInstancedIndirect!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DrawProceduralNow_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<MeshTopology>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out MeshTopology v);
				int vertexCount = Lua.xlua_tointeger(L, 2);
				int instanceCount = Lua.xlua_tointeger(L, 3);
				Graphics.DrawProceduralNow(v, vertexCount, instanceCount);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<MeshTopology>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out MeshTopology v2);
				int vertexCount2 = Lua.xlua_tointeger(L, 2);
				Graphics.DrawProceduralNow(v2, vertexCount2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<MeshTopology>(L, 1) && objectTranslator.Assignable<GraphicsBuffer>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out MeshTopology v3);
				GraphicsBuffer indexBuffer = (GraphicsBuffer)objectTranslator.GetObject(L, 2, typeof(GraphicsBuffer));
				int indexCount = Lua.xlua_tointeger(L, 3);
				int instanceCount2 = Lua.xlua_tointeger(L, 4);
				Graphics.DrawProceduralNow(v3, indexBuffer, indexCount, instanceCount2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<MeshTopology>(L, 1) && objectTranslator.Assignable<GraphicsBuffer>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out MeshTopology v4);
				GraphicsBuffer indexBuffer2 = (GraphicsBuffer)objectTranslator.GetObject(L, 2, typeof(GraphicsBuffer));
				int indexCount2 = Lua.xlua_tointeger(L, 3);
				Graphics.DrawProceduralNow(v4, indexBuffer2, indexCount2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.DrawProceduralNow!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DrawProceduralIndirectNow_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<MeshTopology>(L, 1) && objectTranslator.Assignable<ComputeBuffer>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out MeshTopology v);
				ComputeBuffer bufferWithArgs = (ComputeBuffer)objectTranslator.GetObject(L, 2, typeof(ComputeBuffer));
				int argsOffset = Lua.xlua_tointeger(L, 3);
				Graphics.DrawProceduralIndirectNow(v, bufferWithArgs, argsOffset);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<MeshTopology>(L, 1) && objectTranslator.Assignable<ComputeBuffer>(L, 2))
			{
				objectTranslator.Get(L, 1, out MeshTopology v2);
				ComputeBuffer bufferWithArgs2 = (ComputeBuffer)objectTranslator.GetObject(L, 2, typeof(ComputeBuffer));
				Graphics.DrawProceduralIndirectNow(v2, bufferWithArgs2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<MeshTopology>(L, 1) && objectTranslator.Assignable<GraphicsBuffer>(L, 2) && objectTranslator.Assignable<ComputeBuffer>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out MeshTopology v3);
				GraphicsBuffer indexBuffer = (GraphicsBuffer)objectTranslator.GetObject(L, 2, typeof(GraphicsBuffer));
				ComputeBuffer bufferWithArgs3 = (ComputeBuffer)objectTranslator.GetObject(L, 3, typeof(ComputeBuffer));
				int argsOffset2 = Lua.xlua_tointeger(L, 4);
				Graphics.DrawProceduralIndirectNow(v3, indexBuffer, bufferWithArgs3, argsOffset2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<MeshTopology>(L, 1) && objectTranslator.Assignable<GraphicsBuffer>(L, 2) && objectTranslator.Assignable<ComputeBuffer>(L, 3))
			{
				objectTranslator.Get(L, 1, out MeshTopology v4);
				GraphicsBuffer indexBuffer2 = (GraphicsBuffer)objectTranslator.GetObject(L, 2, typeof(GraphicsBuffer));
				ComputeBuffer bufferWithArgs4 = (ComputeBuffer)objectTranslator.GetObject(L, 3, typeof(ComputeBuffer));
				Graphics.DrawProceduralIndirectNow(v4, indexBuffer2, bufferWithArgs4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.DrawProceduralIndirectNow!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DrawProcedural_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 10 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10))
			{
				Material material = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val);
				objectTranslator.Get(L, 3, out MeshTopology v);
				int vertexCount = Lua.xlua_tointeger(L, 4);
				int instanceCount = Lua.xlua_tointeger(L, 5);
				Camera camera = (Camera)objectTranslator.GetObject(L, 6, typeof(Camera));
				MaterialPropertyBlock properties = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v2);
				Graphics.DrawProcedural(receiveShadows: Lua.lua_toboolean(L, 9), layer: Lua.xlua_tointeger(L, 10), material: material, bounds: val, topology: v, vertexCount: vertexCount, instanceCount: instanceCount, camera: camera, properties: properties, castShadows: v2);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9))
			{
				Material material2 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val2);
				objectTranslator.Get(L, 3, out MeshTopology v3);
				int vertexCount2 = Lua.xlua_tointeger(L, 4);
				int instanceCount2 = Lua.xlua_tointeger(L, 5);
				Camera camera2 = (Camera)objectTranslator.GetObject(L, 6, typeof(Camera));
				MaterialPropertyBlock properties2 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v4);
				Graphics.DrawProcedural(receiveShadows: Lua.lua_toboolean(L, 9), material: material2, bounds: val2, topology: v3, vertexCount: vertexCount2, instanceCount: instanceCount2, camera: camera2, properties: properties2, castShadows: v4);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8))
			{
				Material material3 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val3);
				objectTranslator.Get(L, 3, out MeshTopology v5);
				int vertexCount3 = Lua.xlua_tointeger(L, 4);
				int instanceCount3 = Lua.xlua_tointeger(L, 5);
				Camera camera3 = (Camera)objectTranslator.GetObject(L, 6, typeof(Camera));
				MaterialPropertyBlock properties3 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v6);
				Graphics.DrawProcedural(material3, val3, v5, vertexCount3, instanceCount3, camera3, properties3, v6);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7))
			{
				Material material4 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val4);
				objectTranslator.Get(L, 3, out MeshTopology v7);
				Graphics.DrawProcedural(vertexCount: Lua.xlua_tointeger(L, 4), instanceCount: Lua.xlua_tointeger(L, 5), camera: (Camera)objectTranslator.GetObject(L, 6, typeof(Camera)), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock)), material: material4, bounds: val4, topology: v7);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6))
			{
				Material material5 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val5);
				objectTranslator.Get(L, 3, out MeshTopology v8);
				Graphics.DrawProcedural(vertexCount: Lua.xlua_tointeger(L, 4), instanceCount: Lua.xlua_tointeger(L, 5), camera: (Camera)objectTranslator.GetObject(L, 6, typeof(Camera)), material: material5, bounds: val5, topology: v8);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				Material material6 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val6);
				objectTranslator.Get(L, 3, out MeshTopology v9);
				Graphics.DrawProcedural(vertexCount: Lua.xlua_tointeger(L, 4), instanceCount: Lua.xlua_tointeger(L, 5), material: material6, bounds: val6, topology: v9);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Material material7 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val7);
				objectTranslator.Get(L, 3, out MeshTopology v10);
				Graphics.DrawProcedural(vertexCount: Lua.xlua_tointeger(L, 4), material: material7, bounds: val7, topology: v10);
				return 0;
			}
			if (num == 11 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Camera>(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8) && objectTranslator.Assignable<ShadowCastingMode>(L, 9) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11))
			{
				Material material8 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val8);
				objectTranslator.Get(L, 3, out MeshTopology v11);
				GraphicsBuffer indexBuffer = (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer));
				int indexCount = Lua.xlua_tointeger(L, 5);
				int instanceCount4 = Lua.xlua_tointeger(L, 6);
				Camera camera4 = (Camera)objectTranslator.GetObject(L, 7, typeof(Camera));
				MaterialPropertyBlock properties4 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 9, out ShadowCastingMode v12);
				Graphics.DrawProcedural(receiveShadows: Lua.lua_toboolean(L, 10), layer: Lua.xlua_tointeger(L, 11), material: material8, bounds: val8, topology: v11, indexBuffer: indexBuffer, indexCount: indexCount, instanceCount: instanceCount4, camera: camera4, properties: properties4, castShadows: v12);
				return 0;
			}
			if (num == 10 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Camera>(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8) && objectTranslator.Assignable<ShadowCastingMode>(L, 9) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 10))
			{
				Material material9 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val9);
				objectTranslator.Get(L, 3, out MeshTopology v13);
				GraphicsBuffer indexBuffer2 = (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer));
				int indexCount2 = Lua.xlua_tointeger(L, 5);
				int instanceCount5 = Lua.xlua_tointeger(L, 6);
				Camera camera5 = (Camera)objectTranslator.GetObject(L, 7, typeof(Camera));
				MaterialPropertyBlock properties5 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 9, out ShadowCastingMode v14);
				Graphics.DrawProcedural(receiveShadows: Lua.lua_toboolean(L, 10), material: material9, bounds: val9, topology: v13, indexBuffer: indexBuffer2, indexCount: indexCount2, instanceCount: instanceCount5, camera: camera5, properties: properties5, castShadows: v14);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Camera>(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8) && objectTranslator.Assignable<ShadowCastingMode>(L, 9))
			{
				Material material10 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val10);
				objectTranslator.Get(L, 3, out MeshTopology v15);
				GraphicsBuffer indexBuffer3 = (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer));
				int indexCount3 = Lua.xlua_tointeger(L, 5);
				int instanceCount6 = Lua.xlua_tointeger(L, 6);
				Camera camera6 = (Camera)objectTranslator.GetObject(L, 7, typeof(Camera));
				MaterialPropertyBlock properties6 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 9, out ShadowCastingMode v16);
				Graphics.DrawProcedural(material10, val10, v15, indexBuffer3, indexCount3, instanceCount6, camera6, properties6, v16);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Camera>(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8))
			{
				Material material11 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val11);
				objectTranslator.Get(L, 3, out MeshTopology v17);
				Graphics.DrawProcedural(indexBuffer: (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer)), indexCount: Lua.xlua_tointeger(L, 5), instanceCount: Lua.xlua_tointeger(L, 6), camera: (Camera)objectTranslator.GetObject(L, 7, typeof(Camera)), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock)), material: material11, bounds: val11, topology: v17);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Camera>(L, 7))
			{
				Material material12 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val12);
				objectTranslator.Get(L, 3, out MeshTopology v18);
				Graphics.DrawProcedural(indexBuffer: (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer)), indexCount: Lua.xlua_tointeger(L, 5), instanceCount: Lua.xlua_tointeger(L, 6), camera: (Camera)objectTranslator.GetObject(L, 7, typeof(Camera)), material: material12, bounds: val12, topology: v18);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				Material material13 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val13);
				objectTranslator.Get(L, 3, out MeshTopology v19);
				Graphics.DrawProcedural(indexBuffer: (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer)), indexCount: Lua.xlua_tointeger(L, 5), instanceCount: Lua.xlua_tointeger(L, 6), material: material13, bounds: val13, topology: v19);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				Material material14 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val14);
				objectTranslator.Get(L, 3, out MeshTopology v20);
				Graphics.DrawProcedural(indexBuffer: (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer)), indexCount: Lua.xlua_tointeger(L, 5), material: material14, bounds: val14, topology: v20);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.DrawProcedural!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DrawProceduralIndirect_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 10 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<ComputeBuffer>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10))
			{
				Material material = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val);
				objectTranslator.Get(L, 3, out MeshTopology v);
				ComputeBuffer bufferWithArgs = (ComputeBuffer)objectTranslator.GetObject(L, 4, typeof(ComputeBuffer));
				int argsOffset = Lua.xlua_tointeger(L, 5);
				Camera camera = (Camera)objectTranslator.GetObject(L, 6, typeof(Camera));
				MaterialPropertyBlock properties = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v2);
				Graphics.DrawProceduralIndirect(receiveShadows: Lua.lua_toboolean(L, 9), layer: Lua.xlua_tointeger(L, 10), material: material, bounds: val, topology: v, bufferWithArgs: bufferWithArgs, argsOffset: argsOffset, camera: camera, properties: properties, castShadows: v2);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<ComputeBuffer>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9))
			{
				Material material2 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val2);
				objectTranslator.Get(L, 3, out MeshTopology v3);
				ComputeBuffer bufferWithArgs2 = (ComputeBuffer)objectTranslator.GetObject(L, 4, typeof(ComputeBuffer));
				int argsOffset2 = Lua.xlua_tointeger(L, 5);
				Camera camera2 = (Camera)objectTranslator.GetObject(L, 6, typeof(Camera));
				MaterialPropertyBlock properties2 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v4);
				Graphics.DrawProceduralIndirect(receiveShadows: Lua.lua_toboolean(L, 9), material: material2, bounds: val2, topology: v3, bufferWithArgs: bufferWithArgs2, argsOffset: argsOffset2, camera: camera2, properties: properties2, castShadows: v4);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<ComputeBuffer>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7) && objectTranslator.Assignable<ShadowCastingMode>(L, 8))
			{
				Material material3 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val3);
				objectTranslator.Get(L, 3, out MeshTopology v5);
				ComputeBuffer bufferWithArgs3 = (ComputeBuffer)objectTranslator.GetObject(L, 4, typeof(ComputeBuffer));
				int argsOffset3 = Lua.xlua_tointeger(L, 5);
				Camera camera3 = (Camera)objectTranslator.GetObject(L, 6, typeof(Camera));
				MaterialPropertyBlock properties3 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 8, out ShadowCastingMode v6);
				Graphics.DrawProceduralIndirect(material3, val3, v5, bufferWithArgs3, argsOffset3, camera3, properties3, v6);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<ComputeBuffer>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 7))
			{
				Material material4 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val4);
				objectTranslator.Get(L, 3, out MeshTopology v7);
				Graphics.DrawProceduralIndirect(bufferWithArgs: (ComputeBuffer)objectTranslator.GetObject(L, 4, typeof(ComputeBuffer)), argsOffset: Lua.xlua_tointeger(L, 5), camera: (Camera)objectTranslator.GetObject(L, 6, typeof(Camera)), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 7, typeof(MaterialPropertyBlock)), material: material4, bounds: val4, topology: v7);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<ComputeBuffer>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera>(L, 6))
			{
				Material material5 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val5);
				objectTranslator.Get(L, 3, out MeshTopology v8);
				Graphics.DrawProceduralIndirect(bufferWithArgs: (ComputeBuffer)objectTranslator.GetObject(L, 4, typeof(ComputeBuffer)), argsOffset: Lua.xlua_tointeger(L, 5), camera: (Camera)objectTranslator.GetObject(L, 6, typeof(Camera)), material: material5, bounds: val5, topology: v8);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<ComputeBuffer>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				Material material6 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val6);
				objectTranslator.Get(L, 3, out MeshTopology v9);
				Graphics.DrawProceduralIndirect(bufferWithArgs: (ComputeBuffer)objectTranslator.GetObject(L, 4, typeof(ComputeBuffer)), argsOffset: Lua.xlua_tointeger(L, 5), material: material6, bounds: val6, topology: v9);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<ComputeBuffer>(L, 4))
			{
				Material material7 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val7);
				objectTranslator.Get(L, 3, out MeshTopology v10);
				Graphics.DrawProceduralIndirect(bufferWithArgs: (ComputeBuffer)objectTranslator.GetObject(L, 4, typeof(ComputeBuffer)), material: material7, bounds: val7, topology: v10);
				return 0;
			}
			if (num == 11 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Camera>(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8) && objectTranslator.Assignable<ShadowCastingMode>(L, 9) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11))
			{
				Material material8 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val8);
				objectTranslator.Get(L, 3, out MeshTopology v11);
				GraphicsBuffer indexBuffer = (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer));
				ComputeBuffer bufferWithArgs4 = (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer));
				int argsOffset4 = Lua.xlua_tointeger(L, 6);
				Camera camera4 = (Camera)objectTranslator.GetObject(L, 7, typeof(Camera));
				MaterialPropertyBlock properties4 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 9, out ShadowCastingMode v12);
				Graphics.DrawProceduralIndirect(receiveShadows: Lua.lua_toboolean(L, 10), layer: Lua.xlua_tointeger(L, 11), material: material8, bounds: val8, topology: v11, indexBuffer: indexBuffer, bufferWithArgs: bufferWithArgs4, argsOffset: argsOffset4, camera: camera4, properties: properties4, castShadows: v12);
				return 0;
			}
			if (num == 10 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Camera>(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8) && objectTranslator.Assignable<ShadowCastingMode>(L, 9) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 10))
			{
				Material material9 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val9);
				objectTranslator.Get(L, 3, out MeshTopology v13);
				GraphicsBuffer indexBuffer2 = (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer));
				ComputeBuffer bufferWithArgs5 = (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer));
				int argsOffset5 = Lua.xlua_tointeger(L, 6);
				Camera camera5 = (Camera)objectTranslator.GetObject(L, 7, typeof(Camera));
				MaterialPropertyBlock properties5 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 9, out ShadowCastingMode v14);
				Graphics.DrawProceduralIndirect(receiveShadows: Lua.lua_toboolean(L, 10), material: material9, bounds: val9, topology: v13, indexBuffer: indexBuffer2, bufferWithArgs: bufferWithArgs5, argsOffset: argsOffset5, camera: camera5, properties: properties5, castShadows: v14);
				return 0;
			}
			if (num == 9 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Camera>(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8) && objectTranslator.Assignable<ShadowCastingMode>(L, 9))
			{
				Material material10 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val10);
				objectTranslator.Get(L, 3, out MeshTopology v15);
				GraphicsBuffer indexBuffer3 = (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer));
				ComputeBuffer bufferWithArgs6 = (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer));
				int argsOffset6 = Lua.xlua_tointeger(L, 6);
				Camera camera6 = (Camera)objectTranslator.GetObject(L, 7, typeof(Camera));
				MaterialPropertyBlock properties6 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock));
				objectTranslator.Get(L, 9, out ShadowCastingMode v16);
				Graphics.DrawProceduralIndirect(material10, val10, v15, indexBuffer3, bufferWithArgs6, argsOffset6, camera6, properties6, v16);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Camera>(L, 7) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 8))
			{
				Material material11 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val11);
				objectTranslator.Get(L, 3, out MeshTopology v17);
				Graphics.DrawProceduralIndirect(indexBuffer: (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer)), bufferWithArgs: (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer)), argsOffset: Lua.xlua_tointeger(L, 6), camera: (Camera)objectTranslator.GetObject(L, 7, typeof(Camera)), properties: (MaterialPropertyBlock)objectTranslator.GetObject(L, 8, typeof(MaterialPropertyBlock)), material: material11, bounds: val11, topology: v17);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Camera>(L, 7))
			{
				Material material12 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val12);
				objectTranslator.Get(L, 3, out MeshTopology v18);
				Graphics.DrawProceduralIndirect(indexBuffer: (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer)), bufferWithArgs: (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer)), argsOffset: Lua.xlua_tointeger(L, 6), camera: (Camera)objectTranslator.GetObject(L, 7, typeof(Camera)), material: material12, bounds: val12, topology: v18);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				Material material13 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val13);
				objectTranslator.Get(L, 3, out MeshTopology v19);
				Graphics.DrawProceduralIndirect(indexBuffer: (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer)), bufferWithArgs: (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer)), argsOffset: Lua.xlua_tointeger(L, 6), material: material13, bounds: val13, topology: v19);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Material>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && objectTranslator.Assignable<GraphicsBuffer>(L, 4) && objectTranslator.Assignable<ComputeBuffer>(L, 5))
			{
				Material material14 = (Material)objectTranslator.GetObject(L, 1, typeof(Material));
				objectTranslator.Get(L, 2, out Bounds val14);
				objectTranslator.Get(L, 3, out MeshTopology v20);
				Graphics.DrawProceduralIndirect(indexBuffer: (GraphicsBuffer)objectTranslator.GetObject(L, 4, typeof(GraphicsBuffer)), bufferWithArgs: (ComputeBuffer)objectTranslator.GetObject(L, 5, typeof(ComputeBuffer)), material: material14, bounds: val14, topology: v20);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.DrawProceduralIndirect!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Blit_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<RenderTexture>(L, 2))
			{
				Texture source = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				RenderTexture dest = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				Graphics.Blit(source, dest);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<Material>(L, 2))
			{
				Texture source2 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				Material mat = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
				Graphics.Blit(source2, mat);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<Material>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Texture source3 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				Material mat2 = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
				int pass = Lua.xlua_tointeger(L, 3);
				Graphics.Blit(source3, mat2, pass);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<RenderTexture>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Texture source4 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				RenderTexture dest2 = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				int sourceDepthSlice = Lua.xlua_tointeger(L, 3);
				int destDepthSlice = Lua.xlua_tointeger(L, 4);
				Graphics.Blit(source4, dest2, sourceDepthSlice, destDepthSlice);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<Material>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Texture source5 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				Material mat3 = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
				int pass2 = Lua.xlua_tointeger(L, 3);
				int destDepthSlice2 = Lua.xlua_tointeger(L, 4);
				Graphics.Blit(source5, mat3, pass2, destDepthSlice2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<RenderTexture>(L, 2) && objectTranslator.Assignable<Material>(L, 3))
			{
				Texture source6 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				RenderTexture dest3 = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				Material mat4 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				Graphics.Blit(source6, dest3, mat4);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<RenderTexture>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Texture source7 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				RenderTexture dest4 = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				Material mat5 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				int pass3 = Lua.xlua_tointeger(L, 4);
				Graphics.Blit(source7, dest4, mat5, pass3);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<RenderTexture>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				Texture source8 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				RenderTexture dest5 = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				Material mat6 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				int pass4 = Lua.xlua_tointeger(L, 4);
				int destDepthSlice3 = Lua.xlua_tointeger(L, 5);
				Graphics.Blit(source8, dest5, mat6, pass4, destDepthSlice3);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<RenderTexture>(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && objectTranslator.Assignable<Vector2>(L, 4))
			{
				Texture source9 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				RenderTexture dest6 = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				objectTranslator.Get(L, 3, out Vector2 val);
				objectTranslator.Get(L, 4, out Vector2 val2);
				Graphics.Blit(source9, dest6, val, val2);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<RenderTexture>(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && objectTranslator.Assignable<Vector2>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				Texture source10 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				RenderTexture dest7 = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				objectTranslator.Get(L, 3, out Vector2 val3);
				objectTranslator.Get(L, 4, out Vector2 val4);
				Graphics.Blit(sourceDepthSlice: Lua.xlua_tointeger(L, 5), destDepthSlice: Lua.xlua_tointeger(L, 6), source: source10, dest: dest7, scale: val3, offset: val4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.Blit!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BlitMultiTap_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num >= 3 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<RenderTexture>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 4) || objectTranslator.Assignable<Vector2>(L, 4)))
			{
				Texture source = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				RenderTexture dest = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				Material mat = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				Vector2[] offsets = objectTranslator.GetParams<Vector2>(L, 4);
				Graphics.BlitMultiTap(source, dest, mat, offsets);
				return 0;
			}
			if (num >= 4 && objectTranslator.Assignable<Texture>(L, 1) && objectTranslator.Assignable<RenderTexture>(L, 2) && objectTranslator.Assignable<Material>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 5) || objectTranslator.Assignable<Vector2>(L, 5)))
			{
				Texture source2 = (Texture)objectTranslator.GetObject(L, 1, typeof(Texture));
				RenderTexture dest2 = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				Material mat2 = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
				int destDepthSlice = Lua.xlua_tointeger(L, 4);
				Vector2[] offsets2 = objectTranslator.GetParams<Vector2>(L, 5);
				Graphics.BlitMultiTap(source2, dest2, mat2, destDepthSlice, offsets2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Graphics.BlitMultiTap!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_activeColorGamut(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Graphics.activeColorGamut);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_activeTier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Graphics.activeTier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_preserveFramebufferAlpha(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Graphics.preserveFramebufferAlpha);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_activeColorBuffer(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Graphics.activeColorBuffer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_activeDepthBuffer(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Graphics.activeDepthBuffer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_activeTier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out GraphicsTier v);
			Graphics.activeTier = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
