namespace System.Security.Policy;

internal sealed class MembershipConditionHelper
{
	private static readonly string XmlTag = "IMembershipCondition";

	internal static int CheckSecurityElement(SecurityElement se, string parameterName, int minimumVersion, int maximumVersion)
	{
		if (se == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		if (se.Tag != XmlTag)
		{
			throw new ArgumentException(string.Format(Locale.GetText("Invalid tag {0}, expected {1}."), se.Tag, XmlTag), parameterName);
		}
		int result = minimumVersion;
		string text = se.Attribute("version");
		if (text != null)
		{
			try
			{
				result = int.Parse(text);
			}
			catch (Exception innerException)
			{
				throw new ArgumentException(string.Format(Locale.GetText("Couldn't parse version from '{0}'."), text), parameterName, innerException);
			}
		}
		return result;
	}

	internal static SecurityElement Element(Type type, int version)
	{
		SecurityElement securityElement = new SecurityElement(XmlTag);
		securityElement.AddAttribute("class", type.FullName + ", " + type.Assembly.ToString().Replace('"', '\''));
		securityElement.AddAttribute("version", version.ToString());
		return securityElement;
	}
}
