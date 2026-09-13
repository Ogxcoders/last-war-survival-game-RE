using System.Web.Services.Description;

namespace System.Web.Services.Protocols;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public sealed class SoapDocumentServiceAttribute : Attribute
{
	private SoapParameterStyle paramStyle;

	private SoapServiceRoutingStyle routingStyle;

	private SoapBindingUse use;

	public SoapParameterStyle ParameterStyle
	{
		get
		{
			return paramStyle;
		}
		set
		{
			paramStyle = value;
		}
	}

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

	public SoapDocumentServiceAttribute()
	{
		paramStyle = SoapParameterStyle.Wrapped;
		routingStyle = SoapServiceRoutingStyle.SoapAction;
		use = SoapBindingUse.Literal;
	}

	public SoapDocumentServiceAttribute(SoapBindingUse use)
		: this()
	{
		this.use = use;
	}

	public SoapDocumentServiceAttribute(SoapBindingUse use, SoapParameterStyle paramStyle)
		: this()
	{
		this.use = use;
		this.paramStyle = paramStyle;
	}
}
