namespace System.Runtime.InteropServices;

public sealed class VariantWrapper
{
	private object m_WrappedObject;

	public object WrappedObject => m_WrappedObject;

	public VariantWrapper(object obj)
	{
		m_WrappedObject = obj;
	}
}
