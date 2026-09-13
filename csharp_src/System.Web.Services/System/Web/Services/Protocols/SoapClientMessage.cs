using System.Runtime.InteropServices;

namespace System.Web.Services.Protocols;

public sealed class SoapClientMessage : SoapMessage
{
	private SoapHttpClientProtocol client;

	private string url;

	internal SoapMethodStubInfo MethodStubInfo;

	internal object[] Parameters;

	public override string Action => MethodStubInfo.Action;

	public SoapHttpClientProtocol Client => client;

	public override LogicalMethodInfo MethodInfo => MethodStubInfo.MethodInfo;

	public override bool OneWay => MethodStubInfo.OneWay;

	public override string Url => url;

	[ComVisible(false)]
	public override SoapProtocolVersion SoapVersion => client.SoapVersion;

	internal SoapClientMessage(SoapHttpClientProtocol client, SoapMethodStubInfo msi, string url, object[] parameters)
	{
		MethodStubInfo = msi;
		this.client = client;
		this.url = url;
		Parameters = parameters;
		if (SoapVersion == SoapProtocolVersion.Soap12)
		{
			base.ContentType = "application/soap+xml";
		}
	}

	protected override void EnsureInStage()
	{
		EnsureStage(SoapMessageStage.BeforeSerialize);
	}

	protected override void EnsureOutStage()
	{
		EnsureStage(SoapMessageStage.AfterDeserialize);
	}
}
