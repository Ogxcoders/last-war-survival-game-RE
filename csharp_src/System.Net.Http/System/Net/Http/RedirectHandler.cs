using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal sealed class RedirectHandler : HttpMessageHandler
{
	private readonly HttpMessageHandler _initialInnerHandler;

	private readonly HttpMessageHandler _redirectInnerHandler;

	private readonly int _maxAutomaticRedirections;

	public RedirectHandler(int maxAutomaticRedirections, HttpMessageHandler initialInnerHandler, HttpMessageHandler redirectInnerHandler)
	{
		_maxAutomaticRedirections = maxAutomaticRedirections;
		_initialInnerHandler = initialInnerHandler;
		_redirectInnerHandler = redirectInnerHandler;
	}

	protected internal override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Enter(this, request, cancellationToken);
		}
		HttpResponseMessage httpResponseMessage = await _initialInnerHandler.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		uint redirectCount = 0u;
		Uri uriForRedirect;
		while ((uriForRedirect = GetUriForRedirect(request.RequestUri, httpResponseMessage)) != null)
		{
			redirectCount++;
			if (redirectCount > _maxAutomaticRedirections)
			{
				if (NetEventSource.IsEnabled)
				{
					NetEventSource.Error(this, $"Exceeded max number of redirects. Redirect from {request.RequestUri} to {uriForRedirect} blocked.");
				}
				break;
			}
			httpResponseMessage.Dispose();
			request.Headers.Authorization = null;
			request.RequestUri = uriForRedirect;
			if (RequestRequiresForceGet(httpResponseMessage.StatusCode, request.Method))
			{
				request.Method = HttpMethod.Get;
				request.Content = null;
				request.Headers.TransferEncodingChunked = false;
			}
			httpResponseMessage = await _redirectInnerHandler.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Exit(this);
		}
		return httpResponseMessage;
	}

	private Uri GetUriForRedirect(Uri requestUri, HttpResponseMessage response)
	{
		HttpStatusCode statusCode = response.StatusCode;
		if ((uint)(statusCode - 300) > 3u && (uint)(statusCode - 307) > 1u)
		{
			return null;
		}
		Uri uri = response.Headers.Location;
		if (uri == null)
		{
			return null;
		}
		if (!uri.IsAbsoluteUri)
		{
			uri = new Uri(requestUri, uri);
		}
		string fragment = requestUri.Fragment;
		if (!string.IsNullOrEmpty(fragment) && string.IsNullOrEmpty(uri.Fragment))
		{
			uri = new UriBuilder(uri)
			{
				Fragment = fragment
			}.Uri;
		}
		if (HttpUtilities.IsSupportedSecureScheme(requestUri.Scheme) && !HttpUtilities.IsSupportedSecureScheme(uri.Scheme))
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(this, $"Insecure https to http redirect from '{requestUri}' to '{uri}' blocked.");
			}
			return null;
		}
		return uri;
	}

	private static bool RequestRequiresForceGet(HttpStatusCode statusCode, HttpMethod requestMethod)
	{
		if ((uint)(statusCode - 300) <= 3u)
		{
			return requestMethod == HttpMethod.Post;
		}
		return false;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_initialInnerHandler.Dispose();
			_redirectInnerHandler.Dispose();
		}
		base.Dispose(disposing);
	}
}
