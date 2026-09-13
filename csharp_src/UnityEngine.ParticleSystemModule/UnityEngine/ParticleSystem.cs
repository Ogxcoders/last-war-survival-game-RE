using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.ParticleSystemJobs;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Modules/ParticleSystem/ScriptBindings/ParticleSystemModulesScriptBindings.h")]
[NativeHeader("Modules/ParticleSystem/ScriptBindings/ParticleSystemScriptBindings.h")]
[NativeHeader("Modules/ParticleSystem/ParticleSystem.h")]
[NativeHeader("ParticleSystemScriptingClasses.h")]
[NativeHeader("ParticleSystemScriptingClasses.h")]
[NativeHeader("Modules/ParticleSystem/ParticleSystem.h")]
[RequireComponent(typeof(Transform))]
[UsedByNativeCode]
[NativeHeader("Modules/ParticleSystem/ScriptBindings/ParticleSystemScriptBindings.h")]
[NativeHeader("Modules/ParticleSystem/ParticleSystemGeometryJob.h")]
public sealed class ParticleSystem : Component
{
	public struct MainModule
	{
		internal ParticleSystem m_ParticleSystem;

		[Obsolete("Please use flipRotation instead. (UnityUpgradable) -> UnityEngine.ParticleSystem/MainModule.flipRotation", false)]
		public float randomizeRotationDirection
		{
			get
			{
				return flipRotation;
			}
			set
			{
				flipRotation = value;
			}
		}

		public float duration
		{
			get
			{
				return get_duration_Injected(ref this);
			}
			set
			{
				set_duration_Injected(ref this, value);
			}
		}

		public bool loop
		{
			get
			{
				return get_loop_Injected(ref this);
			}
			set
			{
				set_loop_Injected(ref this, value);
			}
		}

		public bool prewarm
		{
			get
			{
				return get_prewarm_Injected(ref this);
			}
			set
			{
				set_prewarm_Injected(ref this, value);
			}
		}

		public MinMaxCurve startDelay
		{
			get
			{
				get_startDelay_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_startDelay_Injected(ref this, ref value);
			}
		}

		public float startDelayMultiplier
		{
			get
			{
				return get_startDelayMultiplier_Injected(ref this);
			}
			set
			{
				set_startDelayMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve startLifetime
		{
			get
			{
				get_startLifetime_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_startLifetime_Injected(ref this, ref value);
			}
		}

		public float startLifetimeMultiplier
		{
			get
			{
				return get_startLifetimeMultiplier_Injected(ref this);
			}
			set
			{
				set_startLifetimeMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve startSpeed
		{
			get
			{
				get_startSpeed_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_startSpeed_Injected(ref this, ref value);
			}
		}

		public float startSpeedMultiplier
		{
			get
			{
				return get_startSpeedMultiplier_Injected(ref this);
			}
			set
			{
				set_startSpeedMultiplier_Injected(ref this, value);
			}
		}

		public bool startSize3D
		{
			get
			{
				return get_startSize3D_Injected(ref this);
			}
			set
			{
				set_startSize3D_Injected(ref this, value);
			}
		}

		[NativeName("StartSizeX")]
		public MinMaxCurve startSize
		{
			get
			{
				get_startSize_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_startSize_Injected(ref this, ref value);
			}
		}

		[NativeName("StartSizeXMultiplier")]
		public float startSizeMultiplier
		{
			get
			{
				return get_startSizeMultiplier_Injected(ref this);
			}
			set
			{
				set_startSizeMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve startSizeX
		{
			get
			{
				get_startSizeX_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_startSizeX_Injected(ref this, ref value);
			}
		}

		public float startSizeXMultiplier
		{
			get
			{
				return get_startSizeXMultiplier_Injected(ref this);
			}
			set
			{
				set_startSizeXMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve startSizeY
		{
			get
			{
				get_startSizeY_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_startSizeY_Injected(ref this, ref value);
			}
		}

		public float startSizeYMultiplier
		{
			get
			{
				return get_startSizeYMultiplier_Injected(ref this);
			}
			set
			{
				set_startSizeYMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve startSizeZ
		{
			get
			{
				get_startSizeZ_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_startSizeZ_Injected(ref this, ref value);
			}
		}

		public float startSizeZMultiplier
		{
			get
			{
				return get_startSizeZMultiplier_Injected(ref this);
			}
			set
			{
				set_startSizeZMultiplier_Injected(ref this, value);
			}
		}

		public bool startRotation3D
		{
			get
			{
				return get_startRotation3D_Injected(ref this);
			}
			set
			{
				set_startRotation3D_Injected(ref this, value);
			}
		}

		[NativeName("StartRotationZ")]
		public MinMaxCurve startRotation
		{
			get
			{
				get_startRotation_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_startRotation_Injected(ref this, ref value);
			}
		}

		[NativeName("StartRotationZMultiplier")]
		public float startRotationMultiplier
		{
			get
			{
				return get_startRotationMultiplier_Injected(ref this);
			}
			set
			{
				set_startRotationMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve startRotationX
		{
			get
			{
				get_startRotationX_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_startRotationX_Injected(ref this, ref value);
			}
		}

		public float startRotationXMultiplier
		{
			get
			{
				return get_startRotationXMultiplier_Injected(ref this);
			}
			set
			{
				set_startRotationXMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve startRotationY
		{
			get
			{
				get_startRotationY_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_startRotationY_Injected(ref this, ref value);
			}
		}

		public float startRotationYMultiplier
		{
			get
			{
				return get_startRotationYMultiplier_Injected(ref this);
			}
			set
			{
				set_startRotationYMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve startRotationZ
		{
			get
			{
				get_startRotationZ_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_startRotationZ_Injected(ref this, ref value);
			}
		}

		public float startRotationZMultiplier
		{
			get
			{
				return get_startRotationZMultiplier_Injected(ref this);
			}
			set
			{
				set_startRotationZMultiplier_Injected(ref this, value);
			}
		}

		public float flipRotation
		{
			get
			{
				return get_flipRotation_Injected(ref this);
			}
			set
			{
				set_flipRotation_Injected(ref this, value);
			}
		}

		public MinMaxGradient startColor
		{
			get
			{
				get_startColor_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_startColor_Injected(ref this, ref value);
			}
		}

		public MinMaxCurve gravityModifier
		{
			get
			{
				get_gravityModifier_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_gravityModifier_Injected(ref this, ref value);
			}
		}

		public float gravityModifierMultiplier
		{
			get
			{
				return get_gravityModifierMultiplier_Injected(ref this);
			}
			set
			{
				set_gravityModifierMultiplier_Injected(ref this, value);
			}
		}

		public ParticleSystemSimulationSpace simulationSpace
		{
			get
			{
				return get_simulationSpace_Injected(ref this);
			}
			set
			{
				set_simulationSpace_Injected(ref this, value);
			}
		}

		public Transform customSimulationSpace
		{
			get
			{
				return get_customSimulationSpace_Injected(ref this);
			}
			set
			{
				set_customSimulationSpace_Injected(ref this, value);
			}
		}

		public float simulationSpeed
		{
			get
			{
				return get_simulationSpeed_Injected(ref this);
			}
			set
			{
				set_simulationSpeed_Injected(ref this, value);
			}
		}

		public bool useUnscaledTime
		{
			get
			{
				return get_useUnscaledTime_Injected(ref this);
			}
			set
			{
				set_useUnscaledTime_Injected(ref this, value);
			}
		}

		public ParticleSystemScalingMode scalingMode
		{
			get
			{
				return get_scalingMode_Injected(ref this);
			}
			set
			{
				set_scalingMode_Injected(ref this, value);
			}
		}

		public bool playOnAwake
		{
			get
			{
				return get_playOnAwake_Injected(ref this);
			}
			set
			{
				set_playOnAwake_Injected(ref this, value);
			}
		}

		public int maxParticles
		{
			get
			{
				return get_maxParticles_Injected(ref this);
			}
			set
			{
				set_maxParticles_Injected(ref this, value);
			}
		}

		public ParticleSystemEmitterVelocityMode emitterVelocityMode
		{
			get
			{
				return get_emitterVelocityMode_Injected(ref this);
			}
			set
			{
				set_emitterVelocityMode_Injected(ref this, value);
			}
		}

		public ParticleSystemStopAction stopAction
		{
			get
			{
				return get_stopAction_Injected(ref this);
			}
			set
			{
				set_stopAction_Injected(ref this, value);
			}
		}

		public ParticleSystemRingBufferMode ringBufferMode
		{
			get
			{
				return get_ringBufferMode_Injected(ref this);
			}
			set
			{
				set_ringBufferMode_Injected(ref this, value);
			}
		}

		public Vector2 ringBufferLoopRange
		{
			get
			{
				get_ringBufferLoopRange_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_ringBufferLoopRange_Injected(ref this, ref value);
			}
		}

		public ParticleSystemCullingMode cullingMode
		{
			get
			{
				return get_cullingMode_Injected(ref this);
			}
			set
			{
				set_cullingMode_Injected(ref this, value);
			}
		}

		internal MainModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_duration_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_duration_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_loop_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_loop_Injected(ref MainModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_prewarm_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_prewarm_Injected(ref MainModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_startDelay_Injected(ref MainModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startDelay_Injected(ref MainModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_startDelayMultiplier_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startDelayMultiplier_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_startLifetime_Injected(ref MainModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startLifetime_Injected(ref MainModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_startLifetimeMultiplier_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startLifetimeMultiplier_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_startSpeed_Injected(ref MainModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startSpeed_Injected(ref MainModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_startSpeedMultiplier_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startSpeedMultiplier_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_startSize3D_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startSize3D_Injected(ref MainModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_startSize_Injected(ref MainModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startSize_Injected(ref MainModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_startSizeMultiplier_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startSizeMultiplier_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_startSizeX_Injected(ref MainModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startSizeX_Injected(ref MainModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_startSizeXMultiplier_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startSizeXMultiplier_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_startSizeY_Injected(ref MainModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startSizeY_Injected(ref MainModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_startSizeYMultiplier_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startSizeYMultiplier_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_startSizeZ_Injected(ref MainModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startSizeZ_Injected(ref MainModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_startSizeZMultiplier_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startSizeZMultiplier_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_startRotation3D_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startRotation3D_Injected(ref MainModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_startRotation_Injected(ref MainModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startRotation_Injected(ref MainModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_startRotationMultiplier_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startRotationMultiplier_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_startRotationX_Injected(ref MainModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startRotationX_Injected(ref MainModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_startRotationXMultiplier_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startRotationXMultiplier_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_startRotationY_Injected(ref MainModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startRotationY_Injected(ref MainModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_startRotationYMultiplier_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startRotationYMultiplier_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_startRotationZ_Injected(ref MainModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startRotationZ_Injected(ref MainModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_startRotationZMultiplier_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startRotationZMultiplier_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_flipRotation_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_flipRotation_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_startColor_Injected(ref MainModule _unity_self, out MinMaxGradient ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startColor_Injected(ref MainModule _unity_self, ref MinMaxGradient value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_gravityModifier_Injected(ref MainModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_gravityModifier_Injected(ref MainModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_gravityModifierMultiplier_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_gravityModifierMultiplier_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemSimulationSpace get_simulationSpace_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_simulationSpace_Injected(ref MainModule _unity_self, ParticleSystemSimulationSpace value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern Transform get_customSimulationSpace_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_customSimulationSpace_Injected(ref MainModule _unity_self, Transform value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_simulationSpeed_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_simulationSpeed_Injected(ref MainModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_useUnscaledTime_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_useUnscaledTime_Injected(ref MainModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemScalingMode get_scalingMode_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_scalingMode_Injected(ref MainModule _unity_self, ParticleSystemScalingMode value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_playOnAwake_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_playOnAwake_Injected(ref MainModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_maxParticles_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_maxParticles_Injected(ref MainModule _unity_self, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemEmitterVelocityMode get_emitterVelocityMode_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_emitterVelocityMode_Injected(ref MainModule _unity_self, ParticleSystemEmitterVelocityMode value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemStopAction get_stopAction_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_stopAction_Injected(ref MainModule _unity_self, ParticleSystemStopAction value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemRingBufferMode get_ringBufferMode_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_ringBufferMode_Injected(ref MainModule _unity_self, ParticleSystemRingBufferMode value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_ringBufferLoopRange_Injected(ref MainModule _unity_self, out Vector2 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_ringBufferLoopRange_Injected(ref MainModule _unity_self, ref Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemCullingMode get_cullingMode_Injected(ref MainModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_cullingMode_Injected(ref MainModule _unity_self, ParticleSystemCullingMode value);
	}

	public struct EmissionModule
	{
		internal ParticleSystem m_ParticleSystem;

		[Obsolete("ParticleSystemEmissionType no longer does anything. Time and Distance based emission are now both always active.", false)]
		public ParticleSystemEmissionType type
		{
			get
			{
				return ParticleSystemEmissionType.Time;
			}
			set
			{
			}
		}

		[Obsolete("rate property is deprecated. Use rateOverTime or rateOverDistance instead.", false)]
		public MinMaxCurve rate
		{
			get
			{
				return rateOverTime;
			}
			set
			{
				rateOverTime = value;
			}
		}

		[Obsolete("rateMultiplier property is deprecated. Use rateOverTimeMultiplier or rateOverDistanceMultiplier instead.", false)]
		public float rateMultiplier
		{
			get
			{
				return rateOverTimeMultiplier;
			}
			set
			{
				rateOverTimeMultiplier = value;
			}
		}

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public MinMaxCurve rateOverTime
		{
			get
			{
				get_rateOverTime_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_rateOverTime_Injected(ref this, ref value);
			}
		}

		public float rateOverTimeMultiplier
		{
			get
			{
				return get_rateOverTimeMultiplier_Injected(ref this);
			}
			set
			{
				set_rateOverTimeMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve rateOverDistance
		{
			get
			{
				get_rateOverDistance_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_rateOverDistance_Injected(ref this, ref value);
			}
		}

		public float rateOverDistanceMultiplier
		{
			get
			{
				return get_rateOverDistanceMultiplier_Injected(ref this);
			}
			set
			{
				set_rateOverDistanceMultiplier_Injected(ref this, value);
			}
		}

		public int burstCount
		{
			get
			{
				return get_burstCount_Injected(ref this);
			}
			set
			{
				set_burstCount_Injected(ref this, value);
			}
		}

		internal EmissionModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		public void SetBursts(Burst[] bursts)
		{
			SetBursts(bursts, bursts.Length);
		}

		public void SetBursts(Burst[] bursts, int size)
		{
			burstCount = size;
			for (int i = 0; i < size; i++)
			{
				SetBurst(i, bursts[i]);
			}
		}

		public int GetBursts(Burst[] bursts)
		{
			int num = burstCount;
			for (int i = 0; i < num; i++)
			{
				bursts[i] = GetBurst(i);
			}
			return num;
		}

		public void SetBurst(int index, Burst burst)
		{
			SetBurst_Injected(ref this, index, ref burst);
		}

		public Burst GetBurst(int index)
		{
			GetBurst_Injected(ref this, index, out var ret);
			return ret;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref EmissionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref EmissionModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_rateOverTime_Injected(ref EmissionModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_rateOverTime_Injected(ref EmissionModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_rateOverTimeMultiplier_Injected(ref EmissionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_rateOverTimeMultiplier_Injected(ref EmissionModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_rateOverDistance_Injected(ref EmissionModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_rateOverDistance_Injected(ref EmissionModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_rateOverDistanceMultiplier_Injected(ref EmissionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_rateOverDistanceMultiplier_Injected(ref EmissionModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetBurst_Injected(ref EmissionModule _unity_self, int index, ref Burst burst);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetBurst_Injected(ref EmissionModule _unity_self, int index, out Burst ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_burstCount_Injected(ref EmissionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_burstCount_Injected(ref EmissionModule _unity_self, int value);
	}

	public struct ShapeModule
	{
		internal ParticleSystem m_ParticleSystem;

		[Obsolete("Please use scale instead. (UnityUpgradable) -> UnityEngine.ParticleSystem/ShapeModule.scale", false)]
		public Vector3 box
		{
			get
			{
				return scale;
			}
			set
			{
				scale = value;
			}
		}

		[Obsolete("meshScale property is deprecated.Please use scale instead.", false)]
		public float meshScale
		{
			get
			{
				return scale.x;
			}
			set
			{
				scale = new Vector3(value, value, value);
			}
		}

		[Obsolete("randomDirection property is deprecated. Use randomDirectionAmount instead.", false)]
		public bool randomDirection
		{
			get
			{
				return randomDirectionAmount >= 0.5f;
			}
			set
			{
				randomDirectionAmount = (value ? 1f : 0f);
			}
		}

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public ParticleSystemShapeType shapeType
		{
			get
			{
				return get_shapeType_Injected(ref this);
			}
			set
			{
				set_shapeType_Injected(ref this, value);
			}
		}

		public float randomDirectionAmount
		{
			get
			{
				return get_randomDirectionAmount_Injected(ref this);
			}
			set
			{
				set_randomDirectionAmount_Injected(ref this, value);
			}
		}

		public float sphericalDirectionAmount
		{
			get
			{
				return get_sphericalDirectionAmount_Injected(ref this);
			}
			set
			{
				set_sphericalDirectionAmount_Injected(ref this, value);
			}
		}

		public float randomPositionAmount
		{
			get
			{
				return get_randomPositionAmount_Injected(ref this);
			}
			set
			{
				set_randomPositionAmount_Injected(ref this, value);
			}
		}

		public bool alignToDirection
		{
			get
			{
				return get_alignToDirection_Injected(ref this);
			}
			set
			{
				set_alignToDirection_Injected(ref this, value);
			}
		}

		public float radius
		{
			get
			{
				return get_radius_Injected(ref this);
			}
			set
			{
				set_radius_Injected(ref this, value);
			}
		}

		public ParticleSystemShapeMultiModeValue radiusMode
		{
			get
			{
				return get_radiusMode_Injected(ref this);
			}
			set
			{
				set_radiusMode_Injected(ref this, value);
			}
		}

		public float radiusSpread
		{
			get
			{
				return get_radiusSpread_Injected(ref this);
			}
			set
			{
				set_radiusSpread_Injected(ref this, value);
			}
		}

		public MinMaxCurve radiusSpeed
		{
			get
			{
				get_radiusSpeed_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_radiusSpeed_Injected(ref this, ref value);
			}
		}

		public float radiusSpeedMultiplier
		{
			get
			{
				return get_radiusSpeedMultiplier_Injected(ref this);
			}
			set
			{
				set_radiusSpeedMultiplier_Injected(ref this, value);
			}
		}

		public float radiusThickness
		{
			get
			{
				return get_radiusThickness_Injected(ref this);
			}
			set
			{
				set_radiusThickness_Injected(ref this, value);
			}
		}

		public float angle
		{
			get
			{
				return get_angle_Injected(ref this);
			}
			set
			{
				set_angle_Injected(ref this, value);
			}
		}

		public float length
		{
			get
			{
				return get_length_Injected(ref this);
			}
			set
			{
				set_length_Injected(ref this, value);
			}
		}

		public Vector3 boxThickness
		{
			get
			{
				get_boxThickness_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_boxThickness_Injected(ref this, ref value);
			}
		}

		public ParticleSystemMeshShapeType meshShapeType
		{
			get
			{
				return get_meshShapeType_Injected(ref this);
			}
			set
			{
				set_meshShapeType_Injected(ref this, value);
			}
		}

		public Mesh mesh
		{
			get
			{
				return get_mesh_Injected(ref this);
			}
			set
			{
				set_mesh_Injected(ref this, value);
			}
		}

		public MeshRenderer meshRenderer
		{
			get
			{
				return get_meshRenderer_Injected(ref this);
			}
			set
			{
				set_meshRenderer_Injected(ref this, value);
			}
		}

		public SkinnedMeshRenderer skinnedMeshRenderer
		{
			get
			{
				return get_skinnedMeshRenderer_Injected(ref this);
			}
			set
			{
				set_skinnedMeshRenderer_Injected(ref this, value);
			}
		}

		public Sprite sprite
		{
			get
			{
				return get_sprite_Injected(ref this);
			}
			set
			{
				set_sprite_Injected(ref this, value);
			}
		}

		public SpriteRenderer spriteRenderer
		{
			get
			{
				return get_spriteRenderer_Injected(ref this);
			}
			set
			{
				set_spriteRenderer_Injected(ref this, value);
			}
		}

		public bool useMeshMaterialIndex
		{
			get
			{
				return get_useMeshMaterialIndex_Injected(ref this);
			}
			set
			{
				set_useMeshMaterialIndex_Injected(ref this, value);
			}
		}

		public int meshMaterialIndex
		{
			get
			{
				return get_meshMaterialIndex_Injected(ref this);
			}
			set
			{
				set_meshMaterialIndex_Injected(ref this, value);
			}
		}

		public bool useMeshColors
		{
			get
			{
				return get_useMeshColors_Injected(ref this);
			}
			set
			{
				set_useMeshColors_Injected(ref this, value);
			}
		}

		public float normalOffset
		{
			get
			{
				return get_normalOffset_Injected(ref this);
			}
			set
			{
				set_normalOffset_Injected(ref this, value);
			}
		}

		public ParticleSystemShapeMultiModeValue meshSpawnMode
		{
			get
			{
				return get_meshSpawnMode_Injected(ref this);
			}
			set
			{
				set_meshSpawnMode_Injected(ref this, value);
			}
		}

		public float meshSpawnSpread
		{
			get
			{
				return get_meshSpawnSpread_Injected(ref this);
			}
			set
			{
				set_meshSpawnSpread_Injected(ref this, value);
			}
		}

		public MinMaxCurve meshSpawnSpeed
		{
			get
			{
				get_meshSpawnSpeed_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_meshSpawnSpeed_Injected(ref this, ref value);
			}
		}

		public float meshSpawnSpeedMultiplier
		{
			get
			{
				return get_meshSpawnSpeedMultiplier_Injected(ref this);
			}
			set
			{
				set_meshSpawnSpeedMultiplier_Injected(ref this, value);
			}
		}

		public float arc
		{
			get
			{
				return get_arc_Injected(ref this);
			}
			set
			{
				set_arc_Injected(ref this, value);
			}
		}

		public ParticleSystemShapeMultiModeValue arcMode
		{
			get
			{
				return get_arcMode_Injected(ref this);
			}
			set
			{
				set_arcMode_Injected(ref this, value);
			}
		}

		public float arcSpread
		{
			get
			{
				return get_arcSpread_Injected(ref this);
			}
			set
			{
				set_arcSpread_Injected(ref this, value);
			}
		}

		public MinMaxCurve arcSpeed
		{
			get
			{
				get_arcSpeed_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_arcSpeed_Injected(ref this, ref value);
			}
		}

		public float arcSpeedMultiplier
		{
			get
			{
				return get_arcSpeedMultiplier_Injected(ref this);
			}
			set
			{
				set_arcSpeedMultiplier_Injected(ref this, value);
			}
		}

		public float donutRadius
		{
			get
			{
				return get_donutRadius_Injected(ref this);
			}
			set
			{
				set_donutRadius_Injected(ref this, value);
			}
		}

		public Vector3 position
		{
			get
			{
				get_position_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_position_Injected(ref this, ref value);
			}
		}

		public Vector3 rotation
		{
			get
			{
				get_rotation_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_rotation_Injected(ref this, ref value);
			}
		}

		public Vector3 scale
		{
			get
			{
				get_scale_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_scale_Injected(ref this, ref value);
			}
		}

		public Texture2D texture
		{
			get
			{
				return get_texture_Injected(ref this);
			}
			set
			{
				set_texture_Injected(ref this, value);
			}
		}

		public ParticleSystemShapeTextureChannel textureClipChannel
		{
			get
			{
				return get_textureClipChannel_Injected(ref this);
			}
			set
			{
				set_textureClipChannel_Injected(ref this, value);
			}
		}

		public float textureClipThreshold
		{
			get
			{
				return get_textureClipThreshold_Injected(ref this);
			}
			set
			{
				set_textureClipThreshold_Injected(ref this, value);
			}
		}

		public bool textureColorAffectsParticles
		{
			get
			{
				return get_textureColorAffectsParticles_Injected(ref this);
			}
			set
			{
				set_textureColorAffectsParticles_Injected(ref this, value);
			}
		}

		public bool textureAlphaAffectsParticles
		{
			get
			{
				return get_textureAlphaAffectsParticles_Injected(ref this);
			}
			set
			{
				set_textureAlphaAffectsParticles_Injected(ref this, value);
			}
		}

		public bool textureBilinearFiltering
		{
			get
			{
				return get_textureBilinearFiltering_Injected(ref this);
			}
			set
			{
				set_textureBilinearFiltering_Injected(ref this, value);
			}
		}

		public int textureUVChannel
		{
			get
			{
				return get_textureUVChannel_Injected(ref this);
			}
			set
			{
				set_textureUVChannel_Injected(ref this, value);
			}
		}

		internal ShapeModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref ShapeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemShapeType get_shapeType_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_shapeType_Injected(ref ShapeModule _unity_self, ParticleSystemShapeType value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_randomDirectionAmount_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_randomDirectionAmount_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_sphericalDirectionAmount_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_sphericalDirectionAmount_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_randomPositionAmount_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_randomPositionAmount_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_alignToDirection_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_alignToDirection_Injected(ref ShapeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_radius_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_radius_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemShapeMultiModeValue get_radiusMode_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_radiusMode_Injected(ref ShapeModule _unity_self, ParticleSystemShapeMultiModeValue value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_radiusSpread_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_radiusSpread_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_radiusSpeed_Injected(ref ShapeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_radiusSpeed_Injected(ref ShapeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_radiusSpeedMultiplier_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_radiusSpeedMultiplier_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_radiusThickness_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_radiusThickness_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_angle_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_angle_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_length_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_length_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_boxThickness_Injected(ref ShapeModule _unity_self, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_boxThickness_Injected(ref ShapeModule _unity_self, ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemMeshShapeType get_meshShapeType_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_meshShapeType_Injected(ref ShapeModule _unity_self, ParticleSystemMeshShapeType value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern Mesh get_mesh_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_mesh_Injected(ref ShapeModule _unity_self, Mesh value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern MeshRenderer get_meshRenderer_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_meshRenderer_Injected(ref ShapeModule _unity_self, MeshRenderer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern SkinnedMeshRenderer get_skinnedMeshRenderer_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_skinnedMeshRenderer_Injected(ref ShapeModule _unity_self, SkinnedMeshRenderer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern Sprite get_sprite_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_sprite_Injected(ref ShapeModule _unity_self, Sprite value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern SpriteRenderer get_spriteRenderer_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_spriteRenderer_Injected(ref ShapeModule _unity_self, SpriteRenderer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_useMeshMaterialIndex_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_useMeshMaterialIndex_Injected(ref ShapeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_meshMaterialIndex_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_meshMaterialIndex_Injected(ref ShapeModule _unity_self, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_useMeshColors_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_useMeshColors_Injected(ref ShapeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_normalOffset_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_normalOffset_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemShapeMultiModeValue get_meshSpawnMode_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_meshSpawnMode_Injected(ref ShapeModule _unity_self, ParticleSystemShapeMultiModeValue value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_meshSpawnSpread_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_meshSpawnSpread_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_meshSpawnSpeed_Injected(ref ShapeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_meshSpawnSpeed_Injected(ref ShapeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_meshSpawnSpeedMultiplier_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_meshSpawnSpeedMultiplier_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_arc_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_arc_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemShapeMultiModeValue get_arcMode_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_arcMode_Injected(ref ShapeModule _unity_self, ParticleSystemShapeMultiModeValue value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_arcSpread_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_arcSpread_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_arcSpeed_Injected(ref ShapeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_arcSpeed_Injected(ref ShapeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_arcSpeedMultiplier_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_arcSpeedMultiplier_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_donutRadius_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_donutRadius_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_position_Injected(ref ShapeModule _unity_self, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_position_Injected(ref ShapeModule _unity_self, ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_rotation_Injected(ref ShapeModule _unity_self, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_rotation_Injected(ref ShapeModule _unity_self, ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_scale_Injected(ref ShapeModule _unity_self, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_scale_Injected(ref ShapeModule _unity_self, ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern Texture2D get_texture_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_texture_Injected(ref ShapeModule _unity_self, Texture2D value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemShapeTextureChannel get_textureClipChannel_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_textureClipChannel_Injected(ref ShapeModule _unity_self, ParticleSystemShapeTextureChannel value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_textureClipThreshold_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_textureClipThreshold_Injected(ref ShapeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_textureColorAffectsParticles_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_textureColorAffectsParticles_Injected(ref ShapeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_textureAlphaAffectsParticles_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_textureAlphaAffectsParticles_Injected(ref ShapeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_textureBilinearFiltering_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_textureBilinearFiltering_Injected(ref ShapeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_textureUVChannel_Injected(ref ShapeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_textureUVChannel_Injected(ref ShapeModule _unity_self, int value);
	}

	public struct SubEmittersModule
	{
		internal ParticleSystem m_ParticleSystem;

		[Obsolete("birth0 property is deprecated. Use AddSubEmitter, RemoveSubEmitter, SetSubEmitterSystem and GetSubEmitterSystem instead.", false)]
		public ParticleSystem birth0
		{
			get
			{
				ThrowNotImplemented();
				return null;
			}
			set
			{
				ThrowNotImplemented();
			}
		}

		[Obsolete("birth1 property is deprecated. Use AddSubEmitter, RemoveSubEmitter, SetSubEmitterSystem and GetSubEmitterSystem instead.", false)]
		public ParticleSystem birth1
		{
			get
			{
				ThrowNotImplemented();
				return null;
			}
			set
			{
				ThrowNotImplemented();
			}
		}

		[Obsolete("collision0 property is deprecated. Use AddSubEmitter, RemoveSubEmitter, SetSubEmitterSystem and GetSubEmitterSystem instead.", false)]
		public ParticleSystem collision0
		{
			get
			{
				ThrowNotImplemented();
				return null;
			}
			set
			{
				ThrowNotImplemented();
			}
		}

		[Obsolete("collision1 property is deprecated. Use AddSubEmitter, RemoveSubEmitter, SetSubEmitterSystem and GetSubEmitterSystem instead.", false)]
		public ParticleSystem collision1
		{
			get
			{
				ThrowNotImplemented();
				return null;
			}
			set
			{
				ThrowNotImplemented();
			}
		}

		[Obsolete("death0 property is deprecated. Use AddSubEmitter, RemoveSubEmitter, SetSubEmitterSystem and GetSubEmitterSystem instead.", false)]
		public ParticleSystem death0
		{
			get
			{
				ThrowNotImplemented();
				return null;
			}
			set
			{
				ThrowNotImplemented();
			}
		}

		[Obsolete("death1 property is deprecated. Use AddSubEmitter, RemoveSubEmitter, SetSubEmitterSystem and GetSubEmitterSystem instead.", false)]
		public ParticleSystem death1
		{
			get
			{
				ThrowNotImplemented();
				return null;
			}
			set
			{
				ThrowNotImplemented();
			}
		}

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public int subEmittersCount => get_subEmittersCount_Injected(ref this);

		private static void ThrowNotImplemented()
		{
			throw new NotImplementedException();
		}

		internal SubEmittersModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		public void AddSubEmitter(ParticleSystem subEmitter, ParticleSystemSubEmitterType type, ParticleSystemSubEmitterProperties properties, float emitProbability)
		{
			AddSubEmitter_Injected(ref this, subEmitter, type, properties, emitProbability);
		}

		public void AddSubEmitter(ParticleSystem subEmitter, ParticleSystemSubEmitterType type, ParticleSystemSubEmitterProperties properties)
		{
			AddSubEmitter(subEmitter, type, properties, 1f);
		}

		public void RemoveSubEmitter(int index)
		{
			RemoveSubEmitter_Injected(ref this, index);
		}

		public void SetSubEmitterSystem(int index, ParticleSystem subEmitter)
		{
			SetSubEmitterSystem_Injected(ref this, index, subEmitter);
		}

		public void SetSubEmitterType(int index, ParticleSystemSubEmitterType type)
		{
			SetSubEmitterType_Injected(ref this, index, type);
		}

		public void SetSubEmitterProperties(int index, ParticleSystemSubEmitterProperties properties)
		{
			SetSubEmitterProperties_Injected(ref this, index, properties);
		}

		public void SetSubEmitterEmitProbability(int index, float emitProbability)
		{
			SetSubEmitterEmitProbability_Injected(ref this, index, emitProbability);
		}

		public ParticleSystem GetSubEmitterSystem(int index)
		{
			return GetSubEmitterSystem_Injected(ref this, index);
		}

		public ParticleSystemSubEmitterType GetSubEmitterType(int index)
		{
			return GetSubEmitterType_Injected(ref this, index);
		}

		public ParticleSystemSubEmitterProperties GetSubEmitterProperties(int index)
		{
			return GetSubEmitterProperties_Injected(ref this, index);
		}

		public float GetSubEmitterEmitProbability(int index)
		{
			return GetSubEmitterEmitProbability_Injected(ref this, index);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref SubEmittersModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref SubEmittersModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_subEmittersCount_Injected(ref SubEmittersModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddSubEmitter_Injected(ref SubEmittersModule _unity_self, ParticleSystem subEmitter, ParticleSystemSubEmitterType type, ParticleSystemSubEmitterProperties properties, float emitProbability);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveSubEmitter_Injected(ref SubEmittersModule _unity_self, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSubEmitterSystem_Injected(ref SubEmittersModule _unity_self, int index, ParticleSystem subEmitter);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSubEmitterType_Injected(ref SubEmittersModule _unity_self, int index, ParticleSystemSubEmitterType type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSubEmitterProperties_Injected(ref SubEmittersModule _unity_self, int index, ParticleSystemSubEmitterProperties properties);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSubEmitterEmitProbability_Injected(ref SubEmittersModule _unity_self, int index, float emitProbability);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleSystem GetSubEmitterSystem_Injected(ref SubEmittersModule _unity_self, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleSystemSubEmitterType GetSubEmitterType_Injected(ref SubEmittersModule _unity_self, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleSystemSubEmitterProperties GetSubEmitterProperties_Injected(ref SubEmittersModule _unity_self, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetSubEmitterEmitProbability_Injected(ref SubEmittersModule _unity_self, int index);
	}

	public struct TextureSheetAnimationModule
	{
		internal ParticleSystem m_ParticleSystem;

		[Obsolete("flipU property is deprecated. Use ParticleSystemRenderer.flip.x instead.", false)]
		public float flipU
		{
			get
			{
				return m_ParticleSystem.GetComponent<ParticleSystemRenderer>().flip.x;
			}
			set
			{
				ParticleSystemRenderer component = m_ParticleSystem.GetComponent<ParticleSystemRenderer>();
				Vector3 flip = component.flip;
				flip.x = value;
				component.flip = flip;
			}
		}

		[Obsolete("flipV property is deprecated. Use ParticleSystemRenderer.flip.y instead.", false)]
		public float flipV
		{
			get
			{
				return m_ParticleSystem.GetComponent<ParticleSystemRenderer>().flip.y;
			}
			set
			{
				ParticleSystemRenderer component = m_ParticleSystem.GetComponent<ParticleSystemRenderer>();
				Vector3 flip = component.flip;
				flip.y = value;
				component.flip = flip;
			}
		}

		[Obsolete("useRandomRow property is deprecated. Use rowMode instead.", false)]
		public bool useRandomRow
		{
			get
			{
				return rowMode == ParticleSystemAnimationRowMode.Random;
			}
			set
			{
				rowMode = (value ? ParticleSystemAnimationRowMode.Random : ParticleSystemAnimationRowMode.Custom);
			}
		}

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public ParticleSystemAnimationMode mode
		{
			get
			{
				return get_mode_Injected(ref this);
			}
			set
			{
				set_mode_Injected(ref this, value);
			}
		}

		public ParticleSystemAnimationTimeMode timeMode
		{
			get
			{
				return get_timeMode_Injected(ref this);
			}
			set
			{
				set_timeMode_Injected(ref this, value);
			}
		}

		public float fps
		{
			get
			{
				return get_fps_Injected(ref this);
			}
			set
			{
				set_fps_Injected(ref this, value);
			}
		}

		public int numTilesX
		{
			get
			{
				return get_numTilesX_Injected(ref this);
			}
			set
			{
				set_numTilesX_Injected(ref this, value);
			}
		}

		public int numTilesY
		{
			get
			{
				return get_numTilesY_Injected(ref this);
			}
			set
			{
				set_numTilesY_Injected(ref this, value);
			}
		}

		public ParticleSystemAnimationType animation
		{
			get
			{
				return get_animation_Injected(ref this);
			}
			set
			{
				set_animation_Injected(ref this, value);
			}
		}

		public ParticleSystemAnimationRowMode rowMode
		{
			get
			{
				return get_rowMode_Injected(ref this);
			}
			set
			{
				set_rowMode_Injected(ref this, value);
			}
		}

		public MinMaxCurve frameOverTime
		{
			get
			{
				get_frameOverTime_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_frameOverTime_Injected(ref this, ref value);
			}
		}

		public float frameOverTimeMultiplier
		{
			get
			{
				return get_frameOverTimeMultiplier_Injected(ref this);
			}
			set
			{
				set_frameOverTimeMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve startFrame
		{
			get
			{
				get_startFrame_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_startFrame_Injected(ref this, ref value);
			}
		}

		public float startFrameMultiplier
		{
			get
			{
				return get_startFrameMultiplier_Injected(ref this);
			}
			set
			{
				set_startFrameMultiplier_Injected(ref this, value);
			}
		}

		public int cycleCount
		{
			get
			{
				return get_cycleCount_Injected(ref this);
			}
			set
			{
				set_cycleCount_Injected(ref this, value);
			}
		}

		public int rowIndex
		{
			get
			{
				return get_rowIndex_Injected(ref this);
			}
			set
			{
				set_rowIndex_Injected(ref this, value);
			}
		}

		public UVChannelFlags uvChannelMask
		{
			get
			{
				return get_uvChannelMask_Injected(ref this);
			}
			set
			{
				set_uvChannelMask_Injected(ref this, value);
			}
		}

		public int spriteCount => get_spriteCount_Injected(ref this);

		public Vector2 speedRange
		{
			get
			{
				get_speedRange_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_speedRange_Injected(ref this, ref value);
			}
		}

		internal TextureSheetAnimationModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		public void AddSprite(Sprite sprite)
		{
			AddSprite_Injected(ref this, sprite);
		}

		public void RemoveSprite(int index)
		{
			RemoveSprite_Injected(ref this, index);
		}

		public void SetSprite(int index, Sprite sprite)
		{
			SetSprite_Injected(ref this, index, sprite);
		}

		public Sprite GetSprite(int index)
		{
			return GetSprite_Injected(ref this, index);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref TextureSheetAnimationModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemAnimationMode get_mode_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_mode_Injected(ref TextureSheetAnimationModule _unity_self, ParticleSystemAnimationMode value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemAnimationTimeMode get_timeMode_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_timeMode_Injected(ref TextureSheetAnimationModule _unity_self, ParticleSystemAnimationTimeMode value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_fps_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_fps_Injected(ref TextureSheetAnimationModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_numTilesX_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_numTilesX_Injected(ref TextureSheetAnimationModule _unity_self, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_numTilesY_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_numTilesY_Injected(ref TextureSheetAnimationModule _unity_self, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemAnimationType get_animation_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_animation_Injected(ref TextureSheetAnimationModule _unity_self, ParticleSystemAnimationType value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemAnimationRowMode get_rowMode_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_rowMode_Injected(ref TextureSheetAnimationModule _unity_self, ParticleSystemAnimationRowMode value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_frameOverTime_Injected(ref TextureSheetAnimationModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_frameOverTime_Injected(ref TextureSheetAnimationModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_frameOverTimeMultiplier_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_frameOverTimeMultiplier_Injected(ref TextureSheetAnimationModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_startFrame_Injected(ref TextureSheetAnimationModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startFrame_Injected(ref TextureSheetAnimationModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_startFrameMultiplier_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_startFrameMultiplier_Injected(ref TextureSheetAnimationModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_cycleCount_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_cycleCount_Injected(ref TextureSheetAnimationModule _unity_self, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_rowIndex_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_rowIndex_Injected(ref TextureSheetAnimationModule _unity_self, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern UVChannelFlags get_uvChannelMask_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_uvChannelMask_Injected(ref TextureSheetAnimationModule _unity_self, UVChannelFlags value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_spriteCount_Injected(ref TextureSheetAnimationModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_speedRange_Injected(ref TextureSheetAnimationModule _unity_self, out Vector2 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_speedRange_Injected(ref TextureSheetAnimationModule _unity_self, ref Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddSprite_Injected(ref TextureSheetAnimationModule _unity_self, Sprite sprite);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveSprite_Injected(ref TextureSheetAnimationModule _unity_self, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSprite_Injected(ref TextureSheetAnimationModule _unity_self, int index, Sprite sprite);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Sprite GetSprite_Injected(ref TextureSheetAnimationModule _unity_self, int index);
	}

	[RequiredByNativeCode("particleSystemParticle", Optional = true)]
	public struct Particle
	{
		[Flags]
		private enum Flags
		{
			Size3D = 1,
			Rotation3D = 2,
			MeshIndex = 4
		}

		private Vector3 m_Position;

		private Vector3 m_Velocity;

		private Vector3 m_AnimatedVelocity;

		private Vector3 m_InitialVelocity;

		private Vector3 m_AxisOfRotation;

		private Vector3 m_Rotation;

		private Vector3 m_AngularVelocity;

		private Vector3 m_StartSize;

		private Color32 m_StartColor;

		private uint m_RandomSeed;

		private uint m_ParentRandomSeed;

		private float m_Lifetime;

		private float m_StartLifetime;

		private int m_MeshIndex;

		private float m_EmitAccumulator0;

		private float m_EmitAccumulator1;

		private uint m_Flags;

		[Obsolete("Please use Particle.remainingLifetime instead. (UnityUpgradable) -> UnityEngine.ParticleSystem/Particle.remainingLifetime", false)]
		public float lifetime
		{
			get
			{
				return remainingLifetime;
			}
			set
			{
				remainingLifetime = value;
			}
		}

		[Obsolete("randomValue property is deprecated. Use randomSeed instead to control random behavior of particles.", false)]
		public float randomValue
		{
			get
			{
				return BitConverter.ToSingle(BitConverter.GetBytes(m_RandomSeed), 0);
			}
			set
			{
				m_RandomSeed = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
			}
		}

		[Obsolete("size property is deprecated. Use startSize or GetCurrentSize() instead.", false)]
		public float size
		{
			get
			{
				return startSize;
			}
			set
			{
				startSize = value;
			}
		}

		[Obsolete("color property is deprecated. Use startColor or GetCurrentColor() instead.", false)]
		public Color32 color
		{
			get
			{
				return startColor;
			}
			set
			{
				startColor = value;
			}
		}

		public Vector3 position
		{
			get
			{
				return m_Position;
			}
			set
			{
				m_Position = value;
			}
		}

		public Vector3 velocity
		{
			get
			{
				return m_Velocity;
			}
			set
			{
				m_Velocity = value;
			}
		}

		public Vector3 animatedVelocity => m_AnimatedVelocity;

		public Vector3 totalVelocity => m_Velocity + m_AnimatedVelocity;

		public float remainingLifetime
		{
			get
			{
				return m_Lifetime;
			}
			set
			{
				m_Lifetime = value;
			}
		}

		public float startLifetime
		{
			get
			{
				return m_StartLifetime;
			}
			set
			{
				m_StartLifetime = value;
			}
		}

		public Color32 startColor
		{
			get
			{
				return m_StartColor;
			}
			set
			{
				m_StartColor = value;
			}
		}

		public uint randomSeed
		{
			get
			{
				return m_RandomSeed;
			}
			set
			{
				m_RandomSeed = value;
			}
		}

		public Vector3 axisOfRotation
		{
			get
			{
				return m_AxisOfRotation;
			}
			set
			{
				m_AxisOfRotation = value;
			}
		}

		public float startSize
		{
			get
			{
				return m_StartSize.x;
			}
			set
			{
				m_StartSize = new Vector3(value, value, value);
			}
		}

		public Vector3 startSize3D
		{
			get
			{
				return m_StartSize;
			}
			set
			{
				m_StartSize = value;
				m_Flags |= 1u;
			}
		}

		public float rotation
		{
			get
			{
				return m_Rotation.z * 57.29578f;
			}
			set
			{
				m_Rotation = new Vector3(0f, 0f, value * ((float)Math.PI / 180f));
			}
		}

		public Vector3 rotation3D
		{
			get
			{
				return m_Rotation * 57.29578f;
			}
			set
			{
				m_Rotation = value * ((float)Math.PI / 180f);
				m_Flags |= 2u;
			}
		}

		public float angularVelocity
		{
			get
			{
				return m_AngularVelocity.z * 57.29578f;
			}
			set
			{
				m_AngularVelocity = new Vector3(0f, 0f, value * ((float)Math.PI / 180f));
			}
		}

		public Vector3 angularVelocity3D
		{
			get
			{
				return m_AngularVelocity * 57.29578f;
			}
			set
			{
				m_AngularVelocity = value * ((float)Math.PI / 180f);
				m_Flags |= 2u;
			}
		}

		public float GetCurrentSize(ParticleSystem system)
		{
			return system.GetParticleCurrentSize(ref this);
		}

		public Vector3 GetCurrentSize3D(ParticleSystem system)
		{
			return system.GetParticleCurrentSize3D(ref this);
		}

		public Color32 GetCurrentColor(ParticleSystem system)
		{
			return system.GetParticleCurrentColor(ref this);
		}

		public void SetMeshIndex(int index)
		{
			m_MeshIndex = index;
			m_Flags |= 4u;
		}

		public int GetMeshIndex(ParticleSystem system)
		{
			return system.GetParticleMeshIndex(ref this);
		}
	}

	[NativeType(CodegenOptions.Custom, "MonoBurst", Header = "Runtime/Scripting/ScriptingCommonStructDefinitions.h")]
	public struct Burst
	{
		private float m_Time;

		private MinMaxCurve m_Count;

		private int m_RepeatCount;

		private float m_RepeatInterval;

		private float m_InvProbability;

		public float time
		{
			get
			{
				return m_Time;
			}
			set
			{
				m_Time = value;
			}
		}

		public MinMaxCurve count
		{
			get
			{
				return m_Count;
			}
			set
			{
				m_Count = value;
			}
		}

		public short minCount
		{
			get
			{
				return (short)m_Count.constantMin;
			}
			set
			{
				m_Count.constantMin = value;
			}
		}

		public short maxCount
		{
			get
			{
				return (short)m_Count.constantMax;
			}
			set
			{
				m_Count.constantMax = value;
			}
		}

		public int cycleCount
		{
			get
			{
				return m_RepeatCount + 1;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("cycleCount", "cycleCount must be at least 0: " + value);
				}
				m_RepeatCount = value - 1;
			}
		}

		public float repeatInterval
		{
			get
			{
				return m_RepeatInterval;
			}
			set
			{
				if (value <= 0f)
				{
					throw new ArgumentOutOfRangeException("repeatInterval", "repeatInterval must be greater than 0.0f: " + value);
				}
				m_RepeatInterval = value;
			}
		}

		public float probability
		{
			get
			{
				return 1f - m_InvProbability;
			}
			set
			{
				if (value < 0f || value > 1f)
				{
					throw new ArgumentOutOfRangeException("probability", "probability must be between 0.0f and 1.0f: " + value);
				}
				m_InvProbability = 1f - value;
			}
		}

		public Burst(float _time, short _count)
		{
			m_Time = _time;
			m_Count = _count;
			m_RepeatCount = 0;
			m_RepeatInterval = 0f;
			m_InvProbability = 0f;
		}

		public Burst(float _time, short _minCount, short _maxCount)
		{
			m_Time = _time;
			m_Count = new MinMaxCurve(_minCount, _maxCount);
			m_RepeatCount = 0;
			m_RepeatInterval = 0f;
			m_InvProbability = 0f;
		}

		public Burst(float _time, short _minCount, short _maxCount, int _cycleCount, float _repeatInterval)
		{
			m_Time = _time;
			m_Count = new MinMaxCurve(_minCount, _maxCount);
			m_RepeatCount = _cycleCount - 1;
			m_RepeatInterval = _repeatInterval;
			m_InvProbability = 0f;
		}

		public Burst(float _time, MinMaxCurve _count)
		{
			m_Time = _time;
			m_Count = _count;
			m_RepeatCount = 0;
			m_RepeatInterval = 0f;
			m_InvProbability = 0f;
		}

		public Burst(float _time, MinMaxCurve _count, int _cycleCount, float _repeatInterval)
		{
			m_Time = _time;
			m_Count = _count;
			m_RepeatCount = _cycleCount - 1;
			m_RepeatInterval = _repeatInterval;
			m_InvProbability = 0f;
		}
	}

	[Serializable]
	[NativeType(CodegenOptions.Custom, "MonoMinMaxCurve", Header = "Runtime/Scripting/ScriptingCommonStructDefinitions.h")]
	public struct MinMaxCurve
	{
		[SerializeField]
		private ParticleSystemCurveMode m_Mode;

		[SerializeField]
		private float m_CurveMultiplier;

		[SerializeField]
		private AnimationCurve m_CurveMin;

		[SerializeField]
		private AnimationCurve m_CurveMax;

		[SerializeField]
		private float m_ConstantMin;

		[SerializeField]
		private float m_ConstantMax;

		public ParticleSystemCurveMode mode
		{
			get
			{
				return m_Mode;
			}
			set
			{
				m_Mode = value;
			}
		}

		public float curveMultiplier
		{
			get
			{
				return m_CurveMultiplier;
			}
			set
			{
				m_CurveMultiplier = value;
			}
		}

		public AnimationCurve curveMax
		{
			get
			{
				return m_CurveMax;
			}
			set
			{
				m_CurveMax = value;
			}
		}

		public AnimationCurve curveMin
		{
			get
			{
				return m_CurveMin;
			}
			set
			{
				m_CurveMin = value;
			}
		}

		public float constantMax
		{
			get
			{
				return m_ConstantMax;
			}
			set
			{
				m_ConstantMax = value;
			}
		}

		public float constantMin
		{
			get
			{
				return m_ConstantMin;
			}
			set
			{
				m_ConstantMin = value;
			}
		}

		public float constant
		{
			get
			{
				return m_ConstantMax;
			}
			set
			{
				m_ConstantMax = value;
			}
		}

		public AnimationCurve curve
		{
			get
			{
				return m_CurveMax;
			}
			set
			{
				m_CurveMax = value;
			}
		}

		public MinMaxCurve(float constant)
		{
			m_Mode = ParticleSystemCurveMode.Constant;
			m_CurveMultiplier = 0f;
			m_CurveMin = null;
			m_CurveMax = null;
			m_ConstantMin = 0f;
			m_ConstantMax = constant;
		}

		public MinMaxCurve(float multiplier, AnimationCurve curve)
		{
			m_Mode = ParticleSystemCurveMode.Curve;
			m_CurveMultiplier = multiplier;
			m_CurveMin = null;
			m_CurveMax = curve;
			m_ConstantMin = 0f;
			m_ConstantMax = 0f;
		}

		public MinMaxCurve(float multiplier, AnimationCurve min, AnimationCurve max)
		{
			m_Mode = ParticleSystemCurveMode.TwoCurves;
			m_CurveMultiplier = multiplier;
			m_CurveMin = min;
			m_CurveMax = max;
			m_ConstantMin = 0f;
			m_ConstantMax = 0f;
		}

		public MinMaxCurve(float min, float max)
		{
			m_Mode = ParticleSystemCurveMode.TwoConstants;
			m_CurveMultiplier = 0f;
			m_CurveMin = null;
			m_CurveMax = null;
			m_ConstantMin = min;
			m_ConstantMax = max;
		}

		public float Evaluate(float time)
		{
			return Evaluate(time, 1f);
		}

		public float Evaluate(float time, float lerpFactor)
		{
			return mode switch
			{
				ParticleSystemCurveMode.Constant => m_ConstantMax, 
				ParticleSystemCurveMode.TwoCurves => Mathf.Lerp(m_CurveMin.Evaluate(time), m_CurveMax.Evaluate(time), lerpFactor) * m_CurveMultiplier, 
				ParticleSystemCurveMode.TwoConstants => Mathf.Lerp(m_ConstantMin, m_ConstantMax, lerpFactor), 
				_ => m_CurveMax.Evaluate(time) * m_CurveMultiplier, 
			};
		}

		public static implicit operator MinMaxCurve(float constant)
		{
			return new MinMaxCurve(constant);
		}
	}

	[Serializable]
	[NativeType(CodegenOptions.Custom, "MonoMinMaxGradient", Header = "Runtime/Scripting/ScriptingCommonStructDefinitions.h")]
	public struct MinMaxGradient
	{
		[SerializeField]
		private ParticleSystemGradientMode m_Mode;

		[SerializeField]
		private Gradient m_GradientMin;

		[SerializeField]
		private Gradient m_GradientMax;

		[SerializeField]
		private Color m_ColorMin;

		[SerializeField]
		private Color m_ColorMax;

		public ParticleSystemGradientMode mode
		{
			get
			{
				return m_Mode;
			}
			set
			{
				m_Mode = value;
			}
		}

		public Gradient gradientMax
		{
			get
			{
				return m_GradientMax;
			}
			set
			{
				m_GradientMax = value;
			}
		}

		public Gradient gradientMin
		{
			get
			{
				return m_GradientMin;
			}
			set
			{
				m_GradientMin = value;
			}
		}

		public Color colorMax
		{
			get
			{
				return m_ColorMax;
			}
			set
			{
				m_ColorMax = value;
			}
		}

		public Color colorMin
		{
			get
			{
				return m_ColorMin;
			}
			set
			{
				m_ColorMin = value;
			}
		}

		public Color color
		{
			get
			{
				return m_ColorMax;
			}
			set
			{
				m_ColorMax = value;
			}
		}

		public Gradient gradient
		{
			get
			{
				return m_GradientMax;
			}
			set
			{
				m_GradientMax = value;
			}
		}

		public MinMaxGradient(Color color)
		{
			m_Mode = ParticleSystemGradientMode.Color;
			m_GradientMin = null;
			m_GradientMax = null;
			m_ColorMin = Color.black;
			m_ColorMax = color;
		}

		public MinMaxGradient(Gradient gradient)
		{
			m_Mode = ParticleSystemGradientMode.Gradient;
			m_GradientMin = null;
			m_GradientMax = gradient;
			m_ColorMin = Color.black;
			m_ColorMax = Color.black;
		}

		public MinMaxGradient(Color min, Color max)
		{
			m_Mode = ParticleSystemGradientMode.TwoColors;
			m_GradientMin = null;
			m_GradientMax = null;
			m_ColorMin = min;
			m_ColorMax = max;
		}

		public MinMaxGradient(Gradient min, Gradient max)
		{
			m_Mode = ParticleSystemGradientMode.TwoGradients;
			m_GradientMin = min;
			m_GradientMax = max;
			m_ColorMin = Color.black;
			m_ColorMax = Color.black;
		}

		public Color Evaluate(float time)
		{
			return Evaluate(time, 1f);
		}

		public Color Evaluate(float time, float lerpFactor)
		{
			return m_Mode switch
			{
				ParticleSystemGradientMode.Color => m_ColorMax, 
				ParticleSystemGradientMode.TwoColors => Color.Lerp(m_ColorMin, m_ColorMax, lerpFactor), 
				ParticleSystemGradientMode.TwoGradients => Color.Lerp(m_GradientMin.Evaluate(time), m_GradientMax.Evaluate(time), lerpFactor), 
				ParticleSystemGradientMode.RandomColor => m_GradientMax.Evaluate(lerpFactor), 
				_ => m_GradientMax.Evaluate(time), 
			};
		}

		public static implicit operator MinMaxGradient(Color color)
		{
			return new MinMaxGradient(color);
		}

		public static implicit operator MinMaxGradient(Gradient gradient)
		{
			return new MinMaxGradient(gradient);
		}
	}

	public struct EmitParams
	{
		[NativeName("particle")]
		private Particle m_Particle;

		[NativeName("positionSet")]
		private bool m_PositionSet;

		[NativeName("velocitySet")]
		private bool m_VelocitySet;

		[NativeName("axisOfRotationSet")]
		private bool m_AxisOfRotationSet;

		[NativeName("rotationSet")]
		private bool m_RotationSet;

		[NativeName("rotationalSpeedSet")]
		private bool m_AngularVelocitySet;

		[NativeName("startSizeSet")]
		private bool m_StartSizeSet;

		[NativeName("startColorSet")]
		private bool m_StartColorSet;

		[NativeName("randomSeedSet")]
		private bool m_RandomSeedSet;

		[NativeName("startLifetimeSet")]
		private bool m_StartLifetimeSet;

		[NativeName("meshIndexSet")]
		private bool m_MeshIndexSet;

		[NativeName("applyShapeToPosition")]
		private bool m_ApplyShapeToPosition;

		public Particle particle
		{
			get
			{
				return m_Particle;
			}
			set
			{
				m_Particle = value;
				m_PositionSet = true;
				m_VelocitySet = true;
				m_AxisOfRotationSet = true;
				m_RotationSet = true;
				m_AngularVelocitySet = true;
				m_StartSizeSet = true;
				m_StartColorSet = true;
				m_RandomSeedSet = true;
				m_StartLifetimeSet = true;
				m_MeshIndexSet = true;
			}
		}

		public Vector3 position
		{
			get
			{
				return m_Particle.position;
			}
			set
			{
				m_Particle.position = value;
				m_PositionSet = true;
			}
		}

		public bool applyShapeToPosition
		{
			get
			{
				return m_ApplyShapeToPosition;
			}
			set
			{
				m_ApplyShapeToPosition = value;
			}
		}

		public Vector3 velocity
		{
			get
			{
				return m_Particle.velocity;
			}
			set
			{
				m_Particle.velocity = value;
				m_VelocitySet = true;
			}
		}

		public float startLifetime
		{
			get
			{
				return m_Particle.startLifetime;
			}
			set
			{
				m_Particle.startLifetime = value;
				m_StartLifetimeSet = true;
			}
		}

		public float startSize
		{
			get
			{
				return m_Particle.startSize;
			}
			set
			{
				m_Particle.startSize = value;
				m_StartSizeSet = true;
			}
		}

		public Vector3 startSize3D
		{
			get
			{
				return m_Particle.startSize3D;
			}
			set
			{
				m_Particle.startSize3D = value;
				m_StartSizeSet = true;
			}
		}

		public Vector3 axisOfRotation
		{
			get
			{
				return m_Particle.axisOfRotation;
			}
			set
			{
				m_Particle.axisOfRotation = value;
				m_AxisOfRotationSet = true;
			}
		}

		public float rotation
		{
			get
			{
				return m_Particle.rotation;
			}
			set
			{
				m_Particle.rotation = value;
				m_RotationSet = true;
			}
		}

		public Vector3 rotation3D
		{
			get
			{
				return m_Particle.rotation3D;
			}
			set
			{
				m_Particle.rotation3D = value;
				m_RotationSet = true;
			}
		}

		public float angularVelocity
		{
			get
			{
				return m_Particle.angularVelocity;
			}
			set
			{
				m_Particle.angularVelocity = value;
				m_AngularVelocitySet = true;
			}
		}

		public Vector3 angularVelocity3D
		{
			get
			{
				return m_Particle.angularVelocity3D;
			}
			set
			{
				m_Particle.angularVelocity3D = value;
				m_AngularVelocitySet = true;
			}
		}

		public Color32 startColor
		{
			get
			{
				return m_Particle.startColor;
			}
			set
			{
				m_Particle.startColor = value;
				m_StartColorSet = true;
			}
		}

		public uint randomSeed
		{
			get
			{
				return m_Particle.randomSeed;
			}
			set
			{
				m_Particle.randomSeed = value;
				m_RandomSeedSet = true;
			}
		}

		public int meshIndex
		{
			set
			{
				m_Particle.SetMeshIndex(value);
				m_MeshIndexSet = true;
			}
		}

		public void ResetPosition()
		{
			m_PositionSet = false;
		}

		public void ResetVelocity()
		{
			m_VelocitySet = false;
		}

		public void ResetAxisOfRotation()
		{
			m_AxisOfRotationSet = false;
		}

		public void ResetRotation()
		{
			m_RotationSet = false;
		}

		public void ResetAngularVelocity()
		{
			m_AngularVelocitySet = false;
		}

		public void ResetStartSize()
		{
			m_StartSizeSet = false;
		}

		public void ResetStartColor()
		{
			m_StartColorSet = false;
		}

		public void ResetRandomSeed()
		{
			m_RandomSeedSet = false;
		}

		public void ResetStartLifetime()
		{
			m_StartLifetimeSet = false;
		}

		public void ResetMeshIndex()
		{
			m_MeshIndexSet = false;
		}
	}

	public struct PlaybackState
	{
		internal struct Seed
		{
			public uint x;

			public uint y;

			public uint z;

			public uint w;
		}

		internal struct Seed4
		{
			public Seed x;

			public Seed y;

			public Seed z;

			public Seed w;
		}

		internal struct Emission
		{
			public float m_ParticleSpacing;

			public float m_ToEmitAccumulator;

			public Seed m_Random;
		}

		internal struct Initial
		{
			public Seed4 m_Random;
		}

		internal struct Shape
		{
			public Seed4 m_Random;

			public float m_RadiusTimer;

			public float m_RadiusTimerPrev;

			public float m_ArcTimer;

			public float m_ArcTimerPrev;

			public float m_MeshSpawnTimer;

			public float m_MeshSpawnTimerPrev;

			public int m_OrderedMeshVertexIndex;
		}

		internal struct Force
		{
			public Seed4 m_Random;
		}

		internal struct Collision
		{
			public Seed4 m_Random;
		}

		internal struct Noise
		{
			public float m_ScrollOffset;
		}

		internal struct Lights
		{
			public Seed m_Random;

			public float m_ParticleEmissionCounter;
		}

		internal struct Trail
		{
			public float m_Timer;
		}

		internal float m_AccumulatedDt;

		internal float m_StartDelay;

		internal float m_PlaybackTime;

		internal int m_RingBufferIndex;

		internal Emission m_Emission;

		internal Initial m_Initial;

		internal Shape m_Shape;

		internal Force m_Force;

		internal Collision m_Collision;

		internal Noise m_Noise;

		internal Lights m_Lights;

		internal Trail m_Trail;
	}

	[NativeType(CodegenOptions.Custom, "MonoParticleTrails")]
	public struct Trails
	{
		internal List<Vector4> positions;

		internal List<int> frontPositions;

		internal List<int> backPositions;

		internal List<int> positionCounts;

		internal int maxTrailCount;

		internal int maxPositionsPerTrailCount;
	}

	public struct VelocityOverLifetimeModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public MinMaxCurve x
		{
			get
			{
				get_x_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_x_Injected(ref this, ref value);
			}
		}

		public MinMaxCurve y
		{
			get
			{
				get_y_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_y_Injected(ref this, ref value);
			}
		}

		public MinMaxCurve z
		{
			get
			{
				get_z_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_z_Injected(ref this, ref value);
			}
		}

		public float xMultiplier
		{
			get
			{
				return get_xMultiplier_Injected(ref this);
			}
			set
			{
				set_xMultiplier_Injected(ref this, value);
			}
		}

		public float yMultiplier
		{
			get
			{
				return get_yMultiplier_Injected(ref this);
			}
			set
			{
				set_yMultiplier_Injected(ref this, value);
			}
		}

		public float zMultiplier
		{
			get
			{
				return get_zMultiplier_Injected(ref this);
			}
			set
			{
				set_zMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve orbitalX
		{
			get
			{
				get_orbitalX_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_orbitalX_Injected(ref this, ref value);
			}
		}

		public MinMaxCurve orbitalY
		{
			get
			{
				get_orbitalY_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_orbitalY_Injected(ref this, ref value);
			}
		}

		public MinMaxCurve orbitalZ
		{
			get
			{
				get_orbitalZ_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_orbitalZ_Injected(ref this, ref value);
			}
		}

		public float orbitalXMultiplier
		{
			get
			{
				return get_orbitalXMultiplier_Injected(ref this);
			}
			set
			{
				set_orbitalXMultiplier_Injected(ref this, value);
			}
		}

		public float orbitalYMultiplier
		{
			get
			{
				return get_orbitalYMultiplier_Injected(ref this);
			}
			set
			{
				set_orbitalYMultiplier_Injected(ref this, value);
			}
		}

		public float orbitalZMultiplier
		{
			get
			{
				return get_orbitalZMultiplier_Injected(ref this);
			}
			set
			{
				set_orbitalZMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve orbitalOffsetX
		{
			get
			{
				get_orbitalOffsetX_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_orbitalOffsetX_Injected(ref this, ref value);
			}
		}

		public MinMaxCurve orbitalOffsetY
		{
			get
			{
				get_orbitalOffsetY_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_orbitalOffsetY_Injected(ref this, ref value);
			}
		}

		public MinMaxCurve orbitalOffsetZ
		{
			get
			{
				get_orbitalOffsetZ_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_orbitalOffsetZ_Injected(ref this, ref value);
			}
		}

		public float orbitalOffsetXMultiplier
		{
			get
			{
				return get_orbitalOffsetXMultiplier_Injected(ref this);
			}
			set
			{
				set_orbitalOffsetXMultiplier_Injected(ref this, value);
			}
		}

		public float orbitalOffsetYMultiplier
		{
			get
			{
				return get_orbitalOffsetYMultiplier_Injected(ref this);
			}
			set
			{
				set_orbitalOffsetYMultiplier_Injected(ref this, value);
			}
		}

		public float orbitalOffsetZMultiplier
		{
			get
			{
				return get_orbitalOffsetZMultiplier_Injected(ref this);
			}
			set
			{
				set_orbitalOffsetZMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve radial
		{
			get
			{
				get_radial_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_radial_Injected(ref this, ref value);
			}
		}

		public float radialMultiplier
		{
			get
			{
				return get_radialMultiplier_Injected(ref this);
			}
			set
			{
				set_radialMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve speedModifier
		{
			get
			{
				get_speedModifier_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_speedModifier_Injected(ref this, ref value);
			}
		}

		public float speedModifierMultiplier
		{
			get
			{
				return get_speedModifierMultiplier_Injected(ref this);
			}
			set
			{
				set_speedModifierMultiplier_Injected(ref this, value);
			}
		}

		public ParticleSystemSimulationSpace space
		{
			get
			{
				return get_space_Injected(ref this);
			}
			set
			{
				set_space_Injected(ref this, value);
			}
		}

		internal VelocityOverLifetimeModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref VelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref VelocityOverLifetimeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_x_Injected(ref VelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_x_Injected(ref VelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_y_Injected(ref VelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_y_Injected(ref VelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_z_Injected(ref VelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_z_Injected(ref VelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_xMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_xMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_yMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_yMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_zMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_zMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_orbitalX_Injected(ref VelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_orbitalX_Injected(ref VelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_orbitalY_Injected(ref VelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_orbitalY_Injected(ref VelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_orbitalZ_Injected(ref VelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_orbitalZ_Injected(ref VelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_orbitalXMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_orbitalXMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_orbitalYMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_orbitalYMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_orbitalZMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_orbitalZMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_orbitalOffsetX_Injected(ref VelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_orbitalOffsetX_Injected(ref VelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_orbitalOffsetY_Injected(ref VelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_orbitalOffsetY_Injected(ref VelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_orbitalOffsetZ_Injected(ref VelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_orbitalOffsetZ_Injected(ref VelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_orbitalOffsetXMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_orbitalOffsetXMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_orbitalOffsetYMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_orbitalOffsetYMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_orbitalOffsetZMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_orbitalOffsetZMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_radial_Injected(ref VelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_radial_Injected(ref VelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_radialMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_radialMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_speedModifier_Injected(ref VelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_speedModifier_Injected(ref VelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_speedModifierMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_speedModifierMultiplier_Injected(ref VelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemSimulationSpace get_space_Injected(ref VelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_space_Injected(ref VelocityOverLifetimeModule _unity_self, ParticleSystemSimulationSpace value);
	}

	public struct LimitVelocityOverLifetimeModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public MinMaxCurve limitX
		{
			get
			{
				get_limitX_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_limitX_Injected(ref this, ref value);
			}
		}

		public float limitXMultiplier
		{
			get
			{
				return get_limitXMultiplier_Injected(ref this);
			}
			set
			{
				set_limitXMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve limitY
		{
			get
			{
				get_limitY_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_limitY_Injected(ref this, ref value);
			}
		}

		public float limitYMultiplier
		{
			get
			{
				return get_limitYMultiplier_Injected(ref this);
			}
			set
			{
				set_limitYMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve limitZ
		{
			get
			{
				get_limitZ_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_limitZ_Injected(ref this, ref value);
			}
		}

		public float limitZMultiplier
		{
			get
			{
				return get_limitZMultiplier_Injected(ref this);
			}
			set
			{
				set_limitZMultiplier_Injected(ref this, value);
			}
		}

		[NativeName("Magnitude")]
		public MinMaxCurve limit
		{
			get
			{
				get_limit_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_limit_Injected(ref this, ref value);
			}
		}

		[NativeName("MagnitudeMultiplier")]
		public float limitMultiplier
		{
			get
			{
				return get_limitMultiplier_Injected(ref this);
			}
			set
			{
				set_limitMultiplier_Injected(ref this, value);
			}
		}

		public float dampen
		{
			get
			{
				return get_dampen_Injected(ref this);
			}
			set
			{
				set_dampen_Injected(ref this, value);
			}
		}

		public bool separateAxes
		{
			get
			{
				return get_separateAxes_Injected(ref this);
			}
			set
			{
				set_separateAxes_Injected(ref this, value);
			}
		}

		public ParticleSystemSimulationSpace space
		{
			get
			{
				return get_space_Injected(ref this);
			}
			set
			{
				set_space_Injected(ref this, value);
			}
		}

		public MinMaxCurve drag
		{
			get
			{
				get_drag_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_drag_Injected(ref this, ref value);
			}
		}

		public float dragMultiplier
		{
			get
			{
				return get_dragMultiplier_Injected(ref this);
			}
			set
			{
				set_dragMultiplier_Injected(ref this, value);
			}
		}

		public bool multiplyDragByParticleSize
		{
			get
			{
				return get_multiplyDragByParticleSize_Injected(ref this);
			}
			set
			{
				set_multiplyDragByParticleSize_Injected(ref this, value);
			}
		}

		public bool multiplyDragByParticleVelocity
		{
			get
			{
				return get_multiplyDragByParticleVelocity_Injected(ref this);
			}
			set
			{
				set_multiplyDragByParticleVelocity_Injected(ref this, value);
			}
		}

		internal LimitVelocityOverLifetimeModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref LimitVelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref LimitVelocityOverLifetimeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_limitX_Injected(ref LimitVelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_limitX_Injected(ref LimitVelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_limitXMultiplier_Injected(ref LimitVelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_limitXMultiplier_Injected(ref LimitVelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_limitY_Injected(ref LimitVelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_limitY_Injected(ref LimitVelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_limitYMultiplier_Injected(ref LimitVelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_limitYMultiplier_Injected(ref LimitVelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_limitZ_Injected(ref LimitVelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_limitZ_Injected(ref LimitVelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_limitZMultiplier_Injected(ref LimitVelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_limitZMultiplier_Injected(ref LimitVelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_limit_Injected(ref LimitVelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_limit_Injected(ref LimitVelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_limitMultiplier_Injected(ref LimitVelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_limitMultiplier_Injected(ref LimitVelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_dampen_Injected(ref LimitVelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_dampen_Injected(ref LimitVelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_separateAxes_Injected(ref LimitVelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_separateAxes_Injected(ref LimitVelocityOverLifetimeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemSimulationSpace get_space_Injected(ref LimitVelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_space_Injected(ref LimitVelocityOverLifetimeModule _unity_self, ParticleSystemSimulationSpace value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_drag_Injected(ref LimitVelocityOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_drag_Injected(ref LimitVelocityOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_dragMultiplier_Injected(ref LimitVelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_dragMultiplier_Injected(ref LimitVelocityOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_multiplyDragByParticleSize_Injected(ref LimitVelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_multiplyDragByParticleSize_Injected(ref LimitVelocityOverLifetimeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_multiplyDragByParticleVelocity_Injected(ref LimitVelocityOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_multiplyDragByParticleVelocity_Injected(ref LimitVelocityOverLifetimeModule _unity_self, bool value);
	}

	public struct InheritVelocityModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public ParticleSystemInheritVelocityMode mode
		{
			get
			{
				return get_mode_Injected(ref this);
			}
			set
			{
				set_mode_Injected(ref this, value);
			}
		}

		public MinMaxCurve curve
		{
			get
			{
				get_curve_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_curve_Injected(ref this, ref value);
			}
		}

		public float curveMultiplier
		{
			get
			{
				return get_curveMultiplier_Injected(ref this);
			}
			set
			{
				set_curveMultiplier_Injected(ref this, value);
			}
		}

		internal InheritVelocityModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref InheritVelocityModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref InheritVelocityModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemInheritVelocityMode get_mode_Injected(ref InheritVelocityModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_mode_Injected(ref InheritVelocityModule _unity_self, ParticleSystemInheritVelocityMode value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_curve_Injected(ref InheritVelocityModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_curve_Injected(ref InheritVelocityModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_curveMultiplier_Injected(ref InheritVelocityModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_curveMultiplier_Injected(ref InheritVelocityModule _unity_self, float value);
	}

	public struct ForceOverLifetimeModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public MinMaxCurve x
		{
			get
			{
				get_x_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_x_Injected(ref this, ref value);
			}
		}

		public MinMaxCurve y
		{
			get
			{
				get_y_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_y_Injected(ref this, ref value);
			}
		}

		public MinMaxCurve z
		{
			get
			{
				get_z_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_z_Injected(ref this, ref value);
			}
		}

		public float xMultiplier
		{
			get
			{
				return get_xMultiplier_Injected(ref this);
			}
			set
			{
				set_xMultiplier_Injected(ref this, value);
			}
		}

		public float yMultiplier
		{
			get
			{
				return get_yMultiplier_Injected(ref this);
			}
			set
			{
				set_yMultiplier_Injected(ref this, value);
			}
		}

		public float zMultiplier
		{
			get
			{
				return get_zMultiplier_Injected(ref this);
			}
			set
			{
				set_zMultiplier_Injected(ref this, value);
			}
		}

		public ParticleSystemSimulationSpace space
		{
			get
			{
				return get_space_Injected(ref this);
			}
			set
			{
				set_space_Injected(ref this, value);
			}
		}

		public bool randomized
		{
			get
			{
				return get_randomized_Injected(ref this);
			}
			set
			{
				set_randomized_Injected(ref this, value);
			}
		}

		internal ForceOverLifetimeModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref ForceOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref ForceOverLifetimeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_x_Injected(ref ForceOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_x_Injected(ref ForceOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_y_Injected(ref ForceOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_y_Injected(ref ForceOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_z_Injected(ref ForceOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_z_Injected(ref ForceOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_xMultiplier_Injected(ref ForceOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_xMultiplier_Injected(ref ForceOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_yMultiplier_Injected(ref ForceOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_yMultiplier_Injected(ref ForceOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_zMultiplier_Injected(ref ForceOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_zMultiplier_Injected(ref ForceOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemSimulationSpace get_space_Injected(ref ForceOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_space_Injected(ref ForceOverLifetimeModule _unity_self, ParticleSystemSimulationSpace value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_randomized_Injected(ref ForceOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_randomized_Injected(ref ForceOverLifetimeModule _unity_self, bool value);
	}

	public struct ColorOverLifetimeModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public MinMaxGradient color
		{
			get
			{
				get_color_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_color_Injected(ref this, ref value);
			}
		}

		internal ColorOverLifetimeModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref ColorOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref ColorOverLifetimeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_color_Injected(ref ColorOverLifetimeModule _unity_self, out MinMaxGradient ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_color_Injected(ref ColorOverLifetimeModule _unity_self, ref MinMaxGradient value);
	}

	public struct ColorBySpeedModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public MinMaxGradient color
		{
			get
			{
				get_color_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_color_Injected(ref this, ref value);
			}
		}

		public Vector2 range
		{
			get
			{
				get_range_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_range_Injected(ref this, ref value);
			}
		}

		internal ColorBySpeedModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref ColorBySpeedModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref ColorBySpeedModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_color_Injected(ref ColorBySpeedModule _unity_self, out MinMaxGradient ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_color_Injected(ref ColorBySpeedModule _unity_self, ref MinMaxGradient value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_range_Injected(ref ColorBySpeedModule _unity_self, out Vector2 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_range_Injected(ref ColorBySpeedModule _unity_self, ref Vector2 value);
	}

	public struct SizeOverLifetimeModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		[NativeName("X")]
		public MinMaxCurve size
		{
			get
			{
				get_size_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_size_Injected(ref this, ref value);
			}
		}

		[NativeName("XMultiplier")]
		public float sizeMultiplier
		{
			get
			{
				return get_sizeMultiplier_Injected(ref this);
			}
			set
			{
				set_sizeMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve x
		{
			get
			{
				get_x_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_x_Injected(ref this, ref value);
			}
		}

		public float xMultiplier
		{
			get
			{
				return get_xMultiplier_Injected(ref this);
			}
			set
			{
				set_xMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve y
		{
			get
			{
				get_y_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_y_Injected(ref this, ref value);
			}
		}

		public float yMultiplier
		{
			get
			{
				return get_yMultiplier_Injected(ref this);
			}
			set
			{
				set_yMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve z
		{
			get
			{
				get_z_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_z_Injected(ref this, ref value);
			}
		}

		public float zMultiplier
		{
			get
			{
				return get_zMultiplier_Injected(ref this);
			}
			set
			{
				set_zMultiplier_Injected(ref this, value);
			}
		}

		public bool separateAxes
		{
			get
			{
				return get_separateAxes_Injected(ref this);
			}
			set
			{
				set_separateAxes_Injected(ref this, value);
			}
		}

		internal SizeOverLifetimeModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref SizeOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref SizeOverLifetimeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_size_Injected(ref SizeOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_size_Injected(ref SizeOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_sizeMultiplier_Injected(ref SizeOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_sizeMultiplier_Injected(ref SizeOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_x_Injected(ref SizeOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_x_Injected(ref SizeOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_xMultiplier_Injected(ref SizeOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_xMultiplier_Injected(ref SizeOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_y_Injected(ref SizeOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_y_Injected(ref SizeOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_yMultiplier_Injected(ref SizeOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_yMultiplier_Injected(ref SizeOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_z_Injected(ref SizeOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_z_Injected(ref SizeOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_zMultiplier_Injected(ref SizeOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_zMultiplier_Injected(ref SizeOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_separateAxes_Injected(ref SizeOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_separateAxes_Injected(ref SizeOverLifetimeModule _unity_self, bool value);
	}

	public struct SizeBySpeedModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		[NativeName("X")]
		public MinMaxCurve size
		{
			get
			{
				get_size_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_size_Injected(ref this, ref value);
			}
		}

		[NativeName("XMultiplier")]
		public float sizeMultiplier
		{
			get
			{
				return get_sizeMultiplier_Injected(ref this);
			}
			set
			{
				set_sizeMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve x
		{
			get
			{
				get_x_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_x_Injected(ref this, ref value);
			}
		}

		public float xMultiplier
		{
			get
			{
				return get_xMultiplier_Injected(ref this);
			}
			set
			{
				set_xMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve y
		{
			get
			{
				get_y_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_y_Injected(ref this, ref value);
			}
		}

		public float yMultiplier
		{
			get
			{
				return get_yMultiplier_Injected(ref this);
			}
			set
			{
				set_yMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve z
		{
			get
			{
				get_z_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_z_Injected(ref this, ref value);
			}
		}

		public float zMultiplier
		{
			get
			{
				return get_zMultiplier_Injected(ref this);
			}
			set
			{
				set_zMultiplier_Injected(ref this, value);
			}
		}

		public bool separateAxes
		{
			get
			{
				return get_separateAxes_Injected(ref this);
			}
			set
			{
				set_separateAxes_Injected(ref this, value);
			}
		}

		public Vector2 range
		{
			get
			{
				get_range_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_range_Injected(ref this, ref value);
			}
		}

		internal SizeBySpeedModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref SizeBySpeedModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref SizeBySpeedModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_size_Injected(ref SizeBySpeedModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_size_Injected(ref SizeBySpeedModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_sizeMultiplier_Injected(ref SizeBySpeedModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_sizeMultiplier_Injected(ref SizeBySpeedModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_x_Injected(ref SizeBySpeedModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_x_Injected(ref SizeBySpeedModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_xMultiplier_Injected(ref SizeBySpeedModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_xMultiplier_Injected(ref SizeBySpeedModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_y_Injected(ref SizeBySpeedModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_y_Injected(ref SizeBySpeedModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_yMultiplier_Injected(ref SizeBySpeedModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_yMultiplier_Injected(ref SizeBySpeedModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_z_Injected(ref SizeBySpeedModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_z_Injected(ref SizeBySpeedModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_zMultiplier_Injected(ref SizeBySpeedModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_zMultiplier_Injected(ref SizeBySpeedModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_separateAxes_Injected(ref SizeBySpeedModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_separateAxes_Injected(ref SizeBySpeedModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_range_Injected(ref SizeBySpeedModule _unity_self, out Vector2 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_range_Injected(ref SizeBySpeedModule _unity_self, ref Vector2 value);
	}

	public struct RotationOverLifetimeModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public MinMaxCurve x
		{
			get
			{
				get_x_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_x_Injected(ref this, ref value);
			}
		}

		public float xMultiplier
		{
			get
			{
				return get_xMultiplier_Injected(ref this);
			}
			set
			{
				set_xMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve y
		{
			get
			{
				get_y_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_y_Injected(ref this, ref value);
			}
		}

		public float yMultiplier
		{
			get
			{
				return get_yMultiplier_Injected(ref this);
			}
			set
			{
				set_yMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve z
		{
			get
			{
				get_z_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_z_Injected(ref this, ref value);
			}
		}

		public float zMultiplier
		{
			get
			{
				return get_zMultiplier_Injected(ref this);
			}
			set
			{
				set_zMultiplier_Injected(ref this, value);
			}
		}

		public bool separateAxes
		{
			get
			{
				return get_separateAxes_Injected(ref this);
			}
			set
			{
				set_separateAxes_Injected(ref this, value);
			}
		}

		internal RotationOverLifetimeModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref RotationOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref RotationOverLifetimeModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_x_Injected(ref RotationOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_x_Injected(ref RotationOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_xMultiplier_Injected(ref RotationOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_xMultiplier_Injected(ref RotationOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_y_Injected(ref RotationOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_y_Injected(ref RotationOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_yMultiplier_Injected(ref RotationOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_yMultiplier_Injected(ref RotationOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_z_Injected(ref RotationOverLifetimeModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_z_Injected(ref RotationOverLifetimeModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_zMultiplier_Injected(ref RotationOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_zMultiplier_Injected(ref RotationOverLifetimeModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_separateAxes_Injected(ref RotationOverLifetimeModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_separateAxes_Injected(ref RotationOverLifetimeModule _unity_self, bool value);
	}

	public struct RotationBySpeedModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public MinMaxCurve x
		{
			get
			{
				get_x_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_x_Injected(ref this, ref value);
			}
		}

		public float xMultiplier
		{
			get
			{
				return get_xMultiplier_Injected(ref this);
			}
			set
			{
				set_xMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve y
		{
			get
			{
				get_y_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_y_Injected(ref this, ref value);
			}
		}

		public float yMultiplier
		{
			get
			{
				return get_yMultiplier_Injected(ref this);
			}
			set
			{
				set_yMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve z
		{
			get
			{
				get_z_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_z_Injected(ref this, ref value);
			}
		}

		public float zMultiplier
		{
			get
			{
				return get_zMultiplier_Injected(ref this);
			}
			set
			{
				set_zMultiplier_Injected(ref this, value);
			}
		}

		public bool separateAxes
		{
			get
			{
				return get_separateAxes_Injected(ref this);
			}
			set
			{
				set_separateAxes_Injected(ref this, value);
			}
		}

		public Vector2 range
		{
			get
			{
				get_range_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_range_Injected(ref this, ref value);
			}
		}

		internal RotationBySpeedModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref RotationBySpeedModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref RotationBySpeedModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_x_Injected(ref RotationBySpeedModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_x_Injected(ref RotationBySpeedModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_xMultiplier_Injected(ref RotationBySpeedModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_xMultiplier_Injected(ref RotationBySpeedModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_y_Injected(ref RotationBySpeedModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_y_Injected(ref RotationBySpeedModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_yMultiplier_Injected(ref RotationBySpeedModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_yMultiplier_Injected(ref RotationBySpeedModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_z_Injected(ref RotationBySpeedModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_z_Injected(ref RotationBySpeedModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_zMultiplier_Injected(ref RotationBySpeedModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_zMultiplier_Injected(ref RotationBySpeedModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_separateAxes_Injected(ref RotationBySpeedModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_separateAxes_Injected(ref RotationBySpeedModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_range_Injected(ref RotationBySpeedModule _unity_self, out Vector2 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_range_Injected(ref RotationBySpeedModule _unity_self, ref Vector2 value);
	}

	public struct ExternalForcesModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public float multiplier
		{
			get
			{
				return get_multiplier_Injected(ref this);
			}
			set
			{
				set_multiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve multiplierCurve
		{
			get
			{
				get_multiplierCurve_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_multiplierCurve_Injected(ref this, ref value);
			}
		}

		public ParticleSystemGameObjectFilter influenceFilter
		{
			get
			{
				return get_influenceFilter_Injected(ref this);
			}
			set
			{
				set_influenceFilter_Injected(ref this, value);
			}
		}

		public LayerMask influenceMask
		{
			get
			{
				get_influenceMask_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_influenceMask_Injected(ref this, ref value);
			}
		}

		public int influenceCount => get_influenceCount_Injected(ref this);

		internal ExternalForcesModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		public bool IsAffectedBy(ParticleSystemForceField field)
		{
			return IsAffectedBy_Injected(ref this, field);
		}

		public void AddInfluence([NotNull] ParticleSystemForceField field)
		{
			AddInfluence_Injected(ref this, field);
		}

		private void RemoveInfluenceAtIndex(int index)
		{
			RemoveInfluenceAtIndex_Injected(ref this, index);
		}

		public void RemoveInfluence(int index)
		{
			RemoveInfluenceAtIndex(index);
		}

		public void RemoveInfluence([NotNull] ParticleSystemForceField field)
		{
			RemoveInfluence_Injected(ref this, field);
		}

		public void RemoveAllInfluences()
		{
			RemoveAllInfluences_Injected(ref this);
		}

		public void SetInfluence(int index, [NotNull] ParticleSystemForceField field)
		{
			SetInfluence_Injected(ref this, index, field);
		}

		public ParticleSystemForceField GetInfluence(int index)
		{
			return GetInfluence_Injected(ref this, index);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref ExternalForcesModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref ExternalForcesModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_multiplier_Injected(ref ExternalForcesModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_multiplier_Injected(ref ExternalForcesModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_multiplierCurve_Injected(ref ExternalForcesModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_multiplierCurve_Injected(ref ExternalForcesModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemGameObjectFilter get_influenceFilter_Injected(ref ExternalForcesModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_influenceFilter_Injected(ref ExternalForcesModule _unity_self, ParticleSystemGameObjectFilter value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_influenceMask_Injected(ref ExternalForcesModule _unity_self, out LayerMask ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_influenceMask_Injected(ref ExternalForcesModule _unity_self, ref LayerMask value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_influenceCount_Injected(ref ExternalForcesModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsAffectedBy_Injected(ref ExternalForcesModule _unity_self, ParticleSystemForceField field);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddInfluence_Injected(ref ExternalForcesModule _unity_self, ParticleSystemForceField field);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveInfluenceAtIndex_Injected(ref ExternalForcesModule _unity_self, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveInfluence_Injected(ref ExternalForcesModule _unity_self, ParticleSystemForceField field);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveAllInfluences_Injected(ref ExternalForcesModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetInfluence_Injected(ref ExternalForcesModule _unity_self, int index, ParticleSystemForceField field);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleSystemForceField GetInfluence_Injected(ref ExternalForcesModule _unity_self, int index);
	}

	public struct NoiseModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public bool separateAxes
		{
			get
			{
				return get_separateAxes_Injected(ref this);
			}
			set
			{
				set_separateAxes_Injected(ref this, value);
			}
		}

		[NativeName("StrengthX")]
		public MinMaxCurve strength
		{
			get
			{
				get_strength_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_strength_Injected(ref this, ref value);
			}
		}

		[NativeName("StrengthXMultiplier")]
		public float strengthMultiplier
		{
			get
			{
				return get_strengthMultiplier_Injected(ref this);
			}
			set
			{
				set_strengthMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve strengthX
		{
			get
			{
				get_strengthX_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_strengthX_Injected(ref this, ref value);
			}
		}

		public float strengthXMultiplier
		{
			get
			{
				return get_strengthXMultiplier_Injected(ref this);
			}
			set
			{
				set_strengthXMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve strengthY
		{
			get
			{
				get_strengthY_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_strengthY_Injected(ref this, ref value);
			}
		}

		public float strengthYMultiplier
		{
			get
			{
				return get_strengthYMultiplier_Injected(ref this);
			}
			set
			{
				set_strengthYMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve strengthZ
		{
			get
			{
				get_strengthZ_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_strengthZ_Injected(ref this, ref value);
			}
		}

		public float strengthZMultiplier
		{
			get
			{
				return get_strengthZMultiplier_Injected(ref this);
			}
			set
			{
				set_strengthZMultiplier_Injected(ref this, value);
			}
		}

		public float frequency
		{
			get
			{
				return get_frequency_Injected(ref this);
			}
			set
			{
				set_frequency_Injected(ref this, value);
			}
		}

		public bool damping
		{
			get
			{
				return get_damping_Injected(ref this);
			}
			set
			{
				set_damping_Injected(ref this, value);
			}
		}

		public int octaveCount
		{
			get
			{
				return get_octaveCount_Injected(ref this);
			}
			set
			{
				set_octaveCount_Injected(ref this, value);
			}
		}

		public float octaveMultiplier
		{
			get
			{
				return get_octaveMultiplier_Injected(ref this);
			}
			set
			{
				set_octaveMultiplier_Injected(ref this, value);
			}
		}

		public float octaveScale
		{
			get
			{
				return get_octaveScale_Injected(ref this);
			}
			set
			{
				set_octaveScale_Injected(ref this, value);
			}
		}

		public ParticleSystemNoiseQuality quality
		{
			get
			{
				return get_quality_Injected(ref this);
			}
			set
			{
				set_quality_Injected(ref this, value);
			}
		}

		public MinMaxCurve scrollSpeed
		{
			get
			{
				get_scrollSpeed_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_scrollSpeed_Injected(ref this, ref value);
			}
		}

		public float scrollSpeedMultiplier
		{
			get
			{
				return get_scrollSpeedMultiplier_Injected(ref this);
			}
			set
			{
				set_scrollSpeedMultiplier_Injected(ref this, value);
			}
		}

		public bool remapEnabled
		{
			get
			{
				return get_remapEnabled_Injected(ref this);
			}
			set
			{
				set_remapEnabled_Injected(ref this, value);
			}
		}

		[NativeName("RemapX")]
		public MinMaxCurve remap
		{
			get
			{
				get_remap_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_remap_Injected(ref this, ref value);
			}
		}

		[NativeName("RemapXMultiplier")]
		public float remapMultiplier
		{
			get
			{
				return get_remapMultiplier_Injected(ref this);
			}
			set
			{
				set_remapMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve remapX
		{
			get
			{
				get_remapX_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_remapX_Injected(ref this, ref value);
			}
		}

		public float remapXMultiplier
		{
			get
			{
				return get_remapXMultiplier_Injected(ref this);
			}
			set
			{
				set_remapXMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve remapY
		{
			get
			{
				get_remapY_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_remapY_Injected(ref this, ref value);
			}
		}

		public float remapYMultiplier
		{
			get
			{
				return get_remapYMultiplier_Injected(ref this);
			}
			set
			{
				set_remapYMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve remapZ
		{
			get
			{
				get_remapZ_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_remapZ_Injected(ref this, ref value);
			}
		}

		public float remapZMultiplier
		{
			get
			{
				return get_remapZMultiplier_Injected(ref this);
			}
			set
			{
				set_remapZMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve positionAmount
		{
			get
			{
				get_positionAmount_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_positionAmount_Injected(ref this, ref value);
			}
		}

		public MinMaxCurve rotationAmount
		{
			get
			{
				get_rotationAmount_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_rotationAmount_Injected(ref this, ref value);
			}
		}

		public MinMaxCurve sizeAmount
		{
			get
			{
				get_sizeAmount_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_sizeAmount_Injected(ref this, ref value);
			}
		}

		internal NoiseModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref NoiseModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_separateAxes_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_separateAxes_Injected(ref NoiseModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_strength_Injected(ref NoiseModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_strength_Injected(ref NoiseModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_strengthMultiplier_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_strengthMultiplier_Injected(ref NoiseModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_strengthX_Injected(ref NoiseModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_strengthX_Injected(ref NoiseModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_strengthXMultiplier_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_strengthXMultiplier_Injected(ref NoiseModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_strengthY_Injected(ref NoiseModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_strengthY_Injected(ref NoiseModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_strengthYMultiplier_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_strengthYMultiplier_Injected(ref NoiseModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_strengthZ_Injected(ref NoiseModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_strengthZ_Injected(ref NoiseModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_strengthZMultiplier_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_strengthZMultiplier_Injected(ref NoiseModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_frequency_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_frequency_Injected(ref NoiseModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_damping_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_damping_Injected(ref NoiseModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_octaveCount_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_octaveCount_Injected(ref NoiseModule _unity_self, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_octaveMultiplier_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_octaveMultiplier_Injected(ref NoiseModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_octaveScale_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_octaveScale_Injected(ref NoiseModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemNoiseQuality get_quality_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_quality_Injected(ref NoiseModule _unity_self, ParticleSystemNoiseQuality value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_scrollSpeed_Injected(ref NoiseModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_scrollSpeed_Injected(ref NoiseModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_scrollSpeedMultiplier_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_scrollSpeedMultiplier_Injected(ref NoiseModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_remapEnabled_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_remapEnabled_Injected(ref NoiseModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_remap_Injected(ref NoiseModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_remap_Injected(ref NoiseModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_remapMultiplier_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_remapMultiplier_Injected(ref NoiseModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_remapX_Injected(ref NoiseModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_remapX_Injected(ref NoiseModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_remapXMultiplier_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_remapXMultiplier_Injected(ref NoiseModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_remapY_Injected(ref NoiseModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_remapY_Injected(ref NoiseModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_remapYMultiplier_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_remapYMultiplier_Injected(ref NoiseModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_remapZ_Injected(ref NoiseModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_remapZ_Injected(ref NoiseModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_remapZMultiplier_Injected(ref NoiseModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_remapZMultiplier_Injected(ref NoiseModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_positionAmount_Injected(ref NoiseModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_positionAmount_Injected(ref NoiseModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_rotationAmount_Injected(ref NoiseModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_rotationAmount_Injected(ref NoiseModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_sizeAmount_Injected(ref NoiseModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_sizeAmount_Injected(ref NoiseModule _unity_self, ref MinMaxCurve value);
	}

	public struct CollisionModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public ParticleSystemCollisionType type
		{
			get
			{
				return get_type_Injected(ref this);
			}
			set
			{
				set_type_Injected(ref this, value);
			}
		}

		public ParticleSystemCollisionMode mode
		{
			get
			{
				return get_mode_Injected(ref this);
			}
			set
			{
				set_mode_Injected(ref this, value);
			}
		}

		public MinMaxCurve dampen
		{
			get
			{
				get_dampen_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_dampen_Injected(ref this, ref value);
			}
		}

		public float dampenMultiplier
		{
			get
			{
				return get_dampenMultiplier_Injected(ref this);
			}
			set
			{
				set_dampenMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve bounce
		{
			get
			{
				get_bounce_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_bounce_Injected(ref this, ref value);
			}
		}

		public float bounceMultiplier
		{
			get
			{
				return get_bounceMultiplier_Injected(ref this);
			}
			set
			{
				set_bounceMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve lifetimeLoss
		{
			get
			{
				get_lifetimeLoss_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_lifetimeLoss_Injected(ref this, ref value);
			}
		}

		public float lifetimeLossMultiplier
		{
			get
			{
				return get_lifetimeLossMultiplier_Injected(ref this);
			}
			set
			{
				set_lifetimeLossMultiplier_Injected(ref this, value);
			}
		}

		public float minKillSpeed
		{
			get
			{
				return get_minKillSpeed_Injected(ref this);
			}
			set
			{
				set_minKillSpeed_Injected(ref this, value);
			}
		}

		public float maxKillSpeed
		{
			get
			{
				return get_maxKillSpeed_Injected(ref this);
			}
			set
			{
				set_maxKillSpeed_Injected(ref this, value);
			}
		}

		public LayerMask collidesWith
		{
			get
			{
				get_collidesWith_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_collidesWith_Injected(ref this, ref value);
			}
		}

		public bool enableDynamicColliders
		{
			get
			{
				return get_enableDynamicColliders_Injected(ref this);
			}
			set
			{
				set_enableDynamicColliders_Injected(ref this, value);
			}
		}

		public int maxCollisionShapes
		{
			get
			{
				return get_maxCollisionShapes_Injected(ref this);
			}
			set
			{
				set_maxCollisionShapes_Injected(ref this, value);
			}
		}

		public ParticleSystemCollisionQuality quality
		{
			get
			{
				return get_quality_Injected(ref this);
			}
			set
			{
				set_quality_Injected(ref this, value);
			}
		}

		public float voxelSize
		{
			get
			{
				return get_voxelSize_Injected(ref this);
			}
			set
			{
				set_voxelSize_Injected(ref this, value);
			}
		}

		public float radiusScale
		{
			get
			{
				return get_radiusScale_Injected(ref this);
			}
			set
			{
				set_radiusScale_Injected(ref this, value);
			}
		}

		public bool sendCollisionMessages
		{
			get
			{
				return get_sendCollisionMessages_Injected(ref this);
			}
			set
			{
				set_sendCollisionMessages_Injected(ref this, value);
			}
		}

		public float colliderForce
		{
			get
			{
				return get_colliderForce_Injected(ref this);
			}
			set
			{
				set_colliderForce_Injected(ref this, value);
			}
		}

		public bool multiplyColliderForceByCollisionAngle
		{
			get
			{
				return get_multiplyColliderForceByCollisionAngle_Injected(ref this);
			}
			set
			{
				set_multiplyColliderForceByCollisionAngle_Injected(ref this, value);
			}
		}

		public bool multiplyColliderForceByParticleSpeed
		{
			get
			{
				return get_multiplyColliderForceByParticleSpeed_Injected(ref this);
			}
			set
			{
				set_multiplyColliderForceByParticleSpeed_Injected(ref this, value);
			}
		}

		public bool multiplyColliderForceByParticleSize
		{
			get
			{
				return get_multiplyColliderForceByParticleSize_Injected(ref this);
			}
			set
			{
				set_multiplyColliderForceByParticleSize_Injected(ref this, value);
			}
		}

		public int maxPlaneCount => get_maxPlaneCount_Injected(ref this);

		[Obsolete("enableInteriorCollisions property is deprecated and is no longer required and has no effect on the particle system.", false)]
		public bool enableInteriorCollisions
		{
			get
			{
				return get_enableInteriorCollisions_Injected(ref this);
			}
			set
			{
				set_enableInteriorCollisions_Injected(ref this, value);
			}
		}

		internal CollisionModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		public void SetPlane(int index, Transform transform)
		{
			SetPlane_Injected(ref this, index, transform);
		}

		public Transform GetPlane(int index)
		{
			return GetPlane_Injected(ref this, index);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref CollisionModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemCollisionType get_type_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_type_Injected(ref CollisionModule _unity_self, ParticleSystemCollisionType value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemCollisionMode get_mode_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_mode_Injected(ref CollisionModule _unity_self, ParticleSystemCollisionMode value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_dampen_Injected(ref CollisionModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_dampen_Injected(ref CollisionModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_dampenMultiplier_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_dampenMultiplier_Injected(ref CollisionModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_bounce_Injected(ref CollisionModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_bounce_Injected(ref CollisionModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_bounceMultiplier_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_bounceMultiplier_Injected(ref CollisionModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_lifetimeLoss_Injected(ref CollisionModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_lifetimeLoss_Injected(ref CollisionModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_lifetimeLossMultiplier_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_lifetimeLossMultiplier_Injected(ref CollisionModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_minKillSpeed_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_minKillSpeed_Injected(ref CollisionModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_maxKillSpeed_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_maxKillSpeed_Injected(ref CollisionModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_collidesWith_Injected(ref CollisionModule _unity_self, out LayerMask ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_collidesWith_Injected(ref CollisionModule _unity_self, ref LayerMask value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enableDynamicColliders_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enableDynamicColliders_Injected(ref CollisionModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_maxCollisionShapes_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_maxCollisionShapes_Injected(ref CollisionModule _unity_self, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemCollisionQuality get_quality_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_quality_Injected(ref CollisionModule _unity_self, ParticleSystemCollisionQuality value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_voxelSize_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_voxelSize_Injected(ref CollisionModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_radiusScale_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_radiusScale_Injected(ref CollisionModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_sendCollisionMessages_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_sendCollisionMessages_Injected(ref CollisionModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_colliderForce_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_colliderForce_Injected(ref CollisionModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_multiplyColliderForceByCollisionAngle_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_multiplyColliderForceByCollisionAngle_Injected(ref CollisionModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_multiplyColliderForceByParticleSpeed_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_multiplyColliderForceByParticleSpeed_Injected(ref CollisionModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_multiplyColliderForceByParticleSize_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_multiplyColliderForceByParticleSize_Injected(ref CollisionModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPlane_Injected(ref CollisionModule _unity_self, int index, Transform transform);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Transform GetPlane_Injected(ref CollisionModule _unity_self, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_maxPlaneCount_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enableInteriorCollisions_Injected(ref CollisionModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enableInteriorCollisions_Injected(ref CollisionModule _unity_self, bool value);
	}

	public struct TriggerModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public ParticleSystemOverlapAction inside
		{
			get
			{
				return get_inside_Injected(ref this);
			}
			set
			{
				set_inside_Injected(ref this, value);
			}
		}

		public ParticleSystemOverlapAction outside
		{
			get
			{
				return get_outside_Injected(ref this);
			}
			set
			{
				set_outside_Injected(ref this, value);
			}
		}

		public ParticleSystemOverlapAction enter
		{
			get
			{
				return get_enter_Injected(ref this);
			}
			set
			{
				set_enter_Injected(ref this, value);
			}
		}

		public ParticleSystemOverlapAction exit
		{
			get
			{
				return get_exit_Injected(ref this);
			}
			set
			{
				set_exit_Injected(ref this, value);
			}
		}

		public float radiusScale
		{
			get
			{
				return get_radiusScale_Injected(ref this);
			}
			set
			{
				set_radiusScale_Injected(ref this, value);
			}
		}

		public int maxColliderCount => get_maxColliderCount_Injected(ref this);

		internal TriggerModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		public void SetCollider(int index, Component collider)
		{
			SetCollider_Injected(ref this, index, collider);
		}

		public Component GetCollider(int index)
		{
			return GetCollider_Injected(ref this, index);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref TriggerModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref TriggerModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemOverlapAction get_inside_Injected(ref TriggerModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_inside_Injected(ref TriggerModule _unity_self, ParticleSystemOverlapAction value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemOverlapAction get_outside_Injected(ref TriggerModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_outside_Injected(ref TriggerModule _unity_self, ParticleSystemOverlapAction value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemOverlapAction get_enter_Injected(ref TriggerModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enter_Injected(ref TriggerModule _unity_self, ParticleSystemOverlapAction value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemOverlapAction get_exit_Injected(ref TriggerModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_exit_Injected(ref TriggerModule _unity_self, ParticleSystemOverlapAction value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_radiusScale_Injected(ref TriggerModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_radiusScale_Injected(ref TriggerModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCollider_Injected(ref TriggerModule _unity_self, int index, Component collider);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Component GetCollider_Injected(ref TriggerModule _unity_self, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_maxColliderCount_Injected(ref TriggerModule _unity_self);
	}

	public struct LightsModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public float ratio
		{
			get
			{
				return get_ratio_Injected(ref this);
			}
			set
			{
				set_ratio_Injected(ref this, value);
			}
		}

		public bool useRandomDistribution
		{
			get
			{
				return get_useRandomDistribution_Injected(ref this);
			}
			set
			{
				set_useRandomDistribution_Injected(ref this, value);
			}
		}

		public Light light
		{
			get
			{
				return get_light_Injected(ref this);
			}
			set
			{
				set_light_Injected(ref this, value);
			}
		}

		public bool useParticleColor
		{
			get
			{
				return get_useParticleColor_Injected(ref this);
			}
			set
			{
				set_useParticleColor_Injected(ref this, value);
			}
		}

		public bool sizeAffectsRange
		{
			get
			{
				return get_sizeAffectsRange_Injected(ref this);
			}
			set
			{
				set_sizeAffectsRange_Injected(ref this, value);
			}
		}

		public bool alphaAffectsIntensity
		{
			get
			{
				return get_alphaAffectsIntensity_Injected(ref this);
			}
			set
			{
				set_alphaAffectsIntensity_Injected(ref this, value);
			}
		}

		public MinMaxCurve range
		{
			get
			{
				get_range_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_range_Injected(ref this, ref value);
			}
		}

		public float rangeMultiplier
		{
			get
			{
				return get_rangeMultiplier_Injected(ref this);
			}
			set
			{
				set_rangeMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxCurve intensity
		{
			get
			{
				get_intensity_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_intensity_Injected(ref this, ref value);
			}
		}

		public float intensityMultiplier
		{
			get
			{
				return get_intensityMultiplier_Injected(ref this);
			}
			set
			{
				set_intensityMultiplier_Injected(ref this, value);
			}
		}

		public int maxLights
		{
			get
			{
				return get_maxLights_Injected(ref this);
			}
			set
			{
				set_maxLights_Injected(ref this, value);
			}
		}

		internal LightsModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref LightsModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref LightsModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_ratio_Injected(ref LightsModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_ratio_Injected(ref LightsModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_useRandomDistribution_Injected(ref LightsModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_useRandomDistribution_Injected(ref LightsModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern Light get_light_Injected(ref LightsModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_light_Injected(ref LightsModule _unity_self, Light value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_useParticleColor_Injected(ref LightsModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_useParticleColor_Injected(ref LightsModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_sizeAffectsRange_Injected(ref LightsModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_sizeAffectsRange_Injected(ref LightsModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_alphaAffectsIntensity_Injected(ref LightsModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_alphaAffectsIntensity_Injected(ref LightsModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_range_Injected(ref LightsModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_range_Injected(ref LightsModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_rangeMultiplier_Injected(ref LightsModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_rangeMultiplier_Injected(ref LightsModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_intensity_Injected(ref LightsModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_intensity_Injected(ref LightsModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_intensityMultiplier_Injected(ref LightsModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_intensityMultiplier_Injected(ref LightsModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_maxLights_Injected(ref LightsModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_maxLights_Injected(ref LightsModule _unity_self, int value);
	}

	public struct TrailModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		public ParticleSystemTrailMode mode
		{
			get
			{
				return get_mode_Injected(ref this);
			}
			set
			{
				set_mode_Injected(ref this, value);
			}
		}

		public float ratio
		{
			get
			{
				return get_ratio_Injected(ref this);
			}
			set
			{
				set_ratio_Injected(ref this, value);
			}
		}

		public MinMaxCurve lifetime
		{
			get
			{
				get_lifetime_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_lifetime_Injected(ref this, ref value);
			}
		}

		public float lifetimeMultiplier
		{
			get
			{
				return get_lifetimeMultiplier_Injected(ref this);
			}
			set
			{
				set_lifetimeMultiplier_Injected(ref this, value);
			}
		}

		public float minVertexDistance
		{
			get
			{
				return get_minVertexDistance_Injected(ref this);
			}
			set
			{
				set_minVertexDistance_Injected(ref this, value);
			}
		}

		public ParticleSystemTrailTextureMode textureMode
		{
			get
			{
				return get_textureMode_Injected(ref this);
			}
			set
			{
				set_textureMode_Injected(ref this, value);
			}
		}

		public bool worldSpace
		{
			get
			{
				return get_worldSpace_Injected(ref this);
			}
			set
			{
				set_worldSpace_Injected(ref this, value);
			}
		}

		public bool dieWithParticles
		{
			get
			{
				return get_dieWithParticles_Injected(ref this);
			}
			set
			{
				set_dieWithParticles_Injected(ref this, value);
			}
		}

		public bool sizeAffectsWidth
		{
			get
			{
				return get_sizeAffectsWidth_Injected(ref this);
			}
			set
			{
				set_sizeAffectsWidth_Injected(ref this, value);
			}
		}

		public bool sizeAffectsLifetime
		{
			get
			{
				return get_sizeAffectsLifetime_Injected(ref this);
			}
			set
			{
				set_sizeAffectsLifetime_Injected(ref this, value);
			}
		}

		public bool inheritParticleColor
		{
			get
			{
				return get_inheritParticleColor_Injected(ref this);
			}
			set
			{
				set_inheritParticleColor_Injected(ref this, value);
			}
		}

		public MinMaxGradient colorOverLifetime
		{
			get
			{
				get_colorOverLifetime_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_colorOverLifetime_Injected(ref this, ref value);
			}
		}

		public MinMaxCurve widthOverTrail
		{
			get
			{
				get_widthOverTrail_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_widthOverTrail_Injected(ref this, ref value);
			}
		}

		public float widthOverTrailMultiplier
		{
			get
			{
				return get_widthOverTrailMultiplier_Injected(ref this);
			}
			set
			{
				set_widthOverTrailMultiplier_Injected(ref this, value);
			}
		}

		public MinMaxGradient colorOverTrail
		{
			get
			{
				get_colorOverTrail_Injected(ref this, out var ret);
				return ret;
			}
			set
			{
				set_colorOverTrail_Injected(ref this, ref value);
			}
		}

		public bool generateLightingData
		{
			get
			{
				return get_generateLightingData_Injected(ref this);
			}
			set
			{
				set_generateLightingData_Injected(ref this, value);
			}
		}

		public int ribbonCount
		{
			get
			{
				return get_ribbonCount_Injected(ref this);
			}
			set
			{
				set_ribbonCount_Injected(ref this, value);
			}
		}

		public float shadowBias
		{
			get
			{
				return get_shadowBias_Injected(ref this);
			}
			set
			{
				set_shadowBias_Injected(ref this, value);
			}
		}

		public bool splitSubEmitterRibbons
		{
			get
			{
				return get_splitSubEmitterRibbons_Injected(ref this);
			}
			set
			{
				set_splitSubEmitterRibbons_Injected(ref this, value);
			}
		}

		public bool attachRibbonsToTransform
		{
			get
			{
				return get_attachRibbonsToTransform_Injected(ref this);
			}
			set
			{
				set_attachRibbonsToTransform_Injected(ref this, value);
			}
		}

		internal TrailModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref TrailModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemTrailMode get_mode_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_mode_Injected(ref TrailModule _unity_self, ParticleSystemTrailMode value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_ratio_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_ratio_Injected(ref TrailModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_lifetime_Injected(ref TrailModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_lifetime_Injected(ref TrailModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_lifetimeMultiplier_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_lifetimeMultiplier_Injected(ref TrailModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_minVertexDistance_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_minVertexDistance_Injected(ref TrailModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern ParticleSystemTrailTextureMode get_textureMode_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_textureMode_Injected(ref TrailModule _unity_self, ParticleSystemTrailTextureMode value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_worldSpace_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_worldSpace_Injected(ref TrailModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_dieWithParticles_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_dieWithParticles_Injected(ref TrailModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_sizeAffectsWidth_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_sizeAffectsWidth_Injected(ref TrailModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_sizeAffectsLifetime_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_sizeAffectsLifetime_Injected(ref TrailModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_inheritParticleColor_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_inheritParticleColor_Injected(ref TrailModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_colorOverLifetime_Injected(ref TrailModule _unity_self, out MinMaxGradient ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_colorOverLifetime_Injected(ref TrailModule _unity_self, ref MinMaxGradient value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_widthOverTrail_Injected(ref TrailModule _unity_self, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_widthOverTrail_Injected(ref TrailModule _unity_self, ref MinMaxCurve value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_widthOverTrailMultiplier_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_widthOverTrailMultiplier_Injected(ref TrailModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void get_colorOverTrail_Injected(ref TrailModule _unity_self, out MinMaxGradient ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_colorOverTrail_Injected(ref TrailModule _unity_self, ref MinMaxGradient value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_generateLightingData_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_generateLightingData_Injected(ref TrailModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern int get_ribbonCount_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_ribbonCount_Injected(ref TrailModule _unity_self, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern float get_shadowBias_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_shadowBias_Injected(ref TrailModule _unity_self, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_splitSubEmitterRibbons_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_splitSubEmitterRibbons_Injected(ref TrailModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_attachRibbonsToTransform_Injected(ref TrailModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_attachRibbonsToTransform_Injected(ref TrailModule _unity_self, bool value);
	}

	public struct CustomDataModule
	{
		internal ParticleSystem m_ParticleSystem;

		public bool enabled
		{
			get
			{
				return get_enabled_Injected(ref this);
			}
			set
			{
				set_enabled_Injected(ref this, value);
			}
		}

		internal CustomDataModule(ParticleSystem particleSystem)
		{
			m_ParticleSystem = particleSystem;
		}

		public void SetMode(ParticleSystemCustomData stream, ParticleSystemCustomDataMode mode)
		{
			SetMode_Injected(ref this, stream, mode);
		}

		public ParticleSystemCustomDataMode GetMode(ParticleSystemCustomData stream)
		{
			return GetMode_Injected(ref this, stream);
		}

		public void SetVectorComponentCount(ParticleSystemCustomData stream, int count)
		{
			SetVectorComponentCount_Injected(ref this, stream, count);
		}

		public int GetVectorComponentCount(ParticleSystemCustomData stream)
		{
			return GetVectorComponentCount_Injected(ref this, stream);
		}

		public void SetVector(ParticleSystemCustomData stream, int component, MinMaxCurve curve)
		{
			SetVector_Injected(ref this, stream, component, ref curve);
		}

		public MinMaxCurve GetVector(ParticleSystemCustomData stream, int component)
		{
			GetVector_Injected(ref this, stream, component, out var ret);
			return ret;
		}

		public void SetColor(ParticleSystemCustomData stream, MinMaxGradient gradient)
		{
			SetColor_Injected(ref this, stream, ref gradient);
		}

		public MinMaxGradient GetColor(ParticleSystemCustomData stream)
		{
			GetColor_Injected(ref this, stream, out var ret);
			return ret;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern bool get_enabled_Injected(ref CustomDataModule _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SpecialName]
		private static extern void set_enabled_Injected(ref CustomDataModule _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMode_Injected(ref CustomDataModule _unity_self, ParticleSystemCustomData stream, ParticleSystemCustomDataMode mode);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleSystemCustomDataMode GetMode_Injected(ref CustomDataModule _unity_self, ParticleSystemCustomData stream);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetVectorComponentCount_Injected(ref CustomDataModule _unity_self, ParticleSystemCustomData stream, int count);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetVectorComponentCount_Injected(ref CustomDataModule _unity_self, ParticleSystemCustomData stream);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetVector_Injected(ref CustomDataModule _unity_self, ParticleSystemCustomData stream, int component, ref MinMaxCurve curve);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetVector_Injected(ref CustomDataModule _unity_self, ParticleSystemCustomData stream, int component, out MinMaxCurve ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetColor_Injected(ref CustomDataModule _unity_self, ParticleSystemCustomData stream, ref MinMaxGradient gradient);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetColor_Injected(ref CustomDataModule _unity_self, ParticleSystemCustomData stream, out MinMaxGradient ret);
	}

	[Obsolete("startDelay property is deprecated. Use main.startDelay or main.startDelayMultiplier instead.", false)]
	public float startDelay
	{
		get
		{
			return main.startDelayMultiplier;
		}
		set
		{
			MainModule mainModule = main;
			mainModule.startDelayMultiplier = value;
		}
	}

	[Obsolete("loop property is deprecated. Use main.loop instead.", false)]
	public bool loop
	{
		get
		{
			return main.loop;
		}
		set
		{
			MainModule mainModule = main;
			mainModule.loop = value;
		}
	}

	[Obsolete("playOnAwake property is deprecated. Use main.playOnAwake instead.", false)]
	public bool playOnAwake
	{
		get
		{
			return main.playOnAwake;
		}
		set
		{
			MainModule mainModule = main;
			mainModule.playOnAwake = value;
		}
	}

	[Obsolete("duration property is deprecated. Use main.duration instead.", false)]
	public float duration => main.duration;

	[Obsolete("playbackSpeed property is deprecated. Use main.simulationSpeed instead.", false)]
	public float playbackSpeed
	{
		get
		{
			return main.simulationSpeed;
		}
		set
		{
			MainModule mainModule = main;
			mainModule.simulationSpeed = value;
		}
	}

	[Obsolete("enableEmission property is deprecated. Use emission.enabled instead.", false)]
	public bool enableEmission
	{
		get
		{
			return emission.enabled;
		}
		set
		{
			EmissionModule emissionModule = emission;
			emissionModule.enabled = value;
		}
	}

	[Obsolete("emissionRate property is deprecated. Use emission.rateOverTime, emission.rateOverDistance, emission.rateOverTimeMultiplier or emission.rateOverDistanceMultiplier instead.", false)]
	public float emissionRate
	{
		get
		{
			return emission.rateOverTimeMultiplier;
		}
		set
		{
			EmissionModule emissionModule = emission;
			emissionModule.rateOverTime = value;
		}
	}

	[Obsolete("startSpeed property is deprecated. Use main.startSpeed or main.startSpeedMultiplier instead.", false)]
	public float startSpeed
	{
		get
		{
			return main.startSpeedMultiplier;
		}
		set
		{
			MainModule mainModule = main;
			mainModule.startSpeedMultiplier = value;
		}
	}

	[Obsolete("startSize property is deprecated. Use main.startSize or main.startSizeMultiplier instead.", false)]
	public float startSize
	{
		get
		{
			return main.startSizeMultiplier;
		}
		set
		{
			MainModule mainModule = main;
			mainModule.startSizeMultiplier = value;
		}
	}

	[Obsolete("startColor property is deprecated. Use main.startColor instead.", false)]
	public Color startColor
	{
		get
		{
			return main.startColor.color;
		}
		set
		{
			MainModule mainModule = main;
			mainModule.startColor = value;
		}
	}

	[Obsolete("startRotation property is deprecated. Use main.startRotation or main.startRotationMultiplier instead.", false)]
	public float startRotation
	{
		get
		{
			return main.startRotationMultiplier;
		}
		set
		{
			MainModule mainModule = main;
			mainModule.startRotationMultiplier = value;
		}
	}

	[Obsolete("startRotation3D property is deprecated. Use main.startRotationX, main.startRotationY and main.startRotationZ instead. (Or main.startRotationXMultiplier, main.startRotationYMultiplier and main.startRotationZMultiplier).", false)]
	public Vector3 startRotation3D
	{
		get
		{
			return new Vector3(main.startRotationXMultiplier, main.startRotationYMultiplier, main.startRotationZMultiplier);
		}
		set
		{
			MainModule mainModule = main;
			mainModule.startRotationXMultiplier = value.x;
			mainModule.startRotationYMultiplier = value.y;
			mainModule.startRotationZMultiplier = value.z;
		}
	}

	[Obsolete("startLifetime property is deprecated. Use main.startLifetime or main.startLifetimeMultiplier instead.", false)]
	public float startLifetime
	{
		get
		{
			return main.startLifetimeMultiplier;
		}
		set
		{
			MainModule mainModule = main;
			mainModule.startLifetimeMultiplier = value;
		}
	}

	[Obsolete("gravityModifier property is deprecated. Use main.gravityModifier or main.gravityModifierMultiplier instead.", false)]
	public float gravityModifier
	{
		get
		{
			return main.gravityModifierMultiplier;
		}
		set
		{
			MainModule mainModule = main;
			mainModule.gravityModifierMultiplier = value;
		}
	}

	[Obsolete("maxParticles property is deprecated. Use main.maxParticles instead.", false)]
	public int maxParticles
	{
		get
		{
			return main.maxParticles;
		}
		set
		{
			MainModule mainModule = main;
			mainModule.maxParticles = value;
		}
	}

	[Obsolete("simulationSpace property is deprecated. Use main.simulationSpace instead.", false)]
	public ParticleSystemSimulationSpace simulationSpace
	{
		get
		{
			return main.simulationSpace;
		}
		set
		{
			MainModule mainModule = main;
			mainModule.simulationSpace = value;
		}
	}

	[Obsolete("scalingMode property is deprecated. Use main.scalingMode instead.", false)]
	public ParticleSystemScalingMode scalingMode
	{
		get
		{
			return main.scalingMode;
		}
		set
		{
			MainModule mainModule = main;
			mainModule.scalingMode = value;
		}
	}

	[Obsolete("automaticCullingEnabled property is deprecated. Use proceduralSimulationSupported instead (UnityUpgradable) -> proceduralSimulationSupported", true)]
	public bool automaticCullingEnabled => proceduralSimulationSupported;

	public extern bool isPlaying
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("SyncJobs(false)->IsPlaying")]
		get;
	}

	public extern bool isEmitting
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("SyncJobs(false)->IsEmitting")]
		get;
	}

	public extern bool isStopped
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("SyncJobs(false)->IsStopped")]
		get;
	}

	public extern bool isPaused
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("SyncJobs(false)->IsPaused")]
		get;
	}

	public extern int particleCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("SyncJobs(false)->GetParticleCount")]
		get;
	}

	public extern float time
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("SyncJobs(false)->GetSecPosition")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("SyncJobs(false)->SetSecPosition")]
		set;
	}

	public extern uint randomSeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("GetRandomSeed")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("SyncJobs(false)->SetRandomSeed")]
		set;
	}

	public extern bool useAutoRandomSeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("GetAutoRandomSeed")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("SyncJobs(false)->SetAutoRandomSeed")]
		set;
	}

	public extern bool proceduralSimulationSupported
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public MainModule main => new MainModule(this);

	public EmissionModule emission => new EmissionModule(this);

	public ShapeModule shape => new ShapeModule(this);

	public VelocityOverLifetimeModule velocityOverLifetime => new VelocityOverLifetimeModule(this);

	public LimitVelocityOverLifetimeModule limitVelocityOverLifetime => new LimitVelocityOverLifetimeModule(this);

	public InheritVelocityModule inheritVelocity => new InheritVelocityModule(this);

	public ForceOverLifetimeModule forceOverLifetime => new ForceOverLifetimeModule(this);

	public ColorOverLifetimeModule colorOverLifetime => new ColorOverLifetimeModule(this);

	public ColorBySpeedModule colorBySpeed => new ColorBySpeedModule(this);

	public SizeOverLifetimeModule sizeOverLifetime => new SizeOverLifetimeModule(this);

	public SizeBySpeedModule sizeBySpeed => new SizeBySpeedModule(this);

	public RotationOverLifetimeModule rotationOverLifetime => new RotationOverLifetimeModule(this);

	public RotationBySpeedModule rotationBySpeed => new RotationBySpeedModule(this);

	public ExternalForcesModule externalForces => new ExternalForcesModule(this);

	public NoiseModule noise => new NoiseModule(this);

	public CollisionModule collision => new CollisionModule(this);

	public TriggerModule trigger => new TriggerModule(this);

	public SubEmittersModule subEmitters => new SubEmittersModule(this);

	public TextureSheetAnimationModule textureSheetAnimation => new TextureSheetAnimationModule(this);

	public LightsModule lights => new LightsModule(this);

	public TrailModule trails => new TrailModule(this);

	public CustomDataModule customData => new CustomDataModule(this);

	[Obsolete("Emit with specific parameters is deprecated. Pass a ParticleSystem.EmitParams parameter instead, which allows you to override some/all of the emission properties", false)]
	public void Emit(Vector3 position, Vector3 velocity, float size, float lifetime, Color32 color)
	{
		Particle particle = new Particle
		{
			position = position,
			velocity = velocity,
			lifetime = lifetime,
			startLifetime = lifetime,
			startSize = size,
			rotation3D = Vector3.zero,
			angularVelocity3D = Vector3.zero,
			startColor = color,
			randomSeed = 5u
		};
		EmitOld_Internal(ref particle);
	}

	[Obsolete("Emit with a single particle structure is deprecated. Pass a ParticleSystem.EmitParams parameter instead, which allows you to override some/all of the emission properties", false)]
	public void Emit(Particle particle)
	{
		EmitOld_Internal(ref particle);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::GetParticleCurrentSize", HasExplicitThis = true)]
	internal extern float GetParticleCurrentSize(ref Particle particle);

	[FreeFunction(Name = "ParticleSystemScriptBindings::GetParticleCurrentSize3D", HasExplicitThis = true)]
	internal Vector3 GetParticleCurrentSize3D(ref Particle particle)
	{
		GetParticleCurrentSize3D_Injected(ref particle, out var ret);
		return ret;
	}

	[FreeFunction(Name = "ParticleSystemScriptBindings::GetParticleCurrentColor", HasExplicitThis = true)]
	internal Color32 GetParticleCurrentColor(ref Particle particle)
	{
		GetParticleCurrentColor_Injected(ref particle, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::GetParticleMeshIndex", HasExplicitThis = true)]
	internal extern int GetParticleMeshIndex(ref Particle particle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::SetParticles", HasExplicitThis = true, ThrowsException = true)]
	public extern void SetParticles([Out] Particle[] particles, int size, int offset);

	public void SetParticles([Out] Particle[] particles, int size)
	{
		SetParticles(particles, size, 0);
	}

	public void SetParticles([Out] Particle[] particles)
	{
		SetParticles(particles, -1);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::SetParticlesWithNativeArray", HasExplicitThis = true, ThrowsException = true)]
	private extern void SetParticlesWithNativeArray(IntPtr particles, int particlesLength, int size, int offset);

	public unsafe void SetParticles([Out] NativeArray<Particle> particles, int size, int offset)
	{
		SetParticlesWithNativeArray((IntPtr)particles.GetUnsafeReadOnlyPtr(), particles.Length, size, 0);
	}

	public void SetParticles([Out] NativeArray<Particle> particles, int size)
	{
		SetParticles(particles, size, 0);
	}

	public void SetParticles([Out] NativeArray<Particle> particles)
	{
		SetParticles(particles, -1);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::GetParticles", HasExplicitThis = true, ThrowsException = true)]
	public extern int GetParticles([Out][NotNull] Particle[] particles, int size, int offset);

	public int GetParticles([Out] Particle[] particles, int size)
	{
		return GetParticles(particles, size, 0);
	}

	public int GetParticles([Out] Particle[] particles)
	{
		return GetParticles(particles, -1);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::GetParticlesWithNativeArray", HasExplicitThis = true, ThrowsException = true)]
	private extern int GetParticlesWithNativeArray(IntPtr particles, int particlesLength, int size, int offset);

	public unsafe int GetParticles([Out] NativeArray<Particle> particles, int size, int offset)
	{
		return GetParticlesWithNativeArray((IntPtr)particles.GetUnsafeReadOnlyPtr(), particles.Length, size, 0);
	}

	public int GetParticles([Out] NativeArray<Particle> particles, int size)
	{
		return GetParticles(particles, size, 0);
	}

	public int GetParticles([Out] NativeArray<Particle> particles)
	{
		return GetParticles(particles, -1);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::SetCustomParticleData", HasExplicitThis = true)]
	public extern void SetCustomParticleData([NotNull] List<Vector4> customData, ParticleSystemCustomData streamIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::GetCustomParticleData", HasExplicitThis = true)]
	public extern int GetCustomParticleData([NotNull] List<Vector4> customData, ParticleSystemCustomData streamIndex);

	public PlaybackState GetPlaybackState()
	{
		GetPlaybackState_Injected(out var ret);
		return ret;
	}

	public void SetPlaybackState(PlaybackState playbackState)
	{
		SetPlaybackState_Injected(ref playbackState);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::GetTrailData", HasExplicitThis = true)]
	private extern void GetTrailDataInternal(ref Trails trailData);

	public Trails GetTrails()
	{
		Trails trailData = new Trails
		{
			positions = new List<Vector4>(),
			frontPositions = new List<int>(),
			backPositions = new List<int>(),
			positionCounts = new List<int>()
		};
		GetTrailDataInternal(ref trailData);
		return trailData;
	}

	[FreeFunction(Name = "ParticleSystemScriptBindings::SetTrailData", HasExplicitThis = true)]
	public void SetTrails(Trails trailData)
	{
		SetTrails_Injected(ref trailData);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::Simulate", HasExplicitThis = true)]
	public extern void Simulate(float t, [DefaultValue("true")] bool withChildren, [DefaultValue("true")] bool restart, [DefaultValue("true")] bool fixedTimeStep);

	public void Simulate(float t, [DefaultValue("true")] bool withChildren, [DefaultValue("true")] bool restart)
	{
		Simulate(t, withChildren, restart, fixedTimeStep: true);
	}

	public void Simulate(float t, [DefaultValue("true")] bool withChildren)
	{
		Simulate(t, withChildren, restart: true);
	}

	public void Simulate(float t)
	{
		Simulate(t, withChildren: true);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::Play", HasExplicitThis = true)]
	public extern void Play([DefaultValue("true")] bool withChildren);

	public void Play()
	{
		Play(withChildren: true);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::Pause", HasExplicitThis = true)]
	public extern void Pause([DefaultValue("true")] bool withChildren);

	public void Pause()
	{
		Pause(withChildren: true);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::Stop", HasExplicitThis = true)]
	public extern void Stop([DefaultValue("true")] bool withChildren, [DefaultValue("ParticleSystemStopBehavior.StopEmitting")] ParticleSystemStopBehavior stopBehavior);

	public void Stop([DefaultValue("true")] bool withChildren)
	{
		Stop(withChildren, ParticleSystemStopBehavior.StopEmitting);
	}

	public void Stop()
	{
		Stop(withChildren: true);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::Clear", HasExplicitThis = true)]
	public extern void Clear([DefaultValue("true")] bool withChildren);

	public void Clear()
	{
		Clear(withChildren: true);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::IsAlive", HasExplicitThis = true)]
	public extern bool IsAlive([DefaultValue("true")] bool withChildren);

	public bool IsAlive()
	{
		return IsAlive(withChildren: true);
	}

	[RequiredByNativeCode]
	public void Emit(int count)
	{
		Emit_Internal(count);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("SyncJobs()->Emit")]
	private extern void Emit_Internal(int count);

	[NativeName("SyncJobs()->EmitParticlesExternal")]
	public void Emit(EmitParams emitParams, int count)
	{
		Emit_Injected(ref emitParams, count);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("SyncJobs()->EmitParticleExternal")]
	private extern void EmitOld_Internal(ref Particle particle);

	public void TriggerSubEmitter(int subEmitterIndex)
	{
		TriggerSubEmitter(subEmitterIndex, null);
	}

	public void TriggerSubEmitter(int subEmitterIndex, ref Particle particle)
	{
		TriggerSubEmitterForParticle(subEmitterIndex, particle);
	}

	[FreeFunction(Name = "ParticleSystemScriptBindings::TriggerSubEmitterForParticle", HasExplicitThis = true)]
	internal void TriggerSubEmitterForParticle(int subEmitterIndex, Particle particle)
	{
		TriggerSubEmitterForParticle_Injected(subEmitterIndex, ref particle);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::TriggerSubEmitter", HasExplicitThis = true)]
	public extern void TriggerSubEmitter(int subEmitterIndex, List<Particle> particles);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemGeometryJob::ResetPreMappedBufferMemory")]
	public static extern void ResetPreMappedBufferMemory();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemGeometryJob::SetMaximumPreMappedBufferCounts")]
	public static extern void SetMaximumPreMappedBufferCounts(int vertexBuffersCount, int indexBuffersCount);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal unsafe extern void* GetManagedJobData();

	internal JobHandle GetManagedJobHandle()
	{
		GetManagedJobHandle_Injected(out var ret);
		return ret;
	}

	internal void SetManagedJobHandle(JobHandle handle)
	{
		SetManagedJobHandle_Injected(ref handle);
	}

	[FreeFunction("ScheduleManagedJob")]
	internal unsafe static JobHandle ScheduleManagedJob(ref JobsUtility.JobScheduleParameters parameters, void* additionalData)
	{
		ScheduleManagedJob_Injected(ref parameters, additionalData, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	internal unsafe static extern void CopyManagedJobData(void* systemPtr, out NativeParticleData particleData);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetParticleCurrentSize3D_Injected(ref Particle particle, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetParticleCurrentColor_Injected(ref Particle particle, out Color32 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetPlaybackState_Injected(out PlaybackState ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetPlaybackState_Injected(ref PlaybackState playbackState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetTrails_Injected(ref Trails trailData);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Emit_Injected(ref EmitParams emitParams, int count);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void TriggerSubEmitterForParticle_Injected(int subEmitterIndex, ref Particle particle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetManagedJobHandle_Injected(out JobHandle ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetManagedJobHandle_Injected(ref JobHandle handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void ScheduleManagedJob_Injected(ref JobsUtility.JobScheduleParameters parameters, void* additionalData, out JobHandle ret);
}
