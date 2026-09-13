using System.Runtime.Versioning;
using Unity;

namespace System.Configuration;

public class ConfigurationSectionGroup
{
	public bool IsDeclarationRequired
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public bool IsDeclared
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public string Name
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public string SectionGroupName
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ConfigurationSectionGroupCollection SectionGroups
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ConfigurationSectionCollection Sections
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

	public ConfigurationSectionGroup()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void ForceDeclaration()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void ForceDeclaration(bool force)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal virtual bool ShouldSerializeSectionGroupInTargetVersion(FrameworkName targetFramework)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}
}
