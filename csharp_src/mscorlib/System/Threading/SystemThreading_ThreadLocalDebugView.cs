using System.Collections.Generic;

namespace System.Threading;

internal sealed class SystemThreading_ThreadLocalDebugView<T>
{
	private readonly ThreadLocal<T> m_tlocal;

	public bool IsValueCreated => m_tlocal.IsValueCreated;

	public T Value => m_tlocal.ValueForDebugDisplay;

	public List<T> Values => m_tlocal.ValuesForDebugDisplay;

	public SystemThreading_ThreadLocalDebugView(ThreadLocal<T> tlocal)
	{
		m_tlocal = tlocal;
	}
}
