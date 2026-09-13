using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.AI;

[NativeHeader("Modules/AI/Public/NavMeshBindingTypes.h")]
[UsedByNativeCode]
public struct NavMeshBuildSource
{
	private Matrix4x4 m_Transform;

	private Vector3 m_Size;

	private NavMeshBuildSourceShape m_Shape;

	private int m_Area;

	private int m_InstanceID;

	private int m_ComponentID;

	public Matrix4x4 transform
	{
		get
		{
			return m_Transform;
		}
		set
		{
			m_Transform = value;
		}
	}

	public Vector3 size
	{
		get
		{
			return m_Size;
		}
		set
		{
			m_Size = value;
		}
	}

	public NavMeshBuildSourceShape shape
	{
		get
		{
			return m_Shape;
		}
		set
		{
			m_Shape = value;
		}
	}

	public int area
	{
		get
		{
			return m_Area;
		}
		set
		{
			m_Area = value;
		}
	}

	public Object sourceObject
	{
		get
		{
			return InternalGetObject(m_InstanceID);
		}
		set
		{
			m_InstanceID = ((value != null) ? value.GetInstanceID() : 0);
		}
	}

	public Component component
	{
		get
		{
			return InternalGetComponent(m_ComponentID);
		}
		set
		{
			m_ComponentID = ((value != null) ? value.GetInstanceID() : 0);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("NavMeshBuildSource", StaticAccessorType.DoubleColon)]
	private static extern Component InternalGetComponent(int instanceID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("NavMeshBuildSource", StaticAccessorType.DoubleColon)]
	private static extern Object InternalGetObject(int instanceID);
}
