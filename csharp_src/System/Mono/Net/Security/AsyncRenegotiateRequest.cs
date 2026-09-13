namespace Mono.Net.Security;

internal class AsyncRenegotiateRequest : AsyncProtocolRequest
{
	public AsyncRenegotiateRequest(MobileAuthenticatedStream parent)
		: base(parent, sync: false)
	{
	}

	protected override AsyncOperationStatus Run(AsyncOperationStatus status)
	{
		return base.Parent.ProcessHandshake(status, renegotiate: true);
	}
}
