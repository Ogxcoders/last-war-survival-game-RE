using System.Collections.Specialized;
using Unity;

namespace System.Configuration;

public sealed class ProviderSettings : ConfigurationElement
{
	public string Name
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public NameValueCollection Parameters
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	protected internal override ConfigurationPropertyCollection Properties
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public string Type
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public ProviderSettings()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ProviderSettings(string name, string type)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal override bool IsModified()
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected internal override void Reset(ConfigurationElement parentElement)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
