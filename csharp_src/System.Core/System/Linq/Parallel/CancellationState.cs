using System.Threading;

namespace System.Linq.Parallel;

internal class CancellationState
{
	internal CancellationTokenSource InternalCancellationTokenSource;

	internal CancellationToken ExternalCancellationToken;

	internal CancellationTokenSource MergedCancellationTokenSource;

	internal Shared<bool> TopLevelDisposedFlag;

	internal const int POLL_INTERVAL = 63;

	internal CancellationToken MergedCancellationToken
	{
		get
		{
			if (MergedCancellationTokenSource != null)
			{
				return MergedCancellationTokenSource.Token;
			}
			return new CancellationToken(canceled: false);
		}
	}

	internal CancellationState(CancellationToken externalCancellationToken)
	{
		ExternalCancellationToken = externalCancellationToken;
		TopLevelDisposedFlag = new Shared<bool>(value: false);
	}

	internal static void ThrowIfCanceled(CancellationToken token)
	{
		if (token.IsCancellationRequested)
		{
			throw new OperationCanceledException(token);
		}
	}

	internal static void ThrowWithStandardMessageIfCanceled(CancellationToken externalCancellationToken)
	{
		if (externalCancellationToken.IsCancellationRequested)
		{
			throw new OperationCanceledException("The query has been canceled via the token supplied to WithCancellation.", externalCancellationToken);
		}
	}
}
