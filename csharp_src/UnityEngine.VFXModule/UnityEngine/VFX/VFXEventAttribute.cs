using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.VFX;

[StructLayout(LayoutKind.Sequential)]
[RequiredByNativeCode]
[NativeType(Header = "Modules/VFX/Public/VFXEventAttribute.h")]
public sealed class VFXEventAttribute : IDisposable
{
	private IntPtr m_Ptr;

	private bool m_Owner;

	private VisualEffectAsset m_VfxAsset;

	internal VisualEffectAsset vfxAsset => m_VfxAsset;

	private VFXEventAttribute(IntPtr ptr, bool owner, VisualEffectAsset vfxAsset)
	{
		m_Ptr = ptr;
		m_Owner = owner;
		m_VfxAsset = vfxAsset;
	}

	private VFXEventAttribute()
		: this(IntPtr.Zero, owner: false, null)
	{
	}

	public VFXEventAttribute(VFXEventAttribute original)
	{
		if (original == null)
		{
			throw new ArgumentNullException("VFXEventAttribute expect a non null attribute");
		}
		m_Ptr = Internal_Create();
		m_VfxAsset = original.m_VfxAsset;
		Internal_InitFromEventAttribute(original);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern IntPtr Internal_Create();

	internal static VFXEventAttribute Internal_InstanciateVFXEventAttribute(VisualEffectAsset vfxAsset)
	{
		VFXEventAttribute vFXEventAttribute = new VFXEventAttribute(Internal_Create(), owner: true, vfxAsset);
		vFXEventAttribute.Internal_InitFromAsset(vfxAsset);
		return vFXEventAttribute;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern void Internal_InitFromAsset(VisualEffectAsset vfxAsset);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern void Internal_InitFromEventAttribute(VFXEventAttribute vfxEventAttribute);

	private void Release()
	{
		if (m_Owner && m_Ptr != IntPtr.Zero)
		{
			Internal_Destroy(m_Ptr);
		}
		m_Ptr = IntPtr.Zero;
		m_VfxAsset = null;
	}

	~VFXEventAttribute()
	{
		Release();
	}

	public void Dispose()
	{
		Release();
		GC.SuppressFinalize(this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	internal static extern void Internal_Destroy(IntPtr ptr);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("HasValueFromScript<bool>")]
	public extern bool HasBool(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("HasValueFromScript<int>")]
	public extern bool HasInt(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("HasValueFromScript<UInt32>")]
	public extern bool HasUint(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("HasValueFromScript<float>")]
	public extern bool HasFloat(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("HasValueFromScript<Vector2f>")]
	public extern bool HasVector2(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("HasValueFromScript<Vector3f>")]
	public extern bool HasVector3(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("HasValueFromScript<Vector4f>")]
	public extern bool HasVector4(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("HasValueFromScript<Matrix4x4f>")]
	public extern bool HasMatrix4x4(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("SetValueFromScript<bool>")]
	public extern void SetBool(int nameID, bool b);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("SetValueFromScript<int>")]
	public extern void SetInt(int nameID, int i);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("SetValueFromScript<UInt32>")]
	public extern void SetUint(int nameID, uint i);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("SetValueFromScript<float>")]
	public extern void SetFloat(int nameID, float f);

	[NativeName("SetValueFromScript<Vector2f>")]
	public void SetVector2(int nameID, Vector2 v)
	{
		SetVector2_Injected(nameID, ref v);
	}

	[NativeName("SetValueFromScript<Vector3f>")]
	public void SetVector3(int nameID, Vector3 v)
	{
		SetVector3_Injected(nameID, ref v);
	}

	[NativeName("SetValueFromScript<Vector4f>")]
	public void SetVector4(int nameID, Vector4 v)
	{
		SetVector4_Injected(nameID, ref v);
	}

	[NativeName("SetValueFromScript<Matrix4x4f>")]
	public void SetMatrix4x4(int nameID, Matrix4x4 v)
	{
		SetMatrix4x4_Injected(nameID, ref v);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("GetValueFromScript<bool>")]
	public extern bool GetBool(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("GetValueFromScript<int>")]
	public extern int GetInt(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("GetValueFromScript<UInt32>")]
	public extern uint GetUint(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("GetValueFromScript<float>")]
	public extern float GetFloat(int nameID);

	[NativeName("GetValueFromScript<Vector2f>")]
	public Vector2 GetVector2(int nameID)
	{
		GetVector2_Injected(nameID, out var ret);
		return ret;
	}

	[NativeName("GetValueFromScript<Vector3f>")]
	public Vector3 GetVector3(int nameID)
	{
		GetVector3_Injected(nameID, out var ret);
		return ret;
	}

	[NativeName("GetValueFromScript<Vector4f>")]
	public Vector4 GetVector4(int nameID)
	{
		GetVector4_Injected(nameID, out var ret);
		return ret;
	}

	[NativeName("GetValueFromScript<Matrix4x4f>")]
	public Matrix4x4 GetMatrix4x4(int nameID)
	{
		GetMatrix4x4_Injected(nameID, out var ret);
		return ret;
	}

	public bool HasBool(string name)
	{
		return HasBool(Shader.PropertyToID(name));
	}

	public bool HasInt(string name)
	{
		return HasInt(Shader.PropertyToID(name));
	}

	public bool HasUint(string name)
	{
		return HasUint(Shader.PropertyToID(name));
	}

	public bool HasFloat(string name)
	{
		return HasFloat(Shader.PropertyToID(name));
	}

	public bool HasVector2(string name)
	{
		return HasVector2(Shader.PropertyToID(name));
	}

	public bool HasVector3(string name)
	{
		return HasVector3(Shader.PropertyToID(name));
	}

	public bool HasVector4(string name)
	{
		return HasVector4(Shader.PropertyToID(name));
	}

	public bool HasMatrix4x4(string name)
	{
		return HasMatrix4x4(Shader.PropertyToID(name));
	}

	public void SetBool(string name, bool b)
	{
		SetBool(Shader.PropertyToID(name), b);
	}

	public void SetInt(string name, int i)
	{
		SetInt(Shader.PropertyToID(name), i);
	}

	public void SetUint(string name, uint i)
	{
		SetUint(Shader.PropertyToID(name), i);
	}

	public void SetFloat(string name, float f)
	{
		SetFloat(Shader.PropertyToID(name), f);
	}

	public void SetVector2(string name, Vector2 v)
	{
		SetVector2(Shader.PropertyToID(name), v);
	}

	public void SetVector3(string name, Vector3 v)
	{
		SetVector3(Shader.PropertyToID(name), v);
	}

	public void SetVector4(string name, Vector4 v)
	{
		SetVector4(Shader.PropertyToID(name), v);
	}

	public void SetMatrix4x4(string name, Matrix4x4 v)
	{
		SetMatrix4x4(Shader.PropertyToID(name), v);
	}

	public bool GetBool(string name)
	{
		return GetBool(Shader.PropertyToID(name));
	}

	public int GetInt(string name)
	{
		return GetInt(Shader.PropertyToID(name));
	}

	public uint GetUint(string name)
	{
		return GetUint(Shader.PropertyToID(name));
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

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void CopyValuesFrom([NotNull] VFXEventAttribute eventAttibute);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetVector2_Injected(int nameID, ref Vector2 v);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetVector3_Injected(int nameID, ref Vector3 v);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetVector4_Injected(int nameID, ref Vector4 v);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetMatrix4x4_Injected(int nameID, ref Matrix4x4 v);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetVector2_Injected(int nameID, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetVector3_Injected(int nameID, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetVector4_Injected(int nameID, out Vector4 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetMatrix4x4_Injected(int nameID, out Matrix4x4 ret);
}
