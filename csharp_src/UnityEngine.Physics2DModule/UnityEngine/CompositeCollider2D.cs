using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[NativeHeader("Modules/Physics2D/Public/CompositeCollider2D.h")]
public sealed class CompositeCollider2D : Collider2D
{
	public enum GeometryType
	{
		Outlines,
		Polygons
	}

	public enum GenerationType
	{
		Synchronous,
		Manual
	}

	public extern GeometryType geometryType
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern GenerationType generationType
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float vertexDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float edgeRadius
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float offsetDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int pathCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern int pointCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void GenerateGeometry();

	public int GetPathPointCount(int index)
	{
		int num = pathCount - 1;
		if (index < 0 || index > num)
		{
			throw new ArgumentOutOfRangeException("index", $"Path index {index} must be in the range of 0 to {num}.");
		}
		return GetPathPointCount_Internal(index);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetPathPointCount_Binding")]
	private extern int GetPathPointCount_Internal(int index);

	public int GetPath(int index, Vector2[] points)
	{
		if (index < 0 || index >= pathCount)
		{
			throw new ArgumentOutOfRangeException("index", $"Path index {index} must be in the range of 0 to {pathCount - 1}.");
		}
		if (points == null)
		{
			throw new ArgumentNullException("points");
		}
		return GetPathArray_Internal(index, points);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetPathArray_Binding")]
	private extern int GetPathArray_Internal(int index, [NotNull] Vector2[] points);

	public int GetPath(int index, List<Vector2> points)
	{
		if (index < 0 || index >= pathCount)
		{
			throw new ArgumentOutOfRangeException("index", $"Path index {index} must be in the range of 0 to {pathCount - 1}.");
		}
		if (points == null)
		{
			throw new ArgumentNullException("points");
		}
		return GetPathList_Internal(index, points);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetPathList_Binding")]
	private extern int GetPathList_Internal(int index, [NotNull] List<Vector2> points);
}
