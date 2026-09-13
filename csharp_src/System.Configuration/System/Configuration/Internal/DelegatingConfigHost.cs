using System.IO;
using System.Security;
using System.Xml;
using Unity;

namespace System.Configuration.Internal;

public class DelegatingConfigHost : IInternalConfigHost, IInternalConfigurationBuilderHost
{
	protected IInternalConfigurationBuilderHost ConfigBuilderHost
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	protected IInternalConfigHost Host
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

	public virtual bool IsRemote
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public virtual bool SupportsChangeNotifications
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public virtual bool SupportsLocation
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public virtual bool SupportsPath
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public virtual bool SupportsRefresh
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	protected DelegatingConfigHost()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual object CreateConfigurationContext(string configPath, string locationSubPath)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual object CreateDeprecatedConfigContext(string configPath)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual string DecryptSection(string encryptedXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual void DeleteStream(string streamName)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual string EncryptSection(string clearTextXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual string GetConfigPathFromLocationSubPath(string configPath, string locationSubPath)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual Type GetConfigType(string typeName, bool throwOnError)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual string GetConfigTypeName(Type t)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual void GetRestrictedPermissions(IInternalConfigRecord configRecord, out PermissionSet permissionSet, out bool isHostReady)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual string GetStreamName(string configPath)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual string GetStreamNameForConfigSource(string streamName, string configSource)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual object GetStreamVersion(string streamName)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual IDisposable Impersonate()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual void Init(IInternalConfigRoot configRoot, object[] hostInitParams)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual void InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath, IInternalConfigRoot configRoot, object[] hostInitConfigurationParams)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual bool IsAboveApplication(string configPath)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public virtual bool IsConfigRecordRequired(string configPath)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public virtual bool IsDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public virtual bool IsFile(string streamName)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public virtual bool IsFullTrustSectionWithoutAptcaAllowed(IInternalConfigRecord configRecord)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public virtual bool IsInitDelayed(IInternalConfigRecord configRecord)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public virtual bool IsLocationApplicable(string configPath)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public virtual bool IsSecondaryRoot(string configPath)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public virtual bool IsTrustedConfigPath(string configPath)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public virtual Stream OpenStreamForRead(string streamName)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual Stream OpenStreamForRead(string streamName, bool assertPermissions)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext, bool assertPermissions)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual bool PrefetchAll(string configPath, string streamName)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public virtual bool PrefetchSection(string sectionGroupName, string sectionName)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public virtual ConfigurationSection ProcessConfigurationSection(ConfigurationSection configSection, ConfigurationBuilder builder)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual XmlNode ProcessRawXml(XmlNode rawXml, ConfigurationBuilder builder)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual void RequireCompleteInit(IInternalConfigRecord configRecord)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual object StartMonitoringStreamForChanges(string streamName, StreamChangeCallback callback)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public virtual void StopMonitoringStreamForChanges(string streamName, StreamChangeCallback callback)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual void VerifyDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition, IConfigErrorInfo errorInfo)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual void WriteCompleted(string streamName, bool success, object writeContext)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual void WriteCompleted(string streamName, bool success, object writeContext, bool assertPermissions)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
