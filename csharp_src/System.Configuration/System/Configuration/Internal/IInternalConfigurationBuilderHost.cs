using System.Runtime.InteropServices;
using System.Xml;

namespace System.Configuration.Internal;

[ComVisible(false)]
public interface IInternalConfigurationBuilderHost
{
	ConfigurationSection ProcessConfigurationSection(ConfigurationSection configSection, ConfigurationBuilder builder);

	XmlNode ProcessRawXml(XmlNode rawXml, ConfigurationBuilder builder);
}
