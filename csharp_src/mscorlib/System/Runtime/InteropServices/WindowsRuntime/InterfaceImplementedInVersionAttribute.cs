namespace System.Runtime.InteropServices.WindowsRuntime;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = true)]
public sealed class InterfaceImplementedInVersionAttribute : Attribute
{
	private Type m_interfaceType;

	private byte m_majorVersion;

	private byte m_minorVersion;

	private byte m_buildVersion;

	private byte m_revisionVersion;

	public Type InterfaceType => m_interfaceType;

	public byte MajorVersion => m_majorVersion;

	public byte MinorVersion => m_minorVersion;

	public byte BuildVersion => m_buildVersion;

	public byte RevisionVersion => m_revisionVersion;

	public InterfaceImplementedInVersionAttribute(Type interfaceType, byte majorVersion, byte minorVersion, byte buildVersion, byte revisionVersion)
	{
		m_interfaceType = interfaceType;
		m_majorVersion = majorVersion;
		m_minorVersion = minorVersion;
		m_buildVersion = buildVersion;
		m_revisionVersion = revisionVersion;
	}
}
