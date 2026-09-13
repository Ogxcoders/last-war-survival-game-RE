using Unity;

namespace System.Configuration;

public sealed class GenericEnumConverter : ConfigurationConverterBase
{
	public GenericEnumConverter(Type typeEnum)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
