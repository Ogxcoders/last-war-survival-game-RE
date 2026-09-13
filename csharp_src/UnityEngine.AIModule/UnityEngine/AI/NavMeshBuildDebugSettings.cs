using UnityEngine.Bindings;

namespace UnityEngine.AI;

[NativeHeader("Modules/AI/Public/NavMeshBuildDebugSettings.h")]
public struct NavMeshBuildDebugSettings
{
	private byte m_Flags;

	public NavMeshBuildDebugFlags flags
	{
		get
		{
			return (NavMeshBuildDebugFlags)m_Flags;
		}
		set
		{
			m_Flags = (byte)value;
		}
	}
}
