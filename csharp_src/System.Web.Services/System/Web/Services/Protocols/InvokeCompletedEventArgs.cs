using System.ComponentModel;

namespace System.Web.Services.Protocols;

public class InvokeCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] _results;

	public object[] Results => _results;

	internal InvokeCompletedEventArgs(Exception error, bool cancelled, object userState, object[] results)
		: base(error, cancelled, userState)
	{
		_results = results;
	}
}
