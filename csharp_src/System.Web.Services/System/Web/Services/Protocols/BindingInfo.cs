namespace System.Web.Services.Protocols;

internal class BindingInfo
{
	public readonly string Name;

	public readonly string Namespace;

	public readonly string Location;

	public readonly WebServiceBindingAttribute WebServiceBindingAttribute;

	public BindingInfo(WebServiceBindingAttribute at, string name, string ns)
	{
		if (at != null)
		{
			Name = at.Name;
			Namespace = at.Namespace;
			Location = at.Location;
			WebServiceBindingAttribute = at;
		}
		if (Name == null || Name.Length == 0)
		{
			Name = name;
		}
		if (Namespace == null || Namespace.Length == 0)
		{
			Namespace = ns;
		}
	}
}
