using System;

namespace UnityEngine.Experimental.AI;

public struct PolygonId : IEquatable<PolygonId>
{
	internal ulong polyRef;

	public bool IsNull()
	{
		return polyRef == 0;
	}

	public static bool operator ==(PolygonId x, PolygonId y)
	{
		return x.polyRef == y.polyRef;
	}

	public static bool operator !=(PolygonId x, PolygonId y)
	{
		return x.polyRef != y.polyRef;
	}

	public override int GetHashCode()
	{
		return polyRef.GetHashCode();
	}

	public bool Equals(PolygonId rhs)
	{
		return rhs == this;
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !(obj is PolygonId))
		{
			return false;
		}
		PolygonId polygonId = (PolygonId)obj;
		return polygonId == this;
	}
}
