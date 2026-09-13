using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine;

internal class ParticleSystemExtensionsImpl
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::GetSafeCollisionEventSize")]
	internal static extern int GetSafeCollisionEventSize([NotNull] ParticleSystem ps);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::GetCollisionEventsDeprecated")]
	internal static extern int GetCollisionEventsDeprecated([NotNull] ParticleSystem ps, GameObject go, [Out] ParticleCollisionEvent[] collisionEvents);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::GetSafeTriggerParticlesSize")]
	internal static extern int GetSafeTriggerParticlesSize([NotNull] ParticleSystem ps, int type);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::GetCollisionEvents")]
	internal static extern int GetCollisionEvents([NotNull] ParticleSystem ps, [NotNull] GameObject go, [NotNull] List<ParticleCollisionEvent> collisionEvents);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::GetTriggerParticles")]
	internal static extern int GetTriggerParticles([NotNull] ParticleSystem ps, int type, [NotNull] List<ParticleSystem.Particle> particles);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::SetTriggerParticles")]
	internal static extern void SetTriggerParticles([NotNull] ParticleSystem ps, int type, [NotNull] List<ParticleSystem.Particle> particles, int offset, int count);
}
