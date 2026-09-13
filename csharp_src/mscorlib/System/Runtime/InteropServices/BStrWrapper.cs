namespace System.Runtime.InteropServices;

public sealed class BStrWrapper
{
	private string m_WrappedObject;

	public string WrappedObject => m_WrappedObject;

	public BStrWrapper(string value)
	{
		m_WrappedObject = value;
	}

	public BStrWrapper(object value)
	{
		m_WrappedObject = (string)value;
	}
}
