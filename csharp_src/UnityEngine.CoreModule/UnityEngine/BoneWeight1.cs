using System;
using UnityEngine.Scripting;

namespace UnityEngine;

[Serializable]
[UsedByNativeCode]
public struct BoneWeight1 : IEquatable<BoneWeight1>
{
	[SerializeField]
	private float m_Weight;

	[SerializeField]
	private int m_BoneIndex;

	public float weight
	{
		get
		{
			return m_Weight;
		}
		set
		{
			m_Weight = value;
		}
	}

	public int boneIndex
	{
		get
		{
			return m_BoneIndex;
		}
		set
		{
			m_BoneIndex = value;
		}
	}

	public override bool Equals(object other)
	{
		return other is BoneWeight1 && Equals((BoneWeight1)other);
	}

	public bool Equals(BoneWeight1 other)
	{
		return boneIndex.Equals(other.boneIndex) && weight.Equals(other.weight);
	}

	public override int GetHashCode()
	{
		return boneIndex.GetHashCode() ^ weight.GetHashCode();
	}

	public static bool operator ==(BoneWeight1 lhs, BoneWeight1 rhs)
	{
		return lhs.boneIndex == rhs.boneIndex && lhs.weight == rhs.weight;
	}

	public static bool operator !=(BoneWeight1 lhs, BoneWeight1 rhs)
	{
		return !(lhs == rhs);
	}
}
