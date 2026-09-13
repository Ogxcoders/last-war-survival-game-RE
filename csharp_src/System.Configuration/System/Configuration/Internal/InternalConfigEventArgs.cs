using Unity;

namespace System.Configuration.Internal;

public sealed class InternalConfigEventArgs : EventArgs
{
	public string ConfigPath
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public InternalConfigEventArgs(string configPath)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
