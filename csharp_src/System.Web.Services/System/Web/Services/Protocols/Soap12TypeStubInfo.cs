namespace System.Web.Services.Protocols;

internal class Soap12TypeStubInfo : SoapTypeStubInfo
{
	public override string ProtocolName => "Soap12";

	public Soap12TypeStubInfo(LogicalTypeInfo logicalTypeInfo)
		: base(logicalTypeInfo)
	{
	}
}
