using System.Collections.Generic;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal interface IMonoHttpClientHandler : IDisposable
{
	bool SupportsAutomaticDecompression { get; }

	bool UseCookies { get; set; }

	CookieContainer CookieContainer { get; set; }

	SslClientAuthenticationOptions SslOptions { get; set; }

	DecompressionMethods AutomaticDecompression { get; set; }

	bool UseProxy { get; set; }

	IWebProxy Proxy { get; set; }

	ICredentials DefaultProxyCredentials { get; set; }

	bool PreAuthenticate { get; set; }

	ICredentials Credentials { get; set; }

	bool AllowAutoRedirect { get; set; }

	int MaxAutomaticRedirections { get; set; }

	int MaxConnectionsPerServer { get; set; }

	int MaxResponseHeadersLength { get; set; }

	long MaxRequestContentBufferSize { get; set; }

	IDictionary<string, object> Properties { get; }

	Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);

	void SetWebRequestTimeout(TimeSpan timeout);
}
