using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineMeshWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Mesh);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 53, 24, 19);
		Utils.RegisterFunc(L, -3, "SetIndexBufferParams", _m_SetIndexBufferParams);
		Utils.RegisterFunc(L, -3, "SetVertexBufferParams", _m_SetVertexBufferParams);
		Utils.RegisterFunc(L, -3, "GetVertexAttribute", _m_GetVertexAttribute);
		Utils.RegisterFunc(L, -3, "HasVertexAttribute", _m_HasVertexAttribute);
		Utils.RegisterFunc(L, -3, "GetVertexAttributeDimension", _m_GetVertexAttributeDimension);
		Utils.RegisterFunc(L, -3, "GetVertexAttributeFormat", _m_GetVertexAttributeFormat);
		Utils.RegisterFunc(L, -3, "GetNativeVertexBufferPtr", _m_GetNativeVertexBufferPtr);
		Utils.RegisterFunc(L, -3, "GetNativeIndexBufferPtr", _m_GetNativeIndexBufferPtr);
		Utils.RegisterFunc(L, -3, "ClearBlendShapes", _m_ClearBlendShapes);
		Utils.RegisterFunc(L, -3, "GetBlendShapeName", _m_GetBlendShapeName);
		Utils.RegisterFunc(L, -3, "GetBlendShapeIndex", _m_GetBlendShapeIndex);
		Utils.RegisterFunc(L, -3, "GetBlendShapeFrameCount", _m_GetBlendShapeFrameCount);
		Utils.RegisterFunc(L, -3, "GetBlendShapeFrameWeight", _m_GetBlendShapeFrameWeight);
		Utils.RegisterFunc(L, -3, "GetBlendShapeFrameVertices", _m_GetBlendShapeFrameVertices);
		Utils.RegisterFunc(L, -3, "AddBlendShapeFrame", _m_AddBlendShapeFrame);
		Utils.RegisterFunc(L, -3, "SetBoneWeights", _m_SetBoneWeights);
		Utils.RegisterFunc(L, -3, "GetAllBoneWeights", _m_GetAllBoneWeights);
		Utils.RegisterFunc(L, -3, "GetBonesPerVertex", _m_GetBonesPerVertex);
		Utils.RegisterFunc(L, -3, "SetSubMesh", _m_SetSubMesh);
		Utils.RegisterFunc(L, -3, "GetSubMesh", _m_GetSubMesh);
		Utils.RegisterFunc(L, -3, "MarkModified", _m_MarkModified);
		Utils.RegisterFunc(L, -3, "GetUVDistributionMetric", _m_GetUVDistributionMetric);
		Utils.RegisterFunc(L, -3, "GetVertices", _m_GetVertices);
		Utils.RegisterFunc(L, -3, "SetVertices", _m_SetVertices);
		Utils.RegisterFunc(L, -3, "GetNormals", _m_GetNormals);
		Utils.RegisterFunc(L, -3, "SetNormals", _m_SetNormals);
		Utils.RegisterFunc(L, -3, "GetTangents", _m_GetTangents);
		Utils.RegisterFunc(L, -3, "SetTangents", _m_SetTangents);
		Utils.RegisterFunc(L, -3, "GetColors", _m_GetColors);
		Utils.RegisterFunc(L, -3, "SetColors", _m_SetColors);
		Utils.RegisterFunc(L, -3, "SetUVs", _m_SetUVs);
		Utils.RegisterFunc(L, -3, "GetUVs", _m_GetUVs);
		Utils.RegisterFunc(L, -3, "GetVertexAttributes", _m_GetVertexAttributes);
		Utils.RegisterFunc(L, -3, "GetTriangles", _m_GetTriangles);
		Utils.RegisterFunc(L, -3, "GetIndices", _m_GetIndices);
		Utils.RegisterFunc(L, -3, "GetIndexStart", _m_GetIndexStart);
		Utils.RegisterFunc(L, -3, "GetIndexCount", _m_GetIndexCount);
		Utils.RegisterFunc(L, -3, "GetBaseVertex", _m_GetBaseVertex);
		Utils.RegisterFunc(L, -3, "SetTriangles", _m_SetTriangles);
		Utils.RegisterFunc(L, -3, "SetIndices", _m_SetIndices);
		Utils.RegisterFunc(L, -3, "GetBindposes", _m_GetBindposes);
		Utils.RegisterFunc(L, -3, "GetBoneWeights", _m_GetBoneWeights);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "RecalculateBounds", _m_RecalculateBounds);
		Utils.RegisterFunc(L, -3, "RecalculateNormals", _m_RecalculateNormals);
		Utils.RegisterFunc(L, -3, "RecalculateTangents", _m_RecalculateTangents);
		Utils.RegisterFunc(L, -3, "MarkDynamic", _m_MarkDynamic);
		Utils.RegisterFunc(L, -3, "UploadMeshData", _m_UploadMeshData);
		Utils.RegisterFunc(L, -3, "Optimize", _m_Optimize);
		Utils.RegisterFunc(L, -3, "OptimizeIndexBuffers", _m_OptimizeIndexBuffers);
		Utils.RegisterFunc(L, -3, "OptimizeReorderVertexBuffer", _m_OptimizeReorderVertexBuffer);
		Utils.RegisterFunc(L, -3, "GetTopology", _m_GetTopology);
		Utils.RegisterFunc(L, -3, "CombineMeshes", _m_CombineMeshes);
		Utils.RegisterFunc(L, -2, "indexFormat", _g_get_indexFormat);
		Utils.RegisterFunc(L, -2, "vertexBufferCount", _g_get_vertexBufferCount);
		Utils.RegisterFunc(L, -2, "blendShapeCount", _g_get_blendShapeCount);
		Utils.RegisterFunc(L, -2, "bindposes", _g_get_bindposes);
		Utils.RegisterFunc(L, -2, "isReadable", _g_get_isReadable);
		Utils.RegisterFunc(L, -2, "vertexCount", _g_get_vertexCount);
		Utils.RegisterFunc(L, -2, "subMeshCount", _g_get_subMeshCount);
		Utils.RegisterFunc(L, -2, "bounds", _g_get_bounds);
		Utils.RegisterFunc(L, -2, "vertices", _g_get_vertices);
		Utils.RegisterFunc(L, -2, "normals", _g_get_normals);
		Utils.RegisterFunc(L, -2, "tangents", _g_get_tangents);
		Utils.RegisterFunc(L, -2, "uv", _g_get_uv);
		Utils.RegisterFunc(L, -2, "uv2", _g_get_uv2);
		Utils.RegisterFunc(L, -2, "uv3", _g_get_uv3);
		Utils.RegisterFunc(L, -2, "uv4", _g_get_uv4);
		Utils.RegisterFunc(L, -2, "uv5", _g_get_uv5);
		Utils.RegisterFunc(L, -2, "uv6", _g_get_uv6);
		Utils.RegisterFunc(L, -2, "uv7", _g_get_uv7);
		Utils.RegisterFunc(L, -2, "uv8", _g_get_uv8);
		Utils.RegisterFunc(L, -2, "colors", _g_get_colors);
		Utils.RegisterFunc(L, -2, "colors32", _g_get_colors32);
		Utils.RegisterFunc(L, -2, "vertexAttributeCount", _g_get_vertexAttributeCount);
		Utils.RegisterFunc(L, -2, "triangles", _g_get_triangles);
		Utils.RegisterFunc(L, -2, "boneWeights", _g_get_boneWeights);
		Utils.RegisterFunc(L, -1, "indexFormat", _s_set_indexFormat);
		Utils.RegisterFunc(L, -1, "bindposes", _s_set_bindposes);
		Utils.RegisterFunc(L, -1, "subMeshCount", _s_set_subMeshCount);
		Utils.RegisterFunc(L, -1, "bounds", _s_set_bounds);
		Utils.RegisterFunc(L, -1, "vertices", _s_set_vertices);
		Utils.RegisterFunc(L, -1, "normals", _s_set_normals);
		Utils.RegisterFunc(L, -1, "tangents", _s_set_tangents);
		Utils.RegisterFunc(L, -1, "uv", _s_set_uv);
		Utils.RegisterFunc(L, -1, "uv2", _s_set_uv2);
		Utils.RegisterFunc(L, -1, "uv3", _s_set_uv3);
		Utils.RegisterFunc(L, -1, "uv4", _s_set_uv4);
		Utils.RegisterFunc(L, -1, "uv5", _s_set_uv5);
		Utils.RegisterFunc(L, -1, "uv6", _s_set_uv6);
		Utils.RegisterFunc(L, -1, "uv7", _s_set_uv7);
		Utils.RegisterFunc(L, -1, "uv8", _s_set_uv8);
		Utils.RegisterFunc(L, -1, "colors", _s_set_colors);
		Utils.RegisterFunc(L, -1, "colors32", _s_set_colors32);
		Utils.RegisterFunc(L, -1, "triangles", _s_set_triangles);
		Utils.RegisterFunc(L, -1, "boneWeights", _s_set_boneWeights);
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
				Mesh o = new Mesh();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIndexBufferParams(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int indexCount = Lua.xlua_tointeger(L, 2);
			objectTranslator.Get(L, 3, out IndexFormat v);
			mesh.SetIndexBufferParams(indexCount, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVertexBufferParams(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int vertexCount = Lua.xlua_tointeger(L, 2);
			VertexAttributeDescriptor[] attributes = objectTranslator.GetParams<VertexAttributeDescriptor>(L, 3);
			mesh.SetVertexBufferParams(vertexCount, attributes);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetVertexAttribute(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh obj = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			VertexAttributeDescriptor vertexAttribute = obj.GetVertexAttribute(index);
			objectTranslator.Push(L, vertexAttribute);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasVertexAttribute(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VertexAttribute v);
			bool value = mesh.HasVertexAttribute(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetVertexAttributeDimension(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VertexAttribute v);
			int vertexAttributeDimension = mesh.GetVertexAttributeDimension(v);
			Lua.xlua_pushinteger(L, vertexAttributeDimension);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetVertexAttributeFormat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VertexAttribute v);
			VertexAttributeFormat vertexAttributeFormat = mesh.GetVertexAttributeFormat(v);
			objectTranslator.Push(L, vertexAttributeFormat);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNativeVertexBufferPtr(IntPtr L)
	{
		try
		{
			Mesh obj = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			IntPtr nativeVertexBufferPtr = obj.GetNativeVertexBufferPtr(index);
			Lua.lua_pushlightuserdata(L, nativeVertexBufferPtr);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNativeIndexBufferPtr(IntPtr L)
	{
		try
		{
			IntPtr nativeIndexBufferPtr = ((Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetNativeIndexBufferPtr();
			Lua.lua_pushlightuserdata(L, nativeIndexBufferPtr);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearBlendShapes(IntPtr L)
	{
		try
		{
			((Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearBlendShapes();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBlendShapeName(IntPtr L)
	{
		try
		{
			Mesh obj = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int shapeIndex = Lua.xlua_tointeger(L, 2);
			string blendShapeName = obj.GetBlendShapeName(shapeIndex);
			Lua.lua_pushstring(L, blendShapeName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBlendShapeIndex(IntPtr L)
	{
		try
		{
			Mesh obj = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string blendShapeName = Lua.lua_tostring(L, 2);
			int blendShapeIndex = obj.GetBlendShapeIndex(blendShapeName);
			Lua.xlua_pushinteger(L, blendShapeIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBlendShapeFrameCount(IntPtr L)
	{
		try
		{
			Mesh obj = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int shapeIndex = Lua.xlua_tointeger(L, 2);
			int blendShapeFrameCount = obj.GetBlendShapeFrameCount(shapeIndex);
			Lua.xlua_pushinteger(L, blendShapeFrameCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBlendShapeFrameWeight(IntPtr L)
	{
		try
		{
			Mesh obj = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int shapeIndex = Lua.xlua_tointeger(L, 2);
			int frameIndex = Lua.xlua_tointeger(L, 3);
			float blendShapeFrameWeight = obj.GetBlendShapeFrameWeight(shapeIndex, frameIndex);
			Lua.lua_pushnumber(L, blendShapeFrameWeight);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBlendShapeFrameVertices(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int shapeIndex = Lua.xlua_tointeger(L, 2);
			int frameIndex = Lua.xlua_tointeger(L, 3);
			Vector3[] deltaVertices = (Vector3[])objectTranslator.GetObject(L, 4, typeof(Vector3[]));
			Vector3[] deltaNormals = (Vector3[])objectTranslator.GetObject(L, 5, typeof(Vector3[]));
			Vector3[] deltaTangents = (Vector3[])objectTranslator.GetObject(L, 6, typeof(Vector3[]));
			mesh.GetBlendShapeFrameVertices(shapeIndex, frameIndex, deltaVertices, deltaNormals, deltaTangents);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddBlendShapeFrame(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			string shapeName = Lua.lua_tostring(L, 2);
			float frameWeight = (float)Lua.lua_tonumber(L, 3);
			Vector3[] deltaVertices = (Vector3[])objectTranslator.GetObject(L, 4, typeof(Vector3[]));
			Vector3[] deltaNormals = (Vector3[])objectTranslator.GetObject(L, 5, typeof(Vector3[]));
			Vector3[] deltaTangents = (Vector3[])objectTranslator.GetObject(L, 6, typeof(Vector3[]));
			mesh.AddBlendShapeFrame(shapeName, frameWeight, deltaVertices, deltaNormals, deltaTangents);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBoneWeights(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out NativeArray<byte> v);
			objectTranslator.Get(L, 3, out NativeArray<BoneWeight1> v2);
			mesh.SetBoneWeights(v, v2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllBoneWeights(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			NativeArray<BoneWeight1> allBoneWeights = ((Mesh)objectTranslator.FastGetCSObj(L, 1)).GetAllBoneWeights();
			objectTranslator.Push(L, allBoneWeights);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBonesPerVertex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			NativeArray<byte> bonesPerVertex = ((Mesh)objectTranslator.FastGetCSObj(L, 1)).GetBonesPerVertex();
			objectTranslator.Push(L, bonesPerVertex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSubMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<SubMeshDescriptor>(L, 3) && objectTranslator.Assignable<MeshUpdateFlags>(L, 4))
			{
				int index = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out SubMeshDescriptor v);
				objectTranslator.Get(L, 4, out MeshUpdateFlags v2);
				mesh.SetSubMesh(index, v, v2);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<SubMeshDescriptor>(L, 3))
			{
				int index2 = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out SubMeshDescriptor v3);
				mesh.SetSubMesh(index2, v3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.SetSubMesh!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSubMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh obj = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			SubMeshDescriptor subMesh = obj.GetSubMesh(index);
			objectTranslator.Push(L, subMesh);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MarkModified(IntPtr L)
	{
		try
		{
			((Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).MarkModified();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUVDistributionMetric(IntPtr L)
	{
		try
		{
			Mesh obj = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int uvSetIndex = Lua.xlua_tointeger(L, 2);
			float uVDistributionMetric = obj.GetUVDistributionMetric(uvSetIndex);
			Lua.lua_pushnumber(L, uVDistributionMetric);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetVertices(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			List<Vector3> vertices = (List<Vector3>)objectTranslator.GetObject(L, 2, typeof(List<Vector3>));
			mesh.GetVertices(vertices);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVertices(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<List<Vector3>>(L, 2))
			{
				List<Vector3> vertices = (List<Vector3>)objectTranslator.GetObject(L, 2, typeof(List<Vector3>));
				mesh.SetVertices(vertices);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3[]>(L, 2))
			{
				Vector3[] vertices2 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				mesh.SetVertices(vertices2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<List<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				List<Vector3> inVertices = (List<Vector3>)objectTranslator.GetObject(L, 2, typeof(List<Vector3>));
				int start = Lua.xlua_tointeger(L, 3);
				int length = Lua.xlua_tointeger(L, 4);
				mesh.SetVertices(inVertices, start, length);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Vector3[] inVertices2 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				int start2 = Lua.xlua_tointeger(L, 3);
				int length2 = Lua.xlua_tointeger(L, 4);
				mesh.SetVertices(inVertices2, start2, length2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.SetVertices!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNormals(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			List<Vector3> normals = (List<Vector3>)objectTranslator.GetObject(L, 2, typeof(List<Vector3>));
			mesh.GetNormals(normals);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetNormals(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<List<Vector3>>(L, 2))
			{
				List<Vector3> normals = (List<Vector3>)objectTranslator.GetObject(L, 2, typeof(List<Vector3>));
				mesh.SetNormals(normals);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3[]>(L, 2))
			{
				Vector3[] normals2 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				mesh.SetNormals(normals2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<List<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				List<Vector3> inNormals = (List<Vector3>)objectTranslator.GetObject(L, 2, typeof(List<Vector3>));
				int start = Lua.xlua_tointeger(L, 3);
				int length = Lua.xlua_tointeger(L, 4);
				mesh.SetNormals(inNormals, start, length);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Vector3[] inNormals2 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				int start2 = Lua.xlua_tointeger(L, 3);
				int length2 = Lua.xlua_tointeger(L, 4);
				mesh.SetNormals(inNormals2, start2, length2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.SetNormals!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTangents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			List<Vector4> tangents = (List<Vector4>)objectTranslator.GetObject(L, 2, typeof(List<Vector4>));
			mesh.GetTangents(tangents);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTangents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<List<Vector4>>(L, 2))
			{
				List<Vector4> tangents = (List<Vector4>)objectTranslator.GetObject(L, 2, typeof(List<Vector4>));
				mesh.SetTangents(tangents);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector4[]>(L, 2))
			{
				Vector4[] tangents2 = (Vector4[])objectTranslator.GetObject(L, 2, typeof(Vector4[]));
				mesh.SetTangents(tangents2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<List<Vector4>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				List<Vector4> inTangents = (List<Vector4>)objectTranslator.GetObject(L, 2, typeof(List<Vector4>));
				int start = Lua.xlua_tointeger(L, 3);
				int length = Lua.xlua_tointeger(L, 4);
				mesh.SetTangents(inTangents, start, length);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Vector4[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Vector4[] inTangents2 = (Vector4[])objectTranslator.GetObject(L, 2, typeof(Vector4[]));
				int start2 = Lua.xlua_tointeger(L, 3);
				int length2 = Lua.xlua_tointeger(L, 4);
				mesh.SetTangents(inTangents2, start2, length2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.SetTangents!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetColors(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<List<Color>>(L, 2))
			{
				List<Color> colors = (List<Color>)objectTranslator.GetObject(L, 2, typeof(List<Color>));
				mesh.GetColors(colors);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<List<Color32>>(L, 2))
			{
				List<Color32> colors2 = (List<Color32>)objectTranslator.GetObject(L, 2, typeof(List<Color32>));
				mesh.GetColors(colors2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.GetColors!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetColors(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<List<Color>>(L, 2))
			{
				List<Color> colors = (List<Color>)objectTranslator.GetObject(L, 2, typeof(List<Color>));
				mesh.SetColors(colors);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Color[]>(L, 2))
			{
				Color[] colors2 = (Color[])objectTranslator.GetObject(L, 2, typeof(Color[]));
				mesh.SetColors(colors2);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<List<Color32>>(L, 2))
			{
				List<Color32> colors3 = (List<Color32>)objectTranslator.GetObject(L, 2, typeof(List<Color32>));
				mesh.SetColors(colors3);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Color32[]>(L, 2))
			{
				Color32[] colors4 = (Color32[])objectTranslator.GetObject(L, 2, typeof(Color32[]));
				mesh.SetColors(colors4);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<List<Color>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				List<Color> inColors = (List<Color>)objectTranslator.GetObject(L, 2, typeof(List<Color>));
				int start = Lua.xlua_tointeger(L, 3);
				int length = Lua.xlua_tointeger(L, 4);
				mesh.SetColors(inColors, start, length);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Color[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Color[] inColors2 = (Color[])objectTranslator.GetObject(L, 2, typeof(Color[]));
				int start2 = Lua.xlua_tointeger(L, 3);
				int length2 = Lua.xlua_tointeger(L, 4);
				mesh.SetColors(inColors2, start2, length2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<List<Color32>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				List<Color32> inColors3 = (List<Color32>)objectTranslator.GetObject(L, 2, typeof(List<Color32>));
				int start3 = Lua.xlua_tointeger(L, 3);
				int length3 = Lua.xlua_tointeger(L, 4);
				mesh.SetColors(inColors3, start3, length3);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Color32[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Color32[] inColors4 = (Color32[])objectTranslator.GetObject(L, 2, typeof(Color32[]));
				int start4 = Lua.xlua_tointeger(L, 3);
				int length4 = Lua.xlua_tointeger(L, 4);
				mesh.SetColors(inColors4, start4, length4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.SetColors!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetUVs(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Vector2>>(L, 3))
			{
				int channel = Lua.xlua_tointeger(L, 2);
				List<Vector2> uvs = (List<Vector2>)objectTranslator.GetObject(L, 3, typeof(List<Vector2>));
				mesh.SetUVs(channel, uvs);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Vector3>>(L, 3))
			{
				int channel2 = Lua.xlua_tointeger(L, 2);
				List<Vector3> uvs2 = (List<Vector3>)objectTranslator.GetObject(L, 3, typeof(List<Vector3>));
				mesh.SetUVs(channel2, uvs2);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Vector4>>(L, 3))
			{
				int channel3 = Lua.xlua_tointeger(L, 2);
				List<Vector4> uvs3 = (List<Vector4>)objectTranslator.GetObject(L, 3, typeof(List<Vector4>));
				mesh.SetUVs(channel3, uvs3);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector2[]>(L, 3))
			{
				int channel4 = Lua.xlua_tointeger(L, 2);
				Vector2[] uvs4 = (Vector2[])objectTranslator.GetObject(L, 3, typeof(Vector2[]));
				mesh.SetUVs(channel4, uvs4);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3[]>(L, 3))
			{
				int channel5 = Lua.xlua_tointeger(L, 2);
				Vector3[] uvs5 = (Vector3[])objectTranslator.GetObject(L, 3, typeof(Vector3[]));
				mesh.SetUVs(channel5, uvs5);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector4[]>(L, 3))
			{
				int channel6 = Lua.xlua_tointeger(L, 2);
				Vector4[] uvs6 = (Vector4[])objectTranslator.GetObject(L, 3, typeof(Vector4[]));
				mesh.SetUVs(channel6, uvs6);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Vector2>>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int channel7 = Lua.xlua_tointeger(L, 2);
				List<Vector2> uvs7 = (List<Vector2>)objectTranslator.GetObject(L, 3, typeof(List<Vector2>));
				int start = Lua.xlua_tointeger(L, 4);
				int length = Lua.xlua_tointeger(L, 5);
				mesh.SetUVs(channel7, uvs7, start, length);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Vector3>>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int channel8 = Lua.xlua_tointeger(L, 2);
				List<Vector3> uvs8 = (List<Vector3>)objectTranslator.GetObject(L, 3, typeof(List<Vector3>));
				int start2 = Lua.xlua_tointeger(L, 4);
				int length2 = Lua.xlua_tointeger(L, 5);
				mesh.SetUVs(channel8, uvs8, start2, length2);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Vector4>>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int channel9 = Lua.xlua_tointeger(L, 2);
				List<Vector4> uvs9 = (List<Vector4>)objectTranslator.GetObject(L, 3, typeof(List<Vector4>));
				int start3 = Lua.xlua_tointeger(L, 4);
				int length3 = Lua.xlua_tointeger(L, 5);
				mesh.SetUVs(channel9, uvs9, start3, length3);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector2[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int channel10 = Lua.xlua_tointeger(L, 2);
				Vector2[] uvs10 = (Vector2[])objectTranslator.GetObject(L, 3, typeof(Vector2[]));
				int start4 = Lua.xlua_tointeger(L, 4);
				int length4 = Lua.xlua_tointeger(L, 5);
				mesh.SetUVs(channel10, uvs10, start4, length4);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int channel11 = Lua.xlua_tointeger(L, 2);
				Vector3[] uvs11 = (Vector3[])objectTranslator.GetObject(L, 3, typeof(Vector3[]));
				int start5 = Lua.xlua_tointeger(L, 4);
				int length5 = Lua.xlua_tointeger(L, 5);
				mesh.SetUVs(channel11, uvs11, start5, length5);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector4[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int channel12 = Lua.xlua_tointeger(L, 2);
				Vector4[] uvs12 = (Vector4[])objectTranslator.GetObject(L, 3, typeof(Vector4[]));
				int start6 = Lua.xlua_tointeger(L, 4);
				int length6 = Lua.xlua_tointeger(L, 5);
				mesh.SetUVs(channel12, uvs12, start6, length6);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.SetUVs!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUVs(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Vector2>>(L, 3))
			{
				int channel = Lua.xlua_tointeger(L, 2);
				List<Vector2> uvs = (List<Vector2>)objectTranslator.GetObject(L, 3, typeof(List<Vector2>));
				mesh.GetUVs(channel, uvs);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Vector3>>(L, 3))
			{
				int channel2 = Lua.xlua_tointeger(L, 2);
				List<Vector3> uvs2 = (List<Vector3>)objectTranslator.GetObject(L, 3, typeof(List<Vector3>));
				mesh.GetUVs(channel2, uvs2);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Vector4>>(L, 3))
			{
				int channel3 = Lua.xlua_tointeger(L, 2);
				List<Vector4> uvs3 = (List<Vector4>)objectTranslator.GetObject(L, 3, typeof(List<Vector4>));
				mesh.GetUVs(channel3, uvs3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.GetUVs!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetVertexAttributes(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			switch (num)
			{
			case 1:
			{
				VertexAttributeDescriptor[] vertexAttributes2 = mesh.GetVertexAttributes();
				objectTranslator.Push(L, vertexAttributes2);
				return 1;
			}
			case 2:
				if (objectTranslator.Assignable<VertexAttributeDescriptor[]>(L, 2))
				{
					VertexAttributeDescriptor[] attributes = (VertexAttributeDescriptor[])objectTranslator.GetObject(L, 2, typeof(VertexAttributeDescriptor[]));
					int vertexAttributes = mesh.GetVertexAttributes(attributes);
					Lua.xlua_pushinteger(L, vertexAttributes);
					return 1;
				}
				break;
			}
			if (num == 2 && objectTranslator.Assignable<List<VertexAttributeDescriptor>>(L, 2))
			{
				List<VertexAttributeDescriptor> attributes2 = (List<VertexAttributeDescriptor>)objectTranslator.GetObject(L, 2, typeof(List<VertexAttributeDescriptor>));
				int vertexAttributes3 = mesh.GetVertexAttributes(attributes2);
				Lua.xlua_pushinteger(L, vertexAttributes3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.GetVertexAttributes!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTriangles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int submesh = Lua.xlua_tointeger(L, 2);
				int[] triangles = mesh.GetTriangles(submesh);
				objectTranslator.Push(L, triangles);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int submesh2 = Lua.xlua_tointeger(L, 2);
				bool applyBaseVertex = Lua.lua_toboolean(L, 3);
				int[] triangles2 = mesh.GetTriangles(submesh2, applyBaseVertex);
				objectTranslator.Push(L, triangles2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<List<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				List<int> triangles3 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				int submesh3 = Lua.xlua_tointeger(L, 3);
				mesh.GetTriangles(triangles3, submesh3);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<List<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				List<int> triangles4 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				int submesh4 = Lua.xlua_tointeger(L, 3);
				bool applyBaseVertex2 = Lua.lua_toboolean(L, 4);
				mesh.GetTriangles(triangles4, submesh4, applyBaseVertex2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<List<ushort>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				List<ushort> triangles5 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				int submesh5 = Lua.xlua_tointeger(L, 3);
				bool applyBaseVertex3 = Lua.lua_toboolean(L, 4);
				mesh.GetTriangles(triangles5, submesh5, applyBaseVertex3);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<List<ushort>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				List<ushort> triangles6 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				int submesh6 = Lua.xlua_tointeger(L, 3);
				mesh.GetTriangles(triangles6, submesh6);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.GetTriangles!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIndices(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int submesh = Lua.xlua_tointeger(L, 2);
				int[] indices = mesh.GetIndices(submesh);
				objectTranslator.Push(L, indices);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int submesh2 = Lua.xlua_tointeger(L, 2);
				bool applyBaseVertex = Lua.lua_toboolean(L, 3);
				int[] indices2 = mesh.GetIndices(submesh2, applyBaseVertex);
				objectTranslator.Push(L, indices2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<List<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				List<int> indices3 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				int submesh3 = Lua.xlua_tointeger(L, 3);
				mesh.GetIndices(indices3, submesh3);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<List<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				List<int> indices4 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				int submesh4 = Lua.xlua_tointeger(L, 3);
				bool applyBaseVertex2 = Lua.lua_toboolean(L, 4);
				mesh.GetIndices(indices4, submesh4, applyBaseVertex2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<List<ushort>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				List<ushort> indices5 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				int submesh5 = Lua.xlua_tointeger(L, 3);
				bool applyBaseVertex3 = Lua.lua_toboolean(L, 4);
				mesh.GetIndices(indices5, submesh5, applyBaseVertex3);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<List<ushort>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				List<ushort> indices6 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				int submesh6 = Lua.xlua_tointeger(L, 3);
				mesh.GetIndices(indices6, submesh6);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.GetIndices!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIndexStart(IntPtr L)
	{
		try
		{
			Mesh obj = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int submesh = Lua.xlua_tointeger(L, 2);
			uint indexStart = obj.GetIndexStart(submesh);
			Lua.xlua_pushuint(L, indexStart);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIndexCount(IntPtr L)
	{
		try
		{
			Mesh obj = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int submesh = Lua.xlua_tointeger(L, 2);
			uint indexCount = obj.GetIndexCount(submesh);
			Lua.xlua_pushuint(L, indexCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBaseVertex(IntPtr L)
	{
		try
		{
			Mesh obj = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int submesh = Lua.xlua_tointeger(L, 2);
			uint baseVertex = obj.GetBaseVertex(submesh);
			Lua.xlua_pushuint(L, baseVertex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTriangles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<int[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int[] triangles = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
				int submesh = Lua.xlua_tointeger(L, 3);
				mesh.SetTriangles(triangles, submesh);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<List<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				List<int> triangles2 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				int submesh2 = Lua.xlua_tointeger(L, 3);
				mesh.SetTriangles(triangles2, submesh2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<int[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				int[] triangles3 = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
				int submesh3 = Lua.xlua_tointeger(L, 3);
				bool calculateBounds = Lua.lua_toboolean(L, 4);
				mesh.SetTriangles(triangles3, submesh3, calculateBounds);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<List<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				List<int> triangles4 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				int submesh4 = Lua.xlua_tointeger(L, 3);
				bool calculateBounds2 = Lua.lua_toboolean(L, 4);
				mesh.SetTriangles(triangles4, submesh4, calculateBounds2);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<int[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int[] triangles5 = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
				int submesh5 = Lua.xlua_tointeger(L, 3);
				bool calculateBounds3 = Lua.lua_toboolean(L, 4);
				int baseVertex = Lua.xlua_tointeger(L, 5);
				mesh.SetTriangles(triangles5, submesh5, calculateBounds3, baseVertex);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<ushort[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				ushort[] triangles6 = (ushort[])objectTranslator.GetObject(L, 2, typeof(ushort[]));
				int submesh6 = Lua.xlua_tointeger(L, 3);
				bool calculateBounds4 = Lua.lua_toboolean(L, 4);
				int baseVertex2 = Lua.xlua_tointeger(L, 5);
				mesh.SetTriangles(triangles6, submesh6, calculateBounds4, baseVertex2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<ushort[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				ushort[] triangles7 = (ushort[])objectTranslator.GetObject(L, 2, typeof(ushort[]));
				int submesh7 = Lua.xlua_tointeger(L, 3);
				bool calculateBounds5 = Lua.lua_toboolean(L, 4);
				mesh.SetTriangles(triangles7, submesh7, calculateBounds5);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<ushort[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				ushort[] triangles8 = (ushort[])objectTranslator.GetObject(L, 2, typeof(ushort[]));
				int submesh8 = Lua.xlua_tointeger(L, 3);
				mesh.SetTriangles(triangles8, submesh8);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<List<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				List<int> triangles9 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				int submesh9 = Lua.xlua_tointeger(L, 3);
				bool calculateBounds6 = Lua.lua_toboolean(L, 4);
				int baseVertex3 = Lua.xlua_tointeger(L, 5);
				mesh.SetTriangles(triangles9, submesh9, calculateBounds6, baseVertex3);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<List<ushort>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				List<ushort> triangles10 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				int submesh10 = Lua.xlua_tointeger(L, 3);
				bool calculateBounds7 = Lua.lua_toboolean(L, 4);
				int baseVertex4 = Lua.xlua_tointeger(L, 5);
				mesh.SetTriangles(triangles10, submesh10, calculateBounds7, baseVertex4);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<List<ushort>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				List<ushort> triangles11 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				int submesh11 = Lua.xlua_tointeger(L, 3);
				bool calculateBounds8 = Lua.lua_toboolean(L, 4);
				mesh.SetTriangles(triangles11, submesh11, calculateBounds8);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<List<ushort>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				List<ushort> triangles12 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				int submesh12 = Lua.xlua_tointeger(L, 3);
				mesh.SetTriangles(triangles12, submesh12);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<int[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				int[] triangles13 = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
				int trianglesStart = Lua.xlua_tointeger(L, 3);
				int trianglesLength = Lua.xlua_tointeger(L, 4);
				int submesh13 = Lua.xlua_tointeger(L, 5);
				bool calculateBounds9 = Lua.lua_toboolean(L, 6);
				int baseVertex5 = Lua.xlua_tointeger(L, 7);
				mesh.SetTriangles(triangles13, trianglesStart, trianglesLength, submesh13, calculateBounds9, baseVertex5);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<int[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				int[] triangles14 = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
				int trianglesStart2 = Lua.xlua_tointeger(L, 3);
				int trianglesLength2 = Lua.xlua_tointeger(L, 4);
				int submesh14 = Lua.xlua_tointeger(L, 5);
				bool calculateBounds10 = Lua.lua_toboolean(L, 6);
				mesh.SetTriangles(triangles14, trianglesStart2, trianglesLength2, submesh14, calculateBounds10);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<int[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int[] triangles15 = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
				int trianglesStart3 = Lua.xlua_tointeger(L, 3);
				int trianglesLength3 = Lua.xlua_tointeger(L, 4);
				int submesh15 = Lua.xlua_tointeger(L, 5);
				mesh.SetTriangles(triangles15, trianglesStart3, trianglesLength3, submesh15);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<ushort[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				ushort[] triangles16 = (ushort[])objectTranslator.GetObject(L, 2, typeof(ushort[]));
				int trianglesStart4 = Lua.xlua_tointeger(L, 3);
				int trianglesLength4 = Lua.xlua_tointeger(L, 4);
				int submesh16 = Lua.xlua_tointeger(L, 5);
				bool calculateBounds11 = Lua.lua_toboolean(L, 6);
				int baseVertex6 = Lua.xlua_tointeger(L, 7);
				mesh.SetTriangles(triangles16, trianglesStart4, trianglesLength4, submesh16, calculateBounds11, baseVertex6);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<ushort[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				ushort[] triangles17 = (ushort[])objectTranslator.GetObject(L, 2, typeof(ushort[]));
				int trianglesStart5 = Lua.xlua_tointeger(L, 3);
				int trianglesLength5 = Lua.xlua_tointeger(L, 4);
				int submesh17 = Lua.xlua_tointeger(L, 5);
				bool calculateBounds12 = Lua.lua_toboolean(L, 6);
				mesh.SetTriangles(triangles17, trianglesStart5, trianglesLength5, submesh17, calculateBounds12);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<ushort[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				ushort[] triangles18 = (ushort[])objectTranslator.GetObject(L, 2, typeof(ushort[]));
				int trianglesStart6 = Lua.xlua_tointeger(L, 3);
				int trianglesLength6 = Lua.xlua_tointeger(L, 4);
				int submesh18 = Lua.xlua_tointeger(L, 5);
				mesh.SetTriangles(triangles18, trianglesStart6, trianglesLength6, submesh18);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<List<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				List<int> triangles19 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				int trianglesStart7 = Lua.xlua_tointeger(L, 3);
				int trianglesLength7 = Lua.xlua_tointeger(L, 4);
				int submesh19 = Lua.xlua_tointeger(L, 5);
				bool calculateBounds13 = Lua.lua_toboolean(L, 6);
				int baseVertex7 = Lua.xlua_tointeger(L, 7);
				mesh.SetTriangles(triangles19, trianglesStart7, trianglesLength7, submesh19, calculateBounds13, baseVertex7);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<List<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				List<int> triangles20 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				int trianglesStart8 = Lua.xlua_tointeger(L, 3);
				int trianglesLength8 = Lua.xlua_tointeger(L, 4);
				int submesh20 = Lua.xlua_tointeger(L, 5);
				bool calculateBounds14 = Lua.lua_toboolean(L, 6);
				mesh.SetTriangles(triangles20, trianglesStart8, trianglesLength8, submesh20, calculateBounds14);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<List<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				List<int> triangles21 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				int trianglesStart9 = Lua.xlua_tointeger(L, 3);
				int trianglesLength9 = Lua.xlua_tointeger(L, 4);
				int submesh21 = Lua.xlua_tointeger(L, 5);
				mesh.SetTriangles(triangles21, trianglesStart9, trianglesLength9, submesh21);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<List<ushort>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				List<ushort> triangles22 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				int trianglesStart10 = Lua.xlua_tointeger(L, 3);
				int trianglesLength10 = Lua.xlua_tointeger(L, 4);
				int submesh22 = Lua.xlua_tointeger(L, 5);
				bool calculateBounds15 = Lua.lua_toboolean(L, 6);
				int baseVertex8 = Lua.xlua_tointeger(L, 7);
				mesh.SetTriangles(triangles22, trianglesStart10, trianglesLength10, submesh22, calculateBounds15, baseVertex8);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<List<ushort>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				List<ushort> triangles23 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				int trianglesStart11 = Lua.xlua_tointeger(L, 3);
				int trianglesLength11 = Lua.xlua_tointeger(L, 4);
				int submesh23 = Lua.xlua_tointeger(L, 5);
				bool calculateBounds16 = Lua.lua_toboolean(L, 6);
				mesh.SetTriangles(triangles23, trianglesStart11, trianglesLength11, submesh23, calculateBounds16);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<List<ushort>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				List<ushort> triangles24 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				int trianglesStart12 = Lua.xlua_tointeger(L, 3);
				int trianglesLength12 = Lua.xlua_tointeger(L, 4);
				int submesh24 = Lua.xlua_tointeger(L, 5);
				mesh.SetTriangles(triangles24, trianglesStart12, trianglesLength12, submesh24);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.SetTriangles!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIndices(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<int[]>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int[] indices = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
				objectTranslator.Get(L, 3, out MeshTopology v);
				int submesh = Lua.xlua_tointeger(L, 4);
				mesh.SetIndices(indices, v, submesh);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<int[]>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				int[] indices2 = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
				objectTranslator.Get(L, 3, out MeshTopology v2);
				int submesh2 = Lua.xlua_tointeger(L, 4);
				bool calculateBounds = Lua.lua_toboolean(L, 5);
				mesh.SetIndices(indices2, v2, submesh2, calculateBounds);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<int[]>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				int[] indices3 = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
				objectTranslator.Get(L, 3, out MeshTopology v3);
				int submesh3 = Lua.xlua_tointeger(L, 4);
				bool calculateBounds2 = Lua.lua_toboolean(L, 5);
				int baseVertex = Lua.xlua_tointeger(L, 6);
				mesh.SetIndices(indices3, v3, submesh3, calculateBounds2, baseVertex);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<ushort[]>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				ushort[] indices4 = (ushort[])objectTranslator.GetObject(L, 2, typeof(ushort[]));
				objectTranslator.Get(L, 3, out MeshTopology v4);
				int submesh4 = Lua.xlua_tointeger(L, 4);
				bool calculateBounds3 = Lua.lua_toboolean(L, 5);
				int baseVertex2 = Lua.xlua_tointeger(L, 6);
				mesh.SetIndices(indices4, v4, submesh4, calculateBounds3, baseVertex2);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<ushort[]>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				ushort[] indices5 = (ushort[])objectTranslator.GetObject(L, 2, typeof(ushort[]));
				objectTranslator.Get(L, 3, out MeshTopology v5);
				int submesh5 = Lua.xlua_tointeger(L, 4);
				bool calculateBounds4 = Lua.lua_toboolean(L, 5);
				mesh.SetIndices(indices5, v5, submesh5, calculateBounds4);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<ushort[]>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				ushort[] indices6 = (ushort[])objectTranslator.GetObject(L, 2, typeof(ushort[]));
				objectTranslator.Get(L, 3, out MeshTopology v6);
				int submesh6 = Lua.xlua_tointeger(L, 4);
				mesh.SetIndices(indices6, v6, submesh6);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<List<int>>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				List<int> indices7 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				objectTranslator.Get(L, 3, out MeshTopology v7);
				int submesh7 = Lua.xlua_tointeger(L, 4);
				bool calculateBounds5 = Lua.lua_toboolean(L, 5);
				int baseVertex3 = Lua.xlua_tointeger(L, 6);
				mesh.SetIndices(indices7, v7, submesh7, calculateBounds5, baseVertex3);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<List<int>>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				List<int> indices8 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				objectTranslator.Get(L, 3, out MeshTopology v8);
				int submesh8 = Lua.xlua_tointeger(L, 4);
				bool calculateBounds6 = Lua.lua_toboolean(L, 5);
				mesh.SetIndices(indices8, v8, submesh8, calculateBounds6);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<List<int>>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				List<int> indices9 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				objectTranslator.Get(L, 3, out MeshTopology v9);
				int submesh9 = Lua.xlua_tointeger(L, 4);
				mesh.SetIndices(indices9, v9, submesh9);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<List<ushort>>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				List<ushort> indices10 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				objectTranslator.Get(L, 3, out MeshTopology v10);
				int submesh10 = Lua.xlua_tointeger(L, 4);
				bool calculateBounds7 = Lua.lua_toboolean(L, 5);
				int baseVertex4 = Lua.xlua_tointeger(L, 6);
				mesh.SetIndices(indices10, v10, submesh10, calculateBounds7, baseVertex4);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<List<ushort>>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				List<ushort> indices11 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				objectTranslator.Get(L, 3, out MeshTopology v11);
				int submesh11 = Lua.xlua_tointeger(L, 4);
				bool calculateBounds8 = Lua.lua_toboolean(L, 5);
				mesh.SetIndices(indices11, v11, submesh11, calculateBounds8);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<List<ushort>>(L, 2) && objectTranslator.Assignable<MeshTopology>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				List<ushort> indices12 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				objectTranslator.Get(L, 3, out MeshTopology v12);
				int submesh12 = Lua.xlua_tointeger(L, 4);
				mesh.SetIndices(indices12, v12, submesh12);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<int[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<MeshTopology>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				int[] indices13 = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
				int indicesStart = Lua.xlua_tointeger(L, 3);
				int indicesLength = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out MeshTopology v13);
				int submesh13 = Lua.xlua_tointeger(L, 6);
				bool calculateBounds9 = Lua.lua_toboolean(L, 7);
				int baseVertex5 = Lua.xlua_tointeger(L, 8);
				mesh.SetIndices(indices13, indicesStart, indicesLength, v13, submesh13, calculateBounds9, baseVertex5);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<int[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<MeshTopology>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				int[] indices14 = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
				int indicesStart2 = Lua.xlua_tointeger(L, 3);
				int indicesLength2 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out MeshTopology v14);
				int submesh14 = Lua.xlua_tointeger(L, 6);
				bool calculateBounds10 = Lua.lua_toboolean(L, 7);
				mesh.SetIndices(indices14, indicesStart2, indicesLength2, v14, submesh14, calculateBounds10);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<int[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<MeshTopology>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				int[] indices15 = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
				int indicesStart3 = Lua.xlua_tointeger(L, 3);
				int indicesLength3 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out MeshTopology v15);
				int submesh15 = Lua.xlua_tointeger(L, 6);
				mesh.SetIndices(indices15, indicesStart3, indicesLength3, v15, submesh15);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<ushort[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<MeshTopology>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				ushort[] indices16 = (ushort[])objectTranslator.GetObject(L, 2, typeof(ushort[]));
				int indicesStart4 = Lua.xlua_tointeger(L, 3);
				int indicesLength4 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out MeshTopology v16);
				int submesh16 = Lua.xlua_tointeger(L, 6);
				bool calculateBounds11 = Lua.lua_toboolean(L, 7);
				int baseVertex6 = Lua.xlua_tointeger(L, 8);
				mesh.SetIndices(indices16, indicesStart4, indicesLength4, v16, submesh16, calculateBounds11, baseVertex6);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<ushort[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<MeshTopology>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				ushort[] indices17 = (ushort[])objectTranslator.GetObject(L, 2, typeof(ushort[]));
				int indicesStart5 = Lua.xlua_tointeger(L, 3);
				int indicesLength5 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out MeshTopology v17);
				int submesh17 = Lua.xlua_tointeger(L, 6);
				bool calculateBounds12 = Lua.lua_toboolean(L, 7);
				mesh.SetIndices(indices17, indicesStart5, indicesLength5, v17, submesh17, calculateBounds12);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<ushort[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<MeshTopology>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				ushort[] indices18 = (ushort[])objectTranslator.GetObject(L, 2, typeof(ushort[]));
				int indicesStart6 = Lua.xlua_tointeger(L, 3);
				int indicesLength6 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out MeshTopology v18);
				int submesh18 = Lua.xlua_tointeger(L, 6);
				mesh.SetIndices(indices18, indicesStart6, indicesLength6, v18, submesh18);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<List<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<MeshTopology>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				List<int> indices19 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				int indicesStart7 = Lua.xlua_tointeger(L, 3);
				int indicesLength7 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out MeshTopology v19);
				int submesh19 = Lua.xlua_tointeger(L, 6);
				bool calculateBounds13 = Lua.lua_toboolean(L, 7);
				int baseVertex7 = Lua.xlua_tointeger(L, 8);
				mesh.SetIndices(indices19, indicesStart7, indicesLength7, v19, submesh19, calculateBounds13, baseVertex7);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<List<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<MeshTopology>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				List<int> indices20 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				int indicesStart8 = Lua.xlua_tointeger(L, 3);
				int indicesLength8 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out MeshTopology v20);
				int submesh20 = Lua.xlua_tointeger(L, 6);
				bool calculateBounds14 = Lua.lua_toboolean(L, 7);
				mesh.SetIndices(indices20, indicesStart8, indicesLength8, v20, submesh20, calculateBounds14);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<List<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<MeshTopology>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				List<int> indices21 = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
				int indicesStart9 = Lua.xlua_tointeger(L, 3);
				int indicesLength9 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out MeshTopology v21);
				int submesh21 = Lua.xlua_tointeger(L, 6);
				mesh.SetIndices(indices21, indicesStart9, indicesLength9, v21, submesh21);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<List<ushort>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<MeshTopology>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				List<ushort> indices22 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				int indicesStart10 = Lua.xlua_tointeger(L, 3);
				int indicesLength10 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out MeshTopology v22);
				int submesh22 = Lua.xlua_tointeger(L, 6);
				bool calculateBounds15 = Lua.lua_toboolean(L, 7);
				int baseVertex8 = Lua.xlua_tointeger(L, 8);
				mesh.SetIndices(indices22, indicesStart10, indicesLength10, v22, submesh22, calculateBounds15, baseVertex8);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<List<ushort>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<MeshTopology>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				List<ushort> indices23 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				int indicesStart11 = Lua.xlua_tointeger(L, 3);
				int indicesLength11 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out MeshTopology v23);
				int submesh23 = Lua.xlua_tointeger(L, 6);
				bool calculateBounds16 = Lua.lua_toboolean(L, 7);
				mesh.SetIndices(indices23, indicesStart11, indicesLength11, v23, submesh23, calculateBounds16);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<List<ushort>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<MeshTopology>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				List<ushort> indices24 = (List<ushort>)objectTranslator.GetObject(L, 2, typeof(List<ushort>));
				int indicesStart12 = Lua.xlua_tointeger(L, 3);
				int indicesLength12 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out MeshTopology v24);
				int submesh24 = Lua.xlua_tointeger(L, 6);
				mesh.SetIndices(indices24, indicesStart12, indicesLength12, v24, submesh24);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.SetIndices!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBindposes(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			List<Matrix4x4> bindposes = (List<Matrix4x4>)objectTranslator.GetObject(L, 2, typeof(List<Matrix4x4>));
			mesh.GetBindposes(bindposes);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBoneWeights(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			List<BoneWeight> boneWeights = (List<BoneWeight>)objectTranslator.GetObject(L, 2, typeof(List<BoneWeight>));
			mesh.GetBoneWeights(boneWeights);
			return 0;
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
			Mesh mesh = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				mesh.Clear();
				return 0;
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool keepVertexLayout = Lua.lua_toboolean(L, 2);
					mesh.Clear(keepVertexLayout);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.Clear!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecalculateBounds(IntPtr L)
	{
		try
		{
			((Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RecalculateBounds();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecalculateNormals(IntPtr L)
	{
		try
		{
			((Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RecalculateNormals();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecalculateTangents(IntPtr L)
	{
		try
		{
			((Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RecalculateTangents();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MarkDynamic(IntPtr L)
	{
		try
		{
			((Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).MarkDynamic();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UploadMeshData(IntPtr L)
	{
		try
		{
			Mesh obj = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool markNoLongerReadable = Lua.lua_toboolean(L, 2);
			obj.UploadMeshData(markNoLongerReadable);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Optimize(IntPtr L)
	{
		try
		{
			((Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Optimize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OptimizeIndexBuffers(IntPtr L)
	{
		try
		{
			((Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OptimizeIndexBuffers();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OptimizeReorderVertexBuffer(IntPtr L)
	{
		try
		{
			((Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OptimizeReorderVertexBuffer();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTopology(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh obj = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int submesh = Lua.xlua_tointeger(L, 2);
			MeshTopology topology = obj.GetTopology(submesh);
			objectTranslator.Push(L, topology);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CombineMeshes(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<CombineInstance[]>(L, 2))
			{
				CombineInstance[] combine = (CombineInstance[])objectTranslator.GetObject(L, 2, typeof(CombineInstance[]));
				mesh.CombineMeshes(combine);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<CombineInstance[]>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				CombineInstance[] combine2 = (CombineInstance[])objectTranslator.GetObject(L, 2, typeof(CombineInstance[]));
				bool mergeSubMeshes = Lua.lua_toboolean(L, 3);
				mesh.CombineMeshes(combine2, mergeSubMeshes);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<CombineInstance[]>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				CombineInstance[] combine3 = (CombineInstance[])objectTranslator.GetObject(L, 2, typeof(CombineInstance[]));
				bool mergeSubMeshes2 = Lua.lua_toboolean(L, 3);
				bool useMatrices = Lua.lua_toboolean(L, 4);
				mesh.CombineMeshes(combine3, mergeSubMeshes2, useMatrices);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<CombineInstance[]>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				CombineInstance[] combine4 = (CombineInstance[])objectTranslator.GetObject(L, 2, typeof(CombineInstance[]));
				bool mergeSubMeshes3 = Lua.lua_toboolean(L, 3);
				bool useMatrices2 = Lua.lua_toboolean(L, 4);
				bool hasLightmapData = Lua.lua_toboolean(L, 5);
				mesh.CombineMeshes(combine4, mergeSubMeshes3, useMatrices2, hasLightmapData);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Mesh.CombineMeshes!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_indexFormat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.indexFormat);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_vertexBufferCount(IntPtr L)
	{
		try
		{
			Mesh mesh = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, mesh.vertexBufferCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_blendShapeCount(IntPtr L)
	{
		try
		{
			Mesh mesh = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, mesh.blendShapeCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bindposes(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.bindposes);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isReadable(IntPtr L)
	{
		try
		{
			Mesh mesh = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, mesh.isReadable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_vertexCount(IntPtr L)
	{
		try
		{
			Mesh mesh = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, mesh.vertexCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_subMeshCount(IntPtr L)
	{
		try
		{
			Mesh mesh = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, mesh.subMeshCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineBounds(L, mesh.bounds);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_vertices(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.vertices);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_normals(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.normals);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_tangents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.tangents);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uv(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.uv);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uv2(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.uv2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uv3(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.uv3);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uv4(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.uv4);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uv5(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.uv5);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uv6(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.uv6);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uv7(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.uv7);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uv8(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.uv8);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colors(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.colors);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colors32(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.colors32);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_vertexAttributeCount(IntPtr L)
	{
		try
		{
			Mesh mesh = (Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, mesh.vertexAttributeCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_triangles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.triangles);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_boneWeights(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mesh.boneWeights);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_indexFormat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out IndexFormat v);
			mesh.indexFormat = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bindposes(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).bindposes = (Matrix4x4[])objectTranslator.GetObject(L, 2, typeof(Matrix4x4[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_subMeshCount(IntPtr L)
	{
		try
		{
			((Mesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).subMeshCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh mesh = (Mesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Bounds val);
			mesh.bounds = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_vertices(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).vertices = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_normals(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).normals = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tangents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).tangents = (Vector4[])objectTranslator.GetObject(L, 2, typeof(Vector4[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uv(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).uv = (Vector2[])objectTranslator.GetObject(L, 2, typeof(Vector2[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uv2(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).uv2 = (Vector2[])objectTranslator.GetObject(L, 2, typeof(Vector2[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uv3(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).uv3 = (Vector2[])objectTranslator.GetObject(L, 2, typeof(Vector2[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uv4(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).uv4 = (Vector2[])objectTranslator.GetObject(L, 2, typeof(Vector2[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uv5(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).uv5 = (Vector2[])objectTranslator.GetObject(L, 2, typeof(Vector2[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uv6(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).uv6 = (Vector2[])objectTranslator.GetObject(L, 2, typeof(Vector2[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uv7(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).uv7 = (Vector2[])objectTranslator.GetObject(L, 2, typeof(Vector2[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uv8(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).uv8 = (Vector2[])objectTranslator.GetObject(L, 2, typeof(Vector2[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colors(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).colors = (Color[])objectTranslator.GetObject(L, 2, typeof(Color[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colors32(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).colors32 = (Color32[])objectTranslator.GetObject(L, 2, typeof(Color32[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_triangles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).triangles = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_boneWeights(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Mesh)objectTranslator.FastGetCSObj(L, 1)).boneWeights = (BoneWeight[])objectTranslator.GetObject(L, 2, typeof(BoneWeight[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
