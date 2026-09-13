using System.Configuration.Provider;
using System.Xml;
using Unity;

namespace System.Configuration;

public abstract class ProtectedConfigurationProvider : ProviderBase
{
	protected ProtectedConfigurationProvider()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public abstract XmlNode Decrypt(XmlNode encryptedNode);

	public abstract XmlNode Encrypt(XmlNode node);
}
