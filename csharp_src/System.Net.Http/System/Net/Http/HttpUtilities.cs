using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal static class HttpUtilities
{
	internal static Version DefaultRequestVersion => HttpVersion.Version20;

	internal static Version DefaultResponseVersion => HttpVersion.Version11;

	internal static bool IsHttpUri(Uri uri)
	{
		return IsSupportedScheme(uri.Scheme);
	}

	internal static bool IsSupportedScheme(string scheme)
	{
		if (!IsSupportedNonSecureScheme(scheme))
		{
			return IsSupportedSecureScheme(scheme);
		}
		return true;
	}

	internal static bool IsSupportedNonSecureScheme(string scheme)
	{
		if (!string.Equals(scheme, "http", StringComparison.OrdinalIgnoreCase))
		{
			return IsNonSecureWebSocketScheme(scheme);
		}
		return true;
	}

	internal static bool IsSupportedSecureScheme(string scheme)
	{
		if (!string.Equals(scheme, "https", StringComparison.OrdinalIgnoreCase))
		{
			return IsSecureWebSocketScheme(scheme);
		}
		return true;
	}

	internal static bool IsNonSecureWebSocketScheme(string scheme)
	{
		return string.Equals(scheme, "ws", StringComparison.OrdinalIgnoreCase);
	}

	internal static bool IsSecureWebSocketScheme(string scheme)
	{
		return string.Equals(scheme, "wss", StringComparison.OrdinalIgnoreCase);
	}

	internal static Task ContinueWithStandard<T>(this Task<T> task, object state, Action<Task<T>, object> continuation)
	{
		return task.ContinueWith(continuation, state, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
	}
}
