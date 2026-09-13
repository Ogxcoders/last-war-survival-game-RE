namespace System.Web.Services.Protocols;

public abstract class SoapExtensionAttribute : Attribute
{
	public abstract Type ExtensionType { get; }

	public abstract int Priority { get; set; }
}
