using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Xml;
using Unity;

namespace System.Configuration;

[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
public sealed class RsaProtectedConfigurationProvider : ProtectedConfigurationProvider
{
	public string CspProviderName
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public string KeyContainerName
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public RSAParameters RsaPublicKey
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(RSAParameters);
		}
	}

	public bool UseFIPS
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public bool UseMachineContainer
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public bool UseOAEP
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public RsaProtectedConfigurationProvider()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void AddKey(int keySize, bool exportable)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public override XmlNode Decrypt(XmlNode encryptedNode)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public void DeleteKey()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public override XmlNode Encrypt(XmlNode node)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public void ExportKey(string xmlFileName, bool includePrivateParameters)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void ImportKey(string xmlFileName, bool exportable)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public override void Initialize(string name, NameValueCollection configurationValues)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
