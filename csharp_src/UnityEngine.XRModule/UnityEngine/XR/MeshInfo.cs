using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR;

[NativeHeader("Modules/XR/Subsystems/Meshing/XRMeshBindings.h")]
[UsedByNativeCode]
public struct MeshInfo : IEquatable<MeshInfo>
{
	public MeshId MeshId { get; set; }

	public MeshChangeState ChangeState { get; set; }

	public int PriorityHint { get; set; }

	public override bool Equals(object obj)
	{
		if (!(obj is MeshInfo))
		{
			return false;
		}
		return Equals((MeshInfo)obj);
	}

	public bool Equals(MeshInfo other)
	{
		return MeshId.Equals(other.MeshId) && ChangeState.Equals(other.ChangeState) && PriorityHint.Equals(other.PriorityHint);
	}

	public static bool operator ==(MeshInfo lhs, MeshInfo rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(MeshInfo lhs, MeshInfo rhs)
	{
		return !lhs.Equals(rhs);
	}

	public override int GetHashCode()
	{
		return HashCodeHelper.Combine(HashCodeHelper.Combine(MeshId.GetHashCode(), ChangeState.GetHashCode()), PriorityHint);
	}
}
