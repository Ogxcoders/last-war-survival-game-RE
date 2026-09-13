using System.Threading;

namespace System.Web.Services.Protocols;

internal class InvokeAsyncInfo
{
	public SynchronizationContext Context;

	public object UserState;

	public SendOrPostCallback Callback;

	public InvokeAsyncInfo(SendOrPostCallback callback, object userState)
	{
		Callback = callback;
		UserState = userState;
		Context = SynchronizationContext.Current;
	}
}
