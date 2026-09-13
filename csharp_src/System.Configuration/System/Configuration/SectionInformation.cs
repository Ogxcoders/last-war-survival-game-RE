using Unity;

namespace System.Configuration;

public sealed class SectionInformation
{
	public ConfigurationAllowDefinition AllowDefinition
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(ConfigurationAllowDefinition);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public ConfigurationAllowExeDefinition AllowExeDefinition
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(ConfigurationAllowExeDefinition);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public bool AllowLocation
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public bool AllowOverride
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public string ConfigSource
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

	public ConfigurationBuilder ConfigurationBuilder
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public bool ForceSave
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public bool InheritInChildApplications
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

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

	public bool IsLocked
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public bool IsProtected
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

	public OverrideMode OverrideMode
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(OverrideMode);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public OverrideMode OverrideModeDefault
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(OverrideMode);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public OverrideMode OverrideModeEffective
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(OverrideMode);
		}
	}

	public ProtectedConfigurationProvider ProtectionProvider
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public bool RequirePermission
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public bool RestartOnExternalChanges
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public string SectionName
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

	internal SectionInformation()
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

	public ConfigurationSection GetParentSection()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public string GetRawXml()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public void ProtectSection(string protectionProvider)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void RevertToParent()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void SetRawXml(string rawXml)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void UnprotectSection()
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
