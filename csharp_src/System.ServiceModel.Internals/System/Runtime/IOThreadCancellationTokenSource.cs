using System.Threading;

namespace System.Runtime;

internal class IOThreadCancellationTokenSource : IDisposable
{
	private static readonly Action<object> onCancel = Fx.ThunkCallback<object>(OnCancel);

	private readonly TimeSpan timeout;

	private CancellationTokenSource source;

	private CancellationToken? token;

	private IOThreadTimer timer;

	public CancellationToken Token
	{
		get
		{
			if (!token.HasValue)
			{
				if (timeout >= TimeoutHelper.MaxWait)
				{
					token = CancellationToken.None;
				}
				else
				{
					timer = new IOThreadTimer(onCancel, this, isTypicallyCanceledShortlyAfterBeingSet: true);
					source = new CancellationTokenSource();
					timer.Set(timeout);
					token = source.Token;
				}
			}
			return token.Value;
		}
	}

	public IOThreadCancellationTokenSource(TimeSpan timeout)
	{
		TimeoutHelper.ThrowIfNegativeArgument(timeout);
		this.timeout = timeout;
	}

	public IOThreadCancellationTokenSource(int timeout)
		: this(TimeSpan.FromMilliseconds(timeout))
	{
	}

	public void Dispose()
	{
		if (source != null && timer.Cancel())
		{
			source.Dispose();
			source = null;
		}
	}

	private static void OnCancel(object obj)
	{
		((IOThreadCancellationTokenSource)obj).Cancel();
	}

	private void Cancel()
	{
		source.Cancel();
		source.Dispose();
		source = null;
	}
}
