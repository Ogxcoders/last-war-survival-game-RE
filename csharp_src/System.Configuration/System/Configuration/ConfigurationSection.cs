using System.Runtime.Versioning;
using System.Xml;
using Unity;

namespace System.Configuration;

public abstract class ConfigurationSection : ConfigurationElement
{
	public SectionInformation SectionInformation
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	protected ConfigurationSection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal virtual void DeserializeSection(XmlReader reader)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal virtual object GetRuntimeObject()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	protected internal override bool IsModified()
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected internal override void ResetModified()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal virtual string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	protected internal virtual bool ShouldSerializeElementInTargetVersion(ConfigurationElement element, string elementName, FrameworkName targetFramework)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected internal virtual bool ShouldSerializePropertyInTargetVersion(ConfigurationProperty property, string propertyName, FrameworkName targetFramework, ConfigurationElement parentConfigurationElement)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected internal virtual bool ShouldSerializeSectionInTargetVersion(FrameworkName targetFramework)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}
}
