using System;
using System.Runtime.CompilerServices;
using Unity.Jobs;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Animations;

[MovedFrom("UnityEngine.Experimental.Animations")]
[NativeHeader("Modules/Animation/Animator.h")]
[NativeHeader("Modules/Animation/Director/AnimationStreamHandles.h")]
[NativeHeader("Modules/Animation/Director/AnimationSceneHandles.h")]
[NativeHeader("Modules/Animation/Director/AnimationStream.h")]
[StaticAccessor("AnimatorJobExtensionsBindings", StaticAccessorType.DoubleColon)]
[NativeHeader("Modules/Animation/ScriptBindings/AnimatorJobExtensions.bindings.h")]
public static class AnimatorJobExtensions
{
	public static void AddJobDependency(this Animator animator, JobHandle jobHandle)
	{
		InternalAddJobDependency(animator, jobHandle);
	}

	public static TransformStreamHandle BindStreamTransform(this Animator animator, Transform transform)
	{
		TransformStreamHandle transformStreamHandle = default(TransformStreamHandle);
		InternalBindStreamTransform(animator, transform, out transformStreamHandle);
		return transformStreamHandle;
	}

	public static PropertyStreamHandle BindStreamProperty(this Animator animator, Transform transform, Type type, string property)
	{
		return animator.BindStreamProperty(transform, type, property, isObjectReference: false);
	}

	public static PropertyStreamHandle BindCustomStreamProperty(this Animator animator, string property, CustomStreamPropertyType type)
	{
		PropertyStreamHandle propertyStreamHandle = default(PropertyStreamHandle);
		InternalBindCustomStreamProperty(animator, property, type, out propertyStreamHandle);
		return propertyStreamHandle;
	}

	public static PropertyStreamHandle BindStreamProperty(this Animator animator, Transform transform, Type type, string property, [DefaultValue("false")] bool isObjectReference)
	{
		PropertyStreamHandle propertyStreamHandle = default(PropertyStreamHandle);
		InternalBindStreamProperty(animator, transform, type, property, isObjectReference, out propertyStreamHandle);
		return propertyStreamHandle;
	}

	public static TransformSceneHandle BindSceneTransform(this Animator animator, Transform transform)
	{
		TransformSceneHandle transformSceneHandle = default(TransformSceneHandle);
		InternalBindSceneTransform(animator, transform, out transformSceneHandle);
		return transformSceneHandle;
	}

	public static PropertySceneHandle BindSceneProperty(this Animator animator, Transform transform, Type type, string property)
	{
		return animator.BindSceneProperty(transform, type, property, isObjectReference: false);
	}

	public static PropertySceneHandle BindSceneProperty(this Animator animator, Transform transform, Type type, string property, [DefaultValue("false")] bool isObjectReference)
	{
		PropertySceneHandle propertySceneHandle = default(PropertySceneHandle);
		InternalBindSceneProperty(animator, transform, type, property, isObjectReference, out propertySceneHandle);
		return propertySceneHandle;
	}

	public static bool OpenAnimationStream(this Animator animator, ref AnimationStream stream)
	{
		return InternalOpenAnimationStream(animator, ref stream);
	}

	public static void CloseAnimationStream(this Animator animator, ref AnimationStream stream)
	{
		InternalCloseAnimationStream(animator, ref stream);
	}

	public static void ResolveAllStreamHandles(this Animator animator)
	{
		InternalResolveAllStreamHandles(animator);
	}

	public static void ResolveAllSceneHandles(this Animator animator)
	{
		InternalResolveAllSceneHandles(animator);
	}

	internal static void UnbindAllHandles(this Animator animator)
	{
		InternalUnbindAllHandles(animator);
	}

	private static void InternalAddJobDependency([NotNull] Animator animator, JobHandle jobHandle)
	{
		InternalAddJobDependency_Injected(animator, ref jobHandle);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalBindStreamTransform([NotNull] Animator animator, [NotNull] Transform transform, out TransformStreamHandle transformStreamHandle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalBindStreamProperty([NotNull] Animator animator, [NotNull] Transform transform, [NotNull] Type type, [NotNull] string property, bool isObjectReference, out PropertyStreamHandle propertyStreamHandle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalBindCustomStreamProperty([NotNull] Animator animator, [NotNull] string property, CustomStreamPropertyType propertyType, out PropertyStreamHandle propertyStreamHandle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalBindSceneTransform([NotNull] Animator animator, [NotNull] Transform transform, out TransformSceneHandle transformSceneHandle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalBindSceneProperty([NotNull] Animator animator, [NotNull] Transform transform, [NotNull] Type type, [NotNull] string property, bool isObjectReference, out PropertySceneHandle propertySceneHandle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool InternalOpenAnimationStream([NotNull] Animator animator, ref AnimationStream stream);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalCloseAnimationStream([NotNull] Animator animator, ref AnimationStream stream);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalResolveAllStreamHandles([NotNull] Animator animator);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalResolveAllSceneHandles([NotNull] Animator animator);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalUnbindAllHandles([NotNull] Animator animator);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalAddJobDependency_Injected(Animator animator, ref JobHandle jobHandle);
}
