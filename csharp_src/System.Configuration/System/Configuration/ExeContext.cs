using Unity;

namespace System.Configuration;

public sealed class ExeContext
{
	public string ExePath
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ConfigurationUserLevel UserLevel
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(ConfigurationUserLevel);
		}
	}

	internal ExeContext()
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
