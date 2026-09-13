using System.Runtime.InteropServices;
using System.Web.Services.Description;

namespace System.Web.Services.Protocols;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public sealed class SoapRpcServiceAttribute : Attribute
{
	private SoapServiceRoutingStyle routingStyle;

	private SoapBindingUse use;

	public SoapServiceRoutingStyle RoutingStyle
	{
		get
		{
			return routingStyle;
		}
		set
		{
			routingStyle = value;
		}
	}

	[ComVisible(false)]
	public SoapBindingUse Use
	{
		get
		{
			return use;
		}
		set
		{
			use = value;
		}
	}

	public SoapRpcServiceAttribute()
	{
		routingStyle = SoapServiceRoutingStyle.SoapAction;
	}
}
