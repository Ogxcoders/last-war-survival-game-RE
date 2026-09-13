using System.Security.Permissions;

namespace System.Security.Cryptography;

[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public sealed class CngKeyCreationParameters
{
	private CngExportPolicies? m_exportPolicy;

	private CngKeyCreationOptions m_keyCreationOptions;

	private CngKeyUsages? m_keyUsage;

	private CngPropertyCollection m_parameters = new CngPropertyCollection();

	private IntPtr m_parentWindowHandle;

	private CngProvider m_provider = CngProvider.MicrosoftSoftwareKeyStorageProvider;

	private CngUIPolicy m_uiPolicy;

	public CngExportPolicies? ExportPolicy
	{
		get
		{
			return m_exportPolicy;
		}
		set
		{
			m_exportPolicy = value;
		}
	}

	public CngKeyCreationOptions KeyCreationOptions
	{
		get
		{
			return m_keyCreationOptions;
		}
		set
		{
			m_keyCreationOptions = value;
		}
	}

	public CngKeyUsages? KeyUsage
	{
		get
		{
			return m_keyUsage;
		}
		set
		{
			m_keyUsage = value;
		}
	}

	public IntPtr ParentWindowHandle
	{
		get
		{
			return m_parentWindowHandle;
		}
		[SecuritySafeCritical]
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		set
		{
			m_parentWindowHandle = value;
		}
	}

	public CngPropertyCollection Parameters
	{
		[SecuritySafeCritical]
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		get
		{
			return m_parameters;
		}
	}

	internal CngPropertyCollection ParametersNoDemand => m_parameters;

	public CngProvider Provider
	{
		get
		{
			return m_provider;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			m_provider = value;
		}
	}

	public CngUIPolicy UIPolicy
	{
		get
		{
			return m_uiPolicy;
		}
		[SecuritySafeCritical]
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.SafeSubWindows)]
		set
		{
			m_uiPolicy = value;
		}
	}
}
