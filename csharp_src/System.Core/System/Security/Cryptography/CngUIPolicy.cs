using System.Security.Permissions;

namespace System.Security.Cryptography;

[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public sealed class CngUIPolicy
{
	private string m_creationTitle;

	private string m_description;

	private string m_friendlyName;

	private CngUIProtectionLevels m_protectionLevel;

	private string m_useContext;

	public string CreationTitle => m_creationTitle;

	public string Description => m_description;

	public string FriendlyName => m_friendlyName;

	public CngUIProtectionLevels ProtectionLevel => m_protectionLevel;

	public string UseContext => m_useContext;

	public CngUIPolicy(CngUIProtectionLevels protectionLevel)
		: this(protectionLevel, null)
	{
	}

	public CngUIPolicy(CngUIProtectionLevels protectionLevel, string friendlyName)
		: this(protectionLevel, friendlyName, null)
	{
	}

	public CngUIPolicy(CngUIProtectionLevels protectionLevel, string friendlyName, string description)
		: this(protectionLevel, friendlyName, description, null)
	{
	}

	public CngUIPolicy(CngUIProtectionLevels protectionLevel, string friendlyName, string description, string useContext)
		: this(protectionLevel, friendlyName, description, useContext, null)
	{
	}

	public CngUIPolicy(CngUIProtectionLevels protectionLevel, string friendlyName, string description, string useContext, string creationTitle)
	{
		m_creationTitle = creationTitle;
		m_description = description;
		m_friendlyName = friendlyName;
		m_protectionLevel = protectionLevel;
		m_useContext = useContext;
	}
}
