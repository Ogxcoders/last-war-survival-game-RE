using System;
using System.Runtime.InteropServices;

namespace UnityEngine.UIElements;

[StructLayout(LayoutKind.Sequential, Size = 1)]
internal struct EventDebuggerLogCall : IDisposable
{
	public EventDebuggerLogCall(Delegate callback, EventBase evt)
	{
	}

	public void Dispose()
	{
	}
}
