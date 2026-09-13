using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Runtime/Graphics/Mesh/MeshScriptBindings.h")]
[RequiredByNativeCode]
public sealed class Mesh : Object
{
	public extern IndexFormat indexFormat
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int vertexBufferCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "MeshScripting::GetVertexBufferCount", HasExplicitThis = true)]
		get;
	}

	public extern int blendShapeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(Name = "GetBlendShapeChannelCount")]
		get;
	}

	[NativeName("BindPosesFromScript")]
	public extern Matrix4x4[] bindposes
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool isReadable
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetIsReadable")]
		get;
	}

	internal extern bool canAccess
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("CanAccessFromScript")]
		get;
	}

	public extern int vertexCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetVertexCount")]
		get;
	}

	public extern int subMeshCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(Name = "GetSubMeshCount")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "MeshScripting::SetSubMeshCount", HasExplicitThis = true)]
		set;
	}

	public Bounds bounds
	{
		get
		{
			get_bounds_Injected(out var ret);
			return ret;
		}
		set
		{
			set_bounds_Injected(ref value);
		}
	}

	public Vector3[] vertices
	{
		get
		{
			return GetAllocArrayFromChannel<Vector3>(VertexAttribute.Position);
		}
		set
		{
			SetArrayForChannel(VertexAttribute.Position, value);
		}
	}

	public Vector3[] normals
	{
		get
		{
			return GetAllocArrayFromChannel<Vector3>(VertexAttribute.Normal);
		}
		set
		{
			SetArrayForChannel(VertexAttribute.Normal, value);
		}
	}

	public Vector4[] tangents
	{
		get
		{
			return GetAllocArrayFromChannel<Vector4>(VertexAttribute.Tangent);
		}
		set
		{
			SetArrayForChannel(VertexAttribute.Tangent, value);
		}
	}

	public Vector2[] uv
	{
		get
		{
			return GetAllocArrayFromChannel<Vector2>(VertexAttribute.TexCoord0);
		}
		set
		{
			SetArrayForChannel(VertexAttribute.TexCoord0, value);
		}
	}

	public Vector2[] uv2
	{
		get
		{
			return GetAllocArrayFromChannel<Vector2>(VertexAttribute.TexCoord1);
		}
		set
		{
			SetArrayForChannel(VertexAttribute.TexCoord1, value);
		}
	}

	public Vector2[] uv3
	{
		get
		{
			return GetAllocArrayFromChannel<Vector2>(VertexAttribute.TexCoord2);
		}
		set
		{
			SetArrayForChannel(VertexAttribute.TexCoord2, value);
		}
	}

	public Vector2[] uv4
	{
		get
		{
			return GetAllocArrayFromChannel<Vector2>(VertexAttribute.TexCoord3);
		}
		set
		{
			SetArrayForChannel(VertexAttribute.TexCoord3, value);
		}
	}

	public Vector2[] uv5
	{
		get
		{
			return GetAllocArrayFromChannel<Vector2>(VertexAttribute.TexCoord4);
		}
		set
		{
			SetArrayForChannel(VertexAttribute.TexCoord4, value);
		}
	}

	public Vector2[] uv6
	{
		get
		{
			return GetAllocArrayFromChannel<Vector2>(VertexAttribute.TexCoord5);
		}
		set
		{
			SetArrayForChannel(VertexAttribute.TexCoord5, value);
		}
	}

	public Vector2[] uv7
	{
		get
		{
			return GetAllocArrayFromChannel<Vector2>(VertexAttribute.TexCoord6);
		}
		set
		{
			SetArrayForChannel(VertexAttribute.TexCoord6, value);
		}
	}

	public Vector2[] uv8
	{
		get
		{
			return GetAllocArrayFromChannel<Vector2>(VertexAttribute.TexCoord7);
		}
		set
		{
			SetArrayForChannel(VertexAttribute.TexCoord7, value);
		}
	}

	public Color[] colors
	{
		get
		{
			return GetAllocArrayFromChannel<Color>(VertexAttribute.Color);
		}
		set
		{
			SetArrayForChannel(VertexAttribute.Color, value);
		}
	}

	public Color32[] colors32
	{
		get
		{
			return GetAllocArrayFromChannel<Color32>(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4);
		}
		set
		{
			SetArrayForChannel(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4, value);
		}
	}

	public int vertexAttributeCount => GetVertexAttributeCountImpl();

	public int[] triangles
	{
		get
		{
			if (canAccess)
			{
				return GetTrianglesImpl(-1, applyBaseVertex: true);
			}
			PrintErrorCantAccessIndices();
			return new int[0];
		}
		set
		{
			if (canAccess)
			{
				SetTrianglesImpl(-1, IndexFormat.UInt32, value, NoAllocHelpers.SafeLength(value), 0, NoAllocHelpers.SafeLength(value), calculateBounds: true, 0);
			}
			else
			{
				PrintErrorCantAccessIndices();
			}
		}
	}

	public BoneWeight[] boneWeights
	{
		get
		{
			return GetBoneWeightsImpl();
		}
		set
		{
			SetBoneWeightsImpl(value);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("MeshScripting::CreateMesh")]
	private static extern void Internal_Create([Writable] Mesh mono);

	[RequiredByNativeCode]
	public Mesh()
	{
		Internal_Create(this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("MeshScripting::MeshFromInstanceId")]
	internal static extern Mesh FromInstanceID(int id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::SetIndexBufferParams", HasExplicitThis = true)]
	public extern void SetIndexBufferParams(int indexCount, IndexFormat format);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::InternalSetIndexBufferData", HasExplicitThis = true, ThrowsException = true)]
	private extern void InternalSetIndexBufferData(IntPtr data, int dataStart, int meshBufferStart, int count, int elemSize, MeshUpdateFlags flags);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::InternalSetIndexBufferDataFromArray", HasExplicitThis = true, ThrowsException = true)]
	private extern void InternalSetIndexBufferDataFromArray(Array data, int dataStart, int meshBufferStart, int count, int elemSize, MeshUpdateFlags flags);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::SetVertexBufferParams", HasExplicitThis = true, ThrowsException = true)]
	public extern void SetVertexBufferParams(int vertexCount, params VertexAttributeDescriptor[] attributes);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::InternalSetVertexBufferData", HasExplicitThis = true)]
	private extern void InternalSetVertexBufferData(int stream, IntPtr data, int dataStart, int meshBufferStart, int count, int elemSize, MeshUpdateFlags flags);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::InternalSetVertexBufferDataFromArray", HasExplicitThis = true)]
	private extern void InternalSetVertexBufferDataFromArray(int stream, Array data, int dataStart, int meshBufferStart, int count, int elemSize, MeshUpdateFlags flags);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetVertexAttributesAlloc", HasExplicitThis = true)]
	private extern Array GetVertexAttributesAlloc();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetVertexAttributesArray", HasExplicitThis = true)]
	private extern int GetVertexAttributesArray([NotNull] VertexAttributeDescriptor[] attributes);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetVertexAttributesList", HasExplicitThis = true)]
	private extern int GetVertexAttributesList([NotNull] List<VertexAttributeDescriptor> attributes);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetVertexAttributesCount", HasExplicitThis = true)]
	private extern int GetVertexAttributeCountImpl();

	[FreeFunction(Name = "MeshScripting::GetVertexAttributeByIndex", HasExplicitThis = true)]
	public VertexAttributeDescriptor GetVertexAttribute(int index)
	{
		GetVertexAttribute_Injected(index, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetIndexStart", HasExplicitThis = true)]
	private extern uint GetIndexStartImpl(int submesh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetIndexCount", HasExplicitThis = true)]
	private extern uint GetIndexCountImpl(int submesh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetTrianglesCount", HasExplicitThis = true)]
	private extern uint GetTrianglesCountImpl(int submesh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetBaseVertex", HasExplicitThis = true)]
	private extern uint GetBaseVertexImpl(int submesh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetTriangles", HasExplicitThis = true)]
	private extern int[] GetTrianglesImpl(int submesh, bool applyBaseVertex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetIndices", HasExplicitThis = true)]
	private extern int[] GetIndicesImpl(int submesh, bool applyBaseVertex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "SetMeshIndicesFromScript", HasExplicitThis = true)]
	private extern void SetIndicesImpl(int submesh, MeshTopology topology, IndexFormat indicesFormat, Array indices, int arrayStart, int arraySize, bool calculateBounds, int baseVertex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "SetMeshIndicesFromNativeArray", HasExplicitThis = true)]
	private extern void SetIndicesNativeArrayImpl(int submesh, MeshTopology topology, IndexFormat indicesFormat, IntPtr indices, int arrayStart, int arraySize, bool calculateBounds, int baseVertex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::ExtractTrianglesToArray", HasExplicitThis = true)]
	private extern void GetTrianglesNonAllocImpl([Out] int[] values, int submesh, bool applyBaseVertex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::ExtractTrianglesToArray16", HasExplicitThis = true)]
	private extern void GetTrianglesNonAllocImpl16([Out] ushort[] values, int submesh, bool applyBaseVertex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::ExtractIndicesToArray", HasExplicitThis = true)]
	private extern void GetIndicesNonAllocImpl([Out] int[] values, int submesh, bool applyBaseVertex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::ExtractIndicesToArray16", HasExplicitThis = true)]
	private extern void GetIndicesNonAllocImpl16([Out] ushort[] values, int submesh, bool applyBaseVertex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::PrintErrorCantAccessChannel", HasExplicitThis = true)]
	private extern void PrintErrorCantAccessChannel(VertexAttribute ch);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::HasChannel", HasExplicitThis = true)]
	public extern bool HasVertexAttribute(VertexAttribute attr);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetChannelDimension", HasExplicitThis = true)]
	public extern int GetVertexAttributeDimension(VertexAttribute attr);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetChannelFormat", HasExplicitThis = true)]
	public extern VertexAttributeFormat GetVertexAttributeFormat(VertexAttribute attr);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "SetMeshComponentFromArrayFromScript", HasExplicitThis = true)]
	private extern void SetArrayForChannelImpl(VertexAttribute channel, VertexAttributeFormat format, int dim, Array values, int arraySize, int valuesStart, int valuesCount);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "SetMeshComponentFromNativeArrayFromScript", HasExplicitThis = true)]
	private extern void SetNativeArrayForChannelImpl(VertexAttribute channel, VertexAttributeFormat format, int dim, IntPtr values, int arraySize, int valuesStart, int valuesCount);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "AllocExtractMeshComponentFromScript", HasExplicitThis = true)]
	private extern Array GetAllocArrayFromChannelImpl(VertexAttribute channel, VertexAttributeFormat format, int dim);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ExtractMeshComponentFromScript", HasExplicitThis = true)]
	private extern void GetArrayFromChannelImpl(VertexAttribute channel, VertexAttributeFormat format, int dim, Array values);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetNativeVertexBufferPtr", HasExplicitThis = true)]
	[NativeThrows]
	public extern IntPtr GetNativeVertexBufferPtr(int index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetNativeIndexBufferPtr", HasExplicitThis = true)]
	public extern IntPtr GetNativeIndexBufferPtr();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::ClearBlendShapes", HasExplicitThis = true)]
	public extern void ClearBlendShapes();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetBlendShapeName", HasExplicitThis = true)]
	public extern string GetBlendShapeName(int shapeIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetBlendShapeIndex", HasExplicitThis = true)]
	public extern int GetBlendShapeIndex(string blendShapeName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetBlendShapeFrameCount", HasExplicitThis = true)]
	public extern int GetBlendShapeFrameCount(int shapeIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetBlendShapeFrameWeight", HasExplicitThis = true)]
	public extern float GetBlendShapeFrameWeight(int shapeIndex, int frameIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GetBlendShapeFrameVerticesFromScript", HasExplicitThis = true)]
	public extern void GetBlendShapeFrameVertices(int shapeIndex, int frameIndex, Vector3[] deltaVertices, Vector3[] deltaNormals, Vector3[] deltaTangents);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "AddBlendShapeFrameFromScript", HasExplicitThis = true)]
	public extern void AddBlendShapeFrame(string shapeName, float frameWeight, Vector3[] deltaVertices, Vector3[] deltaNormals, Vector3[] deltaTangents);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("HasBoneWeights")]
	private extern bool HasBoneWeights();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetBoneWeights", HasExplicitThis = true)]
	private extern BoneWeight[] GetBoneWeightsImpl();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::SetBoneWeights", HasExplicitThis = true)]
	private extern void SetBoneWeightsImpl(BoneWeight[] weights);

	public unsafe void SetBoneWeights(NativeArray<byte> bonesPerVertex, NativeArray<BoneWeight1> weights)
	{
		InternalSetBoneWeights((IntPtr)bonesPerVertex.GetUnsafeReadOnlyPtr(), bonesPerVertex.Length, (IntPtr)weights.GetUnsafeReadOnlyPtr(), weights.Length);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SecurityCritical]
	[FreeFunction(Name = "MeshScripting::SetBoneWeights", HasExplicitThis = true)]
	private extern void InternalSetBoneWeights(IntPtr bonesPerVertex, int bonesPerVertexSize, IntPtr weights, int weightsSize);

	public unsafe NativeArray<BoneWeight1> GetAllBoneWeights()
	{
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<BoneWeight1>((void*)GetAllBoneWeightsArray(), GetAllBoneWeightsArraySize(), Allocator.None);
	}

	public unsafe NativeArray<byte> GetBonesPerVertex()
	{
		int length = (HasBoneWeights() ? vertexCount : 0);
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>((void*)GetBonesPerVertexArray(), length, Allocator.None);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetAllBoneWeightsArraySize", HasExplicitThis = true)]
	private extern int GetAllBoneWeightsArraySize();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SecurityCritical]
	[FreeFunction(Name = "MeshScripting::GetAllBoneWeightsArray", HasExplicitThis = true)]
	private extern IntPtr GetAllBoneWeightsArray();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetBonesPerVertexArray", HasExplicitThis = true)]
	[SecurityCritical]
	private extern IntPtr GetBonesPerVertexArray();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern int GetBindposeCount();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::ExtractBoneWeightsIntoArray", HasExplicitThis = true)]
	private extern void GetBoneWeightsNonAllocImpl([Out] BoneWeight[] values);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::ExtractBindPosesIntoArray", HasExplicitThis = true)]
	private extern void GetBindposesNonAllocImpl([Out] Matrix4x4[] values);

	[FreeFunction("MeshScripting::SetSubMesh", HasExplicitThis = true, ThrowsException = true)]
	public void SetSubMesh(int index, SubMeshDescriptor desc, MeshUpdateFlags flags = MeshUpdateFlags.Default)
	{
		SetSubMesh_Injected(index, ref desc, flags);
	}

	[FreeFunction("MeshScripting::GetSubMesh", HasExplicitThis = true, ThrowsException = true)]
	public SubMeshDescriptor GetSubMesh(int index)
	{
		GetSubMesh_Injected(index, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("Clear")]
	private extern void ClearImpl(bool keepVertexLayout);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("RecalculateBounds")]
	private extern void RecalculateBoundsImpl();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("RecalculateNormals")]
	private extern void RecalculateNormalsImpl();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("RecalculateTangents")]
	private extern void RecalculateTangentsImpl();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("MarkDynamic")]
	private extern void MarkDynamicImpl();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("MarkModified")]
	public extern void MarkModified();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("UploadMeshData")]
	private extern void UploadMeshDataImpl(bool markNoLongerReadable);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::GetPrimitiveType", HasExplicitThis = true)]
	private extern MeshTopology GetTopologyImpl(int submesh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetMeshMetric")]
	public extern float GetUVDistributionMetric(int uvSetIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "MeshScripting::CombineMeshes", HasExplicitThis = true)]
	private extern void CombineMeshesImpl(CombineInstance[] combine, bool mergeSubMeshes, bool useMatrices, bool hasLightmapData);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("Optimize")]
	private extern void OptimizeImpl();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("OptimizeIndexBuffers")]
	private extern void OptimizeIndexBuffersImpl();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("OptimizeReorderVertexBuffer")]
	private extern void OptimizeReorderVertexBufferImpl();

	internal VertexAttribute GetUVChannel(int uvIndex)
	{
		if (uvIndex < 0 || uvIndex > 7)
		{
			throw new ArgumentException("GetUVChannel called for bad uvIndex", "uvIndex");
		}
		return (VertexAttribute)(4 + uvIndex);
	}

	internal static int DefaultDimensionForChannel(VertexAttribute channel)
	{
		if (channel == VertexAttribute.Position || channel == VertexAttribute.Normal)
		{
			return 3;
		}
		if (channel >= VertexAttribute.TexCoord0 && channel <= VertexAttribute.TexCoord7)
		{
			return 2;
		}
		if (channel == VertexAttribute.Tangent || channel == VertexAttribute.Color)
		{
			return 4;
		}
		throw new ArgumentException("DefaultDimensionForChannel called for bad channel", "channel");
	}

	private T[] GetAllocArrayFromChannel<T>(VertexAttribute channel, VertexAttributeFormat format, int dim)
	{
		if (canAccess)
		{
			if (HasVertexAttribute(channel))
			{
				return (T[])GetAllocArrayFromChannelImpl(channel, format, dim);
			}
		}
		else
		{
			PrintErrorCantAccessChannel(channel);
		}
		return new T[0];
	}

	private T[] GetAllocArrayFromChannel<T>(VertexAttribute channel)
	{
		return GetAllocArrayFromChannel<T>(channel, VertexAttributeFormat.Float32, DefaultDimensionForChannel(channel));
	}

	private void SetSizedArrayForChannel(VertexAttribute channel, VertexAttributeFormat format, int dim, Array values, int valuesArrayLength, int valuesStart, int valuesCount)
	{
		if (canAccess)
		{
			if (valuesStart < 0)
			{
				throw new ArgumentOutOfRangeException("valuesStart", valuesStart, "Mesh data array start index can't be negative.");
			}
			if (valuesCount < 0)
			{
				throw new ArgumentOutOfRangeException("valuesCount", valuesCount, "Mesh data array length can't be negative.");
			}
			if (valuesStart >= valuesArrayLength && valuesCount != 0)
			{
				throw new ArgumentOutOfRangeException("valuesStart", valuesStart, "Mesh data array start is outside of array size.");
			}
			if (valuesStart + valuesCount > valuesArrayLength)
			{
				throw new ArgumentOutOfRangeException("valuesCount", valuesStart + valuesCount, "Mesh data array start+count is outside of array size.");
			}
			if (values == null)
			{
				valuesStart = 0;
			}
			SetArrayForChannelImpl(channel, format, dim, values, valuesArrayLength, valuesStart, valuesCount);
		}
		else
		{
			PrintErrorCantAccessChannel(channel);
		}
	}

	private void SetSizedNativeArrayForChannel(VertexAttribute channel, VertexAttributeFormat format, int dim, IntPtr values, int valuesArrayLength, int valuesStart, int valuesCount)
	{
		if (canAccess)
		{
			if (valuesStart < 0)
			{
				throw new ArgumentOutOfRangeException("valuesStart", valuesStart, "Mesh data array start index can't be negative.");
			}
			if (valuesCount < 0)
			{
				throw new ArgumentOutOfRangeException("valuesCount", valuesCount, "Mesh data array length can't be negative.");
			}
			if (valuesStart >= valuesArrayLength && valuesCount != 0)
			{
				throw new ArgumentOutOfRangeException("valuesStart", valuesStart, "Mesh data array start is outside of array size.");
			}
			if (valuesStart + valuesCount > valuesArrayLength)
			{
				throw new ArgumentOutOfRangeException("valuesCount", valuesStart + valuesCount, "Mesh data array start+count is outside of array size.");
			}
			SetNativeArrayForChannelImpl(channel, format, dim, values, valuesArrayLength, valuesStart, valuesCount);
		}
		else
		{
			PrintErrorCantAccessChannel(channel);
		}
	}

	private void SetArrayForChannel<T>(VertexAttribute channel, VertexAttributeFormat format, int dim, T[] values)
	{
		int num = NoAllocHelpers.SafeLength(values);
		SetSizedArrayForChannel(channel, format, dim, values, num, 0, num);
	}

	private void SetArrayForChannel<T>(VertexAttribute channel, T[] values)
	{
		int num = NoAllocHelpers.SafeLength(values);
		SetSizedArrayForChannel(channel, VertexAttributeFormat.Float32, DefaultDimensionForChannel(channel), values, num, 0, num);
	}

	private void SetListForChannel<T>(VertexAttribute channel, VertexAttributeFormat format, int dim, List<T> values, int start, int length)
	{
		SetSizedArrayForChannel(channel, format, dim, NoAllocHelpers.ExtractArrayFromList(values), NoAllocHelpers.SafeLength(values), start, length);
	}

	private void SetListForChannel<T>(VertexAttribute channel, List<T> values, int start, int length)
	{
		SetSizedArrayForChannel(channel, VertexAttributeFormat.Float32, DefaultDimensionForChannel(channel), NoAllocHelpers.ExtractArrayFromList(values), NoAllocHelpers.SafeLength(values), start, length);
	}

	private void GetListForChannel<T>(List<T> buffer, int capacity, VertexAttribute channel, int dim)
	{
		GetListForChannel(buffer, capacity, channel, dim, VertexAttributeFormat.Float32);
	}

	private void GetListForChannel<T>(List<T> buffer, int capacity, VertexAttribute channel, int dim, VertexAttributeFormat channelType)
	{
		buffer.Clear();
		if (!canAccess)
		{
			PrintErrorCantAccessChannel(channel);
		}
		else if (HasVertexAttribute(channel))
		{
			NoAllocHelpers.EnsureListElemCount(buffer, capacity);
			GetArrayFromChannelImpl(channel, channelType, dim, NoAllocHelpers.ExtractArrayFromList(buffer));
		}
	}

	public void GetVertices(List<Vector3> vertices)
	{
		if (vertices == null)
		{
			throw new ArgumentNullException("The result vertices list cannot be null.", "vertices");
		}
		GetListForChannel(vertices, vertexCount, VertexAttribute.Position, DefaultDimensionForChannel(VertexAttribute.Position));
	}

	public void SetVertices(List<Vector3> inVertices)
	{
		SetVertices(inVertices, 0, NoAllocHelpers.SafeLength(inVertices));
	}

	public void SetVertices(List<Vector3> inVertices, int start, int length)
	{
		SetListForChannel(VertexAttribute.Position, inVertices, start, length);
	}

	public void SetVertices(Vector3[] inVertices)
	{
		SetVertices(inVertices, 0, NoAllocHelpers.SafeLength(inVertices));
	}

	public void SetVertices(Vector3[] inVertices, int start, int length)
	{
		SetSizedArrayForChannel(VertexAttribute.Position, VertexAttributeFormat.Float32, DefaultDimensionForChannel(VertexAttribute.Position), inVertices, NoAllocHelpers.SafeLength(inVertices), start, length);
	}

	public void SetVertices<T>(NativeArray<T> inVertices) where T : struct
	{
		SetVertices(inVertices, 0, inVertices.Length);
	}

	public unsafe void SetVertices<T>(NativeArray<T> inVertices, int start, int length) where T : struct
	{
		if (UnsafeUtility.SizeOf<T>() != 12)
		{
			throw new ArgumentException("SetVertices with NativeArray should use struct type that is 12 bytes (3x float) in size");
		}
		SetSizedNativeArrayForChannel(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, (IntPtr)inVertices.GetUnsafeReadOnlyPtr(), inVertices.Length, start, length);
	}

	public void GetNormals(List<Vector3> normals)
	{
		if (normals == null)
		{
			throw new ArgumentNullException("The result normals list cannot be null.", "normals");
		}
		GetListForChannel(normals, vertexCount, VertexAttribute.Normal, DefaultDimensionForChannel(VertexAttribute.Normal));
	}

	public void SetNormals(List<Vector3> inNormals)
	{
		SetNormals(inNormals, 0, NoAllocHelpers.SafeLength(inNormals));
	}

	public void SetNormals(List<Vector3> inNormals, int start, int length)
	{
		SetListForChannel(VertexAttribute.Normal, inNormals, start, length);
	}

	public void SetNormals(Vector3[] inNormals)
	{
		SetNormals(inNormals, 0, NoAllocHelpers.SafeLength(inNormals));
	}

	public void SetNormals(Vector3[] inNormals, int start, int length)
	{
		SetSizedArrayForChannel(VertexAttribute.Normal, VertexAttributeFormat.Float32, DefaultDimensionForChannel(VertexAttribute.Normal), inNormals, NoAllocHelpers.SafeLength(inNormals), start, length);
	}

	public void SetNormals<T>(NativeArray<T> inNormals) where T : struct
	{
		SetNormals(inNormals, 0, inNormals.Length);
	}

	public unsafe void SetNormals<T>(NativeArray<T> inNormals, int start, int length) where T : struct
	{
		if (UnsafeUtility.SizeOf<T>() != 12)
		{
			throw new ArgumentException("SetNormals with NativeArray should use struct type that is 12 bytes (3x float) in size");
		}
		SetSizedNativeArrayForChannel(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3, (IntPtr)inNormals.GetUnsafeReadOnlyPtr(), inNormals.Length, start, length);
	}

	public void GetTangents(List<Vector4> tangents)
	{
		if (tangents == null)
		{
			throw new ArgumentNullException("The result tangents list cannot be null.", "tangents");
		}
		GetListForChannel(tangents, vertexCount, VertexAttribute.Tangent, DefaultDimensionForChannel(VertexAttribute.Tangent));
	}

	public void SetTangents(List<Vector4> inTangents)
	{
		SetTangents(inTangents, 0, NoAllocHelpers.SafeLength(inTangents));
	}

	public void SetTangents(List<Vector4> inTangents, int start, int length)
	{
		SetListForChannel(VertexAttribute.Tangent, inTangents, start, length);
	}

	public void SetTangents(Vector4[] inTangents)
	{
		SetTangents(inTangents, 0, NoAllocHelpers.SafeLength(inTangents));
	}

	public void SetTangents(Vector4[] inTangents, int start, int length)
	{
		SetSizedArrayForChannel(VertexAttribute.Tangent, VertexAttributeFormat.Float32, DefaultDimensionForChannel(VertexAttribute.Tangent), inTangents, NoAllocHelpers.SafeLength(inTangents), start, length);
	}

	public void SetTangents<T>(NativeArray<T> inTangents) where T : struct
	{
		SetTangents(inTangents, 0, inTangents.Length);
	}

	public unsafe void SetTangents<T>(NativeArray<T> inTangents, int start, int length) where T : struct
	{
		if (UnsafeUtility.SizeOf<T>() != 16)
		{
			throw new ArgumentException("SetTangents with NativeArray should use struct type that is 16 bytes (4x float) in size");
		}
		SetSizedNativeArrayForChannel(VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4, (IntPtr)inTangents.GetUnsafeReadOnlyPtr(), inTangents.Length, start, length);
	}

	public void GetColors(List<Color> colors)
	{
		if (colors == null)
		{
			throw new ArgumentNullException("The result colors list cannot be null.", "colors");
		}
		GetListForChannel(colors, vertexCount, VertexAttribute.Color, DefaultDimensionForChannel(VertexAttribute.Color));
	}

	public void SetColors(List<Color> inColors)
	{
		SetColors(inColors, 0, NoAllocHelpers.SafeLength(inColors));
	}

	public void SetColors(List<Color> inColors, int start, int length)
	{
		SetListForChannel(VertexAttribute.Color, inColors, start, length);
	}

	public void SetColors(Color[] inColors)
	{
		SetColors(inColors, 0, NoAllocHelpers.SafeLength(inColors));
	}

	public void SetColors(Color[] inColors, int start, int length)
	{
		SetSizedArrayForChannel(VertexAttribute.Color, VertexAttributeFormat.Float32, DefaultDimensionForChannel(VertexAttribute.Color), inColors, NoAllocHelpers.SafeLength(inColors), start, length);
	}

	public void GetColors(List<Color32> colors)
	{
		if (colors == null)
		{
			throw new ArgumentNullException("The result colors list cannot be null.", "colors");
		}
		GetListForChannel(colors, vertexCount, VertexAttribute.Color, 4, VertexAttributeFormat.UNorm8);
	}

	public void SetColors(List<Color32> inColors)
	{
		SetColors(inColors, 0, NoAllocHelpers.SafeLength(inColors));
	}

	public void SetColors(List<Color32> inColors, int start, int length)
	{
		SetListForChannel(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4, inColors, start, length);
	}

	public void SetColors(Color32[] inColors)
	{
		SetColors(inColors, 0, NoAllocHelpers.SafeLength(inColors));
	}

	public void SetColors(Color32[] inColors, int start, int length)
	{
		SetSizedArrayForChannel(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4, inColors, NoAllocHelpers.SafeLength(inColors), start, length);
	}

	public void SetColors<T>(NativeArray<T> inColors) where T : struct
	{
		SetColors(inColors, 0, inColors.Length);
	}

	public unsafe void SetColors<T>(NativeArray<T> inColors, int start, int length) where T : struct
	{
		int num = UnsafeUtility.SizeOf<T>();
		if (num != 16 && num != 4)
		{
			throw new ArgumentException("SetColors with NativeArray should use struct type that is 16 bytes (4x float) or 4 bytes (4x unorm) in size");
		}
		SetSizedNativeArrayForChannel(VertexAttribute.Color, (num == 4) ? VertexAttributeFormat.UNorm8 : VertexAttributeFormat.Float32, 4, (IntPtr)inColors.GetUnsafeReadOnlyPtr(), inColors.Length, start, length);
	}

	private void SetUvsImpl<T>(int uvIndex, int dim, List<T> uvs, int start, int length)
	{
		if (uvIndex < 0 || uvIndex > 7)
		{
			Debug.LogError("The uv index is invalid. Must be in the range 0 to 7.");
		}
		else
		{
			SetListForChannel(GetUVChannel(uvIndex), VertexAttributeFormat.Float32, dim, uvs, start, length);
		}
	}

	public void SetUVs(int channel, List<Vector2> uvs)
	{
		SetUVs(channel, uvs, 0, NoAllocHelpers.SafeLength(uvs));
	}

	public void SetUVs(int channel, List<Vector3> uvs)
	{
		SetUVs(channel, uvs, 0, NoAllocHelpers.SafeLength(uvs));
	}

	public void SetUVs(int channel, List<Vector4> uvs)
	{
		SetUVs(channel, uvs, 0, NoAllocHelpers.SafeLength(uvs));
	}

	public void SetUVs(int channel, List<Vector2> uvs, int start, int length)
	{
		SetUvsImpl(channel, 2, uvs, start, length);
	}

	public void SetUVs(int channel, List<Vector3> uvs, int start, int length)
	{
		SetUvsImpl(channel, 3, uvs, start, length);
	}

	public void SetUVs(int channel, List<Vector4> uvs, int start, int length)
	{
		SetUvsImpl(channel, 4, uvs, start, length);
	}

	private void SetUvsImpl(int uvIndex, int dim, Array uvs, int arrayStart, int arraySize)
	{
		if (uvIndex < 0 || uvIndex > 7)
		{
			throw new ArgumentOutOfRangeException("uvIndex", uvIndex, "The uv index is invalid. Must be in the range 0 to 7.");
		}
		SetSizedArrayForChannel(GetUVChannel(uvIndex), VertexAttributeFormat.Float32, dim, uvs, NoAllocHelpers.SafeLength(uvs), arrayStart, arraySize);
	}

	public void SetUVs(int channel, Vector2[] uvs)
	{
		SetUVs(channel, uvs, 0, NoAllocHelpers.SafeLength(uvs));
	}

	public void SetUVs(int channel, Vector3[] uvs)
	{
		SetUVs(channel, uvs, 0, NoAllocHelpers.SafeLength(uvs));
	}

	public void SetUVs(int channel, Vector4[] uvs)
	{
		SetUVs(channel, uvs, 0, NoAllocHelpers.SafeLength(uvs));
	}

	public void SetUVs(int channel, Vector2[] uvs, int start, int length)
	{
		SetUvsImpl(channel, 2, uvs, start, length);
	}

	public void SetUVs(int channel, Vector3[] uvs, int start, int length)
	{
		SetUvsImpl(channel, 3, uvs, start, length);
	}

	public void SetUVs(int channel, Vector4[] uvs, int start, int length)
	{
		SetUvsImpl(channel, 4, uvs, start, length);
	}

	public void SetUVs<T>(int channel, NativeArray<T> uvs) where T : struct
	{
		SetUVs(channel, uvs, 0, uvs.Length);
	}

	public unsafe void SetUVs<T>(int channel, NativeArray<T> uvs, int start, int length) where T : struct
	{
		if (channel < 0 || channel > 7)
		{
			throw new ArgumentOutOfRangeException("channel", channel, "The uv index is invalid. Must be in the range 0 to 7.");
		}
		int num = UnsafeUtility.SizeOf<T>();
		if ((num & 3) != 0)
		{
			throw new ArgumentException("SetUVs with NativeArray should use struct type that is multiple of 4 bytes in size");
		}
		int num2 = num / 4;
		if (num2 < 1 || num2 > 4)
		{
			throw new ArgumentException("SetUVs with NativeArray should use struct type that is 1..4 floats in size");
		}
		SetSizedNativeArrayForChannel(GetUVChannel(channel), VertexAttributeFormat.Float32, num2, (IntPtr)uvs.GetUnsafeReadOnlyPtr(), uvs.Length, start, length);
	}

	private void GetUVsImpl<T>(int uvIndex, List<T> uvs, int dim)
	{
		if (uvs == null)
		{
			throw new ArgumentNullException("The result uvs list cannot be null.", "uvs");
		}
		if (uvIndex < 0 || uvIndex > 7)
		{
			throw new IndexOutOfRangeException("The uv index is invalid. Must be in the range 0 to 7.");
		}
		GetListForChannel(uvs, vertexCount, GetUVChannel(uvIndex), dim);
	}

	public void GetUVs(int channel, List<Vector2> uvs)
	{
		GetUVsImpl(channel, uvs, 2);
	}

	public void GetUVs(int channel, List<Vector3> uvs)
	{
		GetUVsImpl(channel, uvs, 3);
	}

	public void GetUVs(int channel, List<Vector4> uvs)
	{
		GetUVsImpl(channel, uvs, 4);
	}

	public VertexAttributeDescriptor[] GetVertexAttributes()
	{
		return (VertexAttributeDescriptor[])GetVertexAttributesAlloc();
	}

	public int GetVertexAttributes(VertexAttributeDescriptor[] attributes)
	{
		return GetVertexAttributesArray(attributes);
	}

	public int GetVertexAttributes(List<VertexAttributeDescriptor> attributes)
	{
		return GetVertexAttributesList(attributes);
	}

	public unsafe void SetVertexBufferData<T>(NativeArray<T> data, int dataStart, int meshBufferStart, int count, int stream = 0, MeshUpdateFlags flags = MeshUpdateFlags.Default) where T : struct
	{
		if (!canAccess)
		{
			throw new InvalidOperationException("Not allowed to access vertex data on mesh '" + base.name + "' (isReadable is false; Read/Write must be enabled in import settings)");
		}
		if (dataStart < 0 || meshBufferStart < 0 || count < 0 || dataStart + count > data.Length)
		{
			throw new ArgumentOutOfRangeException($"Bad start/count arguments (dataStart:{dataStart} meshBufferStart:{meshBufferStart} count:{count})");
		}
		InternalSetVertexBufferData(stream, (IntPtr)data.GetUnsafeReadOnlyPtr(), dataStart, meshBufferStart, count, UnsafeUtility.SizeOf<T>(), flags);
	}

	public void SetVertexBufferData<T>(T[] data, int dataStart, int meshBufferStart, int count, int stream = 0, MeshUpdateFlags flags = MeshUpdateFlags.Default) where T : struct
	{
		if (!canAccess)
		{
			throw new InvalidOperationException("Not allowed to access vertex data on mesh '" + base.name + "' (isReadable is false; Read/Write must be enabled in import settings)");
		}
		if (!UnsafeUtility.IsArrayBlittable(data))
		{
			throw new ArgumentException("Array passed to SetVertexBufferData must be blittable.\n" + UnsafeUtility.GetReasonForArrayNonBlittable(data));
		}
		if (dataStart < 0 || meshBufferStart < 0 || count < 0 || dataStart + count > data.Length)
		{
			throw new ArgumentOutOfRangeException($"Bad start/count arguments (dataStart:{dataStart} meshBufferStart:{meshBufferStart} count:{count})");
		}
		InternalSetVertexBufferDataFromArray(stream, data, dataStart, meshBufferStart, count, UnsafeUtility.SizeOf<T>(), flags);
	}

	public void SetVertexBufferData<T>(List<T> data, int dataStart, int meshBufferStart, int count, int stream = 0, MeshUpdateFlags flags = MeshUpdateFlags.Default) where T : struct
	{
		if (!canAccess)
		{
			throw new InvalidOperationException("Not allowed to access vertex data on mesh '" + base.name + "' (isReadable is false; Read/Write must be enabled in import settings)");
		}
		if (!UnsafeUtility.IsGenericListBlittable<T>())
		{
			throw new ArgumentException(string.Format("List<{0}> passed to {1} must be blittable.\n{2}", typeof(T), "SetVertexBufferData", UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
		}
		if (dataStart < 0 || meshBufferStart < 0 || count < 0 || dataStart + count > data.Count)
		{
			throw new ArgumentOutOfRangeException($"Bad start/count arguments (dataStart:{dataStart} meshBufferStart:{meshBufferStart} count:{count})");
		}
		InternalSetVertexBufferDataFromArray(stream, NoAllocHelpers.ExtractArrayFromList(data), dataStart, meshBufferStart, count, UnsafeUtility.SizeOf<T>(), flags);
	}

	private void PrintErrorCantAccessIndices()
	{
		Debug.LogError($"Not allowed to access triangles/indices on mesh '{base.name}' (isReadable is false; Read/Write must be enabled in import settings)");
	}

	private bool CheckCanAccessSubmesh(int submesh, bool errorAboutTriangles)
	{
		if (!canAccess)
		{
			PrintErrorCantAccessIndices();
			return false;
		}
		if (submesh < 0 || submesh >= subMeshCount)
		{
			Debug.LogError(string.Format("Failed getting {0}. Submesh index is out of bounds.", errorAboutTriangles ? "triangles" : "indices"), this);
			return false;
		}
		return true;
	}

	private bool CheckCanAccessSubmeshTriangles(int submesh)
	{
		return CheckCanAccessSubmesh(submesh, errorAboutTriangles: true);
	}

	private bool CheckCanAccessSubmeshIndices(int submesh)
	{
		return CheckCanAccessSubmesh(submesh, errorAboutTriangles: false);
	}

	public int[] GetTriangles(int submesh)
	{
		return GetTriangles(submesh, applyBaseVertex: true);
	}

	public int[] GetTriangles(int submesh, [DefaultValue("true")] bool applyBaseVertex)
	{
		return CheckCanAccessSubmeshTriangles(submesh) ? GetTrianglesImpl(submesh, applyBaseVertex) : new int[0];
	}

	public void GetTriangles(List<int> triangles, int submesh)
	{
		GetTriangles(triangles, submesh, applyBaseVertex: true);
	}

	public void GetTriangles(List<int> triangles, int submesh, [DefaultValue("true")] bool applyBaseVertex)
	{
		if (triangles == null)
		{
			throw new ArgumentNullException("The result triangles list cannot be null.", "triangles");
		}
		if (submesh < 0 || submesh >= subMeshCount)
		{
			throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
		}
		NoAllocHelpers.EnsureListElemCount(triangles, (int)(3 * GetTrianglesCountImpl(submesh)));
		GetTrianglesNonAllocImpl(NoAllocHelpers.ExtractArrayFromListT(triangles), submesh, applyBaseVertex);
	}

	public void GetTriangles(List<ushort> triangles, int submesh, bool applyBaseVertex = true)
	{
		if (triangles == null)
		{
			throw new ArgumentNullException("The result triangles list cannot be null.", "triangles");
		}
		if (submesh < 0 || submesh >= subMeshCount)
		{
			throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
		}
		NoAllocHelpers.EnsureListElemCount(triangles, (int)(3 * GetTrianglesCountImpl(submesh)));
		GetTrianglesNonAllocImpl16(NoAllocHelpers.ExtractArrayFromListT(triangles), submesh, applyBaseVertex);
	}

	[ExcludeFromDocs]
	public int[] GetIndices(int submesh)
	{
		return GetIndices(submesh, applyBaseVertex: true);
	}

	public int[] GetIndices(int submesh, [DefaultValue("true")] bool applyBaseVertex)
	{
		return CheckCanAccessSubmeshIndices(submesh) ? GetIndicesImpl(submesh, applyBaseVertex) : new int[0];
	}

	[ExcludeFromDocs]
	public void GetIndices(List<int> indices, int submesh)
	{
		GetIndices(indices, submesh, applyBaseVertex: true);
	}

	public void GetIndices(List<int> indices, int submesh, [DefaultValue("true")] bool applyBaseVertex)
	{
		if (indices == null)
		{
			throw new ArgumentNullException("The result indices list cannot be null.", "indices");
		}
		if (submesh < 0 || submesh >= subMeshCount)
		{
			throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
		}
		NoAllocHelpers.EnsureListElemCount(indices, (int)GetIndexCount(submesh));
		GetIndicesNonAllocImpl(NoAllocHelpers.ExtractArrayFromListT(indices), submesh, applyBaseVertex);
	}

	public void GetIndices(List<ushort> indices, int submesh, bool applyBaseVertex = true)
	{
		if (indices == null)
		{
			throw new ArgumentNullException("The result indices list cannot be null.", "indices");
		}
		if (submesh < 0 || submesh >= subMeshCount)
		{
			throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
		}
		NoAllocHelpers.EnsureListElemCount(indices, (int)GetIndexCount(submesh));
		GetIndicesNonAllocImpl16(NoAllocHelpers.ExtractArrayFromListT(indices), submesh, applyBaseVertex);
	}

	public unsafe void SetIndexBufferData<T>(NativeArray<T> data, int dataStart, int meshBufferStart, int count, MeshUpdateFlags flags = MeshUpdateFlags.Default) where T : struct
	{
		if (!canAccess)
		{
			PrintErrorCantAccessIndices();
			return;
		}
		if (dataStart < 0 || meshBufferStart < 0 || count < 0 || dataStart + count > data.Length)
		{
			throw new ArgumentOutOfRangeException($"Bad start/count arguments (dataStart:{dataStart} meshBufferStart:{meshBufferStart} count:{count})");
		}
		InternalSetIndexBufferData((IntPtr)data.GetUnsafeReadOnlyPtr(), dataStart, meshBufferStart, count, UnsafeUtility.SizeOf<T>(), flags);
	}

	public void SetIndexBufferData<T>(T[] data, int dataStart, int meshBufferStart, int count, MeshUpdateFlags flags = MeshUpdateFlags.Default) where T : struct
	{
		if (!canAccess)
		{
			PrintErrorCantAccessIndices();
			return;
		}
		if (!UnsafeUtility.IsArrayBlittable(data))
		{
			throw new ArgumentException("Array passed to SetIndexBufferData must be blittable.\n" + UnsafeUtility.GetReasonForArrayNonBlittable(data));
		}
		if (dataStart < 0 || meshBufferStart < 0 || count < 0 || dataStart + count > data.Length)
		{
			throw new ArgumentOutOfRangeException($"Bad start/count arguments (dataStart:{dataStart} meshBufferStart:{meshBufferStart} count:{count})");
		}
		InternalSetIndexBufferDataFromArray(data, dataStart, meshBufferStart, count, UnsafeUtility.SizeOf<T>(), flags);
	}

	public void SetIndexBufferData<T>(List<T> data, int dataStart, int meshBufferStart, int count, MeshUpdateFlags flags = MeshUpdateFlags.Default) where T : struct
	{
		if (!canAccess)
		{
			PrintErrorCantAccessIndices();
			return;
		}
		if (!UnsafeUtility.IsGenericListBlittable<T>())
		{
			throw new ArgumentException(string.Format("List<{0}> passed to {1} must be blittable.\n{2}", typeof(T), "SetIndexBufferData", UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
		}
		if (dataStart < 0 || meshBufferStart < 0 || count < 0 || dataStart + count > data.Count)
		{
			throw new ArgumentOutOfRangeException($"Bad start/count arguments (dataStart:{dataStart} meshBufferStart:{meshBufferStart} count:{count})");
		}
		InternalSetIndexBufferDataFromArray(NoAllocHelpers.ExtractArrayFromList(data), dataStart, meshBufferStart, count, UnsafeUtility.SizeOf<T>(), flags);
	}

	public uint GetIndexStart(int submesh)
	{
		if (submesh < 0 || submesh >= subMeshCount)
		{
			throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
		}
		return GetIndexStartImpl(submesh);
	}

	public uint GetIndexCount(int submesh)
	{
		if (submesh < 0 || submesh >= subMeshCount)
		{
			throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
		}
		return GetIndexCountImpl(submesh);
	}

	public uint GetBaseVertex(int submesh)
	{
		if (submesh < 0 || submesh >= subMeshCount)
		{
			throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
		}
		return GetBaseVertexImpl(submesh);
	}

	private void CheckIndicesArrayRange(int valuesLength, int start, int length)
	{
		if (start < 0)
		{
			throw new ArgumentOutOfRangeException("start", start, "Mesh indices array start can't be negative.");
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length", length, "Mesh indices array length can't be negative.");
		}
		if (start >= valuesLength && length != 0)
		{
			throw new ArgumentOutOfRangeException("start", start, "Mesh indices array start is outside of array size.");
		}
		if (start + length > valuesLength)
		{
			throw new ArgumentOutOfRangeException("length", start + length, "Mesh indices array start+count is outside of array size.");
		}
	}

	private void SetTrianglesImpl(int submesh, IndexFormat indicesFormat, Array triangles, int trianglesArrayLength, int start, int length, bool calculateBounds, int baseVertex)
	{
		CheckIndicesArrayRange(trianglesArrayLength, start, length);
		SetIndicesImpl(submesh, MeshTopology.Triangles, indicesFormat, triangles, start, length, calculateBounds, baseVertex);
	}

	[ExcludeFromDocs]
	public void SetTriangles(int[] triangles, int submesh)
	{
		SetTriangles(triangles, submesh, calculateBounds: true, 0);
	}

	[ExcludeFromDocs]
	public void SetTriangles(int[] triangles, int submesh, bool calculateBounds)
	{
		SetTriangles(triangles, submesh, calculateBounds, 0);
	}

	public void SetTriangles(int[] triangles, int submesh, [DefaultValue("true")] bool calculateBounds, [DefaultValue("0")] int baseVertex)
	{
		SetTriangles(triangles, 0, NoAllocHelpers.SafeLength(triangles), submesh, calculateBounds, baseVertex);
	}

	public void SetTriangles(int[] triangles, int trianglesStart, int trianglesLength, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		if (CheckCanAccessSubmeshTriangles(submesh))
		{
			SetTrianglesImpl(submesh, IndexFormat.UInt32, triangles, NoAllocHelpers.SafeLength(triangles), trianglesStart, trianglesLength, calculateBounds, baseVertex);
		}
	}

	public void SetTriangles(ushort[] triangles, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		SetTriangles(triangles, 0, NoAllocHelpers.SafeLength(triangles), submesh, calculateBounds, baseVertex);
	}

	public void SetTriangles(ushort[] triangles, int trianglesStart, int trianglesLength, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		if (CheckCanAccessSubmeshTriangles(submesh))
		{
			SetTrianglesImpl(submesh, IndexFormat.UInt16, triangles, NoAllocHelpers.SafeLength(triangles), trianglesStart, trianglesLength, calculateBounds, baseVertex);
		}
	}

	[ExcludeFromDocs]
	public void SetTriangles(List<int> triangles, int submesh)
	{
		SetTriangles(triangles, submesh, calculateBounds: true, 0);
	}

	[ExcludeFromDocs]
	public void SetTriangles(List<int> triangles, int submesh, bool calculateBounds)
	{
		SetTriangles(triangles, submesh, calculateBounds, 0);
	}

	public void SetTriangles(List<int> triangles, int submesh, [DefaultValue("true")] bool calculateBounds, [DefaultValue("0")] int baseVertex)
	{
		SetTriangles(triangles, 0, NoAllocHelpers.SafeLength(triangles), submesh, calculateBounds, baseVertex);
	}

	public void SetTriangles(List<int> triangles, int trianglesStart, int trianglesLength, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		if (CheckCanAccessSubmeshTriangles(submesh))
		{
			SetTrianglesImpl(submesh, IndexFormat.UInt32, NoAllocHelpers.ExtractArrayFromList(triangles), NoAllocHelpers.SafeLength(triangles), trianglesStart, trianglesLength, calculateBounds, baseVertex);
		}
	}

	public void SetTriangles(List<ushort> triangles, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		SetTriangles(triangles, 0, NoAllocHelpers.SafeLength(triangles), submesh, calculateBounds, baseVertex);
	}

	public void SetTriangles(List<ushort> triangles, int trianglesStart, int trianglesLength, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		if (CheckCanAccessSubmeshTriangles(submesh))
		{
			SetTrianglesImpl(submesh, IndexFormat.UInt16, NoAllocHelpers.ExtractArrayFromList(triangles), NoAllocHelpers.SafeLength(triangles), trianglesStart, trianglesLength, calculateBounds, baseVertex);
		}
	}

	[ExcludeFromDocs]
	public void SetIndices(int[] indices, MeshTopology topology, int submesh)
	{
		SetIndices(indices, topology, submesh, calculateBounds: true, 0);
	}

	[ExcludeFromDocs]
	public void SetIndices(int[] indices, MeshTopology topology, int submesh, bool calculateBounds)
	{
		SetIndices(indices, topology, submesh, calculateBounds, 0);
	}

	public void SetIndices(int[] indices, MeshTopology topology, int submesh, [DefaultValue("true")] bool calculateBounds, [DefaultValue("0")] int baseVertex)
	{
		SetIndices(indices, 0, NoAllocHelpers.SafeLength(indices), topology, submesh, calculateBounds, baseVertex);
	}

	public void SetIndices(int[] indices, int indicesStart, int indicesLength, MeshTopology topology, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		if (CheckCanAccessSubmeshIndices(submesh))
		{
			CheckIndicesArrayRange(NoAllocHelpers.SafeLength(indices), indicesStart, indicesLength);
			SetIndicesImpl(submesh, topology, IndexFormat.UInt32, indices, indicesStart, indicesLength, calculateBounds, baseVertex);
		}
	}

	public void SetIndices(ushort[] indices, MeshTopology topology, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		SetIndices(indices, 0, NoAllocHelpers.SafeLength(indices), topology, submesh, calculateBounds, baseVertex);
	}

	public void SetIndices(ushort[] indices, int indicesStart, int indicesLength, MeshTopology topology, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		if (CheckCanAccessSubmeshIndices(submesh))
		{
			CheckIndicesArrayRange(NoAllocHelpers.SafeLength(indices), indicesStart, indicesLength);
			SetIndicesImpl(submesh, topology, IndexFormat.UInt16, indices, indicesStart, indicesLength, calculateBounds, baseVertex);
		}
	}

	public void SetIndices<T>(NativeArray<T> indices, MeshTopology topology, int submesh, bool calculateBounds = true, int baseVertex = 0) where T : struct
	{
		SetIndices(indices, 0, indices.Length, topology, submesh, calculateBounds, baseVertex);
	}

	public unsafe void SetIndices<T>(NativeArray<T> indices, int indicesStart, int indicesLength, MeshTopology topology, int submesh, bool calculateBounds = true, int baseVertex = 0) where T : struct
	{
		if (CheckCanAccessSubmeshIndices(submesh))
		{
			int num = UnsafeUtility.SizeOf<T>();
			if (num != 2 && num != 4)
			{
				throw new ArgumentException("SetIndices with NativeArray should use type is 2 or 4 bytes in size");
			}
			CheckIndicesArrayRange(indices.Length, indicesStart, indicesLength);
			SetIndicesNativeArrayImpl(submesh, topology, (num != 2) ? IndexFormat.UInt32 : IndexFormat.UInt16, (IntPtr)indices.GetUnsafeReadOnlyPtr(), indicesStart, indicesLength, calculateBounds, baseVertex);
		}
	}

	public void SetIndices(List<int> indices, MeshTopology topology, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		SetIndices(indices, 0, NoAllocHelpers.SafeLength(indices), topology, submesh, calculateBounds, baseVertex);
	}

	public void SetIndices(List<int> indices, int indicesStart, int indicesLength, MeshTopology topology, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		if (CheckCanAccessSubmeshIndices(submesh))
		{
			Array indices2 = NoAllocHelpers.ExtractArrayFromList(indices);
			CheckIndicesArrayRange(NoAllocHelpers.SafeLength(indices), indicesStart, indicesLength);
			SetIndicesImpl(submesh, topology, IndexFormat.UInt32, indices2, indicesStart, indicesLength, calculateBounds, baseVertex);
		}
	}

	public void SetIndices(List<ushort> indices, MeshTopology topology, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		SetIndices(indices, 0, NoAllocHelpers.SafeLength(indices), topology, submesh, calculateBounds, baseVertex);
	}

	public void SetIndices(List<ushort> indices, int indicesStart, int indicesLength, MeshTopology topology, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		if (CheckCanAccessSubmeshIndices(submesh))
		{
			Array indices2 = NoAllocHelpers.ExtractArrayFromList(indices);
			CheckIndicesArrayRange(NoAllocHelpers.SafeLength(indices), indicesStart, indicesLength);
			SetIndicesImpl(submesh, topology, IndexFormat.UInt16, indices2, indicesStart, indicesLength, calculateBounds, baseVertex);
		}
	}

	public void GetBindposes(List<Matrix4x4> bindposes)
	{
		if (bindposes == null)
		{
			throw new ArgumentNullException("The result bindposes list cannot be null.", "bindposes");
		}
		NoAllocHelpers.EnsureListElemCount(bindposes, GetBindposeCount());
		GetBindposesNonAllocImpl(NoAllocHelpers.ExtractArrayFromListT(bindposes));
	}

	public void GetBoneWeights(List<BoneWeight> boneWeights)
	{
		if (boneWeights == null)
		{
			throw new ArgumentNullException("The result boneWeights list cannot be null.", "boneWeights");
		}
		if (HasBoneWeights())
		{
			NoAllocHelpers.EnsureListElemCount(boneWeights, vertexCount);
		}
		GetBoneWeightsNonAllocImpl(NoAllocHelpers.ExtractArrayFromListT(boneWeights));
	}

	public void Clear([DefaultValue("true")] bool keepVertexLayout)
	{
		ClearImpl(keepVertexLayout);
	}

	[ExcludeFromDocs]
	public void Clear()
	{
		ClearImpl(keepVertexLayout: true);
	}

	public void RecalculateBounds()
	{
		if (canAccess)
		{
			RecalculateBoundsImpl();
		}
		else
		{
			Debug.LogError($"Not allowed to call RecalculateBounds() on mesh '{base.name}'");
		}
	}

	public void RecalculateNormals()
	{
		if (canAccess)
		{
			RecalculateNormalsImpl();
		}
		else
		{
			Debug.LogError($"Not allowed to call RecalculateNormals() on mesh '{base.name}'");
		}
	}

	public void RecalculateTangents()
	{
		if (canAccess)
		{
			RecalculateTangentsImpl();
		}
		else
		{
			Debug.LogError($"Not allowed to call RecalculateTangents() on mesh '{base.name}'");
		}
	}

	public void MarkDynamic()
	{
		if (canAccess)
		{
			MarkDynamicImpl();
		}
	}

	public void UploadMeshData(bool markNoLongerReadable)
	{
		if (canAccess)
		{
			UploadMeshDataImpl(markNoLongerReadable);
		}
	}

	public void Optimize()
	{
		if (canAccess)
		{
			OptimizeImpl();
		}
		else
		{
			Debug.LogError($"Not allowed to call Optimize() on mesh '{base.name}'");
		}
	}

	public void OptimizeIndexBuffers()
	{
		if (canAccess)
		{
			OptimizeIndexBuffersImpl();
		}
		else
		{
			Debug.LogError($"Not allowed to call OptimizeIndexBuffers() on mesh '{base.name}'");
		}
	}

	public void OptimizeReorderVertexBuffer()
	{
		if (canAccess)
		{
			OptimizeReorderVertexBufferImpl();
		}
		else
		{
			Debug.LogError($"Not allowed to call OptimizeReorderVertexBuffer() on mesh '{base.name}'");
		}
	}

	public MeshTopology GetTopology(int submesh)
	{
		if (submesh < 0 || submesh >= subMeshCount)
		{
			Debug.LogError($"Failed getting topology. Submesh index is out of bounds.", this);
			return MeshTopology.Triangles;
		}
		return GetTopologyImpl(submesh);
	}

	public void CombineMeshes(CombineInstance[] combine, [DefaultValue("true")] bool mergeSubMeshes, [DefaultValue("true")] bool useMatrices, [DefaultValue("false")] bool hasLightmapData)
	{
		CombineMeshesImpl(combine, mergeSubMeshes, useMatrices, hasLightmapData);
	}

	[ExcludeFromDocs]
	public void CombineMeshes(CombineInstance[] combine, bool mergeSubMeshes, bool useMatrices)
	{
		CombineMeshesImpl(combine, mergeSubMeshes, useMatrices, hasLightmapData: false);
	}

	[ExcludeFromDocs]
	public void CombineMeshes(CombineInstance[] combine, bool mergeSubMeshes)
	{
		CombineMeshesImpl(combine, mergeSubMeshes, useMatrices: true, hasLightmapData: false);
	}

	[ExcludeFromDocs]
	public void CombineMeshes(CombineInstance[] combine)
	{
		CombineMeshesImpl(combine, mergeSubMeshes: true, useMatrices: true, hasLightmapData: false);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetVertexAttribute_Injected(int index, out VertexAttributeDescriptor ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetSubMesh_Injected(int index, ref SubMeshDescriptor desc, MeshUpdateFlags flags = MeshUpdateFlags.Default);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetSubMesh_Injected(int index, out SubMeshDescriptor ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_bounds_Injected(out Bounds ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_bounds_Injected(ref Bounds value);
}
