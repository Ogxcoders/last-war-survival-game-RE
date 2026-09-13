using System.Collections.Specialized;
using System.Security.Permissions;
using System.Xml;
using Unity;

namespace System.Configuration;

[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
public sealed class DpapiProtectedConfigurationProvider : ProtectedConfigurationProvider
{
	public bool UseMachineProtection
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public DpapiProtectedConfigurationProvider()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public override XmlNode Decrypt(XmlNode encryptedNode)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public override XmlNode Encrypt(XmlNode node)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public override void Initialize(string name, NameValueCollection configurationValues)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
