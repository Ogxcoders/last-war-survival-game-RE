using System.Collections;
using Unity;

namespace System.Configuration;

public class ConfigurationLocationCollection : ReadOnlyCollectionBase
{
	public ConfigurationLocation this[int index]
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	internal ConfigurationLocationCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
