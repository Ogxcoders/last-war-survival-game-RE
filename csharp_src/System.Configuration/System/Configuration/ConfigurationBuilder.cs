using System.Configuration.Provider;
using System.Xml;
using Unity;

namespace System.Configuration;

public abstract class ConfigurationBuilder : ProviderBase
{
	protected ConfigurationBuilder()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual ConfigurationSection ProcessConfigurationSection(ConfigurationSection configSection)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual XmlNode ProcessRawXml(XmlNode rawXml)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}
}
