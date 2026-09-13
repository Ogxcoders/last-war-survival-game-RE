using System.Xml;
using Unity;

namespace System.Configuration;

public sealed class AppSettingsSection : ConfigurationSection
{
	public string File
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

	protected internal override ConfigurationPropertyCollection Properties
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public KeyValueConfigurationCollection Settings
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public AppSettingsSection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal override object GetRuntimeObject()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	protected internal override bool IsModified()
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected internal override void Reset(ConfigurationElement parentSection)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal override string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}
}
