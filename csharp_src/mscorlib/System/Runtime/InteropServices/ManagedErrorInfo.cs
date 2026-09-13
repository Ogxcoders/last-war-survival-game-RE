namespace System.Runtime.InteropServices;

internal class ManagedErrorInfo : IErrorInfo
{
	private Exception m_Exception;

	public Exception Exception => m_Exception;

	public ManagedErrorInfo(Exception e)
	{
		m_Exception = e;
	}

	public int GetGUID(out Guid guid)
	{
		guid = Guid.Empty;
		return 0;
	}

	public int GetSource(out string source)
	{
		source = m_Exception.Source;
		return 0;
	}

	public int GetDescription(out string description)
	{
		description = m_Exception.Message;
		return 0;
	}

	public int GetHelpFile(out string helpFile)
	{
		helpFile = m_Exception.HelpLink;
		return 0;
	}

	public int GetHelpContext(out uint helpContext)
	{
		helpContext = 0u;
		return 0;
	}
}
