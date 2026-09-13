using Unity;

namespace System.Configuration;

public class ConfigurationLocation
{
	public string Path
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	internal ConfigurationLocation()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public Configuration OpenConfiguration()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}
}
