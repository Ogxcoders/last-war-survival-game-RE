using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal class AuthenticationHelper
{
	internal class DigestResponse
	{
		internal readonly Dictionary<string, string> Parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		internal const string NonceCount = "00000001";

		internal DigestResponse(string challenge)
		{
			if (!string.IsNullOrEmpty(challenge))
			{
				Parse(challenge);
			}
		}

		private static bool CharIsSpaceOrTab(char ch)
		{
			if (ch != ' ')
			{
				return ch == '\t';
			}
			return true;
		}

		private static bool MustValueBeQuoted(string key)
		{
			if (!key.Equals("realm", StringComparison.OrdinalIgnoreCase) && !key.Equals("nonce", StringComparison.OrdinalIgnoreCase) && !key.Equals("opaque", StringComparison.OrdinalIgnoreCase))
			{
				return key.Equals("qop", StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}

		private string GetNextKey(string data, int currentIndex, out int parsedIndex)
		{
			while (currentIndex < data.Length && CharIsSpaceOrTab(data[currentIndex]))
			{
				currentIndex++;
			}
			int num = currentIndex;
			while (currentIndex < data.Length && data[currentIndex] != '=' && !CharIsSpaceOrTab(data[currentIndex]))
			{
				currentIndex++;
			}
			if (currentIndex == data.Length)
			{
				parsedIndex = currentIndex;
				return null;
			}
			int length = currentIndex - num;
			if (CharIsSpaceOrTab(data[currentIndex]))
			{
				while (currentIndex < data.Length && CharIsSpaceOrTab(data[currentIndex]))
				{
					currentIndex++;
				}
				if (currentIndex == data.Length || data[currentIndex] != '=')
				{
					parsedIndex = currentIndex;
					return null;
				}
			}
			while (currentIndex < data.Length && (CharIsSpaceOrTab(data[currentIndex]) || data[currentIndex] == '='))
			{
				currentIndex++;
			}
			parsedIndex = currentIndex;
			return data.Substring(num, length);
		}

		private string GetNextValue(string data, int currentIndex, bool expectQuotes, out int parsedIndex)
		{
			bool flag = false;
			if (data[currentIndex] == '"')
			{
				flag = true;
				currentIndex++;
			}
			if (expectQuotes && !flag)
			{
				parsedIndex = currentIndex;
				return null;
			}
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			while (currentIndex < data.Length && ((flag && data[currentIndex] != '"') || (!flag && data[currentIndex] != ',')))
			{
				stringBuilder.Append(data[currentIndex]);
				currentIndex++;
				if (currentIndex == data.Length || (!flag && CharIsSpaceOrTab(data[currentIndex])))
				{
					break;
				}
				if (flag && data[currentIndex] == '"' && data[currentIndex - 1] == '\\')
				{
					stringBuilder.Append(data[currentIndex]);
					currentIndex++;
				}
			}
			if (flag)
			{
				currentIndex++;
			}
			while (currentIndex < data.Length && CharIsSpaceOrTab(data[currentIndex]))
			{
				currentIndex++;
			}
			if (currentIndex == data.Length)
			{
				parsedIndex = currentIndex;
				return StringBuilderCache.GetStringAndRelease(stringBuilder);
			}
			if (data[currentIndex++] != ',')
			{
				parsedIndex = currentIndex;
				return null;
			}
			while (currentIndex < data.Length && CharIsSpaceOrTab(data[currentIndex]))
			{
				currentIndex++;
			}
			parsedIndex = currentIndex;
			return StringBuilderCache.GetStringAndRelease(stringBuilder);
		}

		private void Parse(string challenge)
		{
			int parsedIndex = 0;
			while (parsedIndex < challenge.Length)
			{
				string nextKey = GetNextKey(challenge, parsedIndex, out parsedIndex);
				if (!string.IsNullOrEmpty(nextKey) && parsedIndex < challenge.Length)
				{
					string nextValue = GetNextValue(challenge, parsedIndex, MustValueBeQuoted(nextKey), out parsedIndex);
					if (!string.IsNullOrEmpty(nextValue))
					{
						Parameters.Add(nextKey, nextValue);
						continue;
					}
					break;
				}
				break;
			}
		}
	}

	private enum AuthenticationType
	{
		Basic,
		Digest,
		Ntlm,
		Negotiate
	}

	private readonly struct AuthenticationChallenge
	{
		public AuthenticationType AuthenticationType { get; }

		public string SchemeName { get; }

		public NetworkCredential Credential { get; }

		public string ChallengeData { get; }

		public AuthenticationChallenge(AuthenticationType authenticationType, string schemeName, NetworkCredential credential, string challenge)
		{
			AuthenticationType = authenticationType;
			SchemeName = schemeName;
			Credential = credential;
			ChallengeData = challenge;
		}
	}

	private const string Qop = "qop";

	private const string Auth = "auth";

	private const string AuthInt = "auth-int";

	private const string Nonce = "nonce";

	private const string NC = "nc";

	private const string Realm = "realm";

	private const string UserHash = "userhash";

	private const string Username = "username";

	private const string UsernameStar = "username*";

	private const string Algorithm = "algorithm";

	private const string Uri = "uri";

	private const string Sha256 = "SHA-256";

	private const string Md5 = "MD5";

	private const string Sha256Sess = "SHA-256-sess";

	private const string MD5Sess = "MD5-sess";

	private const string CNonce = "cnonce";

	private const string Opaque = "opaque";

	private const string Response = "response";

	private const string Stale = "stale";

	private static int[] s_alphaNumChooser = new int[3] { 48, 65, 97 };

	private const string BasicScheme = "Basic";

	private const string DigestScheme = "Digest";

	private const string NtlmScheme = "NTLM";

	private const string NegotiateScheme = "Negotiate";

	public static async Task<string> GetDigestTokenForCredential(NetworkCredential credential, HttpRequestMessage request, DigestResponse digestResponse)
	{
		StringBuilder sb = StringBuilderCache.Acquire();
		if (digestResponse.Parameters.TryGetValue("algorithm", out var algorithm))
		{
			if (!algorithm.Equals("SHA-256", StringComparison.OrdinalIgnoreCase) && !algorithm.Equals("MD5", StringComparison.OrdinalIgnoreCase) && !algorithm.Equals("SHA-256-sess", StringComparison.OrdinalIgnoreCase) && !algorithm.Equals("MD5-sess", StringComparison.OrdinalIgnoreCase))
			{
				if (NetEventSource.IsEnabled)
				{
					NetEventSource.Error(digestResponse, "Algorithm not supported: {algorithm}");
				}
				return null;
			}
		}
		else
		{
			algorithm = "MD5";
		}
		if (!digestResponse.Parameters.TryGetValue("nonce", out var nonce))
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(digestResponse, "Nonce missing");
			}
			return null;
		}
		digestResponse.Parameters.TryGetValue("opaque", out var opaque);
		if (!digestResponse.Parameters.TryGetValue("realm", out var value))
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(digestResponse, "Realm missing");
			}
			return null;
		}
		if (digestResponse.Parameters.TryGetValue("userhash", out var value2) && value2 == "true")
		{
			sb.AppendKeyValue("username", ComputeHash(credential.UserName + ":" + value, algorithm));
			sb.AppendKeyValue("userhash", value2, includeQuotes: false);
		}
		else if (HeaderUtilities.ContainsNonAscii(credential.UserName))
		{
			string value3 = HeaderUtilities.Encode5987(credential.UserName);
			sb.AppendKeyValue("username*", value3, includeQuotes: false);
		}
		else
		{
			sb.AppendKeyValue("username", credential.UserName);
		}
		if (value != string.Empty)
		{
			sb.AppendKeyValue("realm", value);
		}
		sb.AppendKeyValue("nonce", nonce);
		sb.AppendKeyValue("uri", request.RequestUri.PathAndQuery);
		string qop = "auth";
		if (digestResponse.Parameters.ContainsKey("qop"))
		{
			int num = digestResponse.Parameters["qop"].IndexOf("auth-int");
			if (num != -1 && digestResponse.Parameters["qop"].IndexOf("auth") == num && digestResponse.Parameters["qop"].IndexOf("auth", num + "auth-int".Length) == -1)
			{
				qop = "auth-int";
			}
		}
		string cnonce = GetRandomAlphaNumericString();
		string a1 = credential.UserName + ":" + value + ":" + credential.Password;
		if (algorithm.EndsWith("sess", StringComparison.OrdinalIgnoreCase))
		{
			a1 = ComputeHash(a1, algorithm) + ":" + nonce + ":" + cnonce;
		}
		string a2 = request.Method.Method + ":" + request.RequestUri.PathAndQuery;
		if (qop == "auth-int")
		{
			string text = ((request.Content != null) ? (await request.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
			string data = text;
			a2 = a2 + ":" + ComputeHash(data, algorithm);
		}
		string value4 = ComputeHash(ComputeHash(a1, algorithm) + ":" + nonce + ":00000001:" + cnonce + ":" + qop + ":" + ComputeHash(a2, algorithm), algorithm);
		sb.AppendKeyValue("response", value4);
		sb.AppendKeyValue("algorithm", algorithm, includeQuotes: false);
		if (opaque != null)
		{
			sb.AppendKeyValue("opaque", opaque);
		}
		sb.AppendKeyValue("qop", qop, includeQuotes: false);
		sb.AppendKeyValue("nc", "00000001", includeQuotes: false);
		sb.AppendKeyValue("cnonce", cnonce, includeQuotes: true, includeComma: false);
		return StringBuilderCache.GetStringAndRelease(sb);
	}

	public static bool IsServerNonceStale(DigestResponse digestResponse)
	{
		string value = null;
		if (digestResponse.Parameters.TryGetValue("stale", out value))
		{
			return value == "true";
		}
		return false;
	}

	private static string GetRandomAlphaNumericString()
	{
		Span<byte> data = stackalloc byte[32];
		RandomNumberGenerator.Fill(data);
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		int num = 0;
		while (num < data.Length)
		{
			int num2 = data[num++] % 3;
			int num3 = data[num++] % ((num2 == 0) ? 10 : 26);
			stringBuilder.Append((char)(s_alphaNumChooser[num2] + num3));
		}
		return StringBuilderCache.GetStringAndRelease(stringBuilder);
	}

	private static string ComputeHash(string data, string algorithm)
	{
		using HashAlgorithm hashAlgorithm = (algorithm.StartsWith("SHA-256", StringComparison.OrdinalIgnoreCase) ? ((HashAlgorithm)SHA256.Create()) : ((HashAlgorithm)MD5.Create()));
		Span<byte> destination = stackalloc byte[hashAlgorithm.HashSize / 8];
		hashAlgorithm.TryComputeHash(Encoding.UTF8.GetBytes(data), destination, out var _);
		StringBuilder stringBuilder = StringBuilderCache.Acquire(destination.Length * 2);
		Span<char> span = stackalloc char[2];
		for (int i = 0; i < destination.Length; i++)
		{
			destination[i].TryFormat(span, out var _, "x2");
			stringBuilder.Append(span);
		}
		return StringBuilderCache.GetStringAndRelease(stringBuilder);
	}

	private static Task<HttpResponseMessage> InnerSendAsync(HttpRequestMessage request, bool isProxyAuth, HttpConnectionPool pool, HttpConnection connection, CancellationToken cancellationToken)
	{
		if (!isProxyAuth)
		{
			return pool.SendWithNtProxyAuthAsync(connection, request, cancellationToken);
		}
		return connection.SendAsyncCore(request, cancellationToken);
	}

	private static bool ProxySupportsConnectionAuth(HttpResponseMessage response)
	{
		if (!response.Headers.TryGetValues(KnownHeaders.ProxySupport.Descriptor, out var values))
		{
			return false;
		}
		foreach (string item in values)
		{
			if (item == "Session-Based-Authentication")
			{
				return true;
			}
		}
		return false;
	}

	private static async Task<HttpResponseMessage> SendWithNtAuthAsync(HttpRequestMessage request, Uri authUri, ICredentials credentials, bool isProxyAuth, HttpConnection connection, HttpConnectionPool connectionPool, CancellationToken cancellationToken)
	{
		HttpResponseMessage response = await InnerSendAsync(request, isProxyAuth, connectionPool, connection, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (!isProxyAuth && connection.Kind == HttpConnectionKind.Proxy && !ProxySupportsConnectionAuth(response))
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(connection, $"Proxy doesn't support connection-based auth, uri={authUri}");
			}
			return response;
		}
		if (TryGetAuthenticationChallenge(response, isProxyAuth, authUri, credentials, out var challenge) && (challenge.AuthenticationType == AuthenticationType.Negotiate || challenge.AuthenticationType == AuthenticationType.Ntlm))
		{
			bool isNewConnection = false;
			bool needDrain = true;
			try
			{
				if (response.Headers.ConnectionClose == true)
				{
					(connection, response) = await connectionPool.CreateConnectionAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					if (response != null)
					{
						return response;
					}
					connectionPool.IncrementConnectionCount();
					connection.Acquire();
					isNewConnection = true;
					needDrain = false;
				}
				string challengeData = challenge.ChallengeData;
				string text;
				if (!isProxyAuth && request.HasHeaders && request.Headers.Host != null)
				{
					text = request.Headers.Host;
					if (NetEventSource.IsEnabled)
					{
						NetEventSource.Info(connection, $"Authentication: {challenge.AuthenticationType}, Host: {text}");
					}
				}
				else
				{
					UriHostNameType hostNameType = authUri.HostNameType;
					text = ((hostNameType != UriHostNameType.IPv6 && hostNameType != UriHostNameType.IPv4) ? (await Dns.GetHostEntryAsync(authUri.IdnHost).ConfigureAwait(continueOnCapturedContext: false)).HostName : authUri.IdnHost);
				}
				string text2 = "HTTP/" + text;
				if (NetEventSource.IsEnabled)
				{
					NetEventSource.Info(connection, $"Authentication: {challenge.AuthenticationType}, SPN: {text2}");
				}
				ChannelBinding channelBinding = connection.TransportContext?.GetChannelBinding(ChannelBindingKind.Endpoint);
				NTAuthentication authContext = new NTAuthentication(isServer: false, challenge.SchemeName, challenge.Credential, text2, ContextFlagsPal.Connection, channelBinding);
				try
				{
					while (true)
					{
						string challengeResponse = authContext.GetOutgoingBlob(challengeData);
						if (challengeResponse != null)
						{
							if (needDrain)
							{
								await connection.DrainResponseAsync(response).ConfigureAwait(continueOnCapturedContext: false);
							}
							SetRequestAuthenticationHeaderValue(request, new AuthenticationHeaderValue(challenge.SchemeName, challengeResponse), isProxyAuth);
							response = await InnerSendAsync(request, isProxyAuth, connectionPool, connection, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
							if (!authContext.IsCompleted && TryGetRepeatedChallenge(response, challenge.SchemeName, isProxyAuth, out challengeData))
							{
								needDrain = true;
								continue;
							}
							break;
						}
						break;
					}
				}
				finally
				{
					authContext.CloseContext();
				}
			}
			finally
			{
				if (isNewConnection)
				{
					connection.Release();
				}
			}
		}
		return response;
	}

	public static Task<HttpResponseMessage> SendWithNtProxyAuthAsync(HttpRequestMessage request, Uri proxyUri, ICredentials proxyCredentials, HttpConnection connection, HttpConnectionPool connectionPool, CancellationToken cancellationToken)
	{
		return SendWithNtAuthAsync(request, proxyUri, proxyCredentials, isProxyAuth: true, connection, connectionPool, cancellationToken);
	}

	public static Task<HttpResponseMessage> SendWithNtConnectionAuthAsync(HttpRequestMessage request, ICredentials credentials, HttpConnection connection, HttpConnectionPool connectionPool, CancellationToken cancellationToken)
	{
		return SendWithNtAuthAsync(request, request.RequestUri, credentials, isProxyAuth: false, connection, connectionPool, cancellationToken);
	}

	private static bool TryGetChallengeDataForScheme(string scheme, HttpHeaderValueCollection<AuthenticationHeaderValue> authenticationHeaderValues, out string challengeData)
	{
		foreach (AuthenticationHeaderValue authenticationHeaderValue in authenticationHeaderValues)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(scheme, authenticationHeaderValue.Scheme))
			{
				challengeData = authenticationHeaderValue.Parameter;
				return true;
			}
		}
		challengeData = null;
		return false;
	}

	private static bool TryGetValidAuthenticationChallengeForScheme(string scheme, AuthenticationType authenticationType, Uri uri, ICredentials credentials, HttpHeaderValueCollection<AuthenticationHeaderValue> authenticationHeaderValues, out AuthenticationChallenge challenge)
	{
		challenge = default(AuthenticationChallenge);
		if (!TryGetChallengeDataForScheme(scheme, authenticationHeaderValues, out var challengeData))
		{
			return false;
		}
		NetworkCredential credential = credentials.GetCredential(uri, scheme);
		if (credential == null)
		{
			return false;
		}
		challenge = new AuthenticationChallenge(authenticationType, scheme, credential, challengeData);
		return true;
	}

	private static bool TryGetAuthenticationChallenge(HttpResponseMessage response, bool isProxyAuth, Uri authUri, ICredentials credentials, out AuthenticationChallenge challenge)
	{
		if (!IsAuthenticationChallenge(response, isProxyAuth))
		{
			challenge = default(AuthenticationChallenge);
			return false;
		}
		HttpHeaderValueCollection<AuthenticationHeaderValue> responseAuthenticationHeaderValues = GetResponseAuthenticationHeaderValues(response, isProxyAuth);
		if (!TryGetValidAuthenticationChallengeForScheme("Negotiate", AuthenticationType.Negotiate, authUri, credentials, responseAuthenticationHeaderValues, out challenge) && !TryGetValidAuthenticationChallengeForScheme("NTLM", AuthenticationType.Ntlm, authUri, credentials, responseAuthenticationHeaderValues, out challenge) && !TryGetValidAuthenticationChallengeForScheme("Digest", AuthenticationType.Digest, authUri, credentials, responseAuthenticationHeaderValues, out challenge))
		{
			return TryGetValidAuthenticationChallengeForScheme("Basic", AuthenticationType.Basic, authUri, credentials, responseAuthenticationHeaderValues, out challenge);
		}
		return true;
	}

	private static bool TryGetRepeatedChallenge(HttpResponseMessage response, string scheme, bool isProxyAuth, out string challengeData)
	{
		challengeData = null;
		if (!IsAuthenticationChallenge(response, isProxyAuth))
		{
			return false;
		}
		if (!TryGetChallengeDataForScheme(scheme, GetResponseAuthenticationHeaderValues(response, isProxyAuth), out challengeData))
		{
			return false;
		}
		return true;
	}

	private static bool IsAuthenticationChallenge(HttpResponseMessage response, bool isProxyAuth)
	{
		if (!isProxyAuth)
		{
			return response.StatusCode == HttpStatusCode.Unauthorized;
		}
		return response.StatusCode == HttpStatusCode.ProxyAuthenticationRequired;
	}

	private static HttpHeaderValueCollection<AuthenticationHeaderValue> GetResponseAuthenticationHeaderValues(HttpResponseMessage response, bool isProxyAuth)
	{
		if (!isProxyAuth)
		{
			return response.Headers.WwwAuthenticate;
		}
		return response.Headers.ProxyAuthenticate;
	}

	private static void SetRequestAuthenticationHeaderValue(HttpRequestMessage request, AuthenticationHeaderValue headerValue, bool isProxyAuth)
	{
		if (isProxyAuth)
		{
			request.Headers.ProxyAuthorization = headerValue;
		}
		else
		{
			request.Headers.Authorization = headerValue;
		}
	}

	private static void SetBasicAuthToken(HttpRequestMessage request, NetworkCredential credential, bool isProxyAuth)
	{
		string s = ((!string.IsNullOrEmpty(credential.Domain)) ? (credential.Domain + "\\" + credential.UserName + ":" + credential.Password) : (credential.UserName + ":" + credential.Password));
		string parameter = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
		SetRequestAuthenticationHeaderValue(request, new AuthenticationHeaderValue("Basic", parameter), isProxyAuth);
	}

	private static async Task<bool> TrySetDigestAuthToken(HttpRequestMessage request, NetworkCredential credential, DigestResponse digestResponse, bool isProxyAuth)
	{
		string text = await GetDigestTokenForCredential(credential, request, digestResponse).ConfigureAwait(continueOnCapturedContext: false);
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		AuthenticationHeaderValue headerValue = new AuthenticationHeaderValue("Digest", text);
		SetRequestAuthenticationHeaderValue(request, headerValue, isProxyAuth);
		return true;
	}

	private static Task<HttpResponseMessage> InnerSendAsync(HttpRequestMessage request, bool isProxyAuth, bool doRequestAuth, HttpConnectionPool pool, CancellationToken cancellationToken)
	{
		if (!isProxyAuth)
		{
			return pool.SendWithProxyAuthAsync(request, doRequestAuth, cancellationToken);
		}
		return pool.SendWithRetryAsync(request, doRequestAuth, cancellationToken);
	}

	private static async Task<HttpResponseMessage> SendWithAuthAsync(HttpRequestMessage request, Uri authUri, ICredentials credentials, bool preAuthenticate, bool isProxyAuth, bool doRequestAuth, HttpConnectionPool pool, CancellationToken cancellationToken)
	{
		bool performedBasicPreauth = false;
		if (preAuthenticate)
		{
			NetworkCredential credential;
			lock (pool.PreAuthCredentials)
			{
				credential = pool.PreAuthCredentials.GetCredential(authUri, "Basic");
			}
			if (credential != null)
			{
				SetBasicAuthToken(request, credential, isProxyAuth);
				performedBasicPreauth = true;
			}
		}
		HttpResponseMessage response = await InnerSendAsync(request, isProxyAuth, doRequestAuth, pool, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (TryGetAuthenticationChallenge(response, isProxyAuth, authUri, credentials, out var challenge))
		{
			switch (challenge.AuthenticationType)
			{
			case AuthenticationType.Digest:
			{
				DigestResponse digestResponse = new DigestResponse(challenge.ChallengeData);
				if (!(await TrySetDigestAuthToken(request, challenge.Credential, digestResponse, isProxyAuth).ConfigureAwait(continueOnCapturedContext: false)))
				{
					break;
				}
				response.Dispose();
				response = await InnerSendAsync(request, isProxyAuth, doRequestAuth, pool, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (TryGetRepeatedChallenge(response, challenge.SchemeName, isProxyAuth, out var challengeData))
				{
					digestResponse = new DigestResponse(challengeData);
					bool flag = IsServerNonceStale(digestResponse);
					if (flag)
					{
						flag = await TrySetDigestAuthToken(request, challenge.Credential, digestResponse, isProxyAuth).ConfigureAwait(continueOnCapturedContext: false);
					}
					if (flag)
					{
						response.Dispose();
						response = await InnerSendAsync(request, isProxyAuth, doRequestAuth, pool, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				break;
			}
			case AuthenticationType.Basic:
			{
				if (performedBasicPreauth)
				{
					break;
				}
				response.Dispose();
				SetBasicAuthToken(request, challenge.Credential, isProxyAuth);
				response = await InnerSendAsync(request, isProxyAuth, doRequestAuth, pool, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (!preAuthenticate)
				{
					break;
				}
				HttpStatusCode statusCode = response.StatusCode;
				if (statusCode == HttpStatusCode.Unauthorized || statusCode == HttpStatusCode.ProxyAuthenticationRequired)
				{
					break;
				}
				lock (pool.PreAuthCredentials)
				{
					try
					{
						if (NetEventSource.IsEnabled)
						{
							NetEventSource.Info(pool.PreAuthCredentials, $"Adding Basic credential to cache, uri={authUri}, username={challenge.Credential.UserName}");
						}
						pool.PreAuthCredentials.Add(authUri, "Basic", challenge.Credential);
					}
					catch (ArgumentException)
					{
						if (NetEventSource.IsEnabled)
						{
							NetEventSource.Info(pool.PreAuthCredentials, $"Basic credential present in cache, uri={authUri}, username={challenge.Credential.UserName}");
						}
					}
				}
				break;
			}
			}
		}
		return response;
	}

	public static Task<HttpResponseMessage> SendWithProxyAuthAsync(HttpRequestMessage request, Uri proxyUri, ICredentials proxyCredentials, bool doRequestAuth, HttpConnectionPool pool, CancellationToken cancellationToken)
	{
		return SendWithAuthAsync(request, proxyUri, proxyCredentials, preAuthenticate: false, isProxyAuth: true, doRequestAuth, pool, cancellationToken);
	}

	public static Task<HttpResponseMessage> SendWithRequestAuthAsync(HttpRequestMessage request, ICredentials credentials, bool preAuthenticate, HttpConnectionPool pool, CancellationToken cancellationToken)
	{
		return SendWithAuthAsync(request, request.RequestUri, credentials, preAuthenticate, isProxyAuth: false, doRequestAuth: true, pool, cancellationToken);
	}
}
