using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.VFX;

[StructLayout(LayoutKind.Sequential)]
[RequiredByNativeCode]
[NativeType(Header = "Modules/VFX/Public/VFXExpressionValues.h")]
public class VFXExpressionValues
{
	internal IntPtr m_Ptr;

	private VFXExpressionValues()
	{
	}

	[RequiredByNativeCode]
	internal static VFXExpressionValues CreateExpressionValuesWrapper(IntPtr ptr)
	{
		VFXExpressionValues vFXExpressionValues = new VFXExpressionValues();
		vFXExpressionValues.m_Ptr = ptr;
		return vFXExpressionValues;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	[NativeName("GetValueFromScript<bool>")]
	public extern bool GetBool(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	[NativeName("GetValueFromScript<int>")]
	public extern int GetInt(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("GetValueFromScript<UInt32>")]
	[NativeThrows]
	public extern uint GetUInt(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("GetValueFromScript<float>")]
	[NativeThrows]
	public extern float GetFloat(int nameID);

	[NativeThrows]
	[NativeName("GetValueFromScript<Vector2f>")]
	public Vector2 GetVector2(int nameID)
	{
		GetVector2_Injected(nameID, out var ret);
		return ret;
	}

	[NativeName("GetValueFromScript<Vector3f>")]
	[NativeThrows]
	public Vector3 GetVector3(int nameID)
	{
		GetVector3_Injected(nameID, out var ret);
		return ret;
	}

	[NativeName("GetValueFromScript<Vector4f>")]
	[NativeThrows]
	public Vector4 GetVector4(int nameID)
	{
		GetVector4_Injected(nameID, out var ret);
		return ret;
	}

	[NativeName("GetValueFromScript<Matrix4x4f>")]
	[NativeThrows]
	public Matrix4x4 GetMatrix4x4(int nameID)
	{
		GetMatrix4x4_Injected(nameID, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	[NativeName("GetValueFromScript<Texture*>")]
	public extern Texture GetTexture(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("GetValueFromScript<Mesh*>")]
	[NativeThrows]
	public extern Mesh GetMesh(int nameID);

	public AnimationCurve GetAnimationCurve(int nameID)
	{
		AnimationCurve animationCurve = new AnimationCurve();
		Internal_GetAnimationCurveFromScript(nameID, animationCurve);
		return animationCurve;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	internal extern void Internal_GetAnimationCurveFromScript(int nameID, AnimationCurve curve);

	public Gradient GetGradient(int nameID)
	{
		Gradient gradient = new Gradient();
		Internal_GetGradientFromScript(nameID, gradient);
		return gradient;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	internal extern void Internal_GetGradientFromScript(int nameID, Gradient gradient);

	public bool GetBool(string name)
	{
		return GetBool(Shader.PropertyToID(name));
	}

	public int GetInt(string name)
	{
		return GetInt(Shader.PropertyToID(name));
	}

	public uint GetUInt(string name)
	{
		return GetUInt(Shader.PropertyToID(name));
	}

	public float GetFloat(string name)
	{
		return GetFloat(Shader.PropertyToID(name));
	}

	public Vector2 GetVector2(string name)
	{
		return GetVector2(Shader.PropertyToID(name));
	}

	public Vector3 GetVector3(string name)
	{
		return GetVector3(Shader.PropertyToID(name));
	}

	public Vector4 GetVector4(string name)
	{
		return GetVector4(Shader.PropertyToID(name));
	}

	public Matrix4x4 GetMatrix4x4(string name)
	{
		return GetMatrix4x4(Shader.PropertyToID(name));
	}

	public Texture GetTexture(string name)
	{
		return GetTexture(Shader.PropertyToID(name));
	}

	public AnimationCurve GetAnimationCurve(string name)
	{
		return GetAnimationCurve(Shader.PropertyToID(name));
	}

	public Gradient GetGradient(string name)
	{
		return GetGradient(Shader.PropertyToID(name));
	}

	public Mesh GetMesh(string name)
	{
		return GetMesh(Shader.PropertyToID(name));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetVector2_Injected(int nameID, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetVector3_Injected(int nameID, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetVector4_Injected(int nameID, out Vector4 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetMatrix4x4_Injected(int nameID, out Matrix4x4 ret);
}
