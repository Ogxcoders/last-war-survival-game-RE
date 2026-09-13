using System.ComponentModel;
using Unity;

namespace System.Configuration;

public abstract class ConfigurationConverterBase : TypeConverter
{
	protected ConfigurationConverterBase()
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
