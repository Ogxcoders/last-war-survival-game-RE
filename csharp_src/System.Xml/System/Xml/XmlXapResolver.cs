using System.ComponentModel;

namespace System.Xml;

[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class XmlXapResolver : XmlResolver
{
	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public XmlXapResolver()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
	{
		throw new XmlException("Cannot open '{0}'. The Uri parameter must be a relative path pointing to content inside the Silverlight application's XAP package. If you need to load content from an arbitrary Uri, please see the documentation on Loading XML content using WebClient/HttpWebRequest.", absoluteUri.ToString(), (Exception)null, (IXmlLineInfo)null);
	}

	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void RegisterApplicationResourceStreamResolver(IApplicationResourceStreamResolver appStreamResolver)
	{
	}
}
