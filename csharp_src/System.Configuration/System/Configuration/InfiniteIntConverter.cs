using Unity;

namespace System.Configuration;

public sealed class InfiniteIntConverter : ConfigurationConverterBase
{
	public InfiniteIntConverter()
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
