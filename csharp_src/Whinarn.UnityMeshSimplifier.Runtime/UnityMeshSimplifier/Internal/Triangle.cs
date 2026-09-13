using System;
using System.Runtime.CompilerServices;

namespace UnityMeshSimplifier.Internal;

internal struct Triangle : IEquatable<Triangle>
{
	public int index;

	public int v0;

	public int v1;

	public int v2;

	public int subMeshIndex;

	public int va0;

	public int va1;

	public int va2;

	public double err0;

	public double err1;

	public double err2;

	public double err3;

	public bool deleted;

	public bool dirty;

	public Vector3d n;

	public int this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return index switch
			{
				1 => v1, 
				0 => v0, 
				_ => v2, 
			};
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			switch (index)
			{
			case 0:
				v0 = value;
				break;
			case 1:
				v1 = value;
				break;
			case 2:
				v2 = value;
				break;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Triangle(int index, int v0, int v1, int v2, int subMeshIndex)
	{
		this.index = index;
		this.v0 = v0;
		this.v1 = v1;
		this.v2 = v2;
		this.subMeshIndex = subMeshIndex;
		va0 = v0;
		va1 = v1;
		va2 = v2;
		err0 = (err1 = (err2 = (err3 = 0.0)));
		deleted = (dirty = false);
		n = default(Vector3d);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetAttributeIndices(int[] attributeIndices)
	{
		attributeIndices[0] = va0;
		attributeIndices[1] = va1;
		attributeIndices[2] = va2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetAttributeIndex(int index, int value)
	{
		switch (index)
		{
		case 0:
			va0 = value;
			break;
		case 1:
			va1 = value;
			break;
		case 2:
			va2 = value;
			break;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetErrors(double[] err)
	{
		err[0] = err0;
		err[1] = err1;
		err[2] = err2;
	}

	public override int GetHashCode()
	{
		return index;
	}

	public override bool Equals(object obj)
	{
		if (obj is Triangle triangle)
		{
			return index == triangle.index;
		}
		return false;
	}

	public bool Equals(Triangle other)
	{
		return index == other.index;
	}
}
