using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Animations;

[RequireComponent(typeof(Transform))]
[NativeHeader("Modules/Animation/Constraints/PositionConstraint.h")]
[NativeHeader("Modules/Animation/Constraints/Constraint.bindings.h")]
[UsedByNativeCode]
public sealed class PositionConstraint : Behaviour, IConstraint, IConstraintInternal
{
	public extern float weight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public Vector3 translationAtRest
	{
		get
		{
			get_translationAtRest_Injected(out var ret);
			return ret;
		}
		set
		{
			set_translationAtRest_Injected(ref value);
		}
	}

	public Vector3 translationOffset
	{
		get
		{
			get_translationOffset_Injected(out var ret);
			return ret;
		}
		set
		{
			set_translationOffset_Injected(ref value);
		}
	}

	public extern Axis translationAxis
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool constraintActive
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool locked
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public int sourceCount => GetSourceCountInternal(this);

	private PositionConstraint()
	{
		Internal_Create(this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_Create([Writable] PositionConstraint self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ConstraintBindings::GetSourceCount")]
	private static extern int GetSourceCountInternal([NotNull] PositionConstraint self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ConstraintBindings::GetSources", HasExplicitThis = true)]
	public extern void GetSources([NotNull] List<ConstraintSource> sources);

	public void SetSources(List<ConstraintSource> sources)
	{
		if (sources == null)
		{
			throw new ArgumentNullException("sources");
		}
		SetSourcesInternal(this, sources);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ConstraintBindings::SetSources")]
	private static extern void SetSourcesInternal([NotNull] PositionConstraint self, List<ConstraintSource> sources);

	public int AddSource(ConstraintSource source)
	{
		return AddSource_Injected(ref source);
	}

	public void RemoveSource(int index)
	{
		ValidateSourceIndex(index);
		RemoveSourceInternal(index);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("RemoveSource")]
	private extern void RemoveSourceInternal(int index);

	public ConstraintSource GetSource(int index)
	{
		ValidateSourceIndex(index);
		return GetSourceInternal(index);
	}

	[NativeName("GetSource")]
	private ConstraintSource GetSourceInternal(int index)
	{
		GetSourceInternal_Injected(index, out var ret);
		return ret;
	}

	public void SetSource(int index, ConstraintSource source)
	{
		ValidateSourceIndex(index);
		SetSourceInternal(index, source);
	}

	[NativeName("SetSource")]
	private void SetSourceInternal(int index, ConstraintSource source)
	{
		SetSourceInternal_Injected(index, ref source);
	}

	private void ValidateSourceIndex(int index)
	{
		if (sourceCount == 0)
		{
			throw new InvalidOperationException("The PositionConstraint component has no sources.");
		}
		if (index < 0 || index >= sourceCount)
		{
			throw new ArgumentOutOfRangeException("index", $"Constraint source index {index} is out of bounds (0-{sourceCount}).");
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_translationAtRest_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_translationAtRest_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_translationOffset_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_translationOffset_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern int AddSource_Injected(ref ConstraintSource source);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetSourceInternal_Injected(int index, out ConstraintSource ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetSourceInternal_Injected(int index, ref ConstraintSource source);
}
