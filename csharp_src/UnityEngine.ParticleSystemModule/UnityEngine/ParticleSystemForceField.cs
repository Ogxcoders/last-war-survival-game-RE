using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/ParticleSystem/ParticleSystemForceFieldManager.h")]
[NativeHeader("Modules/ParticleSystem/ParticleSystemForceField.h")]
[NativeHeader("Modules/ParticleSystem/ParticleSystem.h")]
[NativeHeader("ParticleSystemScriptingClasses.h")]
[NativeHeader("Modules/ParticleSystem/ScriptBindings/ParticleSystemScriptBindings.h")]
[RequireComponent(typeof(Transform))]
public class ParticleSystemForceField : Component
{
	[NativeName("ForceShape")]
	public extern ParticleSystemForceFieldShape shape
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float startRange
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float endRange
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float length
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float gravityFocus
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public Vector2 rotationRandomness
	{
		get
		{
			get_rotationRandomness_Injected(out var ret);
			return ret;
		}
		set
		{
			set_rotationRandomness_Injected(ref value);
		}
	}

	public extern bool multiplyDragByParticleSize
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool multiplyDragByParticleVelocity
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern Texture3D vectorField
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public ParticleSystem.MinMaxCurve directionX
	{
		get
		{
			get_directionX_Injected(out var ret);
			return ret;
		}
		set
		{
			set_directionX_Injected(ref value);
		}
	}

	public ParticleSystem.MinMaxCurve directionY
	{
		get
		{
			get_directionY_Injected(out var ret);
			return ret;
		}
		set
		{
			set_directionY_Injected(ref value);
		}
	}

	public ParticleSystem.MinMaxCurve directionZ
	{
		get
		{
			get_directionZ_Injected(out var ret);
			return ret;
		}
		set
		{
			set_directionZ_Injected(ref value);
		}
	}

	public ParticleSystem.MinMaxCurve gravity
	{
		get
		{
			get_gravity_Injected(out var ret);
			return ret;
		}
		set
		{
			set_gravity_Injected(ref value);
		}
	}

	public ParticleSystem.MinMaxCurve rotationSpeed
	{
		get
		{
			get_rotationSpeed_Injected(out var ret);
			return ret;
		}
		set
		{
			set_rotationSpeed_Injected(ref value);
		}
	}

	public ParticleSystem.MinMaxCurve rotationAttraction
	{
		get
		{
			get_rotationAttraction_Injected(out var ret);
			return ret;
		}
		set
		{
			set_rotationAttraction_Injected(ref value);
		}
	}

	public ParticleSystem.MinMaxCurve drag
	{
		get
		{
			get_drag_Injected(out var ret);
			return ret;
		}
		set
		{
			set_drag_Injected(ref value);
		}
	}

	public ParticleSystem.MinMaxCurve vectorFieldSpeed
	{
		get
		{
			get_vectorFieldSpeed_Injected(out var ret);
			return ret;
		}
		set
		{
			set_vectorFieldSpeed_Injected(ref value);
		}
	}

	public ParticleSystem.MinMaxCurve vectorFieldAttraction
	{
		get
		{
			get_vectorFieldAttraction_Injected(out var ret);
			return ret;
		}
		set
		{
			set_vectorFieldAttraction_Injected(ref value);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_rotationRandomness_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_rotationRandomness_Injected(ref Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_directionX_Injected(out ParticleSystem.MinMaxCurve ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_directionX_Injected(ref ParticleSystem.MinMaxCurve value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_directionY_Injected(out ParticleSystem.MinMaxCurve ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_directionY_Injected(ref ParticleSystem.MinMaxCurve value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_directionZ_Injected(out ParticleSystem.MinMaxCurve ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_directionZ_Injected(ref ParticleSystem.MinMaxCurve value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_gravity_Injected(out ParticleSystem.MinMaxCurve ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_gravity_Injected(ref ParticleSystem.MinMaxCurve value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_rotationSpeed_Injected(out ParticleSystem.MinMaxCurve ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_rotationSpeed_Injected(ref ParticleSystem.MinMaxCurve value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_rotationAttraction_Injected(out ParticleSystem.MinMaxCurve ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_rotationAttraction_Injected(ref ParticleSystem.MinMaxCurve value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_drag_Injected(out ParticleSystem.MinMaxCurve ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_drag_Injected(ref ParticleSystem.MinMaxCurve value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_vectorFieldSpeed_Injected(out ParticleSystem.MinMaxCurve ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_vectorFieldSpeed_Injected(ref ParticleSystem.MinMaxCurve value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_vectorFieldAttraction_Injected(out ParticleSystem.MinMaxCurve ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_vectorFieldAttraction_Injected(ref ParticleSystem.MinMaxCurve value);
}
