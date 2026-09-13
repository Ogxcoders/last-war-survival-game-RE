using System.ComponentModel;

namespace System.Xml.XmlConfiguration;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class XsltConfigSection
{
	private static bool s_ProhibitDefaultUrlResolver => false;

	internal static bool LimitXPathComplexity => true;

	internal static bool EnableMemberAccessForXslCompiledTransform => false;

	internal static XmlResolver CreateDefaultResolver()
	{
		if (s_ProhibitDefaultUrlResolver)
		{
			return XmlNullResolver.Singleton;
		}
		return new XmlUrlResolver();
	}
}
