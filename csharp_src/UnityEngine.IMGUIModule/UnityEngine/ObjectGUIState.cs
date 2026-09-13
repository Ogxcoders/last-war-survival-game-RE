using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/IMGUI/GUIState.h")]
[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
internal class ObjectGUIState : IDisposable
{
	internal IntPtr m_Ptr;

	public ObjectGUIState()
	{
		m_Ptr = Internal_Create();
	}

	public void Dispose()
	{
		Destroy();
		GC.SuppressFinalize(this);
	}

	~ObjectGUIState()
	{
		Destroy();
	}

	private void Destroy()
	{
		if (m_Ptr != IntPtr.Zero)
		{
			Internal_Destroy(m_Ptr);
			m_Ptr = IntPtr.Zero;
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern IntPtr Internal_Create();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern void Internal_Destroy(IntPtr ptr);
}
