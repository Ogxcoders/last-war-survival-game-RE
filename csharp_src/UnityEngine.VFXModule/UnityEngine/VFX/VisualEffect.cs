using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine.VFX;

[RequireComponent(typeof(Transform))]
[NativeHeader("Modules/VFX/Public/VisualEffect.h")]
[NativeHeader("Modules/VFX/Public/ScriptBindings/VisualEffectBindings.h")]
public class VisualEffect : Behaviour
{
	public extern bool pause
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float playRate
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern uint startSeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool resetSeedOnPlay
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int initialEventID
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "VisualEffectBindings::GetInitialEventID", HasExplicitThis = true)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "VisualEffectBindings::SetInitialEventID", HasExplicitThis = true)]
		set;
	}

	public extern string initialEventName
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "VisualEffectBindings::GetInitialEventName", HasExplicitThis = true)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "VisualEffectBindings::SetInitialEventName", HasExplicitThis = true)]
		set;
	}

	public extern bool culled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern VisualEffectAsset visualEffectAsset
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int aliveParticleCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public VFXEventAttribute CreateVFXEventAttribute()
	{
		if (visualEffectAsset == null)
		{
			return null;
		}
		return VFXEventAttribute.Internal_InstanciateVFXEventAttribute(visualEffectAsset);
	}

	private void CheckValidVFXEventAttribute(VFXEventAttribute eventAttribute)
	{
		if (eventAttribute != null && eventAttribute.vfxAsset != visualEffectAsset)
		{
			throw new InvalidOperationException("Invalid VFXEventAttribute provided to VisualEffect, has been created with another VisualEffectAsset");
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::SendEventFromScript", HasExplicitThis = true)]
	private extern void SendEventFromScript(int eventNameID, VFXEventAttribute eventAttribute);

	public void SendEvent(int eventNameID, VFXEventAttribute eventAttribute)
	{
		CheckValidVFXEventAttribute(eventAttribute);
		SendEventFromScript(eventNameID, eventAttribute);
	}

	public void SendEvent(string eventName, VFXEventAttribute eventAttribute)
	{
		SendEvent(Shader.PropertyToID(eventName), eventAttribute);
	}

	public void SendEvent(int eventNameID)
	{
		SendEventFromScript(eventNameID, null);
	}

	public void SendEvent(string eventName)
	{
		SendEvent(Shader.PropertyToID(eventName), null);
	}

	public void Play(VFXEventAttribute eventAttribute)
	{
		SendEvent(VisualEffectAsset.PlayEventID, eventAttribute);
	}

	public void Play()
	{
		SendEvent(VisualEffectAsset.PlayEventID);
	}

	public void Stop(VFXEventAttribute eventAttribute)
	{
		SendEvent(VisualEffectAsset.StopEventID, eventAttribute);
	}

	public void Stop()
	{
		SendEvent(VisualEffectAsset.StopEventID);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Reinit();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void AdvanceOneFrame();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::ResetOverrideFromScript", HasExplicitThis = true)]
	public extern void ResetOverride(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::GetTextureDimensionFromScript", HasExplicitThis = true)]
	public extern TextureDimension GetTextureDimension(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<bool>", HasExplicitThis = true)]
	public extern bool HasBool(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<int>", HasExplicitThis = true)]
	public extern bool HasInt(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<UInt32>", HasExplicitThis = true)]
	public extern bool HasUInt(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<float>", HasExplicitThis = true)]
	public extern bool HasFloat(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Vector2f>", HasExplicitThis = true)]
	public extern bool HasVector2(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Vector3f>", HasExplicitThis = true)]
	public extern bool HasVector3(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Vector4f>", HasExplicitThis = true)]
	public extern bool HasVector4(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Matrix4x4f>", HasExplicitThis = true)]
	public extern bool HasMatrix4x4(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Texture*>", HasExplicitThis = true)]
	public extern bool HasTexture(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<AnimationCurve*>", HasExplicitThis = true)]
	public extern bool HasAnimationCurve(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Gradient*>", HasExplicitThis = true)]
	public extern bool HasGradient(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Mesh*>", HasExplicitThis = true)]
	public extern bool HasMesh(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<bool>", HasExplicitThis = true)]
	public extern void SetBool(int nameID, bool b);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<int>", HasExplicitThis = true)]
	public extern void SetInt(int nameID, int i);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<UInt32>", HasExplicitThis = true)]
	public extern void SetUInt(int nameID, uint i);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<float>", HasExplicitThis = true)]
	public extern void SetFloat(int nameID, float f);

	[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Vector2f>", HasExplicitThis = true)]
	public void SetVector2(int nameID, Vector2 v)
	{
		SetVector2_Injected(nameID, ref v);
	}

	[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Vector3f>", HasExplicitThis = true)]
	public void SetVector3(int nameID, Vector3 v)
	{
		SetVector3_Injected(nameID, ref v);
	}

	[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Vector4f>", HasExplicitThis = true)]
	public void SetVector4(int nameID, Vector4 v)
	{
		SetVector4_Injected(nameID, ref v);
	}

	[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Matrix4x4f>", HasExplicitThis = true)]
	public void SetMatrix4x4(int nameID, Matrix4x4 v)
	{
		SetMatrix4x4_Injected(nameID, ref v);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Texture*>", HasExplicitThis = true)]
	public extern void SetTexture(int nameID, Texture t);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<AnimationCurve*>", HasExplicitThis = true)]
	public extern void SetAnimationCurve(int nameID, AnimationCurve c);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Gradient*>", HasExplicitThis = true)]
	public extern void SetGradient(int nameID, Gradient g);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Mesh*>", HasExplicitThis = true)]
	public extern void SetMesh(int nameID, Mesh m);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<bool>", HasExplicitThis = true)]
	public extern bool GetBool(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<int>", HasExplicitThis = true)]
	public extern int GetInt(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<UInt32>", HasExplicitThis = true)]
	public extern uint GetUInt(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<float>", HasExplicitThis = true)]
	public extern float GetFloat(int nameID);

	[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<Vector2f>", HasExplicitThis = true)]
	public Vector2 GetVector2(int nameID)
	{
		GetVector2_Injected(nameID, out var ret);
		return ret;
	}

	[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<Vector3f>", HasExplicitThis = true)]
	public Vector3 GetVector3(int nameID)
	{
		GetVector3_Injected(nameID, out var ret);
		return ret;
	}

	[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<Vector4f>", HasExplicitThis = true)]
	public Vector4 GetVector4(int nameID)
	{
		GetVector4_Injected(nameID, out var ret);
		return ret;
	}

	[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<Matrix4x4f>", HasExplicitThis = true)]
	public Matrix4x4 GetMatrix4x4(int nameID)
	{
		GetMatrix4x4_Injected(nameID, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<Texture*>", HasExplicitThis = true)]
	public extern Texture GetTexture(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<Mesh*>", HasExplicitThis = true)]
	public extern Mesh GetMesh(int nameID);

	public Gradient GetGradient(int nameID)
	{
		Gradient gradient = new Gradient();
		Internal_GetGradient(nameID, gradient);
		return gradient;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::Internal_GetGradientFromScript", HasExplicitThis = true)]
	private extern void Internal_GetGradient(int nameID, Gradient gradient);

	public AnimationCurve GetAnimationCurve(int nameID)
	{
		AnimationCurve animationCurve = new AnimationCurve();
		Internal_GetAnimationCurve(nameID, animationCurve);
		return animationCurve;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectBindings::Internal_GetAnimationCurveFromScript", HasExplicitThis = true)]
	private extern void Internal_GetAnimationCurve(int nameID, AnimationCurve curve);

	public void ResetOverride(string name)
	{
		ResetOverride(Shader.PropertyToID(name));
	}

	public bool HasInt(string name)
	{
		return HasInt(Shader.PropertyToID(name));
	}

	public bool HasUInt(string name)
	{
		return HasUInt(Shader.PropertyToID(name));
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

	public bool HasTexture(string name)
	{
		return HasTexture(Shader.PropertyToID(name));
	}

	public TextureDimension GetTextureDimension(string name)
	{
		return GetTextureDimension(Shader.PropertyToID(name));
	}

	public bool HasAnimationCurve(string name)
	{
		return HasAnimationCurve(Shader.PropertyToID(name));
	}

	public bool HasGradient(string name)
	{
		return HasGradient(Shader.PropertyToID(name));
	}

	public bool HasMesh(string name)
	{
		return HasMesh(Shader.PropertyToID(name));
	}

	public bool HasBool(string name)
	{
		return HasBool(Shader.PropertyToID(name));
	}

	public void SetInt(string name, int i)
	{
		SetInt(Shader.PropertyToID(name), i);
	}

	public void SetUInt(string name, uint i)
	{
		SetUInt(Shader.PropertyToID(name), i);
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

	public void SetTexture(string name, Texture t)
	{
		SetTexture(Shader.PropertyToID(name), t);
	}

	public void SetAnimationCurve(string name, AnimationCurve c)
	{
		SetAnimationCurve(Shader.PropertyToID(name), c);
	}

	public void SetGradient(string name, Gradient g)
	{
		SetGradient(Shader.PropertyToID(name), g);
	}

	public void SetMesh(string name, Mesh m)
	{
		SetMesh(Shader.PropertyToID(name), m);
	}

	public void SetBool(string name, bool b)
	{
		SetBool(Shader.PropertyToID(name), b);
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

	public Mesh GetMesh(string name)
	{
		return GetMesh(Shader.PropertyToID(name));
	}

	public bool GetBool(string name)
	{
		return GetBool(Shader.PropertyToID(name));
	}

	public AnimationCurve GetAnimationCurve(string name)
	{
		return GetAnimationCurve(Shader.PropertyToID(name));
	}

	public Gradient GetGradient(string name)
	{
		return GetGradient(Shader.PropertyToID(name));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Simulate(float stepDeltaTime, uint stepCount = 1u);

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
