using Unity;

namespace System.Configuration;

public sealed class InfiniteTimeSpanConverter : ConfigurationConverterBase
{
	public InfiniteTimeSpanConverter()
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
