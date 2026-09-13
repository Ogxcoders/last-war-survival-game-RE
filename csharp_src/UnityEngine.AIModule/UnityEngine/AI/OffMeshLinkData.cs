using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI;

[MovedFrom("UnityEngine")]
[NativeHeader("Modules/AI/Components/OffMeshLink.bindings.h")]
public struct OffMeshLinkData
{
	internal int m_Valid;

	internal int m_Activated;

	internal int m_InstanceID;

	internal OffMeshLinkType m_LinkType;

	internal Vector3 m_StartPos;

	internal Vector3 m_EndPos;

	public bool valid => m_Valid != 0;

	public bool activated => m_Activated != 0;

	public OffMeshLinkType linkType => m_LinkType;

	public Vector3 startPos => m_StartPos;

	public Vector3 endPos => m_EndPos;

	public OffMeshLink offMeshLink => GetOffMeshLinkInternal(m_InstanceID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("OffMeshLinkScriptBindings::GetOffMeshLinkInternal")]
	internal static extern OffMeshLink GetOffMeshLinkInternal(int instanceID);
}
