using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI;

[StructLayout(LayoutKind.Sequential)]
[MovedFrom("UnityEngine")]
[NativeHeader("Modules/AI/NavMeshPath.bindings.h")]
public sealed class NavMeshPath
{
	internal IntPtr m_Ptr;

	internal Vector3[] m_Corners;

	public Vector3[] corners
	{
		get
		{
			CalculateCorners();
			return m_Corners;
		}
	}

	public extern NavMeshPathStatus status
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public NavMeshPath()
	{
		m_Ptr = InitializeNavMeshPath();
	}

	~NavMeshPath()
	{
		DestroyNavMeshPath(m_Ptr);
		m_Ptr = IntPtr.Zero;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("NavMeshPathScriptBindings::InitializeNavMeshPath")]
	private static extern IntPtr InitializeNavMeshPath();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("NavMeshPathScriptBindings::DestroyNavMeshPath", IsThreadSafe = true)]
	private static extern void DestroyNavMeshPath(IntPtr ptr);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("NavMeshPathScriptBindings::GetCornersNonAlloc", HasExplicitThis = true)]
	public extern int GetCornersNonAlloc([Out] Vector3[] results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("NavMeshPathScriptBindings::CalculateCornersInternal", HasExplicitThis = true)]
	private extern Vector3[] CalculateCornersInternal();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("NavMeshPathScriptBindings::ClearCornersInternal", HasExplicitThis = true)]
	private extern void ClearCornersInternal();

	public void ClearCorners()
	{
		ClearCornersInternal();
		m_Corners = null;
	}

	private void CalculateCorners()
	{
		if (m_Corners == null)
		{
			m_Corners = CalculateCornersInternal();
		}
	}
}
