namespace System.Runtime.InteropServices;

public sealed class UnknownWrapper
{
	private object m_WrappedObject;

	public object WrappedObject => m_WrappedObject;

	public UnknownWrapper(object obj)
	{
		m_WrappedObject = obj;
	}
}
