using Unity;

namespace System.Configuration;

public sealed class ContextInformation
{
	public object HostingContext
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public bool IsMachineLevel
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	internal ContextInformation()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public object GetSection(string sectionName)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}
}
