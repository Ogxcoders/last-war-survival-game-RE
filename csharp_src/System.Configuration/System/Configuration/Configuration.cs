using System.Runtime.Versioning;
using System.Security.Permissions;
using Unity;

namespace System.Configuration;

public sealed class Configuration
{
	public AppSettingsSection AppSettings
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public Func<string, string> AssemblyStringTransformer
	{
		get
		{
			//IL_0007: Expected O, but got I4
			ThrowStub.ThrowNotSupportedException();
			return (Func<string, string>)0;
		}
		[ConfigurationPermission(SecurityAction.Demand, Unrestricted = true)]
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public ConnectionStringsSection ConnectionStrings
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ContextInformation EvaluationContext
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public string FilePath
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public bool HasFile
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public ConfigurationLocationCollection Locations
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public bool NamespaceDeclared
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

	public ConfigurationSectionGroup RootSectionGroup
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

	public FrameworkName TargetFramework
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		[ConfigurationPermission(SecurityAction.Demand, Unrestricted = true)]
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public Func<string, string> TypeStringTransformer
	{
		get
		{
			//IL_0007: Expected O, but got I4
			ThrowStub.ThrowNotSupportedException();
			return (Func<string, string>)0;
		}
		[ConfigurationPermission(SecurityAction.Demand, Unrestricted = true)]
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	internal Configuration()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationSection GetSection(string sectionName)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public ConfigurationSectionGroup GetSectionGroup(string sectionGroupName)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public void Save()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Save(ConfigurationSaveMode saveMode)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void Save(ConfigurationSaveMode saveMode, bool forceSaveAll)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void SaveAs(string filename)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void SaveAs(string filename, ConfigurationSaveMode saveMode)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void SaveAs(string filename, ConfigurationSaveMode saveMode, bool forceSaveAll)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
