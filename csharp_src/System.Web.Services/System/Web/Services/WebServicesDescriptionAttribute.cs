using System.ComponentModel;

namespace System.Web.Services;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Event, Inherited = true)]
internal class WebServicesDescriptionAttribute : DescriptionAttribute
{
	public override string Description => base.DescriptionValue;

	public WebServicesDescriptionAttribute(string description)
		: base(description)
	{
	}
}
