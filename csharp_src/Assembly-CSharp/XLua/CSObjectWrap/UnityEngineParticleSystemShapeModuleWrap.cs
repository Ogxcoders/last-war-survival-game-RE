using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineParticleSystemShapeModuleWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ParticleSystem.ShapeModule);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 45, 45);
		Utils.RegisterFunc(L, -2, "enabled", _g_get_enabled);
		Utils.RegisterFunc(L, -2, "shapeType", _g_get_shapeType);
		Utils.RegisterFunc(L, -2, "randomDirectionAmount", _g_get_randomDirectionAmount);
		Utils.RegisterFunc(L, -2, "sphericalDirectionAmount", _g_get_sphericalDirectionAmount);
		Utils.RegisterFunc(L, -2, "randomPositionAmount", _g_get_randomPositionAmount);
		Utils.RegisterFunc(L, -2, "alignToDirection", _g_get_alignToDirection);
		Utils.RegisterFunc(L, -2, "radius", _g_get_radius);
		Utils.RegisterFunc(L, -2, "radiusMode", _g_get_radiusMode);
		Utils.RegisterFunc(L, -2, "radiusSpread", _g_get_radiusSpread);
		Utils.RegisterFunc(L, -2, "radiusSpeed", _g_get_radiusSpeed);
		Utils.RegisterFunc(L, -2, "radiusSpeedMultiplier", _g_get_radiusSpeedMultiplier);
		Utils.RegisterFunc(L, -2, "radiusThickness", _g_get_radiusThickness);
		Utils.RegisterFunc(L, -2, "angle", _g_get_angle);
		Utils.RegisterFunc(L, -2, "length", _g_get_length);
		Utils.RegisterFunc(L, -2, "boxThickness", _g_get_boxThickness);
		Utils.RegisterFunc(L, -2, "meshShapeType", _g_get_meshShapeType);
		Utils.RegisterFunc(L, -2, "mesh", _g_get_mesh);
		Utils.RegisterFunc(L, -2, "meshRenderer", _g_get_meshRenderer);
		Utils.RegisterFunc(L, -2, "skinnedMeshRenderer", _g_get_skinnedMeshRenderer);
		Utils.RegisterFunc(L, -2, "sprite", _g_get_sprite);
		Utils.RegisterFunc(L, -2, "spriteRenderer", _g_get_spriteRenderer);
		Utils.RegisterFunc(L, -2, "useMeshMaterialIndex", _g_get_useMeshMaterialIndex);
		Utils.RegisterFunc(L, -2, "meshMaterialIndex", _g_get_meshMaterialIndex);
		Utils.RegisterFunc(L, -2, "useMeshColors", _g_get_useMeshColors);
		Utils.RegisterFunc(L, -2, "normalOffset", _g_get_normalOffset);
		Utils.RegisterFunc(L, -2, "meshSpawnMode", _g_get_meshSpawnMode);
		Utils.RegisterFunc(L, -2, "meshSpawnSpread", _g_get_meshSpawnSpread);
		Utils.RegisterFunc(L, -2, "meshSpawnSpeed", _g_get_meshSpawnSpeed);
		Utils.RegisterFunc(L, -2, "meshSpawnSpeedMultiplier", _g_get_meshSpawnSpeedMultiplier);
		Utils.RegisterFunc(L, -2, "arc", _g_get_arc);
		Utils.RegisterFunc(L, -2, "arcMode", _g_get_arcMode);
		Utils.RegisterFunc(L, -2, "arcSpread", _g_get_arcSpread);
		Utils.RegisterFunc(L, -2, "arcSpeed", _g_get_arcSpeed);
		Utils.RegisterFunc(L, -2, "arcSpeedMultiplier", _g_get_arcSpeedMultiplier);
		Utils.RegisterFunc(L, -2, "donutRadius", _g_get_donutRadius);
		Utils.RegisterFunc(L, -2, "position", _g_get_position);
		Utils.RegisterFunc(L, -2, "rotation", _g_get_rotation);
		Utils.RegisterFunc(L, -2, "scale", _g_get_scale);
		Utils.RegisterFunc(L, -2, "texture", _g_get_texture);
		Utils.RegisterFunc(L, -2, "textureClipChannel", _g_get_textureClipChannel);
		Utils.RegisterFunc(L, -2, "textureClipThreshold", _g_get_textureClipThreshold);
		Utils.RegisterFunc(L, -2, "textureColorAffectsParticles", _g_get_textureColorAffectsParticles);
		Utils.RegisterFunc(L, -2, "textureAlphaAffectsParticles", _g_get_textureAlphaAffectsParticles);
		Utils.RegisterFunc(L, -2, "textureBilinearFiltering", _g_get_textureBilinearFiltering);
		Utils.RegisterFunc(L, -2, "textureUVChannel", _g_get_textureUVChannel);
		Utils.RegisterFunc(L, -1, "enabled", _s_set_enabled);
		Utils.RegisterFunc(L, -1, "shapeType", _s_set_shapeType);
		Utils.RegisterFunc(L, -1, "randomDirectionAmount", _s_set_randomDirectionAmount);
		Utils.RegisterFunc(L, -1, "sphericalDirectionAmount", _s_set_sphericalDirectionAmount);
		Utils.RegisterFunc(L, -1, "randomPositionAmount", _s_set_randomPositionAmount);
		Utils.RegisterFunc(L, -1, "alignToDirection", _s_set_alignToDirection);
		Utils.RegisterFunc(L, -1, "radius", _s_set_radius);
		Utils.RegisterFunc(L, -1, "radiusMode", _s_set_radiusMode);
		Utils.RegisterFunc(L, -1, "radiusSpread", _s_set_radiusSpread);
		Utils.RegisterFunc(L, -1, "radiusSpeed", _s_set_radiusSpeed);
		Utils.RegisterFunc(L, -1, "radiusSpeedMultiplier", _s_set_radiusSpeedMultiplier);
		Utils.RegisterFunc(L, -1, "radiusThickness", _s_set_radiusThickness);
		Utils.RegisterFunc(L, -1, "angle", _s_set_angle);
		Utils.RegisterFunc(L, -1, "length", _s_set_length);
		Utils.RegisterFunc(L, -1, "boxThickness", _s_set_boxThickness);
		Utils.RegisterFunc(L, -1, "meshShapeType", _s_set_meshShapeType);
		Utils.RegisterFunc(L, -1, "mesh", _s_set_mesh);
		Utils.RegisterFunc(L, -1, "meshRenderer", _s_set_meshRenderer);
		Utils.RegisterFunc(L, -1, "skinnedMeshRenderer", _s_set_skinnedMeshRenderer);
		Utils.RegisterFunc(L, -1, "sprite", _s_set_sprite);
		Utils.RegisterFunc(L, -1, "spriteRenderer", _s_set_spriteRenderer);
		Utils.RegisterFunc(L, -1, "useMeshMaterialIndex", _s_set_useMeshMaterialIndex);
		Utils.RegisterFunc(L, -1, "meshMaterialIndex", _s_set_meshMaterialIndex);
		Utils.RegisterFunc(L, -1, "useMeshColors", _s_set_useMeshColors);
		Utils.RegisterFunc(L, -1, "normalOffset", _s_set_normalOffset);
		Utils.RegisterFunc(L, -1, "meshSpawnMode", _s_set_meshSpawnMode);
		Utils.RegisterFunc(L, -1, "meshSpawnSpread", _s_set_meshSpawnSpread);
		Utils.RegisterFunc(L, -1, "meshSpawnSpeed", _s_set_meshSpawnSpeed);
		Utils.RegisterFunc(L, -1, "meshSpawnSpeedMultiplier", _s_set_meshSpawnSpeedMultiplier);
		Utils.RegisterFunc(L, -1, "arc", _s_set_arc);
		Utils.RegisterFunc(L, -1, "arcMode", _s_set_arcMode);
		Utils.RegisterFunc(L, -1, "arcSpread", _s_set_arcSpread);
		Utils.RegisterFunc(L, -1, "arcSpeed", _s_set_arcSpeed);
		Utils.RegisterFunc(L, -1, "arcSpeedMultiplier", _s_set_arcSpeedMultiplier);
		Utils.RegisterFunc(L, -1, "donutRadius", _s_set_donutRadius);
		Utils.RegisterFunc(L, -1, "position", _s_set_position);
		Utils.RegisterFunc(L, -1, "rotation", _s_set_rotation);
		Utils.RegisterFunc(L, -1, "scale", _s_set_scale);
		Utils.RegisterFunc(L, -1, "texture", _s_set_texture);
		Utils.RegisterFunc(L, -1, "textureClipChannel", _s_set_textureClipChannel);
		Utils.RegisterFunc(L, -1, "textureClipThreshold", _s_set_textureClipThreshold);
		Utils.RegisterFunc(L, -1, "textureColorAffectsParticles", _s_set_textureColorAffectsParticles);
		Utils.RegisterFunc(L, -1, "textureAlphaAffectsParticles", _s_set_textureAlphaAffectsParticles);
		Utils.RegisterFunc(L, -1, "textureBilinearFiltering", _s_set_textureBilinearFiltering);
		Utils.RegisterFunc(L, -1, "textureUVChannel", _s_set_textureUVChannel);
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
				objectTranslator.Push(L, default(ParticleSystem.ShapeModule));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.ShapeModule constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enabled(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushboolean(L, v.enabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shapeType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.shapeType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_randomDirectionAmount(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.randomDirectionAmount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sphericalDirectionAmount(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.sphericalDirectionAmount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_randomPositionAmount(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.randomPositionAmount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alignToDirection(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushboolean(L, v.alignToDirection);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_radius(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.radius);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_radiusMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.radiusMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_radiusSpread(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.radiusSpread);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_radiusSpeed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.radiusSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_radiusSpeedMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.radiusSpeedMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_radiusThickness(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.radiusThickness);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_angle(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.angle);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_length(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.length);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_boxThickness(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.PushUnityEngineVector3(L, v.boxThickness);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_meshShapeType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.meshShapeType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.mesh);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_meshRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.meshRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_skinnedMeshRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.skinnedMeshRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.sprite);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spriteRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.spriteRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useMeshMaterialIndex(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushboolean(L, v.useMeshMaterialIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_meshMaterialIndex(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.xlua_pushinteger(L, v.meshMaterialIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useMeshColors(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushboolean(L, v.useMeshColors);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_normalOffset(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.normalOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_meshSpawnMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.meshSpawnMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_meshSpawnSpread(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.meshSpawnSpread);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_meshSpawnSpeed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.meshSpawnSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_meshSpawnSpeedMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.meshSpawnSpeedMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_arc(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.arc);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_arcMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.arcMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_arcSpread(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.arcSpread);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_arcSpeed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.arcSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_arcSpeedMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.arcSpeedMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_donutRadius(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.donutRadius);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.PushUnityEngineVector3(L, v.position);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.PushUnityEngineVector3(L, v.rotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.PushUnityEngineVector3(L, v.scale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_texture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.texture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureClipChannel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Push(L, v.textureClipChannel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureClipThreshold(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushnumber(L, v.textureClipThreshold);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureColorAffectsParticles(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushboolean(L, v.textureColorAffectsParticles);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureAlphaAffectsParticles(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushboolean(L, v.textureAlphaAffectsParticles);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureBilinearFiltering(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.lua_pushboolean(L, v.textureBilinearFiltering);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureUVChannel(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ShapeModule v);
			Lua.xlua_pushinteger(L, v.textureUVChannel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enabled(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.enabled = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shapeType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Get(L, 2, out ParticleSystemShapeType v2);
			v.shapeType = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_randomDirectionAmount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.randomDirectionAmount = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sphericalDirectionAmount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.sphericalDirectionAmount = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_randomPositionAmount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.randomPositionAmount = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alignToDirection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.alignToDirection = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_radius(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.radius = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_radiusMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Get(L, 2, out ParticleSystemShapeMultiModeValue v2);
			v.radiusMode = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_radiusSpread(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.radiusSpread = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_radiusSpeed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.radiusSpeed = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_radiusSpeedMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.radiusSpeedMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_radiusThickness(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.radiusThickness = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_angle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.angle = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_length(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.length = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_boxThickness(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Get(L, 2, out Vector3 val);
			v.boxThickness = val;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_meshShapeType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Get(L, 2, out ParticleSystemMeshShapeType v2);
			v.meshShapeType = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.mesh = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_meshRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.meshRenderer = (MeshRenderer)objectTranslator.GetObject(L, 2, typeof(MeshRenderer));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_skinnedMeshRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.skinnedMeshRenderer = (SkinnedMeshRenderer)objectTranslator.GetObject(L, 2, typeof(SkinnedMeshRenderer));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.sprite = (Sprite)objectTranslator.GetObject(L, 2, typeof(Sprite));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_spriteRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.spriteRenderer = (SpriteRenderer)objectTranslator.GetObject(L, 2, typeof(SpriteRenderer));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useMeshMaterialIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.useMeshMaterialIndex = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_meshMaterialIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.meshMaterialIndex = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useMeshColors(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.useMeshColors = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_normalOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.normalOffset = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_meshSpawnMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Get(L, 2, out ParticleSystemShapeMultiModeValue v2);
			v.meshSpawnMode = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_meshSpawnSpread(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.meshSpawnSpread = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_meshSpawnSpeed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.meshSpawnSpeed = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_meshSpawnSpeedMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.meshSpawnSpeedMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_arc(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.arc = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_arcMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Get(L, 2, out ParticleSystemShapeMultiModeValue v2);
			v.arcMode = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_arcSpread(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.arcSpread = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_arcSpeed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.arcSpeed = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_arcSpeedMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.arcSpeedMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_donutRadius(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.donutRadius = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Get(L, 2, out Vector3 val);
			v.position = val;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Get(L, 2, out Vector3 val);
			v.rotation = val;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_scale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Get(L, 2, out Vector3 val);
			v.scale = val;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_texture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.texture = (Texture2D)objectTranslator.GetObject(L, 2, typeof(Texture2D));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textureClipChannel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			objectTranslator.Get(L, 2, out ParticleSystemShapeTextureChannel v2);
			v.textureClipChannel = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textureClipThreshold(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.textureClipThreshold = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textureColorAffectsParticles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.textureColorAffectsParticles = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textureAlphaAffectsParticles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.textureAlphaAffectsParticles = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textureBilinearFiltering(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.textureBilinearFiltering = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textureUVChannel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ShapeModule v);
			v.textureUVChannel = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
