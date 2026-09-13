using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Runtime/Interfaces/IRaycast.h")]
[NativeHeader("Modules/Physics/RaycastHit.h")]
[UsedByNativeCode]
[NativeHeader("PhysicsScriptingClasses.h")]
public struct RaycastHit
{
	[NativeName("point")]
	internal Vector3 m_Point;

	[NativeName("normal")]
	internal Vector3 m_Normal;

	[NativeName("faceID")]
	internal uint m_FaceID;

	[NativeName("distance")]
	internal float m_Distance;

	[NativeName("uv")]
	internal Vector2 m_UV;

	[NativeName("collider")]
	internal int m_Collider;

	public Collider collider => Object.FindObjectFromInstanceID(m_Collider) as Collider;

	public Vector3 point
	{
		get
		{
			return m_Point;
		}
		set
		{
			m_Point = value;
		}
	}

	public Vector3 normal
	{
		get
		{
			return m_Normal;
		}
		set
		{
			m_Normal = value;
		}
	}

	public Vector3 barycentricCoordinate
	{
		get
		{
			return new Vector3(1f - (m_UV.y + m_UV.x), m_UV.x, m_UV.y);
		}
		set
		{
			m_UV = value;
		}
	}

	public float distance
	{
		get
		{
			return m_Distance;
		}
		set
		{
			m_Distance = value;
		}
	}

	public int triangleIndex => (int)m_FaceID;

	public Vector2 textureCoord => CalculateRaycastTexCoord(collider, m_UV, m_Point, m_FaceID, 0);

	public Vector2 textureCoord2 => CalculateRaycastTexCoord(collider, m_UV, m_Point, m_FaceID, 1);

	[Obsolete("Use textureCoord2 instead. (UnityUpgradable) -> textureCoord2")]
	public Vector2 textureCoord1 => textureCoord2;

	public Transform transform
	{
		get
		{
			Rigidbody rigidbody = this.rigidbody;
			if (rigidbody != null)
			{
				return rigidbody.transform;
			}
			if (collider != null)
			{
				return collider.transform;
			}
			return null;
		}
	}

	public Rigidbody rigidbody => (collider != null) ? collider.attachedRigidbody : null;

	public Vector2 lightmapCoord
	{
		get
		{
			Vector2 result = CalculateRaycastTexCoord(collider, m_UV, m_Point, m_FaceID, 1);
			if (collider.GetComponent<Renderer>() != null)
			{
				Vector4 lightmapScaleOffset = collider.GetComponent<Renderer>().lightmapScaleOffset;
				result.x = result.x * lightmapScaleOffset.x + lightmapScaleOffset.z;
				result.y = result.y * lightmapScaleOffset.y + lightmapScaleOffset.w;
			}
			return result;
		}
	}

	[FreeFunction]
	private static Vector2 CalculateRaycastTexCoord(Collider collider, Vector2 uv, Vector3 pos, uint face, int textcoord)
	{
		CalculateRaycastTexCoord_Injected(collider, ref uv, ref pos, face, textcoord, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void CalculateRaycastTexCoord_Injected(Collider collider, ref Vector2 uv, ref Vector3 pos, uint face, int textcoord, out Vector2 ret);
}
