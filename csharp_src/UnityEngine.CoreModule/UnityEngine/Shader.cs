using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine;

[NativeHeader("Runtime/Shaders/ComputeShader.h")]
[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
[NativeHeader("Runtime/Shaders/GpuPrograms/ShaderVariantCollection.h")]
[NativeHeader("Runtime/Shaders/ShaderNameRegistry.h")]
[NativeHeader("Runtime/Misc/ResourceManager.h")]
[NativeHeader("Runtime/Shaders/Shader.h")]
[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
public sealed class Shader : Object
{
	[Obsolete("Use Graphics.activeTier instead (UnityUpgradable) -> UnityEngine.Graphics.activeTier", false)]
	public static ShaderHardwareTier globalShaderHardwareTier
	{
		get
		{
			return (ShaderHardwareTier)Graphics.activeTier;
		}
		set
		{
			Graphics.activeTier = (GraphicsTier)value;
		}
	}

	[NativeProperty("MaximumShaderLOD")]
	public extern int maximumLOD
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("GlobalMaximumShaderLOD")]
	public static extern int globalMaximumLOD
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool isSupported
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("IsSupported")]
		get;
	}

	public static extern string globalRenderPipeline
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int renderQueue
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction("ShaderScripting::GetRenderQueue", HasExplicitThis = true)]
		get;
	}

	internal extern DisableBatchingType disableBatching
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction("ShaderScripting::GetDisableBatchingType", HasExplicitThis = true)]
		get;
	}

	public extern int passCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "ShaderScripting::GetPassCount", HasExplicitThis = true)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("GetScriptMapper().FindShader")]
	public static extern Shader Find(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("GetBuiltinResource<Shader>")]
	internal static extern Shader FindBuiltin(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::EnableKeyword")]
	public static extern void EnableKeyword(string keyword);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::DisableKeyword")]
	public static extern void DisableKeyword(string keyword);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::IsKeywordEnabled")]
	public static extern bool IsKeywordEnabled(string keyword);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void WarmupAllShaders();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::TagToID")]
	internal static extern int TagToID(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::IDToTag")]
	internal static extern string IDToTag(int name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ShaderScripting::PropertyToID", IsThreadSafe = true)]
	public static extern int PropertyToID(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern Shader GetDependency(string name);

	public ShaderTagId FindPassTagValue(int passIndex, ShaderTagId tagName)
	{
		if (passIndex < 0 || passIndex >= passCount)
		{
			throw new ArgumentOutOfRangeException("passIndex");
		}
		int id = Internal_FindPassTagValue(passIndex, tagName.id);
		return new ShaderTagId
		{
			id = id
		};
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ShaderScripting::FindPassTagValue", HasExplicitThis = true)]
	private extern int Internal_FindPassTagValue(int passIndex, int tagName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::SetGlobalFloat")]
	private static extern void SetGlobalFloatImpl(int name, float value);

	[FreeFunction("ShaderScripting::SetGlobalVector")]
	private static void SetGlobalVectorImpl(int name, Vector4 value)
	{
		SetGlobalVectorImpl_Injected(name, ref value);
	}

	[FreeFunction("ShaderScripting::SetGlobalMatrix")]
	private static void SetGlobalMatrixImpl(int name, Matrix4x4 value)
	{
		SetGlobalMatrixImpl_Injected(name, ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::SetGlobalTexture")]
	private static extern void SetGlobalTextureImpl(int name, Texture value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::SetGlobalRenderTexture")]
	private static extern void SetGlobalRenderTextureImpl(int name, RenderTexture value, RenderTextureSubElement element);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::SetGlobalBuffer")]
	private static extern void SetGlobalBufferImpl(int name, ComputeBuffer value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::SetGlobalConstantBuffer")]
	private static extern void SetGlobalConstantBufferImpl(int name, ComputeBuffer value, int offset, int size);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetGlobalFloat")]
	private static extern float GetGlobalFloatImpl(int name);

	[FreeFunction("ShaderScripting::GetGlobalVector")]
	private static Vector4 GetGlobalVectorImpl(int name)
	{
		GetGlobalVectorImpl_Injected(name, out var ret);
		return ret;
	}

	[FreeFunction("ShaderScripting::GetGlobalMatrix")]
	private static Matrix4x4 GetGlobalMatrixImpl(int name)
	{
		GetGlobalMatrixImpl_Injected(name, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetGlobalTexture")]
	private static extern Texture GetGlobalTextureImpl(int name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::SetGlobalFloatArray")]
	private static extern void SetGlobalFloatArrayImpl(int name, float[] values, int count);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::SetGlobalVectorArray")]
	private static extern void SetGlobalVectorArrayImpl(int name, Vector4[] values, int count);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::SetGlobalMatrixArray")]
	private static extern void SetGlobalMatrixArrayImpl(int name, Matrix4x4[] values, int count);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetGlobalFloatArray")]
	private static extern float[] GetGlobalFloatArrayImpl(int name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetGlobalVectorArray")]
	private static extern Vector4[] GetGlobalVectorArrayImpl(int name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetGlobalMatrixArray")]
	private static extern Matrix4x4[] GetGlobalMatrixArrayImpl(int name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetGlobalFloatArrayCount")]
	private static extern int GetGlobalFloatArrayCountImpl(int name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetGlobalVectorArrayCount")]
	private static extern int GetGlobalVectorArrayCountImpl(int name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetGlobalMatrixArrayCount")]
	private static extern int GetGlobalMatrixArrayCountImpl(int name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::ExtractGlobalFloatArray")]
	private static extern void ExtractGlobalFloatArrayImpl(int name, [Out] float[] val);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::ExtractGlobalVectorArray")]
	private static extern void ExtractGlobalVectorArrayImpl(int name, [Out] Vector4[] val);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::ExtractGlobalMatrixArray")]
	private static extern void ExtractGlobalMatrixArrayImpl(int name, [Out] Matrix4x4[] val);

	private static void SetGlobalFloatArray(int name, float[] values, int count)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		if (values.Length == 0)
		{
			throw new ArgumentException("Zero-sized array is not allowed.");
		}
		if (values.Length < count)
		{
			throw new ArgumentException("array has less elements than passed count.");
		}
		SetGlobalFloatArrayImpl(name, values, count);
	}

	private static void SetGlobalVectorArray(int name, Vector4[] values, int count)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		if (values.Length == 0)
		{
			throw new ArgumentException("Zero-sized array is not allowed.");
		}
		if (values.Length < count)
		{
			throw new ArgumentException("array has less elements than passed count.");
		}
		SetGlobalVectorArrayImpl(name, values, count);
	}

	private static void SetGlobalMatrixArray(int name, Matrix4x4[] values, int count)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		if (values.Length == 0)
		{
			throw new ArgumentException("Zero-sized array is not allowed.");
		}
		if (values.Length < count)
		{
			throw new ArgumentException("array has less elements than passed count.");
		}
		SetGlobalMatrixArrayImpl(name, values, count);
	}

	private static void ExtractGlobalFloatArray(int name, List<float> values)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		values.Clear();
		int globalFloatArrayCountImpl = GetGlobalFloatArrayCountImpl(name);
		if (globalFloatArrayCountImpl > 0)
		{
			NoAllocHelpers.EnsureListElemCount(values, globalFloatArrayCountImpl);
			ExtractGlobalFloatArrayImpl(name, (float[])NoAllocHelpers.ExtractArrayFromList(values));
		}
	}

	private static void ExtractGlobalVectorArray(int name, List<Vector4> values)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		values.Clear();
		int globalVectorArrayCountImpl = GetGlobalVectorArrayCountImpl(name);
		if (globalVectorArrayCountImpl > 0)
		{
			NoAllocHelpers.EnsureListElemCount(values, globalVectorArrayCountImpl);
			ExtractGlobalVectorArrayImpl(name, (Vector4[])NoAllocHelpers.ExtractArrayFromList(values));
		}
	}

	private static void ExtractGlobalMatrixArray(int name, List<Matrix4x4> values)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		values.Clear();
		int globalMatrixArrayCountImpl = GetGlobalMatrixArrayCountImpl(name);
		if (globalMatrixArrayCountImpl > 0)
		{
			NoAllocHelpers.EnsureListElemCount(values, globalMatrixArrayCountImpl);
			ExtractGlobalMatrixArrayImpl(name, (Matrix4x4[])NoAllocHelpers.ExtractArrayFromList(values));
		}
	}

	public static void SetGlobalFloat(string name, float value)
	{
		SetGlobalFloatImpl(PropertyToID(name), value);
	}

	public static void SetGlobalFloat(int nameID, float value)
	{
		SetGlobalFloatImpl(nameID, value);
	}

	public static void SetGlobalInt(string name, int value)
	{
		SetGlobalFloatImpl(PropertyToID(name), value);
	}

	public static void SetGlobalInt(int nameID, int value)
	{
		SetGlobalFloatImpl(nameID, value);
	}

	public static void SetGlobalVector(string name, Vector4 value)
	{
		SetGlobalVectorImpl(PropertyToID(name), value);
	}

	public static void SetGlobalVector(int nameID, Vector4 value)
	{
		SetGlobalVectorImpl(nameID, value);
	}

	public static void SetGlobalColor(string name, Color value)
	{
		SetGlobalVectorImpl(PropertyToID(name), value);
	}

	public static void SetGlobalColor(int nameID, Color value)
	{
		SetGlobalVectorImpl(nameID, value);
	}

	public static void SetGlobalMatrix(string name, Matrix4x4 value)
	{
		SetGlobalMatrixImpl(PropertyToID(name), value);
	}

	public static void SetGlobalMatrix(int nameID, Matrix4x4 value)
	{
		SetGlobalMatrixImpl(nameID, value);
	}

	public static void SetGlobalTexture(string name, Texture value)
	{
		SetGlobalTextureImpl(PropertyToID(name), value);
	}

	public static void SetGlobalTexture(int nameID, Texture value)
	{
		SetGlobalTextureImpl(nameID, value);
	}

	public static void SetGlobalTexture(string name, RenderTexture value, RenderTextureSubElement element)
	{
		SetGlobalRenderTextureImpl(PropertyToID(name), value, element);
	}

	public static void SetGlobalTexture(int nameID, RenderTexture value, RenderTextureSubElement element)
	{
		SetGlobalRenderTextureImpl(nameID, value, element);
	}

	public static void SetGlobalBuffer(string name, ComputeBuffer value)
	{
		SetGlobalBufferImpl(PropertyToID(name), value);
	}

	public static void SetGlobalBuffer(int nameID, ComputeBuffer value)
	{
		SetGlobalBufferImpl(nameID, value);
	}

	public static void SetGlobalConstantBuffer(int nameID, ComputeBuffer value, int offset, int size)
	{
		SetGlobalConstantBufferImpl(nameID, value, offset, size);
	}

	public static void SetGlobalFloatArray(string name, List<float> values)
	{
		SetGlobalFloatArray(PropertyToID(name), NoAllocHelpers.ExtractArrayFromListT(values), values.Count);
	}

	public static void SetGlobalFloatArray(int nameID, List<float> values)
	{
		SetGlobalFloatArray(nameID, NoAllocHelpers.ExtractArrayFromListT(values), values.Count);
	}

	public static void SetGlobalFloatArray(string name, float[] values)
	{
		SetGlobalFloatArray(PropertyToID(name), values, values.Length);
	}

	public static void SetGlobalFloatArray(int nameID, float[] values)
	{
		SetGlobalFloatArray(nameID, values, values.Length);
	}

	public static void SetGlobalVectorArray(string name, List<Vector4> values)
	{
		SetGlobalVectorArray(PropertyToID(name), NoAllocHelpers.ExtractArrayFromListT(values), values.Count);
	}

	public static void SetGlobalVectorArray(int nameID, List<Vector4> values)
	{
		SetGlobalVectorArray(nameID, NoAllocHelpers.ExtractArrayFromListT(values), values.Count);
	}

	public static void SetGlobalVectorArray(string name, Vector4[] values)
	{
		SetGlobalVectorArray(PropertyToID(name), values, values.Length);
	}

	public static void SetGlobalVectorArray(int nameID, Vector4[] values)
	{
		SetGlobalVectorArray(nameID, values, values.Length);
	}

	public static void SetGlobalMatrixArray(string name, List<Matrix4x4> values)
	{
		SetGlobalMatrixArray(PropertyToID(name), NoAllocHelpers.ExtractArrayFromListT(values), values.Count);
	}

	public static void SetGlobalMatrixArray(int nameID, List<Matrix4x4> values)
	{
		SetGlobalMatrixArray(nameID, NoAllocHelpers.ExtractArrayFromListT(values), values.Count);
	}

	public static void SetGlobalMatrixArray(string name, Matrix4x4[] values)
	{
		SetGlobalMatrixArray(PropertyToID(name), values, values.Length);
	}

	public static void SetGlobalMatrixArray(int nameID, Matrix4x4[] values)
	{
		SetGlobalMatrixArray(nameID, values, values.Length);
	}

	public static float GetGlobalFloat(string name)
	{
		return GetGlobalFloatImpl(PropertyToID(name));
	}

	public static float GetGlobalFloat(int nameID)
	{
		return GetGlobalFloatImpl(nameID);
	}

	public static int GetGlobalInt(string name)
	{
		return (int)GetGlobalFloatImpl(PropertyToID(name));
	}

	public static int GetGlobalInt(int nameID)
	{
		return (int)GetGlobalFloatImpl(nameID);
	}

	public static Vector4 GetGlobalVector(string name)
	{
		return GetGlobalVectorImpl(PropertyToID(name));
	}

	public static Vector4 GetGlobalVector(int nameID)
	{
		return GetGlobalVectorImpl(nameID);
	}

	public static Color GetGlobalColor(string name)
	{
		return GetGlobalVectorImpl(PropertyToID(name));
	}

	public static Color GetGlobalColor(int nameID)
	{
		return GetGlobalVectorImpl(nameID);
	}

	public static Matrix4x4 GetGlobalMatrix(string name)
	{
		return GetGlobalMatrixImpl(PropertyToID(name));
	}

	public static Matrix4x4 GetGlobalMatrix(int nameID)
	{
		return GetGlobalMatrixImpl(nameID);
	}

	public static Texture GetGlobalTexture(string name)
	{
		return GetGlobalTextureImpl(PropertyToID(name));
	}

	public static Texture GetGlobalTexture(int nameID)
	{
		return GetGlobalTextureImpl(nameID);
	}

	public static float[] GetGlobalFloatArray(string name)
	{
		return GetGlobalFloatArray(PropertyToID(name));
	}

	public static float[] GetGlobalFloatArray(int nameID)
	{
		return (GetGlobalFloatArrayCountImpl(nameID) != 0) ? GetGlobalFloatArrayImpl(nameID) : null;
	}

	public static Vector4[] GetGlobalVectorArray(string name)
	{
		return GetGlobalVectorArray(PropertyToID(name));
	}

	public static Vector4[] GetGlobalVectorArray(int nameID)
	{
		return (GetGlobalVectorArrayCountImpl(nameID) != 0) ? GetGlobalVectorArrayImpl(nameID) : null;
	}

	public static Matrix4x4[] GetGlobalMatrixArray(string name)
	{
		return GetGlobalMatrixArray(PropertyToID(name));
	}

	public static Matrix4x4[] GetGlobalMatrixArray(int nameID)
	{
		return (GetGlobalMatrixArrayCountImpl(nameID) != 0) ? GetGlobalMatrixArrayImpl(nameID) : null;
	}

	public static void GetGlobalFloatArray(string name, List<float> values)
	{
		ExtractGlobalFloatArray(PropertyToID(name), values);
	}

	public static void GetGlobalFloatArray(int nameID, List<float> values)
	{
		ExtractGlobalFloatArray(nameID, values);
	}

	public static void GetGlobalVectorArray(string name, List<Vector4> values)
	{
		ExtractGlobalVectorArray(PropertyToID(name), values);
	}

	public static void GetGlobalVectorArray(int nameID, List<Vector4> values)
	{
		ExtractGlobalVectorArray(nameID, values);
	}

	public static void GetGlobalMatrixArray(string name, List<Matrix4x4> values)
	{
		ExtractGlobalMatrixArray(PropertyToID(name), values);
	}

	public static void GetGlobalMatrixArray(int nameID, List<Matrix4x4> values)
	{
		ExtractGlobalMatrixArray(nameID, values);
	}

	private Shader()
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetPropertyName")]
	private static extern string GetPropertyName([NotNull] Shader shader, int propertyIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetPropertyNameId")]
	private static extern int GetPropertyNameId([NotNull] Shader shader, int propertyIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetPropertyType")]
	private static extern ShaderPropertyType GetPropertyType([NotNull] Shader shader, int propertyIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetPropertyDescription")]
	private static extern string GetPropertyDescription([NotNull] Shader shader, int propertyIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetPropertyFlags")]
	private static extern ShaderPropertyFlags GetPropertyFlags([NotNull] Shader shader, int propertyIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetPropertyAttributes")]
	private static extern string[] GetPropertyAttributes([NotNull] Shader shader, int propertyIndex);

	[FreeFunction("ShaderScripting::GetPropertyDefaultValue")]
	private static Vector4 GetPropertyDefaultValue([NotNull] Shader shader, int propertyIndex)
	{
		GetPropertyDefaultValue_Injected(shader, propertyIndex, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetPropertyTextureDimension")]
	private static extern TextureDimension GetPropertyTextureDimension([NotNull] Shader shader, int propertyIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetPropertyTextureDefaultName")]
	private static extern string GetPropertyTextureDefaultName([NotNull] Shader shader, int propertyIndex);

	private static void CheckPropertyIndex(Shader s, int propertyIndex)
	{
		if (propertyIndex < 0 || propertyIndex >= s.GetPropertyCount())
		{
			throw new ArgumentOutOfRangeException("propertyIndex");
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern int GetPropertyCount();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern int FindPropertyIndex(string propertyName);

	public string GetPropertyName(int propertyIndex)
	{
		CheckPropertyIndex(this, propertyIndex);
		return GetPropertyName(this, propertyIndex);
	}

	public int GetPropertyNameId(int propertyIndex)
	{
		CheckPropertyIndex(this, propertyIndex);
		return GetPropertyNameId(this, propertyIndex);
	}

	public ShaderPropertyType GetPropertyType(int propertyIndex)
	{
		CheckPropertyIndex(this, propertyIndex);
		return GetPropertyType(this, propertyIndex);
	}

	public string GetPropertyDescription(int propertyIndex)
	{
		CheckPropertyIndex(this, propertyIndex);
		return GetPropertyDescription(this, propertyIndex);
	}

	public ShaderPropertyFlags GetPropertyFlags(int propertyIndex)
	{
		CheckPropertyIndex(this, propertyIndex);
		return GetPropertyFlags(this, propertyIndex);
	}

	public string[] GetPropertyAttributes(int propertyIndex)
	{
		CheckPropertyIndex(this, propertyIndex);
		return GetPropertyAttributes(this, propertyIndex);
	}

	public float GetPropertyDefaultFloatValue(int propertyIndex)
	{
		CheckPropertyIndex(this, propertyIndex);
		ShaderPropertyType propertyType = GetPropertyType(propertyIndex);
		if (propertyType != ShaderPropertyType.Float && propertyType != ShaderPropertyType.Range)
		{
			throw new ArgumentException("Property type is not Float or Range.");
		}
		return GetPropertyDefaultValue(this, propertyIndex)[0];
	}

	public Vector4 GetPropertyDefaultVectorValue(int propertyIndex)
	{
		CheckPropertyIndex(this, propertyIndex);
		ShaderPropertyType propertyType = GetPropertyType(propertyIndex);
		if (propertyType != ShaderPropertyType.Color && propertyType != ShaderPropertyType.Vector)
		{
			throw new ArgumentException("Property type is not Color or Vector.");
		}
		return GetPropertyDefaultValue(this, propertyIndex);
	}

	public Vector2 GetPropertyRangeLimits(int propertyIndex)
	{
		CheckPropertyIndex(this, propertyIndex);
		if (GetPropertyType(propertyIndex) != ShaderPropertyType.Range)
		{
			throw new ArgumentException("Property type is not Range.");
		}
		Vector4 propertyDefaultValue = GetPropertyDefaultValue(this, propertyIndex);
		return new Vector2(propertyDefaultValue[1], propertyDefaultValue[2]);
	}

	public TextureDimension GetPropertyTextureDimension(int propertyIndex)
	{
		CheckPropertyIndex(this, propertyIndex);
		if (GetPropertyType(propertyIndex) != ShaderPropertyType.Texture)
		{
			throw new ArgumentException("Property type is not TexEnv.");
		}
		return GetPropertyTextureDimension(this, propertyIndex);
	}

	public string GetPropertyTextureDefaultName(int propertyIndex)
	{
		CheckPropertyIndex(this, propertyIndex);
		ShaderPropertyType propertyType = GetPropertyType(propertyIndex);
		if (propertyType != ShaderPropertyType.Texture)
		{
			throw new ArgumentException("Property type is not Texture.");
		}
		return GetPropertyTextureDefaultName(this, propertyIndex);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetGlobalVectorImpl_Injected(int name, ref Vector4 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetGlobalMatrixImpl_Injected(int name, ref Matrix4x4 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetGlobalVectorImpl_Injected(int name, out Vector4 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetGlobalMatrixImpl_Injected(int name, out Matrix4x4 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetPropertyDefaultValue_Injected(Shader shader, int propertyIndex, out Vector4 ret);
}
