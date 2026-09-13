using System.Xml;
using Unity;

namespace System.Configuration;

public sealed class IgnoreSection : ConfigurationSection
{
	protected internal override ConfigurationPropertyCollection Properties
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public IgnoreSection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal override void DeserializeSection(XmlReader xmlReader)
	{
		ThrowStub.ThrowNotSupportedException();
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

	protected internal override void ResetModified()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal override string SerializeSection(ConfigurationElement parentSection, string name, ConfigurationSaveMode saveMode)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}
}
